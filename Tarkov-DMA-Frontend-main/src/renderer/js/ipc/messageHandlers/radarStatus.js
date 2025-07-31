import { quitApp } from "../../../env";
import { showAlert } from "../../alert";
import { onRaidEnd as exfilsOnRaidEnd } from "../../rendering/exfil/exfilManager";
import { onRaidEnd as grenadesOnRaidEnd } from "../../rendering/grenade/grenadeManager";
import { initialize as initializeCanvas, shutdown as shutdownCanvas } from "../../rendering/index.ts";
import { onRaidEnd as lootOnRaidEnd } from "../../rendering/loot/lootManager";
import { currentMap, setMap } from "../../rendering/maps/mapManager.ts";
import { onRaidEnd as playersOnRaidEnd } from "../../rendering/player/playerManager";
import { onRaidEnd as questLocationsOnRaidEnd } from "../../rendering/questLocation/questManager";
import { onRaidEnd as mortarsOnRaidEnd } from "../../rendering/mortar/mortarManager";
import { ResetCache as ResetStreamerCheckerCache } from "../../streamerChecker";
import { shutdown as shutdownBackend } from "../backend";

const mainAppStatusEl = document.getElementById("mainAppStatus");
const mainAppStatusTextEl = document.getElementById("mainAppStatusText");

/**
 * @param {import("../utils/BufferReader.js").default} bufferReader
 */
export async function deserialize(bufferReader) {
    const Status = bufferReader.readUInt8();

    const onRaidEnd = () => {
        playersOnRaidEnd();
        lootOnRaidEnd();
        questLocationsOnRaidEnd();
        grenadesOnRaidEnd();
        exfilsOnRaidEnd();
        mortarsOnRaidEnd();
        shutdownCanvas();
        ResetStreamerCheckerCache();
    };

    if (Status === 1) {
        // Raid Began
        await initializeCanvas(currentMap);
        setMainAppStatus("", true);
    } else if (Status === 2) {
        // Raid Ended
        onRaidEnd();
        setMainAppStatus("Waiting for raid start");
    } else if (Status === 3) {
        // woods Selected
        setMap("woods");
    } else if (Status === 4) {
        // shoreline Selected
        setMap("shoreline");
    } else if (Status === 5) {
        // rezervbase Selected
        setMap("rezervbase");
    } else if (Status === 6) {
        // laboratory Selected
        setMap("laboratory");
    } else if (Status === 7) {
        // interchange Selected
        setMap("interchange");
    } else if (Status === 8) {
        // factory Selected
        setMap("factory");
    } else if (Status === 9) {
        // bigmap Selected
        setMap("bigmap");
    } else if (Status === 10) {
        // lighthouse Selected
        setMap("lighthouse");
    } else if (Status === 11) {
        // tarkovstreets Selected
        setMap("tarkovstreets");
    } else if (Status === 12) {
        // groundzero Selected
        setMap("groundzero");
    } else if (Status === 21) {
        // Process Started
        setMainAppStatus("Waiting for raid start");
    } else if (Status === 22) {
        // Process Ended
        setMainAppStatus("Searching for game process");
    } else if (Status === 23) {
        // Loading Features
        setMainAppStatus("Loading features");
    } else if (Status === 24) {
        // Game restart required
        setMainAppStatus("Error - please restart your game");
    } else if (Status === 25) {
        // Disable auto ram cleaner
        showAlert(
            "error",
            "Game Error",
            "Please disable Auto RAM Cleaner and then restart your game.",
            false,
            false,
            "Close",
            "",
            ""
        );
    } else if (Status === 26) {
        // Teleporting AI countdown
        setMainAppStatus("Teleporting AI in 3");
        setTimeout(() => {
            setMainAppStatus("Teleporting AI in 2");
            setTimeout(() => {
                setMainAppStatus("Teleporting AI in 1");
                setTimeout(() => {
                    setMainAppStatus("", true);
                }, 1000);
            }, 1000);
        }, 1000);
    } else if (Status === 27) {
        // Initializing DMA
        setMainAppStatus("Initializing DMA Connection");
    } else if (Status === 28) {
        onRaidEnd();
        setMainAppStatus("Membership expired.<br>Renew your subscription to keep playing!<br>The app will close in 10 seconds.");
        canUpdateStatus = false;
        shutdownBackend();
        setTimeout(() => {
            quitApp();
        }, 10000);
    }
}

let canUpdateStatus = true;

export function setMainAppStatus(text, hide = false, force = false) {
    if (!canUpdateStatus && !force) return;

    if (hide) mainAppStatusEl.style.display = "none";
    else mainAppStatusEl.style.display = "";

    mainAppStatusTextEl.innerHTML = text;
}
