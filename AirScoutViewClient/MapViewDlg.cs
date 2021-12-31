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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Configuration;
using System.Diagnostics;
using AirScout;
using AirScout.Core;
using AirScout.Aircrafts;
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Elevation;
using ScoutBase.Stations;
using ScoutBase.Propagation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLiteDatabase;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Data.SQLite;

namespace AirScoutViewClient
{

    public partial class MapViewDlg : Form
    {

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

        // Routes
        GMapRoute gmr_FullPath;
        GMapRoute gmr_VisiblePpath;
        GMapRoute gmr_NearestFull;
        GMapRoute gmr_NearestVisible;

        // Markers
        GMapMarker gmm_MyLoc;
        GMapMarker gmm_DXLoc;
        GMapMarker gmm_CurrentMarker;
        bool isDraggingMarker;

        // Bitmap indices
        private int bmindex_darkorange;
        private int bmindex_lightgreen;
        private int bmindex_red;
        private int bmindex_gray;
        private int bmindex_magenta;

        // charting

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


        // Tooltip font
        public Font ToolTipFont;

        public VarConverter VC = new VarConverter();

        public static LogWriter Log;

        public DateTime CurrentTime;

        AIRSCOUTPLAYMODE PlayMode = AIRSCOUTPLAYMODE.NONE;

        CultureInfo CurrentCulture = Application.CurrentCulture;

        ElevationPathDesignator EPath = null;
        PropagationPathDesignator PPath = null;

        SortedList<string, PlaneInfo> ActivePlanes = new SortedList<string, PlaneInfo>();
        private List<string> SelectedPlanes = new List<string>();

        private VIEWCLIENTSTATUS ViewClientStatus = VIEWCLIENTSTATUS.NONE;

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

        public MapViewDlg()
        {
            Application.CurrentCulture = CultureInfo.InvariantCulture;
            // gets an instance of a LogWriter
            Log = LogWriter.Instance;
            InitializeComponent();

        }

        private void Say (string msg)
        {
            if (tsl_Status.Text != msg)
            {
                tsl_Status.Text = msg;
                ss_Main.Refresh();
            }
        }

        #region Initialization

        private void InitializeSettings()
        {
            Say("Getting settings...");
            if (!SettingsFromJSON())
                return;
            // initialize map
            // setting User Agent to fix Open Street Map issue 2016-09-20
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
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
            gm_Main.Overlays.Add(gmo_Airports);
            gm_Main.Overlays.Add(gmo_Callsigns);
            gm_Main.Overlays.Add(gmo_PropagationPaths);
            gm_Main.Overlays.Add(gmo_Routes);
            gm_Main.Overlays.Add(gmo_Objects);
            gm_Main.Overlays.Add(gmo_Planes);
            // center map
            gm_Main.Position = new PointLatLng(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
            // intially fill dialog box elements and set band
            string[] bands = Bands.GetStringValuesExceptNoneAndAll();
            foreach (string b in bands)
                cb_Band.Items.Add(b);
            BAND band = Properties.Settings.Default.Band;
            cb_Band.SelectedItem = Bands.GetStringValue(band);
            PlayMode = AIRSCOUTPLAYMODE.PAUSE;
            ViewClientStatus = VIEWCLIENTSTATUS.CONNECTED;
            UpdateStatus();
            Say("");
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
            pm_Path.DefaultFontSize = 6F;
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
            this.gb_Path.Controls.Add(pv_Path);
            pv_Path.Paint += new PaintEventHandler(pv_Path_Paint);

            // zoomed elevation chart
            pm_Elevation.Title = String.Empty;
            pm_Elevation.DefaultFontSize = 6F;
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
            this.gb_Path.Controls.Add(pv_Elevation);
            gb_Path_Resize(this, null);
        }

        #endregion

        private void UpdateStatus()
        {
            // upddate TextBoxes
            tb_UTC.Text = CurrentTime.ToString("yyyy-MM-dd HH:mm:ss");
            tb_UTC.BackColor = Color.LightSalmon;
            string call = Properties.Settings.Default.MyCall;
            tb_MyCall.SilentText = Properties.Settings.Default.MyCall;
            cb_MyLoc.SilentText = MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon,
                Properties.Settings.Default.Locator_SmallLettersForSubsquares,
                (int)Properties.Settings.Default.Locator_MaxLength / 2,
                Properties.Settings.Default.Locator_AutoLength);
            tb_DXCall.Text = Properties.Settings.Default.DXCall;
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

        private string GetServerURL(string server, int port, string filename, string paramstr = "")
        {
            if (!server.StartsWith("http://"))
                server = "http://" + server;
            server = server.TrimEnd('/');
            return (String.IsNullOrEmpty(paramstr)) ? server + ":" + port.ToString() + "/" + filename : server + ":" + port.ToString() + "/" + filename + "?" + paramstr;
        }

        private string GetJSONFromURL(string url)
        {
            // reads JSON from webserver 
            // returns an empty string or null if webserver is not responding
            // returns error message from webserver in case of error
            // returns JSON string if successful
            string json = "";
            // calculate url and get json
            // replace all vaiables, if any
            url = VC.ReplaceAllVars(url);
            try
            {
                Console.WriteLine("Creating web request: " + url);
                HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webrequest.Referer = "";
                webrequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:37.0) Gecko/20100101 Firefox/37.0";
                webrequest.Accept = "application/json, text/javascript, */*;q=0.01";
                webrequest.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
                Console.WriteLine("Getting web response");
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                Console.WriteLine("Reading stream");
                using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
                // do nothing
            }
            return json;
        }

