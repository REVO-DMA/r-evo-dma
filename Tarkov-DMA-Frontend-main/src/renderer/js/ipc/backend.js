import { spawn } from "child_process";
import asyncFS from "fs/promises";
import path from "path";
import { APP_DIR, APP_PATH, DEV_MODE, quitApp } from "../../env";
import { showAlert } from "../alert";

/** @type {import("child_process").ChildProcessWithoutNullStreams} */
let backendProcess = null;

export async function initialize() {
    await prerequisites();
    startConsoleApp();
}

async function prerequisites() {
    /** @type {string} */
    let src;
    /** @type {string} */
    let dest;

    if (DEV_MODE) src = path.join(APP_PATH, "../../../bin/backend.exe");
    else src = path.join(APP_PATH, "webpack-output/bin/backend.exe");

    dest = path.join(APP_DIR, "backend.exe");

    await asyncFS.copyFile(src, dest);
}

function startConsoleApp() {
    backendProcess = spawn(path.join(APP_DIR, "backend.exe"), {
        cwd: APP_DIR,
    });

    backendProcess.stdout.on("data", (data) => {
        console.log("STDOUT: " + data.toString());
    });

    backendProcess.stderr.on("data", (data) => {
        console.error("STDERR: " + data.toString());
    });

    backendProcess.on("exit", (code) => {
        backendProcess = null;
        console.log(`Backend exited with code: "${code}"`);
        shutdown();

        if (!DEV_MODE) {
            showAlert(
                "error",
                "App Error",
                "DMA worker crashed unexpectedly. The app will close in 10 seconds. You may relaunch the app to continue playing.",
                false,
                false,
                "Close",
                "",
                ""
            );

            setTimeout(() => {
                quitApp();
            }, 10000);
        }
    });
}

export function shutdown() {
    if (backendProcess === null) return;

    backendProcess.kill();
}
