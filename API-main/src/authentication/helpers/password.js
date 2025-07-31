import argon2 from "argon2";
import { passwordStrength } from "check-password-strength";

/**
 * Check whether or not a given hash/password combo is valid.
 * @param {string} hash The argon2 hash.
 * @param {string} password The password.
 * @returns {Promise<boolean>} Whether or not the hash/password combo was valid.
 */
export async function verifyPassword(hash, password) {
    const verify = await argon2.verify(hash, password);
    return verify;
}

/**
 * Hash a string with argon2.
 * @param {string} password The password to hash.
 * @returns {Promise<string>} The hashed password.
 */
export async function hashPassword(password) {
    const hash = await argon2.hash(password);
    return hash;
}

/**
 * @param {string} password
 */
export function validatePassword(password) {
    const errors = [];

    if (password.length < 8 || password.length > 128) {
        errors.push("The entered password has an invalid length. It must be between 8 & 128 characters.");
        return { success: false, message: errors };
    }

    // Only allow decent and better passwords
    const strength = passwordStrength(password);
    if (strength.id < 2) {
        errors.push(`The chosen password is ${strength.value.toLowerCase()}. Strengthen it by using a combination of lowercase & uppercase letters, symbols, and numbers.`);
        return { success: false, message: errors };
    }

    return { success: true, message: errors };
}