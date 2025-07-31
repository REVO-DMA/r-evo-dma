using System.Security.Cryptography;

namespace IPC_LIB.Crypto
{
    internal sealed class AES_Decryptor : IDisposable
    {
        #region

        private readonly Aes _aes;
        private readonly RSA_Helper _rsa;

        private byte[] IV => _aes.IV;
        private byte[] Key => _aes.Key;

        #endregion

        internal AES_Decryptor(RSA_Helper rsa)
        {
            _aes = Aes.Create();
            _rsa = rsa;
        }

        #region Public Methods

        public byte[] Decrypt(ReadOnlySpan<byte> data)
        {
            byte[] iv = ExtractIV(data);

            SetState(iv);

            ReadOnlySpan<byte> ciphertext = ExtractCiphertext(data);

            byte[] plaintext = _aes.DecryptCbc(ciphertext, IV);

            return plaintext;
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
        /// Sets the IV used by the internal Aes instance.
        /// </summary>
        private void SetState(byte[] iv)
        {
            _aes.IV = iv;
        }

        /// <summary>
        /// Extracts the IV from a payload.
        /// </summary>
        private static byte[] ExtractIV(ReadOnlySpan<byte> data)
        {
            ReadOnlySpan<byte> ivSpan = data.Slice(0, AES_Helper.Sizes.IV);
            return ivSpan.ToArray();
        }

        /// <summary>
        /// Extracts the ciphertext from a payload.
        /// </summary>
        private static ReadOnlySpan<byte> ExtractCiphertext(ReadOnlySpan<byte> data)
        {
            ReadOnlySpan<byte> ciphertext = data.Slice(AES_Helper.Sizes.IV);

            return ciphertext;
        }

        #endregion
    }
}
