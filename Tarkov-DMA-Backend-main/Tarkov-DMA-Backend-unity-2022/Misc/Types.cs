using MemoryPack;
using Tarkov_DMA_Backend.Tarkov;
using Tarkov_DMA_Backend.Unity.LowLevel.PersistentCache;
using Tarkov_DMA_Backend.Unity.LowLevel;
using Tarkov_DMA_Backend.Unity;
using static Tarkov_DMA_Backend.Unity.LowLevel.PersistentCache.Classes;

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_Realtime
{
    public IPC_Realtime(IPC_RealtimePlayer[] players, IPC_RealtimeGrenade[] grenades)
    {
        Players = players;
        Grenades = grenades;
    }

    public readonly IPC_RealtimePlayer[] Players;
    public readonly IPC_RealtimeGrenade[] Grenades;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_RealtimePlayer
{
    public IPC_RealtimePlayer(ushort id, IPC_Vector2 position, float rotation)
    {
        ID = id;
        Position = position;
        Rotation = rotation;
    }

    public readonly ushort ID;
    public readonly IPC_Vector2 Position;
    public readonly float Rotation;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_Vector2
{
    public IPC_Vector2(float x, float y)
    {
        Y = y;
        X = x;
    }

    public readonly float X;
    public readonly float Y;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_Vector3
{
    public IPC_Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly float X;
    public readonly float Y;
    public readonly float Z;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_StaticGrenade
{
    public IPC_StaticGrenade(ushort id, string name)
    {
        ID = id;
        Name = name;
    }

    public readonly ushort ID;
    public readonly string Name;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_RealtimeGrenade
{
    public IPC_RealtimeGrenade(ushort id, IPC_Vector2 position, int distance)
    {
        ID = id;
        Position = position;
        Distance = distance;
    }

    public readonly ushort ID;
    public readonly IPC_Vector2 Position;
    public readonly int Distance;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_StaticPlayer
{
    public IPC_StaticPlayer(ushort id, string accountID, string groupID, string name, string faction, PlayerType playerType)
    {
        ID = id;
        AccountID = accountID;
        GroupID = groupID;
        Name = name;
        Faction = faction;
        PlayerType = playerType;
    }

    public readonly ushort ID;
    public readonly string AccountID;
    public readonly string GroupID;
    public readonly string Name;
    public readonly string Faction;
    public readonly PlayerType PlayerType;
}

public enum EPlayerStatus
{
    Alive,
    Die,
    Exfil,
    Destroy
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_PlayerStatus
{
    public IPC_PlayerStatus(ushort id, EPlayerStatus status)
    {
        ID = id;
        Status = status;
    }

    public readonly ushort ID;
    public readonly EPlayerStatus Status;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_PlayerStats
{
    public IPC_PlayerStats(string name, ushort id, string level, string accountType, string onlineTime, string killDeathRatio, string survivalRate)
    {
        Name = name;
        ID = id;
        Level = level;
        AccountType = accountType;
        OnlineTime = onlineTime;
        KillDeathRatio = killDeathRatio;
        SurvivalRate = survivalRate;
    }

    public readonly string Name;
    public readonly ushort ID;
    public readonly string Level;
    public readonly string AccountType;
    public readonly string OnlineTime;
    public readonly string KillDeathRatio;
    public readonly string SurvivalRate;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_DeferredPlayer
{
    public IPC_DeferredPlayer(ushort id, byte healthPercent, ushort distance, short height, string hands, string value)
    {
        ID = id;
        HealthPercent = healthPercent;
        Distance = distance;
        Height = height;
        Hands = hands;
        Value = value;
    }

    public readonly ushort ID;
    public readonly byte HealthPercent;
    public readonly ushort Distance;
    public readonly short Height;
    public readonly string Hands;
    public readonly string Value;
}

public enum AuthStatusType
{
    CanLaunch,
    InvalidCredentials,
    MembershipExpired,
    InvalidHWID,
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_AuthStatus
{
    public IPC_AuthStatus(AuthStatusType authStatusType, string expiration, string message)
    {
        AuthStatusType = authStatusType;
        Expiration = expiration;
        Message = message;
    }

    public readonly AuthStatusType AuthStatusType;
    public readonly string Expiration;
    public readonly string Message;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_FeatureState
{
    public IPC_FeatureState(string id, bool enabled)
    {
        ID = id;
        Enabled = enabled;
    }

    public readonly string ID;
    public readonly bool Enabled;
}

[MemoryPackable]
[GenerateTypeScript]
public partial class IPC_Exfil
{
    public IPC_Exfil(byte id, string name, string description, IPC_Vector2 position, ExfilStatus status)
    {
        ID = id;
        Name = name;
        Description = description;
        Position = position;
        Status = status;
    }

    public readonly byte ID;
    public readonly string Name;
    public readonly string Description;
    public readonly IPC_Vector2 Position;
    public readonly ExfilStatus Status;
}

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

[MemoryPackable]
public partial struct ChamsMaterial
{
    public ulong Address;
    public int InstanceID;
    public int _ColorVisible;
    public int _ColorInvisible;
}

[MemoryPackable]
public readonly partial struct ClassesSearchInfo(FindType ft, ResolveType rt)
{
    public readonly FindType FT = ft;
    public readonly ResolveType RT = rt;
}

[MemoryPackable]
public readonly partial struct ClassesEntry(ClassesSearchInfo searchInfo, string assemblyName, string nameSpace = null, string className = null, uint classToken = 0x0, uint methodToken = 0x0, bool canFail = false, bool canSkip = true)
{
    public readonly ClassesSearchInfo SearchInfo = searchInfo;
    public readonly string AssemblyName = assemblyName;
    public readonly string NameSpace = nameSpace;
    public readonly string ClassName = className;
    public readonly uint ClassToken = classToken;
    public readonly uint MethodToken = methodToken;
    /// <summary>
    /// Whether or not this class is critical and must be found before fully loading the cheat.
    /// </summary>
    public readonly bool CanFail = canFail;
    /// <summary>
    /// Whether or not this class can be skipped if it's already been resolved.
    /// </summary>
    public readonly bool CanSkip = canSkip;

    public string GetFullClassName()
    {
        if (!string.IsNullOrEmpty(NameSpace))
            return $"{NameSpace}.{ClassName}";
        else
            return ClassName;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"Assembly: {AssemblyName} -> ");

        if (ClassToken != 0x0)
            sb.Append($"Token: {ClassToken:X}");
        else
            sb.Append($"Class: {GetFullClassName()}");

        return sb.ToString();
    }
}

[MemoryPackable]
public readonly partial struct PrimitiveDictionary<TKey, TVal>(TKey[] keys, TVal[] values)
{
    public readonly TKey[] Keys = keys;
    public readonly TVal[] Values = values;

    [MemoryPackIgnore]
    public int Count => Keys?.Length ?? 0;

    public static PrimitiveDictionary<TKey, TVal> FromDictionary(IDictionary<TKey, TVal> dict)
    {
        TKey[] keys = dict.Keys.ToArray();
        TVal[] values = dict.Values.ToArray();

        return new(keys, values);
    }

    public readonly void ToDictionary(IDictionary<TKey, TVal> dict)
    {
        if (Count == 0)
            return;

        for (int i = 0; i < Count; i++)
        {
            TKey key = Keys[i];
            TVal value = Values[i];

            dict.TryAdd(key, value);
        }
    }
}

[MemoryPackable]
public partial class PersistentCacheItems
{
    public ulong HookAddress;

    // Chams
    public PlayerType[] ChamsMaterialsKeys;
    public ChamsMaterial[] ChamsMaterialsValues;

    // Classes
    public ClassesEntry[] ResolvedStaticClassesKeys;
    public ulong[] ResolvedStaticClassesValues;

    public ClassesEntry[] ResolvedClassesKeys;
    public ulong[] ResolvedClassesValues;

    public ClassesEntry[] ResolvedSingletonsKeys;
    public ulong[] ResolvedSingletonsValues;
}