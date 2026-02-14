using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ScoutBase.Core
{
    public static class Encryption
    {

        // provides simple encryption/decryption algorithms for strings

        public static string SimpleEncryptString(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            byte[] b = Encoding.ASCII.GetBytes(s);
            for (byte i = 0; i < b.Length; i++)
            {
                b[i] = System.Convert.ToByte(b[i] + i);
                if (b[i] >= 128)
                    b[i] -= 96;
            }
            return Encoding.UTF8.GetString(b);
        }

        public static string SimpleDecryptString (string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            byte[] b = Encoding.ASCII.GetBytes(s);
            for (byte i = 0; i < b.Length; i++)
            {
                b[i] = System.Convert.ToByte(b[i] - i);
                if (b[i] < 32)
                    b[i] += 96;
            }
            return Encoding.UTF8.GetString(b);
        }

        /// <summary>
        /// Decodes an OpenSSL-encrypted (AES256-CBC) string.<br></br><br></br>
        /// The equivalent encoding in PHP is like:<br></br><br></br>
        /// $encrypt_method = "AES-256-CBC";<br></br>
        /// $secret_key = hash('md5',$key);<br></br>
        /// $encoded = openssl_encrypt($data, $encrypt_method, $secret_key);
        /// </summary>
        /// <param name="encrypteddata">The encrypted string (Base64 encoded).</param>
        /// <param name="pwd">The password as a string.</param>
        /// <returns>The decoded string.</returns>
        public static string OpenSSLDecrypt(string encrypteddata, string pwd)
        {
            // create a 32bit MD5 hash of the password
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pwd));
            StringBuilder sb = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            // use the MD5 hash as the key
            byte[] key = Encoding.UTF8.GetBytes(sb.ToString());
            //get the encrypted data as byte[]
            byte[] encrypted = Convert.FromBase64String(encrypteddata);
            //setup an empty iv
            var iv = new byte[16];
            // Declare the RijndaelManaged object used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold the decrypted text.
            string decrypted;

            // Create a RijndaelManaged object
            // with the specified key and IV.
            aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // Create the streams used for decryption.
            using (MemoryStream ms = new MemoryStream(encrypted))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        decrypted = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            // return decrypted string
            return decrypted;
        }

        /// <summary>
        /// Encodes a string into OpenSSL-encrypted (AES256-CBC) value.<br></br><br></br>
        /// The equivalent decoding in PHP is like:<br></br><br></br>
        /// $encrypt_method = "AES-256-CBC";<br></br>
        /// $secret_key = hash('md5',$key);<br></br>
        /// $decoded = openssl_decrypt($data, $encrypt_method, $secret_key);
        /// </summary>
        /// <param name="datatoencrypt">The string to encrypt.</param>
        /// <param name="pwd">The password as a string.</param>
        /// <returns>The encoded string as Base64</returns>
        public static string OpenSSLEncrypt(string datatoencrypt, string pwd)
        {
            // create a 32bit MD5 hash of the password
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pwd));
            StringBuilder sb = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            // use the MD5 hash as the key
            byte[] key = Encoding.UTF8.GetBytes(sb.ToString());
            //setup an empty iv
            var iv = new byte[16];
            // Declare the RijndaelManaged object used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold the decrypted text.
            string encrypted;

            // Create a RijndaelManaged object
            // with the specified key and IV.
            aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

            // Create a encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            // Create the streams used for decryption.
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(datatoencrypt);
                        sw.Close();
                    }
                    cs.Close();
                }
                byte[] encoded = ms.ToArray();
                encrypted = Convert.ToBase64String(encoded);
            }

            // return decrypted string
            return encrypted;
        }
    }
}
