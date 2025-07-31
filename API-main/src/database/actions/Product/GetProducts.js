import { Op } from "sequelize";
import { Product } from "../../tables/Product/Product.js";
import { Session } from "../../tables/User/Session.js";
import { User } from "../../tables/User/User.js";
import { Key } from "../../tables/Keys/Key.js";

/**
 * Get all products the user has access to.
 * @param {string} Session_ID
 */
export async function getProducts(Session_ID) {
    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as invalid session verification attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    /** @type {User} */
    const user = await User.findByPk(session.userId);
    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await user.getAccount();

    // Set up default visible statuses
    let statusConstraint = ["Available", "Unavailable", "Updating"];
    if (account.Access_Private || !user.PublicUser) {
        statusConstraint.push("Private");
    }
    if (account.Type === "administrator") {
        statusConstraint.push("Hidden");
    }
    // Get all products matching the criteria
    /** @type {Product[]} */
    const products = await Product.findAll({
        where: {
            Status: {
                [Op.or]: statusConstraint,
            },
        },
    });

    const stock = {};

    for (let i = 0; i < products.length; i++) {
        const product = products[i];
        
        stock[product.ID] = await getStockForProduct(product);
        
        if (user.LifetimeDiscount && product.ID === "b558f30e-c4cf-4b47-bb30-71e70ba72983") {
            product.Price_Lifetime -= 50;
        }
    }

    const output = {
        products: products,
        stock: stock
    };

    return { success: true, message: output };
}

/**
 * Get all products the user has access to.
 * @param {string} Session_ID
 * @param {string} Product_ID
 */
export async function getProductByID(Session_ID, Product_ID) {
    /** @type {Session} */
    const session = await Session.findOne({
        where: { Session_ID: Session_ID },
    });

    // TODO: Log as invalid session verification attempt
    if (session == null) {
        return { success: false, message: ["Invalid Session."] };
    }

    /** @type {User} */
    const user = await User.findByPk(session.userId);
    /** @type {import("../../tables/User/Account.js").Account} */
    const account = await user.getAccount();

    // Set up default visible statuses
    let statusConstraint = ["Available", "Unavailable", "Updating"];
    if (account.Access_Private || !user.PublicUser) {
        statusConstraint.push("Private");
    }
    if (account.Type === "administrator") {
        statusConstraint.push("Hidden");
    }

    /** @type {Product} */
    const product = await Product.findOne({
        where: {
            Status: {
                [Op.or]: statusConstraint,
            },
            ID: Product_ID,
        },
    });

    // Make sure a product was found
    if (product == null) return { success: false, message: ["No product found."] };

    const stock = {};
    stock[product.ID] = await getStockForProduct(product);

    // Try to apply lifetime discount
    if (user.LifetimeDiscount && product.ID === "b558f30e-c4cf-4b47-bb30-71e70ba72983") {
        product.Price_Lifetime -= 50;
    }

    const output = {
        product: product,
        stock: stock
    };

    return { success: true, message: output };
}

/**
 * @param {Product} product
 */
export async function getStockForProduct(product) {
    const availableStock = {};

    if (product.Track_Stock) {
        /** @type {Key[]} */
        const keys = await Key.findAll({
            where: {
                Product_ID: product.ID,
                Claimed: false
            },
        });

        for (let ii = 0; ii < keys.length; ii++) {
            const key = keys[ii];
            
            if (availableStock[key.Term] == null) {
                availableStock[key.Term] = 1;
            } else {
                availableStock[key.Term]++;
            }
        }
    }

    return availableStock;
}
