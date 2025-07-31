import { syncLootFilter } from "../ipc/manager";
import { setMainAppStatus } from "../ipc/messageHandlers/radarStatus";
import { updatePrices } from "../rendering/loot/lootManager";
import { updateLootFilterItemsList } from "./lootFilter";
import { GET_Raw, POST } from "../utils/http";
import { inflate } from "../utils/zlib";
import { LootItems } from "../../../../../Loot-API/src/types/eft";
import { createModal } from "../modalMaster";

export type LootCategoryLUT = {
    [key: string]: string
};

const LootCategoryLUT: LootCategoryLUT = {
    "High Value": "highValue",
    "Corpses": "corpses",
    "Gear": "gear",
    "Weapons": "weapons",
    "Weapon Parts": "weaponParts",
    "Sights": "sights",
    "Keys": "keys",
    "Barter Items": "barterItems",
    "Containers": "containers",
    "Provisions": "provisions",
    "Medical Items": "medicalItems",
    "Other": "other",
};

const ContainerTypes = [
    "Loose Loot",
    "Buried Barrel Cache",
    "Cash Register",
    "Dead Body",
    "Drawer",
    "Duffle Bag",
    "Grenade Box",
    "Ground Cache",
    "Jacket",
    "Medbag SMU06",
    "Medcase",
    "Medical Supply Crate",
    "PC Block",
    "Plastic Suitcase",
    "Ration Supply Crate",
    "Safe",
    "Technical Supply Crate",
    "Toolbox",
    "Weapon Box",
    "Wooden Ammo Box",
    "Wooden Crate",
];

export let ALL_ITEMS: LootItems = null;

export async function initialize() {
    setMainAppStatus("Loading items");

    // TODO: clean this mess up!

    // Init localStorage for item filter if it has not been initialized yet
    if (localStorage.getItem("itemFilterPreferences") == null) {
        localStorage.setItem("itemFilterPreferences", JSON.stringify({}));
    }

    if (localStorage.getItem("itemFilterPreferences_containerCategories") == null) {
        localStorage.setItem("itemFilterPreferences_containerCategories", JSON.stringify({}));
    }

    const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));

    // Loot categories
    const lootCategoryToggles = Array.from(document.getElementsByClassName("lootCategoryToggle"));
    for (let i = 0; i < lootCategoryToggles.length; i++) {
        const el = lootCategoryToggles[i];
        
        if (el.classList.contains("lootCategoryToggleRight")) {
            continue;
        }
        
        const ID = LootCategoryLUT[el.innerHTML];
        if (preferences[ID] != null && preferences[ID].value === true) el.classList.add("lootCategoryToggleActive");
    }

    // Quest item visibility
    const questItemVisibilityEl = document.getElementById("questItemVisibility");
    if (preferences["questItemVisibility"] != null && preferences["questItemVisibility"].value === true)
        questItemVisibilityEl.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;

    // Quest item visibility
    const questItems_ShowAllEl = document.getElementById("questItems_ShowAll");
    if (preferences["questItems_ShowAll"] != null && preferences["questItems_ShowAll"].value === true)
        questItems_ShowAllEl.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;

    // Price per slot
    const pricePerSlotEl = document.getElementById("pricePerSlot");
    if (preferences["pricePerSlot"] != null && preferences["pricePerSlot"].value === true)
        pricePerSlotEl.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;

    // High value threshold
    const highValueThresholdEl = (document.getElementById("highValueThreshold") as HTMLInputElement);
    if (preferences["highValueThreshold"] != null) highValueThresholdEl.value = preferences["highValueThreshold"].value;

    // High value threshold
    const importantThresholdEl = (document.getElementById("importantThreshold") as HTMLInputElement);
    if (preferences["importantThreshold"] != null) importantThresholdEl.value = preferences["importantThreshold"].value;

    await getFreshLoot();

    // Bind loot filter events
    initializeLootFilterUI();
}

