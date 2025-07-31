import markdownit from "markdown-it";
import { httpPost } from "../../http";
import { navigate } from "../../router";
import { attachEvents as attachHeaderEvents, header as getHeader } from "../../templates/header";
import { showImageViewer } from "./imageViewer";
import defaultIcon from "../../../img/defaultIcon.svg";
import { AccountData } from "../auth/accountDataManager";

const MD = markdownit();

/**
 * Represents a Product of the application.
 * @typedef {Object} Product
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
 */

/** @type {Product[]?} */
let products = null;
let stock = {};
/**
 * Whether or not any Terms are available for purchase.
 */
let anyTermsAvailable = false;

export async function show(queryParams) {
    const result = await httpPost("get-products", {}, true);
    const response = result.response;
    if (response.success) {
        products = response.message.products;
        stock = response.message.stock;
    } else {
        // Show message - unable to retrieve products
        return;
    }

    document.getElementById("baseContainer").innerHTML = getTemplate();
    attachProductEvents();
    attachHeaderEvents();
}

/**
 * Get a product by it's ID.
 * @param {string} ID
 */
export function getProductByID(ID) {
    let output = null;

    for (let i = 0; i < products.length; i++) {
        const product = products[i];

        if (product.ID === ID) {
            output = product;
            break;
        }
    }

    return output;
}

function getTemplate() {
    const storeItems = [];
    products.forEach((product) => {
        storeItems.push(createProduct(product));
    });

    return /*html*/ `
        ${getHeader("Store")}

        <div class="row m-0 p-0 justify-content-center align-items-center storeItemsContainer">
            ${storeItems.join("")}
        </div>
    `;
}

/**
 * @param {Product} ProductInfo
 */
