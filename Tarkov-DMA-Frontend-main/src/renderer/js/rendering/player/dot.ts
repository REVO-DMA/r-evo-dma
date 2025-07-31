import { ColorSource, Graphics } from "pixi.js";

export class Dot extends Graphics {
    fillColor: ColorSource;
    borderColor: ColorSource;

    constructor(fillColor: ColorSource, borderColor: ColorSource) {
        super();

        // Set pixi property
        this.interactive = true;

        this.updateColors(fillColor, borderColor);
    }

    updateColors(fillColor: ColorSource, borderColor: ColorSource) {
        this.fillColor = fillColor;
        this.borderColor = borderColor;

        this.draw(fillColor, borderColor);
    }

    draw(fillColor: ColorSource, borderColor: ColorSource) {
        this.fillColor = fillColor;
        this.borderColor = borderColor;

        this.clear();

        this.beginFill(this.fillColor);

        this.lineStyle(2, this.borderColor);
        this.drawCircle(0, 0, 12);

        this.endFill();
    }
}
