import { Container, Sprite, Texture } from "pixi.js";
import { PIXI_VIEWPORT, VIEWPORT_SCALES } from "..";
import exfilSVG from "../../../img/icons/exfil.svg";
import transitSVG from "../../../img/icons/transit.svg";
import { Background } from "./background";
import { ExfilText } from "./text";
import { IPC_Exfil } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_Exfil";
import { IPC_Vector2 } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_Vector2";
import { ExfilStatus } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/ExfilStatus";

export class Exfil {
    staticData: {
        ID: number;
        Name: string;
        Description: string;
        Position: IPC_Vector2;
    };

    Container: Container<import("pixi.js").DisplayObject>;
    exfilIcon: Sprite;
    background: Background;
    text: ExfilText;

    finishAllocation: () => Promise<void>;

    constructor(staticExfil: IPC_Exfil) {
        console.log(`Allocating Exfil: ID: ${staticExfil.iD} | Name: ${staticExfil.name} | Status: ${staticExfil.status}`);

        this.staticData = {
            ID: staticExfil.iD,
            Name: staticExfil.name,
            Description: staticExfil.description,
            Position: staticExfil.position,
        };

        this.Container = new Container();
        this.Container.cullable = true;
        this.Container.position.x = staticExfil.position.x;
        this.Container.position.y = staticExfil.position.y;
        this.Container.zIndex = 20;

        this.finishAllocation = async () => {
            this.background = new Background(staticExfil.status);
            this.Container.addChild(this.background);

            if (staticExfil.status === ExfilStatus.Transit)
                this.text = new ExfilText(this.staticData.Description);
            else
                this.text = new ExfilText(this.staticData.Name);

            this.Container.addChild(this.text);

            let image: string;
            if (staticExfil.status === ExfilStatus.Transit) {
                image = transitSVG;
            } else {
                image = exfilSVG;
            }

            let rawSVG = await Texture.fromURL(image);
            /** @type {Sprite} */
            this.exfilIcon = new Sprite(rawSVG);

            // Image is pretty big, scale it down
            this.exfilIcon.width = 32;
            this.exfilIcon.height = 32;

            // Center the sprite
            this.exfilIcon.x = -(this.exfilIcon.getBounds().width / 2);
            this.exfilIcon.y = -(this.exfilIcon.getBounds().height / 2);

            this.Container.addChild(this.exfilIcon);

            // Set scale to current scale
            this.setScale(VIEWPORT_SCALES.Exfils[0], VIEWPORT_SCALES.Exfils[1]);

            this._setDirty();

            PIXI_VIEWPORT.addChild(this.Container);
        };

        this.finishAllocation();
    }

    updateStatus(newStatus: number) {
        if (this.background.updateStatus(newStatus)) this._setDirty();
    }

    setScale(x: number, y: number) {
        this.Container.scale.x = x;
        this.Container.scale.y = y;
    }

    /** Regenerates the cached texture of this loot item. */
    _setDirty() {
        this.Container.cacheAsBitmap = false;
        this.Container.cacheAsBitmap = true;
    }

    destroy() {
        this.Container.destroy(true);
    }
}
