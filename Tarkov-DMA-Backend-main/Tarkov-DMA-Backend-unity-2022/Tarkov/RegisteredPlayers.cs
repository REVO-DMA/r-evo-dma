using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Collections;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;

namespace Tarkov_DMA_Backend.Tarkov
{
    public sealed class RegisteredPlayers
    {
        public ulong Base { get; }
        private readonly ConcurrentDictionary<ulong, Player> _players = new();
        private readonly ConcurrentDictionary<ulong, QueuedPlayer> _allocationQueue = new();
        private Player _localPlayer = null;
        private Player[] _displayPlayers = null;
        private Player[] _activeAlivePlayers = null;
        private ushort _playersCount = 0;
        private Stopwatch _validatePlayersStopwatch = new();
        private Stopwatch _deepValidatePlayersStopwatch = new();

        private class QueuedPlayer
        {
            public Stopwatch WaitTimer = Stopwatch.StartNew();

            public QueuedPlayer() { }
        }

        #region Getters
        /// <summary>
        /// Contains all players in Local Game World.
        /// </summary>
        public IReadOnlyDictionary<ulong, Player> Players
        {
            get => _players;
        }
        /// <summary>
        /// The LocalPlayer.
        /// </summary>
        public Player LocalPlayer
        {
            get => _localPlayer;
        }
        /// <summary>
        /// Non-local players that are displayed on the radar UI and ESP.
        /// </summary>
        public Player[] DisplayPlayers
        {
            get => _displayPlayers;
        }

        public Player[] ActiveAlivePlayers
        {
            get => _activeAlivePlayers;
        }

        public string[] Teams = null;

        public int PlayerCount
        {
            get
            {
                for (int i = 0; i < 3; i++) // Re-attempt if read fails
                {
                    try
                    {
                        var count = Memory.ReadValue<int>(Base + UnityOffsets.UnityList.Count, false);
                        if (count < 1 || count > 128) 
                            throw new ValueOutOfRangeException(nameof(count));
                        return count;
                    }
                    catch { Thread.Sleep(500); } // short delay between read attempts
                }
                throw new RaidEnded(); // Can't get valid player count? Raid must have ended.
            }
        }

        #endregion

        /// <summary>
        /// RegisteredPlayers List Constructor.
        /// </summary>
        public RegisteredPlayers(ulong baseAddr)
        {
            Base = baseAddr;

            _validatePlayersStopwatch.Start();
            _deepValidatePlayersStopwatch.Start();
        }

        #region UpdateList

