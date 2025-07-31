import { SetFeatureStatus, getFeatureSettings, isFeatureEnabled } from "../features/individualFeature";
import { getPreferences } from "../preferenceManager";
import { deserialize as deserializePlayerStatus } from "./messageHandlers/playerStatus";
import { deserialize as deserializeQuestLocations } from "./messageHandlers/questLocations";
import { deserialize as deserializeRadarLoot } from "./messageHandlers/radarLoot";
import { deserialize as deserializeRadarStatus, setMainAppStatus } from "./messageHandlers/radarStatus";
import { BonesLUT } from "./utils/Bones";
import BufferReader from "./utils/BufferReader";
import { KeyCodeLUT } from "./utils/KeyCode";

import { IPC_DeferredPlayer } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_DeferredPlayer";
import { allocate as allocatePlayer, updateDeferred, updateRealtime as updateRealtimePlayers, updateStats } from "../rendering/player/playerManager";
import { allocate as allocateGrenade, destroy as destroyGrenade, updateRealtime as updateRealtimeGrenades } from "../rendering/grenade/grenadeManager";
import { allocate as allocateMortar, destroy as destroyMortar, updateRealtime as updateRealtimeMortars } from "../rendering/mortar/mortarManager";
import { IPC_StaticPlayer } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_StaticPlayer";
import { IPC_StaticGrenade } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_StaticGrenade";
import { IPC_AuthStatus } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_AuthStatus";
import { ProcessStatus as ProcessAuthStatus } from "../auth";
import { IPC_FeatureState } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_FeatureState";
import { IPC_Exfil } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_Exfil";
import { update as updateExfils } from "../rendering/exfil/exfilManager";
import { IPC_PlayerStats } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_PlayerStats";

import { fromBinary } from "@bufbuild/protobuf";
import { RadarRealtimeSchema } from "../../../gen/radar_realtime_pb";
import { MortarSchema } from "../../../gen/mortar_pb";

export let IPC_READY = false;

/** @type {Worker} */
let IPC_WORKER = null;

/**
 * Initialize IPC.
 */
export function initialize() {
    setMainAppStatus("Starting DMA");

    IPC_WORKER = new Worker(new URL("./worker.js", import.meta.url));

    // Handle IPC Server data
    IPC_WORKER.onmessage = (e) => {
        // This is a message originating from the remote IPC server
        const buffer = Buffer.from(new Uint8Array(e.data));
        const bufferReader = new BufferReader(buffer);

        const messageType = bufferReader.readUInt8();

        if (messageType === 1) {
            // Player (re)allocation
            const staticPlayer = IPC_StaticPlayer.deserialize(e.data.slice(2));

            allocatePlayer(staticPlayer);
        } else if (messageType === 2) {
            // World update - realtime
            const realtime = fromBinary(RadarRealtimeSchema, buffer.subarray(2));

            updateRealtimePlayers(realtime.players);
            updateRealtimeGrenades(realtime.grenades);
            updateRealtimeMortars(realtime.mortars);
        } else if (messageType === 3) {
            // Player update - deferred
            const deferredPlayers = IPC_DeferredPlayer.deserializeArray(e.data.slice(2));

            updateDeferred(deferredPlayers);
        } else if (messageType === 20) {
            // Radar status
            deserializeRadarStatus(bufferReader);
        } else if (messageType === 21) {
            // Player status
            deserializePlayerStatus(bufferReader);
        } else if (messageType === 22) {
            // Radar Loot
            deserializeRadarLoot(bufferReader);
        } else if (messageType === 25 || messageType === 26) {
            // Grenade update - static
            const staticGrenade = IPC_StaticGrenade.deserialize(e.data.slice(2));

            if (messageType === 25) {
                allocateGrenade(staticGrenade);
            } else if (messageType === 26) {
                destroyGrenade(staticGrenade.iD);
            }
        } else if (messageType === 27) {
            const exfils = IPC_Exfil.deserializeArray(e.data.slice(2));

            updateExfils(exfils);
        } else if (messageType === 33) {
            // Auth status
            const authStatus = IPC_AuthStatus.deserialize(e.data.slice(2));
            
            ProcessAuthStatus(authStatus);
        } else if (messageType === 39) {
            const featureState = IPC_FeatureState.deserialize(e.data.slice(2));

            const checkboxContainer = document.getElementById(`${featureState.iD}_checkbox`);
            if (featureState.enabled) {
                // Enable
                checkboxContainer.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
                SetFeatureStatus(featureState.iD, true);
            } else {
                // Disable
                checkboxContainer.innerHTML = "";
                SetFeatureStatus(featureState.iD, false);
            }
        } else if (messageType === 41) {
            // Radar Loot
            deserializeQuestLocations(bufferReader);
        } else if (messageType === 42) {
            // Player Stats
            const playerStats = IPC_PlayerStats.deserialize(e.data.slice(2));

            updateStats(playerStats);
        } else if (messageType === 44 || messageType === 45) {
            const mortar = fromBinary(MortarSchema, buffer.subarray(2));

            if (messageType === 44) {
                allocateMortar(mortar);
            } else if (messageType === 45) {
                destroyMortar(mortar.id);
            }
        } else if (messageType === 200) {
            // IPC Ready
            IPC_READY = true;
        }
    };
}

