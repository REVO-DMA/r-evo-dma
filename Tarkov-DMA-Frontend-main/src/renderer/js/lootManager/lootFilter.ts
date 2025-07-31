import { dialog } from "@electron/remote";
import Pickr from "@simonwep/pickr";
import Clusterize from "clusterize.js";
import * as Color from "color";
import { readFile, writeFile } from "fs/promises";
import Fuse from "fuse.js";
import { homedir } from "os";
import { join as joinPath } from "path";
import sanitize from "sanitize-filename";
import { v4 as uuidv4, validate } from "uuid";
import { showAlert } from "../alert";
import { send as ipcSend } from "../ipc/manager";
import { getItemPriceNumber, getItemPriceString, updateColors } from "../rendering/loot/lootManager";
import { ALL_ITEMS } from "./lootManager";
import { deflate, inflate } from "../utils/zlib";
import { getPickrBaseConfig } from "../colorPicker/config"
import { LootItem } from "../../../../../Loot-API/src/types/eft";

type LootFilterItem = {
    shown: boolean;
    id: string;
};

type LootFilter = {
    id: string;
    filter_name: string;
    enabled: boolean;
    color: number;
    border_color: number;
    text_color: number;
    shape: number;
    items: LootFilterItem[];
};

export type LootFilters = {
    [key: string]: LootFilter;
};

type BackendLootFilter = {
    id: string;
    filter_name: string;
    color: string;
    border_color: string;
    text_color: string;
    shape: number;
    items: string[];
};

const lootFiltersToggleEl = document.getElementById("lootFiltersToggle");

const lootFilters_ItemsTableContainerEl = document.getElementById("lootFilters_ItemsTableContainer");
const lootFilters_itemsListTableBodyEl = document.getElementById("lootFilters_itemsListTableBody");

const lootFilters_itemsListTableSearchEl = (document.getElementById("lootFilters_itemsListTableSearch") as HTMLInputElement);
const itemsListTableClearSearchButtonEl = document.getElementById("itemsListTableClearSearchButton");
const itemsListTableSearchButtonEl = document.getElementById("itemsListTableSearchButton");

let activeLootFilter: string = null;

let loadedLootFilters: LootFilters = null;

let itemFilterColorPicker_main: Pickr;
let itemFilterColorPicker_border: Pickr;
let itemFilterColorPicker_text: Pickr;

const defaultFilterColors = {
    main: "#00A0E9",
    border: "#000000",
    text: "#FFFFFF",
};

let lootFiltersToggleElTimeout: NodeJS.Timeout = null;
lootFiltersToggleEl.addEventListener("click", () => {
    if (lootFiltersToggleElTimeout != null) {
        clearTimeout(lootFiltersToggleElTimeout);
    }

    lootFiltersToggleElTimeout = setTimeout(() => {
        updateLootFilterItemsList();
        lootFiltersToggleElTimeout = null;
    }, 100);
});

// Search on enter
lootFilters_itemsListTableSearchEl.addEventListener("keypress", (e) => {
    if (e.key === "Enter") {
        e.preventDefault();
        itemsListTableSearchButtonEl.click();
    }
});

itemsListTableClearSearchButtonEl.addEventListener("click", () => {
    lootFilters_itemsListTableSearchEl.value = "";
    updateLootFilterItemsList();
});

itemsListTableSearchButtonEl.addEventListener("click", () => {
    updateLootFilterItemsList();
});

const lootFiltersItemsList_Clusterize = new Clusterize({
    rows: null,
    scrollElem: lootFilters_ItemsTableContainerEl,
    contentElem: lootFilters_itemsListTableBodyEl,
    tag: "tr",
    show_no_data_row: false,
    keep_parity: false,
    rows_in_block: 30,
    callbacks: {
        clusterChanged: () => {
            const rows = Array.from(document.getElementsByClassName("lootFilters_allItemsListAddToActiveFilterButton"));
            for (let i = 0; i < rows.length; i++) {
                const row = rows[i];

                row.addEventListener("click", () => {
                    addItemToLootFilter(activeLootFilter, row.getAttribute("bsgid"));
                });
            }
        },
    },
});

const lootFilters_createNewLootFilterEl = document.getElementById("lootFilters_createNewLootFilter");
const lootFilters_importLootFilterEl = document.getElementById("lootFilters_importLootFilter");

