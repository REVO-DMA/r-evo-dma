using pubg_dma_esp.MemDMA.Collections;

namespace pubg_dma_esp.PUBG.AC
{
    internal static class GNames
    {
        public static string ReadUnicode(ulong address, uint size)
        {
            int cb = (int)size * 2;
            using var nameBytes = new SharedMemory<byte>(cb);
            var buf = nameBytes.Span;
            Memory.ReadBuffer(address, buf);
            var mem = Marshal.StringToCoTaskMemUni(Encoding.Unicode.GetString(buf));
            try
            {
                return Marshal.PtrToStringUni(mem);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(mem);
            }
        }
    }
}
