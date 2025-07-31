import { Graphics } from "pixi.js";
import { ExfilStatus } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/ExfilStatus";

const BACKGROUND_OUTLINE_COLOR = "#000000";
const BACKGROUND_FILL_OPACITY = 0.8;

export class Background extends Graphics {
    Status: number;

    constructor(Status: number) {
        super();

        this.Status = -1;

        this.updateStatus(Status);
    }

    updateStatus(newStatus: number): boolean {
        if (this.Status === newStatus) return false;

        this.Status = newStatus;

        let color;
        if (this.Status === ExfilStatus.Open) {
            color = "green";
        } else if (this.Status === ExfilStatus.Pending) {
            color = "yellow";
        } else if (this.Status === ExfilStatus.Closed) {
            color = "red";
        } else if (this.Status === ExfilStatus.Transit) {
            color = "orange";
        }

        this.clear();

        this.beginFill(color, BACKGROUND_FILL_OPACITY);

        this.lineStyle(2, BACKGROUND_OUTLINE_COLOR);
        this.drawCircle(0, 0, 24);

        this.endFill();

        return true;
    }
}
