namespace arena_dma_backend.Misc
{
    public static class Extensions
    {
        #region Generic

        /// <summary>
        /// Converts 'Degrees' to 'Radians'.
        /// </summary>
        public static double ToRadians(this float degrees)
        {
            return (Math.PI / 180) * degrees;
        }

        /// <summary>
        /// Converts 'Radians' to 'Degrees'.
        /// </summary>
        public static double ToDegrees(this double radians)
        {
            return (180 / Math.PI) * radians;
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

        #region Vectors

        public static bool IsNormal(this Vector2 vector)
        {
            if (vector.X.IsNormal() && vector.Y.IsNormal())
                return true;

            return false;
        }

        public static bool IsNormal(this Vector3 vector)
        {
            if (vector.X.IsNormal() && vector.Y.IsNormal() && vector.Y.IsNormal())
                return true;

            return false;
        }

        #endregion

        #region Floats

        public static bool IsNormal(this float f)
        {
            int bits = BitConverter.SingleToInt32Bits(f);
            bits &= 0x7FFFFFFF;
            return (bits < 0x7F800000) && ((bits & 0x7F800000) != 0);
        }

        #endregion
    }
}
