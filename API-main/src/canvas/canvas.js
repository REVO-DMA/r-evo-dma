import { Image, createCanvas, loadImage, registerFont } from "canvas";
import path from "path";
import { fileURLToPath } from "url";
import { randomNumberInRange } from "../utils.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const colorSets = [
    {
        fill: "rgb(0, 160, 233)",
        outline: "rgb(199, 100, 31)",
    },
    {
        fill: "rgb(233, 0, 43)",
        outline: "rgb(157, 0, 29)",
    },
    {
        fill: "rgb(0, 157, 50)",
        outline: "rgb(0, 81, 26)",
    }
];

const avatarWidth = 512;
const avatarHeight = 512;
const avatarFontSize = 256;
const avatarFontFamily = "Neo Sans Std";
const avatarFontOutlineThickness = 10;
const avatarFontShadowColor = "rgba(0, 0, 0, 1)";
const avatarFontBlurAmount = 12;
/** @type {Image} */
let avatarBackground;

export async function initialize() {
    console.log("Preloading canvas assets...");

    registerFont(path.join(__dirname, "NeoSans.otf"), { family: avatarFontFamily });
    avatarBackground = await loadImage(path.join(__dirname, "avatarBackground.png"));
}

/**
 * @param {string} initials
 * @returns {Buffer}
 */
export function create(initials) {
    const colorSet = getColorSet();

    const canvas = createCanvas(avatarWidth, avatarHeight);
    const ctx = canvas.getContext("2d");

    // Add the background to the canvas
    ctx.drawImage(avatarBackground, 0, 0, avatarWidth, avatarHeight);

    ctx.font = `${avatarFontSize}px "${avatarFontFamily}"`;
    ctx.textAlign = "center";
    ctx.textBaseline = "top";
    ctx.shadowColor = avatarFontShadowColor;
    ctx.shadowBlur = avatarFontBlurAmount;

    // Measure text
    const fontMeasurement = ctx.measureText(initials);
    const actualFontHeight = fontMeasurement.actualBoundingBoxAscent + fontMeasurement.actualBoundingBoxDescent;

    const xPos = avatarWidth / 2;
    const yPos = (avatarHeight - actualFontHeight) / 2;

    // Stroke
    ctx.strokeStyle = colorSet.outline;
    ctx.lineWidth = avatarFontOutlineThickness;
    ctx.strokeText(initials, xPos, yPos);

    // Fill
    ctx.shadowBlur = 0; // Disable shadow
    ctx.fillStyle = colorSet.fill;
    ctx.fillText(initials, xPos, yPos);

    return canvas.toBuffer();
}

function getColorSet() {
    const index = randomNumberInRange(0, 2);

    return colorSets[index];
}