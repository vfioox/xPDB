using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace xPDB.Encryption
{
    class Encryptor
    {
        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };
        public static string Encrypt(string plainText, string password)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (password == null || password.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = pdb.GetBytes(32);
                rijAlg.IV = pdb.GetBytes(16);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            string ciphertext = Convert.ToBase64String(encrypted);
            // Return the encrypted bytes from the memory stream. 
            return ciphertext;

        }
        public static string Decrypt(string cipherText, string password)
        {
            try
            {
                // Check arguments. 
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (password == null || password.Length <= 0)
                    throw new ArgumentNullException("Key");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                // Declare the string used to hold 
                // the decrypted text. 
                string plaintext = null;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
                // Create an RijndaelManaged object 
                // with the specified key and IV. 
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = pdb.GetBytes(32);
                    rijAlg.IV = pdb.GetBytes(16);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for decryption. 
                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream 
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }

                return plaintext;
            }
            catch
            {
                return null;
            }
        }
    }
}
