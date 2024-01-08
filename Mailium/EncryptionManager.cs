using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mailium
{
    public class EncryptionManager
    {
        public static string EncryptSymmetric(string plainText)
        {
            byte[] encrypted;
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = Convert.FromBase64String(SharedSecretManager.GetSharedSecret());
                myAes.Padding = PaddingMode.PKCS7;
                myAes.GenerateIV();

                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, myAes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        encryptedStream.Write(myAes.IV, 0, myAes.IV.Length);

                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);

                        cryptoStream.FlushFinalBlock();
                    }
                    encrypted = encryptedStream.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptSymmetric(byte[] encryptedText, string clientId)
        {
            string decryptedText;
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = Convert.FromBase64String(SharedSecretManager.GetSharedSecret());
                myAes.Padding = PaddingMode.PKCS7;

                byte[] iv = new byte[myAes.BlockSize / 8];
                Array.Copy(encryptedText, 0, iv, 0, iv.Length);
                myAes.IV = iv;

                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(decryptedStream, myAes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedText, iv.Length, encryptedText.Length - iv.Length);
                    }
                    byte[] decryptedBytes = decryptedStream.ToArray();
                    decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            return decryptedText;
        }
    }
}
