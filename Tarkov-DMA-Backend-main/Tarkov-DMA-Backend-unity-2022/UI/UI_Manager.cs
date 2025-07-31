using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.IPC.Serializers;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov;
using Tarkov_DMA_Backend.Tarkov.Loot;

namespace Tarkov_DMA_Backend.UI
{
    public static class UI_Manager
    {
        private const bool ENABLE_MAP_ALIGNMENT_MODE = false;

        public static int RealtimeUpdateFrequency = 16;
        public static volatile bool ShouldSendLootToGUI = false;
        private const int _exfilUpdateFrequency = 3000;

        private static readonly Stopwatch _LootStopwatch;
        private static readonly Stopwatch _ExfilsStopwatch;

        private static readonly Thread _t1;
        private static readonly Thread _t2;

        static UI_Manager()
        {
            try
            {
                Logger.WriteLine("Instantiating UI Manager...");

                _LootStopwatch = new();
                _LootStopwatch.Start();

                _ExfilsStopwatch = new();
                _ExfilsStopwatch.Start();

                // Init threads
                _t1 = new Thread(RealtimeWorker)
                {
                    IsBackground = true
                };
                _t2 = new Thread(DeferredWorker)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal
                };
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[UI MANAGER] Early fail: {ex}");
                MessageBox.ShowError($"[UI MANAGER] Early fail: {ex.Message}", "EVO DMA");
                Environment.Exit(1);
            }
        }

        public static void Start()
        {
            try
            {
                Logger.WriteLine("Starting UI Manager...");

                _t1.Start();
                _t2.Start();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[UI MANAGER] Start() -> exception: {ex}");
                MessageBox.ShowError($"[UI MANAGER] Start() -> exception: {ex.Message}", "EVO DMA");
                Environment.Exit(1);
            }
        }

        private static void RealtimeWorker()
        {
            Exception caughtEx = null;

            try
            {
                Logger.WriteLine("Radar realtime thread is starting...");

                while (true)
                {
                    if (EFTDMA.InRaid)
                    {
                        Realtime();
                        Thread.Sleep(RealtimeUpdateFrequency);
                    }
                    else
                        Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                caughtEx = ex;
                Logger.WriteLine($"CRITICAL ERROR on Radar realtime thread: {ex}"); // Log CRITICAL error
            }
            finally
            {
                Logger.WriteLine($"[UI MANAGER] RealtimeWorker() -> exception: {caughtEx}");
                MessageBox.ShowError($"[UI MANAGER] RealtimeWorker() -> exception: {caughtEx?.Message}", "EVO DMA");
                Environment.Exit(1);
            }
        }

        private static void DeferredWorker()
        {
            Exception caughtEx = null;

            try
            {
                Logger.WriteLine("Radar deferred thread is starting...");
                while (true)
                {
                    if (EFTDMA.InRaid)
                    {
                        DeferredInRaid();
                        Thread.Sleep(100);
                    }
                    else
                        Thread.Sleep(1000);

                    DeferredAlways();
                }
            }
            catch (Exception ex)
            {
                caughtEx = ex;
                Logger.WriteLine($"CRITICAL ERROR on Radar deferred thread: {ex}"); // Log CRITICAL error
            }
            finally
            {
                Logger.WriteLine($"[UI MANAGER] DeferredWorker() -> exception: {caughtEx}");
                MessageBox.ShowError($"[UI MANAGER] DeferredWorker() -> exception: {caughtEx?.Message}", "EVO DMA");
                Environment.Exit(1);
            }
        }

        private static void Realtime()
        {
            Player LocalPlayer = EFTDMA.LocalPlayer;
            Player[] Players = EFTDMA.DisplayPlayers;

            if (LocalPlayer == null) return;
            if (Players == null) return;
            if (UICache.MapConfig == null) return;

            try
            {
                int playersLength = 1;

                if (Players.Length > 0)
                    playersLength += Players.Length;

                IPC_RealtimePlayer[] ipcPlayers = new IPC_RealtimePlayer[playersLength];

                int playersCurrentIndex = 0;
                // Add LocalPlayer
                MapPosition localPlayerPositionRaw = LocalPlayer.Position.ToMapPos(UICache.MapConfig);
                IPC_Vector2 localPlayerPosition = new(localPlayerPositionRaw.X, localPlayerPositionRaw.Y);
                float localPlayerRadians = LocalPlayer.Rotation.X.ToRadians();

                ipcPlayers[playersCurrentIndex++] = new(LocalPlayer.ID, localPlayerPosition, localPlayerRadians);

                // Add all non-LocalPlayer players
                if (Players?.Length > 0)
                {
                    foreach (Player Player in Players)
                    {
                        if (Player == null) continue;

                        if (Player.Status != Constants.PlayerStatuses.Alive) continue;

                        MapPosition positionRaw = Player.Position.ToMapPos(UICache.MapConfig);
                        IPC_Vector2 position = new(positionRaw.X, positionRaw.Y);
                        float radians = Player.Rotation.X.ToRadians();

                        ipcPlayers[playersCurrentIndex++] = new(Player.ID, position, radians);
                    }
                }

                IPC_RealtimeGrenade[] ipcGrenades = [];

                var Grenades = EFTDMA.Grenades;
                if (Grenades != null)
                {
                    int grenadeCount = Grenades.Count;
                    if (grenadeCount > 0)
                    {
                        ipcGrenades = new IPC_RealtimeGrenade[grenadeCount];

                        int grenadesCurrentIndex = 0;
                        foreach (var grenade in Grenades.Values)
                        {
                            if (!grenade.IsActive) continue;

                            MapPosition grenadePositionRaw = grenade.Position.ToMapPos(UICache.MapConfig);
                            IPC_Vector2 grenadePosition = new(grenadePositionRaw.X, grenadePositionRaw.Y);
                            int grenadeDistance = (int)Vector3.Distance(LocalPlayer.Position, grenade.Position);

                            ipcGrenades[grenadesCurrentIndex++] = new(grenade.ID, grenadePosition, grenadeDistance);
                        }
                    }
                }

                IPC_Realtime ipcRealtime = new(ipcPlayers, ipcGrenades);
                byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcRealtime);
                Server.RadarSend(Constants.MessageTypes.RealtimePlayer, serializedData);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Radar Realtime [FAIL] {ex}");
            }
        }

