import { getAccountData } from "./routes/auth/accountDataManager";

/** @type {string} */
export let sessionToken = null;

/**
 * Set/save the session token.
 * @param {string} token
 * @param {boolean?} shouldGetAccountData
 */
export async function set(token, shouldGetAccountData = false) {
    sessionToken = token;
    localStorage.setItem("sessionToken", token);

    if (shouldGetAccountData) await getAccountData();
}

/**
 * Get the saved session token.
 * @param {boolean?} shouldGetAccountData
 */
export async function get(shouldGetAccountData = false) {
    sessionToken = localStorage.getItem("sessionToken");

    if (shouldGetAccountData) await getAccountData();
}
