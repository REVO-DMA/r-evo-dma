import { DateTime } from "luxon";
import { v4 as uuidv4 } from "uuid";
import globals from "../../../globals.js";
import { Account } from "../../tables/User/Account.js";
import { Session } from "../../tables/User/Session.js";
import { User } from "../../tables/User/User.js";

/**
 * Create a session for the supplied user.
 * @param {import("../../tables/User/User.js").User} User The User instance.
 */
export async function createSession(User) {
    try {
        /** @type {Session} */
        const session = await Session.create({
            Session_ID: uuidv4(),
        });

        await User.addSession(session);

        return { success: true, message: session.Session_ID };
    } catch (error) {
        // TODO: Add an error tracking system with error #IDs output to the user for easy reference
        console.log("createSession() ERROR: " + error);

        return { success: false, message: ["An unknown error occurred while creating your session, please try again later."] };
    }
}

/**
 * Determine whether or not the specified `Session_ID` is linked to a valid session.
 * @param {string} Session_ID
 */
export async function verifySession(Session_ID) {
    // TODO: when ban logic is implemented, sessions should be destroyed
    // so that rather than having to do many checks here we can just count
    // on sessions being revoked

    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as invalid session verification attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    // Verify the session is not expired
    const expirationTime = DateTime.fromJSDate(session.createdAt).plus({ days: globals.sessionLifetime });
    if (DateTime.local() > expirationTime) {
        // Session has expired
        await deleteSession(Session_ID);

        return { success: false, message: ["Session expired."] };
    }

    return { success: true, message: session };
}

/**
 * Delete the session associated with the given `Session_ID`.
 * @param {string} Session_ID
 */
export async function deleteSession(Session_ID) {
    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as an invalid session deletion attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    // Delete this specific session
    await session.destroy();

    return { success: true, message: ["Session destroyed."] };
}

/**
 * Determine the access level of the User account associated with a specific session instance.
 * @param {Session} Session The Session instance.
 */
export async function getSessionAccessLevel(Session) {
    /** @type {User} */
    const user = await User.findByPk(Session.userId);

    /** @type {Account} */
    const account = await user.getAccount();

    return { success: true, message: account.Type };
}