        /// <summary>
        /// This only runs when in raid.
        /// </summary>
        private static  void DeferredInRaid()
        {
            UpdateDeferredPlayerInfo();

            // Loot
            if (ShouldSendLootToGUI)
            {
                _LootStopwatch.Restart();
                UpdateLoot();
                ShouldSendLootToGUI = false;
            }

            // Exfils
            if (_ExfilsStopwatch.ElapsedMilliseconds >= _exfilUpdateFrequency)
            {
                _ExfilsStopwatch.Restart();
                UpdateExfils();
            }
        }

        /// <summary>
        /// This runs all the time, even out of raid.
        /// </summary>
        public static void DeferredAlways()
        {
            // Update map cache
            UICache.MapConfig = MapsManager.GetMapConfig(EFTDMA.MapID);

            if (ENABLE_MAP_ALIGNMENT_MODE)
            {
                if (UICache.MapConfig != null)
                {
                    try
                    {
                        var data = File.ReadAllLines("mapData.evo");
                        UICache.MapConfig.X = float.Parse(data[0].Split("=")[1]);
                        UICache.MapConfig.Y = float.Parse(data[1].Split("=")[1]);
                        UICache.MapConfig.Scale = float.Parse(data[2].Split("=")[1]);
                        UICache.MapConfig.Rotation = float.Parse(data[3].Split("=")[1]);
                    }
                    catch
                    {
                        Logger.WriteLine("Error parsing new map data!");
                    }
                }
            }

            // Check if map changed
            if (UICache.MapConfig is not null && UICache.MapConfig.MapID != UICache.CurrentMap)
            {
                if (ENABLE_MAP_ALIGNMENT_MODE)
                    File.WriteAllText("mapData.evo", $"X = {UICache.MapConfig.X}\r\nY = {UICache.MapConfig.Y}\r\nScale = {UICache.MapConfig.Scale}\r\nRot = {UICache.MapConfig.Rotation}\r\n");

                // Update current map
                UICache.CurrentMap = UICache.MapConfig.MapID;
                // Send new map to UI
                UpdateMap();
            }
        }

