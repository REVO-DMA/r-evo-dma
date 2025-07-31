import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a User's session.
 * @typedef {Object} SessionProperties
 * @property {string} Session_ID The unique identifier for this session.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & SessionProperties} Session
 */

/**
 * @type {Session}
 */
export let Session;

/**
 * Instantiate the `Session` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    Session = sequelize.define("session", {
        Session_ID: {
            type: DataTypes.UUID,
            allowNull: false,
            unique: true,
        },
    });
}
