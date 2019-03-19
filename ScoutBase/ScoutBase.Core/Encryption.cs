using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutBase.Core
{
    public static class Encryption
    {

        // provides simple encryption/decryption algorithms for strings

        public static string EncryptString(string s)
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

        public static string DecryptString (string s)
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

    }
}
