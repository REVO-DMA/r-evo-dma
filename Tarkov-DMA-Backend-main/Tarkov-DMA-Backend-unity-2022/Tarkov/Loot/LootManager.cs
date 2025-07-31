using System.Linq;
using Tarkov_DMA_Backend.MemDMA;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Collections;
using static Tarkov_DMA_Backend.Unity.ChamsManager;

namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public sealed class LootManager
    {
        #region Fields/Properties/Constructor

        private readonly ulong _lgw;
        private readonly CancellationToken _ct;
        private readonly object _filterLock = new();
        public ConcurrentDictionary<ulong, LootItem> AllLoot = new();
        public HashSet<ulong> ChammedCorpses = new();
        public Dictionary<ulong, string> LootClassNameCache = new();
        /// <summary>
        /// This represents the current loot refresh iteration. It is used to determine if an item should be removed from the items dict.
        /// </summary>
        private ulong _currentLootVersion = 0;
        /// <summary>
        /// Filtered loot ready for display by GUI.
        /// </summary>
        public IReadOnlyDictionary<ulong, LootItem> DisplayLoot { get; private set; }

        public LootManager(ulong localGameWorld, CancellationToken ct)
        {
            _lgw = localGameWorld;
            _ct = ct;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Refreshes loot, only call from a memory thread (Non-GUI).
        /// </summary>
        public void Refresh()
        {
            try
            {
                GetLoot();
                ApplyLootFilter();
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR - Failed to refresh loot: {ex}");
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Updates referenced Loot List with fresh values.
        /// </summary>
        private void GetLoot()
        {
            ulong lootListAddr = Memory.ReadPtr(_lgw + Offsets.ClientLocalGameWorld.LootList);
            MemList<ulong> memLoot = new(lootListAddr);
            var memLootItems = memLoot.Items;
            int memLootItemsCount = memLootItems.Count;

            EFTScatterMap map = new(memLootItemsCount);
            var round1 = map.AddRound();
            var round2 = map.AddRound();
            var round3 = map.AddRound();
            var round4 = map.AddRound();

            for (int i = 0; i < memLootItemsCount; i++)
            {
                _ct.ThrowIfCancellationRequested();

                try
                {
                    ulong lootBase = memLootItems[i];

                    var unknownPtr = round1.AddEntry<MemPointer>(i, 1, lootBase, null, UnityOffsets.EFTClass.To_GameObject[0]);
                    round2.AddEntry<MemPointer>(i, 2, unknownPtr, null, UnityOffsets.EFTClass.SuperType);
                    var gameObject = round2.AddEntry<MemPointer>(i, 3, unknownPtr, null, UnityOffsets.EFTClass.To_GameObject[1]);
                    var components = round3.AddEntry<MemPointer>(i, 4, gameObject, null, UnityOffsets.GameObject.Components);
                    round4.AddEntry<MemPointer>(i, 5, components, null, UnityOffsets.GameObject.To_TransformInternal[3]);

                    if (!LootClassNameCache.TryGetValue(lootBase, out _))
                    {
                        var c1 = round1.AddEntry<MemPointer>(i, 6, lootBase, null, UnityOffsets.Component.To_NativeClassName[0]);
                        var c2 = round2.AddEntry<MemPointer>(i, 7, c1, null, UnityOffsets.Component.To_NativeClassName[1]);
                        var classNamePtr = round3.AddEntry<MemPointer>(i, 8, c2, null, UnityOffsets.Component.To_NativeClassName[2]);
                        round4.AddEntry<string>(i, 9, classNamePtr, 64);
                    }
                }
                catch { }
            }

            map.Execute();

            List<Player> deadPlayers = EFTDMA.Players?.Select(x => x.Value)?.Where(x => x.Corpse != null)?.ToList();

            List<ScatterWriteEntry> corpseChamsWrites = [];

            // Bump refresh iteration
            _currentLootVersion++;

            for (int i = 0; i < memLootItemsCount; i++)
            {
                _ct.ThrowIfCancellationRequested();

                try
                {
                    ulong lootBase = memLootItems[i];

                    if (!map.Results[i][2].TryGetResult<MemPointer>(out var interactiveClass)) continue;
                    if (!map.Results[i][5].TryGetResult<MemPointer>(out var transformInternal)) continue;

                    string className;
                    if (!LootClassNameCache.TryGetValue(lootBase, out className))
                    {
                        if (!map.Results[i][9].TryGetResult(out className)) continue;

                        LootClassNameCache.Add(lootBase, className);
                    }

                    Vector3 pos;
                    if (!AllLoot.TryGetValue(lootBase, out var existingLootItem))
                        pos = new UnityTransform(transformInternal, UnityTransform.TransformType.Normal, true, true).GetPosition();
                    else
                        pos = existingLootItem.Position;

                    if (className.Contains("Corpse", StringComparison.OrdinalIgnoreCase))
                    {
                        string label = "Corpse";

                        Player player = deadPlayers?.FirstOrDefault(x => x.Corpse == interactiveClass);
                        if (player != null)
                            label = player.Name;

                        ulong corpseBody = Memory.ReadPtr(interactiveClass + Offsets.InteractiveCorpse.PlayerBody);
                        if (!ChammedCorpses.Contains(corpseBody))
                        {
                            if (Player.SetCorpseChams(ref corpseChamsWrites, corpseBody))
                                ChammedCorpses.Add(corpseBody);
                        }

                        List<LootItem> corpseLoot = new();
                        GetCorpseLoot(interactiveClass, pos, corpseLoot, lootBase);
                        // Important / Highest priced items first
                        corpseLoot = corpseLoot.OrderByDescending(x => x.IsQuestItem).ThenByDescending(x => x.Important).ThenByDescending(x => x.Price).ToList();

                        var container = new LootContainer(label)
                        {
                            BaseAddress = lootBase,
                            CurrentLootVersion = _currentLootVersion,
                            Position = pos,
                            AllContainedItems = corpseLoot,
                            IsCorpse = true
                        };

                        AllLoot.AddOrUpdate(lootBase, (newItem) => container, (key, existing) => container);
                    }
                    else if (className.Equals("LootableContainer", StringComparison.OrdinalIgnoreCase))
                    {
                        {
                            ulong containerNameAddr = Memory.ReadPtr(interactiveClass + Offsets.LootableContainer.Template);
                            string containerNameRaw = Memory.ReadUnityString(containerNameAddr);
                            string containerName = LocaleManager.GetItemShortName(containerNameRaw);

                            LootContainer container;

                            var itemOwner = Memory.ReadPtr(interactiveClass + Offsets.LootableContainer.ItemOwner);
                            var ownerItemBase = Memory.ReadPtr(itemOwner + Offsets.LootableContainerItemOwner.RootItem);
                            var grids = Memory.ReadPtr(ownerItemBase + Offsets.LootItemMod.Grids);
                            var containerLoot = new List<LootItem>();
                            GetItemsInGrid(grids, null, pos, containerLoot, lootBase);
                            containerLoot = containerLoot.OrderByDescending(x => x.IsQuestItem).ThenByDescending(x => x.Important).ThenByDescending(x => x.Price).ToList();
                            if (containerLoot.Count > 0)
                            {
                                var firstItem = containerLoot[0]._item;
                                if (firstItem != null) containerName = firstItem.ShortName;

                                bool important = containerLoot.Any(x => x.Important);
                                container = new LootContainer(containerName)
                                {
                                    BaseAddress = lootBase,
                                    CurrentLootVersion = _currentLootVersion,
                                    AllContainedItems = containerLoot,
                                    Important = important,
                                    Position = pos
                                };

                                AllLoot.AddOrUpdate(lootBase, (newItem) => container, (key, existing) => container);
                            }
                            else
                            {
                                container = new LootContainer(containerName)
                                {
                                    BaseAddress = lootBase,
                                    CurrentLootVersion = _currentLootVersion,
                                    Position = pos
                                };
                            }

                            AllLoot.AddOrUpdate(lootBase, (newItem) => container, (key, existing) => container);
                        }
                    }
                    else if (className.Equals("ObservedLootItem", StringComparison.OrdinalIgnoreCase))
                    {
                        var item = Memory.ReadPtr(interactiveClass + Offsets.InteractiveLootItem.Item);
                        var itemTemplate = Memory.ReadPtr(item + Offsets.LootItem.Template);
                        bool isQuestItem = Memory.ReadValue<bool>(itemTemplate + Offsets.ItemTemplate.QuestItem);

                        var BSGIdPtr = Memory.ReadPtr(itemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                        var id = Memory.ReadUnityString(BSGIdPtr);
                        if (!isQuestItem)
                        {
                            try
                            {
                                var grids = Memory.ReadPtr(item + Offsets.LootItemMod.Grids);
                                var containerLoot = new List<LootItem>();
                                GetItemsInGrid(grids, id, pos, containerLoot, lootBase);
                                containerLoot = containerLoot.OrderByDescending(x => x.IsQuestItem).ThenByDescending(x => x.Important).ThenByDescending(x => x.Price).ToList();
                                if (containerLoot.Count > 1)
                                {
                                    string containerName = null;
                                    var firstItem = containerLoot[0]._item;
                                    if (firstItem != null) containerName = firstItem.ShortName;

                                    var container = new LootContainer(containerName)
                                    {
                                        BaseAddress = lootBase,
                                        CurrentLootVersion = _currentLootVersion,
                                        AllContainedItems = containerLoot,
                                        Position = pos
                                    };

                                    AllLoot.AddOrUpdate(lootBase, (newItem) => container, (key, existing) => container);
                                }
                                else throw new Exception("Single loot item");
                            }
                            catch
                            {
                                // The loot item we found does not have any grids so it's basically like a keycard or a ledx etc. Therefore add it to our loot dictionary.
                                if (AllItems.TryGetValue(id, out var entry))
                                {
                                    var lootItem = new LootItem(entry)
                                    {
                                        BaseAddress = lootBase,
                                        CurrentLootVersion = _currentLootVersion,
                                        Position = pos
                                    };

                                    AllLoot.AddOrUpdate(lootBase, (newItem) => lootItem, (key, existing) => lootItem);
                                }
                            }
                        }
                        else
                        {
                            LootItem questItem = new(id, LocaleManager.GetItemName(id), true)
                            {
                                BaseAddress = lootBase,
                                CurrentLootVersion = _currentLootVersion,
                                Position = pos
                            };

                            AllLoot.AddOrUpdate(lootBase, (newItem) => questItem, (key, existing) => questItem);
                        }
                    }
                }
                catch { }
            }

            if (corpseChamsWrites.Count > 0)
                Memory.WriteScatter(corpseChamsWrites);

            // Remove stale items
            foreach (var item in AllLoot)
            {
                if (item.Value.CurrentLootVersion != _currentLootVersion)
                {
                    AllLoot.TryRemove(item);
                    LootClassNameCache.Remove(item.Key);
                }
            }
        }

        /// <summary>
        /// Applies the currently set loot filter.
        /// </summary>
        public void ApplyLootFilter()
        {
            lock (_filterLock) // one thread at a time
            {
                Dictionary<ulong, LootItem> filteredLoot = new(AllLoot.Count);

                // Get all loose loot shown by category
                var looseLootCategoryItems = AllLoot.Where(x =>
                {
                    // Always add quest items if enabled
                    if (x.Value.IsQuestItem && ShowQuestItems) return true;

                    if (ShowContainers &&
                        x.Value._itemName != null &&
                        ItemManager.ContainerCategories.TryGetValue(x.Value._itemName, out bool showContainerCategory) &&
                        showContainerCategory)
                    {
                        return true;
                    }

                    var item = x.Value._item;

                    if (item == null) return false;
                    else if (item.Categories.IsGear && ShowGear) return true;
                    else if (item.Categories.IsWeapon && ShowWeapons) return true;
                    else if (item.Categories.IsWeaponPart && ShowWeaponParts) return true;
                    else if (item.Categories.IsSight && ShowSights) return true;
                    else if (item.Categories.IsKey && ShowKeys) return true;
                    else if (item.Categories.IsBarterItem && ShowBarterItems) return true;
                    else if (item.Categories.IsContainer && ShowContainers && ItemManager.ContainerCategories.TryGetValue("Loose Loot", out bool container_looseLoot) && container_looseLoot) return true;
                    else if ((item.Categories.IsFood || item.Categories.IsDrink) && ShowProvisions) return true;
                    else if (item.Categories.IsMedical && ShowMedicalItems) return true;
                    else if (item.Categories.IsOther && ShowOther) return true;
                    else return false;
                });

                // Add all categorically shown loose loot items to the filtered loot list
                foreach (var item in looseLootCategoryItems)
                {
                    filteredLoot.Add(item.Key, item.Value);
                }

                // Add all categorically shown container loot items to the filtered loot list
                foreach (var item in AllLoot)
                {
                    if (item.Value is LootContainer container)
                    {
                        // Get all loose loot shown by category
                        var containerLootCategoryItems = container.AllContainedItems.Where(x =>
                        {
                            // Always add quest items if enabled
                            if (x.IsQuestItem && ShowQuestItems) return true;

                            var item = x._item;

                            if (item == null) return false;
                            else if (item.Categories.IsGear && ShowGear) return true;
                            else if (item.Categories.IsWeapon && ShowWeapons) return true;
                            else if (item.Categories.IsWeaponPart && ShowWeaponParts) return true;
                            else if (item.Categories.IsSight && ShowSights) return true;
                            else if (item.Categories.IsKey && ShowKeys) return true;
                            else if (item.Categories.IsBarterItem && ShowBarterItems) return true;
                            else if (item.Categories.IsContainer && ShowContainers) return true;
                            else if ((item.Categories.IsFood || item.Categories.IsDrink) && ShowProvisions) return true;
                            else if (item.Categories.IsMedical && ShowMedicalItems) return true;
                            else if (item.Categories.IsOther && ShowOther) return true;
                            else return false;
                        });

                        var filteredContainerItems = containerLootCategoryItems.OrderByDescending(x => x.IsQuestItem).ThenByDescending(x => x.Important).ThenByDescending(x => x.Price).ToList();
                        if (filteredContainerItems.Count > 0)
                        {
                            var filteredContainerItem = filteredContainerItems[0];

                            if (!filteredLoot.ContainsKey(filteredContainerItem.BaseAddress) && filteredContainerItem != null)
                            {
                                filteredLoot.Add(filteredContainerItem.BaseAddress, filteredContainerItem);
                            }
                        }
                    }
                }

                foreach (var item in AllLoot)
                {
                    if (item.Value is LootContainer container)
                    {
                        if (container.IsCorpse)
                        {
                            if (ShowCorpses && !filteredLoot.Contains(item))
                                filteredLoot.Add(item.Key, container);
                        }
                        else
                        {
                            var containerValuableLoot = container.AllContainedItems.Where(x => x.IsValuable || container.AlwaysShow).ToList();
                            if (containerValuableLoot.Count > 0)
                            {
                                if (!filteredLoot.Contains(item))
                                {
                                    container.FilteredDisplayItems = containerValuableLoot;

                                    filteredLoot.Add(item.Key, container);
                                }
                            }
                        }
                    }
                    else if (item.Value.IsValuable)
                    {
                        if (!filteredLoot.Contains(item))
                            filteredLoot.Add(item.Key, item.Value);
                    }
                }

                if (filteredLoot == null)
                {
                    Logger.WriteLine("[ApplyLootFilter]: Error -> filteredLoot is null!");
                    return;
                }

                DisplayLoot = filteredLoot;

                UI_Manager.ShouldSendLootToGUI = true;
            }
        }

        private static readonly IReadOnlyCollection<string> _skipSlots = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Scabbard", "SecuredContainer", "Dogtag", "Compass", "Eyewear", "ArmBand"
        };

        /// <summary>
        /// Recurse slots for gear.
        /// </summary>
        public static void GetItemsInSlots(ulong slotsPtr, Vector3 pos, List<LootItem> loot, ulong containerAddress = 0x0)
        {
            var slotDict = new Dictionary<string, ulong>(StringComparer.OrdinalIgnoreCase);
            var slots = new MemArray<ulong>(slotsPtr);

            foreach (var slot in slots.Items)
            {
                try
                {
                    var namePtr = Memory.ReadPtr(slot + Offsets.Slot.ID);
                    var name = Memory.ReadUnityString(namePtr);
                    if (_skipSlots.Contains(name)) continue;
                    slotDict.TryAdd(name, slot);
                }
                catch { }
            }

            foreach (var slot in slotDict)
            {
                try
                {
                    var containedItem = Memory.ReadPtr(slot.Value + Offsets.Slot.ContainedItem);
                    var inventoryTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                    var idPtr = Memory.ReadPtr(inventoryTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                    var id = Memory.ReadUnityString(idPtr);
                    if (AllItems.TryGetValue(id, out var entry))
                    {
                        loot.Add(new LootItem(entry)
                        {
                            ContainerAddress = containerAddress,
                            BaseAddress = slot.Value
                        });
                    }

                    var childGrids = Memory.ReadPtr(containedItem + Offsets.LootItemMod.Grids);
                    GetItemsInGrid(childGrids, null, pos, loot, containerAddress);

                    if (slot.Key == "FirstPrimaryWeapon" || slot.Key == "SecondPrimaryWeapon" || slot.Key == "Headwear")
                    {
                        var childslots = Memory.ReadPtr(containedItem + Offsets.LootItemMod.Slots);
                        GetItemsInSlots(childslots, pos, loot, containerAddress);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets all loot on a corpse.
        /// </summary>
        private static void GetCorpseLoot(ulong lootInteractiveClass, Vector3 pos, List<LootItem> loot, ulong containerAddress)
        {
            var itemBase = Memory.ReadPtr(lootInteractiveClass + Offsets.InteractiveLootItem.Item);
            var slots = Memory.ReadPtr(itemBase + Offsets.LootItemMod.Slots);
            try
            {
                GetItemsInSlots(slots, pos, loot, containerAddress);
            }
            catch { }
        }

        #endregion

        #region Static Public Methods
        
        public static void GetItemsInGrid(ulong gridsArrayPtr, string id, Vector3 pos, List<LootItem> containerLoot, ulong containerAddress = 0x0, int recurseDepth = 0)
        {
            if (id != null && AllItems.TryGetValue(id, out var entry))
            {
                containerLoot.Add(new(entry)
                {
                    ContainerAddress = containerAddress,
                    BaseAddress = gridsArrayPtr,
                    Position = pos
                });
            }

            if (gridsArrayPtr == 0x0 || recurseDepth++ > 3)
                return;

            MemArray<ulong> gridsArray = new(gridsArrayPtr);

            try
            {
                foreach (var grid in gridsArray.Items)
                {
                    try
                    {
                        ulong gridEnumerableClass = Memory.ReadPtr(grid + Offsets.LootItemModGrids.ItemCollection);

                        ulong itemListPtr = Memory.ReadPtr(gridEnumerableClass + Offsets.LootItemModGridsItemCollection.List);
                        MemList<ulong> itemList = new(itemListPtr);

                        foreach (ulong childItem in itemList.Items)
                        {
                            try
                            {
                                ulong childItemTemplate = Memory.ReadPtr(childItem + Offsets.LootItem.Template);
                                ulong childItemIdPtr = Memory.ReadPtr(childItemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                                var childItemIdStr = Memory.ReadUnityString(childItemIdPtr);

                                // Check to see if the child item has children
                                // Don't throw on nullPtr since GetItemsInGrid needs to record the current item still
                                ulong childGridsArrayPtr = Memory.ReadPtrUnsafe(childItem + Offsets.LootItemMod.Grids); // Pointer

                                GetItemsInGrid(childGridsArrayPtr, childItemIdStr, pos, containerLoot, containerAddress, recurseDepth);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        #endregion
    }

    #region Loot Classes

    public class LootItem
    {
        public Vector3 Position = Vector3.Zero;

        public ulong CurrentLootVersion = 0;
        public UnityTransform Transform = null;
        public readonly string _itemID = null;
        public readonly string _itemName = null;
        public readonly JSON.TarkovItem _item;
        public bool QuestConsumed = false;

        /// <summary>
        /// Item's Price (In roubles).
        /// </summary>
        public int Price
        {
            get
            {
                if (_item == null || !_item.ItemPrice.HasPrice) return 0;

                JSON.TarkovItem.TraderPrice_t price;
                if (_item.ItemPrice.Sell.HasPrice)
                    price = _item.ItemPrice.Sell.HighestPrice;
                else
                    price = _item.ItemPrice.Buy.LowestPrice;

                if (PricePerSlot)
                    return price.PricePerSlot;
                else
                    return price.Price;
            }
        }
        public bool _important = false; // Backing Field
        /// <summary>
        /// True if Item is Important (either manually or via UI).
        /// </summary>
        public bool Important
        {
            get
            {
                if (_item?.ActiveLootFilter != null) return true;

                if (_important) return true;

                return false;
            }
            set
            {
                _important = value;
            }
        }
        /// <summary>
        /// Always show this item on the map.
        /// </summary>
        public bool AlwaysShow { get; init; }
        /// <summary>
        /// True if a Corpse.
        /// </summary>
        public bool IsCorpse { get; init; }
        /// <summary>
        /// The container address of this item. Used to identify the container this item came from.
        /// </summary>
        public ulong ContainerAddress { get; set; }
        /// <summary>
        /// The base address of this item. Used to identify specific items.
        /// </summary>
        public ulong BaseAddress { get; set; }
        /// <summary>
        /// The gear slot this item is contained within.
        /// </summary>
        public Enums.EquipmentSlot PlayerGearSlot { get; set; }

        public LootItem(JSON.TarkovItem item, bool isQuestItem = false)
        {
            _item = item;
            _questItem = isQuestItem;
        }

        public LootItem(string bsgID, string itemName, bool isQuestItem = false)
        {
            _itemID = bsgID;
            _itemName = itemName;
            _questItem = isQuestItem;
        }

        /// <summary>
        /// Checks if an item is valuable (meets regular loot value threshold or greater).
        /// Do not use directly on containers.
        /// </summary>
        public bool IsValuable => ShowHighValue && (Important || AlwaysShow || Price >= HighValueThreshold);

        /// <summary>
        /// Checks if an item/container is important.
        /// </summary>
        public bool IsImportant
        {
            get
            {
                if (this is LootContainer container)
                {
                    return container.Important || container.AllContainedItems.Any(x => x.IsImportant);
                }

                return Important || Price >= ImportantThreshold;
            }
        }
        public bool _questItem = false;
        /// <summary>
        /// True if a Quest Item.
        /// </summary>
        public bool IsQuestItem
        {
            get
            {
                if (Game.QuestHelperShowAll && _questItem)
                    return true;

                // Don't show currency
                if (_item is not null && _item.Categories.IsCurrency)
                    return false;

                // Check container items
                if (this is LootContainer container)
                {
                    if (container.AllContainedItems.Where(x => !x._item.Categories.IsCurrency).Any(x => QuestManager.QuestRequiredItems.Where(xx => xx.ID == x._itemID).Any() || QuestManager.QuestRequiredItems.Where(xx => xx.ID == x._item.ID).Any()))
                        return true;
                }

                // Check normal quest items from loot list
                if (_itemID is not null && QuestManager.QuestRequiredItems.Where(x => x.ID == _itemID).Any())
                    return true;

                if (_item is not null && QuestManager.QuestRequiredItems.Where(x => x.ID == _item.ID).Any())
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Gets a user-friendly label for this item.
        /// </summary>
        /// <returns>Item Label string cleaned up for UI usage.</returns>
        public string GetLabel()
        {
            if (_itemName != null)
            {
                return _itemName;
            }

            string label = "";
            if (this is LootContainer container)
            {
                if (container.FilteredDisplayItems.Count == 1)
                {
                    var firstItem = container.FilteredDisplayItems[0];
                    if (firstItem._item != null)
                    {
                        label = firstItem._item.ShortName;
                    }
                }
                else if (container._item != null)
                    label = container._item.Name;
                else
                    label = "Container";
            }
            else
            {
                if (_item != null)
                    label += _item.ShortName;
                else
                    label += "Item";
            }

            return label;
        }

        public byte GetLootType()
        {
            // Always show quest items
            if (ShowQuestItems && IsQuestItem) return 4; // Quest item
            if (IsImportant) return 6; // Important

            if (_item == null) return 255;

            if (IsCorpse) return 1; // Corpse
            if (ShowMedicalItems && _item.Categories.IsMedical) return 2; // Meds
            if (ShowProvisions && (_item.Categories.IsFood || _item.Categories.IsDrink)) return 3; // Food
            // return 5; // Quest Location

            // Default
            return 255;
        }
    }

    public sealed class LootContainer : LootItem
    {
        public LootContainer(string name) : base(null, name) { }

        public IReadOnlyList<LootItem> AllContainedItems { get; init; } = new List<LootItem>();
        public IReadOnlyList<LootItem> FilteredDisplayItems { get; set; } = new List<LootItem>();
    }

    #endregion
}