function initializeLootFilterUI() {
    const lootCategoryToggles = Array.from(document.getElementsByClassName("lootCategoryToggle"));

    for (let i = 0; i < lootCategoryToggles.length; i++) {
        const el = lootCategoryToggles[i];
        
        if (el.classList.contains("lootCategoryToggleRight")) {
            el.addEventListener("click", () => {
                const lootTable = (document.getElementById("lootTable") as HTMLDivElement);
                const configurationModalBody = createModal("Container Categories", "containerLootShownCategoriesModal", "configureContainerCategoriesModalContainer", lootTable);
                if (configurationModalBody === null) return;

                let html = `<div class="row m-0 modalSettingInnerContainer align-items-center">`;

                const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences_containerCategories"));

                for (let ii = 0; ii < ContainerTypes.length; ii++) {
                    const container = ContainerTypes[ii];
                    
                    const containerID = container.replace(/\s+/g, "");
                    const checkboxID = `containerCategory_${containerID}`;

                    // Init this category if it does not exist
                    if (preferences[containerID] == null) {
                        preferences[containerID] = false;
                    }

                    let margin = "";
                    if (ii !== 0)
                        margin = "mt-2";

                    html += /*html*/`
                        <div class="row m-0 ${margin} p-0 align-items-center">
                            <div class="col-auto p-0">
                                <div class="checkbox" id="${checkboxID}">
                                    ${preferences[containerID] ? `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>` : ""}
                                </div>
                            </div>
                            <div class="col-auto pe-0">${container}</div>
                        </div>
                    `;
                }

                html += `</div>`;

                configurationModalBody.insertAdjacentHTML("beforeend", html);

                ContainerTypes.forEach(container => {
                    const containerID = container.replace(/\s+/g, "");
                    const checkboxID = `containerCategory_${containerID}`;

                    const checkbox = document.getElementById(checkboxID);

                    const onEnabled = () => {
                        preferences[containerID] = true;
                    };

                    const onDisabled = () => {
                        preferences[containerID] = false;
                    };

                    checkbox.addEventListener("click", () => {
                        if (checkbox.innerHTML.trim() === "") {
                            // Enable
                            checkbox.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
                
                            onEnabled();
                        } else {
                            // Disable
                            checkbox.innerHTML = "";
                
                            onDisabled();
                        }

                        // Persist update
                        localStorage.setItem("itemFilterPreferences_containerCategories", JSON.stringify(preferences));
            
                        syncWithBackend();
                    });
                });
            });
        } else {
            el.addEventListener("click", () => {
                let state = false;
                if (el.classList.contains("lootCategoryToggleActive")) {
                    // Disable
                    el.classList.remove("lootCategoryToggleActive");
                } else {
                    // Enable
                    el.classList.add("lootCategoryToggleActive");
                    state = true;
                }
    
                const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));
                const ID = LootCategoryLUT[el.innerHTML];
                // Init this category if it does not exist
                if (preferences[ID] == null) {
                    preferences[ID] = {};
                }
    
                // Update this preference
                preferences[ID].value = state;
    
                // Persist update
                localStorage.setItem("itemFilterPreferences", JSON.stringify(preferences));
    
                syncWithBackend();
            });
        }
    }

    const questItems_ShowAllEl = document.getElementById("questItems_ShowAll");
    questItems_ShowAllEl.addEventListener("click", () => {
        /** @type {bool} */
        let state;
        if (questItems_ShowAllEl.innerHTML === "") {
            // Enable
            questItems_ShowAllEl.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
            state = true;
        } else {
            // Disable
            questItems_ShowAllEl.innerHTML = "";
            state = false;
        }

        const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));
        const ID = "questItems_ShowAll";
        // Init this category if it does not exist
        if (preferences[ID] == null) {
            preferences[ID] = {};
        }

        // Update this preference
        preferences[ID].value = state;

        // Persist update
        localStorage.setItem("itemFilterPreferences", JSON.stringify(preferences));

        syncWithBackend();
    });

    const questItemVisibilityEl = document.getElementById("questItemVisibility");
    questItemVisibilityEl.addEventListener("click", () => {
        /** @type {bool} */
        let state;
        if (questItemVisibilityEl.innerHTML === "") {
            // Enable
            questItemVisibilityEl.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
            state = true;
        } else {
            // Disable
            questItemVisibilityEl.innerHTML = "";
            state = false;
        }

        const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));
        const ID = "questItemVisibility";
        // Init this category if it does not exist
        if (preferences[ID] == null) {
            preferences[ID] = {};
        }

        // Update this preference
        preferences[ID].value = state;

        // Persist update
        localStorage.setItem("itemFilterPreferences", JSON.stringify(preferences));

        syncWithBackend();
    });

    const pricePerSlotEl = document.getElementById("pricePerSlot");
    pricePerSlotEl.addEventListener("click", () => {
        /** @type {bool} */
        let state;
        if (pricePerSlotEl.innerHTML === "") {
            // Enable
            pricePerSlotEl.innerHTML = `<i class="fa-solid fa-check buttonIcon checkboxCheckMark"></i>`;
            state = true;
        } else {
            // Disable
            pricePerSlotEl.innerHTML = "";
            state = false;
        }

        const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));
        const ID = "pricePerSlot";
        // Init this category if it does not exist
        if (preferences[ID] == null) {
            preferences[ID] = {};
        }

        updateLootFilterItemsList();

        // Update this preference
        preferences[ID].value = state;

        // Persist update
        localStorage.setItem("itemFilterPreferences", JSON.stringify(preferences));

        // Re-Price existing loot items
        updatePrices();

        syncWithBackend();
    });

    const highValueThresholdEl = (document.getElementById("highValueThreshold") as HTMLInputElement);
    highValueThresholdEl.addEventListener("keyup", () => {
        const newValue = Number(highValueThresholdEl.value);

        if (highValueThresholdEl.value == null || isNaN(newValue) || newValue > 2000000000 || highValueThresholdEl.value === "") {
            highValueThresholdEl.classList.add("form-control-error");
            return;
        } else {
            highValueThresholdEl.classList.remove("form-control-error");
        }

        const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));
        const ID = "highValueThreshold";
        // Init this category if it does not exist
        if (preferences[ID] == null) {
            preferences[ID] = {};
        }

        // Update this preference
        preferences[ID].value = newValue;

        // Persist update
        localStorage.setItem("itemFilterPreferences", JSON.stringify(preferences));

        syncWithBackend();
    });

    const importantThresholdEl = (document.getElementById("importantThreshold") as HTMLInputElement);
    importantThresholdEl.addEventListener("keyup", () => {
        const newValue = Number(importantThresholdEl.value);

        if (importantThresholdEl.value == null || isNaN(newValue) || newValue > 2000000000 || importantThresholdEl.value === "") {
            importantThresholdEl.classList.add("form-control-error");
            return;
        } else {
            importantThresholdEl.classList.remove("form-control-error");
        }

        const preferences = JSON.parse(localStorage.getItem("itemFilterPreferences"));
        const ID = "importantThreshold";
        // Init this category if it does not exist
        if (preferences[ID] == null) {
            preferences[ID] = {};
        }

        // Update this preference
        preferences[ID].value = newValue;

        // Persist update
        localStorage.setItem("itemFilterPreferences", JSON.stringify(preferences));

        syncWithBackend();
    });
}

