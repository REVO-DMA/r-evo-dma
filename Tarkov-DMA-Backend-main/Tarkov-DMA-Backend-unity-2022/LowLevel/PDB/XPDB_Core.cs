using Tarkov_DMA_Backend.Misc;
using TerraFX.Interop.Windows;
using static Tarkov_DMA_Backend.LowLevel.PDB.XPDB_Structs;
using static TerraFX.Interop.Windows.Windows;

namespace Tarkov_DMA_Backend.LowLevel.PDB
{
    public sealed unsafe class XPDB_Core : IDisposable
    {
        private static readonly string[] _baseTypes = new string[]
        {
            "<NoType>",
            "void",
            "char",
            "wchar_t",
            "signed char",
            "unsigned char",
            "int",
            "unsigned int",
            "float",
            "<BCD>",
            "bool",
            "short",
            "unsigned short",
            "long",
            "unsigned long",
            "__int8",
            "__int16",
            "__int32",
            "__int64",
            "__int128",
            "unsigned __int8",
            "unsigned __int16",
            "unsigned __int32",
            "unsigned __int64",
            "unsigned __int128",
            "<currency>",
            "<date>",
            "VARIANT",
            "<complex>",
            "<bit>",
            "BSTR",
            "HRESULT",
            "char16_t",
            "char32_t",
            "char8_t"
        };
        private const string UnknownTypeName = "<UnknownType>";
        private const string DebugInterfaceAccessDLL = "msdia140.dll";
        private const string DebugInterfaceAccessDLL_EVO = "dbg.evo";

        private readonly byte[] _pdbData;
        private readonly HGLOBAL _pdbMem;
        private readonly SymbolLeadup _symbolLeadup;

        static XPDB_Core()
        {
            byte[] msdia = File.ReadAllBytes(DebugInterfaceAccessDLL);
            File.WriteAllBytes(DebugInterfaceAccessDLL_EVO, msdia);
            Runner.RunCommand($"regsvr32 {DebugInterfaceAccessDLL_EVO} /s");
        }

        public XPDB_Core(string pdbPath) : this(File.ReadAllBytes(pdbPath)) { }

        public XPDB_Core(byte[] pdbBytes)
        {
            _pdbData = pdbBytes;

            int pdbLen = _pdbData.Length;
            _pdbMem = GlobalAlloc(GMEM.GMEM_MOVEABLE, (nuint)pdbLen);
            if (_pdbMem == 0x0)
                throw new Exception($"Failed to allocate global memory for PDB");

            void* pMem = GlobalLock(_pdbMem);
            fixed (byte* pPDB = _pdbData)
            {
                Buffer.MemoryCopy(pPDB, pMem, pdbLen, pdbLen);
            }
            GlobalUnlock(_pdbMem);

            var symbolLeadup = GetSymbolLeadup();
            if (!symbolLeadup)
                throw new Exception("Failed to get symbol leadup");
            else
                _symbolLeadup = symbolLeadup;
        }

        private static string GetBaseTypeName(uint type)
        {
            if (type > _baseTypes.Length - 1)
                return UnknownTypeName;
            return _baseTypes[type];
        }

        private static string GetTypeName(ComPtr<IDiaSymbol> pType)
        {
            if (pType.Get() == null)
                return UnknownTypeName;

            uint symTag;
            pType.Get()->get_symTag(&symTag);

            if (symTag == (uint)SymTagEnum.SymTagBaseType)
            {
                uint baseType;
                pType.Get()->get_baseType(&baseType);
                return GetBaseTypeName(baseType);
            }
            else if (symTag == (uint)SymTagEnum.SymTagPointerType)
            {
                using ComPtr<IDiaSymbol> pBaseType = new();
                pType.Get()->get_type((IDiaSymbol**)&pBaseType);
                return $"{GetTypeName(pBaseType)}*";
            }
            else if (symTag == (uint)SymTagEnum.SymTagArrayType)
            {
                uint count;
                pType.Get()->get_count(&count);
                using ComPtr<IDiaSymbol> pElementType = new();
                pType.Get()->get_type((IDiaSymbol**)&pElementType);
                return $"{GetTypeName(pElementType)}[{count}]";
            }
            else
            {
                using PChar pTypeName = new(512);
                pType.Get()->get_name(&pTypeName.Ptr);
                string typeName = pTypeName.ToString();
                return typeName.Length > 0 ? typeName : UnknownTypeName;
            }
        }

