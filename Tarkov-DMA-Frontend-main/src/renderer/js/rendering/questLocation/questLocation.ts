import { Container, DisplayObject } from "pixi.js";
import { PIXI_VIEWPORT, VIEWPORT_SCALES } from "..";
import { LootText } from "../loot/text";
import { Marker } from "./marker";
import { LootPosition } from "./questManager";

export class QuestLocation {
    dynamicData: {
        CurrentQuestLocationVersion: number;
    };
    questLocation: Container<DisplayObject>;
    questLocationMarker: Marker;
    lootText: LootText;

    constructor(Name: string, Position: LootPosition, CurrentQuestLocationVersion: number) {
        this.dynamicData = {
            CurrentQuestLocationVersion: CurrentQuestLocationVersion,
        };

        this.questLocation = new Container();
        this.questLocation.position.x = Position[0];
        this.questLocation.position.y = Position[1];
        this.questLocation.zIndex = 30;

        this.questLocationMarker = new Marker();
        this.questLocation.addChild(this.questLocationMarker);

        this.lootText = new LootText(Name, "#FFFFFF", "#000000");
        this.questLocation.addChild(this.lootText);

        // Set scale to current scale
        this.setScale(VIEWPORT_SCALES.Loot[0], VIEWPORT_SCALES.Loot[1]);

        // Add loot to canvas
        PIXI_VIEWPORT.addChild(this.questLocation);
    }

    updateData(CurrentQuestLocationVersion: number) {
        this.dynamicData.CurrentQuestLocationVersion = CurrentQuestLocationVersion;
    }

    setScale(x: number, y: number) {
        this.questLocation.scale.x = x;
        this.questLocation.scale.y = y;
    }

    destroy() {
        this.questLocation.destroy(true);
    }
}
