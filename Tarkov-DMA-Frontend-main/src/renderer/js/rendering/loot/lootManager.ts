import { TraderPrice } from "../../../../../../Loot-API/src/types/eft";
import { isPPSActive } from "../../lootManager/lootFilter";
import { ALL_ITEMS } from "../../lootManager/lootManager";
import { localPlayer } from "../player/playerManager";
import { Loot } from "./loot";

const itemsTableBodyEl = document.getElementById("itemsTableBody");
const itemsTableTableEl = document.getElementById("itemsTableTable");

export type LootPosition = [X: number, Y: number, Height: number];

type LootItem = [ID: number, Shown: boolean, BSG_ID: string, Type: number, Name: string, Position: LootPosition];

export let filteredLootItems: { [n: number]: Loot; } = {};

let allLootItems: { [n: number]: LootItem; } = {};

// This represents the current loot refresh iteration. It is used to determine if an item should be removed from the items object.
let currentLootVersion = 0;

export function onRaidEnd() {
    // Destroy all loot items
    for (const key in filteredLootItems) {
        const lootItem = filteredLootItems[key];

        if (lootItem == null) continue;

        lootItem.destroy();
    }

    filteredLootItems = {};
    allLootItems = {};
    currentLootVersion = 0;
    itemsTableTableEl.style.display = "none";
    itemsTableBodyEl.innerHTML = "";
}

/**
 * @param {} newLootItems
 */
export function update(newLootItems: LootItem[]) {
    // Bump refresh iteration
    currentLootVersion++;

    // Clear all items array (only used for the loot table)
    allLootItems = {};

    let localPlayerHeight = 0;
    if (localPlayer != null) localPlayerHeight = localPlayer.DynamicData.Height;

    for (let i = 0; i < newLootItems.length; i++) {
        const newLootItem = newLootItems[i];
        const key = newLootItem[0];

        if (newLootItem[1] === true) {
            // Shown item
            if (filteredLootItems[key] == null) {
                // Add new item
                filteredLootItems[key] = new Loot(newLootItem[2], newLootItem[3], newLootItem[4], newLootItem[5], currentLootVersion);
            } else {
                // Update existing item
                filteredLootItems[key].updateData(localPlayerHeight, currentLootVersion, newLootItem[4]);
            }
        } else {
            // Hidden item (only on loot table)
            allLootItems[key] = newLootItem;
        }
    }

    // Remove stale items
    for (const key in filteredLootItems) {
        const lootItem = filteredLootItems[key];

        if (lootItem == null) continue;

        if (lootItem.dynamicData.CurrentLootVersion != currentLootVersion) {
            lootItem.destroy();
            delete filteredLootItems[key];
        }
    }

    const ppsEnabled = isPPSActive();

    // Regenerate the loot table
    const lootTableHTML = [];
    for (const key in allLootItems) {
        const lootItem = allLootItems[key];

        if (lootItem == null) continue;

        let itemPrice = getItemPriceString(lootItem[2]);
        if (itemPrice === "0") continue;

        lootTableHTML.push(/*html*/ `
            <tr>
                <td></td>
                <td>${lootItem[4]}</td>
                <td>${itemPrice}</td>
            </tr>
        `);
    }

    itemsTableBodyEl.innerHTML = lootTableHTML.join("");
    itemsTableTableEl.style.display = "";
}

export function scaleLoot(x: number, y: number) {
    for (const key in filteredLootItems) {
        const lootItem = filteredLootItems[key];

        if (lootItem == null) continue;

        lootItem.setScale(x, y);
    }
}

export function updateColors() {
    for (const key in filteredLootItems) {
        const lootItem = filteredLootItems[key];

        if (lootItem == null) continue;

        lootItem.updateColor();
    }
}

export function updatePrices() {
    for (const key in filteredLootItems) {
        const lootItem = filteredLootItems[key];

        if (lootItem == null) continue;

        lootItem.updatePrice();
    }
}

export function getItemPriceString(id: string)
{
    const item = ALL_ITEMS[id];
    
    if (item == null || !item.itemPrice.hasPrice)
        return "0";

    let price: TraderPrice;
    if (item.itemPrice.sell.hasPrice)
        price = item.itemPrice.sell.highestPrice;
    else
        price = item.itemPrice.buy.lowestPrice;

    if (isPPSActive())
        return price.ppsStr;
    else
        return price.priceStr;
}

export function getItemPriceNumber(id: string)
{
    const item = ALL_ITEMS[id];
    
    if (item == null || !item.itemPrice.hasPrice)
        return 0;

    let price: TraderPrice;
    if (item.itemPrice.sell.hasPrice)
        price = item.itemPrice.sell.highestPrice;
    else
        price = item.itemPrice.buy.lowestPrice;

    if (isPPSActive())
        return price.pricePerSlot;
    else
        return price.price;
}
