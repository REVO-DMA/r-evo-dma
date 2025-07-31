namespace Tarkov_DMA_Backend.IPC.Serializers
{
    public static class TextHelper
    {
        public readonly struct SingleContainer
        {
            public readonly SingleText[] Text;
            public readonly int TextLength;
            public SingleContainer(SingleText[] text, int textLength)
            {
                Text = text;
                TextLength = textLength;
            }
        }

        public readonly struct SingleText
        {
            public readonly byte TotalLength;
            public readonly byte TextLength;
            public readonly byte[] ASCIIBytes;
            public SingleText(byte totalLength, byte length, byte[] asciiBytes)
            {
                TotalLength = totalLength;
                TextLength = length;
                ASCIIBytes = asciiBytes;
            }
        }

        public readonly struct Container
        {
            public readonly Text[] Text;
            public readonly int TextLength;
            public Container(Text[] text, int textLength)
            {
                Text = text;
                TextLength = textLength;
            }
        }

        public readonly struct Text
        {
            public readonly byte Length;
            public readonly byte[] ASCIIBytes;
            public Text(byte length, byte[] asciiBytes)
            {
                Length = length;
                ASCIIBytes = asciiBytes;
            }
        }

        /// <summary>
        /// Create the text data in EVO IPC Packet format.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static Container Create(string[] strings)
        {
            int stringCount = strings.Length;

            byte[][] stringBytes = new byte[stringCount][];
            for (int i = 0; i < stringCount; i++)
            {
                string thisString = strings[i];

                if (thisString == null)
                    stringBytes[i] = Array.Empty<byte>();
                else
                    stringBytes[i] = Encoding.UTF8.GetBytes(thisString);
            }

            Text[] text = new Text[stringCount];
            int totalLength = 0;
            for (int i = 0; i < stringCount; i++)
            {
                byte thisLength = (byte)stringBytes[i].Length;

                // + 1 is for the byte that indicates the proceeding text's length
                totalLength += 1 + thisLength;

                if (thisLength == 0)
                    text[i] = new Text(thisLength, null);
                else
                    text[i] = new Text(thisLength, stringBytes[i]);
            }

            return new Container(text, totalLength);
        }

        /// <summary>
        /// Create the text data in EVO IPC Packet format.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SingleText CreateSingle(string text)
        {
            if (text == null)
                return new SingleText(1, 0, null);

            byte[] stringBytes = Encoding.UTF8.GetBytes(text);

            // + 1 is for the byte that indicates the proceeding text's length
            return new SingleText((byte)(stringBytes.Length + 1), (byte)stringBytes.Length, stringBytes);
        }

        /// <summary>
        /// Get a string from a byte[].
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string GetString(byte[] stringBytes)
        {
            return Encoding.UTF8.GetString(stringBytes);
        }
    }
}
