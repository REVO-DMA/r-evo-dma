import delay from "delay";
import { DEV_MODE } from "./env";
import { fadePages, showPage } from "./pageHelper";

const splashScreen = (document.getElementById("appSplashScreen") as HTMLDivElement);
const splashVideoEl = (document.getElementById("appSplashScreenVideo") as HTMLVideoElement);

const loginPageContainerEl = (document.getElementById("loginPageContainer") as HTMLDivElement);

export async function Run() {
    // Skip the splash screen if app is in dev mode
    if (DEV_MODE) {
        loginPageContainerEl.style.display = "";
        loginPageContainerEl.classList.add("shown");
        return;
    }

    showPage(splashScreen);

    await Play();
    
    await fadePages(splashScreen, loginPageContainerEl);
    splashScreen.remove();
}

async function Play() {
    splashVideoEl.play();
    await delay(7000);
}
