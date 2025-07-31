/** @type {HTMLDivElement} */
const loaderContainerEl = document.getElementById("loaderContainer");
/** @type {HTMLDivElement} */
const loaderTextEl = document.getElementById("loaderText");

/** @type {NodeJS.Timeout} */
let loaderTimeout;
/** @type {NodeJS.Timeout} */
let loaderHideDelayTimeout;

/**
 * Show the app loader.
 * @param {string} text
 */
export function show(text) {
    clearTimeout(loaderHideDelayTimeout);
    clearTimeout(loaderTimeout);
    loaderTextEl.innerText = text;
    loaderContainerEl.style.display = "";
    setTimeout(() => {
        loaderContainerEl.classList.add("loaderContainerShown");
    }, 0);
}

/**
 * Hide the app loader.
 * @param {number} delay
 */
export function hide(delay = 0) {
    const doHide = () => {
        loaderContainerEl.classList.remove("loaderContainerShown");
        clearTimeout(loaderTimeout);
        loaderTimeout = setTimeout(() => {
            loaderContainerEl.style.display = "none";
        }, 1000);
    };

    clearTimeout(loaderHideDelayTimeout);

    if (delay > 0) {
        loaderHideDelayTimeout = setTimeout(doHide, delay);
    } else {
        doHide();
    }
}