const lootFilters_savedFiltersListEl = document.getElementById("lootFilters_savedFiltersList");
/** @type {HTMLInputElement} */
const lootFilters_lootFilterNameInputEl = (document.getElementById("lootFilters_lootFilterNameInput") as HTMLInputElement);
const lootFilters_lootFilterClearAllItemsButtonEl = document.getElementById("lootFilters_lootFilterClearAllItemsButton");
const lootFilters_lootFilterDeleteButtonEl = document.getElementById("lootFilters_lootFilterDeleteButton");
const lootFilters_lootFilterExportButtonEl = document.getElementById("lootFilters_lootFilterExportButton");
const lootFilters_selectedFilterItemListEl = document.getElementById("lootFilters_selectedFilterItemList");

export function initialize() {
    updateLootFilterItemsList();

    lootFilters_createNewLootFilterEl.addEventListener("click", () => {
        resetUI();

        const id = uuidv4();
        saveLootFilter(id, {
            id: id,
            filter_name: "New loot filter",
            enabled: true,
            color: getDecimalFromHex(defaultFilterColors.main),
            border_color: getDecimalFromHex(defaultFilterColors.border),
            text_color: getDecimalFromHex(defaultFilterColors.text),
            shape: 0,
            items: [],
        });

        loadLootFilter(id);
    });

    lootFilters_importLootFilterEl.addEventListener("click", () => {
        importLootFilter();
    });

    lootFilters_lootFilterNameInputEl.addEventListener("keyup", () => {
        setLootFilterName(activeLootFilter, lootFilters_lootFilterNameInputEl.value);
    });

    lootFilters_lootFilterClearAllItemsButtonEl.addEventListener("click", () => {
        if (activeLootFilter == null) return;

        showAlert(
            "question",
            "Loot Filter",
            `Are you sure you want to remove all items from "${lootFilters_lootFilterNameInputEl.value}"?`,
            true,
            false,
            "Remove",
            "",
            "Cancel",
            (result) => {
                if (result) {
                    removeAllItemsFromLootFilter(activeLootFilter);
                }
            }
        );
    });

    lootFilters_lootFilterDeleteButtonEl.addEventListener("click", () => {
        if (activeLootFilter == null) return;

        showAlert("question", "Loot Filter", `Are you sure you want to delete "${lootFilters_lootFilterNameInputEl.value}"?`, true, false, "Delete", "", "Cancel", (result) => {
            if (result) {
                deleteLootFilter(activeLootFilter);
            }
        });
    });

    lootFilters_lootFilterExportButtonEl.addEventListener("click", () => {
        if (activeLootFilter == null) return;

        exportLootFilter(activeLootFilter);
    });

    loadedLootFilters = getSavedLootFilters();

    // Main
    itemFilterColorPicker_main = Pickr.create({
        default: defaultFilterColors.main,
        ...getPickrBaseConfig(".itemFilterColorPicker_main"),
    });
    itemFilterColorPicker_main.on("changestop", () => {
        setLootFilterColors(activeLootFilter);
    });
    // Border
    itemFilterColorPicker_border = Pickr.create({
        default: defaultFilterColors.border,
        ...getPickrBaseConfig(".itemFilterColorPicker_border"),
    });
    itemFilterColorPicker_border.on("changestop", () => {
        setLootFilterColors(activeLootFilter);
    });
    // Text
    itemFilterColorPicker_text = Pickr.create({
        default: defaultFilterColors.text,
        ...getPickrBaseConfig(".itemFilterColorPicker_text"),
    });
    itemFilterColorPicker_text.on("changestop", () => {
        setLootFilterColors(activeLootFilter);
    });

    loadLootFiltersList();

    syncAllWithBackend();
}

export function getItemColor(BSG_ID?: string)
{
    if (BSG_ID == null) return null;

    const Colors = {
        Main: "#F403FC",
        Border: "#F403FC",
        Text: "#F403FC",
    };

    let itemFound = false;
    for (const key in loadedLootFilters) {
        const lootFilter = loadedLootFilters[key];

        if (!lootFilter.enabled) continue;

        const items = lootFilter.items;

        for (let i = 0; i < items.length; i++) {
            const item = items[i];

            if (item.id === BSG_ID) {
                itemFound = true;
                Colors.Main = getHexFromDecimal(lootFilter.color);
                Colors.Border = getHexFromDecimal(lootFilter.border_color);
                Colors.Text = getHexFromDecimal(lootFilter.text_color);
                break;
            }
        }

        if (itemFound) break;
    }

    if (itemFound) return Colors;
    else return null;
}

