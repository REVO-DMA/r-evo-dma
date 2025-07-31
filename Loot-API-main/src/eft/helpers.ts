import { GetLootQuery, ItemType, Item as LootItem, ItemPrice as LootQueryItemPrice } from "../gql/graphql";
import { LootItems, ItemCategories, TraderPrice, ItemPrice, Crafts, BuyPrice, SellPrice, PriceComparison } from "../types/eft";
import { Result } from "@badrap/result";
import { formatPrice, shouldSkipItem } from "./utils";
import globals from "../globals";

export function process(data: GetLootQuery)
{
    try
    {
        const processedData = processItemData(data);
        return Result.ok(processedData);
    } catch (err)
    {
        return Result.err(err);
    }
}

function processItemData(data: GetLootQuery): LootItems
{
    let lootItems: LootItems = {};
    let processedCount = 0;

    for (let i = 0; i < data.items.length; i++) {
        const item = data.items[i];
        
        if (shouldSkipItem((item as LootItem)))
            continue;

        const itemPrice = getItemPrice((item as LootItem));
        const categories = getItemCategories((item as LootItem));

        lootItems[item.id] = {
            id: item.id,
            name: item.name,
            shortName: item.shortName,
            width: item.width,
            height: item.height,
            categories: categories,
            itemPrice: itemPrice
        };

        processedCount++;
    }

    lootItems = applyCraftPrices(data, lootItems);
    lootItems = fixAmmoPacks(data, lootItems);
    lootItems = fixLabsKeyCards(data, lootItems);

    console.log(`Processed ${processedCount} items!`);

    return lootItems;
}

/**
 * Fixes the cost of craftable items instead of using the trader price.
 * This allows us to show the true value of the item if a craft exists for it.
 */
function applyCraftPrices(data: GetLootQuery, lootItems: LootItems): LootItems
{
    const crafts: Crafts = {};

    // Build database of craft costs per item
    data.crafts.forEach(craft => {
        let totalCraftCost = 0;

        for (let i = 0; i < craft.requiredItems.length; i++) {
            const requiredItem = craft.requiredItems[i];

            const item = lootItems[requiredItem.item.id];
            if (item == null)
                continue;

            let price = 0;
            if (item.itemPrice.buy.hasPrice)
                price = item.itemPrice.buy.lowestPrice.price;
            if (item.itemPrice.sell.hasPrice)
                price = item.itemPrice.sell.highestPrice.price;

            totalCraftCost += price * requiredItem.count;
        }

        for (let i = 0; i < craft.rewardItems.length; i++) {
            const rewardItem = craft.rewardItems[i];
            
            const item = lootItems[rewardItem.item.id];
            if (item == null)
                continue;

            // Make sure we aren't overriding a lower price
            const existingEntry = crafts[item.id];
            if (existingEntry != null &&
                existingEntry.totalCost < totalCraftCost
            )
            {
                continue;
            }

            crafts[rewardItem.item.id] = {
                totalCost: totalCraftCost / rewardItem.count
            };
        }
    });

    for (let i = 0; i < data.items.length; i++) {
        const itemRaw = (data.items[i] as LootItem);
        
        if (shouldSkipItem(itemRaw))
            continue;

        // Only apply craft prices to flea banned items
        const isFleaBanned = isItemFleaBanned(itemRaw);
        if (!isFleaBanned)
            continue;

        const item = lootItems[itemRaw.id];
        if (item == null)
            continue;

        const craft = crafts[item.id];
        if (craft == null)
            continue;

        if (craft.totalCost > item.itemPrice.sell.highestPrice.price)
        {
            item.itemPrice.sell.highestPrice.name = "Craft";

            item.itemPrice.sell.highestPrice.price = Math.ceil(craft.totalCost);
            item.itemPrice.sell.highestPrice.priceStr = formatPrice(craft.totalCost);

            const ppsPrice = Math.ceil(craft.totalCost / (item.width * item.height));
            item.itemPrice.sell.highestPrice.pricePerSlot = ppsPrice;
            item.itemPrice.sell.highestPrice.ppsStr = formatPrice(ppsPrice);
        }
    }

    return lootItems;
}

/**
 * Fixes the pricing of ammo packs to reflect the total cost of all contained ammo.
 */
