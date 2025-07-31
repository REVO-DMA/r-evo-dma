import { getPreferences, savePreferences } from "../preferenceManager";

export function setFeatureSectionState(id) {
    const featureSectionHeader = document.getElementById(`${id}_section`);
    let expand = false;

    // Update styling of Feature Section header
    if (featureSectionHeader.classList.contains("featureSectionExpanded")) {
        // Collapse
        expand = false;

        // Update the icon
        featureSectionHeader.children[1].innerHTML = `<i class="fa-duotone fa-circle-caret-up"></i>`;

        // The element state is tracked via these classes, update it to reflect the new state
        featureSectionHeader.classList.remove("featureSectionExpanded");
        featureSectionHeader.classList.add("featureSectionCollapsed");
    } else {
        // Expand
        expand = true;

        // Update the icon
        featureSectionHeader.children[1].innerHTML = `<i class="fa-duotone fa-circle-caret-down"></i>`;

        // The element state is tracked via these classes, update it to reflect the new state
        featureSectionHeader.classList.remove("featureSectionCollapsed");
        featureSectionHeader.classList.add("featureSectionExpanded");
    }

    // Persist this state change
    persistFeatureSectionState(id, expand);

    // Show/hide all section children
    Array.from(document.getElementsByClassName(`childOf_${id}`)).forEach((child) => {
        if (expand) {
            child.style.display = "";
        } else {
            child.style.display = "none";
        }
    });
}

/**
 * Get the expanded/collapsed state of a given Feature Section.
 * @param {string} id
 * @returns {("Expanded"|"Collapsed")|null}
 */
export function getFeatureSectionState(id) {
    const preferences = getPreferences("featureSection");

    let output = null;
    for (let i = 0; i < preferences.length; i++) {
        const preference = preferences[i];

        if (preference.id === id) {
            if (preference.expanded === true) {
                output = "Expanded";
            } else {
                output = "Collapsed";
            }

            break;
        }
    }

    return output;
}

/**
 * Set the state of a given Feature Section to expanded/collapsed.
 * @param {string} id
 * @param {boolean} expanded
 */
function persistFeatureSectionState(id, expanded) {
    const preferences = getPreferences("featureSection");

    for (let i = 0; i < preferences.length; i++) {
        if (preferences[i].id === id) {
            preferences[i].expanded = expanded;

            break;
        }
    }

    savePreferences("featureSection", preferences);
}

/**
 * Generate the HTML for a given Feature Separator.
 * @param {string} name
 * @param {string[]} classes
 */
export function generateFeatureSection(id, name, classes) {
    const state = getFeatureSectionState(id);

    return /*html*/ `
        <div class="row m-0 mb-2 p-0 featureSeparator justify-content-between featureSection${state}" id="${id}_section">
            <div class="col-auto p-0 featureSeparatorContentAlwaysWhite"><i class="${classes.join(" ")} ms-2 me-2"></i>${name}</div>
            <div class="col-auto p-0 clickableIcon">${
                state === "Expanded" ? `<i class="fa-duotone fa-circle-caret-down"></i>` : `<i class="fa-duotone fa-circle-caret-up"></i>`
            }</div>
        </div>
    `;
}
