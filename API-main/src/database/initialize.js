import { Sequelize } from "sequelize";
import { v4 as uuidv4 } from "uuid";
import globals, { ENVIRONMENT } from "../globals.js";
import { sync } from "./sync.js";
import { initialize as initializeProductModel, Product } from "./tables/Product/Product.js";
import { initialize as initializeUserModel, User } from "./tables/User/User.js";
import { Invite } from "./tables/User/Invite.js";

/** @type {Sequelize} */
let sequelize = null;

/**
 * Initialize sequelize and models.
 */
export async function initialize() {
    sequelize = new Sequelize(globals.database.name, globals.database.username, globals.database.password, {
        host: globals.database.host,
        dialect: "postgres",
        dialectOptions: {
            application_name: `[${ENVIRONMENT}] API`,
        },
        logging: globals.database.logging,
    });

    try {
        await sequelize.authenticate();
        console.log("Database connection has been established successfully!");
    } catch (error) {
        console.error("Unable to connect to the database:", error);
        return;
    }

    console.log("Instantiating models...");
    initializeProductModel(sequelize);
    initializeUserModel(sequelize);
    await sync(sequelize);
    console.log("Database initialized successfully!");

    // const user = await User.findOne({
    //     where: { Username: "Justgoose" },
    // });

    // const invite = await Invite.create({
    //     ID: uuidv4(),
    // });

    // await user.addInvite(invite);
}

export async function shutdown() {
    if (sequelize === null) return;

    await sequelize.close();
}
