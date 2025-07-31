import { ColorSource, Graphics, LINE_CAP, LINE_JOIN } from "pixi.js";

const CHEVRON_WIDTH = 17;
const CHEVRON_HEIGHT = 8;
const CHEVRON_X_OFFSET = 20; // The horizontal spacing between the player dot and the chevron
const CHEVRON_Y_OFFSET = 6; // The vertical spacing between multiple chevrons

const ABOVE_COLOR: ColorSource = "#ff0000";
const BELOW_COLOR: ColorSource = "#0096ff";

export class HeightIndicator extends Graphics {
    relativeHeight: number;

    constructor(relativeHeight: number) {
        super();

        this.relativeHeight = relativeHeight;
        this.updateRelativeHeight(relativeHeight, true);
    }

    doDrawAbove(count: number) {
        for (let i = 0; i < count; i++) {
            this.moveTo(0, 0 + CHEVRON_Y_OFFSET * i);
            this.lineTo(CHEVRON_WIDTH / 2, -CHEVRON_HEIGHT + CHEVRON_Y_OFFSET * i);
            this.lineTo(CHEVRON_WIDTH, 0 + CHEVRON_Y_OFFSET * i);
        }
    }

    doDrawBelow(count: number) {
        for (let i = 0; i < count; i++) {
            this.moveTo(0, 0 + CHEVRON_Y_OFFSET * i);
            this.lineTo(CHEVRON_WIDTH / 2, CHEVRON_HEIGHT + CHEVRON_Y_OFFSET * i);
            this.lineTo(CHEVRON_WIDTH, 0 + CHEVRON_Y_OFFSET * i);
        }
    }

    updateRelativeHeight(relativeHeight: number, force = false) {
        // Only redraw on change
        if (!force && this.relativeHeight === relativeHeight) return;

        // Update cached height
        this.relativeHeight = relativeHeight;

        this.clear();

        let absHeight = Math.floor(Math.abs(relativeHeight) / 2);
        if (absHeight > 3) absHeight = 3; // Limit to 3 chevrons

        if (relativeHeight > 0) {
            this.drawAbove(absHeight);
        } else if (relativeHeight < 0) {
            this.drawBelow(absHeight);
        }

        this.position.x = CHEVRON_X_OFFSET;
    }

    drawAbove(count: number) {
        // Draw background
        this.lineStyle({
            cap: LINE_CAP.ROUND,
            join: LINE_JOIN.ROUND,
            width: 6,
            color: "#000000",
        });
        this.doDrawAbove(count);

        // Draw foreground
        this.lineStyle({
            cap: LINE_CAP.ROUND,
            join: LINE_JOIN.ROUND,
            width: 2,
            color: ABOVE_COLOR,
        });
        this.doDrawAbove(count);
    }

    drawBelow(count: number) {
        // Draw background
        this.lineStyle({
            cap: LINE_CAP.ROUND,
            join: LINE_JOIN.ROUND,
            width: 6,
            color: "#000000",
        });
        this.doDrawBelow(count);

        // Draw foreground
        this.lineStyle({
            cap: LINE_CAP.ROUND,
            join: LINE_JOIN.ROUND,
            width: 2,
            color: BELOW_COLOR,
        });
        this.doDrawBelow(count);

        this.position.y = -((CHEVRON_HEIGHT * count) / 2) + 2;
    }
}
