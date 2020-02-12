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

//TODO: Rename namespace to a name of your choice
namespace AirScout.PlaneFeeds.Plugin.Template
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
    public class TemplateSettings
    {
        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        public TemplateSettings()
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
    public class TemplatePlugin : IPlaneFeedPlugin
    {
        private TemplateSettings Settings = new TemplateSettings();

        // start of interface

        //TODO: Change return values so that they represent plugin's functionality
        #region Interface

        public string Name
        {
            get
            {
                return "[WebFeed]           Template";
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
                return "Generic template for ceration of new plane feed projects";
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
        }

        public PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args)
        {
            PlaneFeedPluginPlaneInfoList planes = new PlaneFeedPluginPlaneInfoList();
            // add code for getting plane info here
            return planes;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            // add code for stopping here
        }

        #endregion

        // End of interface


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
