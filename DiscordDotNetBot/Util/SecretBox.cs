using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DiscordDotNetBot.Util
{
    public static class SecretBox
    {
        private const int KEY_LENGTH = 256;
        private const int ITERATIONS = 1000;
        private const int DERIVED_KEY_LENGTH = 16;

        public static byte[] GenerateKey()
        {
            byte[] key = new byte[KEY_LENGTH];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(key);
            }
            return key;
        }

        public static string CalculateChecksum(byte[] data)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] salt)
        {
            byte[] encryptedData;
            using (var aes = Aes.Create())
            {
                key = GetAesDecryptionKey(key, salt);

                aes.Key = key;
                aes.IV = key;
                
                using (var encryptor = aes.CreateEncryptor())
                {
                    using (var memStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data);
                        }
                        encryptedData = memStream.ToArray();
                    }
                }
            }
            return encryptedData;
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] salt)
        {
            List<byte> decryptedData = new List<byte>();
            byte[] decryptionBuffer = new byte[1000];
            using (var aes = Aes.Create())
            {
                key = GetAesDecryptionKey(key, salt);

                aes.Key = key;
                aes.IV = key;

                using (var decryptor = aes.CreateDecryptor())
                {
                    using (var memStream = new MemoryStream(data))
                    {
                        using (var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                        {
                            int readBytes = cryptoStream.Read(decryptionBuffer, 0, decryptionBuffer.Length);
                            var segment = new ArraySegment<byte>(decryptionBuffer, 0, readBytes);
                            decryptedData.AddRange(segment);
                        }
                    }
                }
            }
            return decryptedData.ToArray();
        }

        private static byte[] GetAesDecryptionKey(byte[] password, byte[] salt)
        {
            using (var sha1 = SHA256.Create())
            {
                byte[] hashedSalt = sha1.ComputeHash(salt); // ensures valid salt length

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, hashedSalt, ITERATIONS))
                {
                    return pbkdf2.GetBytes(DERIVED_KEY_LENGTH);
                }
            }
        }

    }
}
