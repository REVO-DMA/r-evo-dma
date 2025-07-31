import { passwordStrength } from "check-password-strength";
import { showAlert } from "../../alert";
import { httpPost } from "../../http";
import { hide as hideLoader, show as showLoader } from "../../loader";
import { navigate } from "../../router";
import { set as setSessionToken } from "../../sessionManager";

// State
let privateRegistrationEnabled = false;

// Elements
/** @type {HTMLDivElement} */
let authFormTitleEl;
/** @type {HTMLDivElement} */
let authAlertsEl;

// Buttons
/** @type {HTMLButtonElement} */
let authResetPasswordEl;
/** @type {HTMLButtonElement} */
let authSendForgotPasswordEmailEl;
/** @type {HTMLButtonElement} */
let authActivateAccountEl;
/** @type {HTMLButtonElement} */
let authShowLoginEl;
/** @type {HTMLButtonElement} */
let authLoginEl;
/** @type {HTMLButtonElement} */
let authShowRegistrationEl;
/** @type {HTMLButtonElement} */
let authRegisterEl;

// Inputs
/** @type {HTMLInputElement} */
let authUsernameEl;
/** @type {HTMLInputElement} */
let authEmailEl;
/** @type {HTMLInputElement} */
let authPasswordResetTokenEl;
/** @type {HTMLInputElement} */
let authAccountActivationTokenEl;
/** @type {HTMLInputElement} */
let authPasswordEl;
/** @type {HTMLInputElement} */
let authNewPasswordEl;
/** @type {HTMLInputElement} */
let authInviteCodeEl;

// Show/Hide Password
/** @type {HTMLDivElement} */
let authPassword_visibilityToggleEl;
/** @type {HTMLDivElement} */
let authNewPassword_visibilityToggleEl;

/** @type {HTMLDivElement} */
let authShowForgotPasswordEl;

// Password Strength
/** @type {HTMLSpanElement} */
let passwordStrengthTextEl;
// Lowercase
/** @type {HTMLDivElement} */
let passwordStrengthCriteria_lowercaseContainerEl;
/** @type {HTMLSpanElement} */
let passwordStrengthCriteria_lowercaseIconEl;
// Uppercase
/** @type {HTMLDivElement} */
let passwordStrengthCriteria_uppercaseContainerEl;
/** @type {HTMLSpanElement} */
let passwordStrengthCriteria_uppercaseIconEl;
// Number
/** @type {HTMLDivElement} */
let passwordStrengthCriteria_numberContainerEl;
/** @type {HTMLSpanElement} */
let passwordStrengthCriteria_numberIconEl;
// Symbol
/** @type {HTMLDivElement} */
let passwordStrengthCriteria_symbolContainerEl;
/** @type {HTMLSpanElement} */
let passwordStrengthCriteria_symbolIconEl;


/**
 * Display auth alerts from a string array.
 * @param {boolean} success
 * @param {string[]} alerts
 */
function alertHandler(success, alerts) {
    /** @type {string} */
    let alertsClass;

    if (success) {
        alertsClass = "authAlertInfo";
    } else {
        alertsClass = "authAlertError";
    }

    authAlertsEl.innerHTML = "";

    alerts.forEach((alert) => {
        authAlertsEl.innerHTML += /*html*/ `
            <div>
                <div class="col-12 mb-2 authAlert ${alertsClass}">${alert}</div>
            </div>
        `;
    });

    if (alerts.length > 0) {
        authAlertsEl.style.display = "";
    } else {
        authAlertsEl.style.display = "none";
        authAlertsEl.innerHTML = "";
    }
}

/**
 * Show/hide all elements with the given class.
 * @param {string} name
 * @param {("show"|"hide")} action
 */
function setRelativesVisibility(name, action) {
    Array.from(document.getElementsByClassName(name)).forEach((el) => {
        if (action === "show") {
            el.style.display = "";
        } else if (action === "hide") {
            el.style.display = "none";
        }
    });
}

