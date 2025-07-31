import { validate as uuidValidate } from "uuid";
import { validateEmail } from "./email.js";
import { validatePassword } from "./password.js";

/**
 * @param {string?} username
 * @param {string?} email
 * @param {string?} password
 * @param {string?} invite
 */
export function checkRegistrationInfo(username, email, password, invite) {
    const errors = [];

    // Ensure all data is non-null
    if (username == null || username === "") errors.push("The Username is empty.");
    if (email == null || email === "") errors.push("The Email is empty.");
    if (password == null || password === "") errors.push("The Password is empty.");

    // Terminate early if there is empty data
    if (errors.length > 0) return { success: false, message: errors };

    // Username
    // Disallow certain usernames
    // TODO: Add some better means of storing disallowed names
    if (username === "meta") {
        return { success: false, message: ["The entered username is not allowed."] };
    }

    // Only allow alphanumeric and spaces
    const regEx = new RegExp(/^[a-zA-Z0-9 ]*$/);
    if (!regEx.test(username)) {
        errors.push("The entered username contains invalid characters. Only a-z, A-Z, 0-9 and spaces are allowed.");
    }

    // Minimum and maximum length
    if (username.length < 2 || username.length > 18) {
        errors.push("The entered username has an invalid length. It must be between 2 & 18 characters.");
    }

    // Email
    const validateEmailResult = validateEmail(email);

    validateEmailResult.message.forEach((message) => {
        errors.push(message);
    });

    if (!validateEmailResult.success) {
        return { success: false, message: errors };
    }

    // Password
    const validatePasswordResult = validatePassword(password);

    validatePasswordResult.message.forEach((message) => {
        errors.push(message);
    });

    if (!validatePasswordResult.success) {
        return { success: false, message: errors };
    }

    // Invite code
    // Ensure invite code is in a valid format
    if (invite != null && invite.length > 0) {
        if (!uuidValidate(invite)) errors.push("The entered Invite Code is malformed.");
    }

    // Terminate early if there are any errors
    if (errors.length > 0) return { success: false, message: errors };

    return { success: true, message: [] };
}