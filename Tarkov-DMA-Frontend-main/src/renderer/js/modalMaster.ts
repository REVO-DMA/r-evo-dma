import Pickr from "@simonwep/pickr";
import { setESP_SettingValue } from "./esp/esp";
import { setFeatureSettingValue, setFeatureSettingVisCheckColorValue } from "./features/individualFeature";
import { send as ipcSend, syncESPSettings, syncESPStyles, syncFeatures } from "./ipc/manager";
import { BonesArray, FriendlyBones } from "./ipc/utils/Bones";
import { FriendlyKeyCodes, KeyCodeLUT, KeyCodesArray } from "./ipc/utils/KeyCode";
import { initializeChildModal } from "./modals";
import { SetAppSettingPlayerColor, setAppSettingValue, setFeatureToggleHotkey } from "./settings/appSettings";
import { getPickrBaseConfig } from "./colorPicker/config";
import { screen } from "@electron/remote";
import { initialize as initializeTooltips } from "./tooltips";
import { PlayerType } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/PlayerType";

const appContainerEl = (document.getElementById("appContainer") as HTMLDivElement);

export function createModal(name: string, id: string, className: string, parentElement: HTMLDivElement) {
    // Make sure this modal does not exist
    if (document.getElementById(id) !== null) {
        // Bring the already existing element to front
        document.getElementById(id).dispatchEvent(new MouseEvent("mousedown"));

        return null;
    }

    const HTML = /*html*/ `
        <div class="modalContainer ${className}" id="${id}">
            <div class="row m-0 align-items-center justify-content-between modalContainerTitlebar" id="${id}Titlebar">
                <div class="col-auto p-0 ms-2 modalContainerTitlebarTitle">${name}</div>
                <div class="col-auto p-0 me-2 modalContainerTitlebarCloseButton">
                    <i class="fa-duotone fa-circle-xmark buttonIcon"></i>
                </div>
            </div>
            <!-- Modal Body -->
            <div class="row m-0 modalContainerContent"></div>
        </div>
    `;

    appContainerEl.insertAdjacentHTML("beforeend", HTML);

    const childElement: HTMLDivElement = (document.getElementById(id) as HTMLDivElement);

    initializeChildModal(childElement, parentElement);

    return childElement.children[1];
}

export function addCheckbox(modalBody: HTMLDivElement, name: string, info: string, id: string, checked: boolean, valueType: ("featureSetting"|"appSetting"|"espSetting"), settingOwnerID: string = null) {
    const tooltip = info ? `
    <div class="col-auto ms-2 p-0" data-tippy-content="${info}">
        <i class="fa-duotone fa-circle-info"></i>
    </div>` : "";

    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer align-items-center" id="${id}_modalSettingInnerContainer">
            <div class="col-auto p-0">
                <div class="checkbox" id="${id}">
                    ${checked ? `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>` : ""}
                </div>
            </div>
            <div class="col-auto pe-0">${name}</div>
            ${tooltip}
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    /** @type {HTMLDivElement} */
    const checkboxContainer = document.getElementById(id);
    checkboxContainer.addEventListener("click", () => {
        /** @type {Function<void>} */
        let onEnabled = null;
        /** @type {Function<void>} */
        let onDisabled = null;

        if (valueType === "featureSetting") {
            onEnabled = () => {
                // Persist feature setting
                setFeatureSettingValue(id, true);

                // Sync feature with backend
                syncFeatures([settingOwnerID]);
            };

            onDisabled = () => {
                // Persist feature setting
                setFeatureSettingValue(id, false);

                // Sync feature with backend
                syncFeatures([settingOwnerID]);
            };
        } else if (valueType === "appSetting") {
            onEnabled = () => {
                setAppSettingValue(id, true);
            };

            onDisabled = () => {
                setAppSettingValue(id, false);
            };
        } else if (valueType === "espSetting") {
            onEnabled = () => {
                // Persist ESP setting
                setESP_SettingValue(id, true);

                // Sync ESP settings with backend
                syncESPSettings();
            };

            onDisabled = () => {
                // Persist ESP setting
                setESP_SettingValue(id, false);

                // Sync ESP settings with backend
                syncESPSettings();
            };
        } else {
            console.log(`addCheckbox() click event was unable to persist the checkbox state. No handler for valueType "${valueType}" exists!`);
            return;
        }

        if (onEnabled === null || onDisabled === null) {
            console.log(`addCheckbox() click event was unable to handle the checkbox. Either "onEnabled" or "onDisabled" or both bindings are null for "${valueType}"!`);
            return;
        }

        if (checkboxContainer.innerHTML.trim() === "") {
            // Enable
            checkboxContainer.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;

            onEnabled();
        } else {
            // Disable
            checkboxContainer.innerHTML = "";

            onDisabled();
        }
    });
}

