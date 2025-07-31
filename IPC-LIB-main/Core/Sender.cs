using IPC_LIB.Crypto;
using System.Collections.Concurrent;
using System.IO.Pipes;

namespace IPC_LIB.Core
{
    internal sealed class Sender
    {
        #region Fields

        private readonly NamedPipeServerStream _ipcServerOutStream;
        private readonly NamedPipeClientStream _ipcClientOutStream;

        private readonly Receiver _receiver;

        private readonly bool _encrypted;
        private readonly CryptoHandler _cryptoHandler;

        /// <summary>
        /// Whether or not this instance is for the IPC Server.
        /// </summary>
        private readonly bool _isServer;

        private const int MaxQueuedMessages = 20;
        private readonly BlockingCollection<Transaction> _queuedMessages;
        private ulong _transactionIndex;

        private readonly Thread _tMain;

        #endregion

        #region .ctor()

        private Sender(Receiver receiver, bool encrypted, CryptoHandler cryptoHandler)
        {
            _receiver = receiver;

            _encrypted = encrypted;
            _cryptoHandler = cryptoHandler;

            // Message queue
            _queuedMessages = new(MaxQueuedMessages);

            // Worker thread
            _tMain = new Thread(Worker)
            {
                IsBackground = true
            };
        }

        internal Sender(NamedPipeServerStream ipcServerOutStream, Receiver receiver, bool encrypted, CryptoHandler cryptoHandler) : this(receiver, encrypted, cryptoHandler)
        {
            _ipcServerOutStream = ipcServerOutStream;

            _isServer = true;

            _tMain.Start();
        }

        internal Sender(NamedPipeClientStream ipcClientOutStream, Receiver receiver, bool encrypted, CryptoHandler cryptoHandler) : this(receiver, encrypted, cryptoHandler)
        {
            _ipcClientOutStream = ipcClientOutStream;

            _transactionIndex = 999;

            _isServer = false;

            _tMain.Start();
        }

        #endregion

        #region Public Methods

        public void SendMessage(int messageType, ReadOnlySpan<byte> message, bool forcePlaintext = false) => SendMessageInternal(messageType, message, ulong.MaxValue, forcePlaintext);
        public void SendMessage(int messageType, byte[] message, bool forcePlaintext = false) => SendMessageInternal(messageType, message.AsSpan(), ulong.MaxValue, forcePlaintext);
        public void SendResponse(int messageType, byte[] message, ulong transactionID, bool forcePlaintext = false) => SendMessageInternal(messageType, message.AsSpan(), transactionID, forcePlaintext);
        public void GetResponse(int messageType, byte[] message, Action<byte[]> onResponse, bool forcePlaintext = false)
        {
            ulong transactionID = Interlocked.Increment(ref _transactionIndex);

            _receiver.AwaitingResponse.TryAdd(transactionID, onResponse);

            SendMessageInternal(messageType, message.AsSpan(), transactionID, forcePlaintext);
        }

        #endregion

        #region Private Methods

        private void SendMessageInternal(int messageType, ReadOnlySpan<byte> messageRaw, ulong transactionID = ulong.MaxValue, bool forcePlaintext = false)
        {
            if (transactionID == ulong.MaxValue)
                transactionID = Interlocked.Increment(ref _transactionIndex);

            ReadOnlySpan<byte> message;
            bool encrypted = _encrypted == true && forcePlaintext == false;

            if (encrypted)
                message = _cryptoHandler.Encrypt(messageRaw).AsSpan();
            else
                message = messageRaw;

            Transaction transaction = new(transactionID, messageType, encrypted, message);

            _queuedMessages.Add(transaction);
        }

        private void Worker()
        {
            while (true)
            {
                using Transaction transaction = _queuedMessages.Take();

                if (_isServer)
                {
                    _ipcServerOutStream.Write(transaction.Payload.ReadOnlySpan);
                    _ipcServerOutStream.Flush();
                }
                else
                {
                    _ipcClientOutStream.Write(transaction.Payload.ReadOnlySpan);
                    _ipcClientOutStream.Flush();
                }
            }
        }

        #endregion
    }
}