function getSavedLootFilters()
{
    const savedFilters = localStorage.getItem("lootFilters");

    // Initialize localStorage
    if (savedFilters == null) {
        localStorage.setItem("lootFilters", JSON.stringify({}));

        return {};
    } else {
        return JSON.parse(savedFilters);
    }
}

function getLootFilter(id: string)
{
    const lootFilter = loadedLootFilters[id];

    if (lootFilter == null) return null;
    else return lootFilter;
}

async function importLootFilter()
{
    const openDialog = await dialog.showOpenDialog(null, {
        title: "Import Loot Filter",
        defaultPath: joinPath(homedir(), "Desktop"),
        properties: ["openFile", "multiSelections"],
    });

    if (openDialog.canceled || openDialog.filePaths.length === 0) return;

    let filterData: LootFilter = null;

    let shouldCheckID = false;

    const fromAtomicToEVO = (items: any[]) => {
        const newItems: LootFilterItem[] = [];

        items.forEach((item) => {
            newItems.push({ shown: true, id: item });
        });

        return newItems;
    };

    const showImportError = async (index: number, total: number) => {
        await showAlert("error", `Loot Filter (${index}/${total})`, `Error while importing Loot Filter!`, false, false, "Close", "", "", null, () => {});
    };

    for (let i = 0; i < openDialog.filePaths.length; i++) {
        const path = openDialog.filePaths[i];
        
        try {
            const rawData = await readFile(path);
            const inflated = await inflate(rawData);
            if (inflated == null) {
                try {
                    const parsedData = JSON.parse(rawData.toString());
                    const newItems = fromAtomicToEVO(parsedData.items);
    
                    // Assign new items format
                    parsedData.items = newItems;
    
                    // Fix properties
                    parsedData.id = uuidv4();
                    parsedData.enabled = true;
    
                    filterData = parsedData;
                } catch (error) {
                    console.error(`importLootFilter() -> Unable to parse imported data as JSON: ${error}`);
                    filterData = null;
                }
            } else {
                filterData = JSON.parse(inflated.toString("utf8"));
                shouldCheckID = true;
            }
        } catch (error) {
            console.error(`importLootFilter() -> Unable to load imported data: ${error}`);
            filterData = null;
        }
    
        if (filterData == null) {
            await showImportError(i + 1, openDialog.filePaths.length);
            continue;
        }
    
        if (shouldCheckID && !validate(filterData.id)) {
            await showImportError(i + 1, openDialog.filePaths.length);
            continue;
        }
    
        const finalize = () => {
            filterData.enabled = true;
    
            saveLootFilter(filterData.id, filterData);
    
            // Only load the last one
            if (i == openDialog.filePaths.length - 1) {
                loadLootFilter(filterData.id);
            } else if (filterData.id === activeLootFilter) {
                reloadLootFilter();
            }
        };
    
        // Prompt the user if a Loot Filter with the same id is already imported
        const existingFilter = loadedLootFilters[filterData.id];
        if (existingFilter != null) {
            await showAlert(
                "question",
                `Loot Filter (${i + 1}/${openDialog.filePaths.length})`,
                `A Loot Filter with the name "${existingFilter.filter_name}" has the same ID. Would you like to overwrite it?`,
                true,
                false,
                "Overwrite",
                "",
                "Cancel",
                null,
                (result) => {
                    if (result) {
                        finalize();
                    }
                }
            );
        } else {
            finalize();
        }
    }
}

async function exportLootFilter(id: string) {
    const lootFilter = getLootFilter(id);
    if (lootFilter == null) return;

    const saveDialog = await dialog.showSaveDialog(null, {
        title: "Export Loot Filter",
        defaultPath: joinPath(homedir(), "Desktop", `${sanitize(lootFilter.filter_name)}.loot`),
    });

    if (saveDialog.canceled || saveDialog.filePath == null) return;

    const compressedData = await deflate(JSON.stringify(lootFilter));
    if (compressedData == null) {
        showAlert("error", "Loot Filter", "Error while packing Loot Filter data.", false, false, "Close", "", "");

        return;
    }

    await writeFile(saveDialog.filePath, compressedData);
}

