using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov
{
    public sealed class TripwireManager
    {
        public readonly ConcurrentDictionary<ulong, Tripwire> Tripwires = new();
        
        private ulong LastUpdate = 0;

        private readonly ulong _lgw;

        public TripwireManager(ulong localGameWorld)
        {
            _lgw = localGameWorld;
        }

        public void Update()
        {
            ulong synchronizableObjectLogicProcessor = Memory.ReadPtr(_lgw + Offsets.ClientLocalGameWorld.SynchronizableObjectLogicProcessor);
            ulong synchronizableObjects = Memory.ReadPtr(synchronizableObjectLogicProcessor + Offsets.SynchronizableObjectLogicProcessor.SynchronizableObjects);
            MemList<ulong> tripwires = new(synchronizableObjects);

            LastUpdate++;

            foreach (ulong tripwire in tripwires.Items)
            {
                try
                {
                    var objectType = (Enums.SynchronizableObjectType)Memory.ReadValue<int>(tripwire + Offsets.SynchronizableObject.Type);
                    if (objectType != Enums.SynchronizableObjectType.Tripwire)
                        continue;

                    var state = (Enums.ETripwireState)Memory.ReadValue<int>(tripwire + Offsets.TripwireSynchronizableObject._tripwireState);
                    
                    // Only update "healthy" tripwires
                    if (!Tripwire.IsHealthy(state))
                        continue;

                    if (Tripwires.TryGetValue(tripwire, out Tripwire allocated))
                    {
                        allocated.LastUpdate = LastUpdate;
                        continue;
                    }

                    Tripwire newTripwire = new(tripwire, LastUpdate);
                    Tripwires.TryAdd(tripwire, newTripwire);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[TRIPWIRE] Failed to allocate ~ {ex}");
                }
            }

            CleanUp();
        }

        private void CleanUp()
        {
            foreach (var tripwire in Tripwires)
            {
                if (tripwire.Value.LastUpdate != LastUpdate)
                    Tripwires.TryRemove(tripwire.Key, out _);
            }
        }

        public sealed class Tripwire
        {
            public ulong LastUpdate;
            
            public readonly string ShortName = "";
            public readonly Vector3 FromPosition;
            public readonly Vector3 ToPosition;

            private static readonly Vector3 HeightOffset = new(0f, 0.17f, 0f);

            public Tripwire(ulong tBase, ulong lastUpdate)
            {
                ulong idPtr = Memory.ReadPtr(tBase + Offsets.TripwireSynchronizableObject.GrenadeTemplateId + Offsets.MongoID._stringID, false);
                string id = Memory.ReadUnityString(idPtr, false);
                if (AllItems.TryGetValue(id, out var entry) && entry != null)
                    ShortName = entry.ShortName;

                FromPosition = Memory.ReadValue<Vector3>(tBase + Offsets.TripwireSynchronizableObject.FromPosition, false) + HeightOffset;
                ToPosition = Memory.ReadValue<Vector3>(tBase + Offsets.TripwireSynchronizableObject.ToPosition, false) + HeightOffset;

                LastUpdate = lastUpdate;
            }

            public static bool IsHealthy(Enums.ETripwireState state)
            {
                if (state == Enums.ETripwireState.Wait ||
                    state == Enums.ETripwireState.Active)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
