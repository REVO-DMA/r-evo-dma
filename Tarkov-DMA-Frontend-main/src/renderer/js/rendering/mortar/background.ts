import { Graphics } from "pixi.js";

const BACKGROUND_OUTLINE_COLOR = "#000000";
const BACKGROUND_FILL_OPACITY = 0.8;

export class Background extends Graphics {
    constructor() {
        super();
        
        this.clear();

        this.beginFill("red", BACKGROUND_FILL_OPACITY);

        this.lineStyle(2, BACKGROUND_OUTLINE_COLOR);
        this.drawCircle(0, 0, 24);

        this.endFill();
    }
}
