using SkiaSharp;
using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public class ItemManager
    {
        /// <summary>
        /// Master items dictionary - mapped via BSGID String.
        /// </summary>
        public static readonly ConcurrentDictionary<string, JSON.TarkovItem> AllItems = new();
        public static readonly ConcurrentDictionary<string, JSON.LootFilter> LootFilters = new();
        public static readonly ConcurrentDictionary<string, bool> ContainerCategories = new(
            new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
            {
                { "Bank cash register", false },
                { "Bank safe", false },
                { "Buried barrel cache", false },
                { "Cash register", false },
                { "Cash register TAR2-2", false },
                { "Dead Scav", false },
                { "Drawer", false },
                { "Duffle bag", false },
                { "Grenade box", false },
                { "Ground cache", false },
                { "Jacket", false },
                { "Medbag SMU06", false },
                { "Medcase", false },
                { "Medical supply crate", false },
                { "PC block", false },
                { "PMC body", false },
                { "Plastic suitcase", false },
                { "Ration supply crate", false },
                { "Safe", false },
                { "Technical supply crate", false },
                { "Toolbox", false },
                { "Weapon box", false },
                { "Wooden ammo box", false },
                { "Wooden crate", false },
            }
        , StringComparer.OrdinalIgnoreCase);

        public static int HighValueThreshold = 60000;
        public static int ImportantThreshold = 200000;
        public static bool ShowQuestItems
        {
            get => Game.QuestHelperEnabled;
        }
        public static bool PricePerSlot = false;
        public static bool ShowHighValue = false;
        public static bool ShowCorpses = false;
        public static bool ShowGear = false;
        public static bool ShowWeapons = false;
        public static bool ShowWeaponParts = false;
        public static bool ShowSights = false;
        public static bool ShowKeys = false;
        public static bool ShowBarterItems = false;
        public static bool ShowContainers = false;
        public static bool ShowProvisions = false;
        public static bool ShowMedicalItems = false;
        public static bool ShowOther = false;

        #region Methods
        public static void CreteItemsDict(Dictionary<string, JSON.TarkovItem> rawItems)
        {
            foreach (var item in rawItems)
                AllItems.TryAdd(item.Key, item.Value);
        }

        public static void SetItemFilter(JSON.ItemFilterSync[] itemFilterConfigs)
        {
            Logger.WriteLine("[ITEM MANAGER] SetItemFilter()");

            if (itemFilterConfigs == null) return;

            foreach (var config in itemFilterConfigs)
            {
                if (config == null) continue;

                if (config.Type == "bool")
                {
                    bool value = bool.Parse(config.Value);

                    if (ContainerCategories.ContainsKey(config.ID))
                    {
                        if (config.ID == "Safe")
                        {
                            ContainerCategories.AddOrUpdate("Bank Safe", (newItem) => value, (key, existing) => value);
                            ContainerCategories.AddOrUpdate("Safe", (newItem) => value, (key, existing) => value);
                        }
                        else if (config.ID == "Cash Register")
                        {
                            ContainerCategories.AddOrUpdate("Cash Register", (newItem) => value, (key, existing) => value);
                            ContainerCategories.AddOrUpdate("Bank Cash Register", (newItem) => value, (key, existing) => value);
                            ContainerCategories.AddOrUpdate("Cash Register TAR2-2", (newItem) => value, (key, existing) => value);
                        }
                        else if (config.ID == "Dead Body")
                        {
                            ContainerCategories.AddOrUpdate("Dead Scav", (newItem) => value, (key, existing) => value);
                            ContainerCategories.AddOrUpdate("PMC Body", (newItem) => value, (key, existing) => value);
                        }
                        else
                            ContainerCategories.AddOrUpdate(config.ID, (newItem) => value, (key, existing) => value);


                        continue;
                    }

                    if (config.ID == "quest") Game.QuestHelperEnabled = value;
                    else if (config.ID == "questShowAll") Game.QuestHelperShowAll = value;
                    else if (config.ID == "pps") PricePerSlot = value;
                    else if (config.ID == "highValue") ShowHighValue = value;
                    else if (config.ID == "corpses") ShowCorpses = value;
                    else if (config.ID == "gear") ShowGear = value;
                    else if (config.ID == "weapons") ShowWeapons = value;
                    else if (config.ID == "weaponParts") ShowWeaponParts = value;
                    else if (config.ID == "sights") ShowSights = value;
                    else if (config.ID == "keys") ShowKeys = value;
                    else if (config.ID == "barterItems") ShowBarterItems = value;
                    else if (config.ID == "containers") ShowContainers = value;
                    else if (config.ID == "provisions") ShowProvisions = value;
                    else if (config.ID == "medicalItems") ShowMedicalItems = value;
                    else if (config.ID == "other") ShowOther = value;
                }
                else if (config.Type == "int")
                {
                    int value = int.Parse(config.Value);

                    if (config.ID == "highValueThreshold") HighValueThreshold = value;
                    else if (config.ID == "importantThreshold") ImportantThreshold = value;
                }
            }

            EFTDMA.Loot?.ApplyLootFilter();
            UI_Manager.ShouldSendLootToGUI = true;
        }

        /// <summary>
        /// Format price numeral as a string.
        /// </summary>
        /// <param name="price">Price to convert to string format.</param>
        public static string FormatPrice(int price)
        {
            if (price >= 1000000)
                return (price / 1000000D).ToString("0.##") + "M";
            else if (price >= 1000)
                return (price / 1000D).ToString("0") + "K";

            return price.ToString();
        }

        public static void SetAllLootFilters(JSON.LootFilter[] lootFilters)
        {
            ClearLootFilterItems();

            foreach (var filter in LootFilters.Values)
            {
                filter.ColorPaint?.Dispose();
                filter.BorderColorPaint?.Dispose();
                filter.TextColorPaint?.Dispose();
            }

            LootFilters.Clear();

            // Add all filters to the dict
            foreach (var lootFilter in lootFilters)
            {
                lootFilter.ColorPaint = new()
                {
                    TextSize = ESP_Config.LootFontSize,
                    IsAntialias = ESP_Config.ESP_Antialiasing,
                    Color = SKColor.Parse(lootFilter.Color),
                    TextAlign = SKTextAlign.Center,
                    Typeface = PaintsManager.FontFamily,
                };
                lootFilter.BorderColorPaint = new()
                {
                    TextSize = ESP_Config.LootFontSize,
                    IsAntialias = ESP_Config.ESP_Antialiasing,
                    Color = SKColor.Parse(lootFilter.BorderColor),
                    TextAlign = SKTextAlign.Center,
                    Typeface = PaintsManager.FontFamily,
                };
                lootFilter.TextColorPaint = new()
                {
                    TextSize = ESP_Config.LootFontSize,
                    IsAntialias = ESP_Config.ESP_Antialiasing,
                    Color = SKColor.Parse(lootFilter.TextColor),
                    TextAlign = SKTextAlign.Center,
                    Typeface = PaintsManager.FontFamily,
                };

                LootFilters.TryAdd(lootFilter.ID, lootFilter);
            }

            UpdateLootFilterItems();
        }

        public static void SetLootFilter(JSON.LootFilter lootFilter)
        {
            ClearLootFilterItems();

            lootFilter.ColorPaint = new()
            {
                TextSize = ESP_Config.LootFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColor.Parse(lootFilter.Color),
                TextAlign = SKTextAlign.Center,
                Typeface = PaintsManager.FontFamily,
            };
            lootFilter.BorderColorPaint = new()
            {
                TextSize = ESP_Config.LootFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColor.Parse(lootFilter.BorderColor),
                TextAlign = SKTextAlign.Center,
                Typeface = PaintsManager.FontFamily,
            };
            lootFilter.TextColorPaint = new()
            {
                TextSize = ESP_Config.LootFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColor.Parse(lootFilter.TextColor),
                TextAlign = SKTextAlign.Center,
                Typeface = PaintsManager.FontFamily,
            };

            LootFilters.AddOrUpdate(lootFilter.ID, lootFilter, (key, oldValue) =>
            {
                oldValue.ColorPaint?.Dispose();
                oldValue.BorderColorPaint?.Dispose();
                oldValue.TextColorPaint?.Dispose();

                return lootFilter;
            });

            UpdateLootFilterItems();
        }

        private static void UpdateLootFilterItems()
        {
            foreach (var item in AllItems.Values)
            {
                var lootFilter = LootFilters.FirstOrDefault(x => x.Value.Items.Contains(item.ID)).Value;

                if (lootFilter == null)
                {
                    item.ActiveLootFilter = null;
                    item.FilterItem = false;
                }
                else
                {
                    item.ActiveLootFilter = lootFilter;
                    item.FilterItem = true;
                }
            }

            EFTDMA.Loot?.ApplyLootFilter();
            UI_Manager.ShouldSendLootToGUI = true;
        }

        private static void ClearLootFilterItems()
        {
            var items = AllItems.Values;

            foreach (var item in items)
            {
                if (item == null) continue;

                item.ActiveLootFilter = null;
                item.FilterItem = false;
            }
        }
        #endregion
    }
}