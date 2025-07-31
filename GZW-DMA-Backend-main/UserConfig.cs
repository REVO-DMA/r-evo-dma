using LightJson;
using FileWatcherEx;
using Kokuban;

namespace gzw_dma_backend
{
    public sealed class UserConfig
    {
        public int SelectedMonitor { get; set; } = 0;
        public bool Moonlight { get; set; } = false;
        public bool FullScreen { get; set; } = true;
        public bool MemoryAimbot { get; set; } = false;
        public bool SpeedHack { get; set; } = false;
        public int SpeedMultiplier { get; set; } = 3;
        public bool InfiniteSprint { get; set; } = false;
        public bool GodMode { get; set; } = false;

        private const string ConfigFile = "Config.json";

        private static readonly object _lock = new();

        private static readonly FileSystemWatcherEx _fsWatcher;

        static UserConfig()
        {
            _fsWatcher = new(Directory.GetCurrentDirectory());

            WatchConfig();
        }

        private static void WatchConfig()
        {
            _fsWatcher.OnChanged += OnChanged;

            _fsWatcher.Start();
        }

        private static void OnChanged(object sender, FileChangedEvent e)
        {
            if (Path.GetFileName(e.FullPath) != ConfigFile)
                return;

            if (!TryLoadConfig(out var userConfig))
                Console.WriteLine(Chalk.Red + $"ERROR - {ConfigFile} file is formatted improperly! Please make sure all values are correct.");
            else
            {
                Program.UserConfig = userConfig;
            }
        }

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
                MemoryAimbot = parsed["memoryAimbot"].IsNull ? throw new Exception("memoryAimbot is null!") : parsed["memoryAimbot"].AsBoolean,
                SpeedHack = parsed["speedHack"].IsNull ? throw new Exception("speedHack is null!") : parsed["speedHack"].AsBoolean,
                SpeedMultiplier = parsed["speedMultiplier"].IsNull ? throw new Exception("speedMultiplier is null!") : parsed["speedMultiplier"].AsInteger,
                InfiniteSprint = parsed["infiniteSprint"].IsNull ? throw new Exception("infiniteSprint is null!") : parsed["infiniteSprint"].AsBoolean,
                GodMode = parsed["godMode"].IsNull ? throw new Exception("godMode is null!") : parsed["godMode"].AsBoolean,
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
                ["memoryAimbot"] = new JsonValue(data.MemoryAimbot),
                ["speedHack"] = new JsonValue(data.SpeedHack),
                ["speedMultiplier"] = new JsonValue(data.SpeedMultiplier),
                ["infiniteSprint"] = new JsonValue(data.InfiniteSprint),
                ["godMode"] = new JsonValue(data.GodMode),
            }.ToString(pretty: true);

            return json;
        }
    }
}
