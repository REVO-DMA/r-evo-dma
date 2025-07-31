using System.Security.Cryptography;

namespace IPC_LIB.Crypto
{
    internal sealed class RSA_Helper : IDisposable
    {
        #region Fields

        public readonly struct Sizes
        {
            public const int Key = 128;
        }

        private readonly RSA _localRSA;
        private RSA _remoteRSA;

        #endregion

        internal RSA_Helper()
        {
            _localRSA = RSA.Create(Sizes.Key * 8);
        }

        #region Public Methods

        public byte[] Encrypt(ReadOnlySpan<byte> data) => _remoteRSA.Encrypt(data, RSAEncryptionPadding.OaepSHA256);

        public byte[] Decrypt(ReadOnlySpan<byte> data) => _localRSA.Decrypt(data, RSAEncryptionPadding.OaepSHA256);

        public byte[] ExportPublicKey() => _localRSA.ExportRSAPublicKey();

        public void CreateRemoteRSA(ReadOnlySpan<byte> publicKey)
        {
            _remoteRSA = RSA.Create();
            _remoteRSA.ImportRSAPublicKey(publicKey, out _);
        }

        public void Dispose()
        {
            _localRSA.Dispose();
            _remoteRSA?.Dispose();
        }

        #endregion
    }
}
