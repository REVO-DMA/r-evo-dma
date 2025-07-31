import http from "http";
import { handleWebhookEvent } from "../Stripe/Stripe.js";
import { accessLevelFromType, doesMeetAccessLevel } from "../authentication/helpers/accessLevel.js";
import { login, logout, register, resetPassword, verifySession } from "../authentication/logic.js";
import { canUserLaunchProduct } from "../database/actions/Product/CheeseGuardian.js";
import { getProductByID, getProducts } from "../database/actions/Product/GetProducts.js";
import { paymentIntentMiddleman } from "../database/actions/Product/Purchase.js";
import { getAccountDetails } from "../database/actions/User/GetAccountDetails.js";
import { updatePassword } from "../database/actions/User/PasswordReset.js";
import { activateAccount } from "../database/actions/User/Registration.js";
import { getSessionAccessLevel } from "../database/actions/User/Session.js";
import { getPurchases } from "../database/actions/User/Subscription.js";
import globals from "../globals.js";
import { isRouteInRoutes, safeJSONParse } from "../utils.js";
import { getDownloads, getDownloadURL } from "./downloads.js";

/** @type {http.Server} */
let HTTP_SERVER = null;

const SERVER_HOST = "127.0.0.1";
const SERVER_PORT = globals.http.port;

const SOFTWARE_ROUTES = ["/radar-login", "/can-launch"];

const AUTH_ROUTES = ["/login", "/register", "/logout", "/forgot-password", "/password-reset", "/activate-account"];

const CUSTOMER_ROUTES = ["/get-account-data", "/get-purchase-data", "/get-products", "/get-product-by-id", "/create-payment-intent", "/get-downloads-list", "/get-download"];

const ADMIN_ROUTES = [""];

const STRIPE_ROUTES = ["/stripe-webhook"];

export function initialize() {
    HTTP_SERVER = http.createServer(requestListener);
    HTTP_SERVER.listen(SERVER_PORT, SERVER_HOST, () => {
        console.log(`Server is running @ http://${SERVER_HOST}:${SERVER_PORT}`);
    });
}

/**
 * @returns {Promise<void>}
 */
export function shutdown() {
    return new Promise((resolve) => {
        if (HTTP_SERVER === null) {
            resolve();
            return;
        }

        HTTP_SERVER.close((err) => {
            if (err) console.error(`ERROR while closing HTTP Server: ${err}`);
            resolve();
        });
    });
}

/**
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function requestListener(req, res) {
    const client_ip = req.headers["cf-connecting-ip"] || req.headers["x-forwarded-for"] || req.socket.remoteAddress; // Get appropriate header for client IP, but prefer cf header
    const route = req.url;

    // Handle preflight requests (CORS)
    if (req.method === "OPTIONS") {
        console.log(`CORS: ${client_ip} | ${route}`);

        res.setHeader("Access-Control-Allow-Origin", "*");
        res.setHeader("Access-Control-Allow-Methods", "*");
        res.setHeader("Access-Control-Allow-Headers", "*");
        res.setHeader("Access-Control-Expose-Headers", "*");
        res.setHeader("Access-Control-Max-Age", "86400");
        res.writeHead(204);
        res.end();
        return;
    }

    // Enforce POST method
    if (req.method !== "POST") {
        sendResponse({ success: false, message: ["Invalid HTTP method."] }, 401, res);
        return;
    }

    // Try to process stripe request (processed out of order because of application/json content type enforcement)
    if (isRouteInRoutes(route, STRIPE_ROUTES)) {
        console.log(`STRIPE: ${client_ip} | ${route}`);

        await processStripeRequest(req, res);
        return;
    }

    // Enforce application/json content type
    if (req.headers["content-type"] !== "application/json") {
        sendResponse({ success: false, message: ["Invalid content type"] }, 401, res);
        return;
    }

    if (isRouteInRoutes(route, SOFTWARE_ROUTES)) {
        console.log(`SOFTWARE: ${client_ip} | ${route}`);

        await processSoftwareRequest(req, res);
    } else if (isRouteInRoutes(route, AUTH_ROUTES)) {
        console.log(`AUTH: ${client_ip} | ${route}`);

        await processAuthRequest(req, res);
    } else if (isRouteInRoutes(route, CUSTOMER_ROUTES)) {
        console.log(`CUSTOMER: ${client_ip} | ${route}`);

        await processCustomerRequest(req, res);
    } else if (isRouteInRoutes(route, ADMIN_ROUTES)) {
        console.log(`ADMIN: ${client_ip} | ${route}`);

        await processAdminRequest(req, res);
    } else {
        console.log(`404: ${client_ip} | ${route}`);

        sendResponse({ success: false, message: ["Route does not exist"] }, 404, res);
    }
}

/**
 * @param {any} data
 * @param {number} httpStatusCode
 * @param {http.ServerResponse} res
 */