function createProduct(ProductInfo) {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center storeItem" id="${ProductInfo.ID}">
            <!-- Icon -->
            <div class="col-auto">
                <img src="${ProductInfo.Icon == null || ProductInfo.Icon == "" ? defaultIcon : ProductInfo.Icon}" class="p-0 storeItemIcon" />
            </div>
            <!-- Name/Blurb -->
            <div class="col">
                <!-- Name -->
                <div class="row">
                    <div class="col-auto storeItemName">${ProductInfo.Name}</div>
                </div>
                <!-- Blurb -->
                <div class="row mt-2">
                    <div class="col-auto storeItemDescriptionBlurb">${ProductInfo.Blurb}</div>
                </div>
            </div>
            <!-- Status -->
            <div class="col-auto storeItemStatusBadge storeItemStatusBadge_${ProductInfo.Status}">${ProductInfo.Status}</div>
            <!-- Chevron -->
            <div class="col-auto">
                <i class="fa-solid fa-chevron-right"></i>
            </div>
        </div>
    `;
}

/**
 * @param {Product} ProductInfo
 */
function viewProduct(ProductInfo) {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center storeViewItemContainer">
            <!-- Main Image -->
            <div class="col-4">
                <img src="${ProductInfo.Main_Image}" class="p-0 storeItemMainImage" />
            </div>
            <div class="row m-0 col-8">
                <!-- Main -->
                <div class="col-12 storeViewItemMain">
                    <!-- Name -->
                    <div class="row m-0">
                        <div class="col-auto storeItemName">${ProductInfo.Name}</div>
                    </div>
                    <!-- Blurb -->
                    <div class="row m-0 mt-2">
                        <div class="col-auto storeItemDescriptionBlurb">${ProductInfo.Blurb}</div>
                    </div>
                    <!-- Images -->
                    ${ProductInfo.Images.length > 0 ? /*html*/`
                        <div class="row m-0 mt-3 justify-content-center align-items-center storeViewItemImages">
                            ${createProductImages(ProductInfo.Images)}
                        </div>
                    ` : ""}
                    <!-- Description/Features Area -->
                    <div class="row m-0 mt-3 storeItemTabsContainer">
                        <!-- Description/Features Toggle -->
                        <div class="row m-0 p-0 storeItemTabSelectorOuter">
                            <div class="col-auto p-0 storeItemTabSelector storeItemTabSelectorActive" id="storeItemTabSelector_Description"><i class="fa-solid fa-quote-right me-2"></i>Description</div>
                            <div class="col-auto p-0 storeItemTabSelector" id="storeItemTabSelector_Features"><i class="fa-solid fa-list-ul me-2"></i>Features</div>
                        </div>
                        <div class="row m-0 p-3 storeItemSelectedTabContent">
                            <div class="col-auto p-0 storeItemDescription" id="storeItemTabContent_Description">${MD.render(ProductInfo.Description)}</div>
                            <div class="col-auto p-0 storeItemDescription" id="storeItemTabContent_Features" style="display: none;">${MD.render(ProductInfo.Features)}</div>
                        </div>
                    </div>
                </div>
                <!-- Purchase -->
                <div class="col-12 mt-3 storeViewItemMain">
                    <div class="form-text mb-2">All subscriptions grant access to the Subscribers Only area of our Discord.</div>
                    <div class="row m-0" id="SubscriptionTermDropdownContainer">
                        <div class="col p-0">
                            <select class="form-select" id="SubscriptionTermDropdown">
                                ${getPriceMarkup(1, ProductInfo)}
                                ${getPriceMarkup(7, ProductInfo)}
                                ${getPriceMarkup(15, ProductInfo)}
                                ${getPriceMarkup(30, ProductInfo)}
                                ${getPriceMarkup(90, ProductInfo)}
                                ${getPriceMarkup(-1, ProductInfo)}
                            </select>
                        </div>
                        <div class="col-auto pe-0">
                            <button class="btn btn-primary" id="PurchaseNowButton">Checkout</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

/**
 * @param {number?} price
 */
function isValidPrice(price) {
    if (price != null && price !== -1)
        return true;

    return false;
}

/**
 * @param {number} day
 * @param {Product} product
 * @param {any} stock
 */
export function getPrice(day, product, stock) {
    const subPrice = product[`Price_${day}_Day_Sub`];
    const nonSubPrice = product[`Price_${day}_Day`];
    const lifetimePrice = product["Price_Lifetime"];

    if (product.Track_Stock) {
        const stockInfo = stock[product.ID][day];
        if (stockInfo == null || stockInfo <= 0) return -1;
    }

    if (AccountData.account.canHaveActiveSubDiscount && isValidPrice(subPrice)) return subPrice;
    else if (day === -1 && isValidPrice(lifetimePrice)) return lifetimePrice;
    else if (isValidPrice(nonSubPrice)) return nonSubPrice;
    else return -1;
}

/**
 * @param {number} day
 * @param {Product} productInfo
 */
function getPriceMarkup(day, product) {
    const price = getPrice(day, product, stock);

    if (price === -1) return "";
    else {
        anyTermsAvailable = true;
        let daysStr = `${day} DAY`;
        if (day === -1) daysStr = "Lifetime";
        else if (day > 1) daysStr = `${day} DAYS`;

        return `<option value="${day}">${daysStr} - $${price}</option>`;
    }
}

/**
 * @param {string[]} Images
 */
function createProductImages(Images) {
    /**
     * @param {string} Image
     */
    const getBase = (Image) => {
        if (Image.endsWith(".mp4")) {
            return /*html*/ `
                <div class="col-auto">
                    <div class="row m-0 storeViewItemImage storeViewItemVideo justify-content-center align-items-center">
                        <div class="col-auto">
                            <i class="fa-duotone fa-play storeViewItemVideoIcon"></i>
                        </div>
                    </div>
                </div>
            `;
        } else {
            return /*html*/ `
                <div class="col-auto">
                    <img src="${Image}" class="p-0 storeViewItemImage" />
                </div>
            `;
        }
    };

    const HTML = [];

    for (let i = 0; i < Images.length; i++) {
        const image = Images[i];

        if (i === 3) break;

        HTML.push(getBase(image));
    }

    return HTML.join("");
}

function attachProductEvents() {
    /** @type {HTMLDivElement[]} */
    const storeItems = document.getElementsByClassName("storeItem");
    Array.from(storeItems).forEach((storeItem) => {
        storeItem.addEventListener("click", () => {
            const product = getProductByID(storeItem.id);

            if (product === null) {
                // Show alert - unable to view product
                return;
            }

            const HTML = /*html*/ `
                ${getHeader("Store")}
                ${viewProduct(product)}
            `;

            document.getElementById("baseContainer").innerHTML = HTML;
            attachHeaderEvents();

            Array.from(document.getElementsByClassName("storeViewItemImage")).forEach((element, i) => {
                element.addEventListener("click", () => {
                    showImageViewer(product.Images, i);
                });
            });

            const storeItemTabSelector_DescriptionEl = document.getElementById("storeItemTabSelector_Description");
            const storeItemTabSelector_FeaturesEl = document.getElementById("storeItemTabSelector_Features");

            const storeItemTabContent_FeaturesEl = document.getElementById("storeItemTabContent_Features");
            const storeItemTabContent_DescriptionEl = document.getElementById("storeItemTabContent_Description");

            storeItemTabSelector_DescriptionEl.addEventListener("click", () => {
                storeItemTabSelector_FeaturesEl.classList.remove("storeItemTabSelectorActive");
                storeItemTabSelector_DescriptionEl.classList.add("storeItemTabSelectorActive");

                storeItemTabContent_FeaturesEl.style.display = "none";
                storeItemTabContent_DescriptionEl.style.display = "";
            });

            storeItemTabSelector_FeaturesEl.addEventListener("click", () => {
                storeItemTabSelector_DescriptionEl.classList.remove("storeItemTabSelectorActive");
                storeItemTabSelector_FeaturesEl.classList.add("storeItemTabSelectorActive");

                storeItemTabContent_DescriptionEl.style.display = "none";
                storeItemTabContent_FeaturesEl.style.display = "";
            });

            if (!anyTermsAvailable) {
                document.getElementById("SubscriptionTermDropdownContainer").innerHTML = /*html*/`
                    <div class="col-auto p-0">
                        <h3>This product is temporarily unavailable.</h3>
                    </div>
                `;
            } else {
                document.getElementById("PurchaseNowButton").addEventListener("click", () => {
                    const term = document.getElementById("SubscriptionTermDropdown").value.split(" ")[0];
                    navigate(`/checkout?ProductID=${product.ID}&SubscriptionTerm=${term}`);
                });
            }
        });
    });
}
