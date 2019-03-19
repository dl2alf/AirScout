using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinTest
{
    public enum WTMESSAGES
    {
        NONE = 0,
        STATUS = 1,
        PKTRCVD = 2,
        SETAZIMUTH = 250,
        SETELEVATION = 251,
        ASSETPATH = 252,
        ASSHOWPATH = 253,
        ASNEAREST = 254,
        UNKNOWN = 255
    }

    public class wtMessage
    {
        public WTMESSAGES Msg;
        public string Src;
        public string Dst;
        public string Data;
        public string Extra;
        public byte Checksum;
        public bool HasChecksum;

        public wtMessage(byte[] bytes)
        {
            Msg = WTMESSAGES.NONE;
            Src = "";
            Dst = "";
            Data = "";
            Extra = "";
            Checksum = 0;
            HasChecksum = false;
            this.FromBytes(bytes);
        }

        public wtMessage(WTMESSAGES MSG, string src, string dst, string data)
        {
            Msg = MSG;
            Src = src;
            Dst = dst;
            Data = data;
            Extra = "";
            Checksum = 0;
            HasChecksum = false;
        }

        public wtMessage(WTMESSAGES MSG, string src, string dst, string data, string extra)
        {
            Msg = MSG;
            Src = src;
            Dst = dst;
            Data = data;
            Extra = extra;
            Checksum = 0;
            HasChecksum = false;
        }

        public void FromBytes(byte[] bytes)
        {
            // convert bytes coded in UTF8 to text
            string text = Encoding.ASCII.GetString(bytes);
            string msg = text.Substring(0, text.IndexOf(": "));
            try
            {
                Msg = (WTMESSAGES)Enum.Parse(typeof(WTMESSAGES), msg);
            }
            catch
            {
                Msg = WTMESSAGES.UNKNOWN;
            }
            text = text.Remove(0, text.IndexOf(": ") + 2);
            Src = text.Substring(0, text.IndexOf(" ")).Replace("\"", "");
            text = text.Remove(0, text.IndexOf(" ") + 1);
            Dst = text.Substring(0, text.IndexOf(" ")).Replace("\"", "");
            text = text.Remove(0, text.IndexOf(" ") + 1);
            // Clean up the message --> scrub last byte
            text = text.Substring(0, text.Length - 1).Replace("\"", "");
            // convert bytes coded in UTF8 to text
            Data = text.Substring(0, text.Length);
            // get checksum
            Checksum = (byte)(bytes[bytes.Length - 1] | 0x80);
            byte sum = 0;
            for (int i = 0; i < bytes.Length-1; i++)
                sum += bytes[i];
            sum = (byte)(sum | 0x80);
            if (Checksum == sum)
                HasChecksum = true;
            else
                HasChecksum = false;
        }

        public byte[] ToBytes()
        {
            byte[] b = null;
            string s;
            try
            {
                switch (Msg)
                {
                    case WTMESSAGES.NONE:
                        break;
                    case WTMESSAGES.UNKNOWN:
                        break;
                    default:
                        // combine all fields to string incl. placeholder for checksum
                        s = Msg + ": " + "\"" + Src + "\" \"" + Dst + "\" \"" + Data + "\"" + Extra + "?";
                        // translate into ASCII bytes
                        b = Encoding.ASCII.GetBytes(s);
                        // calculate checksum
                        byte sum = 0;
                        for (int i = 0; i < b.Length-1; i++)
                            sum += b[i];
                        sum = (byte)(sum | (byte)0x80);
                        b[b.Length - 1] = sum;
                        break;
                }
            }
            catch
            {
                throw new ArgumentOutOfRangeException(Msg.ToString(), "Unkwon Message.");
            }
            return b;
        }
    }

    public class WinTest
    {
    }
}