function sendResponse(data, httpStatusCode, res) {
    try {
        if (typeof data === "object") {
            data = JSON.stringify(data);
        }
    
        res.setHeader("Content-Type", "application/json");
        res.setHeader("Access-Control-Allow-Origin", "*");
        res.setHeader("Access-Control-Expose-Headers", "*");
    
        res.writeHead(httpStatusCode);
        res.end(data);
    } catch (error) {
        console.log(`Error sending API response: ${error}`);
    }
}

/**
 * @param {any} data
 * @param {number} httpStatusCode
 * @param {http.ServerResponse} res
 */
function sendEncryptedResponse(data, httpStatusCode, res) {
    if (typeof data === "object") {
        data = JSON.stringify(data);
    }

    res.setHeader("Content-Type", "application/json");
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.setHeader("Access-Control-Expose-Headers", "*");

    res.writeHead(httpStatusCode);
    res.end(data);
}

/**
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function isUserAuthenticated(req, res) {
    const sendFail = () => {
        sendResponse({ success: false, message: ["Access denied"] }, 401, res);
    };

    const sessionToken = req.headers["session-token"];

    if (sessionToken == null) {
        sendFail();
        return { isAuthenticated: false, session: null };
    }

    const isSignedIn = await verifySession(sessionToken);
    if (!isSignedIn.success) {
        sendFail();
        return { isAuthenticated: false, session: null };
    }

    return { isAuthenticated: true, session: isSignedIn.message };
}

/**
 * @param {import("../database/tables/User/Session.js").Session} session
 * @param {("customer"|"administrator")} requiredAccessLevelString
 * @param {http.ServerResponse} res
 */
async function canUserAccess(session, requiredAccessLevelString, res) {
    const sendFail = () => {
        sendResponse({ success: false, message: ["Access denied"] }, 401, res);
    };

    // Ensure this session has a high enough access level for this endpoint
    const accessLevel = await getSessionAccessLevel(session);
    const wasAccessLevelMet = doesMeetAccessLevel(accessLevel.message, accessLevelFromType(requiredAccessLevelString));
    if (!wasAccessLevelMet.success) {
        // TODO: Log this event as a specific user attempting to access a restricted area.
        sendFail();
        return { canAccess: false };
    }

    return { canAccess: true };
}

/**
 * @param {http.IncomingMessage} req
 */
function getBody(req) {
    return new Promise(async (resolve) => {
        try {
            const buffer = [];

            for await (const chunk of req) {
                buffer.push(chunk);
            }

            const data = Buffer.concat(buffer).toString();

            const jsonData = safeJSONParse(data);
            if (!jsonData) {
                resolve(false);
            }

            resolve(jsonData);
        } catch (error) {
            console.log(`Error getting request body: ${error}`);
            resolve(false);
        }
    });
}

/**
 * @param {http.IncomingMessage} req
 */
function getEncryptedBody(req) {
    return new Promise(async (resolve) => {
        try {
            const buffer = [];

            for await (const chunk of req) {
                buffer.push(chunk);
            }

            const data = Buffer.concat(buffer).toString();

            const jsonData = safeJSONParse(data);
            if (!jsonData) {
                resolve(false);
            }

            resolve(jsonData);
        } catch (error) {
            console.log(`Error getting encrypted request body: ${error}`);
            resolve(false);
        }
    });
}

