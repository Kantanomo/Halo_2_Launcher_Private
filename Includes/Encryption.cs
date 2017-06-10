using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace H2Shield.Includes
{
    public class Encryption
    {
        private byte[] _salt = Encoding.ASCII.GetBytes("GIVEITTOMEDICK");

        public static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length) throw new SystemException("Stream did not contain properly formatted byte array");

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length) throw new SystemException("Did not read byte array properly");

            return buffer;
        }

        public string EncryptStringAES(string plainText)
        {
            string Secret = "";
            ManagementClass Class = new ManagementClass("win32_processor");
            ManagementObjectCollection Collec = Class.GetInstances();

            foreach (ManagementObject Obj in Collec)
            {
                if (Secret == "")
                {
                    Secret = Obj.Properties["processorID"].Value.ToString().Substring(0, 8);
                    break;
                }
            }

            if (!string.IsNullOrEmpty(plainText) & !string.IsNullOrEmpty(Secret))
            {
                string outStr = null;
                RijndaelManaged aesAlg = null;

                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Secret, _salt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) swEncrypt.Write(plainText);

                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally { if (aesAlg != null) aesAlg.Clear(); }    
                return outStr;
            }
            return "";
        }

        public string DecryptStringAES(string cipherText)
        {
            string Secret = "";
            ManagementClass Class = new ManagementClass("win32_processor");
            ManagementObjectCollection Collec = Class.GetInstances();
            foreach (ManagementObject Obj in Collec)
            {
                if (Secret == "")
                {
                    Secret = Obj.Properties["processorID"].Value.ToString().Substring(0, 8);
                    break;
                }
            }
            if (!string.IsNullOrEmpty(cipherText) & !string.IsNullOrEmpty(Secret))
            {
                RijndaelManaged aesAlg = null;
                string plaintext = null;

                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Secret, _salt);
                    byte[] bytes = Convert.FromBase64String(cipherText);

                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        aesAlg = new RijndaelManaged();
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                        aesAlg.IV = ReadByteArray(msDecrypt);
                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) plaintext = srDecrypt.ReadToEnd();
                    }
                }
                finally { if (aesAlg != null) aesAlg.Clear(); }
                return plaintext;
            }
            return "";
        }
    }
}