        private static void UpdateDeferredPlayerInfo()
        {
            Player LocalPlayer = EFTDMA.LocalPlayer;
            Player[] Players = EFTDMA.DisplayPlayers;
            JSON.MapConfig MapConfig = UICache.MapConfig;

            if (LocalPlayer == null) return;
            if (MapConfig == null) return;

            try
            {
                int playersLength = 1;

                if (Players != null && Players.Length > 0)
                    playersLength += Players.Length;

                IPC_DeferredPlayer[] ipcPlayers = new IPC_DeferredPlayer[playersLength];

                int playersCurrentIndex = 0;
                // Add LocalPlayer
                MapPosition localPlayerPositionRaw = LocalPlayer.Position.ToMapPos(MapConfig);
                short localPlayerHeight = (short)Math.Round(localPlayerPositionRaw.Height);

                if (ENABLE_MAP_ALIGNMENT_MODE)
                    Logger.WriteLine($"[{localPlayerHeight}] {LocalPlayer.Position} ({localPlayerPositionRaw.X}, {localPlayerPositionRaw.Y})");

                string LocalPlayerValue;
                if (LocalPlayer.Gear == null)
                    LocalPlayerValue = null;
                else
                    LocalPlayerValue = FormatPrice(LocalPlayer.Gear.Value);

                ipcPlayers[playersCurrentIndex++] = new(LocalPlayer.ID, LocalPlayer.HealthPercent, 0, localPlayerHeight, LocalPlayerValue, null);

                if (Players != null && Players.Length > 0)
                {
                    // Add all non-LocalPlayer players
                    foreach (Player player in Players)
                    {
                        if (player.Status != Constants.PlayerStatuses.Alive) continue;

                        MapPosition positionRaw = player.Position.ToMapPos(MapConfig);
                        short height = (short)Math.Round(positionRaw.Height - localPlayerPositionRaw.Height);

                        string PlayerValue;
                        if (player.Gear == null)
                            PlayerValue = null;
                        else
                            PlayerValue = FormatPrice(player.Gear.Value);

                        ipcPlayers[playersCurrentIndex++] = new(player.ID, player.HealthPercent, (ushort)player.Distance, height, player.ItemInHands, PlayerValue);
                    }
                }

                byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcPlayers);
                Server.RadarSend(Constants.MessageTypes.DeferredPlayer, serializedData);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Radar Deferred [FAIL] {ex}");
            }
        }

        private static void UpdateLoot()
        {
            if (UICache.MapConfig == null) return;
            if (EFTDMA.Loot == null) return;
            if (EFTDMA.Loot.DisplayLoot == null) return;

            var allLootItems = EFTDMA.Loot.AllLoot;
            var shownLootItems = EFTDMA.Loot.DisplayLoot;
            int allLootItemsCount = allLootItems.Count;
            int shownLootItemsCount = shownLootItems.Count;

            try
            {
                var questLocations = QuestManager.ActiveQuests;
                int questLocationsCount = 0;
                if (ShowQuestItems && questLocations != null)
                    questLocationsCount = questLocations.Count();

                RadarLoot.LootItem[] lootItems_shown = new RadarLoot.LootItem[shownLootItemsCount];

                int lootItemsIndex = 0;
                foreach (var lootItemRaw in shownLootItems)
                {
                    var lootItem = lootItemRaw.Value;

                    // Get the first item in the container
                    if (lootItem is LootContainer container && container.FilteredDisplayItems.Count > 0)
                    {
                        lootItem = container.FilteredDisplayItems[0];
                    }

                    var positionRaw = lootItem.Position.ToMapPos(UICache.MapConfig);
                    lootItems_shown[lootItemsIndex++] = new(lootItemRaw.Key, true, lootItem._item?.ID, lootItem.GetLootType(), lootItem.GetLabel(), new RadarLoot.LootItemPosition(positionRaw.X, positionRaw.Y, (short)positionRaw.Height));
                }

                // Get all items from the gameworld
                Dictionary<ulong, LootItem> actuallyAllItems = new();
                foreach (var lootItemRaw in allLootItems)
                {
                    var lootItem = lootItemRaw.Value;

                    if (lootItem is LootContainer container && container.AllContainedItems.Count > 0)
                    {
                        foreach (var containerLootItem in container.AllContainedItems)
                        {
                            if (containerLootItem == null || actuallyAllItems.ContainsKey(containerLootItem.BaseAddress)) continue;

                            actuallyAllItems.TryAdd(containerLootItem.BaseAddress, containerLootItem);
                        }
                    }
                    else
                        actuallyAllItems.TryAdd(lootItemRaw.Key, lootItem);
                }

                // Sort all items by price
                var lootItems_sorted = actuallyAllItems.OrderByDescending(x => x.Value.Price);

                // Add all items to array
                List<RadarLoot.LootItem> lootItems_all = new();
                foreach (var lootItemRaw in lootItems_sorted)
                {
                    var lootItem = lootItemRaw.Value;

                    // Get the first item in the container
                    if (lootItem is LootContainer container && container.FilteredDisplayItems.Count > 0)
                    {
                        lootItem = container.FilteredDisplayItems[0];
                    }

                    var positionRaw = lootItem.Position.ToMapPos(UICache.MapConfig);
                    
                    // Don't add quest items
                    if (lootItem.GetLootType() == 4) continue;

                    lootItems_all.Add(new(lootItemRaw.Key, false, lootItem._item?.ID, lootItem.GetLootType(), lootItem.GetLabel(), new RadarLoot.LootItemPosition(positionRaw.X, positionRaw.Y, (short)positionRaw.Height)));
                }

                if (ShowQuestItems && questLocations != null)
                {
                    var questLocationsArray = new RadarLoot.LootItem[questLocationsCount];
                    lootItemsIndex = 0;
                    foreach (var location in questLocations)
                    {
                        var positionRaw = location.Position.ToMapPos(UICache.MapConfig);
                        questLocationsArray[lootItemsIndex++] = new(0, true, location.Description, 0, location.Name, new RadarLoot.LootItemPosition(positionRaw.X, positionRaw.Y, (short)positionRaw.Height));
                    }

                    Server.RadarSend(RadarLoot.Serialize(new(Constants.MessageTypes.QuestLocations, questLocationsArray)));
                    Thread.Sleep(50);
                }

                // Join item arrays
                var lootItems_combined = lootItems_all.Concat(lootItems_shown).ToArray();

                RadarLoot.Packet Packet = new(Constants.MessageTypes.RadarLoot, lootItems_combined);
                Server.RadarSend(RadarLoot.Serialize(Packet));
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"RadarManager - UpdateLoot: ERROR updating loot: {ex}");
            }
        }

        private static void UpdateExfils()
        {
            if (UICache.MapConfig == null) return;
            if (EFTDMA.Exfils == null) return;
            if (EFTDMA.Exfils.Count == 0) return;

            try
            {
                int exfilsCount = EFTDMA.Exfils.Count;
                IPC_Exfil[] ipcExfils = new IPC_Exfil[exfilsCount];

                int currentExfilIndex = 0;
                foreach (var exfil in EFTDMA.Exfils)
                {
                    if (exfil == null || exfil.Name.Length == 0) continue;

                    var positionRaw = exfil.Position.ToMapPos(UICache.MapConfig);
                    ipcExfils[currentExfilIndex++] = new(exfil.ID, exfil.Name, exfil.Description, new IPC_Vector2(positionRaw.X, positionRaw.Y), exfil.Status);
                }

                byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcExfils);
                Server.RadarSend(Constants.MessageTypes.RadarExfils, serializedData);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"RadarManager - UpdateExfils: ERROR updating exfils: {ex}");
            }
        }

        private static void UpdateMap()
        {
            string newMap = UICache.CurrentMap;

            byte mapByte;

            if (newMap == "woods")
                mapByte = 3;
            else if (newMap == "shoreline")
                mapByte = 4;
            else if (newMap == "rezervbase")
                mapByte = 5;
            else if (newMap == "laboratory")
                mapByte = 6;
            else if (newMap == "interchange")
                mapByte = 7;
            else if (newMap == "factory")
                mapByte = 8;
            else if (newMap == "bigmap")
                mapByte = 9;
            else if (newMap == "lighthouse")
                mapByte = 10;
            else if (newMap == "tarkovstreets")
                mapByte = 11;
            else if (newMap == "groundzero")
                mapByte = 12;
            else
                return;
            // Reserve up to byte 20 for maps on the IPC Client

            Logger.WriteLine($"[UI MANAGER] Sent map byte for \"{newMap}\".");

            Server.SendRadarStatus(mapByte);
        }
    }
}
