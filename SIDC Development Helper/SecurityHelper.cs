using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SIDC_Development_Helper
{
    /// <summary>
    /// Name: SIDC DLL Common Security Helper
    /// <para/>
    /// Developer: Angelito D. De Sagun
    /// <para/>
    /// Date: 2021-05-23
    /// <para/>
    /// Revision Date:--
    /// </summary>
    public class SecurityHelper
    {
        private string key = "CesInadDrewMonetteRuelArleneCarl";
        private string iv = "*sup3r5dm1n*";
        private char pad = 'x';

        /// <summary>
        /// Security Type: AesCryptoServiceProvider
        /// </summary>
        public SecurityHelper()
        {

        }

        /// <summary>
        /// Call this function to encrypt string using AesCryptoServiceProvider
        /// </summary>
        /// <param name="text">String to encrypt</param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            try
            {
                byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(text);

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
                    aes.IV = ASCIIEncoding.ASCII.GetBytes(iv.PadRight(16, pad));
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] encrypted = crypto.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

                    crypto.Dispose();

                    return Convert.ToBase64String(encrypted);

                }
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Call this function to decrypt string using AesCryptoServiceProvider
        /// </summary>
        /// <param name="encryptedText">Encrypted Text</param>
        /// <param name="_iv">Pattern</param>
        /// <returns></returns>
        public string Decrypt(string encryptedText, string _iv)
        {
            try
            {
                byte[] encrypted = Convert.FromBase64String(encryptedText);

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
                    aes.IV = ASCIIEncoding.ASCII.GetBytes(_iv.PadRight(16, pad));
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] secret = crypto.TransformFinalBlock(encrypted, 0, encrypted.Length);

                    crypto.Dispose();

                    return ASCIIEncoding.ASCII.GetString(secret);

                }
            }
            catch
            {

                throw;
            }
        }
    }
}
