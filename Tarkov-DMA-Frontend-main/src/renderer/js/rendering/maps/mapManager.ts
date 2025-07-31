import labs_m1f from "../../../img/maps/Labs_m1f.svg";
import labs_1f from "../../../img/maps/Labs_1f.svg";
import labs_2f from "../../../img/maps/Labs_2f.svg";

import reserve_base from "../../../img/maps/Reserve_base.svg";
import reserve_m1f from "../../../img/maps/Reserve_m1f.svg";

import shoreline_base from "../../../img/maps/Shoreline_base.svg";
import shoreline_m1f from "../../../img/maps/Shoreline_-1f.svg";
import shoreline_1f from "../../../img/maps/Shoreline_1f.svg";
import shoreline_2f from "../../../img/maps/Shoreline_2f.svg";
import shoreline_3f from "../../../img/maps/Shoreline_3f.svg";
import groundZero_base from "../../../img/maps/GroundZero_base.svg";
import groundZero_1f from "../../../img/maps/GroundZero_1f.svg";
import groundZero_m1f from "../../../img/maps/GroundZero_-1f.svg";
import groundZero_2f from "../../../img/maps/GroundZero_2f.svg";
import groundZero_3f from "../../../img/maps/GroundZero_3f.svg";

import factory_base from "../../../img/maps/Factory_base.svg";
import factory_m1f from "../../../img/maps/Factory_-1f.svg";
import factory_2f from "../../../img/maps/Factory_2f.svg";
import factory_3f from "../../../img/maps/Factory_3f.svg";

import customs_base from "../../../img/maps/Customs_base.svg";
import customs_m1f from "../../../img/maps/Customs_-1f.svg";
import customs_1f from "../../../img/maps/Customs_1f.svg";
import customs_2f from "../../../img/maps/Customs_2f.svg";
import customs_3f from "../../../img/maps/Customs_3f.svg";

import interchange_base from "../../../img/maps/Interchange_base.svg";
import interchange_1f from "../../../img/maps/Interchange_1f.svg";
import interchange_2f from "../../../img/maps/Interchange_2f.svg";

import streets_base from "../../../img/maps/StreetsOfTarkov_base.svg";
import streets_m1f from "../../../img/maps/StreetsOfTarkov_-1f.svg";
import streets_1f from "../../../img/maps/StreetsOfTarkov_1f.svg";
import streets_2f from "../../../img/maps/StreetsOfTarkov_2f.svg";
import streets_3f from "../../../img/maps/StreetsOfTarkov_3f.svg";
import streets_4f from "../../../img/maps/StreetsOfTarkov_4f.svg";
import streets_5f from "../../../img/maps/StreetsOfTarkov_5f.svg";

import lighthouse_base from "../../../img/maps/Lighthouse_base.svg";

import woods_base from "../../../img/maps/Woods_base.svg";

export let currentMap: string = null;

export function setMap(newMap: string) {
    currentMap = newMap;
    
    console.log(`Map set: ${currentMap}`);
}

export const enum MapType {
    Raster = 0,
    SVG = 1,
}

export interface MapProperties {
    X: number;
    Y: number;
    Scale: number;
    Rotation: number;
    PawnRotation: number;
    HasMainLevel: boolean;
    Type: MapType;
    InitialZoom: number;
    InitialX: number;
    InitialY: number;
}

export interface MapLayer {
    minHeight: number;
    maxHeight: number;
    file: string;
    dimMainLevel: boolean;
    dimAmount: number;
}

export interface MapEntry {
    Properties: MapProperties;
    Layers: MapLayer[];
}

