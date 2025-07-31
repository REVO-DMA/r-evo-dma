import { syncESPSettings, syncESPStyles } from "../ipc/manager";
import { addCheckbox, addColorPicker, addDisplayPicker, addHotkey, addPlayerTypeEspStyle, addSlider, addToggleButtons, addVisibleInvisibleColorPicker } from "../modalMaster";
import { getPreferences, savePreferences } from "../preferenceManager";

const espSettingsModalEl = document.getElementById("espSettingsModal");
const espSettingsContainerContentEl = document.getElementById("espSettingsContainerContent");

export async function initialize() {
    generateESP_HTML();
    updateSectionVisibility();
    syncESPSettings();
    syncESPStyles();
}

/**
 * @param {string} id
 * @param {any} newValue
 * @param {string} property
 * @param {string} subproperty
 */
export function setESP_SettingValue(id, newValue, property = "value", subproperty = null) {
    const preferences = getPreferences("espSettings");

    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.id === id) {
            if (subproperty === null)
                preference[property] = newValue;
            else {
                const parsed = JSON.parse(preference[property]);
                parsed[subproperty] = newValue;

                preference[property] = JSON.stringify(parsed);
            }

            break;
        }
    }

    savePreferences("espSettings", preferences);
    updateSectionVisibility();
}

function getSettingValue(espSettings, searchSetting) {
    for (let i = 0; i < espSettings.length; i++) {
        const setting = espSettings[i];
        
        if (setting.id === searchSetting) {
            return setting.value;
        }
    }

    return null;
}

function updateSectionVisibility() {
    console.log("running");
    const espSettings = getPreferences("espSettings");

    // Low Latency Mode
    {
        const value = getSettingValue(espSettings, "esp_lowLatencyMode");
        if (value === "Disabled")
            document.getElementById("esp_updateSpeed_modalSettingInnerContainer").style.display = "";
        else
            document.getElementById("esp_updateSpeed_modalSettingInnerContainer").style.display = "none";
    }

    // Display Style
    {
        const value = getSettingValue(espSettings, "esp_displayStyle");
        if (value === "Normal") {
            document.getElementById("espSection_windowProperties").style.display = "";
            document.getElementById("esp_monitor_modalSettingInnerContainer").style.display = "none";
            document.getElementById("esp_lowLatencyMode_modalSettingInnerContainer").style.display = "none";
            document.getElementById("esp_updateSpeed_modalSettingInnerContainer").style.display = "";
        }
        else {
            document.getElementById("espSection_windowProperties").style.display = "none";
            document.getElementById("esp_monitor_modalSettingInnerContainer").style.display = "";
            document.getElementById("esp_lowLatencyMode_modalSettingInnerContainer").style.display = "";
        }
    }

    // Player Name
    {
        const value = getSettingValue(espSettings, "esp_showPlayersName");
        if (value)
            document.getElementById("esp_showPlayersTeamNumber_modalSettingInnerContainer").style.display = "";
        else
            document.getElementById("esp_showPlayersTeamNumber_modalSettingInnerContainer").style.display = "none";
    }

    // Dynamic Crosshair
    {
        const value = getSettingValue(espSettings, "esp_dynamicCrosshair");
        if (value)
            document.getElementById("esp_fakeLaser_modalSettingInnerContainer").style.display = "";
        else
            document.getElementById("esp_fakeLaser_modalSettingInnerContainer").style.display = "none";
    }
}

function generateESP_HTML() {
    const espSettings = getPreferences("espSettings");

    espSettings.forEach((setting) => {
        let modalBody = espSettingsContainerContentEl;
        if (setting.sectionID != null) {
            const el = document.getElementById(setting.sectionID);
            if (el !== null) modalBody = el;
        }

        if (setting.uiType === "section") {
            espSettingsContainerContentEl.insertAdjacentHTML(
                "beforeend",
                /*html*/ `
                <div class="row m-0 modalSettingContainer" id="${setting.id}">
                    <div class="row m-0 mb-1 p-0 modalSettingSection">
                        <div class="col p-0">${setting.name}</div>
                    </div>
                </div>
            `
            );
        } else if (setting.uiType === "toggleButtons") {
            addToggleButtons(modalBody, setting.name, setting.id, setting.info, setting.value, setting.values, "espSetting");

            if (setting.id === "esp_mode") {
                // Initialize ESP window
                setESP_SettingValue("esp_mode", setting.value);
            }
        } else if (setting.uiType === "slider") {
            addSlider(modalBody, setting.name, setting.info, setting.id, setting.value, setting.min, setting.max, setting.step, "espSetting", setting.valueSuffix);
        } else if (setting.uiType === "checkbox") {
            addCheckbox(modalBody, setting.name, setting.info, setting.id, setting.value, "espSetting");
        } else if (setting.uiType === "colorPicker") {
            addColorPicker(modalBody, setting.name, setting.id, setting.value, "espSetting");
        } else if (setting.uiType === "displayChooser") {
            addDisplayPicker(modalBody, setting.name, setting.id, setting.value);
        } else if (setting.uiType === "visibleInvisibleColorPicker") {
            addVisibleInvisibleColorPicker(modalBody, setting.name, setting.id, setting.visibleColor, setting.invisibleColor);
        } else if (setting.uiType === "hotkey") {
            addHotkey(espSettingsModalEl, modalBody, setting.name, setting.name.split(" Hotkey")[0], setting.id, setting.value, null, "espSetting");
        } else if (setting.uiType === "playerTypeEspStyle") {
            addPlayerTypeEspStyle(espSettingsModalEl, modalBody, setting.name, setting.name, setting.id, setting.value, setting.playerType);
        }
    });
}
