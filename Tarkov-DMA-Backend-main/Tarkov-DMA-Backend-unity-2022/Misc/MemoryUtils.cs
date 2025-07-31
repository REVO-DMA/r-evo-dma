namespace Tarkov_DMA_Backend.Misc
{
    public static class MemoryUtils
    {
        public static bool IsValidAddress(ulong va)
        {
            if (va < 0x100000 || va >= 0x7FFFFFFFFFF)
                return false;

            return true;
        }

        public static ulong AlignAddress(ulong addr, uint bound = 8)
        {
            if (addr % bound != 0)
                addr += bound - addr % bound;

            return addr;
        }
    }
}
