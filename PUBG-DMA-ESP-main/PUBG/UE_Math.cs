namespace pubg_dma_esp.PUBG
{
    public static class UE_Math
    {
        public const float DEG_TO_RAD = MathF.PI / 180f;
        public const float FOV_DEG_TO_RAD = MathF.PI / 360f;

        public readonly struct FRotator
        {
            public readonly float Pitch;
            public readonly float Yaw;
            public readonly float Roll;

            public readonly override string ToString() => $"({Pitch}, {Yaw}, {Roll})";
        };

        public readonly struct FQuat
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly float W;
        };

        public unsafe struct FTransform
        {
            public readonly FQuat Rot;
            public readonly Vector3 Translation;
            private fixed byte _p1[0x4];
            public readonly Vector3 Scale;

            public readonly Matrix4x4 ToMatrixWithScale()
            {
                Matrix4x4 m;

                m.M41 = Translation.X;
                m.M42 = Translation.Y;
                m.M43 = Translation.Z;

                float x2 = Rot.X + Rot.X;
                float y2 = Rot.Y + Rot.Y;
                float z2 = Rot.Z + Rot.Z;

                float xx2 = Rot.X * x2;
                float yy2 = Rot.Y * y2;
                float zz2 = Rot.Z * z2;
                m.M11 = (1f - (yy2 + zz2)) * Scale.X;
                m.M22 = (1f - (xx2 + zz2)) * Scale.Y;
                m.M33 = (1f - (xx2 + yy2)) * Scale.Z;

                float yz2 = Rot.Y * z2;
                float wx2 = Rot.W * x2;
                m.M32 = (yz2 - wx2) * Scale.Z;
                m.M23 = (yz2 + wx2) * Scale.Y;

                float xy2 = Rot.X * y2;
                float wz2 = Rot.W * z2;
                m.M21 = (xy2 - wz2) * Scale.Y;
                m.M12 = (xy2 + wz2) * Scale.X;

                float xz2 = Rot.X * z2;
                float wy2 = Rot.W * y2;
                m.M31 = (xz2 + wy2) * Scale.Z;
                m.M13 = (xz2 - wy2) * Scale.X;

                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;

                return m;
            }
        };

        public static Matrix4x4 CreateViewMatrix(FRotator rotation, Vector3 origin = new Vector3())
        {
            float radPitch = rotation.Pitch * DEG_TO_RAD;
            float radYaw = rotation.Yaw * DEG_TO_RAD;
            float radRoll = rotation.Roll * DEG_TO_RAD;

            float sinPitch = MathF.Sin(radPitch);
            float sinYaw = MathF.Sin(radYaw);
            float sinRoll = MathF.Sin(radRoll);

            float cosPitch = MathF.Cos(radPitch);
            float cosYaw = MathF.Cos(radYaw);
            float cosRoll = MathF.Cos(radRoll);

            Matrix4x4 m;
            m.M11 = cosPitch * cosYaw;
            m.M12 = cosPitch * sinYaw;
            m.M13 = sinPitch;
            m.M14 = 0f;

            m.M21 = sinRoll * sinPitch * cosYaw - cosRoll * sinYaw;
            m.M22 = sinRoll * sinPitch * sinYaw + cosRoll * cosYaw;
            m.M23 = -sinRoll * cosPitch;
            m.M24 = 0f;

            m.M31 = -cosRoll * sinPitch * cosYaw - sinRoll * sinYaw;
            m.M32 = cosYaw * sinRoll - cosRoll * sinPitch * sinYaw;
            m.M33 = cosRoll * cosPitch;
            m.M34 = 0f;

            m.M41 = origin.X;
            m.M42 = origin.Y;
            m.M43 = origin.Z;
            m.M44 = 1f;

            return m;
        }
    }
}
