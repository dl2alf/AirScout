using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Net;
using Microsoft.Win32;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Stations;
using ScoutBase.Elevation;
using AirScout.PlaneFeeds.Plugin.MEFContract;

namespace AirScout
{
    public partial class FirstRunWizard : Form
    {
        GMapOverlay Coveragepolygons = new GMapOverlay("Coveragepolygons");
        GMapOverlay GLOBEpolygons = new GMapOverlay("GLOBEpolygons");
        GMapOverlay SRTM3polygons = new GMapOverlay("SRTM3polygons");
        GMapOverlay SRTM1polygons = new GMapOverlay("SRTM1polygons");
        GMapOverlay ASTER3polygons = new GMapOverlay("ASTER3polygons");
        GMapOverlay ASTER1polygons = new GMapOverlay("ASTER1polygons");
        GMapOverlay Callsignpolygons = new GMapOverlay("Callsignpolygons");
        GMapOverlay Callsignspositions = new GMapOverlay("Callsignpositions");

        GMarkerGoogle UserPos = new GMarkerGoogle(new PointLatLng(0.0, 0.0), GMarkerGoogleType.red_dot);

        private bool IsDraggingMarker = false;

        private LogWriter Log = LogWriter.Instance;

        MapDlg ParentDlg;

        public FirstRunWizard(MapDlg parentdlg)
        {
            ParentDlg = parentdlg;
            InitializeComponent();
        }

        private void FirstRunWizard_Load(object sender, EventArgs e)
        {
            this.Text = "Welcome to AirScout - Aircraft Scatter Prediction V" + Application.ProductVersion + " (c) 2013-2020 DL2ALF";
            Log.WriteMessage("Loading.");
            // set initial settings for CoverageMap
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_Coverage.MapProvider.RefererUrl = "";
            gm_Coverage.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Coverage.IgnoreMarkerOnMouseWheel = true;
            gm_Coverage.MinZoom = 0;
            gm_Coverage.MaxZoom = 20;
            gm_Coverage.Zoom = 6;
            gm_Coverage.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Coverage.CanDragMap = true;
            gm_Coverage.ScalePen = new Pen(Color.Black, 3);
            gm_Coverage.HelperLinePen = null;
            gm_Coverage.SelectionPen = null;
            gm_Coverage.MapScaleInfoEnabled = true;
            gm_Coverage.Overlays.Add(Coveragepolygons);

            // set initial settings for GLOBEMap
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_GLOBE.MapProvider.RefererUrl = "";
            gm_GLOBE.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_GLOBE.IgnoreMarkerOnMouseWheel = true;
            gm_GLOBE.MinZoom = 0;
            gm_GLOBE.MaxZoom = 20;
            gm_GLOBE.Zoom = 1;
            gm_GLOBE.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_GLOBE.CanDragMap = true;
            gm_GLOBE.ScalePen = new Pen(Color.Black, 3);
            gm_GLOBE.HelperLinePen = null;
            gm_GLOBE.SelectionPen = null;
            gm_GLOBE.MapScaleInfoEnabled = true;
            gm_GLOBE.Overlays.Add(GLOBEpolygons);

            // set initial settings for SRTM3Map
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_SRTM3.MapProvider.RefererUrl = "";
            gm_SRTM3.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_SRTM3.IgnoreMarkerOnMouseWheel = true;
            gm_SRTM3.MinZoom = 0;
            gm_SRTM3.MaxZoom = 20;
            gm_SRTM3.Zoom = 1;
            gm_SRTM3.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_SRTM3.CanDragMap = true;
            gm_SRTM3.ScalePen = new Pen(Color.Black, 3);
            gm_SRTM3.HelperLinePen = null;
            gm_SRTM3.SelectionPen = null;
            gm_SRTM3.MapScaleInfoEnabled = true;
            gm_SRTM3.Overlays.Add(SRTM3polygons);

            // set initial settings for SRTM1Map
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_SRTM1.MapProvider.RefererUrl = "";
            gm_SRTM1.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_SRTM1.IgnoreMarkerOnMouseWheel = true;
            gm_SRTM1.MinZoom = 0;
            gm_SRTM1.MaxZoom = 20;
            gm_SRTM1.Zoom = 1;
            gm_SRTM1.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_SRTM1.CanDragMap = true;
            gm_SRTM1.ScalePen = new Pen(Color.Black, 3);
            gm_SRTM1.HelperLinePen = null;
            gm_SRTM1.SelectionPen = null;
            gm_SRTM1.MapScaleInfoEnabled = true;
            gm_SRTM1.Overlays.Add(SRTM1polygons);

            // set initial settings for ASTER3Map
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_ASTER3.MapProvider.RefererUrl = "";
            gm_ASTER3.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_ASTER3.IgnoreMarkerOnMouseWheel = true;
            gm_ASTER3.MinZoom = 0;
            gm_ASTER3.MaxZoom = 20;
            gm_ASTER3.Zoom = 1;
            gm_ASTER3.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_ASTER3.CanDragMap = true;
            gm_ASTER3.ScalePen = new Pen(Color.Black, 3);
            gm_ASTER3.HelperLinePen = null;
            gm_ASTER3.SelectionPen = null;
            gm_ASTER3.MapScaleInfoEnabled = true;
            gm_ASTER3.Overlays.Add(ASTER3polygons);

            // set initial settings for ASTER1Map
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_ASTER1.MapProvider.RefererUrl = "";
            gm_ASTER1.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_ASTER1.IgnoreMarkerOnMouseWheel = true;
            gm_ASTER1.MinZoom = 0;
            gm_ASTER1.MaxZoom = 20;
            gm_ASTER1.Zoom = 1;
            gm_ASTER1.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_ASTER1.CanDragMap = true;
            gm_ASTER1.ScalePen = new Pen(Color.Black, 3);
            gm_ASTER1.HelperLinePen = null;
            gm_ASTER1.SelectionPen = null;
            gm_ASTER1.MapScaleInfoEnabled = true;
            gm_ASTER1.Overlays.Add(ASTER1polygons);

            // set initial settings for user details
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // clearing referrer URL issue 2019-12-14
            gm_Callsign.MapProvider.RefererUrl = "";
            gm_Callsign.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Callsign.IgnoreMarkerOnMouseWheel = true;
            gm_Callsign.MinZoom = 0;
            gm_Callsign.MaxZoom = 20;
            gm_Callsign.Zoom = 1;
            gm_Callsign.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Callsign.CanDragMap = true;
            gm_Callsign.ScalePen = new Pen(Color.Black, 3);
            gm_Callsign.HelperLinePen = null;
            gm_Callsign.SelectionPen = null;
            gm_Callsign.MapScaleInfoEnabled = true;
            gm_Callsign.Overlays.Add(Callsignpolygons);
            gm_Callsign.Overlays.Add(Callsignspositions);
            Callsignspositions.Markers.Add(UserPos);
            Say("");
            Log.WriteMessage("Finished.");
        }

