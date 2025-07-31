import { Sequelize } from "sequelize";
import { ENVIRONMENT } from "../globals.js";

/**
 * Handle sequelize model syncing.
 * @param {Sequelize} sequelize
 * @param {boolean?} force
 */
export async function sync(sequelize, force = false) {
    console.log("Synchronizing models with database...");
    if (force) {
        if (ENVIRONMENT === "PROD") {
            console.warn("WARNING! Not force syncing database as we are in PROD environment. Falling back to normal sync.");
        } else {
            await sequelize.sync({ force: true });
        }
    }

    await sequelize.sync();

    console.log("Finished synchronizing models with database");
}
