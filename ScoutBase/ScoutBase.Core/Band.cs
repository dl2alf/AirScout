using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScoutBase.Core
{
    public enum BAND
    {
        [StringCustomAttribute("None")]
        BNONE = 0,
        [StringCustomAttribute("50M")]
        B50M = 50,
        [StringCustomAttribute("70M")]
        B70M = 70,
        [StringCustomAttribute("144M")]
        B144M = 144,
        [StringCustomAttribute("432M")]
        B432M = 432,
        [StringCustomAttribute("1.2G")]
        B1_2G = 1296,
        [StringCustomAttribute("2.3G")]
        B2_3G = 2320,
        [StringCustomAttribute("3.4G")]
        B3_4G = 3400,
        [StringCustomAttribute("5.7G")]
        B5_7G = 5760,
        [StringCustomAttribute("10G")]
        B10G = 10368,
        [StringCustomAttribute("24G")]
        B24G = 24048,
        [StringCustomAttribute("47G")]
        B47G = 47088,
        [StringCustomAttribute("76G")]
        B76G = 76032,
        [StringCustomAttribute("All")]
        BALL = 999999999
    }

    public static class Bands
    {
        public static string GetName(BAND band)
        {
            return Enum.GetName(typeof(BAND), band);
        }

        public static string[] GetNames()
        {
            return Enum.GetNames(typeof(BAND));
        }

        public static string[] GetNamesExceptNoneAndAll()
        {
            BAND[] bandsexceptnone = GetValuesExceptNoneAndAll();
            string[] namesexceptnone = new string[bandsexceptnone.Length];
            for (int i = 0; i < bandsexceptnone.Length; i++)
                namesexceptnone[i] = GetName(bandsexceptnone[i]);
            return namesexceptnone;
        }

        public static BAND[] GetValues()
        {
            return (BAND[])Enum.GetValues(typeof(BAND));
        }

        public static BAND[] GetValuesExceptNoneAndAll()
        {
            BAND[] bands = (BAND[])Enum.GetValues(typeof(BAND));
            BAND[] bandsexceptnoneandall = new BAND[bands.Length - 2];
            int i = 0;
            foreach(BAND band in bands)
            {
                if ((band != BAND.BNONE) && (band != BAND.BALL))
                {
                    bandsexceptnoneandall[i] = band;
                    i++;
                }
            }
            return bandsexceptnoneandall;
        }

        public static string GetStringValue(BAND band)
        {
            string output = null;
            FieldInfo fi = typeof(BAND).GetField(band.ToString());
            StringCustomAttribute[] attrs = fi.GetCustomAttributes(typeof(StringCustomAttribute), false) as StringCustomAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }


        public static string[] GetStringValues()
        {
            List<string> bands = new List<string>();
            foreach (BAND band in Enum.GetValues(typeof(BAND)))
            {
                bands.Add(GetStringValue(band));
            }
            if (bands.Count > 0)
                return bands.ToArray();
            return null;
        }

        public static string[] GetStringValuesExceptNoneAndAll()
        {
            List<string> bands = new List<string>();
            foreach (BAND band in Enum.GetValues(typeof(BAND)))
            {
                if ((band != BAND.BNONE) && (band != BAND.BALL))
                    bands.Add(GetStringValue(band));
            }
            if (bands.Count > 0)
                return bands.ToArray();
            return null;
        }

        public static BAND ParseStringValue(string bandstr)
        {
            foreach (BAND b in Bands.GetValuesExceptNoneAndAll())
            {
                if (String.Compare(Bands.GetStringValue(b), bandstr,true) == 0)
                    return b;

            }
            return BAND.BNONE;
        }

        public static BAND Next(BAND band)
        {
            BAND[] bands = Bands.GetValuesExceptNoneAndAll();
            int i = Array.IndexOf(bands, band);
            if (i < 0)
                i = 0;
            else if (i > bands.Length - 2)
                i = bands.Length - 2;
            return bands[i + 1];
        }

        public static BAND Previous(BAND band)
        {
            BAND[] bands = Bands.GetValuesExceptNoneAndAll();
            int i = Array.IndexOf(bands, band);
            if (i < 1)
                i = 1;
            else if (i > bands.Length - 1)
                i = bands.Length - 1;
            return bands[i - 1];
        }

        public static double To100Hz(BAND band)
        {
            return (int)band *10000.0;
        }

        public static double TokHz(BAND band)
        {
            return (int)band * 1000.0;
        }

        public static double ToMHz(BAND band)
        {
            return (int)band;
        }

        public static double ToGHz(BAND band)
        {
            return (int)band / 1000.0;
        }
    }
}
