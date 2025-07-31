import { Graphics, LINE_CAP, LINE_JOIN } from "pixi.js";

const CHEVRON_WIDTH = 17;
const CHEVRON_HEIGHT = 8;
const CHEVRON_Y_OFFSET = 6; // The vertical spacing between multiple chevrons

export class Marker extends Graphics {
    staticData:{
        Type: number;
        Height: number;
    };

    dynamicData: {
        MainColor: string;
        BorderColor: string;
        RelativeHeight: number;
    };

    constructor(type: number, mainColor: string, borderColor: string, itemHeight: number) {
        super();

        // Set pixi property
        //this.interactive = true;

        this.staticData = {
            Type: type,
            Height: itemHeight,
        };

        this.dynamicData = {
            MainColor: mainColor,
            BorderColor: borderColor,
            RelativeHeight: 0,
        };

        // Perform initial render
        if (type === 5) {
            this._drawSquare();
        } else {
            this._drawMarker();
        }
    }

    updateMarker(newMainColor: string, newBorderColor: string, playerHeight?: number): boolean {
        const relativeHeight = Math.ceil((this.staticData.Height - playerHeight) / 2);

        // Exit early if no data changed
        if (playerHeight != null) {
            if (newMainColor === this.dynamicData.MainColor && newBorderColor === this.dynamicData.BorderColor && relativeHeight == this.dynamicData.RelativeHeight) return false;
        } else {
            if (newMainColor === this.dynamicData.MainColor && newBorderColor === this.dynamicData.BorderColor) return false;
        }

        this.dynamicData.MainColor = newMainColor;
        this.dynamicData.BorderColor = newBorderColor;
        if (playerHeight != null) this.dynamicData.RelativeHeight = relativeHeight;

        // Update rendered marker
        if (this.staticData.Type === 5) {
            this._drawSquare();
        } else {
            this._drawMarker();
        }

        return true;
    }

    _drawMarker() {
        this.clear();

        if (this.dynamicData.RelativeHeight > 0) {
            // Above

            this.position.y = 0;
            // Draw background
            this.lineStyle({
                cap: LINE_CAP.ROUND,
                join: LINE_JOIN.ROUND,
                width: 8,
                color: this.dynamicData.BorderColor,
            });
            this._drawAbove();

            // Draw foreground
            this.lineStyle({
                cap: LINE_CAP.ROUND,
                join: LINE_JOIN.ROUND,
                width: 4,
                color: this.dynamicData.MainColor,
            });
            this._drawAbove();
        } else if (this.dynamicData.RelativeHeight < 0) {
            // Below

            this.position.y = -CHEVRON_HEIGHT;
            // Draw background
            this.lineStyle({
                cap: LINE_CAP.ROUND,
                join: LINE_JOIN.ROUND,
                width: 8,
                color: this.dynamicData.BorderColor,
            });
            this._drawBelow();

            // Draw foreground
            this.lineStyle({
                cap: LINE_CAP.ROUND,
                join: LINE_JOIN.ROUND,
                width: 4,
                color: this.dynamicData.MainColor,
            });
            this._drawBelow();
        } else {
            // Level

            this._drawLevel();
            this.position.y = 0;
        }
    }

    _drawLevel() {
        this.beginFill(this.dynamicData.MainColor);
        this.lineStyle(2, this.dynamicData.BorderColor);
        this.drawCircle(0, 0, 12);
        this.endFill();
    }

    _drawAbove() {
        const startX = -(CHEVRON_WIDTH / 2);

        this.moveTo(startX, CHEVRON_Y_OFFSET);
        this.lineTo(startX + CHEVRON_WIDTH / 2, -CHEVRON_HEIGHT + CHEVRON_Y_OFFSET);
        this.lineTo(startX + CHEVRON_WIDTH, 0 + CHEVRON_Y_OFFSET);
    }

    _drawBelow() {
        const startX = -(CHEVRON_WIDTH / 2);

        this.moveTo(startX, CHEVRON_Y_OFFSET);
        this.lineTo(startX + CHEVRON_WIDTH / 2, CHEVRON_HEIGHT + CHEVRON_Y_OFFSET);
        this.lineTo(startX + CHEVRON_WIDTH, 0 + CHEVRON_Y_OFFSET);
    }

    _drawSquare() {
        this.clear();
        this.beginFill(this.dynamicData.MainColor);
        this.lineStyle(2, this.dynamicData.BorderColor);
        this.drawRoundedRect(0, 0, 20, 20, 6);
        this.endFill();
    }
}
