import randomColor from "randomcolor";
import { hexToRGBA } from "../../utils/misc";
import { AITypes, PlayerColors } from "./playerType";
import { PlayerType } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/PlayerType";
import { Player } from "./player";
import { IPC_PlayerStats } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_PlayerStats";
import { shell } from "@electron/remote";

const playersTableColors = {
    USEC: "darkblue",
    BEAR: "saddlebrown",
};

/** @type {HTMLDivElement} */
const playersTableNoPlayersEl = document.getElementById("playersTableNoPlayers");
/** @type {HTMLTableElement} */
const playersTableTableEl = document.getElementById("playersTableTable");
const playersTableBodyEl = document.getElementById("playersTableBody");

let PlayersOnTable: { [n: number]: boolean; } = {};

let SHOW_AI = false;

export function add(player: Player) {
    const ID = player.StaticData.ID;
    
    // Prevent duplicates
    if (PlayersOnTable[ID] != null) return;

    const name = player.DynamicData.Name;
    const playerType = player.DynamicData.PlayerType;
    const groupID = player.StaticData.GroupID;
    const faction = player.StaticData.Faction;

    if (!SHOW_AI && playerType !== PlayerType.LocalPlayer && playerType !== PlayerType.Teammate && playerType !== PlayerType.EnemyPMC && playerType !== PlayerType.PlayerScav && playerType !== PlayerType.AI_PMC)
        return;

    // Additional row classes
    let additionalRowClasses = "";
    if (playerType === PlayerType.LocalPlayer) additionalRowClasses = "playersTablePlayerRow_LocalPlayer";
    else if (playerType === PlayerType.Teammate) additionalRowClasses = "playersTablePlayerRow_LocalPlayerTeammate";

    // Group border
    let groupBorderStyle = `border-left: 4px solid transparent;`;
    if (groupID != null) {
        const borderColor = randomColor({
            seed: groupID,
            format: "rgba",
            alpha: 40,
            luminosity: "bright",
        });

        groupBorderStyle = `border-left: 4px solid ${borderColor};`;
    }

    // Row background color
    const playerColors = PlayerColors.get(playerType);
    const rowBackground = hexToRGBA(playerColors.dot, 60);

    // Faction background color
    let factionStyles = "";
    if (faction === "USEC") factionStyles = `background-color: ${playersTableColors.USEC};`;
    else if (faction === "BEAR") factionStyles = `background-color: ${playersTableColors.BEAR};`;

    const markup = /*html*/`
        <tr playertype="${playerType}" groupid="${groupID}" class="playersTablePlayerRow ${additionalRowClasses}" id="playersTable_${ID}" style="${groupBorderStyle} background-color: ${rowBackground};">
            <td><div class="row m-0"></div></td>
            <td>${name}</td>
            <td>N/A</td>
            <td style="${factionStyles}">${faction}</td>
            <td>N/A</td>
            <td>N/A</td>
            <td>N/A</td>
            <td>N/A</td>
            <td>&#8381; N/A</td>
        </tr>
    `;

    playersTableBodyEl.insertAdjacentHTML("beforeend", markup);

    playersTableNoPlayersEl.style.display = "none";
    playersTableTableEl.style.display = "";

    PlayersOnTable[ID] = true;

    sort();
}

export function UpdateColor(ID: number, playerType: PlayerType) {
    // Ensure this player is in the table
    if (PlayersOnTable[ID] == null) return;

    const tableRow = document.getElementById(`playersTable_${ID}`);

    tableRow.style.backgroundColor = PlayerColors.get(playerType).dot;

    if (playerType === PlayerType.Teammate) tableRow.classList.add("playersTablePlayerRow_LocalPlayerTeammate");
    else tableRow.classList.remove("playersTablePlayerRow_LocalPlayerTeammate");
}

