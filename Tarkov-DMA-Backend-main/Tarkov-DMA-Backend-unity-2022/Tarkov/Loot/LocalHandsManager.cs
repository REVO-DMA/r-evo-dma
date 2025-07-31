using System.Text.RegularExpressions;
using System.Timers;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Collections;
using Timer = System.Timers.Timer;

namespace Tarkov_DMA_Backend.Tarkov.Loot
{
    public static partial class LocalHandsManager
    {
        public static bool IsHoldingGun { get; private set; } = false;
        public static HashSet<string> WeaponScopeIDs { get; private set; } = new(StringComparer.OrdinalIgnoreCase);
        public static AmmoLevel WeaponAmmoLevel { get; private set; } = AmmoLevel.NotAvailable;
        public static AmmoData WeaponAmmoData { get; private set; } = AmmoData.NotAvailable;
        public static FireModeData WeaponFireModeData { get; private set; } = FireModeData.NotAvailable;
        public static bool IsAiming { get; private set; } = false;
        public static ulong ChamberedAmmoTemplate { get; private set; } = 0x0;
        public static float WeaponVelocityModifier { get; private set; } = 0f;
        public static ulong HandsVersion { get; private set; } = 0;


        private static int lastVersion_fast = int.MinValue;
        private static int lastVersion_slow = int.MinValue;
        private static ulong lastItemBase = 0x0;
        private static ulong magSlot = 0x0;

        private static readonly Timer _fastTimer;
        private static readonly Timer _slowTimer;
        private static readonly Stopwatch _stopwatch;

        private const int FAST_REFRESH_INTERVAL_MS = 32;
        private const int SLOW_REFRESH_INTERVAL_MS = 450;
        private const int FORCE_REFRESH_INTERVAL_FAST_MS = 350;

        [GeneratedRegex(@"_\d{3}$")]
        private static partial Regex TrailingNumberRegex();

        public readonly struct AmmoLevel(int count, int max)
        {
            public static implicit operator string(AmmoLevel x) => x.ToString();
            
            public static readonly AmmoLevel NotAvailable = new(-1, -1);

            public readonly int Count = count;
            public readonly int Max = max;

            public readonly bool ShouldRender()
            {
                if (Count == -1 || Max == -1)
                    return false;

                return true;
            }

            public readonly override string ToString()
            {
                if (ShouldRender())
                {
                    if (Max >= 100)
                        return $"{Count:D3}/{Max:D3}";
                    else
                        return $"{Count:D2}/{Max:D2}";
                }

                return "";
            }
        }

        public readonly struct ChambersData(ulong ammoInChamber, int filledChambers, int totalChambers)
        {
            public readonly ulong AmmoInChamber = ammoInChamber;

            public readonly int FilledChambers = filledChambers;
            public readonly int TotalChambers = totalChambers;
        }

        public readonly struct AmmoData(
            JSON.TarkovItem item,
            float bulletMassGrams,
            float bulletDiameterMillimeters,
            float ballisticCoefficient,
            float bulletSpeed,
            ChambersData chambersData)
        {
            public static implicit operator string(AmmoData x) => x.ToString();

            private const float NotOK = -1f;
            public static readonly AmmoData NotAvailable = new(null, NotOK, NotOK, NotOK, NotOK, default);

            public readonly JSON.TarkovItem Item = item;
            public readonly float BulletMassGrams = bulletMassGrams;
            public readonly float BulletDiameterMillimeters = bulletDiameterMillimeters;
            public readonly float BallisticCoefficient = ballisticCoefficient;
            public readonly float BulletSpeed = bulletSpeed;

            public readonly ChambersData ChambersData = chambersData;

            public readonly bool IsOK()
            {
                if (Item == null &&
                    BulletMassGrams == NotOK &&
                    BulletDiameterMillimeters == NotOK &&
                    BallisticCoefficient == NotOK &&
                    BulletSpeed == NotOK)
                {
                    return false;
                }

                return true;
            }

            public readonly override string ToString()
            {
                if (IsOK() && Item != null)
                    return Item.ShortName;

                return "";
            }
        }

