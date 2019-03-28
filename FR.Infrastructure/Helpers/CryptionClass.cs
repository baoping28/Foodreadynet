using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace FR.Infrastructure.Helpers
{
    public class CryptionClass
    {
        private static RijndaelManaged cryptoProvider;
        private static readonly byte[] Key = { 18, 19, 8, 24, 36, 22, 4, 22, 17, 5, 11, 9, 13, 15, 06, 23 };
        private static readonly byte[] IV = { 14, 2, 16, 7, 5, 9, 17, 8, 4, 47, 16, 12, 1, 32, 25, 18 };
        static CryptionClass()
        {
            cryptoProvider = new RijndaelManaged();
            cryptoProvider.Mode = CipherMode.CBC;
            cryptoProvider.Padding = PaddingMode.PKCS7;
        }

        public static string Encrypt(string unencryptedString)
        {
            byte[] bytin = ASCIIEncoding.ASCII.GetBytes(unencryptedString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            cs.Write(bytin, 0, bytin.Length);
            cs.FlushFinalBlock();
            byte[] bytout = ms.ToArray();
            return Convert.ToBase64String(bytout);
        }

        public static string Decrypt(string encryptedString)
        {
            if (encryptedString.Trim().Length != 0)
            {
                byte[] bytin = Convert.FromBase64String(encryptedString);
                MemoryStream ms = new MemoryStream(bytin, 0, bytin.Length);
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            else
            {
                return "";
            }
        }
    }
}
