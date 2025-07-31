using arena_dma_backend.DMA.Collections.Implementation;
using arena_dma_backend.DMA.Collections.Implementation.NoThrow;
using arena_dma_backend.Unity;
using System.Collections.Frozen;
using static arena_dma_backend.Arena.Misc;

namespace arena_dma_backend.Arena
{
    public class Player
    {
        public enum PurgeStatus
        {
            None,
            Queued,
            CanPurge
        }

        #region Bone Stuff

        public enum BoneNames
        {
            HumanBase,
            HumanPelvis,
            HumanLThigh1,
            HumanLCalf,
            HumanLFoot,
            HumanRThigh1,
            HumanRCalf,
            HumanRFoot,
            HumanSpine1,
            HumanSpine2,
            HumanSpine3,
            HumanLUpperarm,
            HumanLForearm1,
            HumanLForearm3,
            HumanRUpperarm,
            HumanRForearm1,
            HumanRForearm3,
            HumanNeck,
            HumanHead,
        }

        public static readonly FrozenDictionary<int, string> BoneIndexToName = new Dictionary<int, string>()
        {
            { 1, "Groin" },
            { 2, "Left Hip" },
            { 3, "Left Knee" },
            { 4, "Left Ankle" },
            { 5, "Right Hip" },
            { 6, "Right Knee" },
            { 7, "Right Ankle" },
            { 8, "Stomach" },
            { 9, "Center Mass" },
            { 10, "Chest" },
            { 11, "Left Shoulder" },
            { 12, "Left Elbow" },
            { 13, "Left Wrist" },
            { 14, "Right Shoulder" },
            { 15, "Right Elbow" },
            { 16, "Right Wrist" },
            { 17, "Neck" },
            { 18, "Head" }
        }.ToFrozenDictionary();

