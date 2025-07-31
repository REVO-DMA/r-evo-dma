namespace pubg_dma_esp.PUBG.AC
{
    public static class Decryption
    {
        public static byte ROL(byte value, int count)
        {
            const int nbits = 8;
            count %= nbits;
            return (byte)(value << count | value >> nbits - count);
        }

        public static ushort ROL(ushort value, int count)
        {
            const int nbits = 16;
            count %= nbits;
            return (ushort)(value << count | value >> nbits - count);
        }

        public static uint ROL(uint value, int count)
        {
            const int nbits = 32;
            count %= nbits;
            return value << count | value >> nbits - count;
        }

        public static ulong ROL(ulong value, int count)
        {
            const int nbits = 64;
            count %= nbits;
            return value << count | value >> nbits - count;
        }


        public static byte ROL1(byte value, int count) => ROL(value, count);
        public static ushort ROL2(ushort value, int count) => ROL(value, count);
        public static uint ROL4(uint value, int count) => ROL(value, count);
        public static ulong ROL8(ulong value, int count) => ROL(value, count);
        public static byte ROR1(byte value, int count) => ROL(value, -count);
        public static ushort ROR2(ushort value, int count) => ROL(value, -count);
        public static uint ROR4(uint value, int count) => ROL(value, -count);
        public static ulong ROR8(ulong value, int count) => ROL(value, -count);

        public static uint DecryptNameIndex(uint value)
        {
            uint RO;
            if (Offsets.DecryptFNameIndex.NameIsROR)
                RO = ROR4(value ^ Offsets.DecryptFNameIndex.NameIndexXor1, Offsets.DecryptFNameIndex.NameIndexOne);
            else
                RO = ROL4(value ^ Offsets.DecryptFNameIndex.NameIndexXor1, Offsets.DecryptFNameIndex.NameIndexOne);

            return RO ^ RO << Offsets.DecryptFNameIndex.NameIndexTwo ^ Offsets.DecryptFNameIndex.NameIndexXor2;
        }

        public static uint Actor_ID_Decrypt(uint id)
        {
            return DecryptNameIndex(id);
        }
    }
}
