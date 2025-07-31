import { Viewport } from "pixi-viewport";
import { Application, Point, Sprite, Texture } from "pixi.js";
import { send as ipcSend } from "../ipc/manager";
import { getAppSettingValue } from "../settings/appSettings";
import { scaleExfils } from "./exfil/exfilManager";
import { scaleGrenades } from "./grenade/grenadeManager";
import { scaleLoot } from "./loot/lootManager";
import { MapProperties, MapType, Maps } from "./maps/mapManager";
import { localPlayer, scalePlayers } from "./player/playerManager";
import { scaleQuestLocations } from "./questLocation/questManager";
import { SVGScene } from "@pixi-essentials/svg";
import { scaleMortars } from "./mortar/mortarManager";

const appContainerEl: HTMLDivElement = (document.getElementById("appContainer") as HTMLDivElement);

let PIXI_APP: Application<import("pixi.js").ICanvas> = null;

let CURRENT_MAP_NAME: string = null;

/** The base map. */
let PIXI_BASE_MAP: SVGScene | Sprite = null;

/** The active floor layer. */
let PIXI_CURRENT_FLOOR_MAP: SVGScene | Sprite = null;

let CURRENT_MAP_LAYER: number = 0;

let MAP_LAYER_DATA: import("./maps/mapManager").MapLayer[] = null;

let MAP_IMAGE_LAYERS: SVGScene[] | Sprite[] = null;

let MAP_PROPERTIES: MapProperties = null;

export let PIXI_VIEWPORT: Viewport = null;

/** Whether or not the active map has multiple map levels. */
let MAP_HAS_MULTI_FLOORS = false;

/** Whether or not the renderer is instantiated. */
let ACTIVE = false;

/** Whether or not the viewport should follow the LocalPlayer */
export let FOLLOW_LOCALPLAYER: boolean;
if (localStorage.getItem("followLocalplayer") == null) {
    FOLLOW_LOCALPLAYER = true;
} else {
    FOLLOW_LOCALPLAYER = JSON.parse(localStorage.getItem("followLocalplayer")) as boolean;
}

const mapFollowModeToggleEl = document.getElementById("mapFollowModeToggle");
const mapFollowModeIconEl = document.getElementById("mapFollowModeIcon");
const mapFollowModeTextEl = document.getElementById("mapFollowModeText");

mapFollowModeToggleEl.addEventListener("click", () => {
    followLocalPlayer();
});

export const VIEWPORT_SCALES = {
    Players: [0, 0],
    Loot: [0, 0],
    Grenades: [0, 0],
    Exfils: [0, 0],
};

const PIXI_APP_PREF = {
    antialias: true,
    resolution: window.devicePixelRatio,
};

