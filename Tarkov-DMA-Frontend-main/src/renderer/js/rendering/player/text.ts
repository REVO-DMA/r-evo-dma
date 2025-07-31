import { BitmapText } from "pixi.js";
import { GetFont } from "../bitmapFontManager";

const TEXT_Y_OFFSET = 43;

export class PlayerText extends BitmapText {
    constructor(string: string) {
        super(string, {
            fontName: GetFont("#ffffff", "#000000"),
        });

        this.anchor.x = 0.5;
        this.anchor.y = 0.5;

        this.position.x = 0;
        this.position.y = TEXT_Y_OFFSET;
    }

    update(string: string) {
        if (this.text === string) return;

        this.text = string;
    }
}