function saveLootFilter(id: string, data: LootFilter, skipSync: boolean = false) {
    loadedLootFilters[id] = data;

    localStorage.setItem("lootFilters", JSON.stringify(loadedLootFilters));

    loadLootFiltersList();

    if (!skipSync) syncSpecificWithBackend(id);
}

function deleteLootFilter(id: string) {
    if (loadedLootFilters[id] == null) return;

    delete loadedLootFilters[id];

    activeLootFilter = null;

    resetUI();

    localStorage.setItem("lootFilters", JSON.stringify(loadedLootFilters));

    syncAllWithBackend();

    loadLootFiltersList();
}

function removeAllItemsFromLootFilter(filterID: string) {
    if (loadedLootFilters[filterID] == null) return;

    loadedLootFilters[filterID].items = [];

    saveLootFilter(filterID, loadedLootFilters[filterID]);

    loadLootFilter(activeLootFilter);
}

function addItemToLootFilter(filterID: string, itemID: string) {
    if (loadedLootFilters[filterID] == null) return;

    // Make sure this item is not already in the filter
    const index = loadedLootFilters[filterID].items
        .map((e) => {
            return e.id;
        })
        .indexOf(itemID);
    console.log(index);
    if (index > -1) return;

    loadedLootFilters[filterID].items.push({ shown: true, id: itemID });

    saveLootFilter(filterID, loadedLootFilters[filterID]);

    loadLootFilter(activeLootFilter);
}

function removeItemFromLootFilter(filterID: string, itemID: string) {
    if (loadedLootFilters[filterID] == null) return;

    const index = loadedLootFilters[filterID].items
        .map((e) => {
            return e.id;
        })
        .indexOf(itemID);
    if (index !== -1) {
        loadedLootFilters[filterID].items.splice(index, 1);
    }

    saveLootFilter(filterID, loadedLootFilters[filterID]);
}

function setLootFilterState(filterID: string, newState: boolean) {
    if (loadedLootFilters[filterID] == null) return;

    loadedLootFilters[filterID].enabled = newState;

    saveLootFilter(filterID, loadedLootFilters[filterID], true);

    syncAllWithBackend();
}

function setLootFilterName(filterID: string, newName: string) {
    if (loadedLootFilters[filterID] == null) return;

    loadedLootFilters[filterID].filter_name = newName;

    saveLootFilter(filterID, loadedLootFilters[filterID]);
}

function setLootFilterItemVisibility(filterID: string, itemID: string, shown: boolean) {
    if (loadedLootFilters[filterID] == null) return;

    const index = loadedLootFilters[filterID].items
        .map((e) => {
            return e.id;
        })
        .indexOf(itemID);
    if (index !== -1) {
        loadedLootFilters[filterID].items[index].shown = shown;
    }

    saveLootFilter(filterID, loadedLootFilters[filterID]);
}

function setLootFilterColors(filterID: string) {
    if (loadedLootFilters[filterID] == null) return;

    const main = getDecimalFromPickr(itemFilterColorPicker_main);
    const border = getDecimalFromPickr(itemFilterColorPicker_border);
    const text = getDecimalFromPickr(itemFilterColorPicker_text);

    loadedLootFilters[filterID].color = main;
    loadedLootFilters[filterID].border_color = border;
    loadedLootFilters[filterID].text_color = text;

    saveLootFilter(filterID, loadedLootFilters[filterID]);
}

function syncAllWithBackend() {
    const lootFilters = [];

    // Transform format
    const entries = Object.entries(loadedLootFilters);
    for (const [key, lootFilter] of entries) {
        // Skip disabled filters
        if (lootFilter.enabled === false) continue;

        const shownItems = [];

        const filterItems = lootFilter.items;
        // Add all shown items to shownItems[]
        for (let i = 0; i < filterItems.length; i++) {
            const filterItem = filterItems[i];

            if (filterItem.shown) shownItems.push(filterItem.id);
        }

        /** @type {BackendLootFilter} */
        const obj = {
            ...lootFilter,
            color: getHexFromDecimal(lootFilter.color),
            border_color: getHexFromDecimal(lootFilter.border_color),
            text_color: getHexFromDecimal(lootFilter.text_color),
            items: shownItems,
        };

        lootFilters.push(obj);
    }

    // Re-Color existing loot items
    updateColors();

    ipcSend(36, lootFilters);
}

