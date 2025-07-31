import "./css/loader.sass";

import "@fortawesome/fontawesome-pro/js/brands";
import "@fortawesome/fontawesome-pro/js/duotone";
import "@fortawesome/fontawesome-pro/js/fontawesome";
import "@fortawesome/fontawesome-pro/js/light";
import "@fortawesome/fontawesome-pro/js/regular";
import "@fortawesome/fontawesome-pro/js/solid";

import { injectTitlebar } from "@dzltest/electron-title-bar/dist/renderer";
import { changeCSSRule } from "./js/utils";
import { initialize as initializeENV } from "./js/env";
import { Run as RunSplashScreen } from "./js/splashScreen";
import { Log, LogType, initialize as initializeConsole } from "./js/console";
import { initialize as initializeTooltips } from "./js/tooltips";
import { skipLogin } from "./js/devHelper";

// Inject titlebar
(async () => {
    const titlebarEvents = await injectTitlebar();
    document.getElementsByClassName("titlebar_manufacturer")[0].textContent = "EVO DMA";
    document.getElementById("titlebar_windowTitle").innerText = "Launcher";
    titlebarEvents.on("beforeClose", (callback) => {
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
})();

(async () => {
    await initializeENV();

    await RunSplashScreen();

    initializeTooltips();

    initializeConsole();

    skipLogin();
})();
