using System;
using System.Reflection;
using System.ComponentModel.Composition;
using System.ComponentModel;
using AirScout.PlaneFeeds.Plugin.MEFContract;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;

namespace AirScout.PlaneFeeds.Plugin
{

    [Serializable]
    public class PlaneFeedPluginSettings
    {
        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        public PlaneFeedPluginSettings()
        {
            // set all properties to their default values according to definition in [XmlElement(...] and overload all properties from configuration file
            foreach (var property in this.GetType().GetProperties())
            {
                try
                {
                    // initialize all properties with default value if set
                    if (Attribute.IsDefined(property, typeof(DefaultValueAttribute)))
                    {
                        property.SetValue(this, ((DefaultValueAttribute)Attribute.GetCustomAttribute(
                            property, typeof(DefaultValueAttribute)))?.Value, null);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            Load();
        }

        public void Load()
        {
            string filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.ToLower().Replace(".dll", ".cfg")).LocalPath;
            // do nothing if file not exists
            if (!File.Exists(filename))
                return;
            try
            {
                string xml = "";
                using (StreamReader sr = new StreamReader(filename))
                {
                    xml = sr.ReadToEnd();
                }
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xml);
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    XmlNodeList list = xdoc.GetElementsByTagName(p.Name);
                    if (list != null)
                    {
                        // setting found
                        p.SetValue(this,list[0].InnerText,null);
                    }
                }
                
            }
            catch (Exception ex)
            {
               Console.WriteLine("[" + this.GetType().Name + "]: Cannot load settings from " + filename + ", " + ex.Message);
            }
        }

        public void Save()
        {
            string filename = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase.ToLower().Replace(".dll", ".cfg")).LocalPath;
            try
            {
                XmlSerializer s = new XmlSerializer(this.GetType());
                s.Serialize(File.Create(filename), this);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("[" + this.GetType().Name + "]: Cannot save settings to " + filename + ", " + ex.Message);
            }
        }

        
    }

    [Export(typeof(IPlaneFeedPlugin))]
    [ExportMetadata("Name", "PlaneFeedPlugin")]
    public class PlaneFeedPlugin : IPlaneFeedPlugin
    {
        protected PlaneFeedPluginSettings Settings = new PlaneFeedPluginSettings();

        public string Name
        {
            get
            {
                return "";
            }
        }
        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string Info { get; }

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
            // nothing to do
        }

        public void LoadSettings()
        {
            // nothing to do
        }

        public void SaveSettings()
        {
            // nothing to do
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
            // nothing to do
        }

        public PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args)
        {
            return null;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            // nothing to do
        }

       // **************************** END OF INTERFACE *********************************

    }
}
