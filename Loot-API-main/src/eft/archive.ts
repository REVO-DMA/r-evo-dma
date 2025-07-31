import globals from "../globals";
import { readFile, writeFile, readdir } from "fs/promises";
import { buildDate } from "./utils";
import * as fse from "fs-extra";
import { DateTime } from "luxon";
import { Result } from "@badrap/result";
import { compress, decompress } from "../utils";
import { GetLootQuery } from "../gql/graphql";

/**
 * Handles all archive operations.
 */
export async function doArchiveOps(data: GetLootQuery)
{
    await fse.ensureDir(globals.archive.directory);

    if (!shouldUseArchivedLootFile())
        await backUpLootData(data);
    
    await cleanUp();
}

/**
 * Tries to get an archived loot file.
 */
export async function getArchivedLootFile()
{
    try
    {
        const lootFile = `${globals.archive.directory}/${globals.archive.dateToUse}.evo`;

        const fileExists = await fse.exists(lootFile);
        if (!fileExists)
            return Result.err();
    
        const compressed = await readFile(lootFile);
    
        const decompressed = await decompress(compressed);
        if (decompressed.isOk)
        {
            const rawData = JSON.parse(decompressed.value.toString("utf8"));
            const data = (rawData as GetLootQuery);
    
            return Result.ok(data);
        }
        else if (decompressed.isErr)
            return Result.err(decompressed.error);
    }
    catch (err)
    {
        return Result.err(err);
    }
}

/**
 * Determines whether or not an archived loot file should be used.
 */
export function shouldUseArchivedLootFile()
{
    if (globals.archive.dateToUse == "")
        return false;

    return true;
}

/**
 * Backs up the supplied loot data if required.
 */
async function backUpLootData(data: GetLootQuery)
{
    const doBackup = await shouldBackup();
    if (!doBackup)
    {
        console.log("Skipping backup. Today's file already exists.");
        return;
    }

    const fileName = buildDate(true);

    const asString = JSON.stringify(data);
    const compressed = await compress(asString);
    if (compressed.isOk)
        await writeFile(`${globals.archive.directory}/${fileName}`, compressed.value);
    else if (compressed.isErr)
        console.error(compressed.error);
}

/**
 * Determines whether or not a backup is required.
 */
async function shouldBackup()
{
    const searchFileName = buildDate(true);

    const archivedFiles = await readdir(globals.archive.directory);

    if (archivedFiles.includes(searchFileName))
        return false;

    return true;
}

/**
 * Cleans up archived loot data older than the configured max age.
 */
async function cleanUp()
{
    const archivedFiles = await readdir(globals.archive.directory);

    const now = DateTime.local();

    for (let i = 0; i < archivedFiles.length; i++) {
        const file = archivedFiles[i];
        
        const fileDateStr = file.split(".")[0];
        const jsFileDate = new Date(fileDateStr);

        const fileDate = DateTime.fromJSDate(jsFileDate);
        const daysOld = now.diff(fileDate, "days").days;
        if (daysOld > globals.archive.maxAgeDays)
        {
            const fileToRemove = `${globals.archive.directory}/${file}`;
            console.log(`[CLEAN UP] Removed loot data file: \"${fileToRemove}\"`)
            await fse.remove(fileToRemove);
        }
    }
}