export function syncWithBackend() {
    const data = [];

    // Loot categories
    const lootCategoryToggles = Array.from(document.getElementsByClassName("lootCategoryToggle"));
    for (let i = 0; i < lootCategoryToggles.length; i++) {
        const el = lootCategoryToggles[i];

        if (el.classList.contains("lootCategoryToggleRight")) {
            continue;
        }
        
        if (el.classList.contains("lootCategoryToggleActive")) {
            // Enabled
            data.push({
                ID: LootCategoryLUT[el.innerHTML],
                Type: "bool",
                Value: "True",
            });
        } else {
            // Disabled
            data.push({
                ID: LootCategoryLUT[el.innerHTML],
                Type: "bool",
                Value: "False",
            });
        }
    }

    const ContainerTypesPreferences = JSON.parse(localStorage.getItem("itemFilterPreferences_containerCategories"));
    for (let i = 0; i < ContainerTypes.length; i++) {
        const container = ContainerTypes[i];
        
        const containerID = container.replace(/\s+/g, "");
        const savedBool = ContainerTypesPreferences[containerID];

        if (savedBool == null)
            continue;

        data.push({
            ID: container,
            Type: "bool",
            Value: `${ContainerTypesPreferences[containerID]}`,
        });
    }

    const questItemVisibilityEl = document.getElementById("questItemVisibility");
    if (questItemVisibilityEl.innerHTML === "") {
        // Disabled
        data.push({
            ID: "quest",
            Type: "bool",
            Value: "False",
        });
    } else {
        // Enabled
        data.push({
            ID: "quest",
            Type: "bool",
            Value: "True",
        });
    }

    const questItems_ShowAllEl = document.getElementById("questItems_ShowAll");
    if (questItems_ShowAllEl.innerHTML === "") {
        // Disabled
        data.push({
            ID: "questShowAll",
            Type: "bool",
            Value: "False",
        });
    } else {
        // Enabled
        data.push({
            ID: "questShowAll",
            Type: "bool",
            Value: "True",
        });
    }

    const pricePerSlotEl = document.getElementById("pricePerSlot");
    if (pricePerSlotEl.innerHTML === "") {
        // Disabled
        data.push({
            ID: "pps",
            Type: "bool",
            Value: "False",
        });
    } else {
        // Enabled
        data.push({
            ID: "pps",
            Type: "bool",
            Value: "True",
        });
    }

    const highValueThresholdEl = (document.getElementById("highValueThreshold") as HTMLInputElement);
    if (!highValueThresholdEl.classList.contains("form-control-error")) {
        data.push({
            ID: "highValueThreshold",
            Type: "int",
            Value: String(highValueThresholdEl.value),
        });
    }

    const importantThresholdEl = (document.getElementById("importantThreshold") as HTMLInputElement);
    if (!importantThresholdEl.classList.contains("form-control-error")) {
        data.push({
            ID: "importantThreshold",
            Type: "int",
            Value: String(importantThresholdEl.value),
        });
    }

    syncLootFilter(data);
}

async function getFreshLoot() {
    setMainAppStatus("Fetching up-to-date Tarkov data");

    const requestBody = {
        session_token: "bb4f5241-0eb4-416d-9a26-1a2093762391"
    };
    const response = JSON.parse(await POST("https://loot-api.evodma.com/eft-item-data", JSON.stringify(requestBody)));
    const lootDataURI = await GET_Raw(response.uri);
    const lootData = JSON.parse((await inflate(lootDataURI)).toString("utf8"));
    ALL_ITEMS = (lootData as LootItems);

    return { success: true };
}
