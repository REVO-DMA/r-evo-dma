using LightJson;

namespace pubg_dma_esp
{
    public sealed class UserConfig
    {
        public int SelectedMonitor { get; set; } = 0;
        public bool Moonlight { get; set; } = false;
        public bool FullScreen { get; set; } = true;

        private const string ConfigFile = "Config.json";

        private static readonly object _lock = new();

        /// <summary>
        /// Attempt to load Config.json
        /// </summary>
        /// <param name="config">'Config' instance to populate.</param>
        public static bool TryLoadConfig(out UserConfig config)
        {
            lock (_lock)
            {
                try
                {
                    if (!File.Exists(ConfigFile)) throw new FileNotFoundException($"{ConfigFile} does not exist!");
                    var json = File.ReadAllText(ConfigFile);
                    config = DeserializeUserConfig(json);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex);
                    config = null;
                    return false;
                }
            }
        }
        /// <summary>
        /// Save to Config.json
        /// </summary>
        /// <param name="config">'Config' instance</param>
        public static void SaveConfig(UserConfig config)
        {
            lock (_lock)
            {
                var json = SerializeUserConfig(config);
                File.WriteAllText(ConfigFile, json);
            }
        }

        private static UserConfig DeserializeUserConfig(string data)
        {
            var parsed = JsonValue.Parse(data);

            UserConfig userConfig = new()
            {
                SelectedMonitor = parsed["selectedMonitor"].IsNull ? throw new Exception("selectedMonitor is null!") : parsed["selectedMonitor"].AsInteger,
                Moonlight = parsed["moonlight"].IsNull ? throw new Exception("moonlight is null!") : parsed["moonlight"].AsBoolean,
                FullScreen = parsed["fullScreen"].IsNull ? throw new Exception("fullScreen is null!") : parsed["fullScreen"].AsBoolean,
            };

            return userConfig;
        }

        private static string SerializeUserConfig(UserConfig data)
        {
            var json = new JsonObject
            {
                ["selectedMonitor"] = new JsonValue(data.SelectedMonitor),
                ["moonlight"] = new JsonValue(data.Moonlight),
                ["fullScreen"] = new JsonValue(data.FullScreen),
            }.ToString(pretty: true);

            return json;
        }
    }
}