function fixAmmoPacks(data: GetLootQuery, lootItems: LootItems): LootItems
{
    for (let i = 0; i < data.items.length; i++) {
        const item = data.items[i];
        
        if (shouldSkipItem((item as LootItem)))
            continue;

        const processedItem = lootItems[item.id];
        if (processedItem == null)
            continue;

        if (processedItem.categories.isAmmoPack &&
            item.containsItems != null)
        {
            const containedItems = item.containsItems;
            let totalPrice = 0;
            let totalCount = 0;
            for (let ii = 0; ii < containedItems.length; ii++) {
                const containedItem = containedItems[ii];
                
                const item = lootItems[containedItem.item.id];
                if (item == null)
                    continue;

                let price = item.itemPrice.sell.highestPrice.price;
                if (item.itemPrice.buy.lowestPrice.price > price)
                    price = item.itemPrice.buy.lowestPrice.price;

                totalPrice += price * containedItem.count;
                totalCount += containedItem.count;
            }

            processedItem.shortName = `${processedItem.shortName} (${totalCount} pcs)`;
            
            processedItem.itemPrice.sell.highestPrice.price = Math.ceil(totalPrice);
            processedItem.itemPrice.sell.highestPrice.priceStr = formatPrice(totalPrice);
            
            const ppsPrice = Math.ceil(totalPrice / (item.width * item.height));
            processedItem.itemPrice.sell.highestPrice.pricePerSlot = ppsPrice;
            processedItem.itemPrice.sell.highestPrice.ppsStr = formatPrice(ppsPrice);
        }
    }

    return lootItems;
}

/**
 * Fixes the shortNames of Labs colored and access cards.
 */
function fixLabsKeyCards(data: GetLootQuery, lootItems: LootItems): LootItems
{
    for (let i = 0; i < data.items.length; i++) {
        const item = data.items[i];
        
        if (shouldSkipItem((item as LootItem)))
            continue;

        const processedItem = lootItems[item.id];
        if (processedItem == null)
            continue;

        const comparisonStr = processedItem.name.toLowerCase();
        if (comparisonStr.includes("labs keycard") ||
        comparisonStr.includes("access keycard"))
        {
            processedItem.shortName = `${processedItem.shortName} keycard`;
        }
    }

    return lootItems;
}

/**
 * Categorizes the given item.
 */
function getItemCategories(item: LootItem): ItemCategories
{
    const itemCategories: ItemCategories = {
        isKey: false,
        isBarterItem: false,
        isContainer: false,
        isFood: false,
        isDrink: false,
        isMedical: false,
        isSpecialEquipment: false,
        isMap: false,
        isAmmo: false,
        isAmmoPack: false,
        isCurrency: false,
        isRepairKit: false,
        isOther: false,
        isSight: false,
        isGear: false,
        isWeapon: false,
        isMeleeWeapon: false,
        isThrowable: false,
        isWeaponPart: false,
    }

    item.categories.forEach(category => {
        const name = category.normalizedName;
    
        if (name === "key")
            itemCategories.isKey = true;
        else if (name === "barter-item")
            itemCategories.isBarterItem = true;
        else if (name === "locking-container")
            itemCategories.isContainer = true;
        else if (name === "food")
            itemCategories.isFood = true;
        else if (name === "drink")
            itemCategories.isDrink = true;
        else if (name === "meds")
            itemCategories.isMedical = true;
        else if (name === "special-item")
        {
            itemCategories.isSpecialEquipment = true;
            itemCategories.isOther = true;
        }
        else if (name === "map")
        {
            itemCategories.isMap = true;
            itemCategories.isOther = true;
        }
        else if (name === "money")
        {
            itemCategories.isCurrency = true;
            itemCategories.isOther = true;
        }
        else if (name === "repair-kits")
        {
            itemCategories.isRepairKit = true;
            itemCategories.isOther = true;
        }
        else if (name === "ammo")
            itemCategories.isAmmo = true;
        else if (name === "ammo-container")
            itemCategories.isAmmoPack = true;
        else if (name === "sights")
            itemCategories.isSight = true;
        else if (name === "equipment")
            itemCategories.isGear = true;
        else if (name === "weapon")
            itemCategories.isWeapon = true;
        else if (name === "knife")
            itemCategories.isMeleeWeapon = true;
        else if (name === "throwable-weapon")
            itemCategories.isThrowable = true;
        else if (name === "weapon-mod")
            itemCategories.isWeaponPart = true;
    });
    
    item.handbookCategories.forEach(category => {
        const name = category.normalizedName;
    
        if (name === "keys")
            itemCategories.isKey = true;
        else if (name === "barter-items")
            itemCategories.isBarterItem = true;
        else if (name === "storage-containers")
            itemCategories.isContainer = true;
        else if (name === "food")
            itemCategories.isFood = true;
        else if (name === "drinks")
            itemCategories.isDrink = true;
        else if (name === "medication")
            itemCategories.isMedical = true;
        else if (name === "special-equipment")
        {
            itemCategories.isSpecialEquipment = true;
            itemCategories.isOther = true;
        }
        else if (name === "maps")
        {
            itemCategories.isMap = true;
            itemCategories.isOther = true;
        }
        else if (name === "money")
        {
            itemCategories.isCurrency = true;
            itemCategories.isOther = true;
        }
        else if (name === "ammo")
            itemCategories.isAmmo = true;
        else if (name === "sights")
            itemCategories.isSight = true;
        else if (name === "gear")
            itemCategories.isGear = true;
        else if (name === "weapons")
            itemCategories.isWeapon = true;
        else if (name === "melee-weapons")
            itemCategories.isMeleeWeapon = true;
        else if (name === "throwables")
            itemCategories.isThrowable = true;
        else if (name === "weapon-parts-mods")
            itemCategories.isWeaponPart = true;
    });

    return itemCategories;
}

