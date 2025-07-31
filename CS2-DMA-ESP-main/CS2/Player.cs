namespace cs2_dma_esp.CS2
{
    public class Player
    {
        #region Bone Stuff

        public const int BonesCount = 30;
        public static unsafe readonly uint BoneSize = (uint)sizeof(BoneJointData);

        public static readonly int[] BoneLinkIndices = {
            /// Head
            // Head to neck
            6, // Head
            5, // Neck

            /// Center Mass
            // Neck to stomach
            5, // Neck
            2, // Spine 2
            // Stomach to pelvis
            2, // Spine 2
            0, // Pelvis

            /// Right arm
            // Neck to right shoulder
            5, // Neck
            13, // Right Upperarm
            // Right shoulder to elbow
            13, // Right Upperarm
            14, // Right Lowerarm
            // Right elbow to wrist
            14, // Right Lowerarm
            15, // Right Hand

            /// Left arm
            // Neck to left shoulder
            5, // Neck
            8, // Left Upperarm
            // Left shoulder to elbow
            8, // Left Upperarm
            9, // Left Lowerarm
            // Left elbow to wrist
            9, // Left Lowerarm
            10, // Left Hand
            
            /// Right leg
            // Pelvis to right hip
            0, // Pelvis
            25, // Right Thigh
            // Right hip to calf
            25, // Right Thigh
            26, // Right Calf
            // Right calf to ankle
            26, // Right Calf
            27, // Right Foot

            /// Left leg
            // Pelvis to left hip
            0, // Pelvis
            22, // Left Thigh
            // Left hip to calf
            22, // Left Thigh
            23, // Left Calf
            // Left calf to ankle
            23, // Left Calf
            24, // Left Foot
        };

        public unsafe struct BoneJointData
        {
            public readonly Vector3 Position;
            private fixed byte _p1[0x14];
        }

        #endregion

        #region Fields

        public readonly ulong UpdateIteration;

        public readonly ulong BaseAddress;

        public readonly ulong Pawn;
        public readonly ulong BoneArray;

        public readonly bool IsLocal;
        public readonly int TeamID;

        public int Health;
        public bool IsAlive;
        public Vector3 Position;
        public Vector3[] BonePositions = new Vector3[30];
        public Vector2 ViewAngle;
        public bool IsVisible;

        #endregion

        public Player(ulong baseAddress, ulong pawn, ulong boneArray, int teamID, ulong updateIteration)
        {
            UpdateIteration = updateIteration;

            BaseAddress = baseAddress;

            // Check if this is the LocalPlayer
            if (baseAddress == Game.LocalControllerAddress)
                IsLocal = true;

            Pawn = pawn;

            BoneArray = boneArray;
            TeamID = teamID;
        }
    }
}
