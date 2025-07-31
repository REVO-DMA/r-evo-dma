import { app } from "@electron/remote";
import { showAlert } from "../alert";
import { featuresManifest } from "../features/featuresManifest";
import { send as ipcSend } from "../ipc/manager";
import { KeyCodeLUT } from "../ipc/utils/KeyCode";
import { addCheckbox, addHotkey, addPlayerColorPicker, addSlider } from "../modalMaster";
import { getPreferences, savePreferences } from "../preferenceManager";
import { scaleViewportExfils, scaleViewportExplosives, scaleViewportLoot, scaleViewportPlayers, updateRenderingPerformanceMode, updateTickerMaxFPS } from "../rendering";
import { PUBLIC_BUILD } from "../../env";

/** @type {HTMLDivElement} */
const appSettingsModalEl = document.getElementById("appSettingsModal");
/** @type {HTMLDivElement} */
const appSettingsModalContainerTabsEl = document.getElementById("appSettingsModalContainerTabs");
/** @type {HTMLDivElement} */
const appSettingsModalContainerContentEl = document.getElementById("appSettingsModalContainerContent");

export function initialize() {
    const appSettings = getPreferences("appSettings");

    /** @type {Object<string, string>} */
    const Tabs = {};

    const canAddSetting = (id, tab) => {
        if (Object.keys(Tabs).length > 0 && tab == null) {
            console.error(`Unable to add app setting: "${id}". It is in a tabbed modal but is missing the "tab" property.`);
            return false;
        }

        return true;
    };

    appSettings.forEach((setting) => {
        if (setting.uiType === "tab") {
            Tabs[setting.id] = {
                name: setting.name,
            };

            const isFirstTab = Object.keys(Tabs).length === 1;

            // Add tab to UI
            const tabHTML = /*html*/ `
                <div class="col-auto modalContainerTab ${isFirstTab ? "modalContainerTabActive" : ""}" id="appSettings_${setting.id}Selector">${setting.name}</div>
            `;
            appSettingsModalContainerTabsEl.insertAdjacentHTML("beforeend", tabHTML);
            Tabs[setting.id].selector = document.getElementById(`appSettings_${setting.id}Selector`);
            const selector = Tabs[setting.id].selector;

            // Add tab body to UI
            const tabBodyHTML = /*html*/ `
                <div class="row m-0 p-0 modalContainerTabContent" id="appSettings_${setting.id}Body" ${isFirstTab ? "" : `style="display: none;"`}></div>
            `;
            appSettingsModalContainerContentEl.insertAdjacentHTML("beforeend", tabBodyHTML);
            Tabs[setting.id].body = document.getElementById(`appSettings_${setting.id}Body`);
            const body = Tabs[setting.id].body;

            // Add tab click event
            selector.addEventListener("click", () => {
                // Deactivate/hide all tab content
                for (const key in Tabs) {
                    const tab = Tabs[key];

                    // Hide tab
                    tab.selector.classList.remove("modalContainerTabActive");

                    // Hide body
                    tab.body.style.display = "none";
                }

                // Activate/show this tab
                selector.classList.add("modalContainerTabActive");
                body.style.display = "";
            });

            // Mark as the active tab if this is the first tab in the list
            if (isFirstTab) {
                // Show tabs
                appSettingsModalContainerTabsEl.style.display = "";

                // Show body
                appSettingsModalContainerContentEl.style.display = "";

                // Mark tab as active
                Tabs[setting.id].active = true;
            }
        } else if (setting.uiType === "slider") {
            if (!canAddSetting(setting.id, setting.tab)) return;

            addSlider(Tabs[setting.tab].body, setting.name, setting.info, setting.id, setting.value, setting.min, setting.max, setting.step, "appSetting", setting.valueSuffix);

            // Initialize value on backend
            if (setting.id === "radar_updateRate") {
                ipcSend(34, {
                    Value: setting.value,
                });
            }
        } else if (setting.uiType === "checkbox") {
            addCheckbox(Tabs[setting.tab].body, setting.name, setting.info, setting.id, setting.value, "appSetting");
        } else if (setting.uiType === "colorPicker") {
            addPlayerColorPicker(Tabs[setting.tab].body, setting.name, setting.id, setting.dot, setting.border);
        }
    });

    // Add hotkeys
    const hotkeysSync = [];
    for (let i = 0; i < featuresManifest.length; i++) {
        const feature = featuresManifest[i];

        if (feature.type !== "featureRoot") continue;

        let savedHotkey = getFeatureToggleHotkey(feature.id);
        if (savedHotkey == null || (getFeatureToggleHotkeyIsPrivate(feature.id) && PUBLIC_BUILD)) continue;

        hotkeysSync.push({
            ID: feature.id,
            Hotkey: KeyCodeLUT[savedHotkey],
        });

        addHotkey(appSettingsModalEl, Tabs["hotkeysTab"].body, feature.name, feature.name, feature.id, savedHotkey, null, "appSetting");
    }
    ipcSend(38, hotkeysSync);
}