export function addSlider(modalBody: HTMLDivElement, name: string, info: string, id: string, value: number, min: number, max: number, step: number, valueType: ("featureSetting"|"appSetting"|"espSetting"), valueSuffix: string = null, settingOwnerID: string = null) {
    const tooltip = info ? `
    <div class="col-auto ms-2 p-0" data-tippy-content="${info}">
        <i class="fa-duotone fa-circle-info"></i>
    </div>` : "";

    const getDecimalPlaces = (number: number) => {
        // Convert the number to a string
        const numberString = number.toString();

        // Use a regular expression to match the decimal places
        const decimalMatch = numberString.match(/\.(\d+)/);

        // If there is a match, return the length of the decimal places
        if (decimalMatch) {
            return decimalMatch[1].length;
        }

        // If there is no decimal places, return 0
        return 0;
    };

    const getPadStartCount = (number: number) => {
        return number.toFixed(0).toString().length;
    };

    const decimalPlaces = getDecimalPlaces(step);
    /** @type {number} */
    let padStartCount;
    if (decimalPlaces === 0) {
        padStartCount = getPadStartCount(max);
    } else {
        padStartCount = getPadStartCount(max) + decimalPlaces + 1; // The 1 is for the decimal
    }

    if (valueSuffix == null) valueSuffix = "";
    else valueSuffix = " " + valueSuffix;

    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col-auto p-0">${name}</div>
                ${tooltip}
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
                <div class="col p-0">
                    <input type="range" min="${min}" max="${max}" step="${step}" value="${value}" id="${id}">
                </div>
                <div class="col-auto ms-2 p-0 rangeValue" id="${id}RangeValue">${value.toFixed(decimalPlaces).padStart(padStartCount, "0")}${valueSuffix}</div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    /** @type {HTMLInputElement} */
    const slider: HTMLInputElement = (document.getElementById(id) as HTMLInputElement);
    const sliderReadout: HTMLDivElement = (document.getElementById(`${id}RangeValue`) as HTMLDivElement);
    slider.addEventListener("input", () => {
        const newValue = Number(slider.value);
        sliderReadout.innerText = newValue.toFixed(decimalPlaces).padStart(padStartCount, "0") + valueSuffix;
    });
    slider.addEventListener("change", () => {
        const newValue = Number(slider.value);

        if (valueType === "featureSetting") {
            // Persist feature setting
            setFeatureSettingValue(id, newValue);
            // Sync feature with backend
            syncFeatures([settingOwnerID]);
        } else if (valueType === "appSetting") {
            setAppSettingValue(id, newValue);
        } else if (valueType === "espSetting") {
            // Persist ESP setting
            setESP_SettingValue(id, newValue);
            // Sync ESP settings with backend
            syncESPSettings();
        } else {
            console.log(`addSlider() change event was unable to persist the slider value. No handler for valueType "${valueType}" exists!`);
            return;
        }
    });
}

export function addHotkey(selfModal: HTMLDivElement, modalBody: HTMLDivElement, name: string, parentName: string, id: string, value: string, settingOwnerID: string = null, valueType: ("featureSetting"|"appSetting"|"espSetting")) {
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-between align-items-center">
                <div class="col-auto p-0 currentItemVisualization">
                    <i class="fa-duotone fa-keyboard me-2"></i><span id="${id}CurrentHotkeyVisualization">${value === "None" ? "Not Set" : FriendlyKeyCodes[value]}</span>
                </div>
                <div class="col-auto ms-2 p-0 changeHotkeyButton" id="${id}ChangeHotkey">
                    <i class="fa-duotone fa-arrows-rotate me-2"></i>Change
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    const currentHotkeyVisualizationEl = document.getElementById(`${id}CurrentHotkeyVisualization`);

    const changeHotkeyEl = document.getElementById(`${id}ChangeHotkey`);
    changeHotkeyEl.addEventListener("click", () => {
        const generateHotkeyBody = () => {
            const HTML: string[] = [];

            KeyCodesArray.forEach((keyCode) => {
                const friendly = FriendlyKeyCodes[keyCode];

                if (friendly == null) return;

                HTML.push(/*html*/ `
                    <div class="row m-0 justify-content-between align-items-center hotkeyRow ${id}HotkeyRow ${keyCode === value ? "selectedHotkeyRow" : ""}">
                        <div class="col-auto friendlyHotkey">
                            <kbd>${friendly}</kbd>
                        </div>
                        <div class="col-auto hotkeyName">${keyCode}</div>
                    </div>
                `);
            });

            return HTML.join("");
        };

        const hotkeySelectionModalBody = createModal(`${parentName} Hotkey`, `${id}HotkeySelection`, "hotkeySelectionModalContainer", selfModal);
        if (hotkeySelectionModalBody === null) return;

        hotkeySelectionModalBody.insertAdjacentHTML("beforeend", generateHotkeyBody());

        const hotkeyRows = Array.from(document.getElementsByClassName(`${id}HotkeyRow`));
        hotkeyRows.forEach((hotkeyRow) => {
            hotkeyRow.addEventListener("click", () => {
                // Get clicked key's keyCode
                const keyCode = (hotkeyRow.children[1] as HTMLElement).innerText;

                if (valueType === "featureSetting") {
                    // Save the new keyCode
                    setFeatureSettingValue(id, keyCode);

                    // Sync feature with backend
                    syncFeatures([settingOwnerID]);
                } else if (valueType === "appSetting") {
                    // Save the new keyCode
                    setFeatureToggleHotkey(id, keyCode);

                    // Sync keyCode with backend
                    ipcSend(38, [
                        {
                            ID: id,
                            Hotkey: KeyCodeLUT[keyCode],
                        },
                    ]);
                } else if (valueType === "espSetting") {
                     // Persist ESP setting
                    setESP_SettingValue(id, keyCode);
                    // Sync ESP settings with backend
                    syncESPSettings();
                }

                // Update parameter to new keyCode
                value = keyCode;

                // Update current hotkey visualization
                currentHotkeyVisualizationEl.innerText = keyCode === "None" ? "Not Set" : FriendlyKeyCodes[keyCode];

                // Close hotkey selection modal (deletes it)
                (hotkeySelectionModalBody.closest(".modalContainer").children[0].children[1] as HTMLElement).click();
            });
        });
    });
}

