import { AppUpdater, CancellationToken } from "electron-updater";
import { APP_DIR, APP_PATH, DEV_MODE, initialize as initializeENV, PUBLIC_BUILD } from "../../renderer/env";
import { BrowserWindow } from "electron";
import { v4 as uuidv4 } from "uuid";
import { MainProcessIpc } from "electron-better-ipc";
import * as asyncFS from "fs/promises";
import { spawn } from "child_process";
import * as path from "path";
import { ReleaseChannel, UpdaterStatus } from "./updaterShared";

export class MainUpdater
{
    private static UpdateURL: string = "https://software-updates.evodma.com";

    private appUpdater: AppUpdater;
    private targetWindow: BrowserWindow;
    private ipc: MainProcessIpc;

    constructor(appUpdater: AppUpdater, targetWindow: BrowserWindow, ipc: MainProcessIpc)
    {
        this.appUpdater = appUpdater;
        this.targetWindow = targetWindow;
        this.ipc = ipc;

        this.SetPreferences();
        this.BindUpdaterEvents();
        this.BindRendererEvents();
    }

    private SetPreferences()
    {
        this.appUpdater.autoRunAppAfterInstall = false;
        this.appUpdater.autoDownload = false;
        this.appUpdater.allowDowngrade = true;
    }

    private BindUpdaterEvents()
    {
        this.appUpdater.on("checking-for-update", () => {
            this.ipc.callRenderer(this.targetWindow, "updater-status", UpdaterStatus.CheckingForUpdate);
        });
        
        this.appUpdater.on("update-available", (info) => {
            this.ipc.callRenderer(this.targetWindow, "updater-status", UpdaterStatus.UpdateAvailable);
        });
        
        this.appUpdater.on("update-not-available", async (info) => {
            this.ipc.callRenderer(this.targetWindow, "updater-status", UpdaterStatus.NoUpdateAvailable);
        });
        
        this.appUpdater.on("error", (err) => {
            this.ipc.callRenderer(this.targetWindow, "updater-error", err);
        });
        
        this.appUpdater.on("download-progress", (progressObj) => {
            this.ipc.callRenderer(this.targetWindow, "updater-download-progress", `${progressObj.percent.toFixed(2)}%`);
        });
        
        this.appUpdater.on("update-downloaded", async (info) => {
            this.ipc.callRenderer(this.targetWindow, "updater-status", UpdaterStatus.UpdateDownloadComplete);
        });
    }

    private BindRendererEvents()
    {
        this.ipc.answerRenderer("set-update-channel", (channel: ReleaseChannel) => {
            this.appUpdater.channel = channel;
        });
        
        this.ipc.answerRenderer("check-for-update", async () => {
            this.SetFeedURL();
            await this.appUpdater.checkForUpdates();
        });
        
        this.ipc.answerRenderer("download-update", async () => {
            await this.appUpdater.downloadUpdate();
        });
        
        this.ipc.answerRenderer("install-update", async () => {
            await initializeENV(true, true);
            const updateHelperPath_external = path.join(APP_DIR, "UpdateHelper.exe");
            let updateHelperPath_internal: string;
            if (DEV_MODE) {
                updateHelperPath_internal = path.join(APP_PATH, "../../bin/UpdateHelper.exe");
            } else {
                updateHelperPath_internal = path.join(APP_PATH, "webpack-output/bin/UpdateHelper.exe");
            }
            await asyncFS.copyFile(
                updateHelperPath_internal,
                updateHelperPath_external
            );
        
            const child = spawn(`\"${updateHelperPath_external}\"`, {
                detached: true,
                shell: true,
                windowsHide: true,
            });
        
            child.unref();
        
            this.appUpdater.quitAndInstall(true, false);
        });
    }

    private static GetUpdateURL(): string
    {
        let url: string;
        if (PUBLIC_BUILD) url = `${MainUpdater.UpdateURL}/eft-dma-radar-toolkit-public`;
        else url = `${MainUpdater.UpdateURL}/eft-dma-radar-toolkit`;

        return url;
    }

    private SetFeedURL()
    {
        this.appUpdater.setFeedURL({
            provider: "generic",
            url: `${MainUpdater.GetUpdateURL()}?cache_buster=${uuidv4()}`,
            useMultipleRangeRequest: true,
            publishAutoUpdate: true,
            timeout: 12000,
        });
    }
}
