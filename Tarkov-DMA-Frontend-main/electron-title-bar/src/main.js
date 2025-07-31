import { ipcMain as ipc } from "electron-better-ipc";

import { darkenColor } from "./utils";

/** @type {string} */
let originalBackground;

/**
 * @param {import("electron").BrowserWindow} browserWindow
 */
export function titlebarProcessBridge(browserWindow) {
    originalBackground = browserWindow.getBackgroundColor();

    ipc.answerRenderer("titlebar_isMaximized", () => {
        return browserWindow.isMaximized();
    });

    ipc.answerRenderer("titlebar_minimize", () => {
        browserWindow.minimize();
    });

    browserWindow.on("blur", () => {
        ipc.callRenderer(browserWindow, "titlebar_blur", null);

        // Darken the window background on blur
        originalBackground = browserWindow.getBackgroundColor();
        browserWindow.setBackgroundColor(darkenColor(originalBackground, 20));
    });

    browserWindow.on("focus", () => {
        ipc.callRenderer(browserWindow, "titlebar_focus", null);
        browserWindow.setBackgroundColor(originalBackground);
    });

    ipc.answerRenderer("titlebar_maximize", () => {
        browserWindow.maximize();
    });

    ipc.answerRenderer("titlebar_restore", () => {
        browserWindow.unmaximize();
    });

    ipc.answerRenderer("titlebar_close", () => {
        browserWindow.close();
    });

    browserWindow.on("maximize", () => {
        ipc.callRenderer(browserWindow, "titlebar_maximize", null);
    });

    browserWindow.on("unmaximize", () => {
        ipc.callRenderer(browserWindow, "titlebar_restore", null);
    });

    ipc.answerRenderer("titlebar_config", () => {
        const config = {
            minimizable: browserWindow.minimizable,
            maximizable: browserWindow.isMaximizable(),
            closable: browserWindow.closable,
            title: browserWindow.getTitle(),
        };

        return config;
    });

    browserWindow.on("enter-full-screen", () => {
        ipc.callRenderer(browserWindow, "enter_fullscreen", null);
    });

    browserWindow.on("leave-full-screen", () => {
        ipc.callRenderer(browserWindow, "exit_fullscreen", null);
    })
}
