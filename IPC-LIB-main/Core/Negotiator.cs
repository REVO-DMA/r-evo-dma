using IPC_LIB.Core;
using IPC_LIB.Crypto;
using System.Security.Cryptography;

namespace IPC_LIB
{
    internal sealed class Negotiator
    {
        #region Fields

        private readonly RSA_Helper _rsaHelper;
        private readonly CryptoHandler _cryptoHandler;
        private readonly Receiver _receiver;
        private readonly Sender _sender;
        private readonly bool _isServer;

        private bool _rsaReady = false;
        private bool _aesReady = false;
        private IPC_Types.eRemoteStatus _remoteStatus = IPC_Types.eRemoteStatus.DEFAULT;
        
        private bool Initialized => _rsaReady && _aesReady;

        #endregion

        internal Negotiator(RSA_Helper rsaHelper, CryptoHandler cryptoHandler, Receiver receiver, Sender sender, bool isServer)
        {
            _rsaHelper = rsaHelper;
            _cryptoHandler = cryptoHandler;
            _receiver = receiver;
            _sender = sender;
            _isServer = isServer;
        }

        #region Public Methods

        /// <summary>
        /// Blocks while negotiating crypto keys.
        /// </summary>
        public void Negotiate()
        {
            BindSystemMessageHandlers();

            _sender.SendMessage((int)IPC_Types.eSystemMessage.REMOTE_STATUS, BitConverter.GetBytes((int)IPC_Types.eRemoteStatus.WAITING), true);

            while (_remoteStatus != IPC_Types.eRemoteStatus.WAITING)
                Thread.Sleep(1);

            Initialize();

            while (!Initialized)
                Thread.Sleep(1);

            _sender.SendMessage((int)IPC_Types.eSystemMessage.REMOTE_STATUS, BitConverter.GetBytes((int)IPC_Types.eRemoteStatus.READY), true);

            while (_remoteStatus != IPC_Types.eRemoteStatus.READY)
                Thread.Sleep(1);
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            GetRemotePublicKey();

            if (_isServer)
                SendAesKey();
        }

        private void BindSystemMessageHandlers()
        {
            _receiver.SystemMessageHandlers.Add(IPC_Types.eSystemMessage.RSA_PUBLIC_KEY, (header, _) =>
            {
                _sender.SendResponse(-1, _rsaHelper.ExportPublicKey(), header.TransactionID, true);
            });

            _receiver.SystemMessageHandlers.Add(IPC_Types.eSystemMessage.AES_KEY, (header, data) =>
            {
                _cryptoHandler.SetAesKey(data);

                _aesReady = true;
            });

            _receiver.SystemMessageHandlers.Add(IPC_Types.eSystemMessage.REMOTE_STATUS, (_, status) =>
            {
                _remoteStatus = (IPC_Types.eRemoteStatus)BitConverter.ToInt32(status);
            });
        }

        private void GetRemotePublicKey()
        {
            _sender.GetResponse((int)IPC_Types.eSystemMessage.RSA_PUBLIC_KEY, Array.Empty<byte>(), (publicKey) =>
            {
                _rsaHelper.CreateRemoteRSA(publicKey);

                _rsaReady = true;
            }, true);
        }

        private void SendAesKey()
        {
            using Aes aes = Aes.Create();
            aes.GenerateKey();
            byte[] key = aes.Key;

            _sender.SendMessage((int)IPC_Types.eSystemMessage.AES_KEY, key, true);

            _cryptoHandler.SetAesKey(key.AsSpan());

            _aesReady = true;
        }

        #endregion
    }
}
