namespace Tarkov_DMA_Backend.Misc
{
    public readonly struct offset<T>
    {
        public static offset<T> Of(string field) => new offset<T>(field);

        public static implicit operator nint(offset<T> x) => x._offset;
        public static implicit operator int(offset<T> x) => (int)x._offset;
        public static implicit operator uint(offset<T> x) => (uint)x._offset;

        private readonly nint _offset;

        public offset(string field)
        {
            _offset = Marshal.OffsetOf<T>(field);
        }
    }
}
