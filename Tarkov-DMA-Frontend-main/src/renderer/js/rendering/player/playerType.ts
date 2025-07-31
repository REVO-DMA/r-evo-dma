import { PlayerType } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/PlayerType"

interface PlayerColorDefinition {
    dot: string;
    border: string;
}

export let PlayerColors = new Map<PlayerType, PlayerColorDefinition>([
    [PlayerType.Default, { dot: "#000000", border: "#000000" }],
    [PlayerType.LocalPlayer, { dot: "#17c8e6", border: "#000000" }],
    [PlayerType.Teammate, { dot: "#17e617", border: "#000000" }],
    [PlayerType.Streamer, { dot: "#9147ff", border: "#000000" }],
    [PlayerType.EnemyPMC, { dot: "#ff0000", border: "#000000" }],
    [PlayerType.PlayerScav, { dot: "#feff00", border: "#000000" }],
    [PlayerType.Scav, { dot: "#ffffff", border: "#000000" }],
    [PlayerType.Boss, { dot: "#e617c8", border: "#000000" }],
    [PlayerType.BossGuard, { dot: "#c975d7", border: "#000000" }],
    [PlayerType.Raider, { dot: "#8d17e6", border: "#000000" }],
    [PlayerType.Rogue, { dot: "#8d17e6", border: "#000000" }],
    [PlayerType.AI_PMC, { dot: "#ff0000", border: "#000000" }],
    [PlayerType.Cultist, { dot: "#e617c8", border: "#e7ff06" }],
    [PlayerType.CultistBoss, { dot: "#e617c8", border: "#ffd900" }],
    [PlayerType.Santa, { dot: "#ffffff", border: "#feff00" }],
    [PlayerType.Event, { dot: "#8d17e6", border: "#000000" }],
    [PlayerType.BTR, { dot: "#ffffff", border: "#feff00" }],
]);

export const FriendlyTypes = [
    PlayerType.LocalPlayer,
    PlayerType.Teammate,
];

export const HostileHumanTypes = [
    PlayerType.Streamer,
    PlayerType.EnemyPMC,
    PlayerType.PlayerScav,
];

export const AITypes = [
    PlayerType.Scav,
    PlayerType.Boss,
    PlayerType.BossGuard,
    PlayerType.Raider,
    PlayerType.Rogue,
    PlayerType.AI_PMC,
    PlayerType.Cultist,
    PlayerType.CultistBoss,
    PlayerType.Santa,
    PlayerType.Event,
    PlayerType.BTR,
];
