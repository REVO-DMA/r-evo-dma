import { QuestLocation } from "./questLocation";

export type LootPosition = [X: number, Y: number, Height: number];

type LootItem = [ID: number, Shown: boolean, BSG_ID: string, Type: number, Name: string, Position: LootPosition];

export type QuestLocations = {
    [key: string]: QuestLocation
};

export let questLocations: QuestLocations = {};

// This represents the current location refresh iteration. It is used to determine if an location should be removed from the locations object.
let currentQuestLocationVersion = 0;

export function onRaidEnd() {
    // Destroy all quest locations
    for (const key in questLocations) {
        const questLocation = questLocations[key];

        if (questLocation == null) continue;

        questLocation.destroy();
    }

    questLocations = {};
    currentQuestLocationVersion = 0;
}

export function update(newQuestLocations: LootItem[]) {
    // Bump refresh iteration
    currentQuestLocationVersion++;

    for (let i = 0; i < newQuestLocations.length; i++) {
        const newQuestLocation = newQuestLocations[i];
        const key = newQuestLocation[2];

        if (questLocations[key] == null) {
            questLocations[key] = new QuestLocation(newQuestLocation[4], newQuestLocation[5], currentQuestLocationVersion);
        } else {
            questLocations[key].updateData(currentQuestLocationVersion);
        }
    }

    // Remove stale locations
    for (const key in questLocations) {
        const questLocation = questLocations[key];

        if (questLocation == null) continue;

        if (questLocation.dynamicData.CurrentQuestLocationVersion != currentQuestLocationVersion) {
            console.log("[QUEST] Destroyed:", questLocation.lootText.text);
            questLocation.destroy();
            delete questLocations[key];
        }
    }
}

/**
 * @param {number} x
 * @param {number} y
 */
export function scaleQuestLocations(x: number, y: number) {
    for (const key in questLocations) {
        const questLocation = questLocations[key];

        if (questLocation == null) continue;

        questLocation.setScale(x, y);
    }
}
