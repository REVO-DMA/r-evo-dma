import { DateTime } from "luxon";
import { Op } from "sequelize";
import { validate as uuidValidate } from "uuid";
import { Product } from "../../tables/Product/Product.js";
import { Session } from "../../tables/User/Session.js";
import { Subscription } from "../../tables/User/Subscription.js";
import { User } from "../../tables/User/User.js";

/**
 * Determines whether or not the product can be used by the given User.
 * @param {string} Session_ID
 * @param {string} Product_ID
 * @param {string} HWID
 */
export async function canUserLaunchProduct(Session_ID, Product_ID, HWID) {
    if (!uuidValidate(Session_ID)) return { success: false, message: ["Malformed Session ID."] };
    if (!uuidValidate(Product_ID)) return { success: false, message: ["Malformed Product ID."] };

    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as invalid session verification attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    /** @type {User} */
    const user = await User.findByPk(session.userId, {
        include: {
            model: Subscription,
        },
    });

    const hwidValid = await checkHWID(user, HWID);
    if (!hwidValid) {
        return { success: false, message: ["Invalid hardware ID."] };
    }

    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await user.getAccount();

    /** @type {Subscription[]} */
    const rawSubscriptions = user.subscriptions;

    if (rawSubscriptions.length === 0) {
        return { success: false, message: ["You have no subscriptions."] };
    }

    let statusConstraint = ["Available", "Unavailable", "Updating"];
    if (account.Access_Private || !user.PublicUser) {
        statusConstraint.push("Private");
    }
    if (account.Type === "administrator") {
        statusConstraint.push("Hidden");
    }

    /** @type {Product} */
    const product = await Product.findOne({
        where: {
            Status: {
                [Op.or]: statusConstraint,
            },
            ID: Product_ID,
        },
    });

    if (product == null) return { success: false, message: ["No matching product found."] };

    let doesHaveActiveSubscriptionStatus = await doesUserHaveActiveSubscriptionForProduct(rawSubscriptions, product);

    // If this fails and this is the public EFT product check if they have an active private sub
    if (!user.PublicUser && !doesHaveActiveSubscriptionStatus.success && Product_ID === "b558f30e-c4cf-4b47-bb30-71e70ba72983") {
        /** @type {Product} */
        const privateProduct = await Product.findOne({
            where: {
                Status: {
                    [Op.or]: statusConstraint,
                },
                ID: "ee728768-bd14-489b-9a7c-69b48324e003",
            },
        });

        doesHaveActiveSubscriptionStatus = await doesUserHaveActiveSubscriptionForProduct(rawSubscriptions, privateProduct);
    }

    return doesHaveActiveSubscriptionStatus;
}

/**
 * @param {Subscription[]} Subscriptions
 * @param {Product} Product
 */
async function doesUserHaveActiveSubscriptionForProduct(Subscriptions, Product) {
    let message = null;

    for (let i = 0; i < Subscriptions.length; i++) {
        const subscription = Subscriptions[i];

        // Skip nullish subscriptions
        if (subscription.Expiration == null) continue;

        const expirationTime = DateTime.fromJSDate(subscription.Expiration);
        // Check if this subscription is expired
        if (DateTime.local() > expirationTime) {
            // Deactivate subscription if it is active
            if (subscription.Activated) {
                await subscription.update({
                    Activated: false,
                });
            }

            // If this subscription's product ID matches the supplied product ID, preemptively set a message
            if (subscription.Product === Product.ID) {
                message = { success: false, message: ["Your subscription has expired."] };
            }
        } else {
            // Make sure this subscription's product ID matches the supplied product ID
            if (subscription.Activated && subscription.Product === Product.ID) {
                message = {
                    success: true,
                    message: {
                        Expiration: subscription.Expiration,
                        Runtime: DateTime.fromJSDate(subscription.Expiration).diff(DateTime.now(), "seconds").seconds
                    },
                };

                break;
            }
        }
    }

    if (message === null) {
        return { success: false, message: ["You do not have an active subscription for this product."] };
    }

    return message;
}

/**
 * @param {User} user
 * @param {string} hwid
 */
async function checkHWID(user, hwid) {
    if (hwid == null || hwid.length < 4) {
        return false;
    }

    if (user.HWID == null || user.HWID.length === 0) { // Accept any hwid if null or empty
        await user.update({
            HWID: hwid,
        });

        return true;
    } else {
        if (user.HWID === hwid) {
            return true;
        } else {
            return false;
        }
    }
}
