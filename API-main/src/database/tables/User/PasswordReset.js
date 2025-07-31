import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a User's password reset instance.
 * @typedef {Object} PasswordResetProperties
 * @property {string?} Token The password reset token.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & PasswordResetProperties} PasswordReset
 */

/**
 * @type {PasswordReset}
 */
export let PasswordReset;

/**
 * Instantiate the `PasswordReset` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    PasswordReset = sequelize.define("passwordReset", {
        Token: {
            type: DataTypes.UUID,
            unique: true,
        },
    });
}
