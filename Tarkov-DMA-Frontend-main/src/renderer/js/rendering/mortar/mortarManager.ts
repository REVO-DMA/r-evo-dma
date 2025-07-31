import { Mortar } from "./mortar";
import { MortarArray, Mortar as IPC_Mortar } from "../../../../gen/mortar_pb";

let mortars: { [n: number]: Mortar; } = {};

export function onRaidEnd() {
    // Destroy all mortars
    for (const key in mortars) {
        const grenade = mortars[key];

        if (grenade == null) continue;

        grenade.destroy();
    }

    mortars = {};
}

export function scaleMortars(x: number, y: number) {
    for (const key in mortars) {
        const mortar = mortars[key];

        if (mortar == null) continue;

        mortar.setScale(x, y);
    }
}

export function allocate(ipc_mortar: IPC_Mortar) {
    const mortar = mortars[ipc_mortar.id];

    if (mortar != null) {
        console.log(`Reallocating Mortar: ${mortar.staticData.ID}`);
        mortar.destroy();
    }

    mortars[ipc_mortar.id] = new Mortar(ipc_mortar);
}

export function updateRealtime(realtimeMortars: MortarArray) {
    for (let i = 0; i < realtimeMortars.messages.length; i++) {
        const realtimeMortar = realtimeMortars.messages[i];

        if (realtimeMortar === null) continue;

        const mortar = mortars[realtimeMortar.id];

        if (mortar == null) continue;

        mortar.setPosition(realtimeMortar.position.x, realtimeMortar.position.y);
    }
}

export function destroy(ID: number) {
    const mortar = mortars[ID];

    if (mortar == null) return;

    console.log(`[MORTAR] -> Disposed ${ID}`);

    mortar.destroy();

    delete mortars[ID];
}
