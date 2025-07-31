using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.IPC.Serializers
{
    public static class PlayerStatus
    {
        public readonly struct Packet
        {
            public readonly byte MessageType;
            public readonly Player Player;
            public Packet(byte messageType, Player player)
            {
                MessageType = messageType;
                Player = player;
            }
        }

        public readonly struct Player
        {
            public readonly ushort ID;
            public readonly byte Status;
            public Player(ushort id, byte status)
            {
                ID = id;
                Status = status;
            }
        }

        public static byte[] Serialize(Packet Status)
        {
            byte[] serializedData = new byte[8];

            MemoryStream memoryStream = new(serializedData);
            BinaryWriter writer = new(memoryStream);

            // Write the data fields to the stream
            writer.Write(Status.MessageType);
            writer.Write(Status.Player.ID);
            writer.Write(Status.Player.Status);

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
            byte messageType = reader.ReadByte();
            Player Player = new(reader.ReadUInt16(), reader.ReadByte());

            // There is an unread 4-byte delimiter at the end of the data.

            return new Packet(messageType, Player);
        }
    }
}
