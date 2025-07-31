import { Op } from "sequelize";
import { validate as uuidValidate } from "uuid";
import { createPaymentIntent } from "../../../Stripe/Stripe.js";
import { Product } from "../../tables/Product/Product.js";
import { Session } from "../../tables/User/Session.js";
import { User } from "../../tables/User/User.js";
import { DateTime } from "luxon";
import { Subscription } from "../../tables/User/Subscription.js";
import { PaymentIntent } from "../../tables/Stripe/PaymentIntent.js";
import { getStockForProduct } from "./GetProducts.js";

/**
 * Get the price of a product from the database given the product's ID and a subscription term.
 * @param {string} Session_ID
 * @param {string} Product_ID
 * @param {(1|7|15|30|90|-1)} subscriptionTerm
 */
export async function paymentIntentMiddleman(Session_ID, Product_ID, subscriptionTerm) {
    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as a possible session session harvesting attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    /** @type {User} */
    const user = await User.findByPk(session.userId);

    // Validate Product ID
    if (!uuidValidate(Product_ID)) return { success: false, message: ["Invalid Product ID."] };
    // Validate Subscription Term
    if (subscriptionTerm != 1 &&
        subscriptionTerm != 7 &&
        subscriptionTerm != 15 &&
        subscriptionTerm != 30 &&
        subscriptionTerm != 90 &&
        subscriptionTerm != -1) {
        return { success: false, message: ["Invalid Subscription Term."] };
    }

    // Try to get the price
    const getProductStatus = await getProductFromID(user, Product_ID);
    if (!getProductStatus.success) return { success: false, message: getProductStatus.message };

    const product = getProductStatus.product;

    if (product.Track_Stock) {
        const productStock = await getStockForProduct(product);
        const stockInfo = productStock[subscriptionTerm];
        if (stockInfo == null || stockInfo <= 0) {
            return { success: false, message: ["Product temporarily unavailable."] };
        }
    }

    let subscriptionTermFixed = "";

    // If this is a lifetime purchase attempt, make sure this user meets the criteria
    if (subscriptionTerm === -1) {
        if (Product_ID === "ee728768-bd14-489b-9a7c-69b48324e003") { // EFT DMA Private
            const isEligible = await isEligibleForLifetime(session, 90);
            if (!isEligible.success)
                return isEligible;
        } else if (Product_ID === "b558f30e-c4cf-4b47-bb30-71e70ba72983") { // EFT DMA Public
            const isEligible = await isEligibleForLifetime(session, 14);
            if (!isEligible.success)
                return isEligible;
        }
    }

    // Format term name
    if (subscriptionTerm === -1) subscriptionTermFixed = "Lifetime";
    else subscriptionTermFixed = `${subscriptionTerm}_Day`;

    let priceSuffix = "";
    const canHaveActiveSubDiscount = await isEligibleForActiveSubDiscount(session);
    if (product.Track_Stock && canHaveActiveSubDiscount) priceSuffix = "_Sub";

    // Try to get the specified term's price
    const termPrice = product[`Price_${subscriptionTermFixed}${priceSuffix}`];
    if (termPrice == null || termPrice < 0) return { success: false, message: ["Invalid subscription term."] };

    // Create a Stripe payment intent
    const createPaymentIntentStatus = await createPaymentIntent(user, termPrice, subscriptionTerm, subscriptionTermFixed, product);
    return { success: true, message: createPaymentIntentStatus };
}

/**
 * Get the price of a product from the database given the product's ID and a subscription term.
 * @param {User} User
 * @param {string} Product_ID
 */
