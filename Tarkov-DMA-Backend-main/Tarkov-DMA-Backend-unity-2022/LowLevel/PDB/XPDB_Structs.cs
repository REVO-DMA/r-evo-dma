using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;

namespace Tarkov_DMA_Backend.LowLevel.PDB
{
    public static class XPDB_Structs
    {
        public readonly struct StructDetailsEntry(string name, string type, uint offset, uint size)
        {
            public readonly string Name = name;
            public readonly string Type = type;
            public readonly uint Offset = offset;
            public readonly uint Size = size;
        }

        public readonly struct FunctionDetails(ulong rva = 0x0, uint size = 0x0)
        {
            public readonly ulong RVA = rva;
            public readonly uint Size = size;
        }

        public readonly struct SymbolLeadup(HRESULT hr, bool coInitialized = true, ComPtr<IDiaDataSource> source = default, ComPtr<IDiaSession> session = default, ComPtr<IDiaSymbol> global = default, ComPtr<IStream> stream = default) : IDisposable
        {
            private readonly bool _coInitialized = coInitialized;

            public readonly HRESULT HR = hr;
            public readonly ComPtr<IDiaDataSource> Source = source;
            public readonly ComPtr<IDiaSession> Session = session;
            public readonly ComPtr<IDiaSymbol> Global = global;
            public readonly ComPtr<IStream> Stream = stream;

            public void Dispose()
            {
                Source.Dispose();
                Session.Dispose();
                Global.Dispose();
                Stream.Dispose();

                if (_coInitialized)
                    CoUninitialize();
            }
        }
    }
}
