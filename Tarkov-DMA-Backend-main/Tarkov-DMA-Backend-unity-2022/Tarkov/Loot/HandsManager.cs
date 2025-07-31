namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public sealed class HandsManager
    {
        private readonly Player _parent;
        private readonly ulong _handsController;

        private LootItem _cachedItem;
        private string _ammo = null;
        /// <summary>
        /// Formatted string describing the player's hands contents.
        /// </summary>
        public string ItemInHands = null;

        public HandsManager(Player player)
        {
            _parent = player;

            if (_parent.IsClientPlayer)
                _handsController = Memory.ReadPtr(_parent + Offsets.Player._handsController);
            else
                _handsController = Memory.ReadPtr(_parent.ObservedPlayerController + Offsets.ObservedPlayerController.HandsController);
        }

        public void Refresh()
        {
            try
            {
                ulong itemBase;
                if (_parent.IsClientPlayer)
                    itemBase = Memory.ReadPtr(_handsController + Offsets.ItemHandsController.Item);
                else
                    itemBase = Memory.ReadPtr(_handsController + Offsets.ObservedHandsController.ItemInHands);

                ulong itemTemplate = Memory.ReadPtr(itemBase + Offsets.LootItem.Template);
                ulong itemIDPtr = Memory.ReadPtr(itemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                var itemID = Memory.ReadUnityString(itemIDPtr);
                if (AllItems.TryGetValue(itemID, out var heldItem))
                {
                    _cachedItem = new(heldItem);
                    if (heldItem.Categories.IsWeapon == false || heldItem.Categories.IsMeleeWeapon == true || heldItem.Categories.IsThrowable == true)
                        _ammo = null;
                    else
                    {
                        try
                        {
                            ulong chambers = Memory.ReadPtr(itemBase + Offsets.LootItemWeapon.Chambers);
                            ulong slotPtr = Memory.ReadPtr(chambers + UnityOffsets.UnityListBase.Start + (0 * 0x8));
                            ulong slotItem = Memory.ReadPtr(slotPtr + Offsets.Slot.ContainedItem);
                            ulong ammoTemplate = Memory.ReadPtr(slotItem + Offsets.LootItem.Template);
                            ulong ammoIDPtr = Memory.ReadPtr(ammoTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                            var ammoID = Memory.ReadUnityString(ammoIDPtr);
                            if (AllItems.TryGetValue(ammoID, out var ammo))
                            {
                                if (ammo != null) _ammo = ammo.ShortName;
                                else _ammo = null;
                            }
                            else _ammo = null;
                        }
                        catch { }
                    }
                }
                else
                {
                    _cachedItem = null;
                    _ammo = null;
                }
            }
            catch (Exception ex)
            {
                _cachedItem = null;
                _ammo = null;

                Logger.WriteLine($"[HandsManager] -> Refresh(): Failed to refresh hands contents for player: \"{_parent.Name}\" with exception: {ex}");
            }
            finally
            {
                string itemName = _cachedItem?._item?.ShortName;
                if (itemName == null) ItemInHands = null;
                else
                {
                    string ammoName = null;
                    if (_ammo != null) ammoName = $" ({_ammo})";

                    if (ammoName != null)
                        ItemInHands = $"{itemName}{ammoName}";
                    else
                        ItemInHands = itemName;
                }
            }
        }
    }
}
