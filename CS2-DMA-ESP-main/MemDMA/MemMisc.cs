namespace cs2_dma_esp.MemDMA
{
    /// <summary>
    /// Represents a 64-Bit Unsigned Pointer Address.
    /// </summary>
    public readonly struct MemPointer
    {
        public static implicit operator MemPointer(ulong x) => x;
        public static implicit operator ulong(MemPointer x) => x.Va;
        /// <summary>
        /// Virtual Address of this Pointer.
        /// </summary>
        public readonly ulong Va;

        /// <summary>
        /// Validates the Pointer.
        /// </summary>
        /// <exception cref="NullPtrException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Validate()
        {
            if (Va == 0x0)
                throw new NullPtrException();
        }

        /// <summary>
        /// Validates the Pointer without throwing an exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool ValidateEx()
        {
            if (Va == 0x0 || Va < 0x1000000 || Va >= 0x7FFFFFFFFFF)
                return false;

            return true;
        }
    }

    /// <summary>
    /// FPGA Read Algorithm
    /// </summary>
    public enum FpgaAlgo : int
    {
        /// <summary>
        /// Auto 'fpga' parameter.
        /// </summary>
        Auto = -1,
        /// <summary>
        /// Async Normal Read (default)
        /// </summary>
        AsyncNormal = 0,
        /// <summary>
        /// Async Tiny Read
        /// </summary>
        AsyncTiny = 1,
        /// <summary>
        /// Old Normal Read
        /// </summary>
        OldNormal = 2,
        /// <summary>
        /// Old Tiny Read
        /// </summary>
        OldTiny = 3
    }
}
