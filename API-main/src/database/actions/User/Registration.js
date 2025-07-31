import { DateTime } from "luxon";
import { v4 as uuidv4 } from "uuid";
import { createStripeCustomer } from "../../../Stripe/Stripe.js";
import { create as createAvatar } from "../../../canvas/canvas.js";
import { accountActivation as sendAccountActivationEmail } from "../../../emails/accountActivation.js";
import { Account } from "../../tables/User/Account.js";
import { AccountActivation } from "../../tables/User/AccountActivation.js";
import { Invite } from "../../tables/User/Invite.js";
import { User } from "../../tables/User/User.js";
import { extractSignificant } from "../utils.js";
import { USER_ERROR_MESSAGES } from "./common.js";
import { hashPassword } from "../../../authentication/helpers/password.js";
import { Op } from "sequelize";

/**
 * Attempt to create a new user account with the supplied details.
 * @param {string} username
 * @param {string} email
 * @param {string} password
 * @param {string?} invite
 */
export async function register(username, email, password, invite) {
    try {
        const totalUsers = await User.count();
        // Do not require a valid invite code if there are no users yet
        let publicUser = true;
        if (totalUsers > 0) {
            // Determine whether or not the specified invite code is available for use
            if (invite != null && invite.length > 0) {
                const inviteValid = await canInviteCodeBeUsedForRegistration(invite);
                if (inviteValid.success) {
                    publicUser = false;
                } else {
                    return { success: false, message: inviteValid.message };
                }
            }
        }

        // If an account with this email already exists, don't let the user know. (thwart user enumeration)
        /** @type {User} */
        const doesUserExist = await User.findOne({
            where: {
                Email: {
                    [Op.iLike]: `%${email}%`
                }
            },
        });
        // TODO: Log as a possible user enumeration attempt
        if (doesUserExist != null) {
            return { success: true, message: ["Registration successful!"] };
        }

        // Create the default avatar based on the supplied username
        const avatar = createAvatar(extractSignificant(username));

        // Hash the supplied password
        const hashedPassword = await hashPassword(password);

        const accountActivationToken = uuidv4();

        // Attempt to create the user account with the supplied details
        /** @type {User} */
        const user = await User.create({
            ID: uuidv4(),
            Avatar: avatar,
            Username: username,
            Email: email,
            Password: hashedPassword,
            PublicUser: publicUser,

            /** @type {Account} */
            account: {
                Type: "customer",
                Invite_Code: publicUser ? null : invite,
                Activated: true,
            },

            /** @type {AccountActivation} */
            accountActivation: {
                Token: accountActivationToken,
            }
        },
        {
            include: [{ all: true }],
        });

        // Lazily create a stripe account for this user
        createStripeCustomer(user);

        // Lazily send an account activation email
        //sendAccountActivationEmail(user, accountActivationToken);

        return { success: true, message: ["Registration successful!"] };
    } catch (error) {
        // TODO: Add an error tracking system with error #IDs output to the user for easy reference
        console.log("createUser() ERROR: " + error);

        // If non-unique values were supplied, output that information to the user
        if (error.name === "SequelizeUniqueConstraintError") {
            if (error.errors.length != null && error.errors.length > 0) {
                const errors = [];

                error.errors.forEach((error) => {
                    if (error.message === `${error.path} must be unique`) {
                        errors.push(`The entered ${error.path} already exists.`);
                    }
                });

                // TODO:
                // If the only error is due to the entered email already existing, silently fail
                // and pretend the account was created. The incident should be logged.
                return { success: false, message: errors };
            }

            return { success: false, message: ["An unknown error occurred while creating your account, please try again later."] };
        } else {
            return { success: false, message: ["An unknown error occurred while creating your account, please try again later."] };
        }
    }
}

/**
 * Attempt to activate a user's account.
 * @param {string} email
 * @param {string} accountActivationToken
 */
export async function activateAccount(email, accountActivationToken) {
    const runFail = () => {
        return { success: false, message: [USER_ERROR_MESSAGES.invalid_account_activation_token] };
    };

    /** @type {User} */
    const user = await User.findOne({
        where: {
            Email: {
                [Op.iLike]: `%${email}%`
            }
        },
        include: [{ all: true }],
    });

    // TODO: Log as a possible user enumeration attempt
    if (user == null) {
        return runFail();
    }

    // Try to get this user's password reset token
    let accountActivation = user.accountActivation;

    if (accountActivation != null && accountActivation.Token === accountActivationToken) {
        // User has a token & supplied token matches database
        
        const expirationTime = DateTime.fromJSDate(accountActivation.Timestamp).plus({ hours: 1 });
        if (DateTime.local() > expirationTime) {
            // Token has expired
            await accountActivation.destroy();
            return runFail();
        }

        // Token is valid

        // Update the user's account activation status
        await user.account.update({
            Activated: true,
        });

        // Delete the account activation token
        await accountActivation.destroy();
    } else {
        return runFail();
    }

    return { success: true, message: ["Account has been activated successfully!"] };
}

/**
 * Determine whether or not the supplied invite code can be used for registration.
 * @param {string} inviteCode
 */
async function canInviteCodeBeUsedForRegistration(inviteCode) {
    // Make sure this invite exists
    /** @type {Invite} */
    const invite = await Invite.findOne({
        where: { ID: inviteCode },
    });

    if (invite == null) return { success: false, message: ["The entered Invite Code does not exist."] };

    // Make sure the code is unused
    /** @type {Account} */
    const account = await Account.findOne({
        where: { Invite_Code: inviteCode },
    });

    if (account == null) {
        return { success: true, message: [] };
    } else {
        if (account.Invite_Code === inviteCode) return { success: false, message: ["The entered Invite Code has already been used."] };

        return { success: false, message: ["The entered Invite Code is unavailable at this time."] };
    }
}