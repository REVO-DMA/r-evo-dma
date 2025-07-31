import headerLogo from "../../img/favicon.svg";
import { navigate } from "../router";
import { logout } from "../routes/auth/logic";

/**
 * Get the page header template.
 * @param {("Store"|"Account"|"Downloads")} activePage
 */
export function header(activePage) {
    return /*html*/ `
        <div class="appHeader row m-0 pe-0">
            <!-- Header Logo -->
            <div class="col-auto h-100">
                <img src="${headerLogo}" class="headerLogo p-0" />
            </div>
            <!-- Page Title -->
            <div class="col-auto p-0">
                <div class="row m-0 h-100 justify-content-center align-items-center">
                    <div class="col-auto pageTitle">EVO DMA - ${activePage}</div>
                </div>
            </div>
            <!-- Site Navigation -->
            <div class="col p-0">
                <div class="row m-0 h-100 justify-content-end align-items-center">
                    <div class="row col-auto justify-content-end align-items-center m-0 headerNav ${activePage === "Account" ? "headerNavActive" : ""}" id="headerNav_Account">
                        <div class="col-auto headerNavInner"><i class="fa-duotone fa-id-card me-2 buttonIcon"></i>Account</div>
                    </div>
                    <div class="row col-auto justify-content-end align-items-center m-0 headerNav ${activePage === "Store" ? "headerNavActive" : ""}" id="headerNav_Store">
                        <div class="col-auto headerNavInner"><i class="fa-duotone fa-bag-shopping me-2 buttonIcon"></i>Store</div>
                    </div>
                    <div class="row col-auto justify-content-end align-items-center m-0 headerNav ${activePage === "Downloads" ? "headerNavActive" : ""}" id="headerNav_Downloads">
                        <div class="col-auto headerNavInner"><i class="fa-duotone fa-download me-2 buttonIcon"></i>Downloads</div>
                    </div>
                    <div class="row col-auto justify-content-end align-items-center m-0 headerNav ${activePage === "Legal" ? "headerNavActive" : ""}" id="headerNav_Legal">
                        <div class="col-auto headerNavInner"><i class="fa-duotone fa-solid fa-scale-balanced me-2 buttonIcon"></i>Legal</div>
                    </div>
                    <div class="row col-auto justify-content-end align-items-center m-0 headerNav" id="headerNav_Logout">
                        <div class="col-auto headerNavInner"><i class="fa-duotone fa-arrow-right-from-bracket me-2 buttonIcon"></i>Logout</div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

export function attachEvents() {
    const headerNav_StoreEl = document.getElementById("headerNav_Store");
    const headerNav_AccountEl = document.getElementById("headerNav_Account");
    const headerNav_DownloadsEl = document.getElementById("headerNav_Downloads");
    const headerNav_LegalEl = document.getElementById("headerNav_Legal");
    const headerNav_LogoutEl = document.getElementById("headerNav_Logout");

    headerNav_StoreEl.addEventListener("click", () => {
        navigate("/store");
    });

    headerNav_AccountEl.addEventListener("click", () => {
        navigate("/account");
    });

    headerNav_DownloadsEl.addEventListener("click", () => {
        navigate("/downloads");
    });

    headerNav_LegalEl.addEventListener("click", () => {
        navigate("/legal");
    });

    headerNav_LogoutEl.addEventListener("click", () => {
        logout();
    });
}
