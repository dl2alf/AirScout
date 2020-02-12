using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using AirScout.PlaneFeeds;
using AirScout.PlaneFeeds.Generic;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Deployment;
using System.Deployment.Application;
using System.Security.Cryptography;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using AirScout.Database.Flightradar;
using System.Collections;
using AirScout.Database.Core;
using ScoutBase.Core;
using ScoutBase.Data;
using ScoutBase.Elevation;

namespace AirScoutPlaneServer
{
    public partial class MainDlg : Form
    {
        string AppName = "AirScout PlaneServer";


        // temp variable for local culture
        private CultureInfo LocalCulture;

        //        private Flightradar FlightRadar;

        private LogWriter Log;

        GMapOverlay gmo_Planes_Coverage = new GMapOverlay("Coveragepolygons");

        private DateTime StartupTime;

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Application Directory")]
        public string AppDirectory
        {
            get
            {
               return Application.StartupPath.TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Local Application Data Directory")]
        public string AppDataDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName).TrimEnd(Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Logfile Directory")]
        public string LogDirectory
        {
            get
            {
                return VC.ReplaceAllVars(Properties.Settings.Default.Log_Directory).TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Tempfile Directory")]
        public string TmpDirectory
        {
            get
            {
                return VC.ReplaceAllVars(Properties.Settings.Default.Tmp_Directory).TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Database Directory")]
        public string DatabaseDirectory
        {
            get
            {
                return VC.ReplaceAllVars(Properties.Settings.Default.Database_Directory).TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Icon Directory")]
        public string IconDirectory
        {
            get
            {
                return VC.ReplaceAllVars(Properties.Settings.Default.Icon_Directory).TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Icon Path")]
        public string IconPath
        {
            get
            {
                return Path.Combine(IconDirectory, Properties.Settings.Default.Icon_Filename);
            }
        }

        private ELEVATIONMODEL model;
        [CategoryAttribute("Elevation")]
        [DescriptionAttribute("Elevation Model")]
        public ELEVATIONMODEL Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
            }

        }

        VarConverter VC;

        public ArrayList PlaneFeeds;
        public PlaneFeed bw_WebFeed1;
        public PlaneFeed bw_WebFeed2;
        public PlaneFeed bw_WebFeed3;

        public MainDlg()
        {
            // save current local LocalCulture
            LocalCulture = Application.CurrentCulture;

            // force culture invariant language for GUI
            Application.CurrentCulture = CultureInfo.InvariantCulture;

            // Initailize variables
            VC = new VarConverter();
            VC.AddVar("APPDIR", AppDirectory);
            VC.AddVar("DATADIR", AppDataDirectory);
            VC.AddVar("LOGDIR", LogDirectory);
            VC.AddVar("DATABASEDIR", DatabaseDirectory);
            VC.AddVar("MINLAT", Properties.Settings.Default.Planes_MinLat);
            VC.AddVar("MAXLAT", Properties.Settings.Default.Planes_MaxLat);
            VC.AddVar("MINLON", Properties.Settings.Default.Planes_MinLon);
            VC.AddVar("MAXLON", Properties.Settings.Default.Planes_MaxLon);
            VC.AddVar("MINALTM", Properties.Settings.Default.Planes_MinAlt);
            VC.AddVar("MAXALTM", Properties.Settings.Default.Planes_MaxAlt);
            VC.AddVar("MINALTFT", (int)UnitConverter.m_ft((double)Properties.Settings.Default.Planes_MinAlt));
            VC.AddVar("MAXALTFT", (int)UnitConverter.m_ft((double)Properties.Settings.Default.Planes_MaxAlt));

            // initilaize web feeds selection boxes and background workers
            InitializeWebFeeds();

            InitializeComponent();

            // uprade settings from previous versions on first run (if not Linux)
            if (Properties.Settings.Default.Application_FirstRun && !SupportFunctions.IsMono)
            {
                Console.WriteLine("First run...");
                Properties.Settings.Default.Upgrade();
                foreach (SettingsPropertyValue p in Properties.Settings.Default.PropertyValues)
                {
                    Console.WriteLine(p.Name + ":" + p.PropertyValue.ToString());
                }
            }

            // open database
            if (Properties.Settings.Default.WebFeed1_Settings != null)
            {
                Console.WriteLine("WebFeed1 Settings:" + Properties.Settings.Default.WebFeed1_Settings.URL);
            }
            if (Properties.Settings.Default.WebFeed2_Settings != null)
            {
                Console.WriteLine("WebFeed2 Settings:" + Properties.Settings.Default.WebFeed2_Settings.URL);
            }
            if (Properties.Settings.Default.WebFeed3_Settings != null)
            {
                Console.WriteLine("WebFeed3 Settings:" + Properties.Settings.Default.WebFeed3_Settings.URL);
            }
            // load application icon here
            Icon = new Icon(IconPath);
            // load notifictaion icon here (if not Linux)
            if (!SupportFunctions.IsMono)
                ni_Main.Icon = new Icon(IconPath);

            // check for all necessary directories
            CheckDirectories();
            // implement an Idle procedure
//             Application.Idle += new EventHandler(OnIdle);

            // configure LogWriter
            ScoutBase.Core.Properties.Settings.Default.LogWriter_Directory = LogDirectory;
            ScoutBase.Core.Properties.Settings.Default.LogWriter_FileFormat = Application.ProductName + "_{0:yyyy-MM-dd}.log";
            ScoutBase.Core.Properties.Settings.Default.LogWriter_MessageFormat = "{0:u}: {1}";
            Log = LogWriter.Instance;
            Console.WriteLine("Initializing logfile in: " + LogDirectory);
            Log.WriteMessage(Application.ProductName + " is starting up.", 0, false);
            
            Model = ELEVATIONMODEL.GLOBE;

            // initialize database
            InitializeDatabase();
            // set initial settings for planes tab
            cb_WebFeed1.DisplayMember = "Name";
            cb_WebFeed2.DisplayMember = "Name";
            cb_WebFeed3.DisplayMember = "Name";
            cb_WebFeed1.Items.Clear();
            cb_WebFeed2.Items.Clear();
            cb_WebFeed3.Items.Clear();
            cb_WebFeed1.Items.Add("[none]");
            cb_WebFeed2.Items.Add("[none]");
            cb_WebFeed3.Items.Add("[none]");

            // get installed plane feeds
            ArrayList feeds = new PlaneFeedEnumeration().EnumFeeds();
            foreach (PlaneFeed feed in feeds)
            {
                cb_WebFeed1.Items.Add(feed);
                cb_WebFeed2.Items.Add(feed);
                cb_WebFeed3.Items.Add(feed);
            }
            cb_WebFeed1.SelectedIndex = cb_WebFeed1.FindStringExact(Properties.Settings.Default.Planes_WebFeed1);
            cb_WebFeed2.SelectedIndex = cb_WebFeed1.FindStringExact(Properties.Settings.Default.Planes_WebFeed2);
            cb_WebFeed3.SelectedIndex = cb_WebFeed1.FindStringExact(Properties.Settings.Default.Planes_WebFeed3);

            double d = ElevationData.Database[51, 10, Model];
        }

        private void InitializeDatabase()
        {
            Log.WriteMessage("Initialize database: " + AircraftDatabase_old.GetDBLocation());
        }

        private void InitializeWebFeeds()
        {
            // get available plane feeds
            PlaneFeeds = new PlaneFeedEnumeration().EnumFeeds();
            // set plane feed event handler
            foreach (PlaneFeed feed in PlaneFeeds)
            {
                feed.ProgressChanged += new ProgressChangedEventHandler(bw_WebFeed_ProgressChanged);
            }
            PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
            args.AppDirectory = AppDirectory;
            args.AppDataDirectory = AppDataDirectory;
            args.LogDirectory = LogDirectory;
            args.TmpDirectory = TmpDirectory;
            args.DatabaseDirectory = DatabaseDirectory;
            args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
            args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
            args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
            args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
            args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
            args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
            // select feeds and start them
            foreach (PlaneFeed feed in PlaneFeeds)
            {
                if (Properties.Settings.Default.Planes_WebFeed1 == feed.Name)
                {
                    bw_WebFeed1 = feed;
                    bw_WebFeed1.RunWorkerAsync(args);
                }
                if (Properties.Settings.Default.Planes_WebFeed2 == feed.Name)
                {
                    bw_WebFeed2 = feed;
                    bw_WebFeed2.RunWorkerAsync(args);
                }
                if (Properties.Settings.Default.Planes_WebFeed3 == feed.Name)
                {
                    bw_WebFeed3 = feed;
                    bw_WebFeed3.RunWorkerAsync(args);
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            // close window
            this.Close();
        }

        private void MainDlg_Load(object sender, EventArgs e)
        {
            // start minimized if enabled
            if (Properties.Settings.Default.Windows_Startup_Minimized)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
            // reset FirstRun flag
            Properties.Settings.Default.Application_FirstRun = false;

            // set initial settings for CoverageMap
            // setting User Agent to fix Open Street Map issue 2016-09-20
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_Planes_Coverage.MapProvider.RefererUrl = "";
            gm_Planes_Coverage.MapProvider = GMapProviders.Find(Properties.Settings.Default.Planes_MapProvider);
            gm_Planes_Coverage.IgnoreMarkerOnMouseWheel = true;
            gm_Planes_Coverage.MinZoom = 0;
            gm_Planes_Coverage.MaxZoom = 20;
            gm_Planes_Coverage.Zoom = 6;
            gm_Planes_Coverage.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Planes_Coverage.CanDragMap = true;
            gm_Planes_Coverage.ScalePen = new Pen(Color.Black, 3);
            gm_Planes_Coverage.HelperLinePen = null;
            gm_Planes_Coverage.SelectionPen = null;
            gm_Planes_Coverage.MapScaleInfoEnabled = true;
            gm_Planes_Coverage.Overlays.Add(gmo_Planes_Coverage);

            // keep startup time
            StartupTime = DateTime.UtcNow;

            // start database

            // enable/disable functions
            cb_WebFeed1_Active_CheckedChanged(this, null);
            cb_WebFeed2_Active_CheckedChanged(this, null);
            cb_WebFeed3_Active_CheckedChanged(this, null);
            cb_Log_Active_CheckedChanged(this, null);
            cb_Server_Active_CheckedChanged(this, null);

            // start timer
            ti_Main.Enabled = true;
            ti_Main.Start();

            // start background workers
            bw_JSONWriter.RunWorkerAsync();
            bw_DatabaseUpdater.RunWorkerAsync();
        }

        #region Misc Functions

        private void Say(string s)
        {
            if ((s != null) && (tsl_Main.Text != s))
            {
                tsl_Main.Text = s;
                ss_Main.Update();
            }
        }

        
        #endregion

        #region Directory Functions

        private void CheckDirectories()
        {
            // check if all direcrtories exist --> create them if not
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
            if (!Directory.Exists(AppDataDirectory))
                Directory.CreateDirectory(AppDataDirectory);
            if (!Directory.Exists(DatabaseDirectory))
                Directory.CreateDirectory(DatabaseDirectory);
            if (!Directory.Exists(IconDirectory))
                Directory.CreateDirectory(IconDirectory);

        }

        #endregion

        private void OnIdle(object sender, EventArgs args)
        {
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clean up map
            gm_Planes_Coverage.Dispose();

            // save user settings
            SaveUserSettings();
            // close database updater
            bw_DatabaseUpdater.CancelAsync();
            // close JSON writer
            bw_JSONWriter.CancelAsync();
            // close webserver
            bw_Webserver.CancelAsync();
            // close all feeds
            if (bw_WebFeed1 != null)
                bw_WebFeed1.CancelAsync();
            if (bw_WebFeed2 != null)
                bw_WebFeed2.CancelAsync();
            if (bw_WebFeed3 != null)
                bw_WebFeed3.CancelAsync();
            bw_ADSBFeed.CancelAsync();
            // stop timer
            ti_Main.Stop();
            // close database
        }
        
        private void MainDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.WriteMessage(Application.ProductName + " is closed.", 0, false);
        }

        private void MainDlg_Resize(object sender, EventArgs e)
        {
            // cut window to normal size when maximized
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            // move to systray when enabled (not under Linux)
            if (!SupportFunctions.IsMono && (WindowState == FormWindowState.Minimized))
            {
                if (Properties.Settings.Default.Windows_Startup_Systray)
                {
                    ni_Main.Visible = true;
                    ni_Main.BalloonTipText = "AirScout PlaneServer minimized to systray.";
                    ni_Main.ShowBalloonTip(500);
                    this.Hide();
                    this.ShowInTaskbar = false;
                }
            }
        }

        private void tsi_Restore_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            ni_Main.Visible = false;
        }

        private void tsi_Close_Click(object sender, EventArgs e)
        {
            // close window
            this.Close();
        }

        private void cms_NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            ni_Main.Visible = false;
        }

        private void cms_NotifyIcon_Opening(object sender, CancelEventArgs e)
        {
            tsi_Restore.Select();
        }

        private void cb_Windows_Startup_Autorun_CheckedChanged(object sender, EventArgs e)
        {
            // try to manage an autorun on Windows startup
            // this does not work with Linux
            // this may not work with Windows 8 or later versions
            try
            {
                // The path to the key where Windows looks for startup applications
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (cb_Windows_Startup_Autorun.Checked)
                {
                    // Add the value in the registry so that the application runs at startup
                    rkApp.SetValue(AppName, Application.ExecutablePath.ToString());
                }
                else
                {
                    // Remove the value from the registry so that the application doesn't start
                    rkApp.DeleteValue(AppName, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while accessing registry key");
            }
        }

        private void SaveUserSettings()
        {
            Log.WriteMessage("Saving configuration...");
            Properties.Settings.Default.Save();
            Console.WriteLine("Creating XML document...");
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement configuration = doc.CreateElement(string.Empty, "configuration", string.Empty);
            doc.AppendChild(configuration);
            XmlElement configsections = doc.CreateElement(string.Empty, "configSections", string.Empty);
            configuration.AppendChild(configsections);
            XmlElement usersettingsgroup = doc.CreateElement(string.Empty, "sectionGroup", string.Empty);
            XmlAttribute usersettingsname = doc.CreateAttribute(string.Empty, "name", string.Empty);
            usersettingsname.InnerText = "userSettings";
            usersettingsgroup.Attributes.Append(usersettingsname);
            XmlElement usersection = doc.CreateElement(string.Empty, "section", string.Empty);
            XmlAttribute sectionname = doc.CreateAttribute(string.Empty, "name", string.Empty);
            sectionname.InnerText = "AirScout.PlaneFeeds.Properties.Settings";
            usersection.Attributes.Append(sectionname);
            XmlAttribute sectiontype = doc.CreateAttribute(string.Empty, "type", string.Empty);
            sectiontype.InnerText = "System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            usersection.Attributes.Append(sectiontype);
            XmlAttribute sectionallowexedefinition = doc.CreateAttribute(string.Empty, "allowExeDefinition", string.Empty);
            sectionallowexedefinition.InnerText = "MachineToLocalUser";
            usersection.Attributes.Append(sectionallowexedefinition);
            XmlAttribute sectionrequirepermission = doc.CreateAttribute(string.Empty, "requirePermission", string.Empty);
            sectionrequirepermission.InnerText = "false";
            usersection.Attributes.Append(sectionrequirepermission);
            usersettingsgroup.AppendChild(usersection);
            configsections.AppendChild(usersettingsgroup);
            XmlElement usersettings = doc.CreateElement(string.Empty, "userSettings", string.Empty);
            configuration.AppendChild(usersettings);
            XmlElement properties = doc.CreateElement(string.Empty, Properties.Settings.Default.ToString(), string.Empty);
            usersettings.AppendChild(properties);
            Console.WriteLine("Writing Properties.Settings...");
            foreach (SettingsPropertyValue p in Properties.Settings.Default.PropertyValues)
            {
                Console.WriteLine(string.Concat(new object[]
    		        {
	    		        "Writing ",
		    	        p.Name,
			            " = ",
			            p.PropertyValue
                }));
                XmlElement setting = doc.CreateElement(string.Empty, "setting", string.Empty);
                XmlAttribute name = doc.CreateAttribute(string.Empty, "name", string.Empty);
                name.InnerText = p.Name.ToString();
                setting.Attributes.Append(name);
                XmlAttribute serializeas = doc.CreateAttribute(string.Empty, "serializeAs", string.Empty);
                serializeas.InnerText = p.Property.SerializeAs.ToString();
                setting.Attributes.Append(serializeas);
                XmlElement value = doc.CreateElement(string.Empty, "value", string.Empty);
                if (p.PropertyValue != null && p.Property.SerializeAs == SettingsSerializeAs.String)
                {
                    XmlText text = doc.CreateTextNode(p.PropertyValue.ToString());
                    value.AppendChild(text);
                }
                else
                {
                    if (p.PropertyValue != null && p.Property.SerializeAs == SettingsSerializeAs.Xml)
                    {
                        MemoryStream ms = new MemoryStream();
                        XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings
                        {
                            NewLineOnAttributes = true,
                            OmitXmlDeclaration = true
                        });
                        XmlSerializer serializer = new XmlSerializer(p.PropertyValue.GetType());
                        serializer.Serialize(writer, p.PropertyValue);
                        byte[] text2 = new byte[ms.ToArray().Length - 3];
                        Array.Copy(ms.ToArray(), 3, text2, 0, text2.Length);
                        XmlText xml = doc.CreateTextNode(Encoding.UTF8.GetString(text2.ToArray<byte>()));
                        value.AppendChild(xml);
                        value.InnerXml = WebUtility.HtmlDecode(value.InnerXml);
                    }
                }
                setting.AppendChild(value);
                properties.AppendChild(setting);
            }
            string usersettingspath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            usersettingspath = Path.Combine(usersettingspath, Application.CompanyName, AppDomain.CurrentDomain.FriendlyName);
            usersettingspath += "_Url_";
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }
            byte[] pkt = assembly.GetName().GetPublicKeyToken();
            byte[] hash = SHA1.Create().ComputeHash((pkt != null && pkt.Length > 0) ? pkt : Encoding.UTF8.GetBytes(assembly.EscapedCodeBase));
            StringBuilder evidence_string = new StringBuilder();
            byte[] array = hash;
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                evidence_string.AppendFormat("{0:x2}", b);
            }
            usersettingspath += evidence_string.ToString();
            if (!Directory.Exists(usersettingspath))
            {
                Directory.CreateDirectory(usersettingspath);
            }
            usersettingspath = Path.Combine(usersettingspath, "user.config");
            doc.Save(usersettingspath);
        }


        # region User Interface

        private void btn_Log_Directory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.SelectedPath = LogDirectory;
            Dlg.ShowNewFolderButton = true;
            Dlg.Description = "Select Logfile Directory";
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_Log_Directory.Text = Dlg.SelectedPath;
                CheckDirectories();
            }
        }

        private void cb_Log_Active_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Log_Active.Checked)
            {
                // disable directory change
                tb_Log_Directory.Enabled = false;
                btn_Log_Directory.Enabled = false;
                // set logfile path if path exists
                if (Directory.Exists(LogDirectory))
                    ScoutBase.Core.Properties.Settings.Default.LogWriter_Directory = LogDirectory;
            }
            else
            {
                // enable directory change
                tb_Log_Directory.Enabled = true;
                btn_Log_Directory.Enabled = true;
            }
        }

        private void btn_Log_Show_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.DefaultExt = ".log";
            Dlg.Filter = "Log Files | *.log";
            Dlg.InitialDirectory = LogDirectory;
            Dlg.CheckFileExists = true;
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Process.Start(Dlg.FileName);
            }
        }

