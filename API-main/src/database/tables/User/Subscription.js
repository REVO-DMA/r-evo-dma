import { DataTypes, Model, Sequelize } from "sequelize";
import { Product } from "../Product/Product.js";
import { PaymentIntent, initialize as initializePaymentIntent } from "../Stripe/PaymentIntent.js";

/**
 * Represents a Subscription.
 * @typedef {Object} SubscriptionProperties
 * @property {string} Product The ID of the product associated with this subscription.
 * @property {boolean} Activated Whether or not this subscription has been activated yet (it is only activated when the payment goes to a status of "succeeded").
 * @property {Date?} Expiration The expiration date of this subscription.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & SubscriptionProperties} Subscription
 */

/**
 * @type {Subscription}
 */
export let Subscription;

/**
 * Instantiate the `Subscription` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    initializePaymentIntent(sequelize);

    Subscription = sequelize.define("subscription", {
        Product: {
            type: DataTypes.UUID,
            references: {
                model: Product,
                key: "ID",
            },
        },
        Activated: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Expiration: {
            type: DataTypes.DATE,
        },
    });

    Subscription.PaymentIntents = Subscription.hasMany(PaymentIntent);
}
