using dnlib.DotNet;
using System.Text;
using System.Text.RegularExpressions;

namespace TarkovDumper
{
    public static class TextHelper
    {
        private static string UTF8ToUTF16(string name)
        {
            if (name == null || name.Length <= 0)
                return null;

            // Exit early if this name is not obfuscated
            if (Encoding.UTF8.GetBytes(name)[0] < 0xE0)
                return name;

            Span<byte> utf16Bytes = stackalloc byte[2 * name.Length];

            var byteCount = Encoding.Unicode.GetBytes(name.AsSpan(), utf16Bytes);
            if (byteCount < 2)
                throw new ArgumentException("Input string is too short.", nameof(name));

            return $"\\u{BitConverter.ToUInt16(utf16Bytes):X4}";
        }

        private static string UTF8ToUTF16_Alt(string name)
        {
            if (name == null || name.Length <= 0)
                return null;

            int startIndex = -1;
            Span<byte> nameBytes = Encoding.UTF8.GetBytes(name);
            for (int i = 0; i < nameBytes.Length; i++)
            {
                if (nameBytes[i] >= 0xE0)
                {
                    startIndex = i;
                    break;
                }
            }

            // Exit early if this name is not obfuscated at all
            if (startIndex == -1)
                return name;

            ReadOnlySpan<char> nameSpan = name.AsSpan().Slice(startIndex);
            Span<byte> utf16Bytes = stackalloc byte[2 * nameSpan.Length];

            var byteCount = Encoding.Unicode.GetBytes(nameSpan, utf16Bytes);
            if (byteCount < 2)
                throw new Exception("The supplied name is too short");

            return $"{name.Substring(0, startIndex)}\\u{BitConverter.ToUInt16(utf16Bytes):X4}";
        }

        private static string GetFullName(string fullName)
        {
            const string additionalHelpInfo = "Maybe you don't need the full name. Set \"outputFullName\" to false and try again!";

            // Exit early if this name is obfuscated
            if (Encoding.UTF8.GetBytes(fullName)[0] == '\\')
                throw new ArgumentException($"fullName: \"{fullName}\" is obfuscated! {additionalHelpInfo}");

            if (!fullName.Contains('/'))
            {
                if (!fullName.Contains('.'))
                    throw new ArgumentException($"fullName: \"{fullName}\" does not contain a \"/\" char! {additionalHelpInfo}");
                else
                    return fullName;
            }

            string[] splitName = fullName.Split('/');

            return $"{splitName[0]}+{splitName[1]}";
        }

        public static string Humanize(this TypeDef typeDef, bool fullName = false)
        {
            if (typeDef == null)
                return null;

            if (fullName) return GetFullName(UTF8ToUTF16(typeDef.FullName));
            else return UTF8ToUTF16(typeDef.Name);
        }

        public static string HumanizeAlt(this TypeDef typeDef, bool fullName = false)
        {
            if (typeDef == null)
                return null;

            if (fullName) return GetFullName(UTF8ToUTF16_Alt(typeDef.FullName));
            else return UTF8ToUTF16_Alt(typeDef.Name);
        }

        public static string Humanize(this MethodDef typeDef, bool fullName = false)
        {
            if (typeDef == null)
                return null;

            if (fullName) return UTF8ToUTF16(typeDef.FullName).Replace('/', '.');
            else return UTF8ToUTF16(typeDef.Name);
        }

        public static string HumanizeAlt(this MethodDef typeDef)
        {
            if (typeDef == null)
                return null;

            return UTF8ToUTF16_Alt(typeDef.Name);
        }

        public static string Humanize(this FieldDef typeDef, bool fullName = false)
        {
            if (typeDef == null)
                return null;

            if (fullName) return UTF8ToUTF16(typeDef.FullName).Replace('/', '.');
            else return UTF8ToUTF16(typeDef.Name);
        }

        public static string HumanizeAlt(this FieldDef typeDef)
        {
            if (typeDef == null)
                return null;

            return UTF8ToUTF16_Alt(typeDef.Name);
        }

        public static string Humanize(this string name)
        {
            return UTF8ToUTF16(name);
        }

        public static string GetTypeName(this FieldDef typeDef)
        {
            if (typeDef == null)
                return null;

            return typeDef.FieldType.GetName().Humanize();
        }

        public static string GetFieldName(this FieldDef typeDef)
        {
            if (typeDef == null)
                return null;

            return typeDef.Name.ToString().Humanize();
        }

        /// <summary>
        /// Finds a substring then goes backwards until white space is found.
        /// </summary>
        public static string FindSubstringAndGoBackwards(string input, string substring, int startIndex = -1)
        {
            if (startIndex == -1)
                startIndex = input.IndexOf(substring);

            if (startIndex == -1)
                return null;

            int backwardIndex = startIndex;

            while (backwardIndex > 0 && !char.IsWhiteSpace(input[backwardIndex - 1]))
            {
                backwardIndex--;
            }

            return input.Substring(backwardIndex, startIndex - backwardIndex).Trim();
        }

        /// <summary>
        /// Finds a substring then goes backwards until the searchChar is reached.
        /// </summary>
        public static string FindSubstringAndGoBackwards(string input, string substring, char searchChar, int startIndex = -1)
        {
            if (startIndex == -1)
                startIndex = input.IndexOf(substring);

            if (startIndex == -1)
                return null;

            int backwardIndex = startIndex;

            while (backwardIndex > 0 && input[backwardIndex - 1] != searchChar)
            {
                backwardIndex--;
            }

            return input.Substring(backwardIndex, startIndex - backwardIndex).Trim();
        }

        public static string CleanANSI(string ansiString)
        {
            return Regex.Replace(ansiString, "[\u001b\u009b][[()#;?]*(?:[0-9]{1,4}(?:;[0-9]{0,4})*)?[0-9A-ORZcf-nqry=><]", string.Empty);
        }
    }
}
