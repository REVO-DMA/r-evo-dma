import syncFS from "fs";
import asyncFS from "fs/promises";
import os from "os";
import path from "path";
import { fileURLToPath } from "url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

export let DEV_MODE: boolean;

export let APP_VERSION: string;

export let APP_PATH: string;

export let APP_HOMEDIR: string;

export let APP_DIR: string;

export async function initialize(thread = false) {
    if (!thread) {
        const remote = await import("@electron/remote");

        if (remote.app.isPackaged) DEV_MODE = false;
        else DEV_MODE = true;

        if (DEV_MODE) APP_VERSION = `${remote.app.getVersion()} DEV`;
        else APP_VERSION = remote.app.getVersion();

        if (DEV_MODE) APP_PATH = __dirname;
        else APP_PATH = remote.app.getAppPath();
    }

    APP_HOMEDIR = path.join(os.homedir(), "EVO DMA");

    // Create the app home dir if it does not exist
    if (!syncFS.existsSync(APP_HOMEDIR)) await asyncFS.mkdir(APP_HOMEDIR);

    APP_DIR = path.join(APP_HOMEDIR, "Launcher");

    // Create the app dir if it does not exist
    if (!syncFS.existsSync(APP_DIR)) await asyncFS.mkdir(APP_DIR);

    // Set app version on UI if this is not a worker thread
    if (!thread) document.getElementById("appVersionText").innerText = APP_VERSION;
}

export function quitApp(thread = false) {
    if (!thread) {
        import("@electron/remote").then((remote) => {
            remote.app.quit();
        });
    }
}
