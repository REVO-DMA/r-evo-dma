using cs2_dma_esp.MemDMA;
using cs2_dma_esp.MemDMA.CS2;

namespace cs2_dma_esp.CS2
{
    public static class Game
    {
        public static Player? LocalPlayer;
        public static ConcurrentDictionary<ulong, Player> Players = new();

        public static bool IsInMatch = false;
        public static bool IsFFA = false;
        public static string MapName = "";

        private static readonly Thread _t1;
        private static ulong _updateIteration;

        // Base addresses
        public static ulong ClientDLL;
        public static ulong Tier0DLL;
        public static ulong InputSystemDLL;

        // Interfaces
        public static ulong Cvar;

        // Convars
        public static ulong mpTeammatesAreEnemies;

        public static ulong GlobalVars;

        public static ulong EntityList;
        public static ulong EntityListDeref;

        public static ulong ViewMatrix;
        public static ulong ViewAngles;
        private static ulong LocalController;
        private static ulong LocalPawn;

        // Pointers
        public static ulong EntityListEntry;

        public static ulong LocalControllerAddress;
        public static ulong LocalPawnAddress;
        public static int LocalPlayerControllerIndex;

        private unsafe struct CGlobalVarsBase
        {
            public readonly float real_time;
            public readonly int frame_count;
            private fixed byte _p0[0x8];
            public readonly int max_clients;
            public readonly float interval_per_tick;
            private fixed byte _p1[0x14];
            public readonly float current_time;
            public readonly float current_time_2;
            private fixed byte _p2[0xC];
            public readonly int tick_count;
            public readonly float interval_per_tick_2;
            private fixed byte _p3[0x138];
            public readonly ulong current_map;
            public readonly ulong current_map_name;
        };

        static Game()
        {
            _t1 = new Thread(Realtime)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };

            _t1.Start();
        }

        public static void Init(ulong clientDLL, ulong tier0DLL, ulong inputSystemDLL)
        {
            ClientDLL = clientDLL;
            Tier0DLL = tier0DLL;
            InputSystemDLL = inputSystemDLL;

            GlobalVars = clientDLL + SDK.Offsets.client_dll.dwGlobalVars;

            EntityList = clientDLL + SDK.Offsets.client_dll.dwEntityList;
            ViewMatrix = clientDLL + SDK.Offsets.client_dll.dwViewMatrix;
            ViewAngles = clientDLL + SDK.Offsets.client_dll.dwViewAngles;
            LocalController = clientDLL + SDK.Offsets.client_dll.dwLocalPlayerController;
            LocalPawn = clientDLL + SDK.Offsets.client_dll.dwLocalPlayerPawn;

            CS_Helper.Cvar = CS_Helper.GetInterface(tier0DLL, "VEngineCvar0");
            if (CS_Helper.Cvar == 0x0)
                throw new Exception("[GAME] -> Init(): Unable to get Convar!");

            mpTeammatesAreEnemies = CS_Helper.GetConvar("mp_teammates_are_enemies");
            if (mpTeammatesAreEnemies == 0x0)
                throw new Exception("[GAME] -> Init(): Unable to get \"mp_teammates_are_enemies\"!");

            Input.PreviousXY = inputSystemDLL + SDK.Offsets.inputsystem_dll.dwPreviousXY;
            Input.Sensitivity = CS_Helper.GetConvar("sensitivity");
        }

