using arena_dma_backend.DMA.Collections.Implementation;
using arena_dma_backend.Unity;

namespace arena_dma_backend.Arena
{
    public static class AimbotPrewarmer
    {
        private static Thread _mainThread;
        private static Thread _deferredThread;

        public static Transform Fireport {  get; private set; }
        public static ulong WeaponDirectionGetter { get; private set; }
        public static BallisticContext Context { get; private set; }

        public static void Start()
        {
            _mainThread = new(Main)
            {
                IsBackground = true
            };
            _deferredThread = new(Deferred)
            {
                IsBackground = true
            };

            _mainThread.Start();
            _deferredThread.Start();
        }

        private static void Main()
        {
            while (true)
            {
                if (!Game.InMatch || Game.LocalPlayer == null)
                {
                    Fireport = null;

                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    ulong handsController = Memory.ReadPtr(Game.LocalPlayer.Base + Offsets.Player._handsController, false);

                    ulong weapon = Memory.ReadPtr(handsController + Offsets.ItemHandsController.Item, false);
                    ulong weaponTemplate = Memory.ReadPtr(weapon + Offsets.LootItem.Template);

                    ulong ammoTemplate = GetAmmoTemplateFromWeapon(weapon);

                    // Get bullet stats
                    var mass = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BulletMassGram);
                    var diameter = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BulletDiameterMilimeters);
                    var coefficient = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BallisticCoeficient);
                    var velocity = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.InitialSpeed);
                    
                    float totalVelocity = velocity + (velocity * GetVelocityModifier(weapon, weaponTemplate));

                    Context = new(mass, diameter, coefficient, totalVelocity);

                    Fireport = GetFireport(handsController);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[AIMBOT PREWARMER] Error in main thread: {ex}");
                }

                Thread.Sleep(32);
            }
        }

        private static void Deferred()
        {
            while (true)
            {
                if (!Game.InMatch)
                {
                    WeaponDirectionGetter = 0x0;
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    WeaponDirectionGetter = SilentAim.GetGetter();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[AIMBOT PREWARMER] Error in main thread: {ex}");
                }

                Thread.Sleep(4000);
            }
        }

        private static Transform GetFireport(ulong handsController)
        {
            var firePort = Memory.ReadPtr(handsController + Offsets.FirearmController.Fireport, false);
            var original = Memory.ReadPtr(firePort + 0x10);
            var native = Memory.ReadPtr(original + 0x10);

            return new Transform(native, Transform.TransformType.Normal, true);
        }

        private static float GetVelocityModifier(ulong weapon, ulong weaponTemplate)
        {
            float velMod = 0f;
            GetWeaponAttachmentVelocity(weapon, ref velMod);

            var velocity = Memory.ReadValue<float>(weaponTemplate + Offsets.WeaponTemplate.Velocity, false);

            velMod += velocity;
            velMod /= 100f;

            if (velMod < -1f || velMod > 1f) // Integrity check -> Modifier % should be +-0.0 to 1.0
                throw new Exception($"[AIMBOT PREWARMER] -> GetVelocityModifier(): Invalid velocity modifier: \"{velMod}\"!");

            return velMod;
        }

        private static void GetWeaponAttachmentVelocity(ulong lootItemBase, ref float velocityModifier)
        {
            try
            {
                ulong parentSlots = Memory.ReadPtr(lootItemBase + Offsets.LootItemMod.Slots);
                using MemArray<ulong> slotList = new(parentSlots);

                if (slotList.Count < 0 || slotList.Count > 100)
                    throw new Exception("[AIMBOT PREWARMER] -> GetWeaponAttachmentVelocity(): Invalid weapon slot count!");

                foreach (ulong slot in slotList)
                {
                    try
                    {
                        var containedItem = Memory.ReadPtr(slot + Offsets.Slot.ContainedItem);
                        var itemTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                        // Add this attachment's Velocity %
                        velocityModifier += Memory.ReadValue<float>(itemTemplate + Offsets.ModTemplate.Velocity);
                        GetWeaponAttachmentVelocity(containedItem, ref velocityModifier);
                    }
                    catch { } // Skip over empty slots
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[AIMBOT PREWARMER] -> GetWeaponAttachmentVelocity(): {ex}");
            }
        }

        private static ulong GetAmmoTemplateFromWeapon(ulong weapon)
        {
            ulong chambersPtr = Memory.ReadPtrUnsafe(weapon + Offsets.LootItemWeapon.Chambers);
            ulong firstRound;
            
            using MemArray<Chamber> chambers = new(chambersPtr);
            if (chambersPtr != 0x0 && chambers.Count > 0) // Single chamber, or for some shotguns, multiple chambers
                firstRound = Memory.ReadPtr(chambers.First(x => x.HasBullet(true)) + Offsets.Slot.ContainedItem);
            else
            {
                var magSlot = Memory.ReadPtr(weapon + Offsets.LootItemWeapon._magSlotCache);
                var magItemPtr = Memory.ReadPtr(magSlot + Offsets.Slot.ContainedItem);
                var magChambersPtr = Memory.ReadPtr(magItemPtr + Offsets.LootItemMod.Slots);
                using MemArray<Chamber> magChambers = new(magChambersPtr);
                if (magChambers.Count > 0) // Revolvers, etc.
                    firstRound = Memory.ReadPtr(magChambers.First(x => x.HasBullet(true)) + Offsets.Slot.ContainedItem);
                else // Regular magazines
                {
                    var cartridges = Memory.ReadPtr(magItemPtr + Offsets.LootItemMagazine.Cartridges);
                    var magStackPtr = Memory.ReadPtr(cartridges + Offsets.StackSlot._items);
                    using MemList<ulong> magStack = new(magStackPtr);
                    firstRound = magStack[0];
                }
            }
            return Memory.ReadPtr(firstRound + Offsets.LootItem.Template);
        }

        /// <summary>
        /// Wrapper defining a Chamber Structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private readonly struct Chamber
        {
            public static implicit operator ulong(Chamber x) => x._base;
            private readonly ulong _base;

            public readonly bool HasBullet(bool useCache = false)
            {
                if (_base == 0x0)
                    return false;
                return Memory.ReadValue<ulong>(_base + Offsets.Slot.ContainedItem, useCache) != 0x0;
            }
        }

        public readonly struct BallisticContext(float mass, float diameter, float coefficient, float velocity)
        {
            public readonly float Mass = mass;
            public readonly float Diameter = diameter;
            public readonly float Coefficient = coefficient;
            public readonly float Velocity = velocity;

            public readonly bool IsDifferent(BallisticContext c)
            {
                return Mass != c.Mass ||
                    Diameter != c.Diameter ||
                    Coefficient != c.Coefficient ||
                    Velocity != c.Velocity;
            }
        }
    }
}