export function initialize(queryParams) {
    // Elements
    authFormTitleEl = document.getElementById("authFormTitle");
    authAlertsEl = document.getElementById("authAlerts");

    // Buttons
    authResetPasswordEl = document.getElementById("authResetPassword");
    authSendForgotPasswordEmailEl = document.getElementById("authSendForgotPasswordEmail");
    authActivateAccountEl = document.getElementById("authActivateAccount");
    authShowLoginEl = document.getElementById("authShowLogin");
    authLoginEl = document.getElementById("authLogin");
    authShowRegistrationEl = document.getElementById("authShowRegistration");
    authRegisterEl = document.getElementById("authRegister");

    // Inputs
    authUsernameEl = document.getElementById("authUsername");
    authEmailEl = document.getElementById("authEmail");
    authPasswordResetTokenEl = document.getElementById("authPasswordResetToken");
    authAccountActivationTokenEl = document.getElementById("authAccountActivationToken");
    authPasswordEl = document.getElementById("authPassword");
    authNewPasswordEl = document.getElementById("authNewPassword");
    authInviteCodeEl = document.getElementById("authInviteCode");

    // Show/Hide Password
    authPassword_visibilityToggleEl = document.getElementById("authPassword_visibilityToggle");
    authNewPassword_visibilityToggleEl = document.getElementById("authNewPassword_visibilityToggle");

    // Forgot password link
    authShowForgotPasswordEl = document.getElementById("authShowForgotPassword");

    // Password Strength
    passwordStrengthTextEl = document.getElementById("passwordStrengthText");
    // Lowercase
    passwordStrengthCriteria_lowercaseContainerEl = document.getElementById("passwordStrengthCriteria_lowercaseContainer");
    passwordStrengthCriteria_lowercaseIconEl = document.getElementById("passwordStrengthCriteria_lowercaseIcon");
    // Uppercase
    passwordStrengthCriteria_uppercaseContainerEl = document.getElementById("passwordStrengthCriteria_uppercaseContainer");
    passwordStrengthCriteria_uppercaseIconEl = document.getElementById("passwordStrengthCriteria_uppercaseIcon");
    // Number
    passwordStrengthCriteria_numberContainerEl = document.getElementById("passwordStrengthCriteria_numberContainer");
    passwordStrengthCriteria_numberIconEl = document.getElementById("passwordStrengthCriteria_numberIcon");
    // Symbol
    passwordStrengthCriteria_symbolContainerEl = document.getElementById("passwordStrengthCriteria_symbolContainer");
    passwordStrengthCriteria_symbolIconEl = document.getElementById("passwordStrengthCriteria_symbolIcon");

    // Login
    authShowLoginEl.addEventListener("click", showLogin);
    authLoginEl.addEventListener("click", async () => {
        showLoader("Logging in");
        const result = await httpPost("login", {
            email: authEmailEl.value,
            password: authPasswordEl.value,
        });

        const response = result.response;
        const xhr = result.xhr;

        alertHandler(response.success, response.message);

        if (response.success) {
            // Try to persist the session token
            try {
                const sessionToken = xhr.getResponseHeader("session-token");

                if (sessionToken != null && sessionToken.length > 0) {
                    await setSessionToken(sessionToken, true);
                    navigate("/account");
                }
            } catch (error) {
                console.error(`ERROR saving session token: ${error}`);
            }
        } else {
            hideLoader();
        }
    });

    // Registration
    authShowRegistrationEl.addEventListener("click", showRegistration);
    authRegisterEl.addEventListener("click", async () => {
        const result = await httpPost("register", {
            username: authUsernameEl.value,
            email: authEmailEl.value,
            password: authPasswordEl.value,
            invite: authInviteCodeEl.value,
        });

        const response = result.response;

        alertHandler(response.success, response.message);

        if (response.success) {
            showAlert(
                "success",
                "Registration",
                ///*html*/ `Your account has been created! Please check your inbox at ${authEmailEl.value} for instructions on activating your account.<br>NOTE: It may take 5-10 minutes for the email to arrive.`,
                /*html*/ `Congratulations, your account has been successfully created! You're one step closer to enhancing your EFT gaming experience now.`,
                false,
                false,
                "Show Login",
                "",
                "",
                (result) => {
                    if (result) {
                        showLogin();
                    }
                }
            );
        }
    });

    // Account Activation
    authActivateAccountEl.addEventListener("click", async () => {
        const result = await httpPost("activate-account", {
            email: authEmailEl.value,
            token: authAccountActivationTokenEl.value,
        });

        const response = result.response;

        alertHandler(response.success, response.message);

        if (response.success) {
            showAlert("success", "Account Activation", "Your account has been activated! You may now login.", false, false, "Show Login", "", "", (result) => {
                if (result) {
                    showLogin();
                }
            });
        }
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
    authNewPassword_visibilityToggleEl.addEventListener("click", () => {
        if (authNewPasswordEl.type === "password") {
            authNewPasswordEl.type = "text";
            authNewPassword_visibilityToggleEl.innerHTML = `<i class="fa-duotone fa-eye"></i>`;
        } else {
            authNewPasswordEl.type = "password";
            authNewPassword_visibilityToggleEl.innerHTML = `<i class="fa-duotone fa-eye-slash"></i>`;
        }
    });

    // Forgot Password
    authShowForgotPasswordEl.addEventListener("click", showForgotPassword);
    authSendForgotPasswordEmailEl.addEventListener("click", async () => {
        const result = await httpPost("forgot-password", {
            email: authEmailEl.value,
        });

        const response = result.response;

        alertHandler(response.success, response.message);

        if (response.success) {
        }
    });

    // Password Reset
    authResetPasswordEl.addEventListener("click", async () => {
        const result = await httpPost("password-reset", {
            email: authEmailEl.value,
            token: authPasswordResetTokenEl.value,
            password: authNewPasswordEl.value,
        });

        const response = result.response;

        alertHandler(response.success, response.message);

        if (response.success) {
            showAlert(
                "success",
                "Password Reset",
                "Your password has been reset successfully! You may now login with your new password.",
                false,
                false,
                "Show Login",
                "",
                "",
                (result) => {
                    if (result) {
                        showLogin();
                    }
                }
            );
        }
    });

    // Update password strength on keyup event of password fields
    authPasswordEl.addEventListener("keyup", () => {
        const value = authPasswordEl.value;

        setPasswordStrength(value);
    });

    authNewPasswordEl.addEventListener("keyup", () => {
        const value = authNewPasswordEl.value;

        setPasswordStrength(value);
    });

    const privateUiEnabled = queryParams["privateUiEnabled"];
    if (privateUiEnabled != null && privateUiEnabled == "true") {
        privateRegistrationEnabled = true;
    }

    // Parse query params and show appropriate content
    const urlAction = queryParams["action"];
    if (urlAction != null) {
        if (urlAction === "password-reset") {
            showPasswordReset();

            // Try to populate token
            const token = queryParams["token"];
            if (token != null) {
                authPasswordResetTokenEl.value = token;
            }

            // Try to populate email
            const email = queryParams["email"];
            if (email != null) {
                authEmailEl.value = email;
            }
        } else if (urlAction === "account-activation") {
            showAccountActivation();

            // Try to populate token
            const token = queryParams["token"];
            if (token != null) {
                authAccountActivationTokenEl.value = token;
            }

            // Try to populate email
            const email = queryParams["email"];
            if (email != null) {
                authEmailEl.value = email;
            }
        } else if (urlAction === "forgot-password") {
            showForgotPassword();

            // Try to populate email
            const email = queryParams["email"];
            if (email != null) {
                authEmailEl.value = email;
            }
        }
    } else {
        showLogin();
    }
}

/**
 * @param {string} password
 */
function setPasswordStrength(password) {
    const strength = passwordStrength(password);

    // Set human-readable password strength text
    passwordStrengthTextEl.innerText = strength.value;

    // Clear all classes
    for (let i = 0; i < 4; i++) {
        passwordStrengthTextEl.classList.remove(`passwordStrengthText_${i}`);
    }
    
    // Add current strength class
    passwordStrengthTextEl.classList.add(`passwordStrengthText_${strength.id}`);

    // Lowercase
    if (strength.contains.indexOf("lowercase") !== -1) {
        passwordStrengthCriteria_lowercaseContainerEl.classList.add("passwordStrengthCriteria_met");
        passwordStrengthCriteria_lowercaseContainerEl.classList.remove("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_lowercaseIconEl.innerHTML = `<i class="fa-duotone fa-circle-check inlineTextIcon me-1"></i>`;
    } else {
        passwordStrengthCriteria_lowercaseContainerEl.classList.add("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_lowercaseContainerEl.classList.remove("passwordStrengthCriteria_met");
        passwordStrengthCriteria_lowercaseIconEl.innerHTML = `<i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i>`;
    }

    // Uppercase
    if (strength.contains.indexOf("uppercase") !== -1) {
        passwordStrengthCriteria_uppercaseContainerEl.classList.add("passwordStrengthCriteria_met");
        passwordStrengthCriteria_uppercaseContainerEl.classList.remove("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_uppercaseIconEl.innerHTML = `<i class="fa-duotone fa-circle-check inlineTextIcon me-1"></i>`;
    } else {
        passwordStrengthCriteria_uppercaseContainerEl.classList.add("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_uppercaseContainerEl.classList.remove("passwordStrengthCriteria_met");
        passwordStrengthCriteria_uppercaseIconEl.innerHTML = `<i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i>`;
    }

    // Number
    if (strength.contains.indexOf("number") !== -1) {
        passwordStrengthCriteria_numberContainerEl.classList.add("passwordStrengthCriteria_met");
        passwordStrengthCriteria_numberContainerEl.classList.remove("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_numberIconEl.innerHTML = `<i class="fa-duotone fa-circle-check inlineTextIcon me-1"></i>`;
    } else {
        passwordStrengthCriteria_numberContainerEl.classList.add("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_numberContainerEl.classList.remove("passwordStrengthCriteria_met");
        passwordStrengthCriteria_numberIconEl.innerHTML = `<i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i>`;
    }

    // Symbol
    if (strength.contains.indexOf("symbol") !== -1) {
        passwordStrengthCriteria_symbolContainerEl.classList.add("passwordStrengthCriteria_met");
        passwordStrengthCriteria_symbolContainerEl.classList.remove("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_symbolIconEl.innerHTML = `<i class="fa-duotone fa-circle-check inlineTextIcon me-1"></i>`;
    } else {
        passwordStrengthCriteria_symbolContainerEl.classList.add("passwordStrengthCriteria_notMet");
        passwordStrengthCriteria_symbolContainerEl.classList.remove("passwordStrengthCriteria_met");
        passwordStrengthCriteria_symbolIconEl.innerHTML = `<i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i>`;
    }
}

export async function logout() {
    const result = await httpPost("logout", {}, true);
    const response = result.response;

    if (!response.success) {
        showAlert("error", "Error Logging Out", "An unknown error occurred while attempting to log you out. Please try again later.", false, false, "Dismiss");
    } else {
        setSessionToken("");
        navigate("/auth");
        alertHandler(response.success, response.message);
    }
}

/**
 * Show the login form.
 */
function showLogin() {
    resetUI();

    authFormTitleEl.innerText = "Login";

    // Email
    setRelativesVisibility("authEmail", "show");

    // Password
    setRelativesVisibility("authPassword", "show");
    setRelativesVisibility("authShowForgotPassword", "show");

    // Buttons
    setRelativesVisibility("authLogin", "show");
    setRelativesVisibility("authShowRegister", "show");
}

/**
 * Show the registration form.
 */
function showRegistration() {
    resetUI();

    authFormTitleEl.innerText = "Registration";

    // Username
    setRelativesVisibility("authUsername", "show");

    // Email
    setRelativesVisibility("authEmail", "show");

    // Password
    setRelativesVisibility("authPassword", "show");
    setRelativesVisibility("authPasswordRelated", "show");

    // Invite Code
    if (privateRegistrationEnabled) {
        setRelativesVisibility("authInviteCode", "show");
    }

    // Buttons
    setRelativesVisibility("authRegister", "show");
    setRelativesVisibility("authShowLogin", "show");
}

/**
 * Show the account activation form.
 */
function showAccountActivation() {
    resetUI();

    authFormTitleEl.innerText = "Account Activation";

    // Account Activation Token
    setRelativesVisibility("authAccountActivationToken", "show");

    // Email
    setRelativesVisibility("authEmail", "show");

    // Buttons
    setRelativesVisibility("authActivateAccount", "show");
    setRelativesVisibility("authShowLogin", "show");
}

/**
 * Show the forgot password form.
 */
function showForgotPassword() {
    resetUI();

    authFormTitleEl.innerText = "Forgot Password";

    // Email
    setRelativesVisibility("authEmail", "show");

    // Buttons
    setRelativesVisibility("authSendForgotPasswordEmail", "show");
    setRelativesVisibility("authShowLogin", "show");
}

/**
 * Show password reset form.
 */
function showPasswordReset() {
    resetUI();

    authFormTitleEl.innerText = "Password Reset";

    // Password Reset Token
    setRelativesVisibility("authPasswordResetToken", "show");

    // Email
    setRelativesVisibility("authEmail", "show");

    // New Password
    setRelativesVisibility("authNewPassword", "show");
    setRelativesVisibility("authPasswordRelated", "show");

    // Buttons
    setRelativesVisibility("authResetPassword", "show");
    setRelativesVisibility("authShowLogin", "show");
}

/**
 * Resets the auth form to it's default state.
 */
function resetUI() {
    // Form Title
    authFormTitleEl.innerText = "Loading";

    // Alerts
    authAlertsEl.style.display = "none";
    authAlertsEl.innerHTML = "";

    // Clear sensitive inputs
    authPasswordResetTokenEl.value = "";
    authPasswordEl.value = "";
    authNewPasswordEl.value = "";
    authInviteCodeEl.value = "";

    // Reset password visibility
    // Password
    authPasswordEl.type = "password";
    authPassword_visibilityToggleEl.innerHTML = `<i class="fa-duotone fa-eye-slash"></i>`;
    // New Password
    authNewPasswordEl.type = "password";
    authNewPassword_visibilityToggleEl.innerHTML = `<i class="fa-duotone fa-eye-slash"></i>`;

    // Username
    setRelativesVisibility("authUsername", "hide");

    // Password Reset Token
    setRelativesVisibility("authPasswordResetToken", "hide");

    // Account Activation Token
    setRelativesVisibility("authAccountActivationToken", "hide");

    // Email
    setRelativesVisibility("authEmail", "hide");

    // Password
    setRelativesVisibility("authPassword", "hide");
    setRelativesVisibility("authShowForgotPassword", "hide");
    setRelativesVisibility("authPasswordRelated", "hide");

    // Password Strength
    setPasswordStrength("");

    // New Password
    setRelativesVisibility("authNewPassword", "hide");

    // Invite Code
    setRelativesVisibility("authInviteCode", "hide");

    // Buttons
    // Activate Account
    setRelativesVisibility("authActivateAccount", "hide");
    // Reset Password
    setRelativesVisibility("authResetPassword", "hide");
    // Send Email
    setRelativesVisibility("authSendForgotPasswordEmail", "hide");
    // Login
    setRelativesVisibility("authShowLogin", "hide");
    setRelativesVisibility("authLogin", "hide");
    // Register
    setRelativesVisibility("authShowRegister", "hide");
    setRelativesVisibility("authRegister", "hide");
}