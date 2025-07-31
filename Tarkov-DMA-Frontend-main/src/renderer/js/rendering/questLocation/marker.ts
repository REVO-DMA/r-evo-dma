import { Graphics } from "pixi.js";

export class Marker extends Graphics {
    constructor() {
        super();

        // Set pixi property
        //this.interactive = true;

        this._drawSquare();
    }

    _drawSquare() {
        this.clear();
        this.beginFill("#1CFF03");
        this.lineStyle(2, "#000000");
        this.drawRoundedRect(0, 0, 20, 20, 6);
        this.endFill();
    }
}
