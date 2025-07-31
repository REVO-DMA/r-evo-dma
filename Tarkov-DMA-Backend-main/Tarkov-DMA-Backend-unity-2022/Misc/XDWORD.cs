namespace Tarkov_DMA_Backend.Misc
{
    public readonly struct XDWORD
    {
        public static implicit operator uint(XDWORD x) => x._value;

        private readonly uint _value;

        public XDWORD(int value)
        {
            _value = (uint)value;
        }

        public XDWORD(uint value)
        {
            _value = value;
        }

        public XDWORD(string value)
        {
            if (value.Length != 4)
                throw new ArgumentException("Input string must be exactly 4 characters long.");

            uint result = 0;
            for (int i = 0; i < 4; i++)
            {
                result |= (uint)(value[i] << (8 * (3 - i)));
            }

            _value = result;
        }
    }
}
