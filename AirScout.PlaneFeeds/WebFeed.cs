using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Globalization;
using System.Net;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using AirScout.Core;
using AirScout.Aircrafts;
using AirScout.PlaneFeeds.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoutBase.Core;
using AirScout.AircraftPositions;
using static ScoutBase.Core.ZIP;

namespace AirScout.PlaneFeeds
{
    [Serializable]
    public class PlanefeedSettings_WF : PlaneFeedSettings
    {
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Base URL for website.")]
        public virtual string URL
        {
            get
            {
                return Properties.Settings.Default.WF_URL;
            }
            set
            {
                // detect change in URL
                if (Properties.Settings.Default.WF_URL != value)
                {
                    // reset array indices
                    ResetIndex();
                }
                Properties.Settings.Default.WF_URL = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Update interval for website request [seconds]")]
        public virtual int Interval
        {
            get
            {
                return Properties.Settings.Default.WF_Interval;
            }
            set
            {
                Properties.Settings.Default.WF_Interval = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Save downloaded JSON to file")]
        public virtual bool SaveToFile
        {
            get
            {
                return Properties.Settings.Default.WF_SaveToFile;
            }
            set
            {
                Properties.Settings.Default.WF_SaveToFile = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(true)]
        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Try to find all Indices automatically")]
        public virtual bool AutoIndex
        {
            get
            {
                return Properties.Settings.Default.WF_AutoIndex;
            }
            set
            {
                // detect change from false to true
                if (!Properties.Settings.Default.WF_AutoIndex && value)
                {
                    // reset array indices
                    ResetIndex();
                }
                Properties.Settings.Default.WF_AutoIndex = value;
                Properties.Settings.Default.Save();

            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for UTC")]
        [XmlElement("Index_UTC")]
        public virtual int Index_UTC
        {
            get
            {
                return Properties.Settings.Default.WF_Index_UTC;
            }
            set
            {
                Properties.Settings.Default.WF_Index_UTC = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Hex")]
        public virtual int Index_Hex
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Hex;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Hex = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Call")]
        public virtual int Index_Call
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Call;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Call = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Latitude")]
        public virtual int Index_Lat
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Lat;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Lat = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Longitde")]
        public virtual int Index_Lon
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Lon;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Lon = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Altitude")]
        public virtual int Index_Alt
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Alt;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Alt = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Speed")]
        public virtual int Index_Speed
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Speed;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Speed = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Registration")]
        public virtual int Index_Reg
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Reg;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Reg = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Flight Code")]
        public virtual int Index_Flight
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Flight;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Flight = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Track")]
        public virtual int Index_Track
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Track;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Track = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Type")]
        public virtual int Index_Type
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Type;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Type = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Squawk")]
        public virtual int Index_Squawk
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Squawk;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Squawk = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Radar")]
        public virtual int Index_Radar
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Radar;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Radar = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Route")]
        public virtual int Index_Route
        {
            get
            {
                return Properties.Settings.Default.WF_Index_Route;
            }
            set
            {
                Properties.Settings.Default.WF_Index_Route = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Minimum Count of Elements per Plane Position Data Set")]
        public virtual int Min_Elements
        {
            get
            {
                return Properties.Settings.Default.WF_Min_Elements;
            }
            set
            {
                Properties.Settings.Default.WF_Min_Elements = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Minium Count of Plane Position Data Sets")]
        public virtual int Min_Planes
        {
            get
            {
                return Properties.Settings.Default.WF_Min_Planes;
            }
            set
            {
                Properties.Settings.Default.WF_Min_Planes = value;
                Properties.Settings.Default.Save();
            }
        }

        private void ResetIndex()
        {
            Index_Alt = -1;
            Index_Call = -1;
            Index_Hex = -1;
            Index_Lat = -1;
            Index_Lon = -1;
            Index_Reg = -1;
            Index_Flight = -1;
            Index_Speed = -1;
            Index_Squawk = -1;
            Index_Track = -1;
            Index_Radar = -1;
            Index_Route = -1;
            Index_Type = -1;
            Index_UTC = -1;
        }

    }

    public class PlaneFeed_WF : PlaneFeed
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return Properties.Settings.Default.WF_Name; ;
            }
            protected set
            {
                Properties.Settings.Default.WF_Name = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.WF_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.WF_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.WF_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.WF_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Info
        {
            get
            {
                return Properties.Settings.Default.WF_Info;
            }
            protected set
            {
                Properties.Settings.Default.WF_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        public new PlanefeedSettings_WF FeedSettings = new PlanefeedSettings_WF();

        private Dictionary<string, JProperty> JProperties = new Dictionary<string, JProperty>();
        private Dictionary<string, JArray> JArrays = new Dictionary<string, JArray>();

        private SortedList<int, int> Maj = new SortedList<int, int>(new DuplicateKeyComparer<int>());

        string[][] Values = new string[0][];
        string[][] Names = new string[0][];

        string[][] Hexs = new string[0][];
        string[][] Regs = new string[0][];
        string[][] Flights = new string[0][];
        string[][] Calls = new string[0][];
        double[][] Lats = new double[0][];
        double[][] Lons = new double[0][];
        int[][] Alts = new int[0][];
        int[][] Speeds = new int[0][];
        int[][] Tracks = new int[0][];
        int[][] Squawks = new int[0][];
        string[][] Radars = new string[0][];
        string[][] Routes = new string[0][];
        string[][] Types = new string[0][];
        DateTime[][] UTCs = new DateTime[0][];

        int[] Maj_Hexs = new int[0];
        int[] Maj_Regs = new int[0];
        int[] Maj_Flights = new int[0];
        int[] Maj_Lats = new int[0];
        int[] Maj_Calls = new int[0];
        int[] Maj_Lons = new int[0];
        int[] Maj_Alts = new int[0];
        int[] Maj_Speeds = new int[0];
        int[] Maj_Tracks = new int[0];
        int[] Maj_Squawks = new int[0];
        int[] Maj_Radars = new int[0];
        int[] Maj_Routes = new int[0];
        int[] Maj_Types = new int[0];
        int[] Maj_UTCs = new int[0];

        double[] Min_Lats = new double[0];
        double[] Min_Lons = new double[0];
        int[] Min_Alts = new int[0];
        int[] Min_Speeds = new int[0];
        int[] Min_Tracks = new int[0];

        double[] Max_Lats = new double[0];
        double[] Max_Lons = new double[0];
        int[] Max_Alts = new int[0];
        int[] Max_Speeds = new int[0];
        int[] Max_Tracks = new int[0];

        int[] Min_Chars = new int[0];
        int[] Max_Chars = new int[0];

        public PlaneFeed_WF()
            : base ()
        {
            HasSettings = true;
            CanImport = true;
            CanExport = true;

        }

        private class DuplicateKeyComparer<TKey>
            : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = y.CompareTo(x);
                if (result == 0)
                    return -1;   // Handle equality as beeing smaller
                else
                    return result;
            }
        }

        private void ParseToken(JToken token)
        {
            foreach (var t in token.Children())
            {
                try
                {
                    if (t.GetType() == typeof(JProperty))
                    {
                        string number = JProperties.Count.ToString("00000000");
                        JProperties.Add(number, (JProperty)t);
                    }
                    else if (t.GetType() == typeof(JArray))
                    {
                        // generate a unique key in case of no parent property is found
                        string number = JArrays.Count.ToString("00000000");
                        // try to use parent's property name as a key
                        if (t.Parent.GetType() == typeof(JProperty))
                        {
                            string pval = ((JProperty)t.Parent).Name;
                            JArray a;
                            if (!JArrays.TryGetValue(pval, out a))
                            {
                                JArrays.Add(pval, (JArray)t);
                            }
                            else
                            {
                                // use number as unique key
                                JArrays.Add(number, (JArray)t);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // do nothing
                }
                ParseToken(t);
            }
        }

        private bool Is_Double(string s)
        {
            bool b;
            try
            {
                // double should contain only following chars
                b = !s.ToCharArray().Any(c => !"+-.,E0123456789".Contains(c));
                // double must contain a decimal separator
                if (b)
                {
                    b = s.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        private bool Is_Integer(string s)
        {
            bool b;
            try
            {
                b = !s.ToCharArray().Any(c => !"0123456789abcdefABCDEF".Contains(c));
            }
            catch (Exception ex)
            {
                b = false;
            }
            return b;
        }

        private void SortMaj(int[] maj)
        {
            // add all majorities > 0 and sort it by value
            // keep the original array index
            Maj.Clear();
            for (int i = 0; i < maj.Length; i++)
            {
                if (maj[i] > 0)
                    Maj.Add(maj[i], i);
            }
        }

        private void ClearMajsByIndex(int index)
        {
            Maj_Hexs[index] = 0;
            Maj_Regs[index] = 0;
            Maj_Flights[index] = 0;
            Maj_Lats[index] = 0;
            Maj_Calls[index] = 0;
            Maj_Lons[index] = 0;
            Maj_Alts[index] = 0;
            Maj_Speeds[index] = 0;
            Maj_Tracks[index] = 0;
            Maj_Squawks[index] = 0;
            Maj_Radars[index] = 0;
            Maj_Routes[index] = 0;
            Maj_Types[index] = 0;
            Maj_UTCs[index] = 0;
        }

        private void InitializeArrays(int size_i, int size_j)
        {

            Values = new string[size_i][]; 
            for (int i = 0; i < Values.Length; i++) 
            { 
                Values[i] = new string[size_j];
                for (int j = 0; j < Values[i].Length; j++)
                {
                    Values[i][j] = "";
                }
            }
            Names = new string[size_i][]; 
            for (int i = 0; i < Names.Length; i++) 
            { 
                Names[i] = new string[size_j];
                for (int j = 0; j < Names[i].Length; j++)
                {
                    Names[i][j] = "";
                }
            }

            Hexs = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Hexs[i] = new string[size_j]; }
            Regs = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Regs[i] = new string[size_j]; }
            Flights = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Flights[i] = new string[size_j]; }
            Calls = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Calls[i] = new string[size_j]; }
            Lats = new double[size_i][]; for (int i = 0; i < Values.Length; i++) { Lats[i] = new double[size_j]; }
            Lons = new double[size_i][]; for (int i = 0; i < Values.Length; i++) { Lons[i] = new double[size_j]; }
            Alts = new int[size_i][]; for (int i = 0; i < Values.Length; i++) { Alts[i] = new int[size_j]; }
            Speeds = new int[size_i][]; for (int i = 0; i < Values.Length; i++) { Speeds[i] = new int[size_j]; }
            Tracks = new int[size_i][]; for (int i = 0; i < Values.Length; i++) { Tracks[i] = new int[size_j]; }
            Squawks = new int[size_i][]; for (int i = 0; i < Values.Length; i++) { Squawks[i] = new int[size_j]; }
            Radars = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Radars[i] = new string[size_j]; }
            Routes = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Routes[i] = new string[size_j]; }
            Types = new string[size_i][]; for (int i = 0; i < Values.Length; i++) { Types[i] = new string[size_j]; }
            UTCs = new DateTime[size_i][]; for (int i = 0; i < Values.Length; i++) { UTCs[i] = new DateTime[size_j]; }

            Maj_Hexs = new int[size_j];
            Maj_Regs = new int[size_j];
            Maj_Flights = new int[size_j];
            Maj_Lats = new int[size_j];
            Maj_Calls = new int[size_j];
            Maj_Lons = new int[size_j];
            Maj_Alts = new int[size_j];
            Maj_Speeds = new int[size_j];
            Maj_Tracks = new int[size_j];
            Maj_Squawks = new int[size_j];
            Maj_Radars = new int[size_j];
            Maj_Routes = new int[size_j];
            Maj_Types = new int[size_j];
            Maj_UTCs = new int[size_j];

            Min_Lats = new double[size_j];
            Min_Lons = new double[size_j];
            Min_Alts = new int[size_j];
            Min_Speeds = new int[size_j];
            Min_Tracks = new int[size_j];
            Max_Lats = new double[size_j];
            Max_Lons = new double[size_j];
            Max_Alts = new int[size_j];
            Max_Speeds = new int[size_j];
            Max_Tracks = new int[size_j];
            Min_Chars = new int[size_j];
            Max_Chars = new int[size_j];
        }

        private void ReadValuesFromArrays()
        {
            // analyze a random dataset
            int i1 = new Random().Next(JArrays.Count - 1);
            JArray a = JArrays.Values.ElementAt(i1);
            int size_i = JArrays.Count;
            int size_j = a.Children().Count() + 1;
            // initialize arrays for values
            InitializeArrays(size_i, size_j);
            // copy strings to array
            for (int i = 0; i < size_i; i++)
            {
                try
                {
                    Values[i][0] = JArrays.Keys.ElementAt(i);
                    a = JArrays.Values.ElementAt(i);
                    for (int j = 0; j < a.Count; j++)
                    {
                        // check all values and try to find the majorities
                        string s = a[j].ToString();
                        Values[i][j + 1] = s;
                    }
                }
                catch
                {
                }
            }
        }

        private void ReadValuesFromProperties()
        {
            Dictionary<string, int> properties = new Dictionary<string, int>();
            foreach (JProperty p in JProperties.Values)
            {
                int count;
                if (!properties.TryGetValue(p.Name, out count))
                {
                    properties.Add(p.Name, 1);
                }
                else
                {
                    properties[p.Name]++;
                }
            }
            // remove all properties with less count from list
            for (int i = properties.Count - 1; i >= 0; i--)
            {
                if (properties.ElementAt(i).Value < FeedSettings.Min_Planes)
                    properties.Remove(properties.Keys.ElementAt(i));
            }
            // find maximum and set ascending index
            int max = 0;
            for (int i = 0; i < properties.Keys.Count; i++ )
            {
                if (properties.ElementAt(i).Value > max)
                    max = properties.ElementAt(i).Value;
                properties[properties.ElementAt(i).Key] = i;
            }
            // initialize arrays
            InitializeArrays(max, properties.Count);
            int row = -1;
            string parent = "";
            foreach (JProperty p in JProperties.Values)
            {
                if (p.Parent.Path != parent)
                {
                    row++;
                    parent = p.Parent.Path;
                }
                int j;
                if (properties.TryGetValue(p.Name, out j))
                    Values[row][j] = p.Value.ToString();
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "AutoJSON_Values.csv"))
                {
                    for (int i = 0; i < Values.Length; i++)
                    {
                        for (int j = 0; j < Values[i].Length; j++)
                        {
                            sw.Write(Values[i][j] + ";");
                        }
                        sw.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AutoJSON] Error while writing file: " + ex.Message);
            }
        }

        private bool IsIndexed()
        { 
            // check for basic indices available
            if (FeedSettings.Index_UTC < 0)
                return false;
            if (FeedSettings.Index_Hex < 0)
                return false;
            if (FeedSettings.Index_Call < 0)
                return false;
            if (FeedSettings.Index_Lat < 0)
                return false;
            if (FeedSettings.Index_Lon < 0)
                return false;
            if (FeedSettings.Index_Alt < 0)
                return false;
            if (FeedSettings.Index_Track < 0)
                return false;
            if (FeedSettings.Index_Speed < 0)
                return false;
            return true;
        }

        private void AutoIndexArrays()
        {
            PlaneInfoConverter pic = new PlaneInfoConverter();

            for (int i = 0; i < Values.Length; i++)
            {
                for (int j = 0; j < Values[i].Length; j++)
                {
                    try
                    {
                        // check all values and try to find the majorities
                        string s = Values[i][j];
                        if (String.IsNullOrEmpty(s))
                            continue;
                        // store Min/Max string lenghts
                        if (s.Length < Min_Chars[j])
                            Min_Chars[j] = s.Length;
                        if (s.Length > Max_Chars[j])
                            Max_Chars[j] = s.Length;
                        // check for double values first
                        if (Is_Double(s))
                        {
                            double lat = pic.To_Lat(s);
                            Lats[i][j] = lat;
                            if (lat != double.MinValue)
                            {
                                Maj_Lats[j]++;
                                if (lat < Min_Lats[j])
                                    Min_Lats[j] = lat;
                                if (lat > Max_Lats[j])
                                    Max_Lats[j] = lat;
                            }
                            double lon = pic.To_Lon(s);
                            Lons[i][j] = lon;
                            if (lon != double.MinValue)
                            {
                                Maj_Lons[j]++;
                                if (lon < Min_Lons[j])
                                    Min_Lons[j] = lon;
                                if (lon > Max_Lons[j])
                                    Max_Lons[j] = lon;
                            }
                            continue;
                        }
                        // check for integer
                        if (Is_Integer(s))
                        {
                            DateTime utc = pic.To_UTC(s);
                            UTCs[i][j] = utc;
                            if (utc != DateTime.MinValue)
                            {
                                Maj_UTCs[j]++;
                            }
                            string hex = pic.To_Hex(s);
                            Hexs[i][j] = hex;
                            if (hex != null)
                            {
                                Maj_Hexs[j]++;
                            }
                            int alt = pic.To_Alt(s);
                            Alts[i][j] = alt;
                            if (alt != int.MinValue)
                            {
                                Maj_Alts[j]++;
                                if (alt < Min_Alts[j])
                                    Min_Alts[j] = alt;
                                if (alt > Max_Alts[j])
                                    Max_Alts[j] = alt;
                            }
                            int speed = pic.To_Speed(s);
                            Speeds[i][j] = speed;
                            if (speed != int.MinValue)
                            {
                                Maj_Speeds[j]++;
                                if (speed < Min_Speeds[j])
                                    Min_Speeds[j] = speed;
                                if (speed > Max_Speeds[j])
                                    Max_Speeds[j] = speed;
                            }
                            int track = pic.To_Track(s);
                            Tracks[i][j] = track;
                            if (track != int.MinValue)
                            {
                                Maj_Tracks[j]++;
                                if (track < Min_Tracks[j])
                                    Min_Tracks[j] = track;
                                if (track > Max_Tracks[j])
                                    Max_Tracks[j] = track;
                            }
                            int squawk = pic.To_Squawk(s);
                            Squawks[i][j] = squawk;
                            if (squawk != int.MinValue)
                            {
                                Maj_Squawks[j]++;
                            }
                            continue;
                        }
                        // the rest is string
                        string reg = pic.To_Reg(s);
                        Regs[i][j] = reg;
                        if (reg != null)
                        {
                            Maj_Regs[j]++;
                        }
                        string flight = pic.To_Flight(s);
                        Flights[i][j] = flight;
                        if (flight != null)
                        {
                            Maj_Flights[j]++;
                        }
                        string radar = pic.To_Radar(s);
                        Radars[i][j] = radar;
                        if (radar != null)
                        {
                            Maj_Radars[j]++;
                        }
                        string route = pic.To_Route(s);
                        Routes[i][j] = route;
                        if (route != null)
                        {
                            Maj_Routes[j]++;
                        }
                        string call = pic.To_Call(s);
                        Calls[i][j] = call;
                        if (call != null)
                        {
                            Maj_Calls[j]++;
                        }
                        string type = pic.To_Type(s);
                        Types[i][j] = type;
                        if (type != null)
                        {
                            Maj_Types[j]++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Values[" + i.ToString() + "][" + j.ToString() + "]=" + Values[i][j] + ":" + ex.Message);
                    }
                }
            }
            // do the forensic work now
            if (FeedSettings.Index_UTC < 0)
            {
                SortMaj(Maj_UTCs);
                foreach (KeyValuePair<int,int> maj in Maj)
                {
                    // UTC must have >=10 chars
                    if (Max_Chars[maj.Value] >= 10)
                    {
                        FeedSettings.Index_UTC = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Hex < 0)
            {
                SortMaj(Maj_Hexs);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Hex must have 6 chars
                    if (Max_Chars[maj.Value] == 6)
                    {
                        FeedSettings.Index_Hex = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Squawk < 0)
            {
                SortMaj(Maj_Squawks);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Squawk must have 4 chars
                    if (Max_Chars[maj.Value] == 4)
                    {
                        FeedSettings.Index_Squawk = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Alt < 0)
            {
                SortMaj(Maj_Alts);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Alt will have > 3 chars and max. values > 10000 feet
                    if ((Max_Chars[maj.Value] > 3) && (Max_Alts[maj.Value] > 10000))
                    {
                        FeedSettings.Index_Alt = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Track < 0)
            {
                SortMaj(Maj_Tracks);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Track will have <= 3 chars and hopefully at last on plane between 180 and 360
                    if ((Max_Chars[maj.Value] <= 3) && (Max_Tracks[maj.Value] > 180) && (Max_Tracks[maj.Value] < 360))
                    {
                        FeedSettings.Index_Track = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Speed < 0)
            {
                SortMaj(Maj_Speeds);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Speed will have <= 3 chars and max > 100 and < 800
                    if ((Max_Chars[maj.Value] <= 3) && (Max_Speeds[maj.Value] > 100) && (Max_Speeds[maj.Value] <= 800))
                    {
                        FeedSettings.Index_Speed = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Lat < 0)
            {
                SortMaj(Maj_Lats);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Lat will have max >= -90 and <= 90
                    if ((Min_Lats[maj.Value] >= -90) && (Max_Tracks[maj.Value] <= 90))
                    {
                        FeedSettings.Index_Lat = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Lon < 0)
            {
                SortMaj(Maj_Lons);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Lon will have max >= -180 and <= 180
                    if ((Min_Lons[maj.Value] >= -180) && (Max_Tracks[maj.Value] <= 180))
                    {
                        FeedSettings.Index_Lon = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Type < 0)
            {
                SortMaj(Maj_Types);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // type will have >= 2 chars
                    if (Max_Chars[maj.Value] >= 2)
                    {
                        FeedSettings.Index_Type = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Flight < 0)
            {
                SortMaj(Maj_Flights);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    FeedSettings.Index_Flight = maj.Value;
                    ClearMajsByIndex(maj.Value);
                    break;
                }
            }
            if (FeedSettings.Index_Reg < 0)
            {
                SortMaj(Maj_Regs);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // Reg will have > 3 chars
                    if (Max_Chars[maj.Value] > 3)
                    {
                        FeedSettings.Index_Reg = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Route < 0)
            {
                SortMaj(Maj_Routes);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    FeedSettings.Index_Route = maj.Value;
                    ClearMajsByIndex(maj.Value);
                    break;
                }
            }
            if (FeedSettings.Index_Radar < 0)
            {
                SortMaj(Maj_Radars);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // radar will have > 2 chars
                    if (Max_Chars[maj.Value] > 2)
                    {
                        FeedSettings.Index_Radar = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
            if (FeedSettings.Index_Call < 0)
            {
                SortMaj(Maj_Calls);
                foreach (KeyValuePair<int, int> maj in Maj)
                {
                    // reg will have > 3 chars
                    if (Max_Chars[maj.Value] > 3)
                    {
                        FeedSettings.Index_Call = maj.Value;
                        ClearMajsByIndex(maj.Value);
                        break;
                    }
                }
            }
        }

        private List<AircraftPositionDesignator> ReadPlanesFromArray()
        {
            List<AircraftPositionDesignator> l = new List<AircraftPositionDesignator>();
            PlaneInfoConverter pic = new PlaneInfoConverter();
            for (int i = 0; i < Values.Length; i++)
            {
                PlaneInfo info = new PlaneInfo();

                if (FeedSettings.Index_UTC >= 0)
                {
                    DateTime utc = pic.To_UTC(Values[i][FeedSettings.Index_UTC]);
                    if (utc != DateTime.MinValue)
                        info.Time = utc;
                }
                if (FeedSettings.Index_Hex >= 0)
                {
                    string hex = pic.To_Hex(Values[i][FeedSettings.Index_Hex]);
                    if (!String.IsNullOrEmpty(hex))
                        info.Hex = hex;
                }
                if (FeedSettings.Index_Squawk >= 0)
                {
                    int squawk = pic.To_Squawk(Values[i][FeedSettings.Index_Squawk]);
//                    if (squawk != int.MinValue)
//                        info.Squawk = squawk;
                }
                if (FeedSettings.Index_Alt >= 0)
                {
                    int alt = pic.To_Alt(Values[i][FeedSettings.Index_Alt]);
                    if (alt != int.MinValue)
                    {
                        info.Alt = alt;
                    }
                }
                if (FeedSettings.Index_Track >= 0)
                {
                    int track = pic.To_Track(Values[i][FeedSettings.Index_Track]);
                    if (track != int.MinValue)
                        info.Track = track;
                }
                if (FeedSettings.Index_Speed >= 0)
                {
                    int speed = pic.To_Speed(Values[i][FeedSettings.Index_Speed]);
                    if (speed != int.MinValue)
                        info.Speed = speed;
                }
                if (FeedSettings.Index_Lat >= 0)
                {
                    double lat = pic.To_Lat(Values[i][FeedSettings.Index_Lat]);
                    if (lat != double.MinValue)
                        info.Lat = lat;
                }
                if (FeedSettings.Index_Lon >= 0)
                {
                    double lon = pic.To_Lon(Values[i][FeedSettings.Index_Lon]);
                    if (lon != double.MinValue)
                        info.Lon = lon;
                }
                if (FeedSettings.Index_Type >= 0)
                {
                    string type = Values[i][FeedSettings.Index_Type];
                    if (!String.IsNullOrEmpty(type))
                        info.Type = type;
                }
                if (FeedSettings.Index_Reg >= 0)
                {
                    string reg = Values[i][FeedSettings.Index_Reg];
                    if (!String.IsNullOrEmpty(reg))
                        info.Reg = reg;
                }
                if (FeedSettings.Index_Call >= 0)
                {
                    string call = Values[i][FeedSettings.Index_Call];
                    if (!String.IsNullOrEmpty(call))
                        info.Call = call;
                }
                if (FeedSettings.Index_Radar >= 0)
                {
                    string radar = Values[i][FeedSettings.Index_Radar];
//                    if (!String.IsNullOrEmpty(radar))
//                        info.Radar = radar;
                }
                // complete info
                if (String.IsNullOrEmpty(info.Hex) && !String.IsNullOrEmpty(info.Reg))
                {
                    AircraftDesignator ad = AircraftData.Database.AircraftFindByReg(info.Reg);
                    if (ad != null)
                        info.Hex = ad.Hex;
                }
                // update aircraft database
                if (!String.IsNullOrEmpty(info.Hex))
                    AircraftData.Database.AircraftInsertOrUpdateIfNewer(new AircraftDesignator(info.Hex,info.Call, info.Reg, info.Type));
                // fill aircraft position table
                if (!String.IsNullOrEmpty(info.Hex) && 
                    !String.IsNullOrEmpty(info.Call) &&
                    (info.Lat >= MinLat) &&
                    (info.Lat <= MaxLat) &&
                    (info.Lon >= MinLon) &&
                    (info.Lon <= MaxLon) &&
                    (info.Alt_m >= MinAlt) &&
                    (info.Alt_m <= MaxAlt))
                {
                    l.Add(new AircraftPositionDesignator(info.Hex, info.Call, info.Lat, info.Lon, info.Alt, info.Track, info.Speed, info.Time));
                }
            }
            return l;
        }

        private void ReadFromURL(string url, string filename)
        {
            int count = 0;
            string json = "";
            DateTime start = DateTime.UtcNow;
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                if (!cl.DownloadFile(url, filename, false, true))
                    return;
                Log.WriteMessage("Reading file from url successful. Try to deserialize JSON...");
                // check if file exists
                if (!File.Exists(filename))
                    return;
                // deserialize JSON file
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    json = sr.ReadToEnd();
                }
                JObject o = (JObject)JsonConvert.DeserializeObject(json);
                // clear collections
                JArrays.Clear();
                JProperties.Clear();
                // parse all child tokens recursively --> can be either a property or an array
                ParseToken(o);
                // we've got all properties and arrays here
                // assuming that plane info is stored in a structure with >10 elements
                // check the child token count
                try
                {
                    var propertychildcounts = JProperties.Values.Select(x => x.Count)
                       .GroupBy(x => x)
                       .Select(x => new { ChildCount = x.Key, Count = x.Count() })
                       .OrderByDescending(x => x)
                       .ToList();
                    // single properties?
                    if (propertychildcounts.Count > 0)
                    {
                        ReadValuesFromProperties();
                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    var arraychildcounts = JArrays.Values.Select(x => x.Count)
                        .GroupBy(x => x)
                        .Select(x => new { ChildCount = x.Key, Count = x.Count() })
                        .OrderByDescending(x => x)
                        .ToList();
                    // checking number of elements to determine the organisation of values
                    // arrays of values?
                    if ((arraychildcounts != null) && (arraychildcounts.Count > 0) && (arraychildcounts[0].ChildCount > FeedSettings.Min_Elements) && (arraychildcounts[0].Count > FeedSettings.Min_Planes))
                    {
                        // assuming that we have data organized in arrays
                        // find the majority of element counts
                        // remove all arrays with different element count from list
                        for (int i = JArrays.Count - 1; i >= 0; i--)
                        {
                            if (JArrays.Values.ElementAt(i).Count != arraychildcounts[0].ChildCount)
                                JArrays.Remove(JArrays.Keys.ElementAt(i));
                        }
                        // convert all properties into the values array of strings
                        ReadValuesFromArrays();
                    }
                }
                catch (Exception ex)
                {
                }
                // auto-index if necessary
                if (FeedSettings.AutoIndex && !IsIndexed())
                    AutoIndexArrays();
                // read data
                if (IsIndexed())
                {
                    if (FeedSettings.SaveToFile)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(Path.Combine(TmpDirectory, "AutoJSON_Values.csv")))
                            {
                                for (int i = 0; i < Values.Length; i++)
                                {
                                    // write header
                                    if (i == 0)
                                    {
                                        string[] a = new string[Values[i].Length];
                                        for (int j = 0; j < a.Length; j++)
                                            a[j] = "[unknown]";
                                        if (FeedSettings.Index_UTC >= 0)
                                        {
                                            a[FeedSettings.Index_UTC] = "UTC";
                                        }
                                        if (FeedSettings.Index_Hex >= 0)
                                        {
                                            a[FeedSettings.Index_Hex] = "Hex";
                                        }
                                        if (FeedSettings.Index_Squawk >= 0)
                                        {
                                            a[FeedSettings.Index_Squawk] = "Squawk";
                                        }
                                        if (FeedSettings.Index_Alt >= 0)
                                        {
                                            a[FeedSettings.Index_Alt] = "Alt";
                                        }
                                        if (FeedSettings.Index_Track >= 0)
                                        {
                                            a[FeedSettings.Index_Track] = "Track";
                                        }
                                        if (FeedSettings.Index_Speed >= 0)
                                        {
                                            a[FeedSettings.Index_Speed] = "Speed";
                                        }
                                        if (FeedSettings.Index_Lat >= 0)
                                        {
                                            a[FeedSettings.Index_Lat] = "Lat";
                                        }
                                        if (FeedSettings.Index_Lon >= 0)
                                        {
                                            a[FeedSettings.Index_Lon] = "Lon";
                                        }
                                        if (FeedSettings.Index_Type >= 0)
                                        {
                                            a[FeedSettings.Index_Type] = "Type";
                                        }
                                        if (FeedSettings.Index_Reg >= 0)
                                        {
                                            a[FeedSettings.Index_Reg] = "Reg";
                                        }
                                        if (FeedSettings.Index_Call >= 0)
                                        {
                                            a[FeedSettings.Index_Call] = "Call";
                                        }
                                        if (FeedSettings.Index_Radar >= 0)
                                        {
                                            a[FeedSettings.Index_Radar] = "Radar";
                                        }
                                        for (int j = 0; j < Values[i].Length; j++)
                                        {
                                            if (j > 0)
                                                sw.Write(";");
                                            sw.Write(a[j]);
                                        }
                                        sw.WriteLine();

                                    }
                                    // write values
                                    for (int j = 0; j < Values[i].Length; j++)
                                    {
                                        sw.Write(Values[i][j] + ";");
                                    }
                                    sw.WriteLine();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage("[AutoJSON] Error while writing file: " + ex.Message);
                        }
                    }
                    List<AircraftPositionDesignator> l = ReadPlanesFromArray();
                    if ((l != null) && (l.Count > 0))
                    {
                        
                        // ReportProgress((int)PROGRESS.PLANES, planes);
                        // AircraftData.Database.AircraftPositionBulkInsertOrUpdateIfNewer(l);
                    }
                    count = l.Count;
                }
            }
            catch (Exception ex)
            {
                // report error
                ReportProgress(-1, ex.Message);
                Log.WriteMessage(ex.Message);
            }
            DateTime stop = DateTime.UtcNow;
            string msg = "[" + start.ToString("HH:mm:ss") + "] " +
                count.ToString() + " Positions updated, " +
                (stop - start).Milliseconds.ToString() + " ms.";
            this.ReportProgress((int)PROGRESS.STATUS, msg);
            Log.WriteMessage(msg);
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            PlaneFeedWorkEventArgs args = (PlaneFeedWorkEventArgs)e.Argument;
            // set parameters from arguments
            AppDirectory = args.AppDirectory;
            AppDataDirectory = args.AppDataDirectory;
            LogDirectory = args.LogDirectory;
            TmpDirectory = args.TmpDirectory;
            DatabaseDirectory = args.DatabaseDirectory;
            MaxLat = args.MaxLat;
            MinLon = args.MinLon;
            MinLat = args.MinLat;
            MaxLon = args.MaxLon;
            MyLat = args.MyLat;
            MyLon = args.MyLon;
            DXLat = args.DXLat;
            DXLon = args.DXLon;
            MinAlt = args.MinAlt;
            MaxAlt = args.MaxAlt;

            // intialize variables
            VC.AddVar("APPDIR", AppDirectory);
            VC.AddVar("DATADIR", AppDataDirectory);
            VC.AddVar("LOGDIR", LogDirectory);
            VC.AddVar("DATABASEDIR", DatabaseDirectory);
            VC.AddVar("MINLAT", MinLat);
            VC.AddVar("MAXLAT", MaxLat);
            VC.AddVar("MINLON", MinLon);
            VC.AddVar("MAXLON", MaxLon);
            VC.AddVar("MINALTM", MinAlt);
            VC.AddVar("MAXALTM", MaxAlt);
            VC.AddVar("MINALTFT", (int)UnitConverter.m_ft((double)MinAlt));
            VC.AddVar("MAXALTFT", (int)UnitConverter.m_ft((double)MaxAlt));


            // check boundaries
            if ((MaxLat <= MinLat) || (MaxLon <= MinLon))
            {
                Status = STATUS.ERROR;
                this.ReportProgress((int)PROGRESS.ERROR, "Area boundaries mismatch. Check your Covered Area parameters!");
                Log.WriteMessage("Area boundaries mismatch. Check your Covered Area parameters!");
            }
            else
            {
                Status = STATUS.OK;
                int interval = Properties.Settings.Default.WF_Interval;
                // run loop
                do
                {
                    // calculate url and get json
                    String url = VC.ReplaceAllVars(FeedSettings.URL);
                    ReadFromURL(url, Path.Combine(TmpDirectory, "AutoJSON.json"));
                    // sleep interval
                    // don't use Thread.Sleep() here as this is not interruptable
                    int i = 0;
                    while (!CancellationPending && (i < interval))
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                }
                while (!this.CancellationPending || (Status != STATUS.OK));
            }
            this.ReportProgress((int)PROGRESS.FINISHED);
            Log.WriteMessage("Finished.");
        }

        public override Object GetFeedSettings()
        {
            return FeedSettings;
        }

        public override void Import()
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = "*.feed";
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds | .feed";
            Dlg.CheckFileExists = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer s = new XmlSerializer(typeof(PlanefeedSettings_WF));
                FeedSettings = (PlanefeedSettings_WF)s.Deserialize(File.OpenRead(Dlg.FileName));
            }
        }

        public override void Export()
        {
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds | .feed";
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer s = new XmlSerializer(typeof(PlanefeedSettings_WF));
                s.Serialize(File.Create(Dlg.FileName), FeedSettings);
            }

        }
    }
}
