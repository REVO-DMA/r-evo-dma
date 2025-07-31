import { fadePages } from "./pageHelper";
import { DEV_MODE } from "./env";

export async function skipLogin() {
    if (DEV_MODE) {
        const loginPageContainerEl = document.getElementById("loginPageContainer");
        const productSelectPageContainerEl = document.getElementById("productSelectPageContainer");

        fadePages(loginPageContainerEl, productSelectPageContainerEl);
    }
}
