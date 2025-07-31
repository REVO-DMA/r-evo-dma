import { loadStripe } from "@stripe/stripe-js";
import { showAlert } from "../../alert";
import globals from "../../globals";
import { navigate } from "../../router";
import { showError } from "./template";
import { AccountData } from "../auth/accountDataManager";

/** @type {import("@stripe/stripe-js").Stripe} */
let stripe = null;
/** @type {import("@stripe/stripe-js").StripeElements} */
let elements = null;
/** @type {import("@stripe/stripe-js").StripePaymentElement} */
let paymentElement = null;
/** @type {import("@stripe/stripe-js").StripeAddressElement} */
let addressElement = null;

export async function initialize() {
    stripe = await loadStripe(globals.stripe.publishableKey);
}

export function createPaymentElement(clientSecret) {
    if (paymentElement != null) paymentElement.destroy();
    if (addressElement != null) addressElement.destroy();
    
    elements = stripe.elements({
        clientSecret: clientSecret,
        fonts: [
            {
                family: "Encode Sans",
                cssSrc: "https://fonts.googleapis.com/css2?family=Encode+Sans",
            }
        ],
        loader: "always",
        appearance: {
            theme: "night",
            disableAnimations: true,
            labels: "above",
            variables: {
                fontFamily: "Encode Sans, sans-serif",
                fontWeightLight: "400",
                fontWeightNormal: "400",
                fontWeightMedium: "400",
                fontWeightBold: "400",
                fontLineHeight: "1",
                fontSizeBase: "18px",
                fontSizeSm: "18px",
                borderRadius: "5px",
                colorText: "#ffffff",
                colorPrimary: "#009d32",
                colorBackground: "#252525",
                colorIconCardError: "#e9002b",
                colorIconCardCvcError: "#e9002b",
                colorDanger: "#e9002b",
                colorDangerText: "#e9002b",
                colorBackgroundText: "#444444",
                focusBoxShadow: "unset",
            },
        },
    });

    /** @type {import("@stripe/stripe-js").StripePaymentElementOptions} */
    const paymentElementOptions = {
        layout: {
            type: "accordion",
            defaultCollapsed: true,
            radios: false,
            spacedAccordionItems: true,
        },
        defaultValues: {
            billingDetails: {
                email: AccountData.user.Email,
            }
        },
        fields: {
            billingDetails: "auto"
        },
        terms: {
            applePay: "never",
            auBecsDebit: "never",
            bancontact: "never",
            card: "never",
            cashapp: "never",
            googlePay: "never",
            ideal: "never",
            paypal: "never",
            sepaDebit: "never",
            sofort: "never",
            usBankAccount: "never",
        },
    };
    paymentElement = elements.create("payment", paymentElementOptions);
    paymentElement.mount("#paymentElement");

    paymentElement.on("change", (event) => {
        const method = event.value.type;

        if (method === "afterpay_clearpay" || method === "affirm") {
            if (addressElement == null) {
                addressElement = createAddressElement();
                addressElement.mount("#addressElement");
            }

            document.getElementById("addressElementHeader").style.display = "";
            document.getElementById("addressElementContainer").style.display = "";
        } else {
            if (addressElement != null) {
                addressElement.unmount();
                addressElement.destroy();
                addressElement = null;
            }

            document.getElementById("addressElementHeader").style.display = "none";
            document.getElementById("addressElementContainer").style.display = "none";
        }
    })
}

export async function retrievePaymentIntent(clientSecret, ProductID, SubscriptionTerm) {
    const { paymentIntent } = await stripe.retrievePaymentIntent(clientSecret);

    const status = paymentIntent.status;

    if (status === "succeeded") {
        showAlert("success", "Payment Status", "Payment succeeded, thank you for your purchase!", false, false, "Show Account", "", "", () => {
            navigate("/account");
        });
    } else if (status === "processing") {
        showAlert("info", "Payment Status", "Your payment is processing...", false, false, "Refresh", "", "", () => {
            location.reload();
        });
    } else if (status === "requires_payment_method") {
        showAlert("error", "Payment Status", "Your payment was not successful, please try again.", false, false, "Try Again", "", "", () => {
            navigate(`/checkout?ProductID=${ProductID}&SubscriptionTerm=${SubscriptionTerm}`);
        });
    } else {
        showAlert("error", "Payment Error", "An unknown error occurred. Please try again.", false, false, "Show Store", "", "", () => {
            navigate(`/checkout?ProductID=${ProductID}&SubscriptionTerm=${SubscriptionTerm}`);
        });
    }
}

export async function confirmPayment() {
    const { error } = await stripe.confirmPayment({
        elements: elements,
        confirmParams: {
            return_url: location.href,
        },
    });

    if (error != null && (error.type != null && error.message != null)) {
        showError(error.message);
    } else {
        showError("An unknown error occurred while processing your payment. Please try again later.");
    }
}

function createAddressElement() {
    /** @type {import("@stripe/stripe-js").StripeAddressElementOptions} */
    const addressElementOptions = {
        mode: "shipping",
    };

    return elements.create("address", addressElementOptions);
}
