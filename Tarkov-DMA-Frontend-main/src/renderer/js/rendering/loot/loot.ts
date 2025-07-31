import { Container, DisplayObject } from "pixi.js";
import { PIXI_VIEWPORT, VIEWPORT_SCALES } from "..";
import { getItemColor, isPPSActive } from "../../lootManager/lootFilter";
import { ALL_ITEMS } from "../../lootManager/lootManager";
import { colors } from "./lootType";
import { Marker } from "./marker";
import { LootText } from "./text";
import { LootPosition } from "./lootManager";
import { TraderPrice } from "../../../../../../Loot-API/src/types/eft";

export class Loot {
    staticData: {
        BSG_ID: string;
        Type: number;
        Position: LootPosition;
    };
    dynamicData: {
        Name: string;
        CurrentLootVersion: number;
        Colors: {
            Main: string;
            Border: string;
            Text: string;
        };
        Price: string;
    };
    container: Container<DisplayObject>;
    lootMarker: Marker;
    lootText: LootText;
    
    constructor(BSG_ID: string, Type: number, Name: string, Position: LootPosition, CurrentLootVersion: number) {
        this.staticData = {
            BSG_ID: BSG_ID,
            Type: Type,
            Position: Position,
        };

        this.dynamicData = {
            Name: Name,
            CurrentLootVersion: CurrentLootVersion,
            Colors: {
                Main: "#0000FF",
                Border: "#000000",
                Text: "#FFFFFF",
            },
            Price: "",
        };

        this.container = new Container();
        this.container.cullable = true;
        this.container.position.x = Position[0];
        this.container.position.y = Position[1];
        this.container.zIndex = 30;
        
        // Get the marker color
        this._getColor();
        
        // Get the item price
        this.dynamicData.Price = this._getPrice();
        
        /** @type {Marker} */
        this.lootMarker = new Marker(Type, this.dynamicData.Colors.Main, this.dynamicData.Colors.Border, Position[2]);
        this.container.addChild(this.lootMarker);
        
        /** @type {LootText} */
        this.lootText = new LootText(`${this.dynamicData.Price}${this.dynamicData.Name}`, this.dynamicData.Colors.Text, this.dynamicData.Colors.Border);
        this.container.addChild(this.lootText);
        
        // Set scale to current scale
        this.setScale(VIEWPORT_SCALES.Loot[0], VIEWPORT_SCALES.Loot[1]);

        this._setDirty();

        // Add loot to canvas
        PIXI_VIEWPORT.addChild(this.container);   
    }

    /**
     * Redraws the marker if data changed.
     */
    updateData(LocalPlayerHeight: number, CurrentLootVersion: number, Name: string) {
        // Update dynamic data
        this.dynamicData.CurrentLootVersion = CurrentLootVersion;

        let anyUpdated = false;

        if (this.dynamicData.Name != Name) {
            this.dynamicData.Name = Name;
            this.lootText.update(`${this.dynamicData.Price}${this.dynamicData.Name}`);
            anyUpdated = true;
        }
        
        if (this.lootMarker.updateMarker(this.dynamicData.Colors.Main, this.dynamicData.Colors.Border, LocalPlayerHeight)) anyUpdated = true;

        if (anyUpdated) this._setDirty();
    }

    /**
     * Redraws the marker if data changed.
     */
    updateRelativeHeight(LocalPlayerHeight: number) {
        if (this.lootMarker.updateMarker(this.dynamicData.Colors.Main, this.dynamicData.Colors.Border, LocalPlayerHeight)) this._setDirty();
    }

    /**
     * Redraws the marker if data changed.
     */
    updateColor() {
        this._getColor();

        this.lootMarker.updateMarker(this.dynamicData.Colors.Main, this.dynamicData.Colors.Border);

        // Recreate text
        this.container.removeChild(this.lootText);
        this.lootText.destroy(true);

        this.lootText = new LootText(`${this.dynamicData.Price}${this.dynamicData.Name}`, this.dynamicData.Colors.Text, this.dynamicData.Colors.Border);
        this.container.addChild(this.lootText);

        this._setDirty();
    }

    /**
     * Redraws the marker if data changed.
     */
    updatePrice() {
        this.dynamicData.Price = this._getPrice();

        // Recreate text
        this.container.removeChild(this.lootText);
        this.lootText.destroy(true);

        this.lootText = new LootText(`${this.dynamicData.Price}${this.dynamicData.Name}`, this.dynamicData.Colors.Text, this.dynamicData.Colors.Border);
        this.container.addChild(this.lootText);

        this._setDirty();
    }

    setScale(x: number, y: number) {
        this.container.scale.x = x;
        this.container.scale.y = y;
    }

    
    _getPrice() {
        const item = ALL_ITEMS[this.staticData.BSG_ID];
        
        if (item == null || !item.itemPrice.hasPrice)
            return "";

        let price: TraderPrice;
        if (item.itemPrice.sell.hasPrice)
            price = item.itemPrice.sell.highestPrice;
        else
            price = item.itemPrice.buy.lowestPrice;

        if (isPPSActive())
            return `[${price.ppsStr}] `;
        else
            return `[${price.priceStr}] `;
    }
    
    _getColor() {
        const filterColors = this._getFilterColors();
        if (filterColors != null) this.dynamicData.Colors = filterColors;
        else {
            this.dynamicData.Colors.Main = this._getTypeColor();
            this.dynamicData.Colors.Border = "#000000";
            this.dynamicData.Colors.Text = "#FFFFFF";
        }
    }
    
    _getFilterColors() {
        const filterColor = getItemColor(this.staticData.BSG_ID);
        if (filterColor != null) return filterColor;
        else return null;
    }
    
    _getTypeColor() {
        const color = colors[this.staticData.Type];
        
        if (color != null) return color;
        else return "#F403FC";
    }
    
    /**
     * Regenerates the cached texture of this loot item.
     */
    _setDirty() {
        this.container.cacheAsBitmap = false;
        this.container.cacheAsBitmap = true;
    }

    destroy() {
        this.container.destroy(true);
    }
}
