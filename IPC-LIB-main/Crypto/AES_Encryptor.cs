using System.Security.Cryptography;

namespace IPC_LIB.Crypto
{
    internal sealed class AES_Encryptor : IDisposable
    {
        #region Fields

        private readonly Aes _aes;
        private readonly RSA_Helper _rsa;

        private byte[] IV => _aes.IV;
        private byte[] Key => _aes.Key;

        #endregion

        internal AES_Encryptor(RSA_Helper rsa)
        {
            _aes = Aes.Create();
            _rsa = rsa;
        }

        #region Public Methods

        public byte[] Encrypt(ReadOnlySpan<byte> data)
        {
            UpdateState();

            ReadOnlySpan<byte> ciphertext = _aes.EncryptCbc(data, IV).AsSpan();

            byte[] payload = CreatePayload(IV.AsSpan(), ciphertext);

            return payload;
        }

        /// <summary>
        /// Updates the Key used by the internal Aes instance.
        /// </summary>
        public void SetKey(byte[] key)
        {
            _aes.Key = key;
        }

        public void Dispose()
        {
            _aes.Dispose();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Securely updates the IV used by the internal Aes instance.
        /// </summary>
        private void UpdateState()
        {
            _aes.GenerateIV();
        }

        /// <summary>
        /// Combines the header and ciphertext.
        /// </summary>
        private static byte[] CreatePayload(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> ciphertext)
        {
            int payloadLength = AES_Helper.Sizes.IV + ciphertext.Length;
            byte[] payload = new byte[payloadLength];
            Span<byte> payloadSpan = payload.AsSpan();

            iv.CopyTo(payloadSpan);
            ciphertext.CopyTo(payloadSpan.Slice(AES_Helper.Sizes.IV));

            return payload;
        }

        #endregion
    }
}
