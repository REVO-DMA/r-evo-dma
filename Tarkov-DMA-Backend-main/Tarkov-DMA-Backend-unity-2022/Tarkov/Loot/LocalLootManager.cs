using System.Timers;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Unity.Collections;
using TerraFX.Interop.Windows;
using Timer = System.Timers.Timer;

namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public static class LocalLootManager
    {
        public static List<LocalLootItem> Loot { get; private set; } = new();
        
        private static List<ulong> Slots = new();
        private static List<ulong> Grids = new();
        private static Timer _timer;

        private static ulong CurrentVersion = 0;

        private const int REFRESH_INTERVAL_MS = 2500;

        private static ulong LastPlayerBase = 0x0;

        public struct LocalLootItem(string id, string name, string shortName, bool foundInRaid)
        {
            public readonly string ID = id;
            public readonly string Name = name;
            public readonly string ShortName = shortName;
            public readonly bool FoundInRaid = foundInRaid;
            public bool QuestConsumed = false;

            public void SetConsumed() => QuestConsumed = true;
            public void ResetConsumed() => QuestConsumed = false;
        }

        public static void Initialize()
        {
            _timer = new(REFRESH_INTERVAL_MS);
            _timer.Elapsed += Refresh;
            _timer.Start();
        }

        private static void Refresh(object sender, ElapsedEventArgs e)
        {
            Player localPlayer = EFTDMA.LocalPlayer;
            if (localPlayer == null)
                return;

            UpdateSlots(localPlayer);
            RefreshLoot();
        }

        private static void UpdateSlots(Player localPlayer)
        {
            try
            {
                if (localPlayer.Base == LastPlayerBase)
                    return;

                List<ulong> slotList = new();
                List<ulong> gridsList = new();

                ulong inventoryController = Memory.ReadPtr(localPlayer.Base + Offsets.Player._inventoryController);
                ulong inventory = Memory.ReadPtr(inventoryController + Offsets.InventoryController.Inventory);

                ulong equipment = Memory.ReadPtr(inventory + Offsets.Inventory.Equipment);
                GetSlots(equipment, slotList);

                ulong questRaidItems = Memory.ReadPtrUnsafe(inventory + Offsets.Inventory.QuestRaidItems);
                if (questRaidItems != 0x0)
                    GetGrids(questRaidItems, gridsList);
                else
                    Logger.WriteLine("[LOCAL LOOT MANAGER] -> UpdateSlots(): Failed to get questRaidItems");

                ulong questStashItems = Memory.ReadPtrUnsafe(inventory + Offsets.Inventory.QuestStashItems);
                if (questStashItems != 0x0)
                    GetGrids(questStashItems, gridsList);
                else
                    Logger.WriteLine("[LOCAL LOOT MANAGER] -> UpdateSlots(): Failed to get questStashItems");

                LastPlayerBase = localPlayer.Base;
                Slots = slotList;
                Grids = gridsList;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LOCAL LOOT MANAGER] -> UpdateSlots(): Failed to update ~ {ex}");
            }
        }

        private static void GetSlots(ulong compoundItem, List<ulong> slotList)
        {
            try
            {
                ulong slots = Memory.ReadPtr(compoundItem + Offsets.Equipment.Slots);
                MemArray<ulong> slotsArray = new(slots);

                foreach (ulong slotAddr in slotsArray.Items)
                    slotList.Add(slotAddr);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LOCAL LOOT MANAGER] -> GetSlots(): Failed to get slots ~ {ex}");
            }
        }

        private static void GetGrids(ulong compoundItem, List<ulong> gridsList)
        {
            try
            {
                ulong grids = Memory.ReadPtr(compoundItem + Offsets.Equipment.Grids);
                gridsList.Add(grids);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LOCAL LOOT MANAGER] -> GetGrids(): Failed to get grids ~ {ex}");
            }
        }

        private static void RefreshLoot()
        {
            try
            {
                List<LocalLootItem> loot = new();

                foreach (ulong slot in Slots)
                {
                    try
                    {
                        ulong containedItem = Memory.ReadPtr(slot + Offsets.Slot.ContainedItem, false);
                        bool foundInRaid = Memory.ReadValue<bool>(containedItem + Offsets.LootItem.SpawnedInSession);
                        
                        ulong inventoryTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                        bool questItem = Memory.ReadValue<bool>(inventoryTemplate + Offsets.ItemTemplate.QuestItem);

                        ulong idAddr = Memory.ReadPtr(inventoryTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                        string id = Memory.ReadUnityString(idAddr);

                        try
                        {
                            ulong grids = Memory.ReadPtr(containedItem + Offsets.LootItemMod.Grids);

                            bool fir = questItem || foundInRaid;
                            LocalLootItem lootItem = new(id, LocaleManager.GetItemName(id), LocaleManager.GetItemShortName(id), fir);
                            EnumerateGrids(grids, lootItem, loot);
                        }
                        catch { }
                    }
                    catch { }
                }

                foreach (ulong grid in Grids)
                {
                    try
                    {
                        EnumerateGrids(grid, default, loot);
                    }
                    catch { }
                }

                Loot = loot;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LOCAL LOOT MANAGER] -> RefreshLoot(): Failed to refresh ~ {ex}");
            }
        }

        private static void EnumerateGrids(ulong gridsArrayPtr, LocalLootItem lootItem, List<LocalLootItem> containerLoot, int recurseDepth = 0)
        {
            if (!string.IsNullOrEmpty(lootItem.ID) &&
                !string.IsNullOrEmpty(LocaleManager.GetItemName(lootItem.ID)))
            {
                containerLoot.Add(lootItem);
            }

            if (gridsArrayPtr == 0x0 || recurseDepth++ > 5)
                return;

            try
            {
                MemArray<ulong> gridsArray = new(gridsArrayPtr);

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
                                bool foundInRaid = Memory.ReadValue<bool>(childItem + Offsets.LootItem.SpawnedInSession);
                                
                                ulong childItemTemplate = Memory.ReadPtr(childItem + Offsets.LootItem.Template);
                                bool questItem = Memory.ReadValue<bool>(childItemTemplate + Offsets.ItemTemplate.QuestItem);
                                
                                ulong childIdAddr = Memory.ReadPtr(childItemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                                string childID = Memory.ReadUnityString(childIdAddr);

                                ulong childGridsArrayAddr = Memory.ReadPtrUnsafe(childItem + Offsets.LootItemMod.Grids);

                                bool fir = questItem || foundInRaid;
                                LocalLootItem childLootItem = new(childID, LocaleManager.GetItemName(childID), LocaleManager.GetItemShortName(childID), fir);
                                EnumerateGrids(childGridsArrayAddr, childLootItem, containerLoot, recurseDepth);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