export type PlayerTypeEspStyle = {
    boxColorVisible: string;
    boxColorInvisible: string;

    boneColorVisible: string;
    boneColorInvisible: string;

    fontColor: string;
};

export function addPlayerTypeEspStyle(selfModal: HTMLDivElement, modalBody: HTMLDivElement, name: string, parentName: string, id: string, value: string, playerType: ("human"|"ai")) {
    const savedEspStyle = (JSON.parse(value) as PlayerTypeEspStyle);

    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0 justify-content-between align-items-center">
                <div class="col p-0">${name}</div>

                <div class="col-auto ms-2 p-0 changeStyleButton" id="${id}ChangeStyle">
                    <i class="fa-duotone fa-arrows-rotate me-2"></i>Configure
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    const changeStyleEl = document.getElementById(`${id}ChangeStyle`);
    changeStyleEl.addEventListener("click", () => {
        const generateColorPicker = (name: string, id: string) => {
            return /*html*/`
                <div class="row m-0 modalSettingInnerContainer">
                <div class="row m-0 mb-2 p-0">
                    <div class="col p-0">${name}</div>
                    
                    <div class="col-auto p-0 modalColorPicker">
                        <div id="PlayerTypeEspStyleColorPicker_${id}"></div>
                    </div>
                    
                    <div class="col-auto">
                        <div>Choose Color</div>
                    </div>
                </div>
            </div>
            `;
        };

        const attachPickr = (color: string, elementID: string) => {
            const colorPickerElement = document.getElementById(`PlayerTypeEspStyleColorPicker_${elementID}`);

            const colorPicker = Pickr.create({
                default: color,
                ...getPickrBaseConfig(colorPickerElement),
            });
    
            const getDecimalFromPickr = () => {
                return colorPicker.getColor().toHEXA().toString();
            };
    
            colorPicker.on("changestop", () => {
                const newColor = getDecimalFromPickr();
                (savedEspStyle as any)[elementID] = newColor;
                setESP_SettingValue(id, newColor, "value", elementID);
                syncESPStyles();
            });
        };

        const configurationModalBody = createModal(parentName, `${id}Configuration`, "changePlayerEspStyleModalContainer", selfModal);
        if (configurationModalBody === null) return;

        const html: string[] = [];

        html.push(generateColorPicker("Bone Visible", "boneColorVisible"));
        html.push(generateColorPicker("Bone Invisible", "boneColorInvisible"));
        
        html.push(generateColorPicker("Box Visible", "boxColorVisible"));
        html.push(generateColorPicker("Box Invisible", "boxColorInvisible"));

        html.push(generateColorPicker("Text", "fontColor"));

        configurationModalBody.insertAdjacentHTML("beforeend", html.join(""));

        attachPickr(savedEspStyle.boneColorVisible, "boneColorVisible");
        attachPickr(savedEspStyle.boneColorInvisible, "boneColorInvisible");

        attachPickr(savedEspStyle.boxColorVisible, "boxColorVisible");
        attachPickr(savedEspStyle.boxColorInvisible, "boxColorInvisible");

        attachPickr(savedEspStyle.fontColor, "fontColor");
    });
}