        private void ti_Main_Tick(object sender, EventArgs e)
        {
            // check and update status
            TimeSpan uptime = DateTime.UtcNow - StartupTime;
            tsl_Uptime.Text = uptime.ToString(@"d\.hh\:mm\:ss");
            // get performance counter values and avoid exceptions
            try
            {
                tsl_CPULoad.Text = "cpu: " + SupportFunctions.CPUCounter.GetLoad().ToString("F0") + " %";
                tsl_MemoryAvailable.Text = "free: " + SupportFunctions.MemoryCounter.GetAvailable().ToString("F0") + " MB";
            }
            catch (Exception ex)
            {
                tsl_CPULoad.Text = "cpu: " + "***";
                tsl_MemoryAvailable.Text = "free: " + "***";
            }
            tsl_DBSize.Text = "DB: " + AircraftDatabase_old.GetDBSize().ToString("F0") + " MB";
        }

        private void Planes_AdjustMap()
        {
            gmo_Planes_Coverage.Clear();
            // add tile to map polygons
            List<PointLatLng> l = new List<PointLatLng>();
            l.Add(new PointLatLng(Properties.Settings.Default.Planes_MinLat, Properties.Settings.Default.Planes_MinLon));
            l.Add(new PointLatLng(Properties.Settings.Default.Planes_MinLat, Properties.Settings.Default.Planes_MaxLon));
            l.Add(new PointLatLng(Properties.Settings.Default.Planes_MaxLat, Properties.Settings.Default.Planes_MaxLon));
            l.Add(new PointLatLng(Properties.Settings.Default.Planes_MaxLat, Properties.Settings.Default.Planes_MinLon));
            GMapPolygon p = new GMapPolygon(l, "Coverage");
            p.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            p.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            gmo_Planes_Coverage.Polygons.Add(p);
            // zoom the map
            gm_Planes_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB((double)Properties.Settings.Default.Planes_MinLon - 1, (double)Properties.Settings.Default.Planes_MaxLat + 1, (double)Properties.Settings.Default.Planes_MaxLon + 1, (double)Properties.Settings.Default.Planes_MinLat - 1));
        }

