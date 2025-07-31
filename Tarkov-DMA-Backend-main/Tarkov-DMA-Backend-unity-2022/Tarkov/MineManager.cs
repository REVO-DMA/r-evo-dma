using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov
{
    public sealed class MineManager
    {
        public IReadOnlyList<Claymore> Claymores;
        public IReadOnlyList<BorderZone> Mines;

        public MineManager(ulong mineDirectional, ulong localGameWorld)
        {
            // Get all claymores
            List<Claymore> refClaymores = new();

            ulong claymoresListAddr = Memory.ReadPtr(mineDirectional + Offsets.MineDirectional.Mines, false);
            var claymoresList = new MemList<ulong>(claymoresListAddr, false);
            var claymores = claymoresList.Items;

            foreach (ulong claymore in claymores)
            {
                refClaymores.Add(new Claymore(claymore));
            }

            Claymores = refClaymores;

            // Try to get all normal mines and sniper zones (fails in offline)
            try
            {
                List<BorderZone> refMines = new();

                ulong borderZonesAddr = Memory.ReadPtr(localGameWorld + Offsets.ClientLocalGameWorld.BorderZones, false);
                var borderZonesArray = new MemArray<ulong>(borderZonesAddr, false);
                var borderZones = borderZonesArray.Items;
                foreach (ulong borderZone in borderZones)
                {
                    try
                    {
                        refMines.Add(new BorderZone(borderZone));
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[Mine Manager] -> ERROR: Unable to add a BorderZone at 0x{borderZone:X}: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[Mine Manager] -> CRITICAL ERROR: {ex}");
            }
        }
    }

    public sealed class Claymore
    {
        public readonly Vector3 Position;
        public readonly Vector3 Rotation;
        public readonly float TriggerDistance;
        public readonly float ExplosionCone;

        public Claymore(ulong address)
        {
            ulong settings = address + Offsets.MineDirectional.MineData;

            ulong transformInternal = Memory.ReadPtrChain(address, new uint[] { 0x10, 0x30, 0x30, 0x8, 0x70, 0x10 }, false);
            UnityTransform transform = new(transformInternal);

            Position = transform.GetPosition();
            Rotation = transform.GetRotation().ToEuler();
            TriggerDistance = Memory.ReadValue<float>(settings + Offsets.MineSettings._maxExplosionDistance, false);
            ExplosionCone = Memory.ReadValue<float>(settings + Offsets.MineSettings._directionalDamageAngle, false);
        }
    }

    public sealed class BorderZone
    {
        public readonly string Name;
        public readonly Vector3 Position;
        public readonly Vector3 Extents;

        public BorderZone(ulong address)
        {
            ulong classNamePtr = Memory.ReadPtrChain(address, UnityOffsets.Component.To_NativeClassName, false);
            string className = Memory.ReadUtf8String(classNamePtr, 64, false);

            if (className == "SniperFiringZone")
                Name = "Sniper Zone";
            else if (className == "Minefield")
                Name = "Minefield";
            else
            {
                Logger.WriteLine($"[Mine Manager] -> WARNING: Unknown BorderZone class: \"{className}\".");
                Name = className;
            }

            ulong transformInternal = Memory.ReadPtrChain(address, new uint[] { 0x10, 0x30, 0x30, 0x8, 0x38 }, false);

            Position = Memory.ReadValue<Vector3>(transformInternal + 0x90, false);
            Extents = Memory.ReadValue<Vector3>(address + Offsets.BorderZone._extents, false);
        }
    }
}