        public static readonly int[] BoneIndices = {
            0, // HumanBase         [0]
            14, // HumanPelvis      [1]
            15, // HumanLThigh1     [2]
            17, // HumanLCalf       [3]
            18, // HumanLFoot       [4]
            20, // HumanRThigh1     [5]
            22, // HumanRCalf       [6]
            23, // HumanRFoot       [7]
            29, // HumanSpine1      [8]
            36, // HumanSpine2      [9]
            37, // HumanSpine3      [10]
            90, // HumanLUpperarm   [11]
            91, // HumanLForearm1   [12]
            93, // HumanLForearm3   [13]
            111, // HumanRUpperarm  [14]
            112, // HumanRForearm1  [15]
            114, // HumanRForearm3  [16]
            132, // HumanNeck       [17]
            133, // HumanHead       [18]
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
        
        public static int BonesCount => BoneIndices.Length;

        public Transform[] Bones { get; private init; } = new Transform[BoneIndices.Length];
        public Vector3[] BonePositions { get; set; } = new Vector3[BoneIndices.Length];
        public Vector3 Position => BonePositions[(int)BoneNames.HumanBase];
        public Vector3 Velocity { get; set; }

        #endregion

        #region Player Properties

        public ulong UpdateIteration { get; private init; }

        public bool IsSafe() => FullyAllocated && IsAlive && EPurgeStatus == PurgeStatus.None;
        public bool CanRender() => IsSafe() && !IsLocal && Distance > 1f;

        // Global Addresses
        public ulong Base {  get; private init; }
        public ulong Body {  get; private init; }
        public ulong CorpseAddress
        {
            get
            {
                if (IsClientPlayer)
                    return Base + Offsets.Player.Corpse;
                else
                    return ObservedHealthController + Offsets.ObservedHealthController.PlayerCorpse;
            }
        }

        // Observed Player Addresses
        public ulong MovementController { get; private init; }
        private ulong ObservedHealthController { get; init; }
        public ulong VelocityAddress => MovementController + Offsets.ObservedMovementController.Velocity;

        // LocalPlayer Addresses
        public ulong MovementContext { get; private init; }
        public ulong PWA { get; private init; }

        // Readonly Player Data
        public string Name {  get; private init; }
        public Enums.ColorType? Color { get; private init; }
        public bool IsClientPlayer { get; private init; }
        public bool IsLocal { get; private init; }
        public bool FullyAllocated { get; init; }

        // Dynamic Player Data
        public bool IsAlive { get; set; } = true;
        public bool IsTeammate { get; set; }

        public string ESP_NameString { get; private set; }
        public string ESP_DistanceString { get; private set; }
        private float _Distance;
        public float Distance
        {
            get { return _Distance; }
            set
            {
                _Distance = value;
                ESP_NameString = $"{Name}";
                ESP_DistanceString = $"{value:F0}m";
            }
        }

        public bool IsAimbotLocked { get; set; }
        public bool ChamsDirty { get; set; }

        public PurgeStatus EPurgeStatus { get; set; } = PurgeStatus.None;

        #endregion

        public Player(ulong baseAddress, ulong updateIteration)
        {
            try
            {
                Base = baseAddress;
                UpdateIteration = updateIteration;

                Enums.ECameraType cameraType = (Enums.ECameraType)Memory.ReadValue<int>(Base + Offsets.ObservedPlayerView.VisibleToCameraType);
                if (cameraType != Enums.ECameraType.Default)
                {
                    FullyAllocated = false;
                    return;
                }

                IsClientPlayer = GetIsClientPlayer(baseAddress);

                if (IsClientPlayer)
                {
                    ulong profile = Memory.ReadPtr(Base + Offsets.Player.Profile);
                    ulong playerInfo = Memory.ReadPtr(profile + Offsets.Profile.Info);
                    ulong namePtr = Memory.ReadPtr(playerInfo + Offsets.PlayerInfo.Nickname);
                    Name = Memory.ReadUnityString(namePtr);
                    Body = Memory.ReadPtr(Base + Offsets.Player._playerBody);
                    MovementContext = Memory.ReadPtr(Base + Offsets.Player.MovementContext);
                    PWA = Memory.ReadPtr(Base + Offsets.Player.ProceduralWeaponAnimation);
                    IsLocal = true;

                    ulong inventoryController = inventoryController = Memory.ReadPtr(Base + Offsets.Player._inventoryController);

                    Color = GetTeamColor(inventoryController);
                    Game.LocalPlayerColor = Color;
                }
                else
                {
                    ulong namePtr = Memory.ReadPtr(Base + Offsets.ObservedPlayerView.NickName);
                    Name = Memory.ReadUnityString(namePtr);
                    Body = Memory.ReadPtr(Base + Offsets.ObservedPlayerView.PlayerBody);
                    IsLocal = false;

                    ulong observedPlayerController = Memory.ReadPtr(Base + Offsets.ObservedPlayerView.ObservedPlayerController);

                    MovementController = Memory.ReadPtrChain(observedPlayerController, Offsets.ObservedPlayerController.MovementController);

                    ObservedHealthController = Memory.ReadPtr(observedPlayerController + Offsets.ObservedPlayerController.HealthController);

                    ulong inventoryController = Memory.ReadPtr(observedPlayerController + Offsets.ObservedPlayerController.InventoryController);

                    Color = GetTeamColor(inventoryController);
                }

                // Get transforms
                ulong transformsAddress = Memory.ReadPtrChain(Body, UnityOffsets.Transform.GetTransformChainLeadup());
                for (int i = 0; i < BonesCount; i++)
                {
                    ulong transformInternal = Memory.ReadPtrChain(transformsAddress, UnityOffsets.Transform.GetTransformChainPartial(BoneIndices[i]));

                    if (i == 0)
                        Bones[i] = new Transform(transformInternal, Transform.TransformType.PlayerRootPos, true);
                    else
                        Bones[i] = new Transform(transformInternal, Transform.TransformType.Normal, true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[PLAYER] Failed to allocate player at 0x{baseAddress:X}: {ex}");
                
                FullyAllocated = false;
                return;
            }

            FullyAllocated = true;
        }

        #region Private Static Methods

        private static bool GetIsClientPlayer(ulong playerBase)
        {
            ulong classNamePtr = Memory.ReadPtrChain(playerBase, [0x0, 0x0, 0x48]);
            string className = Memory.ReadUtf8String(classNamePtr, 64);

            if (className == "ArenaClientPlayer" || className == "LocalPlayer")
                return true;
            else
                return false;
        }

        private static Enums.ColorType? GetTeamColor(ulong inventoryController)
        {
            try
            {
                ulong inventory = Memory.ReadPtr(inventoryController + Offsets.InventoryController.Inventory);
                ulong equipment = Memory.ReadPtr(inventory + Offsets.Inventory.Equipment);
                ulong slots = Memory.ReadPtr(equipment + Offsets.Equipment.Slots);
                using MemArray<ulong> slotsArray = new(slots);

                foreach (var slotPtr in slotsArray)
                {
                    ulong slotNamePtr = Memory.ReadPtr(slotPtr + Offsets.Slot.Id);
                    var name = Memory.ReadUnityString(slotNamePtr);
                    if (name == "ArmBand")
                    {
                        ulong containedItem = Memory.ReadPtr(slotPtr + Offsets.Slot.ContainedItem);
                        ulong inventoryTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                        ulong idPtr = Memory.ReadPtr(inventoryTemplate + Offsets.ItemTemplate._id);
                        string id = Memory.ReadUnityString(idPtr);

                        if (id == "63615c104bc92641374a97c8")
                            return Enums.ColorType.red;
                        else if (id == "63615bf35cb3825ded0db945")
                            return Enums.ColorType.fuchsia;
                        else if (id == "63615c36e3114462cd79f7c1")
                            return Enums.ColorType.yellow;
                        else if (id == "63615bfc5cb3825ded0db947")
                            return Enums.ColorType.green;
                        else if (id == "63615bc6ff557272023d56ac")
                            return Enums.ColorType.azure;
                        else if (id == "63615c225cb3825ded0db949")
                            return Enums.ColorType.white;
                        else if (id == "63615be82e60050cb330ef2f")
                            return Enums.ColorType.blue;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[PLAYER] -> GetTeamColor(): Unable to get team color: {ex}");
            }

            // This is likely the cleanup crew or someone who force dropped their armband. Treat them as an enemy!
            return null;
        }

        #endregion

        #region Chams

        private readonly object _lock = new object();

        public void SetChams()
        {
            if (IsTeammate)
                SetChams(ChamsManager.TeammateMaterial.InstanceID);
            else
                SetChams(ChamsManager.EnemyMaterial.InstanceID);
        }

        public void SetChams(int chamsMaterial)
        {
            static void LogError(Exception ex, string playerName)
            {
                Logger.WriteLine($"[PLAYER]: SetChams() -> Failed to set chams for \"{playerName}\" -- {ex}");
            }

            if (!Game.ChamsEnabled)
                return;

            if (Monitor.TryEnter(_lock, TimeSpan.Zero))
            {
                try
                {
                    try { ApplyClothingChams(this, chamsMaterial); }
                    catch (Exception ex) { LogError(ex, Name); }

                    try { ApplyGearChams(this, chamsMaterial); }
                    catch (Exception ex) { LogError(ex, Name); }
                }
                finally { Monitor.Exit(_lock); }
            }
        }

        private static void ApplyClothingChams(Player player, int chamsMaterial)
        {
            try
            {
                ulong pSkinsDict = Memory.ReadPtr(player.Body + Offsets.PlayerBody.BodySkins);

                using MemDictionary<ulong, ulong> skinsDict = new(pSkinsDict);
                if (skinsDict.Count == 0)
                    throw new Exception("Skins dict is empty!");

                foreach (var loddedSkin in skinsDict)
                {
                    ulong loddedSkinValue = loddedSkin.Value;
                    if (loddedSkinValue == 0x0)
                        continue;

                    ulong pLodsArray = Memory.ReadPtrUnsafe(loddedSkinValue + Offsets.LoddedSkin._lods);
                    if (pLodsArray == 0x0)
                        continue;

                    using var lodsArray = NoThrowMemArray<ulong>.Get(pLodsArray);
                    if (lodsArray.Count == 0)
                        continue;

                    foreach (ulong abstractSkin in lodsArray)
                    {
                        if (abstractSkin == 0x0)
                            continue;
                        
                        ulong skinnedMeshRenderer;

                        /// Determine if this is a TorsoSkin
                        ulong asClassNamePtr = Memory.ReadPtrChainUnsafe(abstractSkin, UnityOffsets.Component.To_NativeClassName);
                        if (asClassNamePtr == 0x0)
                            continue;

                        string asClassName = Memory.ReadUtf8StringUnsafe(asClassNamePtr, 64);
                        if (asClassName.Equals("TorsoSkin", StringComparison.OrdinalIgnoreCase))
                        {
                            ulong skin = Memory.ReadPtrUnsafe(abstractSkin + Offsets.TorsoSkin._skin);
                            if (skin == 0x0)
                                continue;

                            skinnedMeshRenderer = Memory.ReadPtrUnsafe(skin + Offsets.Skin._skinnedMeshRenderer);
                        }
                        else // "Skin"
                            skinnedMeshRenderer = Memory.ReadPtrUnsafe(abstractSkin + Offsets.Skin._skinnedMeshRenderer);

                        if (skinnedMeshRenderer == 0x0)
                            continue;

                        // Cached ptr to Renderer
                        ulong renderer = Memory.ReadPtrUnsafe(skinnedMeshRenderer + UnityOffsets.SkinnedMeshRenderer.Renderer);
                        if (renderer == 0x0)
                            continue;

                        WriteChamsMaterial(player, renderer, chamsMaterial);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[ApplyClothingChams]: {ex}");
            }
        }

        private static void ApplyGearChams(Player player, int chamsMaterial)
        {
            try
            {
                ulong slotViews = Memory.ReadPtr(player.Body + Offsets.PlayerBody.SlotViews);

                ulong pSlotViewsDict = Memory.ReadPtr(slotViews + 0x10);

                using MemDictionary<ulong, ulong> slotViewsDict = new(pSlotViewsDict);
                if (slotViewsDict.Count == 0)
                    throw new Exception("Slot views dict is empty!");

                foreach (var slot in slotViewsDict)
                {
                    ulong slotValue = slot.Value;
                    if (slotValue == 0x0)
                        continue;

                    ulong pDressesArray = Memory.ReadPtrUnsafe(slotValue + Offsets.PlayerBodySubclass.Dresses);
                    if (pDressesArray == 0x0)
                        continue;

                    using var dressesArray = NoThrowMemArray<ulong>.Get(pDressesArray);
                    if (dressesArray.Count == 0)
                        continue;

                    foreach (ulong dress in dressesArray)
                    {
                        if (dress == 0x0)
                            continue;

                        ulong pRenderersArray = Memory.ReadPtrUnsafe(dress + Offsets.Dress.Renderers);
                        if (pRenderersArray == 0x0)
                            continue;

                        using var renderersArray = NoThrowMemArray<ulong>.Get(pRenderersArray);
                        if (renderersArray.Count == 0)
                            continue;

                        foreach (ulong renderer in renderersArray)
                        {
                            if (renderer == 0x0)
                                continue;

                            ulong materials = Memory.ReadPtrUnsafe(renderer + 0x10);
                            if (materials == 0x0)
                                continue;

                            WriteChamsMaterial(player, materials, chamsMaterial);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[ApplyGearChams]: {ex}");
            }
        }

        private static void WriteChamsMaterial(Player player, ulong renderer, int chamsMaterial)
        {
            var materialsCount = Memory.ReadValueUnsafe<int>(renderer + UnityOffsets.Renderer.Count, false);
            if (materialsCount <= 0 || materialsCount > 30)
                return;

            ulong arrayBase = Memory.ReadPtrUnsafe(renderer + UnityOffsets.Renderer.Materials, false);
            if (arrayBase == 0x0)
                return;

            using var materials = NoThrowMemArray<int>.Get(arrayBase, materialsCount);
            if (Memory.CanWrite && player.IsSafe())
                materials.OverwriteValues(chamsMaterial);
        }

        #endregion
    }
}
