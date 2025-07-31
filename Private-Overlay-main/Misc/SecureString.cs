using System.Runtime.CompilerServices;

namespace Private_Overlay.Misc
{
    public static class SecureStringProvider
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Encrypt(this Span<char> plainStr)
        {
            ReadOnlySpan<char> xorKey = stackalloc char[] { 'N', 'n', '0', 'n', 'Z', 'Q', 't', 'V', 'm', '4', 'R', '7', 'o', 'J', 'f', 'y', '0', 'G', '6', 'p', 'l', '2', 'N', 'X', 'K', 'q', 'a', '8', 'Y', 'y', 'd', 'j' };

            char[] xorStringChars = new char[plainStr.Length];
            for (int i = 0; i < plainStr.Length; i++)
                xorStringChars[i] = (char)(plainStr[i] ^ xorKey[i % xorKey.Length]);

            return new string(xorStringChars);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Decrypt(this string encryptedString)
        {
            ReadOnlySpan<char> xorKey = stackalloc char[] { 'N', 'n', '0', 'n', 'Z', 'Q', 't', 'V', 'm', '4', 'R', '7', 'o', 'J', 'f', 'y', '0', 'G', '6', 'p', 'l', '2', 'N', 'X', 'K', 'q', 'a', '8', 'Y', 'y', 'd', 'j' };

            char[] xorStringChars = new char[encryptedString.Length];
            for (int i = 0; i < encryptedString.Length; i++)
                xorStringChars[i] = (char)(encryptedString[i] ^ xorKey[i % xorKey.Length]);

            return new string(xorStringChars);
        }
    }
}
