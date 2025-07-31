import { Container, Resource, Sprite, Texture } from "pixi.js";
import { PIXI_VIEWPORT, VIEWPORT_SCALES } from "..";
import rocketSVG from "../../../img/icons/rocket.svg";
import { Background } from "./background";
import { Mortar as IPC_Mortar } from "../../../../gen/mortar_pb";
import { Vector2 } from "../../../../gen/vector_pb";

let RocketSVG: Texture<Resource>;

(async () => {
    RocketSVG = await Texture.fromURL(rocketSVG);
})();

export class Mortar {
    staticData: {
        ID: number;
        Position: Vector2;
    };

    mortar: Container<import("pixi.js").DisplayObject>;
    mortarIcon: Sprite;
    background: Background;

    constructor(mortar: IPC_Mortar) {
        console.log(`Allocating Mortar: ${mortar.id}`);

        this.staticData = {
            ID: mortar.id,
            Position: mortar.position,
        };

        this.mortar = new Container();
        this.mortar.position.x = mortar.position.x;
        this.mortar.position.y = mortar.position.y;
        this.mortar.zIndex = 90;

        this.background = new Background();
        this.mortar.addChild(this.background);

        /** @type {Sprite} */
        this.mortarIcon = new Sprite(RocketSVG);

        // Image is pretty big, scale it down
        this.mortarIcon.width = 28;
        this.mortarIcon.height = 28;

        // Center the sprite
        this.mortarIcon.x = -(this.mortarIcon.getBounds().width / 2);
        this.mortarIcon.y = -(this.mortarIcon.getBounds().height / 2);

        this.mortar.addChild(this.mortarIcon);

        // Set scale to current scale
        this.setScale(VIEWPORT_SCALES.Exfils[0], VIEWPORT_SCALES.Exfils[1]);

        this._setDirty();

        PIXI_VIEWPORT.addChild(this.mortar);
    }

    setScale(x: number, y: number) {
        this.mortar.scale.x = x;
        this.mortar.scale.y = y;
    }

    setPosition(x: number, y: number) {
        this.mortar.position.x = x;
        this.mortar.position.y = y;
    }

    /**
     * Regenerates the cached texture of this item.
     */
    _setDirty() {
        this.mortar.cacheAsBitmap = false;
        this.mortar.cacheAsBitmap = true;
    }

    destroy() {
        this.mortar.destroy();
    }
}
