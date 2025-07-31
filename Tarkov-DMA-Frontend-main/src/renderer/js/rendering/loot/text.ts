import { BitmapText } from "pixi.js";
import { GetFont } from "../bitmapFontManager";

const TEXT_Y_OFFSET = 25;

export class LootText extends BitmapText {
    constructor(text: string, fill: string, stroke: string) {
        super(text, {
            fontName: GetFont(fill, stroke),
        });

        this.anchor.x = 0.5;
        this.anchor.y = 0.5;

        this.position.x = 0;
        this.position.y = TEXT_Y_OFFSET;
    }

    update(text: string) {
        this.text = text;
    }
}
