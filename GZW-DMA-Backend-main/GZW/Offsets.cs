namespace gzw_dma_backend.GZW
{
    public static class Offsets
    {
        public const uint World = 0x877FFF8;
        public const uint Names = 0x856C7C0; // FNamePool

        public readonly struct UWorld
        {
            public const uint PersistentLevel = 0x30; // ULevel*
            public const uint OwningGameInstance = 0x1B8; // UGameInstance*
        }

        public readonly struct ULevel
        {
            public const uint Actors = 0x98; // TArray<class AActor*>
            public const uint ActorsCount = Actors + 0x8; // uint
        }

        public readonly struct UGameInstance
        {
            public const uint LocalPlayers = 0x38; // TArray<class ULocalPlayer*>
        }

        public readonly struct ULocalPlayer
        {
            public const uint ViewportClient = 0x78; // UGameViewportClient*
        }

        public readonly struct UGameViewportClient
        {
            public const uint World = 0x78; // UWorld*
        }

        public readonly struct UPlayer
        {
            public const uint PlayerController = 0x30; // APlayerController*
        }

        public readonly struct APawn
        {
            public const uint PlayerState = 0x2C0; // APlayerState*
        }

        public readonly struct UObject
        {
            public const uint ID = 0x18; // int
        }

        public readonly struct APlayerController
        {
            public const uint AcknowledgedPawn = 0x348; // APawn*
            public const uint PlayerCameraManager = 0x358; // APlayerCameraManager*
        }

        public readonly struct AController
        {
            public const uint ControlRotation = 0x318; // FRotator
        }

        public readonly struct APlayerCameraManager
        {
            public const uint CameraCachePrivate = 0x1320; // FCameraCacheEntry
        }

        public readonly struct FCameraCacheEntry
        {
            public const uint POV = 0x10; // FMinimalViewInfo
        }

        public readonly struct FMinimalViewInfo
        {
            public const uint Location = 0x0; // FVector
            public const uint Rotation = 0x18; // FRotator
            public const uint FOV = 0x30; // float
        }

        public readonly struct ACharacter
        {
            public const uint Mesh = 0x328; // USkeletalMeshComponent*
            public const uint BoneArray = 0x610; // unknown
        }

        public readonly struct AMFGGameCharacter
        {
            public const uint WantsAiming = 0x712; // bool
            public const uint MyTeamID = 0x778; // FGenericTeamId
            public const uint HealthSystemComponent = 0x7D8; // UMFGHealthSystem*
            public const uint WeaponSwayComponent = 0x7F0; // UMFGWeaponSwayComponent*
            public const uint ScopeRenderComponent = 0x800; // UMFGScopeRenderComponent*
            public const uint CachedMovementComponent = 0x878; // UMFGMovementComponent*
            public const uint IsAI = 0xCED; // bool
            public const uint IsDead = 0xCF8; // bool
        }

        public readonly struct AActor
        {
            public const uint CustomTimeDilation = 0x6C;

            public const uint bReplicateMovementBitfield = 0x60;
            public const int bReplicateMovementBitOffset = 0x4;
        }

        public readonly struct FGenericTeamId
        {
            public const uint TeamID = 0x0; // byte
        }

        public readonly struct UMFGHealthSystem
        {
            public const uint OverallHealthStatus = 0x128; // EMFGOverallHealthStatus
        }

        public readonly struct UMFGWeaponSwayComponent
        {
            public const uint Weapon = 0x138; // UMFGStorageItem
        }

        public readonly struct UMFGStorageItem
        {
            public const uint Setting = 0x38; // UMFGItemBaseSetting
        }

        public readonly struct UMFGItemBaseSetting
        {
            public const uint Weight = 0x140; // float
        }

        public readonly struct USceneCaptureComponent2D
        {
            public const uint FOVAngle = 0x35C; // float
        }

        public readonly struct USceneComponent
        {
            public const uint Bitfield = 0x190; // bitfield
            public const int bVisible = 0x5; // bit index
            public const uint ComponentToWorld = 0x240;
        }

        public readonly struct UMFGMovementComponent
        {
            public const uint CurrentMovementType = 0x1648;
        }
    }
}