        public T PropertyFromJSON<T>(string propertyName, string json)
        {
            var allobjects = JObject.Parse(json);
            JToken jo = allobjects[propertyName];
            return (T)jo.ToObject(typeof(T));
        }

        private bool SettingsFromJSON()
        {
            try
            {
                string json = "";
                // get settings
                json = GetJSONFromURL(GetServerURL(Properties.Settings.Default.Server_URL, (int)Properties.Settings.Default.Server_Port, "settings.json"));
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return false; 
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return false;
                }
                // copy settings
                Properties.Settings.Default.Path_Band_Settings = PropertyFromJSON<BandSettings>("Path_Band_Settings", json);
                Properties.Settings.Default.Map_Provider = PropertyFromJSON<string>("Map_Provider", json);
                Properties.Settings.Default.MinLat = PropertyFromJSON<double>("MinLat", json);
                Properties.Settings.Default.MinLon = PropertyFromJSON<double>("MinLon", json);
                Properties.Settings.Default.MaxLat = PropertyFromJSON<double>("MaxLat", json);
                Properties.Settings.Default.MaxLon = PropertyFromJSON<double>("MaxLon", json);
                Properties.Settings.Default.MyCall = PropertyFromJSON<string>("MyCall", json);
                Properties.Settings.Default.MyLat = PropertyFromJSON<double>("MyLat", json);
                Properties.Settings.Default.MyLon = PropertyFromJSON<double>("MyLon", json);
                // copy DXCall only if empty
                if (String.IsNullOrEmpty(Properties.Settings.Default.DXCall))
                {
                    Properties.Settings.Default.DXCall = PropertyFromJSON<string>("DXCall", json);
                    Properties.Settings.Default.DXLat = PropertyFromJSON<double>("DXLat", json);
                    Properties.Settings.Default.DXLon = PropertyFromJSON<double>("DXLon", json);
                }
                // set band only if NONE
                if (Properties.Settings.Default.Band != BAND.BNONE)
                    Properties.Settings.Default.Band = PropertyFromJSON<BAND>("Band", json);
                Properties.Settings.Default.InfoWin_Position = PropertyFromJSON<bool>("InfoWin_Position", json);
                Properties.Settings.Default.InfoWin_Metric = PropertyFromJSON<bool>("InfoWin_Metric", json);
                Properties.Settings.Default.InfoWin_Track = PropertyFromJSON<bool>("InfoWin_Track", json);
                Properties.Settings.Default.InfoWin_Speed = PropertyFromJSON<bool>("InfoWin_Speed", json);
                Properties.Settings.Default.InfoWin_Alt = PropertyFromJSON<bool>("InfoWin_Alt", json);
                Properties.Settings.Default.InfoWin_Type = PropertyFromJSON<bool>("InfoWin_Type", json);
                Properties.Settings.Default.InfoWin_Time = PropertyFromJSON<bool>("InfoWin_Time", json);
                Properties.Settings.Default.InfoWin_Dist = PropertyFromJSON<bool>("InfoWin_Dist", json);
                Properties.Settings.Default.InfoWin_Angle = PropertyFromJSON<bool>("InfoWin_Angle", json);
                Properties.Settings.Default.InfoWin_Epsilon = PropertyFromJSON<bool>("InfoWin_Epsilon", json);
                Properties.Settings.Default.InfoWin_Squint = PropertyFromJSON<bool>("InfoWin_Squint", json);
                Properties.Settings.Default.Locator_AutoLength = PropertyFromJSON<bool>("Locator_AutoLength", json);
                Properties.Settings.Default.Locator_SmallLettersForSubsquares = PropertyFromJSON<bool>("Locator_SmallLettersForSubsquares", json);
                Properties.Settings.Default.Locator_MaxLength = PropertyFromJSON<decimal>("Locator_MaxLength", json);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return true;
        }

