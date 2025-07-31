using System.Xml.Linq;
using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov
{
    public sealed class GrenadeManager
    {
        private readonly ulong _localGameWorld;
        private readonly ConcurrentDictionary<ulong, Grenade> _dict = new();
        
        private ulong _base;

        /// <summary>
        /// Keyed Collection of "Hot" grenades in Local Game World.
        /// </summary>
        public IReadOnlyDictionary<ulong, Grenade> Grenades
        {
            get => _dict;
        }
        private ushort _grenadeCount = 0;

        public GrenadeManager(ulong localGameWorld)
        {
            _localGameWorld = localGameWorld;
            UpdateRef();
        }

        private void UpdateRef()
        {
            var grenadesPtr = Memory.ReadPtr(_localGameWorld + Offsets.ClientLocalGameWorld.Grenades);
            _base = Memory.ReadPtr(grenadesPtr + Offsets.GenericCollectionContainer.List);
        }

        /// <summary>
        /// Check for "hot" grenades in LocalGameWorld if due.
        /// </summary>
        public void Refresh()
        {
            try
            {
                UpdateRef();

                var allGrenades = new MemList<ulong>(_base, false);
                foreach (var grenadeAddr in allGrenades.Items)
                {
                    if (_dict.TryGetValue(grenadeAddr, out var existing))
                    {
                        existing.UpdatePos();
                    }
                    else
                    {
                        var grenade = new Grenade(grenadeAddr, _grenadeCount);
                        _grenadeCount++;
                        _dict.TryAdd(grenade, grenade);

                        // Send grenade allocation over IPC
                        IPC_StaticGrenade ipcGrenade = new(grenade.ID, grenade.ShortName);
                        byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcGrenade);
                        Server.RadarSend(Constants.MessageTypes.AllocateGrenade, serializedData);
                    }
                }

                foreach (var grenade in _dict.Values)
                {
                    try
                    {
                        var isDetonated = Memory.ReadValue<bool>(grenade + Offsets.Grenade.IsDestroyed, false);

                        if (isDetonated)
                        {
                            // Send grenade destroy over IPC
                            IPC_StaticGrenade ipcGrenade = new(grenade.ID, grenade.ShortName);
                            byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcGrenade);
                            Server.RadarSend(Constants.MessageTypes.DestroyGrenade, serializedData);

                            _dict.Remove(grenade, out var removed); // Doesn't work on smokes though
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// Represents a 'Hot' grenade in Local Game World.
    /// </summary>
    public sealed class Grenade
    {
        public static implicit operator ulong(Grenade x) => x.Base;
        private readonly Stopwatch _sw = new();
        /// <summary>
        /// The unique ID of this grenade.
        /// </summary>
        public ushort ID { get; }
        /// <summary>
        /// Base Address of Grenade Object.
        /// </summary>
        private ulong Base { get; }
        /// <summary>
        /// Position Pointer for the Vector3 location of this object.
        /// </summary>
        private ulong PosAddr { get; }
        /// <summary>
        /// True if grenade is currently active.
        /// </summary>
        public bool IsActive
        {
            get => _sw.ElapsedMilliseconds < 12000;
        }
        /// <summary>
        /// The ShortName of this throwable.
        /// </summary>
        public string ShortName = null;
        /// <summary>
        /// Unity Position of grenade in Local Game World.
        /// </summary>
        public Vector3 Position { get; private set; }

        public Grenade(ulong baseAddress, ushort uniqueID)
        {
            ID = uniqueID;

            // Get the ShortName of this throwable
            var explosiveItem = Memory.ReadPtr(baseAddress + Offsets.Grenade.WeaponSource, false);
            var itemTemplate = Memory.ReadPtr(explosiveItem + Offsets.LootItem.Template, false);
            var idPtr = Memory.ReadPtr(itemTemplate + Offsets.ItemTemplate._id + Offsets.MongoID._stringID, false);
            var id = Memory.ReadUnityString(idPtr, false);

            if (AllItems.TryGetValue(id, out var entry) && entry != null)
                ShortName = entry.ShortName;

            Base = baseAddress;
            PosAddr = Memory.ReadPtrChain(baseAddress,
            [
                UnityOffsets.EFTClass.To_GameObject[0],
                UnityOffsets.EFTClass.To_GameObject[1],
                UnityOffsets.GameObject.To_PositionDefault[0],
                UnityOffsets.GameObject.To_PositionDefault[1],
                UnityOffsets.GameObject.To_PositionDefault[2] 
            ], false);

            UpdatePos();

            _sw.Start();
        }

        /// <summary>
        /// Get the updated Position of this Grenade.
        /// </summary>
        public void UpdatePos()
        {
            if (!IsActive)
                return;

            Position = Memory.ReadValue<Vector3>(PosAddr + UnityOffsets.UnityPosition.Vector3, false);
        }
    }
}