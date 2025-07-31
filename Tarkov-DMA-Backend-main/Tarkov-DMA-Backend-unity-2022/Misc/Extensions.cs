using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.Misc
{
    public static class Extensions
    {
        #region Generic

        /// <summary>
        /// Converts 'Degrees' to 'Radians'.
        /// </summary>
        public static float ToRadians(this float degrees)
        {
            return (MathF.PI / 180f) * degrees;
        }
        /// <summary>
        /// Converts 'Radians' to 'Degrees'.
        /// </summary>
        public static float ToDegrees(this float radians)
        {
            return (180f / MathF.PI) * radians;
        }

        #endregion

        #region UI

        public static MapPosition ToMapPos(this Vector3 vector, JSON.MapConfig map)
        {
            float tmpX = map.X + (vector.X * map.Scale);
            float tmpY = map.Y - (vector.Z * map.Scale);

            float radians = map.Rotation.ToRadians();

            return new()
            {
                X = tmpX * MathF.Cos(radians) - tmpY * MathF.Sin(radians),
                Y = tmpX * MathF.Sin(radians) + tmpY * MathF.Cos(radians),
                Height = vector.Y
            };
        }

        #endregion

        #region nint

        public static string ToManagedString(this nint stringPtr)
        {
            string managedString = Marshal.PtrToStringAnsi(stringPtr);
            Marshal.FreeHGlobal(stringPtr);

            return managedString;
        }

        #endregion

        #region Quaternion

        public static Vector3 ToEuler(this Quaternion q)
        {
            Vector3 res = new();

            double sinr_cosp = +2.0 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = +1.0 - 2.0 * (q.X * q.X + q.Y * q.Y);
            res.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            double sinp = +2.0 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                res.Y = (float)Math.PI / 2 * Math.Sign(sinp);
            else
                res.Y = (float)Math.Asin(sinp);

            double siny_cosp = +2.0 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = +1.0 - 2.0 * (q.Y * q.Y + q.Z * q.Z);
            res.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return res;
        }

        #endregion

        #region byte[]

        public static uint FindSignature(this byte[] data, in byte[] signature, in string mask = null)
        {
            if (data == null || signature == null || data.Length < signature.Length)
                return uint.MaxValue;

            if (mask == null)
            {
                for (int i = 0; i <= data.Length - signature.Length; i++)
                {
                    bool isMatch = true;
                    for (int j = 0; j < signature.Length; j++)
                    {
                        if (data[i + j] != signature[j])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                        return (uint)i;
                }
            }
            else
            {
                if (signature.Length != mask.Length)
                    throw new Exception("[FIND SIGNATURE] Invalid mask length! Make sure mask is the same length as the signature.");

                for (int i = 0; i <= data.Length - signature.Length; i++)
                {
                    bool isMatch = true;
                    for (int j = 0; j < signature.Length; j++)
                    {
                        if (mask[j] == 'x' && data[i + j] != signature[j])
                        {
                            isMatch = false;
                            break;
                        }
                        else if (mask[j] != 'x' && mask[j] != '?')
                            throw new Exception("[FIND SIGNATURE] Invalid character in mask! Use \"x\" for match and \"?\" for wildcard.");
                    }

                    if (isMatch)
                        return (uint)i;
                }
            }

            return uint.MaxValue; // Not found
        }

        public static bool SignatureExists(this byte[] data, in byte[] signature, string mask = null)
        {
            if (data.FindSignature(signature, mask) != uint.MaxValue)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Merges a patch into the byte[] with an optional positive or negative offset.
        /// </summary>
        public static byte[] Patch(this byte[] data, in byte[] patch, int offset = 0x0)
        {
            // Determine the start index of the patch in the merged array
            int startIndex = offset < 0 ? 0 : offset;
            // Determine additional space needed at the beginning for a negative offset
            int prependedSpace = offset < 0 ? Math.Abs(offset) : 0;
            // Final length of the merged array
            int finalLength = Math.Max(data.Length + prependedSpace, startIndex + patch.Length);

            byte[] patchedArray = new byte[finalLength];

            // Copy the original data into the mergedArray, adjusted for any prepended space due to a negative offset
            Array.Copy(data, 0, patchedArray, prependedSpace, data.Length);

            // Merge the patch
            for (int i = 0; i < patch.Length; i++)
            {
                int index = startIndex + i;
                if (index < patchedArray.Length)
                    patchedArray[index] = patch[i];
            }

            return patchedArray;
        }

        #endregion

        #region Span<byte>

        public static int FindUtf16NullTerminatorIndex(this Span<byte> buffer)
        {
            for (int i = 0; i < buffer.Length - 1; i += 2)
            {
                if (buffer[i] == 0x0 && buffer[i + 1] == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region Validation

        /// <summary>
        /// Custom implemenation to check if a float value is valid.
        /// This is the same as float.IsNormal() except it accepts 0 as a valid value.
        /// </summary>
        /// <param name="f">Float value to validate.</param>
        /// <returns>True if valid, otherwise False if invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this float f)
        {
            int bits = BitConverter.SingleToInt32Bits(f);
            bits &= 0x7FFFFFFF; // Clears the sign bit
                                // Checks if the bits represent a non-infinite number (less than 0x7F800000) and
                                // either a normal number ((bits & 0x7F800000) != 0) or 0f/-0f (which is covered by allowing bits == 0)
            return (bits < 0x7F800000) && ((bits & 0x7F800000) != 0 || bits == 0);
        }

        /// <summary>
        /// Validates a Quaternion for invalid values.
        /// </summary>
        /// <param name="quaternion">Input Quaternion.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Validate(this Quaternion quaternion)
        {
            if (quaternion == Quaternion.Zero ||
                !quaternion.W.IsValid() ||
                !quaternion.X.IsValid() ||
                !quaternion.Y.IsValid() ||
                !quaternion.Z.IsValid())
                throw new ArgumentOutOfRangeException(nameof(quaternion));
        }

        /// <summary>
        /// Validates a Vector3 for invalid values.
        /// </summary>
        /// <param name="vector3">Input Vector3.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Validate(this Vector3 vector3)
        {
            if (vector3 == Vector3.Zero ||
                !vector3.X.IsValid() ||
                !vector3.Y.IsValid() ||
                !vector3.Z.IsValid())
                throw new ArgumentOutOfRangeException(nameof(vector3));
        }

        /// <summary>
        /// Validates a Vector2 for invalid values.
        /// </summary>
        /// <param name="vector2">Input Vector2.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Validate(this Vector2 vector2)
        {
            if (vector2 == Vector2.Zero ||
                !vector2.X.IsValid() ||
                !vector2.Y.IsValid())
                throw new ArgumentOutOfRangeException(nameof(vector2));
        }

        #endregion
    }
}
