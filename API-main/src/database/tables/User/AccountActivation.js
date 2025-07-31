import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a User's account activation instance.
 * @typedef {Object} AccountActivationProperties
 * @property {string?} Token The account activation token.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & AccountActivationProperties} AccountActivation
 */

/**
 * @type {AccountActivation}
 */
export let AccountActivation;

/**
 * Instantiate the `AccountActivation` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    AccountActivation = sequelize.define("accountActivation", {
        Token: {
            type: DataTypes.UUID,
            unique: true,
        },
    });
}
