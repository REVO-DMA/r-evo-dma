import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a User's system.
 * @typedef {Object} SystemProperties
 * @property {boolean} Active Whether or not this is the currently used system.
 * @property {string} Radar_PC_Hardware_ID The unique identifier for the user's Radar PC.
 * @property {string} Game_PC_Hardware_ID The unique identifier for the user's Game PC.
 * @property {boolean} Radar_PC_Banned Whether or not this HWID is banned.
 * @property {string} Radar_PC_Ban_Reason The reason this HWID was banned.
 * @property {boolean} Game_PC_Banned Whether or not this HWID is banned.
 * @property {string} Game_PC_Ban_Reason The reason this HWID was banned.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & SystemProperties} System
 */

/**
 * @type {System}
 */
export let System;

/**
 * Instantiate the `System` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    System = sequelize.define("system", {
        Active: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
            comment:
                "One account can have multiple systems (PCs). This allows for only one of the systems to actually be usable at a time, effectively creating a HWID lock system.",
        },
        Radar_PC_Hardware_ID: {
            type: DataTypes.STRING,
            unique: true,
        },
        Game_PC_Hardware_ID: {
            type: DataTypes.STRING,
            unique: true,
        },
        Radar_PC_Banned: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Radar_PC_Ban_Reason: {
            type: DataTypes.STRING,
        },
        Game_PC_Banned: {
            type: DataTypes.BOOLEAN,
            defaultValue: false,
        },
        Game_PC_Ban_Reason: {
            type: DataTypes.STRING,
        },
    });
}
