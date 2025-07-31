import { DateTime } from "luxon";
import { IPC_DeferredPlayer } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_DeferredPlayer";
import { IPC_PlayerStats } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_PlayerStats";
import { IPC_StaticPlayer } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_StaticPlayer";
import { PIXI_VIEWPORT } from "../index";
import { Group } from "./group";
import { Player, SELF_DESTRUCT_TIMEOUT as PLAYER_SELF_DESTRUCT_TIMEOUT } from "./player";
import { PlayerColors } from "./playerType";
import { clear as clearPlayersTable } from "./table";
import { ObservablePoint } from "pixi.js";
import { RealtimePlayerArray } from "../../../../gen/realtime_player_pb";

export let players: { [n: number]: Player; } = {};

type GroupMembers = [UniqueIDs: number[], Group: Group];

let groupMembers: { [s: string]: GroupMembers; } = {};

export let localPlayer: Player = null;

export function setLocalPlayer(player: Player) {
    localPlayer = player;
}

export function onRaidEnd() {
    // Dispose of all players
    for (const key in players) {
        const player = players[key];

        if (player == null) continue;

        player.destroy();
    }

    players = {};
    groupMembers = {};
    localPlayer = null;
    clearPlayersTable();
}

export function scalePlayers(x: number, y: number) {
    // Scale players
    for (const key in players) {
        const player = players[key];

        if (player == null) continue;

        player.setScale(x, y);
    }

    // Scale group connecting lines
    for (const key in groupMembers) {
        const group = groupMembers[key];

        group[1].setLineWidth(x);
    }
}

export function allocate(staticPlayer: IPC_StaticPlayer) {
    const player = players[staticPlayer.iD];

    if (player != null) {
        console.log(`A player with this ID ("${player.DynamicData.Name}") has already been added. Destroying old player in favor of new one...`);

        player.destroy();
    }
    
    players[staticPlayer.iD] = new Player(staticPlayer);
    
    // Player group stuff
    const GroupID = staticPlayer.groupID;

    // Don't add this player to a group, they are not in one
    if (GroupID == null) return;

    // If this group does not exist, create it
    if (groupMembers[GroupID] == null) {
        const groupColor = PlayerColors.get(staticPlayer.playerType);

        groupMembers[GroupID] = [
            [],
            new Group(groupColor.dot)
        ];

        // Add to UI
        PIXI_VIEWPORT.addChild(groupMembers[GroupID][1]);
    }

    groupMembers[GroupID][0].push(staticPlayer.iD);
}

export function updateRealtime(realtimePlayers: RealtimePlayerArray) {
    for (let i = 0; i < realtimePlayers.messages.length; i++) {
        const realtimePlayer = realtimePlayers.messages[i];

        if (realtimePlayer === null) continue;

        const player = players[realtimePlayer.id];

        if (player == null || player.DynamicData.Status !== 1) continue;

        player.setPosition(realtimePlayer.position.x, realtimePlayer.position.y);
        player.setLookDirection(realtimePlayer.rotation);
    }

    // Update group connections
    for (const key in groupMembers) {
        const group = groupMembers[key];
        const playerIDs = group[0];

        const positions: ObservablePoint<any>[] = [];
        for (let i = 0; i < playerIDs.length; i++) {
            const UniqueID = playerIDs[i];
            const player = players[UniqueID];

            positions.push(player.Container.position);
        }

        group[1].connectPoints(positions);
    }
}

export function updateDeferred(deferredPlayers: IPC_DeferredPlayer[]) {
    const dtNow = DateTime.now();

    for (let i = 0; i < deferredPlayers.length; i++) {
        const deferredPlayer = deferredPlayers[i];

        if (deferredPlayer === null) continue;

        const player = players[deferredPlayer.iD];

        if (player == null || player.DynamicData.Status !== 1) continue;

        player.setHealth(deferredPlayer.healthPercent);
        player.setDistance(deferredPlayer.distance);
        player.setHeight(deferredPlayer.height);
        player.setHands(deferredPlayer.hands);
        player.setValue(deferredPlayer.value);

        player.DynamicData.LastUpdate = dtNow;
    }

    // Check for orphaned players
    for (const key in players) {
        const player = players[key];

        if (player == null) continue;

        if (player.DynamicData.LastUpdate.plus({ seconds: PLAYER_SELF_DESTRUCT_TIMEOUT }) < dtNow) {
            player.destroy();
        }
    }
}

export function updateStats(playerStats: IPC_PlayerStats) {
    const player = players[playerStats.iD];

    if (player == null) return;

    player.setStats(playerStats);
}

// Player status processors
export function setPlayerAlive(ID: number) {
    const player = players[ID];

    if (player == null || player.DynamicData.Status === 1) return;

    player.setAlive();
}

export function setPlayerDead(ID: number) {
    const player = players[ID];

    if (player == null || player.DynamicData.Status === 2) return;

    player.setDead();

    delete players[ID];
}

export function setPlayerExfil(ID: number) {
    const player = players[ID];

    if (player == null || player.DynamicData.Status === 3) return;

    player.setExfil();

    delete players[ID];
}

export function destroyPlayer(ID: number) {
    const player = players[ID];

    if (player == null) return;

    player.destroy();

    delete players[ID];
}

export function removePlayerFromGroup(UniqueID: number, GroupID: string) {
    const group = groupMembers[GroupID];

    if (group == null) return;

    const player = group[0].indexOf(UniqueID);

    if (player !== -1) group[0].splice(player, 1);
}