export async function initialize(mapName: string) {
    if (ACTIVE) return;

    CURRENT_MAP_NAME = mapName;

    ACTIVE = true;

    // Apply performance target
    updateRenderingPerformanceMode();

    PIXI_APP = new Application({
        antialias: PIXI_APP_PREF.antialias,
        powerPreference: "high-performance",
        autoDensity: true,
        width: appContainerEl.offsetWidth,
        height: appContainerEl.offsetHeight,
        resolution: PIXI_APP_PREF.resolution,
        background: "#212121",
        resizeTo: appContainerEl,
    });

    updateTickerMaxFPS();

    appContainerEl.appendChild(((PIXI_APP.view as unknown) as Node));

    PIXI_VIEWPORT = new Viewport({
        screenWidth: appContainerEl.offsetWidth,
        screenHeight: appContainerEl.offsetHeight,
        worldWidth: window.innerWidth,
        worldHeight: window.innerHeight,
        events: PIXI_APP.renderer.events,
    });

    // Allow child z-index to work
    PIXI_VIEWPORT.sortableChildren = true;

    // Limit zoom
    PIXI_VIEWPORT.clampZoom({
        minScale: 0.1,
        maxScale: 30,
    });

    // Add the Viewport to the stage
    PIXI_APP.stage.addChild(PIXI_VIEWPORT);

    // Activate viewport plugins
    PIXI_VIEWPORT.drag().wheel();

    const mapEntry = Maps.get(CURRENT_MAP_NAME);
    if (mapEntry == undefined) throw new Error(`Unable to find map entry for map: \"${CURRENT_MAP_NAME}\"!`);
    
    MAP_PROPERTIES = mapEntry.Properties;
    
    MAP_LAYER_DATA = mapEntry.Layers;
    MAP_IMAGE_LAYERS = [];

    if (MAP_LAYER_DATA.length > 1) MAP_HAS_MULTI_FLOORS = true;
    else MAP_HAS_MULTI_FLOORS = false;

    for (let i = 0; i < MAP_LAYER_DATA.length; i++) {
        const mapLayer = MAP_LAYER_DATA[i];

        let map: SVGScene | Sprite;

        if (mapEntry.Properties.Type === MapType.Raster) {
            const texture = Texture.from(mapLayer.file);
            map = new Sprite(texture);
        } else if (mapEntry.Properties.Type === MapType.SVG) {
            map = await SVGScene.from(mapLayer.file);
        }

        if (i === 0) {
            if (MAP_PROPERTIES.HasMainLevel) {
                PIXI_BASE_MAP = PIXI_VIEWPORT.addChild(map);
                continue;
            } else {
                PIXI_CURRENT_FLOOR_MAP = PIXI_VIEWPORT.addChild(map);
                CURRENT_MAP_LAYER = 0;
            }
        }

        MAP_IMAGE_LAYERS[i] = map;
    }

    if (MAP_PROPERTIES.HasMainLevel) {
        PIXI_VIEWPORT.worldWidth = PIXI_BASE_MAP.width;
        PIXI_VIEWPORT.worldHeight = PIXI_BASE_MAP.height;
    } else {
        PIXI_VIEWPORT.worldWidth = PIXI_CURRENT_FLOOR_MAP.width;
        PIXI_VIEWPORT.worldHeight = PIXI_CURRENT_FLOOR_MAP.height;
    }

    applyZoomLevel();
    applyFreeMapCenter();

    // Set initial scales
    const initialScaleX = (1 / PIXI_VIEWPORT.scale.x) * 0.5;
    const initialScaleY = (1 / PIXI_VIEWPORT.scale.y) * 0.5;
    // Players
    const initialPlayerScale = getAppSettingValue("player_scale");
    VIEWPORT_SCALES.Players[0] = initialScaleX * initialPlayerScale;
    VIEWPORT_SCALES.Players[1] = initialScaleY * initialPlayerScale;
    // Loot
    const initialLootScale = getAppSettingValue("loot_scale");
    VIEWPORT_SCALES.Loot[0] = initialScaleX * initialLootScale;
    VIEWPORT_SCALES.Loot[1] = initialScaleY * initialLootScale;
    // Grenades
    const initialGrenadeScale = getAppSettingValue("grenade_scale");
    VIEWPORT_SCALES.Grenades[0] = initialScaleX * initialGrenadeScale;
    VIEWPORT_SCALES.Grenades[1] = initialScaleY * initialGrenadeScale;
    // Exfils
    const initialExfilScale = getAppSettingValue("exfil_scale");
    VIEWPORT_SCALES.Exfils[0] = initialScaleX * initialExfilScale;
    VIEWPORT_SCALES.Exfils[1] = initialScaleY * initialExfilScale;

    // Attach events
    PIXI_VIEWPORT.on("drag-start", () => {
        if (FOLLOW_LOCALPLAYER) followLocalPlayer(false, false);
    });

    PIXI_VIEWPORT.on("drag-end", () => {
        if (!FOLLOW_LOCALPLAYER) {
            localStorage.setItem(`${CURRENT_MAP_NAME}_Center`, JSON.stringify(PIXI_VIEWPORT.center));
        }

        console.log(`Center: ${PIXI_VIEWPORT.center}`);
    });

    PIXI_VIEWPORT.on("wheel-start", () => {
        // If following player, center on them to prevent jumpy behavior
        if (FOLLOW_LOCALPLAYER && localPlayer !== null) {
            PIXI_VIEWPORT.plugins.get("wheel").options.center = localPlayer.Container.position;
        }
    });

    PIXI_VIEWPORT.on("zoomed-end", () => {
        localStorage.setItem(`${CURRENT_MAP_NAME}_Zoom`, JSON.stringify(PIXI_VIEWPORT.scale.x));

        scaleViewportPlayers();
        scaleViewportLoot();
        scaleViewportExplosives();
        scaleViewportExfils();

        console.log(`Scale: ${PIXI_VIEWPORT.scale.x}`);
    });

    // Resize viewport on renderer resize
    PIXI_APP.renderer.on("resize", () => {
        PIXI_VIEWPORT.resize(appContainerEl.offsetWidth, appContainerEl.offsetHeight, PIXI_VIEWPORT.worldWidth, PIXI_VIEWPORT.worldHeight);

        if (!FOLLOW_LOCALPLAYER) {
            applyFreeMapCenter();
        }
    });

    ipcSend(40, null);
}

