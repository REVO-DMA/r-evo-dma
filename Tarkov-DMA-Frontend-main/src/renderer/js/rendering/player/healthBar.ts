import { Graphics } from "pixi.js";

const PLAYER_HEALTH_COLOR_GOOD = "#00ff00"; // Green
const PLAYER_HEALTH_COLOR_WARN = "#faec01"; // Yellow
const PLAYER_HEALTH_COLOR_BAD = "#ff0000"; // Red
const PLAYER_HEALTH_BORDER_WIDTH = 2;
const PLAYER_HEALTH_WIDTH = 80;
const PLAYER_HEALTH_HEIGHT = 5;
const PLAYER_HEALTH_Y_OFFSET = 18;

const PLAYER_MAX_HEALTH = 100;

export class PlayerHealth extends Graphics {
    currentHealth: number;
    
    constructor(currentHealth: number) {
        super();

        this.currentHealth = currentHealth;
        this.updateHealth(currentHealth, true);
    }

    getColor() {
        const health = this.currentHealth;

        if (health === 100) {
            return PLAYER_HEALTH_COLOR_GOOD;
        } else if (health === 75 || health === 50) {
            return PLAYER_HEALTH_COLOR_WARN;
        } else {
            return PLAYER_HEALTH_COLOR_BAD;
        }
    }

    updateHealth(currentHealth: number, force = false) {
        if (!force && this.currentHealth === currentHealth) return;

        // Update cached health
        this.currentHealth = currentHealth;

        this.clear();

        // Draw background
        this.beginFill("#000000");
        this.drawRect(
            -(PLAYER_HEALTH_WIDTH / 2) - PLAYER_HEALTH_BORDER_WIDTH,
            PLAYER_HEALTH_Y_OFFSET - PLAYER_HEALTH_BORDER_WIDTH,
            PLAYER_HEALTH_WIDTH + PLAYER_HEALTH_BORDER_WIDTH * 2,
            PLAYER_HEALTH_HEIGHT + PLAYER_HEALTH_BORDER_WIDTH * 2
        );
        this.endFill();

        // Draw foreground
        const foregroundWidth = (this.currentHealth / PLAYER_MAX_HEALTH) * PLAYER_HEALTH_WIDTH;
        this.beginFill(this.getColor());
        this.drawRect(-(PLAYER_HEALTH_WIDTH / 2), PLAYER_HEALTH_Y_OFFSET, foregroundWidth, PLAYER_HEALTH_HEIGHT);
        this.endFill();
    }
}
