namespace gzw_dma_backend.GZW
{
    public static class UE_Math
    {
        public const double DEG_TO_RAD = Math.PI / 180d;
        public const double FOV_DEG_TO_RAD = Math.PI / 360d;

        public const float F_DEG_TO_RAD = MathF.PI / 180f;
        public const float F_FOV_DEG_TO_RAD = MathF.PI / 360f;

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public readonly struct FTransform
        {
            [FieldOffset(0x0)]
            public readonly Vector4D<double> Rot;
            [FieldOffset(0x20)]
            public readonly Vector3D<double> Translation;
            [FieldOffset(0x40)]
            public readonly Vector3D<double> Scale;

            public readonly Matrix4X4<double> ToMatrixWithScale()
            {
                Matrix4X4<double> m = new();

                m.M41 = Translation.X;
                m.M42 = Translation.Y;
                m.M43 = Translation.Z;

                double x2 = Rot.X + Rot.X;
                double y2 = Rot.Y + Rot.Y;
                double z2 = Rot.Z + Rot.Z;

                double xx2 = Rot.X * x2;
                double yy2 = Rot.Y * y2;
                double zz2 = Rot.Z * z2;
                m.M11 = (1d - (yy2 + zz2)) * Scale.X;
                m.M22 = (1d - (xx2 + zz2)) * Scale.Y;
                m.M33 = (1d - (xx2 + yy2)) * Scale.Z;

                double yz2 = Rot.Y * z2;
                double wx2 = Rot.W * x2;
                m.M32 = (yz2 - wx2) * Scale.Z;
                m.M23 = (yz2 + wx2) * Scale.Y;

                double xy2 = Rot.X * y2;
                double wz2 = Rot.W * z2;
                m.M21 = (xy2 - wz2) * Scale.Y;
                m.M12 = (xy2 + wz2) * Scale.X;

                double xz2 = Rot.X * z2;
                double wy2 = Rot.W * y2;
                m.M31 = (xz2 + wy2) * Scale.Z;
                m.M13 = (xz2 - wy2) * Scale.X;

                m.M14 = 0d;
                m.M24 = 0d;
                m.M34 = 0d;
                m.M44 = 1d;

                return m;
            }
        };

        public static Matrix4X4<double> CreateViewMatrix(Vector3D<double> rotation, Vector3D<double> origin = new())
        {
            double radPitch = rotation.X * DEG_TO_RAD;
            double radYaw = rotation.Y * DEG_TO_RAD;
            double radRoll = rotation.Z * DEG_TO_RAD;

            double sinPitch = Math.Sin(radPitch);
            double sinYaw = Math.Sin(radYaw);
            double sinRoll = Math.Sin(radRoll);

            double cosPitch = Math.Cos(radPitch);
            double cosYaw = Math.Cos(radYaw);
            double cosRoll = Math.Cos(radRoll);

            Matrix4X4<double> m = new();

            m.M11 = cosPitch * cosYaw;
            m.M12 = cosPitch * sinYaw;
            m.M13 = sinPitch;
            m.M14 = 0d;

            m.M21 = sinRoll * sinPitch * cosYaw - cosRoll * sinYaw;
            m.M22 = sinRoll * sinPitch * sinYaw + cosRoll * cosYaw;
            m.M23 = -sinRoll * cosPitch;
            m.M24 = 0d;

            m.M31 = -cosRoll * sinPitch * cosYaw - sinRoll * sinYaw;
            m.M32 = cosYaw * sinRoll - cosRoll * sinPitch * sinYaw;
            m.M33 = cosRoll * cosPitch;
            m.M34 = 0d;

            m.M41 = origin.X;
            m.M42 = origin.Y;
            m.M43 = origin.Z;
            m.M44 = 1d;

            return m;
        }
    }
}