function syncSpecificWithBackend(id: string) {
    const lootFilter = getLootFilter(id);

    if (lootFilter == null || lootFilter.enabled === false) return;

    /** @type {string[]} */
    const shownItems = [];

    const filterItems = lootFilter.items;
    // Add all shown items to shownItems[]
    for (let i = 0; i < filterItems.length; i++) {
        const filterItem = filterItems[i];

        if (filterItem.shown) shownItems.push(filterItem.id);
    }

    const obj: BackendLootFilter = {
        ...lootFilter,
        color: getHexFromDecimal(lootFilter.color),
        border_color: getHexFromDecimal(lootFilter.border_color),
        text_color: getHexFromDecimal(lootFilter.text_color),
        items: shownItems,
    };

    // Re-Color existing loot items
    updateColors();

    ipcSend(35, obj);
}

function loadLootFiltersList() {
    const lootFilters = [];

    const entries = Object.entries(loadedLootFilters);
    /** @type {string[]} */
    const filterIDs = [];
    for (const [key, lootFilter] of entries) {
        lootFilters.push(/*html*/ `
            <tr filterid="${lootFilter.id}" class="${lootFilter.id === activeLootFilter ? "lootFilters_savedFiltersListActiveFilter" : ""}">
                <td>${lootFilter.filter_name}</td>
                <td><div class="col-auto lootFilters_itemsListButton lootFilters_savedFiltersListEditFilterButton">${
                    lootFilter.id === activeLootFilter ? "Editing" : "Edit"
                }</div></td>
                <td><div class="col-auto lootFilters_itemsListButton lootFilters_savedFiltersListToggleStateButton">${lootFilter.enabled === true ? "Disable" : "Enable"}</div></td>
            </tr>
        `);

        filterIDs.push(lootFilter.id);
    }

    lootFilters_savedFiltersListEl.innerHTML = lootFilters.join("");

    // Attach events

    // Edit
    const editButtons = Array.from(document.getElementsByClassName("lootFilters_savedFiltersListEditFilterButton"));
    for (let i = 0; i < filterIDs.length; i++) {
        const filterID = filterIDs[i];
        editButtons[i].addEventListener("click", () => {
            loadLootFilter(filterID);
        });
    }

    // State
    const stateButtons = Array.from(document.getElementsByClassName("lootFilters_savedFiltersListToggleStateButton"));
    for (let i = 0; i < stateButtons.length; i++) {
        const button: HTMLButtonElement = (stateButtons[i] as HTMLButtonElement);

        button.addEventListener("click", () => {
            const filterID = button.closest("tr").getAttribute("filterid");

            if (button.innerText === "Enable") {
                // Hide
                button.innerText = "Disable";
                setLootFilterState(filterID, true);
            } else {
                // Show
                button.innerText = "Enable";
                setLootFilterState(filterID, false);
            }
        });
    }
}

/**
 * Reloads the currently selected loot filter.
 */
function reloadLootFilter() {
    loadLootFilter(activeLootFilter);
}

