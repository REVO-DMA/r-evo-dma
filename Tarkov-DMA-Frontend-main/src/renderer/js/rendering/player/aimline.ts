import { Graphics } from "pixi.js";

const AIMLINE_LENGTH = 300;
const AIMLINE_COLOR = "#ffffff";

export class Aimline extends Graphics {
    aimlineRadians: number;

    constructor(aimlineRadians: number) {
        super();

        this.aimlineRadians = aimlineRadians;
        this.updateAngle(aimlineRadians, true);
    }

    updateAngle(aimlineRadians: number, force: boolean = false) {
        if (!force && this.aimlineRadians === aimlineRadians) return;

        // Update cached aimline radians
        this.aimlineRadians = aimlineRadians;

        this.clear();

        const endX = Math.cos(aimlineRadians) * AIMLINE_LENGTH;
        const endY = Math.sin(aimlineRadians) * AIMLINE_LENGTH;

        this.lineStyle(2, AIMLINE_COLOR);

        this.moveTo(0, 0);
        this.lineTo(endX, endY);
    }
}