        /// <summary>
        /// Updates the ConcurrentDictionary of 'Players'
        /// </summary>
        public void UpdateList()
        {
            try
            {
                var count = PlayerCount; // cache count
                var registered = new HashSet<ulong>();
                var playersList = new MemList<ulong>(Base, false);
                int i = -1;
                foreach (var playerBase in playersList.Items)
                {
                    i++;

                    try
                    {
                        registered.Add(playerBase); // Register Address
                        if (_players.TryGetValue(playerBase, out var existingPlayer))
                        {
                            existingPlayer.ListIndex = i;
                            existingPlayer.SetAlive();
                        }
                        else
                        {
                            if (_allocationQueue.TryGetValue(playerBase, out var queuedPlayer))
                                if (queuedPlayer.WaitTimer.ElapsedMilliseconds < 1000)
                                    continue;

                            // If the localPlayer has not been allocated and this is not the localPlayer, continue.
                            if (LocalPlayer == null && !Player.IsLocalPlayerBase(playerBase))
                                continue;

                            _playersCount++;
                            Player player = new(playerBase, _playersCount)
                            {
                                ListIndex = i
                            };

                            if (_allocationQueue.TryGetValue(playerBase, out var innerQueuedPlayer))
                                innerQueuedPlayer.WaitTimer.Restart();
                            else
                                _allocationQueue.TryAdd(playerBase, new());

                            if (_players.TryAdd(playerBase, player))
                            {
                                Logger.WriteLine($"[REG-PLAYERS] Allocated player: \"{player.Name}\"");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_allocationQueue.TryGetValue(playerBase, out var queuedPlayer))
                            queuedPlayer.WaitTimer.Restart();
                        else
                            _allocationQueue.TryAdd(playerBase, new());

                        Logger.WriteLine($"[REG-PLAYERS] ERROR processing player at: 0x{playerBase:X}: {ex}");
                    }
                }

                var inactivePlayers = _players.Where(x => !registered.Contains(x.Key) && x.Value.IsActive);
                foreach (var player in inactivePlayers)
                {
                    player.Value.LastUpdate = true;
                }

                // Update LocalPlayer
                _localPlayer = _players?.FirstOrDefault(x => x.Value.Type == PlayerType.LocalPlayer).Value;

                // Update display players (non-local players visible on radar UI and ESP)
                _displayPlayers = _players?.Select(x => x.Value)?.Where(x => x.IsNotLocalPlayerActive)?.ToArray();

                // All active & alive players
                _activeAlivePlayers = _players?.Select(x => x.Value)?.Where(x => x.IsActive && x.IsAlive)?.ToArray();

                // Update teams
                Player[] Players = DisplayPlayers;
                if (_displayPlayers == null) Teams = null;
                else
                {
                    // Get all unique groups and alphabetically sort them
                    List<string> groups = new();
                    foreach (var player in _displayPlayers)
                    {
                        if (player == null || !player.FullyAllocated || player.GroupID == null)
                            continue;

                        // Only allow one group entry per group
                        if (!groups.Contains(player.GroupID))
                            groups.Add(player.GroupID);
                    }
                    groups.Sort();
                    Teams = groups.ToArray();

                    // Update player team strings
                    foreach (var player in _displayPlayers)
                    {
                        if (player == null)
                            continue;

                        if (!player.FullyAllocated || player.GroupID == null || !player.IsHuman)
                        {
                            player.TeamNumber = -1;
                            continue;
                        }

                        int groupIndex = Array.IndexOf(Teams, player.GroupID) + 1; // Zero-based
                        player.TeamNumber = groupIndex;
                    }
                }

                // Purge players from the allocation queue
                foreach (var player in _allocationQueue)
                {
                    if (player.Value.WaitTimer.ElapsedMilliseconds > 6000)
                        _allocationQueue.TryRemove(player.Key, out _);
                }
            }
            catch (RaidEnded) { throw; }
            catch (Exception ex)
            {
                Logger.WriteLine($"[REG-PLAYERS] Loop error: {ex}");
            }
        }

        private void ReallocatePlayer(ulong playerBase)
        {
            // If this player is still in memory it will be reallocated
            try
            {
                _players.TryGetValue(playerBase, out var player);
                player.SetDestroy();

                _players.TryRemove(playerBase, out _);

                Logger.WriteLine($"[REG-PLAYERS] Player at: 0x{playerBase:X} has been marked for reallocation.");
            }
            catch (Exception ex)
            {
                throw new Exception($"[REG-PLAYERS] Error while marking player at: 0x{playerBase:X} for reallocation.", ex);
            }
        }
        #endregion

        #region UpdatePlayers
        /// <summary>
        /// Update all player position and rotation values.
        /// </summary>
        public void UpdateAllPlayersRealtime()
        {
            try
            {
                var players = EFTDMA.ActiveAlivePlayers;
                var localPlayer = EFTDMA.LocalPlayer;
                if (players == null) return;
                if (localPlayer == null) return;
                if (players.Length == 0) return;

                var lastUpdate = new HashSet<int>();
                bool[] onlyRootPosition = new bool[players.Length];
                var scatterMap = new EFTScatterMap(players.Length);
                var round1 = scatterMap.AddRound(false);
                
                bool espEnabled = ESP_Config.Enabled;
                bool aimbotEnabled = Aimbot.Enabled;
                float playerDrawDistance = ESP_Config.PlayerDrawDistance;
                float aimbotMaxDistance = Aimbot.MaxDistance;
                int BonesCount = Player.BonesCount;

                // Manually get up-to-date W2S information if ESP is disabled
                if (!espEnabled) ViewportManager.Update();

                for (int i = 0; i < players.Length; i++)
                {
                    Player player = players[i];

                    if (player == null || !player.FullyAllocated) continue;
                    
                    int currentScatterId = 0;

                    if (player.LastUpdate) // player may be dead/exfil'd
                    {
                        lastUpdate.Add(i);
                        round1.AddEntry<ulong>(i, currentScatterId++, player.CorpsePtr);
                    }
                    else
                    {
                        // Position

                        if (player.Type != PlayerType.LocalPlayer)
                        {
                            // Get all bone positions
                            for (int ii = 0; ii < BonesCount; ii++)
                            {
                                UnityTransform boneTransform = player.Bones[ii];
                                round1.AddEntry<Vector128<float>[]>(i, currentScatterId++, boneTransform.VerticesAddr, (3 * boneTransform.HierarchyIndex + 3) * 16);
                            }
                        }
                        else
                        {
                            onlyRootPosition[i] = true;
                            UnityTransform boneTransform = player.Bones[0];
                            round1.AddEntry<Vector128<float>[]>(i, currentScatterId++, boneTransform.VerticesAddr, (3 * boneTransform.HierarchyIndex + 3) * 16);
                        }

                        // Rotation
                        round1.AddEntry<Vector2>(i, currentScatterId++, player.LookDirectionAddress); // x = yaw, y = pitch
                    }
                }

                scatterMap.Execute(); // Execute scatter read

                bool[][] visCheckInfo = VisibilityCheck.GetVisibilityStateBuffer();
                int visCheckInfoMax = visCheckInfo.Length - 1;

                for (int i = 0; i < players.Length; i++)
                {
                    Player player = players[i];

                    if (player == null || !player.FullyAllocated) continue;

                    int currentScatterId = 0;

                    if (lastUpdate.Contains(i)) // player may be dead/exfil'd
                    {
                        bool dead = false;

                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<ulong>(out var corpse))
                        {
                            if (corpse != 0x0)
                            {
                                player.SetDead(corpse);
                                dead = true;
                            }
                        }

                        player.IsActive = false; // mark inactive
                        player.LastUpdate = false; // Last update processed, clear flag

                        if (!dead)
                        {
                            player.SetExfil();
                            ReallocatePlayer(player.Base);
                        }
                    }
                    else
                    {
                        if (onlyRootPosition[i])
                        {
                            if (scatterMap.Results[i][currentScatterId++].TryGetResult<Vector128<float>[]>(out var vertices))
                                if (!player.UpdateBonePositionSingle(vertices, 0)) ReallocatePlayer(player.Base);
                        }
                        else
                        {
                            Vector128<float>[][] vertsArray = new Vector128<float>[BonesCount][];

                            for (int ii = 0; ii < BonesCount; ii++)
                            {
                                if (scatterMap.Results[i][currentScatterId++].TryGetResult<Vector128<float>[]>(out var vertices))
                                    vertsArray[ii] = vertices;
                            }

                            if (!player.UpdateBonePositionMany(vertsArray)) ReallocatePlayer(player.Base);
                        }

                        // Rotation
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<Vector2>(out var rotation))
                            player.SetRotation(rotation);

                        // Distance
                        player.Distance = Vector3.Distance(localPlayer.Position, player.Position);

                        // Visibility
                        if (player.ListIndex <= visCheckInfoMax) // ensure no oob access occurs
                            player.VisibilityInfo = visCheckInfo[player.ListIndex];
                        else
                            player.VisibilityInfo = new bool[VC_Structs.BONE_COUNT];

                        // See if this player is visible at all
                        bool isAnythingVisible = false;

                        if (player.IsAI) // Only head visibility is populated on AI
                            isAnythingVisible = player.VisibilityInfo[0];
                        else
                        {
                            for (int ii = 0; ii < player.VisibilityInfo.Length; ii++)
                            {
                                bool isVisible = player.VisibilityInfo[ii];

                                if (isVisible)
                                {
                                    isAnythingVisible = true;
                                    break;
                                }
                            }
                        }

                        player.IsVisible = isAnythingVisible;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR - UpdatePlayers Loop FAILED: {ex}");
            }
        }

        /// <summary>
        /// Update all deferred player information.
        /// </summary>
        public void UpdateAllPlayersDeferred()
        {
            var players = EFTDMA.ActiveAlivePlayers;
            if (players == null) return;
            if (players.Length == 0) return;

            // Validate players
            if (_validatePlayersStopwatch.ElapsedMilliseconds >= 3000)
            {
                ValidatePlayerTransforms();
                _validatePlayersStopwatch.Restart();
            }

            // Deep Validate Players
            if (_deepValidatePlayersStopwatch.ElapsedMilliseconds >= 6000)
            {
                DeepValidatePlayers();
                _deepValidatePlayersStopwatch.Restart();
            }

            // Update player health
            try
            {
                int playerCount = players.Length;

                EFTScatterMap map = new(playerCount);
                var round1 = map.AddRound();

                bool[] skipIndex = new bool[playerCount];

                for (int i = 0; i < playerCount; i++)
                {
                    Player player = players[i];
                    if (player == null || !player.FullyAllocated || player.IsClientPlayer)
                    {
                        skipIndex[i] = true;
                        continue;
                    }

                    round1.AddEntry<int>(i, 0, player.ObservedHealthController + Offsets.ObservedHealthController.HealthStatus);
                }

                map.Execute();

                for (int i = 0; i < playerCount; i++)
                {
                    Player player = players[i];
                    if (skipIndex[i])
                        continue;

                    if (map.Results[i][0].TryGetResult<int>(out var tag))
                        player.UpdateHealth((Enums.ETagStatus)tag);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[UpdateAllPlayersDeferred] -> UpdateHealth Exception ~ {ex}");
            }
        }

        private void ValidatePlayerTransforms()
        {
            try
            {
                var localPlayer = EFTDMA.LocalPlayer;
                var players = EFTDMA.ActiveAlivePlayers;
                if (localPlayer == null) return;
                if (players == null) return;
                if (players.Length == 0) return;

                EFTScatterMap scatterMap_Normal = new(players.Length);
                var scatter_normal = scatterMap_Normal.AddRound();

                EFTScatterMap scatterMap_Deep = new(players.Length);
                var scatter_deep = scatterMap_Deep.AddRound();

                UnityTransform[][] bones = new UnityTransform[players.Length][];

                bool[] skipPlayer = new bool[players.Length];

                for (int i = 0; i < players.Length; i++)
                {
                    Player player = players[i];

                    if (player == null || !player.FullyAllocated) continue;

                    try
                    {
                        // Only validate distance if this is not the Local Player
                        if (player.Type != PlayerType.LocalPlayer)
                        {
                            float playerDistance = Vector3.Distance(localPlayer.Position, player.Position);
                            if (!float.IsNormal(playerDistance) || float.IsNaN(playerDistance))
                            {
                                Logger.WriteLine($"Player \"{player.Name}\" has an invalid distance. They are being marked for reallocation.");
                                skipPlayer[i] = true;
                                continue;
                            }
                        }

                        bones[i] = new UnityTransform[Player.BonesCount];

                        ulong transformsAddress = Memory.ReadPtrChain(player.Body, UnityOffsets.Transform.GetTransformChainLeadup(), false);
                        for (int ii = 0; ii < Player.BonesCount; ii++)
                        {
                            // Get all player bone positions just like in UpdateAllPlayersRealtime()
                            UnityTransform boneTransformNormal = player.Bones[ii];
                            scatter_normal.AddEntry<Vector128<float>[]>(i, ii, boneTransformNormal.VerticesAddr, (3 * boneTransformNormal.HierarchyIndex + 3) * 16);

                            // Get all info as fresh as possible in case of player allocation issues/addr shifts
                            ulong bonePtr = Memory.ReadPtrChain(transformsAddress, UnityOffsets.Transform.GetTransformChainPartial(Player.BoneIndices[ii]), false);

                            if (ii == 0)
                                bones[i][ii] = new UnityTransform(bonePtr, UnityTransform.TransformType.PlayerRootPos);
                            else
                                bones[i][ii] = new UnityTransform(bonePtr);

                            UnityTransform boneTransformDeep = bones[i][ii];
                            scatter_deep.AddEntry<Vector128<float>[]>(i, ii, boneTransformDeep.VerticesAddr, (3 * boneTransformDeep.HierarchyIndex + 3) * 16);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[VALIDATE PLAYER TRANSFORMS] Player: \"{player.Name}\" is invalid. They are being marked for reallocation: {ex}");
                        skipPlayer[i] = true;
                    }
                }

                scatterMap_Normal.Execute();
                scatterMap_Deep.Execute();

                // Validate all player bone positions
                for (int i = 0; i < players.Length; i++)
                {
                    Player player = players[i];

                    if (player == null || !player.FullyAllocated) continue;

                    // If the last loop marked this player for reallocation, do it now and skip further processing
                    if (skipPlayer[i])
                    {
                        ReallocatePlayer(player.Base);
                        continue;
                    }

                    // Update all player bone positions just like in UpdateAllPlayersRealtime()
                    Vector128<float>[][] vertsArray = new Vector128<float>[Player.BonesCount][];

                    for (int ii = 0; ii < Player.BonesCount; ii++)
                    {
                        if (scatterMap_Normal.Results[i][ii].TryGetResult<Vector128<float>[]>(out var vertices))
                            vertsArray[ii] = vertices;
                    }

                    player.UpdateBonePositionMany(vertsArray);

                    bool wasReallocated = false;

                    // Make sure deep bone position matches normal
                    Vector3[] DeepBonePositions = new Vector3[Player.BonesCount];
                    for (int ii = 0; ii < Player.BonesCount; ii++)
                    {
                        if (scatterMap_Deep.Results[i][ii].TryGetResult<Vector128<float>[]>(out var vertices))
                        {
                            Vector3 newPosition = bones[i][ii].GetPosition(vertices);

                            // If actual position is > 5m away from the assumed position, reallocate
                            if (Vector3.Distance(player.BonePositions[ii], newPosition) > 5f)
                            {
                                Logger.WriteLine($"[VALIDATE PLAYER TRANSFORMS] Player: \"{player.Name}\" has an invalid position. They are being reallocated.");
                                ReallocatePlayer(player.Base);
                                wasReallocated = true;
                                break;
                            }

                            DeepBonePositions[ii] = newPosition;
                        }
                    }

                    // Stop validations if this player has already been reallocated
                    if (wasReallocated) continue;

                    // Make sure all bones are within a reasonable distance of one another
                    const float maxDistance = 5f;
                    for (int ii = 0; ii < Player.BonesCount; ii++)
                    {
                        bool bonesOK = true;

                        for (int iii = 0; iii < Player.BonesCount; iii++)
                        {
                            float distance = Vector3.Distance(DeepBonePositions[ii], player.BonePositions[iii]);

                            if (distance > maxDistance)
                            {
                                bonesOK = false;
                                break;
                            }
                        }

                        if (!bonesOK)
                        {
                            Logger.WriteLine($"[VALIDATE PLAYER TRANSFORMS] Player: \"{player.Name}\" has bones that are too far apart. The player is being reallocated.");
                            ReallocatePlayer(player.Base);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[VALIDATE PLAYER TRANSFORMS] CRITICAL ERROR: ValidatePlayerTransforms failed: {ex}");
            }
        }

        private class DeepValidateException : Exception
        {
            public DeepValidateException() { }

            public DeepValidateException(string message) : base(message) { }

            public DeepValidateException(string message, Exception inner) : base(message, inner) { }
        }

        private void DeepValidatePlayers()
        {
            try
            {
                var players = EFTDMA.ActiveAlivePlayers;
                if (players == null) return;
                if (players.Length == 0) return;

                var reallocatePlayer = (ulong playerBase, string name, string invalidData, bool doThrow) =>
                {
                    Logger.WriteLine($"[DEEP VALIDATE PLAYERS] Player \"{name}\" @ \"0x{playerBase:X}\" has invalid \"{invalidData}\" data. They are being reallocated.");
                    ReallocatePlayer(playerBase);

                    if (doThrow)
                        throw new DeepValidateException(invalidData);
                };

                for (int i = 0; i < players.Length; i++)
                {
                    Player player = players[i];

                    if (player == null || !player.FullyAllocated || player.DeepValidationPassedCount >= 3) continue;

                    try
                    {
                        // Check if this player is really dead if they are marked as inactive
                        // NOTE: Exfil'd players are removed from reg players list
                        if (player.IsActive == false)
                        {
                            ulong corpsePtr = Memory.ReadPtr(player.CorpsePtr, false);

                            if (player.Corpse == null || player.Corpse != corpsePtr)
                            {
                                reallocatePlayer(player.Base, player.Name, "IsReallyDead", true);
                            }
                        }

                        bool isClientPlayer = Player.GetIsClientPlayer(player.Base);
                        if (isClientPlayer != player.IsClientPlayer)
                            reallocatePlayer(player.Base, player.Name, "IsClientPlayer", true);

                        // Validate base addresses
                        if (isClientPlayer)
                        {
                            var ClientPlayerAddresses = Player.GetClientPlayerAddresses(player.Base);

                            if (player.CharacterController != ClientPlayerAddresses.CharacterController)
                                reallocatePlayer(player.Base, player.Name, "CharacterController", true);
                            if (player.MovementContext != ClientPlayerAddresses.MovementContext)
                                reallocatePlayer(player.Base, player.Name, "MovementContext", true);
                            if (player.LookDirectionAddress != ClientPlayerAddresses.LookDirectionAddress)
                                reallocatePlayer(player.Base, player.Name, "LookDirectionAddress", true);
                            if (player.Body != ClientPlayerAddresses.Body)
                                reallocatePlayer(player.Base, player.Name, "Body", true);
                            if (player.PWA != ClientPlayerAddresses.PWA)
                                reallocatePlayer(player.Base, player.Name, "PWA", true);
                            if (player.Profile != ClientPlayerAddresses.Profile)
                                reallocatePlayer(player.Base, player.Name, "Profile", true);
                            if (player.Info != ClientPlayerAddresses.Info)
                                reallocatePlayer(player.Base, player.Name, "Info", true);
                            if (player.Settings != ClientPlayerAddresses.Settings)
                                reallocatePlayer(player.Base, player.Name, "Settings", true);
                            if (player.IsLocalPlayer != ClientPlayerAddresses.IsLocalPlayer)
                                reallocatePlayer(player.Base, player.Name, "IsLocalPlayer", true);
                            if (player.IsAI != ClientPlayerAddresses.IsAI)
                                reallocatePlayer(player.Base, player.Name, "IsAI", true);
                        }
                        else
                        {
                            var ObservedPlayerAddresses = Player.GetObservedPlayerAddresses(player.Base);

                            if (player.Body != ObservedPlayerAddresses.Body)
                                reallocatePlayer(player.Base, player.Name, "Body", true);
                            if (player.ObservedPlayerController != ObservedPlayerAddresses.ObservedPlayerController)
                                reallocatePlayer(player.Base, player.Name, "ObservedPlayerController", true);
                            if (player.ObservedMovementController != ObservedPlayerAddresses.ObservedMovementController)
                                reallocatePlayer(player.Base, player.Name, "ObservedMovementController", true);
                            if (player.LookDirectionAddress != ObservedPlayerAddresses.LookDirectionAddress)
                                reallocatePlayer(player.Base, player.Name, "LookDirectionAddress", true);
                            if (player.ObservedHealthController != ObservedPlayerAddresses.ObservedHealthController)
                                reallocatePlayer(player.Base, player.Name, "ObservedHealthController", true);
                            if (player.IsLocalPlayer != ObservedPlayerAddresses.IsLocalPlayer)
                                reallocatePlayer(player.Base, player.Name, "IsLocalPlayer", true);
                            if (player.IsAI != ObservedPlayerAddresses.IsAI)
                                reallocatePlayer(player.Base, player.Name, "IsAI", true);
                        }

                        // Validate Faction
                        Enums.EPlayerSide ePlayerSide = Player.GetPlayerFaction(player.Base, player.IsClientPlayer);
                        var faction = Player.GetFaction(ePlayerSide, player.IsAI);
                        if (faction == null)
                            reallocatePlayer(player.Base, player.Name, "InvalidFaction", true);
                        if (player.Faction != faction)
                            reallocatePlayer(player.Base, player.Name, "Faction", true);

                        // Validate IsPMC
                        var isPMC = Player.GetIsPMC(ePlayerSide);
                        if (player.IsPMC != isPMC)
                            reallocatePlayer(player.Base, player.Name, "IsPMC", true);

                        // Validate Name
                        var name = Player.GetName(player.Base, player.IsClientPlayer, player.IsLocalPlayer, player.IsPMC, player.IsAI);
                        if (ePlayerSide == Enums.EPlayerSide.Savage && player.IsAI) // AI
                        {
                            Enums.WildSpawnType wildSpawnType;

                            if (player.IsClientPlayer)
                            {
                                wildSpawnType = (Enums.WildSpawnType)Memory.ReadValue<int>(player.Settings + Offsets.PlayerInfoSettings.Role);
                            }
                            else
                            {
                                // Refer to -> FixWildSpawnType.cs
                                ulong infoContainer = Memory.ReadPtr(player.ObservedPlayerController + Offsets.ObservedPlayerController.InfoContainer);
                                wildSpawnType = (Enums.WildSpawnType)Memory.ReadValue<int>(infoContainer + Offsets.InfoContainer.Side);
                            }

                            AIRole role = AIRoleMisc.GetRole(name, wildSpawnType);

                            if (player.Name != role.Name)
                                reallocatePlayer(player.Base, player.Name, "AI_Name", true);
                            if (player.Type != role.Type)
                                reallocatePlayer(player.Base, player.Name, "AI_Type", true);
                        }
                        else // Human
                            if (ePlayerSide != Enums.EPlayerSide.Savage && player.Name != name) // Skip P SCAVS
                                reallocatePlayer(player.Base, player.Name, "Human_Name", true);
                        
                        // Only perform these validations if this is a human player
                        if (ePlayerSide != Enums.EPlayerSide.Savage || (ePlayerSide == Enums.EPlayerSide.Savage && !player.IsAI))
                        {
                            // Validate Group
                            var groupID = Player.GetGroupID(player.Base, player.IsClientPlayer);
                            if (player.GroupID != groupID)
                                reallocatePlayer(player.Base, player.Name, "GroupID", true);

                            // Validate Account ID
                            var accountID = Player.GetAccountID(player.Base, player.IsClientPlayer);
                            if (player.AccountID != accountID)
                                reallocatePlayer(player.Base, player.Name, "AccountID", true);
                        }

                        player.DeepValidationPassedCount++;
                    }
                    catch (DeepValidateException ex) { } // Ignore this exception
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[DEEP VALIDATE PLAYERS] Player: \"{player.Name}\" is invalid. They are being reallocated: {ex}");
                        reallocatePlayer(player.Base, player.Name, ex.Message, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DEEP VALIDATE PLAYERS] CRITICAL ERROR: ValidatePlayerTransforms Loop FAILED: {ex}");
            }
        }

        /// <summary>
        /// Refresh Gear Manager
        /// </summary>
        public void RefreshGear()
        {
            try
            {
                var players = _players.Select(x => x.Value).Where(x => x.IsActive);

                if (players == null || !players.Any())
                    return;

                foreach (var player in players)
                {
                    if (player == null || !player.IsLocalPlayer) continue;

                    player.RefreshGear();
                }
            }
            catch { }
        }

        /// <summary>
        /// Refresh Hands Manager
        /// </summary>
        public void RefreshHands()
        {
            try
            {
                var players = EFTDMA.ActiveAlivePlayers;
                if (players == null) return;
                if (players.Length == 0) return;

                foreach (var player in players)
                {
                    if (player == null || !player.FullyAllocated)
                        continue;

                    player.RefreshHands();
                }
            }
            catch { }
        }
        #endregion
    }
}
