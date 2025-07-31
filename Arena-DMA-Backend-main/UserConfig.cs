using arena_dma_backend.Arena;
using Kokuban;
using FileWatcherEx;
using arena_dma_backend.ESP;
using LightJson;
using arena_dma_backend.Unity;

namespace arena_dma_backend
{
    public sealed class UserConfig
    {
        public bool EnableESP { get; set; } = true;
        public bool FuserMode { get; set; } = false;
        public int ESP_MonitorIndex { get; set; } = 0;
        public bool EnableChams { get; set; } = true;
        public bool NoVisor { get; set; } = false;
        public bool NoInertia { get; set; } = false;
        public bool AlwaysSprint { get; set; } = false;
        public bool NoFlash { get; set; } = false;
        public bool SpeedHack { get; set; } = false;
        public float CustomFOV { get; set; } = 75f;
        public float CustomRecoil { get; set; } = 100f;
        public float CustomSway { get; set; } = 100f;
        public bool DisableExtraWeaponMotion { get; set; } = false;
        public float SmoothingAmount { get; set; } = 30f;
        public float Deadzone { get; set; } = 0.1f;
        public int AimHotkey { get; set; } = 0;
        public int AimBone {  get; set; } = 10;
        public int AimFOV { get; set; } = 30;
        public bool SilentAim { get; set; } = false;
        public bool CQB_Targeting { get; set; } = false;

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
                LogConfig(userConfig);
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

                    HotkeyManager.RegisterHotkey((UnityKeyCode)config.AimHotkey, "aimbot_hotkey");

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

        public static void LogConfig(UserConfig config)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Loaded Config:");

            {
                string espStatus = "\tESP Enabled: ";

                if (config.EnableESP) espStatus += "Yes";
                else espStatus += "No";

                if (config.EnableESP != ESP_Config.ESP_Enabled) espStatus += Chalk.Red + " (pending restart)";

                Console.WriteLine(espStatus);
            }

            {
                string fuserModeStatus = "\tFuser Mode: ";

                if (config.FuserMode) fuserModeStatus += "Yes";
                else fuserModeStatus += "No";

                Console.WriteLine(fuserModeStatus);
            }

            {
                string espMonitor = $"\tESP Monitor: {config.ESP_MonitorIndex}";

                if (config.ESP_MonitorIndex != ESP_Config.ESP_MonitorIndex) espMonitor += Chalk.Red + " (pending restart)";

                Console.WriteLine(espMonitor);
            }

            {
                string chamsStatus = "\tChams Enabled: ";

                if (config.EnableChams) chamsStatus += "Yes";
                else chamsStatus += "No";

                if (config.EnableChams != Game.ChamsEnabled) chamsStatus += Chalk.Red + " (pending restart)";

                Console.WriteLine(chamsStatus);
            }

            if (config.NoVisor) Console.WriteLine($"\tNo Visor: Yes");
            else Console.WriteLine($"\tNo Visor: No");

            if (config.NoInertia) Console.WriteLine($"\tNo Inertia: Yes");
            else Console.WriteLine($"\tNo Inertia: No");

            if (config.AlwaysSprint) Console.WriteLine($"\tAlways Sprint: Yes");
            else Console.WriteLine($"\tAlways Sprint: No");

            if (config.NoFlash) Console.WriteLine($"\tNo Flash: Yes");
            else Console.WriteLine($"\tNo Flash: No");

            if (config.SpeedHack) Console.WriteLine($"\tSpeed Hack: Yes");
            else Console.WriteLine($"\tSpeed Hack: No");

            Console.WriteLine($"\tFOV: {config.CustomFOV}");

            Console.WriteLine($"\tRecoil: {config.CustomRecoil}%");
            Console.WriteLine($"\tSway: {config.CustomSway}%");

            if (config.DisableExtraWeaponMotion) Console.WriteLine($"\tExtra Weapon Motion Disabled: Yes");
            else Console.WriteLine($"\tExtra Weapon Motion Disabled: No");

            if (HotkeyManager.HotkeyIndexToName.TryGetValue(config.AimHotkey, out string hotkeyString))
                Console.WriteLine($"\tAim Key: {hotkeyString}");
            else
                Console.WriteLine($"\tAim Key: ERROR");

            if (Player.BoneIndexToName.TryGetValue(config.AimBone, out string aimBoneName))
                Console.WriteLine($"\tAim Bone: {aimBoneName}");
            else
                Console.WriteLine($"\tAim Bone: ERROR");

            Console.WriteLine($"\tAim FOV: {config.AimFOV}");
            Console.WriteLine($"\tAim Smoothing: {config.SmoothingAmount}");
            Console.WriteLine($"\tAim Deadzone: {config.Deadzone}");

            if (config.SilentAim) Console.WriteLine($"\tSilent Aim: Yes");
            else Console.WriteLine($"\tSilent Aim: No");

            if (config.CQB_Targeting) Console.WriteLine($"\tCQB Targeting: Yes");
            else Console.WriteLine($"\tCQB Targeting: No");

            Console.WriteLine("\n");
        }