export function updateTickerMaxFPS() {
    const newFPS = getAppSettingValue("radar_updateRate");

    if (PIXI_APP != null) PIXI_APP.ticker.maxFPS = newFPS;
}

export function updateRenderingPerformanceMode() {
    const newState = getAppSettingValue("radar_performanceMode");

    if (newState) {
        PIXI_APP_PREF.antialias = false;
        PIXI_APP_PREF.resolution = 1;
    } else {
        PIXI_APP_PREF.antialias = true;
        PIXI_APP_PREF.resolution = window.devicePixelRatio;
    }
}

export function shutdown() {
    if (!ACTIVE) return;
    ACTIVE = false;

    // Destroy all map layers
    for (let i = 0; i < MAP_IMAGE_LAYERS.length; i++) {
        const map = MAP_IMAGE_LAYERS[i];

        if (map != null) MAP_IMAGE_LAYERS[i].destroy(true);
    }

    MAP_IMAGE_LAYERS = null;
    MAP_LAYER_DATA = null;
    CURRENT_MAP_LAYER = 0;
    CURRENT_MAP_NAME = null;
    
    PIXI_BASE_MAP = null;
    PIXI_CURRENT_FLOOR_MAP = null;

    if (PIXI_VIEWPORT != null) {
        PIXI_VIEWPORT.destroy({
            baseTexture: true,
            children: true,
            texture: true
        });
        PIXI_VIEWPORT = null;
    }

    if (PIXI_APP != null) {
        PIXI_APP.destroy(true, true);
        PIXI_APP = null;
    }
}

function applyZoomLevel() {
    const lsZoomLevel = localStorage.getItem(`${CURRENT_MAP_NAME}_Zoom`);
    let zoomLevel: number;
    if (lsZoomLevel == null) {
        zoomLevel = MAP_PROPERTIES.InitialZoom;
    } else {
        zoomLevel = JSON.parse(lsZoomLevel) as number;
    }

    PIXI_VIEWPORT.setZoom(zoomLevel, true);
}

function applyFreeMapCenter() {
    const lsMapCenter = localStorage.getItem(`${CURRENT_MAP_NAME}_Center`);
    let initialPoint: Point;
    if (lsMapCenter == null) {
        initialPoint = new Point(MAP_PROPERTIES.InitialX, MAP_PROPERTIES.InitialY);
    } else {
        initialPoint = JSON.parse(lsMapCenter) as Point;
    }

    PIXI_VIEWPORT.moveCenter(initialPoint.x, initialPoint.y);
}

export function followLocalPlayer(force = false, restoreSavedPos = true) {
    if (!ACTIVE || localPlayer === null || localPlayer.Container === null) return;

    const save = () => {
        localStorage.setItem("followLocalplayer", JSON.stringify(FOLLOW_LOCALPLAYER));
    };

    const follow = () => {
        PIXI_VIEWPORT.follow(localPlayer.Container);
        FOLLOW_LOCALPLAYER = true;
        mapFollowModeIconEl.innerHTML = `<i class="fa-duotone fa-arrows-up-down-left-right buttonIcon"></i>`;
        mapFollowModeTextEl.innerText = "Map Free";
        save();
    };

    const unfollow = () => {
        PIXI_VIEWPORT.plugins.remove("follow");
        PIXI_VIEWPORT.plugins.get("wheel").options.center = null;
        FOLLOW_LOCALPLAYER = false;
        mapFollowModeIconEl.innerHTML = `<i class="fa-duotone fa-arrows-to-circle buttonIcon"></i>`;
        mapFollowModeTextEl.innerText = "Map Follow";
        save();
        if (restoreSavedPos) applyFreeMapCenter();
    };

    if (force) {
        follow();
        return;
    }

    if (FOLLOW_LOCALPLAYER) unfollow();
    else follow();
}

export function scaleViewportPlayers() {
    if (PIXI_APP === null || PIXI_VIEWPORT === null) return;

    const playerScale = getAppSettingValue("player_scale");

    // Update scale
    const newX = (1 / PIXI_VIEWPORT.scale.x) * 0.5 * playerScale;
    const newY = (1 / PIXI_VIEWPORT.scale.y) * 0.5 * playerScale;

    if (VIEWPORT_SCALES.Players[0] !== newX || VIEWPORT_SCALES.Players[1] !== newY) {
        VIEWPORT_SCALES.Players[0] = newX;
        VIEWPORT_SCALES.Players[1] = newY;

        scalePlayers(newX, newY);
    }
}

