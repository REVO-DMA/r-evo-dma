import { navigate } from "../../router";

/** @type {HTMLDivElement} */
let goHomeButton;

export function initialize() {
    goHomeButton = document.getElementById("goHomeButton");
    goHomeButton.addEventListener("click", () => {
        navigate("/store");
    });
}