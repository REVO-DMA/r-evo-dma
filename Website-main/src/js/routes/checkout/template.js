import { attachEvents as attachHeaderEvents, header as getHeader } from "../../templates/header";
import { AccountData } from "../auth/accountDataManager";
import { checkoutProduct, checkoutStock, initialize as initializeLogic, SubscriptionTerm, SubscriptionTermDays } from "./logic";
import { initialize as initializeStripe } from "./Stripe";
import defaultIcon from "../../../img/defaultIcon.svg";
import { getPrice } from "../store/template";

export async function show(queryParams) {
    if (AccountData == null) return;
    document.getElementById("baseContainer").innerHTML = getTemplate();
    attachHeaderEvents();
    await initializeStripe();
    await initializeLogic(queryParams);
}

function getTemplate() {
    return /*html*/ `
        ${getHeader("Checkout")}

        <div class="row m-0 p-0 col-auto checkoutContainer" id="checkoutContainer" style="display: none;">
            <!-- Checkout Summary -->
            <div class="row m-0 p-0">
                <div class="row m-0 p-0 checkoutSummary">
                    <div class="col p-0">Checkout Summary</div>
                </div>
                <div class="row justify-content-center align-items-center m-0 mt-3 p-0 pt-3 pb-3 checkoutProduct" id="checkoutProduct"></div>
            </div>
            <div class="form-text mt-3 mb-2 p-0">We have partnered with Stripe to offer secure and convenient payment options.</div>
            <div class="row m-0 mt-2 mb-3 p-0 checkoutErrorsContainer" id="checkoutErrorsContainer" style="display: none;"></div>

            <!-- Address -->
            <div class="row m-0 p-0 checkoutSectionHeader" id="addressElementHeader" style="display: none">
                <div class="col p-0">Address</div>
            </div>
            <div class="row m-0 p-0 addressElementContainer" id="addressElementContainer" style="display: none">
                <div class="col p-0 addressElement" id="addressElement"><!--Stripe.js injects the Address Element--></div>
            </div>

            <!-- Payment Method -->
            <div class="row m-0 p-0 checkoutSectionHeader">
                <div class="col p-0">Payment Method</div>
            </div>
            <div class="row m-0 p-0">
                <div class="col p-0" id="paymentElement"><!--Stripe.js injects the Payment Element--></div>
            </div>
            <div class="row justify-content-end align-items-center m-0 mt-3 p-0 pt-3" id="purchaseButton"></div>
        </div>
    `;
}

export function createProductMarkup() {
    const price = getPrice(SubscriptionTerm, checkoutProduct, checkoutStock);

    return /*html*/ `
        <div class="col-auto">
            <img src="${checkoutProduct.Icon == null || checkoutProduct.Icon == "" ? defaultIcon : checkoutProduct.Icon}" class="p-0 checkoutItemIcon" />
        </div>
        <div class="col">
            <div class="row m-0">
                <div class="col-auto">${checkoutProduct.Name}</div>
            </div>
            <div class="row m-0">
                <div class="col-auto">
                    <div class="form-text p-0">${SubscriptionTermDays} Subscription</div>
                </div>
            </div>
        </div>
        <div class="col-auto">$${price}</div>
        <div class="row justify-content-between align-items-center m-0 mt-3 p-0 pt-3 checkoutPriceTotal">
            <div class="col-auto">Total</div>
            <div class="col-auto">$${price}</div>
        </div>
    `;
}

export function createPurchaseButtonMarkup() {
    return /*html*/ `
        <button class="btn btn-primary" id="PurchaseNowButton">Pay $${getPrice(SubscriptionTerm, checkoutProduct, checkoutStock)} Now</button>
    `;
}

export function showError(message) {
    const checkoutErrorsContainerEl = document.getElementById("checkoutErrorsContainer");
    checkoutErrorsContainerEl.style.display = "";
    checkoutErrorsContainerEl.innerHTML = /*html*/ `<div class="col checkoutError">${message}</div>`;
}