export function addBones(selfModal: HTMLDivElement, modalBody: HTMLElement, name: string, parentName: string, id: string, value: string, settingOwnerID: string) {
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-between align-items-center">
                <div class="col-auto p-0 currentItemVisualization">
                    <i class="fa-duotone fa-bone me-2"></i><span id="${id}CurrentAimbotBoneVisualization">${FriendlyBones[value]}</span>
                </div>
                <div class="col-auto ms-2 p-0 changeHotkeyButton" id="${id}ChangeAimbotBone">
                    <i class="fa-duotone fa-arrows-rotate me-2"></i>Change
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    const currentAimbotBoneVisualizationEl = document.getElementById(`${id}CurrentAimbotBoneVisualization`);

    const changeAimbotBoneEl = document.getElementById(`${id}ChangeAimbotBone`);
    changeAimbotBoneEl.addEventListener("click", () => {
        const generateBonesBody = () => {
            const HTML: string[] = [];

            BonesArray.forEach((bone) => {
                const friendly = FriendlyBones[bone];

                HTML.push(/*html*/ `
                    <div class="row m-0 justify-content-between align-items-center hotkeyRow ${id}HotkeyRow ${bone === value ? "selectedHotkeyRow" : ""}">
                        <div class="col-auto ms-3 friendlyBone">
                            <i class="fa-duotone fa-bone me-2"></i>${friendly}
                        </div>
                        <div class="col-auto hotkeyName">${bone}</div>
                    </div>
                `);
            });

            return HTML.join("");
        };

        const boneSelectionModalBody = createModal(`${parentName} Bone`, `${id}BoneSelection`, "boneSelectionModalContainer", selfModal);
        if (boneSelectionModalBody === null) return;

        boneSelectionModalBody.insertAdjacentHTML("beforeend", generateBonesBody());

        const hotkeyRows = Array.from(document.getElementsByClassName(`${id}HotkeyRow`));
        hotkeyRows.forEach((boneRow) => {
            boneRow.addEventListener("click", () => {
                // Get clicked bone's id
                const keyCode = (boneRow.children[1] as HTMLElement).innerText;

                // Save the new keyCode
                setFeatureSettingValue(id, keyCode);

                // Sync feature with backend
                syncFeatures([settingOwnerID]);

                // Update parameter to new bone id
                value = keyCode;

                // Update current bone visualization
                currentAimbotBoneVisualizationEl.innerText = FriendlyBones[keyCode];

                // Close bone selection modal (deletes it)
                (boneSelectionModalBody.closest(".modalContainer").children[0].children[1] as HTMLElement).click();
            });
        });
    });
}

export function addToggleButtons(modalBody: HTMLDivElement, name: string, id: string, info: string, value: string, values: string[], valueType: ("featureSetting"|"appSetting"|"espSetting"), settingOwnerID: string = null) {
    const generateButton = (name: string, toggleID: string, active: boolean, margin: boolean) => {
        return /*html*/ `
            <div class="col-auto p-0 ${margin === true ? "me-2" : ""}">
                <div class="col-auto toggleButton ${active === true ? "toggleButtonActive" : ""}" id="${id}_${toggleID}_ToggleButton">${name}</div>
            </div>
        `;
    };

    const removeSpaces = (string: string) => {
        return string.replace(/ /g, "");
    };

    const tooltip = info ? `
    <div class="col-auto ms-2 p-0" data-tippy-content="${info}">
        <i class="fa-duotone fa-circle-info"></i>
    </div>` : "";

    let activeSettingFound = false;
    let buttonsHTML: string[] = [];
    let buttonIDs: string[] = [];

    const generateButtons = () => {
        activeSettingFound = false;
        buttonsHTML = [];
        buttonIDs = [];

        values.forEach((button, i) => {
            let active = false;
            let margin = true;
    
            // Remove any spaces from ID
            const toggleID = removeSpaces(button);
    
            buttonIDs.push(button);
    
            if (toggleID === removeSpaces(value)) {
                active = true;
                activeSettingFound = true;
            }
            if (i === values.length - 1) margin = false;
    
            buttonsHTML.push(generateButton(button, toggleID, active, margin));
        });
    };

    generateButtons();

    if (!activeSettingFound) {
        const newValue = buttonIDs[0];
        value = newValue;

        if (valueType === "featureSetting") {
            setFeatureSettingValue(id, newValue);
        } else if (valueType === "espSetting") {
            setESP_SettingValue(id, newValue);
        }

        generateButtons();
    }

    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col-auto p-0">${name}</div>
                ${tooltip}
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
                ${buttonsHTML.join("")}
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    type ButtonElements = {
        [key: string]: HTMLDivElement
    };

    const buttonElements: ButtonElements = {};

    buttonIDs.forEach((toggleID) => {
        buttonElements[toggleID] = (document.getElementById(`${id}_${removeSpaces(toggleID)}_ToggleButton`) as HTMLDivElement);
    });

    const deselectAll = () => {
        for (const key in buttonElements) {
            const buttonEl = buttonElements[key];
            buttonEl.classList.remove("toggleButtonActive");
        }
    };

    for (const key in buttonElements) {
        const buttonEl = buttonElements[key];

        buttonEl.addEventListener("click", () => {
            deselectAll();
            buttonEl.classList.add("toggleButtonActive");

            if (valueType === "featureSetting") {
                // Persist feature setting
                setFeatureSettingValue(id, key);
                // Sync feature with backend
                syncFeatures([settingOwnerID]);
            } else if (valueType === "espSetting") {
                // Persist ESP setting
                setESP_SettingValue(id, key);
                // Sync ESP settings with backend
                syncESPSettings();
            } else {
                console.log(`addSlider() change event was unable to persist the slider value. No handler for valueType "${valueType}" exists!`);
                return;
            }
        });
    }
}

