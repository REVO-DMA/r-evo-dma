using static apex_dma_esp.Apex.ApexMath;

namespace apex_dma_esp.Apex
{
    public class Player
    {
        #region Bone Stuff

        public static readonly uint[] BoneIndices = {
            0, // Head              [0]
            1, // Neck              [1]

            2, // Chest             [2]
            3, // Stomach           [3]
            4, // Pelvis            [4]

            6, // Left Shoulder     [5]
            7, // Left Elbow        [6]
            8, // Left Hand         [7]

            9, // Right Shoulder    [8]
            10, // Right Elbow      [9]
            11, // Right Hand       [10]

            12, // Left Hip         [11]
            13, // Left Knee        [12]
            14, // Left Foot        [13]

            16, // Right Hip        [14]
            17, // Right Knee       [15]
            18, // Right Foot       [16]
        };
        public static readonly int BonesCount = BoneIndices.Length;
        public static readonly int[] BoneLinkIndices = {
            /// Head
            // Head to Neck
            0, // Head
            1, // Neck

            /// Center Mass
            // Neck to Chest
            1, // Neck
            2, // Chest
            // Chest to Stomach
            2, // Chest
            3, // Stomach
            // Stomach to Pelvis
            3, // Stomach
            4, // Pelvis

            /// Right arm
            // Neck to Shoulder
            1, // Neck
            8, // Shoulder
            // Shoulder to Elbow
            8, // Shoulder
            9, // Elbow
            // Elbow to Hand
            9, // Elbow
            10, // Hand

            /// Left arm
            // Neck to Shoulder
            1, // Neck
            5, // Shoulder
            // Shoulder to Elbow
            5, // Shoulder
            6, // Elbow
            // Elbow to Hand
            6, // Elbow
            7, // Hand
            
            /// Right leg
            // Pelvis to Hip
            4, // Pelvis
            14, // Hip
            // Hip to Knee
            14, // Thigh
            15, // Knee
            // Knee to Foot
            15, // Knee
            16, // Foot

            /// Left leg
            // Pelvis to Hip
            4, // Pelvis
            11, // Hip
            // Hip to Knee
            11, // Hip
            12, // Knee
            // Knee to ankle
            12, // Knee
            13, // Foot
        };

        #endregion

        private readonly ushort[] DynamicBoneIndexes = new ushort[BonesCount];
        public Vector3[] BonePositions = new Vector3[BonesCount];

        public ulong UpdateIteration;

        public readonly ulong _Base;

        private readonly ulong BoneArray;

        public readonly string Name;
        public readonly int Team;

        public bool IsDead;
        public bool IsKnocked;

        public Vector3 LocalOrigin;
        public Vector3 AbsoluteVelocity;

        public int Health;
        public int MaxHealth;
        public int Shield;
        public int MaxShield;

        public int LastTimeAimedAtPrevious;
        public bool IsAimedAt;

        public int LastTimeVisiblePrevious;
        public bool IsVisible;

        public bool IsLocal;
        public bool IsAlly;
        public bool IsHostile;

        public bool IsLockedOn;

        public Player(ulong baseAddr, string name, int team, ulong boneArray, ushort[] dynamicBoneIndexes, ulong updateIteration)
        {
            _Base = baseAddr;

            BoneArray = boneArray;

            DynamicBoneIndexes = dynamicBoneIndexes;

            Name = name;
            Team = team;

            Health = Memory.ReadValue<int>(_Base + Offsets.HEALTH);

            UpdateIteration = updateIteration;

            if (Manager.LocalPlayer.IsValid())
            {
                IsLocal = Manager.LocalPlayer._Base == _Base;
                IsAlly = Manager.LocalPlayer.Team == Team;
                IsHostile = !IsAlly;
            }

            if (!IsPlayer() && !IsDummy())
                throw new Exception("[PLAYER] Error during allocation -> Unknown player type!");
        }

        public static bool IsDifferent(in Player p1, in Player p2)
        {
            if (p1.Name != p2.Name ||
                p1.Team != p2.Team)
            {
                return true;
            }

            return false;
        }

        public bool IsValid()
        {
            return Health > 0;
        }

        public bool IsCombatReady()
        {
            if (!IsValid()) return false;
            if (IsDummy()) return true;
            if (IsDead) return false;
            if (IsKnocked) return false;
            return true;
        }

        public bool IsPlayer()
        {
            return Name == "player";
        }

        public bool IsDummy()
        {
            return Team == 97;
        }

        public ulong GetBoneIndexAddress(uint index)
        {
            return BoneArray + (uint)(DynamicBoneIndexes[index] * 48); // 48 = sizeof(Matrix3x4)
        }

        public Vector3 GetBonePosition(Matrix3x4 boneMatrix)
        {
            Vector3 BonePosition = boneMatrix.GetPosition();

            if (!BonePosition.IsValid())
                return LocalOrigin;

            BonePosition += LocalOrigin;

            return BonePosition;
        }
    }
}
