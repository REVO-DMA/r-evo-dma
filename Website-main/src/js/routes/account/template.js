import { attachEvents as attachHeaderEvents, header as getHeader } from "../../templates/header";
import { AccountData, getAccountData } from "../auth/accountDataManager";
import { initialize as initializeLogic } from "./logic";

export async function show(queryParams) {
    if (AccountData == null) return;
    await getAccountData();
    document.getElementById("baseContainer").innerHTML = getTemplate();
    attachHeaderEvents();
    initializeLogic();
}

function getTemplate() {
    return /*html*/ `
        ${getHeader("Account")}

        <!-- Account Page -->
        <div class="row m-0 p-0 col-auto accountContainer">
            <!-- Sidebar -->
            <div class="col-auto p-0 accountContainerSidebar">
                <div class="row m-0">
                    <div class="row m-0 p-0 accountProfilePicture">
                        <div class="membershipLevelBadge membershipLevelBadge_${AccountData.user.PublicUser === true ? "public" : "private"}">${AccountData.user.PublicUser === true ? "Public" : "Private"}</div>
                        <img src="data:image/png;base64,${AccountData.user.Avatar}" class="p-0 accountProfilePicture" />
                    </div>
                    <div class="row m-0 pt-2 pb-2 p-0 accountUsername justify-content-center align-items-center">
                        <div class="col-auto pe-0 uid-text">
                            #${AccountData.user.uid}
                        </div>
                        <div class="col text-center username-text">
                            ${AccountData.user.Username}
                        </div>
                    </div>
                </div>
                <div class="row m-0 accountTabsContainer">
                    <div class="accountTab" id="accountTab_overview">Overview</div>
                    <div class="accountTab" id="accountTab_subscriptions">Subscriptions</div>
                    <div class="accountTab" id="accountTab_keys">Keys</div>
                    <div class="accountTab" id="accountTab_orders">Orders</div>
                    <!-- <div class="accountTab" id="accountTab_referralProgram">Referral Program</div> -->
                    <div class="accountTab" id="accountTab_accountInfo">Account Info</div>
                </div>
            </div>
            <!-- Main -->
            <div class="col accountContainerMain p-0" id="accountContainerMain"></div>
        </div>
    `;
}
