import { DataTypes, Model, Sequelize } from "sequelize";
import { Account, initialize as initializeAccountModel } from "./Account.js";
import { AccountActivation, initialize as initializeAccountActivationModel } from "./AccountActivation.js";
import { Invite, initialize as initializeInviteModel } from "./Invite.js";
import { PasswordReset, initialize as initializePasswordResetModel } from "./PasswordReset.js";
import { Session, initialize as initializeSessionModel } from "./Session.js";
import { Subscription, initialize as initializeSubscriptionModel } from "./Subscription.js";
import { System, initialize as initializeSystemModel } from "./System.js";
import { Key, initialize as initializeKeyModel } from "../Keys/Key.js";

/**
 * Represents a User of the application.
 * @typedef {Object} UserProperties
 * @property {string} ID The unique identifier for the user.
 * @property {string?} Discord_ID The unique identifier for the user's Discord account.
 * @property {string} StripeID The ID of this user on Stripe.
 * @property {Buffer} Avatar The user's profile picture.
 * @property {string} Username The user's username.
 * @property {string} Email The user's email address.
 * @property {string} Password The user's password.
 * @property {boolean} PublicUser Whether or not this is a public user.
 * @property {string} HWID The radar PC hwid.
 * @property {boolean} LifetimeDiscount Whether or not the public cheat lifetime discount is available.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & UserProperties} User
 */

/**
 * @type {User}
 */
export let User;

/**
 * Instantiate the `User` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    initializePasswordResetModel(sequelize);
    initializeInviteModel(sequelize);
    initializeSystemModel(sequelize);
    initializeSessionModel(sequelize);
    initializeAccountModel(sequelize);
    initializeAccountActivationModel(sequelize);
    initializeSubscriptionModel(sequelize);
    initializeKeyModel(sequelize);

    User = sequelize.define("user", {
        ID: {
            type: DataTypes.UUID,
            allowNull: false,
            unique: true,
        },
        Discord: {
            type: DataTypes.STRING,
            unique: true,
        },
        StripeID: {
            type: DataTypes.STRING,
            unique: true,
        },
        Avatar: {
            type: DataTypes.BLOB,
            allowNull: false,
        },
        Username: {
            type: DataTypes.STRING,
            allowNull: false,
            unique: true,
        },
        Email: {
            type: DataTypes.STRING,
            allowNull: false,
            unique: true,
        },
        Password: {
            type: DataTypes.STRING,
            allowNull: false,
        },
        PublicUser: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        HWID: {
            type: DataTypes.STRING,
        },
        LifetimeDiscount: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
    });

    User.PasswordReset = User.hasOne(PasswordReset);
    User.Invites = User.hasMany(Invite);
    User.Systems = User.hasMany(System);
    User.Sessions = User.hasMany(Session);
    User.Account = User.hasOne(Account);
    User.AccountActivation = User.hasOne(AccountActivation);
    User.Subscriptions = User.hasMany(Subscription);
    User.Keys = User.hasMany(Key);
}
