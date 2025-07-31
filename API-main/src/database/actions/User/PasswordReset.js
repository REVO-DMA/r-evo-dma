import { DateTime } from "luxon";
import { v4 as uuidv4 } from "uuid";
import { passwordReset as sendPasswordResetEmail } from "../../../emails/passwordReset.js";
import { PasswordReset } from "../../tables/User/PasswordReset.js";
import { User } from "../../tables/User/User.js";
import { USER_ERROR_MESSAGES } from "./common.js";
import { hashPassword, validatePassword } from "../../../authentication/helpers/password.js";
import { Op } from "sequelize";

/**
 * Generate a password reset token and send an email.
 * @param {string} email
 */
export async function resetPassword(email) {
    /** @type {User} */
    const user = await User.findOne({
        where: {
            Email: {
                [Op.iLike]: `%${email}%`
            }
        },
    });

    // TODO: Log as a possible user enumeration attempt
    if (user == null) {
        return { success: true, message: ["A password reset email has been sent!"] };
    }

    // Create password reset token
    const passwordResetToken = uuidv4();

    // Check if this user already has a password reset association, and if so, update it with a new token
    /** @type {PasswordReset} */
    let passwordReset = await PasswordReset.findOne({
        where: { userId: user.id },
    });

    if (passwordReset) {
        await passwordReset.update({
            Token: passwordResetToken,
        });
    } else {
        // Create password reset entry for this user
        passwordReset = await PasswordReset.create({
            Token: passwordResetToken,
        });

        // Associate the user with the password reset
        await user.setPasswordReset(passwordReset);
    }

    // Lazily send a password reset email
    sendPasswordResetEmail(user, passwordResetToken);

    return { success: true, message: ["A password reset email has been sent!"] };
}

/**
 * Attempt to update a user's password.
 * @param {string} email
 * @param {string} passwordResetToken
 * @param {string} newPassword
 */
export async function updatePassword(email, passwordResetToken, newPassword) {
    const runFail = () => {
        return { success: false, message: [USER_ERROR_MESSAGES.invalid_password_reset_token] };
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
    let passwordReset = user.passwordReset;

    if (passwordReset != null && passwordReset.Token === passwordResetToken) {
        // User has a token & supplied token matches database

        const expirationTime = DateTime.fromJSDate(passwordReset.Timestamp).plus({ hours: 1 });
        if (DateTime.local() > expirationTime) {
            // Token has expired
            await passwordReset.destroy();
            return runFail();
        }

        // Token is valid

        // Make sure supplied password meets requirements
        const validatePasswordResult = validatePassword(newPassword);

        if (!validatePasswordResult.success || validatePasswordResult.message.length > 0) {
            return { success: false, message: validatePasswordResult.message };
        }

        // Hash the supplied password
        const hashedPassword = await hashPassword(newPassword);

        // Update the user's password
        await user.update({
            Password: hashedPassword,
        });

        // Delete the password reset token
        await passwordReset.destroy();
    } else {
        return runFail();
    }

    return { success: true, message: ["Password has been updated successfully!"] };
}