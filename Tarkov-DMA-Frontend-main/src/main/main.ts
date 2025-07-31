import { autoUpdater } from "electron-updater";
import { app, BrowserWindow } from "electron";
import { ipcMain as ipc } from "electron-better-ipc";
import { titlebarProcessBridge } from "../../electron-title-bar/src/main";
import { MainUpdater } from "./updater/updater";
import * as path from "path";

let updater: MainUpdater = null;

// Only allow one instance to run
if (!app.requestSingleInstanceLock()) {
    app.quit();
}

require("@electron/remote/main").initialize();

let devMode: boolean;

if (!app.isPackaged) {
    devMode = true;
} else {
    devMode = false;
}

// Disallow any args in prod
if (!devMode) {
    const args = process.argv;

    if (args.length > 1) {
        app.exit(1);
    }
}

let radarWindow: BrowserWindow;

function createRadarWindow() {
    radarWindow = new BrowserWindow({
        show: false,
        title: "EVO DMA - Tarkov",
        backgroundColor: "#212121",
        frame: false,
        width: 1280,
        height: 720,
        minWidth: 1280,
        minHeight: 720,
        webPreferences: {
            sandbox: false,
            backgroundThrottling: false,
            webSecurity: false,
            spellcheck: false,
            nodeIntegration: true,
            nodeIntegrationInWorker: true,
            contextIsolation: false,
            enableWebSQL: false,
            devTools: devMode,
        },
    });

    titlebarProcessBridge(radarWindow);

    require("@electron/remote/main").enable(radarWindow.webContents);

    if (devMode) {
        radarWindow.loadURL("http://127.0.0.1:8080");
    } else {
        radarWindow.loadFile(path.join(__dirname, "../renderer/index.html"));
    }

    // Create updater
    updater = new MainUpdater(autoUpdater, radarWindow, ipc);

    radarWindow.on("ready-to-show", () => {
        if (devMode) {
            radarWindow.webContents.openDevTools({
                mode: "undocked",
                activate: false,
            });

            radarWindow.showInactive();
        } else {
            radarWindow.show();
            radarWindow.maximize();
        }
    });

    radarWindow.on("closed", () => {
        app.quit();
    });
}

app.on("second-instance", (event, commandLine, workingDirectory, additionalData) => {
    if (radarWindow) {
        if (radarWindow.isMinimized()) {
            radarWindow.restore();
        }
        radarWindow.focus();
    }
});

app.whenReady().then(() => {
    createRadarWindow();
});