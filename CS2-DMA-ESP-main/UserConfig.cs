using System.Text.Json.Serialization;
using System.Text.Json;
using cs2_dma_esp.Misc;

namespace cs2_dma_esp
{
    public sealed class UserConfig
    {
        [JsonIgnore]
        private const string ConfigFile = "Config.json";
        [JsonIgnore]
        internal static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
        };
        [JsonIgnore]
        private static readonly object _lock = new();

        /// <summary>
        /// Sets the monitor for ESP to render in.
        /// </summary>
        [JsonPropertyName("selectedMonitor")]
        public int SelectedMonitor { get; set; } = 0;


        /// <summary>
        /// Attempt to load Config.json
        /// </summary>
        /// <param name="config">'Config' instance to populate.</param>
        /// <returns></returns>
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
                catch
                {
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
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.UserConfig);
        }

        private static string SerializeUserConfig(UserConfig data)
        {
            return JsonSerializer.Serialize(data, JSONSourceGenerationContext.Default.UserConfig);
        }
    }
}