        private void tab_Planes_Enter(object sender, EventArgs e)
        {
            Planes_AdjustMap();
        }

        private void ud_Planes_LatLon_ValueChanged(object sender, EventArgs e)
        {
            Planes_AdjustMap();
        }

        private void tp_WebFeed1_Enter(object sender, EventArgs e)
        {
            cb_WebFeed1.SelectedIndex = cb_WebFeed1.FindStringExact(Properties.Settings.Default.Planes_WebFeed1);
        }

        private void tp_WebFeed2_Enter(object sender, EventArgs e)
        {
            cb_WebFeed2.SelectedIndex = cb_WebFeed1.FindStringExact(Properties.Settings.Default.Planes_WebFeed2);
        }

        private void tp_WebFeed3_Enter(object sender, EventArgs e)
        {
            cb_WebFeed3.SelectedIndex = cb_WebFeed1.FindStringExact(Properties.Settings.Default.Planes_WebFeed3);
        }

        private void cb_WebFeed1_Active_CheckedChanged(object sender, EventArgs e)
        {
            /*
            if (cb_WebFeed1_Active.Checked)
            {
                if (!bw_WebFeed1.IsBusy)
                {
                    if (Properties.Settings.Default.WebFeed1_Settings != null)
                    {
                        PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
                        args.AppDirectory = AppDirectory;
                        args.AppDataDirectory = AppDataDirectory;
                        args.LogDirectory = LogDirectory;
                        args.TmpDirectory = TmpDirectory;
                        args.DatabaseDirectory = DatabaseDirectory;
                        args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
                        args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
                        args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
                        args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
                        args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
                        args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
                        bw_WebFeed1.RunWorkerAsync(args);
                    }
                }
            }
            else
            {
                if (bw_WebFeed1.IsBusy)
                    bw_WebFeed1.CancelAsync();
            }
            // enable/disable property grid
            pg_WebFeed1.Enabled = !cb_WebFeed1_Active.Checked;
            */
        }

