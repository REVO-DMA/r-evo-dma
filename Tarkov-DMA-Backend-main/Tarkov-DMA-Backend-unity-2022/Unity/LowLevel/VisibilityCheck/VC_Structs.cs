using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck
{
    public static class VC_Structs
    {
        public const int PATCH_SIZE = 16;
        public const int MAX_PLAYER_COUNT = 80;
        public const int BONE_COUNT = 6;

        public const string TRAMPOLINE_METHOD_NAME = "_code";
        public const string EXECUTOR_METHOD_NAME = "_hook";

        enum BoneNames
        {
            Head = 133,
            CenterMass = 36,
            LeftShoulder = 90,
            RightShoulder = 111,
            LeftKnee = 17,
            RightKnee = 22
        };

        private static readonly int[] CheckBones = new int[BONE_COUNT] {
            (int)BoneNames.Head,
            (int)BoneNames.CenterMass,
            (int)BoneNames.LeftShoulder,
            (int)BoneNames.RightShoulder,
            (int)BoneNames.LeftKnee,
            (int)BoneNames.RightKnee
        };

        private static readonly byte[] OriginalBytes = new byte[PATCH_SIZE]
        {
            0x55,                   // push rbp
            0x48, 0x8B, 0xEC,       // mov rbp,rsp
            0x48, 0x83, 0xEC, 0x50, // sub rsp,50
            0x48, 0x89, 0x75, 0xD0, // mov[rbp - 30],rsi
            0x48, 0x89, 0x7D, 0xD8, // mov[rbp - 28],rdi
        };

        public static byte[] GetPatchBytes(ulong trampoline)
        {
            byte[] b = BitConverter.GetBytes(trampoline);

            return new byte[PATCH_SIZE]
            {
                0x49, 0xBB, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], // mov r11,0xTRAMPOLINE
                0x4D, 0x8D, 0x3B,                                           // lea r15,[r11]
                0x41, 0xFF, 0xE7,                                           // jmp r15
            };
        }

        public readonly struct InitData(ulong shellCodeDataStruct, ulong codeCave, ulong gameWorldUpdate, ulong unityLinecast, ulong set_POV, ulong adjustShotVectors)
        {
            public readonly ulong shellCodeDataStruct = shellCodeDataStruct;
            public readonly ulong CodeCave = codeCave;
            public readonly ulong GameWorldUpdate = gameWorldUpdate;
            public readonly ulong UnityLinecast = unityLinecast;
            public readonly ulong Set_POV = set_POV;
            public readonly ulong AdjustShotVectors = adjustShotVectors;
        }

        public readonly struct ShellCodeLocation(ulong trampoline, ulong executor)
        {
            public readonly ulong Trampoline = trampoline;
            public readonly ulong Executor = executor;
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Vector3
        {
            public float x, y, z;

            /// <summary>
            /// Converts this custom type to the System.Numerics type.
            /// </summary>
            public static System.Numerics.Vector3 ToSystem(Vector3 v)
            {
                return new(v.x, v.y, v.z);
            }
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Vector2
        {
            public float x, y;

            /// <summary>
            /// Converts this custom type to the System.Numerics type.
            /// </summary>
            public static System.Numerics.Vector2 ToSystem(Vector2 v)
            {
                return new(v.x, v.y);
            }
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct Matrix4x4
        {
            public float M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44;

            /// <summary>
            /// Converts this custom type to the System.Numerics type.
            /// </summary>
            public static System.Numerics.Matrix4x4 ToSystem(Matrix4x4 m)
            {
                return new(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
            }
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct RaycastHit
        {
            Vector3 point;
            Vector3 normal;
            uint faceId;
            float distance;
            Vector2 uv;
            uint colliderId;
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct AABB
        {
            public Vector3 Center;
            public Vector3 Extent;
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct ESPData
        {
            public byte IsScoped;
            public Vector3 LensPosition;
            public Matrix4x4 ScopeMatrix;
            public float ScopeFOV;
            public Matrix4x4 CameraMatrix;
            public AABB AABB;

            // Weapon Stuff
            public Vector3 firePortPos;
            public Vector3 firePortDirection;
            public Vector3 firePortIntersectionPoint;
            public float intersectionTestDistance;
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public unsafe struct ShellCodeData
        {
            public bool shellcodeActive;

            public ulong linecastFunction;
            public ulong transform_getPosition;
            public ulong transform_setPosition;
            public ulong transform_transformDirection;
            public ulong eft_player_set_PointOfView;
            public ulong eft_player_firearmController_adjustShotVectors;
            public ulong executorFunction;

            // TPP Stuff
            public byte tpp_currentState;
            public byte tpp_enabled;
            public float tpp_horizontalDistance;
            public float tpp_horizontalOffset;
            public float tpp_verticalDistance;

            // ESP Data Stuff
            public ulong ocmContainer;
            public ESPData espData;

            public VC_Offsets offsets;

            public Vector3 rayStart;
            public Vector3 tmpVec3;
            public RaycastHit tmpRaycastHit;
            public int highPolyWithTerrainMask;
            public int hitMask;
            public fixed int checkBones[BONE_COUNT];

            public ulong gameWorld_Update;
            public fixed byte originalBytes[PATCH_SIZE];
            public fixed byte patchBytes[PATCH_SIZE];

            public fixed byte visiblePlayers[MAX_PLAYER_COUNT * BONE_COUNT];

            /// <summary>
            /// Creates an instance of this struct pre-populated with all of the correct data for the shellcode.
            /// </summary>
            public static Result<ShellCodeData> Get(InitData initData, ShellCodeLocation shellCodeLocation)
            {
                try
                {
                    ShellCodeData shellCodeData = new();
                    shellCodeData.linecastFunction = initData.UnityLinecast;
                    
                    shellCodeData.transform_getPosition = Memory.UnityPlayerModuleBase + 0x6779B0;
                    shellCodeData.transform_setPosition = Memory.UnityPlayerModuleBase + 0x676EA0;
                    shellCodeData.transform_transformDirection = Memory.UnityPlayerModuleBase + 0x678210;

                    shellCodeData.eft_player_set_PointOfView = initData.Set_POV;
                    shellCodeData.eft_player_firearmController_adjustShotVectors = initData.AdjustShotVectors;

                    //// Set HighPolyWithTerrainMask layer mask
                    //{
                    //    ulong layerManager = EFTDMA.MonoClasses.GetStaticClass(ClassNames.LayerManager.ClassName_ClassToken);
                    //    var layerMaskRaw = Memory.ReadValueEnsure<int>(layerManager + Offsets.LayerManager.HighPolyWithTerrainMask + Offsets.LayerMask.m_Mask);
                    //    if (layerMaskRaw is int layerMask && layerMask > 0x0)
                    //    {
                    //        Logger.WriteLine($"[VC STRUCTS] -> ShellCodeData::Get(): HighPolyWithTerrainMask layer mask ~ 0x{layerMask:X}");
                    //        shellCodeData.highPolyWithTerrainMask = layerMask;
                    //    }
                    //    else
                    //        throw new Exception("Failed to get layer mask: HighPolyWithTerrainMask");
                    //}
                    shellCodeData.highPolyWithTerrainMask = 0x1800;

                    //// Set HitMask layer mask
                    //{
                    //    ulong layerManager = EFTDMA.MonoClasses.GetStaticClass(ClassNames.BallisticLayerManager.ClassName_ClassToken);
                    //    var layerMaskRaw = Memory.ReadValueEnsure<int>(layerManager + Offsets.BallisticLayerManager.HitMask + Offsets.LayerMask.m_Mask);
                    //    if (layerMaskRaw is int layerMask && layerMask > 0x0)
                    //    {
                    //        Logger.WriteLine($"[VC STRUCTS] -> ShellCodeData::Get(): HitMask layer mask ~ 0x{layerMask:X}");
                    //        shellCodeData.hitMask = layerMask;
                    //    }
                    //    else
                    //        throw new Exception("Failed to get layer mask: HitMask");
                    //}
                    shellCodeData.hitMask = 0x40811810;

                    shellCodeData.executorFunction = shellCodeLocation.Executor;
                    shellCodeData.ocmContainer = EFTDMA.MonoClasses.GetStaticClass(ClassNames.OpticCameraManagerContainer.ClassName_ClassToken);
                    shellCodeData.espData.intersectionTestDistance = 9999f;
                    fixed (int* pCheckBones = CheckBones) // checkBones
                    {
                        const int size = sizeof(int) * BONE_COUNT;
                        Buffer.MemoryCopy(pCheckBones, shellCodeData.checkBones, size, size);
                    }
                    shellCodeData.gameWorld_Update = initData.GameWorldUpdate;
                    fixed (byte* pOriginalBytes = OriginalBytes) // originalBytes
                    {
                        Buffer.MemoryCopy(pOriginalBytes, shellCodeData.originalBytes, PATCH_SIZE, PATCH_SIZE);
                    }
                    byte[] patchBytes = GetPatchBytes(shellCodeLocation.Trampoline); // patchBytes
                    fixed (byte* pPatchBytes = patchBytes)
                    {
                        Buffer.MemoryCopy(pPatchBytes, shellCodeData.patchBytes, PATCH_SIZE, PATCH_SIZE);
                    }

                    return new(true, shellCodeData);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[VC STRUCTS] -> ShellCodeData::Get(): {ex}");
                    SentrySdk.CaptureException(ex);
                    return Result<ShellCodeData>.Fail;
                }
            }

            /// <summary>
            /// Allocates memory in the game process for this struct.
            /// </summary>
            public static ulong Allocate()
            {
                try
                {
                    ulong address = NativeHelper.AllocBytes((uint)sizeof(ShellCodeData) * 2);
                    address = MemoryUtils.AlignAddress(address);
                    if (!MemoryUtils.IsValidAddress(address))
                        throw new Exception("Bytes were allocated at an invalid address");

                    return address;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[VC STRUCTS] -> ShellCodeData::Allocate(): Failed to allocate memory for ShellCodeData struct ~ {ex}");
                    return 0x0;
                }
            }

            /// <summary>
            /// Writes an instance of this struct to the game's memory.
            /// </summary>
            public static bool Write(ulong address, ShellCodeData data)
            {
                try
                {
                    Memory.WriteValue(address, data);

                    return true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[VC STRUCTS] -> ShellCodeData::Write(): Failed to write ShellCodeData ~ {ex}");
                    return false;
                }
            }

            /// <summary>
            /// Writes to a field of this struct that is allocated in the game's memory.
            /// </summary>
            public static void WriteValue<T>(ulong address, string fieldName, T value) where T : struct
            {
                Memory.WriteValue(address + offset<ShellCodeData>.Of(fieldName), value);
            }

            /// <summary>
            /// Reads from a field of this struct that is allocated in the game's memory.
            /// </summary>
            public static T ReadValue<T>(ulong address, string fieldName) where T : struct
            {
                return Memory.ReadValue<T>(address + offset<ShellCodeData>.Of(fieldName), false);
            }
        }

        /// <summary>
        /// Has a native counterpart. Ensure they are kept in sync.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public readonly struct VC_Offsets(uint playerBody)
        {
            // List
            public readonly uint ListBase = UnityOffsets.UnityList.Base;
            public readonly uint ListCount = UnityOffsets.UnityList.Count;
            public readonly uint ListFirstElement = UnityOffsets.UnityListBase.Start;

            // Player
            public readonly uint MovementContext = Offsets.Player.MovementContext;
            public readonly uint PlayerBodyClientPlayer = Offsets.Player._playerBody;
            public readonly uint AIDataClientPlayer = Offsets.Player.AIData;
            public readonly uint IsAIClientPlayer = Offsets.AIData.IsAI;
            public readonly uint IsAI = Offsets.ObservedPlayerView.IsAI;
            public readonly uint PlayerBody = playerBody;
            public readonly uint SkeletonValues = Offsets.Skeleton._values;
            public readonly uint ProceduralWeaponAnimation = Offsets.Player.ProceduralWeaponAnimation;
            public readonly uint HandsController = Offsets.Player._handsController;

            // Player Body
            public readonly uint SkeletonRootJoint = Offsets.PlayerBody.SkeletonRootJoint;
            public readonly uint PointOfView = Offsets.PlayerBody.PointOfView;

            // Point Of View
            public readonly uint POV = Offsets.PointOfView.POV;

            // Movement CTX
            public readonly uint LookDirection = Offsets.MovementContext._lookDirection;

            // Firearm Controller
            public readonly uint FirePort = Offsets.FirearmController.Fireport;

            // PWA
            public readonly uint HandsContainer = Offsets.ProceduralWeaponAnimation.HandsContainer;

            // PlayerSpring
            public readonly uint CameraTransform = Offsets.PlayerSpring.CameraTransform;

            // GameWorld
            public readonly uint RegisteredPlayers = Offsets.ClientLocalGameWorld.RegisteredPlayers;
            public readonly uint MainPlayer = Offsets.ClientLocalGameWorld.MainPlayer;

            // OpticCameraManagerContainer
            public readonly uint OCMContainerInstance = Offsets.OpticCameraManagerContainer.Instance;
            public readonly uint OCM = Offsets.OpticCameraManagerContainer.OpticCameraManager;
            public readonly uint FPSCamera = Offsets.OpticCameraManagerContainer.FPSCamera;

            // OpticCameraManager
            public readonly uint OCMCamera = Offsets.OpticCameraManager.Camera;
            public readonly uint CurrentOpticSight = Offsets.OpticCameraManager.CurrentOpticSight;

            // OpticSight
            public readonly uint LensRenderer = Offsets.OpticSight.LensRenderer;

            // Unity Renderer
            public readonly uint LocalAABB = UnityOffsets.Component.Size + UnityOffsets.BaseRenderer.RendererData + UnityOffsets.SharedRendererData.TransformInfo + UnityOffsets.TransformInfo.LocalAABB;

            // Unity Camera
            public readonly uint ViewMatrix = UnityOffsets.Camera.ViewMatrix;
            public readonly uint FOV = UnityOffsets.Camera.FOV;
            public readonly uint LastPosition = UnityOffsets.Camera.LastPosition;

            // Unity Behaviour
            public readonly uint IsAdded = UnityOffsets.Behaviour.IsAdded;
        };
    }
}
