import { Text as PIXI_Text } from "pixi.js";

const TEXT_Y_OFFSET = 38;

export class ExfilText extends PIXI_Text {
    constructor(string: string) {
        super(string, {
            fontFamily: "Neo Sans Std",
            fontSize: 20,
            fill: "#ffffff",
            align: "center",
            lineJoin: "bevel",
            stroke: "#000000",
            strokeThickness: 3,
        });

        this.x = -(this.getBounds().width / 2);
        this.y = -(this.getBounds().height / 2) + TEXT_Y_OFFSET;
    }
}
