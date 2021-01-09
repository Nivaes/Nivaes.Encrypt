namespace Nivaes
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public static class EncryptHelper
    {
        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        public static async Task<string> Encrypt(string plainText, string password)
        {
            if (plainText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = string.Empty;
            }

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            using var sha256 = SHA256.Create();
            passwordBytes = sha256.ComputeHash(passwordBytes);

            var bytesEncrypted = await Encrypt(bytesToBeEncrypted, passwordBytes).ConfigureAwait(true);

            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public static async Task<string> Decrypt(string encryptedText, string password)
        {
            if (encryptedText == null)
            {
                return null;
            }

            if (password == null)
            {
                password = string.Empty;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using var sha256 = SHA256.Create();
            passwordBytes = sha256.ComputeHash(passwordBytes);

            var bytesDecrypted = await Decrypt(bytesToBeDecrypted, passwordBytes).ConfigureAwait(true);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static async Task<byte[]> Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            using (RijndaelManaged AES = new RijndaelManaged())
            {
#if NETSTANDARD2_0
                    using var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
#else
                using var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000, HashAlgorithmName.SHA256);
#endif

                AES.KeySize = 256;
                AES.BlockSize = 128;
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
#if NETSTANDARD2_0
                    await cs.WriteAsync(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length).ConfigureAwait(true);
#else
                    await cs.WriteAsync(bytesToBeEncrypted).ConfigureAwait(true);
#endif
                    cs.Close();
                }

                encryptedBytes = ms.ToArray();
            }

            return encryptedBytes;
        }

        private static async Task<byte[]> Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
#if NETSTANDARD2_0
                    using var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
#else
                    using var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000, HashAlgorithmName.SHA256);
#endif

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
#if NETSTANDARD2_0
                        await cs.WriteAsync(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length).ConfigureAwait(true);
#else
                        await cs.WriteAsync(bytesToBeDecrypted).ConfigureAwait(true);
#endif
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
