import cron from "node-cron";
import globals from "../globals";
import { gql, Client as gqlClient, fetchExchange } from "@urql/core";
import { GetLootQuery } from "../gql/graphql";
import { writeFile, readFile } from "fs/promises";
import { compress, decompress } from "../utils";
import { process } from "./helpers";
import { Client as ftpClient } from "basic-ftp";
import { bufferToStream } from "./utils";
import { Readable } from "stream";
import { doArchiveOps, getArchivedLootFile, shouldUseArchivedLootFile } from "./archive";

const LOCAL_TESTING = false;

const graphql = new gqlClient({
    url: globals.loot.server,
    exchanges: [fetchExchange],
});

const GET_LOOT = gql<GetLootQuery>`
query GetLoot {
  items {
    id
    updated
    name
    shortName
    description
    width
    height
    backgroundColor
    baseImageLink
    image512pxLink
    types
    categories {
      name
      normalizedName
    }
    handbookCategories {
      name
      normalizedName
    }
    containsItems {
      item {
        id
      }
      count
    }
    sellFor {
      vendor {
        name
      }
      priceRUB
    }
    buyFor {
      vendor {
        name
      }
      priceRUB
    }
    changeLast48h
    changeLast48hPercent
    lastOfferCount
  }
  questItems {
    id
    name
    shortName
    description
    baseImageLink
    image512pxLink
  }
  tasks {
    id
    name
    trader {
      name
    }
    objectives {
      __typename
      ... on TaskObjectiveBuildItem {
        item {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
      }
      ... on TaskObjectiveExtract {
        exitStatus
        exitName
        zoneNames
      }
      ... on TaskObjectiveItem {
        item {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        count
        foundInRaid
        dogTagLevel
        maxDurability
        minDurability
      }
      ... on TaskObjectiveMark {
        markerItem {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
      }
      ... on TaskObjectivePlayerLevel {
        playerLevel
      }
      ... on TaskObjectiveQuestItem {
        questItem {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        count
      }
      ... on TaskObjectiveShoot {
        targetNames
        count
        shotType
        zoneNames
        bodyParts
        usingWeapon {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        usingWeaponMods {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        wearing {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        notWearing {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        distance {
          value
        }
        playerHealthEffect {
          bodyParts
          effects
          time {
            value
          }
        }
        enemyHealthEffect {
          bodyParts
          effects
          time {
            value
          }
        }
        timeFromHour
        timeUntilHour
      }
      ... on TaskObjectiveSkill {
        skillLevel {
          name
          level
        }
      }
      ... on TaskObjectiveTaskStatus {
        task {
          id
          name
        }
        status
      }
      ... on TaskObjectiveTraderLevel {
        trader {
          name
        }
        level
      }
      ... on TaskObjectiveTraderStanding {
        trader {
          name
        }
        compareMethod
        value
      }
      ... on TaskObjectiveUseItem {
        useAny {
          id
          name
          shortName
          description
          baseImageLink
          image512pxLink
        }
        compareMethod
        count
        zoneNames
      }
      id
      type
      description
      maps {
        name
      }
      optional
    }
    wikiLink
    kappaRequired
    lightkeeperRequired
    finishRewards {
      traderStanding {
        trader {
          name
        }
        standing
      }
      items {
        item {
          id
          name
        }
        count
        quantity
      }
    }
    experience
  }
  crafts {
    requiredItems {
      count
      item {
        id
      }
    }
    rewardItems {
      count
      item {
        id
      }
    }
  }
}
`;

export async function initialize()
{
    if (LOCAL_TESTING)
    {
        // await updateLocal_FS();
        await process_FS();

        return;
    }

    await updateLocal();

    cron.schedule(`*/${globals.loot.refreshIntervalMins} * * * *`, async () => {
        await updateLocal();
    });
}

async function updateLocal_FS()
{
    const result = await graphql.query(GET_LOOT, {}).toPromise();

    const stringData = JSON.stringify(result.data);
    const compressed = await compress(stringData);
    if (compressed.isOk)
    {
        await writeFile("itemData.evo", compressed.value);
        console.log("new data saved");
    }
    else if (compressed.isErr)
    {
        console.error(compressed.error)
        return;
    }
}

async function process_FS()
{
    let lootData: GetLootQuery = null;

    const useArchive = shouldUseArchivedLootFile();
    if (useArchive)
    {
        const lootDataTmp = await getArchivedLootFile();
        if (lootDataTmp.isOk)
        {
            lootData = lootDataTmp.value;
            console.log(`Using archived loot file from ${globals.archive.dateToUse}`);
        }
        else if (lootDataTmp.isErr)
            console.error(`Unable to fetch archived loot file from ${globals.archive.dateToUse} ~ ${lootDataTmp.error}`);
    }

    if (lootData === null)
    {
        const compressed = await readFile("itemData.evo");
    
        const decompressed = await decompress(compressed);
        if (decompressed.isOk)
        {
            const rawData = JSON.parse(decompressed.value.toString("utf8"));
            lootData = (rawData as GetLootQuery);
        }
        else if (decompressed.isErr)
            console.error(decompressed.error);
    }

    await doArchiveOps(lootData);

    const processed = process(lootData);
    if (processed.isOk)
    {
        const asString = JSON.stringify(processed.value);
        const compressed = await compress(asString);
        if (compressed.isOk)
        {
            await writeFile("processedData.evo", compressed.value);
        }
        else if (compressed.isErr)
            console.error(compressed.error);
    }
}

async function updateLocal()
{
    try
    {
        let lootData: GetLootQuery = null;

        const useArchive = shouldUseArchivedLootFile();
        if (useArchive)
        {
            const lootDataTmp = await getArchivedLootFile();
            if (lootDataTmp.isOk)
            {
                lootData = lootDataTmp.value;
                console.log(`Using archived loot file from ${globals.archive.dateToUse}`);
            }
            else if (lootDataTmp.isErr)
                console.error(`Unable to fetch archived loot file from ${globals.archive.dateToUse} ~ ${lootDataTmp.error}`);
        }

        if (lootData === null)
            lootData = (await graphql.query(GET_LOOT, {}).toPromise()).data;

        await doArchiveOps(lootData);

        const processed = process(lootData);
        if (processed.isOk)
        {
            const data = processed.value;

            const asString = JSON.stringify(data);
            const compressed = await compress(asString);
            if (compressed.isOk)
            {
                const dataStream = bufferToStream(compressed.value);
                await sendToBunny(dataStream);
            }
            else if (compressed.isErr)
                console.error(compressed.error);
        }
        else if (processed.isErr)
        {
            console.error(processed.error);
            return;
        }
    }
    catch (err)
    {
        console.error(err);
    }
}

async function sendToBunny(data: Readable)
{
    const client = new ftpClient(globals.bunny.ftp.timeout);

    await client.access({
        host: globals.bunny.ftp.hostname,
        port: globals.bunny.ftp.port,
        user: globals.bunny.ftp.username,
        password: globals.bunny.ftp.password,
        secure: true
    });

    await client.uploadFrom(data, "eft/itemData.evo");
    console.log("Data uploaded to Bunny");
}
