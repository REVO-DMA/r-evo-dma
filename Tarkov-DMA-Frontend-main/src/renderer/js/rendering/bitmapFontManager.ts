import { BitmapFont, ITextStyle, TextStyle } from "pixi.js";

export const BASE_FONT_NAME = "Neo Sans";
const CHARSET = [['a', 'z'], ['A', 'Z'], ['0', '9'], " !\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"];

const baseFontProps: Partial<ITextStyle> = {
    fontFamily: "Neo Sans Std",
    fontSize: 20,
    align: "center",
    lineJoin: "bevel",
    strokeThickness: 3,
};

interface FontEntry {
    FontName: string;
    Fill: string,
    Stroke: string;
}

let LootFonts = new Map<string, FontEntry>();

export function Initialize() {
    BitmapFont.from(
        BASE_FONT_NAME,
        new TextStyle({
            ...baseFontProps,
            fill: "#ffffff",
            stroke: "#000000",
        }),
        {
            chars: CHARSET
        }
    );
}

/**
 * Gets the name of a bitmap font for a given style.
 * Creates the font if it does not exist yet.
 */
export function GetFont(fill: string, stroke: string): string {
    const lfKey = fill + stroke;

    const res = LootFonts.get(lfKey);

    // Font for this style is not created yet
    if (res === undefined) {
        const bmFontName = BASE_FONT_NAME + lfKey;

        BitmapFont.from(
            bmFontName,
            new TextStyle({
                ...baseFontProps,
                fill: fill,
                stroke: stroke,
            }),
            {
                chars: CHARSET,
            }
        );

        const lfEntry: FontEntry = {
            FontName: bmFontName,
            Fill: fill,
            Stroke: stroke
        };

        LootFonts.set(lfKey, lfEntry);

        return bmFontName;
    }

    return res.FontName;
}
