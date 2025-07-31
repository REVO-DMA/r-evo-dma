import { AuthData } from "../auth";
import { send as ipcSend } from "../ipc/manager";
import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import { DateTime } from "luxon";

TimeAgo.addDefaultLocale(en)
const timeAgo = new TimeAgo('en-US')

const membershipExpirationContainerEl = document.getElementById("membershipExpirationContainer");
const membershipExpirationEl = document.getElementById("membershipExpiration");

const mainMenu_reloadRadarEl = document.getElementById("mainMenu_reloadRadar");

export function initialize() {
    UpdateExpirationUI();
    setInterval(UpdateExpirationUI, 10000);

    mainMenu_reloadRadarEl.addEventListener("click", () => {
        ipcSend(37, {});
    });
}

function UpdateExpirationUI() {
    if (AuthData == null) return;

    const expiration = new Date(AuthData.expiration);
    const daysTillExpiration = DateTime.fromJSDate(expiration).diff(DateTime.now(), "days").days;
    const friendlyExpiration = timeAgo.format(expiration);

    // Set the background color & text based on remaining sub days
    if (daysTillExpiration > 1000) {
        membershipExpirationContainerEl.style.backgroundColor = "rgba(251, 255, 0, 0.1)";
        membershipExpirationEl.innerText = `Membership never expires!`;
    } else if (daysTillExpiration >= 2) {
        membershipExpirationContainerEl.style.backgroundColor = "rgba(0, 255, 0, 0.1)";
        membershipExpirationEl.innerText = `Membership expires ${friendlyExpiration}`;
    } else {
        membershipExpirationContainerEl.style.backgroundColor = "rgba(255, 0, 0, 0.1)";
        membershipExpirationEl.innerText = `Membership expires ${friendlyExpiration}`;
    }
}
