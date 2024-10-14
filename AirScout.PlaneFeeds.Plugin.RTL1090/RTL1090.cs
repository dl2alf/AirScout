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
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Web.Script.Serialization;
using LibADSB;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace AirScout.PlaneFeeds.Plugin.RTL1090
{

    public class RTL1090Settings
    {
        [Browsable(true)]
        [DescriptionAttribute("Server address for raw ADS-B data.\nUse localhost for running on the same machine.")]
        [DefaultValue("localhost")]
        public virtual string Server { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Server port for raw ADS-B data.\nRTL1090: Port 31001, Dump1090: 30005, ADSBSharp: 47806")]
        [DefaultValue(31001)]
        public virtual int Port { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Use binary data format for ADS-B data.\nTrue: Use binary format (ADS Beast with MLAT)\nFalse: Use ASCII format (AVR with/without MLAT)")]
        [DefaultValue(true)]
        public virtual bool Binary { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Use geometric altitudes only (instead of both barometric/geometric) to enhance tracking accuracy.")]
        [DefaultValue(false)]
        public virtual bool UseGeometricAltOnly { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Report ADS-B messages to console output.")]
        [DefaultValue(true)]
        public virtual bool ReportMessages { get; set; }

        [Browsable(false)]
        [DescriptionAttribute("Marks locally received aircrafts by adding '@' to the call sign")]
        [DefaultValue(false)]
        public virtual bool MarkLocal { get; set; }

        [Browsable(false)]
        [DefaultValue("")]
        [XmlIgnore]
        public string DisclaimerAccepted { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Save received aircraft positions to file")]
        [DefaultValue(false)]
        public virtual bool SaveToFile { get; set; }

        [Browsable(true)]
        [DescriptionAttribute("Log all received messages to file")]
        [DefaultValue(false)]
        public virtual bool LogMessagesToFile { get; set; }

        [Browsable(false)]
        [DefaultValue("RTL1090Messages.log")]
        [XmlIgnore]
        public string LogFileName { get; set; }

        public RTL1090Settings()
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

    public class RTLMessage
    {
        public string RawMessage = "";
        public DateTime TimeStamp = DateTime.UtcNow;
        public int SignalStrength = 0;
    }

    [Export(typeof(IPlaneFeedPlugin))]
    [ExportMetadata("Name", "PlaneFeedPlugin")]
    public class RTL1090Plugin : IPlaneFeedPlugin
    {
        private RTL1090Settings Settings = new RTL1090Settings();

        ADSBDecoder Decoder = new ADSBDecoder();

        private BackgroundWorker bw_Receciver;

        public string Name
        {
            get
            {
                return "[RawData]           RTL1090 data ";
            }
        }

        public string Info
        {
            get
            {
                return "Raw data feed from simple ADS-B receivers (DVB-T dongles).\n\n" +
                       "(c) AirScout(www.airscout.eu)\n\n" +
                       "Use this feed together with RTL1090.exe or similar software.\n" +
                       "Feed software must output raw data either binary or ASCII format via TCP.\n" +
                       "Use the follwing settings as default:\n\n" +
                       "RTL1090:    port=31001/binary=true\n" +
                       "Dump1090:   port=30005/binary=true\n" + 
                       "ADSBSharp:  port=47806/binary=false"  ;
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
            // start receiver thread
            if (!bw_Receciver.IsBusy)
            {
                if (Settings.LogMessagesToFile)
                {
                    try
                    {
                        File.WriteAllText(Path.Combine(args.TmpDirectory, Settings.LogFileName), "RTL1090 logging started: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                bw_Receciver.RunWorkerAsync(args);
            }
        }

        public PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args)
        {
            PlaneFeedPluginPlaneInfoList planes = new PlaneFeedPluginPlaneInfoList();
            // time to report planes
            ArrayList list = Decoder.GetPlanes();
            if (list.Count > 0)
            {
                // convert to plane info list
                foreach (ADSBInfo info in list)
                {
                    PlaneFeedPluginPlaneInfo plane = new PlaneFeedPluginPlaneInfo();
                    plane.Time = info.Timestamp;
                    plane.Hex = info.ICAO24;
                    // mark call with "@" if option is enabled
                    plane.Call = (Settings.MarkLocal) ? "@" + info.Call : info.Call;
                    plane.Lat = info.Lat;
                    plane.Lon = info.Lon;
                    // use geometric altitude, if enabled and available
                    if (Settings.UseGeometricAltOnly)
                    {
                        if (info.GeoMinusBaro != int.MinValue)
                            plane.Alt = info.BaroAlt + info.GeoMinusBaro;
                        else
                            plane.Alt = int.MinValue;
                    }
                    else
                    {
                        plane.Alt = info.BaroAlt; // + info.GeoMinusBaro;
                    }
                    plane.Speed = info.Speed;
                    plane.Track = info.Heading;
                    plane.Reg = "[unknown]";
                    plane.Type = "[unknown]";
                    plane.Manufacturer = "[unknown]";
                    plane.Model = "[unknown]";
                    plane.Category = 0;

                    // add only valid positions
                    if (plane.Alt > 0)
                    {
                        planes.Add(plane);
                    }
                }
                // save raw data to file if enabled
                if (Settings.SaveToFile)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(planes);
                    using (StreamWriter sw = new StreamWriter(args.TmpDirectory + Path.DirectorySeparatorChar + this.GetType().Name + "_" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss") + ".json"))
                    {
                        sw.WriteLine(json);
                    }
                }

            }
            Console.WriteLine("[" + this.GetType().Name + "]: Returning " + planes.Count + " planes");
            return planes;
        }

        public void Stop(PlaneFeedPluginArgs args)
        {
            while (bw_Receciver.IsBusy)
            {
                bw_Receciver.CancelAsync();
            }
            Settings.Save(true);
        }

        // end of Interface

        public RTL1090Plugin()
        {
            bw_Receciver = new BackgroundWorker();
            bw_Receciver.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Receiver_DoWork);
            bw_Receciver.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Receiver_ProgressChanged);
            bw_Receciver.WorkerReportsProgress = true;
            bw_Receciver.WorkerSupportsCancellation = true;
        }

        private void bw_Receiver_DoWork(object sender, DoWorkEventArgs e)
        {
            PlaneFeedPluginArgs args = (PlaneFeedPluginArgs)e.Argument;

            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            StreamReader sr = null;
            TcpClient client = null;
            RTLMessage msg = null;
            // outer loop
            do
            {
                try
                {
                    // setup TCP listener
                    client = new TcpClient();
                    // set receive timeout to 1s
                    client.ReceiveTimeout = 1000;
                    string server = Settings.Server;
                    client.Connect(server, Settings.Port);
                    sr = new StreamReader(client.GetStream());
                    // inner loop
                    // receive messages in a loop
                    do
                    {
                        if (Settings.Binary)
                            msg = ReceiveBinaryMsg(sr.BaseStream);
                        else
                            msg = ReceiveAVRMsg(sr);
                        // decode the message
                        if (msg.RawMessage.StartsWith("*") && msg.RawMessage.EndsWith(";"))
                        {
                            // try to decode the message
                            string info = "";
                            string line = "";
                            try
                            {
                                info = "[" + this.GetType().Name + "]: " + msg.RawMessage + "-- > ";
                                Console.Write(info);
                                line = info;
                                info = Decoder.DecodeMessage(msg.RawMessage, msg.TimeStamp, Settings.UseGeometricAltOnly);
                                line = line + info + "\n";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            Console.WriteLine(info);
                            
                            if (Settings.LogMessagesToFile)
                            {
                                try
                                {
                                    File.AppendAllText(Path.Combine(args.TmpDirectory, Settings.LogFileName), line);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    while ((msg != null) && !bw_Receciver.CancellationPending);
                }
                catch (Exception ex)
                {
                    // report error
                    Console.WriteLine("Error reading from TCP connection: " + ex.Message);
                    // wait 10 sec
                    int i = 0;
                    while ((i < 10) && !bw_Receciver.CancellationPending)
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                }
                finally
                {
                    // try to close the stream and TCP client
                    try
                    {
                        if (sr != null)
                            sr.Close();
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (client != null)
                            client.Close();
                    }
                    catch
                    {
                    }
                }
            }
            while (!bw_Receciver.CancellationPending);
        }

        private void bw_Receiver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private RTLMessage ReceiveBinaryMsg(Stream stream)
        {
            // read Mode-S beast binary input
            string RTL = null;
            int signal_strength = 0;
            long nanosec = 0;
            long daysec = 0;
            DateTime timestamp = DateTime.UtcNow;
            byte[] buffer = new byte[23];
            // wait for escape character
            DateTime start = DateTime.UtcNow;
            DateTime stop = DateTime.Now;
            do
            {
                stream.Read(buffer, 0, 1);
                //                System.Console.WriteLine(BitConverter.ToString(buffer,0,1));
                if (buffer[0] == 0x1A)
                {
                    // read next character
                    stream.Read(buffer, 1, 1);
                    switch (buffer[1])
                    {
                        case 0x31:
                            // do not decode
                            RTL = null;
                            break;
                        case 0x32:
                            // 7 byte short frame
                            // read timestamp
                            stream.Read(buffer, 2, 6);
                            nanosec = ((buffer[4] & 0x3f) << 24) |
                                (buffer[5] << 16) |
                                (buffer[6] << 8) |
                                (buffer[7]);
                            daysec = (buffer[2] << 10) |
                                (buffer[3] << 2) |
                                (buffer[4] >> 6);
                            timestamp = DateTime.Today.AddSeconds(daysec);
                            timestamp = timestamp.AddMilliseconds(nanosec / 1000);
                            // plausibility check
                            if (Math.Abs((DateTime.Now - timestamp).Seconds) > 10)
                            {
                                // time difference > 10sec --> discard timestamp
                                timestamp = DateTime.UtcNow;
                            }
                            // read signal strength
                            stream.Read(buffer, 8, 1);
                            // plausibility check
                            if (Math.Abs((DateTime.Now - timestamp).Seconds) > 10)
                            {
                                // time difference > 10sec --> discard timestamp
                                timestamp = DateTime.UtcNow;
                            }
                            signal_strength = buffer[8];
                            // read frame
                            stream.Read(buffer, 9, 7);
                            // convert to AVR string
                            RTL = BitConverter.ToString((byte[])buffer, 9, 7).Replace("-", String.Empty);
                            RTL = "*" + RTL + ";";
                            break;
                        case 0x33:
                            // 14 byte long frame
                            // read timestamp
                            stream.Read(buffer, 2, 6);
                            nanosec = ((buffer[4] & 0x3f) << 24) |
                                (buffer[5] << 16) |
                                (buffer[6] << 8) |
                                (buffer[7]);

                            daysec = (buffer[2] << 10) |
                                (buffer[3] << 2) |
                                (buffer[4] >> 6);
                            timestamp = DateTime.Today.AddSeconds(daysec);
                            timestamp = timestamp.AddMilliseconds(nanosec / 1000);
                            // plausibility check
                            if (Math.Abs((DateTime.Now - timestamp).Seconds) > 10)
                            {
                                // time difference > 10sec --> discard timestamp
                                timestamp = DateTime.UtcNow;
                            }
                            // read signal strength
                            stream.Read(buffer, 8, 1);
                            signal_strength = buffer[8];
                            // read frame
                            stream.Read(buffer, 9, 14);
                            // convert to AVR string
                            RTL = BitConverter.ToString((byte[])buffer, 9, 14).Replace("-", String.Empty);
                            RTL = "*" + RTL + ";";
                            break;
                        default:
                            // false decode
                            RTL = null;
                            break;
                    }
                }
                // check for timeout 10sec
                stop = DateTime.UtcNow;
                if (stop - start > new TimeSpan(0, 0, 10))
                    throw new TimeoutException();
            }
            //            while ((RTL == null) && !this.CancellationPending);
            while (RTL == null);
            if (RTL == null)
                return null;
            RTLMessage msg = new RTLMessage();
            msg.RawMessage = RTL;
            msg.TimeStamp = timestamp;
            msg.SignalStrength = signal_strength;
            return msg;
        }

        private RTLMessage ReceiveAVRMsg(StreamReader sr)
        {
            RTLMessage msg = new RTLMessage();
            // read AVR format input
            msg.RawMessage = sr.ReadLine();
            if (msg.RawMessage.StartsWith("*"))
            {
                // standard AVR message
                // no timestamp in telegram --> set timestamp after reading
                msg.TimeStamp = DateTime.UtcNow;
                msg.SignalStrength = 0;
            }
            else if (msg.RawMessage.StartsWith("@"))
            {
                // extended AVR message wit MLAT
                // convert into standard message format
                // extract time string
                string time = msg.RawMessage.Substring(1, 12);
                // TODO: interprete the MLAT timestamp!
                msg.TimeStamp = DateTime.UtcNow;
                msg.RawMessage = "*" + msg.RawMessage.Remove(0, 13);
            }
            return msg;
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

}
