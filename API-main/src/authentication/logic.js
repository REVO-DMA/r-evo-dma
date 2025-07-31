import { login as doLogin } from "../database/actions/User/Login.js";
import { resetPassword as doPasswordReset } from "../database/actions/User/PasswordReset.js";
import { register as doRegistration } from "../database/actions/User/Registration.js";
import { deleteSession as doSessionDeletion, verifySession as doSessionValidation } from "../database/actions/User/Session.js";
import { checkLoginInfo } from "./helpers/login.js";
import { checkRegistrationInfo } from "./helpers/registration.js";
import { isSessionDataValid } from "./helpers/session.js";

/**
 * @param {string} username
 * @param {string} email
 * @param {string} password
 * @param {string} invite
 */
export async function register(username, email, password, invite) {
    const isRegistrationInfoValid = checkRegistrationInfo(username, email, password, invite);
    if (!isRegistrationInfoValid.success) return { success: false, message: isRegistrationInfoValid.message };

    const doRegistrationStatus = await doRegistration(username, email, password, invite);
    if (!doRegistrationStatus.success) return { success: false, message: doRegistrationStatus.message };

    return { success: true, message: ["Account created successfully!"] };
}

/**
 * @param {string} email
 * @param {string} password
 */
export async function login(email, password) {
    const isLoginInfoValid = await checkLoginInfo(email, password);
    if (!isLoginInfoValid.success) return { success: false, message: isLoginInfoValid.message };

    const doLoginStatus = await doLogin(email, password);
    if (!doLoginStatus.success) return { success: false, message: doLoginStatus.message };

    return { success: true, message: doLoginStatus.message };
}

/**
 * @param {string} Session_ID
 */
export async function logout(Session_ID) {
    // Ensure the session data is in a valid format
    const dataValid = isSessionDataValid(Session_ID);
    if (!dataValid.success) return { success: false, message: dataValid.message };

    // Try to delete the session
    const doSessionSessionDeletionStatus = await doSessionDeletion(Session_ID);
    if (!doSessionSessionDeletionStatus.success) return { success: false, message: doSessionSessionDeletionStatus.message };

    return { success: true, message: ["Logged out."] };
}

/**
 * @param {string} Session_ID
 */
export async function verifySession(Session_ID) {
    const dataValid = isSessionDataValid(Session_ID);
    if (!dataValid.success) return { success: false, message: dataValid.message };

    const doSessionValidationStatus = await doSessionValidation(Session_ID);
    if (!doSessionValidationStatus.success) return { success: false, message: doSessionValidationStatus.message };

    return { success: true, message: doSessionValidationStatus.message };
}

/**
 * @param {string} email
 */
export async function resetPassword(email) {
    const doPasswordResetStatus = await doPasswordReset(email);
    if (!doPasswordResetStatus.success) return { success: false, message: doPasswordResetStatus.message };

    return { success: true, message: doPasswordResetStatus.message };
}
