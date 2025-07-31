using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Tarkov;
using static MemoryPack.MemoryPackSerializer;

namespace Tarkov_DMA_Backend.Unity.LowLevel.PersistentCache
{
    public static class Cache
    {
        private const string PersistentCacheName = "persistentCache.evo";

        private static PersistentCacheItems ActiveCache = new();
        private static readonly object _cacheLock = new();

        public static bool TryLoad()
        {
            lock (_cacheLock)
            {
                try
                {
                    if (!File.Exists(PersistentCacheName))
                        return false;

                    byte[] persistentCacheRaw = File.ReadAllBytes(PersistentCacheName);

                    var cache = Deserialize<PersistentCacheItems>(persistentCacheRaw);

                    if (EFTDMA.HookAddress != cache.HookAddress)
                    {
                        Logger.WriteLine("[PERSISTENT CACHE] -> TryLoad(): Cache is not valid for this game.");
                        ActiveCache = new();
                        PersistCache();
                        return false;
                    }

                    ActiveCache = cache;

                    Logger.WriteLine("[PERSISTENT CACHE] -> TryLoad(): Loaded saved cache for this game instance.");

                    Logger.WriteLine("================== Persistent Cache Stats ==================\n");

                    Logger.WriteLine($"Hook Address: 0x{ActiveCache?.HookAddress:X}");

                    Logger.WriteLine("");

                    Logger.WriteLine($"Chams Materials: {ActiveCache?.ChamsMaterialsKeys?.Length}");

                    Logger.WriteLine("");

                    Logger.WriteLine($"Resolved Static Classes: {ActiveCache?.ResolvedStaticClassesKeys?.Length}");
                    Logger.WriteLine($"Resolved Classes: {ActiveCache?.ResolvedClassesKeys?.Length}");
                    Logger.WriteLine($"Resolved Singletons: {ActiveCache?.ResolvedSingletonsKeys?.Length}");

                    Logger.WriteLine("\n================== Persistent Cache Stats ==================");

                    return true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[PERSISTENT CACHE] -> TryLoad(): Failed to load cache ~ {ex}");
                    ActiveCache = new();
                    PersistCache();
                }

                return false;
            }
        }

        public static void SaveHookAddress()
        {
            ActiveCache.HookAddress = EFTDMA.HookAddress;

            PersistCache();
        }

        public static void LoadChams()
        {
            if (ActiveCache.ChamsMaterialsKeys?.Length > 0 &&
                ActiveCache.ChamsMaterialsValues?.Length > 0)
            {
                int count = ActiveCache.ChamsMaterialsKeys.Length;
                for (int i = 0; i < count; i++)
                {
                    var key = ActiveCache.ChamsMaterialsKeys[i];
                    var value = ActiveCache.ChamsMaterialsValues[i];

                    ChamsManager.LoadChamsMaterialFromPersistentCache(key, value);
                }
            }
        }

        public static void SaveChams(ConcurrentDictionary<PlayerType, ChamsMaterial> chamsMaterials)
        {
            ActiveCache.ChamsMaterialsKeys = chamsMaterials.Keys.ToArray();
            ActiveCache.ChamsMaterialsValues = chamsMaterials.Values.ToArray();

            PersistCache();
        }

        public static void LoadClasses(Classes classes)
        {
            if (ActiveCache.ResolvedStaticClassesKeys?.Length > 0 &&
                ActiveCache.ResolvedStaticClassesValues?.Length > 0)
            {
                int count = ActiveCache.ResolvedStaticClassesKeys.Length;
                for (int i = 0; i < count; i++)
                {
                    var key = ActiveCache.ResolvedStaticClassesKeys[i];
                    var value = ActiveCache.ResolvedStaticClassesValues[i];

                    classes.ResolvedStaticClasses.TryAdd(key, value);
                }
            }

            if (ActiveCache.ResolvedClassesKeys?.Length > 0 &&
                ActiveCache.ResolvedClassesValues?.Length > 0)
            {
                int count = ActiveCache.ResolvedClassesKeys.Length;
                for (int i = 0; i < count; i++)
                {
                    var key = ActiveCache.ResolvedClassesKeys[i];
                    var value = ActiveCache.ResolvedClassesValues[i];

                    classes.ResolvedClasses.TryAdd(key, value);
                }
            }

            if (ActiveCache.ResolvedSingletonsKeys?.Length > 0 &&
                ActiveCache.ResolvedSingletonsValues?.Length > 0)
            {
                int count = ActiveCache.ResolvedSingletonsKeys.Length;
                for (int i = 0; i < count; i++)
                {
                    var key = ActiveCache.ResolvedSingletonsKeys[i];
                    var value = ActiveCache.ResolvedSingletonsValues[i];

                    classes.ResolvedSingletons.TryAdd(key, value);
                }
            }
        }

        public static void SaveClasses(Classes classes)
        {
            ActiveCache.ResolvedStaticClassesKeys = classes.ResolvedStaticClasses.Keys.ToArray();
            ActiveCache.ResolvedStaticClassesValues = classes.ResolvedStaticClasses.Values.ToArray();

            ActiveCache.ResolvedClassesKeys = classes.ResolvedClasses.Keys.ToArray();
            ActiveCache.ResolvedClassesValues = classes.ResolvedClasses.Values.ToArray();

            ActiveCache.ResolvedSingletonsKeys = classes.ResolvedSingletons.Keys.ToArray();
            ActiveCache.ResolvedSingletonsValues = classes.ResolvedSingletons.Values.ToArray();

            PersistCache();
        }

        private static void PersistCache()
        {
            lock (_cacheLock)
            {
                byte[] serialized = Serialize(ActiveCache);

                File.WriteAllBytes(PersistentCacheName, serialized);
            }
        }
    }
}