export const Maps = new Map<string, MapEntry>([
    [
        "shoreline",
        {
            Properties: {
                X: -503.5,
                Y: -414.9,
                Scale: 1.00335,
                Rotation: -180,
                PawnRotation: -180,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 1.16,
                InitialX: 821,
                InitialY: 507,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: shoreline_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -9,
                    maxHeight: -6,
                    file: shoreline_m1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -4,
                    maxHeight: -2,
                    file: shoreline_1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -1,
                    maxHeight: 1,
                    file: shoreline_2f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 2,
                    maxHeight: 999,
                    file: shoreline_3f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        },
    ],
    [
        "groundzero",
        {
            Properties: {
                X: 99.35,
                Y: 364.4,
                Scale: 1,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 2.5,
                InitialX: 167,
                InitialY: 243,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: groundZero_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -999,
                    maxHeight: 16,
                    file: groundZero_m1f,
                    dimMainLevel: true,
                    dimAmount: 0.4,
                },
                {
                    minHeight: 22,
                    maxHeight: 26,
                    file: groundZero_1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 27,
                    maxHeight: 31,
                    file: groundZero_2f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 32,
                    maxHeight: 999,
                    file: groundZero_3f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        }
    ],
    [
        "factory",
        {
            Properties: {
                X: 64.7,
                Y: 67.15,
                Scale: 0.994,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 8.5,
                InitialX: 72,
                InitialY: 65,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: factory_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -999,
                    maxHeight: -2,
                    file: factory_m1f,
                    dimMainLevel: true,
                    dimAmount: null,
                },
                {
                    minHeight: 3,
                    maxHeight: 6,
                    file: factory_2f,
                    dimMainLevel: true,
                    dimAmount: null,
                },
                {
                    minHeight: 7,
                    maxHeight: 999,
                    file: factory_3f,
                    dimMainLevel: true,
                    dimAmount: null,
                },
            ],
        },
    ],
    [
        "bigmap",
        {
            Properties: {
                X: -693.13,
                Y: -302.7,
                Scale: 0.992,
                Rotation: -180,
                PawnRotation: -180,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 2.15,
                InitialX: 530,
                InitialY: 283,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: customs_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -3,
                    maxHeight: -1,
                    file: customs_m1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 0,
                    maxHeight: 2,
                    file: customs_1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 3,
                    maxHeight: 5,
                    file: customs_2f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 6,
                    maxHeight: 999,
                    file: customs_3f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        },
    ],
    [
        "tarkovstreets",
        {
            Properties: {
                X: 281,
                Y: 533.5,
                Scale: 1.005,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 1.58,
                InitialX: 287,
                InitialY: 415,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: streets_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                // {
                //     minHeight: -100,
                //     maxHeight: -1,
                //     file: streets_m1f,
                //     dimMainLevel: true,
                //     dimAmount: null,
                // },
                {
                    minHeight: -100,
                    maxHeight: 999,
                    file: streets_1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                // {
                //     minHeight: 4,
                //     maxHeight: 6,
                //     file: streets_2f,
                //     dimMainLevel: false,
                //     dimAmount: null,
                // },
                // {
                //     minHeight: 7,
                //     maxHeight: 9,
                //     file: streets_3f,
                //     dimMainLevel: false,
                //     dimAmount: null,
                // },
                // {
                //     minHeight: 10,
                //     maxHeight: 12,
                //     file: streets_4f,
                //     dimMainLevel: false,
                //     dimAmount: null,
                // },
                // {
                //     minHeight: 13,
                //     maxHeight: 999,
                //     file: streets_5f,
                //     dimMainLevel: false,
                //     dimAmount: null,
                // },
            ],
        },
    ],
    [
        "lighthouse",
        {
            Properties: {
                X: 544.859,
                Y: -997.95,
                Scale: 1,
                Rotation: 90,
                PawnRotation: -90,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 1.16,
                InitialX: 873,
                InitialY: 536,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: lighthouse_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        },
    ],
    [
        "woods",
        {
            Properties: {
                X: 724.25,
                Y: 462.7,
                Scale: 1.05,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 0.85,
                InitialX: 719,
                InitialY: 697,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: woods_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        }
    ],
    [
        "rezervbase",
        {
            Properties: {
                X: 1815,
                Y: 1613,
                Scale: 6,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 0.4,
                InitialX: 1768,
                InitialY: 1613,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: reserve_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: -999,
                    maxHeight: -8,
                    file: reserve_m1f,
                    dimMainLevel: true,
                    dimAmount: null,
                },
            ],
        },
    ],
    [
        "laboratory",
        {
            Properties: {
                X: 3114.5,
                Y: -1088,
                Scale: 8.276,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: false,
                Type: MapType.SVG,
                InitialZoom: 0.6,
                InitialX: 1556,
                InitialY: 1697,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: -1,
                    file: labs_m1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 0,
                    maxHeight: 3,
                    file: labs_1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 4,
                    maxHeight: 999,
                    file: labs_2f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        },
    ],
    [
        "interchange",
        {
            Properties: {
                X: 398,
                Y: 494.5,
                Scale: 1.09,
                Rotation: 0,
                PawnRotation: 0,
                HasMainLevel: true,
                Type: MapType.SVG,
                InitialZoom: 1.35,
                InitialX: 372,
                InitialY: 500,
            },
            Layers: [
                {
                    minHeight: -1000,
                    maxHeight: 999,
                    file: interchange_base,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 27,
                    maxHeight: 36,
                    file: interchange_1f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
                {
                    minHeight: 37,
                    maxHeight: 999,
                    file: interchange_2f,
                    dimMainLevel: false,
                    dimAmount: null,
                },
            ],
        },
    ],
]);
