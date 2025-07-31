namespace apex_dma_esp.Apex
{
    public static class ApexMath
    {
        public struct QAngle
        {
            float X;
            float Y;

            #region Operator Overloads

            public static QAngle operator +(QAngle a, QAngle b)
            {
                return new()
                {
                    X = a.X + b.X,
                    Y = a.Y + b.Y,
                };
            }

            public static QAngle operator -(QAngle a, QAngle b)
            {
                return new()
                {
                    X = a.X - b.X,
                    Y = a.Y - b.Y,
                };
            }

            public static QAngle operator *(QAngle a, float scalar)
            {
                return new()
                {
                    X = a.X * scalar,
                    Y = a.Y * scalar,
                };
            }

            public static QAngle operator /(QAngle a, float scalar)
            {
                return new()
                {
                    X = a.X / scalar,
                    Y = a.Y / scalar,
                };
            }

            #endregion

            public float Dot(QAngle other) {
                return X * other.X + Y * other.Y;
            }

            public float Length()
            {
                return MathF.Sqrt(X * X + Y * Y);
            }

            public float DistanceTo(QAngle other)
            {
                return (other - this).Length();
            }

            public QAngle Normalize()
            {
                float len = Length();
                if (len > 0)
                {
                    X /= len;
                    Y /= len;
                }
                
                return this;
            }

            public QAngle Clamp(float minVal, float maxVal)
            {
                X = X.Clamp(minVal, maxVal);
                Y = Y.Clamp(minVal, maxVal);

                return this;
            }

            public QAngle Lerp(QAngle other, float t)
            {
                return this * (1f - t) + other * t;
            }

            public QAngle FixAngle()
            {
                if (!IsValid())
                    return this;

                while (X > 89f)
                    X -= 180f;

                while (X < -89f)
                    X += 180f;

                while (Y > 180f)
                    Y -= 360f;

                while (Y < -180f)
                    Y += 360f;

                return this;
            }

            public bool IsValid()
            {
                if (float.IsNaN(X) || float.IsInfinity(X) || float.IsNaN(Y) || float.IsInfinity(Y))
                    return false;

                return true;
            }
        }

        public struct Matrix3x4
        {
            public float M11;
            public float M12;
            public float M13;
            public float M14;

            public float M21;
            public float M22;
            public float M23;
            public float M24;

            public float M31;
            public float M32;
            public float M33;
            public float M34;
            
            public Vector3 GetPosition()
            {
		        return new(M14, M24, M34);
            }
        }

        public readonly struct ViewMatrix
        {
            public readonly Matrix4x4 Matrix;

            public Vector3 Transform(Vector3 vector)
            {
                return new()
                {
                    X = vector.Y * Matrix.M12 + vector.X * Matrix.M11 + vector.Z * Matrix.M13 + Matrix.M14,
                    Y = vector.Y * Matrix.M22 + vector.X * Matrix.M21 + vector.Z * Matrix.M23 + Matrix.M24,
                    Z = vector.Y * Matrix.M42 + vector.X * Matrix.M41 + vector.Z * Matrix.M43 + Matrix.M44,
                };
            }
        }

        public static bool IsValid(this Vector3 v)
        {
            if (float.IsNaN(v.X) || float.IsInfinity(v.X) || float.IsNaN(v.Y) || float.IsInfinity(v.Y) || float.IsNaN(v.Z) || float.IsInfinity(v.Z))
                return false;

            return true;
        }

        public static float Clamp(this float value, float min, float max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}
