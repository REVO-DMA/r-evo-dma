import { DataTypes, Model, Sequelize } from "sequelize";
import { Invite } from "./Invite.js";

/**
 * Represents a User's account.
 * @typedef {Object} AccountProperties
 * @property {("administrator"|"customer")} Type The type of account.
 * @property {boolean} Access_Private Whether or not this user can access private products.
 * @property {string?} Invite_Code The invite code.
 * @property {boolean} Can_Invite Whether or not this user is a member of the referral program.
 * @property {boolean} Activated Whether or not this account is activated.
 * @property {boolean} Banned Whether or not this account is banned.
 * @property {string} Ban_Reason The reason this account was banned.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & AccountProperties} Account
 */

/**
 * @type {Account}
 */
export let Account;

/**
 * Instantiate the `Account` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    Account = sequelize.define("account", {
        Type: {
            type: DataTypes.STRING,
            allowNull: false,
        },
        Access_Private: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Invite_Code: {
            type: DataTypes.UUID,
            references: {
                model: Invite,
                key: "ID",
            },
        },
        Can_Invite: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Activated: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Banned: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Ban_Reason: {
            type: DataTypes.STRING,
            defaultValue: "",
        },
    });
}
