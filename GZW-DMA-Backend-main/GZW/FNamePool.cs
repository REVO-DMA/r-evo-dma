using gzw_dma_backend.DMA.Collections;
using System.Collections.Concurrent;

namespace gzw_dma_backend.GZW
{
    public static class FNamePool
    {
        private static readonly ConcurrentDictionary<uint, string> FNameCache = new();

        public static ulong Pool => Memory.ModuleBase + Offsets.Names;

        public static string GetFNameFromCache(uint nameIndex)
        {
            if (FNameCache.TryGetValue(nameIndex, out var name))
                return name;
            else
                return null;
        }

        public static bool AddFNameToCache(uint nameIndex, string fName)
        {
            return FNameCache.TryAdd(nameIndex, fName);
        }

        public static ulong GetNamePoolChunk(uint nameIndex)
        {
            uint Block = nameIndex >> 16;

            ulong FNamePool = Memory.ModuleBase + Offsets.Names + 0x10;
            return FNamePool + (Block * 0x8);
        }

        public static ulong GetFNameEntry(uint nameIndex, ulong NamePoolChunk)
        {
            ushort Offset = (ushort)nameIndex;

            return NamePoolChunk + ((uint)2 * Offset);
        }

        public readonly struct StringData(ulong strPtr, int strLength)
        {
            public readonly ulong StrPtr = strPtr;
            public readonly int StrLength = strLength;
        }

        public static StringData GetStringData(ulong fNameEntry, short fNameEntryHeader)
        {
            ulong StrPtr = fNameEntry + 0x2;
            int StrLength = fNameEntryHeader >> 0x6;

            return new(StrPtr, StrLength);
        }

        public static string GetName(uint nameIndex)
        {
            string cachedName = GetFNameFromCache(nameIndex);
            if (cachedName != null)
                return cachedName;

            uint Block = nameIndex >> 16;
            ushort Offset = (ushort)nameIndex;

            ulong FNamePool = Memory.ModuleBase + Offsets.Names + 0x10;
            ulong NamePoolChunk = Memory.ReadPtr(FNamePool + (Block * 0x8));

            ulong FNameEntry = NamePoolChunk + ((uint)2 * Offset);
            var FNameEntryHeader = Memory.ReadValue<short>(FNameEntry);
            
            ulong StrPtr = FNameEntry + 0x2;
            int StrLength = FNameEntryHeader >> 0x6;

            using var hBuf = new SharedMemory<byte>(StrLength);
            Memory.ReadBuffer(StrPtr, hBuf.Span);

            string name = Encoding.UTF8.GetString(hBuf.Span);

            AddFNameToCache(nameIndex, name);

            return name;
        }
    }
}