export function addColorPicker(modalBody: HTMLDivElement, name: string, id: string, color: string, valueType: ("featureSetting"|"appSetting"|"espSetting"), settingOwnerID: string = null) {
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
                <div class="col-auto p-0">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${id}ColorPicker"></div>
                                </div>
                                <div class="col-auto">
                                    <div>Choose Color</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    /** @type {HTMLDivElement} */
    const colorPickerElement = document.getElementById(`${id}ColorPicker`);

    const colorPicker = Pickr.create({
        default: color,
        ...getPickrBaseConfig(colorPickerElement),
    });

    const getDecimalFromPickr = () => {
        return colorPicker.getColor().toHEXA().toString();
    };

    colorPicker.on("changestop", () => {
        if (valueType === "featureSetting") {
            // Persist feature setting
            setFeatureSettingValue(id, getDecimalFromPickr());
            // Sync feature with backend
            syncFeatures([settingOwnerID]);
        } else if (valueType === "espSetting") {
            // Persist ESP setting
            setESP_SettingValue(id, getDecimalFromPickr());
            // Sync ESP settings with backend
            syncESPSettings();
        } else {
            console.log(`addSlider() change event was unable to persist the slider value. No handler for valueType "${valueType}" exists!`);
            return;
        }
    });
}

/**
 * @param {HTMLDivElement} modalBody
 * @param {string} name
 * @param {string} id
 * @param {number} value
 */
export function addDisplayPicker(modalBody: HTMLDivElement, name: string, id: string, value: number) {
    const generateDisplay = (display: Electron.Display, index: number) => {
        return /*html*/`
            <div class="col-auto me-3 p-0 espSettings_displayContainer toggleButton ${value === index ? "toggleButtonActive" : ""}" id="displaySelectionButton_${index}">
                <div class="row m-0 mb-2">
                    <div class="col p-0">
                        [${index + 1}] ${display.label}
                    </div>
                </div>
                <div class="row m-0">
                    <div class="col p-0">
                        ${display.size.width}x${display.size.height}
                    </div>
                </div>
                <div class="row m-0">
                    <div class="col p-0">
                        ${display.displayFrequency} Hz
                    </div>
                </div>
            </div>
        `;
    };

    let HTML = /*html*/`
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
    `;
    
    const displays = screen.getAllDisplays();
    let index = -1;
    displays.forEach(display => {
        index++;
        HTML += generateDisplay(display, index);
    });

    HTML += /*html*/`
            </div>
        </div>
    `;
    
    modalBody.insertAdjacentHTML("beforeend", HTML);


    // Bind events

    type ButtonElements = {
        [key: number]: HTMLDivElement
    };

    const buttonElements: ButtonElements = {};

    index = -1;
    displays.forEach(display => {
        index++;
        
        buttonElements[index] = (document.getElementById(`displaySelectionButton_${index}`) as HTMLDivElement);
    });

    const deselectAll = () => {
        for (const key in buttonElements) {
            const buttonEl = buttonElements[key];
            buttonEl.classList.remove("toggleButtonActive");
        }
    };

    for (const key in buttonElements) {
        const buttonEl = buttonElements[key];

        buttonEl.addEventListener("click", () => {
            deselectAll();
            buttonEl.classList.add("toggleButtonActive");
            // Persist ESP setting
            setESP_SettingValue(id, Number(key));
            // Sync ESP settings with backend
            syncESPSettings();
        });
    }
}

let visCheckUpdateTimeout: NodeJS.Timeout = null;
const chamsSyncTimeout = 1000;

export type VisCheckColorPickerConfig = {
    id: string;
    color: string;
};