/**
 * Send data to the IPC Server.
 * @param {number} type
 * @param {any} data
 */
export function send(type, data) {
    if (IPC_WORKER === null) return;

    // TODO: Make this send data to IPC thread utilizing a transferable
    IPC_WORKER.postMessage([type, data]);
}

/**
 * @param {string[]} featureIDs
 */
export function syncFeatures(featureIDs) {
    const syncPacket = [];

    featureIDs.forEach((featureID) => {
        // Get the feature's enabled status
        const isEnabled = isFeatureEnabled(featureID);

        if (isEnabled == null) {
            console.log(`Error syncing feature with id of "${featureID}". "isFeatureEnabled" returned null.`);
            return;
        }

        const featureData = {
            ID: featureID,
            Enabled: isEnabled,
            Settings: [],
        };

        // Get all of it's feature settings
        const featureSettings = getFeatureSettings(featureID);

        if (featureSettings.length > 0) {
            featureSettings.forEach((featureSetting) => {
                let overriddenDataType = null;

                // Add this setting to the feature's settings array
                let settingValue = String(featureSetting.value);
                if (featureSetting.dataType === "UnityKeyCode") {
                    settingValue = String(KeyCodeLUT[featureSetting.value]);
                } else if (featureSetting.dataType === "Bones") {
                    settingValue = String(BonesLUT[featureSetting.value]);
                    overriddenDataType = "int";
                } else if (featureSetting.dataType === "ChamsMode") {
                    if (featureSetting.value === "Always") 
                    {
                        settingValue = String(0);

                        const visibleTextElements = Array.from(document.getElementsByClassName("visCheckColorPicker_visibleText"));
                        visibleTextElements.forEach(element => {
                            element.innerText = "Color";
                        });

                        const invisibleContainerElements = Array.from(document.getElementsByClassName("visCheckColorPicker_invisibleContainer"));
                        invisibleContainerElements.forEach(element => {
                            element.style.display = "none";
                        });
                    }
                    else if (featureSetting.value === "Vis Check")
                    {
                        settingValue = String(1);

                        const visibleTextElements = Array.from(document.getElementsByClassName("visCheckColorPicker_visibleText"));
                        visibleTextElements.forEach(element => {
                            element.innerText = "Visible";
                        });

                        const invisibleContainerElements = Array.from(document.getElementsByClassName("visCheckColorPicker_invisibleContainer"));
                        invisibleContainerElements.forEach(element => {
                            element.style.display = "";
                        });
                    }
                    else if (featureSetting.value === "Visible")
                    {
                        settingValue = String(2);

                        const visibleTextElements = Array.from(document.getElementsByClassName("visCheckColorPicker_visibleText"));
                        visibleTextElements.forEach(element => {
                            element.innerText = "Color";
                        });

                        const invisibleContainerElements = Array.from(document.getElementsByClassName("visCheckColorPicker_invisibleContainer"));
                        invisibleContainerElements.forEach(element => {
                            element.style.display = "none";
                        });
                    }

                    // Fix LocalPlayer settings
                    {
                        const containerVisible = document.getElementsByClassName("visCheckColorPicker_visibleContainer_chams_LocalPlayer_visible");
                        if (containerVisible.length > 0) containerVisible[0].style.display = "";

                        const containerInvisible = document.getElementsByClassName("visCheckColorPicker_invisibleContainer_chams_LocalPlayer_invisible");
                        if (containerInvisible.length > 0) containerInvisible[0].style.display = "";

                        const localPlayerVisible = document.getElementsByClassName("visCheckColorPicker_visibleText_chams_LocalPlayer_visible");
                        if (localPlayerVisible.length > 0) localPlayerVisible[0].innerText = "Wire";

                        const localPlayerInvisible = document.getElementsByClassName("visCheckColorPicker_invisibleText_chams_LocalPlayer_invisible");
                        if (localPlayerInvisible.length > 0) localPlayerInvisible[0].innerText = "Base";
                    }

                    overriddenDataType = "int";
                } else if (featureSetting.dataType === "TargetingMode") {
                    if (featureSetting.value === "Smart") settingValue = String(1);
                    else if (featureSetting.value === "CQB") settingValue = String(2);
                    else if (featureSetting.value === "Crosshair") settingValue = String(3);

                    overriddenDataType = "int";
                } else if (featureSetting.dataType === "LockingMode") {
                    if (featureSetting.value === "Silent Aim") settingValue = String(1);
                    else if (featureSetting.value === "Hard Lock") settingValue = String(2);

                    overriddenDataType = "int";
                } else if (featureSetting.dataType === "VisCheckColor") {
                    settingValue = String(`${featureSetting.color_visible}_${featureSetting.color_invisible}`);
                }

                featureData.Settings.push({
                    ID: featureSetting.id,
                    Type: overriddenDataType !== null ? overriddenDataType : featureSetting.dataType,
                    Value: settingValue,
                });
            });
        }

        // Push this feature to the sync packet
        syncPacket.push(featureData);
    });

    // Send to backend
    send(23, syncPacket);
}

