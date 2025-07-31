import { getPreferences, savePreferences } from "../preferenceManager";
import { getFeatureSectionState } from "./featureSection";
import { veryRiskyFeatures } from "./featuresManifest";

/**
 * Generate the HTML for a given Feature Root.
 * @param {string} currentSection
 * @param {string} id
 * @param {string} name
 * @param {any} icons
 * @param {boolean} available
 */
export function generateFeatureRoot(currentSection, id, name, icons, available) {
    const iconsHTML = [];

    icons.forEach((icon) => {
        const type = icon.type;
        const template = icon.template ? `data-tippy-content="${icon.template}"` : "";

        if (type === "info") {
            iconsHTML.push(/*html*/ `
                <div class="col-auto ms-2 p-0" ${template}>
                    <i class="fa-duotone fa-circle-info"></i>
                </div>
            `);
        } else if (type === "warning") {
            iconsHTML.push(/*html*/ `
                <div class="col-auto ms-2 p-0" ${template}>
                    <i class="fa-duotone fa-triangle-exclamation"></i> 
                </div>
            `);
        } else if (type === "settings") {
            iconsHTML.push(/*html*/ `
                <div class="col-auto ms-2 p-0 clickableIcon" id="${id}_settings" ${template}>
                    <i class="fa-duotone fa-wrench buttonIcon"></i>
                </div>
            `);
        }
    });

    const sectionState = getFeatureSectionState(currentSection);

    return /*html*/ `
        <div class="row m-0 mb-2 p-0 align-items-center childOf_${currentSection} ${veryRiskyFeatures.includes(id) ? "veryRiskyFeature" : ""} ${
        !available ? "unavailableFeature" : ""
    }" style="display: ${sectionState === "Expanded" ? "" : "none"}">
            <div class="col-auto p-0">
                <div class="checkbox" id="${id}_checkbox">
                    ${isFeatureEnabled(id) ? `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>` : ""}
                </div>
            </div>
            <div class="col pe-0">
                <div class="row m-0">
                    <div class="col-auto p-0">${name}</div>
                    ${iconsHTML.join("")}
                </div>
            </div>
        </div>
    `;
}

/**
 * Generate the HTML for a given Feature Root.
 * @param {string} currentSection
 * @param {string} id
 * @param {string} name
 * @param {boolean} available
 */
export function generateFeatureRootNoIcons(currentSection, id, name, available) {
    const sectionState = getFeatureSectionState(currentSection);

    return /*html*/ `
        <div class="row m-0 mb-2 p-0 align-items-center childOf_${currentSection} ${!available ? "unavailableFeature" : ""}" style="display: ${
        sectionState === "Expanded" ? "" : "none"
    }">
            <div class="col-auto p-0">
                <div class="checkbox" id="${id}_checkbox">
                    ${isFeatureEnabled(id) ? `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>` : ""}
                </div>
            </div>
            <div class="col pe-0">${name}</div>
        </div>
    `;
}

/**
 * @param {string} id
 * @param {boolean} enabled
 */
export function SetFeatureStatus(id, enabled) {
    const preferences = getPreferences("feature");

    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.id === id) {
            preference.enabled = enabled;

            break;
        }
    }

    savePreferences("feature", preferences);
}

/**
 * @param {string} featureID
 */
export function getFeatureSettings(featureID) {
    const preferences = getPreferences("featureSettings");

    let output = [];
    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.featureID === featureID) {
            output.push(preference);
        }
    }

    return output;
}

/**
 * @param {string} id
 * @param {any} newValue
 */
export function setFeatureSettingValue(id, newValue) {
    const preferences = getPreferences("featureSettings");

    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.id === id) {
            preference.value = newValue;

            break;
        }
    }

    savePreferences("featureSettings", preferences);
}

/**
 * @param {string} id
 * @param {any} newValue
 * @param {("visible"|"invisible")} type
 */
export function setFeatureSettingVisCheckColorValue(id, newValue, type) {
    const preferences = getPreferences("featureSettings");

    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.id === id) {
            if (type === "visible") preference.color_visible = newValue;
            else if (type === "invisible") preference.color_invisible = newValue;

            break;
        }
    }

    savePreferences("featureSettings", preferences);
}

/**
 * @param {string} id
 */
export function isFeatureEnabled(id) {
    const preferences = getPreferences("feature");

    let output = null;
    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.id === id) {
            output = preference.enabled;

            break;
        }
    }

    return output;
}
