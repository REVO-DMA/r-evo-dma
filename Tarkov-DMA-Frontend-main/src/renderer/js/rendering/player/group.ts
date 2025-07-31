import { ColorSource, Graphics, ObservablePoint } from "pixi.js";
import { VIEWPORT_SCALES } from "..";

const BASE_GROUP_LINE_WIDTH = 2;

export class Group extends Graphics {
    lineWidth: number;
    color: ColorSource;

    constructor(color: ColorSource) {
        super();

        this.zIndex = 10;

        // Set line width to current scale
        this.lineWidth = VIEWPORT_SCALES.Players[0] * BASE_GROUP_LINE_WIDTH;

        this.color = color;
    }

    connectPoints(points: ObservablePoint<any>[]) {
        this.clear();

        if (points.length === 0) return;

        // Draw background
        this.lineStyle({
            width: this.lineWidth,
            color: this.color,
        });

        this.moveTo(points[0].x, points[0].y);

        for (let i = 1; i < points.length; i++) {
            const playerPosition = points[i];

            this.lineTo(playerPosition.x, playerPosition.y);
        }

        this.lineTo(points[0].x, points[0].y);
    }

    setLineWidth(x: number) {
        this.lineWidth = x * BASE_GROUP_LINE_WIDTH;
    }
}
