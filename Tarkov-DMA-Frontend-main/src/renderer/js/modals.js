import { isEqual } from "lodash";
import { closeAlert, showAlert } from "./alert";

/** @type {HTMLDivElement[]} */
let sectionToggles;
let modalsCount = 0;

const modalDefaults = {
    x: 20,
    y: 20,
};

const modalOverflow = {
    xTol: 10,
    yTol: 10,
};

/** @type {HTMLDivElement} */
const appContainerEl = document.getElementById("appContainer");

/** @type {HTMLDivElement} */
const hoveredToggleTextContainerEl = document.getElementById("hoveredToggleTextContainer");

/** @type {HTMLDivElement} */
const sectionTogglesContainerEl = document.getElementById("sectionTogglesContainer");

/** @type {HTMLDivElement} */
const hoveredToggleTextEl = document.getElementById("hoveredToggleText");

const modalElements = ["mainMenu", "featuresModal", "killfeed", "playersTable", "appSettingsModal", "espSettingsModal", "lootTable", "lootFilters", "updatesModal"];

export let preferences = {};

const modals = {};

export function initialize() {
    const modalPreferences = localStorage.getItem("modalPreferences");
    if (modalPreferences == null) {
        localStorage.setItem("modalPreferences", JSON.stringify({}));
        preferences = {};
    } else {
        preferences = JSON.parse(modalPreferences);
    }

    sectionToggles = Array.from(document.getElementsByClassName("sectionToggle"));

    sectionToggles.forEach((element) => {
        element.addEventListener("mouseenter", () => {
            hoveredToggleTextContainerEl.style.display = "";
            hoveredToggleTextEl.innerText = element.children[0].children[1].innerText;
        });

        element.addEventListener("mouseleave", () => {
            hoveredToggleTextContainerEl.style.display = "none";
            hoveredToggleTextEl.innerText = "";
        });
    });

    // Initialize all modals
    modalElements.forEach((element) => {
        const htmlElement = document.getElementById(element);

        if (htmlElement == null) {
            console.error(`Error initializing modal by id: ${element}`);
        } else {
            initializeModal(htmlElement);
        }
    });

    window.addEventListener("resize", findOutOfBoundsModals);

    findOutOfBoundsModals();

    // Show modal toggles
    sectionTogglesContainerEl.style.display = "";
}

/**
 * @param {HTMLDivElement[]} modals
 */
function recoverOutOfBoundsModals(modals) {
    for (let i = 0; i < modals.length; i++) {
        const modal = modals[i];

        const xPos = modalDefaults.x * (i + 1);
        const yPos = modalDefaults.y * (i + 1);

        modal.style.left = `${xPos}px`;
        modal.style.top = `${yPos}px`;

        preferences[modal.id].x = xPos;
        preferences[modal.id].y = yPos;
    }

    findOutOfBoundsModals();

    savePreferences();
}

/** @type {HTMLDivElement[]} */
let lastOutOfBoundsModals = [];

function findOutOfBoundsModals() {
    /** @type {HTMLDivElement[]} */
    const outOfBoundsModals = [];

    for (const [key, value] of Object.entries(modals)) {
        let xOut = false;
        let yOut = false;

        if (value.element.clientWidth + value.element.offsetLeft + modalOverflow.xTol - 2 >= appContainerEl.clientWidth) xOut = true;
        if (value.element.clientHeight + value.element.offsetTop + modalOverflow.yTol - 2 >= appContainerEl.clientHeight) yOut = true;

        if (xOut || yOut) {
            value.element.classList.add("modalContainerOutOfBounds");

            outOfBoundsModals.push(value.element);
        } else {
            value.element.classList.remove("modalContainerOutOfBounds");
        }
    }

    if (outOfBoundsModals.length > 0) {
        if (!isEqual(outOfBoundsModals, lastOutOfBoundsModals)) {
            lastOutOfBoundsModals = outOfBoundsModals;

            /** @type {string[]} */
            const modalNames = [];
            outOfBoundsModals.forEach((element) => {
                modalNames.push(`- ${element.children[0].children[0].innerText}`);
            });

            showAlert(
                "warning",
                "Modal Recovery",
                /*html*/ `<p>One or more modals were detected to be outside the bounds of the screen. Would you like to reset the position of the modals?</p><h4>Modals to Recover</h4><hr>${modalNames.join(
                    "<br>"
                )}`,
                true,
                false,
                "Recover",
                "",
                "Cancel",
                (result) => {
                    if (result) {
                        recoverOutOfBoundsModals(outOfBoundsModals);
                    }
                }
            );
        }
    } else {
        closeAlert();
        lastOutOfBoundsModals = [];
    }
}

function savePreferences() {
    localStorage.setItem("modalPreferences", JSON.stringify(preferences));
}

/**
 * @param {string} topElementID
 * @param {string} parentElementID
 */
function reindexModals(topElementID = null, parentElementID = null) {
    let index = 0;
    for (const [key, value] of Object.entries(modals)) {
        // Don't mess with the top element's index
        if (key === topElementID || key === parentElementID) continue;

        value.element.style.zIndex = index;
        value.zIndex = index;
        index++;
    }
}