/**
 * @param {http.IncomingMessage} req
 * @returns {Promise<Buffer>}
 */
function getRawBody(req) {
    try {
        return new Promise(async (resolve) => {
            const buffer = [];
    
            for await (const chunk of req) {
                buffer.push(chunk);
            }
    
            const data = Buffer.concat(buffer);
    
            resolve(data);
        });
    } catch (error) {
        console.log(`Error getting raw request body: ${error}`);
        resolve(null);
    }
}

/**
 * Handle a request from Stripe.
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function processStripeRequest(req, res) {
    const route = req.url;

    if (route === "/stripe-webhook") {
        const bodyData = await getRawBody(req);

        if (bodyData == null) {
            console.log("Unable to handle stripe webhook request. Body was null!");
            return;
        }

        const stripeSignature = req.headers["stripe-signature"];
        if (stripeSignature == null) {
            sendResponse({ success: false, message: ["Missing signature header"] }, 400, res);
            return;
        }

        const handleWebhookStatus = await handleWebhookEvent(bodyData, stripeSignature);

        if (!handleWebhookStatus.success) {
            sendResponse({ success: false, message: handleWebhookStatus.message }, 400, res);
            return;
        }

        sendResponse({ success: true, message: handleWebhookStatus.message }, 200, res);
    }
}

/**
 * Handle a request for customer level for the radar.
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function processSoftwareRequest(req, res) {
    const route = req.url;

    if (route === "/radar-login") {
        const bodyData = await getEncryptedBody(req);

        if (!bodyData) {
            sendEncryptedResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const loginStatus = await login(bodyData["email"], bodyData["password"]);
        if (!loginStatus.success) {
            sendEncryptedResponse({ success: false, message: loginStatus.message }, 200, res);
            return;
        }

        res.setHeader("session-token", loginStatus.message);

        sendEncryptedResponse({ success: true, message: ["Login successful!"] }, 200, res);

        return;
    }

    // Make sure this is an authenticated request
    const session = await isUserAuthenticated(req, res);
    if (!session.isAuthenticated) return;

    // Make sure this user can access this content
    const wasAccessLevelMet = await canUserAccess(session.session, "customer", res);
    if (!wasAccessLevelMet.canAccess) return;

    const Session_ID = req.headers["session-token"];
    
    if (route === "/can-launch") {
        const bodyData = await getEncryptedBody(req);

        if (!bodyData) {
            sendEncryptedResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const canLaunchStatus = await canUserLaunchProduct(Session_ID, bodyData["productID"], bodyData["hwid"]);

        if (!canLaunchStatus.success) {
            sendEncryptedResponse({ success: false, message: canLaunchStatus.message }, 200, res);
            return;
        }

        sendEncryptedResponse({ success: true, message: canLaunchStatus.message }, 200, res);
    }
}

/**
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function processAuthRequest(req, res) {
    const route = req.url;

    if (route === "/login") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const loginStatus = await login(bodyData["email"], bodyData["password"]);
        if (!loginStatus.success) {
            sendResponse({ success: false, message: loginStatus.message }, 200, res);
            return;
        }

        res.setHeader("session-token", loginStatus.message);

        sendResponse({ success: true, message: ["Login successful!"] }, 200, res);
    } else if (route === "/register") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const registerStatus = await register(bodyData["username"], bodyData["email"], bodyData["password"], bodyData["invite"]);

        if (!registerStatus.success) {
            sendResponse({ success: false, message: registerStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: registerStatus.message }, 200, res);
    } else if (route === "/logout") {
        const session = await isUserAuthenticated(req, res);
        if (!session.isAuthenticated) return;

        const sessionToken = req.headers["session-token"];
        const logoutStatus = await logout(sessionToken);
        if (!logoutStatus.success) {
            sendResponse({ success: false, message: logoutStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: ["You've been logged out."] }, 200, res);
    } else if (route === "/forgot-password") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const resetPasswordStatus = await resetPassword(bodyData["email"]);

        if (!resetPasswordStatus.success) {
            sendResponse({ success: false, message: resetPasswordStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: resetPasswordStatus.message }, 200, res);
    } else if (route === "/password-reset") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const updatePasswordStatus = await updatePassword(bodyData["email"], bodyData["token"], bodyData["password"]);

        if (!updatePasswordStatus.success) {
            sendResponse({ success: false, message: updatePasswordStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: updatePasswordStatus.message }, 200, res);
    } else if (route === "/activate-account") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const activateAccountStatus = await activateAccount(bodyData["email"], bodyData["token"]);

        if (!activateAccountStatus.success) {
            sendResponse({ success: false, message: activateAccountStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: activateAccountStatus.message }, 200, res);
    } else {
        sendResponse({ success: false, message: ["Invalid request!"] }, 404, res);
    }
}

/**
 * Handle a request for customer level data.
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function processCustomerRequest(req, res) {
    const route = req.url;

    // Make sure this is an authenticated request
    const session = await isUserAuthenticated(req, res);
    if (!session.isAuthenticated) return;

    // Make sure this user can access this content
    const wasAccessLevelMet = await canUserAccess(session.session, "customer", res);
    if (!wasAccessLevelMet.canAccess) return;

    const Session_ID = req.headers["session-token"];

    if (route === "/get-account-data") {
        const getAccountDetailsStatus = await getAccountDetails(Session_ID);

        if (!getAccountDetailsStatus.success) {
            sendResponse({ success: false, message: getAccountDetailsStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: getAccountDetailsStatus.message }, 200, res);
    } else if (route === "/get-purchase-data") {
        const getPurchasesStatus = await getPurchases(Session_ID);

        if (!getPurchasesStatus.success) {
            sendResponse({ success: false, message: getPurchasesStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: getPurchasesStatus.message }, 200, res);
    } else if (route === "/get-products") {
        const getProductsStatus = await getProducts(Session_ID);

        if (!getProductsStatus.success) {
            sendResponse({ success: false, message: getProductsStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: getProductsStatus.message }, 200, res);
    } else if (route === "/get-product-by-id") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const getProductByID_Status = await getProductByID(Session_ID, bodyData["productID"]);

        if (!getProductByID_Status.success) {
            sendResponse({ success: false, message: getProductByID_Status.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: getProductByID_Status.message }, 200, res);
    } else if (route === "/create-payment-intent") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const createPaymentIntentStatus = await paymentIntentMiddleman(Session_ID, bodyData["productID"], bodyData["subscriptionTerm"]);

        if (!createPaymentIntentStatus.success) {
            sendResponse({ success: false, message: createPaymentIntentStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: createPaymentIntentStatus.message }, 200, res);
    } else if (route === "/get-downloads-list") {
        const getDownloadsStatus = await getDownloads(Session_ID);

        if (!getDownloadsStatus.success) {
            sendResponse({ success: false, message: getDownloadsStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: getDownloadsStatus.message }, 200, res);
    } else if (route === "/get-download") {
        const bodyData = await getBody(req);

        if (!bodyData) {
            sendResponse({ success: false, message: ["Malformed POST body."] }, 200, res);
            return;
        }

        const getDownloadsURLStatus = getDownloadURL(bodyData["id"]);

        if (!getDownloadsURLStatus.success) {
            sendResponse({ success: false, message: getDownloadsURLStatus.message }, 200, res);
            return;
        }

        sendResponse({ success: true, message: getDownloadsURLStatus.message }, 200, res);
    }
}

/**
 * Handle a request for admin level data.
 * @param {http.IncomingMessage} req
 * @param {http.ServerResponse} res
 */
async function processAdminRequest(req, res) {
    const route = req.url;

    // Make sure this is an authenticated request
    const session = await isUserAuthenticated(req, res);
    if (!session.isAuthenticated) return;

    // Make sure this user can access this content
    const wasAccessLevelMet = await canUserAccess(session.session, "administrator", res);
    if (!wasAccessLevelMet.canAccess) return;

    if (route === "/admin") {
        // Example restricted access endpoint

        sendResponse({ success: true, message: "Secret data." }, 200, res);
    }
}