/**
 * @param {string} id
 */
export function getFeatureToggleHotkey(id) {
    const featureToggleHotkeys = getPreferences("featureToggleHotkeys");

    let output = null;
    for (let i = 0; i < featureToggleHotkeys.length; i++) {
        const setting = featureToggleHotkeys[i];

        if (setting.id === id) {
            output = setting.hotkey;

            break;
        }
    }

    return output;
}

/**
 * @param {string} id
 */
export function getFeatureToggleHotkeyIsPrivate(id) {
    const featureToggleHotkeys = getPreferences("featureToggleHotkeys");

    for (let i = 0; i < featureToggleHotkeys.length; i++) {
        const setting = featureToggleHotkeys[i];

        if (setting.id === id) {
            if (setting.privateOnly != null && setting.privateOnly === true)
                return true;
            else
                return false;
        }
    }

    return true;
}

/**
 * @param {string} id
 * @param {any} hotkey
 */
export function setFeatureToggleHotkey(id, hotkey) {
    const featureToggleHotkeys = getPreferences("featureToggleHotkeys");

    for (let i = 0; i < featureToggleHotkeys.length; i++) {
        const setting = featureToggleHotkeys[i];

        if (setting.id === id) {
            setting.hotkey = hotkey;

            break;
        }
    }

    savePreferences("featureToggleHotkeys", featureToggleHotkeys);
}

/**
 * @param {string} id
 */
export function getAppSettingValue(id) {
    const appSettings = getPreferences("appSettings");

    let output = null;
    for (let i = 0; i < appSettings.length; i++) {
        const setting = appSettings[i];

        if (setting.id === id) {
            output = setting.value;

            break;
        }
    }

    return output;
}

/**
 * @param {string} id
 * @param {any} value
 */
export function setAppSettingValue(id, value) {
    const appSettings = getPreferences("appSettings");

    for (let i = 0; i < appSettings.length; i++) {
        const setting = appSettings[i];

        if (setting.id === id) {
            setting.value = value;

            break;
        }
    }

    savePreferences("appSettings", appSettings);

    // Perform post-save operations
    if (id === "player_scale") {
        scaleViewportPlayers();
    } else if (id === "loot_scale") {
        scaleViewportLoot();
    } else if (id === "grenade_scale") {
        scaleViewportExplosives();
    } else if (id === "exfil_scale") {
        scaleViewportExfils();
    } else if (id === "radar_updateRate") {
        updateTickerMaxFPS();

        ipcSend(34, {
            Value: getAppSettingValue("radar_updateRate"),
        });
    } else if (id === "radar_performanceMode") {
        updateRenderingPerformanceMode();

        showAlert(
            "warning",
            "Restart Required",
            "The setting you just changed will not take effect until the application is restarted. Would you like to restart now?",
            true,
            false,
            "Restart",
            "",
            "Cancel",
            (result) => {
                if (result) {
                    app.relaunch();
                    app.quit();
                }
            }
        );
    }
}

/**
 * @param {string} id
 * @param {boolean} isDot
 * @param {string} newColor
 */
export function SetAppSettingPlayerColor(id, isDot, newColor) {
    const appSettings = getPreferences("appSettings");

    for (let i = 0; i < appSettings.length; i++) {
        const setting = appSettings[i];

        if (setting.id === id) {
            if (isDot) setting.dot = newColor;
            else setting.border = newColor;

            break;
        }
    }

    savePreferences("appSettings", appSettings);
}