/**
 * @param {HTMLDivElement} element
 * @param {HTMLDivElement} parentElement The child element's parent.
 */
export function initializeChildModal(element, parentElement) {
    let pos1 = 0;
    let pos2 = 0;

    modalsCount++;

    if (preferences[element.id] == null) {
        // Set default preferences for this modal if they do not exist

        const parentRect = parentElement.getBoundingClientRect();
        const offsetLeft = parentRect.left + 30;
        const offsetTop = parentRect.top + 30;

        preferences[element.id] = {
            show: true,
            x: offsetLeft,
            y: offsetTop,
        };

        element.style.left = `${offsetLeft}px`;
        element.style.top = `${offsetTop}px`;

        savePreferences();
    } else {
        // Apply existing preferences to this modal
        const elementPreferences = preferences[element.id];

        element.style.left = `${elementPreferences.x}px`;
        element.style.top = `${elementPreferences.y}px`;

        preferences[element.id].show = true;
    }

    // Initialize data for this modal
    modals[element.id] = {
        element: element,
        zIndex: modalsCount,
    };
    element.style.zIndex = modalsCount;

    const onmouseup = () => {
        document.onmouseup = null;
        document.onmousemove = null;

        // Save position
        preferences[element.id].x = element.offsetLeft;
        preferences[element.id].y = element.offsetTop;
        savePreferences();

        findOutOfBoundsModals();
    };

    const onmousemove = (e) => {
        e = e || window.event;
        e.preventDefault();

        // Calculate the new cursor position
        pos1 = e.movementX; // X dir
        pos2 = e.movementY; // Y dir

        if (
            element.clientWidth + element.offsetLeft + modalOverflow.xTol >= appContainerEl.clientWidth ||
            element.clientWidth + element.offsetLeft + pos1 + modalOverflow.xTol >= appContainerEl.clientWidth
        ) {
            // Prevent x from exceeding right bounds
            // Still allows to drag to left

            if (element.offsetLeft + pos1 > element.offsetLeft) return;
        } else if (element.offsetLeft <= modalOverflow.xTol || element.offsetLeft + pos1 <= modalOverflow.xTol) {
            // Prevent x from exceeding left bounds
            // Still allows to drag to right

            if (element.offsetLeft + pos1 < element.offsetLeft) return;
        } else if (element.offsetTop <= modalOverflow.yTol || element.offsetTop + pos2 <= modalOverflow.yTol) {
            // Prevent y from exceeding top bounds
            // Still allows to drag down

            if (element.offsetTop + pos2 < element.offsetTop) return;
        } else if (
            element.offsetHeight + element.offsetTop + modalOverflow.yTol >= appContainerEl.clientHeight ||
            element.offsetHeight + element.offsetTop + pos2 + modalOverflow.yTol >= appContainerEl.clientHeight
        ) {
            // Prevent y from exceeding bottom bounds
            // Still allows to drag up

            if (element.offsetTop + pos2 > element.offsetTop) return;
        }

        // Set the element's new position
        element.style.left = `${element.offsetLeft + pos1}px`;
        element.style.top = `${element.offsetTop + pos2}px`;
    };

    const moveElementToTop = () => {
        // If this element is already on top, skip
        if (Number(element.style.zIndex) === modalsCount) return;

        // Move parent and child modal to z-top
        parentElement.style.zIndex = modalsCount - 1;
        element.style.zIndex = modalsCount;
        // Re-index modals z-index
        reindexModals(element.id, parentElement.id);
    };

    const onmousedown = (e) => {
        e = e || window.event;
        e.preventDefault();

        if (e.target.classList.contains("modalContainerTitlebarCloseButton")) {
            return;
        }

        // Since this element is being dragged, move it to top
        moveElementToTop();

        document.onmouseup = onmouseup;
        document.onmousemove = onmousemove;
    };

    if (document.getElementById(element.id + "Titlebar")) {
        document.getElementById(element.id + "Titlebar").onmousedown = onmousedown;
    } else {
        console.error(`modals.js ERROR for ${element.id}: No titlebar to bind events to!`);
    }

    // Anywhere you click the element should bring it to top
    element.onmousedown = moveElementToTop;

    // Bind events to close button
    const modalCloseElement = element.children[0].children[1];
    if (!modalCloseElement.classList.contains("modalContainerTitlebarCloseButton")) {
        console.error(`modals.js ERROR for ${element.id}: Malformed titlebar! Close element is not in the expected location.`);
    } else {
        modalCloseElement.addEventListener("click", () => {
            preferences[element.id].show = false;
            element.remove();
            modalsCount--;
            savePreferences();
        });
    }
}

/**
 * @param {HTMLDivElement} element
 */
