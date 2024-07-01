using Infrastructure.helpers.IHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.helpers.Helper
{
    public class Encryption : IEncryption
    {
        private byte[] key = { };

        private byte[] IV = {
        0x12,
        0x34,
        0x56,
        0x78,
        0x90,
        0xab,
        0xcd,
        0xef};

        private string EncryptionKey = "!38#83!@$%38#83a#de@388Ad";

        public string Decrypt(string Input)
        {
            byte[] inputByteArray = new byte[Input.Length];
            try
            {
                key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(Input);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                Encoding encoding__1 = Encoding.UTF8;

                return encoding__1.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }

        }

        public string Encrypt(string Input)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(EncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(Input);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }
    }
}