        private void cb_WebFeed2_Active_CheckedChanged(object sender, EventArgs e)
        {
            /*
            if (cb_WebFeed2_Active.Checked)
            {
                if (!bw_WebFeed2.IsBusy)
                {
                    if (Properties.Settings.Default.WebFeed2_Settings != null)
                    {
                        PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
                        args.AppDirectory = AppDirectory;
                        args.AppDataDirectory = AppDataDirectory;
                        args.LogDirectory = LogDirectory;
                        args.TmpDirectory = TmpDirectory;
                        args.DatabaseDirectory = DatabaseDirectory;
                        args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
                        args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
                        args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
                        args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
                        args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
                        args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
                        bw_WebFeed2.RunWorkerAsync(args);
                    }
                }
            }
            else
            {
                if (bw_WebFeed2.IsBusy)
                    bw_WebFeed2.CancelAsync();
            }
            // enable/disable property grid
            pg_WebFeed2.Enabled = !cb_WebFeed2_Active.Checked;
            */
        }

        private void cb_WebFeed3_Active_CheckedChanged(object sender, EventArgs e)
        {
            /*
            if (cb_WebFeed3_Active.Checked)
            {
                if (!bw_WebFeed3.IsBusy)
                {
                    if (Properties.Settings.Default.WebFeed3_Settings != null)
                    {
                        PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
                        args.AppDirectory = AppDirectory;
                        args.AppDataDirectory = AppDataDirectory;
                        args.LogDirectory = LogDirectory;
                        args.TmpDirectory = TmpDirectory;
                        args.DatabaseDirectory = DatabaseDirectory;
                        args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
                        args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
                        args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
                        args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
                        args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
                        args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
                        bw_WebFeed3.RunWorkerAsync(args);
                    }
                }
            }
            else
            {
                if (bw_WebFeed3.IsBusy)
                    bw_WebFeed3.CancelAsync();
            }
            // enable/disable property grid
            pg_WebFeed3.Enabled = !cb_WebFeed3_Active.Checked;
            */
        }

