using System.Security.Cryptography;
using System.Text;

namespace SaveSystem.Internal.Utils
{
    internal static class SaveSystemEncryption
    {
        internal const string DEFAULT_KEY = "Save system default key reference";
        internal const string DEFAULT_IV = "Default IV reference";

        internal static void GenerateKey(ref byte[] key, int keySize, string reference)
        {
            key = new byte[keySize];

            var bytes = GetBytes(reference);
            for (int i = 0; i < keySize; i++)
            {
                key[i] = bytes[i % bytes.Length];
            }
        }

        internal static byte[] Encrypt(byte[] key, byte[] iv, string data)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = GetBytes(data);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }

                return encryptedBytes;
            }
        }

        internal static string Decrypt(byte[] key, byte[] iv, byte[] data)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;

                using (var msDecrypt = new System.IO.MemoryStream(data))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new System.IO.MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }

                return GetString(decryptedBytes);
            }
        }

        internal static byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
        internal static string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
