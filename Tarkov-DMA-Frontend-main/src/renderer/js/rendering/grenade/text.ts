import { BitmapText } from "pixi.js";
import { GetFont } from "../bitmapFontManager";

const TEXT_Y_OFFSET = 36;

export class GrenadeText extends BitmapText {
    constructor(text: string) {
        super(text, {
            fontName: GetFont("#ffffff", "#000000"),
        });

        this.anchor.x = 0.5;
        this.anchor.y = 0.5;

        this.position.x = 0;
        this.position.y = TEXT_Y_OFFSET;
    }

    update(newText: string) {
        if (this.text === newText) return;

        this.text = newText;
    }
}