export function addVisCheckColorPicker(modalBody: HTMLDivElement, name: string, id: string, visible: VisCheckColorPickerConfig, invisible: VisCheckColorPickerConfig, settingOwnerID: string) {
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer visCheckColorPickerInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
                <div class="col-auto p-0 visCheckColorPicker_visibleContainer visCheckColorPicker_visibleContainer_${visible.id}">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${visible.id}ColorPicker"></div>
                                </div>
                                <div class="col-auto">
                                    <div class="visCheckColorPicker_visibleText visCheckColorPicker_visibleText_${visible.id}">Visible</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-auto p-0 visCheckColorPicker_invisibleContainer visCheckColorPicker_invisibleContainer_${invisible.id}">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${invisible.id}ColorPicker"></div>
                                </div>
                                <div class="col-auto">
                                    <div class="visCheckColorPicker_invisibleText visCheckColorPicker_invisibleText_${invisible.id}">Invisible</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    /** @type {HTMLDivElement} */
    const colorPickerVisibleElement = document.getElementById(`${visible.id}ColorPicker`);

    /** @type {HTMLDivElement} */
    const colorPickerInvisibleElement = document.getElementById(`${invisible.id}ColorPicker`);

    const colorPickerVisible = Pickr.create({
        el: colorPickerVisibleElement,
        default: visible.color,
        ...getPickrBaseConfig(colorPickerVisibleElement),
    });

    const colorPickerInvisible = Pickr.create({
        el: colorPickerInvisibleElement,
        default: invisible.color,
        ...getPickrBaseConfig(colorPickerInvisibleElement),
    });

    const getDecimalFromPickr = (pickr: Pickr) => {
        return pickr.getColor().toHEXA().toString();
    };

    colorPickerVisible.on("changestop", () => {
        clearTimeout(visCheckUpdateTimeout);

        setFeatureSettingVisCheckColorValue(id, getDecimalFromPickr(colorPickerVisible), "visible");

        // Debounce this since it will perform a heavy operation on the backend
        visCheckUpdateTimeout = setTimeout(() => {
            syncFeatures([settingOwnerID]);
        }, chamsSyncTimeout);
    });

    colorPickerInvisible.on("changestop", () => {
        clearTimeout(visCheckUpdateTimeout);

        setFeatureSettingVisCheckColorValue(id, getDecimalFromPickr(colorPickerInvisible), "invisible");

        // Debounce this since it will perform a heavy operation on the backend
        visCheckUpdateTimeout = setTimeout(() => {
            syncFeatures([settingOwnerID]);
        }, chamsSyncTimeout);
    });
}

export function addVisibleInvisibleColorPicker(modalBody: HTMLDivElement, name: string, id: string, visible: string, invisible: string) {
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
                <div class="col-auto p-0">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${id}ColorPicker_visible"></div>
                                </div>
                                <div class="col-auto">
                                    <div>Visible</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-auto p-0">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${id}ColorPicker_invisible"></div>
                                </div>
                                <div class="col-auto">
                                    <div>Invisible</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    /** @type {HTMLDivElement} */
    const colorPickerVisibleElement = document.getElementById(`${id}ColorPicker_visible`);

    /** @type {HTMLDivElement} */
    const colorPickerInvisibleElement = document.getElementById(`${id}ColorPicker_invisible`);

    const colorPickerVisible = Pickr.create({
        el: colorPickerVisibleElement,
        default: visible,
        ...getPickrBaseConfig(colorPickerVisibleElement),
    });

    const colorPickerInvisible = Pickr.create({
        el: colorPickerInvisibleElement,
        default: invisible,
        ...getPickrBaseConfig(colorPickerInvisibleElement),
    });

    const getDecimalFromPickr = (pickr: Pickr) => {
        return pickr.getColor().toHEXA().toString();
    };

    colorPickerVisible.on("changestop", () => {
        // Persist ESP setting
        setESP_SettingValue(id, getDecimalFromPickr(colorPickerVisible), "visibleColor");
        // Sync ESP settings with backend
        syncESPSettings();
    });

    colorPickerInvisible.on("changestop", () => {
        // Persist ESP setting
        setESP_SettingValue(id, getDecimalFromPickr(colorPickerInvisible), "invisibleColor");
        // Sync ESP settings with backend
        syncESPSettings();
    });
}

export function addPlayerColorPicker(modalBody: HTMLDivElement, name: string, id: string, Dot: string, Border: string) {
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0">
                <div class="col p-0">${name}</div>
            </div>
            <div class="row m-0 p-0 justify-content-center align-items-center">
                <div class="col-auto p-0">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${id}ColorPicker_Dot"></div>
                                </div>
                                <div class="col-auto">
                                    <div>Dot</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-auto p-0">
                    <div class="row m-0 justify-content-center align-items-center">
                        <div class="col p-0">
                            <div class="row m-0">
                                <div class="col-auto p-0 modalColorPicker">
                                    <div id="${id}ColorPicker_Border"></div>
                                </div>
                                <div class="col-auto">
                                    <div>Border</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    /** @type {HTMLDivElement} */
    const colorPickerDotElement = document.getElementById(`${id}ColorPicker_Dot`);

    /** @type {HTMLDivElement} */
    const colorPickerBorderElement = document.getElementById(`${id}ColorPicker_Border`);

    const colorPickerDot = Pickr.create({
        el: colorPickerDotElement,
        default: Dot,
        ...getPickrBaseConfig(colorPickerDotElement),
    });

    const colorPickerBorder = Pickr.create({
        el: colorPickerBorderElement,
        default: Border,
        ...getPickrBaseConfig(colorPickerBorderElement),
    });

    const getDecimalFromPickr = (pickr: Pickr) => {
        return pickr.getColor().toHEXA().toString();
    };

    colorPickerDot.on("changestop", () => {
        SetAppSettingPlayerColor(id, true, getDecimalFromPickr(colorPickerDot));
    });

    colorPickerBorder.on("changestop", () => {
        SetAppSettingPlayerColor(id, false, getDecimalFromPickr(colorPickerBorder));
    });
}

