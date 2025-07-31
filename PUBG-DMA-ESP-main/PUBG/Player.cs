namespace pubg_dma_esp.PUBG
{
    public class Player
    {
        public const float VisionTick = 0.04f; // was 0.06f

        #region Bone Stuff

        public static readonly int[] BoneIndices = {
            0, // Root              [0]
            1, // Pelvis            [1]
            2, // Spine 1           [2]
            3, // Spine 2           [3]
            4, // Spine 3           [4]
            5, // Neck              [5]
            15, // Forehead         [6]
            88, // Left Upperarm    [7]
            89, // Left Lowerarm    [8]
            90, // Left Hand        [9]
            115, // Right Upperarm  [10]
            116, // Right Lowerarm  [11]
            117, // Right Hand      [12]
            172, // Left Thigh      [13]
            173, // Left Calf       [14]
            174, // Left Foot       [15]
            178, // Right Thigh     [16]
            179, // Right Calf      [17]
            180, // Right Foot      [18]
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

        #region Fields

        public ulong UpdateIteration;

        public readonly ulong Base;
        public readonly ulong Mesh;
        public readonly ulong BoneArray;
        public readonly ulong RootComponent;

        // Health
        public ulong HealthAddress {  get; private set; }
        public bool HealthNeedsDecryption { get; private set; }
        public int HealthDecryptionOffset { get; private set; }

        public readonly string Name;
        public readonly int Team;
        public readonly bool IsHuman;
        public readonly bool IsLocalPlayer;

        public Matrix4x4 C2W;
        public Vector3[] BonePositions = new Vector3[BonesCount];
        public Vector3 Position = Vector3.Zero;
        public int Distance;

        public float Health;
        public float GroggyHealth;
        public bool Knocked;

        public bool IsVisible;
        /// <summary>
        /// Whether or not the ESP should render this player.
        /// </summary>
        public bool ShouldRender;

        #endregion

        public Player(ulong baseAddr, ulong rootComponent, ulong mesh, ulong boneArray, string name, int team, bool isHuman, ulong updateIteration = 0)
        {
            // Make sure the localPlayer is always added first
            if (!LocalPlayer.IsValid())
                throw new Exception("Skipping allocation, LocalPlayer not found!");

            if (Offsets.Util.OffsetDebug)
            {
                Logger.WriteLine("PLAYER=======================================");
                Logger.WriteLine($"baseAddr -> 0x{baseAddr:X}");
                Logger.WriteLine($"rootComponent -> 0x{rootComponent:X}");
                Logger.WriteLine($"mesh -> 0x{mesh:X}");
                Logger.WriteLine($"boneArray -> 0x{boneArray:X}");
                Logger.WriteLine($"name -> {name}");
                Logger.WriteLine($"team -> {team}");
                Logger.WriteLine($"isHuman -> {isHuman}");
                Logger.WriteLine("PLAYER=======================================");
            }

            UpdateIteration = updateIteration;

            Base = baseAddr;
            RootComponent = rootComponent;
            Mesh = mesh;
            BoneArray = boneArray;

            Name = name;
            Team = team;

            IsHuman = isHuman;

            GetHealthDetails();

            if (LocalPlayer.AcknowledgedPawn == Base)
                IsLocalPlayer = true;
        }

        public bool SetShouldRender() => ShouldRender = (Health > 0f || GroggyHealth > 0f) && Distance > 0 && Distance <= 600 && Team != LocalPlayer.Team;

        public void SetPosition(Vector3 newPosition) => Position = newPosition;

        public void SetBonePosition(int index, Vector3 newPosition) => BonePositions[index] = newPosition;
        
        public Vector3 GetBonePosition(UE_Math.FTransform bone)
        {
            Matrix4x4 Matrix = Matrix4x4.Multiply(bone.ToMatrixWithScale(), C2W);
            return new Vector3(Matrix.M41, Matrix.M42, Matrix.M43);
        }

        public ulong GetBoneIndexAddress(int index)
        {
            return BoneArray + (uint)(index * 0x30);
        }

        #region Health Decryption

        private void GetHealthDetails()
        {
            if (Memory.ReadValue<byte>(Base + Offsets.Character.HealthIf1) != 3 && Memory.ReadValue<uint>(Base + Offsets.Character.HealthIf) != 0)
            {
                bool v1 = Memory.ReadValue<byte>(Base + Offsets.Character.HealthBool) == 0;
                ulong v2 = Memory.ReadValue<byte>(Base + Offsets.Character.Healthcmp);

                HealthAddress = v2 + Base + Offsets.Character.HealthCheck;

                if (!v1)
                {
                    HealthNeedsDecryption = true;
                    HealthDecryptionOffset = (int)Memory.ReadValue<uint>(Base + Offsets.Character.Healthxor);
                }
                else
                    HealthNeedsDecryption = false;
            }
            else
            {
                HealthAddress = Base + Offsets.Character.HealthOffset;
                HealthNeedsDecryption = false;
            }
        }

        public unsafe float DecryptHealth(Span<byte> value)
        {
            try
            {
                fixed (uint* keysPtr = Offsets.Character.HealthXorKeys)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        byte* keyBytePtr = (byte*)keysPtr + ((i + HealthDecryptionOffset) & 0x3F);
                        value[i] ^= *keyBytePtr;
                    }
                }

                return BitConverter.ToSingle(value);
            }
            catch { return 100f; }
        }

        #endregion
    }
}
