using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.UI
{
    public static class MapsManager
    {
        public static readonly IReadOnlyDictionary<string, string> MapNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "woods", "woods" },
            { "shoreline", "shoreline" },
            { "rezervbase", "rezervbase" },
            { "laboratory", "laboratory" },
            { "interchange", "interchange" },
            { "factory4_day", "factory" },
            { "factory4_night", "factory" },
            { "bigmap", "bigmap" },
            { "lighthouse", "lighthouse" },
            { "tarkovstreets", "tarkovstreets" },
            { "Sandbox", "groundzero" },
            { "Sandbox_high", "groundzero" }
        };

        public static Dictionary<string, JSON.MapConfig> MapConfigs = new();

        public static void LoadMapConfigs(JSON.MapConfig[] configs)
        {
            Logger.WriteLine("[MAPS MANAGER] LoadMapConfigs()");

            if (configs == null) return;

            foreach (JSON.MapConfig config in configs)
            {
                if (config == null) continue;

                MapConfigs.Add(config.MapID, config);
            }
        }

        public static JSON.MapConfig GetMapConfig(string mapID)
        {
            if (mapID == null) return null;

            if (MapConfigs.TryGetValue(mapID, out JSON.MapConfig config))
                if (config != null) return config;

            return null;
        }
    }
}
