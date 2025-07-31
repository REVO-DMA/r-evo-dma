using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TarkovDumper
{
    public sealed partial class DumpParser
    {
        [GeneratedRegex("^(0x|0X)?[a-fA-F0-9]+$")]
        private static partial Regex OffsetRegex();

        private readonly string[] _dump;

        public DumpParser(string dumpPath)
        {
            _dump = File.ReadAllLines(dumpPath);
        }

        public readonly struct Result<T>(bool success, T value = default)
        {
            public readonly bool Success = success;
            public readonly T Value = value;
        }

        public readonly struct OffsetData(string offsetName, string typeName, uint offset)
        {
            public readonly string OffsetName = offsetName;
            public readonly string TypeName = typeName;
            public readonly uint Offset = offset;

            public override string ToString() => $"0x{Offset:X}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<OffsetData> FindOffsetByName(string offsetGroupName, string offsetName) => FindOffset(offsetGroupName, offsetName: offsetName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Result<OffsetData> FindOffsetByTypeName(string offsetGroupName, string typeName) => FindOffset(offsetGroupName, typeName: typeName);

        public enum SearchType
        {
            TypeName,
            OffsetName
        }

        public readonly struct EntitySearchListEntry(string name, SearchType searchType)
        {
            public readonly string Name = name;
            public readonly SearchType SearchType = searchType;
        }

        /// <summary>
        /// Finds an offset group that contains all of the given entities.
        /// </summary>
        public string FindOffsetGroupWithEntities(List<EntitySearchListEntry> entities)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;

            for (int i = 0; i < _dump.Length; i++)
            {
                string cleaned = CleanLine(_dump[i]);

                if (IsOffsetGroup(cleaned))
                {
                    ReadOnlySpan<string> foundOffsetGroup = GetOffsetGroupExtents(i + 1);
                    if (foundOffsetGroup == null)
                        continue;

                    int foundCount = 0;

                    foreach (EntitySearchListEntry entity in entities)
                    {
                        foreach (string line in foundOffsetGroup)
                        {
                            var lineData = ExtractLineData(CleanLine(line));
                            if (!lineData.Success)
                                continue;

                            if (entity.SearchType == SearchType.TypeName &&
                                lineData.Value.TypeName.Equals(entity.Name, ct))
                            {
                                foundCount++;
                                break;
                            }
                            else if (entity.SearchType == SearchType.OffsetName &&
                                lineData.Value.OffsetName.Equals(entity.Name, ct))
                            {
                                foundCount++;
                                break;
                            }
                        }
                    }

                    if (foundCount == entities.Count)
                    {
                        string className = cleaned.Split(']')[1].Split(':')[0].Trim();
                        return className;
                    }
                }
            }

            return null;
        }

        #region Private Methods

        private Result<OffsetData> FindOffset(string offsetGroupName, string offsetName = null, string typeName = null)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;

            if (offsetGroupName == null)
                return new(false);

            if (offsetName == null && typeName == null)
                return new(false);

            ReadOnlySpan<string> foundClass = FindOffsetGroupByName(offsetGroupName);
            if (foundClass == null)
                return new(false);

            foreach (string line in foundClass)
            {
                string cleaned = CleanLine(line);

                var lineData = ExtractLineData(cleaned);
                if (!lineData.Success)
                    continue;

                if (offsetName != null && lineData.Value.OffsetName.Equals(offsetName, ct))
                    goto found;
                else if (typeName != null && lineData.Value.TypeName.Equals(typeName, ct))
                    goto found;
                else
                    continue;

                found:
                uint offset = uint.Parse(lineData.Value.Offset, System.Globalization.NumberStyles.HexNumber);
                return new(true, new(lineData.Value.OffsetName, lineData.Value.TypeName, offset));
            }

            return new(false);
        }

        /// <summary>
        /// Finds an offset group by name.
        /// </summary>
        private ReadOnlySpan<string> FindOffsetGroupByName(string name)
        {
            for (int i = 0; i < _dump.Length; i++)
            {
                string cleaned = CleanLine(_dump[i]);

                if (IsOffsetGroup(cleaned, name))
                    return GetOffsetGroupExtents(i + 1);
            }

            return null;
        }

        /// <summary>
        /// Finds the bounds of an offset group starting at the given line.
        /// </summary>
        private ReadOnlySpan<string> GetOffsetGroupExtents(int startIndex)
        {
            int endIndex = -1;
            int lastLineWithOffset = -1;

            for (int i = startIndex; i < _dump.Length; i++)
            {
                string cleaned = CleanLine(_dump[i], true);

                var lineData = ExtractLineData(cleaned);
                if (!lineData.Success)
                    continue;

                if (IsHexString(lineData.Value.Offset))
                    lastLineWithOffset = i + 1;
                else if (cleaned.Length > 0)
                {
                    endIndex = lastLineWithOffset;
                    break;
                }
            }

            if (endIndex > -1)
                return _dump.AsSpan(startIndex, endIndex - startIndex);

            return null;
        }

        private static string CleanLine(string line, bool minimal = false)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;

            line = line.Trim();

            if (!minimal)
            {
                line = line.Replace("[C]", "", ct).Replace("[S]", "", ct);
            }

            return line;
        }

        private readonly struct LineData(string offset, string offsetName, string typeName)
        {
            public readonly string Offset = offset;
            public readonly string OffsetName = offsetName;
            public readonly string TypeName = typeName;
        }

        private static Result<LineData> ExtractLineData(string cleaned)
        {
            try
            {
                string offset = cleaned.Split('[')[1].Split(']')[0];
                string offsetName = cleaned.Split(']')[1].Split(':')[0].Trim();
                string typeName = cleaned.Split(':')[1].Trim();

                return new(true, new(offset, offsetName, typeName));
            }
            catch
            {
                return new(false);
            }
        }

        private static bool IsOffsetGroup(string cleaned, string name = null)
        {
            const StringComparison ct = StringComparison.OrdinalIgnoreCase;

            if (name == null)
            {
                const string v1 = $"[class]";
                const string v2 = $"[struct]";

                if (cleaned.StartsWith(v1, ct) ||
                    cleaned.StartsWith(v2, ct))
                {
                    return true;
                }
            }
            else
            {
                string v1 = $"[class] -.{name}";
                string v2 = $"[class] {name}";

                string v3 = $"[struct] -.{name}";
                string v4 = $"[struct] {name}";

                if (cleaned.StartsWith(v1, ct) ||
                    cleaned.StartsWith(v2, ct) ||
                    cleaned.StartsWith(v3, ct) ||
                    cleaned.StartsWith(v4, ct))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsHexString(string data)
        {
            if (OffsetRegex().IsMatch(data))
                return true;

            return false;
        }

        #endregion
    }
}