async function getProductFromID(User, Product_ID) {
    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await User.getAccount();

    // Set up default visible statuses
    let statusConstraint = ["Available", "Unavailable", "Updating"];
    if (account.Access_Private || !User.PublicUser) {
        statusConstraint.push("Private");
    }
    if (account.Type === "administrator") {
        statusConstraint.push("Hidden");
    }
    // Get all products matching the criteria
    /** @type {Product} */
    const product = await Product.findOne({
        where: {
            Status: {
                [Op.or]: statusConstraint,
            },
            ID: Product_ID,
        },
    });

    // Make sure a product was found
    if (product == null) return { success: false, message: ["No product found."] };

    // Try to apply lifetime discount
    if (User.LifetimeDiscount && product.ID === "b558f30e-c4cf-4b47-bb30-71e70ba72983") {
        product.Price_Lifetime -= 50;
    }

    return { success: true, product: product };
}

/**
 * @param {Session} Session
 * @param {number} requiredDays
 */
async function isEligibleForLifetime(Session, requiredDays) {
    const session = Session;

    /** @type {User} */
    const user = await User.findByPk(session.userId, {
        include: {
            model: Subscription,
            include: PaymentIntent,
        },
    });

    /** @type {string[]} */
    const errors = [];

    // Check if they have been a member for at least "requiredDays" days
    const userRegDate = DateTime.fromJSDate(user.createdAt);
    const ninetyDaysFromReg = userRegDate.plus({ days: requiredDays });
    const daysAway = DateTime.local().diff(ninetyDaysFromReg, "days").days;
    if (daysAway < 0) {
        const absDays = Math.abs(daysAway);
        let dayOrDays = "";
        if (absDays > 1)
            dayOrDays = "days";
        else
            dayOrDays = "day";

        errors.push(`Your account does not meet the minimum age requirement. You will be unable to purchase lifetime until your account is ${absDays.toFixed(0)} ${dayOrDays} older.`);
    }

    // Check if they have had an active sub for at least "requiredDays" days
    let totalSubscribedDays = 0;

    /** @type {Subscription[]} */
    const rawSubscriptions = user.subscriptions;

    for (const subscription of rawSubscriptions) {
        /** @type {PaymentIntent[]} */
        const rawOrders = subscription.paymentIntents;
        rawOrders.forEach((order) => {
            if (order.Status === "created") return;

            if (order.Status === "succeeded" && order.Term > 0) {
                totalSubscribedDays += order.Term;
            }
        });
    }

    if (totalSubscribedDays < requiredDays) {
        const absDays = requiredDays - totalSubscribedDays;
        let dayOrDays = "";
        if (absDays > 1)
            dayOrDays = "days";
        else
            dayOrDays = "day";

        errors.push(`You will be unable to purchase lifetime until you have accumulated ${absDays} more ${dayOrDays} of active subscription time.`);
    }

    if (errors.length > 0)
        return { success: false, message: errors };
    else
        return { success: true, message: [] };
}

/**
 * @param {Session} Session
 */
export async function isEligibleForActiveSubDiscount(Session) {
    const remainingSubTime = (await getRemainingSubTime(Session)).message;

    const expiration = DateTime.local().toMillis() + remainingSubTime;
    const days = DateTime.fromMillis(expiration).diffNow('days').days;
    
    if (days >= 2) return true;
    else return false;
}

/**
 * @param {Session} Session
 */
async function getRemainingSubTime(Session) {
    const session = Session;

    /** @type {User} */
    const user = await User.findByPk(session.userId, {
        include: {
            model: Subscription,
            include: PaymentIntent,
        },
    });

    /** @type {Subscription[]} */
    const Subscriptions = user.subscriptions;
    
    let remainingSubTime = 0;

    for (let i = 0; i < Subscriptions.length; i++) {
        const subscription = Subscriptions[i];

        // Skip nullish subscriptions
        if (subscription.Expiration == null) continue;

        const expirationTime = DateTime.fromJSDate(subscription.Expiration);
        const currentTime = DateTime.local();
        if (currentTime <= expirationTime && subscription.Activated) {
            const remainingTime = expirationTime.toMillis() - currentTime.toMillis();
            remainingSubTime += remainingTime;
        }
    }

    return { success: true, message: remainingSubTime };
}
