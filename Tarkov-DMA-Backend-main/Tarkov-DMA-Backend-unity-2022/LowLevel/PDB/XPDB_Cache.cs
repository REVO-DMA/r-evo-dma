using Tarkov_DMA_Backend.LowLevel.Compression.Cab;
using Tarkov_DMA_Backend.Misc;
using static MemoryPack.MemoryPackSerializer;

namespace Tarkov_DMA_Backend.LowLevel.PDB
{
    public sealed class XPDB_Cache
    {
        private const string CacheFileDir = "xpdb";

        private readonly string _cacheFileName;

        private readonly ConcurrentDictionary<string, CacheEntry> _pdbCacheDict;

        public XPDB_Cache(string cacheName = "evo")
        {
            _cacheFileName = $"{CacheFileDir}/{cacheName}.xpdb";
            _pdbCacheDict = new();

            ReadCache();
        }

        public byte[] GetPDB(string pdbLocation)
        {
            if (!File.Exists(pdbLocation))
                return null;

            return File.ReadAllBytes(pdbLocation);
        }

        public byte[] GetPDB(string symbolServer, ulong peBase)
        {
            var debugData = PE_Parser.GetDebugData(peBase);
            if (!debugData)
                return null;

            return GetPDB(symbolServer, debugData);
        }

        public byte[] GetPDB(string symbolServer, byte[] pe)
        {
            var debugData = PE_Parser.GetDebugData(pe);
            if (!debugData)
                return null;

            return GetPDB(symbolServer, debugData);
        }

        private byte[] GetPDB(string symbolServer, Result<PE_Parser.DebugData> debugData)
        {
            var formattedDebugData = debugData.Value.GetUrlFormat();

            if (TryGetPDB(formattedDebugData.Signature, out byte[] cachedPDB))
                return cachedPDB;
            else
                Logger.WriteLine($"Attempting to fetch \"{formattedDebugData.PdbFileName}\" from \"{symbolServer}\". This may take a while...");

            byte[] pdb;
            CacheEntry entry = new(formattedDebugData.Signature, $"xpdb/{formattedDebugData.PdbFileName}");
            if (HTTP.TryGetRemoteBytes(debugData.Value.ToURL(symbolServer), out pdb))
            {
                if (TryAddPDB(entry, pdb, false, out byte[] outPDB))
                    return outPDB;
            }
            else if (HTTP.TryGetRemoteBytes(debugData.Value.ToURLCompressed(symbolServer), out pdb))
            {
                if (TryAddPDB(entry, pdb, true, out byte[] outPDB))
                    return outPDB;
            }

            return null;
        }

        private bool TryGetPDB(string peGUID, out byte[] pdb)
        {
            try
            {
                if (!_pdbCacheDict.ContainsKey(peGUID))
                {
                    pdb = null;
                    return false;
                }

                if (_pdbCacheDict.TryGetValue(peGUID, out CacheEntry entry))
                {
                    EnsureFsDeps();
                    pdb = File.ReadAllBytes(entry.Path);
                    return true;
                }

                throw new Exception("Failed to get PDB from cache");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[XPDB CACHE] -> TryGetPDB(): Failed to get PDB ~ {ex}");
                _pdbCacheDict.TryRemove(peGUID, out _);
                pdb = null;
                return false;
            }
        }

        private bool TryAddPDB(CacheEntry pdbCacheInfo, byte[] inPDB, bool compressed, out byte[] outPDB)
        {
            try
            {
                if (_pdbCacheDict.TryAdd(pdbCacheInfo.GUID, pdbCacheInfo))
                {
                    EnsureFsDeps();

                    if (compressed)
                    {
                        if (!TryDecompressPDB(inPDB, pdbCacheInfo.Path, out outPDB))
                            throw new Exception("Failed to decompress the PDB");
                    }
                    else
                    {
                        File.WriteAllBytes(pdbCacheInfo.Path, inPDB);
                        outPDB = inPDB;
                    }

                    SaveCache();
                    return true;
                }

                throw new Exception("Failed to add PDB to cache");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[XPDB CACHE] -> TryAddPDB(): Failed to add PDB ~ {ex}");
                outPDB = null;
                return false;
            }
        }

        private void ReadCache()
        {
            _pdbCacheDict.Clear();

            EnsureFsDeps();
            if (!File.Exists(_cacheFileName))
            {
                SaveCache();
                return;
            }

            byte[] serializedCache = File.ReadAllBytes(_cacheFileName);
            PDB_Cache deserializedCache = Deserialize<PDB_Cache>(serializedCache);

            foreach (var entry in deserializedCache.Entries)
                _pdbCacheDict.TryAdd(entry.GUID, entry);
        }

        private void SaveCache()
        {
            CacheHeader header = new(_pdbCacheDict.Count);
            CacheEntry[] entries = new CacheEntry[_pdbCacheDict.Count];

            int i = -1;
            foreach (var entry in _pdbCacheDict)
            {
                i++;
                entries[i] = entry.Value;
            }

            PDB_Cache pdbCache = new(header, entries);

            byte[] serializedCache = Serialize(pdbCache);

            EnsureFsDeps();
            File.WriteAllBytes(_cacheFileName, serializedCache);
        }

        private static void EnsureFsDeps()
        {
            Directory.CreateDirectory(CacheFileDir);
        }

        private static bool TryDecompressPDB(byte[] inPDB, string destFile, out byte[] outPDB)
        {
            try
            {
                string tmp = Path.GetTempFileName();
                File.WriteAllBytes(tmp, inPDB);

                CabInfo ci = new(tmp);
                string destDir = Path.GetDirectoryName(destFile);
                ci.Unpack(destDir);

                outPDB = File.ReadAllBytes(destFile);
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[XPDB CACHE] -> TryDecompressPDB(): Failed to decompress PDB ~ {ex}");
                outPDB = null;
                return false;
            }
        }
    }
}
