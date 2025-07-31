import { Graphics } from "pixi.js";

const GRENADE_DOT_OUTLINE_COLOR = "#000000";
const GRENADE_FILL_COLOR = "#B60102";
const GRENADE_FILL_OPACITY = 0.8;

export class Dot extends Graphics {
    constructor() {
        super();

        this.clear();

        this.beginFill(GRENADE_FILL_COLOR, GRENADE_FILL_OPACITY);

        this.lineStyle(2, GRENADE_DOT_OUTLINE_COLOR);
        this.drawCircle(0, 0, 24);

        this.endFill();
    }
}
