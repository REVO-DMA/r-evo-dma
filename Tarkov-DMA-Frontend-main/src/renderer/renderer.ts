import "./css/index.sass";

import "@fortawesome/fontawesome-pro/js/brands";
import "@fortawesome/fontawesome-pro/js/duotone";
import "@fortawesome/fontawesome-pro/js/fontawesome";
import "@fortawesome/fontawesome-pro/js/light";
import "@fortawesome/fontawesome-pro/js/regular";
import "@fortawesome/fontawesome-pro/js/solid";

import { injectTitlebar } from "../../electron-title-bar/src/renderer";
import delay from "delay";
import { ipcRenderer as ipc } from "electron-better-ipc";
import { DEV_MODE, initialize as initializeENV } from "./env";
import { initialize as initializeESP_Settings } from "./js/esp/esp";
import { initialize as initializeFeaturesManager } from "./js/features/featuresManager";
import { initialize as initializeBackend, shutdown } from "./js/ipc/backend";
import { initialize as initializeIPC, IPC_READY, send as ipcSend } from "./js/ipc/manager";
import { setMainAppStatus } from "./js/ipc/messageHandlers/radarStatus";
import { initialize as initializeLootFilters } from "./js/lootManager/lootFilter";
import { ALL_ITEMS, initialize as initializeLootManager, syncWithBackend as syncLootFilterState } from "./js/lootManager/lootManager";
import { initialize as initializeMainMenu } from "./js/mainMenu/index";
import { initialize as initializeModals } from "./js/modals";
import { initialize as initializePreferenceManager } from "./js/preferenceManager";
import { initialize as initializeAppSettings } from "./js/settings/appSettings";
import { initialize as initializeTooltips } from "./js/tooltips";
import { Initialize as InitializeFontManager } from "./js/rendering/bitmapFontManager"
import { changeCSSRule } from "./js/utils/misc";
import { Initialize as InitializeAuth } from "./js/auth";
import { RendererUpdater } from "./js/updater";
import { StreamerChecker } from "./js/streamerChecker";

const ytAPI = require("youtube-search-api");

let updater: RendererUpdater;

// Inject titlebar
(async () => {
    const titlebarEvents = await injectTitlebar();
    (document.getElementsByClassName("titlebar_manufacturer")[0] as HTMLElement).innerText = "EVO DMA";
    document.getElementById("titlebar_windowTitle").innerText = "";
    titlebarEvents.on("beforeClose", (callback) => {
        shutdown();
        callback();
    });

    let originalStyles: string = null;
    titlebarEvents.on("enter_fullscreen", () => {
        if (originalStyles === null) {
            originalStyles = changeCSSRule(".appContainer", "");
        }

        changeCSSRule(".appContainer", "margin-top: 0px;");
        changeCSSRule(".appContainer", "height: 100vh;");
    });

    titlebarEvents.on("exit_fullscreen", () => {
        changeCSSRule(".appContainer", originalStyles, true);
    });

    // const results = await ytAPI.GetListByKeyword("drlupo", false, 20, [{type:'video'}]);
    // console.log(results);
})();

let authenticationComplete = false;

export function setAuthenticationComplete() {
    authenticationComplete = true;
}

(async () => {
    await initializeENV();

    let skip_login = false;
    
    // ALWAYS FALSE IN PROD
    if (!DEV_MODE) skip_login = false;

    initializeIPC();
    if (!DEV_MODE) {
        await initializeBackend();

        // Await IPC ready state
        while (true) {
            if (IPC_READY) break;
            await delay(10);
        }

        await delay(100);
    }

    InitializeAuth();

    // Wait for authentication to finish
    while (true && !skip_login) {
        if (authenticationComplete) break;
        await delay(10);
    }
    // Auth complete, show main app
    document.getElementById("appContainer").style.display = "";
    document.getElementById("loginFormContainer").style.display = "none";

    updater = new RendererUpdater(ipc);
    updater.RunAutoUpdate();

    // Try to get fresh loot information
    await initializeLootManager();

    setMainAppStatus("Waiting for backend");

    // Send map configs
    ipcSend(69420, null);

    // Send loot items
    ipcSend(30, ALL_ITEMS);

    InitializeFontManager();
    initializePreferenceManager();
    initializeModals();
    initializeMainMenu();
    await initializeFeaturesManager();
    initializeAppSettings();
    await initializeESP_Settings();
    syncLootFilterState();
    initializeTooltips();

    // Initialize loot filters
    initializeLootFilters();
})();

const loadingDotsEl = document.getElementById("loadingDots");

let currentDots = 0;
setInterval(() => {
    if (currentDots === 0) {
        loadingDotsEl.innerHTML = "&nbsp;";
    } else if (currentDots === 1) {
        loadingDotsEl.innerText = ".";
    } else if (currentDots === 2) {
        loadingDotsEl.innerText = "..";
    } else if (currentDots === 3) {
        loadingDotsEl.innerText = "...";
    } else if (currentDots === 4) {
        loadingDotsEl.innerText = "..";
    } else if (currentDots === 5) {
        loadingDotsEl.innerText = ".";
    } else if (currentDots === 6) {
        loadingDotsEl.innerHTML = "&nbsp;";
        currentDots = 0;
    }

    currentDots++;
}, 1500);
