import { Stripe } from "stripe";
import { updatePaymentIntentStatus } from "../database/actions/Stripe/PaymentIntent.js";
import { activateSubscription } from "../database/actions/User/Subscription.js";
import { PaymentIntent } from "../database/tables/Stripe/PaymentIntent.js";
import { Subscription } from "../database/tables/User/Subscription.js";
import globals from "../globals.js";

/** @type {Stripe} */
let stripe = null;

export function initialize() {
    stripe = new Stripe(globals.stripe.secretKey, {
        telemetry: false,
    });
}

/**
 * @param {import("../database/tables/User/User.js").User} User
 */
export async function createStripeCustomer(User) {
    console.log(`Creating Stripe customer for user: "${User.Username}" with email: "${User.Email}"...`);

    const stripeCustomer = await stripe.customers.create({
        name: User.Username,
        email: User.Email,
    });

    await User.update({
        StripeID: stripeCustomer.id,
    });

    console.log(`Successfully created a Stripe customer with ID: "${stripeCustomer.id}" for user: "${User.Username}"!`);
}

/**
 * @param {string} StripeID
 */
export async function getStripeCustomer(StripeID) {
    const stripeCustomer = await stripe.customers.retrieve(StripeID);

    return stripeCustomer;
}

/**
 * Create a payment intent.
 * @param {import("../database/tables/User/User.js").User} User
 * @param {number} price
 * @param {(7|30|90|-1)} subscriptionTerm
 * @param {string} subscriptionTermFixed
 * @param {import("../database/tables/Product/Product.js").Product} Product
 */
export async function createPaymentIntent(User, price, subscriptionTerm, subscriptionTermFixed, Product) {
    const remotePaymentIntent = await stripe.paymentIntents.create({
        customer: User.StripeID,
        statement_descriptor: "EVO DMA",
        statement_descriptor_suffix: "EVO DMA",
        description: `Product: ${Product.ID} Term: ${subscriptionTermFixed}`,
        amount: price * 100, // * 100 to convert to cents
        currency: "usd",
        payment_method_configuration: "pmc_1N8DKZIKeJsElTL1qv7qo6pQ",
    });

    /** @type {PaymentIntent} */
    const localPaymentIntent = await PaymentIntent.create({
        ID: remotePaymentIntent.id,
        Product_ID: Product.ID,
        User_ID: User.ID,
        Status: "created",
        Amount: price,
        Term: subscriptionTerm,
    });

    if (!Product.Track_Stock) {
        /** @type {Subscription} */
        const subscription = await Subscription.create({
            Product: Product.ID,
        });

        // Associate the PaymentIntent with the Subscription
        await subscription.addPaymentIntent(localPaymentIntent);

        // Associate the Subscription with the user
        await User.addSubscription(subscription);
    }

    return { clientSecret: remotePaymentIntent.client_secret };
}

/**
 * Handle a Stripe webhook.
 * @param {Buffer} requestBody
 * @param {string|string[]|undefined} stripeSignature
 */
export async function handleWebhookEvent(requestBody, stripeSignature) {
    /** @type {Stripe.Event} */
    let event;

    try {
        event = stripe.webhooks.constructEvent(requestBody, stripeSignature, globals.stripe.webhookSigningSecret);
    } catch (err) {
        console.log(`Caught error while processing Stripe webhook: ${err.message}`);
        return { success: false, message: [err.message] };
    }

    const paymentIntentID = event.data.object.id;

    if (event.type === "payment_intent.canceled") {
        await updatePaymentIntentStatus(paymentIntentID, "canceled");
    } else if (event.type === "payment_intent.created") {
        await updatePaymentIntentStatus(paymentIntentID, "created");
    } else if (event.type === "payment_intent.payment_failed") {
        await updatePaymentIntentStatus(paymentIntentID, "failed");
    } else if (event.type === "payment_intent.processing") {
        await updatePaymentIntentStatus(paymentIntentID, "processing");
    } else if (event.type === "payment_intent.succeeded") {
        await updatePaymentIntentStatus(paymentIntentID, "succeeded");
        await activateSubscription(paymentIntentID);
    } else {
        const message = `handleWebhookEvent(): Unhandled event of type "${event.type}"`;
        console.log(message);
        return { success: false, message: [message] };
    }

    return { success: true, message: ["Handled webhook successfully!"] };
}
