﻿using System;
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
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections;
using Newtonsoft.Json;

//TODO: Rename namespace to a name of your choice
namespace AirScout.PlaneFeeds.Plugin.OpenSky
{

    //TODO: Rename settings class to a name of your choice
    //      Add any persistant setting here
    //      Use [Browsable(true/false)] to present the setting to the user and allow changes or not
    //      Use [CategoryAttribute("<description")] to group settings to categories
    //      Use [DescriptionAttribute("<description")] to add a description to this setting
    //      Use [DefaultValue(<value>)] to set a default value for this setting
    //      Use [XmlIgnore] if you don't want this setting to be ex-/imported, but stiil stored in settings file
    //      Example:
    //              [CategoryAttribute("Web Feed")]
    //              [DescriptionAttribute("Timeout for loading the site.")]
    //              [DefaultValue(30)]
    //              [XmlIgnore]
    //              public int Timeout { get; set; }


    /// <summary>
    /// Keeps all persistant settings of plugin
    /// </summary>
    public class OpenSkySettings
    {
        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Save downloaded JSON to file")]
        [DefaultValue(false)]
        [XmlIgnore]
        public bool SaveToFile { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Base URL for website.")]
        [DefaultValue("https://opensky-network.org/api/states/all?lamin=%MINLAT%&lomin=%MINLON%&lamax=%MAXLAT%&lomax=%MAXLON%")]
        public string URL { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Username for Authentication (blank for anonymous)")]
        [DefaultValue("")]
        public string Username { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Password for Authentication (blank for anonymous)")]
        [DefaultValue("")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Timeout for loading the site.")]
        [DefaultValue(90)]
        [XmlIgnore]
        public int Timeout { get; set; }


        public OpenSkySettings()
        {
            Default();
            Load(true);
            // TODO:
        }

        // Methods für Load/Dave/Default, don't change!

        #region Load/Save/Default

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
                filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL", ".CFG")).LocalPath;
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

    #endregion


    //TODO: Rename plugin class to a name of yout choice

    /// <summary>
    /// Holds the plane feed plugin class
    /// </summary>
    [Export(typeof(IPlaneFeedPlugin))]
    [ExportMetadata("Name", "PlaneFeedPlugin")]
    public class OpenSkyPlugin : IPlaneFeedPlugin
    {
        private OpenSkySettings Settings = new OpenSkySettings();

        // start of interface

        //TODO: Change return values so that they represent plugin's functionality
        #region Interface

        public string Name
        {
            get
            {
                return "[WebFeed]           OpenSky";
            }
        }
        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string Info
        {
            get
            {
                return "Web feed from the OpenSky Network.\n" +
                    "For details see https://opensky-network.org.\n\n" +
                    "As this is a community network, please consider to run a personal ADSB-receiver and to contribute your data to this network.\n\n" +
                    "The daily limit for anonymous requests to OpenSky Network is 100. You can extend the limit to 1000 if you are an registerd user.\n" +
                    "For this, please enter your username & password here.";
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
                return false;
            }
        }

        public bool CanExport
        {
            get
            {
                return false;
            }
        }

        public string Disclaimer
        {
            get
            {
                return "";
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
            // nothing to do
        }

        public void ExportSettings()
        {
            // nothing to do
        }

        public void Start(PlaneFeedPluginArgs args)
        {
            // add code for startup here
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
            Console.WriteLine("[" + this.GetType().Name + "]: Creating web request: " + url);
            // this will only run on .NET 4.0 if you have installed .NET 4.5 or later frameworks on your machine!
            //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            //            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            //            webrequest.Timeout = Settings.Timeout * 1000;
            //            webrequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0";
            //            webrequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //            Console.WriteLine("[" + this.GetType().Name + "]: Getting web response");
            //            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            //            Console.WriteLine("[" + this.GetType().Name + "]: Reading stream");
            //            using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
            //           {
            //                json = sr.ReadToEnd();
            //            }
            DateTime downloadstart = DateTime.UtcNow;
            json = OSNTlsClient.DownloadFile(url, Settings.Timeout * 1000, Settings.Username, Settings.Password);
            Console.WriteLine("[" + this.GetType().Name + "]: Content downloaded, " + (DateTime.UtcNow - downloadstart).TotalMilliseconds.ToString("F0") + "ms.");

            // check for errors
            if (!json.StartsWith("{"))
            {
                // report the first characters of response
                throw new Exception("Response: " + json.Substring(0, 80));
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
            //            JavaScriptSerializer js = new JavaScriptSerializer();
            //            dynamic root = js.Deserialize<dynamic>(json);
            dynamic root = JsonConvert.DeserializeObject(json);
            try
            {
                // analyze json string for planes data
                // get the planes position list
                var aclist = root["states"];
                foreach (var ac in aclist)
                {
                    try
                    {
                        // different handling of reading JSON between Windows (Array) & Linux (ArrayList)
                        // access to data values itself is the same
                        int len = 0;
                        len = ac.Count;
                        // skip if too few fields in record
                        if (len < 17)
                            continue;
                        PlaneFeedPluginPlaneInfo plane = new PlaneFeedPluginPlaneInfo();
                        // get hex first
                        plane.Hex = ReadPropertyString(ac, 0).ToUpper().Trim();
                        // get callsign
                        plane.Call = ReadPropertyString(ac, 1).ToUpperInvariant().Trim();
                        // get position
                        plane.Lon = ReadPropertyDouble(ac, 5);
                        plane.Lat = ReadPropertyDouble(ac, 6);
                        // get altitude (provided in m --> convert to ft)
                        plane.Alt = UnitConverter.m_ft(ReadPropertyDouble(ac, 13));
                        // get track
                        plane.Track = ReadPropertyDouble(ac, 10);
                        // get speed (provided in m/s --> convert to kts)
                        plane.Speed = UnitConverter.ms_kts(ReadPropertyDouble(ac, 9));
                        // registration is not provided
                        plane.Reg = "";
                        // get position timestamp in sec
                        int l = ReadPropertyInt(ac, 3);
                        if (l != int.MinValue)
                        {
                            DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                            timestamp = timestamp.AddSeconds(l);
                            plane.Time = timestamp;
                        }
                        else
                        {
                            // skip plane if no valid timestamp found
                            continue;
                        }
                        // get type info
                        plane.Type = ReadPropertyString(ac, 5);

                        // discard planes on ground
                        bool onground = ReadPropertyBool(ac, 8);
                        if (onground)
                            continue;

                        // discard planes stopped
                        if (plane.Speed <= 0)
                            continue;

                        planes.Add(plane);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message);
            }
            Console.WriteLine("[" + this.GetType().Name + "]: Returning " + planes.Count + " planes");
            return planes;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            // add code for stopping here
        }

        #endregion

        // End of interface


        // ************************************* Helpers ****************************************************

        private string ReadPropertyString(dynamic o, int propertyindex)
        {
            string s = null;
            try
            {
                s = o[propertyindex];
            }
            catch (Exception ex)
            {
                // do nothing if something went wrong
                Console.WriteLine("[ReadPropertyString]: " + ex.ToString());
            }
            return s;
        }

        private int ReadPropertyInt(dynamic o, int propertyindex)
        {
            int i = int.MinValue;
            try
            {
                double d = ReadPropertyDouble(o, propertyindex);
                if ((d != double.MinValue) && (d >= int.MinValue) && (d <= int.MaxValue))
                    i = (int)d;
            }
            catch
            {
                // do nothing if something went wrong
            }
            return i;
        }

        private double ReadPropertyDouble(dynamic o, int propertyindex)
        {
            double d = double.MinValue;
            try
            {
                string s = o[propertyindex].ToString(CultureInfo.InvariantCulture).Trim();
                // check for special values
                if (String.IsNullOrEmpty(s))
                    d = double.MinValue;
                else if (s.ToUpperInvariant().StartsWith("-INF"))
                    d = double.MinValue;
                else if (s.ToUpperInvariant().StartsWith("+INF"))
                    d = double.MaxValue;
                else if (s.ToUpperInvariant().StartsWith("NAN"))
                    d = double.NaN;
                else if (s.ToUpperInvariant().StartsWith("NULL"))
                    d = double.NaN;
                else
                    d = double.Parse(s, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                // do nothing if something went wrong
                Console.WriteLine("[ReadPropertyDouble] got invalid value = \"" + o[propertyindex].ToString() + "\": " + ex.ToString());
            }
            return d;
        }

        private bool ReadPropertyBool(dynamic o, int propertyindex)
        {
            bool b = false;
            try
            {
                string s = o[propertyindex].ToString();
                b = s.ToLower() == "true";
            }
            catch (Exception ex)
            {
                // do nothing if something went wrong
                Console.WriteLine("[ReadPropertyBool]: " + ex.ToString());
            }
            return b;
        }

    }

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

        public static double ms_kts(double ms)
        {
            return ms * 3.6 / 1.852;
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
