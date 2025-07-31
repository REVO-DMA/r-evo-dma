using IPC_LIB.Crypto;
using IPC_LIB.SharedMemory;
using System.Collections.Concurrent;
using System.IO.Pipes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using IPC_LIB.Core;

namespace IPC_LIB
{
    internal sealed class Receiver
    {
        #region Fields

        /// <summary>
        /// Messages with a bound action that are awaiting a response.
        /// The action is invoked when a response is sent.
        /// </summary>
        public readonly ConcurrentDictionary<ulong, Action<byte[]>> AwaitingResponse;

        public readonly Dictionary<IPC_Types.eSystemMessage, Action<IPC_Types.MessageHeader, byte[]>> SystemMessageHandlers;

        private readonly ReceivePool _receivePool;

        private readonly Thread _tMain;
        
        private event IPC_Types.OnMessage OnMessageEvent;

        private readonly CryptoHandler _cryptoHandler;

        #endregion

        #region .ctor()

        private Receiver(IPC_Types.OnMessage onMessage, CryptoHandler cryptoHandler)
        {
            AwaitingResponse = new();
            SystemMessageHandlers = new();

            OnMessageEvent += onMessage;

            _cryptoHandler = cryptoHandler;

            _tMain = new Thread(Worker)
            {
                IsBackground = true,
            };
        }

        internal Receiver(NamedPipeServerStream ipcServerInStream, IPC_Types.OnMessage onMessage, CryptoHandler cryptoHandler) : this(onMessage, cryptoHandler)
        {
            _receivePool = new(ipcServerInStream);

            _tMain.Start();
        }

        internal Receiver(NamedPipeClientStream ipcClientInStream, IPC_Types.OnMessage onMessage, CryptoHandler cryptoHandler) : this(onMessage, cryptoHandler)
        {
            _receivePool = new(ipcClientInStream);

            _tMain.Start();
        }

        #endregion

        #region Private Methods

        private void Worker()
        {
            while (true)
            {
                if (_receivePool.TryReadHeader(out IPC_Types.MessageHeader header) &&
                    _receivePool.TryRead(header.MessageLength, out SharedRaw<byte> messageRaw))
                {
                    byte[] message;
                    if (header.Encrypted)
                        message = _cryptoHandler.Decrypt(messageRaw.ReadOnlySpan);
                    else
                        message = messageRaw.Array;

                    if (IPC_Types.IsSystemMessage(header.MessageType))
                    {
                        IPC_Types.eSystemMessage systemMessageType = (IPC_Types.eSystemMessage)header.MessageType;

                        if (SystemMessageHandlers.TryGetValue(systemMessageType, out Action<IPC_Types.MessageHeader, byte[]> action))
                            action.Invoke(header, message);
                        else
                            Console.WriteLine($"[RECEIVER] -> Worker(): No action is bound to eSystemMessage type \"{header.MessageType}\"!");
                    }
                    else
                    {
                        if (AwaitingResponse.TryRemove(header.TransactionID, out Action<byte[]> onResponse))
                            onResponse.Invoke(message);
                        else
                            OnMessageEvent.Invoke(header, message.AsSpan());
                    }

                    messageRaw.Dispose();
                }
            }
        }

        #endregion
    }
}
