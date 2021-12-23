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
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace AirScout.PlaneFeeds.Plugin.VirtualRadarServer
{

    public class VirtualRadarServerSettings
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
        [DefaultValue("https://public-api.adsbexchange.com/VirtualRadar/AircraftList.json?fNBnd=%MAXLAT%&fSBnd=%MINLAT%&fWBnd=%MINLON%&fEBnd=%MAXLON%&fAltL=%MINALTFT%&fAltU=%MAXALTFT%")]
        public string URL { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Timeout for loading the site.")]
        [DefaultValue(30)]
        [XmlIgnore]
        public int Timeout { get; set; }

        [Browsable(true)]
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Personal API key for this feed. Leave it empty to use AirScout internal.")]
        [DefaultValue("")]
        [XmlIgnore]
        public string APIKey { get; set; }

        public VirtualRadarServerSettings()
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


[Export(typeof(IPlaneFeedPlugin))]
    [ExportMetadata("Name", "PlaneFeedPlugin")]
    public class VirtualRadarServerPlugin : IPlaneFeedPlugin
    {
        private VirtualRadarServerSettings Settings = new VirtualRadarServerSettings();

        private string APIKey = "";

        public string Name
        {
            get
            {
                return "[WebFeed]           Virtual Radar Server";
            }
        }

        public string Info
        {
            get
            {
                return "Web feed from Virtual Radar Server\n\n" +
                       "(c) AirScout(www.airscout.eu)\n\n" +
                       "This feed requires an API-key, either personal or AirScout internal.\n" +
                       "See https://www.adsbexchange.com/ for details.\n\n" +
                        "This webfeed forces TLS1.2 transport layer security. Though this plugin is compiled for .NET4.0 it needs .NET4.5 or higher installed on this machine to work.\n\n" +
                        "This webfeed will probably not work on Windows XP and Linux/Mono systems";
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
            // check for personal API-key
            if (!String.IsNullOrEmpty(Settings.APIKey))
            {
                APIKey = Settings.APIKey;
                return;
            }
            
            // get AirScout internal key
            try
            {
                WebClient client = new WebClient();
                string result = client.DownloadString(args.GetKeyURL +
                            "?id=" + args.InstanceID +
                            "&key=vrs");
                if (!result.StartsWith("Error:"))
                {
                    result = result.Trim('\"');
                    APIKey = OpenSSLDecrypt(result, args.SessionKey);
                }

            }
            catch (Exception ex)
            {
            }
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
            //            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            //            webrequest.Referer = "http://www.vrs-world.com/";
            //            webrequest.Timeout = Settings.Timeout * 1000;
            //            webrequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:37.0) Gecko/20100101 Firefox/37.0";
            //            webrequest.Accept = "application/json, text/javascript, */*;q=0.01";
            //            webrequest.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            //            webrequest.Headers.Add("api-auth:" + APIKey);
            //            Console.WriteLine("[" + this.GetType().Name + "]: Getting web response");
            //            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            //            Console.WriteLine("[" + this.GetType().Name + "]: Reading stream");
            //            
            //            using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
            //            {
            //                json = sr.ReadToEnd();
            //            }
            //           */
            json = VRSTlsClient.DownloadFile(url, Settings.Timeout * 1000, APIKey);
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
            // 2017-07-23: workaround for "jumping planes" due to incorrect time stamps
            // try to get the server time to adjust the time stamps in plane positions
            // --> compare server time with local time and calculate offset
            // default offset is 0
            long toffset = 0;
            try
            {
                // deserialize JSON
                dynamic root = JsonConvert.DeserializeObject(json);

                // get local time of request in milliseconds
                DateTime lt = DateTime.UtcNow;
                long ltime = (long)(lt - new DateTime(1970, 1, 1)).TotalMilliseconds;
                // get server time in milliseconds
                long stime = ReadPropertyLong(root, "stm");
                DateTime sti = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(stime);
                // calculate offset in milliseconds
                toffset = ltime - stime;
                // check value
                string message = "Server timestamp difference (server <> local): " + sti.ToString("yyyy-MM-dd HH:mm:ss,fffZ") + " <> " + lt.ToString("yyyy-MM-dd HH:mm:ss,fffZ");
                // server time is more than 10.000 ms in the future --> keep offset for correction and log an error message
                // else --> set offset to 0 to work with real timestamps of entries without correction
                if (toffset <= -10000)
                {
                    message += " --> timestamp correction applied!";
                    //                    Log.WriteMessage(message);

                }
                else
                {
                    // clear offset
                    toffset = 0;
                }
                // analyze json string for planes data
                // get the planes position list
                var aclist = root["acList"];
                foreach (var ac in aclist)
                {
                    try
                    {
                        PlaneFeedPluginPlaneInfo plane = new PlaneFeedPluginPlaneInfo();
                        // get hex first
                        plane.Hex = ReadPropertyString(ac, "Icao").Trim().Replace("\"","");
                        // get position
                        plane.Lat = ReadPropertyDouble(ac, "Lat");
                        plane.Lon = ReadPropertyDouble(ac, "Long");
                        // get altitude
                        // 2017-07-23: take "GAlt" (corrected altitude by air pressure) rather than "Alt"
                        plane.Alt = ReadPropertyInt(ac, "GAlt");
                        // get callsign
                        plane.Call = ReadPropertyString(ac, "Call");
                        // get registration
                        plane.Reg = ReadPropertyString(ac, "Reg");
                        // get track
                        plane.Track = ReadPropertyInt(ac, "Trak");
                        // get speed
                        plane.Speed = ReadPropertyInt(ac, "Spd");
                        // get position timestamp
                        // CAUTION!! time is UNIX time in milliseconds
                        long l = ReadPropertyLong(ac, "PosTime");
                        if (l != long.MinValue)
                        {
                            // 2017-07-23: correct timestamp with offset
                            l = l + toffset;
                            DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                            timestamp = timestamp.AddMilliseconds(l);
                            plane.Time = timestamp;
                        }
                        else
                        {
                            // skip plane if no valid timestamp found
                            continue;
                        }
                        // get type info
                        plane.Type = ReadPropertyString(ac, "Type");
                        // get extended plane type info
                        plane.Manufacturer = ReadPropertyString(ac, "Man");
                        plane.Model = ReadPropertyString(ac, "Mdl");
                        long cat = ReadPropertyLong(ac, "WTC");
                        switch (cat)
                        {
                            case 1: plane.Category = 1; break;
                            case 2: plane.Category = 2; break;
                            case 3: plane.Category = 3; break;
                            case 4: plane.Category = 4; break;
                            default: plane.Category = 0; break;
                        }
                        // do correction of A380 as "SuperHeavy" is not supported 
                        if (plane.Type == "A388")
                            plane.Category = 4;
                        // get country
                        plane.Country = ReadPropertyString(ac, "Cou");
                        // get departure airport
                        plane.From = ReadPropertyString(ac, "From");
                        // get destination airport
                        plane.To = ReadPropertyString(ac, "To");
                        // get vertical speed
                        plane.VSpeed = ReadPropertyInt(ac, "Vsi");
                        // add plane to list
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
                // do nothing if property is not found
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
