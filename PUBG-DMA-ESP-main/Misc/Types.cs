namespace pubg_dma_esp.Misc
{
    public static class Types
    {
        public class AsciiString
        {
            public static implicit operator string(AsciiString x) => x.Value;

            public string Value { get; init; }

            public AsciiString(string value)
            {
                Value = value;
            }

            public bool IsValid()
            {
                if (Value is null || Value.Length == 0)
                    return false;

                return true;
            }
        }

        public class UnicodeString
        {
            public static implicit operator string(UnicodeString x) => x.Value;

            public string Value { get; init; }

            public UnicodeString(string value)
            {
                Value = value;
            }

            public bool IsValid()
            {
                if (Value is null || Value.Length == 0)
                    return false;

                return true;
            }
        }
    }
}
