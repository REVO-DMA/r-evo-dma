import { DateTime } from "luxon";
import { Product } from "../../tables/Product/Product.js";
import { PaymentIntent } from "../../tables/Stripe/PaymentIntent.js";
import { Session } from "../../tables/User/Session.js";
import { Subscription } from "../../tables/User/Subscription.js";
import { User } from "../../tables/User/User.js";
import { Key } from "../../tables/Keys/Key.js";

/**
 * Activate the subscription linked to a PaymentIntent.
 * @param {string} ID
 */
export async function activateSubscription(ID) {
    /** @type {PaymentIntent} */
    const paymentIntent = await PaymentIntent.findOne({
        where: { ID: ID },
    });

    if (paymentIntent == null) {
        console.log(`Failed to activate subscription linked to null PaymentIntent with ID of: ${ID}`);
        return { success: false };
    }

    /** @type {User} */
    const user = await User.findOne({
        where: { ID: paymentIntent.User_ID },
        include: {
            model: Subscription,
            include: PaymentIntent,
        },
    });
    if (user == null) {
        console.log(`Failed to find user linked to PaymentIntent with ID of: ${ID}`);
        return { success: false };
    }

    if (paymentIntent.subscriptionId == null) {
        /** @type {Key} */
        const key = await Key.findOne({
            where: {
                Product_ID: paymentIntent.Product_ID,
                Term: paymentIntent.Term,
                Claimed: false,
            },
        });
        if (key == null) {
            console.log(`Failed to find key to provision for PaymentIntent with ID of: ${ID}`);
            return { success: false };
        }

        // Activate the key
        await key.update({
            Claimed: true,
        });
        await user.addKey(key);
    } else {
        let expirationDays;
        if (paymentIntent.Term === -1) expirationDays = 18250; // 50 years
        else expirationDays = paymentIntent.Term;

        /** @type {Subscription[]} */
        const Subscriptions = user.subscriptions;
        let extendedCurrentSub = false;
        for (let i = 0; i < Subscriptions.length; i++) {
            const subscription = Subscriptions[i];

            // Skip nullish subscriptions
            if (subscription.Expiration == null) continue;

            const expirationTime = DateTime.fromJSDate(subscription.Expiration);
            const currentTime = DateTime.local();
            if (currentTime <= expirationTime &&
                subscription.Activated &&
                subscription.Product == paymentIntent.Product_ID) {
                // Calculate the expiration date
                const expiration = expirationTime.plus({ days: expirationDays });

                // Activate the subscription
                await subscription.update({
                    Activated: true,
                    Expiration: expiration.toJSDate(),
                });

                extendedCurrentSub = true;
                break;
            }
        }

        /** @type {Subscription} */
        const subscription = await Subscription.findByPk(paymentIntent.subscriptionId);

        if (extendedCurrentSub) {
            await subscription.destroy();
        } else {
            // Calculate the expiration date
            const expiration = DateTime.local().plus({ days: expirationDays });

            // Activate the subscription
            await subscription.update({
                Activated: true,
                Expiration: expiration.toJSDate(),
            });
        }
    }
}

async function getProductFromID(ID) {
    /** @type {Product[]} */
    const products = await Product.findAll();

    let foundProduct = null;

    for (let i = 0; i < products.length; i++) {
        const product = products[i];

        if (product.ID === ID) {
            foundProduct = product;
            break;
        }
    }

    return foundProduct;
};

/**
 * Gets all Subscriptions and PaymentIntents (Orders).
 * @param {string} Session_ID
 */
export async function getPurchases(Session_ID) {
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
        include: [
            {
                model: Subscription,
                include: PaymentIntent,
            },
            {
                model: Key
            }
        ],
    });

    /** @type {Key[]} */
    const rawKeys = user.keys;

    const keys = [];

    for (const key of rawKeys) {
        keys.push({
            Product: (await getProductFromID(key.Product_ID)).Name,
            Key: key.Key,
            Term: key.Term,
            Date: DateTime.fromJSDate(key.updatedAt).toRelative(),
        });
    }

    /** @type {Subscription[]} */
    const rawSubscriptions = user.subscriptions;

    const subscriptions = [];
    const orders = [];

    for (const subscription of rawSubscriptions) {
        if (subscription.Expiration != null) {
            const expiration = DateTime.fromJSDate(subscription.Expiration);
            
            let friendlyExpiration;
            if (expiration.diffNow("days").days > 6000)
                friendlyExpiration = "Never";
            else
                friendlyExpiration = expiration.toRelative({ unit: ["days", "hours", "minutes"] });

            let isActive;
            if (expiration.diffNow("milliseconds").milliseconds < 0)
                isActive = false;
            else
                isActive = subscription.Activated;

            subscriptions.push({
                Product: (await getProductFromID(subscription.Product)).Name,
                Active: isActive,
                Expiration: friendlyExpiration,
            });
        }

        /** @type {PaymentIntent[]} */
        const rawOrders = subscription.paymentIntents;
        rawOrders.forEach((order) => {
            if (order.Status === "created") return;

            orders.push({
                ID: order.ID.split("pi_")[1],
                Status: order.Status,
                Price: order.Amount,
                Term: order.Term,
                Date: DateTime.fromJSDate(order.createdAt).toRelative(),
            });
        });
    }

    return {
        success: true,
        message: {
            subscriptions: subscriptions.reverse(),
            orders: orders.reverse(),
            keys: keys.reverse(),
        },
    };
}