/**
 * Determines whether or not the item is banned from the flea market.
 */
function isItemFleaBanned(item: LootItem): boolean
{
    for (let i = 0; i < item.types.length; i++) {
        const type = item.types[i];
        
        if (type == ItemType.NoFlea)
            return true;
    }

    return false;
}

function getFleaPrice(item: LootItem, itemPrices: LootQueryItemPrice[]): TraderPrice
{
    const fleaPrice: TraderPrice = {
        name: "Flea Market",
        price: globals.general.defaultPrice,
        priceStr: "",
        pricePerSlot: globals.general.defaultPrice,
        ppsStr: "",
    };

    itemPrices?.forEach(itemPrice => {
        if (itemPrice.priceRUB != null &&
            itemPrice.vendor.name === "Flea Market")
        {
            fleaPrice.price = Math.ceil(itemPrice.priceRUB);
            fleaPrice.priceStr = formatPrice(itemPrice.priceRUB);

            const ppsPrice = itemPrice.priceRUB / (item.width * item.height);
            fleaPrice.pricePerSlot = Math.ceil(ppsPrice);
            fleaPrice.ppsStr = formatPrice(ppsPrice);
        }
    });

    return fleaPrice;
}

function getTraderPrice(item: LootItem, itemPrices: LootQueryItemPrice[], comparison: PriceComparison): TraderPrice
{
    const traderPrice: TraderPrice = {
        name: "N/A",
        price: globals.general.defaultPrice,
        priceStr: "",
        pricePerSlot: globals.general.defaultPrice,
        ppsStr: "",
    };

    const setPrice = (itemPrice: LootQueryItemPrice) => {
        const pps = itemPrice.priceRUB / (item.width * item.height);

        traderPrice.name = itemPrice.vendor.name;
        traderPrice.price = Math.ceil(itemPrice.priceRUB);
        traderPrice.priceStr = formatPrice(itemPrice.priceRUB);
        traderPrice.pricePerSlot = Math.ceil(pps);
        traderPrice.ppsStr = formatPrice(pps);
    };

    itemPrices?.forEach(itemPrice => {
        if (itemPrice.priceRUB != null && itemPrice.vendor.name !== "Flea Market")
        {
            const skipComparison = traderPrice.price === globals.general.defaultPrice;

            if (comparison === PriceComparison.GT)
            {
                if (itemPrice.priceRUB > traderPrice.price || skipComparison)
                    setPrice(itemPrice);
            }
            else if (comparison === PriceComparison.LT)
            {
                if (itemPrice.priceRUB < traderPrice.price || skipComparison)
                    setPrice(itemPrice);
            }
        }
    });

    return traderPrice;
}

/**
 * Gets the sell price data for the given item.
 */
function getSellPrice(item: LootItem): SellPrice
{
    const fleaPrice = getFleaPrice(item, item.sellFor);
    const traderPrice = getTraderPrice(item, item.sellFor, PriceComparison.GT);

    return {
        highestPrice: (fleaPrice.price > traderPrice.price ? fleaPrice : traderPrice),
        hasPrice: (fleaPrice.price !== globals.general.defaultPrice || traderPrice.price !== globals.general.defaultPrice ? true : false),
    };
}

/**
 * Gets the buy price data for the given item.
 */
function getBuyPrice(item: LootItem): BuyPrice
{
    const fleaPrice = getFleaPrice(item, item.buyFor);
    const traderPrice = getTraderPrice(item, item.buyFor, PriceComparison.LT);

    return {
        lowestPrice: (fleaPrice.price !== globals.general.defaultPrice && fleaPrice.price < traderPrice.price ? fleaPrice : traderPrice),
        hasPrice: (fleaPrice.price !== globals.general.defaultPrice || traderPrice.price !== globals.general.defaultPrice ? true : false),
    };
}

/**
 * Gets all of the pricing data for the given item.
 */
function getItemPrice(item: LootItem): ItemPrice
{
    const sellPrices = getSellPrice(item);
    const buyPrices = getBuyPrice(item);

    const hasPrice = sellPrices.hasPrice || buyPrices.hasPrice;

    return {
        sell: sellPrices,
        buy: buyPrices,
        hasPrice: hasPrice
    };
}