        public readonly struct FireModeData(Enums.EFireMode fireMode, string fireModeString)
        {
            public static implicit operator string(FireModeData x) => x.ToString();

            public static readonly FireModeData NotAvailable = new(Enums.EFireMode.single, null);

            public readonly Enums.EFireMode FireMode = fireMode;
            public readonly string FireModeString = fireModeString;

            public readonly bool IsOK()
            {
                if (FireModeString == null)
                    return false;

                return true;
            }

            public readonly override string ToString()
            {
                if (IsOK())
                    return FireModeString;

                return "";
            }
        }

        static LocalHandsManager()
        {
            _fastTimer = new(FAST_REFRESH_INTERVAL_MS);
            _fastTimer.Elapsed += RefreshFast;
            _fastTimer.Start();

            _slowTimer = new(SLOW_REFRESH_INTERVAL_MS);
            _slowTimer.Elapsed += RefreshSlow;
            _slowTimer.Start();

            _stopwatch = Stopwatch.StartNew();
        }

        #region Private Methods

        private static void RefreshFast(object sender, ElapsedEventArgs e)
        {
            try
            {
                Player localPlayer = EFTDMA.LocalPlayer;
                if (localPlayer == null)
                    return;

                int itemVersion = Memory.ReadValue<int>(lastItemBase + Offsets.LootItem.Version, false);

                // Only update data if the version has changed or the force refresh interval has been reached
                if (lastVersion_fast == itemVersion &&
                    _stopwatch.ElapsedMilliseconds < FORCE_REFRESH_INTERVAL_FAST_MS)
                {
                    return;
                }
                else
                    _stopwatch.Restart();

                UpdateDataFast();

                if (lastVersion_fast != itemVersion)
                    HandsVersion++;
                lastVersion_fast = itemVersion;
            }
            catch
            {
                lastVersion_fast = int.MinValue;
                HandsVersion++;
            }
        }

        private static void RefreshSlow(object sender, ElapsedEventArgs e)
        {
            try
            {
                Player localPlayer = EFTDMA.LocalPlayer;
                if (localPlayer == null)
                    return;

                ulong handsController = Memory.ReadPtr(localPlayer + Offsets.Player._handsController);
                ulong itemBase = Memory.ReadPtr(handsController + Offsets.ItemHandsController.Item, false);
                bool itemBaseChanged = false;
                if (itemBase != lastItemBase) // Reset the last version when hands contents change
                {
                    lastVersion_fast = int.MinValue;
                    lastVersion_slow = int.MinValue;
                    itemBaseChanged = true;
                }

                int itemVersion = Memory.ReadValue<int>(itemBase + Offsets.LootItem.Version, false);                
                
                // Only update data if the version has changed
                if (lastVersion_slow == itemVersion)
                    return;

                UpdateDataSlow(itemBase, itemBaseChanged);
                AimbotTasks(handsController, itemBaseChanged);

                lastItemBase = itemBase;
                lastVersion_slow = itemVersion;
                HandsVersion++;
            }
            catch
            {
                lastItemBase = 0x0;
                lastVersion_slow = int.MinValue;
                HandsVersion++;
            }
        }

        private static void UpdateDataFast()
        {
            try
            {
                if (IsHoldingGun)
                {
                    UpdateAimingState();
                    UpdateChamberedAmmoData(lastItemBase, magSlot);
                    UpdateAmmoLevel(magSlot);
                    UpdateFireMode(lastItemBase);
                }
                else
                {
                    IsAiming = false;
                    WeaponAmmoData = AmmoData.NotAvailable;
                    WeaponAmmoLevel = AmmoLevel.NotAvailable;
                    WeaponFireModeData = FireModeData.NotAvailable;
                }
            }
            catch { }
        }

        private static void AimbotTasks(ulong handsController, bool itemBaseChanged)
        {
            if (itemBaseChanged && IsHoldingGun)
            {
                Memory.WriteValue(handsController + Offsets.FirearmController.TotalCenterOfImpact, 0f);
            }
        }

