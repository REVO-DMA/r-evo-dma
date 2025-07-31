using Iced.Intel;
using PeNet;
using PeNet.Header.Pe;
using Decoder = Iced.Intel.Decoder;

namespace Tarkov_DMA_Backend.LowLevel
{
    public static class Disassembler
    {
        public static ImageSectionHeader GetTextSection(byte[] pe)
        {
            PeFile peHeader = new(pe);
            ImageSectionHeader[] sections = peHeader.ImageSectionHeaders;
            foreach (ImageSectionHeader section in sections)
            {
                if (section.Name.Equals(".text"))
                    return section;
            }

            return null;
        }

        public static ulong GetFunctionLength(Span<byte> asm, int bitness = 64)
        {
            ByteArrayCodeReader reader = new(asm.ToArray());
            Decoder decoder = Decoder.Create(bitness, reader);

            while (reader.CanReadByte)
            {
                decoder.Decode(out Instruction i);

                if (i.Mnemonic == Mnemonic.Ret)
                    return i.IP + 1;
            }

            return 0x0;
        }

        public static Span<byte> ExtractFunction(Span<byte> asm, ulong length)
        {
            return asm.Slice(0, (int)length);
        }
    }
}
