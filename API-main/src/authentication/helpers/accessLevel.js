/**
 * Determine the access level of a given account type.
 * @param {string} accountType
 * @returns {number}
 */
export function accessLevelFromType(accountType) {
    if (accountType === "customer") {
        return 1;
    } else if (accountType === "administrator") {
        return 999;
    } else {
        return -1;
    }
}

/**
 * @param {string} accessLevel Format: username_sessionID
 * @param {number} requiredLevel The minimum required access level.
 */
export function doesMeetAccessLevel(accessLevel, requiredLevel) {
    const accessLevelInt = accessLevelFromType(accessLevel);

    if (accessLevelInt >= requiredLevel) {
        return { success: true, message: ["Access granted."] };
    } else {
        return { success: false, message: ["Access denied. Your account is below the minimum access level."] };
    }
}
