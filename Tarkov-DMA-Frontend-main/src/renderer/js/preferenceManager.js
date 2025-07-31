import { espSettingsDefaults } from "./esp/espManifest";
import { featureSectionStateDefaults, featureSettingsDefaults, featureStateDefaults, featureToggleHotkeysDefaults } from "./features/featuresManifest";
import { appSettingsDefaults } from "./settings/settingsManifest";

const preferences = [
    {
        id: "feature",
        defaults: featureStateDefaults,
    },
    {
        id: "featureToggleHotkeys",
        defaults: featureToggleHotkeysDefaults,
    },
    {
        id: "featureSettings",
        defaults: featureSettingsDefaults,
    },
    {
        id: "featureSection",
        defaults: featureSectionStateDefaults,
    },
    {
        id: "appSettings",
        defaults: appSettingsDefaults,
    },
    {
        id: "espSettings",
        defaults: espSettingsDefaults,
    },
];

const alwaysUpdateKeys = [
    "name",
    "uiType",
    "dataType",
    "info",
    "min",
    "max",
    "step",
    "valueSuffix",
    "values",
    "sectionID",
    "remote",
    "playerType",
    "availableBones",
    "side",
    "playerID",
];

const cachedPreferences = [];

export function initialize() {
    let preferencesIndex = 0;
    preferences.forEach((preference) => {
        const localStorageName = `${preference.id}_preferences`;
        let usedData = localStorage.getItem(localStorageName);
        if (usedData == null) {
            usedData = preference.defaults;
        } else {
            usedData = syncPreferences(JSON.parse(usedData), preference.defaults);
        }

        localStorage.setItem(localStorageName, JSON.stringify(usedData));
        cachedPreferences[preferencesIndex] = usedData;

        preferencesIndex++;
    });
}

/**
 * Merges properties from `newPreferences` that are not already present in `oldPreferences`. Also removes orphaned preferences.
 * @param {Object[]} oldPreferences The old preferences array.
 * @param {Object[]} newPreferences The new preferences array.
 * @returns {Object[]} The merged preferences array.
 */
function syncPreferences(oldPreferences, newPreferences) {
    const syncedPreferences = [];

    // Loop through each element in oldPreferences
    for (let i = 0; i < oldPreferences.length; i++) {
        const oldPref = oldPreferences[i];
        const newPref = newPreferences.find((pref) => pref.id === oldPref.id);

        if (newPref) {
            // Merge the new preference's properties that are not already present in oldPref
            const mergedPref = { ...oldPref };

            for (const [key, value] of Object.entries(newPref)) {
                const alwaysUpdate = isAlwaysUpdateKey(key);

                if (!(key in oldPref) || alwaysUpdate.alwaysUpdate) {
                    // Reset the value to default
                    if (alwaysUpdate.shouldUpdateValueOnChange && mergedPref[key] !== value) {
                        mergedPref["value"] = newPref["value"];
                    }

                    mergedPref[key] = value;
                }
            }

            // Delete any preferences from oldPref that are not in newPref
            for (const [key, value] of Object.entries(oldPref)) {
                if (newPref[key] == undefined) {
                    delete mergedPref[key];
                }
            }

            syncedPreferences.push(mergedPref);
        }
    }

    // Add any preferences from newPreferences that are not in oldPreferences
    for (let i = 0; i < newPreferences.length; i++) {
        const newPref = newPreferences[i];
        const oldPref = oldPreferences.find((pref) => pref.id === newPref.id);
        if (!oldPref) {
            syncedPreferences.splice(i, 0, newPref);
        }
    }

    // Return the merged and sorted preferences
    return syncedPreferences.sort((a, b) => {
        const indexA = newPreferences.findIndex((item) => item.id === a.id);
        const indexB = newPreferences.findIndex((item) => item.id === b.id);
        return indexA - indexB;
    });
}

/**
 * @param {string} key
 */
function isAlwaysUpdateKey(key) {
    let isAlwaysUpdate = false;
    let shouldUpdateValue = false;
    for (let i = 0; i < alwaysUpdateKeys.length; i++) {
        const alwaysUpdateKey = alwaysUpdateKeys[i];

        if (key === alwaysUpdateKey) {
            isAlwaysUpdate = true;

            if (key === "uiType" || key === "dataType") shouldUpdateValue = true;

            break;
        }
    }

    return { alwaysUpdate: isAlwaysUpdate, shouldUpdateValueOnChange: shouldUpdateValue };
}

export function savePreferences(id, newPreferences) {
    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (id === preference.id) {
            const localStorageName = `${preference.id}_preferences`;

            localStorage.setItem(localStorageName, JSON.stringify(newPreferences));
            cachedPreferences[i] = newPreferences;

            break;
        }
    }
}

export function getPreferences(id) {
    let output = null;
    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (id === preference.id) {
            output = cachedPreferences[i];

            break;
        }
    }

    return output;
}