export function syncESPSettings() {
    const espSettings = getPreferences("espSettings");

    const featureData = {
        Settings: [],
    };

    espSettings.forEach((setting, i) => {
        if (setting.remote == null || setting.remote === false) return;

        if (setting.uiType === "visibleInvisibleColorPicker") {
            featureData.Settings.push({
                ID: `${setting.id}_visible`,
                Value: setting.visibleColor,
            });

            featureData.Settings.push({
                ID: `${setting.id}_invisible`,
                Value: setting.invisibleColor,
            });
        } else if (setting.uiType === "hotkey") {
            featureData.Settings.push({
                ID: setting.id,
                Value: String(KeyCodeLUT[setting.value]),
            });
        } else {
            featureData.Settings.push({
                ID: setting.id,
                Value: String(setting.value),
            });
        }
    });

    // Push this feature to the sync packet
    const syncPacket = featureData;

    // Send to backend
    send(29, syncPacket);
}

export function syncESPStyles() {
    const espSettings = getPreferences("espSettings");

    const config = {
        Styles: [],
    };

    espSettings.forEach((setting, i) => {
        if (setting.remote == null || setting.remote === false) return;

        if (setting.uiType === "playerTypeEspStyle") {
            const settingValue = JSON.parse(setting.value);
            settingValue.id = setting.playerID;
            config.Styles.push(settingValue);
        }
    });

    send(43, config);
}

export function syncLootFilter(data) {
    send(31, data);
}
