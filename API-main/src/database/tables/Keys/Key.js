import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a Product of the application.
 * @typedef {Object} KeyProperties
 * @property {string} Product_ID The product ID this key is for.
 * @property {string} Key The key itself.
 * @property {number} Term The amount of time this key is for.
 * @property {boolean} Claimed Whether or not a user has purchased this key.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & KeyProperties} Key
 */

/**
 * @type {Key}
 */
export let Key;

/**
 * Instantiate the `User` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    Key = sequelize.define("key", {
        Product_ID: {
            type: DataTypes.UUID,
            allowNull: false,
            unique: true,
            comment: "The product ID this key is for.",
        },
        Key: {
            type: DataTypes.STRING,
            allowNull: false,
            comment: "The key itself.",
        },
        Term: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The amount of time this key is for.",
        },
        Claimed: {
            type: DataTypes.BOOLEAN,
            allowNull: false,
            comment: "Whether or not a user has purchased this key.",
        }        
    });
}
