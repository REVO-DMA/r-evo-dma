/**
 * Set the ban status of an account.
 * @param {import("../../tables/User/User.js").User} User The User instance.
 * @param {boolean} banned Whether or not the account should be marked as banned.
 * @param {string} banReason The reason the account was banned.
 */
export async function setBan(User, banned, banReason = null) {
    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await User.getAccount();

    await account.update({
        Banned: banned,
        Ban_Reason: banReason,
    });
}

/**
 * Get the ban status of an account.
 * @param {import("../../tables/User/User.js").User} User The User instance.
 */
export async function isBanned(User) {
    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await User.getAccount();

    return { isBanned: account.Banned, banReason: account.Ban_Reason };
}
