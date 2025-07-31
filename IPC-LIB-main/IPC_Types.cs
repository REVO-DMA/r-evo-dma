using System.Runtime.InteropServices;

namespace IPC_LIB
{
    public static class IPC_Types
    {
        internal enum eSystemMessage
        {
            MIN,
            DEFAULT,
            RSA_PUBLIC_KEY,
            AES_KEY,
            REMOTE_STATUS,
            MAX,
        }

        internal enum eRemoteStatus
        {
            DEFAULT,
            WAITING,
            READY
        }

        internal static bool IsSystemMessage(int messageType)
        {
            if (messageType > (int)eSystemMessage.MIN && messageType < (int)eSystemMessage.MAX)
                return true;

            return false;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = Size)]
        public readonly struct MessageHeader(int messageLength, ulong transactionID, int messageType, bool encrypted)
        {
            /// <summary>
            /// The byte size of this type.
            /// </summary>
            public const int Size = 17;

            [FieldOffset(0)]
            public readonly int MessageLength = messageLength;
            [FieldOffset(4)]
            public readonly ulong TransactionID = transactionID;
            [FieldOffset(12)]
            public readonly int MessageType = messageType;
            [FieldOffset(16)]
            public readonly bool Encrypted = encrypted;
        }

        public delegate void OnMessage(MessageHeader header, ReadOnlySpan<byte> data);
    }
}
