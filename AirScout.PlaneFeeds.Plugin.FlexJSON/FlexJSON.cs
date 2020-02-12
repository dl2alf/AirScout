using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Globalization;
using AirScout.PlaneFeeds.Plugin.MEFContract;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Windows.Forms;
using AirScout.PlaneFeeds.Plugin;
using System.Xml;
using System.Xml.Linq;

namespace AirScout.PlaneFeeds.Plugin.FlexJSON
{

    [Serializable]
    public class FlexJSONSettings
    {
        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for UTC [OPTIONAL]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_UTC { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Hex [OPTIONAL]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Hex { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Call [OPTIONAL]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Call { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Latitude [deg] [MANDATORY]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Lat { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Longitde [deg] [MANDATORY]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Lon { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Altitude [ft, m] [MANDATORY]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Alt { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Units for Altitude [ft, m]")]
        [DefaultValue("ft")]
        public string Units_Alt { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Speed [kts, km/h] [MANDATORY]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Speed { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Units for Speed [kts, km/h]")]
        [DefaultValue("kts")]
        public string Units_Speed { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Track in [deg] [MANDATORY]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Track { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Type [OPTIONAL]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Type { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Index for Registration [OPTIONAL]. Use -1 for detect, 255 for not used.")]
        [DefaultValue(-1)]
        public int Index_Reg { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Minimum Count of Elements per Plane Position Data Set")]
        [DefaultValue(10)]
        public int Min_Elements { get; set; }

        [CategoryAttribute("Data Field Properties")]
        [DescriptionAttribute("Minium Count of Plane Position Data Sets")]
        [DefaultValue(50)]
        public int Min_Planes { get; set; }

        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Save downloaded JSON to file")]
        [DefaultValue(false)]
        [XmlIgnore]
        public bool SaveToFile { get; set; }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Base URL for website.")]
        [DefaultValue("")]
        public string URL { get; set; }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Timeout for loading the site.")]
        [DefaultValue(30)]
        [XmlIgnore]
        public int Timeout { get; set; }

        public FlexJSONSettings()
        {
            Default();
            Load(true);
        }

        /// <summary>
        /// Sets all properties to their default value according to the [DefaultValue=] attribute
        /// </summary>
        public void Default()
        {
            // set all properties to their default values according to definition in [DeafultValue=]
            foreach (var p in this.GetType().GetProperties())
            {
                try
                {
                    // initialize all properties with default value if set
                    if (Attribute.IsDefined(p, typeof(DefaultValueAttribute)))
                    {
                        p.SetValue(this, ((DefaultValueAttribute)Attribute.GetCustomAttribute(
                            p, typeof(DefaultValueAttribute)))?.Value, null);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[" + this.GetType().Name + "]: Cannot set default value of: " + p.Name + ", " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Loads settings from a XML-formatted configuration file into settings.
        /// </summary>
        /// <param name="loadall">If true, ignore the [XmlIgnore] attribute, e.g. load all settings available in the file.<br>If false, load only settings without [XmlIgore] attrbute.</br></param>
        /// <param name="filename">The filename of the settings file.</param>
        public void Load(bool loadall, string filename = "")
        {
            // use standard filename if empty
            // be careful because Linux file system is case sensitive
            if (String.IsNullOrEmpty(filename))
                filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL", ".CFG")).LocalPath;
            // do nothing if file not exists
            if (!File.Exists(filename))
                return;
            try
            {
                string xml = "";
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    xml = sr.ReadToEnd();
                }
                XDocument xdoc = XDocument.Parse(xml);
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    if (!loadall)
                    {
                        // check on XmlIgnore attribute, skip if set
                        object[] attr = p.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
                        if (attr.Length > 0)
                            continue;
                    }
                    try
                    {
                        // get matching element
                        XElement typenode = xdoc.Element(this.GetType().Name);
                        if (typenode != null)
                        {
                            XElement element = typenode.Element(p.Name);
                            if (element != null)
                                p.SetValue(this, Convert.ChangeType(element.Value, p.PropertyType), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[" + this.GetType().Name + "]: Error while loading property[" + p.Name + " from " + filename + ", " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + this.GetType().Name + "]: Cannot load settings from " + filename + ", " + ex.Message);
            }
        }

        /// <summary>
        /// Saves settings from settings into a XML-formatted configuration file
        /// </summary>
        /// <param name="saveall">If true, ignore the [XmlIgnore] attribute, e.g. save all settings.<br>If false, save only settings without [XmlIgore] attrbute.</param>
        /// <param name="filename">The filename of the settings file.</param>
        public void Save(bool saveall, string filename = "")
        {
            // use standard filename if empty
            // be careful because Linux file system is case sensitive
            if (String.IsNullOrEmpty(filename))
                filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL",".CFG")).LocalPath;
            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            if (saveall)
            {
                // ovverride the XmlIgnore attributes to get all serialized
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    XmlAttributes attribs = new XmlAttributes { XmlIgnore = false };
                    overrides.Add(this.GetType(), p.Name, attribs);
                }
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Create(filename)))
                {
                    XmlSerializer s = new XmlSerializer(this.GetType(), overrides);
                    s.Serialize(sw, this);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot save settings to " + filename + ", " + ex.Message);
            }
        }
    }




    [Export(typeof(IPlaneFeedPlugin))]
    [ExportMetadata("Name", "PlaneFeedPlugin")]
    public class FlexJSONPlugin : IPlaneFeedPlugin
    {

        //********************************** START OF INTERFACE ******************************************

        public string Name
        {
            get
            {
                return "[WebFeed]           Flexible JSON";
            }
        }

        public string Info
        {
            get
            {
                return "Web feed with flexible JSON column assignment. Use it for webfeed with unknown content.\n" +
                       "(c) AirScout(www.airscout.eu)\n" +
                       "This feed tries to read unknown JSON data into an array of plane information.\n" +
                       "If this is successful, a \"forensic\" algorithm tries to find out which information is on which index.\n" +
                       "You can manually enter indices as well.See documentation for further details.";
            }
        }

        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public bool HasSettings
        {
            get
            {
                return true;
            }
        }

        public bool CanImport
        {
            get
            {
                return true;
            }
        }

        public bool CanExport
        {
            get
            {
                return true;
            }
        }
        public string Disclaimer
        {
            get
            {
                return "This plane feed might fetch data from an Internet server via Deep Link\n" + 
                        "technology(see http://en.wikipedia.org/wiki/Deep_link)\n." + 
                        "The use is probably not intended by the website owners and could be changed in URL and data format frequently and without further notice.\n" +
                        "Furthermore, it might cause legal issues in some countries.\n" +
                        "By clicking on \"Accept\" you understand that you are\n\n" +
                        "DOING THAT ON YOUR OWN RISK\n\n" +
                        "The auhor of this software will not be responsible in any case.";
            }
        }

        public string DisclaimerAccepted
        {
            get
            {
                return Settings.DisclaimerAccepted;
            }
            set
            {
                Settings.DisclaimerAccepted = value;
            }
        }

        public void ResetSettings()
        {
            Settings.Default();
        }

        public void LoadSettings()
        {
            Settings.Load(true);
        }

        public void SaveSettings()
        {
            Settings.Save(true);
        }

        public object GetSettings()
        {
            return this.Settings;
        }

        public void ImportSettings()
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = "*.feed";
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds | .feed";
            Dlg.CheckFileExists = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Settings.Load(false, Dlg.FileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot import from " + Dlg.FileName + ", " + ex.Message);
                }
            }
        }

        public void ExportSettings()
        {
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds | .feed";
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Settings.Save(false, Dlg.FileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot export to " + Dlg.FileName + ", " + ex.Message);
                }
            }

        }

        public void Start(PlaneFeedPluginArgs args)
        {
            // keep the TmpDirectory
            TmpDirectory = args.TmpDirectory;
        }

        public PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args)
        {
            // intialize variables
            VarConverter VC = new VarConverter();
            VC.AddVar("APPDIR", args.AppDirectory);
            VC.AddVar("DATADIR", args.AppDataDirectory);
            VC.AddVar("LOGDIR", args.LogDirectory);
            VC.AddVar("DATABASEDIR", args.DatabaseDirectory);
            VC.AddVar("MINLAT", args.MinLat);
            VC.AddVar("MAXLAT", args.MaxLat);
            VC.AddVar("MINLON", args.MinLon);
            VC.AddVar("MAXLON", args.MaxLon);
            VC.AddVar("MINALTM", args.MinAlt);
            VC.AddVar("MAXALTM", args.MaxAlt);
            VC.AddVar("MINALTFT", (int)UnitConverter.m_ft((double)args.MinAlt));
            VC.AddVar("MAXALTFT", (int)UnitConverter.m_ft((double)args.MaxAlt));
            // initialize plane info list
            PlaneFeedPluginPlaneInfoList planes = new PlaneFeedPluginPlaneInfoList();
            string json = "";
            // calculate url and get json
            String url = VC.ReplaceAllVars(Settings.URL);
            // check parameters
            if (String.IsNullOrEmpty(url))
                throw new ArgumentException("The URL is empty.");
            if (Settings.Timeout <= 0)
                throw new ArgumentException("The value for Timout is invalid: " + Settings.Timeout.ToString());
            Console.WriteLine("[" + this.GetType().Name + "]: Creating web request: " + url);
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.Referer = "";
            webrequest.Timeout = (int)Settings.Timeout * 1000;
            webrequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:72.0) Gecko/20100101 Firefox/72.0";
            webrequest.Accept = "application/json, text/javascript, */*;q=0.01";
            webrequest.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            Console.WriteLine("[" + this.GetType().Name + "]: Getting web response");
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Console.WriteLine("[" + this.GetType().Name + "]: Reading stream");
            using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }
            // save raw data to file if enabled
            if (Settings.SaveToFile)
            {
                using (StreamWriter sw = new StreamWriter(args.TmpDirectory + Path.DirectorySeparatorChar + this.GetType().Name + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss") + ".json"))
                {
                    sw.WriteLine(json);
                }
            }
            Console.WriteLine("[" + this.GetType().Name + "]: Analyzing data");
            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic root = js.Deserialize<dynamic>(json);
            // get all planeinfos in an array
            GetAllPlaneInfos(root);
            // try to find all indices automatically if not set
            // mind the search order!
            if (Settings.Index_Hex < 0)
            {
                FindHexIndex();
                Settings.Save(true);
            }
            if (Settings.Index_UTC < 0)
            { 
                FindUTCIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Type < 0)
            {
                FindTypeIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Lat < 0)
            { 
                FindLatIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Lon < 0)
            { 
                FindLonIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Alt < 0)
            { 
                FindAltIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Track < 0)
            { 
                FindTrackIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Speed < 0)
            {
                FindSpeedIndex();
                Settings.Save(true);
            }
            if (Settings.Index_Reg < 0)
            { 
                FindRegIndex();
                Settings.Save(true);
            }
            //  all must produce/have some mandatory data
            if (Settings.Index_Lat < 0)
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot get planes, Lat index not found/not set!");
            if (Settings.Index_Lon < 0)
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot get planes, Lon index not found/not set!");
            if (Settings.Index_Alt < 0)
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot get planes, Alt index not found/not set!");
            if (Settings.Index_Track < 0)
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot get planes, Track index not found/not set!");
            if (Settings.Index_Speed < 0)
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot get planes, Speed index not found/not set!");
            // fill plane info
            foreach (string[] info in PlaneList)
            {
                PlaneFeedPluginPlaneInfo plane = new PlaneFeedPluginPlaneInfo();
                if ((Settings.Index_Hex > 0) && (Settings.Index_Hex < info.Length))
                    plane.Hex = To_Hex(info[Settings.Index_Hex]);
                else
                    plane.Hex = "";
                if ((Settings.Index_UTC > 0) && (Settings.Index_UTC < info.Length))
                    plane.Time = To_UTC(info[Settings.Index_UTC]);
                else
                    plane.Time = DateTime.UtcNow;
                if ((Settings.Index_Lat > 0) && (Settings.Index_Lat < info.Length))
                    plane.Lat = To_Lat(info[Settings.Index_Lat]);
                else
                    plane.Lat = double.MinValue;
                if ((Settings.Index_Lon > 0) && (Settings.Index_Lon < info.Length))
                    plane.Lon = To_Lon(info[Settings.Index_Lon]);
                else
                    plane.Lon = double.MinValue;
                if ((Settings.Index_Alt > 0) && (Settings.Index_Alt < info.Length))
                    plane.Alt = To_Alt(info[Settings.Index_Alt]);
                else
                    plane.Alt = double.MinValue;
                if ((Settings.Index_Speed > 0) && (Settings.Index_Speed < info.Length))
                    plane.Speed = To_Speed(info[Settings.Index_Speed]);
                else
                    plane.Speed = int.MinValue;
                if ((Settings.Index_Track > 0) && (Settings.Index_Track < info.Length))
                    plane.Track = To_Track(info[Settings.Index_Track]);
                else
                    plane.Track = int.MinValue;
                if ((Settings.Index_Call > 0) && (Settings.Index_Call < info.Length))
                    plane.Call = To_Call(info[Settings.Index_Call]);
                else
                    plane.Call = "";
                if ((Settings.Index_Type > 0) && (Settings.Index_Type < info.Length))
                    plane.Type = To_Type(info[Settings.Index_Type]);
                else
                    plane.Type = "";
                if ((Settings.Index_Reg > 0) && (Settings.Index_Reg < info.Length))
                    plane.Reg = To_Reg(info[Settings.Index_Reg]);
                else
                    plane.Reg = "";
                planes.Add(plane);
            }
            Console.WriteLine("[" + this.GetType().Name + "]: Returning planes");
            return planes;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            // save settings
            Settings.Save(true);
        }

        // **************************** END OF INTERFACE *********************************

        private string TmpDirectory;
        private FlexJSONSettings Settings = new FlexJSONSettings(); 

        public FlexJSONPlugin()
        {
        }


        // *********************** Plane info converters **********************************

        #region PlaneInfoCheckers


        // checks for int and long values
        private bool IsInt(string s)
        {
            s = s.Trim();
            // int should contain only these chars
            string allowed = "-01234567890";
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (!allowed.Contains(s[i]))
                    return false;
            }
            long l;
            // try to convert to int
            if (!long.TryParse(s, out l))
                return false;
            // all checks paased
            return true;
        }

        private bool IsDouble(string s)
        {
            s = s.Trim();
            // double must contain at least a decimal separator
            if (!s.Contains(".") && !s.Contains(","))
                return false;
            // try to convert to double
            double d;
            // try to convert with regional settings
            if (!double.TryParse(s, out d))
            {
                // try invariant
                if (!double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                    return false;
            }
            // all checks passed
            return true;
        }

        private bool IsHex(string s)
        {
            bool b;
            s = s.Trim();
            if (s.Length != 6)
                return false;
            long l;
            if (!long.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out l))
                return false;
            // all checks passed
            return true;
        }

        private bool IsUTC(string s)
        {
            // get current time  in all three formats
            DateTime now = DateTime.UtcNow;
            int now_int = (Int32)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            long now_long = (long)now_int * 1000;

            s = s.Trim();
            // to short for a UTC value
            if (s.Length < 10)
                return false;
            if (!IsInt(s))
            {
                // maybe DateTime formatted
                if (!s.ToCharArray().Any(c => "-_:".Contains(c)))
                    return false;
            }
            // assuming int
            long l = 0;
            // assuming integer values
            if (s.Length < 13)
            {
                // assuming UNIX time in sec
                if (!long.TryParse(s, out l))
                    return false;
                if (Math.Abs(now_int - l) > 1000)
                    return false;
            }
            // assuming UNIX time in msec
            if (!long.TryParse(s, out l))
                return false;
            if (Math.Abs(now_long - l) > 1000000)
                return false;
            // all checks passed
            return true;
        }

        private bool IsLat(string s)
        {
            bool b;
            s = s.Trim();
            if (!IsDouble(s))
                return false;
            double d = To_Double(s);
            if (d < -90)
                return false;
            if (d > 90)
                return false;
            // all checks passed
            return true;
        }

        private bool IsLon(string s)
        {
            bool b;
            s = s.Trim();
            if (!IsDouble(s))
                return false;
            double d = To_Double(s);
            if (d < -180)
                return false;
            if (d > 180)
                return false;
            // all checks passed
            return true;
        }

        private bool IsAlt(string s)
        {
            bool b;
            s = s.Trim();
            if (!IsInt(s) && !IsDouble(s))
                return false;
            double d = To_Double(s);
            // don't accept negative values though they might occur
            if (d < 0)
                return false;
            if ((Settings.Units_Alt.ToLower() == "m ") && (d > 30000))
                return false;
            // assuming altitude in feet
            if (d > 100000)
                return false;
            // all checks passed
            return true;
        }

        private bool IsTrack(string s)
        {
            bool b;
            s = s.Trim();
            if (!IsInt(s) && !IsDouble(s))
                return false;
            double d = To_Double(s);
            if (d < 0)
                return false;
            if (d > 360)
                return false;
            // all checks passed
            return true;
        }

        private bool IsSpeed(string s)
        {
            bool b;
            s = s.Trim();
            if (!IsInt(s) && !IsDouble(s))
                return false;
            double d = To_Double(s);
            if (d < 0)
                return false;
            // don't accept higher values though they might occur
            if ((Settings.Units_Speed.ToLower() == "km/h ") && (d > 1000))
                return false;
            // assuming speed in kts
            if (d > 550)
                return false;
            // all checks passed
            return true;
        }

        private bool IsReg(string s)
        {
            bool b;
            s = s.Trim();
            // Registration should contain a "-" or start with "N"
            if (!s.Contains("-"))
            {
                if (!s.StartsWith("N"))
                    return false;
            }
            return true;
        }

        private bool IsType(string s)
        {
            bool b;
            s = s.Trim();
            if (s.Length != 4)
                return false;
            // type should contain at least on number
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsNumber(s[i]))
                    return true;
            }
            return false; ;
        }

        #endregion

        #region PlaneInfoConverters

        public int To_Int(string s)
        {
            int i;
            if (int.TryParse(s, out i))
                return i;
            return int.MinValue;
        }

        public long To_Long(string s)
        {
            long l;
            if (long.TryParse(s, out l))
                return l;
            return long.MinValue;
        }

        public double To_Double(string s)
        {
            double d;
            if (double.TryParse(s, out d))
                return d;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;
            return double.MinValue;
        }

        public DateTime To_UTC(string s)
        {
            if (String.IsNullOrEmpty(s))
                return DateTime.MinValue;
            // remove qoutes and spaces, if any
            s = s.Replace("\"", String.Empty).Trim();
            // UTC could be either:
            // a 32bit integer containing seconds since 1970-01-01  
            // a 64bit ticks containing ticks[ms] since 1970-01-01
            // a valid DateTime string, like "yyyy-MM-dd HH:mm:ss"
            // try to convert UNIX times first
            if ((s.Length >= 10) && IsInt(s))
            {
                try
                {
                    if (s.Length > 10)
                    {
                        // try to convert to milliseconds
                        // try to convert to seconds
                        long l = System.Convert.ToInt64(s);
                        DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        timestamp = timestamp.AddMilliseconds(l);
                        return timestamp;
                    }
                    else
                    {
                        // try to convert to seconds
                        long l = System.Convert.ToInt64(s);
                        DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        timestamp = timestamp.AddSeconds(l);
                        return timestamp;
                    }
                }
                catch (Exception ex)
                {
                    //                    Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
                }
            }
            // check for Standard DateTime notation
            try
            {
                // try to convert to UTC timestamp
                DateTime timestamp;
                if (DateTime.TryParse(s, out timestamp))
                    return timestamp;
            }
            catch (Exception ex)
            {
                //               Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return DateTime.MinValue;
        }

        public string To_Hex(string s)
        {
            if (String.IsNullOrEmpty(s))
                return null;
            // remove qoutes and spaces, if any
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            // check if Hex
            if (!IsHex(s))
                return null;
            try
            {
                // try to convert to Hex value
                long hex = System.Convert.ToInt64(s, 16);
                // check boundaries
                if ((hex < 0) || (hex > 16777215))
                    return null;
                return s;
            }
            catch (Exception ex)
            {
                //                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return null;
        }

        public double To_Lat(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length < 3)
                return double.MinValue;
            // double Lon must contain a decimal separator
            if (s.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) < 0)
                return double.MinValue;
            try
            {
                // try to convert to double
                double d = System.Convert.ToDouble(s);
                // check bounds
                if ((d >= -90.0) && (d <= 90.0))
                    return d;
            }
            catch (Exception ex)
            {
                //                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return double.MinValue;
        }


        public double To_Lon(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            if (s.Length < 3)
                return double.MinValue;
            // double Lon must contain a decimal separator
            if (s.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) < 0)
                return double.MinValue;
            try
            {
                // try to convert to double
                double d = System.Convert.ToDouble(s);
                // check bounds
                if ((d >= -180.0) && (d <= 180.0))
                    return d;
            }
            catch (Exception ex)
            {
                //                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return double.MinValue;
        }


        public int To_Alt(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            try
            {
                // try to convert to integer
                long alt = System.Convert.ToInt64(s);
                // convert m to ft
                if (Settings.Units_Alt.ToLower() == "m")
                    alt = (int)((double)alt * 3.28084);
                // check bounds
                if ((alt < 0) || (alt > 100000))
                    return int.MinValue;
                return (int)alt;
            }
            catch (Exception ex)
            {
                //               Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public int To_Track(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            try
            {
                // try to convert to integer
                long track = System.Convert.ToInt64(s);
                // check bounds
                if ((track < 0) || (track > 360))
                    return int.MinValue;
                return (int)track;
            }
            catch (Exception ex)
            {
                //                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public int To_Speed(string s)
        {
            s = s.Replace("\"", String.Empty).Trim();
            try
            {
                // try to convert to integer
                long speed = System.Convert.ToInt64(s);
                // convert km/h to kts
                if (Settings.Units_Speed.ToLower() == "km/h")
                    return (int)((double)speed / 1.852);
                if ((speed < 0) || (speed > 550))
                    return int.MinValue;
                return (int)speed;
            }
            catch (Exception ex)
            {
                //                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message + ": " + s);
            }
            return int.MinValue;
        }

        public string To_Call(string s)
        {
            if (String.IsNullOrEmpty(s))
                return null;
            // remove qoutes and spaces, if any
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            return s;
        }

        public string To_Type(string s)
        {
            if (String.IsNullOrEmpty(s))
                return null;
            // remove qoutes and spaces, if any
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            return s;
        }

        public string To_Reg(string s)
        {
            if (String.IsNullOrEmpty(s))
                return null;
            // remove qoutes and spaces, if any
            s = s.Replace("\"", String.Empty).ToUpper().Trim();
            return s;
        }

        #endregion

        #region IndexFinders

        private void FindHexIndex()
        {
            if (PlaneList.Count <= 0)
                return;
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsHexColumns.Length; i++)
            {
                if (!DetectedColumns[i])
                {
                    if (IsHexColumns[i] > maxcount)
                    {
                        maxcount = IsHexColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Hex = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindUTCIndex()
        {
            if (PlaneList.Count <= 0)
                return;
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsUTCColumns.Length; i++)
            {
                if (!DetectedColumns[i])
                {
                    if (IsUTCColumns[i] > maxcount)
                    {
                        maxcount = IsUTCColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_UTC = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindLatIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsLatColumns.Length; i++)
            {
                if (!DetectedColumns[i])
                {
                    if (IsLatColumns[i] > maxcount)
                    {
                        maxcount = IsLatColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Lat = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindLonIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsLonColumns.Length; i++)
            {
                if (!DetectedColumns[i])
                {
                    if (IsLonColumns[i] > maxcount)
                    {
                        maxcount = IsLonColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Lon = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindTrackIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsTrackColumns.Length; i++)
            {
                if (!DetectedColumns[i] && (MaxColumns[i] <= 360))
                {
                    if (IsTrackColumns[i] > maxcount)
                    {
                        maxcount = IsTrackColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Track = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindSpeedIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsSpeedColumns.Length; i++)
            {
                if (!DetectedColumns[i] && (MaxColumns[i] > 360))
                {
                    if (IsSpeedColumns[i] > maxcount)
                    {
                        maxcount = IsSpeedColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Speed = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindAltIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsAltColumns.Length; i++)
            {
                if (!DetectedColumns[i] && (MeanColumns[i] > 5000))
                {
                    if (IsAltColumns[i] > maxcount)
                    {
                        maxcount = IsAltColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Alt = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindTypeIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsTypeColumns.Length; i++)
            {
                if (!DetectedColumns[i])
                {
                    if (IsTypeColumns[i] > maxcount)
                    {
                        maxcount = IsTypeColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Type = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        private void FindRegIndex()
        {
            int maxindex = -1;
            int maxcount = 0;
            for (int i = 0; i < IsRegColumns.Length; i++)
            {
                if (!DetectedColumns[i])
                {
                    if (IsRegColumns[i] > maxcount)
                    {
                        maxcount = IsRegColumns[i];
                        maxindex = i;
                    }
                }
            }
            if (maxcount > (int)(0.9 * (double)PlaneList.Count))
            {
                Settings.Index_Reg = maxindex;
                DetectedColumns[maxindex] = true;
            }
        }

        #endregion

        // *********************** Parsing elements ************************************

        private class ElementInfo
        {
            public string Key;
            public string TypeName;
            public int Length;
            public dynamic Element;

            public ElementInfo(string key, string typename, int length, dynamic element)
            {
                Key = key;
                TypeName = typename;
                Length = length;
                Element = element;
            }
        }

        private List<ElementInfo> Elements = new List<ElementInfo>();
        private Dictionary<int, int> ElementLengths = new Dictionary<int, int>();
        private List<string[]> PlaneList = new List<string[]>();
        private double[] MaxColumns;
        private double[] MinColumns;
        private double[] MeanColumns;
        private int[] IsDoubleColumns;
        private int[] IsIntColumns;
        private int[] IsHexColumns;
        private int[] IsUTCColumns;
        private int[] IsLatColumns;
        private int[] IsLonColumns;
        private int[] IsAltColumns;
        private int[] IsTrackColumns;
        private int[] IsSpeedColumns;
        private int[] IsTypeColumns;
        private int[] IsRegColumns;
        private bool[] DetectedColumns;

        private void AddElement(string key, dynamic el)
        {
            string typename = el.GetType().Name;
            if (typename.ToLower().Contains("dictionary"))
            {
                // dictionary found
                // count the length and maintain element lengths
                int i = 0;
                int c = el.Values.Count;
                if (ElementLengths.TryGetValue(c, out i))
                {
                    ElementLengths[c]++;
                }
                else
                {
                    ElementLengths.Add(c, 1);
                }
                // add element to elements list
                ElementInfo info = new ElementInfo(key, typename, c, el);
                Elements.Add(info);
                // recurse element, key ist dictionary key
                foreach (dynamic e in el.Keys)
                {
                    AddElement(e, el[e]);
                }
            }
            else if (typename.ToLower().Contains("[]"))
            {
                // array found
                // add element to elements list
                // count the length and maintain element lengths
                int i = 0;
                int c = el.Length;
                if (ElementLengths.TryGetValue(c, out i))
                {
                    ElementLengths[c]++;
                }
                else
                {
                    ElementLengths.Add(c, 1);
                }
                ElementInfo info = new ElementInfo(key, typename, c, el);
                Elements.Add(info);
                // recurse elements, key is a unique ID 
                foreach (dynamic e in el)
                {
                    AddElement(Guid.NewGuid().ToString(), e);
                }
            }
            else if (typename.ToLower().Contains("arraylist"))
            {
                // ArrayList found
                // add element to elements list
                // count the length and maintain element lengths
                int i = 0;
                int c = el.Count;
                if (ElementLengths.TryGetValue(c, out i))
                {
                    ElementLengths[c]++;
                }
                else
                {
                    ElementLengths.Add(c, 1);
                }
                ElementInfo info = new ElementInfo(key, typename, c, el);
                Elements.Add(info);
                Console.WriteLine("Element found: " + info.Key + "; " + info.TypeName + "; " + c.ToString() + "; ", el.ToString());
                // recurse elements, key is a unique ID 
                foreach (dynamic e in el)
                {
                    AddElement(Guid.NewGuid().ToString(), e);
                }
            }
        }

        private string[] GetPlaneInfo(ElementInfo el)
        {
            string[] info = new string[el.Length + 1];
            // set key
            info[0] = el.Key;
            // grab info
            if (el.TypeName.ToLower().Contains("dictionary"))
            {
                // dictionary found
                int count = 1;
                foreach (dynamic e in el.Element.Values)
                {
                    if (e.GetType().IsPrimitive || (e.GetType().Name.ToLower() == "string"))
                    {
                        info[count] = e.ToString();
                    }
                    else
                    {
                        info[count] = "";
                    }
                    count++;
                }
            }
            else if (el.TypeName.ToLower().Contains("[]"))
            {
                // array found
                int count = 1;
                foreach (dynamic e in el.Element)
                {
                    string typename = e.GetType().Name;
                    if (e.GetType().IsPrimitive || (typename.ToLower() == "string") || (typename.ToLower() == "decimal"))
                    {
                        info[count] = e.ToString();
                    }
                    else
                    {
                        info[count] = "";
                    }
                    count++;
                }
            }
            else if (el.TypeName.ToLower().Contains("arraylist"))
            {
                // ArrayList found
                int count = 1;
                foreach (dynamic e in el.Element)
                {
                    string typename = e.GetType().Name;
                    if (e.GetType().IsPrimitive || (typename.ToLower() == "string") || (typename.ToLower() == "decimal"))
                    {
                        info[count] = e.ToString();
                    }
                    else
                    {
                        info[count] = "";
                    }
                    count++;
                }
            }
            return info;
        }

        private void GetColumnInfo()
        {
            double[] sum = new double[MinColumns.Length];
            for (int i = 0; i < sum.Length; i++)
                sum[i] = 0;
            foreach (string[] info in PlaneList)
            {
                if (info == null)
                    continue;
                for (int i = 0; i < info.Length; i++)
                {
                    if (IsDouble(info[i]))
                    {
                        IsDoubleColumns[i]++;
                        double d = To_Double(info[i]);
                        if (d != double.MinValue)
                        {
                            if (d < MinColumns[i])
                                MinColumns[i] = d;
                            else if (d > MaxColumns[i])
                                MaxColumns[i] = d;
                        }
                        sum[i] = sum[i] + d;
                    }
                    if (IsInt(info[i]))
                    {
                        IsIntColumns[i]++;
                        long l = To_Long(info[i]);
                        if (l != long.MinValue)
                        {
                            if (l < MinColumns[i])
                                MinColumns[i] = l;
                            else if (l > MaxColumns[i])
                                MaxColumns[i] = l;
                        }
                        sum[i] = sum[i] + l;
                    }
                    if (IsHex(info[i]))
                        IsHexColumns[i]++;
                    if (IsUTC(info[i]))
                        IsUTCColumns[i]++;
                    if (IsLat(info[i]))
                        IsLatColumns[i]++;
                    if (IsLon(info[i]))
                        IsLonColumns[i]++;
                    if (IsAlt(info[i]))
                        IsAltColumns[i]++;
                    if (IsTrack(info[i]))
                        IsTrackColumns[i]++;
                    if (IsSpeed(info[i]))
                        IsSpeedColumns[i]++;
                    if (IsType(info[i]))
                        IsTypeColumns[i]++;
                    if (IsReg(info[i]))
                        IsRegColumns[i]++;
                }
            }
            for (int i = 0; i < sum.Length; i++)
                MeanColumns[i] = sum[i] / PlaneList.Count();
        }

        private void GetAllPlaneInfos(dynamic obj)
        {
            // get all elements in a linear list and count the lengths
            Elements.Clear();
            ElementLengths.Clear();
            // start at the root element
            AddElement("", obj);
            // find the majority of element lengths --> assuming that these are the plane elements
            int maxcount = 0;
            int maxkey = 0;
            foreach (int key in ElementLengths.Keys)
            {
                if (ElementLengths[key] > maxcount)
                {
                    maxcount = ElementLengths[key];
                    maxkey = key;
                }
            }
            // get all plane infos in a clean list
            PlaneList.Clear();
            foreach (ElementInfo el in Elements)
            {
                // add matching elements to planes list
                if (el.Length == maxkey)
                {
                    string[] info = GetPlaneInfo(el);
                    PlaneList.Add(info);
                }
            }
            // store the plane i nfo table as CSV, so that a user can determine which columns at which indices are needed
            // catch the exception here to continue grabbing planes if user left thge file open :-)
            string csvfilename = Path.Combine(TmpDirectory, "FlexJSON.csv");
            try
            {
                using (StreamWriter sw = new StreamWriter(csvfilename))
                {
                    foreach (string[] infos in PlaneList)
                    {
                        foreach (string info in infos)
                            sw.Write("\"" + info + "\"" + ";");
                        sw.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing
                Console.WriteLine("[" + this.GetType().Name + "]: Cannot write CSV-table " + csvfilename + ", " + ex.ToString());
            }
            // clear all info arrays
            MaxColumns = new double[maxkey + 1];
            MinColumns = new double[maxkey + 1];
            MeanColumns = new double[maxkey + 1];
            IsIntColumns = new int[maxkey + 1];
            IsDoubleColumns = new int[maxkey + 1];
            IsHexColumns = new int[maxkey + 1];
            IsUTCColumns = new int[maxkey + 1];
            IsLatColumns = new int[maxkey + 1];
            IsLonColumns = new int[maxkey + 1];
            IsAltColumns = new int[maxkey + 1];
            IsTrackColumns = new int[maxkey + 1];
            IsSpeedColumns = new int[maxkey + 1];
            IsTypeColumns = new int[maxkey + 1];
            IsRegColumns = new int[maxkey + 1];
            DetectedColumns = new bool[maxkey + 1];


            for (int i = 0; i < maxkey + 1; i++)
            {
                MaxColumns[i] = 0;
                MinColumns[i] = 0;
                MeanColumns[i] = 0;
                IsIntColumns[i] = 0;
                IsDoubleColumns[i] = 0;
                IsHexColumns[i] = 0;
                IsUTCColumns[i] = 0;
                IsLatColumns[i] = 0;
                IsAltColumns[i] = 0;
                IsTrackColumns[i] = 0;
                IsSpeedColumns[i] = 0;
                IsTypeColumns[i] = 0;
                IsRegColumns[i] = 0;
                DetectedColumns[i] = false;
            }
            // initially set all indices already found
            PropertyInfo[] properties = typeof(PlaneFeedPluginSettings).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                // find all index properties
                if (p.Name.StartsWith("Index_") && (p.PropertyType == typeof(int)))
                {
                    int index = (int)p.GetValue(Settings, null);
                    // mark detected column as true
                    if ((index >= 0) && (index < DetectedColumns.Length))
                        DetectedColumns[index] = true;
                }
            }
            // get min/max info per column
            GetColumnInfo();
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private string ReadPropertyString(dynamic o, string propertyname)
        {
            string s = null;
            try
            {
                s = o[propertyname];
            }
            catch
            {

            }
            return s;
        }



        [System.Diagnostics.DebuggerNonUserCode]
        private int ReadPropertyInt(dynamic o, string propertyname)
        {
            int i = int.MinValue;
            double d = ReadPropertyDouble(o, propertyname);
            if ((d != double.MinValue) && (d >= int.MinValue) && (d <= int.MaxValue))
                i = (int)d;
            return i;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private double ReadPropertyDouble(dynamic o, string propertyname)
        {
            double d = double.MinValue;
            try
            {
                string s = o[propertyname].ToString(CultureInfo.InvariantCulture);
                d = double.Parse(s, CultureInfo.InvariantCulture);
            }
            catch
            {
                // do nothing if something went wrong
            }
            return d;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private long ReadPropertyLong(dynamic o, string propertyname)
        {
            long l = long.MinValue;
            try
            {
                l = o[propertyname];
            }
            catch
            {
                // do nothing if something went wrong
            }
            return l;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private bool ReadPropertyBool(dynamic o, string propertyname)
        {
            bool b = false;
            try
            {
                string s = o[propertyname].ToString();
                b = s.ToLower() == "true";
            }
            catch
            {
                // do nothing if something went wrong
            }
            return b;
        }

    }


    /// <summary>
    /// //////////////////////////////////////////// Helpers ////////////////////////////////////////////
    /// </summary>

    public static class UnitConverter
    {
        public static double ft_m(double feet)
        {
            return feet / 3.28084;
        }

        public static double m_ft(double m)
        {
            return m * 3.28084;
        }

        public static double kts_kmh(double kts)
        {
            return kts * 1.852;
        }

        public static double kmh_kts(double kmh)
        {
            return kmh / 1.852;
        }

        public static double km_mi(double km)
        {
            return km * 1.609;
        }

        public static double mi_km(double mi)
        {
            return mi / 1.609;
        }
    }

    public class VarConverter : Dictionary<string, object>
    {
        public readonly char VarSeparator = '%';

        public void AddVar(string var, object value)
        {
            // adds a new var<>value pair to dictionary
            object o;
            if (this.TryGetValue(var, out o))
            {
                // item found --> update value
                o = value;
            }
            else
            {
                // item not found --> add new
                this.Add(var, value);
            }
        }

        public object GetValue(string var)
        {
            // finds a var in dictionary and returns its value
            object o;
            if (this.TryGetValue(var, out o))
            {
                // item found --> return value
                return o;
            }
            // item not found --> return null
            return null;
        }

        public string ReplaceAllVars(string s)
        {
            // check for var separotors first
            if (s.Contains(VarSeparator))
            {
                // OK, string is containing vars --> crack the string first and replace vars
                try
                {
                    string[] a = s.Split(VarSeparator);
                    // as we are always using a pair of separators the length of a[] must be odd
                    if (a.Length % 2 == 0)
                        throw new ArgumentException("Number of separators is not an even number.");
                    // create new string and replace all vars (on odd indices)
                    s = "";
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // cannot be not a var on that position
                            s = s + a[i];
                        }
                        else
                        {
                            // var identifier: upper the string and try to convert
                            a[i] = a[i].ToUpper();
                            object o;
                            if (this.TryGetValue(a[i], out o))
                            {
                                // convert floating points with invariant culture info
                                if (o.GetType() == typeof(double))
                                    s = s + ((double)o).ToString(CultureInfo.InvariantCulture);
                                else if (o.GetType() == typeof(float))
                                    s = s + ((float)o).ToString(CultureInfo.InvariantCulture);
                                else
                                    s = s + o.ToString();
                            }
                            else
                            {
                                throw new ArgumentException("Var identifier not found: " + a[i]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // throw an excecption 
                    throw new ArgumentException("Error while parsing string for variables [" + ex.Message + "]: " + s);
                }
            }
            return s;
        }

    }

}

