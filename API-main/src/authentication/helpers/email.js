import emailValidator from "email-validator";

/**
 * @param {string} email
 */
export function validateEmail(email) {
    const errors = [];

    // Minimum and maximum length
    if (email.length < 3 || email.length > 128) {
        errors.push("The entered email has an invalid length. It must be between 3 & 128 characters.");
        return { success: false, message: errors };
    }

    // Ensure the email is a valid format
    if (!emailValidator.validate(email)) {
        errors.push("The entered email is invalid.");
        return { success: false, message: errors };
    }

    return { success: true, message: errors };
}