        private static UserConfig DeserializeUserConfig(string data)
        {
            var parsed = JsonValue.Parse(data);

            UserConfig userConfig = new()
            {
                EnableESP = parsed["enableESP"].IsNull ? throw new Exception("enableESP is null!") : parsed["enableESP"].AsBoolean,
                FuserMode = parsed["fuserMode"].IsNull ? throw new Exception("fuserMode is null!") : parsed["fuserMode"].AsBoolean,
                ESP_MonitorIndex = parsed["espMonitor"].IsNull ? throw new Exception("espMonitor is null!") : parsed["espMonitor"].AsInteger,
                EnableChams = parsed["enableChams"].IsNull ? throw new Exception("enableChams is null!") : parsed["enableChams"].AsBoolean,
                NoVisor = parsed["noVisor"].IsNull ? throw new Exception("noVisor is null!") : parsed["noVisor"].AsBoolean,
                NoInertia = parsed["noInertia"].IsNull ? throw new Exception("noInertia is null!") : parsed["noInertia"].AsBoolean,
                AlwaysSprint = parsed["alwaysSprint"].IsNull ? throw new Exception("alwaysSprint is null!") : parsed["alwaysSprint"].AsBoolean,
                NoFlash = parsed["noFlash"].IsNull ? throw new Exception("noFlash is null!") : parsed["noFlash"].AsBoolean,
                SpeedHack = parsed["speedHack"].IsNull ? throw new Exception("speedHack is null!") : parsed["speedHack"].AsBoolean,
                CustomFOV = float.Parse(parsed["customFOV"].IsNull ? throw new Exception("customFOV is null!") : parsed["customFOV"].AsString),
                CustomRecoil = float.Parse(parsed["customRecoil"].IsNull ? throw new Exception("customRecoil is null!") : parsed["customRecoil"].AsString),
                CustomSway = float.Parse(parsed["customSway"].IsNull ? throw new Exception("customSway is null!") : parsed["customSway"].AsString),
                DisableExtraWeaponMotion = parsed["disableExtraWeaponMotion"].IsNull ? throw new Exception("disableExtraWeaponMotion is null!") : parsed["disableExtraWeaponMotion"].AsBoolean,
                SmoothingAmount = float.Parse(parsed["smoothingAmount"].IsNull ? throw new Exception("smoothingAmount is null!") : parsed["smoothingAmount"].AsString),
                Deadzone = float.Parse(parsed["deadzone"].IsNull ? throw new Exception("deadzone is null!") : parsed["deadzone"].AsString),
                AimHotkey = parsed["aimHotkey"].IsNull ? throw new Exception("aimHotkey is null!") : parsed["aimHotkey"].AsInteger,
                AimBone = parsed["aimBone"].IsNull ? throw new Exception("aimBone is null!") : parsed["aimBone"].AsInteger,
                AimFOV = parsed["aimFOV"].IsNull ? throw new Exception("aimFOV is null!") : parsed["aimFOV"].AsInteger,
                SilentAim = parsed["silentAim"].IsNull ? throw new Exception("silentAim is null!") : parsed["silentAim"].AsBoolean,
                CQB_Targeting = parsed["cqbTargeting"].IsNull ? throw new Exception("cqbTargeting is null!") : parsed["cqbTargeting"].AsBoolean
            };

            if (!HotkeyManager.HotkeyIndexToName.TryGetValue(userConfig.AimHotkey, out _))
            {
                const string err = "Invalid hotkey number!";
                Console.WriteLine(Chalk.Red + err);
                throw new Exception($"[USER CONFIG] {err}");
            }

            if (!Player.BoneIndexToName.TryGetValue(userConfig.AimBone, out _))
            {
                const string err = "Invalid bone number!";
                Console.WriteLine(Chalk.Red + err);
                throw new Exception($"[USER CONFIG] {err}");
            }

            return userConfig;
        }

        private static string SerializeUserConfig(UserConfig data)
        {
            var json = new JsonObject
            {
                ["enableESP"] = new JsonValue(data.EnableESP),
                ["fuserMode"] = new JsonValue(data.FuserMode),
                ["espMonitor"] = new JsonValue(data.ESP_MonitorIndex),
                ["enableChams"] = new JsonValue(data.EnableChams),
                ["noVisor"] = new JsonValue(data.NoVisor),
                ["noInertia"] = new JsonValue(data.NoInertia),
                ["alwaysSprint"] = new JsonValue(data.AlwaysSprint),
                ["noFlash"] = new JsonValue(data.NoFlash),
                ["speedHack"] = new JsonValue(data.SpeedHack),
                ["customFOV"] = new JsonValue(data.CustomFOV),
                ["customRecoil"] = new JsonValue(data.CustomRecoil),
                ["customSway"] = new JsonValue(data.CustomSway),
                ["disableExtraWeaponMotion"] = new JsonValue(data.DisableExtraWeaponMotion),
                ["smoothingAmount"] = new JsonValue(data.SmoothingAmount),
                ["deadzone"] = new JsonValue(data.Deadzone),
                ["aimHotkey"] = new JsonValue(data.AimHotkey),
                ["aimBone"] = new JsonValue(data.AimBone),
                ["aimFOV"] = new JsonValue(data.AimFOV),
                ["silentAim"] = new JsonValue(data.SilentAim),
                ["cqbTargeting"] = new JsonValue(data.CQB_Targeting),
            }.ToString(pretty: true);

            return json;
        }
    }
}
