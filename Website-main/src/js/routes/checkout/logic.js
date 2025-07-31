import { showAlert } from "../../alert";
import { httpPost } from "../../http";
import { hide as hideLoader, show as showLoader } from "../../loader";
import { navigate } from "../../router";
import { confirmPayment, createPaymentElement, retrievePaymentIntent } from "./Stripe";
import { createProductMarkup, createPurchaseButtonMarkup } from "./template";

/** @type {import("../store/template.js").Product} */
export let checkoutProduct = null;
export let checkoutStock = {};
/** @type {string} */
export let SubscriptionTermDays = "";
/** @type {number} */
export let SubscriptionTerm = null;

export async function initialize(queryParams) {
    const paymentIntentClientSecret = queryParams["payment_intent_client_secret"];

    if (paymentIntentClientSecret != null) {
        // Purchase has (possibly) been completed

        // Validate query params
        const ProductID = queryParams["ProductID"];
        SubscriptionTerm = Number(queryParams["SubscriptionTerm"]);
        if (SubscriptionTerm === -1)
            SubscriptionTermDays = "Lifetime";
        else
            SubscriptionTermDays = `${SubscriptionTerm} Day`;

        await retrievePaymentIntent(paymentIntentClientSecret, ProductID, SubscriptionTerm);
    } else {
        // This is a purchase attempt

        // Validate query params
        const ProductID = queryParams["ProductID"];
        if (ProductID == null) {
            showAlert("error", "Checkout Error", "Missing Product ID. Please try again later.", false, false, "Show Store", "", "", () => {
                navigate("/store");
            });
        }
        SubscriptionTerm = queryParams["SubscriptionTerm"];
        if (SubscriptionTerm == null) {
            showAlert("error", "Checkout Error", "Missing Subscription Term. Please try again later.", false, false, "Show Store", "", "", () => {
                navigate("/store");
            });
        }
        SubscriptionTerm = Number(SubscriptionTerm);
        if (SubscriptionTerm === -1)
            SubscriptionTermDays = "Lifetime";
        else
            SubscriptionTermDays = `${SubscriptionTerm} Day`;

        // Try to create a payment intent
        const paymentIntentResult = await httpPost(
            "create-payment-intent",
            {
                productID: ProductID,
                subscriptionTerm: SubscriptionTerm,
            },
            true
        );
        const paymentIntentResponse = paymentIntentResult.response;
        if (!paymentIntentResponse.success) {
            showAlert("error", "Checkout Error", paymentIntentResponse.message.join("<hr>"), false, false, "Show Store", "", "", () => {
                navigate("/store");
            });
            return;
        }

        // Try to get the product details
        const getProductResult = await httpPost(
            "get-product-by-id",
            {
                productID: ProductID,
            },
            true
        );
        const getProductResponse = getProductResult.response;
        if (!getProductResponse.success) {
            showAlert("error", "Checkout Error", "Error getting Product Details. Please try again later.", false, false, "Show Store", "", "", () => {
                navigate("/store");
            });
            return;
        }
        checkoutProduct = getProductResponse.message.product;
        checkoutStock = getProductResponse.message.stock;
        // Inject product markup
        document.getElementById("checkoutProduct").innerHTML = createProductMarkup();

        createPaymentElement(paymentIntentResponse.message.clientSecret);

        // Inject purchase button
        document.getElementById("purchaseButton").innerHTML = createPurchaseButtonMarkup();

        document.getElementById("PurchaseNowButton").addEventListener("click", async () => {
            showLoader("Processing Payment");
            document.getElementById("checkoutErrorsContainer").style.display = "none";
            await confirmPayment();
            hideLoader();
        });

        document.getElementById("checkoutContainer").style.display = "";
    }
}
