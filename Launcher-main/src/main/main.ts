import { app, BrowserWindow } from "electron";
import { join } from "path";
import { titlebarProcessBridge } from "@dzltest/electron-title-bar/dist/main";
import { autoUpdater } from "electron-updater";
import { ipcMain as ipc } from "electron-better-ipc";
import delay from "delay";

require("@electron/remote/main").initialize();

// Only allow one instance to run
if (!app.requestSingleInstanceLock()) {
    app.quit();
}

autoUpdater.autoRunAppAfterInstall = false;

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

let mainWindow: BrowserWindow;

function createMainWindow() {
    mainWindow = new BrowserWindow({
        show: false,
        title: "EVO DMA - Launcher",
        backgroundColor: "#212121",
        frame: false,
        width: 1280,
        height: 800,
        minWidth: 1280,
        minHeight: 800,
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

    titlebarProcessBridge(mainWindow);

    require("@electron/remote/main").enable(mainWindow.webContents);

    if (devMode) {
        mainWindow.loadURL("http://127.0.0.1:8080");
    } else {
        mainWindow.loadFile(join(__dirname, "../renderer/index.html"));
    }

    mainWindow.on("ready-to-show", () => {
        if (devMode) {
            mainWindow.webContents.openDevTools({
                mode: "undocked",
                activate: false,
            });

            mainWindow.showInactive();

            ipc.callRenderer(mainWindow, "auto-updater-finished", null);
        } else {
            mainWindow.show();
            mainWindow.maximize();

            autoUpdater.on("checking-for-update", () => {
                ipc.callRenderer(mainWindow, "auto-updater-status", "Checking For Updates");
            });

            autoUpdater.on("update-available", (info) => {
                ipc.callRenderer(mainWindow, "auto-updater-status", "Downloading Updates");
            });

            autoUpdater.on("update-not-available", async (info) => {
                ipc.callRenderer(mainWindow, "auto-updater-status", "App is up-to-date!");
                await delay(1400);
                ipc.callRenderer(mainWindow, "auto-updater-finished", null);
            });

            autoUpdater.on("error", (err) => {
                ipc.callRenderer(mainWindow, "auto-updater-status", `Error during auto-update: [${err.name}] -> ${err.message}`);
            });

            autoUpdater.on("download-progress", (progressObj) => {
                ipc.callRenderer(mainWindow, "auto-updater-status", `Downloading Update (${progressObj.percent.toFixed(2)}%)`);
            });

            autoUpdater.on("update-downloaded", async (info) => {
                ipc.callRenderer(mainWindow, "auto-updater-status", `Preparing to install updates. Manually start the launcher after the updater closes.`);
                await delay(5000);
                autoUpdater.quitAndInstall();
            });

            autoUpdater.checkForUpdates();
        }
    });

    mainWindow.on("closed", () => {
        app.quit();
    });
}

app.on("second-instance", (event, commandLine, workingDirectory, additionalData) => {
    // Someone tried to run a second instance, focus the running app's window.
    if (mainWindow) {
        if (mainWindow.isMinimized()) {
            mainWindow.restore();
        }
        mainWindow.focus();
    }
});

app.whenReady().then(() => {
    createMainWindow();
});