type HitboxSetting = {
    chance: number;
    smartTargeting: boolean;
    availableBones: string[];
    selectedBone: string;
    side: string;
};

type HitboxSettings = {
    [key: string]: HitboxSetting
};

export function addHitboxConfigurator(selfModal: HTMLDivElement, modalBody: HTMLDivElement, name: string, parentName: string, id: string, value: string, playerType: ("Player"|"AI"), settingOwnerID: string) {
    const hitboxSettings = (JSON.parse(value) as HitboxSettings);
    
    const HTML = /*html*/ `
        <div class="row m-0 modalSettingInnerContainer" id="${id}_modalSettingInnerContainer">
            <div class="row m-0 mb-2 p-0 justify-content-between align-items-center">
                <div class="col p-0">${name}</div>

                <div class="col-auto ms-2 p-0 changeStyleButton" id="${id}ChangeConfig">
                    <i class="fa-duotone fa-arrows-rotate me-2"></i>Configure
                </div>
            </div>
        </div>
    `;

    modalBody.insertAdjacentHTML("beforeend", HTML);

    // Bind events

    const removeSpaces = (string: string) => {
        return string.replace(/ /g, "");
    };

    const generateButton = (name: string, toggleID: string, active: boolean, margin: boolean) => {
        return /*html*/ `
            <div class="col-auto p-0 ${margin === true ? "me-2" : ""}">
                <div class="col-auto toggleButton ${active === true ? "toggleButtonActive" : ""}" id="${toggleID}">${name}</div>
            </div>
        `;
    };

    const changeStyleEl = document.getElementById(`${id}ChangeConfig`);
    changeStyleEl.addEventListener("click", () => {
        const generateHitboxSettings = (hitboxName: string, settingID: string) => {
            const hitChanceData = hitboxSettings[settingID];

            const buttonHTML: string[] = [];
            if (hitChanceData.availableBones.length > 1) {
                hitChanceData.availableBones.forEach(bone => {
                    let selected = false;
                    if (hitChanceData.selectedBone === bone)
                        selected = true;

                    buttonHTML.push(generateButton(bone, `${id}_${settingID}_${removeSpaces(bone)}_bone`, selected, true));
                });
            }

            return /*html*/`
                <div class="row m-0 modalSettingContainer" id="${id}_hitboxConfiguration_${settingID}">
                    <div class="row m-0 mb-2 p-0 modalSettingSection">
                        <div class="col-auto p-0">${hitboxName}</div>
                    </div>

                    <div class="row m-0 modalSettingInnerContainer">
                        <div class="row m-0 mb-2 p-0">
                            <div class="col-auto p-0">Hit Chance</div>
                            <div class="col-auto ms-2 p-0" data-tippy-content="How likely it is that this hitbox will be chosen.">
                                <i class="fa-duotone fa-circle-info"></i>
                            </div>
                        </div>
                        <div class="row m-0 p-0 justify-content-center align-items-center">
                            <div class="col p-0">
                                <input type="range" min="0" max="100" step="0.01" value="${hitChanceData.chance}" id="${id}_${settingID}">
                            </div>
                            <div class="col-auto ms-2 p-0 rangeValue" id="${id}_${settingID}RangeValue">${hitChanceData.chance.toFixed(0).padStart(3, "0")}%</div>
                        </div>

                        ${playerType === "Player" ? /*html*/`
                            <div class="row m-0 mt-2 p-0 align-items-center">
                                <div class="col-auto p-0">
                                    <div class="checkbox" id="${id}_smartTargeting_${settingID}">
                                        ${hitChanceData.smartTargeting ? `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>` : ""}
                                    </div>
                                </div>
                                <div class="col-auto pe-0">Smart Targeting</div>
                                <div class="col-auto ms-2 p-0" data-tippy-content="When enabled, this hitbox will only be targeted if it is visible.">
                                    <i class="fa-duotone fa-circle-info"></i>
                                </div>
                            </div>
                        ` : ""}

                        ${buttonHTML.length > 0 ? /*html*/`
                            <div class="row m-0 mt-2 p-0">
                                <div class="row m-0 mb-2 p-0">
                                    <div class="col-auto p-0">Hitbox Bone</div>
                                    <div class="col-auto ms-2 p-0" data-tippy-content="The specific bone this hitbox targets.">
                                        <i class="fa-duotone fa-circle-info"></i>
                                    </div>
                                </div>
                                <div class="row m-0 p-0 justify-content-center align-items-center">
                                    ${buttonHTML.join("")}
                                </div>
                            </div>
                        ` : ""}
                    </div>
                </div>
            `;
        };

        const persistValue = (settingID: string, newValue: number, setValue: boolean = false) => {
            const slider = (document.getElementById(`${id}_${settingID}`) as HTMLInputElement);
            const sliderReadout = document.getElementById(`${id}_${settingID}RangeValue`);

            const valueString = newValue.toFixed(0);

            hitboxSettings[settingID].chance = newValue;

            sliderReadout.innerText = `${valueString.padStart(3, "0")}%`;
            
            if (setValue) slider.value = valueString;
        };

        const attachHitboxSettings = (settingID: string) => {
            attachHitChance(settingID);
            attachCheckbox(settingID);
            attachToggleButtons(settingID);
        };

        const attachHitChance = (settingID: string) => {
            const slider = (document.getElementById(`${id}_${settingID}`) as HTMLInputElement);

            slider.addEventListener("input", () => {
                const newValue = Number(slider.value);

                persistValue(settingID, newValue);
                updateSliders(settingID);
            });
            slider.addEventListener("change", () => {
                const asString = JSON.stringify(hitboxSettings);

                setFeatureSettingValue(id, asString);
                syncFeatures([settingOwnerID]);
            });
        };
        
        const attachCheckbox = (settingID: string) => {
            if (playerType !== "Player")
                return;

            const checkbox = document.getElementById(`${id}_smartTargeting_${settingID}`);

            const onEnabled = () => {
                hitboxSettings[settingID].smartTargeting = true;
            };

            const onDisabled = () => {
                hitboxSettings[settingID].smartTargeting = false;
            };

            checkbox.addEventListener("click", () => {
                if (checkbox.innerHTML.trim() === "") {
                    // Enable
                    checkbox.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
        
                    onEnabled();
                } else {
                    // Disable
                    checkbox.innerHTML = "";
        
                    onDisabled();
                }

                const asString = JSON.stringify(hitboxSettings);

                setFeatureSettingValue(id, asString);
                syncFeatures([settingOwnerID]);
            });
        };

        const attachToggleButtons = (settingID: string) => {
            type ButtonElements = {
                [key: string]: HTMLDivElement
            };
        
            const buttonElements: ButtonElements = {};

            const settings = hitboxSettings[settingID];

            if (settings.availableBones.length <= 1)
                return;
        
            settings.availableBones.forEach((bone) => {
                buttonElements[bone] = (document.getElementById(`${id}_${settingID}_${removeSpaces(bone)}_bone`) as HTMLDivElement);
            });
        
            const deselectAll = () => {
                for (const key in buttonElements) {
                    const buttonEl = buttonElements[key];
                    buttonEl.classList.remove("toggleButtonActive");
                }
            };
        
            for (const key in buttonElements) {
                const buttonEl = buttonElements[key];
        
                buttonEl.addEventListener("click", () => {
                    deselectAll();
                    buttonEl.classList.add("toggleButtonActive");
        
                    hitboxSettings[settingID].selectedBone = key;

                    const asString = JSON.stringify(hitboxSettings);

                    setFeatureSettingValue(id, asString);
                    syncFeatures([settingOwnerID]);
                });
            }
        };

        const updateSliders = (changedID: string) => {
            const total = 100;
            const changedValue = parseFloat((document.getElementById(`${id}_${changedID}`) as HTMLInputElement).value);
            const remainingSliders = Object.keys(hitboxSettings).filter(key => key !== changedID);
            
            const remainingTotal = total - changedValue;

            const otherTotal = remainingSliders.reduce((sum, key) => sum + hitboxSettings[key].chance, 0);
            let distributedTotal = 0;
            for (let i = 0; i < remainingSliders.length; i++) {
                const key = remainingSliders[i];
                
                let newValue = 0;
                if (!isNaN(otherTotal) && otherTotal !== 0) {
                    newValue = (hitboxSettings[key].chance / otherTotal) * remainingTotal;
                } else {
                    const remainder = remainingTotal / remainingSliders.length;
                    newValue = remainder;
                }

                distributedTotal += newValue;

                persistValue(key, newValue, true);
            }
        };
        
        const configurationModalBody = createModal(`${playerType} Hitboxes`, `${id}Configuration`, "configureHitboxesModalContainer", selfModal);
        if (configurationModalBody === null) return;

        const html: string[] = [];

        html.push(generateHitboxSettings("Head", "headHitboxChance"));
        html.push(generateHitboxSettings("Thorax", "thoraxHitboxChance"));
        html.push(generateHitboxSettings("Stomach", "stomachHitboxChance"));
        html.push(generateHitboxSettings("Left Arm", "leftArmHitboxChance"));
        html.push(generateHitboxSettings("Right Arm", "rightArmHitboxChance"));
        html.push(generateHitboxSettings("Left Leg", "leftLegHitboxChance"));
        html.push(generateHitboxSettings("Right Leg", "rightLegHitboxChance"));

        configurationModalBody.insertAdjacentHTML("beforeend", html.join(""));

        attachHitboxSettings("headHitboxChance");
        attachHitboxSettings("thoraxHitboxChance");
        attachHitboxSettings("stomachHitboxChance");
        attachHitboxSettings("leftArmHitboxChance");
        attachHitboxSettings("rightArmHitboxChance");
        attachHitboxSettings("leftLegHitboxChance");
        attachHitboxSettings("rightLegHitboxChance");

        initializeTooltips();
    });
}