export function SetStreamLink(ID: number, streamURL: string) {
    // Ensure this player is in the table
    if (PlayersOnTable[ID] == null) return;

    const tableRow = document.getElementById(`playersTable_${ID}`);

    tableRow.children[0].children[0].innerHTML += /*html*/`
        <div class="col-auto p-0 playersTableActionItem playersTableActionItem_streamLink" id="playersTableStreamLink_${ID}">
            <i class="fa-solid fa-globe buttonIcon"></i>
        </div>
    `;

    document.getElementById(`playersTableStreamLink_${ID}`).addEventListener("click", () => {
        shell.openExternal(streamURL);
    });
}

export function UpdateValue(ID: number, value: string) {
    // Ensure this player is in the table
    if (PlayersOnTable[ID] == null) return;

    const tableRow = document.getElementById(`playersTable_${ID}`);

    tableRow.children[8].innerHTML = `&#8381; ${value}`;
}

export function UpdateStats(ID: number, stats: IPC_PlayerStats) {
    // Ensure this player is in the table
    if (PlayersOnTable[ID] == null) return;

    const tableRow = document.getElementById(`playersTable_${ID}`);

    if (stats.name != null) tableRow.children[1].innerHTML = stats.name;
    tableRow.children[2].innerHTML = stats.onlineTime == null ? "N/A" : stats.onlineTime;
    tableRow.children[4].innerHTML = stats.accountType == null ? "N/A" : stats.accountType;
    tableRow.children[5].innerHTML = stats.level == null ? "N/A" : stats.level;
    tableRow.children[6].innerHTML = stats.killDeathRatio == null ? "N/A" : stats.killDeathRatio;
    tableRow.children[7].innerHTML = stats.survivalRate == null ? "N/A" : stats.survivalRate;
}

export function RemovePlayer(ID: number) {
    // Ensure this player is in the table
    if (PlayersOnTable[ID] == null) return;

    const element = document.getElementById(`playersTable_${ID}`);
    element.remove();

    delete PlayersOnTable[ID];
}

export function clear() {
    PlayersOnTable = {};

    playersTableTableEl.style.display = "none";
    playersTableNoPlayersEl.style.display = "";

    playersTableBodyEl.innerHTML = "";
}

function sort() {
    const rows = Array.from(document.getElementsByClassName("playersTablePlayerRow"));

    const friendlyRows: Element[] = [];
    const hostileHumanRows: Element[] = [];
    const aiRows: Element[] = [];

    rows.forEach((row) => {
        const PlayerType = Number(row.getAttribute("playertype"));

        if (row.classList.contains("playersTablePlayerRow_LocalPlayerTeammate") || row.classList.contains("playersTablePlayerRow_LocalPlayer")) friendlyRows.push(row);
        else if (AITypes.indexOf(PlayerType) !== -1) aiRows.push(row);
        else hostileHumanRows.push(row);
    });

    // Get the LocalPlayer row so it can be placed at the front of the alwaysOnTopRows array
    const alwaysFirstRow = document.getElementsByClassName("playersTablePlayerRow_LocalPlayer")[0];
    const alwaysFirstRowIndex = friendlyRows.indexOf(alwaysFirstRow);

    // Move the LocalPlayer row to the front of the alwaysOnTopRows
    if (alwaysFirstRowIndex > -1) {
        friendlyRows.splice(alwaysFirstRowIndex, 1);
        friendlyRows.unshift(alwaysFirstRow);
    }

    // Sort hostile human rows
    hostileHumanRows.sort((a, b) => {
        const textA = a.getAttribute("groupid");
        const textB = b.getAttribute("groupid");

        return textA.localeCompare(textB);
    });

    // Sort AI rows
    aiRows.sort((a, b) => {
        const textA = a.getAttribute("playertype");
        const textB = b.getAttribute("playertype");

        return textA.localeCompare(textB);
    });

    // Clear table
    playersTableBodyEl.innerHTML = "";

    // Add friendly rows
    friendlyRows.forEach((row) => {
        playersTableBodyEl.insertAdjacentElement("beforeend", row);
    });

    // Add hostile human rows
    hostileHumanRows.forEach((row) => {
        playersTableBodyEl.insertAdjacentElement("beforeend", row);
    });

    // Add AI rows
    aiRows.forEach((row) => {
        playersTableBodyEl.insertAdjacentElement("beforeend", row);
    });
}
