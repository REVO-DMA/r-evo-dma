namespace arena_dma_backend.DMA
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
        public readonly void Validate()
        {
            if (Va == 0x0)
                throw new NullPtrException();
        }

        /// <summary>
        /// Validates the Pointer without throwing an exception.
        /// </summary>
        public readonly bool ValidateEx()
        {
            if (Va == 0x0 || Va < 0x1000000 || Va >= 0x7FFFFFFFFFF)
                return false;

            return true;
        }
    }
}