        private List<LocationDesignator> MyLocationsFromJSON()
        {
            string json = "";
            List<LocationDesignator> l = null;
            try
            {
                string url = GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "location.json",
                    "call=" + Properties.Settings.Default.MyCall +
                    "&loc=all");
                json = GetJSONFromURL(url);
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                settings.Culture = CultureInfo.InvariantCulture;
                l = JsonConvert.DeserializeObject<List<LocationDesignator>>(json, settings);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return l;
        }

        private LocationDesignator MyLocationFromJSON()
        {
            string json = "";
            LocationDesignator ld = null;
            try
            {
                string url = GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "location.json",
                    "call=" + Properties.Settings.Default.MyCall +
                    "&loc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, false, 3));
                json = GetJSONFromURL(url);
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                ld = LocationDesignator.FromJSON<LocationDesignator>(json);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return ld;
        }

        private List<LocationDesignator> DXLocationsFromJSON()
        {
            string json = "";
            List<LocationDesignator> l = null;
            try
            {
                string url = GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "location.json",
                    "call=" + Properties.Settings.Default.DXCall +
                    "&loc=all");
                json = GetJSONFromURL(url);
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                settings.Culture = CultureInfo.InvariantCulture;
                l = JsonConvert.DeserializeObject<List<LocationDesignator>>(json, settings);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return l;
        }

        private LocationDesignator DXLocationFromJSON()
        {
            string json = "";
            LocationDesignator ld = null;
            try
            {
                string url = GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "location.json",
                    "&call=" + Properties.Settings.Default.DXCall +
                    "&loc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3));
                json = GetJSONFromURL(url);
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                ld = LocationDesignator.FromJSON<LocationDesignator>(json);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return ld;
        }

        private ElevationPathDesignator ElevationPathFromJSON()
        {
            string json = "";
            try
            {
                json = GetJSONFromURL(GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "elevationpath.json",
                    "mycall=" + Properties.Settings.Default.MyCall +
                    "&myloc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, false, 3) +
                    "&dxcall=" + Properties.Settings.Default.DXCall +
                    "&dxloc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3)));
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                EPath = ElevationPathDesignator.FromJSON<ElevationPathDesignator>(json);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return EPath;
        }

        private PropagationPathDesignator PropagationPathFromJSON()
        {
            string json = "";
            try
            {
                json = GetJSONFromURL(GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "propagationpath.json",
                    "mycall=" + Properties.Settings.Default.MyCall +
                    "&myloc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, false, 3) +
                    "&dxcall=" + Properties.Settings.Default.DXCall +
                    "&dxloc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3)));
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                PPath = PropagationPathDesignator.FromJSON<PropagationPathDesignator>(json);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return PPath;
        }

        private List<PlaneInfo> NearestPlanesFromJSON()
        {
            string json = "";
            List<PlaneInfo> l = null;
            try
            {
                json = GetJSONFromURL(GetServerURL(
                    Properties.Settings.Default.Server_URL,
                    (int)Properties.Settings.Default.Server_Port,
                    "nearestplanes.json",
                    "mycall=" + Properties.Settings.Default.MyCall +
                    "&myloc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, false, 3) +
                    "&dxcall=" + Properties.Settings.Default.DXCall +
                "&dxloc=" + MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3)));
                if (String.IsNullOrEmpty(json))
                {
                    ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
                    return null;
                }
                if (json.StartsWith("Error:"))
                {
                    Say(json);
                    return null;
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                settings.Culture = CultureInfo.InvariantCulture;
                l = (List<PlaneInfo>)JsonConvert.DeserializeObject<List<PlaneInfo>>(json, settings);
            }
            catch (Exception ex)
            {
                Say(ex.Message);
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return l;
        }

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
            GMarkerGoogle m = new GMarkerGoogle(new PointLatLng(info.Lat, info.Lon), ToolTipFont, RotateImageByAngle(bm, (float)info.Track));
            m.Tag = info.Call;
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
            GMarkerGoogle m = new GMarkerGoogle(new PointLatLng(info.Lat, info.Lon), ToolTipFont, RotateImageByAngle(bm, (float)info.Track));
            m.Tag = info.Call;
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
                m.ToolTipText += "\nTrack: " + info.Track + "°";
            if (Properties.Settings.Default.InfoWin_Speed)
            {
                if (Properties.Settings.Default.InfoWin_Metric)
                    m.ToolTipText += "\nSpeed: " + info.Speed_kmh.ToString("F0") + "km/h";
                else
                    m.ToolTipText += "\nSpeed: " + info.Speed.ToString("F0") + "kts";
            }
            if (Properties.Settings.Default.InfoWin_Type)
                m.ToolTipText += "\nType: " + info.Manufacturer + " " + info.Model + " [" + PlaneCategories.GetShortStringValue(info.Category) + "]";
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
            bool isselected = false;

            if (EPath == null)
                return;
            if (PPath == null)
                return;

            // check for filter settings
            // and color filter box
            int planes_filter_minalt = 0;
            planes_filter_minalt = Properties.Settings.Default.Planes_Filter_Min_Alt;
            if (planes_filter_minalt < 0)
                planes_filter_minalt = 0;
            Stopwatch st = new Stopwatch();
            st.Start();

            // clear planes overlay in map
            gmo_Planes.Clear();
            // clear all routes except paths
            gmo_Routes.Clear();
            // clear active planes
            ActivePlanes.Clear();

            // clear data points in chart
            Planes_Hi.Points.Clear();
            Planes_Lo.Points.Clear();

            // get nearest planes per path
            List<PlaneInfo> nearestplanes = NearestPlanesFromJSON();

            if (nearestplanes == null)
                return;
            foreach (PlaneInfo plane in nearestplanes)
            {
                // add or update plane in active planes list
                PlaneInfo activeplane = null;
                if (ActivePlanes.TryGetValue(plane.Hex, out activeplane))
                {
                    // plane found --> update if necessary
                    bool update = false;
                    // plane has higher potential
                    if (plane.Potential > activeplane.Potential)
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
            }
            st.Stop();
            Say("Get nearest planes: " + ActivePlanes.Count.ToString() + " plane(s), " + st.ElapsedMilliseconds.ToString() + " ms.");
            st.Reset();
            st.Start();
            Log.WriteMessage("Drawing planes: " + ActivePlanes.Count.ToString() + " plane(s), " + st.ElapsedMilliseconds.ToString() + " ms.");

            // check if any plane is on list --> return empty list
            if ((ActivePlanes == null) || (ActivePlanes.Count == 0))
                return;
            // draw all planes
            foreach (PlaneInfo plane in ActivePlanes.Values)
            {
                try
                {
                    // show planes if it meets filter criteria
                    if ((plane.Alt_m >= Properties.Settings.Default.Planes_MinAlt) && (plane.Category >= Properties.Settings.Default.Planes_Filter_Min_Category))
                    {
                        // check selected state
                        isselected = SelectedPlanes.IndexOf(plane.Call) >= 0;
                        // now, show plane according to potential
                        switch (plane.Potential)
                        {
                            case 100:
                                gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                break;
                            case 75:
                                gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                break;
                            case 50:
                                gmo_Planes.Markers.Add(CreatePlaneDetailed(plane, isselected));
                                break;
                            default:
                                gmo_Planes.Markers.Add(CreatePlaneSimple(plane, isselected));
                                break;
                        }

                        // if selected: draw the thin path to crossing point if one
                        if (isselected)
                        {
                            if (plane.IntPoint != null)
                            {
                                GMapRoute intpath = new GMapRoute(plane.Call);
                                intpath.Stroke = new Pen(Color.Black, 1);
                                intpath.Points.Add(new PointLatLng(plane.Lat, plane.Lon));
                                intpath.Points.Add(new PointLatLng(plane.IntPoint.Lat, plane.IntPoint.Lon));
                                gmo_Routes.Routes.Add(intpath);
                                //                                Console.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + ";" + info.IntQRB.ToString("F3"));
                            }
                        }

                        // show planes on chart if in sigle path mode
                        if ((plane.IntPoint != null) && (plane.IntQRB <= Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].MaxDistance))
                        {
                            // calculate distance from mylat/mylon
                            double dist = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, plane.IntPoint.Lat, plane.IntPoint.Lon);
                            // add new data points
                            if (plane.AltDiff > 0)
                            {
                                Planes_Hi.Points.Add(new DataPoint(dist, plane.Alt_m));
                            }
                            else
                            {
                                Planes_Lo.Points.Add(new DataPoint(dist, plane.Alt_m));
                            }
                        }
                    }

                    // invalidate chart
                    pm_Path.InvalidatePlot(true);

                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
        }

        private void DrawPath()
        {
            ElevationPathDesignator epath = ElevationPathFromJSON();
            if (epath == null)
                return;
            // set MyDetails and DXDetails
            Properties.Settings.Default.MyCall = epath.Location1.Call;
            Properties.Settings.Default.MyLat = epath.Lat1;
            Properties.Settings.Default.MyLon = epath.Lon1;
            Properties.Settings.Default.DXCall = epath.Location2.Call;
            Properties.Settings.Default.DXLat = epath.Lat2;
            Properties.Settings.Default.DXLon = epath.Lon2;
            PropagationPathDesignator ppath = PropagationPathFromJSON();
            if (ppath == null)
                return;
            // clear charts
            ClearCharts();
            // draw my end on the map
            gmm_MyLoc = new GMarkerGoogle(new PointLatLng(epath.Location1.Lat, epath.Location1.Lon), ToolTipFont, Properties.Settings.Default.Map_SmallMarkers ? GMarkerGoogleType.red_small : GMarkerGoogleType.red_dot);
            gmm_MyLoc.ToolTipText = epath.Location1.Call + "\n" +
                epath.Location1.Lat.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                epath.Location1.Lon.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                epath.Location1.Loc + "\n" +
                epath.Location1.Elevation.ToString("F0") + "m";
            gmm_MyLoc.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            gmm_MyLoc.Tag = epath.Location1.Call;
            gmo_Objects.Markers.Add(gmm_MyLoc);
            // draws a propagation path to map
            PropagationPoint[] ppoints = new PropagationPoint[0];
            try
            {
                // get infopoints for map
                ppoints = ppath.GetInfoPoints();
                // calculate midpoint
                ScoutBase.Core.LatLon.GPoint midpoint = LatLon.MidPoint(ppath.Lat1, ppath.Lon1, ppath.Lat2, ppath.Lon2);
                GMapMarker gmmid = new GMarkerGoogle(new PointLatLng(midpoint.Lat, midpoint.Lon), ToolTipFont, Properties.Settings.Default.Map_SmallMarkers ? GMarkerGoogleType.blue_small : GMarkerGoogleType.blue_dot);
                gmmid.ToolTipText = ppath.Location1.Call + " <> " + ppath.Location2.Call;
                gmmid.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                gmo_Objects.Markers.Add(gmmid);
                // calculate dx end
                gmm_DXLoc = new GMarkerGoogle(new PointLatLng(epath.Lat2, epath.Lon2), ToolTipFont, Properties.Settings.Default.Map_SmallMarkers ? GMarkerGoogleType.yellow_small : GMarkerGoogleType.yellow_dot);
                gmm_DXLoc.ToolTipText = epath.Location2.Call + "\n" +
                    epath.Location2.Lat.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                    epath.Location2.Lon.ToString("F8", CultureInfo.InvariantCulture) + "\n" +
                    epath.Location2.Loc + "\n" +
                    epath.Location2.Elevation.ToString("F0") + "m";
                gmm_DXLoc.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                gmm_DXLoc.Tag = epath.Location2.Call;
                gmo_Objects.Markers.Add(gmm_DXLoc);
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

                // calculate bounds
                double minlat = Math.Min(epath.Lat1, epath.Lat2);
                double minlon = Math.Min(epath.Lon1, epath.Lon2);
                double maxlat = Math.Max(epath.Lat1, epath.Lat2);
                double maxlon = Math.Max(epath.Lon1, epath.Lon2);

                // calculate center
                double centerlat = LatLon.MidPoint(minlat, minlon, maxlat, maxlon).Lat;
                double centerlon = LatLon.MidPoint(minlat, minlon, maxlat, maxlon).Lon;

                // ensure that whole path is visible and optionally centered
                gm_Main.SetZoomToFitRect(RectLatLng.FromLTRB(minlon, maxlat, maxlon, minlat));
                gm_Main.Position = new PointLatLng(centerlat, centerlon);

                UpdateCharts(epath, ppath);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void ti_Startup_Tick(object sender, EventArgs e)
        {
            ti_Startup.Stop();
            // start timer
            ti_Progress.Start();
        }

        private void ti_Progress_Tick(object sender, EventArgs e)
        {
            tsl_ConnectionStatus.Text = ViewClientStatus.ToString();
            switch (ViewClientStatus)
            {
                case VIEWCLIENTSTATUS.CONNECTING:
                    {
                        tsl_ConnectionStatus.BackColor = Color.Yellow;
                        tsl_ConnectionStatus.ForeColor = Color.Green;
                        InitializeSettings();
                        break;
                    }
                case VIEWCLIENTSTATUS.CONNECTED:
                    {
                        tsl_ConnectionStatus.BackColor = Color.Green;
                        tsl_ConnectionStatus.ForeColor = Color.White;
                        if (PlayMode != AIRSCOUTPLAYMODE.FORWARD)
                            return;
                        CurrentTime = DateTime.UtcNow;
                        UpdateStatus();
                        DrawPlanes();
                        break;
                    }
                case VIEWCLIENTSTATUS.ERROR:
                    {
                        tsl_ConnectionStatus.BackColor = Color.Red;
                        tsl_ConnectionStatus.ForeColor = Color.White;
                        break;
                    }
                default:
                    {
                        tsl_ConnectionStatus.BackColor = SystemColors.Control;
                        tsl_ConnectionStatus.ForeColor = SystemColors.ControlText;
                        break;
                    }
            }
        }

        private void MapViewDlg_Load(object sender, EventArgs e)
        {
            ViewClientStatus = VIEWCLIENTSTATUS.INIT;
            InitializeIcons();
            InitializeCharts();
            this.Text = "AirScout View Client V" + Application.ProductVersion;
            this.Show();
            ti_Progress.Start();
            ViewClientStatus = VIEWCLIENTSTATUS.CONNECTING;
        }

        private void tsi_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MapViewDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            ti_Progress.Stop();
            Properties.Settings.Default.Save();
        }

        private double GetMinH(double max_alt, double H1, double H2)
        {
            double max = Math.Max(H1, H2);
            if (max <= max_alt)
                return max;
            return max_alt;
        }


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
                double eps_los = Propagation.EpsilonFromHeights(epath.Location1.Elevation + ppath.QRV1.AntennaHeight, ppath.Distance, epath.Location2.Elevation + ppath.QRV2.AntennaHeight, LatLon.Earth.Radius);
                // fill chart
                short maxelv = short.MinValue;
                double myelev = epath.Location1.Elevation;
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

        private void gb_Path_Resize(object sender, EventArgs e)
        {
            // adjust charts
            pv_Path.Location = new System.Drawing.Point(0, 0);
            pv_Path.Width = gb_Path.Width;
            pv_Path.Height = gb_Path.Height / 2;
            pv_Elevation.Location = new System.Drawing.Point(0, gb_Path.Height / 2);
            pv_Elevation.Width = gb_Path.Width;
            pv_Elevation.Height = gb_Path.Height / 2;
        }

        private void gm_Main_OnMapZoomChanged()
        {
            double midlat = LatLon.MidPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon).Lat;
            double midlon = LatLon.MidPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon).Lon;
            if ((PlayMode == AIRSCOUTPLAYMODE.FORWARD) && Properties.Settings.Default.Map_AutoCenter)
                gm_Main.Position = new PointLatLng(midlat, midlon);
            tb_Map_Zoom.Text = gm_Main.Zoom.ToString();
        }

        private void gm_Main_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    // check if plane clicked
                    if (item.Overlay == gmo_Planes)
                    {
                        // keep the selected state
                        int selectedindex = SelectedPlanes.IndexOf((string)item.Tag);
                        // clear all other selections if tracking is en
                        // toogle selection of the selected plane
                        if (selectedindex >= 0)
                        {
                            // remove item from selected planes list
                            SelectedPlanes.RemoveAt(selectedindex);
                        }
                        else
                        {
                            SelectedPlanes.Add((string)item.Tag);
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

        private void btn_Map_Zoom_In_Click(object sender, EventArgs e)
        {
            if (gm_Main.Zoom < 20)
                gm_Main.Zoom++;
        }

        private void btn_Map_Zoom_Out_Click(object sender, EventArgs e)
        {
            if (gm_Main.Zoom > 0)
                gm_Main.Zoom--;
        }

        private void tb_MyCall_TextChanged(object sender, EventArgs e)
        {
            if (!Callsign.Check(tb_MyCall.Text))
                return;
            Properties.Settings.Default.MyCall = tb_MyCall.Text;
            // get callsign info
            List<LocationDesignator> l = MyLocationsFromJSON();
            if (l == null)
            {
                cb_MyLoc.Items.Clear();
                cb_MyLoc.SilentText = "";
                return;
            }
            cb_MyLoc.Items.Clear();
            cb_MyLoc.DisplayMember = "Loc";
            foreach (LocationDesignator ld in l)
            {
                cb_MyLoc.Items.Add(ld);
            }
            if (cb_MyLoc.Items.Count > 0)
            {
                cb_MyLoc.SelectedItem = cb_MyLoc.Items[0];
                Properties.Settings.Default.MyLat = ((LocationDesignator)cb_MyLoc.Items[0]).Lat;
                Properties.Settings.Default.MyLon = ((LocationDesignator)cb_MyLoc.Items[0]).Lon;
            }
        }

        private void tb_DXCall_TextChanged(object sender, EventArgs e)
        {
            if (!Callsign.Check(tb_DXCall.Text))
                return;
            Properties.Settings.Default.DXCall = tb_DXCall.Text;
            // get callsign info
            List<LocationDesignator> l = DXLocationsFromJSON();
            if (l == null)
            {
                cb_DXLoc.Items.Clear();
                cb_DXLoc.SilentText = "";
                return;
            }
            cb_DXLoc.Items.Clear();
            cb_DXLoc.DisplayMember = "Loc";
            foreach (LocationDesignator ld in l)
            {
                cb_DXLoc.Items.Add(ld);
            }
            if (cb_DXLoc.Items.Count > 0)
            {
                cb_DXLoc.SelectedItem = cb_DXLoc.Items[0];
                Properties.Settings.Default.DXLat = ((LocationDesignator)cb_DXLoc.Items[0]).Lat;
                Properties.Settings.Default.DXLon = ((LocationDesignator)cb_DXLoc.Items[0]).Lon;
            }
        }

        private void cb_MyLoc_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_DXLoc_TextChanged(object sender, EventArgs e)
        {

        }

        private void tsi_Info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AirScout View Client V" + Application.ProductVersion.ToString(), "Info", MessageBoxButtons.OK);
        }

        private void btn_Map_PlayPause_Click(object sender, EventArgs e)
        {
            if (PlayMode != AIRSCOUTPLAYMODE.FORWARD)
            {
                PlayMode = AIRSCOUTPLAYMODE.FORWARD;
                btn_Map_PlayPause.ImageIndex = 0;
                // draw path
                DrawPath();
            }
            else
            {
                PlayMode = AIRSCOUTPLAYMODE.PAUSE;
                btn_Map_PlayPause.ImageIndex = 1;
            }

        }

        private void mnu_Settings_Click(object sender, EventArgs e)
        {
            SettingsDlg Dlg = new SettingsDlg();
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Save();
                ViewClientStatus = VIEWCLIENTSTATUS.INIT;
                InitializeSettings();
            }
            else
            {
                Properties.Settings.Default.Reload();
            }
        }
    }
}
