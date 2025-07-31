namespace SDK
{
	public readonly struct GameData
	{
		public const string GameHostname = @"prod.escapefromtarkov.com";
		public const string LauncherHostname = @"launcher.escapefromtarkov.com";
		public const string UNITY_VERSION = @"2019.4.39.7917901";
	}

	public readonly struct ClassNames
	{
		public const string AFKMonitorClass = @"\uE92E";
		public const string DogtagNicknameMethod = @"\uE000";
		public const string GameAPIClient = @"\uE2D6";
		public const string GameSettingsName = @"\uE98D";
		public const string InertiaSettingsName = @"\uE747";
		public const string InventoryGridDogtagNameMethod = @"\uE011";
		public const string NetworkContainerName = @"\uE2DD";
		public const string VitalPartsTestClass = @"\uEE3C";
		public const string VitalPartsTestMethod = @"\uE007";

		public readonly struct GameVersion
		{
			public const string Value = @"0.2.1.0.32571";
		}

		public readonly struct StreamerMode
		{
			public const string ClassName = @"\uE99D";
			public const string MethodName = @"IsLocalStreamer";
		}

		public readonly struct FixWildSpawnType
		{
			public const string ClassName = @"\uEBFE";
			public const string MethodName = @"SetUpSpawnInfo";
		}

		public readonly struct EquipmentPenaltyComponent
		{
			public const string BaseCalculationMethod = @"\uE000";
			public const string ClassName = @"EFT.InventoryLogic.EquipmentPenaltyComponent+\uE000";
			public const string MousePenalty = @"\uE003";
			public const string SpeedPenaltyPercent = @"\uE001";
			public const string WeaponErgonomicPenalty = @"\uE005";
		}

		public readonly struct UnlimitedSearch
		{
			public const string ClassName = @"\uEDC1";
			public const string MethodName = @"CanStartNewSearchOperation";
		}

		public readonly struct AmmoTemplate
		{
			public const string ClassName = @"\uEE6A";
		}

		public readonly struct NoMalfunctions
		{
			public const string ClassName = @"EFT.Player+FirearmController";
			public const string MethodName = @"GetMalfunctionState";
		}

		public readonly struct InventoryController
		{
			public const string ClassName = @"\uEDC1";
			public const string KeybindFromAnywhereMethodA = @"IsAtReachablePlace";
			public const string KeybindFromAnywhereMethodB = @"IsAtBindablePlace";
			public const string ShowOwnDogTagMethod = @"IsAllowedToSeeSlot";
		}

		public readonly struct OpticCameraManagerContainer
		{
			public const string ClassName = @"\uEFBF";
		}

		public readonly struct ScreenManager
		{
			public const string ClassName = @"\uF051";
		}

		public readonly struct FovChanger
		{
			public const string ClassName = @"\uEFBF";
			public const string MethodName = @"SetFov";
		}
	}

	public readonly struct Offsets
	{
		public readonly struct TarkovApplication
		{
			public static readonly uint[] To_Profile = new uint[] { 0xE0, 0xB0, 0x28 }; // -.\uE937, -.\uE90D, EFT.Profile
		}

		public readonly struct GameWorld
		{
			public const uint LootMaskObstruction = 0x1C; // Int32
		}

		public readonly struct ClientLocalGameWorld
		{
			public const uint ExfilController = 0x18; // -.\uE620
			public const uint LocationId = 0x40; // String
			public const uint LootList = 0xC8; // System.Collections.Generic.List<\uE2C3>
			public const uint RegisteredPlayers = 0xF0; // System.Collections.Generic.List<IPlayer>
			public const uint BorderZones = 0x140; // EFT.Interactive.BorderZone[]
			public const uint MainPlayer = 0x150; // EFT.Player
			public const uint Grenades = 0x1B0; // -.\uE38B<Int32, Throwable>
			public const uint LoadBundlesAndCreatePools = 0x230; // Boolean
		}

		public readonly struct MineDirectional
		{
			public const uint Mines = 0x8; // System.Collections.Generic.List<MineDirectional>
			public const uint MineData = 0x18; // -.MineDirectional.MineSettings
		}

		public readonly struct MineSettings
		{
			public const uint _maxExplosionDistance = 0x28; // Single
			public const uint _directionalDamageAngle = 0x64; // Single
		}

		public readonly struct BorderZone
		{
			public const uint Description = 0x30; // String
			public const uint _extents = 0x40; // UnityEngine.Vector3
		}

		public readonly struct LevelSettings
		{
			public const uint AmbientMode = 0x68; // System.Int32
			public const uint EquatorColor = 0x7C; // UnityEngine.Color
			public const uint GroundColor = 0x8C; // UnityEngine.Color
			public const uint HeightFalloff = 0x134; // Single
		}

		public readonly struct EFTHardSettings
		{
			public const uint DecelerationSpeed = 0x180; // Single
			public const uint LOOT_RAYCAST_DISTANCE = 0x23C; // Single
			public const uint DOOR_RAYCAST_DISTANCE = 0x240; // Single
			public const uint STOP_AIMING_AT = 0x290; // Single
			public const uint MOUSE_LOOK_HORIZONTAL_LIMIT = 0x380; // UnityEngine.Vector2
		}

		public readonly struct GlobalConfigs
		{
			public const uint Inertia = 0xD8; // -.\uE747.InertiaSettings
		}

		public readonly struct InertiaSettings
		{
			public const uint FallThreshold = 0x20; // Single
			public const uint BaseJumpPenaltyDuration = 0x4C; // Single
			public const uint BaseJumpPenalty = 0x54; // Single
			public const uint MoveTimeRange = 0xF4; // UnityEngine.Vector2
		}

		public readonly struct ExfilController
		{
			public const uint ExfiltrationPointArray = 0x20; // EFT.Interactive.ExfiltrationPoint[]
			public const uint ScavExfiltrationPointArray = 0x28; // EFT.Interactive.ScavExfiltrationPoint[]
		}

		public readonly struct Exfil
		{
			public const uint Settings = 0x58; // EFT.Interactive.ExitTriggerSettings
			public const uint EligibleEntryPoints = 0x80; // System.String[]
			public const uint _status = 0xA8; // System.Byte
		}

		public readonly struct ScavExfil
		{
			public const uint EligibleIds = 0xC0; // System.Collections.Generic.List<String>
		}

		public readonly struct ExfilSettings
		{
			public const uint Name = 0x10; // String
		}

		public readonly struct GenericCollectionContainer
		{
			public const uint List = 0x18; // System.Collections.Generic.List<Var>
		}

		public readonly struct Grenade
		{
			public const uint IsDestroyed = 0x55; // Boolean
			public const uint WeaponSource = 0x78; // -.\uEEDF
		}

		public readonly struct Player
		{
			public const uint _characterController = 0x28; // -.ICharacterController
			public const uint MovementContext = 0x40; // EFT.MovementContext
			public const uint _playerBody = 0xA8; // EFT.PlayerBody
			public const uint ProceduralWeaponAnimation = 0x1C8; // EFT.Animations.ProceduralWeaponAnimation
			public const uint _animators = 0x398; // -.IAnimator[]
			public const uint Corpse = 0x3D0; // EFT.Interactive.Corpse
			public const uint Location = 0x598; // String
			public const uint InteractableObject = 0x5A8; // EFT.Interactive.InteractableObject
			public const uint Profile = 0x5D0; // EFT.Profile
			public const uint Physical = 0x5E8; // -.\uE34F
			public const uint AIData = 0x5F8; // -.AIData
			public const uint _healthController = 0x618; // EFT.HealthSystem.IHealthController
			public const uint _inventoryController = 0x630; // -.\uEDC1
			public const uint _handsController = 0x638; // -.Player.AbstractHandsController
			public const uint EnabledAnimators = 0x8EC; // System.Int32
			public const uint InteractionRayOriginOnStartOperation = 0x960; // UnityEngine.Vector3
			public const uint InteractionRayDirectionOnStartOperation = 0x96C; // UnityEngine.Vector3
			public const uint IsYourPlayer = 0x982; // Boolean
		}

		public readonly struct AIData
		{
			public const uint IsAI = 0xCC; // Boolean
		}

		public readonly struct ObservedPlayerView
		{
			public const uint GroupID = 0x18; // String
			public const uint NickName = 0x48; // String
			public const uint AccountId = 0x50; // String
			public const uint PlayerBody = 0x60; // EFT.PlayerBody
			public const uint ObservedPlayerController = 0x80; // -.\uEBE7
			public const uint Side = 0xF0; // System.Int32
			public const uint VisibleToCameraType = 0x104; // System.Int32
			public const uint IsAI = 0x109; // Boolean
		}

		public readonly struct ObservedPlayerController
		{
			public static readonly uint[] MovementController = new uint[] { 0x110, 0x10 }; // -.\uEC0A, -.\uEC0C
			public const uint HandsController = 0x120; // -.\uEBF5
			public const uint InfoContainer = 0x130; // -.\uEBFE
			public const uint HealthController = 0x138; // -.\uE3F0
			public const uint InventoryController = 0x160; // -.\uEBE2
		}

		public readonly struct ObservedMovementController
		{
			public const uint Rotation = 0x78; // UnityEngine.Vector2
			public const uint Velocity = 0xD8; // UnityEngine.Vector3
		}

		public readonly struct ObservedHandsController
		{
			public const uint ItemInHands = 0x58; // EFT.InventoryLogic.Item
		}

		public readonly struct ObservedHealthController
		{
			public const uint PlayerCorpse = 0x18; // EFT.Interactive.ObservedCorpse
			public const uint HealthStatus = 0xD8; // System.Int32
		}

		public readonly struct SimpleCharacterController
		{
			public const uint _collisionMask = 0x58; // UnityEngine.LayerMask
			public const uint _speedLimit = 0x74; // Single
			public const uint _sqrSpeedLimit = 0x78; // Single
			public const uint velocity = 0xE4; // UnityEngine.Vector3
		}

		public readonly struct InfoContainer
		{
			public const uint Side = 0x20; // System.Int32
		}

		public readonly struct PlayerSpawnInfo
		{
			public const uint Side = 0x18; // System.Int32
			public const uint WildSpawnType = 0x1C; // System.Int32
		}

		public readonly struct Physical
		{
			public const uint Stamina = 0x38; // -.\uE34E
			public const uint HandsStamina = 0x40; // -.\uE34E
			public const uint Oxygen = 0x48; // -.\uE34E
			public const uint Overweight = 0x8C; // Single
			public const uint WalkOverweight = 0x90; // Single
			public const uint WalkSpeedLimit = 0x94; // Single
			public const uint Inertia = 0x98; // Single
			public const uint WalkOverweightLimits = 0xD8; // UnityEngine.Vector2
			public const uint BaseOverweightLimits = 0xE0; // UnityEngine.Vector2
			public const uint SprintOverweightLimits = 0xF4; // UnityEngine.Vector2
			public const uint SprintWeightFactor = 0x104; // Single
			public const uint SprintAcceleration = 0x114; // Single
			public const uint PreSprintAcceleration = 0x118; // Single
			public const uint IsOverweightA = 0x11C; // Boolean
			public const uint IsOverweightB = 0x11D; // Boolean
		}

		public readonly struct PhysicalValue
		{
			public const uint Current = 0x48; // Single
		}

		public readonly struct ProceduralWeaponAnimation
		{
			public const uint HandsContainer = 0x18; // EFT.Animations.PlayerSpring
			public const uint Breath = 0x28; // EFT.Animations.BreathEffector
			public const uint MotionReact = 0x38; // -.MotionEffector
			public const uint Shootingg = 0x48; // -.ShotEffector
			public const uint _optics = 0xC0; // System.Collections.Generic.List<SightNBone>
			public const uint Mask = 0x150; // System.Int32
			public const uint _isAiming = 0x1D5; // Boolean
			public const uint _aimingSpeed = 0x1F4; // Single
			public const uint _fovCompensatoryDistance = 0x208; // Single
			public const uint _compensatoryScale = 0x238; // Single
			public const uint CameraSmoothOut = 0x270; // Single
			public const uint PositionZeroSum = 0x348; // UnityEngine.Vector3
			public const uint ShotNeedsFovAdjustments = 0x3F1; // Boolean
		}

		public readonly struct SightNBone
		{
			public const uint Mod = 0x10; // EFT.InventoryLogic.SightComponent
		}

		public readonly struct MotionEffector
		{
			public const uint _mouseProcessors = 0x18; // -.\uE3E7[]
			public const uint _movementProcessors = 0x20; // -.\uE3E6[]
		}

		public readonly struct PlayerSpring
		{
			public const uint CameraTransform = 0x68; // UnityEngine.Transform
		}

		public readonly struct BreathEffector
		{
			public const uint Intensity = 0xA4; // Single
		}

		public readonly struct ShotEffector
		{
			public const uint NewShotRecoil = 0x18; // EFT.Animations.NewRecoil.NewRecoilShotEffect
		}

		public readonly struct NewShotRecoil
		{
			public const uint IntensitySeparateFactors = 0x8C; // UnityEngine.Vector3
		}

		public readonly struct ThermalVision
		{
			public const uint Material = 0x90; // UnityEngine.Material
			public const uint On = 0xE0; // Boolean
		}

		public readonly struct NightVision
		{
			public const uint _on = 0xEC; // Boolean
		}

		public readonly struct VisorEffect
		{
			public const uint Intensity = 0xC0; // Single
		}

		public readonly struct Profile
		{
			public const uint Id = 0x10; // String
			public const uint AccountId = 0x18; // String
			public const uint Info = 0x28; // -.\uE7AB
			public const uint Skills = 0x60; // EFT.SkillManager
			public const uint QuestsData = 0x78; // System.Collections.Generic.List<\uED9B>
			public const uint Stats = 0xF0; // -.\uE364
		}

		public readonly struct ProfileStatsContainer
		{
			public const uint Eft = 0x10; // -.ProfileStats
		}

		public readonly struct ProfileStats
		{
			public const uint OverallCounters = 0x28; // -.\uEB46
			public const uint TotalInGameTime = 0x80; // Int64
		}

		public readonly struct OverallCounters
		{
			public const uint Counters = 0x10; // System.Collections.Generic.Dictionary<\uE000, Int64>
		}

		public readonly struct PlayerInfo
		{
			public const uint Nickname = 0x20; // String
			public const uint GroupId = 0x30; // String
			public const uint EntryPoint = 0x40; // String
			public const uint Settings = 0x60; // -.\uE8DA
			public const uint Side = 0x88; // System.Int32
			public const uint RegistrationDate = 0x8C; // Int32
			public const uint MemberCategory = 0xA8; // System.Int32
			public const uint Experience = 0xAC; // Int32
		}

		public readonly struct PlayerInfoSettings
		{
			public const uint Role = 0x10; // System.Int32
		}

		public readonly struct SkillManager
		{
			public const uint StrengthBuffJumpHeightInc = 0x60; // -.SkillManager.FloatBuff
			public const uint StrengthBuffThrowDistanceInc = 0x70; // -.SkillManager.FloatBuff
			public const uint MagDrillsLoadSpeed = 0x180; // -.SkillManager.FloatBuff
			public const uint MagDrillsUnloadSpeed = 0x188; // -.SkillManager.FloatBuff
		}

		public readonly struct SkillValueContainer
		{
			public const uint Value = 0x30; // Single
		}

		public readonly struct QuestData
		{
			public const uint Id = 0x10; // String
			public const uint CompletedConditions = 0x20; // System.Collections.Generic.HashSet<String>
			public const uint Template = 0x28; // -.\uED9C
			public const uint Status = 0x34; // System.Int32
		}

		public readonly struct QuestTemplate
		{
			// [ERROR] Unable to find offset: "Conditions"!
		}

		public readonly struct QuestConditionsContainer
		{
			// [ERROR] Unable to find offset: "QuestConditionsContainer"!
		}

		public readonly struct QuestCondition
		{
			public const uint id = 0x10; // String
		}

		public readonly struct QuestConditionFindItem
		{
			public const uint target = 0x48; // System.String[]
		}

		public readonly struct QuestConditionCounterCreator
		{
			public const uint _templateConditions = 0x48; // -.ConditionCounterCreator.ConditionCounterTemplate
		}

		public readonly struct QuestConditionPlaceBeacon
		{
			public const uint zoneId = 0x50; // String
		}

		public readonly struct QuestConditionCounterTemplate
		{
			public const uint Conditions = 0x10; // -.\uED77
		}

		public readonly struct ItemHandsController
		{
			public const uint Item = 0x60; // EFT.InventoryLogic.Item
		}

		public readonly struct FirearmController
		{
			public const uint Fireport = 0xD0; // EFT.BifacialTransform
			public const uint TotalCenterOfImpact = 0x188; // Single
		}

		public readonly struct MovementContext
		{
			public const uint CurrentState = 0xD0; // EFT.BaseMovementState
			public const uint _states = 0x1D0; // System.Collections.Generic.Dictionary<Byte, BaseMovementState>
			public const uint _tilt = 0x248; // Single
			public const uint _rotation = 0x25C; // UnityEngine.Vector2
			public const uint _physicalCondition = 0x2E0; // System.Int32
			public const uint _speedLimitIsDirty = 0x2E5; // Boolean
			public const uint StateSpeedLimit = 0x2E8; // Single
			public const uint StateSprintSpeedLimit = 0x2EC; // Single
			public const uint _lookDirection = 0x3EC; // UnityEngine.Vector3
			public const uint WalkInertia = 0x478; // Single
			public const uint SprintBrakeInertia = 0x47C; // Single
		}

		public readonly struct MovementState
		{
			public const uint Name = 0x21; // System.Byte
			public const uint StickToGround = 0x5C; // Boolean
		}

		public readonly struct StationaryWeapon
		{
			public const uint IsMounted = 0xE8; // Boolean
		}

		public readonly struct InventoryController
		{
			public const uint Inventory = 0x138; // EFT.InventoryLogic.Inventory
		}

		public readonly struct Inventory
		{
			public const uint Equipment = 0x10; // -.\uEF0B
		}

		public readonly struct Equipment
		{
			public const uint Slots = 0x90; // EFT.InventoryLogic.Slot[]
		}

		public readonly struct Slot
		{
			public const uint Id = 0x18; // String
			public const uint ContainedItem = 0x40; // EFT.InventoryLogic.Item
			public const uint Required = 0x60; // Boolean
		}

		public readonly struct InteractiveLootItem
		{
			public const uint Item = 0xB0; // EFT.InventoryLogic.Item
		}

		public readonly struct LootableContainer
		{
			public const uint ItemOwner = 0x120; // -.\uEDBE
		}

		public readonly struct LootableContainerItemOwner
		{
			public const uint RootItem = 0xC8; // EFT.InventoryLogic.Item
		}

		public readonly struct LootItem
		{
			public const uint Template = 0x58; // EFT.InventoryLogic.ItemTemplate
			public const uint StackObjectsCount = 0x7C; // Int32
			public const uint Version = 0x80; // Int32
			public const uint SpawnedInSession = 0x84; // Boolean
		}

		public readonly struct LootItemMod
		{
			public const uint Grids = 0x88; // -.\uEDE0[]
			public const uint Slots = 0x90; // EFT.InventoryLogic.Slot[]
		}

		public readonly struct LootItemModGrids
		{
			public const uint ItemCollection = 0x48; // -.\uEDE2
		}

		public readonly struct LootItemModGridsItemCollection
		{
			public const uint List = 0x18; // System.Collections.Generic.List<Item>
		}

		public readonly struct LootItemWeapon
		{
			public const uint FireMode = 0xA8; // EFT.InventoryLogic.FireModeComponent
			public const uint Chambers = 0xB8; // EFT.InventoryLogic.Slot[]
			public const uint _magSlotCache = 0xD8; // EFT.InventoryLogic.Slot
		}

		public readonly struct FireModeComponent
		{
			public const uint FireMode = 0x28; // System.Byte
		}

		public readonly struct LootItemMagazine
		{
			public const uint Cartridges = 0xA8; // EFT.InventoryLogic.StackSlot
			public const uint LoadUnloadModifier = 0x194; // Single
		}

		public readonly struct StackSlot
		{
			public const uint _items = 0x18; // System.Collections.Generic.List<Item>
			public const uint MaxCount = 0x40; // Int32
		}

		public readonly struct ItemTemplate
		{
			public const uint ShortName = 0x18; // String
			public const uint _id = 0x60; // String
			public const uint Weight = 0xA0; // Single
			public const uint QuestItem = 0xAC; // Boolean
		}

		public readonly struct ModTemplate
		{
			public const uint Velocity = 0x160; // Single
		}

		public readonly struct AmmoTemplate
		{
			public const uint InitialSpeed = 0x1B0; // Single
			public const uint BallisticCoeficient = 0x1C4; // Single
			public const uint BulletMassGram = 0x24C; // Single
			public const uint BulletDiameterMilimeters = 0x250; // Single
		}

		public readonly struct WeaponTemplate
		{
			public const uint Velocity = 0x228; // Single
		}

		public readonly struct PlayerBody
		{
			public const uint SkeletonRootJoint = 0x28; // Diz.Skinning.Skeleton
			public const uint BodySkins = 0x40; // System.Collections.Generic.Dictionary<Int32, LoddedSkin>
			public const uint SlotViews = 0x58; // -.\uE38B<Int32, \uE000>
			public const uint PointOfView = 0x78; // -.\uE6A8<Int32>
		}

		public readonly struct PlayerBodySubclass
		{
			public const uint Dresses = 0x40; // EFT.Visual.Dress[]
		}

		public readonly struct Dress
		{
			public const uint Renderers = 0x28; // UnityEngine.Renderer[]
		}

		public readonly struct Skeleton
		{
			public const uint _values = 0x28; // System.Collections.Generic.List<Transform>
		}

		public readonly struct LoddedSkin
		{
			public const uint _lods = 0x18; // Diz.Skinning.AbstractSkin[]
		}

		public readonly struct Skin
		{
			public const uint _skinnedMeshRenderer = 0x20; // UnityEngine.SkinnedMeshRenderer
		}

		public readonly struct TorsoSkin
		{
			public const uint _skin = 0x20; // Diz.Skinning.Skin
		}

		public readonly struct SlotViewsContainer
		{
			public const uint Dict = 0x10; // System.Collections.Generic.Dictionary<Var, Var>
		}

		public readonly struct PointOfView
		{
			public const uint POV = 0x10; // Var
		}

		public readonly struct InventoryBlur
		{
			public const uint _upsampleTexDimension = 0x2C; // System.Int32
			public const uint _blurCount = 0x34; // Int32
		}

		public readonly struct WeatherController
		{
			public const uint WeatherDebug = 0x80; // EFT.Weather.WeatherDebug
		}

		public readonly struct WeatherDebug
		{
			public const uint isEnabled = 0x18; // Boolean
			public const uint WindMagnitude = 0x1C; // Single
			public const uint CloudDensity = 0x2C; // Single
			public const uint Fog = 0x30; // Single
			public const uint Rain = 0x34; // Single
			public const uint LightningThunderProbability = 0x38; // Single
		}

		public readonly struct TOD_Scattering
		{
			public const uint sky = 0x18; // -.TOD_Sky
		}

		public readonly struct TOD_Sky
		{
			public const uint Cycle = 0x18; // -.TOD_CycleParameters
			public const uint TOD_Components = 0x80; // -.TOD_Components
		}

		public readonly struct TOD_CycleParameters
		{
			public const uint Hour = 0x10; // Single
		}

		public readonly struct TOD_Components
		{
			public const uint TOD_Time = 0x140; // -.TOD_Time
		}

		public readonly struct TOD_Time
		{
			public const uint LockCurrentTime = 0x68; // Boolean
		}

		public readonly struct PrismEffects
		{
			public const uint useVignette = 0x124; // Boolean
			public const uint useExposure = 0x1B8; // Boolean
		}

		public readonly struct CC_Vintage
		{
			public const uint amount = 0x38; // Single
		}

		public readonly struct GPUInstancerManager
		{
			public const uint runtimeDataList = 0x40; // System.Collections.Generic.List<\uE532>
		}

		public readonly struct RuntimeDataList
		{
			public const uint instanceBounds = 0x68; // UnityEngine.Bounds
		}

		public readonly struct GameSettingsContainer
		{
			public const uint Game = 0x10; // -.\uE98D.\uE000<\uE99E, \uE99D>
			public const uint Graphics = 0x28; // -.\uE98D.\uE000<\uE99A, \uE998>
		}

		public readonly struct GameSettingsInnerContainer
		{
			public const uint Settings = 0x10; // Var
			public const uint Controller = 0x30; // Var
		}

		public readonly struct GameSettings
		{
			public const uint FieldOfView = 0x58; // Bsg.GameSettings.GameSetting<Int32>
			public const uint HeadBobbing = 0x60; // Bsg.GameSettings.GameSetting<Single>
			public const uint AutoEmptyWorkingSet = 0x68; // Bsg.GameSettings.GameSetting<Boolean>
		}

		public readonly struct GraphicsSettings
		{
			public const uint DisplaySettings = 0x20; // Bsg.GameSettings.GameSetting<\uE993>
		}

		public readonly struct NetworkContainer
		{
			public const uint NextRequestIndex = 0x8; // Int64
			public const uint PhpSessionId = 0x30; // String
		}

		public readonly struct ScreenManager
		{
			public const uint Instance = 0x0; // -.\uF051
			public const uint CurrentScreenController = 0x28; // -.\uF053<Var>
		}

		public readonly struct CurrentScreenController
		{
			public const uint Generic = 0x20; // Var
		}

		public readonly struct BSGGameSetting
		{
			public const uint ValueClass = 0x28; // [HUMAN] ulong
		}

		public readonly struct BSGGameSettingValueClass
		{
			public const uint Value = 0x30; // [HUMAN] T
		}

		public readonly struct SSAA
		{
			public const uint OpticMaskMaterial = 0x58; // [HUMAN] UnityEngine.Material
		}

		public readonly struct BloomAndFlares
		{
			public const uint BloomIntensity = 0xB8; // [HUMAN] Single
		}

		public readonly struct OpticCameraManagerContainer
		{
			public const uint Instance = 0x0; // -.\uEFBF
			public const uint OpticCameraManager = 0x10; // -.\uEFC3
			public const uint FPSCamera = 0x68; // UnityEngine.Camera
		}

		public readonly struct OpticCameraManager
		{
			public const uint Camera = 0x68; // UnityEngine.Camera
			public const uint CurrentOpticSight = 0x70; // EFT.CameraControl.OpticSight
		}

		public readonly struct OpticSight
		{
			public const uint LensRenderer = 0x18; // UnityEngine.Renderer
			public const uint OpticCullingMask = 0x30; // EFT.PostEffects.OpticCullingMask
		}
	}

	public readonly struct Enums
	{
		public enum EPlayerState
		{
			None = 0,
			Idle = 1,
			ProneIdle = 2,
			ProneMove = 3,
			Run = 4,
			Sprint = 5,
			Jump = 6,
			FallDown = 7,
			Transition = 8,
			BreachDoor = 9,
			Loot = 10,
			Pickup = 11,
			Open = 12,
			Close = 13,
			Unlock = 14,
			Sidestep = 15,
			DoorInteraction = 16,
			Approach = 17,
			Prone2Stand = 18,
			Transit2Prone = 19,
			Plant = 20,
			Stationary = 21,
			Roll = 22,
			JumpLanding = 23,
			ClimbOver = 24,
			ClimbUp = 25,
			VaultingFallDown = 26,
			VaultingLanding = 27,
			BlindFire = 28,
		}

		public enum EPlayerSide
		{
			Usec = 1,
			Bear = 2,
			Savage = 4,
			Observer = 8,
		}

		[Flags]
		public enum ETagStatus
		{
			Unaware = 1,
			Aware = 2,
			Combat = 4,
			Solo = 8,
			Coop = 16,
			Bear = 32,
			Usec = 64,
			Scav = 128,
			TargetSolo = 256,
			TargetMultiple = 512,
			Healthy = 1024,
			Injured = 2048,
			BadlyInjured = 4096,
			Dying = 8192,
			Birdeye = 16384,
			Knight = 32768,
			BigPipe = 65536,
		}

		[Flags]
		public enum EMemberCategory
		{
			Default = 0,
			Developer = 1,
			UniqueId = 2,
			Trader = 4,
			Group = 8,
			System = 16,
			ChatModerator = 32,
			ChatModeratorWithPermanentBan = 64,
			UnitTest = 128,
			Sherpa = 256,
			Emissary = 512,
		}

		public enum WildSpawnType
		{
			marksman = 0,
			assault = 1,
			bossTest = 2,
			bossBully = 3,
			followerTest = 4,
			followerBully = 5,
			bossKilla = 6,
			bossKojaniy = 7,
			followerKojaniy = 8,
			pmcBot = 9,
			cursedAssault = 10,
			bossGluhar = 11,
			followerGluharAssault = 12,
			followerGluharSecurity = 13,
			followerGluharScout = 14,
			followerGluharSnipe = 15,
			followerSanitar = 16,
			bossSanitar = 17,
			test = 18,
			assaultGroup = 19,
			sectantWarrior = 20,
			sectantPriest = 21,
			bossTagilla = 22,
			followerTagilla = 23,
			exUsec = 24,
			gifter = 25,
			bossKnight = 26,
			followerBigPipe = 27,
			followerBirdEye = 28,
			bossZryachiy = 29,
			followerZryachiy = 30,
			bossBoar = 32,
			followerBoar = 33,
			arenaFighter = 34,
			arenaFighterEvent = 35,
			bossBoarSniper = 36,
			crazyAssaultEvent = 37,
			peacefullZryachiyEvent = 38,
			sectactPriestEvent = 39,
			ravangeZryachiyEvent = 40,
			followerBoarClose1 = 41,
			followerBoarClose2 = 42,
			bossKolontay = 43,
			followerKolontayAssault = 44,
			followerKolontaySecurity = 45,
			shooterBTR = 46,
			purifyGroupBot = 47,
		}

		public enum EExfiltrationStatus
		{
			NotPresent = 1,
			UncompleteRequirements = 2,
			Countdown = 3,
			RegularMode = 4,
			Pending = 5,
			AwaitsManualActivation = 6,
		}

		public enum EMalfunctionState
		{
			None = 0,
			Misfire = 1,
			Jam = 2,
			HardSlide = 3,
			SoftSlide = 4,
			Feed = 5,
		}

		[Flags]
		public enum EPhysicalCondition
		{
			None = 0,
			OnPainkillers = 1,
			LeftLegDamaged = 2,
			RightLegDamaged = 4,
			ProneDisabled = 8,
			LeftArmDamaged = 16,
			RightArmDamaged = 32,
			Tremor = 64,
			UsingMeds = 128,
			HealingLegs = 256,
			JumpDisabled = 512,
			SprintDisabled = 1024,
			ProneMovementDisabled = 2048,
			Panic = 4096,
		}

		[Flags]
		public enum EProceduralAnimationMask
		{
			Breathing = 1,
			Walking = 2,
			MotionReaction = 4,
			ForceReaction = 8,
			Shooting = 16,
			DrawDown = 32,
			Aiming = 64,
			HandShake = 128,
		}

		[Flags]
		public enum EAnimatorMask
		{
			Thirdperson = 1,
			Arms = 2,
			Procedural = 4,
			FBBIK = 8,
			IK = 16,
		}

		public enum InventoryBlurDimensions
		{
			_128 = 128,
			_256 = 256,
			_512 = 512,
			_1024 = 1024,
			_2048 = 2048,
		}

		public enum ECameraType
		{
			Default = 0,
			Spectator = 1,
			UIBackground = 2,
			KillCamera = 3,
		}

		public enum ColorType
		{
			red = 1,
			fuchsia = 2,
			yellow = 3,
			green = 4,
			azure = 5,
			white = 6,
			blue = 7,
			grey = 8,
		}

		public enum EWeaponModType
		{
			mod_mount = 1,
			mod_scope = 2,
			mod_tactical = 4,
			mod_stock = 8,
			mod_magazine = 16,
			mod_barrel = 32,
			mod_handguard = 64,
			mod_muzzle = 128,
			mod_sight_front = 256,
			mod_sight_rear = 512,
			mod_foregrip = 1024,
			mod_reciever = 2048,
			mod_charge = 4096,
			mod_pistol_grip = 8192,
			mod_launcher = 16384,
			mod_bipod = 32768,
			mod_mag_shaft = 65536,
			mod_silencer = 131072,
			mod_tactical_2 = 262144,
			chamber0 = 524288,
			chamber1 = 1048576,
			patron_in_weapon = 2097152,
			mod_gas_block = 4194304,
			mod_equipment = 8388608,
			mod_equipment_000 = 16777216,
			mod_equipment_001 = 33554432,
			mod_nvg = 67108864,
			mod_flashlight = 134217728,
			mod_muzzle_001 = 268435456,
		}

		public enum EquipmentSlot
		{
			FirstPrimaryWeapon = 0,
			SecondPrimaryWeapon = 1,
			Holster = 2,
			Scabbard = 3,
			Backpack = 4,
			SecuredContainer = 5,
			TacticalVest = 6,
			ArmorVest = 7,
			Pockets = 8,
			Eyewear = 9,
			FaceCover = 10,
			Headwear = 11,
			Earpiece = 12,
			Dogtag = 13,
			ArmBand = 14,
		}

		public enum EFireMode
		{
			fullauto = 0,
			single = 1,
			doublet = 2,
			burst = 3,
			doubleaction = 4,
			semiauto = 5,
		}
	}
}
