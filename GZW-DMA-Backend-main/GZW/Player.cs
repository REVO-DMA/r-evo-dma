using gzw_dma_backend.Misc;

namespace gzw_dma_backend.GZW
{
    public sealed class Player
    {
        #region Bone Stuff

        public static readonly int[] BoneIndices = {
            0, // Root              [0]
            10, // Pelvis           [1]
            11, // Spine 1          [2]
            12, // Spine 2          [3]
            13, // Spine 3          [4]
            16, // Neck             [5]
            18, // Head             [6]
            201, // Left Upperarm   [7]
            202, // Left Lowerarm   [8]
            210, // Left Hand       [9]
            150, // Right Upperarm  [10]
            151, // Right Lowerarm  [11]
            159, // Right Hand      [12]
            251, // Left Thigh      [13]
            252, // Left Calf       [14]
            253, // Left Foot       [15]
            275, // Right Thigh     [16]
            276, // Right Calf      [17]
            277, // Right Foot      [18]
        };
        public static readonly int BonesCount = BoneIndices.Length;
        public static readonly int[] BoneLinkIndices = {
            /// Head
            // Head to neck
            6, // Forehead
            5, // Neck

            /// Center Mass
            // Neck to stomach
            5, // Neck
            4, // Spine 3
            // Stomach to pelvis
            4, // Spine 3
            1, // Pelvis

            /// Right arm
            // Neck to right shoulder
            5, // Neck
            10, // Right Upperarm
            // Right shoulder to elbow
            10, // Right Upperarm
            11, // Right Lowerarm
            // Right elbow to wrist
            11, // Right Lowerarm
            12, // Right Hand

            /// Left arm
            // Neck to left shoulder
            5, // Neck
            7, // Left Upperarm
            // Left shoulder to elbow
            7, // Left Upperarm
            8, // Left Lowerarm
            // Left elbow to wrist
            8, // Left Lowerarm
            9, // Left Hand
            
            /// Right leg
            // Pelvis to right hip
            1, // Pelvis
            16, // Right Thigh
            // Right hip to calf
            16, // Right Thigh
            17, // Right Calf
            // Right calf to ankle
            17, // Right Calf
            18, // Right Foot

            /// Left leg
            // Pelvis to left hip
            1, // Pelvis
            13, // Left Thigh
            // Left hip to calf
            13, // Left Thigh
            14, // Left Calf
            // Left calf to ankle
            14, // Left Calf
            15, // Left Foot
        };

        #endregion

        #region Readonly Fields

        public readonly ulong Base;
        public readonly ulong Mesh;
        public readonly ulong BoneArray;
        public readonly ulong ComponentToWorld;
        public readonly ulong HealthSystemComponent;

        public readonly bool IsLocalPlayer;
        public readonly bool IsAI;
        public readonly byte TeamID;

        #endregion

        #region Dynamic Fields

        public ulong UpdateIteration;

        public Matrix4X4<double> C2W;
        public Vector3D<double>[] BonePositions = new Vector3D<double>[BonesCount];
        public Vector3D<double> Position => BonePositions[0];

        public bool IsDead = false;
        public HealthStatus Health = HealthStatus.Healthy;
        public int Distance = 0;

        #endregion

        #region LocalPlayer Only

        public readonly ulong WeaponSwayComponent;
        public readonly ulong ScopeRenderComponent;
        public readonly ulong CachedMovementComponent;

        public bool IsScoped = false;
        public bool IsADS = false;

        #endregion

        // EMFGOverallHealthStatus
        public enum HealthStatus : byte
        {
            Healthy = 0,
            Injured = 1,
            Critical = 2,
            Coma = 3,
            Dead = 4,
            EMFGOverallHealthStatus_MAX = 5,
        }

        // EMFGCharacterMovementType
        public enum MovementType
        {
            Default = 0,
            Sprinting = 1,
            Sneaking = 2,
            MAX = 3,
        }

        public Player(Actors.PlayerActor actor, ulong updateIteration)
        {
            UpdateIteration = updateIteration;
            
            Base = actor.Address;

            IsLocalPlayer = actor.PlayerState == Manager.LocalPlayerState;

            if (IsLocalPlayer)
            {
                WeaponSwayComponent = Memory.ReadPtr(actor.Address + Offsets.AMFGGameCharacter.WeaponSwayComponent);
                if (!WeaponSwayComponent.Validate())
                {
                    Manager.LocalPlayer = null;
                    throw new Exception($"[PLAYER] -> .ctor(): Invalid WeaponSwayComponent!");
                }

                ScopeRenderComponent = Memory.ReadPtr(actor.Address + Offsets.AMFGGameCharacter.ScopeRenderComponent);
                if (!ScopeRenderComponent.Validate())
                {
                    Manager.LocalPlayer = null;
                    throw new Exception($"[PLAYER] -> .ctor(): Invalid ScopeRenderComponent!");
                }

                CachedMovementComponent = Memory.ReadPtr(actor.Address + Offsets.AMFGGameCharacter.CachedMovementComponent);
                if (!CachedMovementComponent.Validate())
                {
                    Manager.LocalPlayer = null;
                    throw new Exception($"[PLAYER] -> .ctor(): Invalid CachedMovementComponent!");
                }

                Manager.LocalPlayer = this;
            }

            Mesh = actor.Mesh;
            BoneArray = actor.BoneArray;
            ComponentToWorld = Mesh + Offsets.USceneComponent.ComponentToWorld;
            HealthSystemComponent = actor.HealthSystemComponent;

            IsAI = actor.IsAI;
            TeamID = actor.MyTeamID;

            // All players should have a valid state address
            if (!IsAI && !actor.PlayerState.Validate())
                throw new Exception($"[PLAYER] -> .ctor(): Invalid player state!");
        }

        public bool ShouldRender() => IsLocalPlayer == false && IsDead == false;

        public void SetBonePosition(Matrix4X4<double> bone, int index)
        {
            var Matrix = Matrix4X4.Multiply(bone, C2W);

            BonePositions[index] = new(Matrix.M41, Matrix.M42, Matrix.M43);
        }
    }
}