        private static void UpdateDataSlow(ulong itemBase, bool itemBaseChanged)
        {
            try
            {
                ulong itemTemplate = Memory.ReadPtr(itemBase + Offsets.LootItem.Template);

                if (itemBaseChanged)
                {
                    ulong idPtr = Memory.ReadPtr(itemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                    string id = Memory.ReadUnityString(idPtr);
                    if (AllItems.TryGetValue(id, out var entry))
                    {
                        if (entry.Categories.IsWeapon && !entry.Categories.IsMeleeWeapon && !entry.Categories.IsThrowable)
                            IsHoldingGun = true;
                        else
                            IsHoldingGun = false;
                    }

                    if (IsHoldingGun)
                    {
                        UpdateWeaponVelocityModifier(itemBase, itemTemplate);
                    }
                }

                if (IsHoldingGun)
                {
                    magSlot = GetWeaponMod(itemBase, Enums.EWeaponModType.mod_magazine);

                    UpdateScopeIDs(itemBase);
                }
            }
            catch
            {
                IsHoldingGun = false;
            }
        }

        private static void UpdateAimingState()
        {
            try
            {
                Player localPlayer = EFTDMA.LocalPlayer;
                if (localPlayer == null)
                    throw new Exception("LocalPlayer not found.");

                bool isAiming = Memory.ReadValue<bool>(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._isAiming, false);
                IsAiming = isAiming;
            }
            catch
            {
                IsAiming = false;
            }
        }

        private static void UpdateFireMode(ulong weaponBase)
        {
            try
            {
                ulong fireModeAddress = Memory.ReadPtr(weaponBase + Offsets.LootItemWeapon.FireMode);

                var fireMode = (Enums.EFireMode)Memory.ReadValue<byte>(fireModeAddress + Offsets.FireModeComponent.FireMode);

                string fireModeString = null;
                if (fireMode == Enums.EFireMode.fullauto)
                    fireModeString = "Auto";
                else if (fireMode == Enums.EFireMode.single)
                    fireModeString = "Single";
                else if (fireMode == Enums.EFireMode.doublet)
                    fireModeString = "DbTap";
                else if (fireMode == Enums.EFireMode.burst)
                    fireModeString = "Burst";
                else if (fireMode == Enums.EFireMode.doubleaction)
                    fireModeString = "DbAction";
                else if (fireMode == Enums.EFireMode.semiauto)
                    fireModeString = "Semi";
                else
                    throw new Exception("Unrecognized weapon fire mode.");

                WeaponFireModeData = new(fireMode, fireModeString);
            }
            catch
            {
                WeaponFireModeData = FireModeData.NotAvailable;
            }
        }

        private static Result<AmmoLevel> GetAmmoLevel_Chambers()
        {
            if (WeaponAmmoData.IsOK())
                return new(true, new(WeaponAmmoData.ChambersData.FilledChambers, WeaponAmmoData.ChambersData.TotalChambers));

            return Result<AmmoLevel>.Fail;
        }

        private static Result<AmmoLevel> GetAmmoLevel_Mag(ulong magSlot)
        {
            try
            {
                int count = 0;
                int max = 0;

                ulong containedItem = Memory.ReadPtr(magSlot + Offsets.Slot.ContainedItem);
                ulong cartridge = Memory.ReadPtr(containedItem + Offsets.LootItemMagazine.Cartridges);
                ulong cartridgeStack = Memory.ReadPtr(cartridge + Offsets.StackSlot._items);

                MemList<ulong> cartridgeStackList = new(cartridgeStack);

                // There should always be at least a single cartridge
                if (cartridgeStackList.Items.Count <= 0)
                    return Result<AmmoLevel>.Fail;

                foreach (var stackSlot in cartridgeStackList.Items)
                {
                    try
                    {
                        int bullet = Memory.ReadValue<int>(stackSlot + Offsets.LootItem.StackObjectsCount, false);

                        count += bullet;
                    }
                    catch { }
                }

                max = Memory.ReadValue<int>(cartridge + Offsets.StackSlot.MaxCount);

                return new(true, new(count, max));
            }
            catch
            {
                return Result<AmmoLevel>.Fail;
            }
        }

        private static void UpdateAmmoLevel(ulong magSlot)
        {
            int count = 0;
            int max = 0;

            Result<AmmoLevel> ammoLevel_chambers = GetAmmoLevel_Chambers();
            Result<AmmoLevel> ammoLevel_mag = GetAmmoLevel_Mag(magSlot);
            if (ammoLevel_mag)
            {
                count += ammoLevel_mag.Value.Count;
                max += ammoLevel_mag.Value.Max;

                if (ammoLevel_chambers)
                    count += ammoLevel_chambers.Value.Count;

                WeaponAmmoLevel = new(count, max);
            }
            else if (ammoLevel_chambers)
            {
                count += ammoLevel_chambers.Value.Count;
                max += ammoLevel_chambers.Value.Max;

                WeaponAmmoLevel = new(count, max);
            }
            else
                WeaponAmmoLevel = AmmoLevel.NotAvailable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private readonly struct Chamber
        {
            public static implicit operator ulong(Chamber x) => x._chamber;

            private readonly ulong _chamber;

            public readonly bool HasBullet()
            {
                if (_chamber == 0x0)
                    return false;

                return Memory.ReadValue<ulong>(_chamber + Offsets.Slot.ContainedItem, false) != 0x0;
            }
        }

        private static Result<ChambersData> GetChamberData_Chambers(ulong weaponBase)
        {
            try
            {
                int filledChambers = 0;
                ulong ammoInChamber = 0x0;

                ulong chambers = Memory.ReadPtr(weaponBase + Offsets.LootItemWeapon.Chambers);
                MemArray<Chamber> chambersArray = new(chambers);
                int totalChambers = chambersArray.Items.Count;

                // There should always be at least a single chamber
                if (totalChambers <= 0)
                    return Result<ChambersData>.Fail;

                foreach (Chamber chamber in chambersArray.Items)
                {
                    if (chamber.HasBullet())
                    {
                        filledChambers++;

                        if (ammoInChamber == 0x0)
                            ammoInChamber = chamber;
                    }
                }

                return new(true, new(ammoInChamber, filledChambers, totalChambers));
            }
            catch
            {
                return Result<ChambersData>.Fail;
            }
        }

        private static Result<ChambersData> GetChamberData_Mag(ulong magSlot)
        {
            try
            {
                int filledChambers = 0;
                ulong ammoInChamber = 0x0;

                ulong mag = Memory.ReadPtr(magSlot + Offsets.Slot.ContainedItem);
                ulong chambers = Memory.ReadPtr(mag + Offsets.LootItemMod.Slots);
                MemArray<Chamber> chambersArray = new(chambers);
                int totalChambers = chambersArray.Items.Count;

                foreach (Chamber chamber in chambersArray.Items)
                {
                    if (chamber.HasBullet())
                    {
                        filledChambers++;

                        if (ammoInChamber == 0x0)
                            ammoInChamber = chamber;
                    }
                }

                // There should always be at least a single chamber
                if (totalChambers <= 0)
                    return Result<ChambersData>.Fail;

                return new(true, new(ammoInChamber, filledChambers, totalChambers));
            }
            catch
            {
                return Result<ChambersData>.Fail;
            }
        }

        private static Result<ChambersData> GetChambersData_MagAlt(ulong magSlot)
        {
            try
            {
                ulong ammoInChamber = 0x0;

                ulong containedItem = Memory.ReadPtr(magSlot + Offsets.Slot.ContainedItem);
                ulong cartridge = Memory.ReadPtr(containedItem + Offsets.LootItemMagazine.Cartridges);
                ulong cartridgeStack = Memory.ReadPtr(cartridge + Offsets.StackSlot._items);

                MemList<ulong> cartridgeStackList = new(cartridgeStack);

                // There should always be at least a single cartridge
                if (cartridgeStackList.Items.Count <= 0)
                    return Result<ChambersData>.Fail;

                ammoInChamber = cartridgeStackList.Items.Last(x => x != 0x0);

                return new(true, new(ammoInChamber, 0, 0));
            }
            catch
            {
                return Result<ChambersData>.Fail;
            }
        }

        private static void UpdateChamberedAmmoData(ulong weaponBase, ulong magSlot)
        {
            try
            {
                ulong containedItem = 0x0;
                ChambersData chambersData;
                Result<ChambersData> chamberData_chambers = GetChamberData_Chambers(weaponBase);
                if (chamberData_chambers)
                    chambersData = chamberData_chambers;
                else
                {
                    Result<ChambersData> chamberData_mag = GetChamberData_Mag(magSlot);
                    if (chamberData_mag)
                        chambersData = chamberData_mag;
                    else
                    {
                        Result<ChambersData> chamberData_magAlt = GetChambersData_MagAlt(magSlot);
                        if (chamberData_magAlt)
                        {
                            chambersData = chamberData_magAlt;
                            containedItem = chambersData.AmmoInChamber;
                        }
                        else
                            throw new Exception("Unable to get chamber data!");
                    }
                }

                if (containedItem == 0x0)
                    containedItem = Memory.ReadPtr(chambersData.AmmoInChamber + Offsets.Slot.ContainedItem);
                ulong ammoTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);

                ulong bulletIdAddress = Memory.ReadPtr(ammoTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                string bulletID = Memory.ReadUnityString(bulletIdAddress);
                if (!AllItems.TryGetValue(bulletID, out JSON.TarkovItem bulletItem))
                    bulletItem = null;

                float bulletMassGrams = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BulletMassGram);
                float bulletDiameterMillimeters = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BulletDiameterMilimeters);
                float ballisticCoeficient = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BallisticCoeficient);
                float initialSpeed = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.InitialSpeed);
                
