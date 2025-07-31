import { syncFeatures } from "../ipc/manager";
import { addBones, addCheckbox, addColorPicker, addHitboxConfigurator, addHotkey, addSlider, addToggleButtons, addVisCheckColorPicker, createModal } from "../modalMaster";
import { preferences } from "../modals";
import { generateFeatureSection, setFeatureSectionState } from "./featureSection";
import { featuresManifest } from "./featuresManifest";
import { generateFeatureRoot, generateFeatureRootNoIcons, getFeatureSettings, SetFeatureStatus } from "./individualFeature";
import { initialize as initializeTooltips } from "../tooltips";
import { PUBLIC_BUILD } from "../../env";

const featuresModalEl = document.getElementById("featuresModal");
const featuresModalContainerContentEl = document.getElementById("featuresModalContainerContent");

export async function initialize() {
    const backendSyncIDs = generateFeaturesHTML();
    await bindFeatureEvents();
    syncFeatures(backendSyncIDs);
}

function generateFeaturesHTML() {
    const HTML = [];
    let currentSection = null;

    /** @type {string[]} */
    const backendSyncIDs = [];

    for (let i = 0; i < featuresManifest.length; i++) {
        const item = featuresManifest[i];
        
        const type = item.type;
        const id = item.id;
        const name = item.name;
        const available = item.available;

        if (!item.privateOnly != null && item.privateOnly === true && PUBLIC_BUILD)
            continue;

        if (type === "featureSection") {
            currentSection = id;
            HTML.push(generateFeatureSection(id, name, item.iconClasses));
        } else if (type === "featureRoot") {
            if (item.icons != null && item.icons.length > 0) {
                const icons = item.icons;

                HTML.push(generateFeatureRoot(currentSection, id, name, icons, available));
            } else {
                HTML.push(generateFeatureRootNoIcons(currentSection, id, name, available));
            }

            backendSyncIDs.push(id);
        }
    }

    featuresModalContainerContentEl.innerHTML = HTML.join("");

    return backendSyncIDs;
}

async function bindFeatureEvents() {
    for (let i = 0; i < featuresManifest.length; i++) {
        const item = featuresManifest[i];
        
        const type = item.type;
        const ID = item.id;

        if (!item.privateOnly != null && item.privateOnly === true && PUBLIC_BUILD)
            continue;

        if (type === "featureSection") {
            document.getElementById(`${ID}_section`).addEventListener("click", () => {
                setFeatureSectionState(ID);
            });
        } else if (type === "featureRoot") {
            // Bind checkbox event
            const checkboxContainer = document.getElementById(`${ID}_checkbox`);
            checkboxContainer.addEventListener("click", () => {
                if (checkboxContainer.innerHTML.trim() === "") {
                    // Enable
                    checkboxContainer.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
                    SetFeatureStatus(ID, true);
                } else {
                    // Disable
                    checkboxContainer.innerHTML = "";
                    SetFeatureStatus(ID, false);
                }

                syncFeatures([ID]);
            });

            // Bind icon event
            if (item.icons != null && item.icons.length > 0) {
                const icons = item.icons;

                icons.forEach((icon) => {
                    if (icon.type === "settings") {
                        document.getElementById(`${ID}_settings`).addEventListener("click", () => {
                            const settings = getFeatureSettings(ID);

                            if (settings.length === 0) {
                                console.log(`Error getting feature settings for "${ID}". "getFeatureSettings" returned a zero-length array.`);
                                return;
                            }

                            let className = "featureSettingModalContainer";
                            if (ID === "aimbot")
                                className = "aimbotSettingsModalContainer";
                            else if (ID === "chams")
                                className = "chamsSettingsModalContainer";

                            const modalBody = createModal(
                                `${item.name} ${settings.length > 1 ? "Settings" : "Setting"}`,
                                `${item.id}Settings`,
                                className,
                                featuresModalEl
                            );

                            if (modalBody === null) return;

                            settings.forEach((setting) => {
                                if (setting.uiType === "slider") {
                                    addSlider(
                                        modalBody,
                                        setting.name,
                                        setting.info,
                                        setting.id,
                                        setting.value,
                                        setting.min,
                                        setting.max,
                                        setting.step,
                                        "featureSetting",
                                        setting.valueSuffix,
                                        ID
                                    );
                                } else if (setting.uiType === "checkbox") {
                                    addCheckbox(modalBody, setting.name, setting.info, setting.id, setting.value, "featureSetting", ID);
                                } else if (setting.uiType === "hotkey") {
                                    const selfModal = document.getElementById(`${item.id}Settings`);
                                    addHotkey(selfModal, modalBody, setting.name, item.name, setting.id, setting.value, ID, "featureSetting");
                                } else if (setting.uiType === "dropdown") {
                                    const selfModal = document.getElementById(`${item.id}Settings`);
                                    addBones(selfModal, modalBody, setting.name, item.name, setting.id, setting.value, ID);
                                } else if (setting.uiType === "toggleButtons") {
                                    addToggleButtons(modalBody, setting.name, setting.id, setting.info, setting.value, setting.values, "featureSetting", ID);
                                } else if (setting.uiType === "colorPicker") {
                                    addColorPicker(modalBody, setting.name, setting.id, setting.value, "featureSetting", ID);
                                } else if (setting.uiType === "visCheckColorPicker") {
                                    /** @type {import("../modalMaster").VisCheckColorPickerConfig} */
                                    var visible = {
                                        id: setting.id_visible,
                                        color: setting.color_visible,
                                    };

                                    /** @type {import("../modalMaster").VisCheckColorPickerConfig} */
                                    var invisible = {
                                        id: setting.id_invisible,
                                        color: setting.color_invisible,
                                    };

                                    addVisCheckColorPicker(modalBody, setting.name, setting.id, visible, invisible, ID);
                                } else if (setting.uiType === "hitboxConfigurator") {
                                    const selfModal = document.getElementById(`${item.id}Settings`);
                                    addHitboxConfigurator(selfModal, modalBody, setting.name, item.name, setting.id, setting.value, setting.playerType, ID);
                                }
                            });

                            initializeTooltips();
                        });

                        if (preferences[`${item.id}Settings`] != null && preferences[`${item.id}Settings`].show) {
                            document.getElementById(`${ID}_settings`).click();
                        }
                    }
                });
            }
        }
    }
}
