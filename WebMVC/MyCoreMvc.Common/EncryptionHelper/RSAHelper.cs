using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace VaCant.Common.EncryptionHelper
{
    /// <summary>
    /// RSA加密验证
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="data">需要加密的字符串对象</param>
        /// <param name="privateKeyPem">私钥</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string RSASignCharSet(string data, string privateKeyPem, string charset = "utf-8")
        {
            byte[] signatureBytes = null;
            try
            {
                RSACryptoServiceProvider rsaCsp = null;
                //字符串获取
                rsaCsp = LoadCertificateString(privateKeyPem);

                byte[] dataBytes = null;
                if (string.IsNullOrEmpty(charset))
                {
                    dataBytes = Encoding.UTF8.GetBytes(data);
                }
                else
                {
                    dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
                }
                if (null == rsaCsp)
                {
                    return string.Empty;
                }
                signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return Convert.ToBase64String(signatureBytes);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadCertificateString(string strKey)
        {
            RSACryptoServiceProvider rsa = null;
            try
            {
                var data = Convert.FromBase64String(strKey);
                rsa = DecodeRSAPrivateKey(data);
                return rsa;
            }
            catch
            {
            }
            //异常继续执行
            try
            {
                rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(strKey);
                return rsa;
            }
            catch
            {
            }
            return rsa;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="data">需要验证的内容</param>
        /// <param name="signedString">签名结果</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="charset">编码格式</param>
        public static bool Verify(string data, string signedString, string publicKey, string charset = "utf-8")
        {
            var result = false;

            byte[] signedData;
            try
            {
                signedData = Convert.FromBase64String(signedString);
            }
            catch
            {
                return false;
            }

            Encoding code = Encoding.GetEncoding(charset);
            byte[] byteData = code.GetBytes(data);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);

            result = rsaPub.VerifyData(byteData, "SHA1", signedData);
            return result;
        }

        /// 解析.net 生成的Pem
        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }
    }
}
