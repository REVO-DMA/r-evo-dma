import { PaymentIntent } from "../../tables/Stripe/PaymentIntent.js";

/**
 * Update the status of a PaymentIntent by it's ID (invoked from a webhook).
 * @param {string} ID
 * @param {import("../../tables/Stripe/PaymentIntent.js").PaymentIntentStatus} status
 * @returns
 */
export async function updatePaymentIntentStatus(ID, status) {
    /** @type {PaymentIntent} */
    const paymentIntent = await PaymentIntent.findOne({
        where: { ID: ID },
    });

    if (paymentIntent == null) {
        console.log(`Failed to update null PaymentIntent with ID of: ${ID}`);
        return { success: false };
    }

    await paymentIntent.update({
        Status: status,
    });
}
