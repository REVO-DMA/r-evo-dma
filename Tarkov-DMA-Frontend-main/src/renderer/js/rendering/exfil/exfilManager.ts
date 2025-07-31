import { IPC_Exfil } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_Exfil";
import { Exfil } from "./exfil";

let exfils: { [n: number]: Exfil; } = {};

export function onRaidEnd() {
    // Destroy all exfils
    for (const key in exfils) {
        const exfil = exfils[key];

        if (exfil == null) continue;

        exfil.destroy();
    }

    exfils = {};
}

export function scaleExfils(x: number, y: number) {
    for (const key in exfils) {
        const exfil = exfils[key];

        if (exfil == null) continue;

        exfil.setScale(x, y);
    }
}

export function update(radarExfils: IPC_Exfil[]) {
    for (let i = 0; i < radarExfils.length; i++) {
        const radarExfil = radarExfils[i];

        const exfil = exfils[radarExfil.iD];

        // If this exfil is not allocated, add it
        if (exfil == null) {
            exfils[radarExfil.iD] = new Exfil(radarExfil);
        } else {
            // This exfil is already allocated, update it's status
            exfil.updateStatus(radarExfil.status);
        }
    }
}
