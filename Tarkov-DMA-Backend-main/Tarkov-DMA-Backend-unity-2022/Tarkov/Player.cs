using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.GameAPI;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov
{
    /// <summary>
    /// Class containing Game Player Data.
    /// </summary>
    public sealed class Player
    {
        public static implicit operator ulong(Player x) => x.Base;
        private static string _localPlayerGroup = null;

        #region Bone Stuff

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHeadIndex(int index)
        {
            if (index == (int)Bone.HumanHead ||
                index == (int)Bone.HumanNeck)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCenterMassIndex(int index)
        {
            if (index == (int)Bone.HumanSpine1 ||
                index == (int)Bone.HumanSpine2 ||
                index == (int)Bone.HumanSpine3 ||
                index == (int)Bone.HumanPelvis)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeftArmIndex(int index)
        {
            if (index == (int)Bone.HumanLUpperarm ||
                index == (int)Bone.HumanLForearm1 ||
                index == (int)Bone.HumanLForearm3)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRightArmIndex(int index)
        {
            if (index == (int)Bone.HumanRUpperarm ||
                index == (int)Bone.HumanRForearm1 ||
                index == (int)Bone.HumanRForearm3)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeftLegIndex(int index)
        {
            if (index == (int)Bone.HumanLThigh1 ||
                index == (int)Bone.HumanLCalf ||
                index == (int)Bone.HumanLFoot)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRightLegIndex(int index)
        {
            if (index == (int)Bone.HumanRThigh1 ||
                index == (int)Bone.HumanRCalf ||
                index == (int)Bone.HumanRFoot)
                return true;

            return false;
        }

        public static int BoneIndexToVisCheckIndex(int boneIndex)
        {
            if (IsHeadIndex(boneIndex))
                return 0;
            else if (IsCenterMassIndex(boneIndex))
                return 1;
            else if (IsLeftArmIndex(boneIndex))
                return 2;
            else if (IsRightArmIndex(boneIndex))
                return 3;
            else if (IsLeftLegIndex(boneIndex))
                return 4;
            else if (IsRightLegIndex(boneIndex))
                return 5;
            else
                return -1;
        }

        private enum BoneSide
        {
            Left,
            Right
        }

        public static Bone BoneNameToBone(string bone, string side)
        {
            BoneSide boneSide = BoneSide.Left;
            if (side == "right")
                boneSide = BoneSide.Right;

            switch (bone)
            {
                // Head
                case "Head":
                    return Bone.HumanHead;
                case "Neck":
                    return Bone.HumanNeck;
                // Center Mass
                case "Thorax":
                    return Bone.HumanSpine3;
                case "Stomach":
                    return Bone.HumanSpine1;
                // Arms
                case "Shoulder":
                    if (boneSide == BoneSide.Left)
                        return Bone.HumanLUpperarm;
                    else
                        return Bone.HumanRUpperarm;
                case "Elbow":
                    if (boneSide == BoneSide.Left)
                        return Bone.HumanLForearm1;
                    else
                        return Bone.HumanRForearm1;
                case "Hand":
                    if (boneSide == BoneSide.Left)
                        return Bone.HumanLForearm3;
                    else
                        return Bone.HumanRForearm3;
                // Legs
                case "Hip":
                    if (boneSide == BoneSide.Left)
                        return Bone.HumanLThigh1;
                    else
                        return Bone.HumanRThigh1;
                case "Knee":
                    if (boneSide == BoneSide.Left)
                        return Bone.HumanLCalf;
                    else
                        return Bone.HumanRCalf;
                case "Ankle":
                    if (boneSide == BoneSide.Left)
                        return Bone.HumanLFoot;
                    else
                        return Bone.HumanRFoot;
                default:
                    return Bone.HumanBase;
            }
        }

        public static readonly int[] BoneIndices = {
            (int)Bone.HumanBase,        // [0]
            (int)Bone.HumanPelvis,      // [1]
            (int)Bone.HumanLThigh1,     // [2]
            (int)Bone.HumanLCalf,       // [3]
            (int)Bone.HumanLFoot,       // [4]
            (int)Bone.HumanRThigh1,     // [5]
            (int)Bone.HumanRCalf,       // [6]
            (int)Bone.HumanRFoot,       // [7]
            (int)Bone.HumanSpine1,      // [8]
            (int)Bone.HumanSpine2,      // [9]
            (int)Bone.HumanSpine3,      // [10]
            (int)Bone.HumanLUpperarm,   // [11]
            (int)Bone.HumanLForearm1,   // [12]
            (int)Bone.HumanLForearm3,   // [13]
            (int)Bone.HumanRUpperarm,   // [14]
            (int)Bone.HumanRForearm1,   // [15]
            (int)Bone.HumanRForearm3,   // [16]
            (int)Bone.HumanNeck,        // [17]
            (int)Bone.HumanHead,        // [18]
        };

        public static readonly int[] BoneLinkIndices = {
            /// Head
            // Head to neck
            18, // HumanHead
            17, // HumanNeck

            /// Chest
            // Neck to upper chest
            17, // HumanNeck
            10, // HumanSpine3
            // Upper chest to center mass
            10, // HumanSpine3
            9, // HumanSpine2

            /// Stomach
            // Center mass to stomach
            9, // HumanSpine2
            8, // HumanSpine1
            // Stomach to pelvis
            8, // HumanSpine1
            1, // HumanPelvis

            /// Right arm
            // Neck to right shoulder
            17, // HumanNeck
            14, // HumanRUpperarm
            // Right shoulder to elbow
            14, // HumanRUpperarm
            15, // HumanRForearm1
            // Right elbow to wrist
            15, // HumanRForearm1
            16, // HumanRForearm3

            /// Left arm
            // Neck to left shoulder
            17, // HumanNeck
            11, // HumanLUpperarm
            // Left shoulder to elbow
            11, // HumanLUpperarm
            12, // HumanLForearm1
            // Left elbow to wrist
            12, // HumanLForearm1
            13, // HumanLForearm3
            
            /// Right leg
            // Pelvis to right hip
            1, // HumanPelvis
            5, // HumanRThigh1
            // Right hip to calf
            5, // HumanRThigh1
            6, // HumanRCalf
            // Right calf to ankle
            6, // HumanRCalf
            7, // HumanRFoot

            /// Left leg
            // Pelvis to left hip
            1, // HumanPelvis
            2, // HumanLThigh1
            // Left hip to calf
            2, // HumanLThigh1
            3, // HumanLCalf
            // Left calf to ankle
            3, // HumanLCalf
            4 // HumanLFoot
        };

        public UnityTransform[] Bones = new UnityTransform[BoneIndices.Length];
        public Vector3[] BonePositions = new Vector3[BoneIndices.Length];
        public Vector3 Position => BonePositions[0];

        #endregion

        private HandsManager _handsManager;
        private GearManager _gearManager;

        #region PlayerProperties
        /// <summary>
        /// The players list index of this player.
        /// </summary>
        public int ListIndex { get; set; }
        /// <summary>
        /// A unique ID for this specific player.
        /// </summary>
        public bool FullyAllocated { get; private set; } = false;
        /// <summary>
        /// The amount of times this player has passed deep validation.
        /// </summary>
        public byte DeepValidationPassedCount { get; set; } = 0;
        /// <summary>
        /// A unique ID for this specific player.
        /// </summary>
        public ushort ID { get; }
        /// <summary>
        /// Player is a PMC Operator.
        /// </summary>
        public bool IsPMC { get; private set; }
        /// <summary>
        /// Player Faction.
        /// </summary>
        public string Faction { get; private set; } = "N/A";
        /// <summary>
        /// The name of the place the player spawned at.
        /// </summary>
        public string InfilPoint { get; private set; }
        /// <summary>
        /// Player is Active (has not exfil'd).
        /// </summary>
        public volatile bool IsActive = true;
        /// <summary>
        /// Player is alive (not dead).
        /// </summary>
        public bool IsAlive
        {
            get => Corpse == null;
        }
        /// <summary>
        /// Account UUID for Human Controlled Players.
        /// </summary>
        public string AccountID { get; private set; } = null;
        public string ProfileID { get; private set; } = null;
        /// <summary>
        /// Player name.
        /// </summary>
        public string Name { get; private set; } = null;
        /// <summary>
        /// Group that the player belongs to.
        /// </summary>
        public string GroupID { get; private set; } = null;
        /// <summary>
        /// Index of the team this player is on. This correlates to the way players are displayed on the players table.
        /// </summary>
        public int TeamNumber { get; set; } = -1;
        /// <summary>
        /// Type of player unit.
        /// </summary>
        public PlayerType Type { get; set; }
        /// <summary>
        /// Player's Rotation (direction/pitch) in Local Game World.
        /// 90 degree offset ~already~ applied to account for 2D-Map orientation.
        /// </summary>
        public Vector2 Rotation { get; private set; } = new(0, 0);
        /// <summary>
        /// Player's distance from the LocalPlayer
        /// </summary>
        public float Distance = 0f;
        /// <summary>
        /// (PMC ONLY) Player's Gear Loadout.
        /// Key = Slot Name, Value = Item 'Long Name' in Slot
        /// </summary>
        public GearManager Gear
        {
            get => _gearManager;
        }
        /// <summary>
        /// Item/weapon in player's hands.
        /// </summary>
        public string ItemInHands
        {
            get => _handsManager?.ItemInHands;
        }
        /// <summary>
        /// If 'true', Player object is no longer in the RegisteredPlayers list.
        /// Will be checked if dead/exfil'd on next loop.
        /// </summary>
        public bool LastUpdate = false;
        /// <summary>
        /// Linecast visibility info.
        /// </summary>
        public bool[] VisibilityInfo { get; set; }
        public bool IsVisible { get; set; } = false;
        /// <summary>
        /// True if Chams are set.
        /// </summary>
        public bool ChamsSet = false;
        /// <summary>
        /// The status of this player. Alive, Dead, Exfil'd, etc...
        /// </summary>
        public byte Status = 1;
        /// <summary>
        /// Whether this player is hostile.
        /// </summary>
        public bool IsHostile { get; private set; } = true;

        public byte HealthPercent { get; private set; } = 100;

        // Player Stats
        public string Level {  get; private set; } = "N/A";
        public string AccountType { get; private set; } = "N/A";
        public string OnlineTime { get; private set; } = "N/A";
        public string KillDeathRatio { get; private set; } = "N/A";
        public string SurvivalRate { get; private set; } = "N/A";
        #endregion

        #region Getters

        public bool IsNotLocalPlayerActive
        {
            get => Type != PlayerType.LocalPlayer && IsActive && IsAlive;
        }
        public bool IsHuman
        {
            get => (Type == PlayerType.LocalPlayer || Type == PlayerType.Teammate || Type == PlayerType.EnemyPMC || Type is PlayerType.PlayerScav);
        }
        public bool IsHumanHostile
        {
            get => (Type == PlayerType.EnemyPMC || Type == PlayerType.PlayerScav);
        }
        public bool IsHumanHostileActive
        {
            get => ((Type is PlayerType.EnemyPMC || Type is PlayerType.PlayerScav) && IsActive && IsAlive);
        }
        public bool HasExfild
        {
            get => !IsActive && IsAlive;
        }
        public ulong Base { get; private init; }
        public ulong Body { get; }
        public ulong Profile { get; }
        public ulong PWA { get; }
        public ulong Info { get; }
        public ulong Settings { get; }
        public ulong MovementContext { get; }
        public ulong LookDirectionAddress { get; set; }
        public ulong VelocityAddress
        {
            get
            {
                if (IsClientPlayer)
                {
                    return CharacterController + Offsets.SimpleCharacterController.velocity;
                }
                else
                {
                    return ObservedMovementController + Offsets.ObservedMovementController.Velocity;
                }
            }
        }
        public bool IsClientPlayer { get; }
        public ulong ObservedPlayerController { get; set; }
        public ulong ObservedMovementController { get; set; }
        public ulong ObservedHealthController { get; set; }
        public ulong CharacterController { get; set; }
        public bool IsLocalPlayer { get; }
        public bool IsAI { get; }
        /// <summary>
        /// Corpse memory address.
        /// </summary>
        public ulong? Corpse { get; private set; } = null;
        public ulong CorpsePtr
        {
            get
            {
                if (IsClientPlayer)
                    return this + Offsets.Player.Corpse;
                else
                    return ObservedHealthController + Offsets.ObservedHealthController.PlayerCorpse;
            }
        }
        public static int BonesCount
        {
            get => BoneIndices.Length;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Player Constructor.
        /// </summary>
        public Player(ulong playerBase, ushort id)
        {
            try
            {
                ID = id;
                
                Base = playerBase;

                bool isClientPlayer = GetIsClientPlayer(Base);
                IsClientPlayer = isClientPlayer;

                // Set base addresses and player info based on class type
                if (IsClientPlayer)
                {
                    var ClientPlayerAddresses = GetClientPlayerAddresses(Base);

                    CharacterController = ClientPlayerAddresses.CharacterController;
                    MovementContext = ClientPlayerAddresses.MovementContext;
                    LookDirectionAddress = ClientPlayerAddresses.LookDirectionAddress;
                    Body = ClientPlayerAddresses.Body;
                    PWA = ClientPlayerAddresses.PWA;
                    Profile = ClientPlayerAddresses.Profile;
                    Info = ClientPlayerAddresses.Info;
                    Settings = ClientPlayerAddresses.Settings;
                    IsLocalPlayer = ClientPlayerAddresses.IsLocalPlayer;
                    IsAI = ClientPlayerAddresses.IsAI;
                }
                else
                {
                    var ObservedPlayerAddresses = GetObservedPlayerAddresses(Base);

                    Body = ObservedPlayerAddresses.Body;
                    ObservedPlayerController = ObservedPlayerAddresses.ObservedPlayerController;
                    ObservedMovementController = ObservedPlayerAddresses.ObservedMovementController;
                    LookDirectionAddress = ObservedPlayerAddresses.LookDirectionAddress;
                    ObservedHealthController = ObservedPlayerAddresses.ObservedHealthController;
                    IsLocalPlayer = ObservedPlayerAddresses.IsLocalPlayer;
                    IsAI = ObservedPlayerAddresses.IsAI;
                }

                // Get transforms
                ulong transformsAddress = Memory.ReadPtrChain(Body, UnityOffsets.Transform.GetTransformChainLeadup(), false);
                for (int i = 0; i < BonesCount; i++)
                {
                    ulong transformInternal = Memory.ReadPtrChain(transformsAddress, UnityOffsets.Transform.GetTransformChainPartial(BoneIndices[i]), false);

                    if (i == 0)
                        Bones[i] = new UnityTransform(transformInternal, UnityTransform.TransformType.PlayerRootPos);
                    else
                        Bones[i] = new UnityTransform(transformInternal, UnityTransform.TransformType.Normal);
                }

                // Get faction
                Enums.EPlayerSide ePlayerSide = GetPlayerFaction(Base, IsClientPlayer);
                string faction = GetFaction(ePlayerSide, IsAI);
                if (faction == null)
                    throw new ValueOutOfRangeException(nameof(faction));
                else
                    Faction = faction;

                // Is this a PMC?
                IsPMC = GetIsPMC(ePlayerSide);

                // Get name
                Name = GetName(Base, IsClientPlayer, IsLocalPlayer, IsPMC, IsAI);

                // Perform final allocations for this player
                if (ePlayerSide == Enums.EPlayerSide.Savage)
                {
                    if (IsAI) // AI
                    {
                        Enums.WildSpawnType wildSpawnType;

                        if (IsClientPlayer)
                        {
                            wildSpawnType = (Enums.WildSpawnType)Memory.ReadValue<int>(Settings + Offsets.PlayerInfoSettings.Role, false);
                        }
                        else
                        {
                            // Refer to -> FixWildSpawnType.cs
                            ulong infoContainer = Memory.ReadPtr(ObservedPlayerController + Offsets.ObservedPlayerController.InfoContainer, false);
                            wildSpawnType = (Enums.WildSpawnType)Memory.ReadValue<int>(infoContainer + Offsets.InfoContainer.Side, false);
                        }

                        AIRole role = AIRoleMisc.GetRole(Name, wildSpawnType);

                        Name = role.Name;
                        Type = role.Type;
                    }
                    else // Player Scav
                        AllocateHuman();
                }
                else
                    AllocateHuman();

                // Is this a teammate?
                if (_localPlayerGroup != null && GroupID != null && Type != PlayerType.LocalPlayer && IsHumanHostile && GroupID == _localPlayerGroup)
                {
                    Type = PlayerType.Teammate;
                    IsHostile = false;
                }

                // Set an initial player position. Non-local players will be "hidden".
                if (IsLocalPlayer)
                    this.BonePositions[0] = Bones[0].GetPosition();
                else
                    this.BonePositions[0] = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                // Allocate on the UI
                IPCAllocate();

                if (IsHuman)
                {
                    // Queue player stats lookup
                    var playerStats = PlayerStatsManager.QueueLookup(AccountID, Base);

                    // If the stats were found in the cache this is not null
                    if (playerStats != null) SetStats((PlayerStatsManager.PlayerStats)playerStats);
                }

                // Mark as fully allocated (prevent race conditions)
                FullyAllocated = true;
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR allocating Player 0x{Base:X}", ex);
            }
        }

        private void AllocateHuman()
        {
            if (IsLocalPlayer)
            {
                // TODO: ensure that if the local player is reallocated all players analyzed to see if they are a teammate
                // If they are in the same group, reallocate them
                string lpGroupID = GetGroupID(Base, IsClientPlayer);
                _localPlayerGroup = lpGroupID;
                GroupID = lpGroupID;

                Type = PlayerType.LocalPlayer;

                if (IsPMC)
                {
                    // Get infil point
                    ulong infilPointStringPtr = Memory.ReadPtr(Info + Offsets.PlayerInfo.EntryPoint, false);
                    InfilPoint = Memory.ReadUnityString(infilPointStringPtr, false);
                }
            }
            else
            {
                if (IsPMC)
                    Type = PlayerType.EnemyPMC;
                else
                    Type = PlayerType.PlayerScav;

                GroupID = GetGroupID(Base, IsClientPlayer);
            }

            AccountID = GetAccountID(Base, IsClientPlayer);

            if (IsLocalPlayer) ProfileID = GetProfileID(Profile);
        }

        private void IPCAllocate()
        {
            IPC_StaticPlayer ipcPlayer = new(ID, AccountID, GroupID, Name, Faction, Type);
            byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcPlayer);
            Server.RadarSend(Constants.MessageTypes.StaticPlayer, serializedData);
        }

        private void IPCStats()
        {
            IPC_PlayerStats ipcStats = new(Name, ID, Level, AccountType, OnlineTime, KillDeathRatio, SurvivalRate);
            byte[] serializedData = MemoryPack.MemoryPackSerializer.Serialize(ipcStats);
            Server.RadarSend(Constants.MessageTypes.PlayerStats, serializedData);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Determine if a given player's base address is the base address of the LocalPlayer.
        /// </summary>
        public static bool IsLocalPlayerBase(ulong playerBase)
        {
            bool isClientPlayer = GetIsClientPlayer(playerBase);

            if (isClientPlayer && Memory.ReadValue<bool>(playerBase + Offsets.Player.IsYourPlayer, false))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determine if a given player's base address is the base address of a ClientPlayer class.
        /// </summary>
        public static bool GetIsClientPlayer(ulong playerBase)
        {
            ulong classNamePtr = Memory.ReadPtrChain(playerBase, UnityOffsets.Component.To_NativeClassName, false);
            string className = Memory.ReadUtf8String(classNamePtr, 64, false);

            if (className == "ClientPlayer" || className == "LocalPlayer")
                return true;
            else
                return false;
        }

        public readonly struct ClientPlayerAddresses
        {
            public readonly ulong CharacterController;
            public readonly ulong MovementContext;
            public readonly ulong LookDirectionAddress;
            public readonly ulong Body;
            public readonly ulong PWA;
            public readonly ulong Profile;
            public readonly ulong Info;
            public readonly ulong Settings;
            public readonly bool IsLocalPlayer;
            public readonly bool IsAI;
            public ClientPlayerAddresses(ulong characterController, ulong movementContext, ulong lookDirectionAddress, ulong body, ulong pwa, ulong profile, ulong info, ulong settings, bool isLocalPlayer, bool isAI)
            {
                CharacterController = characterController;
                MovementContext = movementContext;
                LookDirectionAddress = lookDirectionAddress;
                Body = body;
                PWA = pwa;
                Profile = profile;
                Info = info;
                Settings = settings;
                IsLocalPlayer = isLocalPlayer;
                IsAI = isAI;
            }
        }

        public static ClientPlayerAddresses GetClientPlayerAddresses(ulong playerBase)
        {
            ulong characterController = Memory.ReadPtr(playerBase + Offsets.Player._characterController, false);

            ulong movementContext = Memory.ReadPtr(playerBase + Offsets.Player.MovementContext, false);
            ulong lookDirectionAddress = movementContext + Offsets.MovementContext._rotation;

            ulong body = Memory.ReadPtr(playerBase + Offsets.Player._playerBody, false);
            ulong pwa = Memory.ReadPtr(playerBase + Offsets.Player.ProceduralWeaponAnimation, false);
            ulong profile = Memory.ReadPtr(playerBase + Offsets.Player.Profile, false);
            ulong info = Memory.ReadPtr(profile + Offsets.Profile.Info, false);

            // Determine if this is the LocalPlayer
            bool isLocalPlayer = Memory.ReadValue<bool>(playerBase + Offsets.Player.IsYourPlayer, false);

            // Determine if this is AI
            ulong settings = 0x0;
            bool isAI = false;
            if (!isLocalPlayer)
            {
                isAI = true;
                settings = Memory.ReadPtr(info + Offsets.PlayerInfo.Settings, false);
            }

            return new(characterController, movementContext, lookDirectionAddress, body, pwa, profile, info, settings, isLocalPlayer, isAI);
        }

        public readonly struct ObservedPlayerAddresses
        {
            public readonly ulong Body;
            public readonly ulong ObservedPlayerController;
            public readonly ulong ObservedMovementController;
            public readonly ulong LookDirectionAddress;
            public readonly ulong ObservedHealthController;
            public readonly bool IsLocalPlayer;
            public readonly bool IsAI;
            public ObservedPlayerAddresses(ulong body, ulong observedPlayerController, ulong observedMovementController, ulong lookDirectionAddress, ulong observedHealthController, bool isAI)
            {
                Body = body;
                ObservedPlayerController = observedPlayerController;
                ObservedMovementController = observedMovementController;
                LookDirectionAddress = lookDirectionAddress;
                ObservedHealthController = observedHealthController;
                IsLocalPlayer = false;
                IsAI = isAI;
            }
        }

        public static ObservedPlayerAddresses GetObservedPlayerAddresses(ulong playerBase)
        {
            ulong body = Memory.ReadPtr(playerBase + Offsets.ObservedPlayerView.PlayerBody, false);

            ulong observedPlayerController = Memory.ReadPtr(playerBase + Offsets.ObservedPlayerView.ObservedPlayerController, false);

            // Determine if this is AI
            bool isAI = Memory.ReadValue<bool>(playerBase + Offsets.ObservedPlayerView.IsAI, false);

            ulong observedMovementController = Memory.ReadPtrChain(observedPlayerController, Offsets.ObservedPlayerController.MovementController, false);
            ulong lookDirectionAddress = observedMovementController + Offsets.ObservedMovementController.Rotation;

            ulong observedHealthController = Memory.ReadPtr(observedPlayerController + Offsets.ObservedPlayerController.HealthController, false);

            return new(body, observedPlayerController, observedMovementController, lookDirectionAddress, observedHealthController, isAI);
        }

        /// <summary>
        /// Get Account ID for Human-Controlled Players.
        /// </summary>
        public static string GetAccountID(ulong playerBase, bool isClientPlayer)
        {
            try
            {
                ulong idPtr;
                if (isClientPlayer)
                {
                    ulong profile = Memory.ReadPtr(playerBase + Offsets.Player.Profile, false);
                    idPtr = Memory.ReadPtr(profile + Offsets.Profile.AccountId, false);
                }
                else
                    idPtr = Memory.ReadPtr(playerBase + Offsets.ObservedPlayerView.AccountId, false);

                return Memory.ReadUnityString(idPtr, false);
            }
            catch
            {
                throw new Exception("Error getting account ID!");
            }
        }

        public static string GetProfileID(ulong profile)
        {
            try
            {
                ulong idPtr = Memory.ReadPtr(profile + Offsets.Profile.Id, false);
                return Memory.ReadUnityString(idPtr, false);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets player's Group Number.
        /// </summary>
        public static string GetGroupID(ulong playerBase, bool isClientPlayer)
        {
            try
            {
                ulong groupPtr;
                if (isClientPlayer)
                {
                    ulong profile = Memory.ReadPtr(playerBase + Offsets.Player.Profile, false);
                    ulong info = Memory.ReadPtr(profile + Offsets.Profile.Info, false);
                    groupPtr = Memory.ReadPtr(info + Offsets.PlayerInfo.GroupId, false);
                }
                else
                    groupPtr = Memory.ReadPtr(playerBase + Offsets.ObservedPlayerView.GroupID, false);

                return Memory.ReadUnityString(groupPtr, false);
            }
            catch
            {
                return null;
            }
        }

        public static Enums.EPlayerSide GetPlayerFaction(ulong playerBase, bool isClientPlayer)
        {
            int iPlayerSide;
            if (isClientPlayer)
            {
                ulong profile = Memory.ReadPtr(playerBase + Offsets.Player.Profile, false);
                ulong info = Memory.ReadPtr(profile + Offsets.Profile.Info, false);
                iPlayerSide = Memory.ReadValue<int>(info + Offsets.PlayerInfo.Side, false);
            }
            else
                iPlayerSide = Memory.ReadValue<int>(playerBase + Offsets.ObservedPlayerView.Side, false);

            return (Enums.EPlayerSide)iPlayerSide;
        }

        public static bool GetIsPMC(Enums.EPlayerSide side)
        {
            if (side == Enums.EPlayerSide.Usec)
                return true;
            else if (side == Enums.EPlayerSide.Bear)
                return true;
            else
                return false;
        }

        public static string GetName(ulong playerBase, bool isClientPlayer, bool isLocalPlayer, bool isPMC, bool isAI)
        {
            if (isClientPlayer)
            {
                if (isLocalPlayer)
                    return "Local (You)";
                else
                {
                    ulong profile = Memory.ReadPtr(playerBase + Offsets.Player.Profile, false);
                    ulong info = Memory.ReadPtr(profile + Offsets.Profile.Info, false);
                    ulong namePtr = Memory.ReadPtr(info + Offsets.PlayerInfo.Nickname, false);
                    return Memory.ReadUnityString(namePtr, false);
                }
            }
            else
            {
                var GetName = () =>
                {
                    ulong namePtr = Memory.ReadPtr(playerBase + Offsets.ObservedPlayerView.NickName, false);
                    return Memory.ReadUnityString(namePtr, false);
                };

                if (isPMC)
                    return GetName();
                else
                    if (isAI)
                        return GetName();
                    else
                        return "P SCAV";
            }
        }

        public static string GetFaction(Enums.EPlayerSide side, bool isAI)
        {
            if (side == Enums.EPlayerSide.Usec)
                return "USEC";
            else if (side == Enums.EPlayerSide.Bear)
                return "BEAR";
            else if (side == Enums.EPlayerSide.Savage)
            {
                if (isAI)
                    return "AI";
                else
                    return "P SCAV";
            }
            else
                return null;
        }

        /// <summary>
        /// Resets/Updates 'static' assets in preparation for a new game/raid instance.
        /// </summary>
        public static void Reset()
        {
            Interlocked.Exchange(ref _localPlayerGroup, null);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Set the player stats.
        /// </summary>
        public void SetStats(PlayerStatsManager.PlayerStats playerStats)
        {
            if (!IsLocalPlayer && !string.IsNullOrEmpty(playerStats.Name)) Name = playerStats.Name;
            Level = playerStats.Level;
            AccountType = playerStats.AccountType;
            OnlineTime = playerStats.OnlineTime;
            KillDeathRatio = playerStats.KillDeathRatio;
            SurvivalRate = playerStats.SurvivalRate;

            IPCStats();
        }

        /// <summary>
        /// Update player health.
        /// </summary>
        public void UpdateHealth(Enums.ETagStatus tag)
        {
            if (tag.HasFlag(Enums.ETagStatus.Dying))
                HealthPercent = 25;
            else if (tag.HasFlag(Enums.ETagStatus.BadlyInjured))
                HealthPercent = 50;
            else if (tag.HasFlag(Enums.ETagStatus.Injured))
                HealthPercent = 75;
            else
                HealthPercent = 100;
        }

        /// <summary>
        /// Mark player as exfil'd.
        /// </summary>
        public void SetExfil()
        {
            if (!HasExfild || Status == 3) return;
            Status = 3;

            Server.SendPlayerStatus(ID, Constants.PlayerStatuses.Exfil);
        }

        /// <summary>
        /// Mark player as dead.
        /// </summary>
        /// <param name="corpse">Corpse address.</param>
        public void SetDead(ulong? corpse)
        {
            Corpse = corpse;

            if (Status == 2) return;
            Status = 2;

            Server.SendPlayerStatus(ID, Constants.PlayerStatuses.Die);
        }

        /// <summary>
        /// Mark player as alive.
        /// </summary>
        public void SetAlive()
        {
            Corpse = null;
            IsActive = true;

            if (Status == 1) return;
            Status = 1;

            ChamsSet = false;

            Server.SendPlayerStatus(ID, Constants.PlayerStatuses.Alive);
        }

        /// <summary>
        /// Mark player as destroyed.
        /// </summary>
        public void SetDestroy()
        {
            Server.SendPlayerStatus(ID, Constants.PlayerStatuses.Destroy);
        }

        /// <summary>
        /// Set player rotation (Direction/Pitch)
        /// </summary>
        public void SetRotation(Vector2 rotation)
        {
            Vector2 result;
            rotation.X -= 90 + UICache.MapConfig.PawnRotation; // degs offset
            if (rotation.X < 0) rotation.X += 360f; // handle if neg

            if (rotation.X < 0) result.X = 360f + rotation.X;
            else result.X = rotation.X;

            if (rotation.Y < 0) result.Y = 360f + rotation.Y;
            else result.Y = rotation.Y;

            Rotation = result;
        }

        /// <summary>
        /// Update the position of all player bones.
        /// </summary>
        public bool UpdateBonePositionMany(Vector128<float>[][] verticesArray)
        {
            try
            {
                // Loop through all bones and update their position
                for (int i = 0; i < BonesCount; i++)
                {
                    var vertices = verticesArray[i];

                    BonePositions[i] = Bones[i].GetPosition(vertices);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[PLAYER] UpdateBonePositionMany() -> Error setting Player: \"{Name}\" bone transforms: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Update the position of a specific bone.
        /// </summary>
        public bool UpdateBonePositionSingle(Vector128<float>[] vertices, int index)
        {
            try
            {
                BonePositions[index] = Bones[index].GetPosition(vertices);

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[PLAYER] UpdateBonePositionSingle() -> Error setting Player: \"{Name}\" bone transform: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Refresh Gear if Active Human Player.
        /// </summary>
        public void RefreshGear()
        {
            try
            {
                if (_gearManager == null)
                    _gearManager = new(IsClientPlayer, IsPMC, Base, ObservedPlayerController);

                _gearManager.Refresh();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GearManager] ERROR for Player \"{Name}\": {ex}");
            }
        }

        /// <summary>
        /// Refresh item in player's hands.
        /// </summary>
        public void RefreshHands()
        {
            try
            {
                if (IsNotLocalPlayerActive)
                {
                    if (_handsManager == null)
                        _handsManager = new HandsManager(this);

                    _handsManager.Refresh();
                }
            }
            catch { }
        }
        #endregion

        #region Chams

        private readonly object SetSingleChamsLock = new();

        public void SetAimbotChams(bool restore)
        {
            if (ToolkitManager.FeatureState["chams"] &&
                ToolkitManager.FeatureSettings_bool["chams_AimbotTarget"] &&
                ToolkitManager.FeatureSettings_int["chams_Mode"] == 3 &&
                ChamsManager.ChamsStatus == ChamsManager.ChamsLoadStatus.FullyLoaded)
            {
                ChamsMaterial? tmpMaterial;

                if (restore)
                    tmpMaterial = ChamsManager.GetMaterialForPlayerType(Type);
                else
                    tmpMaterial = ChamsManager.GetMaterialForPlayerType(PlayerType.AimbotLocked);

                if (tmpMaterial is ChamsMaterial material)
                    SetSingleChams(material.InstanceID);
            }
        }

        public static bool SetCorpseChams(ref List<ScatterWriteEntry> writes, ulong playerBody)
        {
            if (ToolkitManager.FeatureState["chams"] &&
                ToolkitManager.FeatureSettings_int["chams_Mode"] == 3 &&
                ChamsManager.ChamsStatus == ChamsManager.ChamsLoadStatus.FullyLoaded)
            {
                var tmpMaterial = ChamsManager.GetMaterialForPlayerType(PlayerType.Corpse);
                ChamsMaterial material;
                if (tmpMaterial != null)
                {
                    material = (ChamsMaterial)tmpMaterial;
                    SetChams(ref writes, material.InstanceID, playerBody);
                }

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Only set chams on this player.
        /// Thread safe.
        /// </summary>
        public void SetSingleChams(int chamsMaterial)
        {
            lock (SetSingleChamsLock)
            {
                List<ScatterWriteEntry> writes = [];
                SetChams(ref writes, chamsMaterial, true);
                if (writes.Count > 0)
                    Memory.WriteScatter(writes);
            }
        }

        public static void SetChams(ref List<ScatterWriteEntry> writes, int chamsMaterial, ulong playerBody)
        {
            try
            {
                ApplyClothingChams(ref writes, chamsMaterial, playerBody);
                ApplyGearChams(ref writes, chamsMaterial, playerBody);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"ERROR setting Chams ~ {ex}");
            }
        }

        /// <summary>
        /// Apply Chams to CurrentPlayer (if not already set).
        /// </summary>
        /// <param name="game">Current Game Instance.</param>
        /// <param name="chamsMaterial">Chams material PPTR to write.</param>
        public void SetChams(ref List<ScatterWriteEntry> writes, int chamsMaterial, bool force = false)
        {
            try
            {
                if (!ChamsSet || force)
                {
                    ApplyClothingChams(ref writes, chamsMaterial, Body);
                    ApplyGearChams(ref writes, chamsMaterial, Body);
                    ChamsSet = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"ERROR setting Chams for Player '{Name}': {ex}");
            }
        }

        private static void ApplyClothingChams(ref List<ScatterWriteEntry> writes, int chamsMaterial, ulong playerBody)
        {
            try
            {
                var pSkinsDict = Memory.ReadPtr(playerBody + Offsets.PlayerBody.BodySkins);
                var skinsDict = new MemDictionary<ulong, ulong>(pSkinsDict);
                if (!skinsDict.Items.Any())
                    throw new ValueOutOfRangeException(nameof(skinsDict));

                foreach (var loddedSkin in skinsDict.Items.Values)
                {
                    if (loddedSkin == 0x0)
                        throw new NullPtrException(nameof(loddedSkin));

                    var pLodsArray = Memory.ReadPtr(loddedSkin + Offsets.LoddedSkin._lods);
                    var lodsArray = new MemArray<ulong>(pLodsArray);
                    if (!lodsArray.Items.Any())
                        throw new ValueOutOfRangeException(nameof(lodsArray));

                    foreach (var abstractSkin in lodsArray.Items)
                    {
                        if (abstractSkin == 0x0)
                            throw new NullPtrException(nameof(abstractSkin));

                        ulong skinnedMeshRenderer;
                        /// Determine if this is a TorsoSkin
                        var asClassNamePtr = Memory.ReadPtrChain(abstractSkin, UnityOffsets.Component.To_NativeClassName);
                        var asClassName = Memory.ReadUtf8String(asClassNamePtr, 64);
                        if (asClassName != "Skin")
                        {
                            var skin = Memory.ReadPtr(abstractSkin + Offsets.TorsoSkin._skin);
                            skinnedMeshRenderer = Memory.ReadPtr(skin + Offsets.Skin._skinnedMeshRenderer);
                        }
                        else // "Skin"
                            skinnedMeshRenderer = Memory.ReadPtr(abstractSkin + Offsets.Skin._skinnedMeshRenderer);

                        // Cached ptr to Renderer
                        var renderer = Memory.ReadPtr(skinnedMeshRenderer + UnityOffsets.SkinnedMeshRenderer.Renderer);
                        WriteChamsMaterial(ref writes, renderer, chamsMaterial);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR applying Clothing Chams", ex);
            }
        }

        private static void ApplyGearChams(ref List<ScatterWriteEntry> writes, int chamsMaterial, ulong playerBody)
        {
            try
            {
                var slotViews = Memory.ReadPtrUnsafe(playerBody + Offsets.PlayerBody.SlotViews);
                if (slotViews == 0x0) return;

                var pSlotViewsDict = Memory.ReadPtrUnsafe(slotViews + Offsets.SlotViewsContainer.Dict);
                if (pSlotViewsDict == 0x0) return;

                var slotViewsDict = new MemDictionary<ulong, ulong>(pSlotViewsDict);
                if (!slotViewsDict.Items.Any()) return;

                foreach (var slot in slotViewsDict.Items.Values)
                {
                    if (slot == 0x0) continue;

                    var pDressesArray = Memory.ReadPtrUnsafe(slot + Offsets.PlayerBodySubclass.Dresses);
                    if (pDressesArray == 0x0) continue;

                    var dressesArray = new MemArray<ulong>(pDressesArray);
                    if (!dressesArray.Items.Any()) continue;

                    foreach (var dress in dressesArray.Items)
                    {
                        if (dress == 0x0) continue;

                        var pRenderersArray = Memory.ReadPtrUnsafe(dress + Offsets.Dress.Renderers);
                        if (pRenderersArray == 0x0) continue;

                        var renderersArray = new MemArray<ulong>(pRenderersArray);
                        if (!renderersArray.Items.Any()) continue;

                        foreach (var renderer in renderersArray.Items)
                        {
                            if (renderer == 0x0) continue;

                            ulong materials = Memory.ReadPtrUnsafe(renderer + 0x10);
                            if (materials == 0x0) continue;

                            WriteChamsMaterial(ref writes, materials, chamsMaterial);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR applying Gear Chams", ex);
            }
        }

        /// <summary>
        /// Write Chams Material to the specified Renderer/Materials.
        /// </summary>
        /// <param name="game">Current Game Instance.</param>
        /// <param name="renderer">Renderer Unity Class.</param>
        /// <param name="chamsMaterial">Chams material PPTR to write.</param>
        private static void WriteChamsMaterial(ref List<ScatterWriteEntry> writes, ulong renderer, int chamsMaterial)
        {
            int materialsCount = Memory.ReadValue<int>(renderer + UnityOffsets.Renderer.Count);

            if (materialsCount > 0 && materialsCount < 30)
            {
                ulong materialsArray = Memory.ReadPtr(renderer + UnityOffsets.Renderer.Materials);
                
                for (uint i = 0; i < materialsCount; i++)
                {
                    ulong material = materialsArray + (i * 0x4);

                    writes.Add(ScatterWriteEntry.Create(material, chamsMaterial));
                }
            }
            else
                throw new Exception($"materialsCount of {materialsCount} is outside the valid range!");
        }
        #endregion

        #region XP Table

        private static readonly IReadOnlyDictionary<int, int> _expTable = new Dictionary<int, int>
        {
            {0, 1},
            {1000, 2},
            {4017, 3},
            {8432, 4},
            {14256, 5},
            {21477, 6},
            {30023, 7},
            {39936, 8},
            {51204, 9},
            {63723, 10},
            {77563, 11},
            {92713, 12},
            {111881, 13},
            {134674, 14},
            {161139, 15},
            {191417, 16},
            {225194, 17},
            {262366, 18},
            {302484, 19},
            {345751, 20},
            {391649, 21},
            {440444, 22},
            {492366, 23},
            {547896, 24},
            {609066, 25},
            {679255, 26},
            {755444, 27},
            {837672, 28},
            {925976, 29},
            {1020396, 30},
            {1120969, 31},
            {1227735, 32},
            {1344260, 33},
            {1470605, 34},
            {1606833, 35},
            {1759965, 36},
            {1923579, 37},
            {2097740, 38},
            {2282513, 39},
            {2477961, 40},
            {2684149, 41},
            {2901143, 42},
            {3132824, 43},
            {3379281, 44},
            {3640603, 45},
            {3929436, 46},
            {4233995, 47},
            {4554372, 48},
            {4890662, 49},
            {5242956, 50},
            {5611348, 51},
            {5995931, 52},
            {6402287, 53},
            {6830542, 54},
            {7280825, 55},
            {7753260, 56},
            {8247975, 57},
            {8765097, 58},
            {9304752, 59},
            {9876880, 60},
            {10512365, 61},
            {11193911, 62},
            {11929835, 63},
            {12727177, 64},
            {13615989, 65},
            {14626588, 66},
            {15864243, 67},
            {17555001, 68},
            {19926895, 69},
            {22926895, 70},
            {26526895, 71},
            {30726895, 72},
            {35526895, 73},
            {40926895, 74},
            {46926895, 75},
            {53526895, 76},
            {60726895, 77},
            {69126895, 78},
            {81126895, 79},
        };

        public static string GetLevel(int experience)
        {
            return (_expTable.Where(x => x.Key > experience).FirstOrDefault().Value - 1).ToString();
        }

        #endregion
    }
}
