import { Op } from "sequelize";
import { verifyPassword } from "../../../authentication/helpers/password.js";
import { User } from "../../tables/User/User.js";
import { isBanned } from "./Ban.js";
import { USER_ERROR_MESSAGES } from "./common.js";
import { createSession } from "./Session.js";

/**
 * Attempt to log the user in with the given credentials.
 * @param {string} email
 * @param {string} password
 */
export async function login(email, password) {
    /** @type {User} */
    const user = await User.findOne({
        where: {
            Email: {
                [Op.iLike]: `%${email}%`
            }
        },
    });

    // TODO: Log as invalid login attempt
    if (user == null) {
        return { success: false, message: [USER_ERROR_MESSAGES.invalid_email_password] };
    }

    // Validate password
    // TODO: Log as invalid login attempt with an existing account
    const passwordValid = await verifyPassword(user.Password, password);
    if (!passwordValid) {
        return { success: false, message: [USER_ERROR_MESSAGES.invalid_email_password] };
    }

    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await user.getAccount();

    // Ensure the account is activated
    if (!account.Activated) {
        return { success: false, message: [USER_ERROR_MESSAGES.invalid_email_password] };
    }

    // Ensure the account is not banned
    const accountBanStatus = await isBanned(user);
    if (accountBanStatus.isBanned) {
        return { success: false, message: [USER_ERROR_MESSAGES.account_banned] };
    }

    // Create session
    const createSessionStatus = await createSession(user);

    if (!createSessionStatus.success) return { success: false, message: createSessionStatus.message };

    return { success: true, message: createSessionStatus.message };
}