        public static void UpdateData()
        {
            try
            {
                UpdateMapName();

                UpdateEntityListEntry();

                UpdateFFA();

                LocalControllerAddress = Memory.ReadPtr(LocalController);
                LocalPawnAddress = Memory.ReadPtr(LocalPawn);

                Logger.WriteLine($"Map: {Maps.Names[MapName]} | FFA: {IsFFA}");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME] -> UpdateData(): Error updating game data!", ex);
            }
        }

        private static void UpdateEntityListEntry()
        {
            EntityListDeref = Memory.ReadPtr(EntityList);
            ulong entityListB = Memory.ReadPtr(EntityListDeref + 0x10);

            EntityListEntry = entityListB;
        }

        private static void UpdateMapName()
        {
            try
            {
                ulong globalVars = Memory.ReadPtr(GlobalVars);
                var globalVarsBase = Memory.ReadValue<CGlobalVarsBase>(globalVars);
                MapName = Memory.ReadUtf8String(globalVarsBase.current_map_name, 32);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME] -> UpdateMapName(): Error updating map name!", ex);
            }
        }

        private static void UpdateFFA()
        {
            IsFFA = Memory.ReadValue<int>(mpTeammatesAreEnemies + 0x40) == 1;
        }

        public static void UpdatePlayers()
        {
            _updateIteration++;

            PubgScatterMap scatterMap1 = new(64);
            PubgScatterMap scatterMap2 = new(64);

            var round1 = scatterMap1.AddRound();
            var round2 = scatterMap1.AddRound();

            var round3 = scatterMap2.AddRound();
            var round4 = scatterMap2.AddRound();
            var round5 = scatterMap2.AddRound();
            var round6 = scatterMap2.AddRound();

            for (int i = 0; i < 64; i++)
            {
                var baseAddress = round1.AddEntry<MemPointer>(i, 0, EntityListEntry + (uint)(i + 1) * 0x78);

                round2.AddEntry<int>(i, 1, baseAddress, null, SDK.Client.C_BaseEntity.m_iTeamNum); // TeamID

                round2.AddEntry<uint>(i, 2, baseAddress, null, SDK.Client.CCSPlayerController.m_hPlayerPawn); // Pawn
            }

            scatterMap1.Execute();

            bool[] skipIndex = new bool[64];

            for (int i = 0; i < 64; i++)
            {
                if (!scatterMap1.Results[i][2].TryGetResult<uint>(out var pawn) || pawn == 0)
                {
                    skipIndex[i] = true;
                    continue;
                }

                var entityPawnListEntry = round3.AddEntry<MemPointer>(i, 0, EntityListDeref, null, 0x10 + 8 * ((pawn & 0x7FFF) >> 9));

                var entityPawnAddress = round4.AddEntry<MemPointer>(i, 1, entityPawnListEntry, null, 0x78 * (pawn & 0x1FF));

                // Bone array
                var GameSceneNode = round5.AddEntry<MemPointer>(i, 2, entityPawnAddress, null, SDK.Client.C_BaseEntity.m_pGameSceneNode);

                // Bone array (0x80 currently) is an undocumented offset inside of CModelState
                round6.AddEntry<MemPointer>(i, 3, GameSceneNode, null, SDK.Client.CSkeletonInstance.m_modelState + 0x80); // BoneArrayAddress
            }

            scatterMap2.Execute();

            for (int i = 0; i < 64; i++)
            {
                if (skipIndex[i])
                    continue;

                if (!scatterMap1.Results[i][0].TryGetResult<MemPointer>(out var baseAddress) || !baseAddress.ValidateEx())
                    continue;

                if (!scatterMap1.Results[i][1].TryGetResult<int>(out var teamID))
                    continue;

                if (!scatterMap2.Results[i][1].TryGetResult<MemPointer>(out var pawn) || !pawn.ValidateEx())
                    continue;

                if (!scatterMap2.Results[i][3].TryGetResult<MemPointer>(out var boneArray) || !boneArray.ValidateEx())
                    continue;

                Player newPlayer = new(baseAddress, pawn, boneArray, teamID, _updateIteration);

                Players.AddOrUpdate(baseAddress, (newItem) => newPlayer, (key, existing) =>
                {
                    newPlayer.Health = existing.Health;
                    newPlayer.IsAlive = existing.IsAlive;
                    newPlayer.Position = existing.Position;
                    newPlayer.BonePositions = existing.BonePositions;
                    newPlayer.IsVisible = existing.IsVisible;

                    return newPlayer;
                });

                if (newPlayer.IsLocal)
                {
                    LocalPlayerControllerIndex = i;
                    LocalPlayer = newPlayer;
                }
            }

            // Purge all old players
            try
            {
                foreach (var player in Players)
                    if (player.Value.UpdateIteration != _updateIteration)
                        Players.TryRemove(player);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME] -> Purge Old Players Exception: {ex}");
            }

            IsInMatch = Players.Count > 1;

            Logger.WriteLine($"Player Count: {Players.Count} | In Match: {IsInMatch}");
        }

        private static void Realtime()
        {
            while (true)
            {
                if (!IsInMatch)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var players = Players.ToArray();
                    int playersLength = players.Length;

                    PubgScatterMap scatterMap = new(playersLength);
                    var scatterRound = scatterMap.AddRound(false);

                    int boneCount = Player.BonesCount;
                    uint boneSize = Player.BoneSize;

                    // Add Players
                    for (int i = 0; i < playersLength; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        int currentScatterId = 0;

                        // Health
                        scatterRound.AddEntry<int>(i, currentScatterId++, player.Pawn + SDK.Client.C_BaseEntity.m_iHealth);

                        // Alive Status
                        scatterRound.AddEntry<bool>(i, currentScatterId++, player.BaseAddress + SDK.Client.CCSPlayerController.m_bPawnIsAlive);

                        // View Angle
                        scatterRound.AddEntry<Vector2>(i, currentScatterId++, player.Pawn + SDK.Client.C_CSPlayerPawnBase.m_angEyeAngles);

                        // Vis Check
                        scatterRound.AddEntry<ulong>(i, currentScatterId++, player.Pawn + SDK.Client.C_CSPlayerPawnBase.m_entitySpottedState + SDK.Client.EntitySpottedState_t.m_bSpottedByMask);

                        // Bones
                        for (uint ii = 0; ii < boneCount; ii++)
                            scatterRound.AddEntry<Player.BoneJointData>(i, currentScatterId++, player.BoneArray + (boneSize * ii));
                    }

                    scatterMap.Execute();

                    // Update Players
                    for (int i = 0; i < playersLength; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        int currentScatterId = 0;

                        // Health
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var health))
                            player.Health = health;

                        // Alive Status
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<bool>(out var aliveStatus))
                            player.IsAlive = aliveStatus;

                        // View Angle
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<Vector2>(out var viewAngle))
                            player.ViewAngle = viewAngle;

                        // Vis Check
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<ulong>(out var mask))
                            player.IsVisible = (mask & ((ulong)1 << LocalPlayerControllerIndex)) != 0;

                        // Bones
                        for (int ii = 0; ii < boneCount; ii++)
                            if (scatterMap.Results[i][currentScatterId++].TryGetResult<Player.BoneJointData>(out var jointData))
                                player.BonePositions[ii] = jointData.Position;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MANAGER] -> Realtime() Exception: {ex}");
                }

                // Update @ approx. 60 FPS
                Thread.Sleep(16);
            }
        }
    }
}
