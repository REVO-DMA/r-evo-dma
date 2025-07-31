using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.IPC.Serializers
{
    public static class RadarLoot
    {
        public readonly struct Packet
        {
            public readonly byte MessageType;
            public readonly LootItem[] LootItems;
            public Packet(byte messageType, LootItem[] lootItems)
            {
                MessageType = messageType;
                LootItems = lootItems;
            }
        }

        public readonly struct LootItemPosition
        {
            public readonly float X;
            public readonly float Y;
            public readonly short Height;
            public LootItemPosition(float x, float y, short height)
            {
                X = x;
                Y = y;
                Height = height;
            }
        }

        public readonly struct LootItem
        {
            public readonly ulong ID;
            public readonly bool Shown;
            public readonly string BSG_ID;
            public readonly byte Type;
            public readonly string Name;
            public readonly LootItemPosition Position;
            public LootItem(ulong id, bool shown, string bsgID, byte type, string name, LootItemPosition position)
            {
                ID = id;
                Shown = shown;
                BSG_ID = bsgID;
                Type = type;
                Name = name;
                Position = position;
            }
        }

        public static byte[] Serialize(Packet Loot)
        {
            int lootLength = Loot.LootItems.Length;

            // Generate all text for the players in this packet
            TextHelper.Container[] textItems = new TextHelper.Container[lootLength];
            int totalTextLength = 0;
            for (int i = 0; i < lootLength; i++)
            {
                TextHelper.Container textItem = TextHelper.Create(new string[] { Loot.LootItems[i].BSG_ID, Loot.LootItems[i].Name });
                totalTextLength += textItem.TextLength;

                textItems[i] = textItem;
            }

            // Calculate the total size of the packet
            int size = GetSize(totalTextLength, lootLength);
            byte[] serializedData = new byte[size];

            MemoryStream memoryStream = new(serializedData);
            BinaryWriter writer = new(memoryStream);

            // Write the data fields to the stream
            writer.Write(Loot.MessageType); // MessageType
            writer.Write(lootLength); // LootItem[] Length
            for (int i = 0; i < lootLength; i++) // All players (11 bytes per)
            {
                LootItem lootItem = Loot.LootItems[i];

                TextHelper.Container playerText = textItems[i];

                writer.Write(lootItem.ID); // Memory address
                writer.Write(lootItem.Shown);
                // BSG ID
                writer.Write(playerText.Text[0].Length); // BSG ID Length
                byte[] BSG_ID_Bytes = playerText.Text[0].ASCIIBytes;
                if (BSG_ID_Bytes != null) writer.Write(BSG_ID_Bytes); // BSG ID
                writer.Write(lootItem.Type);
                // Name
                writer.Write(playerText.Text[1].Length); // Name Length
                byte[] NameBytes = playerText.Text[1].ASCIIBytes;
                if (NameBytes != null) writer.Write(NameBytes); // Name
                writer.Write(lootItem.Position.X);
                writer.Write(lootItem.Position.Y);
                writer.Write(lootItem.Position.Height);
            }

            // Add IPC Delimiter for message framing
            writer.Write(Constants.IPC.Delimiter);

            return serializedData;
        }

        /// <summary>
        /// Reference implementation of deserialization.
        /// </summary>
        public static Packet Deserialize(byte[] data)
        {
            MemoryStream memoryStream = new(data);
            BinaryReader reader = new(memoryStream);

            // Read the data fields from the stream
            byte MessageType = reader.ReadByte();
            int lootCount = reader.ReadInt32();
            LootItem[] lootItems = new LootItem[lootCount];
            for (int i = 0; i < lootCount; i++)
            {
                ulong ID = reader.ReadUInt64();
                bool Shown = reader.ReadBoolean();
                string BSG_ID = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadByte()));
                byte Type = reader.ReadByte();
                string Name = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadByte()));
                LootItemPosition Position = new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt16());

                LootItem lootItem = new(ID, Shown, BSG_ID, Type, Name, Position);

                lootItems[i] = lootItem;
            }

            // There is an unread 4-byte delimiter at the end of the data.

            return new Packet(MessageType, lootItems);
        }

        public static int GetSize(int textLength, int lootLength)
        {
            // Calculate the size needed for serialization
            int size = textLength; // Total text byte length

            size += 1; // MessageType field size (byte)

            size += 4; // LootItem[] Length (int)

            size += 20 * lootLength; // Total size of LootItem[]

            size += 4; // Delimiter

            return size;
        }
    }
}
