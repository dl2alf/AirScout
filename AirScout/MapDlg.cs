
//    AirScout Aircraft Scatter Prediction
//    Copyright (C) DL2ALF
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Zeptomoby.OrbitTools;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Configuration;
using WinTest;
using System.Diagnostics;
using AquaControls;
using NDde;
using NDde.Server;
using NDde.Client;
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Database;
using ScoutBase.Elevation;
using ScoutBase.Stations;
using ScoutBase.Propagation;
using ScoutBase.Maps;
using ScoutBase.CAT;
using SerializableGenerics;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLiteDatabase;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Data.SQLite;
using DeviceId;
using AirScout.Properties;
using AirScout.Core;
using AirScout.Aircrafts;
using AirScout.PlaneFeeds;
using AirScout.AircraftPositions;
using AirScout.Signals;
using AirScout.PlaneFeeds.Plugin.MEFContract;
using AirScout.CAT;
using System.Security.RightsManagement;
using System.Web;

namespace AirScout
{


    public partial class MapDlg : Form
    {

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
                // get Property
                string logdir = Properties.Settings.Default.Log_Directory;
                // replace Windows/Linux directory spearator chars
                logdir = logdir.Replace('\\', Path.DirectorySeparatorChar);
                logdir = logdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(logdir))
                    logdir = "Log";
                // replace variables, if any
                logdir = VC.ReplaceAllVars(logdir);
                // remove directory separator chars at begin and end
                logdir = logdir.TrimStart(Path.DirectorySeparatorChar);
                logdir = logdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!logdir.Contains(Path.VolumeSeparatorChar))
                    logdir = Path.Combine(AppDataDirectory, logdir);
                return logdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Tempfile Directory")]
        public string TmpDirectory
        {
            get
            {
                // get Property
                string tmpdir = Properties.Settings.Default.Tmp_Directory;
                // replace Windows/Linux directory spearator chars
                tmpdir = tmpdir.Replace('\\', Path.DirectorySeparatorChar);
                tmpdir = tmpdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(tmpdir))
                    tmpdir = "Tmp";
                // replace variables, if any
                tmpdir = VC.ReplaceAllVars(tmpdir);
                // remove directory separator chars at begin and end
                tmpdir = tmpdir.TrimStart(Path.DirectorySeparatorChar);
                tmpdir = tmpdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!tmpdir.Contains(Path.VolumeSeparatorChar))
                    tmpdir = Path.Combine(AppDataDirectory, tmpdir);
                return tmpdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Database Directory")]
        public string DatabaseDirectory
        {
            get
            {
                // get Property
                string databasedir = Properties.Settings.Default.Database_Directory;
                // replace Windows/Linux directory spearator chars
                databasedir = databasedir.Replace('\\', Path.DirectorySeparatorChar);
                databasedir = databasedir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(databasedir))
                    databasedir = "Database";
                // replace variables, if any
                databasedir = VC.ReplaceAllVars(databasedir);
                // remove directory separator chars at begin and end
                databasedir = databasedir.TrimStart(Path.DirectorySeparatorChar);
                databasedir = databasedir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!databasedir.Contains(Path.VolumeSeparatorChar))
                    databasedir = Path.Combine(AppDataDirectory, databasedir);
                return databasedir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Icon Directory")]
        public string IconDirectory
        {
            get
            {
                // get Property
                string icondir = Properties.Settings.Default.Icon_Directory;
                // replace Windows/Linux directory spearator chars
                icondir = icondir.Replace('\\', Path.DirectorySeparatorChar);
                icondir = icondir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(icondir))
                    icondir = "Icon";
                // replace variables, if any
                icondir = VC.ReplaceAllVars(icondir);
                // remove directory separator chars at begin and end
                icondir = icondir.TrimStart(Path.DirectorySeparatorChar);
                icondir = icondir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!icondir.Contains(Path.VolumeSeparatorChar))
                    icondir = Path.Combine(AppDataDirectory, icondir);
                return icondir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Elevation Directory")]
        public string ElevationDirectory
        {
            get
            {
                // get Property
                string elevationdir = Properties.Settings.Default.Elevation_Directory;
                // replace Windows/Linux directory spearator chars
                elevationdir = elevationdir.Replace('\\', Path.DirectorySeparatorChar);
                elevationdir = elevationdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(elevationdir))
                    elevationdir = "elevation";
                // replace variables, if any
                elevationdir = VC.ReplaceAllVars(elevationdir);
                // remove directory separator chars at begin and end
                elevationdir = elevationdir.TrimStart(Path.DirectorySeparatorChar);
                elevationdir = elevationdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!elevationdir.Contains(Path.VolumeSeparatorChar))
                    elevationdir = Path.Combine(AppDataDirectory, elevationdir);
                    return elevationdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Plugin Directory")]
        public string PluginDirectory
        {
            get
            {
                // get Property
                string plugindir = Properties.Settings.Default.Plugin_Directory;
                // replace Windows/Linux directory spearator chars
                plugindir = plugindir.Replace('\\', Path.DirectorySeparatorChar);
                plugindir = plugindir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(plugindir))
                    plugindir = "plugin";
                // replace variables, if any
                plugindir = VC.ReplaceAllVars(plugindir);
                // remove directory separator chars at begin and end
                plugindir = plugindir.TrimStart(Path.DirectorySeparatorChar);
                plugindir = plugindir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!plugindir.Contains(Path.VolumeSeparatorChar))
                    plugindir = Path.Combine(AppDataDirectory, plugindir);
                return plugindir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Rig Directory")]
        public string RigDirectory
        {
            get
            {
                // get Property
                string rigdir = Properties.Settings.Default.Rig_Directory;
                // replace Windows/Linux directory spearator chars
                rigdir = rigdir.Replace('\\', Path.DirectorySeparatorChar);
                rigdir = rigdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(rigdir))
                    rigdir = "rig";
                // replace variables, if any
                rigdir = VC.ReplaceAllVars(rigdir);
                // remove directory separator chars at begin and end
                rigdir = rigdir.TrimStart(Path.DirectorySeparatorChar);
                rigdir = rigdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!rigdir.Contains(Path.VolumeSeparatorChar))
                    rigdir = Path.Combine(AppDataDirectory, rigdir);
                return rigdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Plane Positions Directory")]
        public string PlanePositionsDirectory
        {
            get
            {
                // get Property
                string posdir = Properties.Settings.Default.Planes_PositionsDirectory;
                // replace Windows/Linux directory spearator chars
                posdir = posdir.Replace('\\', Path.DirectorySeparatorChar);
                posdir = posdir.Replace('/', Path.DirectorySeparatorChar);
                // set to default value if empty
                if (String.IsNullOrEmpty(posdir))
                    posdir = "PlanePositions";
                // replace variables, if any
                posdir = VC.ReplaceAllVars(posdir);
                // remove directory separator chars at begin and end
                posdir = posdir.TrimStart(Path.DirectorySeparatorChar);
                posdir = posdir.TrimEnd(Path.DirectorySeparatorChar);
                // fully qualify path
                if (!posdir.Contains(Path.VolumeSeparatorChar))
                    posdir = Path.Combine(AppDataDirectory, TmpDirectory, posdir);
                return posdir;
            }
        }

        [CategoryAttribute("Directories")]
        [DescriptionAttribute("Web Server Directory")]
        public string WebserverDirectory
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    // use source code directory when in debug mode
                    string dir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "wwwroot");
                    return dir;
                }
                else
                    return Path.Combine(Application.StartupPath, "wwwroot");
            }
        }

        [CategoryAttribute("General")]
        [DescriptionAttribute("Main Splitter Distance")]
        private int MainSplitterDistance
        {
            get
            {
                return sc_Main.SplitterDistance;
            }
            set
            {
                // check bounds
                if ((value > 0) && (value < this.Width))
                    sc_Main.SplitterDistance = value;
                else
                    sc_Main.SplitterDistance = this.Width - gb_Map_Info_DefaultWidth;
                Console.WriteLine("Setting MainSplitterDistance: " + this.Width + "--->" + sc_Main.SplitterDistance);
            }
        }

        [CategoryAttribute("General")]
        [DescriptionAttribute("Map Splitter Distance")]
        private int MapSplitterDistance
        {
            get
            {
                return sc_Map.SplitterDistance;
            }
            set
            {
                // check bounds
                if ((value > 0) && (value < this.Height))
                    sc_Map.SplitterDistance = value;
                else
                    sc_Map.SplitterDistance = this.Height - tc_Main_DefaultHeight;
                Console.WriteLine("Setting MapSplitterDistance: " + this.Height + "--->" + sc_Map.SplitterDistance);
            }
        }

        [ImportMany(AllowRecomposition = true)] // This is a signal to the MEF framework to load all matching exported assemblies.
        public List<Lazy<IPlaneFeedPlugin>> LazyPlaneFeedPlugins = new List<Lazy<IPlaneFeedPlugin>>();
        public List<IPlaneFeedPlugin> PlaneFeedPlugins = new List<IPlaneFeedPlugin>();

        // Default colors
        private readonly new Color DefaultBackColor = SystemColors.Control;
        private readonly new Color DefaultForeColor = Color.Black;

        // Log
        public static LogWriter Log;

        public VarConverter VC = new VarConverter();

        private int cntdn = 0;

        DateTime CurrentTime = DateTime.UtcNow;

        // FlightRadar
        PlaneInfoCache Planes = new PlaneInfoCache();
        SortedList<string, PlaneInfo> ActivePlanes = new SortedList<string, PlaneInfo>();
        private DateTime History_OldestEntry = DateTime.MinValue;
        private DateTime History_YoungestEntry = DateTime.MinValue;
        private List<string> SelectedPlanes = new List<string>();

        // Path
        public List<ElevationPathDesignator> ElevationPaths = new List<ElevationPathDesignator>();
        public List<PropagationPathDesignator> PropagationPaths = new List<PropagationPathDesignator>();

        // Map
        // Overlays
        GMapOverlay gmo_Routes = new GMapOverlay("Routes");
        GMapOverlay gmo_PropagationPaths = new GMapOverlay("PropagationPaths");
        GMapOverlay gmo_Objects = new GMapOverlay("Objects");
        GMapOverlay gmo_Planes = new GMapOverlay("Planes");
        GMapOverlay gmo_Airports = new GMapOverlay("Airports");
        GMapOverlay gmo_Callsigns = new GMapOverlay("Callsigns");
        GMapOverlay gmo_NearestPaths = new GMapOverlay("PropagationPaths");
        GMapOverlay gmo_NearestPlanes = new GMapOverlay("Planes");
        GMapOverlay gmo_Locators = new GMapOverlay("Locators");
        GMapOverlay gmo_Distances = new GMapOverlay("Distances");
        GMapOverlay gmo_CallsignDetails = new GMapOverlay("CallsignDetails");

        // Routes
        GMapRoute gmr_FullPath;
        GMapRoute gmr_VisiblePpath;
        GMapRoute gmr_NearestFull;
        GMapRoute gmr_NearestVisible;

        // Markers
        GMapMarker gmm_MyLoc;
        GMapMarker gmm_DXLoc;
        GMapMarker gmm_CurrentMarker;

        // Bitmap indices
        private int bmindex_darkorange;
        private int bmindex_lightgreen;
        private int bmindex_red;
        private int bmindex_gray;
        private int bmindex_magenta;

        // Tooltip font
        public Font ToolTipFont;

        // dragging
        bool isDraggingMarker = false;
        bool isDraggingMap = false;

        // Default width of right info box (get on startup as set in Designer)
        int gb_Map_Info_DefaultWidth = 0;

        // Default height of bottom tab control (get on startup as set in Designer)
        int tc_Main_DefaultHeight = 0;

        int gb_Map_Info_MaximizedHeight = 0;
        int gb_Map_Zoom_MaximizedHeight = 0;
        int gb_Map_Filter_MaximizedHeigth = 0;
        int gb_Map_Alarms_MaximizedHeight = 0;
        int gb_Map_Info_MinimizedHeight = 0;
        int gb_Map_Zoom_MinimizedHeight = 0;
        int gb_Map_Filter_MinimizedHeigth = 0;
        int gb_Map_Alarms_MinimizedHeight = 0;

        // Plane feeds
        public PlaneFeed bw_PlaneFeed1;
        public PlaneFeed bw_PlaneFeed2;
        public PlaneFeed bw_PlaneFeed3;

        // Startup
        private bool FirstRun = true;
        private bool CleanRun = false;

        private Splash SplashDlg;

        // Background workers
        public ElevationDatabaseUpdater bw_GLOBEUpdater = new ElevationDatabaseUpdater();
        public ElevationDatabaseUpdater bw_SRTM3Updater = new ElevationDatabaseUpdater();
        public ElevationDatabaseUpdater bw_SRTM1Updater = new ElevationDatabaseUpdater();
        public ElevationDatabaseUpdater bw_ASTER3Updater = new ElevationDatabaseUpdater();
        public ElevationDatabaseUpdater bw_ASTER1Updater = new ElevationDatabaseUpdater();

        public StationDatabaseUpdater bw_StationDatabaseUpdater = new StationDatabaseUpdater();
        public AircraftDatabaseUpdater bw_AircraftDatabaseUpdater = new AircraftDatabaseUpdater();

        public AircraftPositionDatabaseMaintainer bw_AircraftDatabaseMaintainer = new AircraftPositionDatabaseMaintainer();

        public PathCalculator bw_GLOBEPathCalculator = new PathCalculator(ELEVATIONMODEL.GLOBE);
        public PathCalculator bw_SRTM3PathCalculator = new PathCalculator(ELEVATIONMODEL.SRTM3);
        public PathCalculator bw_SRTM1PathCalculator = new PathCalculator(ELEVATIONMODEL.SRTM1);
        public PathCalculator bw_ASTER3PathCalculator = new PathCalculator(ELEVATIONMODEL.ASTER3);
        public PathCalculator bw_ASTER1PathCalculator = new PathCalculator(ELEVATIONMODEL.ASTER1);

        public CATUpdater bw_CATUpdater = new CATUpdater();
        public CATWorker bw_CAT = new CATWorker();

        public MapPreloader bw_MapPreloader = new MapPreloader();

        // Operating modes
        AIRSCOUTPATHMODE PathMode = AIRSCOUTPATHMODE.NONE;
        AIRSCOUTLIFEMODE LifeMode = AIRSCOUTLIFEMODE.NONE;
        AIRSCOUTPLAYMODE PlayMode = AIRSCOUTPLAYMODE.NONE;

        private int Time_Offline_Interval = 0;

        // Charting

        // path chart
        PlotModel pm_Path = new PlotModel();
        PlotView pv_Path = new PlotView();
        LinearAxis Path_X = new LinearAxis();
        LinearAxis Path_Y = new LinearAxis();
        AreaSeries Path_Elevation = new AreaSeries();
        LineSeries Min_H1 = new LineSeries();
        LineSeries Min_H2 = new LineSeries();
        LineSeries Max_H = new LineSeries();
        AreaSeries Min_H = new AreaSeries();
        LineSeries Planes_Hi = new LineSeries();
        LineSeries Planes_Lo = new LineSeries();

        // elevation chart
        PlotModel pm_Elevation = new PlotModel();
        PlotView pv_Elevation = new PlotView();
        LinearAxis Elevation_X = new LinearAxis();
        LinearAxis Elevation_Y = new LinearAxis();
        AreaSeries Elevation = new AreaSeries();
        LineSeries LOS = new LineSeries();

        // spectrum chart
        PlotModel pm_Spectrum = new PlotModel();
        PlotView pv_Spectrum = new PlotView();
        LinearAxis Spectrum_X = new LinearAxis();
        LinearAxis Spectrum_Y = new LinearAxis();
        LineSeries Spectrum = new LineSeries();
        AreaSeries SpectrumRecord = new AreaSeries();
        int SpectrumPointsCount = 0;
        int SpectrumMaxPoints = 600;

        // Webbrowser
        private System.Windows.Forms.WebBrowser wb_News = null;

        CultureInfo LocalCulture;

        // Watchlist control
        private bool WatchlistUpdating = false;
        private bool WatchlistAllCheckedChanging = false;
        bool WatchlistAllChecked = false;
        CheckBoxState WatchlistAllCheckedState = CheckBoxState.UncheckedNormal;
        System.Drawing.Point WatchlistOldMousePos = new System.Drawing.Point(0, 0);

        // Analysis
        private List<DateTime> AllLastUpdated = new List<DateTime>();
        private List<AircraftPositionDesignator> AllPositions = new List<AircraftPositionDesignator>();
        private long AircraftPositionsCount = 0;

        // Nearest plane
        private PlaneInfo NearestPlane = null;

        // Airports
        private List<AirportDesignator> Airports = new List<AirportDesignator>();

        // Session key
        public string SessionKey = "";

        // CAT
        public IRig ConnectedRig = null;
        public RIGSTATUS RigStatus = RIGSTATUS.NONE;


        // Tracking & rotator control
        AIRSCOUTTRACKMODE TrackMode = AIRSCOUTTRACKMODE.NONE;
        TRACKSTATUS TrackStatus = TRACKSTATUS.NONE;
        ROTSTATUS RotStatus = ROTSTATUS.NONE;
        TrackValues TrackValues = null;

        // Gauges display
        public Color GaugesColor = Color.Black;
        public Font TrackDisplayFont = new Font("Courier New", 16, FontStyle.Bold);

        // Location grid updating
        public bool UpdateLocationGrid = false;

        #region Startup & Initialization

        public MapDlg()
        {
            // save current local LocalCulture
            LocalCulture = Application.CurrentCulture;

            // force culture invariant language for GUI
            Application.CurrentCulture = CultureInfo.InvariantCulture;

            // set security protocol to TLS1.2 globally
            // this will work even on .NET 4.0 for most operating systems > Windows XP
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

            InitializeComponent();

            // Running on Windows or Linux/Mono?
            if (SupportFunctions.IsMono)
            {
                Console.WriteLine("Checking system: Running on Linux/Mono.");
            }
            else
            {
                Console.WriteLine("Checking system: Running on Windows.");
            }
            // do basic initialization
            this.Text = "AirScout - Aircraft Scatter Prediction V" + Application.ProductVersion + " (c) 2013-2020 DL2ALF";

            // create a new renderer wich is clipping the status text on overflow
            ss_Main.Renderer = new ClippingToolStripRenderer();

            // initialize settings
            InitializeSettings();

            // initialize charting
            InitializeCharts();

            // Initilialize Webbrowser
            InitializeWebbrowser();

            // check for command line args
            string[] args = Environment.GetCommandLineArgs();
            if ((args != null) && (args.Count() > 1))
            {
                // try to clean installation & database
                if ((args[1].ToUpper() == "/CLEAN") ||
                    (args[1].ToUpper() == "-CLEAN"))
                {
                    CleanupDlg Dlg = new CleanupDlg(this);
                    DialogResult result = Dlg.ShowDialog();
                    // exit immediately if user closes the form without pressing a button
                    if (result == DialogResult.Abort)
                        System.Environment.Exit(-1);
                    if (result == DialogResult.OK)
                    {
                        // re-initialize settings as they can be lost during clean-up
                        InitializeSettings();
                        // set a clean run flag
                        CleanRun = true;
                    }
                }
            }
            // set elevation database update event handler
            bw_GLOBEUpdater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);
            bw_SRTM3Updater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);
            bw_SRTM1Updater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);
            bw_ASTER3Updater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);
            bw_ASTER1Updater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);

            // set station database updater event handler
            bw_StationDatabaseUpdater.ProgressChanged += new ProgressChangedEventHandler(bw_StationDatabaseUpdater_ProgressChanged);
            bw_StationDatabaseUpdater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StationDatabaseUpdater_RunWorkerCompleted);

            // set aircraft database updater event handler
            bw_AircraftDatabaseUpdater.ProgressChanged += new ProgressChangedEventHandler(bw_AircraftDatabaseUpdater_ProgressChanged);
            bw_AircraftDatabaseUpdater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_AircraftDatabaseUpdater_RunWorkerCompleted);

            // set aircraft database maintainer event handler
            bw_AircraftDatabaseMaintainer.ProgressChanged += new ProgressChangedEventHandler(bw_AircraftDatabaseMaintainer_ProgressChanged);
            bw_AircraftDatabaseMaintainer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_AircraftDatabaseMaintainer_RunWorkerCompleted);

            // set elevation path calculator event handler
            bw_GLOBEPathCalculator.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationPathCalculator_ProgressChanged);
            bw_SRTM3PathCalculator.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationPathCalculator_ProgressChanged);
            bw_SRTM1PathCalculator.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationPathCalculator_ProgressChanged);
            bw_ASTER3PathCalculator.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationPathCalculator_ProgressChanged);
            bw_ASTER1PathCalculator.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationPathCalculator_ProgressChanged);

            // set map preloader event handler
            bw_MapPreloader.ProgressChanged += new ProgressChangedEventHandler(bw_MapPreloader_ProgressChanged);

            // CAT updater event handler
            bw_CATUpdater.ProgressChanged += bw_CATUpdater_ProgressChanged;

            // CAT interface event handler
            bw_CAT.ProgressChanged += new ProgressChangedEventHandler(bw_CAT_ProgressChanged);

            // save FirstRun property before trying to upgrade user settings
            FirstRun = Properties.Settings.Default.FirstRun;
        }

        string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            // Create a RijndaelManaged object
            // with the specified key and IV.
            aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                        srDecrypt.Close();
                    }
                }
            }

            return plaintext;
        }

        private string OpenSSLDecrypt(string encrypted, string passphrase)
        {
            //get the key bytes (not sure if UTF8 or ASCII should be used here doesn't matter if no extended chars in passphrase)
            var key = Encoding.UTF8.GetBytes(passphrase);

            //pad key out to 32 bytes (256bits) if its too short
            if (key.Length < 32)
            {
                var paddedkey = new byte[32];
                Buffer.BlockCopy(key, 0, paddedkey, 0, key.Length);
                key = paddedkey;
            }

            //setup an empty iv
            var iv = new byte[16];

            //get the encrypted data and decrypt
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
            return DecryptStringFromBytesAes(encryptedBytes, key, iv);
        }

        public void InitializeSession()
        {
            // register this AirScout instance
            try
            {
                WebClient client;
                string result;
                Log.WriteMessage("Registering AirScout: Creating Instance ID");
                // check AirScout instance id and generate new if not set
                if (String.IsNullOrEmpty(Properties.Settings.Default.AirScout_Instance_ID))
                {
                    Properties.Settings.Default.AirScout_Instance_ID = Guid.NewGuid().ToString();
                }
                Log.WriteMessage("Registering AirScout: Creating Device ID");
                // create an unique device id
                DeviceIdBuilder devid = new DeviceIdBuilder();
                Log.WriteMessage("Registering AirScout: Creating Device ID [MachineName]:" + devid.AddMachineName().ToString());
                // not supported on Mono
                if (!SupportFunctions.IsMono)
                {
                    Log.WriteMessage("Registering AirScout: Creating Device ID [MACAddress]:" + devid.AddMacAddress().ToString());
                    Log.WriteMessage("Registering AirScout: Creating Device ID [Processor]:" + devid.AddProcessorId().ToString());
                    Log.WriteMessage("Registering AirScout: Creating Device ID [Motherboard]:" + devid.AddMotherboardSerialNumber().ToString());
                }
                // store in settings if not set so far
                if (String.IsNullOrEmpty(Properties.Settings.Default.AirScout_Device_ID) || (devid.ToString() != Properties.Settings.Default.AirScout_Device_ID))
                {
                    // not set or not the same, assuming AirScout is running on a new machine --> create a new instance id
                    Properties.Settings.Default.AirScout_Instance_ID = Guid.NewGuid().ToString();
                }
                // store device id in settings
                Properties.Settings.Default.AirScout_Device_ID = devid.ToString();
                Log.WriteMessage("Registering AirScout: Getting Session Key");
                // get new session key
                client = new WebClient();
                string id = Properties.Settings.Default.AirScout_Instance_ID;
                result = client.DownloadString(Properties.Settings.Default.AirScout_Register_URL +
                    "?id=" + Properties.Settings.Default.AirScout_Instance_ID +
                    "&mycall=" + Properties.Settings.Default.MyCall +
                    "&mylat=" + Properties.Settings.Default.MyLat.ToString() +
                    "&mylon=" + Properties.Settings.Default.MyLon.ToString() +
                    "&myversion=" + Application.ProductVersion);
                if (!result.ToLower().Contains("error"))
                    {
                    SessionKey = Encryption.OpenSSLDecrypt(result, Properties.Settings.Default.AirScout_Instance_ID);
                }
                else
                {
                    Log.WriteMessage("Error while registering AirScout: " + result);
                    MessageBox.Show(result + "\n\nSome functionality might be limited.", "Error while registering AirScout");
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage("Error while registering AirScout: " + ex.ToString());
                MessageBox.Show(ex.Message + "\n\nSome functionality might be limited.", "Error while registering AirScout");
            }
            Log.WriteMessage("Registering AirScout successful.");
        }

        public void CheckDirectories()
        {
            // check if directories exist
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);
            if (!Directory.Exists(IconDirectory))
                Directory.CreateDirectory(IconDirectory);
            if (!Directory.Exists(DatabaseDirectory))
                Directory.CreateDirectory(DatabaseDirectory);
            if (!Directory.Exists(ElevationDirectory))
                Directory.CreateDirectory(ElevationDirectory);
            if (!Directory.Exists(PluginDirectory))
                Directory.CreateDirectory(PluginDirectory);
            if (!Directory.Exists(RigDirectory))
                Directory.CreateDirectory(RigDirectory);
            if (!Directory.Exists(PlanePositionsDirectory))
                Directory.CreateDirectory(PlanePositionsDirectory);

            // cleanup plane positions directory
            string[] files = Directory.GetFiles(PlanePositionsDirectory, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // do nothing
                }
            }
        }

        private void CopyPluginWithPDB(string src, string dst)
        {
            try
            {
                // copy plugin itself
                if (File.Exists(src))
                    File.Copy(src, dst, true);
                // copy symbol file for debug, if any
                src = src.Replace(".dll", ".pdb");
                dst = dst.Replace(".dll", ".pdb");
                if (File.Exists(src))
                    File.Copy(src, dst, true);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        public void CopyPlugins(string srcdir, string dstdir, string filespec)
        {
            // move the planefeed plugins from program directory to the plugins directory but only if they not exist
            // otherwise do version check
            string[] srcplugins = Directory.GetFiles(srcdir, filespec);
            foreach (string srcplugin in srcplugins)
            {
                try
                {
                    string filename = Path.GetFileName(srcplugin);
                    string srcversion = FileVersionInfo.GetVersionInfo(srcplugin).FileVersion;
                    DateTime srctime = File.GetLastWriteTimeUtc(srcplugin);
                    // calculate destination file name
                    string dstplugin = dstdir + Path.DirectorySeparatorChar + filename;
                    if (!File.Exists(dstplugin))
                    {
                        // copy file if not exists
                        CopyPluginWithPDB(srcplugin, dstplugin);
                        Log.WriteMessage("Plugin Manager: Copied plugin " + filename + "[" + srcversion + "].");
                    }
                    else
                    {
                        // do version check
                        string dstversion = FileVersionInfo.GetVersionInfo(dstplugin).FileVersion;
                        DateTime dsttime = File.GetLastWriteTimeUtc(dstplugin);
                        if (srcversion.CompareTo(dstversion) > 0)
                        {
                            // overwrite dstfile if newer version
                            CopyPluginWithPDB(srcplugin, dstplugin);
                            Log.WriteMessage("Plugin Manager: Replaced plugin " + filename + "[" + dstversion + "] with newer version [" + srcversion + "].");
                        }
                        else if (srcversion.CompareTo(dstversion) == 0)
                        {
                            if (srctime > dsttime)
                            {
                                // overwrite dstfile if same version but newer 
                                CopyPluginWithPDB(srcplugin, dstplugin);
                                Log.WriteMessage("Plugin Manager: Replaced plugin " + filename + "[" + dsttime.ToString("yyyy-MM-dd HH:mm:ss") + "] with newer file [" + srctime.ToString("yyyy-MM-dd HH:mm:ss") + "].");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
        }

        private void LoadPlugins()
        {
            // chek for planefeed plugins
            Log.WriteMessage("Loading plugins...");
            // get major&minor version
            string mainversion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[0] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[1];
            string filespec = "AirScout.PlaneFeeds.Plugin.*.dll";

            // first copy plugins from application directory to plugin directory if they not exist or newer
            CopyPlugins(Path.Combine(AppDirectory,"Plugin"), PluginDirectory, filespec);

            // check for new plugins on the web resource
            try
            {
                // clear temporary files
                try
                {
                    SupportFunctions.DeleteFilesFromDirectory(TmpDirectory, new string[] { "*.tmp", "*.PendingOverwrite" });
                    SupportFunctions.DeleteFilesFromDirectory(TmpDirectory, filespec);
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
                // calculate url
                string url = Properties.Settings.Default.Plugins_Update_URL + "/" + mainversion + "/" + "planefeeds.zip";
                string filename = Path.Combine(TmpDirectory, "planefeeds.zip");
                // get zip file
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.NOTFOUND) > 0))
                {
                    Log.WriteMessage("PluginManager: checking plugins --> nothing found on web or error while downloading.");
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    // update files with web resource if newer
                    // copy plugins from application directory to plugin directory
                    CopyPlugins(TmpDirectory, PluginDirectory, filespec);
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }

            // load available plugins, we need a robust algorithm here!
            // error while composing a single plugin or error during call to plugin constructor must not affect loading of other plugins!
            // furthermore: DirectoryCatalog does not work on Linux when plugins are placed in other directory than program's main directory
            // solution -->
            // try to compose each single plugin first and collect "clean" plugins in a container for error-free composing with Lazy<T>
            // after that, try to call each plugin's constructor and collect "clean" plugins in a separate list for further use
            try
            {
                List<AssemblyCatalog> catalog = new List<AssemblyCatalog>();
                CompositionContainer tmpcontainer = null;
                foreach (string plfile in Directory.GetFiles(PluginDirectory, filespec, SearchOption.AllDirectories))
                {
                    try
                    {
                        // try to import a single plugin 
                        var tmpcatalog = new AssemblyCatalog(System.Reflection.Assembly.UnsafeLoadFrom(plfile));
                        tmpcontainer = new CompositionContainer(tmpcatalog);
                        tmpcontainer.ComposeParts(this);
                        // add to main catalog only when composition was OK
                        tmpcontainer.Dispose();
                        catalog.Add(tmpcatalog);
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(ReflectionTypeLoadException))
                        {
                            foreach (Exception le in ((ReflectionTypeLoadException)ex).LoaderExceptions)
                            {
                                Log.WriteMessage("Error while loading plugin[" + plfile + "]: " + le.ToString(), LogLevel.Error);
                            }
                        }
                        else
                        {
                            Log.WriteMessage("Error while loading plugin[" + plfile + "]: " + ex.ToString(), LogLevel.Error);
                        }
                    }
                }
                // catalog should cointain only "clean" plugins so composing all parts should be possible without errors
                var container = new CompositionContainer(new AggregateCatalog(catalog));
                container.ComposeParts(this);
                foreach (Lazy<IPlaneFeedPlugin> lazyplugin in LazyPlaneFeedPlugins)
                {
                    try
                    {
                        // try to call plaugin constructor and add the value to Planefeeds list
                        // 2022_04_24 check plugin version against program version: do not load older plugins
                        var plugin = lazyplugin.Value;
                        if (plugin != null)
                        {
                            if (plugin.Version.StartsWith(mainversion))
                            {
                                PlaneFeedPlugins.Add(plugin);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage("Error while loading plugin: " + ex.ToString(), LogLevel.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            Log.WriteMessage("Loading plugins successful.");
        }

        /// <summary>
        /// Returns the default value of a property
        /// </summary>
        /// <param name="propertyname">The property name.</param>
        /// <returns>The property value.</returns>
        public static dynamic GetPropertyDefaultValue(string propertyname)
        {
            SettingsProperty prop = Properties.Settings.Default.Properties[propertyname];
            if (prop == null)
                return null;

            object def = prop.DefaultValue; 
            if (def == null)
                return null;

            Type t = Properties.Settings.Default.Properties[propertyname].PropertyType;
            if (t == null)
                return null;

            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(t);
                object value = tc.ConvertFromString(null, CultureInfo.InvariantCulture, (string)def);
                
                return value;
            }
            catch (Exception ex)
            {

            }

            return null;
            /*
            string p = prop.DefaultValue.ToString();
            Type t = Properties.Settings.Default.Properties[propertyname].PropertyType;
            return Convert.ChangeType(p, t);
            */

        }

        private string QualifyDatabaseDirectory(string databasedir)
        {
            // replace Windows/Linux directory spearator chars
            databasedir = databasedir.Replace('\\', Path.DirectorySeparatorChar);
            databasedir = databasedir.Replace('/', Path.DirectorySeparatorChar);
            // replace variables, if any
            databasedir = VC.ReplaceAllVars(databasedir);
            // remove directory separator chars at begin and end
            databasedir = databasedir.TrimStart(Path.DirectorySeparatorChar);
            databasedir = databasedir.TrimEnd(Path.DirectorySeparatorChar);
            // fully qualify path
            if (!databasedir.Contains(Path.VolumeSeparatorChar))
                databasedir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databasedir);
            return databasedir;
        }

        private void InitializeSettings()
        {

            // Load user settings
            LoadUserSettings();

            // check for invalid settings
            // check if band is BNONE --> set to 1.2G
            if (Properties.Settings.Default.Band == BAND.BNONE)
                Properties.Settings.Default.Band = BAND.B1_2G;
            // chekc if band settings are NULL --> set to default
            if (Properties.Settings.Default.Path_Band_Settings == null)
                Properties.Settings.Default.Path_Band_Settings = new BandSettings(true);
            // check if watchlist in null --> create new
            if (Properties.Settings.Default.Watchlist == null)
                Properties.Settings.Default.Watchlist = new Watchlist();
            string location = StationData.Database.GetDBLocation();
            // check calls
            if (String.IsNullOrEmpty(Properties.Settings.Default.MyCall))
                Properties.Settings.Default.MyCall = GetPropertyDefaultValue(nameof(Properties.Settings.Default.MyCall));
            if (String.IsNullOrEmpty(Properties.Settings.Default.DXCall))
                Properties.Settings.Default.DXCall = GetPropertyDefaultValue(nameof(Properties.Settings.Default.DXCall));
            // check lat/lon
            if (!GeographicalPoint.Check(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon))
            {
                Properties.Settings.Default.MyLat = GetPropertyDefaultValue(nameof(Properties.Settings.Default.MyLat));
                Properties.Settings.Default.MyLon = GetPropertyDefaultValue(nameof(Properties.Settings.Default.MyLon));
                UpdateLocation(Properties.Settings.Default.MyCall, Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, GEOSOURCE.FROMUSER);
            }
            if (!GeographicalPoint.Check(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon))
            {
                Properties.Settings.Default.DXLat = GetPropertyDefaultValue(nameof(Properties.Settings.Default.DXLat));
                Properties.Settings.Default.DXLon = GetPropertyDefaultValue(nameof(Properties.Settings.Default.DXLon));
                UpdateLocation(Properties.Settings.Default.DXCall, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, GEOSOURCE.FROMUSER);
            }
            // set current elevation model
            SetElevationModel();
            // set antenna height to 10m be default
            Properties.Settings.Default.MyHeight = 10;
            Properties.Settings.Default.DXHeight = 10;

            // check database directory settings
            Properties.Settings.Default.AircraftDatabase_Directory = QualifyDatabaseDirectory(Properties.Settings.Default.AircraftDatabase_Directory);
            Properties.Settings.Default.StationsDatabase_Directory = QualifyDatabaseDirectory(Properties.Settings.Default.StationsDatabase_Directory);
            Properties.Settings.Default.ElevationDatabase_Directory = QualifyDatabaseDirectory(Properties.Settings.Default.StationsDatabase_Directory);
            Properties.Settings.Default.PropagationDatabase_Directory = QualifyDatabaseDirectory(Properties.Settings.Default.PropagationDatabase_Directory);
        }

        private void DumpSettingsToLog(string name, SettingsPropertyValueCollection settings)
        {
            foreach (SettingsPropertyValue p in settings)
            {
                try
                {
                    if ((p != null) && (p.Name != null))
                    {
                        string value = p.PropertyValue != null ? p.PropertyValue.ToString() : "[null]";
                        string default_value = GetPropertyDefaultValue(p.Name) != null ? "[Default = " + GetPropertyDefaultValue(p.Name) + "]" : "[Default = null]";
                        Log.WriteMessage(name + "." + p.Name + " = " + value + default_value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while checking property + " + p.Name + ": " + ex.ToString());
                }
            }

        }

        private void CheckSettings()
        {
            Log.WriteMessage("Checking properties...");
            // check for empty MyCalls list
            if (Properties.Settings.Default.MyCalls == null)
            {
                Properties.Settings.Default.MyCalls = new List<string>();
                Properties.Settings.Default.MyCalls.Add("DL2ALF");
            }
            if (Properties.Settings.Default.MyCalls.Count == 0)
                Properties.Settings.Default.MyCalls.Add("DL2ALF");
            // checking window size & location
            Rectangle bounds = Screen.FromControl(this).Bounds;
            if ((Properties.Settings.Default.General_WindowLocation.X < bounds.Left) ||
                (Properties.Settings.Default.General_WindowLocation.Y < bounds.Top) ||
                (Properties.Settings.Default.General_WindowLocation.X > bounds.Right) ||
                (Properties.Settings.Default.General_WindowLocation.Y > bounds.Bottom))
            { 
                Properties.Settings.Default.General_WindowLocation = new System.Drawing.Point(bounds.Left, bounds.Top);
            }
            if ((Properties.Settings.Default.General_WindowSize.Width > bounds.Width) ||
                (Properties.Settings.Default.General_WindowSize.Height > bounds.Height))
            {
                Properties.Settings.Default.General_WindowSize = new System.Drawing.Size(bounds.Width, bounds.Height);
            }
            // list all properties in log
            foreach (SettingsPropertyValue p in Properties.Settings.Default.PropertyValues)
            {
                if ((p != null) && (p.Name != null))
                {
                    string value = p.PropertyValue != null ? p.PropertyValue.ToString() : "[null]";
                    Log.WriteMessage("Checking property " + p.Name + " = " + value);
                }
            }
            /*
            // check URLs for trailing separator
            if (!Properties.Settings.Default.Elevation_GLOBE_URL.EndsWith("/"))
                Properties.Settings.Default.Elevation_GLOBE_URL = Properties.Settings.Default.Elevation_GLOBE_URL + "/";
            if (!Properties.Settings.Default.Elevation_SRTM3_URL.EndsWith("/"))
                Properties.Settings.Default.Elevation_SRTM3_URL = Properties.Settings.Default.Elevation_SRTM3_URL + "/";
            if (!Properties.Settings.Default.Elevation_SRTM1_URL.EndsWith("/"))
                Properties.Settings.Default.Elevation_SRTM1_URL = Properties.Settings.Default.Elevation_SRTM1_URL + "/";
            if (!Properties.Settings.Default.StationDatabase_Update_URL.EndsWith("/"))
                Properties.Settings.Default.StationDatabase_Update_URL = Properties.Settings.Default.StationDatabase_Update_URL + "/";
            */
            // check for last saved stations not in database and revert to at least DL2ALF and GB3MHZ if necessary
            // first check for saved stations
            string mycall = Properties.Settings.Default.MyCall;
            double mylat = Properties.Settings.Default.MyLat;
            double mylon = Properties.Settings.Default.MyLon;
            string myloc = MaidenheadLocator.LocFromLatLon(mylat, mylon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, 3);
            LocationDesignator ld = StationData.Database.LocationFind(mycall, myloc);
            if (ld == null)
            {
                mycall = GetPropertyDefaultValue(nameof(Properties.Settings.Default.MyCall));
                mylat = GetPropertyDefaultValue(nameof(Properties.Settings.Default.MyLat));
                mylon = GetPropertyDefaultValue(nameof(Properties.Settings.Default.MyLon));
                myloc = MaidenheadLocator.LocFromLatLon(mylat, mylon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, 3);
                UpdateLocation(mycall, mylat, mylon, GEOSOURCE.FROMUSER);
                Properties.Settings.Default.MyCall = mycall;
                Properties.Settings.Default.MyLat = mylat;
                Properties.Settings.Default.MyLon = mylon;
            }
            string dxcall = Properties.Settings.Default.DXCall;
            double dxlat = Properties.Settings.Default.DXLat;
            double dxlon = Properties.Settings.Default.DXLon;
            string dxloc = MaidenheadLocator.LocFromLatLon(dxlat, dxlon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, 3);
            ld = StationData.Database.LocationFind(dxcall, dxloc);
            if (ld == null)
            {
                dxcall = GetPropertyDefaultValue(nameof(Properties.Settings.Default.DXCall));
                dxlat = GetPropertyDefaultValue(nameof(Properties.Settings.Default.DXLat));
                dxlon = GetPropertyDefaultValue(nameof(Properties.Settings.Default.DXLon));
                dxloc = MaidenheadLocator.LocFromLatLon(dxlat, dxlon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, 3);
                UpdateLocation(dxcall, dxlat, dxlon, GEOSOURCE.FROMUSER);
                Properties.Settings.Default.DXCall = dxcall;
                Properties.Settings.Default.DXLat = dxlat;
                Properties.Settings.Default.DXLon = dxlon;
            }
            Log.WriteMessage("Checking properties finished...");
        }

        private void CheckInternet()
        {
            // check internet connectivity
            try
            {
                using (var client = new WebClient())
                {
                    string url = Properties.Settings.Default.Connectivity_URL;
                    using (var stream = client.OpenRead(url))
                    {
                    }
                }
            }
            catch
            {
                MessageBox.Show("Could not find an internet connection.\nThis is required on first run.\nAirScout will close.", "Internet connectivity");
                this.Close();
            }
        }

        private void InitializeLogfile()
        {
            // set directories and formats for logfile
            ScoutBase.Core.Properties.Settings.Default.LogWriter_Directory = LogDirectory;
            ScoutBase.Core.Properties.Settings.Default.LogWriter_FileFormat = "AirScout_{0:yyyy_MM_dd}.log";
            ScoutBase.Core.Properties.Settings.Default.LogWriter_MessageFormat = "{0:u} {1}";
            // gets an instance of a LogWriter
            Log = LogWriter.Instance;
            Log.WriteMessage("-------------------------------------------------------------------------------------");
            Log.WriteMessage(Application.ProductName + " is starting up.", 0, false);
            Log.WriteMessage("-------------------------------------------------------------------------------------");
            Log.WriteMessage("Operating system          : " + Environment.OSVersion.Platform);
            Log.WriteMessage("OS Major version          : " + Environment.OSVersion.Version.Major);
            Log.WriteMessage("OS Minor version          : " + Environment.OSVersion.Version.Minor);
            Log.WriteMessage("Application directory     : " + AppDirectory);
            Log.WriteMessage("Application data directory: " + AppDataDirectory);
            Log.WriteMessage("Log directory             : " + LogDirectory);
            Log.WriteMessage("Temp directory            : " + TmpDirectory);
            Log.WriteMessage("Database directory        : " + DatabaseDirectory);
            Log.WriteMessage("Elevation directory       : " + ElevationDirectory);
            Log.WriteMessage("Plugin directory          : " + PluginDirectory);
            Log.WriteMessage("Rig directory             : " + RigDirectory);
            Log.WriteMessage("Application settings file : " + AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Log.WriteMessage("User settings file        : " + GetUserSettingsPath());
            Log.WriteMessage("Disk space available      : " + SupportFunctions.GetDriveAvailableFreeSpace(Path.GetPathRoot(AppDataDirectory)) / 1024 / 1024 + " MB");
            Log.WriteMessage("-------------------------------------------------------------------------------------");
        }

        private void InitializeDatabase()
        {
            Log.WriteMessage("Started.");
            SetElevationModel();
            // reset database status
            Properties.Settings.Default.Elevation_GLOBE_DatabaseStatus = DATABASESTATUS.UNDEFINED;
            Properties.Settings.Default.Elevation_SRTM3_DatabaseStatus = DATABASESTATUS.UNDEFINED;
            Properties.Settings.Default.Elevation_SRTM1_DatabaseStatus = DATABASESTATUS.UNDEFINED;
            Properties.Settings.Default.Elevation_ASTER3_DatabaseStatus = DATABASESTATUS.UNDEFINED;
            Properties.Settings.Default.Elevation_ASTER1_DatabaseStatus = DATABASESTATUS.UNDEFINED;
            // set nearfield suppression
            PropagationData.Database.NearFieldSuppression = Properties.Settings.Default.Path_NearFieldSuppression;
            // get all database directories and store it in settings
            Properties.Settings.Default.AircraftDatabase_Directory = AircraftData.Database.DefaultDatabaseDirectory();
            Properties.Settings.Default.StationsDatabase_Directory = StationData.Database.DefaultDatabaseDirectory();
            Properties.Settings.Default.ElevationDatabase_Directory = ElevationData.Database.DefaultDatabaseDirectory();
            Properties.Settings.Default.PropagationDatabase_Directory = PropagationData.Database.DefaultDatabaseDirectory();
            Properties.Settings.Default.Rig_Directory = RigData.Database.DefaultDatabaseDirectory();
            MapData.Database.DefaultDatabaseDirectory();
            Log.WriteMessage("Finished.");
        }

        private void CreateDistances()
        {
            gmo_Distances.Clear();
            for (int dist = 100; dist <= 1000; dist += 100)
            {
                GMapRoute circle = new GMapRoute("Distance: " + dist.ToString());
                circle.Stroke = new Pen(Color.DarkGray, 1);
                for (int i = 0; i <= 360; i++)
                {
                    LatLon.GPoint p = LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, dist);
                    circle.Points.Add(new PointLatLng(p.Lat, p.Lon));

                }
                gmo_Distances.Routes.Add(circle);
            }

        }

        private void InitializeMaps()
        {
            // setting User Agent to fix Open Street Map issue 2016-09-20
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_Main.MapProvider.RefererUrl = "";
            // set initial settings for main map
            gm_Main.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Main.IgnoreMarkerOnMouseWheel = true;
            gm_Main.MinZoom = 0;
            gm_Main.MaxZoom = 20;
            gm_Main.Zoom = 6;
            gm_Main.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Main.CanDragMap = true;
            gm_Main.ScalePen = new Pen(Color.Black, 3);
            gm_Main.MapScaleInfoEnabled = true;

            // clear all overlays
            gm_Main.Overlays.Clear();
            gmo_Airports.Clear();
            gmo_Callsigns.Clear();
            gmo_Locators.Clear();
            gmo_Objects.Clear();
            gmo_Planes.Clear();
            gmo_PropagationPaths.Clear();
            gmo_Routes.Clear();
            gmo_Locators.Clear();
            gmo_Distances.Clear();

            // add all overlays
            gm_Main.Overlays.Add(gmo_Locators);
            gm_Main.Overlays.Add(gmo_Distances);
            gm_Main.Overlays.Add(gmo_Airports);
            gm_Main.Overlays.Add(gmo_PropagationPaths);
            gm_Main.Overlays.Add(gmo_Objects);
            gm_Main.Overlays.Add(gmo_Routes);
            gm_Main.Overlays.Add(gmo_Planes);
            gm_Main.Overlays.Add(gmo_Callsigns);
            gm_Main.Overlays.Add(gmo_CallsignDetails);

            gm_Main.Opacity = (double)Properties.Settings.Default.Map_Opacity;

            // create locators, if enabled
            if (Properties.Settings.Default.Map_ShowLocators)
                InitializeLocators();

            // setting User Agent to fix Open Street Map issue 2016-09-20
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_Nearest.MapProvider.RefererUrl = "";
            // set initial settings for main map
            gm_Nearest.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Main.MapProvider.RefererUrl = "";
            gm_Nearest.IgnoreMarkerOnMouseWheel = true;
            gm_Nearest.MinZoom = 0;
            gm_Nearest.MaxZoom = 20;
            gm_Nearest.Zoom = 6;
            gm_Nearest.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Nearest.CanDragMap = true;
            gm_Nearest.ScalePen = new Pen(Color.Black, 3);
            gm_Nearest.MapScaleInfoEnabled = true;

            // clear all overlays
            gm_Nearest.Overlays.Clear();
            gmo_NearestPaths.Clear();
            gmo_NearestPlanes.Clear();

            // add all overlays
            gm_Nearest.Overlays.Add(gmo_NearestPaths);
            gm_Nearest.Overlays.Add(gmo_NearestPlanes);
            gm_Nearest.Position = new PointLatLng(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
            gm_Nearest.ShowCenter = false;

            // setting User Agent to fix Open Street Map issue 2016-09-20
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_Cache.MapProvider.RefererUrl = "";
            // set initial settings for main map
            gm_Cache.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);

        }

        private void CreateLocators(double steplat, double steplon, Pen pen)
        {
            // get visible map bounds first
            double minlat = (int)(gm_Main.ViewArea.Bottom / steplat) * steplat - steplat;
            double maxlat = gm_Main.ViewArea.Top;
            double minlon = (int)(gm_Main.ViewArea.Left / steplon) * steplon - steplon;
            double maxlon = gm_Main.ViewArea.Right;

            Console.WriteLine("ViewArea = " + minlat.ToString("F2") + "," + minlon.ToString("F2") + " <> " + maxlat.ToString("F2") + "," + maxlon.ToString("F2"));

            for (double lon = minlon; lon < maxlon; lon += steplon)
            {
                for (double lat = minlat; lat < maxlat; lat += steplat)
                {
                    // create name 
                    string loc = "";
                    if (steplat >= 10.0)
                    {
                        loc = MaidenheadLocator.LocFromLatLon(lat + steplat / 2, lon + steplon / 2, false, 1);
                    }
                    else if (steplat >= 1.0)
                    {
                        loc = MaidenheadLocator.LocFromLatLon(lat + steplat / 2, lon + steplon / 2, false, 2);
                    }
                    else if (steplat >= 1.0 / 24.0)
                    {
                        loc = MaidenheadLocator.LocFromLatLon(lat + steplat / 2, lon + steplon / 2, false, 3);
                    }
                    else if (steplat >= 1.0 / 24.0 / 10.0)
                    {
                        loc = MaidenheadLocator.LocFromLatLon(lat + steplat / 2, lon + steplon / 2, false, 4);
                    }
                    else if (steplat >= 1.0 / 24.0 / 10.0 / 24.0)
                    {
                        loc = MaidenheadLocator.LocFromLatLon(lat + steplat / 2, lon + steplon / 2, false, 5);
                    }

                    // create polygon
                    List<PointLatLng> l = new List<PointLatLng>();
                    l.Add(new PointLatLng(lat, lon));
                    l.Add(new PointLatLng(lat, lon + steplon));
                    l.Add(new PointLatLng(lat + steplat, lon + steplon));
                    l.Add(new PointLatLng(lat + steplat, lon));
                    GMapLocatorPolygon p = new GMapLocatorPolygon(l, loc);
                    p.Stroke = pen;
                    p.Fill = Brushes.Transparent;
                    p.Tag = loc;
                    gmo_Locators.Polygons.Add(p);
                }
            }

        }
        private void InitializeLocators()
        {
            // NASTY!! still throws execption sometimes
            // when restoring cursor
            try
            {
                // clear locator overlay anyway
                gmo_Locators.Clear();
            }
            catch (Exception ex)
            {
                // do all most nothing
            }

            // return if not activated
            if (!Properties.Settings.Default.Map_ShowLocators)
                return;

            // create great circles, if enabled
            if (Properties.Settings.Default.Map_ShowLocators)
            {
                try
                {
                    gmo_Locators.IsVisibile = true;
                    // declutter: calculate an approbiate stepwidth according to the zoom level
                    double bigsteplat = 0;
                    double bigsteplon = 0;
                    double smallsteplat = 0;
                    double smallsteplon = 0;
                    Pen bigpen = new Pen(Brushes.Transparent);
                    Pen smallpen = new Pen(Brushes.Transparent);
                    switch (gm_Main.Zoom)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            bigsteplat = 10.0;
                            bigsteplon = 20.0;
                            bigpen = new Pen(Color.DarkGray, 1);
                            smallsteplat = 0;
                            smallsteplon = 0;
                            smallpen = new Pen(Brushes.Transparent);
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            bigsteplat = 1.0;
                            bigsteplon = 2.0;
                            bigpen = new Pen(Color.DarkGray, 1);
                            smallsteplat = 0;
                            smallsteplon = 0;
                            smallpen = new Pen(Brushes.Transparent);
                            break;
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            bigsteplat = 1.0;
                            bigsteplon = 2.0;
                            bigpen = new Pen(Color.DarkGray, 2);
                            smallsteplat = 1.0 / 24.0;
                            smallsteplon = 2.0 / 24.0;
                            smallpen = new Pen(Color.DarkGray, 1);
                            break;
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                            bigsteplat = 1.0 / 24.0;
                            bigsteplon = 2.0 / 24.0;
                            bigpen = new Pen(Color.DarkGray, 2);
                            smallsteplat = 1.0 / 24.0 / 10.0;
                            smallsteplon = 2.0 / 24.0 / 10.0;
                            smallpen = new Pen(Color.DarkGray, 1);
                            break;
                        case 18:
                        case 19:
                        case 20:
                            bigsteplat = 1.0 / 24.0 / 10.0;
                            bigsteplon = 2.0 / 24.0 / 10.0;
                            bigpen = new Pen(Color.DarkGray, 2);
                            smallsteplat = 1.0 / 24.0 / 10.0 / 24.0;
                            smallsteplon = 2.0 / 24.0 / 10.0 / 24.0;
                            smallpen = new Pen(Color.DarkGray, 1);
                            break;
                    }

                    if ((bigsteplon > 0) && (bigsteplat > 0))
                    {
                        CreateLocators(bigsteplat, bigsteplon, bigpen);
                    }
                    if ((smallsteplon > 0) && (smallsteplat > 0))
                    {
                        CreateLocators(smallsteplat, smallsteplon, smallpen);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }

        }

        private void Splash(string text)
        {
            Splash(text, Color.White);
        }

        private void Splash(string text, Color color)
        {
            // show message in splash window
            if (SplashDlg != null)
            {
                SplashDlg.Status(text, color);
            }
        }

        private void MapDlg_Load(object sender, EventArgs e)
        {
            try
            {
                // Show splash screen
                SplashDlg = new Splash();
                SplashDlg.Show();
                // bring window to front
                SplashDlg.BringToFront();
                // wait for splash window is fully visible
                while (SplashDlg.Opacity < 1)
                {
                    Application.DoEvents();
                }
                // show AirScout main window
                this.BringToFront();
                // Check directories, complete it and create, if not exist
                Splash("Checking directories...");
                CheckDirectories();
                // start a log, specify format of logfile and entries
                Splash("Initializing logfile...");
                InitializeLogfile();
                // Check properties
                Splash("Checking settings...");
                CheckSettings();
                // check, copy and load plugins
                Splash("Loading plugins...");
                LoadPlugins();
                // Initialize database
                Splash("Initializing database...");
                InitializeDatabase();
                // initialize icons
                Splash("Creating icons...");
                InitializeIcons();
                ToolTipFont = CreateFontFromString(Properties.Settings.Default.Map_ToolTipFont);
                Splash("Loading Map...");

                cntdn = Properties.Settings.Default.Planes_Update;
                btn_Map_PlayPause.Select();

                // intially fill dialog box elements and set band
                string[] bands = Bands.GetStringValuesExceptNoneAndAll();
                foreach (string b in bands)
                    cb_Band.Items.Add(b);
                BAND band = Properties.Settings.Default.Band;
                cb_Band.SelectedItem = Bands.GetStringValue(band);
                string[] cats = PlaneCategories.GetStringValues();
                foreach (string cat in cats)
                    cb_Planes_Filter_Min_Cat.Items.Add(cat);
                // initialize gauge controls
                ag_Azimuth.fromAngle = -90;
                ag_Azimuth.toAngle = 270;

                ag_Elevation.fromAngle = 180;
                ag_Elevation.toAngle = 270;

                // install additional mouse events
                gb_Map_Info.MouseClick += new MouseEventHandler(this.gb_Map_Info_MouseClick);
                gb_Map_Zoom.MouseClick += new MouseEventHandler(this.gb_Map_Zoom_MouseClick);
                gb_Map_Filter.MouseClick += new MouseEventHandler(this.gb_Map_Filter_MouseClick);
                gb_Map_Alarms.MouseClick += new MouseEventHandler(this.gb_Map_Alarms_MouseClick);

                if (PlaneFeedPlugins != null)
                {
                    foreach (IPlaneFeedPlugin plugin in PlaneFeedPlugins)
                    {
                        if (Properties.Settings.Default.Planes_PlaneFeed1 == plugin.Name)
                        {
                            bw_PlaneFeed1 = new PlaneFeed();
                            bw_PlaneFeed1.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                        if (Properties.Settings.Default.Planes_PlaneFeed2 == plugin.Name)
                        {
                            bw_PlaneFeed2 = new PlaneFeed();
                            bw_PlaneFeed2.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                        if (Properties.Settings.Default.Planes_PlaneFeed3 == plugin.Name)
                        {
                            bw_PlaneFeed3 = new PlaneFeed();
                            bw_PlaneFeed3.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                    }
                }

                // update image list sizes
                il_Planes_L.ImageSize = new System.Drawing.Size(Properties.Settings.Default.Planes_IconSize_L, Properties.Settings.Default.Planes_IconSize_L);
                il_Planes_M.ImageSize = new System.Drawing.Size(Properties.Settings.Default.Planes_IconSize_M, Properties.Settings.Default.Planes_IconSize_M);
                il_Planes_H.ImageSize = new System.Drawing.Size(Properties.Settings.Default.Planes_IconSize_H, Properties.Settings.Default.Planes_IconSize_H);
                il_Planes_S.ImageSize = new System.Drawing.Size(Properties.Settings.Default.Planes_IconSize_S, Properties.Settings.Default.Planes_IconSize_S);

                // try to upgrade user settings from prevoius version on first run
                try
                {
                    if (FirstRun)
                    {
                        Log.WriteMessage("Preparing for first run.");

                        // Reload settings
                        LoadUserSettings();

                        // try to ugrade settings when not started with /CLEAN option
                        if (!CleanRun)
                        {
                            Log.WriteMessage("Upgrading settings.");
                            Properties.Settings.Default.Upgrade();
                            // handle skip to version 1.3.3.1
                            if (String.IsNullOrEmpty(Properties.Settings.Default.Version) || (String.Compare(Properties.Settings.Default.Version, "1.3.3.0") < 0))
                            {
                                /*
                                // reset elevation data url to new default values
                                Properties.Settings.Default.Elevation_GLOBE_URL = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Elevation_GLOBE_URL));
                                Properties.Settings.Default.Elevation_SRTM3_URL = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Elevation_SRTM3_URL));
                                Properties.Settings.Default.Elevation_SRTM1_URL = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Elevation_SRTM1_URL));
                                // reset elevation data path to default values
                                Properties.Settings.Default.Elevation_GLOBE_DataPath = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Elevation_GLOBE_DataPath));
                                Properties.Settings.Default.Elevation_SRTM3_DataPath = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Elevation_SRTM3_DataPath));
                                Properties.Settings.Default.Elevation_SRTM1_DataPath = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Elevation_SRTM1_DataPath));
                                // reset stations data url to its default value
                                Properties.Settings.Default.StationDatabase_Update_URL = GetPropertyDefaultValue(nameof(Properties.Settings.Default.StationDatabase_Update_URL));
                                */

                                // reset Sync with KST option 
                                Properties.Settings.Default.Watchlist_SyncWithKST = GetPropertyDefaultValue(nameof(Properties.Settings.Default.Watchlist_SyncWithKST));

                                Properties.Settings.Default.Version = Application.ProductVersion;

                                SaveUserSettings();
                            }
                            AirScout.PlaneFeeds.Properties.Settings.Default.Upgrade();
                        }

                        CheckDirectories();
                        CheckSettings();
                        // reset topmost state
                        if (SplashDlg != null)
                            SplashDlg.TopMost = false;
                        /*
                        // run database updater once for basic information
                        bw_DatabaseUpdater.RunWorkerAsync(UPDATERSTARTOPTIONS.FIRSTRUN);
                        // wait till finished
                        while (bw_DatabaseUpdater.IsBusy)
                            Application.DoEvents();
                        */
                        if (SplashDlg != null)
                            SplashDlg.Close();
                        // must have internet connection on FirstRun
                        CheckInternet();
                        // show FirstRunWizard
                        try
                        {
                            FirstRunWizard Dlg = new FirstRunWizard(this);
                            if (Dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                Log.WriteMessage("Aborting FirstRunWizard.");
                                // flush the log and exit immediately
                                Log.FlushLog();
                                System.Environment.Exit(-1);
                            }
                            else
                            {
                                // reset FirstRun property
                                Properties.Settings.Default.FirstRun = false;
                                Properties.Settings.Default.FirstRun = Properties.Settings.Default.FirstRun;

                                // set privacy statements (for legacy)
                                Properties.Settings.Default.First_Agree = true;
                                Properties.Settings.Default.First_Disagree = false;
                                Properties.Settings.Default.First_Privacy = false;

                                // save settings
                                SaveUserSettings();
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage(ex.ToString(), LogLevel.Error);
                            // flush the log and exit immediately
                            Log.FlushLog();
                            System.Environment.Exit(-1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
                // get initial widths and heigths
                gb_Map_Info_DefaultWidth = gb_Map_Info.Width;
                tc_Main_DefaultHeight = tc_Main.Height;

                // refresh the Layout
                OnSizeChanged(null);

                // get initial height of boxes
                gb_Map_Info_MinimizedHeight = gb_Map_Info.Height - gb_Map_Info.DisplayRectangle.Height;
                gb_Map_Zoom_MinimizedHeight = gb_Map_Zoom.Height - gb_Map_Zoom.DisplayRectangle.Height;
                gb_Map_Filter_MinimizedHeigth = gb_Map_Filter.Height - gb_Map_Filter.DisplayRectangle.Height;
                gb_Map_Alarms_MinimizedHeight = gb_Map_Alarms.Height - gb_Map_Alarms.DisplayRectangle.Height;
                gb_Map_Info_MaximizedHeight = gb_Map_Info.Height;
                gb_Map_Zoom_MaximizedHeight = gb_Map_Zoom.Height;
                gb_Map_Filter_MaximizedHeigth = gb_Map_Filter.Height;
                gb_Map_Alarms_MaximizedHeight = gb_Map_Alarms.Height;

                // check directories and settings for missing values
                CheckDirectories();
                CheckSettings();

                if (PlaneFeedPlugins != null)
                {
                    foreach (IPlaneFeedPlugin plugin in PlaneFeedPlugins)
                    {
                        if (Properties.Settings.Default.Planes_PlaneFeed1 == plugin.Name)
                        {
                            bw_PlaneFeed1 = new PlaneFeed();
                            bw_PlaneFeed1.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                        if (Properties.Settings.Default.Planes_PlaneFeed2 == plugin.Name)
                        {
                            bw_PlaneFeed2 = new PlaneFeed();
                            bw_PlaneFeed2.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                        if (Properties.Settings.Default.Planes_PlaneFeed3 == plugin.Name)
                        {
                            bw_PlaneFeed3 = new PlaneFeed();
                            bw_PlaneFeed3.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                    }
                }

                // register this instance of AirScout and get a session key
                InitializeSession();

                // start permanent background workers
                StartAllBackgroundWorkers();

                // start main timer
                ti_Progress.Start();

                FirstRun = false;

                // check if a vaild feed is on
                if ((bw_PlaneFeed1 == null) && (bw_PlaneFeed2 == null) && (bw_PlaneFeed3 == null))
                    MessageBox.Show("Plane Feeds are disabled. \n\nYou can use the software anyway, but you will never see a plane on the map. \nIn order to get valid plane positions do the following:\n\n1. Go to the \"Options/Plane\" tab and activate at least one plane feed\n2. Go to the \"Options/General\" tab and adjust \"Planes Positions Range\" to a suitable area around your location", "Plane Feeds Disabled");

                // invalidate tracking values
                TrackMode = AIRSCOUTTRACKMODE.NONE;

                // set online mode by default
                Properties.Settings.Default.Time_Mode_Online = true;

                // install OnIdle event handler
                // must be here at first!
                Application.Idle += new EventHandler(OnIdle);
                // move map to MyLoc
                if (!double.IsNaN(Properties.Settings.Default.MyLat) && !double.IsNaN(Properties.Settings.Default.MyLon))
                    gm_Main.Position = new PointLatLng(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
                // update status
                UpdateStatus();
                //initialize maps
                InitializeMaps();
                // show airports on map
                UpdateAirports();
                // show watchlist on map
                UpdateWatchlistInMap();
                // set special event handlers on locator combo boxes
                // set callsign history and locs
                cb_MyCall_TextChanged(this, null);
                // set MyLoc and DXLoc combobox properties
                cb_MyLoc.DisplayMember = nameof(LocatorDropDownItem.Locator);
                cb_MyLoc.ValueMember = nameof(LocatorDropDownItem.GeoLocation);
                cb_MyLoc.Precision = (int)Properties.Settings.Default.Locator_MaxLength / 2;
                cb_MyLoc.SmallLettersForSubsquares = Properties.Settings.Default.Locator_SmallLettersForSubsquares;
                cb_MyLoc.AutoLength = Properties.Settings.Default.Locator_AutoLength;
                cb_DXLoc.DisplayMember = nameof(LocatorDropDownItem.Locator);
                cb_DXLoc.ValueMember = nameof(LocatorDropDownItem.GeoLocation);
                cb_DXLoc.Precision = (int)Properties.Settings.Default.Locator_MaxLength / 2;
                cb_DXLoc.SmallLettersForSubsquares = Properties.Settings.Default.Locator_SmallLettersForSubsquares;
                cb_DXLoc.AutoLength = Properties.Settings.Default.Locator_AutoLength;
                // populate watchlist
                RefreshWatchlistView();
                // Linux/Mon Hacks for layout
                if (SupportFunctions.IsMono)
                {
                    // cycle control tab view to ensure that all elements are drawn
                    tc_Control.SelectedTab = tp_Control_Options;
                    tc_Control.SelectedTab = tp_Control_Multi;
                    tc_Control.SelectedTab = tp_Control_Single;

                    btn_Map_PlayPause.Image = null;
                    btn_Map_PlayPause.Text = "Play";
                    btn_Map_PlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    btn_Map_PlayPause.Font = new Font(btn_Map_PlayPause.Font, FontStyle.Bold);
                }
                // set players bounds
                sb_Analysis_Play.Minimum = 0;
                sb_Analysis_Play.Maximum = int.MaxValue;
                // set path mode to single
                PathMode = AIRSCOUTPATHMODE.SINGLE;
                // set life mode to life
                LifeMode = AIRSCOUTLIFEMODE.LIFE;
                // set play mode to pause
                PlayMode = AIRSCOUTPLAYMODE.PAUSE;

                // maintain background calculations thread wait
                Properties.Settings.Default.Background_Calculations_ThreadWait = 0;
                Log.WriteMessage("Finished.");
                // start timer to finish startup
                ti_Startup.Start();
            }
            catch (Exception ex)
            {
                // close the application in case of any exception
                if (Log != null)
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                else
                    Console.WriteLine("Fatal Error: " + ex.ToString());
                // close the splash window
                if (SplashDlg != null)
                    SplashDlg.Close();
                MessageBox.Show("An error occured during startup: " + ex.ToString() + "\n\nPress >OK< to close the application.", "AirScout", MessageBoxButtons.OK);
                this.Close();
            }
            // restore window size, state and location
            try
            {
                if (!SupportFunctions.IsMono)
                {
                    this.Size = Properties.Settings.Default.General_WindowSize;
                    this.Location = Properties.Settings.Default.General_WindowLocation;
                    this.WindowState = Properties.Settings.Default.General_WindowState;
                }
                else
                {
                    // ignore window settings under Linux/Mono
                    // start always maximized
                    this.WindowState = FormWindowState.Maximized;

                }
            }
            catch (Exception ex)
            {
                // do nothing if failed
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
                // reset splitter positions to its default value in settings
                Properties.Settings.Default.MainSplitter_Distance = -1;
                Properties.Settings.Default.MapSplitter_Distance = -1;
            }

            // dump all properties to log
            Log.WriteMessage("============================= Application Settings ==================================");
            DumpSettingsToLog("Properties.Settings.Default", Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("AirScout.PlaneFeeds.Properties.Settings.Default", AirScout.PlaneFeeds.Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("ScoutBase.Elevation.Properties.Settings.Default", ScoutBase.Elevation.Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("ScoutBase.Stations.Properties.Settings.Default", ScoutBase.Stations.Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("ScoutBase.Propagation.Properties.Settings.Default", ScoutBase.Propagation.Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("ScoutBase.CAT.Properties.Settings.Default", ScoutBase.CAT.Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("AirScout.Aircrafts.Properties.Settings.Default", AirScout.Aircrafts.Properties.Settings.Default.PropertyValues);
            DumpSettingsToLog("AirScout.CAT.Properties.Settings.Default", AirScout.CAT.Properties.Settings.Default.PropertyValues);
            Log.WriteMessage("=====================================================================================");

            // set Pause Mode
 //           Pause();

        }

        private void FinishStartup()
        {
            // finish startup
            // close splash window
            // set window layout
            if (SplashDlg != null)
                SplashDlg.Close();
            // restore splitter positions
            try
            {
                if (!SupportFunctions.IsMono)
                {
                    MapSplitterDistance = Properties.Settings.Default.MapSplitter_Distance;
                    MainSplitterDistance = Properties.Settings.Default.MainSplitter_Distance;
                }
                else
                {
                    // ignore window settings under Linux/Mono and always use default values
                    MapSplitterDistance = -1;
                    MainSplitterDistance = -1;
                }
            }
            catch (Exception ex)
            {
                // do nothing if failed
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            // make main window visible
            this.Visible = true;
            // set focus to map
            this.btn_Map_Save.Focus();
            // Linux/Mono compatibility
            // simulate splitter click
            this.sc_Map_SplitterMoved(this, null);
            this.sc_Main_SplitterMoved(this, null);
        }

        // creates plane feed work event arguments from settings
        private PlaneFeedWorkEventArgs CreatePlaneFeedWorkEventArgs(string feedname)
        {
            PlaneFeedWorkEventArgs feedargs = new PlaneFeedWorkEventArgs();
            feedargs.Feed = null;
            foreach (IPlaneFeedPlugin plugin in PlaneFeedPlugins)
            {
                if (plugin.Name == feedname)
                    feedargs.Feed = plugin;
            }
            feedargs.AppDirectory = AppDirectory;
            feedargs.AppDataDirectory = AppDataDirectory;
            feedargs.LogDirectory = LogDirectory;
            feedargs.TmpDirectory = TmpDirectory;
            feedargs.DatabaseDirectory = DatabaseDirectory;
            feedargs.PlanePositionsDirectory = PlanePositionsDirectory;
            feedargs.MaxLat = Properties.Settings.Default.MaxLat;
            feedargs.MinLon = Properties.Settings.Default.MinLon;
            feedargs.MinLat = Properties.Settings.Default.MinLat;
            feedargs.MaxLon = Properties.Settings.Default.MaxLon;
            feedargs.MinAlt = Properties.Settings.Default.Planes_MinAlt;
            feedargs.MaxAlt = Properties.Settings.Default.Planes_MaxAlt;
            feedargs.MyLat = Properties.Settings.Default.MyLat;
            feedargs.MyLon = Properties.Settings.Default.MyLon;
            feedargs.DXLat = Properties.Settings.Default.DXLat;
            feedargs.DXLon = Properties.Settings.Default.DXLon;
            feedargs.KeepHistory = Properties.Settings.Default.Planes_KeepHistory;
            feedargs.Interval = Properties.Settings.Default.Planes_Interval;
            feedargs.ExtendedPlausibilityCheck_Enable = Properties.Settings.Default.Planes_ExtendedPlausibilityCheck_Enabled;
            feedargs.ExtendedPlausiblityCheck_MaxErrorDist = Properties.Settings.Default.Planes_ExtendedPlausibilityCheck_MaxErrorDist;
            feedargs.LogErrors = Properties.Settings.Default.Planes_LogErrors;
            feedargs.InstanceID = Properties.Settings.Default.AirScout_Instance_ID;
            feedargs.SessionKey = SessionKey;
            feedargs.GetKeyURL = Properties.Settings.Default.AirScout_GetKey_URL;
            feedargs.LogPlanePositions = Properties.Settings.Default.Planes_TracePositions;
            return feedargs;
        }

        private void StartAllBackgroundWorkers()
        {
            // start all background workers
            // check if the thread is not NULL and not activated
            Say("Starting background threads...");
            if ((bw_AirportMapper != null) && !bw_AirportMapper.IsBusy)
                bw_AirportMapper.RunWorkerAsync();
            if ((bw_JSONWriter != null) && !bw_JSONWriter.IsBusy)
                bw_JSONWriter.RunWorkerAsync();
            if ((bw_NewsFeed != null) && !bw_NewsFeed.IsBusy)
                bw_NewsFeed.RunWorkerAsync();
            if ((bw_AircraftDatabaseMaintainer != null) && (!bw_AircraftDatabaseMaintainer.IsBusy))
            {
                AircraftPositionDatabaseMaintainerStartOptions startoptions = new AircraftPositionDatabaseMaintainerStartOptions();
                startoptions.Name = "Aircrafts";
                startoptions.Database_MaxCount = (long)Properties.Settings.Default.AircraftDatabase_MaxCount;
                startoptions.Database_MaxSize = (double)Properties.Settings.Default.AircraftDatabase_MaxSize;
                startoptions.Database_MaxDaysLifetime = (int)Properties.Settings.Default.AircraftDatabase_MaxDaysLifetime;
                bw_AircraftDatabaseMaintainer.RunWorkerAsync(startoptions);
            }
            if (Properties.Settings.Default.Background_Update_OnStartup)
            {
                if ((bw_StationDatabaseUpdater != null) && !bw_StationDatabaseUpdater.IsBusy)
                {
                    StationDatabaseUpdaterStartOptions startoptions = new StationDatabaseUpdaterStartOptions();
                    startoptions.Name = "Stations";
                    startoptions.RestrictToAreaOfInterest = Properties.Settings.Default.Location_RestrictToAreaOfInterest;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.InstanceID = Properties.Settings.Default.AirScout_Instance_ID;
                    startoptions.SessionKey = SessionKey;
                    startoptions.GetKeyURL = Properties.Settings.Default.AirScout_GetKey_URL;
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    bw_StationDatabaseUpdater.RunWorkerAsync(startoptions);
                }
                if ((bw_AircraftDatabaseUpdater != null) && !bw_AircraftDatabaseUpdater.IsBusy)
                {
                    AircraftDatabaseUpdaterStartOptions startoptions = new AircraftDatabaseUpdaterStartOptions();
                    startoptions.Name = "Aircrafts";
                    startoptions.InstanceID = Properties.Settings.Default.AirScout_Instance_ID;
                    startoptions.SessionKey = SessionKey;
                    startoptions.GetKeyURL = Properties.Settings.Default.AirScout_GetKey_URL;
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    bw_AircraftDatabaseUpdater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (bw_GLOBEUpdater != null) && !bw_GLOBEUpdater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "GLOBE";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.GLOBE;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_GLOBE_EnableCache;
                    bw_GLOBEUpdater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (bw_SRTM3Updater != null) && !bw_SRTM3Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "SRTM3";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.SRTM3;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_SRTM3_EnableCache;
                    bw_SRTM3Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (bw_SRTM1Updater != null) && !bw_SRTM1Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "SRTM1";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.SRTM1;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_SRTM1_EnableCache;
                    bw_SRTM1Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_ASTER3_Enabled && (bw_ASTER3Updater != null) && !bw_ASTER3Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "ASTER3";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.ASTER3;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_ASTER3_EnableCache;
                    bw_ASTER3Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_ASTER1_Enabled && (bw_ASTER1Updater != null) && !bw_ASTER1Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "ASTER1";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.ASTER1;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_ASTER1_EnableCache;
                    bw_ASTER1Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (bw_GLOBEPathCalculator != null) && !bw_GLOBEPathCalculator.IsBusy)
                    bw_GLOBEPathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE);
                if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (bw_SRTM3PathCalculator != null) && !bw_SRTM3PathCalculator.IsBusy)
                    bw_SRTM3PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE);
                if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (bw_SRTM1PathCalculator != null) && !bw_SRTM1PathCalculator.IsBusy)
                    bw_SRTM1PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE);
                if (Properties.Settings.Default.Elevation_ASTER3_Enabled && (bw_ASTER3PathCalculator != null) && !bw_ASTER3PathCalculator.IsBusy)
                    bw_ASTER3PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE);
                if (Properties.Settings.Default.Elevation_ASTER1_Enabled && (bw_ASTER1PathCalculator != null) && !bw_ASTER1PathCalculator.IsBusy)
                    bw_ASTER1PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE);
                if (Properties.Settings.Default.Map_Preloader_Enabled && (bw_MapPreloader != null) && !bw_MapPreloader.IsBusy)
                    bw_MapPreloader.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE);
            }
            else if (Properties.Settings.Default.Background_Update_Periodically)
            {
                if ((bw_StationDatabaseUpdater != null) && !bw_StationDatabaseUpdater.IsBusy)
                {
                    StationDatabaseUpdaterStartOptions startoptions = new StationDatabaseUpdaterStartOptions();
                    startoptions.Name = "Stations";
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.RestrictToAreaOfInterest = Properties.Settings.Default.Location_RestrictToAreaOfInterest;
                    startoptions.InstanceID = Properties.Settings.Default.AirScout_Instance_ID;
                    startoptions.SessionKey = SessionKey;
                    startoptions.GetKeyURL = Properties.Settings.Default.AirScout_GetKey_URL;
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    bw_StationDatabaseUpdater.RunWorkerAsync(startoptions);
                }
                if ((bw_AircraftDatabaseUpdater != null) && !bw_AircraftDatabaseUpdater.IsBusy)
                {
                    AircraftDatabaseUpdaterStartOptions startoptions = new AircraftDatabaseUpdaterStartOptions();
                    startoptions.Name = "Aircrafts";
                    startoptions.InstanceID = Properties.Settings.Default.AirScout_Instance_ID;
                    startoptions.SessionKey = SessionKey;
                    startoptions.GetKeyURL = Properties.Settings.Default.AirScout_GetKey_URL;
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    bw_AircraftDatabaseUpdater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (bw_GLOBEUpdater != null) && !bw_GLOBEUpdater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "GLOBE";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    startoptions.Model = ELEVATIONMODEL.GLOBE;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_GLOBE_EnableCache;
                    bw_GLOBEUpdater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (bw_SRTM3Updater != null) && !bw_SRTM3Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "SRTM3";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    startoptions.Model = ELEVATIONMODEL.SRTM3;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_SRTM3_EnableCache;
                    bw_SRTM3Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (bw_SRTM1Updater != null) && !bw_SRTM1Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "SRTM1";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    startoptions.Model = ELEVATIONMODEL.SRTM1;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_SRTM1_EnableCache;
                    bw_SRTM1Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_ASTER3_Enabled && (bw_ASTER3Updater != null) && !bw_ASTER3Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "ASTER3";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    startoptions.Model = ELEVATIONMODEL.ASTER3;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_ASTER3_EnableCache;
                    bw_ASTER3Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_ASTER1_Enabled && (bw_ASTER1Updater != null) && !bw_ASTER1Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "ASTER1";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                    startoptions.Model = ELEVATIONMODEL.ASTER1;
                    startoptions.MinLat = Properties.Settings.Default.MinLat;
                    startoptions.MinLon = Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_ASTER1_EnableCache;
                    bw_ASTER1Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (bw_GLOBEPathCalculator != null) && !bw_GLOBEPathCalculator.IsBusy)
                    bw_GLOBEPathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (bw_SRTM3PathCalculator != null) && !bw_SRTM3PathCalculator.IsBusy)
                    bw_SRTM3PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (bw_SRTM1PathCalculator != null) && !bw_SRTM1PathCalculator.IsBusy)
                    bw_SRTM1PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (Properties.Settings.Default.Elevation_ASTER3_Enabled && (bw_ASTER3PathCalculator != null) && !bw_ASTER3PathCalculator.IsBusy)
                    bw_ASTER3PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (Properties.Settings.Default.Elevation_ASTER1_Enabled && (bw_ASTER1PathCalculator != null) && !bw_ASTER1PathCalculator.IsBusy)
                    bw_ASTER1PathCalculator.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (Properties.Settings.Default.Map_Preloader_Enabled && (bw_MapPreloader != null) && !bw_MapPreloader.IsBusy)
                    bw_MapPreloader.RunWorkerAsync(BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
            }
            
            if ((bw_PlaneFeed1 != null) && (!bw_PlaneFeed1.IsBusy))
                bw_PlaneFeed1.RunWorkerAsync(CreatePlaneFeedWorkEventArgs(Properties.Settings.Default.Planes_PlaneFeed1));
            if ((bw_PlaneFeed2 != null) && (!bw_PlaneFeed2.IsBusy))
                bw_PlaneFeed2.RunWorkerAsync(CreatePlaneFeedWorkEventArgs(Properties.Settings.Default.Planes_PlaneFeed2));
            if ((bw_PlaneFeed3 != null) && (!bw_PlaneFeed3.IsBusy))
                bw_PlaneFeed3.RunWorkerAsync(CreatePlaneFeedWorkEventArgs(Properties.Settings.Default.Planes_PlaneFeed3));
            
            if (Properties.Settings.Default.Server_Activate)
            {
                if ((bw_WinTestReceive != null) && (!bw_WinTestReceive.IsBusy))
                    bw_WinTestReceive.RunWorkerAsync();
                WebserverStartArgs args = new WebserverStartArgs();
                args.TmpDirectory = TmpDirectory;
                args.WebserverDirectory = WebserverDirectory;
                if ((bw_Webserver != null) && (!bw_Webserver.IsBusy))
                    bw_Webserver.RunWorkerAsync(args);
            }
            
            if (Properties.Settings.Default.SpecLab_Enabled)
            {
                if ((bw_SpecLab_Receive != null) && (!bw_SpecLab_Receive.IsBusy))
                    bw_SpecLab_Receive.RunWorkerAsync();
            }
            
            if (Properties.Settings.Default.Track_Activate)
            {
                if ((bw_Track != null) && (!bw_Track.IsBusy))
                    bw_Track.RunWorkerAsync();
            }
            
            if (AirScout.CAT.Properties.Settings.Default.CAT_Activate)
            {
                CATUpdaterStartOptions startoptions = new CATUpdaterStartOptions();
                startoptions.Name = "CAT Updater";
                startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY;
                if ((bw_CATUpdater != null) && (!bw_CATUpdater.IsBusy))
                    bw_CATUpdater.RunWorkerAsync(startoptions);
            }

            if (AirScout.CAT.Properties.Settings.Default.CAT_Activate)
            {
                CATWorkerStartOptions startoptions = new CATWorkerStartOptions();
                startoptions.Name = "CAT Interface";
                startoptions.RigType = AirScout.CAT.Properties.Settings.Default.CAT_RigType;
                startoptions.PortName = AirScout.CAT.Properties.Settings.Default.CAT_PortName;
                startoptions.Baudrate = AirScout.CAT.Properties.Settings.Default.CAT_Baudrate;
                startoptions.DataBits = AirScout.CAT.Properties.Settings.Default.CAT_DataBits;
                startoptions.Parity = AirScout.CAT.Properties.Settings.Default.CAT_Parity;
                startoptions.StopBits = AirScout.CAT.Properties.Settings.Default.CAT_StopBits;
                startoptions.RTS = AirScout.CAT.Properties.Settings.Default.CAT_RTS;
                startoptions.DTR = AirScout.CAT.Properties.Settings.Default.CAT_DTR;
                startoptions.Poll = AirScout.CAT.Properties.Settings.Default.CAT_Poll;
                startoptions.Timeout = AirScout.CAT.Properties.Settings.Default.CAT_Timeout;
                if ((bw_CAT != null) && (!bw_CAT.IsBusy))
                    bw_CAT.RunWorkerAsync(startoptions);
            }
            Say("Background threads started.");
        }

        private void StopBackgroundworker(BackgroundWorker worker, string name, int count, int total)
        {
            if (worker == null)
                return;
            if (!worker.IsBusy)
                return;
            worker.CancelAsync();
            // waiting for background threads to finish
            int timeout = 10000;    // timeout in ms 
            Stopwatch st = new Stopwatch();
            st.Start();
            Say("Stopping background thread " + count.ToString() + " of " + total.ToString() + " [" + name + "]...");
            while ((worker != null) && worker.IsBusy)
            {
                Application.DoEvents();
            }
            st.Stop();
            Log.WriteMessage("Stopping " + name + ", " + st.ElapsedMilliseconds.ToString() + " ms.");
        }

        private void StopAllBackgroundWorkers()
        {
            Say("Stopping background threads...");
            // cancel permanent background workers, wait for finish
            int bcount = 18;
            int i = 1;
            // cancel all threads
            StopBackgroundworker(bw_WinTestReceive, nameof(bw_WinTestReceive), i, bcount); i++;
            StopBackgroundworker(bw_SpecLab_Receive, nameof(bw_SpecLab_Receive), i, bcount); i++;
            StopBackgroundworker(bw_Track, nameof(bw_Track), i, bcount); i++;
            StopBackgroundworker(bw_JSONWriter, nameof(bw_JSONWriter), i, bcount); i++;
            StopBackgroundworker(bw_NewsFeed, nameof(bw_NewsFeed), i, bcount); i++;
            StopBackgroundworker(bw_PlaneFeed1, nameof(bw_PlaneFeed1), i, bcount); i++;
            StopBackgroundworker(bw_PlaneFeed2, nameof(bw_PlaneFeed2), i, bcount); i++;
            StopBackgroundworker(bw_PlaneFeed3, nameof(bw_PlaneFeed3), i, bcount); i++;
            StopBackgroundworker(bw_GLOBEPathCalculator, nameof(bw_GLOBEPathCalculator), i, bcount); i++;
            StopBackgroundworker(bw_SRTM3PathCalculator, nameof(bw_SRTM3PathCalculator), i, bcount); i++;
            StopBackgroundworker(bw_SRTM1PathCalculator, nameof(bw_SRTM1PathCalculator), i, bcount); i++;
            StopBackgroundworker(bw_ASTER3PathCalculator, nameof(bw_ASTER3PathCalculator), i, bcount); i++;
            StopBackgroundworker(bw_ASTER1PathCalculator, nameof(bw_ASTER1PathCalculator), i, bcount); i++;
            StopBackgroundworker(bw_AircraftDatabaseUpdater, nameof(bw_AircraftDatabaseUpdater), i, bcount); i++;
            StopBackgroundworker(bw_StationDatabaseUpdater, nameof(bw_StationDatabaseUpdater), i, bcount); i++;
            StopBackgroundworker(bw_MapPreloader, nameof(bw_MapPreloader), i, bcount); i++;
            StopBackgroundworker(bw_CATUpdater, nameof(bw_CATUpdater), i, bcount); i++;
            StopBackgroundworker(bw_CAT, nameof(bw_CAT), i, bcount); i++;
            Say("Background threads stopped.");
        }

        private void CancelAllBackgroundWorkers()
        {
            // cancel all background workers, don't wait for finish
            if (bw_AirportMapper != null)
                bw_AirportMapper.CancelAsync();
            if (bw_PlaneFeed1 != null)
                bw_PlaneFeed1.CancelAsync();
            if (bw_PlaneFeed2 != null)
                bw_PlaneFeed2.CancelAsync();
            if (bw_PlaneFeed3 != null)
                bw_PlaneFeed3.CancelAsync();
            if (bw_WinTestReceive != null)
                bw_WinTestReceive.CancelAsync();
            if (bw_SpecLab_Receive != null)
                bw_SpecLab_Receive.CancelAsync();
            if (bw_Track != null)
                bw_Track.CancelAsync();
            if (bw_JSONWriter != null)
                bw_JSONWriter.CancelAsync();
            if (bw_Webserver != null)
                bw_Webserver.CancelAsync();
            if (bw_NewsFeed != null)
                bw_NewsFeed.CancelAsync();
            if (bw_HistoryDownloader != null)
                bw_HistoryDownloader.CancelAsync();
            if (bw_StationDatabaseUpdater != null)
                bw_StationDatabaseUpdater.CancelAsync();
            if (bw_AircraftDatabaseMaintainer != null)
                bw_AircraftDatabaseMaintainer.CancelAsync();
            if (bw_AircraftDatabaseUpdater != null)
                bw_AircraftDatabaseUpdater.CancelAsync();
            if (bw_GLOBEPathCalculator != null)
                bw_GLOBEUpdater.CancelAsync();
            if (bw_SRTM3Updater != null)
                bw_SRTM3Updater.CancelAsync();
            if (bw_SRTM1Updater != null)
                bw_SRTM1Updater.CancelAsync();
            if (bw_ASTER3Updater != null)
                bw_ASTER3Updater.CancelAsync();
            if (bw_ASTER1Updater != null)
                bw_ASTER1Updater.CancelAsync();
            if (bw_GLOBEPathCalculator != null)
                bw_GLOBEPathCalculator.CancelAsync();
            if (bw_SRTM3PathCalculator != null)
                bw_SRTM3PathCalculator.CancelAsync();
            if (bw_SRTM1PathCalculator != null)
                bw_SRTM1PathCalculator.CancelAsync();
            if (bw_ASTER3PathCalculator != null)
                bw_ASTER3PathCalculator.CancelAsync();
            if (bw_ASTER1PathCalculator != null)
                bw_ASTER1PathCalculator.CancelAsync();
            if (bw_CATUpdater != null)
                bw_CATUpdater.CancelAsync();
            if (bw_CAT != null)
                bw_CAT.CancelAsync();
        }

        private Bitmap CreatePlaneIcon(Color color)
        {
            // get the basic icon
            Bitmap bm = new Bitmap(AppDirectory + Path.DirectorySeparatorChar + Properties.Settings.Default.Planes_IconFileName);
            // read the content and change color of each pixel
            for (int j = 0; j < bm.Width; j++)
            {
                for (int k = 0; k < bm.Height; k++)
                {
                    // get the color of each pixel
                    Color c = bm.GetPixel(j, k);
                    // check if not transparent
                    if (c.A > 0)
                    {
                        // change color
                        bm.SetPixel(j, k, Color.FromArgb(c.A, color.R, color.G, color.B));
                    }
                }
            }
            return bm;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public Color GetColor(double power)
        {
            double H = power * 0.3; // Hue (note 0.4 = Green, see huge chart below)
            double S = 0.95; // Saturation
            double B = 0.95; // Brightness

            return ColorFromHSV((float)H * 360, (float)S, (float)B);
        }

        private Bitmap CreateAirportIcon(int alpha)
        {
            // get the basic icon
            Bitmap bm = new Bitmap(AppDirectory + Path.DirectorySeparatorChar + Properties.Settings.Default.Airports_IconFileName);
            // read the content and change opacity of each pixel
            for (int j = 0; j < bm.Width; j++)
            {
                for (int k = 0; k < bm.Height; k++)
                {
                    // get the color of each pixel
                    Color c = bm.GetPixel(j, k);
                    // check if not transparent
                    if (c.A > 0)
                    {
                        // change color
                        bm.SetPixel(j, k, Color.FromArgb(alpha, c.R, c.G, c.B));
                    }
                }
            }
            return bm;
        }

        private void InitializeIcons()
        {
            // create extra icons regular size
            Log.WriteMessage("Started.");
            try
            {
                // now generate 0% - 100% colored planes
                for (int i = 0; i <= 100; i++)
                {
                    Bitmap bm = CreatePlaneIcon(GetColor(1.0f - (float)i / 100.0f));
                    il_Planes_L.Images.Add(bm);
                    il_Planes_M.Images.Add(bm);
                    il_Planes_H.Images.Add(bm);
                    il_Planes_S.Images.Add(bm);
                }
                il_Planes_L.Images.Add(CreatePlaneIcon(Color.Gray));
                il_Planes_M.Images.Add(CreatePlaneIcon(Color.Gray));
                il_Planes_H.Images.Add(CreatePlaneIcon(Color.Gray));
                il_Planes_S.Images.Add(CreatePlaneIcon(Color.Gray));
                bmindex_gray = il_Planes_M.Images.Count - 1;
                il_Planes_L.Images.Add(CreatePlaneIcon(Color.LightGreen));
                il_Planes_M.Images.Add(CreatePlaneIcon(Color.LightGreen));
                il_Planes_H.Images.Add(CreatePlaneIcon(Color.LightGreen));
                il_Planes_S.Images.Add(CreatePlaneIcon(Color.LightGreen));
                bmindex_lightgreen = il_Planes_M.Images.Count - 1;
                il_Planes_L.Images.Add(CreatePlaneIcon(Color.DarkOrange));
                il_Planes_M.Images.Add(CreatePlaneIcon(Color.DarkOrange));
                il_Planes_H.Images.Add(CreatePlaneIcon(Color.DarkOrange));
                il_Planes_S.Images.Add(CreatePlaneIcon(Color.DarkOrange));
                bmindex_darkorange = il_Planes_M.Images.Count - 1;
                il_Planes_L.Images.Add(CreatePlaneIcon(Color.Red));
                il_Planes_M.Images.Add(CreatePlaneIcon(Color.Red));
                il_Planes_H.Images.Add(CreatePlaneIcon(Color.Red));
                il_Planes_S.Images.Add(CreatePlaneIcon(Color.Red));
                bmindex_red = il_Planes_M.Images.Count - 1;
                il_Planes_L.Images.Add(CreatePlaneIcon(Color.Magenta));
                il_Planes_M.Images.Add(CreatePlaneIcon(Color.Magenta));
                il_Planes_H.Images.Add(CreatePlaneIcon(Color.Magenta));
                il_Planes_S.Images.Add(CreatePlaneIcon(Color.Magenta));
                bmindex_magenta = il_Planes_M.Images.Count - 1;
                il_Airports.Images.Add(CreateAirportIcon(255));

                // save icons to Icons directory
                il_Planes_L.Images[bmindex_gray].Save(
                        Path.Combine(IconDirectory, "plane_l_gray.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_M.Images[bmindex_gray].Save(
                        Path.Combine(IconDirectory, "plane_m_gray.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_H.Images[bmindex_gray].Save(
                        Path.Combine(IconDirectory, "plane_h_gray.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_S.Images[bmindex_gray].Save(
                        Path.Combine(IconDirectory, "plane_s_gray.png"),
                        System.Drawing.Imaging.ImageFormat.Png);

                il_Planes_L.Images[bmindex_darkorange].Save(
                        Path.Combine(IconDirectory, "plane_l_darkorange.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_M.Images[bmindex_darkorange].Save(
                        Path.Combine(IconDirectory, "plane_m_darkorange.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_H.Images[bmindex_darkorange].Save(
                        Path.Combine(IconDirectory, "plane_h_darkorange.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_S.Images[bmindex_darkorange].Save(
                        Path.Combine(IconDirectory, "plane_s_darkorange.png"),
                        System.Drawing.Imaging.ImageFormat.Png);

                il_Planes_L.Images[bmindex_red].Save(
                        Path.Combine(IconDirectory, "plane_l_red.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_M.Images[bmindex_red].Save(
                        Path.Combine(IconDirectory, "plane_m_red.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_H.Images[bmindex_red].Save(
                        Path.Combine(IconDirectory, "plane_h_red.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_S.Images[bmindex_red].Save(
                        Path.Combine(IconDirectory, "plane_s_red.png"),
                        System.Drawing.Imaging.ImageFormat.Png);

                il_Planes_L.Images[bmindex_magenta].Save(
                        Path.Combine(IconDirectory, "plane_l_magenta.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_M.Images[bmindex_magenta].Save(
                        Path.Combine(IconDirectory, "plane_m_magenta.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_H.Images[bmindex_magenta].Save(
                        Path.Combine(IconDirectory, "plane_h_magenta.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
                il_Planes_S.Images[bmindex_magenta].Save(
                        Path.Combine(IconDirectory, "plane_s_magenta.png"),
                        System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            Log.WriteMessage("Finished.");
        }

        private void InitializeCharts()
        {
            // propagation path chart
            pm_Path.Title = String.Empty;
            pm_Path.DefaultFontSize = (double)Properties.Settings.Default.Charts_FontSize;
            pm_Path.IsLegendVisible = false;
            pv_Path.BackColor = Color.White;
            pv_Path.Model = pm_Path;
            // add axes
            pm_Path.Axes.Clear();
            // add X-axis
            Path_X.IsZoomEnabled = false;
            Path_X.Maximum = 1000;
            Path_X.Minimum = 0;
            Path_X.MajorGridlineStyle = LineStyle.Solid;
            Path_X.MinorGridlineStyle = LineStyle.Dot;
            Path_X.Position = AxisPosition.Bottom;
            this.pm_Path.Axes.Add(Path_X);
            // add Y-axis
            Path_Y.IsZoomEnabled = false;
            Path_Y.Maximum = 20000;
            Path_Y.Minimum = 0;
            Path_Y.MajorGridlineStyle = LineStyle.Solid;
            Path_Y.MinorGridlineStyle = LineStyle.Dot;
            Path_Y.Position = AxisPosition.Left;
            this.pm_Path.Axes.Add(Path_Y);
            // add series
            pm_Path.Series.Clear();
            Path_Elevation.Title = "Elevation";
            Min_H1.Title = "Min_H1";
            Min_H1.StrokeThickness = 2;
            Min_H1.LineStyle = LineStyle.Solid;
            Min_H1.Color = OxyColors.Red;
            Min_H2.Title = "Min_H2";
            Min_H2.StrokeThickness = 2;
            Min_H2.LineStyle = LineStyle.Solid;
            Min_H2.Color = OxyColors.Gold;
            Max_H.Title = "Max_H";
            Max_H.StrokeThickness = 2;
            Max_H.LineStyle = LineStyle.Dot;
            Max_H.Color = OxyColors.DarkBlue;
            Min_H.Title = "Min_H";
            Min_H.StrokeThickness = 0;
            Min_H.LineStyle = LineStyle.Solid;
            Min_H.Color = OxyColors.Magenta.ChangeSaturation(0.4);
            Planes_Hi.Title = "Planes_Hi";
            Planes_Hi.Color = OxyColors.Transparent;
            Planes_Hi.MarkerType = MarkerType.Square;
            Planes_Hi.MarkerFill = OxyColors.Magenta;
            Planes_Lo.Title = "Planes_Lo";
            Planes_Lo.Color = OxyColors.Transparent;
            Planes_Lo.MarkerType = MarkerType.Square;
            Planes_Lo.MarkerFill = OxyColors.Gray;
            pm_Path.Series.Add(Path_Elevation);
            pm_Path.Series.Add(Min_H1);
            pm_Path.Series.Add(Min_H2);
            pm_Path.Series.Add(Max_H);
            pm_Path.Series.Add(Min_H);
            pm_Path.Series.Add(Planes_Hi);
            pm_Path.Series.Add(Planes_Lo);
            // add legend
            pm_Path.LegendTitle = "";
            pm_Path.LegendPosition = LegendPosition.TopRight;
            pm_Path.LegendBackground = OxyColors.White;
            pm_Path.LegendBorder = OxyColors.Black;
            pm_Path.LegendBorderThickness = 1;
            // add control
            this.tp_Elevation.Controls.Add(pv_Path);
            pv_Path.Paint += new PaintEventHandler(pv_Path_Paint);

            // zoomed elevation chart
            pm_Elevation.Title = String.Empty;
            pm_Elevation.DefaultFontSize = (double)Properties.Settings.Default.Charts_FontSize;
            pm_Elevation.IsLegendVisible = false;
            pv_Elevation.BackColor = Color.White;
            pv_Elevation.Model = pm_Elevation;
            // add series
            pm_Elevation.Series.Clear();
            Elevation.Title = "Elevation";
            LOS.Title = "LOS";
            LOS.StrokeThickness = 2;
            LOS.Color = OxyColors.Black;
            pm_Elevation.Series.Add(Elevation);
            pm_Elevation.Series.Add(LOS);
            // create axes
            pm_Elevation.Axes.Clear();
            // add X-axis
            Elevation_X.IsZoomEnabled = false;
            Elevation_X.Maximum = 1000;
            Elevation_X.Minimum = 0;
            Elevation_X.MajorGridlineStyle = LineStyle.Solid;
            Elevation_X.MinorGridlineStyle = LineStyle.Dot;
            Elevation_X.Position = AxisPosition.Bottom;
            this.pm_Elevation.Axes.Add(Elevation_X);
            // add Y-axis
            Elevation_Y.IsZoomEnabled = false;
            // auto size maximum
            //                    Elevation_Y.Maximum = maxelv,
            Elevation_Y.Minimum = 0;
            Elevation_Y.MajorGridlineStyle = LineStyle.Solid;
            Elevation_Y.MinorGridlineStyle = LineStyle.Dot;
            Elevation_Y.Position = AxisPosition.Left;
            this.pm_Elevation.Axes.Add(Elevation_Y);
            // add legend
            pm_Elevation.LegendTitle = "";
            pm_Elevation.LegendPosition = LegendPosition.TopRight;
            pm_Elevation.LegendBackground = OxyColors.White;
            pm_Elevation.LegendBorder = OxyColors.Black;
            pm_Elevation.LegendBorderThickness = 1;
            // add control
            this.tp_Elevation.Controls.Add(pv_Elevation);

            // spectrum chart
            pm_Spectrum.Title = String.Empty;
            pm_Spectrum.DefaultFontSize = (double)Properties.Settings.Default.Charts_FontSize;
            pv_Spectrum.BackColor = Color.White;
            pv_Spectrum.Model = pm_Spectrum;
            // add Spectrum series
            pm_Spectrum.Series.Clear();
            // create axes
            pm_Spectrum.Axes.Clear();
            // add X-axis
            Spectrum_X.IsZoomEnabled = false;
            Spectrum_X.Maximum = SpectrumMaxPoints;
            Spectrum_X.Minimum = 0;
            Spectrum_X.MajorGridlineStyle = LineStyle.Solid;
            Spectrum_X.MinorGridlineStyle = LineStyle.Dot;
            Spectrum_X.Position = AxisPosition.Bottom;
            this.pm_Spectrum.Axes.Add(Spectrum_X);
            // add Y-axis
            Spectrum_Y.IsZoomEnabled = false;
            Spectrum_Y.Maximum = 0;
            Spectrum_Y.Minimum = -120;
            Spectrum_Y.MajorGridlineStyle = LineStyle.Solid;
            Spectrum_Y.MinorGridlineStyle = LineStyle.Dot;
            Spectrum_Y.Position = AxisPosition.Left;
            this.pm_Spectrum.Axes.Add(Spectrum_Y);
            // add series
            SpectrumRecord.Color = OxyColors.Magenta;
            pm_Spectrum.Series.Add(SpectrumRecord);
            Spectrum.InterpolationAlgorithm = OxyPlot.InterpolationAlgorithms.CanonicalSpline;
            Spectrum.StrokeThickness = 3;
            Spectrum.Color = OxyColors.Goldenrod;
            pm_Spectrum.Series.Add(Spectrum);
            // add control
            this.pv_Spectrum.Dock = DockStyle.Fill;
            this.gb_Spectrum.Controls.Add(pv_Spectrum);
            return;

        }

        private void InitializeWebbrowser()
        {
            // do not initialize webbrowser --> not working on all Linux systems
            if (SupportFunctions.IsMono)
                return;
            // iniitialize webbrowser on Windows
            this.wb_News = new System.Windows.Forms.WebBrowser();
            // 
            // wb_News
            // 
            this.wb_News.DataBindings.Add(new System.Windows.Forms.Binding("Url", global::AirScout.Properties.Settings.Default, "News_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.wb_News.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb_News.Location = new System.Drawing.Point(0, 0);
            this.wb_News.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb_News.Name = "wb_News";
            this.wb_News.Size = new System.Drawing.Size(844, 197);
            this.wb_News.TabIndex = 0;
            this.wb_News.ScriptErrorsSuppressed = true;
            this.wb_News.Url = global::AirScout.Properties.Settings.Default.News_URL;
            this.tp_News.Controls.Add(this.wb_News);
        }

        private void UpdateAirports()
        {
            // clear airports first
            gmo_Airports.Clear();
            if (!Properties.Settings.Default.Airports_Activate)
                return;
            if ((Airports == null) || (Airports.Count == 0))
                return;
            foreach (AirportDesignator airport in Airports)
            {
                try
                {
                    GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(airport.Lat, airport.Lon), ToolTipFont, RotateImageByAngle(il_Airports.Images[0], 0));
                    gm.ToolTipText = airport.Airport + "\n" +
                        airport.IATA + "/" + airport.ICAO;
                    gm.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    gm.Tag = airport.IATA + "," + airport.ICAO;
                    gmo_Airports.Markers.Add(gm);
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            gm_Main.Refresh();
        }

        private void ti_Startup_Tick(object sender, EventArgs e)
        {
            FinishStartup();
            ti_Startup.Stop();
        }


        #endregion

        #region User Settings

        private string GetUserSettingsPath()
        {
            if (!SupportFunctions.IsMono)
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            // try to build a path to user specific settings under Linux/Mono
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
            return usersettingspath;
        }

        private void LoadSettingsFromJSON(ApplicationSettingsBase settings)
        {
            string filename = GetUserSettingsPath().Replace("user.config", settings.GetType().FullName + ".json");

            if (!File.Exists(filename))
                return;
            JsonSerializerSettings serializersettings = new JsonSerializerSettings();
            Dictionary<string, string> props =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filename));
            foreach (string key in props.Keys)
            {
                try
                {
                    settings[key] = JsonConvert.DeserializeObject(props[key], settings.Properties[key].PropertyType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while loading user setting: " + ex.ToString());
                }
            }
        }

        private void LoadUserSettings()
        {
            try
            {
                Console.WriteLine("Loading user settings...");

                if (!SupportFunctions.IsMono)
                {
                    // use Windows standard Properties.Settings.Default behavoir
                    ScoutBase.Elevation.Properties.Settings.Default.Reload();
                    ScoutBase.Stations.Properties.Settings.Default.Reload();
                    ScoutBase.Propagation.Properties.Settings.Default.Reload();
                    ScoutBase.CAT.Properties.Settings.Default.Reload();
                    AirScout.Aircrafts.Properties.Settings.Default.Reload();
                    AirScout.PlaneFeeds.Properties.Settings.Default.Reload();
                    AirScout.CAT.Properties.Settings.Default.Reload();
                    Properties.Settings.Default.Reload();

                    return;
                }

                // Mono hack to assure that default values were initilaized
                ScoutBase.Elevation.Properties.Settings.Default.Reset();
                ScoutBase.Stations.Properties.Settings.Default.Reset();
                ScoutBase.Propagation.Properties.Settings.Default.Reset();
                ScoutBase.CAT.Properties.Settings.Default.Reset();
                AirScout.Aircrafts.Properties.Settings.Default.Reset();
                AirScout.PlaneFeeds.Properties.Settings.Default.Reset();
                AirScout.CAT.Properties.Settings.Default.Reset();
                Properties.Settings.Default.Reset();

                // Rather load settings as JSON
                LoadSettingsFromJSON(ScoutBase.Elevation.Properties.Settings.Default);
                LoadSettingsFromJSON(ScoutBase.Stations.Properties.Settings.Default);
                LoadSettingsFromJSON(ScoutBase.Propagation.Properties.Settings.Default);
                LoadSettingsFromJSON(ScoutBase.CAT.Properties.Settings.Default);
                LoadSettingsFromJSON(AirScout.Aircrafts.Properties.Settings.Default);
                LoadSettingsFromJSON(AirScout.PlaneFeeds.Properties.Settings.Default);
                LoadSettingsFromJSON(AirScout.CAT.Properties.Settings.Default);
                LoadSettingsFromJSON(Properties.Settings.Default);

                Console.WriteLine("Loading user settings finished successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while loading user settings: " + ex.ToString(), LogLevel.Error);
            }
        }

        private void SaveSettingsToJSON(ApplicationSettingsBase settings)
        {
            string filename = GetUserSettingsPath().Replace("user.config", settings.GetType().FullName + ".json");

            Dictionary<string, object> props = new Dictionary<string, object>();
            foreach (SettingsProperty prop in settings.Properties)
            {
                props.Add(prop.Name, JsonConvert.SerializeObject(settings[prop.Name]));
            }
            File.WriteAllText(filename, JsonConvert.SerializeObject(props, Newtonsoft.Json.Formatting.Indented));
        }

        private void SaveUserSettings()
        {
            try
            {
                Console.WriteLine("Saving configuration, FirstRun = " + Properties.Settings.Default.FirstRun);
                Log.WriteMessage("Saving configuration...");

                if (!SupportFunctions.IsMono)
                {
                    // save all settings
                    ScoutBase.CAT.Properties.Settings.Default.Save();
                    ScoutBase.Elevation.Properties.Settings.Default.Save();
                    ScoutBase.Stations.Properties.Settings.Default.Save();
                    ScoutBase.Propagation.Properties.Settings.Default.Save();
                    AirScout.Aircrafts.Properties.Settings.Default.Save();
                    AirScout.CAT.Properties.Settings.Default.Save();
                    Properties.Settings.Default.Save();

                    return;
                }

                // Rather save settings as JSON
                SaveSettingsToJSON(Properties.Settings.Default);
                SaveSettingsToJSON(AirScout.Aircrafts.Properties.Settings.Default);
                SaveSettingsToJSON(AirScout.CAT.Properties.Settings.Default);
                SaveSettingsToJSON(ScoutBase.Propagation.Properties.Settings.Default);
                SaveSettingsToJSON(ScoutBase.Stations.Properties.Settings.Default);
                SaveSettingsToJSON(ScoutBase.Elevation.Properties.Settings.Default);
                SaveSettingsToJSON(ScoutBase.CAT.Properties.Settings.Default);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save settings: " + ex.ToString());
            }
        }

        #endregion

        #region Idle

        private void OnIdle(object sender, EventArgs args)
        {
        }

        #endregion

        #region Closing Down

        private void MapDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Log != null)
            {
                Log.WriteMessage(Application.ProductName + " is closing.");
                // flush the Log for the first time to save all messages
                Log.FlushLog();
            }

            // stop playing when in PLAY mode
            if (PlayMode != AIRSCOUTPLAYMODE.PAUSE)
                Pause();

            //save window size, state and location
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.General_WindowSize = this.Size;
                Properties.Settings.Default.General_WindowLocation = this.Location;
            }
            else
            {
                Properties.Settings.Default.General_WindowSize = this.RestoreBounds.Size;
                Properties.Settings.Default.General_WindowLocation = this.RestoreBounds.Location;
            }
            Properties.Settings.Default.General_WindowState = this.WindowState;

            // save properties to file
            SaveUserSettings();

            Say("Waiting for background threads to close...");
            // close background threads, save database and settings
            try
            {
                // cancel background workers
                // causes ThreadAbortExceptions on Linux ?!?
                try
                {
                    CancelAllBackgroundWorkers();
                }
                catch (Exception ex)
                {

                }
                // save splitter positions
                Properties.Settings.Default.MainSplitter_Distance = MainSplitterDistance;
                Properties.Settings.Default.MapSplitter_Distance = MapSplitterDistance;

                // stop tracking
                if (ConnectedRig != null)
                {
                    ConnectedRig.LeaveDoppler();
                }
                TrackMode = AIRSCOUTTRACKMODE.NONE;
                
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            finally
            {
                // save InMemory databases if any
                if (StationData.Database.IsInMemory())
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving station database...");
                    StationData.Database.BackupDatabase();
                    st.Stop();
                    Log.WriteMessage("Station database saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
                if (PropagationData.Database.IsInMemory(ELEVATIONMODEL.GLOBE))
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving GLOBE database...");
                    PropagationData.Database.BackupDatabase(ELEVATIONMODEL.GLOBE);
                    st.Stop();
                    Log.WriteMessage("Propagation database GLOBE saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
                if (PropagationData.Database.IsInMemory(ELEVATIONMODEL.SRTM3))
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving SRTM3 database...");
                    PropagationData.Database.BackupDatabase(ELEVATIONMODEL.SRTM3);
                    st.Stop();
                    Log.WriteMessage("Propagation database SRTM3 saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
                if (PropagationData.Database.IsInMemory(ELEVATIONMODEL.SRTM1))
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving SRTM1 database...");
                    PropagationData.Database.BackupDatabase(ELEVATIONMODEL.SRTM1);
                    st.Stop();
                    Log.WriteMessage("Propagation database SRTM1 saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
                if (PropagationData.Database.IsInMemory(ELEVATIONMODEL.ASTER3))
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving ASTER3 database...");
                    PropagationData.Database.BackupDatabase(ELEVATIONMODEL.ASTER3);
                    st.Stop();
                    Log.WriteMessage("Propagation database ASTER3 saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
                if (PropagationData.Database.IsInMemory(ELEVATIONMODEL.ASTER1))
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving ASTER1 database...");
                    PropagationData.Database.BackupDatabase(ELEVATIONMODEL.ASTER1);
                    st.Stop();
                    Log.WriteMessage("Propagation database ASTER1 saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }
                if (AircraftData.Database.IsInMemory())
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    SayDatabase("Saving aircraft database...");
                    AircraftData.Database.BackupDatabase();
                    st.Stop();
                    Log.WriteMessage("Aircraft database saved, " + st.ElapsedMilliseconds.ToString() + " ms.");
                }

                // flush the Log again in case of any exception to save all messages
                Log.FlushLog();
            }
        }

        private void MapDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.WriteMessage(Application.ProductName + " is closed.");
            // flush the Log for the first time to save all messages
            Log.FlushLog();
        }

        #endregion


        #region Service Functions

        private void Say(string text)
        {
            try
            {
                if (String.Compare(tsl_Status.Text, text) == 0)
                    return;
                tsl_Status.Text = text;
            }
            catch (Exception ex)
            {

            }
        }

        private void SayDatabase(string text)
        {
            try
            {
                if (String.Compare(tsl_Database.Text, text) == 0)
                    return;
                tsl_Database.Text = text;
            }
            catch (Exception ex)
            {

            }
        }

        private void SayCalculations(string text)
        {
            try
            {
                if (String.Compare(tsl_Calculations.Text, text) == 0)
                    return;
                tsl_Calculations.Text = text;
            }
            catch (Exception ex)
            {

            }
        }

        private void SayAnalysis(string text)
        {
            try
            {
                if (String.Compare(tb_Analysis_Status.Text, text) == 0)
                    return;
                tb_Analysis_Status.Text = text;
                tb_Analysis_Status.Refresh();
            }
            catch (Exception ex)
            {

            }
        }

        private void SayTrack(string text, Color forecolor, Color backcolor)
        {
            try
            {
                if (tsl_Track.Text != text)
                {
                    tsl_Track.Text = text;
                }
                if (tsl_Track.ForeColor != forecolor)
                {
                    tsl_Track.ForeColor = forecolor;
                }
                if (tsl_Track.BackColor != backcolor)
                {
                    tsl_Track.BackColor = backcolor;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void SayRot(string text, Color forecolor, Color backcolor)
        {
            try
            {
                if (tsl_Rot.Text != text)
                {
                    tsl_Rot.Text = text;
                }
                if (tsl_Rot.ForeColor != forecolor)
                {
                    tsl_Rot.ForeColor = forecolor;
                }
                if (tsl_Rot.BackColor != backcolor)
                {
                    tsl_Rot.BackColor = backcolor;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void SayCAT(string text, Color forecolor, Color backcolor)
        {
            try
            {
                if (tsl_CAT.Text != text)
                {
                    tsl_CAT.Text = text;
                }
                if (tsl_CAT.ForeColor != forecolor)
                {
                    tsl_CAT.ForeColor = forecolor;
                }
                if (tsl_CAT.BackColor != backcolor)
                {
                    tsl_CAT.BackColor = backcolor;
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }
        
        private void UpdateStatus()
        {
            // upddate TextBoxes
            tb_UTC.Text = CurrentTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (Properties.Settings.Default.Time_Mode_Online)
                tb_UTC.BackColor = Color.LightSalmon;
            else
                tb_UTC.BackColor = Color.Plum;
            string call = Properties.Settings.Default.MyCall;
            cb_MyCall.SilentText = Properties.Settings.Default.MyCall;
            cb_MyLoc.SilentText = MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon,
                Properties.Settings.Default.Locator_SmallLettersForSubsquares,
                (int)Properties.Settings.Default.Locator_MaxLength / 2,
                Properties.Settings.Default.Locator_AutoLength);
            cb_DXCall.Text = Properties.Settings.Default.DXCall;
            cb_DXLoc.SilentText = MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat,
                Properties.Settings.Default.DXLon,
                Properties.Settings.Default.Locator_SmallLettersForSubsquares,
                (int)Properties.Settings.Default.Locator_MaxLength / 2,
                Properties.Settings.Default.Locator_AutoLength);
            if (MaidenheadLocator.Check(cb_MyLoc.Text) && MaidenheadLocator.Check(cb_DXLoc.Text))
            {
                tb_QTF.Text = Math.Round(LatLon.Bearing(Properties.Settings.Default.MyLat,
                    Properties.Settings.Default.MyLon,
                    Properties.Settings.Default.DXLat,
                    Properties.Settings.Default.DXLon)).ToString("F0");
                tb_QRB.Text = Math.Round(LatLon.Distance(Properties.Settings.Default.MyLat,
                    Properties.Settings.Default.MyLon,
                    Properties.Settings.Default.DXLat,
                    Properties.Settings.Default.DXLon)).ToString("F0");
            }
            else
            {
                tb_QRB.Text = "0";
                tb_QTF.Text = "0";
            }
            // colour Textbox if more precise lat/lon information is available
            if (MaidenheadLocator.IsPrecise(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, 3))
            {
                cb_MyLoc.BackColor = Color.PaleGreen;
            }
            else
            {
                cb_MyLoc.BackColor = Color.FloralWhite;
            }
            // colour Textbox if more precise lat/lon information is available
            if (MaidenheadLocator.IsPrecise(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, 3))
            {
                cb_DXLoc.BackColor = Color.PaleGreen;
            }
            else
            {
                cb_DXLoc.BackColor = Color.FloralWhite;
            }
            cb_Band.SelectedItem = Bands.GetStringValue(Properties.Settings.Default.Band);
        }

        private void Alarm(string msg)
        {
            if (Properties.Settings.Default.Alarm_Activate)
            {
                gb_Map_Alarms.BackColor = Color.Plum;
                if (Properties.Settings.Default.Alarm_BringWindowToFront)
                {
                    // try different methods to bring the window to front under WinXP and Win7
                    this.TopMost = true;
                    SetForegroundWindow(this.Handle);
                    // restore window size, state and location
                    try
                    {
                        this.WindowState = Properties.Settings.Default.General_WindowState;
                        this.Size = Properties.Settings.Default.General_WindowSize;
                        this.Location = Properties.Settings.Default.General_WindowLocation;
                    }
                    catch (Exception ex)
                    {
                        // do nothing if failed
                        Log.WriteMessage(ex.ToString(), LogLevel.Error);
                    }
                    this.BringToFront();
                    this.Activate();
                    this.TopMost = false;
                }
                if (Properties.Settings.Default.Alarm_PlaySound)
                    System.Media.SystemSounds.Beep.Play();
            }
        }

        private void MapSave()
        {
            Log.WriteMessage("Started.");
            try
            {
                Bitmap bmp = new Bitmap(this.Width, this.Height);
                this.DrawToBitmap(bmp, new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), this.Size));
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                    {
                        bmp.Save(TmpDirectory + Path.DirectorySeparatorChar + Properties.Settings.Default.Band + "_" + Properties.Settings.Default.MyCall.Replace("/", "_") + "_" + Properties.Settings.Default.DXCall.Replace("/", "_") + "_" + CurrentTime.ToString("yyyyMMdd") + "_" + CurrentTime.ToString("HHmmss") + ".jpg", codec, encoderParameters);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            Log.WriteMessage("Finished.");
        }


        private void cb_Alarms_Activate_CheckedChanged(object sender, EventArgs e)
        {
            if (!cb_Alarms_Activate.Checked)
                gb_Map_Alarms.BackColor = SystemColors.Control;
        }

        private void tb_UTC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (PlayMode != AIRSCOUTPLAYMODE.PAUSE)
                return;
            SetTimeDlg Dlg = new SetTimeDlg();
            Dlg.cb_Time_Online.Checked = Properties.Settings.Default.Time_Mode_Online;
            Dlg.dtp_SetTimeDlg_Start.Value = Properties.Settings.Default.Time_Offline;
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Time_Offline = Dlg.dtp_SetTimeDlg_Start.Value;
                Properties.Settings.Default.Time_Mode_Online = Dlg.cb_Time_Online.Checked;
                UpdateStatus();
            }
        }

        private void UpdateLocation(string call, double lat, double lon, GEOSOURCE source)
        {
            // update callsign database with new lat/lon info
            if (Callsign.Check(call))
                StationData.Database.LocationInsertOrUpdateIfNewer(new LocationDesignator(call, lat, lon, source));
        }

        public LocationDesignator LocationFindOrCreate(string call, string loc)
        {
            // check all parameters
            if (!Callsign.Check(call))
                return null;
            if (!MaidenheadLocator.Check(loc))
                return null;
            // get location info
            LocationDesignator ld = StationData.Database.LocationFindOrCreate(call, loc);
            // get elevation
            ld.Elevation = GetElevation(ld.Lat, ld.Lon);
            ld.BestCaseElevation = false;
            // modify location in case of best case elevation is selected --> but do not store in database or settings!
            if (Properties.Settings.Default.Path_BestCaseElevation)
            {
                if (!MaidenheadLocator.IsPrecise(ld.Lat, ld.Lon, 3))
                {
                    ElvMinMaxInfo maxinfo = GetMinMaxElevationLoc(ld.Loc);
                    if (maxinfo.MaxElv != ElevationData.Database.ElvMissingFlag)
                    {
                        ld.Lat = maxinfo.MaxLat;
                        ld.Lon = maxinfo.MaxLon;
                        ld.Elevation = maxinfo.MaxElv;
                        ld.BestCaseElevation = true;
                    }
                }
            }
            return ld;
        }

        public LocationDesignator LocationFindOrUpdateOrCreate(string call, double lat, double lon)
        {
            // check all parameters
            if (!Callsign.Check(call))
                return null;
            if (!GeographicalPoint.Check(lat, lon))
                return null;
            // get location info
            LocationDesignator ld = StationData.Database.LocationFindOrUpdateOrCreate(call, lat, lon);
            // get elevation
            ld.Elevation = GetElevation(ld.Lat, ld.Lon);
            ld.BestCaseElevation = false;
            // modify location in case of best case elevation is selected --> but do not store in database or settings!
            if (Properties.Settings.Default.Path_BestCaseElevation)
            {
                if (!MaidenheadLocator.IsPrecise(ld.Lat, ld.Lon, 3))
                {
                    ElvMinMaxInfo maxinfo = GetMinMaxElevationLoc(ld.Loc);
                    if (maxinfo.MaxElv != ElevationData.Database.ElvMissingFlag)
                    {
                        ld.Lat = maxinfo.MaxLat;
                        ld.Lon = maxinfo.MaxLon;
                        ld.Elevation = maxinfo.MaxElv;
                        ld.BestCaseElevation = true;
                    }
                }
            }
            return ld;
        }

        public LocationDesignator LocationFind(string call, string loc = "")
        {
            // check all parameters
            if (!Callsign.Check(call))
                return null;
            if (!String.IsNullOrEmpty(loc) && !MaidenheadLocator.Check(loc))
                return null;
            // get location info
            LocationDesignator ld = (String.IsNullOrEmpty(loc)) ? StationData.Database.LocationFind(call) : StationData.Database.LocationFind(call, loc);
            // return null if not found
            if (ld == null)
                return null;
            // get elevation
            ld.Elevation = GetElevation(ld.Lat, ld.Lon);
            ld.BestCaseElevation = false;
            // modify location in case of best case elevation is selected --> but do not store in database or settings!
            if (Properties.Settings.Default.Path_BestCaseElevation)
            {
                if (!MaidenheadLocator.IsPrecise(ld.Lat, ld.Lon, 3))
                {
                    ElvMinMaxInfo maxinfo = GetMinMaxElevationLoc(ld.Loc);
                    if (maxinfo.MaxElv != ElevationData.Database.ElvMissingFlag)
                    {
                        ld.Lat = maxinfo.MaxLat;
                        ld.Lon = maxinfo.MaxLon;
                        ld.Elevation = maxinfo.MaxElv;
                        ld.BestCaseElevation = true;
                    }
                }
            }
            return ld;
        }

        public short GetElevation(string loc)
        {
            return GetElevation(MaidenheadLocator.LatFromLoc(loc), MaidenheadLocator.LonFromLoc(loc));
        }

        public short GetElevation(double lat, double lon)
        {
            if (!GeographicalPoint.Check(lat, lon))
                return 0;
            short elv = ElevationData.Database.ElvMissingFlag;
            // try to get elevation data from distinct elevation model
            // start with detailed one
            if (Properties.Settings.Default.Elevation_ASTER1_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.ASTER1, false];
            if (Properties.Settings.Default.Elevation_ASTER3_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.ASTER3, false];
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.SRTM1, false];
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.SRTM3, false];
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.GLOBE, false];
            // set it to zero if still invalid
            if (elv <= ElevationData.Database.TileMissingFlag)
                elv = 0;
            return elv;
        }

        public ElvMinMaxInfo GetMinMaxElevationLoc(string loc)
        {
            ElvMinMaxInfo elv = new ElvMinMaxInfo();
            // try to get elevation data from distinct elevation model
            // start with detailed one
            if (Properties.Settings.Default.Elevation_ASTER1_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.ASTER1, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            if (Properties.Settings.Default.Elevation_ASTER3_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.ASTER3, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.SRTM1, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.SRTM3, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (elv.MaxElv == ElevationData.Database.ElvMissingFlag))
            {
                ElvMinMaxInfo info = ElevationData.Database.GetMaxElvLoc(loc, ELEVATIONMODEL.GLOBE, false);
                if (info != null)
                {
                    elv.MaxLat = info.MaxLat;
                    elv.MaxLon = info.MaxLon;
                    elv.MaxElv = info.MaxElv;
                    elv.MinLat = info.MinLat;
                    elv.MinLon = info.MinLon;
                    elv.MinElv = info.MinElv;
                }
            }
            /*
            // set it to zero if still invalid
            if (elv.MaxElv == ElevationData.Database.ElvMissingFlag)
                elv.MaxElv = 0;
            if (elv.MinElv == ElevationData.Database.ElvMissingFlag)
                elv.MinElv = 0;
            */
            return elv;
        }

        public void SetElevationModel()
        {
            if (Properties.Settings.Default.Elevation_ASTER1_Enabled)
                Properties.Settings.Default.ElevationModel = ELEVATIONMODEL.ASTER1;
            else if (Properties.Settings.Default.Elevation_ASTER3_Enabled)
                Properties.Settings.Default.ElevationModel = ELEVATIONMODEL.ASTER3;
            else if (Properties.Settings.Default.Elevation_SRTM1_Enabled)
                Properties.Settings.Default.ElevationModel = ELEVATIONMODEL.SRTM1;
            else if (Properties.Settings.Default.Elevation_SRTM3_Enabled)
                Properties.Settings.Default.ElevationModel = ELEVATIONMODEL.SRTM3;
            else if (Properties.Settings.Default.Elevation_GLOBE_Enabled)
                Properties.Settings.Default.ElevationModel = ELEVATIONMODEL.GLOBE;
            else
                Properties.Settings.Default.ElevationModel = ELEVATIONMODEL.NONE;
        }

        public static Font CreateFontFromString(string font)
        {
            try
            {
                string[] a = Properties.Settings.Default.Map_ToolTipFont.Split(';');
                string fontfamily = a[0].Trim();
                float emsize = 0;
                float.TryParse(a[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out emsize);
                FontStyle fontstyle = 0;
                // check if any additional font style is given
                if (a.Length > 2)
                {
                    if (a[2].ToUpper().IndexOf("BOLD") >= 0)
                        fontstyle = fontstyle | FontStyle.Bold;
                    if (a[2].ToUpper().IndexOf("ITALIC") >= 0)
                        fontstyle = fontstyle | FontStyle.Italic;
                    if (a[2].ToUpper().IndexOf("UNDERLINE") >= 0)
                        fontstyle = fontstyle | FontStyle.Underline;
                    if (a[2].ToUpper().IndexOf("STRIKEOUT") >= 0)
                        fontstyle = fontstyle | FontStyle.Strikeout;
                }
                else
                {
                    fontstyle = FontStyle.Regular;
                }
                return new Font(fontfamily, emsize, fontstyle, GraphicsUnit.Point);
            }
            catch
            {

            }
            return null;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        enum ShowWindowCommands : int
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>      
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except
            /// the window is not activated.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position.
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
            /// that owns the window is not responding. This flag should only be
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }

        private void ShowOptionsDlg()
        {
            // disable buttons
            btn_Map_PlayPause.Enabled = false;
            btn_Map_Save.Enabled = false;
            btn_Options.Enabled = false;

            // stop background threads
            Say("Waiting for background threads to close....");
            StopAllBackgroundWorkers();
            // save current settings
            SaveUserSettings();
            // show options dialog
            OptionsDlg Dlg = new OptionsDlg(this);
            Say("Options");
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // save current settings
                SaveUserSettings();
                // enbale/disable manage watchlist button
                btn_Control_Manage_Watchlist.Enabled = !Properties.Settings.Default.Watchlist_SyncWithKST || !Properties.Settings.Default.Server_Activate;
                // Re-initialze charts
                InitializeCharts();
                // clear paths cache assuming that new options were set
                ElevationPaths.Clear();
                PropagationPaths.Clear();
                // check and update station infos
                LocationDesignator ld = StationData.Database.LocationFind(Properties.Settings.Default.MyCall, MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, false,3));
                if ((ld == null) || (ld.Lat != Properties.Settings.Default.MyLat) || (ld.Lon != Properties.Settings.Default.MyLon))
                {
                    UpdateLocation(Properties.Settings.Default.MyCall,
                        Properties.Settings.Default.MyLat,
                        Properties.Settings.Default.MyLon,
                        MaidenheadLocator.IsPrecise(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC);
                }
                ld = StationData.Database.LocationFind(Properties.Settings.Default.DXCall, MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3));
                if ((ld == null) || (ld.Lat != Properties.Settings.Default.DXLat) || (ld.Lon != Properties.Settings.Default.DXLon))
                {
                    UpdateLocation(Properties.Settings.Default.DXCall,
                        Properties.Settings.Default.DXLat,
                        Properties.Settings.Default.DXLon,
                        MaidenheadLocator.IsPrecise(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC);
                }
                // update map provider
                gm_Main.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);

                // update ToolTipFont
                ToolTipFont = CreateFontFromString(Properties.Settings.Default.Map_ToolTipFont);

                // update planefeeds
                bw_PlaneFeed1 = null;
                bw_PlaneFeed2 = null;
                bw_PlaneFeed3 = null;

                if (PlaneFeedPlugins != null)
                {
                    foreach (IPlaneFeedPlugin plugin in PlaneFeedPlugins)
                    {
                        if (Properties.Settings.Default.Planes_PlaneFeed1 == plugin.Name)
                        {
                            bw_PlaneFeed1 = new PlaneFeed();
                            bw_PlaneFeed1.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                        if (Properties.Settings.Default.Planes_PlaneFeed2 == plugin.Name)
                        {
                            bw_PlaneFeed2 = new PlaneFeed();
                            bw_PlaneFeed2.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                        if (Properties.Settings.Default.Planes_PlaneFeed3 == plugin.Name)
                        {
                            bw_PlaneFeed3 = new PlaneFeed();
                            bw_PlaneFeed3.ProgressChanged += new ProgressChangedEventHandler(bw_PlaneFeed_ProgressChanged);
                        }
                    }
                }
                // update timer interval
                ti_Progress.Interval = Properties.Settings.Default.Map_Update * 1000;
                // update background update intervals
                ScoutBase.Elevation.Properties.Settings.Default.Datatbase_BackgroundUpdate_Period = (int)Properties.Settings.Default.Background_Update_Period;
                ScoutBase.Stations.Properties.Settings.Default.Database_BackgroundUpdate_Period = (int)Properties.Settings.Default.Background_Update_Period;
                AirScout.Aircrafts.Properties.Settings.Default.Database_BackgroundUpdate_Period = (int)Properties.Settings.Default.Background_Update_Period;
                // update database path path and elevation model
                InitializeDatabase();

                // resize map window
                gm_Main_SizeChanged(this, null);
            }
            else
            {
                // nothing was changed --> reload settings
                LoadUserSettings();
            }

            // (re)initialize maps
            InitializeMaps();

            // start permanent background workers
            StartAllBackgroundWorkers();

            // enable buttons
            btn_Map_PlayPause.Enabled = true;
            btn_Map_Save.Enabled = true;
            btn_Options.Enabled = true;

            // update status window
            UpdateStatus();
            UpdateAirports();
            UpdateWatchlistInMap();
            RefreshWatchlistView();

        }

        #endregion

        #region Play & Pause

        private void Play()
        {  
            PlayMode = AIRSCOUTPLAYMODE.FORWARD;
            // switch tab control according to path mode
            if (PathMode == AIRSCOUTPATHMODE.SINGLE)
                tc_Control.SelectedTab = tp_Control_Single;
            else if (PathMode == AIRSCOUTPATHMODE.MULTI)
                tc_Control.SelectedTab = tp_Control_Multi;

            // create distances, if enabled
            if (Properties.Settings.Default.Map_ShowDistances)
                CreateDistances();

            // update tab control
            tc_Control.Refresh();
            // refresh watch list
            RefreshWatchlistView();
            // update all current paths
            UpdatePaths();
            // clear spectrum
            try
            {
                Spectrum.Points.Clear();
                SpectrumPointsCount = 0;
                Spectrum_X.Reset();
                SpectrumRecord.Points.Clear();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }

            // Linux/Mono hack to display text in button instead of symbols
            if (SupportFunctions.IsMono)
            {
                btn_Map_PlayPause.Image = null;
                btn_Map_PlayPause.Text = "Pause";
                btn_Map_PlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                btn_Map_PlayPause.Font = new Font(btn_Map_PlayPause.Font, FontStyle.Bold);
            }
            else
            {
                // change button image
                btn_Map_PlayPause.Text = "";
                btn_Map_PlayPause.Image = il_Main.Images[0];
            }

            // disable controls
            cb_Band.Enabled = false;
            btn_Options.Enabled = false;
            cb_MyCall.Enabled = false;
            cb_MyLoc.Enabled = false;
            cb_DXCall.Enabled = false;
            cb_DXLoc.Enabled = false;
//            tc_Control.Enabled = false;
            pa_Planes_Filter.Enabled = false;
            gb_Analysis_Controls.Enabled = false;
            gb_Analysis_Database.Enabled = false;
            gb_Analysis_Player.Enabled = false;
            //referesh main window
            this.Refresh();
        }

        private void Pause()
        {
            PlayMode = AIRSCOUTPLAYMODE.PAUSE;

            // Linux/Mono hack to display text in button instead of symbols
            if (SupportFunctions.IsMono)
            {
                btn_Map_PlayPause.Image = null;
                btn_Map_PlayPause.Text = "Play";
                btn_Map_PlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                btn_Map_PlayPause.Font = new Font(btn_Map_PlayPause.Font, FontStyle.Bold);
            }
            else
            {
                // change button image
                btn_Map_PlayPause.Text = "";
                btn_Map_PlayPause.Image = il_Main.Images[1];
            }
            // update tab control
            tc_Control.Refresh();
            // enable controls
            cb_Band.Enabled = true;
            btn_Options.Enabled = true;
            cb_MyCall.Enabled = true;
            cb_MyLoc.Enabled = true;
            cb_DXCall.Enabled = true;
            cb_DXLoc.Enabled = true;
            tc_Control.Enabled = true;
            pa_Planes_Filter.Enabled = true;
            gb_Analysis_Controls.Enabled = true;
            gb_Analysis_Database.Enabled = true;
            gb_Analysis_Player.Enabled = true;
            tc_Main.Enabled = true;
            tc_Map.Enabled = true;
            // stop tracking
            TrackMode = AIRSCOUTTRACKMODE.NONE;
            //referesh main window
            this.Refresh();
        }

        #endregion

        #region Paths

        private double GetMinH(double max_alt, double H1, double H2)
        {
            double max = Math.Max(H1, H2);
            if (max <= max_alt)
                return max;
            return max_alt;
        }

        private void ClearAllPathsInMap()
        {
            gmo_PropagationPaths.Clear();
        }

        private void DrawPath(PropagationPathDesignator ppath)
        {
            // draws a propagation path to map
            PropagationPoint[] ppoints = new PropagationPoint[0];
            try
            {
                // get infopoints for map
                ppoints = ppath.GetInfoPoints();
                // calculate midpoint
                ScoutBase.Core.LatLon.GPoint midpoint = LatLon.MidPoint(ppath.Lat1, ppath.Lon1, ppath.Lat2, ppath.Lon2);
                GMapMarker gmmid = new GMarkerGoogle(new PointLatLng(midpoint.Lat, midpoint.Lon), ToolTipFont, ((PathMode == AIRSCOUTPATHMODE.MULTI) && Properties.Settings.Default.Map_SmallMarkers) ? GMarkerGoogleType.blue_small : GMarkerGoogleType.blue_dot);
                gmmid.ToolTipText = ppath.Location1.Call + " <> " + ppath.Location2.Call;
                gmmid.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                gmo_CallsignDetails.Markers.Add(gmmid);
                // calculate dx end
                gmm_DXLoc = new GMarkerGoogle(new PointLatLng(ppath.Lat2, ppath.Lon2), ToolTipFont, ((PathMode == AIRSCOUTPATHMODE.MULTI) && Properties.Settings.Default.Map_SmallMarkers) ? GMarkerGoogleType.yellow_small : GMarkerGoogleType.yellow_dot);
                gmm_DXLoc.ToolTipText = ppath.Location2.Call + "\n" +
                    ppath.Location2.Lat.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                    ppath.Location2.Lon.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                    ppath.Location2.Loc + "\n" +
                    GetElevation(ppath.Location2.Lat, ppath.Location2.Lon).ToString("F0") + "m\n" +
                    LatLon.Bearing(ppath.Location1.Lat, ppath.Location1.Lon, ppath.Location2.Lat, ppath.Location2.Lon).ToString("F0") + "°\n" +
                    LatLon.Distance(ppath.Location1.Lat, ppath.Location1.Lon, ppath.Location2.Lat, ppath.Location2.Lon).ToString("F0") + "km";
                if (Properties.Settings.Default.Track_Activate)
                    gmm_DXLoc.ToolTipText += "\nRight+Click to Turn Antenna";
                gmm_DXLoc.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                gmm_DXLoc.Tag = ppath.Location2.Call;
                gmo_CallsignDetails.Markers.Add(gmm_DXLoc);
                // set three small points for hot path, if one
                if (!Properties.Settings.Default.Map_SmallMarkers)
                {
                    int i1 = -1;
                    int i3 = -1;
                    for (int i = 0; i < ppoints.Length; i++)
                    {
                        if (Math.Max(ppoints[i].H1, ppoints[i].H2) < Properties.Settings.Default.Planes_MaxAlt)
                        {
                            if (i1 == -1)
                                i1 = i;
                            else i3 = i;

                        }
                    }
                    if ((i1 >= 0) && (i3 >= 0))
                    {
                        GMapMarker gmi1 = new GMarkerGoogle(new PointLatLng(ppoints[i1].Lat, ppoints[i1].Lon), GMarkerGoogleType.red_small);
                        gmo_Objects.Markers.Add(gmi1);
                        LatLon.GPoint gp = LatLon.MidPoint(ppoints[i1].Lat, ppoints[i1].Lon, ppoints[i3].Lat, ppoints[i3].Lon);
                        GMapMarker gmi2 = new GMarkerGoogle(new PointLatLng(gp.Lat, gp.Lon), GMarkerGoogleType.blue_small);
                        gmo_Objects.Markers.Add(gmi2);
                        GMapMarker gmi3 = new GMarkerGoogle(new PointLatLng(ppoints[i3].Lat, ppoints[i3].Lon), GMarkerGoogleType.yellow_small);
                        gmo_Objects.Markers.Add(gmi3);
                    }
                }
                // draw propagation path according to path status
                // valid: black
                // invalid: red
                gmr_FullPath = new GMapRoute("fullpath");
                gmr_FullPath.Stroke = (ppath.Valid) ? new Pen(Color.Black, 3) : new Pen(Color.Red, 3);
                gmo_PropagationPaths.Routes.Add(gmr_FullPath);
                gmr_NearestFull = new GMapRoute("fullpath");
                gmr_NearestFull.Stroke = (ppath.Valid) ? new Pen(Color.Black, 3) : new Pen(Color.Red, 3);
                gmo_NearestPaths.Routes.Add(gmr_NearestFull);
                foreach (PropagationPoint ppoint in ppoints)
                {
                    gmr_FullPath.Points.Add(new PointLatLng(ppoint.Lat, ppoint.Lon));
                    gmr_NearestFull.Points.Add(new PointLatLng(ppoint.Lat, ppoint.Lon));
                }
                // draw mutual visible path
                gmr_VisiblePpath = new GMapRoute("visiblepath");
                gmr_VisiblePpath.Stroke = new Pen(Color.Magenta, 3);
                gmr_NearestVisible = new GMapRoute("visiblepath");
                gmr_NearestVisible.Stroke = new Pen(Color.Magenta, 3);
                for (int i = 0; i < ppoints.Length; i++)
                {
                    if ((Math.Max(ppoints[i].H1, ppoints[i].H2) > 0) && (Math.Max(ppoints[i].H1, ppoints[i].H2) < Properties.Settings.Default.Planes_MaxAlt))
                    {
                        PointLatLng p = new PointLatLng(ppoints[i].Lat, ppoints[i].Lon);
                        gmr_VisiblePpath.Points.Add(p);
                        gmr_NearestVisible.Points.Add(p);
                    }
                }
                gmo_PropagationPaths.Routes.Add(gmr_VisiblePpath);
                gmo_NearestPaths.Routes.Add(gmr_NearestVisible);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void UpdatePaths()
        {
            // updates all current path to calculate
            try
            {
                Log.WriteMessage("UpdatePath started.");
                Stopwatch st = new Stopwatch();
                st.Start();

                // check if there are a valid home settings
                if (!Callsign.Check(Properties.Settings.Default.MyCall) ||
                    !GeographicalPoint.Check(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon))
                    return;

                // OK valid, lets continue

                // slow down background calculations
                Properties.Settings.Default.Background_Calculations_ThreadWait = 1000;

                //clear map overlays
                gmo_PropagationPaths.Clear();
                gmo_NearestPaths.Clear();
                gmo_Objects.Clear();

                // clear all planes and tooltips
                gmo_Planes.Clear();

                // clear paths
                ElevationPaths.Clear();
                PropagationPaths.Clear();

                // clear charts
                ClearCharts();

                // put call on MyCalls last recent collection if not already in
                if (Properties.Settings.Default.MyCalls.IndexOf(Properties.Settings.Default.MyCall) < 0)
                {
                    Properties.Settings.Default.MyCalls.Insert(0, Properties.Settings.Default.MyCall);
                }

                // keep the MyCalls list small
                while (Properties.Settings.Default.MyCalls.Count > 10)
                {
                    Properties.Settings.Default.MyCalls.RemoveAt(Properties.Settings.Default.MyCalls.Count - 1);
                }


                // check and update station database
                LocationDesignator myloc = LocationFindOrUpdateOrCreate(Properties.Settings.Default.MyCall, Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
                Properties.Settings.Default.MyElevation = myloc.Elevation;

                // get qrv info or create default
                QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(myloc.Call, myloc.Loc, Properties.Settings.Default.Band);
                // set qrv defaults if zero
                if (myqrv.AntennaHeight == 0)
                    myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                if (myqrv.AntennaGain == 0)
                    myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                if (myqrv.Power == 0)
                    myqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);
                // draw my end on the map
                gmm_MyLoc = new GMarkerGoogle(new PointLatLng(myloc.Lat, myloc.Lon), ToolTipFont, ((PathMode == AIRSCOUTPATHMODE.MULTI) && Properties.Settings.Default.Map_SmallMarkers) ? GMarkerGoogleType.red_small : GMarkerGoogleType.red_dot);
                gmm_MyLoc.ToolTipText = myloc.Call + "\n" +
                    myloc.Lat.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                    myloc.Lon.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                    myloc.Loc + "\n" +
                    GetElevation(myloc.Lat, myloc.Lon).ToString("F0") + "m";
                gmm_MyLoc.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                gmm_MyLoc.Tag = myloc.Call;
                gmo_Objects.Markers.Add(gmm_MyLoc);

                // do single path mode
                if (PathMode == AIRSCOUTPATHMODE.SINGLE)
                {
                    // check if there are a valid DX settings
                    if (!Callsign.Check(Properties.Settings.Default.DXCall) ||
                        !GeographicalPoint.Check(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon))
                        return;

                    // OK valid, lets continue
                    // check and update station database
                    LocationDesignator dxloc = LocationFindOrUpdateOrCreate(Properties.Settings.Default.DXCall, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon);
                    Properties.Settings.Default.DXElevation = dxloc.Elevation;

                    // get qrv info or create default
                    QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxloc.Call, dxloc.Loc, Properties.Settings.Default.Band);
                    // set qrv defaults if zero
                    if (dxqrv.AntennaHeight == 0)
                        dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                    if (dxqrv.AntennaGain == 0)
                        dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                    if (dxqrv.Power == 0)
                        dxqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);

                    // find local obstruction, if any
                    LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(myloc.Lat, myloc.Lon, Properties.Settings.Default.ElevationModel);
                    double mybearing = LatLon.Bearing(myloc.Lat, myloc.Lon, dxloc.Lat, dxloc.Lon);
                    double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;

                    // try to find elevation path in database or create new one and store
                    ElevationPathDesignator epath = ElevationData.Database.ElevationPathFindOrCreateFromLatLon(
                        null,
                        myloc.Lat,
                        myloc.Lon,
                        dxloc.Lat,
                        dxloc.Lon,
                        ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                        Properties.Settings.Default.ElevationModel);
                    // add additional info to ppath
                    epath.Location1 = myloc;
                    epath.Location2 = dxloc;
                    epath.QRV1 = myqrv;
                    epath.QRV2 = dxqrv;

                    // try to find propagation path in database or create new one and store
                    PropagationPathDesignator ppath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                        null,
                        myloc.Lat,
                        myloc.Lon,
                        GetElevation(myloc.Lat, myloc.Lon) + myqrv.AntennaHeight,
                        dxloc.Lat,
                        dxloc.Lon,
                        GetElevation(dxloc.Lat, dxloc.Lon) + dxqrv.AntennaHeight,
                        Bands.ToGHz(Properties.Settings.Default.Band),
                        LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor,
                        Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance,
                        ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                        Properties.Settings.Default.ElevationModel,
                        myobstr);

                    // add additional info to ppath
                    ppath.Location1 = myloc;
                    ppath.Location2 = dxloc;
                    ppath.QRV1 = myqrv;
                    ppath.QRV2 = dxqrv;

                    // add single path to paths list
                    ElevationPaths.Add(epath);
                    PropagationPaths.Add(ppath);
                    // put DXCall on the watchlist if not already in
                    if (Properties.Settings.Default.Watchlist.IndexOf(Properties.Settings.Default.DXCall, MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3)) < 0)
                    {
                        Properties.Settings.Default.Watchlist.Insert(0, new WatchlistItem(Properties.Settings.Default.DXCall, MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3), ppath.Distance > Properties.Settings.Default.Path_MaxLength));
                    }
                    // keep the watchlist small
                    while (Properties.Settings.Default.Watchlist.Count() > Properties.Settings.Default.Watchlist_MaxCount)
                    {
                        Properties.Settings.Default.Watchlist.RemoveAt(Properties.Settings.Default.Watchlist.Count() - 1);
                    }

                }
                else if (PathMode == AIRSCOUTPATHMODE.MULTI)
                {
                    // iterate through watchlist and add selected
                    foreach (ListViewItem item in lv_Control_Watchlist.Items)
                    {
                        // use only selected items
                        if (!item.Checked)
                            continue;
                        string call = item.Text;
                        string loc = item.SubItems[1].Text;

                        // check if call & loc are valid
                        if (!Callsign.Check(call) || !MaidenheadLocator.Check(loc))
                            continue;

                        // check and update station database
                        LocationDesignator dxloc = LocationFindOrCreate(call, loc);

                        // get qrv info or create default
                        QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxloc.Call, dxloc.Loc, Properties.Settings.Default.Band);
                        // set qrv defaults if zero
                        if (dxqrv.AntennaHeight == 0)
                            dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                        if (dxqrv.AntennaGain == 0)
                            dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                        if (dxqrv.Power == 0)
                            dxqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);

                        // find local obstruction, if any
                        LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(myloc.Lat, myloc.Lon, Properties.Settings.Default.ElevationModel);
                        double mybearing = LatLon.Bearing(myloc.Lat, myloc.Lon, dxloc.Lat, dxloc.Lon);
                        double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;

                        // try to find elevation path in database or create new one and store
                        ElevationPathDesignator epath = ElevationData.Database.ElevationPathFindOrCreateFromLatLon(
                            null,
                            myloc.Lat,
                            myloc.Lon,
                            dxloc.Lat,
                            dxloc.Lon,
                            ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                            Properties.Settings.Default.ElevationModel);
                        // try to find propagation path in database or create new one and store
                        PropagationPathDesignator ppath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                            null,
                            myloc.Lat,
                            myloc.Lon,
                            GetElevation(myloc.Lat, myloc.Lon) + myqrv.AntennaHeight,
                            dxloc.Lat,
                            dxloc.Lon,
                            GetElevation(dxloc.Lat, dxloc.Lon) + dxqrv.AntennaHeight,
                            Bands.ToGHz(Properties.Settings.Default.Band),
                            LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor,
                            Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance,
                            ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                            Properties.Settings.Default.ElevationModel,
                            myobstr);
                        // add additional info to ppath
                        ppath.Location1 = myloc;
                        ppath.Location2 = dxloc;
                        ppath.QRV1 = myqrv;
                        ppath.QRV2 = dxqrv;
                        // add path to paths list

                        ElevationPaths.Add(epath);
                        PropagationPaths.Add(ppath);
                    }
                }

                // calculate the area to show in map
                // initially set to my location
                double minlat = myloc.Lat;
                double minlon = myloc.Lon;
                double maxlat = myloc.Lat;
                double maxlon = myloc.Lon;

                double centerlat = myloc.Lat;
                double centerlon = myloc.Lon;

                // now do the drawing
                foreach (PropagationPathDesignator ppath in PropagationPaths)
                {
                    DrawPath(ppath);

                    // maintain Min/Max values
                    minlat = Math.Min(minlat, ppath.Lat2);
                    minlon = Math.Min(minlon, ppath.Lon2);
                    maxlat = Math.Max(maxlat, ppath.Lat2);
                    maxlon = Math.Max(maxlon, ppath.Lon2);
                }

                // show diagram when in SINGLE mode
                if (PathMode == AIRSCOUTPATHMODE.SINGLE)
                {
                    // both Elevationpaths & PropagationPaths should contain only one entry
                    if ((ElevationPaths.Count > 0) && (PropagationPaths.Count > 0))
                        UpdateCharts(ElevationPaths[ElevationPaths.Count - 1], PropagationPaths[PropagationPaths.Count - 1]);
                }

                // calculate center
                centerlat = LatLon.MidPoint(minlat, minlon, maxlat, maxlon).Lat;
                centerlon = LatLon.MidPoint(minlat, minlon, maxlat, maxlon).Lon;

                // ensure that whole path is visible and optionally centered
                gm_Main.SetZoomToFitRect(RectLatLng.FromLTRB(minlon, maxlat, maxlon, minlat));
                if (Properties.Settings.Default.Map_AutoCenter)
                    gm_Main.Position = new PointLatLng(centerlat, centerlon);

                // clear all selections
                SelectedPlanes.Clear();

                // update watchlist locations in map
                UpdateWatchlistInMap();

                // update status window
                UpdateStatus();

                // stop tracking
                //TrackMode = AIRSCOUTTRACKMODE.NONE;

                // speed up background calculations
                Properties.Settings.Default.Background_Calculations_ThreadWait = 0;

                st.Stop();
                Log.WriteMessage("UpdatePath finished, " + st.ElapsedMilliseconds.ToString() + "ms.");
            }
            catch (Exception ex)
            {
                Say("Error while updating path: " + ex.Message);
                Log.WriteMessage("Error while updating path: " + ex.ToString());
            }
        }

        #endregion

        #region Charts

        private void UpdateCharts(ElevationPathDesignator epath, PropagationPathDesignator ppath)
        {
            // updates the diagram area
            short[] epoints = new short[0];
            PropagationPoint[] ppoints = new PropagationPoint[0];
            try
            {
                ClearCharts();
                // adjust diagram axes
                Path_X.Maximum = ppath.Distance;
                Elevation_X.Maximum = epath.Distance;
                // get infopoints for charting
                epoints = epath.GetInfoPoints();
                ppoints = ppath.GetInfoPoints();
                // calculate epsilon for LOS
                double eps_los = Propagation.EpsilonFromHeights(GetElevation(ppath.Lat1, ppath.Lon1) + ppath.QRV1.AntennaHeight, ppath.Distance, GetElevation(ppath.Lat2, ppath.Lon2) + ppath.QRV2.AntennaHeight, LatLon.Earth.Radius);
                // fill chart
                short maxelv = short.MinValue;
                double myelev = GetElevation(ppath.Lat1, ppath.Lon1);
                for (int i = 0; i < epoints.Length; i++)
                {
                    Path_Elevation.Points.Add(new OxyPlot.DataPoint(i, epoints[i]));
                    Min_H1.Points.Add(new OxyPlot.DataPoint(i, ppoints[i].H1));
                    Min_H2.Points.Add(new OxyPlot.DataPoint(i, ppoints[i].H2));
                    Max_H.Points.Add(new OxyPlot.DataPoint(i, Properties.Settings.Default.Planes_MaxAlt));
                    Min_H.Points.Add(new OxyPlot.DataPoint(i, Properties.Settings.Default.Planes_MaxAlt));
                    Min_H.Points2.Add(new OxyPlot.DataPoint(i, GetMinH(Properties.Settings.Default.Planes_MaxAlt, Min_H1.Points[i].Y, Min_H2.Points[i].Y)));
                    LOS.Points.Add(new OxyPlot.DataPoint(i, Propagation.HeightFromEpsilon(myelev + ppath.QRV1.AntennaHeight, i, eps_los, LatLon.Earth.Radius)));
                    Elevation.Points.Add(new OxyPlot.DataPoint(i, epoints[i]));
                    if (maxelv < epoints[i])
                        maxelv = epoints[i];
                }
                // adjust Y-axis --> max elv + 10%
                Elevation_Y.Maximum = maxelv + maxelv * 0.1;
                // invalidate plots
                pm_Path.InvalidatePlot(true);
                pm_Elevation.InvalidatePlot(true);
                // show path legends
                Charts_ShowLegends(5000);
                // refresh path info
                tp_Elevation.Text = "Pathinfo ";
                if (Properties.Settings.Default.ElevationModel == ELEVATIONMODEL.ASTER1)
                    tp_Elevation.Text = tp_Elevation.Text + "[ASTER1]";
                else if (Properties.Settings.Default.ElevationModel == ELEVATIONMODEL.ASTER3)
                    tp_Elevation.Text = tp_Elevation.Text + "[ASTER3]";
                else if (Properties.Settings.Default.ElevationModel == ELEVATIONMODEL.SRTM1)
                    tp_Elevation.Text = tp_Elevation.Text + "[SRTM1]";
                else if (Properties.Settings.Default.ElevationModel == ELEVATIONMODEL.SRTM3)
                    tp_Elevation.Text = tp_Elevation.Text + "[SRTM3]";
                else if (Properties.Settings.Default.ElevationModel == ELEVATIONMODEL.GLOBE)
                    tp_Elevation.Text = tp_Elevation.Text + "[GLOBE]";
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void ClearCharts()
        {
            try
            {
                // clear all points
                Path_Elevation.Points.Clear();
                Min_H1.Points.Clear();
                Min_H2.Points.Clear();
                Max_H.Points.Clear();
                Min_H.Points.Clear();
                Min_H.Points2.Clear();
                Planes_Hi.Points.Clear();
                Planes_Lo.Points.Clear();
                Elevation.Points.Clear();
                LOS.Points.Clear();
                // update view
                pm_Path.InvalidatePlot(true);
                pm_Elevation.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void Charts_ShowLegends(int ms)
        {
            // enable path legends for some seconds
            pm_Path.IsLegendVisible = true;
            pm_Path.InvalidatePlot(true);
            pm_Elevation.IsLegendVisible = true;
            pm_Elevation.InvalidatePlot(true);
            ti_ShowLegends.Interval = ms;
            ti_ShowLegends.Start();
        }

        private void ti_ShowLegends_Tick(object sender, EventArgs e)
        {
            pm_Path.IsLegendVisible = false;
            pm_Path.InvalidatePlot(true);
            pm_Elevation.IsLegendVisible = false;
            pm_Elevation.InvalidatePlot(true);
            ti_ShowLegends.Enabled = false;
        }

        private void pv_Path_Paint(object sender, PaintEventArgs e)
        {
            // draw calsign s on chart
            string mycall = Properties.Settings.Default.MyCall;
            string dxcall = Properties.Settings.Default.DXCall;
            int top = pv_Path.Bottom - pv_Path.Height / 2;
            int left = pv_Path.Left + 50;
            int right = pv_Path.Width - 38;
            Font font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
            Graphics g = e.Graphics;
            using (StringFormat format = new StringFormat(StringFormatFlags.DirectionVertical))
            {
                // measure text to emulate TextAlign.Middle
                int mywidth = (int)g.MeasureString(mycall, font).Width;
                int dxwidth = (int)g.MeasureString(dxcall, font).Width;
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    if (pv_Path.Height - 50 > mywidth)
                        g.DrawString(mycall, font, brush, left, top - mywidth / 2, format);
                    if (pv_Path.Height - 50 > dxwidth)
                        g.DrawString(dxcall, font, brush, right, top - dxwidth / 2, format);
                    /*
                    if (pv_Path.Height > 2 * mywidth + 50)
                        g.DrawString(mycall, font, brush, left, top, format);
                    if (pv_Path.Height > 2 * dxwdith + 50)
                        g.DrawString(dxcall, font, brush, right, top, format);
                    */
                }
            }
        }

        #endregion

        #region Planes

        private static Bitmap RotateImageByAngle(System.Drawing.Image oldBitmap, float angle)
        {
            var newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.TranslateTransform((float)oldBitmap.Width / 2, (float)oldBitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)oldBitmap.Width / 2, -(float)oldBitmap.Height / 2);
            graphics.DrawImage(oldBitmap, new System.Drawing.Point(0, 0));
            return newBitmap;
        }

        private GMarkerGoogle CreatePlaneSimple(PlaneInfo info, bool selected)
        {
            // return on empty info
            if (info == null)
                return null;
            // show flight info only
            // get bitmap according to category
            Bitmap bm;
            int bmindex = bmindex_gray;
            Brush brush = new SolidBrush(Color.FromArgb(180, Color.White));
            if (info.Potential == 100)
            {
                bmindex = bmindex_magenta;
                brush = new SolidBrush(Color.FromArgb(150, Color.Plum));
            }
            else if (info.Potential == 75)
            {
                bmindex = bmindex_red;
                brush = new SolidBrush(Color.FromArgb(150, Color.Red));
            }
            else if (info.Potential == 50)
            {
                bmindex = bmindex_darkorange;
                brush = new SolidBrush(Color.FromArgb(150, Color.DarkOrange));
            }
            if (info.Category == PLANECATEGORY.SUPERHEAVY)
                bm = new Bitmap(il_Planes_S.Images[bmindex]);
            else if (info.Category == PLANECATEGORY.HEAVY)
                bm = new Bitmap(il_Planes_H.Images[bmindex]);
            else if (info.Category == PLANECATEGORY.MEDIUM)
                bm = new Bitmap(il_Planes_M.Images[bmindex]);
            else if (info.Category == PLANECATEGORY.LIGHT)
                bm = new Bitmap(il_Planes_L.Images[bmindex]);
            else
                bm = new Bitmap(il_Planes_M.Images[bmindex]);
            GMarkerGoogle m = new GMarkerGoogle(new PointLatLng(info.Lat, info.Lon), ToolTipFont, RotateImageByAngle(bm, ((info.Track >= 0) && (info.Track <= 360))? (float)info.Track : 0));
            m.Tag = info.Hex;
            string lat = "";
            if (info.Lat >= 0)
                lat = Math.Abs(info.Lat).ToString("00.00") + "°N";
            else
                lat = Math.Abs(info.Lat).ToString("00.00") + "°S";
            string lon = "";
            if (info.Lon >= 0)
                lon = Math.Abs(info.Lon).ToString("000.00") + "°E";
            else
                lon = Math.Abs(info.Lon).ToString("000.00") + "°W";
            m.ToolTipText = info.Call + "\n--------------------";
            if (Properties.Settings.Default.InfoWin_Position)
                m.ToolTipText += "\nPos: " + lat + " , " + lon;
            if (Properties.Settings.Default.InfoWin_Alt)
            {
                if (Properties.Settings.Default.InfoWin_Metric)
                    m.ToolTipText += "\nAlt: " + (int)info.Alt_m + "m";
                else
                    m.ToolTipText += "\nAlt: " + (int)info.Alt + "ft";
            }
            if (Properties.Settings.Default.InfoWin_Track)
                m.ToolTipText += "\nTrack: " + (int)info.Track + "°";
            if (Properties.Settings.Default.InfoWin_Type)
                m.ToolTipText += "\nType: " + info.Manufacturer + " " + info.Model + " [" + PlaneCategories.GetShortStringValue(info.Category) + "]";
            // set tooltip on if hot
            if (selected)
                m.ToolTipMode = MarkerTooltipMode.Always;
            else
                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            if (m.ToolTip != null)
                m.ToolTip.Fill = brush;
            return m;
        }

        private GMarkerGoogle CreatePlaneDetailed(PlaneInfo info, bool selected)
        {
            // return on empty info
            if (info == null)
                return null;
            // get bitmap according to category
            Bitmap bm;
            int bmindex = bmindex_gray;
            Brush brush = new SolidBrush(Color.FromArgb(180, Color.White));
            if (info.Potential == 100)
            {
                bmindex = bmindex_magenta;
                brush = new SolidBrush(Color.FromArgb(150, Color.Plum));
            }
            else if (info.Potential == 75)
            {
                bmindex = bmindex_red;
                brush = new SolidBrush(Color.FromArgb(150, Color.Red));
            }
            else if (info.Potential == 50)
            {
                bmindex = bmindex_darkorange;
                brush = new SolidBrush(Color.FromArgb(150, Color.DarkOrange));
            }
            if (info.Category == PLANECATEGORY.SUPERHEAVY)
                bm = new Bitmap(il_Planes_S.Images[bmindex]);
            else if (info.Category == PLANECATEGORY.HEAVY)
                bm = new Bitmap(il_Planes_H.Images[bmindex]);
            else if (info.Category == PLANECATEGORY.MEDIUM)
                bm = new Bitmap(il_Planes_M.Images[bmindex]);
            else if (info.Category == PLANECATEGORY.LIGHT)
                bm = new Bitmap(il_Planes_L.Images[bmindex]);
            else
                bm = new Bitmap(il_Planes_M.Images[bmindex]);
            GMarkerGoogle m = new GMarkerGoogle(new PointLatLng(info.Lat, info.Lon), ToolTipFont, RotateImageByAngle(bm, ((info.Track >= 0) && (info.Track <= 360)) ? (float)info.Track : 0));
            m.Tag = info.Hex;
            string lat = "";
            if (info.Lat >= 0)
                lat = Math.Abs(info.Lat).ToString("00.00") + "°N";
            else
                lat = Math.Abs(info.Lat).ToString("00.00") + "°S";
            string lon = "";
            if (info.Lon >= 0)
                lon = Math.Abs(info.Lon).ToString("000.00") + "°E";
            else
                lon = Math.Abs(info.Lon).ToString("000.00") + "°W";
            int mins = 0;
            if (info.Speed > 0)
                mins = (int)(info.IntQRB / UnitConverter.kts_kmh(info.Speed) * 60.0);
            // fill tooltip texts
            m.ToolTipText = info.Call + "\n--------------------";
            if (Properties.Settings.Default.InfoWin_Position)
                m.ToolTipText += "\nPos: " + lat + " , " + lon;
            if (Properties.Settings.Default.InfoWin_Alt)
            {
                if (Properties.Settings.Default.InfoWin_Metric)
                    m.ToolTipText += "\nAlt: " + (int)info.Alt_m + "m [" + info.AltDiff.ToString("+#;-#;0") + "m]";
                else
                    m.ToolTipText += "\nAlt: " + (int)info.Alt + "ft [" + UnitConverter.m_ft(info.AltDiff).ToString("+#;-#;0") + "ft]";
            }
            if (Properties.Settings.Default.InfoWin_Track)
                m.ToolTipText += "\nTrack: " + (int)info.Track + "°";
            if (Properties.Settings.Default.InfoWin_Speed)
            {
                if (Properties.Settings.Default.InfoWin_Metric)
                    m.ToolTipText += "\nSpeed: " + info.Speed_kmh.ToString("F0") + "km/h";
                else
                    m.ToolTipText += "\nSpeed: " + info.Speed.ToString("F0") + "kts";
            }
            if (Properties.Settings.Default.InfoWin_Type)
                m.ToolTipText += "\nType: " + info.Manufacturer + " " + info.Model + " [" + PlaneCategories.GetShortStringValue(info.Category) + "]";

            if (info.Potential > 0)
            {
                if (Properties.Settings.Default.InfoWin_Dist)
                {
                    if (Properties.Settings.Default.InfoWin_Metric)
                        m.ToolTipText += "\nDist: " + info.IntQRB.ToString("F0") + "km";
                    else
                        m.ToolTipText += "\nDist: " + UnitConverter.km_mi(info.IntQRB).ToString("F0") + "mi";
                }
                if (Properties.Settings.Default.InfoWin_Time)
                    m.ToolTipText += "\nTime: " + (CurrentTime + new TimeSpan(0, mins, 0)).ToString("HH:mm") + " [ " + mins.ToString("") + "min]";
                if (Properties.Settings.Default.InfoWin_Angle)
                    m.ToolTipText += "\nAngle: " + (info.Angle / Math.PI * 180.0).ToString("F0") + "°";
                if (Properties.Settings.Default.InfoWin_Epsilon)
                    m.ToolTipText += "\nEps: " + (info.Eps1 / Math.PI * 180.0).ToString("00.00") + "° <> " + (info.Eps2 / Math.PI * 180.0).ToString("00.00") + "°";
                if (Properties.Settings.Default.InfoWin_Squint)
                    m.ToolTipText += "\nSquint: " + (info.Squint / Math.PI * 180).ToString("00.00") + "°";
            }

            if ((TrackMode == AIRSCOUTTRACKMODE.TRACK) && (TrackValues != null) && (
                (Properties.Settings.Default.InfoWin_MyAzimuth) ||
                (Properties.Settings.Default.InfoWin_MyElevation) ||
                (Properties.Settings.Default.InfoWin_MyDoppler)
                ))
            {
                m.ToolTipText += "\n--------------------";
                if (Properties.Settings.Default.InfoWin_MyAzimuth)
                    m.ToolTipText += "\nMyAzimuth: " + TrackValues.MyAzimuth.ToString("00.00") + "°";
                if (Properties.Settings.Default.InfoWin_MyElevation)
                    m.ToolTipText += "\nMyElevation: " + TrackValues.MyElevation.ToString("00.00") + "°";
                if (Properties.Settings.Default.InfoWin_MyDoppler)
                    m.ToolTipText += "\nMyDoppler: " + TrackValues.MyDoppler.ToString("F0") + "Hz";
            }
            if ((TrackMode == AIRSCOUTTRACKMODE.TRACK) && (TrackValues != null) && (
                    (Properties.Settings.Default.InfoWin_DXAzimuth) ||
                    (Properties.Settings.Default.InfoWin_DXElevation) ||
                    (Properties.Settings.Default.InfoWin_DXDoppler)
                    ))
            {
                m.ToolTipText += "\n--------------------";
                if (Properties.Settings.Default.InfoWin_DXAzimuth)
                    m.ToolTipText += "\nDXAzimuth: " + TrackValues.DXAzimuth.ToString("00.00") + "°";
                if (Properties.Settings.Default.InfoWin_DXElevation)
                    m.ToolTipText += "\nDXElevation: " + TrackValues.DXElevation.ToString("00.00") + "°";
                if (Properties.Settings.Default.InfoWin_DXDoppler)
                    m.ToolTipText += "\nDXDoppler: " + TrackValues.DXDoppler.ToString("F0") + "Hz";
            }


            if (selected)
            {
                m.ToolTipMode = MarkerTooltipMode.Always;
            }
            else
            {
                m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            }
            if (m.ToolTip != null)
                m.ToolTip.Fill = brush;
            return m;
        }

        private void DrawPlanes()
        {
            bool alarm = false;
            bool isselected = false;
            string alarm_msg = "";

            List<PlaneInfo> pathplanes = new List<PlaneInfo>();

            // check if any plane is on list --> return empty list
            if ((ActivePlanes == null) || (ActivePlanes.Count == 0))
                return;
            bool anyselected = false;
            List<TooltipDataPoint> planes_hi = new List<TooltipDataPoint>();
            List<TooltipDataPoint> planes_lo = new List<TooltipDataPoint>();
            // draw all planes
            foreach (PlaneInfo plane in ActivePlanes.Values)
            {
                try
                {
                    // show planes if it meets filter criteria
                    if ((plane.Alt_m >= Properties.Settings.Default.Planes_MinAlt) && (plane.Category >= Properties.Settings.Default.Planes_Filter_Min_Category))
                    {
                        // check selected state
                        isselected = SelectedPlanes.IndexOf(plane.Hex) >= 0;
                        // now, show plane according to potential
                        switch (plane.Potential)
                        {
                            case 100:
                                gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                // set alarm
                                if (plane.IntQRB < Properties.Settings.Default.Alarm_Distance)
                                {
                                    alarm = true;
                                    alarm_msg = plane.Call;
                                }
                                break;
                            case 75:
                                gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                // set alarm
                                if (plane.IntQRB < Properties.Settings.Default.Alarm_Distance)
                                {
                                    alarm = true;
                                    alarm_msg = plane.Call;
                                }
                                break;
                            case 50:
                                gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                break;
                            default:
                                if (Properties.Settings.Default.InfoWin_AlwaysDetailed)
                                {
                                    gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                }
                                else
                                {
                                    gmo_Planes.Markers.Add(CreatePlaneSimple(plane, isselected));
                                }
                                break;
                        }

                        // count the planes drawed and update caption, if not under Linux
                        // Linux/Mono is drawing the whole control again --> performance issue!
                        if (!SupportFunctions.IsMono)
                            tp_Map.Text = "Map [" + gmo_Planes.Markers.Count.ToString() + " plane(s)]";
                        // if selected: draw the thin path to crossing point if one
                        if (isselected)
                        {
                            anyselected = true;
                            if (plane.IntPoint != null)
                            {
                                GMapRoute intpath = new GMapRoute(plane.Call);
                                intpath.Stroke = new Pen(Color.Black, 1);
                                intpath.Points.Add(new PointLatLng(plane.Lat, plane.Lon));
                                intpath.Points.Add(new PointLatLng(plane.IntPoint.Lat, plane.IntPoint.Lon));
                                gmo_Routes.Routes.Add(intpath);
                                //                                Console.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + ";" + info.IntQRB.ToString("F3"));
                            }
                            // track plane, if enabled
                            if (Properties.Settings.Default.Track_Activate)
                            {
                                Properties.Settings.Default.Track_CurrentPlane = plane.Hex;
                            }
                        }

                        // show planes on chart if in sigle path mode
                        if (PathMode == AIRSCOUTPATHMODE.SINGLE)
                        {
                            if ((plane.IntPoint != null) && (plane.IntQRB <= Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].MaxDistance))
                            {
                                // calculate distance from mylat/mylon
                                double dist = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, plane.IntPoint.Lat, plane.IntPoint.Lon);
                                // add new data points
                                /*
                                if (plane.AltDiff > 0)
                                {
                                    Planes_Hi.Points.Add(new DataPoint(dist, plane.Alt_m));
                                }
                                else
                                {
                                    Planes_Lo.Points.Add(new DataPoint(dist, plane.Alt_m));
                                }
                                */
                                
                                TooltipDataPoint p = new TooltipDataPoint(dist, plane.Alt_m,plane.Call);
                                if (plane.AltDiff > 0)
                                {
                                    planes_hi.Add(p);
                                }
                                else
                                {
                                    planes_lo.Add(p);
                                }
                            }
                        }
                    }

                    // add planes to chart
                    Planes_Hi.ItemsSource = planes_hi;
                    Planes_Lo.ItemsSource = planes_lo;

                    // change tracker display
                    Planes_Hi.TrackerFormatString = "{Tooltip}";
                    Planes_Lo.TrackerFormatString = "{Tooltip}";

                    // invalidate chart
                    pm_Path.InvalidatePlot(true);

                    // set alarm if one
                    if (alarm)
                        Alarm(alarm_msg);
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            // stop tracking if selected object is lost for any reason
            if (!anyselected)
            {
                TrackMode = AIRSCOUTTRACKMODE.NONE;
            }

        }

        private void UpdatePlanes()
        {

            // get current time

            // update status
            UpdateStatus();

            // check for filter settings
            // and color filter box
            int planes_filter_minalt = 0;
            planes_filter_minalt = Properties.Settings.Default.Planes_Filter_Min_Alt;
            if (planes_filter_minalt < 0)
                planes_filter_minalt = 0;
            if ((planes_filter_minalt != 0) || (Properties.Settings.Default.Planes_Filter_Min_Category > PLANECATEGORY.LIGHT))
            {
                pa_Planes_Filter.BackColor = Color.Plum;
            }
            else
            {
                pa_Planes_Filter.BackColor = SystemColors.Control;
            }

            Stopwatch st = new Stopwatch();
            st.Start();
            List<PlaneInfo> allplanes = Planes.GetAll(CurrentTime, Properties.Settings.Default.Planes_Position_TTL);
            // TODO: maintain selected status
            st.Stop();
            // Log.WriteMessage("Getting plane positions from database: " + allplanes.Count().ToString() + " plane(s), " + st.ElapsedMilliseconds.ToString() + " ms.");
            // clear active planes
            ActivePlanes.Clear();

            foreach (PropagationPathDesignator ppath in PropagationPaths)
            {
                st.Reset();
                st.Start();
                NearestPlane = null;
                double highestpotential = 0;
                // get nearest planes per path
                List<PlaneInfo> nearestplanes = AircraftData.Database.GetNearestPlanes(DateTime.UtcNow, ppath, allplanes, Properties.Settings.Default.Planes_Filter_Max_Circumcircle, Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].MaxDistance, Properties.Settings.Default.Planes_MaxAlt);

                foreach (PlaneInfo plane in nearestplanes)
                {
                    // maintain highest potential
                    if ((plane.Alt_m >= Properties.Settings.Default.Planes_Filter_Min_Alt) && (plane.Category >= Properties.Settings.Default.Planes_Filter_Min_Category) && (plane.Potential > highestpotential))
                        highestpotential = plane.Potential;
                    // add or update plane in active planes list
                    PlaneInfo activeplane;
                    if (ActivePlanes.TryGetValue(plane.Hex, out activeplane))
                    {
                        // plane found --> update if necessary
                        bool update = false;
                        // plane has higher potential
                        if ((plane.IntPoint != null) && (plane.Potential > activeplane.Potential))
                            update = true;
                        // plane has same potetial but is nearer to path
                        else if ((plane.Potential == activeplane.Potential) && (plane.IntQRB < activeplane.IntQRB))
                            update = true;
                        // update if necessary
                        if (update)
                        {
                            ActivePlanes.Remove(activeplane.Hex);
                            ActivePlanes.Add(plane.Hex, plane);
                        }
                    }
                    else
                    {
                        // add plane to list if not foound
                        ActivePlanes.Add(plane.Hex, plane);
                    }
                    // maintain nearest plane info
                    // check if plane is within MaxDistance
                    if (plane.IntQRB < Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].MaxDistance)
                    {
                        if (NearestPlane == null)
                        {
                            // set first nearest plane info anyway
                            NearestPlane = plane;
                        }
                        // use higher potential anyway
                        if (plane.Potential > NearestPlane.Potential)
                        {
                            NearestPlane = plane;
                        }
                        else if (plane.Potential == NearestPlane.Potential)
                        {
                            // use nearer plane if same potential
                            if (plane.IntQRB < NearestPlane.IntQRB)
                                NearestPlane = plane;
                        }
                    }
                }
                // colour callsign in watchlist if in MULTIPATH mode
                if (PathMode == AIRSCOUTPATHMODE.MULTI)
                {
                    string dxcall = ppath.Location2.Call;
                    ListViewItem item = lv_Control_Watchlist.FindItemWithText(dxcall);
                    if (item != null)
                    {
                        // store potential in watchlist item and refresh if necessaray
                        if (item.ToolTipText != highestpotential.ToString())
                        {
                            item.ToolTipText = highestpotential.ToString();
                            lv_Control_Watchlist.Refresh();
                        }
                    }
                }
                st.Stop();
                // Log.WriteMessage("Get nearest planes: " + ActivePlanes.Count.ToString() + " plane(s), " + st.ElapsedMilliseconds.ToString() + " ms.");
            }
            st.Start();
            // clear planes overlay in map
            gmo_Planes.Clear();
            // clear all routes except paths
            gmo_Routes.Clear();

            // clear data points in chart
            Planes_Hi.Points.Clear();
            Planes_Lo.Points.Clear();
            pm_Path.Annotations.Clear();

            // draw planes
            DrawPlanes();
            st.Stop();
            // Log.WriteMessage("Drawing planes: " + ActivePlanes.Count.ToString() + " plane(s), " + st.ElapsedMilliseconds.ToString() + " ms.");

            // set focus on the map object
            this.ActiveControl = gm_Main;
        }

        #endregion

        #region Watchlist

        private void UpdateWatchlistInMap()
        {
            // show callsigns from watchlist
            gmo_Callsigns.Clear();
            if (!Properties.Settings.Default.Watchlist_Activated)
                return;
            // create new watchlist if null
            if (Properties.Settings.Default.Watchlist == null)
                Properties.Settings.Default.Watchlist = new Watchlist();
            foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
            {
                // nasty!! Should never be null!
                if (item == null)
                    continue;
                LocationDesignator dxloc = LocationFindOrCreate(item.Call, item.Loc);
                GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(dxloc.Lat, dxloc.Lon), ToolTipFont, (dxloc.Source == GEOSOURCE.FROMUSER) ? GMarkerGoogleType.green_small : GMarkerGoogleType.white_small);
                gm.ToolTipText = dxloc.Call;
                if ((PathMode == AIRSCOUTPATHMODE.MULTI) && Properties.Settings.Default.Map_LabelCalls && item.Checked)
                    gm.ToolTipMode = MarkerTooltipMode.Always;
                else
                    gm.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                gm.Tag = dxloc.Call;
                gmo_Callsigns.Markers.Add(gm);
            }
        }

        private void AddListViewItem (WatchlistItem item)
        {
            LocationDesignator dxcall = StationData.Database.LocationFindOrCreate(item.Call, item.Loc);
            ListViewItem lvi = new ListViewItem(item.Call);
            lvi.Name = "Call";
            ListViewItem.ListViewSubItem lsi = new ListViewItem.ListViewSubItem(lvi, item.Loc);
            lsi.Name = "Loc";
            lvi.SubItems.Add(lsi);
            lv_Control_Watchlist.Items.Add(lvi);
            if (item.Checked)
                lvi.Checked = true;
            if (item.Selected)
                lvi.Selected = true;
            // tag item as "Out of Range"
            if (item.OutOfRange)
            {
                lvi.Tag = "OOR";
                lvi.ForeColor = Color.LightGray;
            }
            else
            {
                lvi.BackColor = (dxcall.Source == GEOSOURCE.FROMUSER) ? Color.PaleGreen : Color.White;
            }

        }

        private void RefreshWatchlistView()
        {
            
            // set watchlistupdating flag
            WatchlistUpdating = true;
            // keep scroll position
            int topItemIndex = 0;
            try
            {
                if ((PlayMode != AIRSCOUTPLAYMODE.FORWARD) && (lv_Control_Watchlist.TopItem != null))
                {
                    topItemIndex = lv_Control_Watchlist.TopItem.Index;
                }
            }
            catch (Exception ex)
            { 
                // do nothing
            }
            // update listview
            lv_Control_Watchlist.BeginUpdate();
            lv_Control_Watchlist.Items.Clear();
            Properties.Settings.Default.Watchlist.Sort();
            // run twice, add checked items first, then all others
            foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
            {
                // nasty!! Should never be null!
                if (item == null)
                    continue;
                if (item.Checked)
                    AddListViewItem(item);
            }
            foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
            {
                // nasty!! Should never be null!
                if (item == null)
                    continue;
                if (!item.Checked)
                    AddListViewItem(item);
            }
            //            lv_Control_Watchlist.Sort();
            lv_Control_Watchlist.EndUpdate();
            // restore scroll position
            try
            {
                lv_Control_Watchlist.TopItem = lv_Control_Watchlist.Items[topItemIndex];
            }
            catch (Exception ex)
            { 
                // do nothing
            }
            // reset watchlistupdating flag
            WatchlistUpdating = false;
        }

        #endregion

        #region User Interface

        private void MapDlg_SizeChanged(object sender, EventArgs e)
        {
        }

        private void MapDlg_Resize(object sender, EventArgs e)
        {
        }

        private void ti_Progress_Tick(object sender, EventArgs e)
        {
            // prevent timer tick from overflow when heavy loaded
            // stop timer --> do update procedure --> start timer again
            ti_Progress.Stop();
            if (LifeMode == AIRSCOUTLIFEMODE.LIFE)
            {
                if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                {
                    // update current time
                    if (Properties.Settings.Default.Time_Mode_Online)
                    {
                        CurrentTime = DateTime.UtcNow;
                        UpdatePlanes();
                    }
                }
            }
            else if (LifeMode == AIRSCOUTLIFEMODE.HISTORY)
            {
                if (PlayMode != AIRSCOUTPLAYMODE.PAUSE)
                {
                    Properties.Settings.Default.Time_Offline = Properties.Settings.Default.Time_Offline.AddSeconds(Time_Offline_Interval);
                    if (Properties.Settings.Default.Time_Offline < dtp_Analysis_MinValue.Value)
                        Properties.Settings.Default.Time_Offline = dtp_Analysis_MinValue.Value;
                    if (Properties.Settings.Default.Time_Offline > dtp_Analysis_MaxValue.Value)
                        Properties.Settings.Default.Time_Offline = dtp_Analysis_MaxValue.Value;
                    CurrentTime = Properties.Settings.Default.Time_Offline;
                    tb_Analysis_Time.Text = CurrentTime.ToString("yyyy-MM-hh HH:mm:ss");
                    double span = (CurrentTime - dtp_Analysis_MinValue.Value).TotalSeconds;
                    if ((span > sb_Analysis_Play.Minimum) && (span < sb_Analysis_Play.Maximum))
                        sb_Analysis_Play.Value = (int)span;
                    UpdatePlanes();
                }
            }
            // restart timer
            ti_Progress.Start();
        }

        private void sc_Main_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }

        private void sc_Map_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }



        #region Map

        private void gm_Main_OnMapZoomChanged()
        {
            double midlat = LatLon.MidPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon).Lat;
            double midlon = LatLon.MidPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon).Lon;
            if ((PlayMode == AIRSCOUTPLAYMODE.FORWARD) && Properties.Settings.Default.Map_AutoCenter)
                gm_Main.Position = new PointLatLng(midlat, midlon);
            tb_Zoom.Text = gm_Main.Zoom.ToString();

            // (re)initialize locator overlay
            InitializeLocators();
        }

        private void gm_Main_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    // check if callsign clicked
                    if (item.Overlay == gmo_Callsigns)
                    {
                        string call = (string)item.Tag;
                        // check if callsign clicked and not own callsign
                        if (Callsign.Check(call) && (call != Properties.Settings.Default.MyCall))
                        {
                            if (PathMode == AIRSCOUTPATHMODE.MULTI)
                            {
                                // search call on watchlist
                                int index = Properties.Settings.Default.Watchlist.IndexOf(call);
                                if (index >= 0)
                                {
                                    // toggle checked state
                                    Properties.Settings.Default.Watchlist.ElementAt(index).Checked = !Properties.Settings.Default.Watchlist.ElementAt(index).Checked;
                                    // refresh watchlist view
                                    RefreshWatchlistView();
                                    // update paths if running
                                    if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                                        UpdatePaths();
                                }
                            }
                            else if (PathMode == AIRSCOUTPATHMODE.SINGLE)
                            {
                                // search call on watchlist
                                int index = Properties.Settings.Default.Watchlist.IndexOf(call);
                                if (index >= 0)
                                {
                                    string loc = Properties.Settings.Default.Watchlist.ElementAt(index).Loc;
                                    // get location info from database
                                    LocationDesignator ld = LocationFindOrCreate(call, loc);
                                    Properties.Settings.Default.DXCall = ld.Call;
                                    Properties.Settings.Default.DXLat = ld.Lat;
                                    Properties.Settings.Default.DXLon = ld.Lon;
                                    UpdateStatus();
                                    // update paths if running
                                    if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                                        UpdatePaths();
                                }
                            }
                        }
                    }
                    // check if plane clicked
                    else if (item.Overlay == gmo_Planes)
                    {
                        // keep the selected state
                        int selectedindex = SelectedPlanes.IndexOf((string)item.Tag);
                        // clear all other selections if tracking is enabled
                        if (Properties.Settings.Default.Track_Activate)
                        {
                            SelectedPlanes.Clear();
                            if (selectedindex < 0)
                            {
                                SelectedPlanes.Add((string)item.Tag);
                            }
                        }
                        else
                        {
                            // toogle selection of the selected plane
                            if (selectedindex >= 0)
                            {
                                // remove item from selected planes list
                                SelectedPlanes.RemoveAt(selectedindex);
                                // invalidate tracking
                                TrackMode = AIRSCOUTTRACKMODE.NONE;
                            }
                            else
                            {
                                SelectedPlanes.Add((string)item.Tag);
                            }
                        }
                        // set track mode
                        if (Properties.Settings.Default.Track_Activate)
                        {
                            TrackMode = AIRSCOUTTRACKMODE.TRACK;
                        }
                    }
                    // check if call clicked
                    else if (item.Overlay == gmo_Callsigns)
                    {
                        LocationDesignator ld = StationData.Database.LocationFind(item.Tag.ToString());
                        if (ld != null)
                        {
                            if (PlayMode != AIRSCOUTPLAYMODE.PAUSE)
                                this.Pause();
                            Properties.Settings.Default.DXCall = item.Tag.ToString();
                            Properties.Settings.Default.DXLat = ld.Lat;
                            Properties.Settings.Default.DXLon = ld.Lon;
                            UpdateStatus();
                            this.Play();
                        }
                    }
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    // new for tracking antenna
                    if ((string)item.Tag == Properties.Settings.Default.DXCall)
                    {
                        // Right click on DXCall needle --> set antenna position
                        // get antenna direction
                        double qtf = LatLon.Bearing(Properties.Settings.Default.MyLat,
                            Properties.Settings.Default.MyLon,
                            Properties.Settings.Default.DXLat,
                            Properties.Settings.Default.DXLon);
                        // store new QTF in properties
                        lock (Properties.Settings.Default)
                        {
                            TrackMode = AIRSCOUTTRACKMODE.SINGLE;
                            Properties.Settings.Default.Track_SetAz = qtf;
                            Properties.Settings.Default.Track_SetEl = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing if item not found in planes list
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void gm_Main_OnMarkerEnter(GMapMarker item)
        {
            if (((item == gmm_MyLoc) || (item == gmm_DXLoc)) && !isDraggingMarker)
                gmm_CurrentMarker = item;
        }

        private void gm_Main_OnMarkerLeave(GMapMarker item)
        {

        }

        private void gm_Main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && gmm_CurrentMarker != null && gmm_CurrentMarker.IsMouseOver)
                isDraggingMarker = true;
        }

        private void gm_Main_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void gm_Main_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingMarker)
            {
                isDraggingMarker = false;
            }

            if (isDraggingMap)
            {
                // (re)initialize locator overlay
                InitializeLocators();
                isDraggingMap = false;
            }
        }

        private void gm_Main_OnMapDrag()
        {
            // set dragging mode
            isDraggingMap = true;

            // switch off locator overlay
            gmo_Locators.IsVisibile = false;
        }


        private void gm_Main_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Settings.Default.Map_TrackingGaugesShow && (TrackMode == AIRSCOUTTRACKMODE.TRACK) && (TrackValues != null))
            {
                // paint gauges on top of the map if enabled
                ag_Azimuth.Value = (float)TrackValues.MyAzimuth;
                ag_Elevation.Value = (float)TrackValues.MyElevation;

                // get gauges forecolor from properties
                Color gaugescolor = Color.FromName(Properties.Settings.Default.Map_TrackingGaugeColor);
                if (gaugescolor == null)
                {
                    // set to black if fails
                    gaugescolor = Color.Black;
                }

                // get brushes, pens and fonts
                Brush gaugesbrush = new SolidBrush(gaugescolor);
                Pen gaugespen = new Pen(gaugesbrush, 3);
                Font trackfont = new Font("Courier New", (int)((double)pa_Rig.Height / 7.0), FontStyle.Bold);

                // set colors, brushes and pens
                ag_Azimuth.ForeColor = gaugescolor;
                ag_Elevation.ForeColor = gaugescolor;

                e.Graphics.DrawRectangle(gaugespen, new Rectangle(ag_Azimuth.Left, ag_Azimuth.Top, ag_Azimuth.Width + ag_Elevation.Width + 20, ag_Elevation.Height));

                // draw elements
                int ofsX = ag_Azimuth.Left;
                int ofsY = ag_Azimuth.Top;
                ag_Azimuth.DrawDialText(e.Graphics, ofsX, ofsY);
                ag_Azimuth.DisplayNumber(e.Graphics, ofsX, ofsY);
                ag_Azimuth.DrawCalibration(e.Graphics, ofsX, ofsY);
                ag_Azimuth.DrawCenterPoint(e.Graphics, ofsX, ofsY);
                ag_Azimuth.DrawPointer(e.Graphics, ofsX, ofsY);

                ofsX = ag_Elevation.Left;
                ofsY = ag_Elevation.Top;
                ag_Elevation.DrawDialText(e.Graphics, ofsX, ofsY);
                ag_Elevation.DisplayNumber(e.Graphics, ofsX, ofsY);
                ag_Elevation.DrawCalibration(e.Graphics, ofsX, ofsY);
                ag_Elevation.DrawCenterPoint(e.Graphics, ofsX, ofsY);
                ag_Elevation.DrawPointer(e.Graphics, ofsX, ofsY);

                if (!Properties.Settings.Default.Doppler_Strategy_None)
                {
                    // paint rig frequencies on top of the map if doppler compensation enabled
                    NumberFormatInfo info = new NumberFormatInfo();
                    info.NumberDecimalSeparator = ",";
                    info.NumberGroupSeparator = ".";
                    int top = 0;
                    int left = 10;
                    e.Graphics.DrawRectangle(gaugespen, new Rectangle(pa_Rig.Left, pa_Rig.Top, pa_Rig.Width, pa_Rig.Height));
                    e.Graphics.DrawString("MyDop: " + TrackValues.MyDoppler.ToString("F0"), trackfont, gaugesbrush, new PointF(pa_Rig.Left + left, pa_Rig.Top + top));
                    e.Graphics.DrawString("DXDop: " + TrackValues.DXDoppler.ToString("F0"), trackfont, gaugesbrush, new PointF(pa_Rig.Left + left + pa_Rig.Width / 2, pa_Rig.Top + top));
                    e.Graphics.DrawString("Dial : " + Properties.Settings.Default.Doppler_DialFreq.ToString(info), trackfont, gaugesbrush, new PointF(pa_Rig.Left + left, pa_Rig.Top + top + pa_Rig.Height / 4 * 1));
                    e.Graphics.DrawString("RX   : " + TrackValues.RXFrequency.ToString(info), trackfont, gaugesbrush, new PointF(pa_Rig.Left + left, pa_Rig.Top + top + pa_Rig.Height / 4 * 2));
                    e.Graphics.DrawString("TX   : " + TrackValues.TXFrequency.ToString(info), trackfont, gaugesbrush, new PointF(pa_Rig.Left + left, pa_Rig.Top + top + pa_Rig.Height / 4 * 3));
                }
            }
        }

        private void gm_Main_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                // get width from properties
                int width = (int)Properties.Settings.Default.Map_TrackingGaugeWidth;


                // check bounds
                if (width < 175)
                    width = 175;
                if (width > this.Width / 2)
                    width = this.Width / 2;

                // adjust position and size of gauges
                ag_Azimuth.Width = width;
                ag_Elevation.Width = width;
                ag_Azimuth.Left = gm_Main.Right - ag_Azimuth.Width - ag_Elevation.Width - 40;
                ag_Azimuth.Top = gm_Main.Bottom - ag_Azimuth.Height - 20;

                ag_Elevation.Left = gm_Main.Right - ag_Elevation.Width - 20;
                ag_Elevation.Top = gm_Main.Bottom - ag_Elevation.Height - 20;

                // adjust position and size of rig panel
                pa_Rig.Width = ag_Elevation.Right - ag_Azimuth.Left;
                pa_Rig.Height = ag_Azimuth.Height / 2;
                pa_Rig.Left = ag_Azimuth.Left;
                pa_Rig.Top = gm_Main.Bottom - ag_Azimuth.Height - pa_Rig.Height - 30;
            }
            catch (Exception ex)
            {

            }
        }

        private void gm_Main_OnPositionChanged(PointLatLng point)
        {
        }

        private void gm_Main_OnTileLoadComplete(long ElapsedMilliseconds)
        {
            // use thread safe call here!
            this.BeginInvoke((Action)delegate ()
            {
                InitializeLocators();
            });
        }

        #endregion

        #region Right Controls

        private void cb_Band_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Band.SelectedItem != null)
                Properties.Settings.Default.Band = Bands.ParseStringValue((string)cb_Band.SelectedItem);
            else Properties.Settings.Default.Band = BAND.BNONE;

//            SaveUserSettings();
//            Properties.Settings.Default.Reload();

        }

        #region Tab Control Control Panel

        private void tc_Control_DrawItem(object sender, DrawItemEventArgs e)
        {
            // This event is called once for each tab button in your tab control
            // First paint the background with a color based on the current tab
            // e.Index is the index of the tab in the TabPages collection.
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);
                    break;
                case 2:
                    e.Graphics.FillRectangle(new SolidBrush((Properties.Settings.Default.Planes_Filter_Min_Category > PLANECATEGORY.LIGHT) ? Color.Plum : SystemColors.Control), e.Bounds);
                    break;
                default:
                    break;
            }

            // Then draw the current tab button text 
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            e.Graphics.DrawString(tc_Control.TabPages[e.Index].Text, tc_Control.Font, (tc_Control.Enabled) ? SystemBrushes.ControlText : SystemBrushes.GrayText, paddedBounds);
        }


        #region Single Tab

        private void tp_Control_Single_Enter(object sender, EventArgs e)
        {
            if (PathMode != AIRSCOUTPATHMODE.SINGLE)
            {
                PathMode = AIRSCOUTPATHMODE.SINGLE;
                tc_Map.SelectedTab = tp_Map;
            }
            // update all info
            UpdateStatus();
        }

        private void gb_Map_Info_MouseClick(object sender, MouseEventArgs e)
        {
            // get font size in pixels
            double pixels;
            using (Graphics g = this.CreateGraphics())
            {
                pixels = gb_Map_Info.Font.SizeInPoints * g.DpiX / 72;
            }
            if ((e.Y <= pixels) && (e.Button == MouseButtons.Left))
            {
                Properties.Settings.Default.Map_ShowInfoBox = !Properties.Settings.Default.Map_ShowInfoBox;
            }
        }


        private void cb_MyCall_TextChanged(object sender, EventArgs e)
        {
            int i = cb_MyCall.SelectionStart;
            Properties.Settings.Default.MyCall = cb_MyCall.Text;
            // clear locator entries
            cb_MyLoc.Text = "";
            cb_MyLoc.Items.Clear();
            Properties.Settings.Default.MyLat = double.NaN;
            Properties.Settings.Default.MyLon = double.NaN;
            Properties.Settings.Default.MyElevation = 0;
            if (Callsign.Check(cb_MyCall.Text))
            {
                // select last recent loc
                LocationDesignator ld = StationData.Database.LocationFind(cb_MyCall.Text);
                if (ld != null)
                {
                    Properties.Settings.Default.MyLat = ld.Lat;
                    Properties.Settings.Default.MyLon = ld.Lon;
                    Properties.Settings.Default.MyElevation = GetElevation(ld.Lat, ld.Lon);
                }
            }
            UpdateStatus();
            cb_MyCall.Select(i, 0);
        }

        private void cb_MyCall_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_MyCall.SelectedItem != null)
            {
                cb_MyCall.Text = cb_MyCall.SelectedItem.ToString();
                LocationDesignator ld = StationData.Database.LocationFind(cb_MyCall.Text);
                if (Callsign.Check(cb_MyCall.Text) && (ld != null))
                {
                    // update Settings
                    cb_MyLoc.Text = MaidenheadLocator.LocFromLatLon(ld.Lat, ld.Lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength);
                    Properties.Settings.Default.MyCall = cb_MyCall.Text;
                    Properties.Settings.Default.MyLat = ld.Lat;
                    Properties.Settings.Default.MyLon = ld.Lon;
                    Properties.Settings.Default.MyElevation = GetElevation(ld.Lat, ld.Lon);
                    Properties.Settings.Default.MyHeight = 10;
                }
                else
                {
                    Properties.Settings.Default.MyLat = double.NaN;
                    Properties.Settings.Default.MyLon = double.NaN;
                    Properties.Settings.Default.MyElevation = 0;
                    Properties.Settings.Default.MyHeight = 0;
                }
                UpdateStatus();
            }
        }

        private void cb_MyCall_DropDown(object sender, EventArgs e)
        {
            // poulate from MyCalls last recent collection
            // populate drop down list
            cb_MyCall.Items.Clear();
            foreach (string call in Properties.Settings.Default.MyCalls)
            {
                cb_MyCall.Items.Add(call);
            }
        }

        private void cb_MyLoc_TextChanged(object sender, EventArgs e)
        {
            if (cb_MyLoc.Focused)
            {
                if (MaidenheadLocator.Check(cb_MyLoc.Text) && (cb_MyLoc.Text.Length >= 6))
                {
                    Properties.Settings.Default.MyLat = cb_MyLoc.GeoLocation.Lat;
                    Properties.Settings.Default.MyLon = cb_MyLoc.GeoLocation.Lon;
                    Properties.Settings.Default.MyElevation = GetElevation(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
                    Properties.Settings.Default.MyHeight = 10;
                    // colour Textbox if more precise lat/lon information is available
                    if (MaidenheadLocator.IsPrecise(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, 3))
                    {
                        cb_MyLoc.BackColor = Color.PaleGreen;
                    }
                    UpdateStatus();
                }
                else
                {
                    cb_MyLoc.BackColor = Color.FloralWhite;
                }
            }
        }

        private void cb_MyLoc_DropDown(object sender, EventArgs e)
        {
            // fill MyLoc combo box with all known locators
            cb_MyLoc.BackColor = Color.FloralWhite;
            cb_MyLoc.SilentText = "";
            cb_MyLoc.Items.Clear();
            List<LocationDesignator> lds = StationData.Database.LocationFindAll(cb_MyCall.Text);
            if (lds != null)
            {
                // fill item list of locator box
                foreach (LocationDesignator ld in lds)
                {
                    LocatorDropDownItem ldd = new LocatorDropDownItem(MaidenheadLocator.LocFromLatLon(ld.Lat, ld.Lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength), new LatLon.GPoint(ld.Lat, ld.Lon));
                    cb_MyLoc.Items.Add(ldd);
                }
            }
        }

        private void cb_MyLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocatorDropDownItem ldd = (LocatorDropDownItem)cb_MyLoc.SelectedItem;
            if (ldd != null)
            {
                // update properties
                cb_MyLoc.Precision = (int)Properties.Settings.Default.Locator_MaxLength / 2;
                cb_MyLoc.SmallLettersForSubsquares = Properties.Settings.Default.Locator_SmallLettersForSubsquares;
                cb_MyLoc.AutoLength = Properties.Settings.Default.Locator_AutoLength;
                // set geolocation instead of text
                cb_MyLoc.GeoLocation = ldd.GeoLocation;
            }
        }

        private void cb_MyLoc_SelectionChangeCommittedWithNoUpdate(object sender, EventArgs e)
        {

        }

        private void cb_DXCall_TextChanged(object sender, EventArgs e)
        {
            int i = cb_DXCall.SelectionStart;
            Properties.Settings.Default.DXCall = cb_DXCall.Text;
            // clear locator entries
            cb_DXLoc.Text = "";
            cb_DXLoc.Items.Clear();
            Properties.Settings.Default.DXLat = double.NaN;
            Properties.Settings.Default.DXLon = double.NaN;
            Properties.Settings.Default.DXElevation = 0;
            if (Callsign.Check(cb_DXCall.Text))
            {
                // select last recent loc
                LocationDesignator ld = StationData.Database.LocationFind(cb_DXCall.Text);
                if (ld != null)
                {
                    Properties.Settings.Default.DXLat = ld.Lat;
                    Properties.Settings.Default.DXLon = ld.Lon;
                    Properties.Settings.Default.DXElevation = GetElevation(ld.Lat, ld.Lon);
                }
            }
            UpdateStatus();
            cb_DXCall.Select(i, 0);
        }

        private void cb_DXCall_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_DXCall.SelectedItem != null)
            {
                cb_DXCall.Text = cb_DXCall.SelectedItem.ToString();
                LocationDesignator ld = StationData.Database.LocationFind(cb_DXCall.Text);
                if (Callsign.Check(cb_DXCall.Text) && (ld != null))
                {
                    // update Settings
                    cb_DXLoc.Text = MaidenheadLocator.LocFromLatLon(ld.Lat, ld.Lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength);
                    Properties.Settings.Default.DXCall = cb_DXCall.Text;
                    Properties.Settings.Default.DXLat = ld.Lat;
                    Properties.Settings.Default.DXLon = ld.Lon;
                    Properties.Settings.Default.DXElevation = GetElevation(ld.Lat, ld.Lon);
                    Properties.Settings.Default.DXHeight = 10;
                }
                else
                {
                    Properties.Settings.Default.DXLat = double.NaN;
                    Properties.Settings.Default.DXLon = double.NaN;
                    Properties.Settings.Default.DXElevation = 0;
                    Properties.Settings.Default.DXHeight = 0;
                }
                UpdateStatus();
            }
        }

        private void cb_DXCall_DropDown(object sender, EventArgs e)
        {
            // populate from watchlist
            // return on empty watchlist
            if (Properties.Settings.Default.Watchlist == null)
                return;
            // populate drop down list
            List<string> dxcalls = new List<string>();
            foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
            {
                // nasty!! Should never be null!
                if (item == null)
                    continue;
                dxcalls.Add(item.Call);
            }
            dxcalls.Sort();
            cb_DXCall.Items.Clear();
            cb_DXCall.Items.AddRange(dxcalls.ToArray());
        }


        private void cb_DXLoc_TextChanged(object sender, EventArgs e)
        {
            if (cb_DXLoc.Focused)
            {
                if (MaidenheadLocator.Check(cb_DXLoc.Text) && (cb_DXLoc.Text.Length >= 6))
                {
                    Properties.Settings.Default.DXLat = cb_DXLoc.GeoLocation.Lat;
                    Properties.Settings.Default.DXLon = cb_DXLoc.GeoLocation.Lon;
                    Properties.Settings.Default.DXElevation = GetElevation(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon);
                    Properties.Settings.Default.DXHeight = 10;
                    // colour Textbox if more precise lat/lon information is available
                    if (MaidenheadLocator.IsPrecise(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, 3))
                    {
                        cb_DXLoc.BackColor = Color.PaleGreen;
                    }
                    UpdateStatus();
                }
                else
                {
                    cb_DXLoc.BackColor = Color.FloralWhite;
                }
            }
        }

        private void cb_DXLoc_DropDown(object sender, EventArgs e)
        {
            // fill DXLoc combo box with all known locators
            cb_DXLoc.BackColor = Color.FloralWhite;
            cb_DXLoc.SilentText = "";
            cb_DXLoc.Items.Clear();
            List<LocationDesignator> lds = StationData.Database.LocationFindAll(cb_DXCall.Text);
            if (lds != null)
            {
                // fill item list of locator box
                foreach (LocationDesignator ld in lds)
                {
                    LocatorDropDownItem ldd = new LocatorDropDownItem(MaidenheadLocator.LocFromLatLon(ld.Lat, ld.Lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength), new LatLon.GPoint(ld.Lat, ld.Lon));
                    cb_DXLoc.Items.Add(ldd);
                }
            }
        }

        private void cb_DXLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocatorDropDownItem ldd = (LocatorDropDownItem)cb_DXLoc.SelectedItem;
            if (ldd != null)
            {
                // update properties
                cb_DXLoc.Precision = (int)Properties.Settings.Default.Locator_MaxLength / 2;
                cb_DXLoc.SmallLettersForSubsquares = Properties.Settings.Default.Locator_SmallLettersForSubsquares;
                cb_DXLoc.AutoLength = Properties.Settings.Default.Locator_AutoLength;
                // set geolocation instead of text
                cb_DXLoc.GeoLocation = ldd.GeoLocation;
            }
        }

        private void cb_DXLoc_SelectionChangeCommittedWithNoUpdate(object sender, EventArgs e)
        {
        }

        #endregion

        #region Tab Multi

        private void tp_Control_Multi_Enter(object sender, EventArgs e)
        {
            // enbale/disable manage watchlist button
            btn_Control_Manage_Watchlist.Enabled = !Properties.Settings.Default.Watchlist_SyncWithKST || !Properties.Settings.Default.Server_Activate;

            if (PathMode != AIRSCOUTPATHMODE.MULTI)
            {
                PathMode = AIRSCOUTPATHMODE.MULTI;
                tc_Map.SelectedTab = tp_Map;
            }
            tp_Control_Multi.Refresh();
        }

        private void lv_Control_Watchlist_Resize(object sender, EventArgs e)
        {
            // list view resized
            // resize locator column to fit the client size
            try
            {
                lv_Control_Watchlist.Columns[1].Width = lv_Control_Watchlist.ClientSize.Width - lv_Control_Watchlist.Columns[0].Width;
            }
            catch
            {
                // do nothing, if resize fails
            }
        }

        private void lv_Control_Watchlist_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            // adjust width of Locator column to list view width
            if (e.ColumnIndex == 0)
            {
                // call sign column changed
                // resize locator column to fit the client size
                try
                {
                    lv_Control_Watchlist.Columns[1].Width = lv_Control_Watchlist.ClientSize.Width - lv_Control_Watchlist.Columns[0].Width;
                }
                catch
                {
                    // do nothing, if resize fails
                }
            }
        }

        private void lv_Control_Watchlist_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // ignore when in PLAY mode
            if (!WatchlistUpdating && (PlayMode == AIRSCOUTPLAYMODE.FORWARD))
                e.NewValue = e.CurrentValue;
        }

        private void lv_Control_Watchlist_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // ignore event while populating list view
            if (WatchlistUpdating)
                return;
            // sync watchlist with list view
            ListViewItem lvi = e.Item;
            if (lvi == null)
                return;
            // search item in watchlist
            int index = Properties.Settings.Default.Watchlist.IndexOf(lvi.Text, lvi.SubItems[1].Text);
            if (index >= 0)
                Properties.Settings.Default.Watchlist[index].Checked = lvi.Checked;
            if (WatchlistAllCheckedChanging)
                return;
            // maintain AllChecked checkbox
            foreach (ListViewItem item in lv_Control_Watchlist.Items)
            {
                // stop on first different checked state
                if ((item != null) && (item.Checked != lvi.Checked))
                {
                    WatchlistAllCheckedState = CheckBoxState.MixedNormal;
                    lv_Control_Watchlist.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
                    return;
                }
            }
            // all items in the same state
            WatchlistAllCheckedState = (lvi.Checked) ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
            lv_Control_Watchlist.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void lv_Control_Watchlist_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // ignore when in PLAY mode
            if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                return;
            WatchlistAllCheckedChanging = true;
            if (!WatchlistAllChecked)
            {
                WatchlistAllChecked = true;
                WatchlistAllCheckedState = CheckBoxState.CheckedPressed;
                foreach (ListViewItem item in lv_Control_Watchlist.Items)
                {
                    // check all items within range
                    if ((item.Tag == null) || (item.Tag.ToString() != "OOR"))
                        item.Checked = true;
                }

                Invalidate();
            }
            else
            {
                WatchlistAllChecked = false;
                WatchlistAllCheckedState = CheckBoxState.UncheckedNormal;
                Invalidate();

                foreach (ListViewItem item in lv_Control_Watchlist.Items)
                {
                    item.Checked = false;
                }
            }
            WatchlistAllCheckedChanging = false;
        }

        private void lv_Control_Watchlist_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.LeftAndRightPadding;
            e.DrawBackground();
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new System.Drawing.Point(ClientRectangle.Location.X + 4, ClientRectangle.Location.Y + 1), WatchlistAllCheckedState);
            e.DrawText(flags);
        }

        private void lv_Control_Watchlist_DrawItem(object sender, DrawListViewItemEventArgs e)
        {   
            if (e.Item != null)
            {
                if ((PlayMode == AIRSCOUTPLAYMODE.FORWARD) &&  !String.IsNullOrEmpty(e.Item.ToolTipText))
                {
                    Color bkcolor = e.Item.BackColor;
                    try
                    {
                        double potential = double.Parse(e.Item.ToolTipText);
                        // set default color
                        bkcolor = Color.White;
                        if (potential > 0)
                            bkcolor = Color.Orange;
                        if (potential > 50)
                            bkcolor = Color.Red;
                        if (potential > 75)
                            bkcolor = Color.Magenta;
                    }
                    catch (Exception ex)
                    {
                        // do nothing
                    }
                    if (bkcolor != e.Item.BackColor)
                    {
                        e.Item.BackColor = bkcolor;
                    }
                }
                else
                {
                    e.Item.BackColor = Color.White;
                }
            }
            e.DrawDefault = true;
        }


        private void lv_Control_Watchlist_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
             e.DrawDefault = true;
        }

        private void lv_Control_Watchlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            // synchronize station in SINGLE mode when selection changed in MULTI mode
            try
            {
                if ((lv_Control_Watchlist.SelectedItems != null) && (lv_Control_Watchlist.SelectedItems.Count == 1))
                {
                    string call = lv_Control_Watchlist.SelectedItems[0].Text;
                    string loc = lv_Control_Watchlist.SelectedItems[0].SubItems[1].Text;
                    double lat = MaidenheadLocator.LatFromLoc(loc);
                    double lon = MaidenheadLocator.LonFromLoc(loc);
                    LocationDesignator ld = StationData.Database.LocationFind(call, loc);
                    if (ld != null)
                    {
                        // update lat/lon from database if found
                        lat = ld.Lat;
                        lon = ld.Lon;
                    }
                    Properties.Settings.Default.DXCall = call;
                    Properties.Settings.Default.DXLat = lat;
                    Properties.Settings.Default.DXLon = lon;
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
        }

        private void btn_Control_Manage_Watchlist_Click(object sender, EventArgs e)
        {
            // sync watchlist, try to keep previously checked calls
            // you can have a call only once in the watch list
            List<string> checkedcalls = new List<string>();
            foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
            {
                if (item.Checked)
                    checkedcalls.Add(item.Call);
            }
            WatchlistDlg Dlg = new WatchlistDlg();
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                // clear watch list
                Properties.Settings.Default.Watchlist.Clear();
                foreach (DataGridViewRow row in Dlg.dgv_Watchlist_Selected.Rows)
                {
                    string call = row.Cells[0].Value.ToString();
                    string loc = row.Cells[1].Value.ToString();
                    bool oor = true;
                    // try to get the location from database
                    LocationDesignator dxloc = StationData.Database.LocationFind(call, loc);
                    if (dxloc != null)
                    {
                        oor = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, dxloc.Lat, dxloc.Lon) > Properties.Settings.Default.Path_MaxLength;
                    }
                    // add call to watch list
                    WatchlistItem item = new WatchlistItem(call, loc, oor);
                    Properties.Settings.Default.Watchlist.Add(item);
                }
                // reselect previously selected
                foreach (string checkedcall in checkedcalls)
                {
                    int index = Properties.Settings.Default.Watchlist.IndexOf(checkedcall);
                    if (index >= 0)
                        Properties.Settings.Default.Watchlist[index].Checked = true;
                }
                // refresh watchlist view
                RefreshWatchlistView();
            }
        }

        private void ShowToolTip(ToolTip tt, string text, Control control, System.Drawing.Point p, int ms = 5000)
        {
            if (String.IsNullOrEmpty(text))
                return;
            // int BorderWidth = this.Width – this.ClientSize.Width)/2;
            // int TitlebarHeight = this.Height – this.ClientSize.Height – 2 * BorderWidth;
            int BorderWith = (this.Width - this.ClientSize.Width) / 2;
            int TitleBarHeight = this.Height - this.ClientSize.Height - 2 * BorderWith;
            p = control.PointToScreen(p);
            p = this.PointToClient(p);
            p.X = p.X + BorderWith;
            p.Y = p.Y + TitleBarHeight + Cursor.Size.Height;
            tt.Show(text, this, p, ms);
        }

        private void lv_Control_Watchlist_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
        }

        private void lv_Control_Watchlist_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                System.Drawing.Point p = new System.Drawing.Point(e.X, e.Y);
                ListViewHitTestInfo info = lv_Control_Watchlist.HitTest(p);
                if ((WatchlistOldMousePos != p) && (info != null) && (info.SubItem != null))
                {
                    WatchlistOldMousePos = p;
                    // check whether the column name is one of the following
                    if (info.SubItem.Name == "Call")
                    {
                        string dxcall = info.Item.SubItems[0].Text;
                        string dxloc = info.Item.SubItems[1].Text;
                        LocationDesignator ld = StationData.Database.LocationFind(dxcall, dxloc);
                        // location found --> show Tooltip with details
                        if (ld != null)
                        {
                            string qrb = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, ld.Lat, ld.Lon).ToString("F0", CultureInfo.InvariantCulture);
                            string qtf = LatLon.Bearing(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, ld.Lat, ld.Lon).ToString("F0", CultureInfo.InvariantCulture);
                            ShowToolTip(tt_Control_Watchlist, dxcall + "\n" + dxloc + "\n" + qrb + " km\n" + qtf + "°", lv_Control_Watchlist, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        #endregion

        #region Tab Options

        private void tp_Control_Options_Enter(object sender, EventArgs e)
        {
            // set plane filter
            try
            {
                cb_Planes_Filter_Min_Cat.SelectedItem = PlaneCategories.GetStringValue(Properties.Settings.Default.Planes_Filter_Min_Category);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void gb_Map_Zoom_MouseClick(object sender, MouseEventArgs e)
        {
            // get font size in pixels
            double pixels;
            using (Graphics g = this.CreateGraphics())
            {
                pixels = gb_Map_Zoom.Font.SizeInPoints * g.DpiX / 72;
            }
            if ((e.Y <= pixels) && (e.Button == MouseButtons.Left))
            {
                Properties.Settings.Default.Map_ShowZoomBox = !Properties.Settings.Default.Map_ShowZoomBox;
            }
        }

        private void gb_Map_Filter_MouseClick(object sender, MouseEventArgs e)
        {
            // get font size in pixels
            double pixels;
            using (Graphics g = this.CreateGraphics())
            {
                pixels = gb_Map_Filter.Font.SizeInPoints * g.DpiX / 72;
            }
            if ((e.Y <= pixels) && (e.Button == MouseButtons.Left))
            {
                Properties.Settings.Default.Map_ShowFilterBox = !Properties.Settings.Default.Map_ShowFilterBox;
            }
        }

        private void gb_Map_Alarms_MouseClick(object sender, MouseEventArgs e)
        {
            // get font size in pixels
            double pixels;
            using (Graphics g = this.CreateGraphics())
            {
                pixels = gb_Map_Alarms.Font.SizeInPoints * g.DpiX / 72;
            }
            if ((e.Y <= pixels) && (e.Button == MouseButtons.Left))
            {
                Properties.Settings.Default.Map_ShowAlarmBox = !Properties.Settings.Default.Map_ShowAlarmBox;
            }
        }

        private void btn_Zoom_In_Click(object sender, EventArgs e)
        {
            if (gm_Main.Zoom < 20)
                gm_Main.Zoom++;
        }

        private void btn_Zoom_Out_Click(object sender, EventArgs e)
        {
            if (gm_Main.Zoom > 0)
                gm_Main.Zoom--;
        }

        private void cb_Planes_Filter_Min_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Planes_Filter_Min_Category = PlaneCategories.ParseStringValue((string)cb_Planes_Filter_Min_Cat.SelectedItem);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }


        #endregion

        #endregion

        #region Buttons

        private void btn_Options_Click(object sender, EventArgs e)
        {
            ShowOptionsDlg();
        }

        private void btn_Map_Save_Click(object sender, EventArgs e)
        {
            MapSave();
        }

        private void btn_Map_PlayPause_Click(object sender, EventArgs e)
        {
            if (PlayMode != AIRSCOUTPLAYMODE.FORWARD)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }


        #endregion

        #endregion

        #region Main Tab Control

        private void tc_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tc_Main_DrawItem(object sender, DrawItemEventArgs e)
        {
            // This event is called once for each tab button in your tab control
            // First paint the background with a color based on the current tab
            // e.Index is the index of the tab in the TabPages collection.
            // fill background
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);
            // Then draw the current tab button text 
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            e.Graphics.DrawString(tc_Main.TabPages[e.Index].Text, tc_Main.Font, (tc_Main.Enabled) ? SystemBrushes.ControlText : SystemBrushes.GrayText, paddedBounds);

        }

        #region Tab Page Spectrum

        private void tp_Spectrum_Enter(object sender, EventArgs e)
        {
        }

        private void tp_Spectrum_Resize(object sender, EventArgs e)
        {
            // adjust group boxes
            // get quadratical nearest plane box
            gb_NearestPlaneMap.Width = gb_NearestPlaneMap.Height;
            gb_NearestPlaneMap.Left = tp_Spectrum.Width - gb_NearestPlaneMap.Width - 5;
            // adjust plane info box
            gb_Spectrum_NearestInfo.Left = tp_Spectrum.Width - gb_NearestPlaneMap.Width - gb_Spectrum_NearestInfo.Width - 10;
            // adjust spectrum box
            gb_Spectrum.Width = tp_Spectrum.Width - gb_NearestPlaneMap.Width - gb_Spectrum_NearestInfo.Width - 30;
            tb_Spectrum_Status.Left = 5;
            tb_Spectrum_Status.Width = pv_Spectrum.Width - 10;
        }

        #endregion

        #region Tab Page Elevation

        private void tp_Elevation_Enter(object sender, EventArgs e)
        {
        }

        private void tp_Elevation_Resize(object sender, EventArgs e)
        {
            // adjust charts
            pv_Path.Location = new System.Drawing.Point(0, 0);
            pv_Path.Width = tp_Elevation.Width;
            pv_Path.Height = tp_Elevation.Height / 2;
            pv_Elevation.Location = new System.Drawing.Point(0, tp_Elevation.Height / 2);
            pv_Elevation.Width = tp_Elevation.Width;
            pv_Elevation.Height = tp_Elevation.Height / 2;
        }


        #endregion

        #region Tab Page Analysis

        private void UpdatePlayer()
        {
            // set players bounds
            sb_Analysis_Play.Minimum = 0;
            sb_Analysis_Play.Maximum = (int)(dtp_Analysis_MaxValue.Value - dtp_Analysis_MinValue.Value).TotalSeconds;
            sb_Analysis_Play.Value = 0;
        }

        private void tp_Analysis_Enter(object sender, EventArgs e)
        {
            if (LifeMode == AIRSCOUTLIFEMODE.LIFE)
            {
                btn_Analysis_ON.Enabled = true;
                btn_Analysis_ON.BackColor = Color.YellowGreen;
                btn_Analysis_OFF.Enabled = false;
                btn_Analysis_OFF.BackColor = Color.Gray;
            }
            else
            {
                btn_Analysis_ON.Enabled = false;
                btn_Analysis_ON.BackColor = Color.Gray;
                btn_Analysis_OFF.Enabled = true;
                btn_Analysis_OFF.BackColor = Color.LightCoral;
            }
            SayAnalysis("Press <ON> to start analysis.");
        }

        private void tp_Analysis_Leave(object sender, EventArgs e)
        {
        }

        private void btn_Analysis_ON_Click(object sender, EventArgs e)
        {
            SayAnalysis("Getting database properties, please wait...");
            // go into offline mode
            btn_Analysis_ON.Enabled = false;
            btn_Analysis_ON.BackColor = Color.Gray;
            btn_Analysis_OFF.Enabled = true;
            btn_Analysis_OFF.BackColor = Color.LightCoral;
            btn_Map_PlayPause.Enabled = false;
            LifeMode = AIRSCOUTLIFEMODE.HISTORY;
            PlayMode = AIRSCOUTPLAYMODE.PAUSE;
            if (!bw_Analysis_DataGetter.IsBusy)
                bw_Analysis_DataGetter.RunWorkerAsync();
        }

        private void btn_Analysis_OFF_Click(object sender, EventArgs e)
        {
            if (bw_Analysis_FileSaver.IsBusy)
                bw_Analysis_FileSaver.CancelAsync();
            if (bw_Analysis_FileLoader.IsBusy)
                bw_Analysis_FileLoader.CancelAsync();
            if (bw_Analysis_DataGetter.IsBusy)
            {
                SayAnalysis("Cancelling background thread, please wait...");
                bw_Analysis_DataGetter.CancelAsync();
            }
            else
            {
                btn_Analysis_ON.Enabled = true;
                btn_Analysis_ON.BackColor = Color.YellowGreen;
            }
            lock (AllPositions)
            {
                AllPositions.Clear();
            }
            GC.Collect();
            // go into online mode
            btn_Analysis_OFF.Enabled = false;
            btn_Analysis_OFF.BackColor = Color.Gray;
            btn_Analysis_Planes_Load.Enabled = false;
            btn_Analysis_Planes_Save.Enabled = false;
            btn_Analysis_Planes_Clear.Enabled = false;
            btn_Analysis_Planes_History.Enabled = false;
            btn_Analysis_Planes_ShowTraffic.Enabled = false;
            btn_Analysis_Path_SaveToFile.Enabled = false;
            btn_Analysis_CrossingHistory.Enabled = false;
            btn_Analysis_Plane_History.Enabled = false; 
            btn_Analysis_Rewind.Enabled = false;
            btn_Analysis_Back.Enabled = false;
            btn_Analysis_Pause.Enabled = false;
            btn_Analysis_Forward.Enabled = false;
            btn_Analysis_FastForward.Enabled = false;
            sb_Analysis_Play.Enabled = false;
            dtp_Analysis_MinValue.Enabled = false;
            dtp_Analysis_MaxValue.Enabled = false;
            btn_Map_PlayPause.Enabled = true;
            LifeMode = AIRSCOUTLIFEMODE.LIFE;
            PlayMode = AIRSCOUTPLAYMODE.PAUSE;
            UpdateStatus();
            btn_Map_PlayPause.Focus();
        }

        private void btn_Analysis_Planes_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog Dlg = new SaveFileDialog();
                Dlg.AddExtension = true;
                Dlg.Filter = "Java Script Object Notation File|*.json|Comma Separated Values|*.csv";
                Dlg.DefaultExt = "json";
                Dlg.FileName = "Plane Positions " + dtp_Analysis_MinValue.Value.ToString("yyyy_MM_dd_HH_mm_ss") + " to " + dtp_Analysis_MaxValue.Value.ToString("yyyy_MM_dd_HH_mm_ss");
                Dlg.InitialDirectory = TmpDirectory;
                Dlg.OverwritePrompt = true;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!bw_Analysis_FileSaver.IsBusy)
                    {
                        btn_Analysis_Planes_Save.Enabled = false;
                        bw_Analysis_FileSaver.RunWorkerAsync(Dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void btn_Analysis_Planes_Load_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog Dlg = new OpenFileDialog();
                Dlg.Filter = "Java Script Object Notation File|*.json|Comma Separated Values|*.csv";
                Dlg.DefaultExt = "json";
                Dlg.CheckFileExists = true;
                Dlg.CheckPathExists = true;
                Dlg.Multiselect = false;
                Dlg.InitialDirectory = TmpDirectory;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!bw_Analysis_FileLoader.IsBusy)
                    {
                        btn_Analysis_Planes_Load.Enabled = false;
                        bw_Analysis_FileLoader.RunWorkerAsync(Dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void btn_Analysis_Planes_Clear_Click(object sender, EventArgs e)
        {
            AircraftPositionData.Database.AircraftPositionDeleteAll();
        }

        private void btn_Analysis_Planes_History_Click(object sender, EventArgs e)
        {
            HistoryFromURLDlg Dlg = new HistoryFromURLDlg();
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                btn_Analysis_Planes_History.Enabled = false;
                bw_HistoryDownloader.RunWorkerAsync(Properties.Settings.Default.Analysis_History_Date);
            }
        }

        private void btn_Analysis_Planes_ShowTraffic_Click(object sender, EventArgs e)
        {
            TrafficDlg Dlg = new TrafficDlg();
            Dlg.ShowDialog();
        }

        private void btn_Analysis_Path_SaveToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.CheckPathExists = true;
            Dlg.AddExtension = true;
            Dlg.DefaultExt = "csv";
            Dlg.Filter = "Comma Separated Values *.csv |csv";
            Dlg.FileName = "Path Information " + Properties.Settings.Default.MyCall.Replace("/", "_") + " to " + Properties.Settings.Default.DXCall.Replace("/", "_");
            Dlg.InitialDirectory = TmpDirectory;
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // calculate propagation path

                    // check and update station database
                    LocationDesignator myloc = LocationFindOrUpdateOrCreate(Properties.Settings.Default.MyCall, Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
                    Properties.Settings.Default.MyElevation = myloc.Elevation;

                    // get qrv info or create default
                    QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(myloc.Call, myloc.Loc, Properties.Settings.Default.Band);
                    // set qrv defaults if zero
                    if (myqrv.AntennaHeight == 0)
                        myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                    if (myqrv.AntennaGain == 0)
                        myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                    if (myqrv.Power == 0)
                        myqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);
                    // check if there are a valid DX settings
                    if (!Callsign.Check(Properties.Settings.Default.DXCall) ||
                        !GeographicalPoint.Check(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon))
                        return;

                    // OK valid, lets continue
                    // check and update station database
                    LocationDesignator dxloc = LocationFindOrUpdateOrCreate(Properties.Settings.Default.DXCall, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon);
                    Properties.Settings.Default.DXElevation = dxloc.Elevation;
                    // get qrv info or create default
                    QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxloc.Call, dxloc.Loc, Properties.Settings.Default.Band);
                    // set qrv defaults if zero
                    if (dxqrv.AntennaHeight == 0)
                        dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                    if (dxqrv.AntennaGain == 0)
                        dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                    if (dxqrv.Power == 0)
                        dxqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);

                    // find local obstruction, if any
                    LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(myloc.Lat, myloc.Lon, Properties.Settings.Default.ElevationModel);
                    double mybearing = LatLon.Bearing(myloc.Lat, myloc.Lon, dxloc.Lat, dxloc.Lon);
                    double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;

                    // try to find elevation path in database or create new one and store
                    ElevationPathDesignator epath = ElevationData.Database.ElevationPathFindOrCreateFromLatLon(
                        null,
                        myloc.Lat,
                        myloc.Lon,
                        dxloc.Lat,
                        dxloc.Lon,
                        ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                        Properties.Settings.Default.ElevationModel);
                    // add additional info to ppath
                    epath.Location1 = myloc;
                    epath.Location2 = dxloc;
                    epath.QRV1 = myqrv;
                    epath.QRV2 = dxqrv;

                    // try to find propagation path in database or create new one and store
                    PropagationPathDesignator ppath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                        null,
                        myloc.Lat,
                        myloc.Lon,
                        GetElevation(myloc.Lat, myloc.Lon) + myqrv.AntennaHeight,
                        dxloc.Lat,
                        dxloc.Lon,
                        GetElevation(dxloc.Lat, dxloc.Lon) + dxqrv.AntennaHeight,
                        Bands.ToGHz(Properties.Settings.Default.Band),
                        LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor,
                        Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance,
                        ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                        Properties.Settings.Default.ElevationModel,
                        myobstr);

                    // add additional info to ppath
                    ppath.Location1 = myloc;
                    ppath.Location2 = dxloc;
                    ppath.QRV1 = myqrv;
                    ppath.QRV2 = dxqrv;
                    using (StreamWriter sw = new StreamWriter(Dlg.FileName))
                    {
                        sw.WriteLine("Distance[km];Lat[deg];Lon[deg];Elevation[m]; Min_h1[m]; Min_h2[m]; Min_h[m]; Max_h[m]; F1[m];eps1[deg];eps2[deg];eps1_min[deg];eps2_min[deg];ElevationModel");
                        for (int i = 0; i < epath.Count; i++)
                        {
                            double dist = (double)i * epath.StepWidth / 1000.0;
                            PropagationPoint p = ppath.GetInfoPoint(dist);
                            sw.WriteLine(dist.ToString() + ";" +
                                p.Lat.ToString() + ";" +
                                p.Lon.ToString() + ";" +
                                epath.Path[i].ToString() + ";" +
                                p.H1.ToString() + ";" +
                                p.H2.ToString() + ";" +
                                Math.Max(p.H1, p.H2).ToString() + ";" +
                                Properties.Settings.Default.Planes_MaxAlt.ToString() + ";" +
                                p.F1.ToString() + ";" +
                                (Propagation.EpsilonFromHeights(ppath.h1, dist, epath.Path[i],ppath.Radius) / Math.PI * 180.0).ToString() + ";" +
                                (Propagation.EpsilonFromHeights(ppath.h2, ppath.Distance-dist, epath.Path[i], ppath.Radius) / Math.PI * 180.0).ToString() + ";" +
                                (ppath.Eps1_Min / Math.PI * 180.0).ToString() + ";" +
                                (ppath.Eps2_Min / Math.PI * 180.0).ToString());
                    }
                }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage("Error while saving path to file [" + Dlg.FileName + "]:" + ex.ToString(), LogLevel.Error);
                }
            }
        }

        private void btn_Analysis_CrossingHistory_Click(object sender, EventArgs e)
        {
            if (dtp_Analysis_MaxValue.Value <= dtp_Analysis_MinValue.Value)
                return;
            if (Time_Offline_Interval <= 0)
                Time_Offline_Interval = 1;
            if ((AllPositions == null) || (AllPositions.Count == 0))
                return;
            CrossingHistoryDlg Dlg = new CrossingHistoryDlg(dtp_Analysis_MinValue.Value, dtp_Analysis_MaxValue.Value, Time_Offline_Interval, ref AllPositions);
            Dlg.ShowDialog();
        }

        private void sb_Analysis_Play_SizeChanged(object sender, EventArgs e)
        {
            // handle change of scrollbar
            // set scrollbar markers
            // reduce them to one marker per pixel
            // get initial scrollbar width and build array
            int[] sb = new int[sb_Analysis_Play.Width];
            double stepwidth = (double)sb.Length / (sb_Analysis_Play.Maximum - sb_Analysis_Play.Minimum);
            foreach (DateTime dt in AllLastUpdated)
            {
                int index = (int)((dt - dtp_Analysis_MinValue.Value).TotalSeconds * stepwidth);
                int i = (int)(dt - dtp_Analysis_MinValue.Value).TotalSeconds;
                if ((i > sb_Analysis_Play.Minimum) && (i < sb_Analysis_Play.Maximum))
                    sb[index] = i;
            }
            sb_Analysis_Play.BackgroundMarkers = sb.ToList<int>();
        }

        private void gb_Analysis_Player_SizeChanged(object sender, EventArgs e)
        {
            // handle change of size --> arrange elements
            sb_Analysis_Play.Left = dtp_Analysis_MinValue.Right + 10;
            sb_Analysis_Play.Width = dtp_Analysis_MaxValue.Left - dtp_Analysis_MinValue.Width - 20;
            sb_Analysis_Play.Height = dtp_Analysis_MinValue.Height;
        }

        private void btn_Analysis_Rewind_Click(object sender, EventArgs e)
        {
            PlayMode = AIRSCOUTPLAYMODE.FASTBACK;
            Time_Offline_Interval -= 10;
            if (Time_Offline_Interval < 1)
                Time_Offline_Interval = 1;
            tb_Analysis_Stepwidth.Text = Time_Offline_Interval.ToString();
        }

        private void btn_Analysis_Back_Click(object sender, EventArgs e)
        {
            PlayMode = AIRSCOUTPLAYMODE.BACK;
            Time_Offline_Interval -= 1;
            if (Time_Offline_Interval < 1)
                Time_Offline_Interval = 1;
            tb_Analysis_Stepwidth.Text = Time_Offline_Interval.ToString();
        }

        private void btn_Analysis_Pause_Click(object sender, EventArgs e)
        {
            PlayMode = AIRSCOUTPLAYMODE.PAUSE;
            Time_Offline_Interval = 0;
            tb_Analysis_Stepwidth.Text = Time_Offline_Interval.ToString();
        }

        private void btn_Analysis_Forward_Click(object sender, EventArgs e)
        {
            PlayMode = AIRSCOUTPLAYMODE.FORWARD;
            Time_Offline_Interval += 1;
            if (Time_Offline_Interval > 3600)
                Time_Offline_Interval = 3600;
            tb_Analysis_Stepwidth.Text = Time_Offline_Interval.ToString();
        }

        private void btn_Analysis_FastForward_Click(object sender, EventArgs e)
        {
            PlayMode = AIRSCOUTPLAYMODE.FASTFORWARD;
            Time_Offline_Interval += 10;
            if (Time_Offline_Interval > 3600)
                Time_Offline_Interval = 3600;
            tb_Analysis_Stepwidth.Text = Time_Offline_Interval.ToString();
        }

        private void sb_Analysis_Play_Scroll(object sender, ScrollEventArgs e)
        {
//            PlayMode = AIRSCOUTPLAYMODE.PAUSE;
//            Time_Offline_Interval = 0;
            tb_Analysis_Stepwidth.Text = Time_Offline_Interval.ToString();
            Properties.Settings.Default.Time_Offline = dtp_Analysis_MinValue.Value.AddSeconds(sb_Analysis_Play.Value);
            if (Properties.Settings.Default.Time_Offline < dtp_Analysis_MinValue.Value)
                Properties.Settings.Default.Time_Offline = dtp_Analysis_MinValue.Value;
            if (Properties.Settings.Default.Time_Offline > dtp_Analysis_MaxValue.Value)
                Properties.Settings.Default.Time_Offline = dtp_Analysis_MaxValue.Value;
            CurrentTime = Properties.Settings.Default.Time_Offline;
            tb_Analysis_Time.Text = CurrentTime.ToString("yyyy-MM-hh HH:mm:ss");
            UpdatePlanes();

        }

        private void dtp_Analysis_MinValue_ValueChanged(object sender, EventArgs e)
        {
            // check bounds
            if (dtp_Analysis_MinValue.Value < History_OldestEntry)
                dtp_Analysis_MinValue.Value = History_OldestEntry;
            if (dtp_Analysis_MinValue.Value > History_YoungestEntry)
                dtp_Analysis_MinValue.Value = History_YoungestEntry;
            UpdatePlayer();
        }

        private void dtp_Analysis_MaxValue_ValueChanged(object sender, EventArgs e)
        {
            // check bounds
            if (dtp_Analysis_MinValue.Value < History_OldestEntry)
                dtp_Analysis_MinValue.Value = History_OldestEntry;
            if (dtp_Analysis_MinValue.Value > History_YoungestEntry)
                dtp_Analysis_MinValue.Value = History_YoungestEntry;
            UpdatePlayer();
        }

        private void btn_Analysis_Plane_History_Click(object sender, EventArgs e)
        {
            PlaneHistoryDlg Dlg = new PlaneHistoryDlg(dtp_Analysis_MinValue.Value, dtp_Analysis_MaxValue.Value, TmpDirectory);
            Dlg.ShowDialog();
        }

        #endregion

        #endregion

        #region Map Tab Control

        private void tp_Map_Enter(object sender, EventArgs e)
        {
            if (PathMode == AIRSCOUTPATHMODE.SINGLE)
                sc_Map.Panel2Collapsed = false;
            else if (PathMode == AIRSCOUTPATHMODE.MULTI)
                sc_Map.Panel2Collapsed = true;
        }

        private void tp_News_Enter(object sender, EventArgs e)
        {
            sc_Map.Panel2Collapsed = true;
        }

        #endregion

        #endregion

        #region Background Workers

        #region PlaneFeed

        private void bw_PlaneFeed_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == (int)PROGRESS.ERROR)
            {
                if (e.UserState == null)
                    return;
                Say((string)e.UserState);
            }
            else if (e.ProgressPercentage == (int)PROGRESS.STATUS)
            {
                if (e.UserState == null)
                    return;
                Say((string)e.UserState);
            }
            else if (e.ProgressPercentage == (int)PROGRESS.PLANES)
            {
                List<PlaneInfo> planes = (List<PlaneInfo>)e.UserState;
                Planes.BulkInsertOrUpdateIfNewer(planes);
            }
        }

        #endregion

        #region WinTestReceive

        // suppress console output when in debug mode, needs "Just my code" set in Tools\Options\Debugging
        [System.Diagnostics.DebuggerNonUserCode]
        private void bw_WinTestReceive_DoWork(object sender, DoWorkEventArgs e)
        {
            // Background thread for receiving Win-Test messages
            // listens to the UDP broadcasts
            // use the ReportProgress method to
            // return status code and received message
            // status values are: <0 = error orrcured (not supported yet)
            //                     0 = function calls OK, but no bytes received
            //                    >0 = function calls OK, number of bytes received
            Log.WriteMessage("Started.");
            if (Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = "bw_WinTestReceive";
            // Get own IP addresses
            string hostname;
            IPAddress[] hostaddresses = new IPAddress[256];
            try
            {
                hostname = Dns.GetHostName();
                hostaddresses = Dns.GetHostAddresses(hostname);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            // initialize UDP socket
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, Properties.Settings.Default.Server_Port);
            UdpClient u = new UdpClient();
            u.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            u.Client.ReceiveTimeout = 1000;
            try
            {
                u.Client.Bind(ep);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }

            // receive Win-Test messages in a loop
            while (!bw_WinTestReceive.CancellationPending)
            {
                try
                {
                    // receive bytes in blocking mode
                    // handle the receive timeout and cancellation
                    try
                    {
                        byte[] data = u.Receive(ref ep);
                        wtMessage Msg = new wtMessage(data);
                        // check if message is directed to server
                        if (Msg.Dst == Properties.Settings.Default.Server_Name)
                            DispatchWinTestMsg(Msg);
                    }
                    catch (SocketException ex)
                    {
                        // do nothing
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            Log.WriteMessage("Finished.");
        }

        private void ASShowPath(wtMessage msg)
        {
            // a show path message received
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                if (PlayMode != AIRSCOUTPLAYMODE.PAUSE)
                    Pause();
                // set single mode
                tc_Control.SelectedTab = tp_Control_Single;
                PathMode = AIRSCOUTPATHMODE.SINGLE;
                string[] a = msg.Data.Split(',');
                string qrgstr = a[0].Replace("\"", "");
                string mycallstr = a[1].Replace("\"", "");
                string myloc = a[2].Replace("\"", "").Substring(0, 6);
                double mylat = MaidenheadLocator.LatFromLoc(myloc);
                double mylon = MaidenheadLocator.LonFromLoc(myloc);
                string dxcallstr = a[3].Replace("\"", "");
                string dxloc = a[4].Replace("\"", "").Substring(0, 6);
                double dxlat = MaidenheadLocator.LatFromLoc(dxloc);
                double dxlon = MaidenheadLocator.LonFromLoc(dxloc);
                if (Callsign.Check(mycallstr) &&
                    Callsign.Check(dxcallstr) &&
                    MaidenheadLocator.Check(myloc) &&
                    MaidenheadLocator.Check(dxloc))
                {
                    Properties.Settings.Default.MyCall = mycallstr;
                    Properties.Settings.Default.MyLat = mylat;
                    Properties.Settings.Default.MyLon = mylon;
                    Properties.Settings.Default.MyHeight = 10;
                    Properties.Settings.Default.DXCall = dxcallstr;
                    Properties.Settings.Default.DXLat = dxlat;
                    Properties.Settings.Default.DXLon = dxlon;
                    Properties.Settings.Default.DXHeight = 10;
                    Properties.Settings.Default.MyElevation = GetElevation(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
                    LocationDesignator info = StationData.Database.LocationFind(mycallstr);
                    if ((info != null) && (MaidenheadLocator.LocFromLatLon(info.Lat, info.Lon, false, 3) == myloc))
                    {
                        // loc is matching with database --> use detailed info then
                        Properties.Settings.Default.MyLat = info.Lat;
                        Properties.Settings.Default.MyLon = info.Lon;
                        Properties.Settings.Default.MyElevation = GetElevation(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
                    }
                    else
                    {
                        // update database if not found or locator does not match
                        UpdateLocation(mycallstr, mylat, mylon, GEOSOURCE.FROMLOC);
                    }
                    info = StationData.Database.LocationFind(dxcallstr);
                    if ((info != null) && (MaidenheadLocator.LocFromLatLon(info.Lat, info.Lon, false, 3) == dxloc))
                    {
                        // loc is matching with database --> use detailed info then
                        Properties.Settings.Default.DXLat = info.Lat;
                        Properties.Settings.Default.DXLon = info.Lon;
                        Properties.Settings.Default.DXElevation = GetElevation(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon);
                    }
                    else
                    {
                        // update database if not found or locator does not match
                        UpdateLocation(dxcallstr, dxlat, dxlon, GEOSOURCE.FROMLOC);
                    }
                    if (qrgstr == "500000")
                        Properties.Settings.Default.Band = BAND.B50M;
                    if (qrgstr == "700000")
                        Properties.Settings.Default.Band = BAND.B70M;
                    if (qrgstr == "1440000")
                        Properties.Settings.Default.Band = BAND.B144M;
                    if (qrgstr == "4320000")
                        Properties.Settings.Default.Band = BAND.B432M;
                    if (qrgstr == "12960000")
                        Properties.Settings.Default.Band = BAND.B1_2G;
                    if (qrgstr == "23200000")
                        Properties.Settings.Default.Band = BAND.B2_3G;
                    if (qrgstr == "34000000")
                        Properties.Settings.Default.Band = BAND.B3_4G;
                    if (qrgstr == "57600000")
                        Properties.Settings.Default.Band = BAND.B5_7G;
                    if (qrgstr == "103680000")
                        Properties.Settings.Default.Band = BAND.B10G;
                    if (qrgstr == "240480000")
                        Properties.Settings.Default.Band = BAND.B24G;
                    if (qrgstr == "470880000")
                        Properties.Settings.Default.Band = BAND.B47G;
                    if (qrgstr == "760320000")
                        Properties.Settings.Default.Band = BAND.B76G;
                    UpdateStatus();
                    Play();
                    // try different methods to bring the window to front under WinXP and Win7
                    bool max = this.WindowState == FormWindowState.Maximized;
                    // try to restore window from minimized and normal state --> normal state
                    this.TopMost = true;
                    SetForegroundWindow(this.Handle);
                    ShowWindow(this.Handle, ShowWindowCommands.ShowNoActivate);
                    this.BringToFront();
                    this.TopMost = false;
                    // maximize it if it was maximized before
                    if (max)
                        this.WindowState = FormWindowState.Maximized;
                    // set antenna direction 
                    double az = LatLon.Bearing(Properties.Settings.Default.MyLat,
                        Properties.Settings.Default.MyLon,
                        Properties.Settings.Default.DXLat,
                        Properties.Settings.Default.DXLon);
                    // set tracking values
                    Properties.Settings.Default.Track_SetAz = az;
                    Properties.Settings.Default.Track_SetEl = 0;
                }
                st.Stop();
                Log.WriteMessage("Processing ASSHOWPATH[" + mycallstr + "," + dxcallstr + "]: " + st.ElapsedMilliseconds.ToString() + " ms.");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }

        }

        private void ASSetPath(wtMessage msg)
        {
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                string[] a = msg.Data.Split(',');
                int qrg = 1296;
                int.TryParse(a[0].Replace("\"", ""), out qrg);
                qrg = qrg / 10000;
                BAND band = BAND.BNONE;
                try
                {
                    band = (BAND)qrg;
                }
                catch
                {
                    // do nothing
                }
                string mycallstr = a[1].Replace("\"", "");
                string mylocstr = a[2].Replace("\"", "").Substring(0, 6);
                string dxcallstr = a[3].Replace("\"", "");
                string dxlocstr = a[4].Replace("\"", "").Substring(0, 6);
                int count = 0;
                // return on failure
                if (!Callsign.Check(mycallstr))
                    return;
                if (!Callsign.Check(dxcallstr))
                    return;
                if (!MaidenheadLocator.Check(mylocstr))
                    return;
                if (!MaidenheadLocator.Check(dxlocstr))
                    return;
                // get my location info --> if loc is matching, use precise information automatically
                LocationDesignator myloc = StationData.Database.LocationFindOrCreate(mycallstr, mylocstr);
                // get my QRV info
                QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(mycallstr, mylocstr, band);
                // set qrv defaults if zero
                if (myqrv.AntennaHeight == 0)
                    myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                if (myqrv.AntennaGain == 0)
                    myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                if (myqrv.Power == 0)
                    myqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);
                // get dx location info --> if loc is matching, use precise information automatically
                LocationDesignator dxloc = LocationFindOrCreate(dxcallstr, dxlocstr);
                // get dx QRV info
                QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxcallstr, dxlocstr, band);
                // set qrv defaults if zero
                if (dxqrv.AntennaHeight == 0)
                    dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
                if (dxqrv.AntennaGain == 0)
                    dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
                if (dxqrv.Power == 0)
                    dxqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);
                // find local obstruction, if any
                LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(myloc.Lat, myloc.Lon, Properties.Settings.Default.ElevationModel);
                double mybearing = LatLon.Bearing(myloc.Lat, myloc.Lon, dxloc.Lat, dxloc.Lon);
                double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;

                // try to find propagation path in database or create new one and store
                PropagationPathDesignator ppath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                    null,
                    myloc.Lat,
                    myloc.Lon,
                    GetElevation(myloc.Lat, myloc.Lon) + myqrv.AntennaHeight,
                    dxloc.Lat,
                    dxloc.Lon,
                    GetElevation(dxloc.Lat, dxloc.Lon) + dxqrv.AntennaHeight,
                    Bands.ToGHz(Properties.Settings.Default.Band),
                    LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor,
                    Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance,
                    ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                    Properties.Settings.Default.ElevationModel,
                    myobstr);
                List<PlaneInfo> allplanes = Planes.GetAll(DateTime.UtcNow, Properties.Settings.Default.Planes_Position_TTL);
                List<PlaneInfo> nearestplanes = AircraftData.Database.GetNearestPlanes(DateTime.UtcNow, ppath, allplanes, Properties.Settings.Default.Planes_Filter_Max_Circumcircle, Properties.Settings.Default.Path_Band_Settings[band].MaxDistance, Properties.Settings.Default.Planes_MaxAlt);
                count = 0;
                string planes = "";
                wtMessage SendMsg = new wtMessage(WTMESSAGES.ASNEAREST,
                    Properties.Settings.Default.Server_Name,
                    msg.Src,
                    DateTime.UtcNow.ToString("u") + "," +
                    mycallstr + "," +
                    mylocstr + "," +
                    dxcallstr + "," +
                    dxlocstr + ",");
                if ((nearestplanes != null) && (nearestplanes.Count() > 0))
                {
                    foreach (PlaneInfo planeinfo in nearestplanes)
                    {
                        if ((planeinfo.IntPoint != null) && (planeinfo.Potential > 0))
                        {
                            int mins = 0;
                            if (planeinfo.Speed > 0)
                                mins = (int)(planeinfo.IntQRB / (double)planeinfo.Speed / 1.852 * 60.0);
                            planes = planes + planeinfo.Call + "," + PlaneCategories.GetShortStringValue(planeinfo.Category) + "," + ((int)planeinfo.IntQRB).ToString() + "," + planeinfo.Potential.ToString() + "," + mins.ToString() + ",";
                            count++;
                        }
                    }
                }
                planes = planes.TrimEnd(',');
                SendMsg.Data = SendMsg.Data + count.ToString() + "," + planes;
                SendMsg.Data = SendMsg.Data.TrimEnd(',');
                SendMsg.HasChecksum = true;
                // send message
                UdpClient client = new UdpClient();
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                client.Client.ReceiveTimeout = 10000;    // 10s Receive timeout
                IPEndPoint groupEp = new IPEndPoint(IPAddress.Broadcast, Properties.Settings.Default.Server_Port);
                client.Connect(groupEp);
                byte[] b = SendMsg.ToBytes();
                client.Send(b, b.Length);
                client.Close();
                st.Stop();
                Log.WriteMessage("Processing ASSETPATH[" + mycallstr + "," + dxcallstr + "]: " + count.ToString() + " Plane(s), " + st.ElapsedMilliseconds.ToString() + " ms.");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void ASWatchlist(wtMessage msg)
        {
            if (!Properties.Settings.Default.Watchlist_SyncWithKST)
                return;
            // maintain watchlist
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                // mark all watchlist items to remove wich are not currently tracked
                foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
                {
                    // nasty!! Should never be null!
                    if (item == null)
                        continue;
                    if (!item.Checked)
                        item.Remove = true;
                }
                // split message
                string[] a = msg.Data.Split(',');
                // ignore band string so far
                string qrgstr = a[0];
                // iterate through calls
                for (int i = 1; i < a.Length - 1; i += 2)
                {
                    // get call
                    string dxcallstr = a[i].Trim().ToUpper();
                    // get loc
                    string dxlocstr = a[i + 1].Trim().ToUpper();
                    // skip when invalid
                    if (!Callsign.Check(dxcallstr))
                        continue;
                    if (!MaidenheadLocator.Check(dxlocstr))
                        continue;
                    // skip own callsign
                    if (dxcallstr == Callsign.Cut(Properties.Settings.Default.MyCall))
                        continue;
                    // find or create call & loc combination in station database
                    LocationDesignator dxcall = StationData.Database.LocationFindOrCreate(dxcallstr, dxlocstr);
                    double qrb = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, dxcall.Lat, dxcall.Lon);
                    // add to watchlist
                    int index = Properties.Settings.Default.Watchlist.IndexOf(dxcallstr, dxlocstr);
                    // reset remove flag if item found, add to watchlist if not
                    if (index >= 0)
                    {
                        Properties.Settings.Default.Watchlist[index].Remove = false;
                        Properties.Settings.Default.Watchlist[index].OutOfRange = qrb > Properties.Settings.Default.Path_MaxLength;
                    }
                    else
                    {
                        Properties.Settings.Default.Watchlist.Add(new WatchlistItem(dxcallstr, dxlocstr, qrb > Properties.Settings.Default.Path_MaxLength));
                    }
                }

                // remove all items from watchlist which are not logged in anymore
                Properties.Settings.Default.Watchlist.RemoveAll(item => item.Remove);

                // update watchlist in map
                UpdateWatchlistInMap();

                // update ListView control
                RefreshWatchlistView();
                st.Stop();
                Log.WriteMessage("Processing ASWATCHLIST: " + Properties.Settings.Default.Watchlist.Count.ToString() + " call(s), " + st.ElapsedMilliseconds.ToString() + " ms.");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void ASAddWatch(wtMessage msg)
        {
            // abort if in wtKST sync mode
            if (Properties.Settings.Default.Watchlist_SyncWithKST)
                return;

            // maintain watchlist
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();

                // check for malformatted message
                if (!msg.Data.Contains(","))
                    return;

                // split message
                string[] a = msg.Data.Split(',');

                // check for malformatted message
                if ((a.Length - 1) % 3 != 0)
                    return;

                // ignore band string so far
                string qrgstr = a[0];
                // iterate through calls
                for (int i = 1; i < a.Length - 2; i += 3)
                {
                    // get call
                    string dxcallstr = a[i].Trim().ToUpper();
                    // get loc
                    string dxlocstr = a[i + 1].Trim().ToUpper();
                    // get checked
                    string checkstr = a[i + 2].Trim().ToUpper();

                    // skip when invalid
                    if (!Callsign.Check(dxcallstr))
                        continue;

                    // empty loc --> try to find one in the database
                    if (String.IsNullOrEmpty(dxlocstr))
                    {
                        LocationDesignator ld = StationData.Database.LocationFindLastRecent(dxcallstr);
                        if (ld != null)
                            dxlocstr = ld.Loc;
                    }
                    // skip when loc is still invalid
                    if (!MaidenheadLocator.Check(dxlocstr))
                        continue;

                    // skip own callsign
                    if (dxcallstr == Callsign.Cut(Properties.Settings.Default.MyCall))
                        continue;

                    // find or create call & loc combination in station database
                    LocationDesignator dxcall = StationData.Database.LocationFindOrCreate(dxcallstr, dxlocstr);
                    double qrb = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, dxcall.Lat, dxcall.Lon);

                    // add to watchlist
                    int index = Properties.Settings.Default.Watchlist.IndexOf(dxcallstr, dxlocstr);
                    // reset remove flag if item found, add to watchlist if not
                    if (index >= 0)
                    {
                        Properties.Settings.Default.Watchlist[index].Remove = false;
                        Properties.Settings.Default.Watchlist[index].OutOfRange = qrb > Properties.Settings.Default.Path_MaxLength;
                        if (!Properties.Settings.Default.Watchlist[index].OutOfRange)
                        {
                            Properties.Settings.Default.Watchlist[index].Checked = checkstr == "1";
                        }
                    }
                    else
                    {
                        if (qrb > Properties.Settings.Default.Path_MaxLength)
                        {
                            Properties.Settings.Default.Watchlist.Add(new WatchlistItem(dxcallstr, dxlocstr, true));
                        }
                        else
                        {
                            Properties.Settings.Default.Watchlist.Add(new WatchlistItem(dxcallstr, dxlocstr, false, checkstr == "1"));
                        }
                    }
                }

                // update watchlist in map
                UpdateWatchlistInMap();

                // update ListView control
                RefreshWatchlistView();

                // refresh paths
                UpdatePaths();

                // start playing if at least one active watch
                if (Properties.Settings.Default.Watchlist.Find(item => item.Checked) != null)
                {
                    if (PlayMode == AIRSCOUTPLAYMODE.PAUSE)
                        Play();
                }

                st.Stop();
                Log.WriteMessage("Processing ASADDWATCH: " + st.ElapsedMilliseconds.ToString() + " ms.");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void ASRemoveWatch(wtMessage msg)
        {
            // abort if in wtKST sync mode
            if (Properties.Settings.Default.Watchlist_SyncWithKST)
                return;

            // maintain watchlist
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();

                // check for empty call sign list --> remove all
                if (!msg.Data.Contains(","))
                {
                    // mark all watchlist items to remove wich are not currently tracked
                    foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
                    {
                        // nasty!! Should never be null!
                        if (item == null)
                            continue;
                        item.Remove = true;
                    }
                }
                else
                {
                    // split message
                    string[] a = msg.Data.Split(',');
                    // ignore band string so far
                    string qrgstr = a[0];
                    for (int i = 1; i < a.Length; i++)
                    {
                        // get call
                        string dxcallstr = a[i].Trim().ToUpper();

                        // mark all watchlist items to remove wich are not currently tracked
                        foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
                        {
                            // nasty!! Should never be null!
                            if (item == null)
                                continue;
                            if (item.Call == dxcallstr)
                                item.Remove = true;
                        }
                    }
                }

                // remove all items from watchlist which marked as remove
                Properties.Settings.Default.Watchlist.RemoveAll(item => item.Remove);

                // update watchlist in map
                UpdateWatchlistInMap();

                // update ListView control
                RefreshWatchlistView();

                // refresh paths
                UpdatePaths();

                // stop playing if no more active watches
                if (Properties.Settings.Default.Watchlist.Find(item => item.Checked) == null)
                { 
                    if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                        Pause();
                }

                st.Stop();
                Log.WriteMessage("Processing ASREMOVEWATCH: " + st.ElapsedMilliseconds.ToString() + " ms.");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void DispatchWinTestMsg(wtMessage msg)
        {
            // a Win-Test message was received by the background thread
            if (msg.Msg == WTMESSAGES.ASSHOWPATH)
            {
                // a show path message received
                // dispatch it to main thread 
                bw_WinTestReceive.ReportProgress(1, msg);
            }
            else if (msg.Msg == WTMESSAGES.ASSETPATH)
            {
                // a path calculation message received
                // keep it in the background thread and calculate
                ASSetPath(msg);
            }
            else if (msg.Msg == WTMESSAGES.ASWATCHLIST)
            {
                // set users list on watchlist
                // dispatch it to main thread 
                bw_WinTestReceive.ReportProgress(1, msg);
            }
            else if (msg.Msg == WTMESSAGES.ASADDWATCH)
            {
                // add call to watchlist
                // dispatch it to main thread 
                bw_WinTestReceive.ReportProgress(1, msg);
            }
            else if (msg.Msg == WTMESSAGES.ASREMOVEWATCH)
            {
                // add call to watchlist
                // dispatch it to main thread 
                bw_WinTestReceive.ReportProgress(1, msg);
            }
        }

        private void bw_WinTestReceive_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // a Win-Test message was received by the background thread
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            wtMessage msg = (wtMessage)e.UserState;
            if (msg.Msg == WTMESSAGES.ASSHOWPATH)
                ASShowPath(msg);
            else if (msg.Msg == WTMESSAGES.ASWATCHLIST)
                ASWatchlist(msg);
            else if (msg.Msg == WTMESSAGES.ASADDWATCH)
                ASAddWatch(msg);
            else if (msg.Msg == WTMESSAGES.ASREMOVEWATCH)
                ASRemoveWatch(msg);
        }

        private void bw_WinTestReceive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

        # region SpecLabReceive

        // suppress console output when in debug mode, needs "Just my code" set in Tools\Options\Debugging
        //        [System.Diagnostics.DebuggerNonUserCode]
        private void bw_SpecLab_Receive_DoWork(object sender, DoWorkEventArgs e)
        {
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_SpecLabReceive";
            Log.WriteMessage("Started.");
            // get the update interval
            int interval = System.Convert.ToInt32(Properties.Settings.Default.SpecLab_Update) * 1000;
            // check for minimum
            if (interval < 1000)
                interval = 1000;
            bw_SpecLab_Receive.ReportProgress(0);
            int maxcount = 60;
            List<SignalLevelDesignator> ads = new List<SignalLevelDesignator>();
            while (!bw_SpecLab_Receive.CancellationPending)
            {
                Thread.Sleep(interval);
                // do nothing when not in play mode
                if ((LifeMode != AIRSCOUTLIFEMODE.LIFE) || (PlayMode != AIRSCOUTPLAYMODE.FORWARD))
                    continue;
                try
                {
                    // get boundary frequencies
                    int f1 = Properties.Settings.Default.SpecLab_F1;
                    if (f1 < 0)
                        f1 = 0;
                    if (f1 > 3000)
                        f1 = 3000;
                    int f2 = Properties.Settings.Default.SpecLab_F2;
                    if (f2 < 1)
                        f2 = 1;
                    if (f2 > 3000)
                        f2 = 3000;
                    if (f1 >= f2)
                        f1 = f2 - 1;
                    // get the url
                    string url = Properties.Settings.Default.SpecLab_URL;
                    // get the filename
                    string filename = Properties.Settings.Default.SpecLab_FileName;
                    if (!url.EndsWith("/"))
                        url = url + "/";
                    url = url + filename + "?f1=" + f1.ToString() + "?f2=" + f2.ToString();
                    string msg = "";
                    bw_SpecLab_Receive.ReportProgress(1, "Trying to connect to Spectrum Lab...");
                    WebRequest myWebRequest = WebRequest.Create(url);
                    WebResponse myWebResponse = myWebRequest.GetResponse();
                    Stream ReceiveStream = myWebResponse.GetResponseStream();
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader readStream = new StreamReader(ReceiveStream, encode);
                    string json = readStream.ReadToEnd();
                    if (!json.StartsWith("["))
                        throw (new FormatException("Format Exception: Not a JSON file."));
                    // split the JSON string
                    // a[1] contains the header 
                    // h contains alle header items
                    // d contains all data items
                    string[] a = json.Split('[');
                    a[1] = a[1].Replace(":", ",");
                    string[] h = a[1].Split(',');
                    a[2] = a[2].Replace("\r\n", "");
                    a[2] = a[2].Remove(a[2].IndexOf("]") - 1);
                    a[2] = a[2].Replace(",", ";");
                    a[2] = a[2].Replace(".", ",");
                    double[] d = System.Array.ConvertAll(a[2].Split(';'), new Converter<string, double>(Double.Parse));
                    // get the time stamp
                    long l = (long)System.Convert.ToDouble(h[1], CultureInfo.InvariantCulture);
                    DateTime utc = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    utc = utc.AddSeconds(l);
                    // get number of bins
                    int count = System.Convert.ToInt32(h[13]);
                    msg = "FFT-Data received: " + count.ToString() + "bins. F1=" + f1.ToString() + "Hz, F2=" + f2.ToString() + "Hz";
                    bw_SpecLab_Receive.ReportProgress(1, msg);
                    double max = d.Max();
                    // collect and save maximum if in play mode and NearestPlane != null
                    if ((PlayMode == AIRSCOUTPLAYMODE.FORWARD) && (NearestPlane != null))
                        ads.Add(new SignalLevelDesignator(max, utc));
                    // bulk save maximum to database if maxcount is reached
                    if (ads.Count > maxcount)
                    {
                        SignalData.Database.SignalLevelBulkInsertOrUpdateIfNewer(ads);
                        ads.Clear();
                    }
                    // report maximum
                    bw_SpecLab_Receive.ReportProgress(100, max);
                }
                catch (WebException ex)
                {
                    // do nothing
                }
                catch (SocketException ex)
                {
                    // do nothing
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                    bw_SpecLab_Receive.ReportProgress(-1, ex.Message);
                }
            }
            Log.WriteMessage("Finished.");
        }

        private void bw_SpecLab_Receive_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // an error occured
                    tb_Spectrum_Status.ForeColor = Color.White;
                    tb_Spectrum_Status.BackColor = Color.Red;
                    tb_Spectrum_Status.Text = (string)e.UserState;
                }
                if (e.ProgressPercentage == 0)
                {
                    // a init message occured
                    Spectrum.Points.Clear();
                    SpectrumPointsCount = 0;
                    Spectrum_X.Reset();
                    SpectrumRecord.Points.Clear();
                }
                if (e.ProgressPercentage == 1)
                {
                    // a status message occured
                    tb_Spectrum_Status.ForeColor = Color.Black;
                    tb_Spectrum_Status.BackColor = SystemColors.Control;
                    tb_Spectrum_Status.Text = (string)e.UserState;
                }
                if (e.ProgressPercentage == 100)
                {
                    // FFT - data received
                    if ((LifeMode == AIRSCOUTLIFEMODE.LIFE) && (PlayMode == AIRSCOUTPLAYMODE.FORWARD))
                    {
                        // draw data
                        double max = (double)e.UserState;
                        if (Spectrum.Points.Count >= SpectrumMaxPoints)
                        {
                            double pan = -(Spectrum_X.ScreenMax.X - Spectrum_X.ScreenMin.X) / (double)SpectrumMaxPoints;
                            Spectrum_X.Pan(pan);
                            Spectrum.Points.RemoveAt(0);
                            SpectrumRecord.Points.RemoveAt(0);
                        }
                        // add background area
                        SpectrumRecord.Fill = OxyColor.FromArgb(20, 255, 0, 255);
                        double on, off;
                        if (Spectrum_Y.Minimum < 0)
                        {
                            on = -1000;
                            off = 1000;
                        }
                        else
                        {
                            on = 1000;
                            off = -1000;
                        }
                        if (NearestPlane == null)
                            SpectrumRecord.Points.Add(new DataPoint(SpectrumPointsCount, off));
                        else
                            SpectrumRecord.Points.Add(new DataPoint(SpectrumPointsCount, on));
                        // add signal level point
                        Spectrum.Points.Add(new DataPoint(SpectrumPointsCount, max));
                        SpectrumPointsCount++;
                        // autoscale Y axis
                        double y_min = double.MaxValue;
                        double y_max = double.MinValue;
                        foreach (DataPoint p in Spectrum.Points)
                        {
                            if (p.Y > y_max)
                                y_max = p.Y;
                            if (p.Y < y_min)
                                y_min = p.Y;
                        }
                        // enlarge scaling Y-axis by 10% in both directions
                        double y_diff = (y_max - y_min) * 0.1;
                        Spectrum_Y.Minimum = y_min - y_diff;
                        Spectrum_Y.Maximum = y_max + y_diff;
                        pm_Spectrum.InvalidatePlot(true);
                    }
                }
                // maintain nearest plane map
                if (NearestPlane != null)
                {
                    lbl_Nearest_Call.Text = NearestPlane.Call;
                    lbl_Nearest_Type.Text = NearestPlane.Type;
                    lbl_Nearest_Cat.Text = PlaneCategories.GetStringValue(NearestPlane.Category);
                    lbl_Nearest_Alt.Text = NearestPlane.Alt_m.ToString("F0") + "m";
                    lbl_Nearest_Angle.Text = (NearestPlane.Angle / Math.PI * 180.0).ToString("F0") + "°";
                    lbl_Nearest_Dist.Text = NearestPlane.IntQRB.ToString("F0") + "km";
                    gmo_NearestPlanes.Clear();
                    gmo_NearestPlanes.Markers.Add(CreatePlaneDetailed(NearestPlane, false));
                    gm_Nearest.Position = new PointLatLng(NearestPlane.IntPoint.Lat, NearestPlane.IntPoint.Lon);
                    gm_Nearest.Zoom = 10;
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void bw_SpecLab_Receive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        # endregion

        #region Track

        private bool Track_DDE_HRD(DdeClient client, double az, double el)
        {
            // send Az/EL vie DDE to Ham Radio Deluxe Rotor Control (HRDRotator.exe)
            // no position feedback expected
            if (client == null)
                throw new NullReferenceException("[DDE]: Client not initialized.");
            if (!client.IsConnected)
                throw new InvalidOperationException("[DDE]: Client not connected.");

            byte[] data;
            if ((az >= 0) && (az <= 360))
            {
                // send azimuth data
                data = Encoding.ASCII.GetBytes("SET-AZ:" + az.ToString("F1"));
                client.TryPoke("PositionData", data, 1, 10000);
            }
            if ((el >= 0) && (el <= 90))
            {
                // send elevation data
                data = Encoding.ASCII.GetBytes("SET-EL:" + el.ToString("F1"));
                client.TryPoke("PositionData", data, 1, 10000);
            }
            return true;
        }

        private bool Track_UDP_WinTest(UdpClient client, IPEndPoint ip, double az, double el)
        {
            // send UDP broadcast like Win-Test
            // no position feedback expected
            if ((client == null) || (ip == null))
                if (client == null)
                    throw new NullReferenceException("[UDP]: Client and/or IP endpoint not initialized.");
            wtMessage msg = new wtMessage(WTMESSAGES.SETAZIMUTH, Properties.Settings.Default.Server_Name, "", "AUTO", " 00 " + az.ToString("000"));
            byte[] bytes = msg.ToBytes();
            client.Send(bytes, bytes.Length, ip);
            return true;
        }

        private bool Track_UDP_AirScout(UdpClient client, IPEndPoint ip, double az, double el)
        {
            // send UDP broadcast like Win-Test
            // no position feedback expected
            if ((client == null) || (ip == null))
                throw new NullReferenceException("[UDP]: Client and/or IP endpoint not initialized.");
            wtMessage msg;
            msg = new wtMessage(WTMESSAGES.SETAZIMUTH, Properties.Settings.Default.Server_Name, "", "AUTO", " 00 " + az.ToString("000"));
            byte[] bytes = msg.ToBytes();
            client.Send(bytes, bytes.Length, ip);
            msg = new wtMessage(WTMESSAGES.SETELEVATION, Properties.Settings.Default.Server_Name, "", "AUTO", " 00 " + el.ToString("000"));
            bytes = msg.ToBytes();
            client.Send(bytes, bytes.Length, ip);
            // new: high precision az/el
            msg = new wtMessage(WTMESSAGES.SETAZEL, Properties.Settings.Default.Server_Name, "", "AUTO", " 00 " + az.ToString("F8", CultureInfo.InvariantCulture) + " " + el.ToString("F8", CultureInfo.InvariantCulture));
            bytes = msg.ToBytes();
            client.Send(bytes, bytes.Length, ip);
            return true;
        }

        private string Serial_SendCommand(SerialPort sp, string command, bool waitanswer)
        {
            // sends a command via serial port (and optional wait fo answer)
            if ((sp == null) || (!sp.IsOpen))
                return "";
            string s = "";
            {
                sp.WriteLine(command);
                if (waitanswer)
                {
                    s = sp.ReadLine();
                    s = s.Replace("\n", "");
                }
            }
            return s;
        }


        private bool Track_SER__GS_232A_AZ(SerialPort sp, double az)
        {
            // send Az value via serial port (GS-232A protocol)
            // communictaion test --> get azimuth value
            if (sp == null)
                throw new NullReferenceException("[Serial]: Port not initialized.");
            if (!sp.IsOpen)
                throw new InvalidOperationException("[Serial]: Port not open.");
            // communictaion test --> get azimuth value
            string s = Serial_SendCommand(sp, "C", true);
            if (!s.StartsWith("+0"))
                throw new FormatException("[Serial]: Wrong serial data format.");
            try
            {
                double result = System.Convert.ToDouble(s.Substring(2, 3), CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new FormatException("[Serial]: Wrong serial data format.");
            }
            // set azimuth value --> no feedback
            Serial_SendCommand(sp, "M" + az.ToString("000"), false);
            return true;
        }

        private bool Track_SER__GS_232A_AZEL(SerialPort sp, double az, double el)
        {
            // send Az/El value via serial port (GS-232A protocol)
            if (sp == null)
                throw new NullReferenceException("[Serial]: Port not initialized.");
            if (!sp.IsOpen)
                throw new InvalidOperationException("[Serial]: Port not open.");
            // communictaion test --> get azimuth and elevation value
            string s = Serial_SendCommand(sp, "C2", true);
            if (!s.StartsWith("+0"))
                throw new FormatException("[Serial]: Wrong serial data format.");
            try
            {
                double result = System.Convert.ToDouble(s.Substring(2, 3), CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new FormatException("[Serial]: Wrong serial data format.");
            }
            // set azimuth value --> no feedback
            Serial_SendCommand(sp, "W" + az.ToString("000") + " " + el.ToString("000"), false);
            return true;
        }

        private void Track_File_Native(double az, double el)
        {
            // writes a file with Az/El values in a file (native)
            // Syntax: <Az value>,<El value> 
            using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "azel.dat"))
            {
                sw.WriteLine(az.ToString("F1", CultureInfo.InvariantCulture) + "," + el.ToString("F1", CultureInfo.InvariantCulture));
            }
        }

        private void Track_File_WSJT(double az, double el)
        {
            // writes a file with Az/El values in a file (WSJT)
            // the info is filled in the "Source" line (originally intended to follow a radio source
            using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "azel.dat"))
            {
                string utc = DateTime.UtcNow.ToString("HH:mm:ss");
                sw.WriteLine(utc + ",0,0,Moon");
                sw.WriteLine(utc + ",0,0,Sun");
                sw.WriteLine(utc + "," + az.ToString("F1", CultureInfo.InvariantCulture) + "," + el.ToString("F1", CultureInfo.InvariantCulture) + ",Source");
                sw.WriteLine("0,  0, 0,  0, 0,Doppler, R");
            }
        }

        private void bw_Track_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_Track";

            // last recent calculated values
            TrackValues oldvalues = null;
            TRACKSTATUS oldstatus = TRACKSTATUS.NONE;

            // clients and ports
            DdeClient ddeclient = null;
            UdpClient udpclient = null;
            IPEndPoint udpip = null;
            SerialPort serialport = null;
         
            // error counters
            int ddeerr = 0;
            int udperr = 0;
            int serialerr = 0;
            int maxerr = 10;

            // outer loop
            do
            {
                try
                {
                    // intializations
                    if (Properties.Settings.Default.Track_DDE_HRD)
                    {
                        ddeclient = new DdeClient("HRDRotator", "Position");
                        int result = ddeclient.TryConnect();

                    }
                    if (Properties.Settings.Default.Track_UDP_WinTest)
                    {
                        udpclient = new UdpClient();
                        udpip = new IPEndPoint(IPAddress.Broadcast, Properties.Settings.Default.Track_UDP_WinTest_Port);
                    }
                    else if (Properties.Settings.Default.Track_UDP_AirScout)
                    {
                        udpclient = new UdpClient();
                        udpip = new IPEndPoint(IPAddress.Broadcast, Properties.Settings.Default.Track_UDP_AirScout_Port);
                    }
                    if ((Properties.Settings.Default.Track_Serial_GS232_AZ) || (Properties.Settings.Default.Track_Serial_GS232_AZEL))
                    {
                        serialport = new SerialPort(Properties.Settings.Default.Track_Serial_Port,
                            Properties.Settings.Default.Track_Serial_Baudrate,
                            System.IO.Ports.Parity.None,
                            8,
                            System.IO.Ports.StopBits.One);
                        serialport.Handshake = System.IO.Ports.Handshake.None;
                        serialport.NewLine = "\r";
                        serialport.Encoding = Encoding.ASCII;
                        serialport.ReadTimeout = 1000;
                        serialport.WriteTimeout = 1000;
                        serialport.Open();
                    }

                    // init OK --> ready for tracking
                    bw_Track.ReportProgress(1, TRACKSTATUS.STOPPED);
                    bw_Track.ReportProgress(2, ROTSTATUS.STOPPED);

                    // inner loop
                    while (Properties.Settings.Default.Track_Activate && !bw_Track.CancellationPending)
                    {
                        try
                        {
                            // get current plane position and calculate set of tracking values
                            DateTime time = DateTime.UtcNow.AddSeconds(Properties.Settings.Default.Track_Offset);
                            TrackValues trackvalues = new TrackValues();
                            trackvalues.Timestamp = time;
                            if (TrackMode == AIRSCOUTTRACKMODE.TRACK)
                            {
                                // invalidate oldvalues on plane change
                                if ((oldvalues != null) && (Properties.Settings.Default.Track_CurrentPlane != null) && (oldvalues.Hex != Properties.Settings.Default.Track_CurrentPlane))
                                {
                                    oldvalues = null;
                                }
                                // track plane --> get plane position and calculate values
                                PlaneInfo plane = Planes.Get(Properties.Settings.Default.Track_CurrentPlane, time, Properties.Settings.Default.Planes_Position_TTL);
                                if (plane != null)
                                {
                                    trackvalues.Hex = Properties.Settings.Default.Track_CurrentPlane;
                                    trackvalues.MyAzimuth = LatLon.Bearing(Properties.Settings.Default.MyLat,
                                                                Properties.Settings.Default.MyLon,
                                                                plane.Lat,
                                                                plane.Lon);
                                    trackvalues.DXAzimuth = LatLon.Bearing(Properties.Settings.Default.DXLat,
                                                                Properties.Settings.Default.DXLon,
                                                                plane.Lat,
                                                                plane.Lon);
                                    double myh = (GetElevation(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon) + Properties.Settings.Default.MyHeight);
                                    double dxh = (GetElevation(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon) + Properties.Settings.Default.DXHeight);
                                    double H = plane.Alt_m;
                                    trackvalues.MyDistance = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, plane.Lat, plane.Lon);
                                    trackvalues.MySlantRange = Propagation.SlantRangeFromHeights(
                                                            myh,
                                                            Properties.Settings.Default.MyLat,
                                                            Properties.Settings.Default.MyLon,
                                                            plane.Lat,
                                                            plane.Lon,
                                                            H,
                                                            LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor);
                                    trackvalues.MyElevation = Propagation.EpsilonFromHeights(myh,
                                                                trackvalues.MyDistance,
                                                                H,
                                                                LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor)
                                                                / Math.PI * 180;
                                    trackvalues.DXDistance = LatLon.Distance(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, plane.Lat, plane.Lon);
                                    trackvalues.DXSlantRange = Propagation.SlantRangeFromHeights(
                                                            dxh,
                                                            Properties.Settings.Default.DXLat,
                                                            Properties.Settings.Default.DXLon,
                                                            plane.Lat,
                                                            plane.Lon,
                                                            H,
                                                            LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor);
                                    trackvalues.DXElevation = Propagation.EpsilonFromHeights(dxh,
                                                                trackvalues.DXDistance,
                                                                H,
                                                                LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor)
                                                                / Math.PI * 180;

                                    // calculate doppler, we need a last recent calculated values to calculate relative speed
                                    if (oldvalues != null)
                                    {
                                        // get both resulting speeds in m/s
                                        double timediff = (trackvalues.Timestamp - oldvalues.Timestamp).TotalSeconds;
                                        double myspeed = 0;
                                        double dxspeed = 0;

                                        if (timediff > 0)
                                        {
                                            myspeed = (oldvalues.MySlantRange - trackvalues.MySlantRange) * 1000.0 / timediff;
                                            dxspeed = (oldvalues.DXSlantRange - trackvalues.DXSlantRange) * 1000.0 / timediff;
                                        }

                                        // calculate both doppler shifts
                                        trackvalues.MyDoppler = Propagation.DopplerShift(Bands.ToHz(Properties.Settings.Default.Band), myspeed);
                                        trackvalues.DXDoppler = Propagation.DopplerShift(Bands.ToHz(Properties.Settings.Default.Band), dxspeed);
                                    }
                                }
                            }
                            else if (TrackMode == AIRSCOUTTRACKMODE.SINGLE)
                            {
                                // single shot --> get values from settings and track only once
                                trackvalues.MyAzimuth = Properties.Settings.Default.Track_SetAz;
                                trackvalues.MyElevation = Properties.Settings.Default.Track_SetEl;
                            }

                            // valid values --> start tracking
                            if (!double.IsNaN(trackvalues.MyAzimuth) &&
                                !double.IsNaN(trackvalues.MyElevation) &&
                                (trackvalues.MyAzimuth >= 0) &&
                                (trackvalues.MyAzimuth < 360))
                            {
                                // report track start
                                if (TrackMode == AIRSCOUTTRACKMODE.SINGLE)
                                {
                                    bw_Track.ReportProgress(1, TRACKSTATUS.SINGLE);
                                }
                                else if (TrackMode == AIRSCOUTTRACKMODE.TRACK)
                                {
                                    bw_Track.ReportProgress(1, TRACKSTATUS.TRACKING);
                                }

                                // rotator control
                                try
                                {
                                    // log tracking to console
                                    Console.WriteLine("Tracking[" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss,fff") + "]: Az=" + trackvalues.MyAzimuth + ", El=" + trackvalues.MyElevation);
                                    if (Properties.Settings.Default.Track_DDE_HRD)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_DDE_HRD(ddeclient, trackvalues.MyAzimuth, trackvalues.MyElevation);
                                    }
                                    if (Properties.Settings.Default.Track_UDP_WinTest)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_UDP_WinTest(udpclient, udpip, trackvalues.MyAzimuth, trackvalues.MyElevation);
                                    }
                                    if (Properties.Settings.Default.Track_UDP_AirScout)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_UDP_AirScout(udpclient, udpip, trackvalues.MyAzimuth, trackvalues.MyElevation);
                                    }
                                    if (Properties.Settings.Default.Track_Serial_GS232_AZ)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_SER__GS_232A_AZ(serialport, trackvalues.MyAzimuth);
                                    }
                                    if (Properties.Settings.Default.Track_Serial_GS232_AZEL)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_SER__GS_232A_AZEL(serialport, trackvalues.MyAzimuth, trackvalues.MyElevation);
                                    }
                                    if (Properties.Settings.Default.Track_File_Native)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_File_Native(trackvalues.MyAzimuth, trackvalues.MyElevation);
                                    }
                                    if (Properties.Settings.Default.Track_File_WSJT)
                                    {
                                        bw_Track.ReportProgress(2, ROTSTATUS.TRACKING);
                                        Track_File_WSJT(trackvalues.MyAzimuth, trackvalues.MyElevation);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                                    //report error
                                    bw_Track.ReportProgress(-1, ex.Message);
                                    // increment error counters and switch off in case of subsequent errors
                                    if (ex.Message.StartsWith("[DDE]:"))
                                    {
                                        ddeerr++;
                                        if (ddeerr > maxerr)
                                        {
                                            // switch off DDE
                                            Properties.Settings.Default.Track_DDE_None = true;
                                            Properties.Settings.Default.Track_DDE_HRD = false;
                                            bw_Track.ReportProgress(-1, "Tracking via DDE disabled.");
                                            bw_Track.ReportProgress(2, ROTSTATUS.ERROR);
                                            bw_Track.ReportProgress(1, TRACKSTATUS.ERROR);

                                        }
                                    }
                                    if (ex.Message.StartsWith("[UDP]:"))
                                    {
                                        udperr++;
                                        if (udperr > maxerr)
                                        {
                                            // switch off UDP
                                            Properties.Settings.Default.Track_UDP_None = true;
                                            Properties.Settings.Default.Track_UDP_WinTest = false;
                                            Properties.Settings.Default.Track_UDP_AirScout = false;
                                            bw_Track.ReportProgress(-1, "Tracking via UDP disabled.");
                                            bw_Track.ReportProgress(2, ROTSTATUS.ERROR);
                                            bw_Track.ReportProgress(1, TRACKSTATUS.ERROR);
                                        }
                                    }
                                    if (ex.Message.StartsWith("[Serial]:"))
                                    {
                                        serialerr++;
                                        if (serialerr > maxerr)
                                        {
                                            // switch off Serial
                                            Properties.Settings.Default.Track_Serial_None = true;
                                            Properties.Settings.Default.Track_Serial_GS232_AZ = false;
                                            Properties.Settings.Default.Track_Serial_GS232_AZEL = false;
                                            bw_Track.ReportProgress(-1, "Tracking via Serial disabled.");
                                            bw_Track.ReportProgress(2, ROTSTATUS.ERROR);
                                            bw_Track.ReportProgress(1, TRACKSTATUS.ERROR);
                                        }
                                        if (ex.Message.StartsWith("[Serial]:"))
                                            serialerr++;
                                    }

                                }

                                // doppler shift compensation if activated
                                if (Properties.Settings.Default.Doppler_Strategy_A)
                                {
                                    trackvalues.RXFrequency = Properties.Settings.Default.Doppler_DialFreq +
                                                                                    (long)trackvalues.MyDoppler +
                                                                                    (long)trackvalues.DXDoppler;
                                    trackvalues.TXFrequency = Properties.Settings.Default.Doppler_DialFreq;
                                }
                                else if (Properties.Settings.Default.Doppler_Strategy_B)
                                {
                                    trackvalues.RXFrequency = Properties.Settings.Default.Doppler_DialFreq;
                                    trackvalues.TXFrequency = Properties.Settings.Default.Doppler_DialFreq -
                                                                                    (long)trackvalues.MyDoppler -
                                                                                    (long)trackvalues.DXDoppler;
                                }
                                else if (Properties.Settings.Default.Doppler_Strategy_C)
                                {
                                    trackvalues.RXFrequency = Properties.Settings.Default.Doppler_DialFreq +
                                                                                    (long)trackvalues.MyDoppler +
                                                                                    (long)trackvalues.DXDoppler;
                                    trackvalues.TXFrequency = Properties.Settings.Default.Doppler_DialFreq -
                                                                                    (long)trackvalues.MyDoppler -
                                                                                    (long)trackvalues.DXDoppler;
                                }
                                else if (Properties.Settings.Default.Doppler_Strategy_D)
                                {
                                    trackvalues.RXFrequency = Properties.Settings.Default.Doppler_DialFreq +
                                                                                    (long)trackvalues.MyDoppler;
                                    trackvalues.TXFrequency = Properties.Settings.Default.Doppler_DialFreq -
                                                                                    (long)trackvalues.MyDoppler;
                                }

                                // report values
                                bw_Track.ReportProgress(3, trackvalues);

                                // store last values
                                oldvalues = trackvalues;

                                // stop tracking when single shot
                                if (TrackMode == AIRSCOUTTRACKMODE.SINGLE)
                                {
                                    bw_Track.ReportProgress(1, TRACKSTATUS.STOPPED);
                                    bw_Track.ReportProgress(2, ROTSTATUS.STOPPED);
                                }

                            }
                            else
                            {
                                // no tracking!
                                bw_Track.ReportProgress(1, TRACKSTATUS.STOPPED);
                                bw_Track.ReportProgress(2, ROTSTATUS.STOPPED);
                            }
                        }
                        catch (Exception ex)
                        {
                            // leave inner loop
                            bw_Track.ReportProgress(1, TRACKSTATUS.ERROR);
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(Properties.Settings.Default.Track_Update);

                }
                catch (Exception ex)
                {
                    bw_Track.ReportProgress(-1, "Track error: " + ex.Message);
                    bw_Track.ReportProgress(1, TRACKSTATUS.ERROR);
                }

            }
            while (!bw_Track.CancellationPending);

            // try to close all connections
            try
            {
                if ((ddeclient != null) && (ddeclient.IsConnected))
                    ddeclient.Disconnect();
                if (udpclient != null)
                    udpclient.Close();
                if ((serialport != null) && (serialport.IsOpen))
                    serialport.Close();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }

            bw_Track.ReportProgress(1, TRACKSTATUS.NONE);
            bw_Track.ReportProgress(2, ROTSTATUS.NONE);

            Log.WriteMessage("Finished.");
        }

        private void bw_Track_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 0)
            {
                // report Error
                tsl_Status.Text = (string)e.UserState;
            }
            else if (e.ProgressPercentage == 1)
            {
                // report tracking status
                TRACKSTATUS trackstatus = (TRACKSTATUS)e.UserState;
                switch (trackstatus)
                {
                    case TRACKSTATUS.NONE:
                        SayTrack("TRK", Color.DarkGray, SystemColors.Control);
                        break;
                    case TRACKSTATUS.STOPPED:
//                        TrackMode = AIRSCOUTTRACKMODE.NONE;
                        SayTrack("TRK", SystemColors.Control, Color.DarkGray);
                        break;
                    case TRACKSTATUS.SINGLE:
                    case TRACKSTATUS.TRACKING:
                        SayTrack("TRK", Color.White, Color.DarkGreen);
                        break;
                    case TRACKSTATUS.ERROR:
                        SayTrack("TRK", Color.Yellow, Color.Red);
                        break;
                    default:
                        SayTrack("TRK", Color.DarkGray, SystemColors.Control);
                        break;
                }

                // restore settings when returning from tracking
                if ((trackstatus == TRACKSTATUS.TRACKING) || (trackstatus == TRACKSTATUS.SINGLE))
                {
                    if (bw_CAT != null)
                    {
                        if (Properties.Settings.Default.Doppler_Strategy_A)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_A;
                        else if (Properties.Settings.Default.Doppler_Strategy_B)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_B;
                        else if (Properties.Settings.Default.Doppler_Strategy_C)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_C;
                        else if (Properties.Settings.Default.Doppler_Strategy_D)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_D;

                    }
                }
                else
                {
                    if (bw_CAT != null)
                    {
                        bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_NONE;
                    }
                }

                // save TrackStatus
                TrackStatus = trackstatus;
            }
            else if (e.ProgressPercentage == 2)
            {
                // report Status
                RotStatus = (ROTSTATUS)e.UserState;
                switch (RotStatus)
                {
                    case ROTSTATUS.STOPPED:
                        SayRot("ROT", SystemColors.Control, Color.DarkGray);
                        break;
                    case ROTSTATUS.TRACKING:
                        SayRot("ROT", Color.White, Color.DarkGreen);
                        break;
                    case ROTSTATUS.ERROR:
                        SayRot("ROT", Color.Yellow, Color.Red);
                        break;
                    default:
                        SayRot("ROT", Color.DarkGray, SystemColors.Control);
                        break;
                }
            }
            else if (e.ProgressPercentage == 3)
            {
                // report track values
                TrackValues = (TrackValues)e.UserState;
                if (Properties.Settings.Default.Doppler_Strategy_A ||
                    Properties.Settings.Default.Doppler_Strategy_B ||
                    Properties.Settings.Default.Doppler_Strategy_C ||
                    Properties.Settings.Default.Doppler_Strategy_D)
                {
                    // adjust rig if frequencies are valid
                    if ((bw_CAT != null) && (TrackValues.RXFrequency != 0) && (TrackValues.TXFrequency != 0))
                    {
                        if (Properties.Settings.Default.Doppler_Strategy_A)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_A;
                        if (Properties.Settings.Default.Doppler_Strategy_B)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_B;
                        if (Properties.Settings.Default.Doppler_Strategy_C)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_C;
                        if (Properties.Settings.Default.Doppler_Strategy_D)
                            bw_CAT.DopplerStrategy = DOPPLERSTRATEGY.DOPPLER_D;
                        bw_CAT.RxFrequency = TrackValues.RXFrequency;
                        bw_CAT.TxFrequency = TrackValues.TXFrequency;
                    }
                }
            }
        }

        private void bw_Track_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        #region JSONWriter

        private void bw_JSONWriter_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            int interval = 60;
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_JSONWriter";
            while (!bw_JSONWriter.CancellationPending)
            {
                // get planes each minute
                List<PlaneInfo> list = Planes.GetAll(DateTime.UtcNow, Properties.Settings.Default.Planes_Position_TTL);
                if (list.Count > 0)
                {
                    // write json file
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "planes.json"))
                        {
                            int major = Assembly.GetExecutingAssembly().GetName().Version.Major;
                            sw.Write("{\"full_count\":" + list.Count().ToString() + ",\"version\":" + major.ToString());
                            int i = 0;
                            foreach (PlaneInfo info in list)
                            {
                                string index = "\"" + i.ToString("x8") + "\"";
                                string hex = "\"" + info.Hex + "\"";
                                string lat = info.Lat.ToString("F4", CultureInfo.InvariantCulture);
                                string lon = info.Lon.ToString("F4", CultureInfo.InvariantCulture);
                                string track = info.Track.ToString("F0", CultureInfo.InvariantCulture);
                                string alt = info.Alt.ToString("F0", CultureInfo.InvariantCulture);
                                string speed = info.Speed.ToString("F0", CultureInfo.InvariantCulture);
                                string squawk = "\"" + "" + "\"";
                                string radar = "\"" + "" + "\"";
                                string type = "\"" + info.Type + "\"";
                                string reg = "\"" + info.Reg + "\"";
                                DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                string time = ((long)(info.Time - sTime).TotalSeconds).ToString();
                                string dep = "\"\"";
                                string dest = "\"\"";
                                string flight = "\"\"";
                                string dummy1 = "0";
                                string dummy2 = "0";
                                string call = "\"" + info.Call + "\"";
                                string dummy3 = "0";
                                sw.WriteLine("," + index + ":[" +
                                    hex + "," +
                                    lat + "," +
                                    lon + "," +
                                    track + "," +
                                    alt + "," +
                                    speed + "," +
                                    squawk + "," +
                                    radar + "," +
                                    type + "," +
                                    reg + "," +
                                    time + "," +
                                    dep + "," +
                                    dest + "," +
                                    flight + "," +
                                    dummy1 + "," +
                                    dummy2 + "," +
                                    call + "," +
                                    dummy3 +
                                    "]");
                                i++;
                            }
                            sw.WriteLine("}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString(), LogLevel.Error);
                        // do nothing
                    }
                }
                int ii = 0;
                while (!bw_JSONWriter.CancellationPending && (ii < interval))
                {
                    Thread.Sleep(1000);
                    ii++;
                }
            }
            Log.WriteMessage("Finished.");
        }


        #endregion

        #region NewsFeed

        private void bw_NewsFeed_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_NewsFeed";
            Uri uri = Properties.Settings.Default.News_URL;
            int interval = Properties.Settings.Default.News_Interval;
            while (!bw_NewsFeed.CancellationPending)
            {
                if (Properties.Settings.Default.NewsFeed_Enabled)
                {
                    try
                    {
                        // get the last modified time of the website
                        AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                        DateTime dt = cl.GetWebCreationTimeUtc(Properties.Settings.Default.News_URL);
                        Log.WriteMessage("Checking news page: " + dt.ToString("yyyy-MM-dd HH:mm:ss") + "<> " + Properties.Settings.Default.News_LastUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("Checking news page: " + dt.ToString("yyyy-MM-dd HH:mm:ss") + "<> " + Properties.Settings.Default.News_LastUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
                        // report latest news if updated
                        if (dt > Properties.Settings.Default.News_LastUpdate)
                        {
                            // report news to main window
                            bw_NewsFeed.ReportProgress(1, dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // report error
                        Log.WriteMessage(ex.ToString(), LogLevel.Error);
                        bw_NewsFeed.ReportProgress(-1, DateTime.UtcNow.ToString("[" + "HH:mm:ss") + "] Error while reading the website " + uri.ToString() + ": " + ex.Message);
                    }
                }

                int i = 0;
                while (!bw_NewsFeed.CancellationPending && (i < interval))
                {
                    Thread.Sleep(1000);
                    i++;
                }
            }
            Log.WriteMessage("Finished.");
        }

        private void bw_NewsFeed_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                // report error
                Say((string)e.UserState);
            }
            else
            {
                // stop background thread
                while (bw_NewsFeed.IsBusy)
                {
                    bw_NewsFeed.CancelAsync();
                    Application.DoEvents();
                }

                // report website changes
                DateTime dt = (DateTime)e.UserState;
                if (!SupportFunctions.IsMono)
                {
                    if (MessageBox.Show("There are news on the website. Latest update: " + dt.ToString() + "\n Do you want to read it now?", "Website News", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            if (wb_News != null)
                                wb_News.Refresh();
                            tc_Map.SelectedTab = tp_News;
                        }
                        catch (Exception ex)
                        {
                            // do nothing if wb_News fails to refresh
                        }
                    }
                    // save time to settings
                    Properties.Settings.Default.News_LastUpdate = dt;
                }
                else
                {
                    MessageBox.Show("There are news on the website. Latest update: " + dt.ToString() + "\n Do you want to read it now?\n\n Under Linux/Mono open web browser of your choice and goto: \n" + Properties.Settings.Default.News_URL + "\n\n", "Website News", MessageBoxButtons.YesNo);
                    // save time to settings
                    Properties.Settings.Default.News_LastUpdate = dt;

                }

                // restart background thread
                if (!bw_NewsFeed.IsBusy)
                    bw_NewsFeed.RunWorkerAsync();
            }
        }

        private void bw_NewsFeed_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        #region HistoryDownloader

        private void HistoryDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // get byte in GB
            double bytesin = double.Parse(e.BytesReceived.ToString()) / 1024.0 / 1024.0 / 1024.0;
            double totalbytes = double.Parse(e.TotalBytesToReceive.ToString()) / 1024.0 / 1024.0 / 1024.0;
            double percentage = bytesin / totalbytes * 100;
            try
            {
                if (bw_HistoryDownloader.IsBusy)
                    bw_HistoryDownloader.ReportProgress(1, "Downloaded " + bytesin.ToString("F2") + " GB of " + totalbytes.ToString("F2") + " GB  (" + percentage.ToString("F2") + "%).");
            }
            catch
            {

            }
        }

        private string ReadPropertyString(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return null;
            return o.Property(propertyname).Value.Value<string>();
        }

        private int ReadPropertyDoubleToInt(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return int.MinValue;
            double d = ReadPropertyDouble(o, propertyname);
            if ((d != double.MinValue) && (d >= int.MinValue) && (d <= int.MaxValue))
                return (int)d;
            return int.MinValue;
        }

        private double ReadPropertyDouble(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return double.MinValue;
            return o.Property(propertyname).Value.Value<double>();
        }

        private long ReadPropertyLong(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return long.MinValue;
            return o.Property(propertyname).Value.Value<long>();
        }

        private void HistoryDownloader_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (bw_HistoryDownloader.IsBusy)
                    bw_HistoryDownloader.ReportProgress(1, "Reading " + Properties.Settings.Default.Analysis_History_ZIPFileName);
                // unzip the file
                if (File.Exists(Properties.Settings.Default.Analysis_History_ZIPFileName))
                {
                    string filename = Properties.Settings.Default.Analysis_History_ZIPFileName;
                    // unzips a zip file content to the same directory
                    string downloaddir = Path.GetDirectoryName(Properties.Settings.Default.Analysis_History_ZIPFileName);
                    // set path to calling assembly's path if not otherwise specified
                    if (String.IsNullOrEmpty(downloaddir))
                        downloaddir = Assembly.GetCallingAssembly().Location;
                    // open the zip file
                    using (ZipFile zip = new ZipFile(filename))
                    {
                        zip.ZipErrorAction = ZipErrorAction.Skip;
                        // here, we extract every entry, but we could extract conditionally
                        // based on entry name, size, date, checkbox status, etc.  
                        foreach (ZipEntry ze in zip)
                        {
                            if (bw_HistoryDownloader.IsBusy)
                                bw_HistoryDownloader.ReportProgress(1, "Extracting " + ze.FileName);
                            try
                            {
                                /*
                                ze.Extract(downloaddir, ExtractExistingFileAction.OverwriteSilently);
                                string fname = Path.Combine(downloaddir, ze.FileName);
                                File.SetLastWriteTime(fname, ze.LastModified);
                                */
                            }
                            catch (Exception ex)
                            {
                                if (bw_HistoryDownloader.IsBusy)
                                    bw_HistoryDownloader.ReportProgress(-1, ex.Message);
                            }
                        }
                    }
                    // create csv to load into database
                    using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.Analysis_History_ZIPFileName.ToLower().Replace(".zip", ".csv")))
                    {
                        sw.WriteLine("time;call;reg;hex;lat;lon;track;alt;speed;squawk;radar;type");
                        // read all files
                        for (int i = 0; i < 1440; i += (int)Properties.Settings.Default.Analysis_History_Stepwidth)
                        {
                            // calculate filename
                            int hours = i / 60;
                            int minutes = i % 60;
                            string fname = Path.Combine(Properties.Settings.Default.Analysis_History_Directory, Properties.Settings.Default.Analysis_History_Date.ToString("yyyy-MM-dd") + "-" + hours.ToString("00") + minutes.ToString("00") + "Z.json");
                            if (bw_HistoryDownloader.IsBusy)
                                bw_HistoryDownloader.ReportProgress(1, "Processing " + fname);
                            if (File.Exists(fname))
                            {
                                string json = "";
                                using (StreamReader sr = new StreamReader(fname))
                                    json = sr.ReadToEnd();
                                // analyze json string for planes data
                                JObject root = (JObject)JsonConvert.DeserializeObject(json);
                                foreach (JProperty proot in root.Children<JProperty>())
                                {
                                    // get the planes position list
                                    if (proot.Name == "acList")
                                    {
                                        foreach (JArray a in proot.Children<JArray>())
                                        {
                                            foreach (JObject o in a.Values<JObject>())
                                            {
                                                PlaneInfo info = new PlaneInfo();
                                                try
                                                {
                                                    info.Call = ReadPropertyString(o, "Call");
                                                    info.Lat = ReadPropertyDouble(o, "Lat");
                                                    info.Lon = ReadPropertyDouble(o, "Long");
                                                    info.Track = ReadPropertyDoubleToInt(o, "Trak");
                                                    // 2017-07-23: take "GAlt" (corrected altitude by air pressure) rather than "Alt"
                                                    info.Alt = ReadPropertyDoubleToInt(o, "GAlt");
                                                    //                                            info.Alt = ReadPropertyDoubleToInt(o, "Alt");
                                                    info.Speed = ReadPropertyDoubleToInt(o, "Spd");
                                                    info.Reg = ReadPropertyString(o, "Reg");
                                                    try
                                                    {
                                                        string squawk = ReadPropertyString(o, "Sqk");
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    info.Hex = ReadPropertyString(o, "Icao");
                                                    info.Type = ReadPropertyString(o, "Type");
                                                    // complete type info
                                                    AircraftTypeDesignator td = AircraftData.Database.AircraftTypeFindByICAO(info.Type);
                                                    if (td != null)
                                                    {
                                                        info.Manufacturer = td.Manufacturer;
                                                        info.Model = td.Model;
                                                        info.Category = td.Category;
                                                    }
                                                    else
                                                    {
                                                        info.Manufacturer = "[unknown]";
                                                        info.Model = "[unknown]";
                                                        info.Category = PLANECATEGORY.NONE;
                                                    }
                                                    // CAUTION!! time is UNIX time in milliseconds
                                                    long l = ReadPropertyLong(o, "PosTime");
                                                    if (l != long.MinValue)
                                                    {
                                                        DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                                        timestamp = timestamp.AddMilliseconds(l);
                                                        info.Time = timestamp;
                                                    }
                                                    else
                                                    {
                                                        info.Time = DateTime.MinValue;
                                                    }
                                                    if (PlaneInfoChecker.Check(info) &&
                                                        (info.Alt_m >= Properties.Settings.Default.Planes_MinAlt) &&
                                                        (info.Alt_m <= Properties.Settings.Default.Planes_MaxAlt) &&
                                                        (info.Lat >= Properties.Settings.Default.MinLat) &&
                                                        (info.Lat <= Properties.Settings.Default.MaxLat) &&
                                                        (info.Lon >= Properties.Settings.Default.MinLon) &&
                                                        (info.Lon <= Properties.Settings.Default.MaxLon))
                                                    {
                                                        sw.WriteLine(info.Time.ToString("u") + ";" +
                                                            info.Call + ";" +
                                                            info.Reg + ";" +
                                                            info.Hex + ";" +
                                                            info.Lat.ToString("F8", CultureInfo.InvariantCulture) + ";" +
                                                            info.Lon.ToString("F8", CultureInfo.InvariantCulture) + ";" +
                                                            info.Track.ToString() + ";" +
                                                            info.Alt.ToString() + ";" +
                                                            info.Speed.ToString() + ";" +
                                                            "" + ";" +
                                                            "" + ";" +
                                                            info.Type);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (bw_HistoryDownloader.IsBusy)
                    bw_HistoryDownloader.ReportProgress(-1, ex.Message);
            }
            // job is done
            // cancel backgroundworker
            bw_HistoryDownloader.CancelAsync();
        }

        private void bw_HistoryDownloader_DoWork(object sender, DoWorkEventArgs e)
        {
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_HistoryDownloader";
            try
            {
                DateTime date = (DateTime)e.Argument;
                // check history directory first
                if (!Directory.Exists(Properties.Settings.Default.Analysis_History_Directory))
                    Properties.Settings.Default.Analysis_History_Directory = TmpDirectory;
                // check free disk space
                System.IO.DriveInfo drive = new System.IO.DriveInfo(Properties.Settings.Default.Analysis_History_Directory);
                System.IO.DriveInfo a = new System.IO.DriveInfo(drive.Name);
                if (a.AvailableFreeSpace < 50.0 * 1024.0 * 1024.0 * 1024.0)
                    throw new ArgumentException("Not enough disk space to run this operation.");
                string url = Properties.Settings.Default.Analysis_History_URL;
                Properties.Settings.Default.Analysis_History_ZIPFileName = Path.Combine(Properties.Settings.Default.Analysis_History_Directory, date.ToString("yyyy-MM-dd") + ".zip");
                if (!File.Exists(Properties.Settings.Default.Analysis_History_ZIPFileName))
                {
                    // file not found --> donwload it from url
                    // complete url with "/" and date 
                    if (!url.EndsWith("/"))
                        url = url + "/";
                    url = url + date.ToString("yyyy-MM-dd") + ".zip";
                    // create web client for download
                    WebClient client = new WebClient();
                    // register asynchronous file download event handler
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(HistoryDownloader_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(HistoryDownloader_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri(url), Properties.Settings.Default.Analysis_History_ZIPFileName);
                    // remain here in a loop until job is finished
                    // cancellation will be initiated after download and unzip is complet
                }
                else
                {   // call download completed handler directly
                    HistoryDownloader_DownloadFileCompleted(this, null);
                }
            }
            catch (Exception ex)
            {
                bw_HistoryDownloader.ReportProgress(-1, ex.Message);
            }
        }

        private void bw_HistoryDownloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string s = (string)e.UserState;
            if (String.IsNullOrEmpty(s))
                return;
            tb_Analysis_Status.Text = s;
        }

        private void bw_HistoryDownloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_Analysis_Planes_History.Enabled = true;
        }

        #endregion


        #region ElevationDatabaseUpdater

        private void bw_ElevationDatabaseUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // error message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg, LogLevel.Error);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    // redirect output to splash screen on first run
                    if (FirstRun && SplashDlg != null)
                        Splash("Preparing database for first run: " + msg + " (please wait)", Color.Yellow);
                    else
                    {
                        SayDatabase(msg);
                        Log.WriteMessage(msg);
                    }
                }
                else if (e.ProgressPercentage == 1)
                {
                    // database status update message received
                    if (sender == this.bw_GLOBEUpdater)
                    {
                        Properties.Settings.Default.Elevation_GLOBE_DatabaseStatus = (DATABASESTATUS)e.UserState;
                        Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.Elevation_GLOBE_DatabaseStatus);
                        if (tsl_Database_LED_GLOBE.BackColor != color)
                            tsl_Database_LED_GLOBE.BackColor = color;
                        string text = "GLOBE Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.Elevation_GLOBE_DatabaseStatus);
                        if (tsl_Database_LED_GLOBE.ToolTipText != text)
                            tsl_Database_LED_GLOBE.ToolTipText = text;
                    }
                    else if (sender == this.bw_SRTM3Updater)
                    {
                        Properties.Settings.Default.Elevation_SRTM3_DatabaseStatus = (DATABASESTATUS)e.UserState;
                        Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.Elevation_SRTM3_DatabaseStatus);
                        if (tsl_Database_LED_SRTM3.BackColor != color)
                            tsl_Database_LED_SRTM3.BackColor = color;
                        string text = "SRTM3 Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.Elevation_SRTM3_DatabaseStatus);
                        if (tsl_Database_LED_SRTM3.ToolTipText != text)
                            tsl_Database_LED_SRTM3.ToolTipText = text;
                    }
                    else if (sender == this.bw_SRTM1Updater)
                    {
                        Properties.Settings.Default.Elevation_SRTM1_DatabaseStatus = (DATABASESTATUS)e.UserState;
                        Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.Elevation_SRTM1_DatabaseStatus);
                        if (tsl_Database_LED_SRTM1.BackColor != color)
                            tsl_Database_LED_SRTM1.BackColor = color;
                        string text = "SRTM1 Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.Elevation_SRTM1_DatabaseStatus);
                        if (tsl_Database_LED_SRTM1.ToolTipText != text)
                            tsl_Database_LED_SRTM1.ToolTipText = text;
                    }
                    else if (sender == this.bw_ASTER3Updater)
                    {
                        Properties.Settings.Default.Elevation_ASTER3_DatabaseStatus = (DATABASESTATUS)e.UserState;
                        Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.Elevation_ASTER3_DatabaseStatus);
                        if (tsl_Database_LED_ASTER3.BackColor != color)
                            tsl_Database_LED_ASTER3.BackColor = color;
                        string text = "ASTER3 Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.Elevation_ASTER3_DatabaseStatus);
                        if (tsl_Database_LED_ASTER3.ToolTipText != text)
                            tsl_Database_LED_ASTER3.ToolTipText = text;
                    }
                    else if (sender == this.bw_ASTER1Updater)
                    {
                        Properties.Settings.Default.Elevation_ASTER1_DatabaseStatus = (DATABASESTATUS)e.UserState;
                        Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.Elevation_ASTER1_DatabaseStatus);
                        if (tsl_Database_LED_ASTER1.BackColor != color)
                            tsl_Database_LED_ASTER1.BackColor = color;
                        string text = "ASTER1 Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.Elevation_ASTER1_DatabaseStatus);
                        if (tsl_Database_LED_ASTER1.ToolTipText != text)
                            tsl_Database_LED_ASTER1.ToolTipText = text;
                    }
                }
                if (!this.Disposing && (ss_Main != null))
                    ss_Main.Update();   
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }


        #endregion

        #region ElevationPathCalculator

        private void bw_ElevationPathCalculator_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                SayCalculations((string)e.UserState);
        }


        #endregion

        #region StationDatabaseUpdater

        private void bw_StationDatabaseUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // error message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg, LogLevel.Error);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg);
                    // redirect output to splash screen on first run
                    if (FirstRun && SplashDlg != null)
                        Splash("Preparing database for first run: " + msg + " (please wait)", Color.Yellow);
                    else
                    {
                        SayDatabase(msg);
                    }
                }
                else if (e.ProgressPercentage == 1)
                {
                    Properties.Settings.Default.StationsDatabase_Status = (DATABASESTATUS)e.UserState;
                    Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.StationsDatabase_Status);
                    if (tsl_Database_LED_Stations.BackColor != color)
                        tsl_Database_LED_Stations.BackColor = color;
                    string text = "Stations Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.StationsDatabase_Status);
                    if (tsl_Database_LED_Stations.ToolTipText != text)
                        tsl_Database_LED_Stations.ToolTipText = text;
                }
                if (!this.Disposing && (ss_Main != null))
                    ss_Main.Update();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void bw_StationDatabaseUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // refresh display
            if (!this.IsDisposed)
            {
                UpdateAirports();
            }
        }

        #endregion

        #region AircraftDatabaseUpdater

        private void bw_AircraftDatabaseUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // error message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg, LogLevel.Error);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg);
                    // redirect output to splash screen on first run
                    if (FirstRun && SplashDlg != null)
                        Splash("Preparing database for first run: " + msg + " (please wait)", Color.Yellow);
                    else
                    {
                        SayDatabase(msg);
                    }
                }
                else if (e.ProgressPercentage == 1)
                {
                    Properties.Settings.Default.AircraftDatabase_Status = (DATABASESTATUS)e.UserState;
                    Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.AircraftDatabase_Status);
                    if (tsl_Database_LED_Aircraft.BackColor != color)
                    {
                        tsl_Database_LED_Aircraft.BackColor = color;
                    }
                    string text = "Aircraft Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.AircraftDatabase_Status);
                    if (tsl_Database_LED_Aircraft.ToolTipText != text)
                        tsl_Database_LED_Aircraft.ToolTipText = text;
                }
                if (!this.Disposing && (ss_Main != null))
                    ss_Main.Update();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void bw_AircraftDatabaseUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // refresh data tables and display
            if (!this.IsDisposed)
            {
                // refresh all dictionnariees
                //                ScoutData.Database.UpdateFromDataTables(false);
                // refresh airports and map
//                UpdateAirports();
            }
        }

        #endregion

        #region AircraftDatabaseMaintainer

        private void bw_AircraftDatabaseMaintainer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // error message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg, LogLevel.Error);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg);
                    // redirect output to splash screen on first run
                    if (FirstRun && SplashDlg != null)
                        Splash("Preparing database for first run: " + msg + " (please wait)", Color.Yellow);
                    else
                    {
                        SayDatabase(msg);
                    }
                }
                if (!this.Disposing && (ss_Main != null))
                    ss_Main.Update();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void bw_AircraftDatabaseMaintainer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

        #region MapPreloader

        private void bw_MapPreloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                SayCalculations((string)e.UserState);
            else
             { 
                PointLatLng p = (PointLatLng)e.UserState;
                SayCalculations("Preloading map tile: " + MaidenheadLocator.LocFromLatLon(p.Lat, p.Lng, false, 2) + ", level " + e.ProgressPercentage);
                gm_Cache.Zoom = e.ProgressPercentage;
                gm_Cache.Position = p;
                gm_Cache.ReloadMap();
            }
        }


        #endregion

        #region CATUpdater

        private void bw_CATUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // error message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg, LogLevel.Error);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    Log.WriteMessage(msg);
                    // redirect output to splash screen on first run
                    if (FirstRun && SplashDlg != null)
                        Splash("Preparing database for first run: " + msg + " (please wait)", Color.Yellow);
                    else
                    {
                        SayDatabase(msg);
                    }
                }
                else if (e.ProgressPercentage == 1)
                {
                    Properties.Settings.Default.RigDatabase_Status = (DATABASESTATUS)e.UserState;
                    Color color = DatabaseStatus.GetDatabaseStatusColor(Properties.Settings.Default.RigDatabase_Status);
                    if (tsl_Database_LED_Rig.BackColor != color)
                    {
                        tsl_Database_LED_Rig.BackColor = color;
                    }
                    string text = "Rig Database Status\n\n" + DatabaseStatus.GetDatabaseStatusText(Properties.Settings.Default.RigDatabase_Status);
                    if (tsl_Database_LED_Rig.ToolTipText != text)
                        tsl_Database_LED_Rig.ToolTipText = text;
                }
                if (!this.Disposing && (ss_Main != null))
                    ss_Main.Update();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }


        #endregion

        #region CAT

        private void bw_CAT_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage <= 0)
                {
                    Say((string)e.UserState);
                }
                else if (e.ProgressPercentage == 1)
                {
                    // new rig status received
                    RIGSTATUS status = (RIGSTATUS)e.UserState;
                    switch (status)
                    {
                        case RIGSTATUS.ONLINE:
                            SayCAT("CAT", Color.White, Color.DarkGreen);
                            break;
                        case RIGSTATUS.ERROR:
                        case RIGSTATUS.NOCAT:
                        case RIGSTATUS.NOPORT:
                        case RIGSTATUS.NORIG:
                        case RIGSTATUS.NOTSUITABLE:
                            SayCAT("CAT", Color.Yellow, Color.Red);
                            break;
                        case RIGSTATUS.OFFLINE:
                            SayCAT("CAT", Color.White, Color.DarkOrange);
                            break;
                        default:
                            SayCAT("CAT", Color.DarkGray, SystemColors.Control);
                            break;
                    }

                    RigStatus = status;
                }
                else if (e.ProgressPercentage == 2)
                {
                    // new rig info received
                    IRig rig = (IRig)e.UserState;

                    // save info if a valid tracking is not going on
                    if (TrackStatus != TRACKSTATUS.TRACKING)
                    {
                        if (rig != null)
                        {
                            ConnectedRig = rig;
                            // save latest rig settings to switch back after tracking
                            Properties.Settings.Default.Doppler_DialFreq = rig.GetRxFrequency();
                            Properties.Settings.Default.Doppler_DialMode = rig.GetMode();
                            Properties.Settings.Default.Doppler_DialSplit = rig.GetSplit();
                            Properties.Settings.Default.Doppler_DialRit = rig.GetRit();
                        }
                    }

                    // report to status bar
                    NumberFormatInfo info = new NumberFormatInfo();
                    info.NumberDecimalSeparator = ";";
                    info.NumberGroupSeparator = ".";
                    Say("Rig reports RX: " + rig.GetRxFrequency().ToString(info) + ", TX: " + rig.GetTxFrequency().ToString(info) + "Hz, Mode: " + rig.GetMode().ToString() + ", RIT: " + ((rig.GetRit() == RIGRIT.RITON) ? "ON" : "OFF") + ", Split: " + ((rig.GetSplit() == RIGSPLIT.SPLITON) ? "ON" : "OFF"));
                }

                // set Tooltip
                if (ConnectedRig != null)
                {
                    tsl_CAT.ToolTipText = ConnectedRig.CatVersion + "\n" + ConnectedRig.Settings.Type + "\n\n";
                }
                else
                {
                    tsl_CAT.ToolTipText = "CAT error!" + "\n\n";
                }
                tsl_CAT.ToolTipText = tsl_CAT.ToolTipText + RigStatus.ToString();
            }
            catch (Exception ex)
            {
                RigStatus = RIGSTATUS.ERROR;
                tsl_CAT.ToolTipText = "CAT error: " + ex.Message;
            }
        }


        #endregion

        #endregion


        #region Analysis_DataGetter

        private void bw_Analysis_DataGetter_DoWork(object sender, DoWorkEventArgs e)
        {
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_Analysis_DataGetter";
            Stopwatch st = new Stopwatch();
            st.Start();
            bw_Analysis_DataGetter.ReportProgress(0, "Getting timespan of all positions in database...");
            // calculate min/max values for timespan
            History_OldestEntry = AircraftPositionData.Database.AircraftPositionOldestEntry();
            if (bw_Analysis_DataGetter.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            History_YoungestEntry = AircraftPositionData.Database.AircraftPositionYoungestEntry();
            if (bw_Analysis_DataGetter.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            bw_Analysis_DataGetter.ReportProgress(0, "Getting positions...");
            AircraftPositionsCount = AircraftPositionData.Database.AircraftPositionCount();
            // get all aircraft positions into cache
            lock (AllPositions)
            {
                AllPositions.Clear();
            }
            lock (AllPositions)
            {
                // get all positions from database, can be interrupted
                AllPositions = AircraftPositionData.Database.AircraftPositionGetAll(this.bw_Analysis_DataGetter);
                if (bw_Analysis_DataGetter.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
            st.Stop();
            bw_Analysis_DataGetter.ReportProgress(0, "Getting positions finished, " + AllPositions.Count.ToString() + " positions, " + st.ElapsedMilliseconds.ToString() + " ms.");
        }

        private void bw_Analysis_DataGetter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                string msg = (string)e.UserState;
                // NASTY!! Add the total count of positions after the "of" in the message
                // total count is calculated once in DoWork
                if (msg.EndsWith("of)"))
                    msg = msg + " " + AircraftPositionsCount.ToString();
                SayAnalysis(msg);
            }
        }

        private void bw_Analysis_DataGetter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // check if cancelled
            // return to default state
            if (e.Cancelled)
            {
                SayAnalysis("Cancelled.");
                btn_Analysis_ON.Enabled = true;
                btn_Analysis_ON.BackColor = Color.YellowGreen;
                return;
            }
            else
                SayAnalysis("Ready.");
            // nothing found in database --> show message box and do not enter analysis mode
            if ((History_YoungestEntry == DateTime.MinValue) || (History_OldestEntry == DateTime.MinValue))
            {
                MessageBox.Show("Nothing found for analysis. Please let AirScout run in PLAY mode for a while to collect some data.", "AirScout Analysis", MessageBoxButtons.OK);
                return;
            }
            // set scroll bar bounds
            dtp_Analysis_MinValue.Value = History_OldestEntry;
            dtp_Analysis_MaxValue.Value = History_YoungestEntry;
            // enable buttons
            btn_Analysis_Planes_Load.Enabled = true;
            btn_Analysis_Planes_Save.Enabled = true;
            btn_Analysis_Planes_Clear.Enabled = true;
            btn_Analysis_Planes_History.Enabled = true;
            btn_Analysis_Planes_ShowTraffic.Enabled = true;
            btn_Analysis_Path_SaveToFile.Enabled = true;
            btn_Analysis_CrossingHistory.Enabled = true;
            btn_Analysis_Plane_History.Enabled = true;
            btn_Analysis_Rewind.Enabled = true;
            btn_Analysis_Back.Enabled = true;
            btn_Analysis_Pause.Enabled = true;
            btn_Analysis_Forward.Enabled = true;
            btn_Analysis_FastForward.Enabled = true;
            sb_Analysis_Play.Enabled = true;
            dtp_Analysis_MinValue.Enabled = true;
            dtp_Analysis_MaxValue.Enabled = true;
            UpdatePlayer();
            // set time to oldest entry
            Properties.Settings.Default.Time_Offline = History_OldestEntry;
            gb_Analysis_Player_SizeChanged(this, null);
            UpdatePaths();
            UpdateStatus();
            btn_Analysis_Planes_Load.Focus();
        }

        #endregion

        #region Analysis_FileSaver


        private void Analysis_Planes_Save_JSON(string filename)
        {
            int saved = 0;
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("[");
                for (int i = 0; i < AllPositions.Count; i++)
                {
                    if (AllPositions[i].LastUpdated < dtp_Analysis_MinValue.Value)
                        continue;
                    if (AllPositions[i].LastUpdated > dtp_Analysis_MaxValue.Value)
                        continue;
                    string json = AllPositions[i].ToJSON();
                    sw.Write(json);
                    if (i < AllPositions.Count - 1)
                    {
                        sw.WriteLine(",");
                    }
                    else
                        sw.WriteLine();
                    saved++;
                    if (saved % 1000 == 0)
                        bw_Analysis_FileSaver.ReportProgress(0, "Saving position " + saved.ToString() + "...");
                    if (bw_Analysis_FileSaver.CancellationPending)
                        return;
                }
                sw.WriteLine("]");
            }
        }

        private void Analysis_Planes_Save_CSV(string filename)
        {
            int saved = 0;
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("time[utc];hex;call;lat[deg];lon[deg];alt[ft];track[deg];speed[kts]");
                foreach (AircraftPositionDesignator ap in AllPositions)
                {
                    if (ap.LastUpdated < dtp_Analysis_MinValue.Value)
                        continue;
                    if (ap.LastUpdated > dtp_Analysis_MaxValue.Value)
                        continue;
                    sw.WriteLine(ap.LastUpdated.ToString("yyyy-MM-dd HH:mm:ssZ") + ";" +
                        ap.Hex + ";" +
                        ap.Call + ";" +
                        ap.Lat.ToString("F8", CultureInfo.InvariantCulture) + ";" +
                        ap.Lon.ToString("F8", CultureInfo.InvariantCulture) + ";" +
                        ap.Alt.ToString("F8", CultureInfo.InvariantCulture) + ";" +
                        ap.Track.ToString("F8", CultureInfo.InvariantCulture) + ";" +
                        ap.Speed.ToString("F8", CultureInfo.InvariantCulture)
                        );
                    saved++;
                    if (saved % 1000 == 0)
                        bw_Analysis_FileSaver.ReportProgress(0, "Saving position " + saved.ToString() + "...");
                    if (bw_Analysis_FileSaver.CancellationPending)
                        return;
                }
            }
        }

        private void bw_Analysis_FileSaver_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = (string)e.Argument;
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_Analysis_FileSaver";
            try
            {
                if (filename.ToLower().EndsWith(".json"))
                    Analysis_Planes_Save_JSON(filename);
                else if (filename.ToLower().EndsWith(".csv"))
                    Analysis_Planes_Save_CSV(filename);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
                bw_Analysis_FileSaver.ReportProgress(-1, ex.Message);
            }
            if (bw_Analysis_FileSaver.CancellationPending)
                e.Cancel = true;
        }

        private void bw_Analysis_FileSaver_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 0)
                SayAnalysis((string)e.UserState);
        }

        private void bw_Analysis_FileSaver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                SayAnalysis("Cancelled.");
            else
            {
                btn_Analysis_Planes_Save.Enabled = true;
                SayAnalysis("Ready.");
            }
        }

        #endregion

        #region Analysis_FileLoader

        private void Analysis_Planes_Load_JSON(string filename)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            settings.Culture = CultureInfo.InvariantCulture;
            List<AircraftPositionDesignator> aps = new List<AircraftPositionDesignator>();
            bw_Analysis_FileLoader.ReportProgress(0, "Opening file...");
            using (StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open)))
            {
                // check for starting bracket of array
                char c = (char)sr.Read();
                if (c != '[')
                    return;
                // read 1000 positions and update database
                int count = 0;
                int updated = 0;
                while (!sr.EndOfStream)
                {
                    aps.Clear();
                    int j = 0;
                    while ((j < 1000) && !sr.EndOfStream)
                    {
                        char nextChar;
                        StringBuilder line = new StringBuilder();
                        while ((j < 1000) && sr.Peek() > 0)
                        {
                            nextChar = (char)sr.Read();
                            line.Append(nextChar);
                            if (line[0] != '{')
                                line.Clear();
                            if (nextChar == '}')
                            {
                                AircraftPositionDesignator ap = JsonConvert.DeserializeObject<AircraftPositionDesignator>(line.ToString(), settings);
                                line.Clear();
                                aps.Add(ap);
                                j++;
                            }
                        }
                        if (bw_Analysis_FileLoader.CancellationPending)
                            return;
                    }
                    count = count + j;
                    updated = updated + AircraftPositionData.Database.AircraftPositionBulkInsertOrUpdateIfNewer(aps);
                    bw_Analysis_FileLoader.ReportProgress(0, "Updating position " + count.ToString() + ", " + updated.ToString() + " updated so far...");
                }

            }
        }

        private void Analysis_Planes_Load_CSV(string filename)
        {
            List<AircraftPositionDesignator> aps = new List<AircraftPositionDesignator>();
            bw_Analysis_FileLoader.ReportProgress(0, "Opening file...");
            using (StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open)))
            {
                // read header
                string header = sr.ReadLine();
                // split header
                string[] a = header.Split(';');
                // remove unit brackets and lower all
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = a[i].ToLower();
                    if (a[i].IndexOf('[') >= 0)
                        a[i] = a[i].Substring(0, a[i].IndexOf('['));
                }
                int lastupdated_index = Array.IndexOf(a, "time");
                int hex_index = Array.IndexOf(a, "hex");
                int call_index = Array.IndexOf(a, "call");
                int lat_index = Array.IndexOf(a, "lat");
                int lon_index = Array.IndexOf(a, "lon");
                int alt_index = Array.IndexOf(a, "alt");
                int track_index = Array.IndexOf(a, "track");
                int speed_index = Array.IndexOf(a, "speed");
                bw_Analysis_FileLoader.ReportProgress(0, "Creating position list...");
                // read 1000 positions and update database
                int count = 0;
                int updated = 0;
                while (!sr.EndOfStream)
                {
                    aps.Clear();
                    int j = 0;
                    while ((j < 1000) && !sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        if (!s.Contains(";"))
                            continue;
                        a = s.Split(';');
                        DateTime lastupdated = System.Convert.ToDateTime(a[lastupdated_index]).ToUniversalTime();
                        string hex = a[hex_index].ToUpper();
                        string call = a[call_index].ToUpper();
                        double lat = System.Convert.ToDouble(a[lat_index], CultureInfo.InvariantCulture);
                        double lon = System.Convert.ToDouble(a[lon_index], CultureInfo.InvariantCulture);
                        double alt = System.Convert.ToDouble(a[alt_index], CultureInfo.InvariantCulture);
                        double track = System.Convert.ToDouble(a[track_index], CultureInfo.InvariantCulture);
                        double speed = System.Convert.ToDouble(a[speed_index], CultureInfo.InvariantCulture);
                        AircraftPositionDesignator ap = new AircraftPositionDesignator(hex, call, lat, lon, alt, track, speed, lastupdated);
                        aps.Add(ap);
                        j++;
                        if (bw_Analysis_FileLoader.CancellationPending)
                            return;
                    }
                    count = count + j;
                    updated = updated + AircraftPositionData.Database.AircraftPositionBulkInsertOrUpdateIfNewer(aps);
                    bw_Analysis_FileLoader.ReportProgress(0, "Updating position " + count.ToString() + ", " + updated.ToString() + " updated so far...");
                }

            }
        }

        private void bw_Analysis_FileLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = (string)e.Argument;
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "bw_Analysis_FileLoader";
            try
            {
                if (filename.ToLower().EndsWith(".json"))
                    Analysis_Planes_Load_JSON(filename);
                else if (filename.ToLower().EndsWith(".csv"))
                    Analysis_Planes_Load_CSV(filename);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
                bw_Analysis_FileLoader.ReportProgress(-1, ex.Message);
            }
            if (bw_Analysis_FileLoader.CancellationPending)
                e.Cancel = true;
        }

        private void bw_Analysis_FileLoader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 0)
                SayAnalysis((string)e.UserState);
        }

        private void bw_Analysis_FileLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                SayAnalysis("Cancelled.");
            else
            {
                btn_Analysis_Planes_Load.Enabled = true;
                SayAnalysis("Ready.");
            }
        }

        #endregion


        #region AirportMapper

        private void bw_AirportMapper_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_AirportMapper.ReportProgress(0, "Getting airports from database...");
            Stopwatch st = new Stopwatch();
            st.Start();
            // fill the airports layer of maps
            // return if switched off
            if (!Properties.Settings.Default.Airports_Activate)
                return;
            List<AirportDesignator> airports = new List<AirportDesignator>();
            airports = AircraftData.Database.AirportGetAll(bw_AirportMapper);
            if (airports != null)
                bw_AirportMapper.ReportProgress(100, airports);
            st.Stop();
            bw_AirportMapper.ReportProgress(0, airports.Count.ToString() + " airports updated, " + st.ElapsedMilliseconds.ToString() + " ms.");
        }

        private void bw_AirportMapper_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                // log error message
                string msg = (string)e.UserState;
                Say(msg);
                Log.WriteMessage(msg, LogLevel.Error);
            }
            else if (e.ProgressPercentage == 100)
            {
                // add aiports to overlay
                lock (Airports)
                {
                    Airports = (List<AirportDesignator>)e.UserState;
                }
                UpdateAirports();
            }
        }

        private void bw_AirportMapper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        private void gm_Main_Load(object sender, EventArgs e)
        {

        }

        private void tc_Main_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // cancel tab change when in PLAY mode
            if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                e.Cancel = true;
        }

        private void tc_Map_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // cancel tab change when in PLAY mode
            if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                e.Cancel = true;
        }

        private void tc_Control_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // cancel tab change when in PLAY mode
            if (PlayMode == AIRSCOUTPLAYMODE.FORWARD)
                e.Cancel = true;
        }
    }


    public class ClippingToolStripRenderer : ToolStripSystemRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            ToolStripStatusLabel label = e.Item as ToolStripStatusLabel;

            if (label != null)
            {
                TextRenderer.DrawText(e.Graphics, label.Text,
                    label.Font, e.TextRectangle,
                    label.ForeColor,
                    TextFormatFlags.EndEllipsis);
            }
            else
            {
                base.OnRenderItemText(e);
            }
        }
    }

    public class LocatorDropDownItem
    {
        public string Locator { get; set; }
        public LatLon.GPoint GeoLocation { get; set; }

        public LocatorDropDownItem(string locator, LatLon.GPoint geolocation)
        {
            Locator = locator;
            GeoLocation = geolocation;
        }
    }

}
