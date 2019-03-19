using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutBase.Core;
using System.Reflection;

namespace AirScout.Aircrafts
{
    public enum PLANECATEGORY
    {
        [StringCustomAttribute("None")]
        NONE = 0,
        [StringCustomAttribute("Light")]
        LIGHT = 1,
        [StringCustomAttribute("Medium")]
        MEDIUM = 2,
        [StringCustomAttribute("Heavy")]
        HEAVY = 3,
        [StringCustomAttribute("Superheavy")]
        SUPERHEAVY = 4,
    }

    public static class PlaneCategories
    {
        public static string GetName(PLANECATEGORY cat)
        {
            return Enum.GetName(typeof(PLANECATEGORY), cat);
        }

        public static string[] GetNames()
        {
            return Enum.GetNames(typeof(PLANECATEGORY));
        }

        public static string[] GetNamesExceptNone()
        {
            PLANECATEGORY[] bandsexceptnone = GetValuesExceptNone();
            string[] namesexceptnone = new string[bandsexceptnone.Length];
            for (int i = 0; i < bandsexceptnone.Length; i++)
                namesexceptnone[i] = GetName(bandsexceptnone[i]);
            return namesexceptnone;
        }

        public static PLANECATEGORY[] GetValues()
        {
            return (PLANECATEGORY[])Enum.GetValues(typeof(PLANECATEGORY));
        }

        public static PLANECATEGORY[] GetValuesExceptNone()
        {
            PLANECATEGORY[] cats = (PLANECATEGORY[])Enum.GetValues(typeof(PLANECATEGORY));
            PLANECATEGORY[] catsexceptnone = new PLANECATEGORY[cats.Length - 1];
            int i = 0;
            foreach (PLANECATEGORY cat in cats)
            {
                if (cat != PLANECATEGORY.NONE)
                {
                    catsexceptnone[i] = cat;
                    i++;
                }
            }
            return catsexceptnone;
        }

        public static string GetStringValue(PLANECATEGORY cat)
        {
            string output = null;
            FieldInfo fi = typeof(PLANECATEGORY).GetField(cat.ToString());
            StringCustomAttribute[] attrs = fi.GetCustomAttributes(typeof(StringCustomAttribute), false) as StringCustomAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }

        public static string GetShortStringValue (PLANECATEGORY cat)
        {
            string output = null;
            FieldInfo fi = typeof(PLANECATEGORY).GetField(cat.ToString());
            StringCustomAttribute[] attrs = fi.GetCustomAttributes(typeof(StringCustomAttribute), false) as StringCustomAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value.Substring(0, 1).ToUpper();
            }
            return output;
        }

        public static string[] GetStringValues()
        {
            List<string> cats = new List<string>();
            foreach (PLANECATEGORY cat in Enum.GetValues(typeof(PLANECATEGORY)))
            {
                cats.Add(GetStringValue(cat));
            }
            if (cats.Count > 0)
                return cats.ToArray();
            return null;
        }

        public static string[] GetStringValuesExceptNone()
        {
            List<string> cats = new List<string>();
            foreach (PLANECATEGORY cat in Enum.GetValues(typeof(PLANECATEGORY)))
            {
                if (cat != PLANECATEGORY.NONE)
                    cats.Add(GetStringValue(cat));
            }
            if (cats.Count > 0)
                return cats.ToArray();
            return null;
        }

        public static PLANECATEGORY ParseStringValue(string catstr)
        {
            foreach (PLANECATEGORY c in PlaneCategories.GetValuesExceptNone())
            {
                if (PlaneCategories.GetStringValue(c) == catstr)
                    return c;

            }
            return PLANECATEGORY.NONE;
        }

    }
}