export function scaleViewportLoot() {
    if (PIXI_APP === null || PIXI_VIEWPORT === null) return;

    const lootScale = getAppSettingValue("loot_scale");

    // Update scale
    const newX = (1 / PIXI_VIEWPORT.scale.x) * 0.5 * lootScale;
    const newY = (1 / PIXI_VIEWPORT.scale.y) * 0.5 * lootScale;

    if (VIEWPORT_SCALES.Loot[0] !== newX || VIEWPORT_SCALES.Loot[1] !== newY) {
        VIEWPORT_SCALES.Loot[0] = newX;
        VIEWPORT_SCALES.Loot[1] = newY;

        scaleLoot(newX, newY);
        scaleQuestLocations(newX, newY);
    }
}

export function scaleViewportExplosives() {
    if (PIXI_APP === null || PIXI_VIEWPORT === null) return;

    const grenadeScale = getAppSettingValue("grenade_scale");

    // Update scale
    const newX = (1 / PIXI_VIEWPORT.scale.x) * 0.5 * grenadeScale;
    const newY = (1 / PIXI_VIEWPORT.scale.y) * 0.5 * grenadeScale;

    if (VIEWPORT_SCALES.Grenades[0] !== newX || VIEWPORT_SCALES.Grenades[1] !== newY) {
        VIEWPORT_SCALES.Grenades[0] = newX;
        VIEWPORT_SCALES.Grenades[1] = newY;

        scaleGrenades(newX, newY);
        scaleMortars(newX, newY);
    }
}

export function scaleViewportExfils() {
    if (PIXI_APP === null || PIXI_VIEWPORT === null) return;

    const exfilScale = getAppSettingValue("exfil_scale");

    // Update scale
    const newX = (1 / PIXI_VIEWPORT.scale.x) * 0.5 * exfilScale;
    const newY = (1 / PIXI_VIEWPORT.scale.y) * 0.5 * exfilScale;

    if (VIEWPORT_SCALES.Exfils[0] !== newX || VIEWPORT_SCALES.Exfils[1] !== newY) {
        VIEWPORT_SCALES.Exfils[0] = newX;
        VIEWPORT_SCALES.Exfils[1] = newY;

        scaleExfils(newX, newY);
    }
}

let ALPHA_SET = false;
function fixAlpha(set: boolean) {
    if (ALPHA_SET) {
        const originalCenter = PIXI_VIEWPORT.center;
        PIXI_VIEWPORT.moveCenter(originalCenter.x + 0.001, originalCenter.y + 0.001);
        setTimeout(() => {
            PIXI_VIEWPORT.moveCenter(originalCenter.x, originalCenter.y);
        }, 32);

        ALPHA_SET = set;
    }
}

export function setMapFloor(localPlayerHeight: number) {
    if (MAP_LAYER_DATA === null || !MAP_HAS_MULTI_FLOORS) return;

    let startIndex = 0;
    if (MAP_PROPERTIES.HasMainLevel) startIndex = 1;

    let floorSet = false;

    for (let i = startIndex; i < MAP_LAYER_DATA.length; i++) {
        const thisData = MAP_LAYER_DATA[i];

        if (localPlayerHeight >= thisData.minHeight && localPlayerHeight <= thisData.maxHeight) {
            floorSet = true;

            if (i === CURRENT_MAP_LAYER) break;

            if (PIXI_CURRENT_FLOOR_MAP != null) PIXI_VIEWPORT.removeChild(PIXI_CURRENT_FLOOR_MAP);
            PIXI_CURRENT_FLOOR_MAP = PIXI_VIEWPORT.addChild(MAP_IMAGE_LAYERS[i]);
            CURRENT_MAP_LAYER = i;

            if (MAP_PROPERTIES.HasMainLevel) {
                if (thisData.dimMainLevel) {
                    const amount = thisData.dimAmount != null ? thisData.dimAmount : 0.5;
                    PIXI_BASE_MAP.alpha = amount;
                    ALPHA_SET = true;
                    fixAlpha(true);
                }
                else {
                    PIXI_BASE_MAP.alpha = 1;
                    ALPHA_SET = false;
                    fixAlpha(false);
                }
            }
            
            break;
        }
    }

    if (floorSet || !MAP_PROPERTIES.HasMainLevel) return;

    // If no suitable floor could be found, remove the current one and leave the base texture
    if (PIXI_CURRENT_FLOOR_MAP != null) {
        PIXI_VIEWPORT.removeChild(PIXI_CURRENT_FLOOR_MAP);
        PIXI_CURRENT_FLOOR_MAP = null;
    }

    CURRENT_MAP_LAYER = 0;
    PIXI_BASE_MAP.alpha = 1;
    fixAlpha(false);
}
