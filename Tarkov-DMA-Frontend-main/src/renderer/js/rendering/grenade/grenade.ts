import { Container, DisplayObject } from "pixi.js";
import { PIXI_VIEWPORT, VIEWPORT_SCALES } from "..";
import { Dot } from "./dot";
import { destroy as destroyGrenade } from "./grenadeManager";
import { GrenadeText } from "./text";
import { IPC_StaticGrenade } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_StaticGrenade";

const SELF_DESTRUCT_TIMEOUT = 12000;

export class Grenade {
    staticData: {
        UniqueID: number;
        Name: string;
        SelfDestructTimeout: NodeJS.Timeout;
    };
    dynamicData: {
        DistanceNumber: number;
        Distance: string;
    };
    grenade: Container<DisplayObject>;
    grenadeDot: Dot;
    grenadeText: GrenadeText;

    constructor(staticGrenade: IPC_StaticGrenade) {
        console.log(`Allocating Grenade: UniqueID: ${staticGrenade.iD} | Name: ${staticGrenade.name}`);

        // This grenade data will never change
        this.staticData = {
            UniqueID: staticGrenade.iD,
            Name: staticGrenade.name,
            SelfDestructTimeout: null,
        };

        this.dynamicData = {
            DistanceNumber: null,
            Distance: null,
        };

        /** @type {Container<import("pixi.js").DisplayObject>} */
        this.grenade = new Container();
        this.grenade.position.x = 0;
        this.grenade.position.y = 0;
        this.grenade.zIndex = 90;

        /** @type {Dot} */
        this.grenadeDot = new Dot();
        this.grenade.addChild(this.grenadeDot);

        let grenadeText = "";
        if (this.staticData.Name !== null) grenadeText = this.staticData.Name;
        /** @type {GrenadeText} */
        this.grenadeText = new GrenadeText(grenadeText);
        this.grenade.addChild(this.grenadeText);

        // Set scale to current scale
        this.setScale(VIEWPORT_SCALES.Grenades[0], VIEWPORT_SCALES.Grenades[1]);

        // Add grenade to canvas
        PIXI_VIEWPORT.addChild(this.grenade);

        // Self destruct if not destroyed upon explosion
        this.staticData.SelfDestructTimeout = setTimeout(() => {
            destroyGrenade(this.staticData.UniqueID);
        }, SELF_DESTRUCT_TIMEOUT);
    }

    setScale(x: number, y: number) {
        this.grenade.scale.x = x;
        this.grenade.scale.y = y;
    }

    setPosition(x: number, y: number) {
        this.grenade.position.x = x;
        this.grenade.position.y = y;
    }

    setDistance(distance: number) {
        // Only update when Distance actually changes
        if (distance !== this.dynamicData.DistanceNumber) {
            this.dynamicData.Distance = String(distance) + "m";
            this.dynamicData.DistanceNumber = distance;
            this.updateText();
        }
    }

    updateText() {
        let grenadeText = "";
        if (this.staticData.Name !== null) {
            grenadeText = `${this.staticData.Name} `;
        }

        if (this.dynamicData.Distance !== null) {
            grenadeText += this.dynamicData.Distance;
        }

        this.grenadeText.update(grenadeText);
    }

    destroy() {
        console.log(`Destroying Grenade: UniqueID: ${this.staticData.UniqueID} | Name: ${this.staticData.Name}`);

        clearTimeout(this.staticData.SelfDestructTimeout);

        // Remove grenade from canvas
        this.grenade.destroy(true);
    }
}
