import { Session } from "../../tables/User/Session.js";
import { User } from "../../tables/User/User.js";
import { isEligibleForActiveSubDiscount } from "../Product/Purchase.js";

/**
 * Get the details of a user account from it's associated `Session_ID`.
 * @param {string} Session_ID
 */
export async function getAccountDetails(Session_ID) {
    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as invalid session verification attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    /** @type {User} */
    const user = await User.findByPk(session.userId);

    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await user.getAccount();

    const canHaveActiveSubDiscount = await isEligibleForActiveSubDiscount(session);

    // Generate output object
    const data = {
        user: {
            uid: user.id,
            Discord: user.Discord,
            Avatar: user.Avatar.toString("base64"), // Convert to data URL
            Username: user.Username,
            Email: user.Email,
            PublicUser: user.PublicUser,
        },
        account: {
            type: account.Type,
            canInvite: account.Can_Invite,
            canHaveActiveSubDiscount: canHaveActiveSubDiscount
        },
    };

    return { success: true, message: data };
}
