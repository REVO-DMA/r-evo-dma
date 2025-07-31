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
		public readonly struct GameVersion
		{
			public const string Value = @"0.15.0.2.32409";
		}

		public readonly struct StreamerMode
		{
			public const uint ClassName_ClassToken = 0x2001BFE; // MDToken
			public const string ClassName = @"\uE915";
			public const string MethodName = @"IsLocalStreamer";
		}

		public readonly struct FixWildSpawnType
		{
			public const uint ClassName_ClassToken = 0x2002323; // MDToken
			public const string ClassName = @"\uEBDF";
			public const string MethodName = @"SetUpSpawnInfo";
		}

		public readonly struct NetworkContainer
		{
			public const uint ClassName_ClassToken = 0x20005E3; // MDToken
			public const string ClassName = @"\uE2EC";
		}

		public readonly struct InertiaSettings
		{
			public const uint ClassName_ClassToken = 0x2001381; // MDToken
			public const string ClassName = @"\uE6B2";
		}

		public readonly struct GameSettings
		{
			public const uint ClassName_ClassToken = 0x2001B9E; // MDToken
			public const string ClassName = @"\uE905";
		}

		public readonly struct GameAPIClient
		{
			public const uint ClassName_ClassToken = 0x20005D0; // MDToken
			public const string ClassName = @"\uE2E5";
		}

		public readonly struct DogtagComponent
		{
			public const uint MethodName_MethodToken = 0x600E545; // MDToken
			public const string MethodName = @"\uE000";
		}

		public readonly struct GridItemView
		{
			public const uint MethodName_MethodToken = 0x6012B60; // MDToken
			public const string MethodName = @"\uE012";
		}

		public readonly struct AFKMonitor
		{
			public const uint ClassName_ClassToken = 0x20018DB; // MDToken
			public const uint MethodName_MethodToken = 0x600A042; // MDToken
			public const string ClassName = @"\uE88E";
			public const string MethodName = @"MoveNext";
		}

		public readonly struct VitalParts
		{
			public const uint ClassName_ClassToken = 0x2002673; // MDToken
			public const uint MethodName_MethodToken = 0x600E9A9; // MDToken
			public const string ClassName = @"EFT.InventoryLogic.CompoundItem";
			public const string MethodName = @"\uE007";
		}

		public readonly struct EquipmentPenaltyComponent
		{
			public const uint ClassName_ClassToken = 0x2002570; // MDToken
			public const uint BaseCalculationMethod_MethodToken = 0x600E563; // MDToken
			public const uint SpeedPenaltyPercent_MethodToken = 0x600E564; // MDToken
			public const uint MousePenalty_MethodToken = 0x600E566; // MDToken
			public const uint WeaponErgonomicPenalty_MethodToken = 0x600E568; // MDToken
			public const string BaseCalculationMethod = @"\uE000";
			public const string ClassName = @"EFT.InventoryLogic.EquipmentPenaltyComponent+\uE000";
			public const string MousePenalty = @"\uE003";
			public const string SpeedPenaltyPercent = @"\uE001";
			public const string WeaponErgonomicPenalty = @"\uE005";
		}

		public readonly struct AmmoTemplate
		{
			public const uint ClassName_ClassToken = 0x20026CC; // MDToken
			public const uint MethodName_MethodToken = 0x600EBD6; // MDToken
			public const string ClassName = @"\uED8C";
			public const string MethodName = @"get_LoadUnloadModifier";
		}

		public readonly struct NoMalfunctions
		{
			public const uint ClassName_ClassToken = 0x2001551; // MDToken
			public const uint GetMalfunctionState_MethodToken = 0x60087C3; // MDToken
			public const string ClassName = @"EFT.Player+FirearmController";
			public const string GetMalfunctionState = @"GetMalfunctionState";
		}

		public readonly struct InventoryController
		{
			public const uint ClassName_ClassToken = 0x2002776; // MDToken
			public const uint ShowOwnDogTagMethod_MethodToken = 0x600EF9D; // MDToken
			public const uint KeybindFromAnywhereMethodB_MethodToken = 0x600EFA0; // MDToken
			public const uint KeybindFromAnywhereMethodA_MethodToken = 0x600EFA2; // MDToken
			public const string ClassName = @"EFT.InventoryLogic.InventoryController";
			public const string KeybindFromAnywhereMethodA = @"IsAtReachablePlace";
			public const string KeybindFromAnywhereMethodB = @"IsAtBindablePlace";
			public const string ShowOwnDogTagMethod = @"IsAllowedToSeeSlot";
		}

		public readonly struct OpticCameraManagerContainer
		{
			public const uint ClassName_ClassToken = 0x2002A36; // MDToken
			public const string ClassName = @"\uEF2C";
		}

		public readonly struct ScreenManager
		{
			public const uint ClassName_ClassToken = 0x20030FD; // MDToken
			public const string ClassName = @"\uEFF6";
		}

		public readonly struct FovChanger
		{
			public const uint ClassName_ClassToken = 0x2002A36; // MDToken
			public const uint MethodName_MethodToken = 0x600FE38; // MDToken
			public const string ClassName = @"\uEF2C";
			public const string MethodName = @"SetFov";
		}

		public readonly struct LocaleManager
		{
			public const uint ClassName_ClassToken = 0x2001A7B; // MDToken
			public const string ClassName = @"\uE8CB";
		}

		public readonly struct LayerManager
		{
			public const uint ClassName_ClassToken = 0x2000830; // MDToken
			public const string ClassName = @"\uE39F";
		}

		public readonly struct BallisticLayerManager
		{
			public const uint ClassName_ClassToken = 0x2002B5A; // MDToken
			public const string ClassName = @"\uEF6A";
		}

		public readonly struct FirearmController
		{
			public const uint ClassName_ClassToken = 0x2001551; // MDToken
			public const string ClassName = @"EFT.Player+FirearmController";
		}

		public readonly struct LookSensor
		{
			public const uint ClassName_ClassToken = 0x2000536; // MDToken
			public const uint MethodName_MethodToken = 0x600272A; // MDToken
			public const string ClassName = @"LookSensor";
			public const string MethodName = @"CheckAllEnemies";
		}

		public readonly struct ActiveHealthController
		{
			public const uint ClassName_ClassToken = 0x20023C8; // MDToken
			public const uint MethodName_MethodToken = 0x600DDB2; // MDToken
			public const string ClassName = @"EFT.HealthSystem.ActiveHealthController";
			public const string MethodName = @"HandleFall";
		}

		public readonly struct InventoryLogic_Mod
		{
			public const uint ClassName_ClassToken = 0x20026E8; // MDToken
			public const uint MethodName_MethodToken = 0x600EC6E; // MDToken
			public const string ClassName = @"EFT.InventoryLogic.Mod";
			public const string MethodName = @"get_RaidModdable";
		}

		public readonly struct ProceduralWeaponAnimation
		{
			public const uint ClassName_ClassToken = 0x20021E7; // MDToken
			public const uint MethodName_MethodToken = 0x600D1F0; // MDToken
			public const string ClassName = @"EFT.Animations.ProceduralWeaponAnimation";
			public const string MethodName = @"get_ShotNeedsFovAdjustments";
		}

		public readonly struct MovementContext
		{
			public const uint ClassName_ClassToken = 0x2001747; // MDToken
			public const uint MethodName_MethodToken = 0x6009653; // MDToken
			public const string ClassName = @"EFT.MovementContext";
			public const string MethodName = @"SetPhysicalCondition";
		}

		public readonly struct GrenadeFlashScreenEffect
		{
			public const uint ClassName_ClassToken = 0x2000B78; // MDToken
			public const uint MethodName_MethodToken = 0x6004C19; // MDToken
			public const string ClassName = @"GrenadeFlashScreenEffect";
			public const string MethodName = @"Update";
		}
	}

	public readonly struct Offsets
	{
		public readonly struct TarkovApplication
		{
			public static readonly uint[] To_Profile = new uint[] { 0xF0, 0xB8, 0x28 }; // -.\uE896, -.\uE86A, EFT.Profile
		}

		public readonly struct GameWorld
		{
			public const uint LootMaskObstruction = 0x14; // Int32
		}

		public readonly struct ClientLocalGameWorld
		{
			public const uint TransitController = 0x20; // -.\uE71F
			public const uint ExfilController = 0x28; // -.\uE63F
			public const uint LocationId = 0x58; // String
			public const uint LootList = 0xC8; // System.Collections.Generic.List<\uE2D4>
			public const uint RegisteredPlayers = 0xF0; // System.Collections.Generic.List<IPlayer>
			public const uint BorderZones = 0x138; // EFT.Interactive.BorderZone[]
			public const uint MainPlayer = 0x148; // EFT.Player
			public const uint SynchronizableObjectLogicProcessor = 0x178; // -.\uEA7D
			public const uint Grenades = 0x1A0; // -.\uE397<Int32, Throwable>
			public const uint LoadBundlesAndCreatePools = 0x218; // Boolean
		}

		public readonly struct TransitController
		{
			public const uint TransitPoints = 0x18; // System.Collections.Generic.Dictionary<Int32, TransitPoint>
		}

		public readonly struct TransitPoint
		{
			public const uint parameters = 0x20; // -.\uE615.Location.TransitParameters
		}

		public readonly struct TransitParameters
		{
			public const uint name = 0x10; // String
			public const uint description = 0x18; // String
		}

		public readonly struct SynchronizableObject
		{
			public const uint Type = 0x30; // System.Int32
		}

		public readonly struct SynchronizableObjectLogicProcessor
		{
			public const uint SynchronizableObjects = 0x18; // System.Collections.Generic.List<SynchronizableObject>
		}

		public readonly struct TripwireSynchronizableObject
		{
			public const uint GrenadeTemplateId = 0xD0; // EFT.MongoID
			public const uint _tripwireState = 0x12C; // System.Int32
			public const uint FromPosition = 0x130; // UnityEngine.Vector3
			public const uint ToPosition = 0x13C; // UnityEngine.Vector3
		}

		public readonly struct MineDirectional
		{
			public const uint Mines = 0x8; // System.Collections.Generic.List<MineDirectional>
			public const uint MineData = 0x20; // -.MineDirectional.MineSettings
		}

		public readonly struct MineSettings
		{
			public const uint _maxExplosionDistance = 0x28; // Single
			public const uint _directionalDamageAngle = 0x64; // Single
		}

		public readonly struct BorderZone
		{
			public const uint Description = 0x38; // String
			public const uint _extents = 0x48; // UnityEngine.Vector3
		}

		public readonly struct LevelSettings
		{
			public const uint AmbientMode = 0x70; // System.Int32
			public const uint EquatorColor = 0x84; // UnityEngine.Color
			public const uint GroundColor = 0x94; // UnityEngine.Color
		}

		public readonly struct EFTHardSettings
		{
			public const uint DecelerationSpeed = 0x180; // Single
			public const uint LOOT_RAYCAST_DISTANCE = 0x210; // Single
			public const uint DOOR_RAYCAST_DISTANCE = 0x214; // Single
			public const uint STOP_AIMING_AT = 0x264; // Single
			public const uint MOUSE_LOOK_HORIZONTAL_LIMIT = 0x35C; // UnityEngine.Vector2
		}

		public readonly struct GlobalConfigs
		{
			public const uint Inertia = 0xD8; // -.\uE6B2.InertiaSettings
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
			public const uint Settings = 0x78; // EFT.Interactive.ExitTriggerSettings
			public const uint EligibleEntryPoints = 0xA0; // System.String[]
			public const uint _status = 0xC8; // System.Byte
		}

		public readonly struct ScavExfil
		{
			public const uint EligibleIds = 0xE0; // System.Collections.Generic.List<String>
		}

		public readonly struct ExfilSettings
		{
			public const uint Name = 0x18; // String
		}

		public readonly struct GenericCollectionContainer
		{
			public const uint List = 0x18; // System.Collections.Generic.List<Var>
		}

		public readonly struct Grenade
		{
			public const uint IsDestroyed = 0x5D; // Boolean
			public const uint WeaponSource = 0x80; // -.\uEDEC
		}

		public readonly struct Player
		{
			public const uint _characterController = 0x40; // -.ICharacterController
			public const uint MovementContext = 0x58; // EFT.MovementContext
			public const uint _playerBody = 0xC0; // EFT.PlayerBody
			public const uint ProceduralWeaponAnimation = 0x1E0; // EFT.Animations.ProceduralWeaponAnimation
			public const uint _animators = 0x368; // -.IAnimator[]
			public const uint Corpse = 0x3A0; // EFT.Interactive.Corpse
			public const uint Location = 0x590; // String
			public const uint InteractableObject = 0x5A0; // EFT.Interactive.InteractableObject
			public const uint Profile = 0x5C8; // EFT.Profile
			public const uint Physical = 0x5D8; // -.\uE358
			public const uint AIData = 0x5E8; // -.IAIData
			public const uint _healthController = 0x608; // EFT.HealthSystem.IHealthController
			public const uint _inventoryController = 0x620; // -.Player.PlayerInventoryController
			public const uint _handsController = 0x628; // -.Player.AbstractHandsController
			public const uint EnabledAnimators = 0x910; // System.Int32
			public const uint InteractionRayOriginOnStartOperation = 0x97C; // UnityEngine.Vector3
			public const uint InteractionRayDirectionOnStartOperation = 0x988; // UnityEngine.Vector3
			public const uint IsYourPlayer = 0x99E; // Boolean
		}

		public readonly struct AIData
		{
			public const uint IsAI = 0xD4; // Boolean
		}

		public readonly struct ObservedPlayerView
		{
			public const uint GroupID = 0x20; // String
			public const uint NickName = 0x50; // String
			public const uint AccountId = 0x58; // String
			public const uint PlayerBody = 0x68; // EFT.PlayerBody
			public const uint ObservedPlayerController = 0x88; // -.\uEBCC
			public const uint Side = 0x100; // System.Int32
			public const uint IsAI = 0x110; // Boolean
			public const uint VisibleToCameraType = 0x114; // System.Int32
		}

		public readonly struct ObservedPlayerController
		{
			public static readonly uint[] MovementController = new uint[] { 0xC8, 0x10 }; // -.\uEBEC, -.\uEBEE
			public const uint HandsController = 0xD8; // -.\uEBD6
			public const uint InfoContainer = 0xE8; // -.\uEBDF
			public const uint HealthController = 0xF0; // -.\uE403
			public const uint InventoryController = 0x118; // -.\uEBE1
		}

		public readonly struct ObservedMovementController
		{
			public const uint Rotation = 0x88; // UnityEngine.Vector2
			public const uint Velocity = 0x10C; // UnityEngine.Vector3
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
			public const uint _collisionMask = 0x60; // UnityEngine.LayerMask
			public const uint _speedLimit = 0x7C; // Single
			public const uint _sqrSpeedLimit = 0x80; // Single
			public const uint velocity = 0xEC; // UnityEngine.Vector3
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
			public const uint Stamina = 0x38; // -.\uE357
			public const uint HandsStamina = 0x40; // -.\uE357
			public const uint Oxygen = 0x48; // -.\uE357
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
			public const uint HandsContainer = 0x20; // EFT.Animations.PlayerSpring
			public const uint Breath = 0x30; // EFT.Animations.BreathEffector
			public const uint MotionReact = 0x40; // -.MotionEffector
			public const uint Shootingg = 0x50; // -.ShotEffector
			public const uint _optics = 0xC8; // System.Collections.Generic.List<SightNBone>
			public const uint Mask = 0x140; // System.Int32
			public const uint _isAiming = 0x1C5; // Boolean
			public const uint _aimingSpeed = 0x1E4; // Single
			public const uint _fovCompensatoryDistance = 0x1F8; // Single
			public const uint _compensatoryScale = 0x228; // Single
			public const uint CameraSmoothOut = 0x268; // Single
			public const uint PositionZeroSum = 0x344; // UnityEngine.Vector3
			public const uint ShotNeedsFovAdjustments = 0x40F; // Boolean
		}

		public readonly struct SightNBone
		{
			public const uint Mod = 0x10; // EFT.InventoryLogic.SightComponent
		}

		public readonly struct MotionEffector
		{
			public const uint _mouseProcessors = 0x18; // -.\uE3F8[]
			public const uint _movementProcessors = 0x20; // -.\uE3F7[]
		}

		public readonly struct PlayerSpring
		{
			public const uint CameraTransform = 0x70; // UnityEngine.Transform
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
			public const uint Material = 0x98; // UnityEngine.Material
			public const uint On = 0xE8; // Boolean
		}

		public readonly struct NightVision
		{
			public const uint _on = 0xF4; // Boolean
		}

		public readonly struct VisorEffect
		{
			public const uint Intensity = 0xC8; // Single
		}

		public readonly struct Profile
		{
			public const uint Id = 0x10; // String
			public const uint AccountId = 0x18; // String
			public const uint Info = 0x28; // -.\uE83A
			public const uint Skills = 0x60; // EFT.SkillManager
			public const uint TaskConditionCounters = 0x70; // System.Collections.Generic.Dictionary<String, \uF03D>
			public const uint QuestsData = 0x78; // System.Collections.Generic.List<\uF05B>
			public const uint Stats = 0xF8; // -.\uE36D
		}

		public readonly struct ProfileStatsContainer
		{
			public const uint Eft = 0x10; // -.ProfileStats
		}

		public readonly struct ProfileStats
		{
			public const uint OverallCounters = 0x18; // -.\uEAEC
			public const uint TotalInGameTime = 0x70; // Int64
		}

		public readonly struct OverallCounters
		{
			public const uint Counters = 0x10; // System.Collections.Generic.Dictionary<\uE000, Int64>
		}

		public readonly struct PlayerInfo
		{
			public const uint Nickname = 0x10; // String
			public const uint GroupId = 0x20; // String
			public const uint EntryPoint = 0x30; // String
			public const uint Settings = 0x50; // -.\uE830
			public const uint Side = 0x70; // System.Int32
			public const uint RegistrationDate = 0x74; // Int32
			public const uint MemberCategory = 0x90; // System.Int32
			public const uint Experience = 0x98; // Int32
		}

		public readonly struct PlayerInfoSettings
		{
			public const uint Role = 0x10; // System.Int32
		}

		public readonly struct SkillManager
		{
			public const uint StrengthBuffJumpHeightInc = 0x60; // -.SkillManager.\uE004
			public const uint StrengthBuffThrowDistanceInc = 0x70; // -.SkillManager.\uE004
			public const uint MagDrillsLoadSpeed = 0x180; // -.SkillManager.\uE004
			public const uint MagDrillsUnloadSpeed = 0x188; // -.SkillManager.\uE004
		}

		public readonly struct SkillValueContainer
		{
			public const uint Value = 0x30; // Single
		}

		public readonly struct QuestData
		{
			public const uint Id = 0x10; // String
			public const uint CompletedConditions = 0x20; // System.Collections.Generic.HashSet<String>
			public const uint Template = 0x28; // -.\uF05C
			public const uint Status = 0x34; // System.Int32
		}

		public readonly struct ProfileTaskConditionCounter
		{
			public const uint Id = 0x18; // String
			public const uint Value = 0x40; // Int32
		}

		public readonly struct QuestTemplate
		{
			public const uint Conditions = 0x40; // -.\uF042
			public const uint Name = 0x50; // String
		}

		public readonly struct QuestConditionsContainer
		{
			public const uint ConditionsList = 0x50; // System.Collections.Generic.List<Var>
		}

		public readonly struct QuestCondition
		{
			public const uint id = 0x10; // String
		}

		public readonly struct QuestConditionItem
		{
			public const uint value = 0x30; // Single
		}

		public readonly struct QuestConditionFindItem
		{
			public const uint target = 0x48; // System.String[]
		}

		public readonly struct QuestConditionCounterCreator
		{
			public const uint Conditions = 0x50; // -.\uF03A
		}

		public readonly struct QuestConditionVisitPlace
		{
			public const uint target = 0x48; // String
		}

		public readonly struct QuestConditionPlaceBeacon
		{
			public const uint zoneId = 0x50; // String
			public const uint plantTime = 0x58; // Single
		}

		public readonly struct QuestConditionCounterTemplate
		{
			public const uint Conditions = 0x10; // -.\uF03A
		}

		public readonly struct ItemHandsController
		{
			public const uint Item = 0x68; // EFT.InventoryLogic.Item
		}

		public readonly struct FirearmController
		{
			public const uint Fireport = 0xC8; // EFT.BifacialTransform
			public const uint TotalCenterOfImpact = 0x198; // Single
		}

		public readonly struct MovementContext
		{
			public const uint CurrentState = 0xE0; // EFT.BaseMovementState
			public const uint _states = 0x1E0; // System.Collections.Generic.Dictionary<Byte, BaseMovementState>
			public const uint _movementStates = 0x200; // -.IPlayerStateContainerBehaviour[]
			public const uint _tilt = 0x268; // Single
			public const uint _rotation = 0x27C; // UnityEngine.Vector2
			public const uint _physicalCondition = 0x300; // System.Int32
			public const uint _speedLimitIsDirty = 0x305; // Boolean
			public const uint StateSpeedLimit = 0x308; // Single
			public const uint StateSprintSpeedLimit = 0x30C; // Single
			public const uint _lookDirection = 0x420; // UnityEngine.Vector3
			public const uint WalkInertia = 0x4A0; // Single
			public const uint SprintBrakeInertia = 0x4A4; // Single
		}

		public readonly struct MovementState
		{
			public const uint Name = 0x21; // System.Byte
			public const uint AnimatorStateHash = 0x24; // Int32
			public const uint StickToGround = 0x5C; // Boolean
			public const uint PlantTime = 0x60; // Single
		}

		public readonly struct PlayerStateContainer
		{
			public const uint Name = 0x39; // System.Byte
			public const uint StateFullNameHash = 0x50; // Int32
		}

		public readonly struct StationaryWeapon
		{
			public const uint IsMounted = 0xF0; // Boolean
		}

		public readonly struct InventoryController
		{
			public const uint Inventory = 0x118; // EFT.InventoryLogic.Inventory
		}

		public readonly struct Inventory
		{
			public const uint Equipment = 0x10; // EFT.InventoryLogic.InventoryEquipment
			public const uint QuestRaidItems = 0x20; // -.\uEE69
			public const uint QuestStashItems = 0x28; // -.\uEE69
		}

		public readonly struct Equipment
		{
			public const uint Grids = 0x70; // -.\uECED[]
			public const uint Slots = 0x78; // EFT.InventoryLogic.Slot[]
		}

		public readonly struct Slot
		{
			public const uint ContainedItem = 0x38; // EFT.InventoryLogic.Item
			public const uint ID = 0x48; // String
			public const uint Required = 0x60; // Boolean
		}

		public readonly struct InteractiveLootItem
		{
			public const uint Item = 0xB8; // EFT.InventoryLogic.Item
		}

		public readonly struct InteractiveCorpse
		{
			public const uint PlayerBody = 0x130; // EFT.PlayerBody
		}

		public readonly struct LootableContainer
		{
			public const uint ItemOwner = 0x120; // -.\uEE21
			public const uint Template = 0x128; // String
		}

		public readonly struct LootableContainerItemOwner
		{
			public const uint RootItem = 0xB8; // EFT.InventoryLogic.Item
		}

		public readonly struct LootItem
		{
			public const uint Template = 0x40; // EFT.InventoryLogic.ItemTemplate
			public const uint StackObjectsCount = 0x64; // Int32
			public const uint Version = 0x68; // Int32
			public const uint SpawnedInSession = 0x6C; // Boolean
		}

		public readonly struct LootItemMod
		{
			public const uint Grids = 0x70; // -.\uECED[]
			public const uint Slots = 0x78; // EFT.InventoryLogic.Slot[]
		}

		public readonly struct LootItemModGrids
		{
			public const uint ItemCollection = 0x30; // -.\uECEF
		}

		public readonly struct LootItemModGridsItemCollection
		{
			public const uint List = 0x18; // System.Collections.Generic.List<Item>
		}

		public readonly struct LootItemWeapon
		{
			public const uint FireMode = 0x98; // EFT.InventoryLogic.FireModeComponent
			public const uint Chambers = 0xA8; // EFT.InventoryLogic.Slot[]
			public const uint _magSlotCache = 0xC0; // EFT.InventoryLogic.Slot
		}

		public readonly struct FireModeComponent
		{
			public const uint FireMode = 0x28; // System.Byte
		}

		public readonly struct LootItemMagazine
		{
			public const uint Cartridges = 0x98; // EFT.InventoryLogic.StackSlot
			public const uint LoadUnloadModifier = 0x19C; // Single
		}

		public readonly struct StackSlot
		{
			public const uint _items = 0x10; // System.Collections.Generic.List<Item>
			public const uint MaxCount = 0x38; // Int32
		}

		public readonly struct ItemTemplate
		{
			public const uint ShortName = 0x18; // String
			public const uint _id = 0x50; // EFT.MongoID
			public const uint Weight = 0xB0; // Single
			public const uint QuestItem = 0xBC; // Boolean
		}

		public readonly struct ModTemplate
		{
			public const uint Velocity = 0x168; // Single
		}

		public readonly struct AmmoTemplate
		{
			public const uint InitialSpeed = 0x1AC; // Single
			public const uint BallisticCoeficient = 0x1C0; // Single
			public const uint BulletMassGram = 0x248; // Single
			public const uint BulletDiameterMilimeters = 0x24C; // Single
		}

		public readonly struct WeaponTemplate
		{
			public const uint Velocity = 0x24C; // Single
		}

		public readonly struct PlayerBody
		{
			public const uint SkeletonRootJoint = 0x30; // Diz.Skinning.Skeleton
			public const uint BodySkins = 0x48; // System.Collections.Generic.Dictionary<Int32, LoddedSkin>
			public const uint SlotViews = 0x70; // -.\uE397<Int32, \uE001>
			public const uint PointOfView = 0x98; // -.\uF0A9<Int32>
		}

		public readonly struct PlayerBodySubclass
		{
			public const uint Dresses = 0x40; // EFT.Visual.Dress[]
		}

		public readonly struct Dress
		{
			public const uint Renderers = 0x30; // UnityEngine.Renderer[]
		}

		public readonly struct Skeleton
		{
			public const uint _values = 0x30; // System.Collections.Generic.List<Transform>
		}

		public readonly struct LoddedSkin
		{
			public const uint _lods = 0x20; // Diz.Skinning.AbstractSkin[]
		}

		public readonly struct Skin
		{
			public const uint _skinnedMeshRenderer = 0x28; // UnityEngine.SkinnedMeshRenderer
		}

		public readonly struct TorsoSkin
		{
			public const uint _skin = 0x28; // Diz.Skinning.Skin
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
			public const uint _upsampleTexDimension = 0x34; // System.Int32
			public const uint _blurCount = 0x3C; // Int32
		}

		public readonly struct WeatherController
		{
			public const uint WeatherDebug = 0x68; // EFT.Weather.WeatherDebug
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
			public const uint sky = 0x20; // -.TOD_Sky
		}

		public readonly struct TOD_Sky
		{
			public const uint Cycle = 0x20; // -.TOD_CycleParameters
			public const uint TOD_Components = 0x80; // -.TOD_Components
		}

		public readonly struct TOD_CycleParameters
		{
			public const uint Hour = 0x10; // Single
		}

		public readonly struct TOD_Components
		{
			public const uint TOD_Time = 0x118; // -.TOD_Time
		}

		public readonly struct TOD_Time
		{
			public const uint LockCurrentTime = 0x70; // Boolean
		}

		public readonly struct PrismEffects
		{
			public const uint useVignette = 0x12C; // Boolean
			public const uint useExposure = 0x1C0; // Boolean
		}

		public readonly struct CC_Vintage
		{
			public const uint amount = 0x40; // Single
		}

		public readonly struct GPUInstancerManager
		{
			public const uint runtimeDataList = 0x48; // System.Collections.Generic.List<\uE56A>
		}

		public readonly struct RuntimeDataList
		{
			public const uint instanceBounds = 0x68; // UnityEngine.Bounds
		}

		public readonly struct GameSettingsContainer
		{
			public const uint Game = 0x10; // -.\uE905.\uE000<\uE916, \uE915>
			public const uint Graphics = 0x28; // -.\uE905.\uE000<\uE912, \uE910>
		}

		public readonly struct GameSettingsInnerContainer
		{
			public const uint Settings = 0x10; // Var
			public const uint Controller = 0x30; // Var
		}

		public readonly struct GameSettings
		{
			public const uint FieldOfView = 0x60; // Bsg.GameSettings.GameSetting<Int32>
			public const uint HeadBobbing = 0x68; // Bsg.GameSettings.GameSetting<Single>
			public const uint AutoEmptyWorkingSet = 0x70; // Bsg.GameSettings.GameSetting<Boolean>
		}

		public readonly struct GraphicsSettings
		{
			public const uint DisplaySettings = 0x20; // Bsg.GameSettings.GameSetting<\uE90B>
		}

		public readonly struct NetworkContainer
		{
			public const uint NextRequestIndex = 0x8; // Int64
			public const uint PhpSessionId = 0x30; // String
		}

		public readonly struct ScreenManager
		{
			public const uint Instance = 0x0; // -.\uEFF6
			public const uint CurrentScreenController = 0x28; // -.\uEFF8<Var>
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
			public const uint Instance = 0x0; // -.\uEF2C
			public const uint OpticCameraManager = 0x10; // -.\uEF2D
			public const uint FPSCamera = 0x60; // UnityEngine.Camera
		}

		public readonly struct OpticCameraManager
		{
			public const uint Camera = 0x68; // UnityEngine.Camera
			public const uint CurrentOpticSight = 0x70; // EFT.CameraControl.OpticSight
		}

		public readonly struct OpticSight
		{
			public const uint LensRenderer = 0x20; // UnityEngine.Renderer
			// [ERROR] Unable to find offset: "OpticCullingMask"!
		}

		public readonly struct MongoID
		{
			public const uint _stringID = 0x10; // String
		}

		public readonly struct LocaleManager
		{
			public const uint Instance = 0x8; // -.\uE8CB
			public const uint LocaleDictionary = 0x38; // System.Collections.Generic.Dictionary<String, \uE8C8>
			public const uint CurrentCulture = 0x70; // String
		}

		public readonly struct LayerMask
		{
			public const uint m_Mask = 0x0; // [HUMAN] Int32
		}

		public readonly struct LayerManager
		{
			public const uint HighPolyWithTerrainMask = 0x0; // UnityEngine.LayerMask
		}

		public readonly struct BallisticLayerManager
		{
			public const uint HitMask = 0x24; // UnityEngine.LayerMask
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
			IdleWeaponMounting = 29,
		}

		public enum EPlayerSide
		{
			Usec = 1,
			Bear = 2,
			Savage = 4,
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
			Unheard = 1024,
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
			bossPartisan = 47,
			spiritWinter = 48,
			spiritSpring = 49,
			peacemaker = 50,
			pmcBEAR = 51,
			pmcUSEC = 52,
			skier = 53,
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
			// No data!
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
			grenadeThrowing = 6,
			greanadePlanting = 7,
		}

		public enum SynchronizableObjectType
		{
			AirDrop = 0,
			AirPlane = 1,
			Tripwire = 2,
		}

		public enum ETripwireState
		{
			None = 0,
			Wait = 1,
			Active = 2,
			Exploding = 3,
			Exploded = 4,
			Inert = 5,
		}

		public enum EQuestStatus
		{
			Locked = 0,
			AvailableForStart = 1,
			Started = 2,
			AvailableForFinish = 3,
			Success = 4,
			Fail = 5,
			FailRestartable = 6,
			MarkedAsFailed = 7,
			Expired = 8,
			AvailableAfter = 9,
		}
	}
}
