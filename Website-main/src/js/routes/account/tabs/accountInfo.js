import { AccountData } from "../../auth/accountDataManager";

export function getTemplate() {
    return /*html*/ `
        <div class="p-3">
            <div class="row m-0 mb-3 justify-content-center align-items-center">
                <h3 class="text-center">Account Info</h3>
            </div>
            <div class="row m-0 mb-3 justify-content-center align-items-center">
                <div class="col-6">
                    ${getDiscord()}
                </div>
                <div class="col-6">
                    <label for="accountInfo_Username" class="form-label">Username</label>
                    <input type="text" class="form-control" id="accountInfo_Username" value="${AccountData.user.Username}" disabled />
                    <div class="form-text">Open a ticket to change your Username.</div>
                </div>
            </div>
            <div class="row m-0 mb-3 justify-content-center align-items-center">
                <div class="col-6">
                    <label for="accountInfo_Email" class="form-label">Email</label>
                    <input type="text" class="form-control" id="accountInfo_Email" value="${AccountData.user.Email}" disabled />
                    <div class="form-text">Open a ticket to update your Email.</div>
                </div>
                <div class="col-6">
                    <label for="accountInfo_Password" class="form-label">New Password</label>
                    <input type="text" class="form-control" id="accountInfo_Password" />
                    <div class="form-text">Update your Password by entering it here.</div>
                </div>
            </div>
            <div class="row m-0 mt-3 mb-3 justify-content-center align-items-center">
                <div class="col-auto">
                    <span class="ctaLearnMore" id="accountInfo_Save">Save</span>
                </div>
            </div>
        </div>
    `;
}

function getDiscord() {
    const Discord_ID = AccountData.user.Discord;
    let subtext = "Open a ticket to change your Discord.";
    if (Discord_ID == null) {
        subtext = "Set your Discord by entering it here.";
    }

    return /*html*/ `
        <label for="accountInfo_DiscordID" class="form-label">Discord</label>
        <input type="text" class="form-control" id="accountInfo_DiscordID" value="${Discord_ID || ""}" ${Discord_ID == null ? "" : "disabled"} />
        <div class="form-text">${subtext}</div>
    `;
}