function initializeModal(element) {
    let pos1 = 0;
    let pos2 = 0;

    modalsCount++;

    if (preferences[element.id] == null) {
        // Set default preferences for this modal if they do not exist
        preferences[element.id] = {
            show: false,
            x: modalDefaults.x,
            y: modalDefaults.y,
        };

        element.style.left = `${modalDefaults.x}px`;
        element.style.top = `${modalDefaults.y}px`;

        savePreferences();
    } else {
        // Apply existing preferences to this modal
        const elementPreferences = preferences[element.id];

        element.style.left = `${elementPreferences.x}px`;
        element.style.top = `${elementPreferences.y}px`;

        if (elementPreferences.show) {
            element.style.display = "";
        }
    }

    // Initialize data for this modal
    modals[element.id] = {
        element: element,
        zIndex: modalsCount,
    };
    element.style.zIndex = modalsCount;

    const onmouseup = () => {
        document.onmouseup = null;
        document.onmousemove = null;

        // Save position
        preferences[element.id].x = element.offsetLeft;
        preferences[element.id].y = element.offsetTop;
        savePreferences();

        findOutOfBoundsModals();
    };

    const onmousemove = (e) => {
        e = e || window.event;
        e.preventDefault();

        // Calculate the new cursor position
        pos1 = e.movementX; // X dir
        pos2 = e.movementY; // Y dir

        if (
            element.clientWidth + element.offsetLeft + modalOverflow.xTol >= appContainerEl.clientWidth ||
            element.clientWidth + element.offsetLeft + pos1 + modalOverflow.xTol >= appContainerEl.clientWidth
        ) {
            // Prevent x from exceeding right bounds
            // Still allows to drag to left

            if (element.offsetLeft + pos1 > element.offsetLeft) return;
        } else if (element.offsetLeft <= modalOverflow.xTol || element.offsetLeft + pos1 <= modalOverflow.xTol) {
            // Prevent x from exceeding left bounds
            // Still allows to drag to right

            if (element.offsetLeft + pos1 < element.offsetLeft) return;
        } else if (element.offsetTop <= modalOverflow.yTol || element.offsetTop + pos2 <= modalOverflow.yTol) {
            // Prevent y from exceeding top bounds
            // Still allows to drag down

            if (element.offsetTop + pos2 < element.offsetTop) return;
        } else if (
            element.offsetHeight + element.offsetTop + modalOverflow.yTol >= appContainerEl.clientHeight ||
            element.offsetHeight + element.offsetTop + pos2 + modalOverflow.yTol >= appContainerEl.clientHeight
        ) {
            // Prevent y from exceeding bottom bounds
            // Still allows to drag up

            if (element.offsetTop + pos2 > element.offsetTop) return;
        }

        // Set the element's new position
        element.style.left = `${element.offsetLeft + pos1}px`;
        element.style.top = `${element.offsetTop + pos2}px`;
    };

    const moveElementToTop = () => {
        // If this element is already on top, skip
        if (Number(element.style.zIndex) === modalsCount) return;

        // Move modal to z-top
        element.style.zIndex = modalsCount;
        // Re-index modals z-index
        reindexModals(element.id);
    };

    const onmousedown = (e) => {
        e = e || window.event;
        e.preventDefault();

        if (e.target.classList.contains("modalContainerTitlebarCloseButton")) {
            return;
        }

        // Since this element is being dragged, move it to top
        moveElementToTop();

        document.onmouseup = onmouseup;
        document.onmousemove = onmousemove;
    };

    if (document.getElementById(element.id + "Titlebar")) {
        document.getElementById(element.id + "Titlebar").onmousedown = onmousedown;
    } else {
        console.error(`modals.js ERROR for ${element.id}: No titlebar to bind events to!`);
    }

    // Anywhere you click the element should bring it to top
    element.onmousedown = moveElementToTop;

    // Bind events for associated modal toggle
    const modalToggle = document.getElementById(`${element.id}Toggle`);
    if (modalToggle == null) {
        console.error(`modals.js ERROR for ${element.id}: Missing modal toggle! Please add the id "${element.id}Toggle" to the associated toggle.`);
    } else {
        modalToggle.addEventListener("click", () => {
            if (element.style.display === "") {
                element.style.display = "none";
                preferences[element.id].show = false;
            } else {
                element.style.display = "";
                preferences[element.id].show = true;
            }

            moveElementToTop();
            savePreferences();
            findOutOfBoundsModals();
        });
    }

    const onCloseClick = () => {
        element.style.display = "none";
        preferences[element.id].show = false;
        savePreferences();
    };

    // Bind events to close button
    const modalCloseContainer = element.children[0].children[1];
    const modalCloseElementCandidate1 = modalCloseContainer.children[0];
    const modalCloseElementCandidate2 = modalCloseContainer.children[1];
    if (modalCloseElementCandidate1.classList.contains("modalContainerTitlebarCloseButton")) {
        modalCloseElementCandidate1.addEventListener("click", onCloseClick);
    } else if (modalCloseElementCandidate2.classList.contains("modalContainerTitlebarCloseButton")) {
        modalCloseElementCandidate2.addEventListener("click", onCloseClick);
    } else {
        console.error(`modals.js ERROR for ${element.id}: Malformed titlebar! Close element is not in the expected location.`);
    }
}