function loadLootFilter(filterID: string) {
    const filter = getLootFilter(filterID);

    if (filter == null) return; // todo: show an error message

    resetUI();

    // Visibly select this filter in the filters list - "deselect" all others
    const editButtons = Array.from(document.getElementsByClassName("lootFilters_savedFiltersListEditFilterButton"));
    for (let i = 0; i < editButtons.length; i++) {
        const button: HTMLButtonElement = (editButtons[i] as HTMLButtonElement);
        const tr = button.closest("tr");
        const rowFilterID = tr.getAttribute("filterid");
        if (rowFilterID === filterID) {
            button.innerText = "Editing";
            tr.classList.add("lootFilters_savedFiltersListActiveFilter");
        } else {
            tr.classList.remove("lootFilters_savedFiltersListActiveFilter");
            button.innerText = "Edit";
        }
    }

    activeLootFilter = filterID;

    lootFilters_lootFilterNameInputEl.value = filter.filter_name;

    const itemsTbody = [];
    const items = filter.items;
    const eftItems = ALL_ITEMS;
    const ppsEnabled = isPPSActive();
    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        const eftItem = eftItems[item.id];

        // Skip if this item does not exist
        if (eftItem == null) continue;

        const price = getItemPriceString(eftItem.id);

        itemsTbody.push(/*html*/ `
            <tr bsgid="${eftItem.id}">
                <td><div class="col-auto lootFilters_itemsListButton lootFilters_itemsListItemVisibilityButton">${item.shown === true ? "Hide" : "Show"}</div></td>
                <td>${eftItem.name}</td>
                <td>${price}</td>
                <td><div class="col-auto lootFilters_itemsListButton lootFilters_itemsListRemoveItemButton">Remove</div></td>
            </tr>
        `);
    }

    lootFilters_selectedFilterItemListEl.innerHTML = itemsTbody.join("");

    // Load colors
    itemFilterColorPicker_main.setColor(getHexFromDecimal(filter.color));
    itemFilterColorPicker_border.setColor(getHexFromDecimal(filter.border_color));
    itemFilterColorPicker_text.setColor(getHexFromDecimal(filter.text_color));

    // Attach events

    // Visibility
    const visibilityButtons = Array.from(document.getElementsByClassName("lootFilters_itemsListItemVisibilityButton"));
    for (let i = 0; i < visibilityButtons.length; i++) {
        const button: HTMLButtonElement = (visibilityButtons[i] as HTMLButtonElement);

        button.addEventListener("click", () => {
            const ID = button.closest("tr").getAttribute("bsgid");

            if (button.innerText === "Show") {
                // Hide
                button.innerText = "Hide";
                setLootFilterItemVisibility(activeLootFilter, ID, true);
            } else {
                // Show
                button.innerText = "Show";
                setLootFilterItemVisibility(activeLootFilter, ID, false);
            }
        });
    }

    // Remove
    const removeButtons = Array.from(document.getElementsByClassName("lootFilters_itemsListRemoveItemButton"));
    for (let i = 0; i < removeButtons.length; i++) {
        const button = removeButtons[i];

        button.addEventListener("click", () => {
            const tr = button.closest("tr");
            const ID = tr.getAttribute("bsgid");
            removeItemFromLootFilter(activeLootFilter, ID);
            // Remove row from dom
            tr.remove();
        });
    }
}

function getDecimalFromPickr(pickr: Pickr) {
    return Color(pickr.getColor().toHEXA().toString()).rgbNumber();
}

function getDecimalFromHex(hex: string) {
    return Color(hex).rgbNumber();
}

function getHexFromDecimal(decimal: number) {
    return Color(decimal).hex();
}

// UI Related functions
function resetUI() {
    // Clear filter name
    lootFilters_lootFilterNameInputEl.value = "";

    // Clear filter item list
    lootFilters_selectedFilterItemListEl.innerHTML = "";
}

export function isPPSActive() {
    const pricePerSlotEl = document.getElementById("pricePerSlot");

    if (pricePerSlotEl.innerHTML !== "") return true;
    else return false;
}

export function updateLootFilterItemsList() {
    const searchQuery = lootFilters_itemsListTableSearchEl.value;

    const items = ALL_ITEMS;

    // Sort items by price - descending
    const sortedItems = Object.keys(items)
        .map((key) => ({ key, price: getItemPriceNumber(key) }))
        .sort((item1, item2) => item2.price - item1.price)
        .map((item) => items[item.key]);

    // Filter based on search query
    let filteredItems: LootItem[] = [];
    if (searchQuery != null && searchQuery !== "") {
        const fuse = new Fuse(sortedItems, {
            findAllMatches: true,
            threshold: 0.1,
            ignoreLocation: true,
            keys: ["name"],
        });

        const searchResults = fuse.search(searchQuery);

        // Change fuse result format back to normal
        searchResults.forEach((result) => {
            filteredItems.push(result.item);
        });
    } else {
        filteredItems = sortedItems;
    }

    // Generate table markup
    const displayItems = [];
    for (let i = 0; i < filteredItems.length; i++) {
        const item = filteredItems[i];

        displayItems.push(/*html*/ `
            <tr>
                <td>${item.name}</td>
                <td>${getItemPriceString(item.id)}</td>
                <td><div class="col-auto lootFilters_itemsListButton lootFilters_allItemsListAddToActiveFilterButton" bsgid="${item.id}">Add</div></td>
            </tr>
        `);
    }

    lootFiltersItemsList_Clusterize.update(displayItems);
}