        private void cb_WebFeed1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_WebFeed1.SelectedItem == null) || (cb_WebFeed1.GetItemText(cb_WebFeed1.SelectedItem) == "[none]"))
            {
                pg_WebFeed1.Enabled = false;
                btn_WebFeed1_Import.Enabled = false;
                btn_WebFeed1_Export.Enabled = false;
                bw_WebFeed1 = null;
                Properties.Settings.Default.Planes_WebFeed1 = "[none]";
                return;
            }
            // get the selected type of feed
            PlaneFeed feed = (PlaneFeed)cb_WebFeed1.SelectedItem;
            // check for changes
            if ((bw_WebFeed1 == null) || bw_WebFeed1.Name != feed.Name)
            {
                // stop background worker
                if (bw_WebFeed1 != null)
                {
                    bw_WebFeed1.CancelAsync();
                    while (bw_WebFeed1.IsBusy)
                        Application.DoEvents();
                }
                // select feed
                foreach (PlaneFeed newfeed in PlaneFeeds)
                {
                    if (feed.Name == newfeed.Name)
                        bw_WebFeed1 = newfeed;
                }
                PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
                args.AppDirectory = AppDirectory;
                args.AppDataDirectory = AppDataDirectory;
                args.LogDirectory = LogDirectory;
                args.TmpDirectory = TmpDirectory;
                args.DatabaseDirectory = DatabaseDirectory;
                args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
                args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
                args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
                args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
                args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
                args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
                bw_WebFeed1.RunWorkerAsync(args);
            }
            pg_WebFeed1.SelectedObject = bw_WebFeed1.GetFeedSettings();
            pg_WebFeed1.Enabled = feed.HasSettings;
            btn_WebFeed1_Import.Enabled = feed.CanImport;
            btn_WebFeed1_Export.Enabled = feed.CanExport;
            Properties.Settings.Default.Planes_WebFeed1 = feed.Name;
        }

        private void cb_WebFeed2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_WebFeed2.SelectedItem == null) || (cb_WebFeed2.GetItemText(cb_WebFeed2.SelectedItem) == "[none]"))
            {
                pg_WebFeed2.Enabled = false;
                btn_WebFeed2_Import.Enabled = false;
                btn_WebFeed2_Export.Enabled = false;
                bw_WebFeed2 = null;
                Properties.Settings.Default.Planes_WebFeed2 = "[none]";
                return;
            }
            // get the selected type of feed
            PlaneFeed feed = (PlaneFeed)cb_WebFeed2.SelectedItem;
            // check for changes
            if ((bw_WebFeed2 == null) || bw_WebFeed2.Name != feed.Name)
            {
                // stop background worker
                if (bw_WebFeed2 != null)
                {
                    bw_WebFeed2.CancelAsync();
                    while (bw_WebFeed2.IsBusy)
                        Application.DoEvents();
                }
                // select feed
                foreach (PlaneFeed newfeed in PlaneFeeds)
                {
                    if (feed.Name == newfeed.Name)
                        bw_WebFeed2 = newfeed;
                }
                PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
                args.AppDirectory = AppDirectory;
                args.AppDataDirectory = AppDataDirectory;
                args.LogDirectory = LogDirectory;
                args.TmpDirectory = TmpDirectory;
                args.DatabaseDirectory = DatabaseDirectory;
                args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
                args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
                args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
                args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
                args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
                args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
                bw_WebFeed2.RunWorkerAsync(args);
            }
            pg_WebFeed2.SelectedObject = bw_WebFeed2.GetFeedSettings();
            pg_WebFeed2.Enabled = feed.HasSettings;
            btn_WebFeed2_Import.Enabled = feed.CanImport;
            btn_WebFeed2_Export.Enabled = feed.CanExport;
            Properties.Settings.Default.Planes_WebFeed2 = feed.Name;
        }

        private void cb_WebFeed3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_WebFeed3.SelectedItem == null) || (cb_WebFeed3.GetItemText(cb_WebFeed3.SelectedItem) == "[none]"))
            {
                pg_WebFeed3.Enabled = false;
                btn_WebFeed3_Import.Enabled = false;
                btn_WebFeed3_Export.Enabled = false;
                bw_WebFeed3 = null;
                Properties.Settings.Default.Planes_WebFeed3 = "[none]";
                return;
            }
            // get the selected type of feed
            PlaneFeed feed = (PlaneFeed)cb_WebFeed3.SelectedItem;
            // check for changes
            if ((bw_WebFeed3 == null) || bw_WebFeed3.Name != feed.Name)
            {
                // stop background worker
                if (bw_WebFeed3 != null)
                {
                    bw_WebFeed3.CancelAsync();
                    while (bw_WebFeed3.IsBusy)
                        Application.DoEvents();
                }
                // select feed
                foreach (PlaneFeed newfeed in PlaneFeeds)
                {
                    if (feed.Name == newfeed.Name)
                        bw_WebFeed3 = newfeed;
                }
                PlaneFeedWorkEventArgs args = new PlaneFeedWorkEventArgs();
                args.AppDirectory = AppDirectory;
                args.AppDataDirectory = AppDataDirectory;
                args.LogDirectory = LogDirectory;
                args.TmpDirectory = TmpDirectory;
                args.DatabaseDirectory = DatabaseDirectory;
                args.MinLon = (double)Properties.Settings.Default.Planes_MinLon;
                args.MaxLon = (double)Properties.Settings.Default.Planes_MaxLon;
                args.MinLat = (double)Properties.Settings.Default.Planes_MinLat;
                args.MaxLat = (double)Properties.Settings.Default.Planes_MaxLat;
                args.MinAlt = (int)Properties.Settings.Default.Planes_MinAlt;
                args.MaxAlt = (int)Properties.Settings.Default.Planes_MaxAlt;
                bw_WebFeed3.RunWorkerAsync(args);
            }
            pg_WebFeed3.SelectedObject = bw_WebFeed3.GetFeedSettings();
            pg_WebFeed3.Enabled = feed.HasSettings;
            btn_WebFeed3_Import.Enabled = feed.CanImport;
            btn_WebFeed3_Export.Enabled = feed.CanExport;
            Properties.Settings.Default.Planes_WebFeed3 = feed.Name;
        }

        private void btn_WebFeed1_Import_Click(object sender, EventArgs e)
        {
            bw_WebFeed1.Import();
        }

        private void btn_WebFeed2_Import_Click(object sender, EventArgs e)
        {
            bw_WebFeed2.Import();
        }

        private void btn_WebFeed3_Import_Click(object sender, EventArgs e)
        {
            bw_WebFeed3.Import();
        }

        private void btn_WebFeed1_Export_Click(object sender, EventArgs e)
        {
            bw_WebFeed1.Export();
        }

        private void btn_WebFeed2_Export_Click(object sender, EventArgs e)
        {
            bw_WebFeed2.Export();
        }

        private void btn_WebFeed3_Export_Click(object sender, EventArgs e)
        {
            bw_WebFeed3.Export();
        }

        private void bw_WebFeed_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == (int)PROGRESS.STATUS)
            {
                if (e.UserState == null)
                    return;
                // NASTY: shorten the message text if too long
                string s = (string)e.UserState;
                int maxlen = 50;
                if (s.Length > maxlen)
                {
                    try
                    {
                        // cut the string intelligent
                        s = s.Remove(s.IndexOf("from"), s.IndexOf(",")-s.IndexOf("from"));
                    }
                    catch
                    {
                        // simple cut the string 
                        s = s.Substring(0, maxlen);
                    }
                }
                this.Say(s);
            }
            else if (e.ProgressPercentage == (int)PROGRESS.FINISHED)
            {

            }
        }

        private void tp_Database_Enter(object sender, EventArgs e)
        {
            // udpate database status
            tb_Database_Location.Text = AircraftDatabase_old.GetDBLocation();
            tb_Database_Filesize.Text = AircraftDatabase_old.GetDBSize().ToString("F0");
            tb_Database_PlanePositions_RowCount.Text = AircraftDatabase_old.GetAircraftPositionsCount().ToString();
        }

        private void cb_Server_Active_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cb_Server_Active.Checked)
            {
                if (!this.bw_Webserver.IsBusy)
                {
                    this.bw_Webserver.RunWorkerAsync();
                }
            }
            else
            {
                this.bw_Webserver.CancelAsync();
            }
        }



        #endregion

    }
}
