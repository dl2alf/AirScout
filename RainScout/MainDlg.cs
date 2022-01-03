using DeviceId;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using RainScout.Radars;
using ScoutBase.Core;
using ScoutBase.Elevation;
using ScoutBase.Propagation;
using ScoutBase.Stations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace RainScout
{
    public partial class MainDlg : Form
    {
        private readonly double HorDistance = 500;

        // Background workers
        public ElevationDatabaseUpdater bw_GLOBEUpdater = new ElevationDatabaseUpdater();
        public ElevationDatabaseUpdater bw_SRTM3Updater = new ElevationDatabaseUpdater();
        public ElevationDatabaseUpdater bw_SRTM1Updater = new ElevationDatabaseUpdater();

        public StationDatabaseUpdater bw_StationDatabaseUpdater = new StationDatabaseUpdater();

        // Session key
        public string SessionKey = "";

        // Map layer
        GMapOverlay gm_mystations = new GMapOverlay("mystations");
        GMapOverlay gm_dxstations = new GMapOverlay("dxstations");
        GMapOverlay gm_scatters = new GMapOverlay("scatters");
        GMapOverlay gm_horizons = new GMapOverlay("horizons");
        GMapOverlay gm_distances = new GMapOverlay("distances");
        GMapOverlay gm_bounds = new GMapOverlay("bounds");
        GMapOverlay gm_intensity = new GMapOverlay("intensity");
        GMapOverlay gm_cloudtops = new GMapOverlay("cloudtops");
        GMapOverlay gm_lightning = new GMapOverlay("lightning");
        GMapOverlay gm_myroutes = new GMapOverlay("myroutes");
        GMapOverlay gm_dxroutes = new GMapOverlay("dxroutes");

        Size RadarSize = new Size(0, 0);
        Size RadarMapSize = new Size(0, 0);

        RadarMap RadarMap = null;

        private bool Loaded = false;
        private bool MapDragging = false;
        private GMapMarker CurrentMarker;
        private bool IsDraggingMarker;

        Dictionary<string, GenericRadar> Radars = new Dictionary<string, GenericRadar>();
        public Dictionary<BAND, Dictionary<string, LocationEntry>> Locations = null;

        public RAINSCOUTSTATUS Status = RAINSCOUTSTATUS.NONE;

        public MainDlg()
        {
            InitializeComponent();

            InitializeSettings();
            InitializeRadars();

            // set elevation database update event handler
            bw_GLOBEUpdater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);
            bw_SRTM3Updater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);
            bw_SRTM1Updater.ProgressChanged += new ProgressChangedEventHandler(bw_ElevationDatabaseUpdater_ProgressChanged);

            // set station database updater event handler
            bw_StationDatabaseUpdater.ProgressChanged += new ProgressChangedEventHandler(bw_StationDatabaseUpdater_ProgressChanged);
            bw_StationDatabaseUpdater.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StationDatabaseUpdater_RunWorkerCompleted);

            // set initial settings for main map
            gm_Main.MapProvider = GMapProviders.OpenStreetMap;
            gm_Main.IgnoreMarkerOnMouseWheel = true;
            gm_Main.MinZoom = 0;
            gm_Main.MaxZoom = 20;
            gm_Main.Zoom = 2;
            gm_Main.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Main.CanDragMap = true;
            gm_Main.ScalePen = new Pen(Color.Black, 3);
            gm_Main.MapScaleInfoEnabled = true;
            gm_Main.Overlays.Add(gm_bounds);
            gm_Main.Overlays.Add(gm_distances);
            gm_Main.Overlays.Add(gm_intensity);
            gm_Main.Overlays.Add(gm_cloudtops);
            gm_Main.Overlays.Add(gm_lightning);
            gm_Main.Overlays.Add(gm_mystations);
            gm_Main.Overlays.Add(gm_horizons);
            gm_Main.Overlays.Add(gm_myroutes);
            gm_Main.Overlays.Add(gm_dxstations);
            gm_Main.Overlays.Add(gm_dxroutes);
            gm_Main.Overlays.Add(gm_scatters);


    }

    private void MainDlg_Shown(object sender, EventArgs e)
        {
            InitializeLocations();
            UpdateMyLocation();

            try
            {
                EnumHelpers.BindToEnum<BAND>(cb_Band);
                cb_Band.SelectedValue = (int)Properties.Settings.Default.CurrentBand;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                EnumHelpers.BindToEnum<ELEVATIONMODEL>(cb_ElevationModel);
                cb_ElevationModel.SelectedValue = (int)Properties.Settings.Default.CurrentElevationModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                cb_Radar.Items.Clear();
                foreach (GenericRadar radar in Radars.Values)
                {
                    cb_Radar.Items.Add(radar.Name);
                }
                cb_Radar.SelectedItem = RainScout.Properties.Settings.Default.CurrentRadar;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            gm_Main.SetZoomToFitRect(RectLatLng.FromLTRB(
                (double)Properties.Settings.Default.MinLon,
                (double)Properties.Settings.Default.MaxLat,
                (double)Properties.Settings.Default.MaxLon,
                (double)Properties.Settings.Default.MinLat));

            ClearScp();

            Loaded = true;

            StartAllBackgroundWorkers();
            Say("Ready.");
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();

        }

        private void Say(string msg)
        {
            if (!String.IsNullOrEmpty(msg) && (tsl_Main.Text != msg))
            {
                tsl_Main.Text = msg;
                ss_Main.Refresh();
            }
        }

        private void SayDatabase(string msg)
        {
            if (!String.IsNullOrEmpty(msg) && (tsl_Database.Text != msg))
            {
                tsl_Database.Text = msg;
                ss_Main.Refresh();
            }
        }

        private void SayLocations(string msg)
        {
            if (!String.IsNullOrEmpty(msg) && (tsl_Locations.Text != msg))
            {
                tsl_Locations.Text = msg;
                ss_Main.Refresh();
            }
        }

        private void SayRadar(string msg)
        {
            if (!String.IsNullOrEmpty(msg) && (tsl_Radar.Text != msg))
            {
                tsl_Radar.Text = msg;
                ss_Main.Refresh();
            }
        }


        private void InitializeSettings()
        {
            if (Properties.Settings.Default.Band_Settings == null)
            {
                try
                {
                    Properties.Settings.Default.Band_Settings = new BandSettings(true);
                    Properties.Settings.Default.Save();
                }
                catch
                {
                    // do nothing
                }
            }

            if (String.IsNullOrEmpty(Properties.Settings.Default.CurrentRadar))
            {
                try
                {
                    Properties.Settings.Default.CurrentRadar = new RadarEU_CZ().Name;
                    Properties.Settings.Default.Save();
                }
                catch
                {
                    // do nothing
                }
            }
        }

        private void InitializeRadars()
        {
            GenericRadar radar;
            radar = new RadarNone();
            Radars.Add(radar.Name, radar);
            radar = new Radar3D_DE();
            Radars.Add(radar.Name, radar);
            radar = new RadarEU_CZ();
            Radars.Add(radar.Name, radar);
            radar = new RadarHD_EU();
            Radars.Add(radar.Name, radar);
        }

        private void InitializeLocations()
        {
            // clear or create new dictionary
            if (Locations != null)
                Locations.Clear();
            else
                Locations = new Dictionary<BAND, Dictionary<string, LocationEntry>>();

            // create one dictionary per band
            foreach (BAND band in Bands.GetValues())
            {
                Locations[band] = new Dictionary<string, LocationEntry>();
            }

            // get locations from database
            bw_Locations.RunWorkerAsync();

        }

        public void InitializeSession()
        {
            // register this AirScout instance
            try
            {
                WebClient client;
                string result;
                // check AirScout instance id and generate new if not set
                if (String.IsNullOrEmpty(Properties.Settings.Default.RainScout_Instance_ID))
                {
                    Properties.Settings.Default.RainScout_Instance_ID = Guid.NewGuid().ToString();
                }
                // create an unique device id
                DeviceIdBuilder devid = new DeviceIdBuilder();
                // store in settings if not set so far
                if (String.IsNullOrEmpty(Properties.Settings.Default.RainScout_Device_ID) || (devid.ToString() != Properties.Settings.Default.RainScout_Device_ID))
                {
                    // not set or not the same, assuming AirScout is running on a new machine --> create a new instance id
                    Properties.Settings.Default.RainScout_Instance_ID = Guid.NewGuid().ToString();
                }
                // store device id in settings
                Properties.Settings.Default.RainScout_Device_ID = devid.ToString();
                // get new session key
                client = new WebClient();
                string id = Properties.Settings.Default.RainScout_Instance_ID;
                result = client.DownloadString(Properties.Settings.Default.AirScout_Register_URL +
                    "?id=" + Properties.Settings.Default.RainScout_Instance_ID +
                    "&mycall=" + Properties.Settings.Default.MyCall +
                    "&mylat=" + Properties.Settings.Default.MyLat.ToString() +
                    "&mylon=" + Properties.Settings.Default.MyLon.ToString() +
                    "&myversion=" + Application.ProductVersion);
                if (!result.ToLower().Contains("error"))
                {
                    SessionKey = Encryption.OpenSSLDecrypt(result, Properties.Settings.Default.RainScout_Instance_ID);
                }
                else
                {
                    MessageBox.Show(result + "\n\nSome functionality might be limited.", "Error while registering RainScout");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nSome functionality might be limited.", "Error while registering AirScout");
            }
        }

        private void UpdateStatus()
        {
            tb_UTC.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void StopAllBackgroundWorkers()
        {
            Say("Stopping background workers...");
            while (bw_Stations.IsBusy)
            {
                bw_Stations.CancelAsync();
                Application.DoEvents();
            }
            while (bw_Horizons.IsBusy)
            {
                bw_Horizons.CancelAsync();
                Application.DoEvents();
            }
            while (bw_Radar.IsBusy)
            {
                bw_Radar.CancelAsync();
                Application.DoEvents();
            }
            Say("Stopping background workers finished.");
        }

        private void StartAllBackgroundWorkers()
        {
            if (Properties.Settings.Default.Background_Update_OnStartup)
            {
                if ((bw_StationDatabaseUpdater != null) && !bw_StationDatabaseUpdater.IsBusy)
                {
                    StationDatabaseUpdaterStartOptions startoptions = new StationDatabaseUpdaterStartOptions();
                    startoptions.Name = "Stations";
                    startoptions.RestrictToAreaOfInterest = Properties.Settings.Default.Location_RestrictToAreaOfInterest;
                    startoptions.MinLat = (double)Properties.Settings.Default.MinLat;
                    startoptions.MinLon = (double)Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = (double)Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = (double)Properties.Settings.Default.MaxLon;
                    startoptions.InstanceID = Properties.Settings.Default.RainScout_Instance_ID;
                    startoptions.SessionKey = SessionKey;
                    startoptions.GetKeyURL = Properties.Settings.Default.AirScout_GetKey_URL;
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    bw_StationDatabaseUpdater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (bw_GLOBEUpdater != null) && !bw_GLOBEUpdater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "GLOBE";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.GLOBE;
                    startoptions.MinLat = (double)Properties.Settings.Default.MinLat;
                    startoptions.MinLon = (double)Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = (double)Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = (double)Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_GLOBE_EnableCache;
                    bw_GLOBEUpdater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (bw_SRTM3Updater != null) && !bw_SRTM3Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "SRTM3";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.SRTM3;
                    startoptions.MinLat = (double)Properties.Settings.Default.MinLat;
                    startoptions.MinLon = (double)Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = (double)Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = (double)Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_SRTM3_EnableCache;
                    bw_SRTM3Updater.RunWorkerAsync(startoptions);
                }
                if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (bw_SRTM1Updater != null) && !bw_SRTM1Updater.IsBusy)
                {
                    ElevationDatabaseUpdaterStartOptions startoptions = new ElevationDatabaseUpdaterStartOptions();
                    startoptions.Name = "SRTM1";
                    startoptions.Options = BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE;
                    startoptions.Model = ELEVATIONMODEL.SRTM1;
                    startoptions.MinLat = (double)Properties.Settings.Default.MinLat;
                    startoptions.MinLon = (double)Properties.Settings.Default.MinLon;
                    startoptions.MaxLat = (double)Properties.Settings.Default.MaxLat;
                    startoptions.MaxLon = (double)Properties.Settings.Default.MaxLon;
                    startoptions.FileCacheEnabled = Properties.Settings.Default.Elevation_SRTM1_EnableCache;
                    bw_SRTM1Updater.RunWorkerAsync(startoptions);
                }
            }
                // bw_Stations is not started here
                Say("Starting background workers...");
            if (!bw_Horizons.IsBusy)
                bw_Horizons.RunWorkerAsync();
            if (!bw_Radar.IsBusy)
                bw_Radar.RunWorkerAsync();
            Say("Starting background workers finished.");
        }

        private void BandChanged()
        {
            StopAllBackgroundWorkers();
            StartAllBackgroundWorkers();
            Say("Ready.");
        }

        private void ElevationModelChanged()
        {
            StopAllBackgroundWorkers();
            StartAllBackgroundWorkers();
            Say("Ready.");
        }

        private void RadarChanged()
        {
            StopAllBackgroundWorkers();

            // clear layers
            gm_intensity.Clear();
            gm_cloudtops.Clear();
            gm_lightning.Clear();
            RadarMap = null;

            StartAllBackgroundWorkers();

            // zoom map to fit rect
            gm_Main.SetZoomToFitRect(RectLatLng.FromLTRB(
                (double)Properties.Settings.Default.MinLon,
                (double)Properties.Settings.Default.MaxLat,
                (double)Properties.Settings.Default.MaxLon,
                (double)Properties.Settings.Default.MinLat));

            // update my location
            UpdateMyLocation();

            Say("Ready.");
        }

        private new void Refresh()
        {

        }

        private void UpdateRadarImage()
        {
            if (RadarMap == null)
                return;

            // adjust radar image size & position
            var tl = gm_Main.FromLatLngToLocal(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            var br = gm_Main.FromLatLngToLocal(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));

            // store sizes for other calcluations
            RadarSize = new System.Drawing.Size((int)(br.X - tl.X), (int)(br.Y - tl.Y));
            RadarMapSize = RadarMap.Size;

            GMapImage radar;
            // add radar image to map layer
            gm_intensity.Markers.Clear();
            if (RadarMap.Intensity != null)
            {
                radar = new GMapImage(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
                radar.Image = RadarMap.Intensity;
                radar.Position = new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon);
                radar.Size = RadarSize;
                gm_intensity.Markers.Add(radar);
            }
            gm_intensity.IsVisibile = Properties.Settings.Default.Map_Intensity;

            gm_cloudtops.Markers.Clear();
            if (RadarMap.CloudTops != null)
            {
                radar = new GMapImage(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
                radar.Image = RadarMap.CloudTops;
                radar.Position = new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon);
                radar.Size = RadarSize;
                gm_cloudtops.Markers.Add(radar);
            }
            gm_cloudtops.IsVisibile = Properties.Settings.Default.Map_CloudTops;

            gm_lightning.Markers.Clear();
            if (RadarMap.Lightning != null)
            {

                radar = new GMapImage(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
                radar.Image = RadarMap.Lightning;
                radar.Position = new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon);
                radar.Size = RadarSize;
                gm_lightning.Markers.Add(radar);
            }
            gm_lightning.IsVisibile = Properties.Settings.Default.Map_Lightning;

            gm_bounds.Clear();
            // add tile to map polygons
            List<PointLatLng> l = new List<PointLatLng>();
            l.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            l.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            l.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            l.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon p = new GMapPolygon(l, "Coverage");
            p.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            p.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            gm_bounds.Polygons.Add(p);
            gm_bounds.IsVisibile = Properties.Settings.Default.Map_Bounds;

            lbl_Radar_Timestamp.Text = RadarMap.Timestamp.ToString("yyyy-MM-dd HH:mm");

        }

        private void UpdateMyLocation()
        {
            // check coordinates first
            if (!GeographicalPoint.CheckLatitude(Properties.Settings.Default.MyLat))
                return;
            if (!GeographicalPoint.CheckLongitude(Properties.Settings.Default.MyLon))
                return;

            // set window title
            this.Text = "RainScout V" + Application.ProductVersion + " - " + Properties.Settings.Default.MyCall + " / " + Properties.Settings.Default.MyLoc;


            // create an LocationEntry in any case
            LocationEntry le = new LocationEntry();
            le.Location = StationData.Database.LocationFind(Properties.Settings.Default.MyCall, Properties.Settings.Default.MyLoc);
            if (le.Location == null)
            {
                le.Location = new LocationDesignator();
                le.Location.Call = Properties.Settings.Default.MyCall;
                le.Location.Lat = Properties.Settings.Default.MyLat;
                le.Location.Lon = Properties.Settings.Default.MyLon;
                le.Location.Source = GEOSOURCE.UNKONWN;
            }
            le.QRV = StationData.Database.QRVFind(Properties.Settings.Default.MyCall, Properties.Settings.Default.MyLoc, Properties.Settings.Default.CurrentBand);
            if (le.QRV == null)
            {
                le.QRV = new QRVDesignator();
                le.QRV.Band = Properties.Settings.Default.CurrentBand;
                le.QRV.Call = Properties.Settings.Default.MyCall;
                le.QRV.Loc = Properties.Settings.Default.MyLoc;
            }
            Locations[Properties.Settings.Default.CurrentBand][Properties.Settings.Default.MyCall] = le;

            double myelv = ScoutBase.Elevation.ElevationData.Database[Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.CurrentElevationModel];
            double myheight = le.QRV.AntennaHeight;
            if (myheight == 0)
            {
                myheight = 10;
            }

            string lat = Properties.Settings.Default.MyLat.ToString("F8");
            string lon = Properties.Settings.Default.MyLon.ToString("F8");
            string loc = Properties.Settings.Default.MyLoc;
            string elv = myelv.ToString("F0");
            string height = le.QRV.AntennaHeight.ToString("F0");
            tb_MyData_Lat.Text = lat;
            tb_MyDataLon.Text = lon;
            tb_MyData_Loc.Text = loc;
            tb_MyData_Elevation.Text = elv;
            tb_MyData_Height.Text = height;

            // set marker on map
            gm_mystations.Markers.Clear();
            SCPPOTENTIAL potential = GetScpPotential(le);
            GMarkerGoogleType type = GetDotMarkerFromScpPotential(potential);
            GMarkerGoogle m = new GMarkerGoogle(new PointLatLng(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon), type);
            m.Tag = Properties.Settings.Default.MyCall;
            m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            m.ToolTipText = Properties.Settings.Default.MyCall + "\n\nLat: " + lat + "\nLon: " + lon + "\nLoc: " + loc;
            gm_mystations.Markers.Add(m);

            // calculate horizon and distance circles
            gm_distances.Clear();
            gm_horizons.Clear();

            try
            {
                SphericalEarth.LatLon.GPoint g;

                // add distance rings
                GMapRoute horz100km = new GMapRoute("myhorizon100km");
                GMapRoute horz200km = new GMapRoute("myhorizon200km");
                GMapRoute horz300km = new GMapRoute("myhorizon300km");
                GMapRoute horz400km = new GMapRoute("myhorizon400km");
                GMapRoute horz500km = new GMapRoute("myhorizon500km");
                GMapRoute horz600km = new GMapRoute("myhorizon600km");

                for (int i = 0; i < 360; i++)
                {
                    // show distance rings
                    g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, 100);
                    horz100km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, 200);
                    horz200km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, 300);
                    horz300km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, 400);
                    horz400km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, 500);
                    horz500km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, i, 600);
                    horz600km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                }

                // add the first point again to close the ring
                horz100km.Points.Add(horz100km.Points[0]);
                horz200km.Points.Add(horz200km.Points[0]);
                horz300km.Points.Add(horz300km.Points[0]);
                horz400km.Points.Add(horz400km.Points[0]);
                horz500km.Points.Add(horz500km.Points[0]);
                horz600km.Points.Add(horz600km.Points[0]);

                // draw distances
                horz100km.Stroke = new Pen(Color.Black, 1);
                horz200km.Stroke = new Pen(Color.Black, 1);
                horz300km.Stroke = new Pen(Color.Black, 1);
                horz400km.Stroke = new Pen(Color.Black, 1);
                horz500km.Stroke = new Pen(Color.Black, 1);
                horz600km.Stroke = new Pen(Color.Black, 1);

                gm_distances.Routes.Add(horz100km);
                gm_distances.Routes.Add(horz200km);
                gm_distances.Routes.Add(horz300km);
                gm_distances.Routes.Add(horz400km);
                gm_distances.Routes.Add(horz500km);
                gm_distances.Routes.Add(horz600km);
                gm_distances.IsVisibile = Properties.Settings.Default.Map_Distances;

                // add both horizons
                GMapRoute horzlow = new GMapRoute("myhorizonlow");
                GMapRoute horzhigh = new GMapRoute("myhorizonhigh");

                PropagationHorizonDesignator hor = PropagationData.Database.PropagationHorizonFind(
                    Properties.Settings.Default.MyLat,
                    Properties.Settings.Default.MyLon,
                    myelv + myheight,
                    HorDistance,
                    Bands.ToGHz(Properties.Settings.Default.CurrentBand),
                    SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                    Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                    ScoutBase.Elevation.ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                    Properties.Settings.Default.CurrentElevationModel,
                    null);

                if (hor == null)
                    return;
                if (hor.Valid)
                {
                    // add both horizons
                    for (int i = 0; i < 360; i++)
                    {
                        g = SphericalEarth.LatLon.DestinationPoint(
                                Properties.Settings.Default.MyLat,
                                Properties.Settings.Default.MyLon,
                                i,
                                Propagation.DistanceFromEpsilon(
                                    myelv + myheight,
                                    Properties.Settings.Default.Scatter_MinHeight,
                                    hor.Horizon[i].Epsmin,
                                    SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                            ));
                        horzlow.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(
                                Properties.Settings.Default.MyLat,
                                Properties.Settings.Default.MyLon,
                                i,
                                Propagation.DistanceFromEpsilon(
                                    myelv + myheight,
                                    Properties.Settings.Default.Scatter_MaxHeight,
                                    hor.Horizon[i].Epsmin,
                                    SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                            ));
                        horzhigh.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    }
                    // add first point again to close the ring
                    horzlow.Points.Add(horzlow.Points[0]);
                    horzhigh.Points.Add(horzhigh.Points[0]);
                }

                // draw horizons
                horzlow.Stroke = new Pen(Color.Red, 3);
                gm_horizons.Routes.Add(horzlow);
                horzhigh.Stroke = new Pen(Color.Yellow, 3);
                gm_horizons.Routes.Add(horzhigh);
                gm_horizons.IsVisibile = Properties.Settings.Default.Map_Horizons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ClearScp()
        {
            Properties.Settings.Default.ScpLat = 0;
            Properties.Settings.Default.ScpLon = 0;
            Properties.Settings.Default.ScpCloudTop = 0;

            gm_myroutes.Routes.Clear();
            gm_scatters.Markers.Clear();
            gm_dxstations.Markers.Clear();

            while (bw_Stations.IsBusy)
            {
                bw_Stations.CancelAsync();
                Application.DoEvents();
            }

            tb_Scp_Lat.Text = "";
            tb_Scp_Lon.Text = "";
            tb_Scp_Loc.Text = "";
            tb_Scp_Intensity.Text = "";
            tb_Scp_Intensity.ForeColor = Color.Black;
            tb_Scp_Intensity.BackColor = SystemColors.Control;
            tb_Scp_CloudTop.Text = "";
            tb_Scp_CloudTop.ForeColor = Color.Black;
            tb_Scp_CloudTop.BackColor = SystemColors.Control;
            tb_Scp_Lightning.Text = "";
            tb_Scp_Lightning.ForeColor = Color.Black;
            tb_Scp_Lightning.BackColor = SystemColors.Control;
        }

        private void UpdateScp()
        {

            double myqtf = SphericalEarth.LatLon.Bearing(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            double myqrb = SphericalEarth.LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            Properties.Settings.Default.ScpCloudTop = GetCloudTopValue(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            string lat = Properties.Settings.Default.ScpLat.ToString("F8");
            string lon = Properties.Settings.Default.ScpLon.ToString("F8");
            string loc = Properties.Settings.Default.ScpLoc;
            string mybearing = SphericalEarth.LatLon.Bearing(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon).ToString("F0");
            string mydistance = SphericalEarth.LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon).ToString("F0");
            
            // change MyLocation according to scp potential
            gm_mystations.Markers.Clear();
            LocationEntry le = Locations[Properties.Settings.Default.CurrentBand][Properties.Settings.Default.MyCall];
            SCPPOTENTIAL potential = GetScpPotential(le);
            GMarkerGoogleType type = GetDotMarkerFromScpPotential(potential);
            GMarkerGoogle mm = new GMarkerGoogle(new PointLatLng(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon), type);
            gm_mystations.Markers.Add(mm);

            // add scatter
            gm_scatters.Markers.Clear();
            GMarkerGoogle m = new GMarkerGoogle(new PointLatLng(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon), GMarkerGoogleType.orange_dot);
            m.Tag = "SCP";
            m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            m.ToolTipText = "SCP" + "\n\nLat: " + lat + "\nLon: " + lon + "\nLoc: " + loc;
            gm_scatters.Markers.Add(m);
            tb_Scp_Lat.Text = lat;
            tb_Scp_Lon.Text = lon;
            tb_Scp_Loc.Text = loc;
            tb_MyData_Bearing.Text = mybearing;
            tb_MyData_Distance.Text = mydistance;
            // Update radar values an colors
            int v = GetIntensityValue(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            Color c = GetIntensityColor(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            if (v > 0)
            {
                tb_Scp_Intensity.Text = v.ToString();
                if (c.A == 255)
                {
                    tb_Scp_Intensity.BackColor = c;
                    if (v <= 20)
                        tb_Scp_Intensity.ForeColor = Color.White;
                    else
                        tb_Scp_Intensity.ForeColor = Color.Black;
                }
                else
                    tb_Scp_Intensity.BackColor = SystemColors.Control;
            }
            else
            {
                tb_Scp_Intensity.Text = "";
                tb_Scp_Intensity.BackColor = SystemColors.Control;
            }
            v = GetCloudTopValue(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            c = GetCloudTopColor(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            if (v > 0)
            {
                tb_Scp_CloudTop.Text = v.ToString();
                if (c.A == 255)
                    tb_Scp_CloudTop.BackColor = c;
                else
                    tb_Scp_CloudTop.BackColor = SystemColors.Control;

            }
            else
            {
                tb_Scp_CloudTop.Text = "";
                tb_Scp_CloudTop.BackColor = SystemColors.Control;
            }
            v = GetLightningValue(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            c = GetLightningColor(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            if (v > 0)
            {
                tb_Scp_Lightning.Text = v.ToString();
                if (c.A == 255)
                    tb_Scp_Lightning.BackColor = c;
                else
                    tb_Scp_Lightning.BackColor = SystemColors.Control;
            }
            else
            {
                tb_Scp_Lightning.Text = "";
                tb_Scp_Lightning.BackColor = SystemColors.Control;
            }

            // show routes
            gm_myroutes.Routes.Clear();
            GMapRoute myroute = new GMapRoute("myroute");
            for (int i = 0; i <= myqrb; i++)
            {
                SphericalEarth.LatLon.GPoint g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, myqtf, i);
                myroute.Points.Add(new PointLatLng(g.Lat, g.Lon));
            }
            Pen p = new Pen(Color.Black, 3);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            myroute.Stroke = p;
            gm_myroutes.Routes.Add(myroute);
            GMapRoute myroutestraight = new GMapRoute("myroutestraight");
            for (int i = (int)myqrb; i <= myqrb + Properties.Settings.Default.Scatter_MaxRadius; i++)
            {
                SphericalEarth.LatLon.GPoint g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, myqtf, i);
                myroutestraight.Points.Add(new PointLatLng(g.Lat, g.Lon));
            }


            // start bw_Stations background worker to show all stations in range
            while (bw_Stations.IsBusy)
            {
                bw_Stations.CancelAsync();
                Application.DoEvents();
            }
            gm_dxstations.Markers.Clear();
            bw_Stations.RunWorkerAsync();
        }

        private void UpdateDXLocation()
        {
            /*
            // get call info from database
            LocationDesignator ld = StationData.Database.LocationFind(Properties.Settings.Default.DXCall);
            Properties.Settings.Default.DXLat = ld.Lat;
            Properties.Settings.Default.DXLon = ld.Lon;
            Properties.Settings.Default.DXLoc = ld.Loc;
            double dxqtf = SphericalEarth.LatLon.Bearing(ld.Lat, ld.Lon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            double dxqrb = SphericalEarth.LatLon.Distance(ld.Lat, ld.Lon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
            string lat = ld.Lat.ToString("F8");
            string lon = ld.Lon.ToString("F8");
            string loc = ld.Loc;
            string qtf = dxqtf.ToString("F8");
            string qrb = dxqrb.ToString("F8");
            tb_DXData_Lat.Text = lat;
            tb_DXData_Lon.Text = lon;
            tb_DXData_Loc.Text = loc;
            tb_DXData_Bearing.Text = qtf;
            tb_DXData_Distance.Text = qrb;
            tb_DXData_Elevation.Text = ElevationData[ld.Lat, ld.Lon].ToString("F0");
            // set marker on map
            gm_dxstations.Routes.Clear();
            // calculate distance circles
            GMapRoute horz100km = new GMapRoute("dxhorizon100km");
            GMapRoute horz200km = new GMapRoute("dxhorizon200km");
            GMapRoute horz300km = new GMapRoute("dxhorizon300km");
            GMapRoute horz400km = new GMapRoute("dxhorizon400km");
            GMapRoute horz500km = new GMapRoute("dxhorizon500km");
            GMapRoute horz600km = new GMapRoute("dxhorizon600km");
            // calculate horizon
            // clear horizon first
            try
            {
                GMapRoute horzlow = new GMapRoute("dxhorizonlow");
                GMapRoute horzhigh = new GMapRoute("dxhorizonhigh");
                CallsignEntry entry = Locations.FindEntry(Properties.Settings.Default.DXCall);
                if (entry == null)
                    return;
                SphericalEarth.LatLon.GPoint g;
                if (entry.Horizon.IsValid)
                {
                    // add both horizons
                    for (int i = 0; i < 360; i++)
                    {
                        double eps = entry.Horizon[i] / 180.0 * Math.PI;
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, GetVisibleDist(SphericalEarth.LatLon.Earth.Radius, 10, Properties.Settings.Default.Scatter_MinHeight, eps));
                        horzlow.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, GetVisibleDist(SphericalEarth.LatLon.Earth.Radius, 10, Properties.Settings.Default.Scatter_MaxHeight, eps));
                        horzhigh.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        // show distance rings
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, 100);
                        horz100km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, 200);
                        horz200km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, 300);
                        horz300km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, 400);
                        horz400km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, 500);
                        horz500km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                        g = SphericalEarth.LatLon.DestinationPoint(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, i, 600);
                        horz600km.Points.Add(new PointLatLng(g.Lat, g.Lon));
                    }
                    // add first point again to close the ring
                    horzlow.Points.Add(horzlow.Points[0]);
                    horzhigh.Points.Add(horzhigh.Points[0]);
                }
                horz100km.Stroke = new Pen(Color.Black, 1);
                horz200km.Stroke = new Pen(Color.Black, 1);
                horz300km.Stroke = new Pen(Color.Black, 1);
                horz400km.Stroke = new Pen(Color.Black, 1);
                horz500km.Stroke = new Pen(Color.Black, 1);
                horz600km.Stroke = new Pen(Color.Black, 1);
                gm_dxstations.Routes.Add(horz100km);
                gm_dxstations.Routes.Add(horz200km);
                gm_dxstations.Routes.Add(horz300km);
                gm_dxstations.Routes.Add(horz400km);
                gm_dxstations.Routes.Add(horz500km);
                gm_dxstations.Routes.Add(horz600km);
                horzlow.Stroke = new Pen(Color.Red, 3);
                gm_dxstations.Routes.Add(horzlow);
                horzhigh.Stroke = new Pen(Color.Yellow, 3);
                gm_dxstations.Routes.Add(horzhigh);
            }
            catch
            {
            }
            */
        }

        private Point RadarMapLatLonToLocal(double lat, double lon)
        {
            GPoint g = gm_Main.FromLatLngToLocal(new PointLatLng(lat, lon));
            GPoint ul = gm_Main.FromLatLngToLocal(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            double scalex = (double)RadarMapSize.Width / (double)RadarSize.Width;
            double scaley = (double)RadarMapSize.Height / (double)RadarSize.Height;
            Point p = new Point();
            p.X = (int)((g.X * scalex - ul.X * scalex));
            p.Y = (int)((g.Y * scaley - ul.Y * scaley));

            return p;
        }

        private int GetIntensityValue(double lat, double lon)
        {
            if (RadarMap == null)
                return -1;

            int v = -1;
            try
            {
                Point p = RadarMapLatLonToLocal(lat, lon);
                v = RadarMap.GetIntensityValue(p.X, p.Y);
            }
            catch (Exception ex)
            {

            }

            return v;
        }

        private Color GetIntensityColor(double lat, double lon)
        {
            if (RadarMap == null)
                return Color.Transparent;

            Color c = Color.Transparent;
            try
            {
                Point p = RadarMapLatLonToLocal(lat, lon);
                c = RadarMap.GetIntensityColor(p.X, p.Y);
            }
            catch (Exception ex)
            {

            }

            return c;
        }

        private int GetCloudTopValue(double lat, double lon)
        {
            if (RadarMap == null)
                return -1;

            int v = -1;
            try
            {
                Point p = RadarMapLatLonToLocal(lat, lon);
                v = RadarMap.GetCloudTopValue(p.X, p.Y);
            }
            catch (Exception ex)
            {

            }

            return v;
        }

        private Color GetCloudTopColor(double lat, double lon)
        {
            if (RadarMap == null)
                return Color.Transparent;

            Color c = Color.Transparent;
            try
            {
                Point p = RadarMapLatLonToLocal(lat, lon);
                c = RadarMap.GetCloudTopColor(p.X, p.Y);
            }
            catch (Exception ex)
            {

            }

            return c;
        }

        private int GetLightningValue(double lat, double lon)
        {
            if (RadarMap == null)
                return -1;

            int v = -1;
            try
            {
                Point p = RadarMapLatLonToLocal(lat, lon);
                v = RadarMap.GetLightningValue(p.X, p.Y);
            }
            catch (Exception ex)
            {

            }

            return v;
        }

        private Color GetLightningColor(double lat, double lon)
        {
            if (RadarMap == null)
                return Color.Transparent;

            Color c = Color.Transparent;
            try
            {
                Point p = RadarMapLatLonToLocal(lat, lon);
                c = RadarMap.GetLightningColor(p.X, p.Y);
            }
            catch (Exception ex)
            {

            }

            return c;
        }

        private SCPPOTENTIAL GetScpPotential (LocationEntry le)
        {
            SCPPOTENTIAL potential = SCPPOTENTIAL.NONELOC;

            try
            {
                lock (le)
                {
                    // calculate all values
                    double dxqrb = SphericalEarth.LatLon.Distance(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon, le.Location.Lat, le.Location.Lon);
                    double scpdxqtf = SphericalEarth.LatLon.Bearing(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon, le.Location.Lat, le.Location.Lon);
                    double dxscpqtf = SphericalEarth.LatLon.Bearing(le.Location.Lat, le.Location.Lon, Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon);
                    double dxelv = ElevationData.Database[le.Location.Lat, le.Location.Lon, Properties.Settings.Default.CurrentElevationModel];
                    

                    // set status according to location quality
                    if (le.Location.Source == GEOSOURCE.FROMUSER)
                    {
                        potential = SCPPOTENTIAL.NONEUSER;
                    }

                    // check for valid horizon
                    if ((le.Horizon != null) && le.Horizon.Valid)
                    {
                        // use cloudtop, if set
                        if (Properties.Settings.Default.ScpCloudTop > 0)
                        {
                            double dx = Propagation.DistanceFromEpsilon(
                                dxelv + le.QRV.AntennaHeight,
                                Properties.Settings.Default.ScpCloudTop,
                                le.Horizon.Horizon[(int)dxscpqtf].Epsmin,
                                LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                            );
                            if (dx >= dxqrb)
                            {
                                potential = SCPPOTENTIAL.CLOUDTOP;
                            }
                        }
                        else
                        {
                            double dxmin = Propagation.DistanceFromEpsilon(
                                dxelv + le.QRV.AntennaHeight,
                                Properties.Settings.Default.Scatter_MinHeight,
                                le.Horizon.Horizon[(int)dxscpqtf].Epsmin,
                                LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                            );
                            if (dxmin >= dxqrb)
                            {
                                potential = SCPPOTENTIAL.MINCLOUD;
                            }
                            else
                            {
                                double dxmax = Propagation.DistanceFromEpsilon(
                                    dxelv + le.QRV.AntennaHeight,
                                    Properties.Settings.Default.Scatter_MaxHeight,
                                    le.Horizon.Horizon[(int)dxscpqtf].Epsmin,
                                    LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                                );
                                if (dxmax >= dxqrb)
                                {
                                    potential = SCPPOTENTIAL.MAXCLOUD;
                                }
                            }
                        }

                        return potential;
                    }

                    // no horizon available --> calculate a single propagation path instead to be reactive
                    // use cloudtop, if set
                    if (Properties.Settings.Default.ScpCloudTop > 0)
                    {
                        PropagationPathDesignator path = PropagationData.Database.PropagationPathFindOrCreateFromBearing(
                            bw_Stations,
                            le.Location.Lat,
                            le.Location.Lon,
                            dxelv + le.QRV.AntennaHeight,
                            dxscpqtf,
                            HorDistance,
                            Properties.Settings.Default.ScpCloudTop,
                            Bands.ToGHz(Properties.Settings.Default.CurrentBand),
                            LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                            Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                            ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                            Properties.Settings.Default.CurrentElevationModel,
                            0
                        );
                        if ((path != null) && path.Valid)
                        {
                            double dx = Propagation.DistanceFromEpsilon(
                            dxelv + le.QRV.AntennaHeight,
                            Properties.Settings.Default.ScpCloudTop,
                            le.Horizon.Horizon[(int)dxscpqtf].Epsmin,
                            LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                        );
                            if (dx >= dxqrb)
                            {
                                potential = SCPPOTENTIAL.CLOUDTOP;
                            }
                        }
                    }
                    else
                    {
                        PropagationPathDesignator pathmin = PropagationData.Database.PropagationPathFindOrCreateFromBearing(
                            bw_Stations,
                            le.Location.Lat,
                            le.Location.Lon,
                            dxelv + le.QRV.AntennaHeight,
                            dxscpqtf,
                            HorDistance,
                            Properties.Settings.Default.Scatter_MinHeight,
                            Bands.ToGHz(Properties.Settings.Default.CurrentBand),
                            LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                            Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                            ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                            Properties.Settings.Default.CurrentElevationModel,
                            0
                        );
                        if ((pathmin != null) && pathmin.Valid)
                        {
                            double dxmin = Propagation.DistanceFromEpsilon(
                                dxelv + le.QRV.AntennaHeight,
                                Properties.Settings.Default.Scatter_MinHeight,
                                pathmin.Eps1_Min,
                                LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                            );
                            if (dxmin >= dxqrb)
                            {
                                potential = SCPPOTENTIAL.MINCLOUD;
                            }
                            else
                            {
                                PropagationPathDesignator pathmax = PropagationData.Database.PropagationPathFindOrCreateFromBearing(
                                    bw_Stations,
                                    le.Location.Lat,
                                    le.Location.Lon,
                                    dxelv + le.QRV.AntennaHeight,
                                    dxscpqtf,
                                    HorDistance,
                                    Properties.Settings.Default.Scatter_MinHeight,
                                    Bands.ToGHz(Properties.Settings.Default.CurrentBand),
                                    LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                                    Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                                    ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                                    Properties.Settings.Default.CurrentElevationModel,
                                    0
                                );
                                if ((pathmax != null) && pathmax.Valid)
                                {
                                    double dxmax = Propagation.DistanceFromEpsilon(
                                        dxelv + le.QRV.AntennaHeight,
                                        Properties.Settings.Default.Scatter_MaxHeight,
                                        pathmax.Eps1_Min,
                                        LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor
                                    );
                                    if (dxmax >= dxqrb)
                                    {
                                        potential = SCPPOTENTIAL.MAXCLOUD;
                                    }
                                }
                            }
                        }

                        return potential;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return potential;
        }

        private GMarkerGoogleType GetSmallMarkerFromScpPotential(SCPPOTENTIAL potential)
        {
            GMarkerGoogleType color = GMarkerGoogleType.gray_small;

            switch (potential)
            {
                case SCPPOTENTIAL.NONELOC: color = GMarkerGoogleType.white_small; break;
                case SCPPOTENTIAL.NONEUSER: color = GMarkerGoogleType.green_small; break;
                case SCPPOTENTIAL.MAXCLOUD: color = GMarkerGoogleType.yellow_small; break;
                case SCPPOTENTIAL.MINCLOUD: color = GMarkerGoogleType.red_small; break;
                case SCPPOTENTIAL.CLOUDTOP: color = GMarkerGoogleType.purple_small; break;
            }

            return color;
        }

        private GMarkerGoogleType GetDotMarkerFromScpPotential(SCPPOTENTIAL potential)
        {
            GMarkerGoogleType color = GMarkerGoogleType.gray_small;

            switch (potential)
            {
                case SCPPOTENTIAL.NONELOC: color = GMarkerGoogleType.white_dot; break;
                case SCPPOTENTIAL.NONEUSER: color = GMarkerGoogleType.green_dot; break;
                case SCPPOTENTIAL.MAXCLOUD: color = GMarkerGoogleType.yellow_dot; break;
                case SCPPOTENTIAL.MINCLOUD: color = GMarkerGoogleType.red_dot; break;
                case SCPPOTENTIAL.CLOUDTOP: color = GMarkerGoogleType.pink_dot; break;
            }

            return color;
        }

        private void gm_Main_OnMapZoomChanged()
        {
            UpdateRadarImage();
            tb_Map_Zoom.Text = gm_Main.Zoom.ToString();
        }

        private void gm_Main_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
        }

        private void MainDlg_SizeChanged(object sender, EventArgs e)
        {
            // maintain splitter on size changed
            int main = this.Height - gb_MyData.Height - ss_Main.Height - 60;
            if (main > 0) 
                spl_Main.SplitterDistance = main;
            int map = this.Width - pa_CommonInfo.MinimumSize.Width - 20;
            if (map > 0)
                spl_Map.SplitterDistance = map;
        }

        private void gm_Main_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void gm_Main_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            
        }

        private void gm_Main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CurrentMarker != null && CurrentMarker.IsMouseOver)
                IsDraggingMarker = true;
        }

        private void gm_Main_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == System.Windows.Forms.MouseButtons.Left) && !MapDragging)
            {
                // check if Mouse is not over Marker
                if ((CurrentMarker != null) && CurrentMarker.IsMouseOver)
                {
                    Properties.Settings.Default.DXCall = (string)CurrentMarker.Tag;
                    foreach (GMapMarker gm in gm_dxstations.Markers)
                    {
                        gm.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    }
                    CurrentMarker.ToolTipMode = MarkerTooltipMode.Always;
                    UpdateDXLocation();
                }
                else
                {
                    // handle left click on map
                    // set new scatterpoint
                    gm_scatters.Markers.Clear();
                    PointLatLng p = gm_Main.FromLocalToLatLng(e.X, e.Y);
                    Properties.Settings.Default.ScpLat = p.Lat;
                    Properties.Settings.Default.ScpLon = p.Lng;
                    Properties.Settings.Default.ScpLoc = SphericalEarth.LatLon.Loc(p.Lat, p.Lng);
                    UpdateScp();
                }
                if (IsDraggingMarker)
                {
                    IsDraggingMarker = false;
                }
            }
            // stop MapDragging if active
            if (MapDragging)
                MapDragging = false;
        }

        private void gm_Main_MouseMove(object sender, MouseEventArgs e)
        {
            // maintain mouse position data
            PointLatLng p = gm_Main.FromLocalToLatLng(e.X, e.Y);
            double lat = p.Lat;
            double lon = p.Lng;
            string loc = SphericalEarth.LatLon.Loc(p.Lat, p.Lng);
            double bearing = SphericalEarth.LatLon.Bearing(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, p.Lat, p.Lng);
            double distance = SphericalEarth.LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, p.Lat, p.Lng);
            double elevation = ElevationData.Database[p.Lat, p.Lng, Properties.Settings.Default.CurrentElevationModel];
            tb_Mouse_Lat.Text = lat.ToString("F8");
            tb_Mouse_Lon.Text = lon.ToString("F8");
            tb_Mouse_Loc.Text = loc;
            tb_Mouse_Bearing.Text = bearing.ToString("F8");
            tb_Mouse_Distance.Text = distance.ToString("F0");
            if (IsDraggingMarker && CurrentMarker != null)
            {
                /*
                LocationDesignator entry = StationData.Database.LocationFind(CurrentMarker.Tag.ToString());
                if (entry != null)
                {
                    entry.Lat = lat;
                    entry.Lon = lon;
                    entry.Loc = loc;
                    entry.Elevation = 
                    CurrentMarker.Position = p;
                    CurrentMarker.ToolTipText = entry.Call + "\n\nLat: " + entry.Lat.ToString("F8") + "\nLon: " + entry.Lon.ToString("F8") + "\nLoc: " + entry.Loc;
                }
                */
            }

            // Update radar values and colors
            int v = GetIntensityValue(lat, lon);
            Color c = GetIntensityColor(lat, lon);
            if (v > 0)
            {
                tb_Mouse_Intensity.Text = v.ToString();
                if (c.A == 255)
                {
                    tb_Mouse_Intensity.BackColor = c;
                    if (v <= 20)
                        tb_Mouse_Intensity.ForeColor = Color.White;
                    else
                        tb_Mouse_Intensity.ForeColor = Color.Black;
                }
                else
                    tb_Mouse_Intensity.BackColor = SystemColors.Control;
            }
            else
            {
                tb_Mouse_Intensity.Text = "";
                tb_Mouse_Intensity.BackColor = SystemColors.Control;
            }
            v = GetCloudTopValue(lat, lon);
            c = GetCloudTopColor(lat, lon);
            if (v > 0)
            {
                tb_Mouse_CloudTop.Text = v.ToString();
                if (c.A == 255)
                    tb_Mouse_CloudTop.BackColor = c;
                else
                    tb_Mouse_CloudTop.BackColor = SystemColors.Control;

            }
            else
            {
                tb_Mouse_CloudTop.Text = "";
                tb_Mouse_CloudTop.BackColor = SystemColors.Control;
            }
            v = GetLightningValue(lat, lon);
            c = GetLightningColor(lat, lon);
            if (v > 0)
            {
                tb_Mouse_Lightning.Text = v.ToString();
                if (c.A == 255)
                    tb_Mouse_Lightning.BackColor = c;
                else
                    tb_Mouse_Lightning.BackColor = SystemColors.Control;
            }
            else
            {
                tb_Mouse_Lightning.Text = "";
                tb_Mouse_Lightning.BackColor = SystemColors.Control;
            }
        }

        private void gm_Main_OnMapDrag()
        {
            // set the MapDragging flag
            MapDragging = true;
        }

        private void gm_Main_MouseEnter(object sender, EventArgs e)
        {

        }

        private void gm_Main_OnMarkerEnter(GMapMarker item)
        {
            if (!IsDraggingMarker)
                CurrentMarker = item;
        }

        private void gm_Main_OnMarkerLeave(GMapMarker item)
        {
            CurrentMarker = null;
        }

        private void bw_Horizons_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_Horizons.ReportProgress(-2, "Wait for locations complete...");

            while (!bw_Horizons.CancellationPending)
            {
                // wait for location update is complete
                if ((Status & RAINSCOUTSTATUS.LOCATIONSCOMPLETE) == 0)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                try
                {
                    PropagationHorizonDesignator hor = null;
                    LocationEntry le = null;

                    // calculate my horizon first
                    bw_Horizons.ReportProgress(-2, "Calculating horizon of " + Properties.Settings.Default.MyCall);
                    hor = PropagationData.Database.PropagationHorizonFindOrCreate(
                        bw_Horizons,
                        Properties.Settings.Default.MyLat,
                        Properties.Settings.Default.MyLon,
                        ScoutBase.Elevation.ElevationData.Database[Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.CurrentElevationModel] + 
                            Properties.Settings.Default.MyHeight,
                        HorDistance,
                        Bands.ToGHz(Properties.Settings.Default.CurrentBand),
                        SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                        Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                        ScoutBase.Elevation.ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                        Properties.Settings.Default.CurrentElevationModel,
                        null);

                    if (hor != null)
                    {
                        bw_Horizons.ReportProgress(360, le);
                    }

                    // cancel calculation
                    if (bw_Horizons.CancellationPending)
                        break;


                    bw_Horizons.ReportProgress(-2, "Getting stations...");
                    // calculate horizons of all stations in database
                    for (int i = 0; i < Locations[Properties.Settings.Default.CurrentBand].Count; i++)
                    {
                        lock (Locations)
                        {
                            // clone a location entry
                            try
                            {
                                le = Cloning.Clone(Locations[Properties.Settings.Default.CurrentBand].Values.ElementAt(i));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }

                        }

                        // cancel calculation
                        if (bw_Horizons.CancellationPending)
                            break;

                        if (le != null)
                        {
                            hor = PropagationData.Database.PropagationHorizonFind(
                                le.Location.Lat,
                                le.Location.Lon,
                                ScoutBase.Elevation.ElevationData.Database[le.Location.Lat, le.Location.Lon, Properties.Settings.Default.CurrentElevationModel] + le.QRV.AntennaHeight,
                                HorDistance,
                                Bands.ToGHz(le.QRV.Band),
                                SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                                Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                                ScoutBase.Elevation.ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                                Properties.Settings.Default.CurrentElevationModel,
                                null);

                            // cancel calculation
                            if (bw_Horizons.CancellationPending)
                                break;

                            if (hor == null)
                            {
                                bw_Horizons.ReportProgress(-2, "Calculating horizon of " + le.Location.Call);
                                hor = PropagationData.Database.PropagationHorizonFindOrCreate(
                                    bw_Horizons,
                                    le.Location.Lat,
                                    le.Location.Lon,
                                    ScoutBase.Elevation.ElevationData.Database[le.Location.Lat, le.Location.Lon, Properties.Settings.Default.CurrentElevationModel] + le.QRV.AntennaHeight,
                                    HorDistance,
                                    Bands.ToGHz(le.QRV.Band),
                                    SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].K_Factor,
                                    Properties.Settings.Default.Band_Settings[Properties.Settings.Default.CurrentBand].F1_Clearance,
                                    ScoutBase.Elevation.ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel),
                                    Properties.Settings.Default.CurrentElevationModel,
                                    null);

                                if (!bw_Horizons.CancellationPending && (hor != null))
                                {
                                    bw_Horizons.ReportProgress(360, le);
                                }
                            }
                        }

                        // cancel calculation
                        if (bw_Horizons.CancellationPending)
                            break;

                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void bw_Horizons_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // use -2 for status messages as -1..359 are occupied with calculation messages
            // show status message
            if (e.ProgressPercentage <= -2)
            {
                SayDatabase((string)e.UserState);
            }
            if (e.ProgressPercentage == -1)
            {
//                SayDatabase((string)e.UserState);
            }
            else if ((e.ProgressPercentage >= 0) && (e.ProgressPercentage <= 359))
            {
                HorizonPoint hp = (HorizonPoint)e.UserState;
                // do nothing with the horizon point so far
            }
            else if (e.ProgressPercentage == 360)
            {
                // horizon of one station finished
                LocationEntry le = (LocationEntry)e.UserState;
            }

        }

        private void bw_Horizons_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void btn_Options_Click(object sender, EventArgs e)
        {
            OptionsDlg Dlg = new OptionsDlg(this);
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Reload();
            }
        }

        private void btn_Map_ZoomOut_Click(object sender, EventArgs e)
        {
            if (gm_Main.Zoom > gm_Main.MinZoom)
                gm_Main.Zoom--;
        }

        private void btn_Map_ZoomIn_Click(object sender, EventArgs e)
        {
            if (gm_Main.Zoom < gm_Main.MaxZoom)
                gm_Main.Zoom++;
        }

        private void ti_Main_Tick(object sender, EventArgs e)
        {
            ti_Main.Stop();
            UpdateStatus();
            ti_Main.Start();
        }

        #region bw_Locations

        private void bw_Locations_DoWork(object sender, DoWorkEventArgs e)
        {
            // get all available locations in the area of interest
            bw_Locations.ReportProgress(0, "Initializing location database...");

            ElevationData.Database.SetDBStatusBit(ELEVATIONMODEL.SRTM3, System.Data.SQLite.DATABASESTATUS.COMPLETE);

            // abort on empty Locations directory
            if (Locations == null)
                return;

            int count = 0;
            List<LocationDesignator> lds = StationData.Database.LocationGetAll(
                (double)Properties.Settings.Default.MinLat, 
                (double)Properties.Settings.Default.MinLon,
                (double)Properties.Settings.Default.MaxLat,
                (double)Properties.Settings.Default.MaxLon);
            foreach (LocationDesignator ld in lds)
            {
                if (bw_Locations.CancellationPending)
                    return;

                List<QRVDesignator> qrvs = StationData.Database.QRVFind(ld.Call, ld.Loc);
                if (qrvs == null)
                    continue;
                foreach (QRVDesignator qrv in qrvs)
                {
                    if (bw_Locations.CancellationPending)
                        return;

                    if (qrv.Band >= BAND.B2_3G)
                    {
                        LocationEntry le = new LocationEntry();
                        le.Location = ld;
                        le.QRV = qrv;
                        if ((le.Location.Call == "OK2KYK") && (qrv.Band == BAND.B10G))
                        {
                            int k = 0;
                        }
                        double dxelv = ScoutBase.Elevation.ElevationData.Database[le.Location.Lat, le.Location.Lon, Properties.Settings.Default.CurrentElevationModel] + qrv.AntennaHeight;
                        double dxfreq = Bands.ToGHz(qrv.Band);
                        double re = SphericalEarth.LatLon.Earth.Radius * Properties.Settings.Default.Band_Settings[qrv.Band].K_Factor;
                        double dxf1clr = Properties.Settings.Default.Band_Settings[qrv.Band].F1_Clearance;
                        double dxstpw = ScoutBase.Elevation.ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.CurrentElevationModel);
                        ELEVATIONMODEL dxmodel = Properties.Settings.Default.CurrentElevationModel;
                        // add horizon if already in database
                        PropagationHorizonDesignator hor = PropagationData.Database.PropagationHorizonFind(
                            le.Location.Lat,
                            le.Location.Lon,
                            dxelv,
                            HorDistance,
                            dxfreq,
                            re,
                            dxf1clr,
                            dxstpw,
                            dxmodel,
                            null);
                        if (hor != null)
                            le.Horizon = hor;

                        // report location
                        bw_Locations.ReportProgress(1, le);

                        count++;

                        Thread.Sleep(10);

                    }
                }
            }
            bw_Locations.ReportProgress(-1, "Initializing location database finished: " + count.ToString() + " locations found.");
            bw_Locations.ReportProgress(0, "Finished.");
            bw_Locations.ReportProgress(100);

        }

        private void bw_Locations_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                Say((string)e.UserState);
            }
            else if (e.ProgressPercentage == 0)
            {
                SayLocations((string)e.UserState);
            }
            else if (e.ProgressPercentage == 1)
            {
                // add location
                LocationEntry le = (LocationEntry)e.UserState;
                lock (Locations)
                {
                    Locations[le.QRV.Band][le.Location.Call] = le;
                }
            }
            else if (e.ProgressPercentage == 100)
            {
                // locations complete
                Status = Status | RAINSCOUTSTATUS.LOCATIONSCOMPLETE;
            }
        }

        private void bw_Locations_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        #region Radar

        private void bw_Radar_DoWork(object sender, DoWorkEventArgs e)
        {
            int mapsize = 2048;
            int update = 60;

            while (!bw_Radar.CancellationPending)
            {
                bw_Radar.ReportProgress(0, "Getting radar images...");
                // create a new radar map
                RadarMap radarmap = new RadarMap(gm_Main, 
                    (double)Properties.Settings.Default.MinLon,
                    (double)Properties.Settings.Default.MaxLat,
                    (double)Properties.Settings.Default.MaxLon,
                    (double)Properties.Settings.Default.MinLat, 
                    mapsize, mapsize);

                // get new radar images
                GenericRadar radar = null;
                try
                {
                    radar = Radars[Properties.Settings.Default.CurrentRadar];
                }
                catch (Exception ex)
                {
                    // radar not found --> do nothing
                }

                // return on empty radar            
                if (radar == null)
                    return;

                if (radar.HasRadarLayer(RADARLAYER.INTENSITY))
                {
                    int[,] intensity = radar.GetRadarLayer(RADARLAYER.INTENSITY);
                    radarmap.ImportIntensity(intensity, radar.Left, radar.Top, radar.Right, radar.Bottom);
                }
                if (radar.HasRadarLayer(RADARLAYER.CLOUDTOPS))
                {
                    int[,] cloudtops = radar.GetRadarLayer(RADARLAYER.CLOUDTOPS);
                    radarmap.ImportCloudTops(cloudtops, radar.Left, radar.Top, radar.Right, radar.Bottom);
                }
                if (radar.HasRadarLayer(RADARLAYER.LIGHTNING))
                {
                    int[,] lightning = radar.GetRadarLayer(RADARLAYER.LIGHTNING);
                    radarmap.ImportLightning(lightning, radar.Left, radar.Top, radar.Right, radar.Bottom);
                }

                radarmap.Timestamp = radar.Timestamp;

                bw_Radar.ReportProgress(1, radarmap);

                bw_Radar.ReportProgress(0, "Waiting...");
                int i = 0;
                while (!bw_Radar.CancellationPending && (i < update))
                {
                    Thread.Sleep(1000);
                    i++;
                }
            }
        }

        private void bw_Radar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                SayRadar((string)e.UserState);
            }
            else if (e.ProgressPercentage == 0)
            {
                SayRadar((string)e.UserState);
            }
            else if (e.ProgressPercentage == 1)
            {
                // update radar
                RadarMap = (RadarMap)e.UserState;
                UpdateRadarImage();
            }
        }

        private void bw_Radar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        private void bw_Stations_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_Stations.ReportProgress(-1, "Calculating stations' visibility of scatter point...");
            foreach (LocationEntry le in Locations[Properties.Settings.Default.CurrentBand].Values)
            {
                // stop, if cancellation pending
                if (bw_Stations.CancellationPending)
                    break;
                
                // sleep to keep cpu load low
                Thread.Sleep(10);

                try
                {
                    // calculate distance to scp
                    double dxqrb = SphericalEarth.LatLon.Distance(Properties.Settings.Default.ScpLat, Properties.Settings.Default.ScpLon, le.Location.Lat, le.Location.Lon);

                    // continue, if distance to scp is outside bounds (regardless of path)
                    if (dxqrb < (double)Properties.Settings.Default.Filter_MinDistance)
                        continue;
                    if (dxqrb > (double)Properties.Settings.Default.Filter_MaxDistance)
                        continue;

                    // get scp potential
                    SCPPOTENTIAL potential = GetScpPotential(le);

                    SCPPOTENTIAL minpotential = SCPPOTENTIAL.NONELOC;
                    if (Properties.Settings.Default.Filter_VisibleScpOnly)
                    {
                        if ((RadarMap != null) && RadarMap.HasCloudTops && (Properties.Settings.Default.ScpCloudTop > 0))
                        {
                            minpotential = SCPPOTENTIAL.CLOUDTOP;
                        }
                        else
                        {
                            minpotential = SCPPOTENTIAL.MAXCLOUD;
                        }
                    }
                    if (potential >= minpotential)
                    {
                        // send update to main thread
                        bw_Stations.ReportProgress((int)potential, le);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            bw_Stations.ReportProgress(-1, "Ready.");
        }

        private void bw_Stations_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -1)
            {
                Say((string)e.UserState);
            }
            else if (e.ProgressPercentage >= 0)
            {
                SCPPOTENTIAL potential = (SCPPOTENTIAL)e.ProgressPercentage;
                LocationEntry le = (LocationEntry)e.UserState;

                // create Marker
                try
                {
                    // create marker if enabled
                    GMarkerGoogleType type = GetSmallMarkerFromScpPotential(potential);
                    GMarkerGoogle dxm = new GMarkerGoogle(new PointLatLng(le.Location.Lat, le.Location.Lon), type);
                    dxm.Tag = le.Location.Call;
                    dxm.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    dxm.ToolTipText = le.Location.Call + "\n\nLat: " + le.Location.Lat.ToString("F8") + "\nLon: " + le.Location.Lon.ToString("F8") + "\nLoc: " + le.Location.Loc;
                    gm_dxstations.Markers.Add(dxm);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(le.Location.Call + ":" + ex.Message);
                }
            }

        }

        private void bw_Stations_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void cb_Band_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)
                return;
            try
            {
                Properties.Settings.Default.CurrentBand = (BAND)cb_Band.SelectedValue;
                Properties.Settings.Default.Save();
                BandChanged();
            }
            catch
            {
                // do nothing
            }
        }

        private void cb_Radar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)
                return;
            try
            {
                string radar = (string)cb_Radar.SelectedItem;
                Properties.Settings.Default.CurrentRadar = radar;
                Properties.Settings.Default.Save();
                RadarChanged();
            }
            catch
            {
                // do nothing
            }
        }

        private void btn_scp_Clear_Click(object sender, EventArgs e)
        {
            ClearScp();
        }

        private void cb_ElevationModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)
                return;
            try
            {
                Properties.Settings.Default.CurrentElevationModel = (ELEVATIONMODEL)cb_ElevationModel.SelectedValue;
                Properties.Settings.Default.Save();
                ElevationModelChanged();
            }
            catch
            {
                // do nothing
            }
        }

        private void cb_Map_Bounds_CheckedChanged(object sender, EventArgs e)
        {
            gm_bounds.IsVisibile = cb_Map_Bounds.Checked;
        }

        private void cb_Map_Distances_CheckedChanged(object sender, EventArgs e)
        {
            gm_distances.IsVisibile = cb_Map_Distances.Checked;
        }

        private void cb_Map_Horizons_CheckedChanged(object sender, EventArgs e)
        {
            gm_horizons.IsVisibile = cb_Map_Horizons.Checked;
        }

        private void cb_Map_Intensity_CheckedChanged(object sender, EventArgs e)
        {
            gm_intensity.IsVisibile = cb_Map_Intensity.Checked;
        }

        private void cb_Map_CloudTops_CheckedChanged(object sender, EventArgs e)
        {
            gm_cloudtops.IsVisibile = cb_Map_CloudTops.Checked;
        }

        private void cb_Map_Lightning_CheckedChanged(object sender, EventArgs e)
        {
            gm_lightning.IsVisibile = cb_Map_Lightning.Checked;
        }


        #region StationDatabaseUpdater

        private void bw_StationDatabaseUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage < 0)
                {
                    // error message received
                    Say((string)e.UserState);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    SayDatabase(msg);
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
               Say(ex.ToString());
            }
        }

        private void bw_StationDatabaseUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
                    Say((string)e.UserState);
                }
                else if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    SayDatabase(msg);
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
                }
                if (!this.Disposing && (ss_Main != null))
                    ss_Main.Update();
            }
            catch (Exception ex)
            {
                Say(ex.ToString());
            }
        }


        #endregion
    }

}
