namespace pubg_dma_esp.PUBG
{
    internal static class Offsets
    {
        public readonly struct Util
        {
            public const bool OffsetDebug = false;
            public const bool CameraDebug = false;
            public const bool NamesDebug = false;
            public const bool PositionDebug = false;
        }

        public readonly struct DecryptFNameIndex // DecryptCIndex
        {
            public const uint Offset = 0xC; // UObject - Name (ObjectID)

            public const uint NameIndexXor1 = 0x6E10A808;
            public const int NameIndexOne = 0xA; // ror (ShiftValue)
            public const int NameIndexTwo = 0x10; // shr/shl (?)
            public const uint NameIndexXor2 = 0x7C25C770;
            public const bool NameIsROR = true;
        }

        public readonly struct Xenuine
        {
            public const uint Decrypt = 0xFED6528;
        }

        public readonly struct GName
        {
            public const uint Base = 0x119C9908;
            public const bool OffsetBase = false;
            public const uint ElementsPerChunk = 0x3E10;
        }

        public readonly struct GWorld
        {
            public const uint Base = 0x1175EEE8;
            // struct UWorld : UObject
            public const uint CurrentLevel = 0x150; // struct ULevel* CurrentLevel;
            public const uint GameInstance = 0x160; // padding[0x8] just below CurrentLevel
        }

        public readonly struct CurrentLevel
        {
            // struct ULevel : UObject
            public const uint Actors = 0x110; // inside of padding with a minimum size of 0x8
        }

        public readonly struct GameInstance
        {
            public const uint LocalPlayers = 0xE8; // unsure
        }

        public readonly struct Player
        {
            public const uint Controller = 0x30; // unsure
        }

        public readonly struct PlayerController
        {
            // struct APlayerController : AController
            public const uint AcknowledgedPawn = 0x4A0; // struct APawn* (Self)
            public const uint CameraManager = 0x4C8; // struct APlayerCameraManager* PlayerCameraManager;
        }

        public readonly struct Actor
        {
            // struct AActor : UObject
            public const uint RootComponent = 0x88; // struct USceneComponent* RootComponent;
        }

        public readonly struct SceneComponent
        {
            public const uint ComponentToWorld = 0x290; // relative_location (unsure, should be +-20 of current value)
            public const uint Translation = 0x10; // Position
        }

        public readonly struct Character
        {
            // struct ACharacter : APawn
            public const uint Mesh = 0x468; // struct USkeletalMeshComponent* Mesh;

            public static readonly uint[] HealthXorKeys = new uint[]
            {
                0xCEC7A52B,
                0x9B63B2E2,
                0xCA9E3951,
                0xE23848A1,
                0x9E911D0A,
                0x23DDE727,
                0x945A1C8,
                0x5151D921,
                0xBE77A58,
                0xEEFE287,
                0xE2755DF8,
                0x7A8ADB1F,
                0xBD1F33D5,
                0xE293A107,
                0x96099E38,
                0x44442A33,
            };

            public const uint HealthIf1 = 0x3A0; // HealthFlag
            public const uint HealthIf = 0xA60; // Health1
            public const uint HealthCheck = 0xA40; // Health4
            public const uint Healthxor = 0xA50; // Health6
            public const uint Healthcmp = 0xA54; // Health3
            public const uint HealthBool = 0xA55; // Health5
            public const uint HealthOffset = 0xA70; // Health2

            // struct ATslCharacterBase : ACharacter
            public const uint GroggyHealth = 0x28F0; // float GroggyHealth;

            public const uint Name = 0xE60;

            // struct ATslCharacter : AMutableCharacter
            public const uint Team = 0xF40; // right after "struct ATeam* Team;"
            public const uint SpectatedCount = 0x2638; // audience (obfuscated int32)
        }

        public readonly struct Mesh
        {
            // struct UPrimitiveComponent : USceneComponent
            // Right after BoundsScale
            public const uint LastRenderTimeOnScreen = 0x740; // 3rd float
            public const uint LastSubmitTime = LastRenderTimeOnScreen - 0x8; // 1st float

            // struct UStaticMeshComponent : UMeshComponent
            public const uint StaticMesh = 0xAB0; // struct UStaticMesh* StaticMesh; (BoneArray)
        }

        public readonly struct CameraManager
        {
            public const uint InfoBase = 0x1610; // Same as Location - Check struct from dump to ensure it's not changed
        }
    }
}
