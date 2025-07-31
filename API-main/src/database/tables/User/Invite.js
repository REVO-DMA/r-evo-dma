import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a User's Invite.
 * @typedef {Object} InviteProperties
 * @property {string} ID The invite code.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & InviteProperties} Invite
 */

/**
 * @type {Invite}
 */
export let Invite;

/**
 * Instantiate the `Invite` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    Invite = sequelize.define("invite", {
        ID: {
            type: DataTypes.UUID,
            allowNull: false,
            unique: true,
        },
    });
}
