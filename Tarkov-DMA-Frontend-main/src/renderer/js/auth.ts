import { setAuthenticationComplete } from "../renderer";
import { AuthStatusType } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/AuthStatusType";
import { IPC_AuthStatus } from "C:/MemoryPackGenerator_TypeScriptOutputDirectory/IPC_AuthStatus";
import { shell } from "@electron/remote";
import { send as ipcSend } from "./ipc/manager";

export let AuthData: IPC_AuthStatus = null;

const authAlertsEl = document.getElementById("authAlerts");

const authLoginEl: HTMLButtonElement = (document.getElementById("authLogin") as HTMLButtonElement);

const authEmailEl: HTMLInputElement = (document.getElementById("authEmail") as HTMLInputElement);
const authPasswordEl: HTMLInputElement = (document.getElementById("authPassword") as HTMLInputElement);

const authPassword_visibilityToggleEl: HTMLDivElement = (document.getElementById("authPassword_visibilityToggle") as HTMLDivElement);

const authShowForgotPasswordEl: HTMLDivElement = (document.getElementById("authShowForgotPassword") as HTMLDivElement);

let loginTimeout: NodeJS.Timeout = null;

export function Initialize()
{
    // Load saved credentials
    if (localStorage.getItem("auth_emailInput") != null || localStorage.getItem("auth_emailInput") != "") {
        authEmailEl.value = localStorage.getItem("auth_emailInput");
    }
    if (localStorage.getItem("auth_passwordInput") != null || localStorage.getItem("auth_passwordInput") != "") {
        authPasswordEl.value = localStorage.getItem("auth_passwordInput");
    }

    // Save auth input
    authEmailEl.addEventListener("keyup", () => {
        localStorage.setItem("auth_emailInput", authEmailEl.value);
    });
    
    authPasswordEl.addEventListener("keyup", () => {
        localStorage.setItem("auth_passwordInput", authPasswordEl.value);
    });

    document.getElementById("loginFormContainer").style.display = "";
    document.getElementById("appContainer").style.display = "none";
    authLoginEl.addEventListener("click", () => {
        authLoginEl.innerText = "Logging in...";
        authLoginEl.classList.add("authLogin_loggingIn");
        authAlertsEl.style.display = "none";

        ipcSend(32, {
            email: authEmailEl.value,
            password: authPasswordEl.value,
        });

        loginTimeout = setTimeout(() => {
            authLoginEl.innerText = "Login";
            authLoginEl.classList.remove("authLogin_loggingIn");

            // No active membership
            authAlertsEl.innerHTML = /*html*/ `
                <div class="col-12 mb-2 authAlert authAlertInfo">Login failed. Please check your connection and try again.</div>
            `;
            authAlertsEl.style.display = "";
        }, 3500);
    });
    
    // Show/Hide Password
    authPassword_visibilityToggleEl.addEventListener("click", () => {
        if (authPasswordEl.type === "password") {
            authPasswordEl.type = "text";
            authPassword_visibilityToggleEl.innerHTML = `<i class="fa-duotone fa-eye"></i>`;
        } else {
            authPasswordEl.type = "password";
            authPassword_visibilityToggleEl.innerHTML = `<i class="fa-duotone fa-eye-slash"></i>`;
        }
    });

    // Open forgot password page in browser
    authShowForgotPasswordEl.addEventListener("click", () => {
        shell.openExternal(`https://evodma.com/auth?action=forgot-password&email=${authEmailEl.value}`);
    });
}

export function ProcessStatus(Status: IPC_AuthStatus)
{
    AuthData = Status;

    if (Status.message !== null)
    {
        // Auth failure, show message
        authAlertsEl.innerHTML = /*html*/ `
            <div class="col-12 mb-2 authAlert authAlertError">${Status.message}</div>
        `;
        authAlertsEl.style.display = "";
    }
    else if (Status.authStatusType === AuthStatusType.CanLaunch)
    {
        // Auth successful

        // Hide errors
        authAlertsEl.style.display = "none";

        // Proceed with app startup
        setAuthenticationComplete();
    }
    else if (Status.authStatusType === AuthStatusType.InvalidCredentials)
    {
        // Auth failure, show message
        authAlertsEl.innerHTML = /*html*/ `
            <div class="col-12 mb-2 authAlert authAlertError">Invalid Email or Password</div>
        `;
        authAlertsEl.style.display = "";
    }
    else if (Status.authStatusType === AuthStatusType.MembershipExpired)
    {
        // No active membership
        authAlertsEl.innerHTML = /*html*/ `
            <div class="col-12 mb-2 authAlert authAlertInfo">No active membership</div>
        `;
        authAlertsEl.style.display = "";
    }

    clearTimeout(loginTimeout);
    authLoginEl.innerText = "Login";
    authLoginEl.classList.remove("authLogin_loggingIn");
}
