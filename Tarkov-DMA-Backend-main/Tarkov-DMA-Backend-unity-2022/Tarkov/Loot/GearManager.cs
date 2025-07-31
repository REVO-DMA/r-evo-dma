using System.Collections.Frozen;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public sealed class GearManager
    {
        private static readonly FrozenSet<Enums.EquipmentSlot> _skipSlots = new HashSet<Enums.EquipmentSlot>()
        {
            Enums.EquipmentSlot.SecuredContainer,
            Enums.EquipmentSlot.Dogtag,
            Enums.EquipmentSlot.ArmBand,
        }.ToFrozenSet();

        public readonly struct GearItem(string name, string shortName)
        {
            public readonly string Name = name;
            public readonly string ShortName = shortName;
        }

        private readonly bool _isPMC;
        private IReadOnlyDictionary<Enums.EquipmentSlot, ulong> Slots { get; }
        public IReadOnlyDictionary<Enums.EquipmentSlot, GearItem> Equipment { get; private set; }
        public IReadOnlyList<LootItem> Loot { get; private set; }
        public int Value { get; private set; }

        public GearManager(bool isLocalPlayer, bool isPMC, ulong playerBase, ulong ObservedPlayerController)
        {
            _isPMC = isPMC;
            Dictionary<Enums.EquipmentSlot, ulong> slotDict = new();

            ulong inventoryController;
            if (isLocalPlayer)
                inventoryController = Memory.ReadPtr(playerBase + Offsets.Player._inventoryController, false);
            else
                inventoryController = Memory.ReadPtr(ObservedPlayerController + Offsets.ObservedPlayerController.InventoryController, false);

            ulong inventory = Memory.ReadPtr(inventoryController + Offsets.InventoryController.Inventory, false);
            ulong equipment = Memory.ReadPtr(inventory + Offsets.Inventory.Equipment, false);
            ulong slots = Memory.ReadPtr(equipment + Offsets.Equipment.Slots, false);
            MemArray<ulong> slotsArray = new(slots, false);

            foreach (var slotPtr in slotsArray.Items)
            {
                ulong namePtr = Memory.ReadPtr(slotPtr + Offsets.Slot.ID, false);
                var name = Memory.ReadUnityString(namePtr, false);
                var equipmentSlot = EnumHelper.TryGetValue<Enums.EquipmentSlot>(name);
                if (equipmentSlot)
                {
                    if (_skipSlots.Contains(equipmentSlot))
                        continue;

                    slotDict.TryAdd(equipmentSlot, slotPtr);
                }
            }

            Slots = slotDict;
        }

        public void Refresh()
        {
            Vector3 pos = new();
            List<LootItem> loot = new();
            Dictionary<Enums.EquipmentSlot, GearItem> gearDict = new();

            foreach (var slot in Slots)
            {
                try
                {
                    // Skip pmc scabbard
                    if (_isPMC && slot.Key == Enums.EquipmentSlot.Scabbard)
                        continue;

                    ulong containedItem = Memory.ReadPtr(slot.Value + Offsets.Slot.ContainedItem, false);
                    ulong inventoryTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                    ulong idPtr = Memory.ReadPtr(inventoryTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                    var id = Memory.ReadUnityString(idPtr);

                    try // Get all items on player
                    {
                        ulong grids = Memory.ReadPtr(containedItem + Offsets.LootItemMod.Grids);
                        LootManager.GetItemsInGrid(grids, id, pos, loot);
                    }
                    catch { }

                    if (AllItems.TryGetValue(id, out var entry))
                    {
                        if (slot.Key == Enums.EquipmentSlot.FirstPrimaryWeapon ||
                            slot.Key == Enums.EquipmentSlot.SecondPrimaryWeapon ||
                            slot.Key == Enums.EquipmentSlot.Holster ||
                            slot.Key == Enums.EquipmentSlot.Headwear)
                        {
                            RecurseGearSlots(slot.Key, containedItem, loot);
                        }

                        GearItem gear = new(entry.Name ?? "Unknown Item", entry.ShortName ?? "Unk. Item");
                        gearDict.TryAdd(slot.Key, gear);
                    }
                }
                catch { }
            }

            Loot = loot.OrderByDescending(x => x.Important).ThenByDescending(x => x.IsQuestItem).ThenByDescending(x => x.Price).ToList();
            Value = loot.Sum(x => x.Price);
            Equipment = gearDict;
        }

        /// <summary>
        /// Checks a 'Primary' weapon for Ammo Type, and Thermal Scope.
        /// </summary>
        private void RecurseGearSlots(Enums.EquipmentSlot equipmentSlot, ulong lootItemBase, List<LootItem> loot)
        {
            try
            {
                // Add this item as the base w/ the associated slot
                ulong itemTemplate = Memory.ReadPtr(lootItemBase + Offsets.LootItem.Template);
                ulong itemIDPtr = Memory.ReadPtr(itemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                var itemID = Memory.ReadUnityString(itemIDPtr);
                if (AllItems.TryGetValue(itemID, out var thisItem)) // Item exists in DB
                {
                    LootItem item = new(thisItem);
                    item.BaseAddress = lootItemBase;
                    item.PlayerGearSlot = equipmentSlot;

                    loot.Add(item);
                }

                ulong parentSlots = Memory.ReadPtr(lootItemBase + Offsets.LootItemMod.Slots);
                MemArray<ulong> slotsArray = new(parentSlots);

                foreach (ulong slotPtr in slotsArray.Items)
                {
                    ulong containedItem = Memory.ReadPtr(slotPtr + Offsets.Slot.ContainedItem);
                    ulong inventoryTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                    ulong idPtr = Memory.ReadPtr(inventoryTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                    var id = Memory.ReadUnityString(idPtr);
                    if (AllItems.TryGetValue(id, out var entry))
                    {
                        LootItem item = new(entry);
                        item.PlayerGearSlot = equipmentSlot;

                        loot.Add(item);
                    }

                    RecurseGearSlots(equipmentSlot, containedItem, loot);
                }
            }
            catch { }
        }
    }
}
