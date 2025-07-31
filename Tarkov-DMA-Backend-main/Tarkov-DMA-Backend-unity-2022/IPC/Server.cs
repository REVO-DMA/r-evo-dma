using System.IO.Pipes;
using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.IPC.Serializers;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.IPC
{
    public static class Server
    {
        public static bool _radarClientReady = false;
        public static string _sessionToken = null;
        private static bool _mapsReady = false;
        private static bool _lootReady = false;

        private static NamedPipeServerStream _radarPipeServer;

        private static readonly Thread _receiveThread;
        private static readonly Thread _sendThread;

        static Server()
        {
            _receiveThread = new Thread(IPCWorker)
            {
                IsBackground = true
            };

            _sendThread = new Thread(IPCSender)
            {
                IsBackground= true
            };

            _receiveThread.Start();
            _sendThread.Start();
        }

        public static void Start()
        {
            try
            {
                // Block until IPC Client sends ready message
                Task.Run(() =>
                {
                    Stopwatch stopwatch = new();
                    stopwatch.Start();

                    while (!_radarClientReady)
                    {
                        if (stopwatch.Elapsed.Seconds >= 5) break;
                        Thread.Sleep(100);
                    }
                }).Wait();

                // If IPC Client is not ready after timeout, close program
                if (!_radarClientReady)
                {
                    Logger.WriteLine("[IPC] UI never sent ready status.");
                    Environment.Exit(0);
                }

                Logger.WriteLine("[IPC] Waiting for authentication...");

                // Block until IPC Client sends login and user is validated to have an active subscription
                Task.Run(() =>
                {
                    VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

                    Stopwatch stopwatch = new();
                    stopwatch.Start();

                    Auth.CanLaunchResponse canLaunchResponse = new(false, null, 0, null);

                    while (true)
                    {
                        // Only allow the application to run for 3 minutes if unauthenticated
                        if (stopwatch.Elapsed.Minutes >= 3)
                            break;

                        // Wait for session token
                        if (_sessionToken is null)
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        // Attempt auth
                        Auth auth = new();

                        auth.CanLaunch(_sessionToken, out canLaunchResponse);
                        if (canLaunchResponse.CanLaunch)
                        {
                            Program.AuthCloseTS = DateTime.Now + TimeSpan.FromSeconds(canLaunchResponse.Runtime);

                            IPC_AuthStatus ipcAuthStatus = new(AuthStatusType.CanLaunch, canLaunchResponse.Expiration, canLaunchResponse.Message);
                            byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcAuthStatus);
                            RadarSend(Constants.MessageTypes.AuthStatus, serializedData);
                            break;
                        }
                        else
                        {
                            IPC_AuthStatus ipcAuthStatus = new(AuthStatusType.MembershipExpired, canLaunchResponse.Expiration, canLaunchResponse.Message);
                            byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcAuthStatus);
                            RadarSend(Constants.MessageTypes.AuthStatus, serializedData);
                            Interlocked.Exchange(ref _sessionToken, null);
                        }

                        Thread.Sleep(100);
                    }

                    // If user did not authenticate, close the program
                    if (!canLaunchResponse.CanLaunch)
                    {
                        Logger.WriteLine("[IPC] User never authenticated.");
                        Environment.FailFast(null);
                    }

                    VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();
                }).Wait();

                Logger.WriteLine("[IPC] Waiting for map configs and loot items...");

                // Block until IPC Client sends map configs & loot items
                Task.Run(() =>
                {
                    Stopwatch stopwatch = new();
                    stopwatch.Start();

                    while (!_mapsReady || !_lootReady)
                    {
                        if (stopwatch.Elapsed.Minutes >= 1) break;
                        Thread.Sleep(100);
                    }
                }).Wait();

                // If map configs are not available after timeout, close program
                if (!_mapsReady || !_lootReady)
                {
                    Logger.WriteLine("[IPC] UI never sent map configs and loot items.");
                    Environment.Exit(0);
                }

                Logger.WriteLine("[IPC] UI <-> Backend ready!");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[IPC] Start() exception: {ex}");
                SentrySdk.CaptureException(ex);
                Environment.Exit(1);
            }
        }

        private static void IPCWorker()
        {
            Exception caughtEx = null;

            try
            {
                Logger.WriteLine("[IPC] Thread is starting...");

                _radarPipeServer = new NamedPipeServerStream(Constants.IPC.RadarPipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous, 0, 0);

                Logger.WriteLine("[IPC] Waiting for Radar UI connection...");
                _radarPipeServer.WaitForConnection();
                Logger.WriteLine("[IPC] Radar UI connected!");

                StreamReader _radarStreamReader = new(_radarPipeServer);

                // Handle IPC Client messages
                while (true)
                {
                    // Handle Radar message
                    string radarPayload = _radarStreamReader?.ReadLine();
                    if (radarPayload != null)
                    {
                        var parsedPayload = JSON.DeserializeIPCClientMessage(radarPayload);
                        if (parsedPayload == null) continue;

                        if (parsedPayload.MessageType == 1) // IPC Client ready
                            _radarClientReady = true;
                        else if (parsedPayload.MessageType == Constants.MessageTypes.RadarFeature)
                            ToolkitManager.SyncFeatures(JSON.DeserializeFeatureSync(parsedPayload.Data));
                        else if (parsedPayload.MessageType == Constants.MessageTypes.FeatureToggleHotkeysSync)
                            ToolkitManager.SyncFeatureToggleHotkeys(JSON.DeserializeFeatureToggleHotkeySync(parsedPayload.Data));
                        else if (parsedPayload.MessageType == Constants.MessageTypes.MapConfigs)
                        {
                            MapsManager.LoadMapConfigs(JSON.DeserializeMapConfigs(parsedPayload.Data));
                            _mapsReady = true;
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.ESPConfig)
                            ESP_Config.SyncConfig(JSON.DeserializeEspSync(parsedPayload.Data));
                        else if (parsedPayload.MessageType == Constants.MessageTypes.EspPlayerTypeStyles)
                            ESP_Style.SyncStyles(JSON.DeserializeEspStyleSync(parsedPayload.Data));
                        else if (parsedPayload.MessageType == Constants.MessageTypes.LootItems)
                        {
                            ItemManager.CreteItemsDict(JSON.DeserializeTarkovItems(parsedPayload.Data));
                            _lootReady = true;
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.ItemFilter)
                        {
                            ItemManager.SetItemFilter(JSON.DeserializeItemFilterSync(parsedPayload.Data));
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.AuthLogin)
                        {
                            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

                            JSON.AuthLogin credentials = JSON.DeserializeAuthLogin(parsedPayload.Data);

                            if (credentials == null)
                            {
                                Logger.WriteLine("[AUTH] Got null credentials from client.");
                                IPC_AuthStatus ipcAuthStatus = new(AuthStatusType.InvalidCredentials, null, null);
                                byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcAuthStatus);
                                RadarSend(Constants.MessageTypes.AuthStatus, serializedData);
                                continue;
                            }

                            Logger.WriteLine($"[AUTH] Attempting authentication with Email: {credentials.Email} Password: {credentials.Password}");

                            Auth auth = new();

                            auth.Login(credentials, out var sessionToken);
                            if (sessionToken is null)
                            {
                                Logger.WriteLine("[AUTH] Got null session token from server.");
                                IPC_AuthStatus ipcAuthStatus = new(AuthStatusType.InvalidCredentials, null, null);
                                byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcAuthStatus);
                                RadarSend(Constants.MessageTypes.AuthStatus, serializedData);
                            }
                            
                            Interlocked.Exchange(ref _sessionToken, sessionToken);

                            // Final auth is processed in the Start() method.

                            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.RadarUpdateRate)
                        {
                            JSON.GenericIntValue newUpdateFrequency = JSON.DeserializeGenericIntValue(parsedPayload.Data);

                            if (newUpdateFrequency == null) continue;

                            UI_Manager.RealtimeUpdateFrequency = 1000 / newUpdateFrequency.Value; // Convert from FPS to ms delay
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.LootFilter)
                        {
                            JSON.LootFilter lootFilter = JSON.DeserializeLootFilter(parsedPayload.Data);

                            if (lootFilter == null) continue;

                            ItemManager.SetLootFilter(lootFilter);
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.AllLootFilters)
                        {
                            JSON.LootFilter[] allLootFilters = JSON.DeserializeAllLootFilters(parsedPayload.Data);

                            if (allLootFilters == null) continue;

                            ItemManager.SetAllLootFilters(allLootFilters);
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.ReloadRadar)
                        {
                            EFTDMA.Restart();
                        }
                        else if (parsedPayload.MessageType == Constants.MessageTypes.RadarReadyForRaid)
                        {
                            EFTDMA.RadarReady = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                caughtEx = ex;

                Logger.WriteLine($"[IPC] CRITICAL ERROR on thread: {ex}"); // Log CRITICAL error
                SentrySdk.CaptureException(ex);
            }
            finally
            {
                Logger.WriteLine("[IPC] Thread is stopping...");

                _radarClientReady = false;
                _mapsReady = false;

                Logger.WriteLine($"[IPC] IPCWorker() -> exception: {caughtEx}");
                MessageBox.ShowError($"[IPC] IPCWorker() -> exception: {caughtEx?.Message}", "EVO DMA");
                Environment.Exit(1);
            }
        }

        public static void SendRadarStatus(byte status)
        {
            RadarStatus.Packet statusPacket = new(Constants.MessageTypes.RadarStatus, status);
            byte[] serializedStatusPacket = RadarStatus.Serialize(statusPacket);
            RadarSend(serializedStatusPacket);
        }

        /// <param name="id">The UniqueID of the player this status is for.</param>
        public static void SendPlayerStatus(ushort id, byte status)
        {
            PlayerStatus.Packet statusPacket = new(Constants.MessageTypes.PlayerStatus, new(id, status));
            byte[] serializedStatusPacket = PlayerStatus.Serialize(statusPacket);
            RadarSend(serializedStatusPacket);
        }

        public static void SendFeatureState(string featureID, bool enabled)
        {
            IPC_FeatureState ipcFeatureState = new(featureID, enabled);
            byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcFeatureState);
            RadarSend(Constants.MessageTypes.FeatureState, serializedData);
        }

        public static void RadarSend(byte[] data)
        {
            MessageQueue.Enqueue(data);
        }

        private static byte[] CreatePacket(ushort messageType, byte[] data)
        {
            byte[] messageTypeBytes = BitConverter.GetBytes(messageType);

            int newLength = messageTypeBytes.Length + data.Length + Constants.IPC.Delimiter.Length;
            byte[] packet = new byte[newLength];

            Buffer.BlockCopy(messageTypeBytes, 0, packet, 0, messageTypeBytes.Length);
            Buffer.BlockCopy(data, 0, packet, messageTypeBytes.Length, data.Length);
            Buffer.BlockCopy(Constants.IPC.Delimiter, 0, packet, messageTypeBytes.Length + data.Length, Constants.IPC.Delimiter.Length);

            return packet;
        }

        public static void RadarSend(ushort messageType, byte[] data)
        {
            MessageQueue.Enqueue(CreatePacket(messageType, data));
        }

        private static ConcurrentQueue<byte[]> MessageQueue = new();

        private static void IPCSender()
        {
            while (true)
            {
                try
                {
                    if (!_radarClientReady || _radarPipeServer == null || !MessageQueue.TryDequeue(out var message))
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    _radarPipeServer.Write(message);
                    _radarPipeServer.Flush();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[IPC] IPC_Sender(): Error while sending queued message {ex}");
                    SentrySdk.CaptureException(ex);
                }

                Thread.Sleep(1);
            }
        }
    }
}
