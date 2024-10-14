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
using System.Xml.Linq;
using System.Xml.Serialization;
using SimpleUdp;

//TODO: Rename namespace to a name of your choice
namespace AirScout.PlaneFeeds.Plugin.ADSBSupport
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
    public class ADSBSupportSettings
    {
        [Browsable(true)]
        [DescriptionAttribute("Server address for UDP server.\nUse localhost for running on the same machine.")]
        [DefaultValue("10.0.2.143")]
        public virtual string Server { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Port number UDP server is listening.")]
        [DefaultValue(58353)]
        public virtual int Port { get; set; }


        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        public ADSBSupportSettings()
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
    public class ADSBSupportPlugin : IPlaneFeedPlugin
    {
        private ADSBSupportSettings Settings = new ADSBSupportSettings();

        static UdpEndpoint UDPEndpoint;
        
        static int count = 0;
        static int errors = 0;
        
        private Dictionary<string, PlaneFeedPluginPlaneInfo> UDPPlanes = new Dictionary<string, PlaneFeedPluginPlaneInfo>();

        // start of interface

        //TODO: Change return values so that they represent plugin's functionality
        #region Interface

        public string Name
        {
            get
            {
                return "[WebFeed]           ADSBSupport";
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
                return "Receiver for ADSBSupport UDP datagrams";
            }
        }
        public bool HasSettings
        {
            get
            {
                return false;
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
            UDPEndpoint = new UdpEndpoint(Settings.Server, Settings.Port);
            UDPEndpoint.EndpointDetected += EndpointDetected;
            UDPEndpoint.DatagramReceived += DatagramReceived;
            UDPEndpoint.ServerStopped += NodeStopped;

        }

        public PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args)
        {
            PlaneFeedPluginPlaneInfoList planes = new PlaneFeedPluginPlaneInfoList();

            foreach (PlaneFeedPluginPlaneInfo info in UDPPlanes.Values)
            {
                planes.Add(info);
            }

            // add code for getting plane info here
            return planes;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            // add code for stopping here
        }

        #endregion

        // End of interface

        private void EndpointDetected(object sender, EndpointMetadata md)
        {
            Console.WriteLine("Endpoint detected: " + md.Ip + ":" + md.Port);
        }

        private void DatagramReceived(object sender, Datagram dg)
        {
            string msg = Encoding.UTF8.GetString(dg.Data);
            //            Console.WriteLine("[" + dg.Ip + ":" + dg.Port + "]: " + msg);
            //            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rcvd.txt"), msg + "\n");
            string[] a;
            double minlat = 0;
            double maxlat = 0;
            double minlon = 0;
            double maxlon = 0;
            try
            {
                a = msg.Split(',');
                PlaneFeedPluginPlaneInfo info = new PlaneFeedPluginPlaneInfo();
                info.Hex = a[1];
                info.Call = a[2];
                info.Alt = System.Convert.ToInt32(a[3]);
                info.Speed = System.Convert.ToInt32(a[4]);
                info.Track = System.Convert.ToInt32(a[5]);
                info.Lat = System.Convert.ToDouble(a[6], CultureInfo.InvariantCulture);
                info.Lon = System.Convert.ToDouble(a[7], CultureInfo.InvariantCulture);
                info.Time = UNIXTimeToDateTime(System.Convert.ToInt32(a[11]));

                // check values
                if ((info.Lat < -90) || (info.Lat > 90)) throw new Exception();
                if ((info.Lon < -180) || (info.Lon > 180)) throw new Exception();
                UDPPlanes[info.Hex] = info;
                count++;

            }
            catch (Exception ex)
            {
                errors++;
            }

            minlat = UDPPlanes.Values.Min(p => p.Lat);
            maxlat = UDPPlanes.Values.Max(p => p.Lat); ;
            minlon = UDPPlanes.Values.Min(p => p.Lon);
            maxlon = UDPPlanes.Values.Max(p => p.Lon);

            Console.WriteLine("Messages = " + count +
                ", Planes:" + UDPPlanes.Count +
                ", Errors" + errors +
                ", Range [" + minlat.ToString("F2") + "," + minlon.ToString("F2") + ":" + maxlat.ToString("F2") + ":" + maxlon.ToString("F2") + "]");

        }

        private static void NodeStarted(object sender, EventArgs e)
        {
            Console.WriteLine("*** Node started");
        }

        private static void NodeStopped(object sender, EventArgs e)
        {
            Console.WriteLine("*** Node stopped");
        }

        private DateTime UNIXTimeToDateTime(int ut)
        {
            if (ut == int.MinValue)
                return DateTime.MinValue;
            else if (ut == int.MaxValue)
                return DateTime.MaxValue;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(ut);
        }


        // ************************************* Helpers ****************************************************

        [System.Diagnostics.DebuggerNonUserCode]
        private string ReadPropertyString(dynamic o, int propertyindex)
        {
            string s = null;
            try
            {
                s = o.Value[propertyindex];
            }
            catch
            {
                // do nothing if something went wrong
            }
            return s;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private int ReadPropertyInt(dynamic o, int propertyindex)
        {
            int i = int.MinValue;
            double d = ReadPropertyDouble(o, propertyindex);
            if ((d != double.MinValue) && (d >= int.MinValue) && (d <= int.MaxValue))
                i = (int)d;
            return i;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private double ReadPropertyDouble(dynamic o, int propertyindex)
        {
            double d = double.MinValue;
            try
            {
                string s = o.Value[propertyindex].ToString(CultureInfo.InvariantCulture);
                d = double.Parse(s, CultureInfo.InvariantCulture);
            }
            catch
            {
                // do nothing if something went wrong
            }
            return d;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private long ReadPropertyLong(dynamic o, int propertyindex)
        {
            long l = long.MinValue;
            try
            {
                l = long.Parse(o.Value[propertyindex].ToString());
            }
            catch
            {
                // do nothing if something went wrong
            }
            return l;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private bool ReadPropertyBool(dynamic o, int propertyindex)
        {
            bool b = false;
            try
            {
                string s = o.Value[propertyindex].ToString();
                b = s.ToLower() == "true";
            }
            catch
            {
                // do nothing if something went wrong
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
