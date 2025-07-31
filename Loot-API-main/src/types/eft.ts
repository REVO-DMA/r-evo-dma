export type TraderPrice = {
    name: string;
    price: number;
    priceStr: string;
    pricePerSlot: number;
    ppsStr: string;
};

export type SellPrice = {
    highestPrice: TraderPrice;
    hasPrice: boolean;
};

export type BuyPrice = {
    lowestPrice: TraderPrice;
    hasPrice: boolean;
};

export type ItemPrice = {
    sell: SellPrice;
    buy: BuyPrice;
    hasPrice: boolean;
};

export type ItemCategories = {
    isKey: boolean,
    isBarterItem: boolean,
    isContainer: boolean,
    isFood: boolean,
    isDrink: boolean,
    isMedical: boolean,
    isSpecialEquipment: boolean,
    isMap: boolean,
    isAmmo: boolean,
    isAmmoPack: boolean,
    isCurrency: boolean,
    isRepairKit: boolean,
    isOther: boolean,
    isSight: boolean,
    isGear: boolean,
    isWeapon: boolean,
    isMeleeWeapon: boolean,
    isThrowable: boolean,
    isWeaponPart: boolean,
};

export type LootItem = {
    id: string;
    name: string;
    shortName: string;
    width: number;
    height: number;
    categories: ItemCategories;
    itemPrice: ItemPrice;
};

export type LootItems = {
    [key: string]: LootItem
};

export type Crafts = {
    [key: string]: {
        totalCost: number;
    };
};

export enum PriceComparison
{
    GT,
    LT
};