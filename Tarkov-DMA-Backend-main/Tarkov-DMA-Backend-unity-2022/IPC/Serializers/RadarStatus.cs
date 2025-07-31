using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.IPC.Serializers
{
    public static class RadarStatus
    {
        public readonly struct Packet
        {
            public readonly byte MessageType;
            public readonly byte Status;
            public Packet(byte messageType, byte status)
            {
                MessageType = messageType;
                Status = status;
            }
        }

        public static byte[] Serialize(Packet Status)
        {
            byte[] serializedData = new byte[6];

            MemoryStream memoryStream = new(serializedData);
            BinaryWriter writer = new(memoryStream);

            // Write the data fields to the stream
            writer.Write(Status.MessageType);
            writer.Write(Status.Status);

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
            byte status = reader.ReadByte();

            // There is an unread 4-byte delimiter at the end of the data.

            return new Packet(messageType, status);
        }
    }
}
