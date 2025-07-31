using MemoryPack;

[MemoryPackable]
public readonly partial struct PDB_Cache(CacheHeader header, CacheEntry[] entries)
{
    public readonly CacheHeader Header = header;
    public readonly CacheEntry[] Entries = entries;
}

[MemoryPackable]
public readonly partial struct CacheHeader(int entryCount)
{
    public readonly int EntryCount = entryCount;
}

[MemoryPackable]
public readonly partial struct CacheEntry(string guid, string path)
{
    public readonly string GUID = guid;
    public readonly string Path = path;
}