                float bulletSpeed = initialSpeed + (initialSpeed * WeaponVelocityModifier);

                AmmoData ammoData = new(
                    bulletItem,
                    bulletMassGrams,
                    bulletDiameterMillimeters,
                    ballisticCoeficient,
                    bulletSpeed,
                    chambersData);

                WeaponAmmoData = ammoData;
                ChamberedAmmoTemplate = ammoTemplate;
            }
            catch
            {
                WeaponAmmoData = AmmoData.NotAvailable;
                ChamberedAmmoTemplate = 0x0;
            }
        }

        private static void UpdateWeaponVelocityModifier(ulong weaponBase, ulong weaponTemplate)
        {
            List<ulong> mods = GetWeaponModDeep(weaponBase, null);
            List<ulong> templates = GetWeaponModTemplates(mods);

            float velocityModifier = 0f;

            foreach (ulong template in templates)
            {
                try
                {
                    velocityModifier += Memory.ReadValue<float>(template + Offsets.ModTemplate.Velocity);
                }
                catch { }
            }

            float weaponVelocity = 0f;
            try
            {
                weaponVelocity = Memory.ReadValue<float>(weaponTemplate + Offsets.WeaponTemplate.Velocity);
            }
            catch { }
            
            velocityModifier += weaponVelocity;
            velocityModifier /= 100f;

            if (velocityModifier < -1f || velocityModifier > 1f) // Modifier % should be +-0.0 to 1.0
            {
                Logger.WriteLine($"[LOCAL HANDS MANAGER] -> UpdateTotalWeaponVelocity(): Invalid velocity modifier ~ {velocityModifier}");
                velocityModifier = 0f;
            }

            WeaponVelocityModifier = velocityModifier;
        }

        private static void UpdateScopeIDs(ulong weaponBase)
        {
            HashSet<string> scopeIDs = new(StringComparer.OrdinalIgnoreCase);

            try
            {
                HashSet<Enums.EWeaponModType> searchMods = new()
                {
                    Enums.EWeaponModType.mod_scope,
                };

                bool isAUG = false;
                ulong weaponTemplate = Memory.ReadPtr(weaponBase + Offsets.LootItem.Template);
                ulong weaponIdAddr = Memory.ReadPtr(weaponTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                string weaponID = Memory.ReadUnityString(weaponIdAddr);
                if (weaponID == "62e7c4fba689e8c9c50dfc38" ||
                    weaponID == "63171672192e68c5460cebc5") // Special cases for the 2 AUG variations
                {
                    isAUG = true;
                    searchMods.Add(Enums.EWeaponModType.mod_mount);
                    searchMods.Add(Enums.EWeaponModType.mod_reciever);
                }

                List<ulong> scopes = GetWeaponModDeep(weaponBase, searchMods);
                if (scopes.Count == 0)
                    return;

                foreach (ulong scopeBase in scopes)
                {
                    try
                    {
                        ulong scopeItem = Memory.ReadPtr(scopeBase + Offsets.Slot.ContainedItem);
                        ulong template = Memory.ReadPtr(scopeItem + Offsets.LootItem.Template);
                        ulong idAddress = Memory.ReadPtr(template + Offsets.ItemTemplate._id + Offsets.MongoID._stringID);
                        string scopeID = Memory.ReadUnityString(idAddress);

                        if (isAUG)
                        {
                            if (scopeID == "62ea7c793043d74a0306e19f" ||
                                scopeID == "62ebd290c427473eff0baafb")
                            {
                                scopeIDs.Add(scopeID);
                            }
                        }
                        else
                            scopeIDs.Add(scopeID);
                    }
                    catch { }
                }
            }
            catch { }

            WeaponScopeIDs = scopeIDs;
        }

        #endregion

        private static ulong GetWeaponMod(ulong weaponBase, Enums.EWeaponModType searchMod)
        {
            try
            {
                ulong slotsAddress = Memory.ReadPtr(weaponBase + Offsets.LootItemMod.Slots);
                MemArray<ulong> slots = new(slotsAddress);

                foreach (ulong slot in slots.Items)
                {
                    try
                    {
                        ulong namePtr = Memory.ReadPtr(slot + Offsets.Slot.ID);
                        string slotNameRaw = Memory.ReadUnityString(namePtr);
                        string slotName = TrailingNumberRegex().Replace(slotNameRaw, "");

                        var modName = EnumHelper.TryGetValue<Enums.EWeaponModType>(slotName);
                        if (modName && modName == searchMod)
                            return slot;
                    }
                    catch { }
                }
            }
            catch { }

            return 0x0;
        }

        private static List<ulong> GetWeaponModDeep(ulong weaponBase, HashSet<Enums.EWeaponModType> searchMods)
        {
            List<ulong> mods = new();
            GetWeaponModDeepInternal(weaponBase, searchMods, mods);
            return mods;
        }

        private static void GetWeaponModDeepInternal(ulong address, HashSet<Enums.EWeaponModType> searchMods, List<ulong> mods)
        {
            try
            {
                ulong slotsAddress = Memory.ReadPtr(address + Offsets.LootItemMod.Slots);
                MemArray<ulong> slots = new(slotsAddress);

                foreach (ulong slot in slots.Items)
                {
                    if (!MemoryUtils.IsValidAddress(slot))
                        continue;

                    try
                    {
                        // Try to iterate through child slots
                        ulong childSlot = Memory.ReadPtr(slot + Offsets.Slot.ContainedItem);
                        GetWeaponModDeepInternal(childSlot, searchMods, mods);
                    }
                    catch { }

                    try
                    {
                        // If searchMods is null add all mods
                        if (searchMods == null)
                        {
                            mods.Add(slot);
                            continue;
                        }

                        ulong namePtr = Memory.ReadPtr(slot + Offsets.Slot.ID);
                        string slotNameRaw = Memory.ReadUnityString(namePtr);
                        string slotName = TrailingNumberRegex().Replace(slotNameRaw, "");

                        var modName = EnumHelper.TryGetValue<Enums.EWeaponModType>(slotName);
                        if (modName && searchMods.Contains(modName))
                            mods.Add(slot);
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static List<ulong> GetWeaponModTemplates(List<ulong> mods)
        {
            List<ulong> templates = new();

            foreach (ulong mod in mods)
            {
                try
                {
                    ulong item = Memory.ReadPtr(mod + Offsets.Slot.ContainedItem);
                    ulong template = Memory.ReadPtr(item + Offsets.LootItem.Template);

                    templates.Add(template);
                }
                catch { }
            }

            return templates;
        }
    }
}
