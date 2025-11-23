using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using AirScout.PlaneFeeds.Plugin.MEFContract;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.ComponentModel.Composition;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Remoting.Contexts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace AirScout.PlaneFeeds.Plugin.BinCraft
{

    public class BinCraftServerSettings
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
        [DefaultValue("http://44.149.131.185/tar1090/data/aircraft.binCraft.zst")]
        public string URL { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Compressed")]
        [DefaultValue(true)]
        [XmlIgnore]
        public bool Compressed { get; set; }

        public BinCraftServerSettings()
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

            // create logfilename 
            string logfile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL", ".CFG")).LocalPath.ToLower().Replace(".cfg", ".log");

            // NASTY!!! delete logfile if it is getting too big
            if (!File.Exists(logfile))
            {
                try
                {
                    if (new System.IO.FileInfo(logfile).Length > 1024 * 1024)
                    {
                        File.Delete(logfile);
                    }
                }
                catch
                {

                }
            }

            // fix issues with loading of configuration
            bool loaded = false;
            int retries = 0;

            while (!loaded && (retries < 10))
            {
                // do nothing if file not exists
                if (File.Exists(filename))
                {
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
                                    {
                                        // fix issues with URL in V1.4.0.0 --> do not load URL from file if not containing filters
                                        if ((p.Name != "URL") || (element.Value.ToString().Contains("?")))
                                        {
                                            p.SetValue(this, Convert.ChangeType(element.Value, p.PropertyType), null);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ignoring property URL do to version conflict: " + element.Value);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("[" + this.GetType().Name + "]: Error while loading property[" + p.Name + " from " + filename + ", " + ex.Message);
                                try
                                {
                                    File.AppendAllText(logfile, "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "] Error while loading property " + p.Name + ": " + ex.ToString() + "\n");
                                }
                                catch
                                {

                                }
                            }
                        }

                        loaded = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[" + this.GetType().Name + "]: Cannot load settings from " + filename + ", " + ex.Message);
                        try
                        {
                            File.AppendAllText(logfile, "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "] Error while loading settings from " + filename + ": " + ex.ToString() + "\n");
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    try
                    {
                        File.AppendAllText(logfile, "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "] The settings file " + filename + " was not found. Using default values.\n");
                    }
                    catch
                    {

                    }
                }

                Thread.Sleep(100);

                retries++;
            }

            if (!loaded)
            {
                string content = "<file not found>";
                if (File.Exists(filename))
                    content = File.ReadAllText(filename);
                string message = "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "] There was a problem loading the VRS Webserver settings from " +
                    filename +
                    "\nFile content is: \n\n "
                    + content +
                    "\n\nPlease report it to DL2ALF\n";
                try
                {
                    File.AppendAllText(message, logfile);
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    string settings = File.ReadAllText(filename);
                    File.AppendAllText(logfile, "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") +
                        "] Settings loaded from " + filename + ": \n\n" +
                         settings +
                        "\n\n");
                }
                catch
                {

                }


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

            // create logfilename 
            string logfile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(".dll", ".cfg").Replace(".DLL", ".CFG")).LocalPath.ToLower().Replace(".cfg", ".log");

            // NASTY!!! delete logfile if it is getting too big
            if (!File.Exists(logfile))
            {
                try
                {
                    if (new System.IO.FileInfo(logfile).Length > 1024 * 1024)
                    {
                        File.Delete(logfile);
                    }
                }
                catch
                {

                }
            }

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

                try
                {
                    string settings = File.ReadAllText(filename);
                    File.AppendAllText(logfile, "[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") +
                        "] Saving settings to " + filename + ": \n\n" +
                       settings +
                        "\n\n");
                }
                catch
                {

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
    public class BinCraftPlugin : IPlaneFeedPlugin
    {
        private BinCraftServerSettings Settings = new BinCraftServerSettings();

        public string Name
        {
            get
            {
                return "[WebFeed]           BinCraft Binary";
            }
        }

        public string Info
        {
            get
            {
                return "Web feed from BinCraft\n\n" +
                       "Use this feed to load a BinCraft binary file from\n" +
                       "the in-build webserver of Readsb.exe.\n" +
                       "It can also be used for other web feeds using this format.";
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
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.FileName = "*.feed";
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds (*.feed)|*.feed";
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
            Dlg.Filter = "Plane Feeds (*.feed)|*.feed";
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
            VC.AddVar("UNIXTIME", SupportFunctions.DateTimeToUNIXTime(DateTime.UtcNow));

            // calculate Max/Min distance for filter
            VC.AddVar("MYLAT", args.MyLat);
            VC.AddVar("MYLON", args.MyLon);

            double mindist = 0;
            double maxdist = 0;

            // check the distance between MyLocation and edges of rect and take the maximum
            maxdist = Math.Max(maxdist, LatLon.Distance(args.MyLat, args.MyLon, args.MinLat, args.MinLon));
            maxdist = Math.Max(maxdist, LatLon.Distance(args.MyLat, args.MyLon, args.MaxLat, args.MinLon));
            maxdist = Math.Max(maxdist, LatLon.Distance(args.MyLat, args.MyLon, args.MinLat, args.MaxLon));
            maxdist = Math.Max(maxdist, LatLon.Distance(args.MyLat, args.MyLon, args.MaxLat, args.MaxLon));

            VC.AddVar("MINDISTKM", mindist);
            VC.AddVar("MAXDISTKM", maxdist);

            // initialize plane info list
            PlaneFeedPluginPlaneInfoList planes = new PlaneFeedPluginPlaneInfoList();

            string url = "";
            byte[] data = new byte[0];

            try
            {
                DateTime start1 = DateTime.UtcNow;

                url = VC.ReplaceAllVars(Settings.URL);

                // get data
                Console.WriteLine("Getting data file from : " + url);
                using (var wc = new System.Net.WebClient())
                    data = wc.DownloadData(url);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting JSON file: " + ex.Message);
            }

            Console.WriteLine("[" + this.GetType().Name + "]: Analyzing data");
            try
            {
                // convert binary data
                Dictionary<string, object> AircraftList = BinCraftParser.Parse(data, Settings.Compressed);

                int count = 0;

                // get DateTime of file generation
                DateTime filetime = DateTime.MinValue;
                if (AircraftList["now"] != null)
                {
                    filetime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    filetime = filetime.AddMilliseconds((long)((double)AircraftList["now"] * 1000));
                }

                Console.WriteLine("Current filetime: " + filetime.ToString());

                // get planes
                System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>> acs = (System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>)AircraftList["aircraft"];
                foreach (System.Collections.Generic.Dictionary<string, object> ac in acs )
                {
                    try
                    {
                        PlaneFeedPluginPlaneInfo plane = new PlaneFeedPluginPlaneInfo();
                        // get hex first
                        plane.Hex = ((string)ac["hex"]).Trim().Replace("\"", "").ToUpper();
                        // get position
                        plane.Lat = (double)ac["lat"];
                        plane.Lon = (double)ac["lon"];
                        try
                        {
                            plane.Alt = System.Convert.ToInt32(ac["alt_baro"]);
                        }
                        catch (Exception ex)
                        {
                            plane.Alt = 0;
                        }
                        // get track
                        plane.Track = (double)ac["track"];

                        // get speed
                        plane.Speed = (double)ac["gs"];

                        // get position timestamp
                        plane.Time = filetime.AddSeconds(-(double)ac["seen_pos"]);

                        // add plane to list
                        planes.Add(plane);

                        count++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing if something else goes wrong
                Console.WriteLine("Error while deserializing data file: " + ex.ToString());

            }
            Console.WriteLine("[" + this.GetType().Name + "]: Returning " + planes.Count + " planes");

            return planes;

        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            Settings.Save(true);
        }


        // end of Interface


        /// <summary>
        /// Decodes an OpenSSL-encrypted (AES256-CBC) string<br></br><br></br>
        /// The equivalent encoding in PHP is like:<br></br><br></br>
        /// $encrypt_method = "AES-256-CBC";<br></br>
        /// $secret_key = hash('md5',$key);<br></br>
        /// $encoded = openssl_encrypt($data, $encrypt_method, $secret_key);
        /// </summary>
        /// <param name="encrypteddata">The encrypted string (Base64 encoded).</param>
        /// <param name="pwd">The password as a string.</param>
        /// <returns></returns>
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

    public static class SupportFunctions
    {
        public static int DateTimeToUNIXTime(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return int.MinValue;
            else if (dt == DateTime.MaxValue)
                return int.MaxValue;
            return (Int32)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

    }

    public static class LatLon
    {
        public class Earth
        {
            public static double Radius = 6371;
        }

        public static double Distance(double mylat, double mylon, double lat, double lon)
        {
            double R = Earth.Radius;
            double dLat = (mylat - lat);
            double dLon = (mylon - lon);
            double a = Math.Sin(dLat / 180 * Math.PI / 2) * Math.Sin(dLat / 180 * Math.PI / 2) +
                    Math.Sin(dLon / 180 * Math.PI / 2) * Math.Sin(dLon / 180 * Math.PI / 2) * Math.Cos(mylat / 180 * Math.PI) * Math.Cos(lat / 180 * Math.PI);
            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
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