        public Result<List<StructDetailsEntry>> GetStruct(string functionName)
        {
            using ComPtr<IDiaSymbol> cpSymbol = FindSymbol(functionName, _symbolLeadup.Global.Get(), SymTagEnum.SymTagUDT);
            
            List<StructDetailsEntry> entries = new();

            if (cpSymbol.Get() != null)
            {
                using ComPtr<IDiaEnumSymbols> pEnumFields = new();
                HRESULT hr = cpSymbol.Get()->findChildren(SymTagEnum.SymTagData, null, (uint)NameSearchOptions.nsNone, (IDiaEnumSymbols**)&pEnumFields);
                if (FAILED(hr) || pEnumFields.Get() == null)
                    return Result<List<StructDetailsEntry>>.Fail;

                uint celtFetched = 0;
                while (pEnumFields.Get()->Next(1, (IDiaSymbol**)&cpSymbol, &celtFetched) == S.S_OK && celtFetched == 1)
                {
                    int offset = 0;
                    cpSymbol.Get()->get_offset(&offset);

                    using PChar name = new(512);
                    cpSymbol.Get()->get_name(&name.Ptr);

                    using ComPtr<IDiaSymbol> pType = new();
                    cpSymbol.Get()->get_type((IDiaSymbol**)&pType);

                    string typeName = GetTypeName(pType);

                    ulong fSize;
                    pType.Get()->get_length(&fSize);

                    entries.Add(new(name.ToString(), typeName, (uint)offset, (uint)fSize));
                }
            }
            else
                return Result<List<StructDetailsEntry>>.Fail;

            return new(true, entries);
        }

        public Result<FunctionDetails> GetFunction(string functionName, ulong moduleBase = 0x0)
        {
            using ComPtr<IDiaSymbol> cpSymbolA = FindSymbol(functionName, _symbolLeadup.Global.Get(), SymTagEnum.SymTagFunction);
            using ComPtr<IDiaSymbol> cpSymbolB = FindSymbol(functionName, _symbolLeadup.Global.Get(), SymTagEnum.SymTagPublicSymbol);

            uint rva = 0;
            ulong size = 0;

            if (cpSymbolA.Get() != null)
            {
                cpSymbolA.Get()->get_relativeVirtualAddress(&rva);
                cpSymbolA.Get()->get_length(&size);
            }
            else if (cpSymbolB.Get() != null)
            {
                cpSymbolB.Get()->get_relativeVirtualAddress(&rva);
                cpSymbolB.Get()->get_length(&size);
            }
            else
                return Result<FunctionDetails>.Fail;

            return new(true, new(rva + moduleBase, (uint)size));
        }

        public void Dispose()
        {
            if (_pdbMem != 0x0)
                GlobalFree(_pdbMem);

            _symbolLeadup.Dispose();
        }

        private Result<SymbolLeadup> GetSymbolLeadup()
        {
            ComPtr<IDiaDataSource> cpSource;
            ComPtr<IDiaSession> cpSession;
            ComPtr<IDiaSymbol> cpGlobal;
            ComPtr<IStream> cpStream;

            HRESULT hr = default;
            bool coInitialized = false;

            try
            {
                hr = CoInitialize(null);
                if (FAILED(hr))
                    throw new Exception("Failed on \"CoInitialize\"");
                else coInitialized = true;

                hr = CreateStreamOnHGlobal(_pdbMem, TRUE, (IStream**)&cpStream);
                if (FAILED(hr))
                    throw new Exception("Failed on \"CreateStreamOnHGlobal\"");

                Guid rclsid = CLSID.CLSID_DiaSource;
                hr = CoCreateInstance(&rclsid, null, (uint)CLSCTX.CLSCTX_INPROC_SERVER, __uuidof<IDiaDataSource>(), (void**)&cpSource);
                if (FAILED(hr))
                    throw new Exception("Failed on \"CoCreateInstance\"");

                hr = cpSource.Get()->loadDataFromIStream(cpStream);
                if (FAILED(hr))
                    throw new Exception("Failed on \"loadDataFromIStream\"");

                hr = cpSource.Get()->openSession((IDiaSession**)&cpSession);
                if (FAILED(hr))
                    throw new Exception("Failed on \"openSession\"");

                hr = cpSession.Get()->get_globalScope((IDiaSymbol**)&cpGlobal);
                if (FAILED(hr))
                    throw new Exception("Failed on \"get_globalScope\"");

                return new(true, new(hr, coInitialized, cpSource, cpSession, cpGlobal, cpStream));
            }
            catch(Exception ex)
            {
                Logger.WriteLine($"[XPDB CORE] -> GetSymbolLeadup(): {ex}");
                return new(false, new(hr, coInitialized));
            }
        }

        private static ComPtr<IDiaSymbol> FindSymbol(string symbolName, IDiaSymbol* pGlobal, SymTagEnum symbolType)
        {
            using ComPtr<IDiaEnumSymbols> cpEnumSymbols = new();
            using PinnedObject<string> pSymbolName = new(symbolName);
            HRESULT hr = pGlobal->findChildren(symbolType, (char*)(nint)pSymbolName, (uint)NameSearchOptions.nsCaseInsensitive, (IDiaEnumSymbols**)&cpEnumSymbols);
            if (FAILED(hr))
                return null;

            ComPtr<IDiaSymbol> cpSymbol;
            uint celtFetched;
            hr = cpEnumSymbols.Get()->Next(1, (IDiaSymbol**)&cpSymbol, &celtFetched);
            if (hr == S.S_OK && celtFetched == 1)
                return cpSymbol;

            return null;
        }
    }
}
