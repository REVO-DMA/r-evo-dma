import { DataTypes, Model, Sequelize } from "sequelize";

/**
 * Represents a Product of the application.
 * @typedef {Object} ProductProperties
 * @property {string} ID The unique identifier of this product.
 * @property {string} Name The name of this product.
 * @property {string} Icon The CDN link to the product's small icon.
 * @property {string} Main_Image The CDN link to the product's main image.
 * @property {string[]} Images An array of CDN links to the product's promotional images.
 * @property {string} Description The description (markdown) of this product.
 * @property {string} Features The features (markdown) of this product.
 * @property {string} Blurb The blurb of this product.
 * @property {number} Price_1_Day The 1-day price of this product.
 * @property {number} Price_7_Day The 7-day price of this product.
 * @property {number} Price_15_Day The 15-day price of this product.
 * @property {number} Price_15_Day_Sub The 15-day subbed price of this product.
 * @property {number} Price_30_Day The 30-day price of this product.
 * @property {number} Price_30_Day_Sub The 30-day subbed price of this product.
 * @property {number} Price_90_Day The 90-day price of this product.
 * @property {number} Price_30_Day_Sub The 30-day subbed price of this product.
 * @property {number} Price_Lifetime The lifetime price of this product.
 * @property {number} Max_Slots If Status is Private, this indicates the max slots of the product.
 * @property {number} Used_Slots If Status is Private, this indicates the used slots of the product.
 * @property {("Hidden"|"Available"|"Unavailable"|"Updating"|"Private")} Status The status of this product.
 * @property {boolean} Track_Stock Whether or not this product tracks key stock.
 *
 * @typedef {import("sequelize").ModelStatic<Model<any, any>> & ProductProperties} Product
 */

/**
 * @type {Product}
 */
export let Product;

/**
 * Instantiate the `User` model.
 * @param {Sequelize} sequelize The application's sequelize instance.
 */
export function initialize(sequelize) {
    Product = sequelize.define("product", {
        ID: {
            type: DataTypes.UUID,
            allowNull: false,
            unique: true,
            comment: "The unique identifier of this product.",
        },
        Name: {
            type: DataTypes.STRING,
            allowNull: false,
            comment: "The name of this product.",
        },
        Description: {
            type: DataTypes.TEXT,
            allowNull: false,
            comment: "The description (markdown) of this product.",
        },
        Features: {
            type: DataTypes.TEXT,
            allowNull: false,
            comment: "The features (markdown) of this product.",
        },
        Blurb: {
            type: DataTypes.TEXT,
            allowNull: false,
            comment: "The blurb of this product.",
        },
        Icon: {
            type: DataTypes.STRING,
            allowNull: false,
            comment: "The CDN link to the product's small icon.",
        },
        Main_Image: {
            type: DataTypes.STRING,
            allowNull: false,
            comment: "The CDN link to the product's main image.",
        },
        Images: {
            type: DataTypes.ARRAY(DataTypes.STRING),
            allowNull: false,
            comment: "An array of CDN links to the product's promotional images.",
        },
        Price_1_Day: {
            type: DataTypes.FLOAT,
            allowNull: true,
            comment: "The 1-day price of this product.",
        },
        Price_7_Day: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 7-day price of this product.",
        },
        Price_15_Day: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 15-day price of this product.",
        },
        Price_15_Day_Sub: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 15-day subbed price of this product.",
        },
        Price_30_Day: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 30-day price of this product.",
        },
        Price_30_Day_Sub: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 30-day subbed price of this product.",
        },
        Price_90_Day: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 90-day price of this product.",
        },
        Price_90_Day_Sub: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The 90-day subbed price of this product.",
        },
        Price_Lifetime: {
            type: DataTypes.FLOAT,
            allowNull: false,
            comment: "The lifetime price of this product.",
        },
        Max_Slots: {
            type: DataTypes.INTEGER,
            defaultValue: -1,
            comment: "If Status is Private, this indicates the max slots of the product.",
        },
        Used_Slots: {
            type: DataTypes.INTEGER,
            defaultValue: -1,
            comment: "If Status is Private, this indicates the used slots of the product.",
        },
        Status: {
            type: DataTypes.STRING,
            allowNull: false,
            comment: "The status of this product.",
        },
        Track_Stock: {
            type: DataTypes.BOOLEAN,
            allowNull: false,
            comment: "Whether or not this product tracks key stock.",
        },
    });
}
