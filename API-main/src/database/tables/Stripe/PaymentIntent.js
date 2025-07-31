import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * The status of a PaymentIntent.
 * @typedef {("canceled"|"created"|"failed"|"processing"|"succeeded")} PaymentIntentStatus
 */

/**
 * Represents a Stripe PaymentIntent.
 * @typedef {Object} PaymentIntentProperties
 * @property {string} ID The Stripe-generated unique identifier for this PaymentIntent.
 * @property {string} Product_ID The ID of the product this payment intent is for.
 * @property {string} User_ID The ID of the user who initiated this payment intent.
 * @property {PaymentIntentStatus} Status The status of this PaymentIntent.
 * @property {number} Amount The price of the product associated with this PaymentIntent.
 * @property {(7|30|90|-1)} Term
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & PaymentIntentProperties} PaymentIntent
 */

/**
 * @type {PaymentIntent}
 */
export let PaymentIntent;

/**
 * Instantiate the `PaymentIntent` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    PaymentIntent = sequelize.define("paymentIntent", {
        ID: {
            type: DataTypes.STRING,
            allowNull: false,
            unique: true,
        },
        User_ID: {
            type: DataTypes.UUID,
            allowNull: false,
        },
        Product_ID: {
            type: DataTypes.UUID,
            allowNull: false,
        },
        Status: {
            type: DataTypes.STRING,
            allowNull: false,
        },
        Amount: {
            type: DataTypes.FLOAT,
            allowNull: false,
        },
        Term: {
            type: DataTypes.FLOAT,
            allowNull: false,
        },
    });
}
