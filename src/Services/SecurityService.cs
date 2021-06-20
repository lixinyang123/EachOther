using System;
using System.Security.Cryptography;
using System.Text;
using EachOther.Models;

namespace EachOther.Services
{
    public class SecurityService : StaticConfig<Secret>
    {
        private readonly Aes aes;

        public SecurityService(string fileName, Secret initSecret) : base(fileName, initSecret)
        {
            aes = Aes.Create();
            aes.IV = Encoding.UTF8.GetBytes(Config.IV);
            aes.Key = Encoding.UTF8.GetBytes(Config.Key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(str);

            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

    }
}