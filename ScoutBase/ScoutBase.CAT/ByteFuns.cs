using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ScoutBase.CAT
{
    public static class ByteFuns
    {
        public static bool BytesEqual(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }
            return true;
        }

        // the length of Arr1 and Arr2 must be verified before this function is called
        public static byte[] BytesAnd(byte[] a1, byte[] a2)
        {
            int len = Math.Min(a1.Length, a2.Length);
            byte[] result = new byte[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = (byte)(a1[i] & a2[i]);
            }
            return result;
        }

        public static void BytesReverse(ref byte[] a)
        {
            Array.Reverse(a, 0, a.Length);
        }

        public static string BytesToStr(byte[] a)
        {
            if (a == null)
                return null;

            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            string s = MyEncoding.GetString(a);
            return s;
        }

        public static byte[] StrToBytes(string s)
        {
            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            return MyEncoding.GetBytes(s);
        }

        public static string BytesToHex(byte[] a)
        {
            return BitConverter.ToString(a).Replace("-", "");
        }

        public static string StrToHex(string s)
        {
            return BytesToHex(StrToBytes(s));
        }

    }
}
