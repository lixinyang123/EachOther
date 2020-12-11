using System;
using System.Security.Cryptography;
using System.Text;
using EachOther.Models;

namespace EachOther.Services
{
    public class SecurityService : StaticConfig<Secret>
    {
        private readonly RijndaelManaged rijndaelManaged;

        public SecurityService(string fileName, Secret initSecret) : base(fileName, initSecret)
        {
            rijndaelManaged = new RijndaelManaged
            {
                IV = Encoding.UTF8.GetBytes(Config.IV),
                Key = Encoding.UTF8.GetBytes(Config.Key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            ICryptoTransform cTransform = rijndaelManaged.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(str);

            ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

    }
}