import { IPC_StaticGrenade } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_StaticGrenade";
import { Grenade } from "./grenade";
import { RealtimeGrenadeArray } from "../../../../gen/realtime_grenade_pb";

let grenades: { [n: number]: Grenade; } = {};

export function onRaidEnd() {
    // Destroy all grenades
    for (const key in grenades) {
        const grenade = grenades[key];

        if (grenade == null) continue;

        grenade.destroy();
    }

    grenades = {};
}

export function scaleGrenades(x: number, y: number) {
    for (const key in grenades) {
        const grenade = grenades[key];

        if (grenade == null) continue;

        grenade.setScale(x, y);
    }
}

export function allocate(staticGrenade: IPC_StaticGrenade) {
    const grenade = grenades[staticGrenade.iD];

    if (grenade != null) {
        console.log(`Reallocating Grenade: ${grenade.staticData.Name}`);
        grenade.destroy();
    }

    grenades[staticGrenade.iD] = new Grenade(staticGrenade);
}

export function updateRealtime(realtimeGrenades: RealtimeGrenadeArray) {
    for (let i = 0; i < realtimeGrenades.messages.length; i++) {
        const realtimeGrenade = realtimeGrenades.messages[i];

        if (realtimeGrenade === null) continue;

        const grenade = grenades[realtimeGrenade.id];

        if (grenade == null) continue;

        grenade.setPosition(realtimeGrenade.position.x, realtimeGrenade.position.y);
        grenade.setDistance(realtimeGrenade.distance);
    }
}

export function destroy(ID: number) {
    const grenade = grenades[ID];

    if (grenade == null) return;

    grenade.destroy();

    delete grenades[ID];
}