        private void Say(string text)
        {
            if (tsl_Status.Text != text)
            {
                tsl_Status.Text = text;
                ss_Main.Refresh();
            }
        }

        private void wp_TermsAndConditions_Enter(object sender, EventArgs e)
        {
            // reset topmost flag
            this.TopMost = false;
            // load terms and conditions file
            rb_TermsAndConditions.Text = "";
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(Application.StartupPath, "TERMSANDCONDITIONS")))
                {
                    rb_TermsAndConditions.Text = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(),LogLevel.Error);
            }
            if (rb_TermsAndConditions.Text.Length > 0)
                cb_TermsAndConditions.Enabled = true;
            else
                cb_TermsAndConditions.Enabled = false;
            }

        private void cb_TermsAndConditions_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_TermsAndConditions.Checked)
                wp_TermsAndConditions.AllowNext = true;
            else
                wp_TermsAndConditions.AllowNext = false;
        }

        private void wizardControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        #region wp_General

        private void wp_GeneralCoverage_Enter(object sender, EventArgs e)
        {
            UpdateCoverage();
            ud_MinLon.Value = (decimal)Properties.Settings.Default.MinLon;
            ud_MaxLon.Value = (decimal)Properties.Settings.Default.MaxLon;
            ud_MinLat.Value = (decimal)Properties.Settings.Default.MinLat;
            ud_MaxLat.Value = (decimal)Properties.Settings.Default.MaxLat;

        }

        private void UpdateCoverage()
        {
            Coveragepolygons.Clear();
            // add tile to map polygons
            List<PointLatLng> l = new List<PointLatLng>();
            l.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            l.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            l.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            l.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon p = new GMapPolygon(l, "Coverage");
            p.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            p.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            Coveragepolygons.Polygons.Add(p);
            // zoom the map
            gm_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void gm_Coverage_Enter(object sender, EventArgs e)
        {
        }

        private void ud_MinLon_ValueChanged(object sender, EventArgs e)
        {
            if ((double)ud_MinLon.Value <= Properties.Settings.Default.MaxLon)
            {
                Properties.Settings.Default.MinLon = (double)ud_MinLon.Value;
                UpdateCoverage();
            }
        }

        private void ud_MaxLon_ValueChanged(object sender, EventArgs e)
        {
            if ((double)ud_MaxLon.Value >= Properties.Settings.Default.MinLon)
            {
                Properties.Settings.Default.MaxLon = (double)ud_MaxLon.Value;
                UpdateCoverage();
            }
        }

        private void ud_MinLat_ValueChanged(object sender, EventArgs e)
        {
            if ((double)ud_MinLat.Value <= Properties.Settings.Default.MaxLat)
            {
                Properties.Settings.Default.MinLat = (double)ud_MinLat.Value;
                UpdateCoverage();
            }
        }

        private void ud_MaxLat_ValueChanged(object sender, EventArgs e)
        {
            if ((double)ud_MaxLat.Value >= Properties.Settings.Default.MinLat)
            {
                Properties.Settings.Default.MaxLat = (double)ud_MaxLat.Value;
                UpdateCoverage();
            }
        }

        #endregion

        #region wp_ElevationModel
        private void wp_ElevationModel_Enter(object sender, EventArgs e)
        {
            cb_GLOBE.Checked = Properties.Settings.Default.Elevation_GLOBE_Enabled;
            cb_SRTM3.Checked = Properties.Settings.Default.Elevation_SRTM3_Enabled;
            cb_SRTM1.Checked = Properties.Settings.Default.Elevation_SRTM1_Enabled;
            cb_ASTER3.Checked = Properties.Settings.Default.Elevation_ASTER3_Enabled;
            cb_ASTER1.Checked = Properties.Settings.Default.Elevation_ASTER1_Enabled;
            UpdateElevationPages();
        }

        private void UpdateElevationPages()
        {
            wp_GLOBE.Suppress = !cb_GLOBE.Checked;
            wp_SRTM3.Suppress = !cb_SRTM3.Checked;
            wp_SRTM1.Suppress = !cb_SRTM1.Checked;
            wp_ASTER3.Suppress = !cb_ASTER3.Checked;
            wp_ASTER1.Suppress = !cb_ASTER1.Checked;
            wp_ElevationModel.AllowNext = cb_GLOBE.Checked || cb_SRTM3.Checked || cb_SRTM1.Checked || cb_ASTER3.Checked || cb_ASTER1.Checked;
            this.Refresh();
        }

        private void cb_GLOBE_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Elevation_GLOBE_Enabled = cb_GLOBE.Checked;
            UpdateElevationPages();
        }

        private void cb_SRTM3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Elevation_SRTM3_Enabled = cb_SRTM3.Checked;
            UpdateElevationPages();
        }

        private void cb_SRTM1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Elevation_SRTM1_Enabled = cb_SRTM1.Checked;
            UpdateElevationPages();
        }


        private void cb_ASTER3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Elevation_ASTER3_Enabled = cb_ASTER3.Checked;
            UpdateElevationPages();
        }

        private void cb_ASTER1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Elevation_ASTER1_Enabled = cb_ASTER1.Checked;
            UpdateElevationPages();
        }

        #endregion

        #region wp_GLOBE

        private void bw_GLOBE_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_GLOBE_MapUpdater.ReportProgress(0, "Creating elevation tile catalogue (please wait)...");
            // get all locs needed for covered area
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_GLOBE_MapUpdater, ELEVATIONMODEL.GLOBE, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_GLOBE_MapUpdater.ReportProgress(0, "Processing tiles...");
            // report tile count
            bw_GLOBE_MapUpdater.ReportProgress(2, availabletiles.Files.Keys.Count);
            int missing = 0;
            int found = 0;
            foreach (string tilename in availabletiles.Files.Keys)
            {
                if (ElevationData.Database.ElevationTileExists(tilename.Substring(0, 6), ELEVATIONMODEL.GLOBE))
                {
                    bw_GLOBE_MapUpdater.ReportProgress(1, tilename);
                    found++;
                }
                else
                {
                    bw_GLOBE_MapUpdater.ReportProgress(-1, tilename);
                    missing++;
                }
                if (bw_GLOBE_MapUpdater.CancellationPending)
                {
                    bw_GLOBE_MapUpdater.ReportProgress(0, "Processing cancelled...");
                    return;
                }
            }
            bw_GLOBE_MapUpdater.ReportProgress(0, found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_GLOBE_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 2)
            {
                try
                {
                    wp_GLOBE.AllowNext = true;
                    // get tile count = available tiles - tiles already in data base
                    long tilecount = (int)e.UserState - ElevationData.Database.ElevationTileCount(ELEVATIONMODEL.GLOBE);
                    if (tilecount < 0)
                        tilecount = 0;
                    // check available disk space and file system
                    // estimate disk space needed = tilecount * tilesize * 150%
                    long spaceneeded = (long)tilecount * (long)(250 * 3 / 2);
                    string rootdrive = Path.GetPathRoot(ElevationData.Database.DefaultDatabaseDirectory(ELEVATIONMODEL.GLOBE));
                    long spaceavailable = SupportFunctions.GetDriveAvailableFreeSpace(rootdrive);
                    // check for available disk space, skip on zero (cannot determine)
                    if ((spaceavailable > 0) && (spaceavailable < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("Not enough disk space for elevation database.\n\nAvailable: " + spaceavailable + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or try to enlarge free space on disk.", "Not enough disk space");
                        wp_GLOBE.AllowNext = false;
                    }
                    long maxfilesize = SupportFunctions.GetDriveMaxFileSize(rootdrive);
                    if ((maxfilesize > 0) && (maxfilesize < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("The elevation database will exceed maximum allowed filesize for this file system.\n\nAllowed: " + maxfilesize + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or change file system on disk.", "File size exceeded");
                        wp_GLOBE.AllowNext = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(),LogLevel.Error);
                }
            }
            else if (e.ProgressPercentage == 1)
            {
                // add a tile found in database to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Green));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
                GLOBEpolygons.Polygons.Add(p);
            }
            else if (e.ProgressPercentage == -1)
            {
                // add missing tile to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Red));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                GLOBEpolygons.Polygons.Add(p);
            }
            else
            {
                lbl_GLOBE_Status.Text = (string)e.UserState;
            }
        }

        private void bw_GLOBE_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gm_GLOBE.Visible = true;
        }

        private void wp_GLOBE_Enter(object sender, EventArgs e)
        {
            wp_GLOBE.AllowNext = false;
            // clear map polygons
            GLOBEpolygons.Clear();
            gm_GLOBE.Visible = true;
            // add coverage to map polygons
            List<PointLatLng> cl = new List<PointLatLng>();
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon c = new GMapPolygon(cl, "Coverage");
            c.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            c.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            GLOBEpolygons.Polygons.Add(c);
            // zoom the map initally
            gm_GLOBE.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_GLOBE_MapUpdater.IsBusy)
            {
                if (SupportFunctions.IsMono)
                    gm_GLOBE.Visible = false;

                bw_GLOBE_MapUpdater.RunWorkerAsync();

            }

            // zoom the map
            gm_GLOBE.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void wp_GLOBE_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_GLOBE_MapUpdater.CancelAsync();
            // clear map polygons
            GLOBEpolygons.Clear();
            // do garbage collection
            GC.Collect();
            lbl_GLOBE_Status.Text = "";
        }

        private void wp_GLOBE_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

        #endregion

        #region wp_SRTM3

        private void bw_SRTM3_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_SRTM3_MapUpdater.ReportProgress(0, "Creating elevation tile catalogue (please wait)...");
            // get all locs needed for covered area
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_SRTM3_MapUpdater, ELEVATIONMODEL.SRTM3, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_SRTM3_MapUpdater.ReportProgress(0, "Processing tiles...");
            // report tile count
            bw_SRTM3_MapUpdater.ReportProgress(2, availabletiles.Files.Keys.Count);
            int missing = 0;
            int found = 0;
            foreach (string tilename in availabletiles.Files.Keys)
            {
                if (ElevationData.Database.ElevationTileExists(tilename.Substring(0, 6), ELEVATIONMODEL.SRTM3))
                {
                    bw_SRTM3_MapUpdater.ReportProgress(1, tilename);
                    found++;
                }
                else
                {
                    bw_SRTM3_MapUpdater.ReportProgress(-1, tilename);
                    missing++;
                }
                if (bw_SRTM3_MapUpdater.CancellationPending)
                {
                    bw_SRTM3_MapUpdater.ReportProgress(0, "Processing cancelled...");
                    return;
                }
            }
            bw_SRTM3_MapUpdater.ReportProgress(0, found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_SRTM3_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 2)
            {
                try
                {
                    wp_SRTM3.AllowNext = true;
                    // get tile count = available tiles - tiles already in data base
                    long tilecount = (int)e.UserState - ElevationData.Database.ElevationTileCount(ELEVATIONMODEL.SRTM3);
                    if (tilecount < 0)
                        tilecount = 0;
                    // check available disk space and file system
                    // estimate disk space needed = tilecount * tilesize * 150%
                    long spaceneeded = (long)tilecount * (long)(10000 * 3 / 2);
                    string rootdrive = Path.GetPathRoot(ElevationData.Database.DefaultDatabaseDirectory(ELEVATIONMODEL.SRTM3));
                    long spaceavailable = SupportFunctions.GetDriveAvailableFreeSpace(rootdrive);
                    // check for available disk space, skip on zero (cannot determine)
                    if ((spaceavailable > 0) && (spaceavailable < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("Not enough disk space for elevation database.\n\nAvailable: " + spaceavailable + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or try to enlarge free space on disk.", "Not enough disk space");
                        wp_SRTM3.AllowNext = false;
                    }
                    long maxfilesize = SupportFunctions.GetDriveMaxFileSize(rootdrive);
                    if ((maxfilesize > 0) && (maxfilesize < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("The elevation database will exceed maximum allowed filesize for this file system.\n\nAllowed: " + maxfilesize + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or change file system on disk.", "File size exceeded");
                        wp_SRTM3.AllowNext = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            else if (e.ProgressPercentage == 1)
            {
                // add a tile found in database to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Green));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
                SRTM3polygons.Polygons.Add(p);
            }
            else if (e.ProgressPercentage == -1)
            {
                // add missing tile to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Red));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                SRTM3polygons.Polygons.Add(p);
            }
            else
            {
                lbl_SRTM3_Status.Text = (string)e.UserState;
            }
        }

        private void bw_SRTM3_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gm_SRTM3.Visible = true;
        }

        private void wp_SRTM3_Enter(object sender, EventArgs e)
        {
            wp_SRTM3.AllowNext = false;
            // clear map polygons
            SRTM3polygons.Clear();
            // add coverage to map polygons
            List<PointLatLng> cl = new List<PointLatLng>();
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon c = new GMapPolygon(cl, "Coverage");
            c.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            c.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            SRTM3polygons.Polygons.Add(c);
            // zoom the map initally
            gm_SRTM3.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_SRTM3_MapUpdater.IsBusy)
            {
                if (SupportFunctions.IsMono)
                    gm_SRTM3.Visible = false;

                bw_SRTM3_MapUpdater.RunWorkerAsync();

            }
            // zoom the map
            gm_SRTM3.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void wp_SRTM3_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_SRTM3_MapUpdater.CancelAsync();
            // clear map polygons
            SRTM3polygons.Clear();
            // do garbage collection
            GC.Collect();
            lbl_SRTM3_Status.Text = "";
        }

        #endregion

        #region wp_SRTM1

        private void bw_SRTM1_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_SRTM1_MapUpdater.ReportProgress(0, "Creating elevation tile catalogue (please wait)...");
            // get all locs needed for covered area
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_SRTM1_MapUpdater, ELEVATIONMODEL.SRTM1, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_SRTM1_MapUpdater.ReportProgress(0, "Processing tiles...");
            // report tile count
            bw_SRTM1_MapUpdater.ReportProgress(2, availabletiles.Files.Keys.Count);
            int missing = 0;
            int found = 0;
            foreach (string tilename in availabletiles.Files.Keys)
            {
                if (ElevationData.Database.ElevationTileExists(tilename.Substring(0, 6), ELEVATIONMODEL.SRTM1))
                {
                    bw_SRTM1_MapUpdater.ReportProgress(1, tilename);
                    found++;
                }
                else
                {
                    bw_SRTM1_MapUpdater.ReportProgress(-1, tilename);
                    missing++;
                }
                if (bw_SRTM1_MapUpdater.CancellationPending)
                {
                    bw_SRTM1_MapUpdater.ReportProgress(0, "Processing cancelled...");
                    return;
                }
            }
            bw_SRTM1_MapUpdater.ReportProgress(0, found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_SRTM1_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 2)
            {
                try
                {
                    wp_SRTM1.AllowNext = true;
                    // get tile count = available tiles - tiles already in data base
                    long tilecount = (int)e.UserState - ElevationData.Database.ElevationTileCount(ELEVATIONMODEL.SRTM1);
                    if (tilecount < 0)
                        tilecount = 0;
                    // check available disk space and file system
                    // estimate disk space needed = tilecount * tilesize * 150%
                    long spaceneeded = (long)tilecount * (long)(100000 * 3 / 2);
                    string rootdrive = Path.GetPathRoot(ElevationData.Database.DefaultDatabaseDirectory(ELEVATIONMODEL.SRTM1));
                    long spaceavailable = SupportFunctions.GetDriveAvailableFreeSpace(rootdrive); 
                    // check for available disk space, skip on zero (cannot determine)
                    if ((spaceavailable > 0) && (spaceavailable < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("Not enough disk space for elevation database.\n\nAvailable: " + spaceavailable + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or try to enlarge free space on disk.", "Not enough disk space");
                        wp_SRTM1.AllowNext = false;
                    }
                    long maxfilesize = SupportFunctions.GetDriveMaxFileSize(rootdrive);
                    if ((maxfilesize > 0) && (maxfilesize < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("The elevation database will exceed maximum allowed filesize for this file system.\n\nAllowed: " + maxfilesize + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or change file system on disk.", "File size exceeded");
                        wp_SRTM1.AllowNext = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            else if (e.ProgressPercentage == 1)
            {
                // add a tile found in database to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Green));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
                SRTM1polygons.Polygons.Add(p);
            }
            else if (e.ProgressPercentage == -1)
            {
                // add missing tile to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Red));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                SRTM1polygons.Polygons.Add(p);
            }
            else
            {
                lbl_SRTM1_Status.Text = (string)e.UserState;
            }
        }

        private void bw_SRTM1_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gm_SRTM1.Visible = true;
        }

        private void wp_SRTM1_Enter(object sender, EventArgs e)
        {
            wp_SRTM1.AllowNext = false;
            // clear map polygons
            SRTM1polygons.Clear();
            // add coverage to map polygons
            List<PointLatLng> cl = new List<PointLatLng>();
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon c = new GMapPolygon(cl, "Coverage");
            c.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            c.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            SRTM1polygons.Polygons.Add(c);
            // zoom the map initally
            gm_SRTM1.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_SRTM1_MapUpdater.IsBusy)
            {
                if (SupportFunctions.IsMono)
                    gm_SRTM1.Visible = false;

                bw_SRTM1_MapUpdater.RunWorkerAsync();

            }

            // zoom the map
            gm_SRTM1.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void wp_SRTM1_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_SRTM1_MapUpdater.CancelAsync();
            // clear map polygons
            SRTM1polygons.Clear();
            // do garbage collection
            GC.Collect();
            lbl_SRTM1_Status.Text = "";
        }

        #endregion

        #region wp_ASTER3

        private void bw_ASTER3_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_ASTER3_MapUpdater.ReportProgress(0, "Creating elevation tile catalogue (please wait)...");
            // get all locs needed for covered area
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_ASTER3_MapUpdater, ELEVATIONMODEL.ASTER3, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_ASTER3_MapUpdater.ReportProgress(0, "Processing tiles...");
            // report tile count
            bw_ASTER3_MapUpdater.ReportProgress(2, availabletiles.Files.Keys.Count);
            int missing = 0;
            int found = 0;
            foreach (string tilename in availabletiles.Files.Keys)
            {
                if (ElevationData.Database.ElevationTileExists(tilename.Substring(0, 6), ELEVATIONMODEL.ASTER3))
                {
                    bw_ASTER3_MapUpdater.ReportProgress(1, tilename);
                    found++;
                }
                else
                {
                    bw_ASTER3_MapUpdater.ReportProgress(-1, tilename);
                    missing++;
                }
                if (bw_ASTER3_MapUpdater.CancellationPending)
                {
                    bw_ASTER3_MapUpdater.ReportProgress(0, "Processing cancelled...");
                    return;
                }
            }
            bw_ASTER3_MapUpdater.ReportProgress(0, found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_ASTER3_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 2)
            {
                try
                {
                    wp_ASTER3.AllowNext = true;
                    // get tile count = available tiles - tiles already in data base
                    long tilecount = (int)e.UserState - ElevationData.Database.ElevationTileCount(ELEVATIONMODEL.ASTER3);
                    if (tilecount < 0)
                        tilecount = 0;
                    // check available disk space and file system
                    // estimate disk space needed = tilecount * tilesize * 150%
                    long spaceneeded = (long)tilecount * (long)(10000 * 3 / 2);
                    string rootdrive = Path.GetPathRoot(ElevationData.Database.DefaultDatabaseDirectory(ELEVATIONMODEL.ASTER3));
                    long spaceavailable = SupportFunctions.GetDriveAvailableFreeSpace(rootdrive);
                    // check for available disk space, skip on zero (cannot determine)
                    if ((spaceavailable > 0) && (spaceavailable < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("Not enough disk space for elevation database.\n\nAvailable: " + spaceavailable + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or try to enlarge free space on disk.", "Not enough disk space");
                        wp_ASTER3.AllowNext = false;
                    }
                    long maxfilesize = SupportFunctions.GetDriveMaxFileSize(rootdrive);
                    if ((maxfilesize > 0) && (maxfilesize < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("The elevation database will exceed maximum allowed filesize for this file system.\n\nAllowed: " + maxfilesize + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or change file system on disk.", "File size exceeded");
                        wp_ASTER3.AllowNext = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            else if (e.ProgressPercentage == 1)
            {
                // add a tile found in database to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Green));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
                ASTER3polygons.Polygons.Add(p);
            }
            else if (e.ProgressPercentage == -1)
            {
                // add missing tile to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Red));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                ASTER3polygons.Polygons.Add(p);
            }
            else
            {
                lbl_ASTER3_Status.Text = (string)e.UserState;
            }
        }

        private void bw_ASTER3_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void wp_ASTER3_Enter(object sender, EventArgs e)
        {
            wp_ASTER3.AllowNext = false;
            // clear map polygons
            ASTER3polygons.Clear();
            // add coverage to map polygons
            List<PointLatLng> cl = new List<PointLatLng>();
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon c = new GMapPolygon(cl, "Coverage");
            c.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            c.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            ASTER3polygons.Polygons.Add(c);
            // zoom the map initally
            gm_ASTER3.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_ASTER3_MapUpdater.IsBusy)
                bw_ASTER3_MapUpdater.RunWorkerAsync();
            // zoom the map
            gm_ASTER3.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void wp_ASTER3_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_ASTER3_MapUpdater.CancelAsync();
            // clear map polygons
            ASTER3polygons.Clear();
            // do garbage collection
            GC.Collect();
            lbl_ASTER3_Status.Text = "";
        }

        #endregion

        #region wp_ASTER1

        private void bw_ASTER1_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_ASTER1_MapUpdater.ReportProgress(0, "Creating elevation tile catalogue (please wait)...");
            // get all locs needed for covered area
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_ASTER1_MapUpdater, ELEVATIONMODEL.ASTER1, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_ASTER1_MapUpdater.ReportProgress(0, "Processing tiles...");
            // report tile count
            bw_ASTER1_MapUpdater.ReportProgress(2, availabletiles.Files.Keys.Count);
            int missing = 0;
            int found = 0;
            foreach (string tilename in availabletiles.Files.Keys)
            {
                if (ElevationData.Database.ElevationTileExists(tilename.Substring(0, 6), ELEVATIONMODEL.ASTER1))
                {
                    bw_ASTER1_MapUpdater.ReportProgress(1, tilename);
                    found++;
                }
                else
                {
                    bw_ASTER1_MapUpdater.ReportProgress(-1, tilename);
                    missing++;
                }
                if (bw_ASTER1_MapUpdater.CancellationPending)
                {
                    bw_ASTER1_MapUpdater.ReportProgress(0, "Processing cancelled...");
                    return;
                }
            }
            bw_ASTER1_MapUpdater.ReportProgress(0, found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_ASTER1_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 2)
            {
                try
                {
                    wp_ASTER1.AllowNext = true;
                    // get tile count = available tiles - tiles already in data base
                    long tilecount = (int)e.UserState - ElevationData.Database.ElevationTileCount(ELEVATIONMODEL.ASTER1);
                    if (tilecount < 0)
                        tilecount = 0;
                    // check available disk space and file system
                    // estimate disk space needed = tilecount * tilesize * 150%
                    long spaceneeded = (long)tilecount * (long)(100000 * 3 / 2);
                    string rootdrive = Path.GetPathRoot(ElevationData.Database.DefaultDatabaseDirectory(ELEVATIONMODEL.ASTER1));
                    long spaceavailable = SupportFunctions.GetDriveAvailableFreeSpace(rootdrive);
                    // check for available disk space, skip on zero (cannot determine)
                    if ((spaceavailable > 0) && (spaceavailable < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("Not enough disk space for elevation database.\n\nAvailable: " + spaceavailable + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or try to enlarge free space on disk.", "Not enough disk space");
                        wp_ASTER1.AllowNext = false;
                    }
                    long maxfilesize = SupportFunctions.GetDriveMaxFileSize(rootdrive);
                    if ((maxfilesize > 0) && (maxfilesize < spaceneeded))
                    {
                        // show message box
                        MessageBox.Show("The elevation database will exceed maximum allowed filesize for this file system.\n\nAllowed: " + maxfilesize + " bytes.\nNeeded: " + spaceneeded + "bytes.\n\nUncheck this option or change file system on disk.", "File size exceeded");
                        wp_ASTER1.AllowNext = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                }
            }
            else if (e.ProgressPercentage == 1)
            {
                // add a tile found in database to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Green));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
                ASTER1polygons.Polygons.Add(p);
            }
            else if (e.ProgressPercentage == -1)
            {
                // add missing tile to map polygons
                double baselat;
                double baselon;
                MaidenheadLocator.LatLonFromLoc(((string)e.UserState).Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                GMapPolygon p = new GMapPolygon(l, (string)e.UserState);
                p.Stroke = new Pen(Color.FromArgb(50, Color.Red));
                p.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                ASTER1polygons.Polygons.Add(p);
            }
            else
            {
                lbl_ASTER1_Status.Text = (string)e.UserState;
            }
        }

        private void bw_ASTER1_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void wp_ASTER1_Enter(object sender, EventArgs e)
        {
            wp_ASTER1.AllowNext = false;
            // clear map polygons
            ASTER1polygons.Clear();
            // add coverage to map polygons
            List<PointLatLng> cl = new List<PointLatLng>();
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MinLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon));
            cl.Add(new PointLatLng(Properties.Settings.Default.MaxLat, Properties.Settings.Default.MinLon));
            GMapPolygon c = new GMapPolygon(cl, "Coverage");
            c.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            c.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            ASTER1polygons.Polygons.Add(c);
            // zoom the map initally
            gm_ASTER1.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_ASTER1_MapUpdater.IsBusy)
                bw_ASTER1_MapUpdater.RunWorkerAsync();
            // zoom the map
            gm_ASTER1.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void wp_ASTER1_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_ASTER1_MapUpdater.CancelAsync();
            // clear map polygons
            ASTER1polygons.Clear();
            // do garbage collection
            GC.Collect();
            lbl_ASTER1_Status.Text = "";
        }

        #endregion

        #region wp_Finish
        private void wp_Finish_Enter(object sender, EventArgs e)
        {
                lbl_Finish.Text = "All inputs are done successfully. You can start AirScout now.\n\nIn case of missing elevation data the download will start immediately.\n\nFull funtionality will not be reached until\nthe database is up to date.";
        }
        #endregion

        #region wp_UserDetails

        private void wp_UserDetails_Enter(object sender, EventArgs e)
        {
            // initially set textboxes
            tb_Callsign.SilentText = Properties.Settings.Default.MyCall;
            tb_Latitude.SilentValue = Properties.Settings.Default.MyLat;
            tb_Longitude.SilentValue = Properties.Settings.Default.MyLon;
            tb_Locator.SilentText = MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength);
            ValidateMyDetails();
        }

        private bool ValidateMyDetails()
        {
            // validates user details and sets position on map
            // enables/disables next button
            double mlat, mlon;
            // colour Textbox if more precise lat/lon information is available
            if (MaidenheadLocator.IsPrecise(tb_Latitude.Value, tb_Longitude.Value, 3))
            {
                if (tb_Locator.BackColor != Color.PaleGreen)
                    tb_Locator.BackColor = Color.PaleGreen;
            }
            else
            {
                if (tb_Locator.BackColor != Color.FloralWhite)
                    tb_Locator.BackColor = Color.FloralWhite;
            }
            if (GeographicalPoint.Check(tb_Latitude.Value, tb_Longitude.Value))
            {
                // update locator text if not focused
                if (!tb_Locator.Focused)
                {
                    tb_Locator.SilentText = MaidenheadLocator.LocFromLatLon(tb_Latitude.Value, tb_Longitude.Value, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, true);
                }
                // get locator polygon
                Callsignpolygons.Clear();
                List<PointLatLng> l = new List<PointLatLng>();
                // add loc bounds to map polygons
                MaidenheadLocator.LatLonFromLoc(tb_Locator.Text, PositionInRectangle.TopLeft, out mlat, out mlon);
                l.Add(new PointLatLng(mlat, mlon));
                MaidenheadLocator.LatLonFromLoc(tb_Locator.Text, PositionInRectangle.TopRight, out mlat, out mlon);
                l.Add(new PointLatLng(mlat, mlon));
                MaidenheadLocator.LatLonFromLoc(tb_Locator.Text, PositionInRectangle.BottomRight, out mlat, out mlon);
                l.Add(new PointLatLng(mlat, mlon));
                MaidenheadLocator.LatLonFromLoc(tb_Locator.Text, PositionInRectangle.BottomLeft, out mlat, out mlon);
                l.Add(new PointLatLng(mlat, mlon));
                GMapPolygon p = new GMapPolygon(l, tb_Locator.Text.ToString());
                p.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
                p.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
                Callsignpolygons.Polygons.Add(p);
                // update user position
                UserPos.Position = new PointLatLng(tb_Latitude.Value, tb_Longitude.Value);
                // update map position
                if (!IsDraggingMarker)
                {
                    string loc = MaidenheadLocator.LocFromLatLon(tb_Latitude.Value, tb_Longitude.Value, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, true);
                    MaidenheadLocator.LatLonFromLoc(loc, PositionInRectangle.MiddleMiddle, out mlat, out mlon);
                    gm_Callsign.Position = new PointLatLng(mlat, mlon);
                    // adjust map zoom level
                    int zoom = loc.Length;
                    switch (zoom)
                    {
                        case 6: gm_Callsign.Zoom = 12;
                            break;
                        case 8: gm_Callsign.Zoom = 15;
                            break;
                        case 10: gm_Callsign.Zoom = 17;
                            break;
                    }
                }
            }
            // check all values and enable/disable next button
            // save settings
            if (Callsign.Check(tb_Callsign.Text))
                Properties.Settings.Default.MyCall = tb_Callsign.Text;
            if (!double.IsNaN(tb_Latitude.Value))
                Properties.Settings.Default.MyLat = tb_Latitude.Value;
            if (!double.IsNaN(tb_Longitude.Value))
                Properties.Settings.Default.MyLon = tb_Longitude.Value;
            if (Callsign.Check(tb_Callsign.Text) && MaidenheadLocator.Check(tb_Locator.Text) && !double.IsNaN(tb_Longitude.Value) && !double.IsNaN(tb_Longitude.Value))
            {
                // StationData.Database.LocationInsertOrUpdateIfNewer(new LocationDesignator(tb_Callsign.Text, tb_Latitude.Value, tb_Longitude.Value, (MaidenheadLocator.IsPrecise(tb_Latitude.Value, tb_Longitude.Value, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC)));
                wp_UserDetails.AllowNext = true;
                return true;
            }
            else
            {
                wp_UserDetails.AllowNext = false;
                return false;
            }
        }

        private void tb_Callsign_TextChanged(object sender, EventArgs e)
        {
            if (tb_Callsign.Focused && Callsign.Check(tb_Callsign.Text))
            {
                LocationDesignator ld = StationData.Database.LocationFind(tb_Callsign.Text);
                if (ld != null)
                {
                    tb_Latitude.SilentValue = ld.Lat;
                    tb_Longitude.SilentValue = ld.Lon;
                    ValidateMyDetails();
                    return;
                }
                else
                {
                    // clear properties
                    Properties.Settings.Default.MyLat = double.NaN;
                    Properties.Settings.Default.MyLon = double.NaN;

                    tb_Latitude.SilentValue = double.NaN;
                    tb_Longitude.SilentValue = double.NaN;
                    tb_Locator.SilentText = "";
                }
            }
            ValidateMyDetails();
        }

        private void tb_Latitude_TextChanged(object sender, EventArgs e)
        {
            ValidateMyDetails();
        }

        private void tb_Longitude_TextChanged(object sender, EventArgs e)
        {
            ValidateMyDetails();
        }

        private void tb_Locator_TextChanged(object sender, EventArgs e)
        {
            // update lat/lon
            double mlat, mlon;
            if (tb_Locator.Focused)
            {
                // locator box is focused --> update lat/lon
                if (MaidenheadLocator.Check(tb_Locator.Text) && tb_Locator.Text.Length >= 6)
                {
                    MaidenheadLocator.LatLonFromLoc(tb_Locator.Text, PositionInRectangle.MiddleMiddle, out mlat, out mlon);
                    tb_Latitude.SilentValue = mlat;
                    tb_Longitude.SilentValue = mlon;
                }
                else
                {
                    tb_Latitude.SilentValue = double.NaN;
                    tb_Longitude.SilentValue = double.NaN;
                }
            }
            ValidateMyDetails();
        }

        private void gm_Callsign_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (UserPos != null && UserPos.IsMouseOver))
            {
                // dummy set user position to set mouse position exact to marker's location
                UserPos.Position = UserPos.Position;
                gm_Callsign.CanDragMap = false;
                IsDraggingMarker = true;
            }
        }

        private void gm_Callsign_MouseMove(object sender, MouseEventArgs e)
        {
            if ((UserPos != null) && IsDraggingMarker)
            {
                if (Callsign.Check(tb_Callsign.Text))
                {
                    // get geographic coordinates of mouse pointer
                    PointLatLng p = gm_Callsign.FromLocalToLatLng(e.X, e.Y);
                    tb_Latitude.SilentValue = p.Lat;
                    tb_Longitude.SilentValue = p.Lng;
                    UserPos.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    UserPos.ToolTipText = tb_Callsign.Text;
                    Properties.Settings.Default.MyLat = p.Lat;
                    Properties.Settings.Default.MyLon = p.Lng;
                    ValidateMyDetails();
                }
                else
                {
                    UserPos.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    UserPos.ToolTipText = "Please enter a valid callsign first.";
                }
            }
        }

        private void gm_Callsign_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsDraggingMarker)
            {
                gm_Callsign.CanDragMap = true;
                IsDraggingMarker = false;
            }
        }

        private void gm_Callsign_OnMarkerEnter(GMapMarker item)
        {
        }

        private void btn_QRZ_Click(object sender, EventArgs e)
        {
            // get an EN format provider
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            try
            {
                WebRequest myWebRequest = WebRequest.Create(Properties.Settings.Default.QRZ_URL_Database + Properties.Settings.Default.MyCall);
                myWebRequest.Timeout = 10000;
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                string s = readStream.ReadToEnd();
                if (s.IndexOf("cs_lat") >= 0)
                {
                    string loc;
                    double lat = 0;
                    double lon = 0;
                    try
                    {
                        s = s.Remove(0, s.IndexOf("cs_lat = \"") + 10);
                        lat = System.Convert.ToDouble(s.Substring(0, s.IndexOf("\n") - 2), provider);
                        s = s.Remove(0, s.IndexOf("cs_lon = \"") + 10);
                        lon = System.Convert.ToDouble(s.Substring(0, s.IndexOf("\n") - 2), provider);
                    }
                    catch
                    {
                    }
                    loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                    // check if loc is matching --> refine lat/lon
                    if ((tb_Locator.Text.Length >= 6) && (loc == MaidenheadLocator.Convert(tb_Locator.Text, false).Substring(0, 6)))
                    {
                        Properties.Settings.Default["MyLat"] = lat;
                        Properties.Settings.Default.MyLon = lon;
                        tb_Latitude.Value = lat;
                        tb_Longitude.Value = lon;
                        MessageBox.Show("Position update from QRZ.com was performed succesfully.", "QRZ.com");
                        return;
                    }
                }
            }
            catch
            {
            }
            MessageBox.Show("Position query at QRZ.com failed or does not match with current locator. \nNo update was performed.", "QRZ.com");

        }

        private void btn_Zoom_In_Click(object sender, EventArgs e)
        {
            if (gm_Callsign.Zoom < 20)
                gm_Callsign.Zoom++;
        }

        private void btn_Zoom_Out_Click(object sender, EventArgs e)
        {
            if (gm_Callsign.Zoom > 0)
                gm_Callsign.Zoom--;
        }

        private void gm_Callsign_OnMapZoomChanged()
        {
            // maintain zoom level
            tb_Zoom.Text = gm_Callsign.Zoom.ToString();
        }

        #endregion

        #region wp_PlaneFeeds
        private void ValidatePlaneFeeds()
        {
            if ((cb_PlaneFeed1.SelectedItem != null) && (cb_PlaneFeed2.SelectedItem != null) && (cb_PlaneFeed3.SelectedItem != null) && (cb_PlaneFeed1.SelectedItem.ToString() == "[none]") && (cb_PlaneFeed2.SelectedItem.ToString() == "[none]") && (cb_PlaneFeed3.SelectedItem.ToString() == "[none]"))
                wp_PlaneFeeds.AllowNext = false;
            else
                wp_PlaneFeeds.AllowNext = true;
        }

        private void wp_PlaneFeeds_Enter(object sender, EventArgs e)
        {
            // set initial settings for planes tab
            cb_PlaneFeed1.DisplayMember = "Name";
            cb_PlaneFeed2.DisplayMember = "Name";
            cb_PlaneFeed3.DisplayMember = "Name";
            cb_PlaneFeed1.Items.Clear();
            cb_PlaneFeed2.Items.Clear();
            cb_PlaneFeed3.Items.Clear();
            cb_PlaneFeed1.Items.Add("[none]");
            cb_PlaneFeed2.Items.Add("[none]");
            cb_PlaneFeed3.Items.Add("[none]");
            if (ParentDlg.PlaneFeedPlugins != null)
            {
                foreach (var plugin in ParentDlg.PlaneFeedPlugins)
                {
                    cb_PlaneFeed1.Items.Add(plugin);
                    cb_PlaneFeed2.Items.Add(plugin);
                    cb_PlaneFeed3.Items.Add(plugin);
                }
            }
            cb_PlaneFeed1.SelectedIndex = cb_PlaneFeed1.FindStringExact(Properties.Settings.Default.Planes_PlaneFeed1);
            cb_PlaneFeed2.SelectedIndex = cb_PlaneFeed1.FindStringExact(Properties.Settings.Default.Planes_PlaneFeed2);
            cb_PlaneFeed3.SelectedIndex = cb_PlaneFeed1.FindStringExact(Properties.Settings.Default.Planes_PlaneFeed3);
            ValidatePlaneFeeds();
        }

        private void cb_PlaneFeed1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_PlaneFeed1.SelectedItem == null) || (cb_PlaneFeed1.GetItemText(cb_PlaneFeed1.SelectedItem) == "[none]"))
            {
                Properties.Settings.Default.Planes_PlaneFeed1 = "[none]";
                ValidatePlaneFeeds();
                return;
            }
            /*
            PlaneFeed feed = (PlaneFeed)cb_PlaneFeed1.SelectedItem;
            // show disclaimer if necessary
            if (!String.IsNullOrEmpty(feed.Disclaimer) && (String.IsNullOrEmpty(feed.DisclaimerAccepted)))
            {
                PlaneFeedDisclaimerDlg Dlg = new PlaneFeedDisclaimerDlg();
                Dlg.tb_DisclaimerText.Text = feed.Disclaimer;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // making a unique ID for confirmation
                    string ID = "";
                    try
                    {
                        ID = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductId", "");
                    }
                    catch
                    {
                        ID = "Key not found!";
                    }
                    ID = ID + "," + DateTime.UtcNow.ToString("u");
                    ID = ID + "," + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    feed.DisclaimerAccepted = ID;
                }
                else
                {
                    cb_PlaneFeed1.SelectedItem = "[none]";
                }
            }
            */
            IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_PlaneFeed1.SelectedItem;
            // show disclaimer if necessary
            if (!String.IsNullOrEmpty(feed.Disclaimer) && (String.IsNullOrEmpty(feed.DisclaimerAccepted)))
            {
                PlaneFeedDisclaimerDlg Dlg = new PlaneFeedDisclaimerDlg();
                Dlg.tb_DisclaimerText.Text = feed.Disclaimer;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // making a unique ID for confirmation
                    string ID = "";
                    try
                    {
                        ID = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductId", "");
                    }
                    catch
                    {
                        ID = "Key not found!";
                    }
                    ID = ID + "," + DateTime.UtcNow.ToString("u");
                    ID = ID + "," + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    feed.DisclaimerAccepted = ID;
                }
                else
                {
                    cb_PlaneFeed1.SelectedItem = "[none]";
                }
            }
            Properties.Settings.Default.Planes_PlaneFeed1 = feed.Name;
            ValidatePlaneFeeds();
        }

        private void cb_PlaneFeed2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_PlaneFeed2.SelectedItem == null) || (cb_PlaneFeed2.GetItemText(cb_PlaneFeed2.SelectedItem) == "[none]"))
            {
                Properties.Settings.Default.Planes_PlaneFeed2 = "[none]";
                ValidatePlaneFeeds();
                return;
            }
            /*
            PlaneFeed feed = (PlaneFeed)cb_PlaneFeed2.SelectedItem;
            // show disclaimer if necessary
            if (!String.IsNullOrEmpty(feed.Disclaimer) && (String.IsNullOrEmpty(feed.DisclaimerAccepted)))
            {
                PlaneFeedDisclaimerDlg Dlg = new PlaneFeedDisclaimerDlg();
                Dlg.tb_DisclaimerText.Text = feed.Disclaimer;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // making a unique ID for confirmation
                    string ID = "";
                    try
                    {
                        ID = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductId", "");
                    }
                    catch
                    {
                        ID = "Key not found!";
                    }
                    ID = ID + "," + DateTime.UtcNow.ToString("u");
                    ID = ID + "," + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    feed.DisclaimerAccepted = ID;
                }
            }
            Properties.Settings.Default.Planes_PlaneFeed2 = feed.Name;
            */
            IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_PlaneFeed2.SelectedItem;
            // show disclaimer if necessary
            if (!String.IsNullOrEmpty(feed.Disclaimer) && (String.IsNullOrEmpty(feed.DisclaimerAccepted)))
            {
                PlaneFeedDisclaimerDlg Dlg = new PlaneFeedDisclaimerDlg();
                Dlg.tb_DisclaimerText.Text = feed.Disclaimer;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // making a unique ID for confirmation
                    string ID = "";
                    try
                    {
                        ID = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductId", "");
                    }
                    catch
                    {
                        ID = "Key not found!";
                    }
                    ID = ID + "," + DateTime.UtcNow.ToString("u");
                    ID = ID + "," + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    feed.DisclaimerAccepted = ID;
                }
                else
                {
                    cb_PlaneFeed2.SelectedItem = "[none]";
                }
            }
            Properties.Settings.Default.Planes_PlaneFeed2 = feed.Name;
            ValidatePlaneFeeds();
        }

        private void cb_PlaneFeed3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_PlaneFeed3.SelectedItem == null) || (cb_PlaneFeed3.GetItemText(cb_PlaneFeed3.SelectedItem) == "[none]"))
            {
                Properties.Settings.Default.Planes_PlaneFeed3 = "[none]";
                ValidatePlaneFeeds();
                return;
            }
            /*
            PlaneFeed feed = (PlaneFeed)cb_PlaneFeed3.SelectedItem;
            // show disclaimer if necessary
            if (!String.IsNullOrEmpty(feed.Disclaimer) && (String.IsNullOrEmpty(feed.DisclaimerAccepted)))
            {
                PlaneFeedDisclaimerDlg Dlg = new PlaneFeedDisclaimerDlg();
                Dlg.tb_DisclaimerText.Text = feed.Disclaimer;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // making a unique ID for confirmation
                    string ID = "";
                    try
                    {
                        ID = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductId", "");
                    }
                    catch
                    {
                        ID = "Key not found!";
                    }
                    ID = ID + "," + DateTime.UtcNow.ToString("u");
                    ID = ID + "," + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    feed.DisclaimerAccepted = ID;
                }
            }
            Properties.Settings.Default.Planes_PlaneFeed3 = feed.Name;
            */
            IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_PlaneFeed3.SelectedItem;
            // show disclaimer if necessary
            if (!String.IsNullOrEmpty(feed.Disclaimer) && (String.IsNullOrEmpty(feed.DisclaimerAccepted)))
            {
                PlaneFeedDisclaimerDlg Dlg = new PlaneFeedDisclaimerDlg();
                Dlg.tb_DisclaimerText.Text = feed.Disclaimer;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // making a unique ID for confirmation
                    string ID = "";
                    try
                    {
                        ID = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductId", "");
                    }
                    catch
                    {
                        ID = "Key not found!";
                    }
                    ID = ID + "," + DateTime.UtcNow.ToString("u");
                    ID = ID + "," + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    feed.DisclaimerAccepted = ID;
                }
                else
                {
                    cb_PlaneFeed3.SelectedItem = "[none]";
                }
            }
            Properties.Settings.Default.Planes_PlaneFeed3 = feed.Name;
            ValidatePlaneFeeds();
        }
        #endregion


        private void FirstRunWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            lbl_Finish.Text = "Please wait for download of last tile is finished...";
            bw_GLOBE_MapUpdater.CancelAsync();
            bw_SRTM3_MapUpdater.CancelAsync();
            bw_SRTM1_MapUpdater.CancelAsync();
            while (bw_GLOBE_MapUpdater.IsBusy)
                Application.DoEvents();
            while (bw_SRTM3_MapUpdater.IsBusy)
                Application.DoEvents();
            while (bw_SRTM1_MapUpdater.IsBusy)
                Application.DoEvents();
        }

    }



    public enum TILEDOWNLOADSTATUS
    {
        ERROR = -1,
        INIT = 0,
        DOWNLOADING = 2,
        DOWNLOADED = 3,
        UPTODATE = 4
    }
}
