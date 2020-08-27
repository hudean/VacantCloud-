using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MyCoreMvc.Common.EncryptionHelper
{
    /// <summary>
    /// AES加解密
    /// </summary>
    public class AESHelper
    {
        //默认密钥向量
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="encryptKey">加密key</param>
        /// <returns></returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = StringExtensions.GetSubString(encryptKey, 0, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = Keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="decryptKey">解密key</param>
        /// <returns></returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = StringExtensions.GetSubString(decryptKey, 0, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }
    }
}
