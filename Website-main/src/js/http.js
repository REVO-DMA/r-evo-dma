import { showAlert } from "./alert";
import globals from "./globals";
import { navigate } from "./router";
import { sessionToken, set as setSessionToken } from "./sessionManager";

/**
 * @typedef {object} defaultApiResponse
 * @property {boolean} success Whether or not the API request was executed successfully.
 * @property {string[]} message An array of messages related to the API request.
 */

/**
 * @typedef {object} httpPostOutput
 * @property {XMLHttpRequest} xhr The raw XHR object.
 * @property {defaultApiResponse} response The API response.
 */

/**
 * Perform an HTTP POST request to the given endpoint with the given body.
 * @param {string} endpoint
 * @param {object} body
 * @param {boolean?} authenticated
 * @returns {Promise<httpPostOutput>}
 */
export function httpPost(endpoint, body, authenticated = false) {
    return new Promise((resolve) => {
        try {
            const xhr = new XMLHttpRequest();
            xhr.withCredentials = false;

            xhr.addEventListener("readystatechange", () => {
                if (xhr.readyState === 4) {
                    resolve({ xhr: xhr, response: JSON.parse(xhr.responseText) });
                } else if (xhr.status === 401) {
                    resolve({ xhr: xhr, response: { success: false, message: "Access denied" } });
                    // Clear saved session token, show login form, and show an alert explaining the redirect
                    setSessionToken("");

                    const url = new URL(window.location.href);
                    const currentUrl = url.pathname;
                    if (currentUrl !== "/auth") {
                        navigate("/auth");
                        showAlert("info", "Access Denied", "Invalid session, please login.", false, false, "Dismiss");
                    }
                }
            });

            xhr.addEventListener("timeout", () => {
                // TODO: Show failure alert
            });

            xhr.open("POST", `${globals.apiURL}/${endpoint}`);
            if (authenticated) xhr.setRequestHeader("Session-Token", sessionToken);
            xhr.setRequestHeader("Content-Type", "application/json");

            xhr.send(JSON.stringify(body));

            xhr.timeout = 6000;
        } catch (err) {
            console.error(`httpPost() ERROR: ${err}`);
        }
    });
}
