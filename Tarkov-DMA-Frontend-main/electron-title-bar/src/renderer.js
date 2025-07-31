import "./css/index.css";

import { EventEmitter } from "events";
import { ipcRenderer as ipc } from "electron-better-ipc";
import { closeSpinnerTemplate, HTML as titlebarHTML, maximizeTemplate, restoreTemplate } from "./titleBar";

/**
 * @typedef {Object} Config
 * @property {boolean} minimizable Indicates if the window can be minimized.
 * @property {boolean} maximizable Indicates if the window can be maximized.
 * @property {boolean} closable Indicates if the window can be closed.
 * @property {string} title The title of the window.
 */

export async function injectTitlebar() {
    /** @type {Config} */
    const titlebarConfig = await ipc.callMain("titlebar_config", null);

    // Inject the titlebar
    const body = document.getElementsByTagName("body")[0];
    body.insertAdjacentHTML("afterbegin", titlebarHTML(titlebarConfig.title, titlebarConfig.minimizable, titlebarConfig.maximizable, titlebarConfig.closable));

    // Attach events
    const event = attachEvents(titlebarConfig);

    return event;
}

/**
 * @param {Config} titlebarConfig
 * @returns {EventEmitter}
 */
function attachEvents(titlebarConfig) {
    const event = new EventEmitter();

    /** @type {HTMLDivElement} */
    let windowMinimize;
    /** @type {HTMLDivElement} */
    let windowMaximizeRestore;
    /** @type {HTMLDivElement} */
    let windowClose;

    const syncMaximizeRestoreIcon = async () => {
        if (await ipc.callMain("titlebar_isMaximized", null)) windowMaximizeRestore.innerHTML = restoreTemplate();
        else windowMaximizeRestore.innerHTML = maximizeTemplate();
    }

    if (titlebarConfig.minimizable) {
        windowMinimize = document.getElementById("titlebar_windowMinimize");

        windowMinimize.addEventListener("click", async () => {
            event.emit("minimize");
            await ipc.callMain("titlebar_minimize", null);
        });
    }

    ipc.answerMain("titlebar_blur", async () => {
        event.emit("blur");
    });

    ipc.answerMain("titlebar_focus", async () => {
        event.emit("focus");
    });

    if (titlebarConfig.maximizable) {
        windowMaximizeRestore = document.getElementById("titlebar_windowMaximizeRestore");

        windowMaximizeRestore.addEventListener("click", async () => {
            if (await ipc.callMain("titlebar_isMaximized", null)) await ipc.callMain("titlebar_restore", null);
            else await ipc.callMain("titlebar_maximize", null);
        });

        ipc.answerMain("titlebar_maximize", async () => {
            event.emit("maximize");
            await syncMaximizeRestoreIcon();
        });

        ipc.answerMain("titlebar_restore", async () => {
            event.emit("restore");
            await syncMaximizeRestoreIcon();
        });
    }

    if (titlebarConfig.closable) {
        windowClose = document.getElementById("titlebar_windowClose");

        windowClose.addEventListener("click", async () => {
            windowClose.innerHTML = closeSpinnerTemplate();
            windowClose.classList.add("titlebar_disabledButton");
            event.emit("beforeClose", () => {
                ipc.callMain("titlebar_close", null);
            });
        });
    }

    ipc.answerMain("enter_fullscreen", () => {
        event.emit("enter_fullscreen");
        document.getElementById("electron_titlebar_outer").style.display = "none";
    });

    ipc.answerMain("exit_fullscreen", () => {
        event.emit("exit_fullscreen");
        document.getElementById("electron_titlebar_outer").style.display = "";
    });

    return event;
}
