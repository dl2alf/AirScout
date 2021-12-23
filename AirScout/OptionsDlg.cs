using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Globalization;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using AirScout.PlaneFeeds;
using AirScout.Core;
using AirScout.Aircrafts;
using ScoutBase.Core;
using ScoutBase.Stations;
using ScoutBase.Elevation;
using ScoutBase.Propagation;
using ScoutBase.Maps;
using ScoutBase;
using Newtonsoft.Json;
using static ScoutBase.Core.ZIP;
using AirScout.PlaneFeeds.Plugin.MEFContract;

namespace AirScout
{
    public partial class OptionsDlg : Form
    {

        GMapOverlay Coveragepolygons = new GMapOverlay("Coveragepolygons");
        GMapOverlay GLOBEpolygons = new GMapOverlay("GLOBEpolygons");
        GMapOverlay SRTM3polygons = new GMapOverlay("SRTM3polygons");
        GMapOverlay SRTM1polygons = new GMapOverlay("SRTM1polygons");

        MapDlg ParentDlg;

        NumberFormatInfo ENprovider;

        private LocalObstructionDesignator LocalObstructions;

        private LogWriter Log = LogWriter.Instance;

        public OptionsDlg(MapDlg parentDlg)
        {
            InitializeComponent();
            ParentDlg = parentDlg;
            // get an EN format provider
            ENprovider = new NumberFormatInfo();
            ENprovider.NumberDecimalSeparator = ".";
            ENprovider.NumberGroupSeparator = ",";


        }

        private void OptionsDlg_Load(object sender, EventArgs e)
        {
            Log.WriteMessage("Loading.");
            // fill data grid with band settings
            dgv_BandSettings.DataSource = Properties.Settings.Default.Path_Band_Settings;
            dgv_BandSettings.Columns[0].Width = 45;
            dgv_BandSettings.Columns[0].HeaderText = "Band\n";
            dgv_BandSettings.Columns[0].ToolTipText = "Band\n50M ... 76G";
            dgv_BandSettings.Columns[0].ReadOnly = true;
            dgv_BandSettings.Columns[1].Width = 60;
            dgv_BandSettings.Columns[1].HeaderText = "K-Factor\n";
            dgv_BandSettings.Columns[1].ToolTipText = "K-Factor\n1 ... 2 x Earth Radius\nDefault = 1.33";
            dgv_BandSettings.Columns[2].Width = 70;
            dgv_BandSettings.Columns[2].HeaderText = "F1-Clearance\n";
            dgv_BandSettings.Columns[2].ToolTipText = "Fresnel Zone Clearance\n0 ... 1 x Fresnel Zone F1\nDefault = 0.6";
            dgv_BandSettings.Columns[3].Width = 105;
            dgv_BandSettings.Columns[3].HeaderText = "Ground Clearance [m]";
            dgv_BandSettings.Columns[3].ToolTipText = "Additional Ground Clearance in m\nDefault = 0";
            dgv_BandSettings.Columns[4].Width = 95;
            dgv_BandSettings.Columns[4].HeaderText = "Max. Distance [km]";
            dgv_BandSettings.Columns[4].ToolTipText = "Max. Distance in km\nan aircraft is considered to be \"on the path\"\nDefault = 10";
            dgv_BandSettings.Columns[5].Width = 80;
            dgv_BandSettings.Columns[5].HeaderText = "Max. Squint [deg]";
            dgv_BandSettings.Columns[5].ToolTipText = "Max. Squint an asymmetric reflection can have";
            dgv_BandSettings.Columns[6].Width = 95;
            dgv_BandSettings.Columns[6].HeaderText = "Max. Elevation [deg]";
            dgv_BandSettings.Columns[6].ToolTipText = "Max. Elevation an aircraft can have to be inside the main lobe of antenna";

            // set initial settings for CoverageMap
            gm_Options_Coverage.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Options_Coverage.IgnoreMarkerOnMouseWheel = true;
            gm_Options_Coverage.MinZoom = 0;
            gm_Options_Coverage.MaxZoom = 20;
            gm_Options_Coverage.Zoom = 6;
            gm_Options_Coverage.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Options_Coverage.CanDragMap = true;
            gm_Options_Coverage.ScalePen = new Pen(Color.Black, 3);
            gm_Options_Coverage.HelperLinePen = null;
            gm_Options_Coverage.SelectionPen = null;
            gm_Options_Coverage.MapScaleInfoEnabled = true;
            gm_Options_Coverage.Overlays.Add(Coveragepolygons);

            // set initial settings for GLOBEMap
            gm_Options_GLOBE.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Options_GLOBE.IgnoreMarkerOnMouseWheel = true;
            gm_Options_GLOBE.MinZoom = 0;
            gm_Options_GLOBE.MaxZoom = 20;
            gm_Options_GLOBE.Zoom = 1;
            gm_Options_GLOBE.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Options_GLOBE.CanDragMap = true;
            gm_Options_GLOBE.ScalePen = new Pen(Color.Black, 3);
            gm_Options_GLOBE.HelperLinePen = null;
            gm_Options_GLOBE.SelectionPen = null;
            gm_Options_GLOBE.MapScaleInfoEnabled = true;
            gm_Options_GLOBE.Overlays.Add(GLOBEpolygons);

            // set initial settings for SRTM3Map
            gm_Options_SRTM3.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Options_SRTM3.IgnoreMarkerOnMouseWheel = true;
            gm_Options_SRTM3.MinZoom = 0;
            gm_Options_SRTM3.MaxZoom = 20;
            gm_Options_SRTM3.Zoom = 6;
            gm_Options_SRTM3.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Options_SRTM3.CanDragMap = true;
            gm_Options_SRTM3.ScalePen = new Pen(Color.Black, 3);
            gm_Options_SRTM3.HelperLinePen = null;
            gm_Options_SRTM3.SelectionPen = null;
            gm_Options_SRTM3.MapScaleInfoEnabled = true;
            gm_Options_SRTM3.Overlays.Add(SRTM3polygons);

            // set initial settings for SRTM1Map
            gm_Options_SRTM1.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Options_SRTM1.IgnoreMarkerOnMouseWheel = true;
            gm_Options_SRTM1.MinZoom = 0;
            gm_Options_SRTM1.MaxZoom = 20;
            gm_Options_SRTM1.Zoom = 6;
            gm_Options_SRTM1.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Options_SRTM1.CanDragMap = true;
            gm_Options_SRTM1.ScalePen = new Pen(Color.Black, 3);
            gm_Options_SRTM1.HelperLinePen = null;
            gm_Options_SRTM1.SelectionPen = null;
            gm_Options_SRTM1.MapScaleInfoEnabled = true;
            gm_Options_SRTM1.Overlays.Add(SRTM1polygons);
            Log.WriteMessage("Finished.");
        }

        #region Helpers
        private bool IsInside(string loc, double lat, double lon)
        {
            string newloc = MaidenheadLocator.LocFromLatLon(lat, lon, false, loc.Length);
            if (loc.ToUpper().Trim() != newloc.ToUpper().Trim())
                return false;
            return true;
        }

        private bool IsPrecise(string loc, double lat, double lon)
        {
            loc = loc.ToUpper();
            double mlat = MaidenheadLocator.LatFromLoc(loc);
            double mlon = MaidenheadLocator.LonFromLoc(loc);
            string mloc = MaidenheadLocator.LocFromLatLon(lat, lon, false, loc.Length);
            if ((loc == mloc) && ((Math.Abs(mlat - lat) > 0.00001) || (mlon - lon) > 0.00001))
            {
                return true;
            }
            return false;
        }

        private void Say(string text)
        {
            if (tsl_Options_Status.Text != text)
            {
                tsl_Options_Status.Text = text;
            }
        }

        private ELEVATIONMODEL GetElevationModel()
        {
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled)
                return ELEVATIONMODEL.SRTM1;
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled)
                return ELEVATIONMODEL.SRTM3;
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled)
                return ELEVATIONMODEL.GLOBE;
            return ELEVATIONMODEL.NONE;
        }

        #endregion

        #region tab_Options_General

        private void tab_Options_General_Enter(object sender, EventArgs e)
        {
            tab_Options_General_Update(this,null);
        }

        private void tab_Options_General_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (tb_Coverage_MaxLat.Value <= tb_Coverage_MinLat.Value)
            {
                MessageBox.Show("MaxLat must be greater than MinLat.", "Parameter Error");
                e.Cancel = true;
            }
            if (tb_Coverage_MaxLon.Value <= tb_Coverage_MinLon.Value)
            {
                MessageBox.Show("MaxLon must be greater than MinLon.", "Parameter Error");
                e.Cancel = true;
            }
            */
        }

        private void tab_Options_General_Update(object sender, EventArgs e)
        {
            Coveragepolygons.Clear();
            // check values
            if (double.IsNaN(tb_Coverage_MinLat.Value) || double.IsNaN(tb_Coverage_MaxLat.Value) || double.IsNaN(tb_Coverage_MinLon.Value) | double.IsNaN(tb_Coverage_MaxLon.Value))
                return;
            // add tile to map polygons
            List<PointLatLng> l = new List<PointLatLng>();
            l.Add(new PointLatLng(tb_Coverage_MinLat.Value, tb_Coverage_MinLon.Value));
            l.Add(new PointLatLng(tb_Coverage_MinLat.Value, tb_Coverage_MaxLon.Value));
            l.Add(new PointLatLng(tb_Coverage_MaxLat.Value, tb_Coverage_MaxLon.Value));
            l.Add(new PointLatLng(tb_Coverage_MaxLat.Value, tb_Coverage_MinLon.Value));
            GMapPolygon p = new GMapPolygon(l, "Coverage");
            p.Stroke = new Pen(Color.FromArgb(255, Color.Magenta), 3);
            p.Fill = new SolidBrush(Color.FromArgb(0, Color.Magenta));
            Coveragepolygons.Polygons.Add(p);
            // zoom the map
            gm_Options_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB(tb_Coverage_MinLon.Value - 1, tb_Coverage_MaxLat.Value + 1, tb_Coverage_MaxLon.Value + 1, tb_Coverage_MinLat.Value - 1));
        }

        #endregion

        #region tab_Options_Database

        private string GetDatabaseDir(string dir)
        {
            if (dir.IndexOf(Path.DirectorySeparatorChar) == 0)
                return Path.GetFileName(dir);
            try
            {
                string databasedir = "..." + Path.DirectorySeparatorChar + dir.Split(Path.DirectorySeparatorChar).Reverse().Take(2).Aggregate((s1, s2) => s2 + Path.DirectorySeparatorChar + s1);
                return databasedir;
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
            return Path.GetFileName(dir);
        }

        private void tab_Options_Database_Enter(object sender, EventArgs e)
        {
            lbl_Options_ScoutBase_Database_FileName.Text = GetDatabaseDir(StationData.Database.GetDBLocation());
            tb_Options_ScoutBase_Database_FileSize.Text = StationData.Database.GetDBSize().ToString("F0");
            lbl_Options_AirScout_Database_FileName.Text = GetDatabaseDir(AircraftData.Database.GetDBLocation());
            tb_Options_AirScout_Database_FileSize.Text = AircraftData.Database.GetDBSize().ToString("F0");
            lbl_Options_Propagation_GLOBE_Database_FileName.Text = GetDatabaseDir(PropagationData.Database.GetDBLocation(ELEVATIONMODEL.GLOBE));
            tb_Options_Propagation_GLOBE_Database_FileSize.Text = PropagationData.Database.GetDBSize(ELEVATIONMODEL.GLOBE).ToString("F0");
            lbl_Options_Propagation_SRTM3_Database_FileName.Text = GetDatabaseDir(PropagationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM3));
            tb_Options_Propagation_SRTM3_Database_FileSize.Text = PropagationData.Database.GetDBSize(ELEVATIONMODEL.SRTM3).ToString("F0");
            lbl_Options_Propagation_SRTM1_Database_FileName.Text = GetDatabaseDir(PropagationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM1));
            tb_Options_Propagation_SRTM1_Database_FileSize.Text = PropagationData.Database.GetDBSize(ELEVATIONMODEL.SRTM1).ToString("F0");
            lbl_Options_Elevation_GLOBE_Database_FileName.Text = GetDatabaseDir(ElevationData.Database.GetDBLocation(ELEVATIONMODEL.GLOBE));
            tb_Options_Elevation_GLOBE_Database_FileSize.Text = ElevationData.Database.GetDBSize(ELEVATIONMODEL.GLOBE).ToString("F0");
            lbl_Options_Elevation_SRTM3_Database_FileName.Text = GetDatabaseDir(ElevationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM3));
            tb_Options_Elevation_SRTM3_Database_FileSize.Text = ElevationData.Database.GetDBSize(ELEVATIONMODEL.SRTM3).ToString("F0");
            lbl_Options_Elevation_SRTM1_Database_FileName.Text = GetDatabaseDir(ElevationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM1));
            tb_Options_Elevation_SRTM1_Database_FileSize.Text = ElevationData.Database.GetDBSize(ELEVATIONMODEL.SRTM1).ToString("F0");
            lbl_Options_Map_Database_FileName.Text = GetDatabaseDir(MapData.Database.GetDBLocation());
            tb_Options_Map_Database_FileSize.Text = MapData.Database.GetDBSize().ToString("F0");
            tt_Options.SetToolTip(lbl_Options_ScoutBase_Database_FileName, StationData.Database.GetDBLocation() + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_AirScout_Database_FileName, AircraftData.Database.GetDBLocation() + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Propagation_GLOBE_Database_FileName, PropagationData.Database.GetDBLocation(ELEVATIONMODEL.GLOBE) + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Propagation_SRTM3_Database_FileName, PropagationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM3) + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Propagation_SRTM1_Database_FileName, PropagationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM1) + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Elevation_GLOBE_Database_FileName, ElevationData.Database.GetDBLocation(ELEVATIONMODEL.GLOBE) + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Elevation_SRTM3_Database_FileName, ElevationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM3) + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Elevation_SRTM1_Database_FileName, ElevationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM1) + "\n\n" + "Click here to open database location in Explorer window.");
            tt_Options.SetToolTip(lbl_Options_Map_Database_FileName, MapData.Database.GetDBLocation() + "\n\n" + "Click here to open database location in Explorer window.");
            double total = StationData.Database.GetDBSize() +
                AircraftData.Database.GetDBSize() +
                PropagationData.Database.GetDBSize(ELEVATIONMODEL.GLOBE) +
                PropagationData.Database.GetDBSize(ELEVATIONMODEL.SRTM3) +
                PropagationData.Database.GetDBSize(ELEVATIONMODEL.SRTM1) +
                ElevationData.Database.GetDBSize(ELEVATIONMODEL.GLOBE) +
                ElevationData.Database.GetDBSize(ELEVATIONMODEL.SRTM3) +
                ElevationData.Database.GetDBSize(ELEVATIONMODEL.SRTM1) + 
                MapData.Database.GetDBSize();
            lbl_Options_Database_TotalSize.Text = total.ToString("F0");
            rb_Options_Database_Update_Never.Checked = !Properties.Settings.Default.Background_Update_OnStartup && !Properties.Settings.Default.Background_Update_Periodically;
            rb_Options_Database_Update_OnStartup.Checked = Properties.Settings.Default.Background_Update_OnStartup;
            rb_Options_Database_Update_Periodically.Checked = Properties.Settings.Default.Background_Update_Periodically;
            ud_Options_Database_Update_Period.Enabled = Properties.Settings.Default.Background_Update_Periodically;
        }

        private void tab_Options_Database_Validating(object sender, CancelEventArgs e)
        {

        }

        private void rb_Options_Database_Update_Never_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Options_Database_Update_Never.Checked)
            {
                ud_Options_Database_Update_Period.Enabled = false;
            }
        }

        private void rb_Options_Database_Update_OnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Options_Database_Update_OnStartup.Checked)
            {
                Properties.Settings.Default.Background_Update_OnStartup = true;
                ud_Options_Database_Update_Period.Enabled = false;
            }
            else
                Properties.Settings.Default.Background_Update_OnStartup = false;
        }

        private void rb_Options_Database_Update_Periodically_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Options_Database_Update_Periodically.Checked)
            {
                Properties.Settings.Default.Background_Update_Periodically = true;
                ud_Options_Database_Update_Period.Enabled = true;
            }
            else
                Properties.Settings.Default.Background_Update_Periodically = false;
        }

        private void btn_Options_DeleteAllElevationPaths_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete all precalculated elevation paths from database?", "Delete all elevation paths", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ElevationData.Database.ElevationPathDeleteAll(ELEVATIONMODEL.GLOBE);
                ElevationData.Database.ElevationPathDeleteAll(ELEVATIONMODEL.SRTM3);
                ElevationData.Database.ElevationPathDeleteAll(ELEVATIONMODEL.SRTM1);
            }
        }

        private void btn_Options_DeleteAllPropagationPaths_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete all precalculated propagation paths from database?", "Delete all propagation paths", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PropagationData.Database.PropagationPathDeleteAll(ELEVATIONMODEL.GLOBE);
                PropagationData.Database.PropagationPathDeleteAll(ELEVATIONMODEL.SRTM3);
                PropagationData.Database.PropagationPathDeleteAll(ELEVATIONMODEL.SRTM1);
            }
        }

        private void btn_DeleteAllMapTiles_Click(object sender, EventArgs e)
        {
            MapData.Database.TileDeleteAll();
        }

        private void btn_DeleteSingleStation_Click(object sender, EventArgs e)
        {
            DeleteSingleStationDlg Dlg = new DeleteSingleStationDlg();
            Dlg.ShowDialog();
        }

        private void btn_Options_ScoutBase_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(StationData.Database);
            Dlg.ShowDialog();
        }

        private void btn_Options_AirScout_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(AircraftData.Database);
            Dlg.ShowDialog();
        }

        private void btn_Options_Propagation_GLOBE_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(PropagationData.Database, ELEVATIONMODEL.GLOBE);
            Dlg.ShowDialog();
        }

        private void btn_Options_Propagation_SRTM3_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(PropagationData.Database, ELEVATIONMODEL.SRTM3);
            Dlg.ShowDialog();
        }

        private void btn_Options_Propagation_SRTM1_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(PropagationData.Database, ELEVATIONMODEL.SRTM1);
            Dlg.ShowDialog();
        }

        private void btn_Options_Elevation_GLOBE_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(ElevationData.Database, ELEVATIONMODEL.GLOBE);
            Dlg.ShowDialog();
        }

        private void btn_Options_Elevation_SRTM3_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(ElevationData.Database, ELEVATIONMODEL.SRTM3);
            Dlg.ShowDialog();
        }

        private void btn_Options_Elevation_SRTM1_Database_Maintenance_Click(object sender, EventArgs e)
        {
            DatabaseMaintenanceDlg Dlg = new DatabaseMaintenanceDlg(ElevationData.Database, ELEVATIONMODEL.SRTM1);
            Dlg.ShowDialog();
        }

        private void lbl_Options_ScoutBase_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(StationData.Database.GetDBLocation()));
        }

        private void lbl_Options_AirScout_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(AircraftData.Database.GetDBLocation()));
        }

        private void lbl_Options_Propagation_GLOBE_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(PropagationData.Database.GetDBLocation(ELEVATIONMODEL.GLOBE)));
        }

        private void lbl_Options_Propagation_SRTM3_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(PropagationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM3)));
        }

        private void lbl_Options_Propagation_SRTM1_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(PropagationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM1)));
        }

        private void lbl_Options_Elevation_GLOBE_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(ElevationData.Database.GetDBLocation(ELEVATIONMODEL.GLOBE)));
        }

        private void lbl_Options_Elevation_SRTM3_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(ElevationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM3)));
        }

        private void lbl_Options_Elevation_SRTM1_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(ElevationData.Database.GetDBLocation(ELEVATIONMODEL.SRTM1)));
        }

        private void lbl_Options_Map_Database_FileName_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(MapData.Database.GetDBLocation()));
        }

        #endregion

        #region tab_Options_Stations

        private void tab_Options_Stations_Enter(object sender, EventArgs e)
        {
            // initially set textboxes
            tb_Options_MyCall.SilentText = Properties.Settings.Default.MyCall;
            tb_Options_MyLat.SilentValue = (double)Properties.Settings.Default.MyLat;
            tb_Options_MyLon.SilentValue = (double)Properties.Settings.Default.MyLon;
            MyLocator_Set(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon);
            tb_Options_MyElevation.Text = ParentDlg.GetElevation(tb_Options_MyLat.Value, tb_Options_MyLon.Value).ToString();
            tb_Options_DXCall.SilentText = Properties.Settings.Default.DXCall;
            tb_Options_DXLat.SilentValue = Properties.Settings.Default.DXLat;
            tb_Options_DXLon.SilentValue = Properties.Settings.Default.DXLon;
            DXLocator_Set(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon);
            tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString();
            tb_Options_Band.Text = Bands.GetStringValue(Properties.Settings.Default.Band);
            MyQRV_Load();
            DXQRV_Load();
            MyLocation_Check();
            DXLocation_Check();
        }

        private void MyLocator_Set(double lat, double lon)
        {
            if (GeographicalPoint.Check(tb_Options_MyLat.Value, tb_Options_MyLon.Value))
            {
                // colour Textbox if more precise lat/lon information is available
                if (MaidenheadLocator.IsPrecise(tb_Options_MyLat.Value, tb_Options_MyLon.Value, 3))
                {
                    tb_Options_MyLoc.SilentText = MaidenheadLocator.LocFromLatLon(lat, lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2);
                    if (tb_Options_MyLoc.BackColor != Color.PaleGreen)
                        tb_Options_MyLoc.BackColor = Color.PaleGreen;
                }
                else
                {
                    tb_Options_MyLoc.SilentText = MaidenheadLocator.LocFromLatLon(lat, lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, 3);
                    if (tb_Options_MyLoc.BackColor != Color.FloralWhite)
                        tb_Options_MyLoc.BackColor = Color.FloralWhite;
                }

            }
            else
            {
                tb_Options_MyLoc.SilentText = "";
                if (tb_Options_MyLoc.BackColor != Color.FloralWhite)
                    tb_Options_MyLoc.BackColor = Color.FloralWhite;
            }
        }

        private void tb_Options_MyCall_TextChanged(object sender, EventArgs e)
        {
            // try to find callsign in database
            LocationDesignator ld = StationData.Database.LocationFind(tb_Options_MyCall.Text);
            if (ld != null)
            {
                // update location info
                tb_Options_MyLat.SilentValue = ld.Lat;
                tb_Options_MyLon.SilentValue = ld.Lon;
                MyLocator_Set(tb_Options_MyLat.Value, tb_Options_MyLon.Value);
                tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
                // update QRV info
                MyQRV_Load();
                return;
            }
            else
            {
                MyLocation_Clear();
                MyQRV_Clear();
            }
            MyLocation_Check();
        }

        private void tb_Options_MyLat_TextChanged(object sender, EventArgs e)
        {
            MyLocator_Set(tb_Options_MyLat.Value, tb_Options_MyLon.Value);
            tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
        }

        private void tb_Options_MyLon_TextChanged(object sender, EventArgs e)
        {
            MyLocator_Set(tb_Options_MyLat.Value, tb_Options_MyLon.Value);
            tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
        }

        private void tb_Options_MyLoc_TextChanged(object sender, EventArgs e)
        {
            // update lat/lon
            double mlat, mlon;
            if (tb_Options_MyLoc.Focused)
            {
                // locator box is focused --> update lat/lon
                if (MaidenheadLocator.Check(tb_Options_MyLoc.Text) && tb_Options_MyLoc.Text.Length >= 6)
                {
                    // colour Textbox if more precise lat/lon information is available
                    if (MaidenheadLocator.IsPrecise(tb_Options_DXLat.Value, tb_Options_DXLon.Value, 3))
                    {
                        if (tb_Options_DXLoc.BackColor != Color.PaleGreen)
                            tb_Options_DXLoc.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (tb_Options_DXLoc.BackColor != Color.FloralWhite)
                            tb_Options_DXLoc.BackColor = Color.FloralWhite;
                    }
                    MaidenheadLocator.LatLonFromLoc(tb_Options_MyLoc.Text, PositionInRectangle.MiddleMiddle, out mlat, out mlon);
                    tb_Options_MyLat.SilentValue = mlat;
                    tb_Options_MyLon.SilentValue = mlon;
                    tb_Options_MyElevation.Text = ParentDlg.GetElevation(tb_Options_MyLat.Value, tb_Options_MyLon.Value).ToString("F0");
                    // update QRV info
                    MyQRV_Load();
                }
                else
                {
                    tb_Options_MyLat.SilentValue = double.NaN;
                    tb_Options_MyLon.SilentValue = double.NaN;
                    MyQRV_Clear();
                }
            }
        }


        private void btn_MyCall_QRZ_Click(object sender, EventArgs e)
        {
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
                        lat = System.Convert.ToDouble(s.Substring(0, s.IndexOf("\n") - 2), ENprovider);
                        s = s.Remove(0, s.IndexOf("cs_lon = \"") + 10);
                        lon = System.Convert.ToDouble(s.Substring(0, s.IndexOf("\n") - 2), ENprovider);
                    }
                    catch
                    {
                    }
                    loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                    // check if loc is matching --> refine lat/lon
                    if ((tb_Options_MyLoc.Text.Length >= 6) && (loc == MaidenheadLocator.Convert(tb_Options_MyLoc.Text, false).Substring(0, 6)))
                    {
                        Properties.Settings.Default["MyLat"] = lat;
                        Properties.Settings.Default.MyLon = lon;
                        tb_Options_MyLat.Text = Properties.Settings.Default.MyLat.ToString("F8", ENprovider);
                        tb_Options_MyLon.Text = Properties.Settings.Default.MyLon.ToString("F8", ENprovider);
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

        private void btn_Options_MyHorizon_Click(object sender, EventArgs e)
        {
            if (!MyLocation_Check())
                return;
            HorizonDlg Dlg = new HorizonDlg(tb_Options_MyCall.Text,
                tb_Options_MyLat.Value,
                tb_Options_MyLon.Value,
                LocalObstructions);
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // do nothing
            }
        }

        private void DXLocator_Set(double lat, double lon)
        {
            if (GeographicalPoint.Check(tb_Options_DXLat.Value, tb_Options_DXLon.Value))
            {
                // colour Textbox if more precise lat/lon information is available
                if (MaidenheadLocator.IsPrecise(tb_Options_DXLat.Value, tb_Options_DXLon.Value, 3))
                {
                    tb_Options_DXLoc.SilentText = MaidenheadLocator.LocFromLatLon(lat, lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2);
                    if (tb_Options_DXLoc.BackColor != Color.PaleGreen)
                        tb_Options_DXLoc.BackColor = Color.PaleGreen;
                }
                else
                {
                    tb_Options_DXLoc.SilentText = MaidenheadLocator.LocFromLatLon(lat, lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, 3);
                    if (tb_Options_DXLoc.BackColor != Color.FloralWhite)
                        tb_Options_DXLoc.BackColor = Color.FloralWhite;
                }

            }
            else
            {
                tb_Options_DXLoc.SilentText = "";
                if (tb_Options_DXLoc.BackColor != Color.FloralWhite)
                    tb_Options_DXLoc.BackColor = Color.FloralWhite;
            }
        }

        private void tb_Options_DXCall_TextChanged(object sender, EventArgs e)
        {
            // try to find callsign in database
            LocationDesignator ld = StationData.Database.LocationFind(tb_Options_DXCall.Text);
            if (ld != null)
            {
                // update location info
                tb_Options_DXLat.SilentValue = ld.Lat;
                tb_Options_DXLon.SilentValue = ld.Lon;
                DXLocator_Set(tb_Options_DXLat.Value, tb_Options_DXLon.Value);
                tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
                // update QRV info
                DXQRV_Load();
                return;
            }
            else
            {
                DXLocation_Clear();
                DXQRV_Clear();
            }
            DXLocation_Check();
        }

        private void tb_Options_DXLat_TextChanged(object sender, EventArgs e)
        {
            DXLocator_Set(tb_Options_DXLat.Value, tb_Options_DXLon.Value);
            tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
        }

        private void tb_Options_DXLon_TextChanged(object sender, EventArgs e)
        {
            DXLocator_Set(tb_Options_DXLat.Value, tb_Options_DXLon.Value);
            tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
        }

        private void tb_Options_DXLoc_TextChanged(object sender, EventArgs e)
        {
            // update lat/lon
            double mlat, mlon;
            if (tb_Options_DXLoc.Focused)
            {
                // locator box is focused --> update lat/lon
                if (MaidenheadLocator.Check(tb_Options_DXLoc.Text) && tb_Options_DXLoc.Text.Length >= 6)
                {
                    // colour Textbox if more precise lat/lon information is available
                    if (MaidenheadLocator.IsPrecise(tb_Options_DXLat.Value, tb_Options_DXLon.Value, 3))
                    {
                        if (tb_Options_DXLoc.BackColor != Color.PaleGreen)
                            tb_Options_DXLoc.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (tb_Options_DXLoc.BackColor != Color.FloralWhite)
                            tb_Options_DXLoc.BackColor = Color.FloralWhite;
                    }
                    MaidenheadLocator.LatLonFromLoc(tb_Options_DXLoc.Text, PositionInRectangle.MiddleMiddle, out mlat, out mlon);
                    tb_Options_DXLat.SilentValue = mlat;
                    tb_Options_DXLon.SilentValue = mlon;
                    tb_Options_DXElevation.Text = ParentDlg.GetElevation(tb_Options_DXLat.Value, tb_Options_DXLon.Value).ToString("F0");
                    // update QRV info
                    DXQRV_Load();
                }
                else
                {
                    tb_Options_DXLat.SilentValue = double.NaN;
                    tb_Options_DXLon.SilentValue = double.NaN;
                    DXQRV_Clear();
                }
            }
        }


        private void btn_DXCall_QRZ_Click(object sender, EventArgs e)
        {
            try
            {
                WebRequest DXWebRequest = WebRequest.Create(Properties.Settings.Default.QRZ_URL_Database + Properties.Settings.Default.DXCall);
                DXWebRequest.Timeout = 10000;
                WebResponse DXWebResponse = DXWebRequest.GetResponse();
                Stream ReceiveStream = DXWebResponse.GetResponseStream();
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
                        lat = System.Convert.ToDouble(s.Substring(0, s.IndexOf("\n") - 2), ENprovider);
                        s = s.Remove(0, s.IndexOf("cs_lon = \"") + 10);
                        lon = System.Convert.ToDouble(s.Substring(0, s.IndexOf("\n") - 2), ENprovider);
                    }
                    catch
                    {
                    }
                    loc = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                    // check if loc is matching --> refine lat/lon
                    if ((tb_Options_DXLoc.Text.Length >= 6) && (loc == MaidenheadLocator.Convert(tb_Options_DXLoc.Text, false).Substring(0, 6)))
                    {
                        Properties.Settings.Default["DXLat"] = lat;
                        Properties.Settings.Default.DXLon = lon;
                        tb_Options_DXLat.Text = Properties.Settings.Default.DXLat.ToString("F8", ENprovider);
                        tb_Options_DXLon.Text = Properties.Settings.Default.DXLon.ToString("F8", ENprovider);
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

        private void btn_Options_DXHorizon_Click(object sender, EventArgs e)
        {
            if (!DXLocation_Check())
                return;
            HorizonDlg Dlg = new HorizonDlg(tb_Options_DXCall.Text,
                tb_Options_DXLat.Value,
                tb_Options_DXLon.Value,
                null);
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // do nothing
            }
        }

        private void cb_Options_SmallLettersForSubSquares_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Locator_SmallLettersForSubsquares = cb_Options_SmallLettersForSubSquares.Checked;
            // refresh layout
            tab_Options_Stations_Enter(this, null);
        }

        private void tab_Options_Stations_Validating(object sender, CancelEventArgs e)
        {
            // validates station details
            // enables leaving tab
            if (!MyLocation_Check() || !MyQRV_Check() || !DXLocation_Check() || !DXQRV_Check())
                e.Cancel = true;
        }

        private void btn_Options_BandDown_Click(object sender, EventArgs e)
        {
            if (MyQRV_Check())
                MyQRV_Save();
            if (DXQRV_Check())
                DXQRV_Save();
            if (MyLocation_Check() && DXLocation_Check())
            {
                Properties.Settings.Default.Band = Bands.Previous(Properties.Settings.Default.Band);
                tb_Options_Band.Text = Bands.GetStringValue(Properties.Settings.Default.Band);
                MyQRV_Load();
                DXQRV_Load();
            }
        }

        private void btn_Options_BandUp_Click(object sender, EventArgs e)
        {
            if (MyQRV_Check())
                MyQRV_Save();
            if (DXQRV_Check())
                DXQRV_Save();
            if (MyLocation_Check() && DXLocation_Check())
            {
                Properties.Settings.Default.Band = Bands.Next(Properties.Settings.Default.Band);
                tb_Options_Band.Text = Bands.GetStringValue(Properties.Settings.Default.Band);
                MyQRV_Load();
                DXQRV_Load();
            }
        }

        private void MyLocation_Clear()
        {
            tb_Options_MyLoc.SilentText = "";
            tb_Options_MyLat.SilentText = "";
            tb_Options_MyLon.SilentText = "";
            tb_Options_MyElevation.Text = "";
            if (tb_Options_MyLoc.BackColor != Color.FloralWhite)
                tb_Options_MyLoc.BackColor = Color.FloralWhite;
        }

        private void DXLocation_Clear()
        {
            tb_Options_DXLoc.SilentText = "";
            tb_Options_DXLat.SilentText = "";
            tb_Options_DXLon.SilentText = "";
            tb_Options_DXElevation.Text = "";
            if (tb_Options_DXLoc.BackColor != Color.FloralWhite)
                tb_Options_DXLoc.BackColor = Color.FloralWhite;
        }

        private bool MyLocation_Check()
        {
            // check all values
            if (Callsign.Check(tb_Options_MyCall.Text) && MaidenheadLocator.Check(tb_Options_MyLoc.Text) && GeographicalPoint.Check(tb_Options_MyLat.Value, tb_Options_MyLon.Value) && MyQRV_Check())
            {
                // check for local obstructions
                LocalObstructions = ElevationData.Database.LocalObstructionFind(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.ElevationModel);
                if (LocalObstructions != null)
                    lbl_Options_LocalObstructions.Text = "Local Obstruction found for this location.";
                else
                    lbl_Options_LocalObstructions.Text = "Local Obstruction not found for this location.";
                return true;
            }
            return false;
        }

        private bool DXLocation_Check()
        {
            // check all values
            if (Callsign.Check(tb_Options_DXCall.Text) && MaidenheadLocator.Check(tb_Options_DXLoc.Text) && GeographicalPoint.Check(tb_Options_DXLat.Value, tb_Options_DXLon.Value) && DXQRV_Check())
            {
                return true;
            }
            return false;
        }

        private void MyLocation_Save()
        {
            // save location to settings && database
            if (MyLocation_Check())
            {
                Properties.Settings.Default.MyCall = tb_Options_MyCall.Text;
                Properties.Settings.Default.MyLat = tb_Options_MyLat.Value;
                Properties.Settings.Default.MyLon = tb_Options_MyLon.Value;
                // find entry in database
                LocationDesignator ld = StationData.Database.LocationFind(tb_Options_MyCall.Text, MaidenheadLocator.LocFromLatLon(tb_Options_MyLat.Value, tb_Options_MyLon.Value, false, 3));
                // update if not found or different values
                if ((ld == null) || (ld.Lat != tb_Options_MyLat.Value) || (ld.Lon != tb_Options_MyLon.Value))
                    StationData.Database.LocationInsertOrUpdateIfNewer(new LocationDesignator(tb_Options_MyCall.Text, tb_Options_MyLat.Value, tb_Options_MyLon.Value, (MaidenheadLocator.IsPrecise(tb_Options_MyLat.Value, tb_Options_MyLon.Value, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC)));
            }
        }

        private void DXLocation_Save()
        {
            // save location to settings && database
            if (DXLocation_Check())
            {
                Properties.Settings.Default.DXCall = tb_Options_DXCall.Text;
                Properties.Settings.Default.DXLat = tb_Options_DXLat.Value;
                Properties.Settings.Default.DXLon = tb_Options_DXLon.Value;
                // find entry in database
                LocationDesignator ld = StationData.Database.LocationFind(tb_Options_DXCall.Text, MaidenheadLocator.LocFromLatLon(tb_Options_DXLat.Value, tb_Options_DXLon.Value, false, 3));
                // update if not found or different values
                if ((ld == null) || (ld.Lat != tb_Options_DXLat.Value) || (ld.Lon != tb_Options_DXLon.Value))
                    StationData.Database.LocationInsertOrUpdateIfNewer(new LocationDesignator(tb_Options_DXCall.Text, tb_Options_DXLat.Value, tb_Options_DXLon.Value, (MaidenheadLocator.IsPrecise(tb_Options_DXLat.Value, tb_Options_DXLon.Value, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC)));
            }
        }

        private void btn_Options_MyUpdate_Click(object sender, EventArgs e)
        {
            if (MyLocation_Check() && MyQRV_Check())
            {
                StationDataUpdaterDoWorkEventArgs args = new StationDataUpdaterDoWorkEventArgs();
                args.ld = StationData.Database.LocationFind(tb_Options_MyCall.Text, MaidenheadLocator.LocFromLatLon(tb_Options_MyLat.Value, tb_Options_MyLon.Value, false, 3));
                args.qrvs = StationData.Database.QRVFind(tb_Options_MyCall.Text, MaidenheadLocator.LocFromLatLon(tb_Options_MyLat.Value, tb_Options_MyLon.Value, false, 3));
                bw_StationDataUpdater.RunWorkerAsync(args);
            }
        }

        private void btn_Options_DXUpdate_Click(object sender, EventArgs e)
        {
            if (DXLocation_Check() && DXQRV_Check())
            {
                StationDataUpdaterDoWorkEventArgs args = new StationDataUpdaterDoWorkEventArgs();
                args.ld = StationData.Database.LocationFind(tb_Options_DXCall.Text, MaidenheadLocator.LocFromLatLon(tb_Options_DXLat.Value, tb_Options_DXLon.Value, false, 3));
                args.qrvs = StationData.Database.QRVFind(tb_Options_DXCall.Text, MaidenheadLocator.LocFromLatLon(tb_Options_DXLat.Value, tb_Options_DXLon.Value, false, 3));
                bw_StationDataUpdater.RunWorkerAsync(args);
            }
        }

        private bool MyQRV_Check()
        {
            if (double.IsNaN(tb_Options_MyAntennaHeight.Value) & double.IsNaN(tb_Options_MyAntennaGain.Value) & double.IsNaN(tb_Options_MyPower.Value))
                return true;
            if (!double.IsNaN(tb_Options_MyAntennaHeight.Value) & !double.IsNaN(tb_Options_MyAntennaGain.Value) & !double.IsNaN(tb_Options_MyPower.Value))
                return true;
            MessageBox.Show("The QRV information you entered is incorrect.\n The fields \"Antenna Heigth\", \"Antenna Gain\" and \"Power\" must either be all filled or all left empty.\nPlease use one of the following options:\n\n<>0   : station is QRV and value is known\n=0   : Station is QRV and value is not konwn\nEmpty: Station is not QRV", "QRV Information incorrect");
            return false;
        }

        private bool DXQRV_Check()
        {
            if (double.IsNaN(tb_Options_DXAntennaHeight.Value) & double.IsNaN(tb_Options_DXAntennaGain.Value) & double.IsNaN(tb_Options_DXPower.Value))
                return true;
            if (!double.IsNaN(tb_Options_DXAntennaHeight.Value) & !double.IsNaN(tb_Options_DXAntennaGain.Value) & !double.IsNaN(tb_Options_DXPower.Value))
                return true;
            MessageBox.Show("The QRV information you entered is incorrect.\n The fields \"Antenna Heigth\", \"Antenna Gain\" and \"Power\" must either be all filled or all left empty.\nPlease use one of the following options:\n\n<>0   : station is QRV and value is known\n=0   : Station is QRV and value is not konwn\nEmpty: Station is not QRV", "QRV Information incorrect");
            return false;
        }

        private void MyQRV_Clear()
        {
            tb_Options_MyAntennaHeight.Text = "";
            tb_Options_MyAntennaGain.Text = "";
            tb_Options_MyPower.Text = "";
            lbl_Options_MyLastUpdated.Text = "";
        }

        private void DXQRV_Clear()
        {
            tb_Options_DXAntennaHeight.Text = "";
            tb_Options_DXAntennaGain.Text = "";
            tb_Options_DXPower.Text = "";
            lbl_Options_DXLastUpdated.Text = "";
        }

        private void MyQRV_Load()
        {
            if (!Callsign.Check(tb_Options_MyCall.Text) || !MaidenheadLocator.Check(tb_Options_MyLoc.Text))
                return;
            if (tb_Options_MyLoc.Text.Length < 6)
                return;
            QRVDesignator qrv = StationData.Database.QRVFind(tb_Options_MyCall.Text, tb_Options_MyLoc.Text, Properties.Settings.Default.Band);
            if (qrv != null)
            {
                tb_Options_MyAntennaHeight.Value = qrv.AntennaHeight;
                tb_Options_MyAntennaGain.Value = qrv.AntennaGain;
                tb_Options_MyPower.Value = qrv.Power;
                lbl_Options_MyLastUpdated.Text = qrv.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                tb_Options_MyAntennaHeight.Value = double.NaN;
                tb_Options_MyAntennaGain.Value = double.NaN;
                tb_Options_MyPower.Value = double.NaN;
                lbl_Options_MyLastUpdated.Text = "";
            }
        }

        private void DXQRV_Load()
        {
            if (!Callsign.Check(tb_Options_DXCall.Text) || !MaidenheadLocator.Check(tb_Options_DXLoc.Text))
                return;
            if (tb_Options_DXLoc.Text.Length < 6)
                return;
            QRVDesignator qrv = StationData.Database.QRVFind(tb_Options_DXCall.Text, tb_Options_DXLoc.Text, Properties.Settings.Default.Band);
            if (qrv != null)
            {
                tb_Options_DXAntennaHeight.Value = qrv.AntennaHeight;
                tb_Options_DXAntennaGain.Value = qrv.AntennaGain;
                tb_Options_DXPower.Value = qrv.Power;
                lbl_Options_DXLastUpdated.Text = qrv.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                tb_Options_DXAntennaHeight.Value = double.NaN;
                tb_Options_DXAntennaGain.Value = double.NaN;
                tb_Options_DXPower.Value = double.NaN;
                lbl_Options_DXLastUpdated.Text = "";
            }
        }

        private void MyQRV_Save()
        {
            if (MyQRV_Check())
            {
                QRVDesignator qrv = StationData.Database.QRVFind(tb_Options_MyCall.Text, tb_Options_MyLoc.Text, Properties.Settings.Default.Band);
                if (double.IsNaN(tb_Options_MyAntennaHeight.Value) && double.IsNaN(tb_Options_MyAntennaGain.Value) && double.IsNaN(tb_Options_MyPower.Value))
                {
                    if (qrv != null)
                        StationData.Database.QRVDelete(qrv);
                }
                else
                {
                    if ((qrv == null) || (qrv.AntennaHeight != tb_Options_MyAntennaHeight.Value) || (qrv.AntennaGain != tb_Options_MyAntennaGain.Value) || (qrv.Power != tb_Options_MyPower.Value))
                    {
                        qrv = new QRVDesignator(tb_Options_MyCall.Text, tb_Options_MyLoc.Text, Properties.Settings.Default.Band, tb_Options_MyAntennaHeight.Value, tb_Options_MyAntennaGain.Value, tb_Options_MyPower.Value);
                        StationData.Database.QRVInsertOrUpdateIfNewer(qrv);
                    }
                }
            }
        }

        private void DXQRV_Save()
        {
            if (DXQRV_Check())
            {
                QRVDesignator qrv = StationData.Database.QRVFind(tb_Options_DXCall.Text, tb_Options_DXLoc.Text, Properties.Settings.Default.Band);
                if (double.IsNaN(tb_Options_DXAntennaHeight.Value) && double.IsNaN(tb_Options_DXAntennaGain.Value) && double.IsNaN(tb_Options_DXPower.Value))
                {
                    if (qrv != null)
                        StationData.Database.QRVDelete(qrv);
                }
                else
                {
                    if ((qrv == null) || (qrv.AntennaHeight != tb_Options_DXAntennaHeight.Value) || (qrv.AntennaGain != tb_Options_DXAntennaGain.Value) || (qrv.Power != tb_Options_DXPower.Value))
                    {
                        qrv = new QRVDesignator(tb_Options_DXCall.Text, tb_Options_DXLoc.Text, Properties.Settings.Default.Band, tb_Options_DXAntennaHeight.Value, tb_Options_DXAntennaGain.Value, tb_Options_DXPower.Value);
                        StationData.Database.QRVInsertOrUpdateIfNewer(qrv);
                    }
                }
            }
        }

        private void btn_Options_MyMap_Click(object sender, EventArgs e)
        {
            if (Callsign.Check(tb_Options_MyCall.Text) && MaidenheadLocator.Check(tb_Options_MyLoc.Text))
            {
                LocationDesignator ld = new LocationDesignator(tb_Options_MyCall.Text, tb_Options_MyLoc.Text);
                MapStationDlg Dlg = new MapStationDlg(ld);
                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    StationData.Database.LocationInsertOrUpdateIfNewer(Dlg.StationLocation);
                    tb_Options_MyLat.SilentValue = Dlg.StationLocation.Lat;
                    tb_Options_MyLon.SilentValue = Dlg.StationLocation.Lon;
                    MyLocator_Set(tb_Options_MyLat.Value, tb_Options_MyLon.Value);
                }
            }
        }

        private void btn_Options_DXMap_Click(object sender, EventArgs e)
        {
            if (Callsign.Check(tb_Options_DXCall.Text) && MaidenheadLocator.Check(tb_Options_DXLoc.Text))
            {
                LocationDesignator ld = new LocationDesignator(tb_Options_DXCall.Text, tb_Options_DXLoc.Text);
                MapStationDlg Dlg = new MapStationDlg(ld);
                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    StationData.Database.LocationInsertOrUpdateIfNewer(Dlg.StationLocation);
                    tb_Options_DXLat.SilentValue = Dlg.StationLocation.Lat;
                    tb_Options_DXLon.SilentValue = Dlg.StationLocation.Lon;
                    DXLocator_Set(tb_Options_DXLat.Value, tb_Options_DXLon.Value);
                }
            }
        }

        private void btn_Options_LocalObstructions_Click(object sender, EventArgs e)
        {
            try
            {
                LocalObstructions = ElevationData.Database.LocalObstructionFindOrCreateDefault(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Properties.Settings.Default.ElevationModel);
                LocalObstructionsDlg Dlg = new LocalObstructionsDlg(LocalObstructions);
                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    // create new local obstructions object and save it to database
                    LocalObstructions = new LocalObstructionDesignator(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, Dlg.LocalObstructions);
                    ElevationData.Database.LocalObstructionInsertOrUpdateIfNewer(LocalObstructions, Properties.Settings.Default.ElevationModel);
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void tab_Options_Stations_Leave(object sender, EventArgs e)
        {
            MyLocation_Save();
            MyQRV_Save();
            DXLocation_Save();
            DXQRV_Save();
        }


        #endregion

        #region tab_Options_Map

        private void tab_Options_Map_Enter(object sender, EventArgs e)
        {
            // populate the list of providers depending on user acception
            cb_Options_Map_Provider.DataSource = null;
            cb_Options_Map_Provider.Items.Clear();
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Map_Provider_Accepted))
            {
                // get a full list of all map providers
                cb_Options_Map_Provider.ValueMember = "Name";
                cb_Options_Map_Provider.DataSource = GMapProviders.List;
            }
            else
            {
                cb_Options_Map_Provider.Items.Add(GMapProviders.Find("None"));
                cb_Options_Map_Provider.Items.Add(GMapProviders.Find("OpenStreetMap"));
            }
            cb_Options_Map_Provider.SelectedItem = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
        }

        private void tab_Options_Map_Validating(object sender, CancelEventArgs e)
        {
            // save map provider
            if (cb_Options_Map_Provider.SelectedItem != null)
            {
                Properties.Settings.Default.Map_Provider = cb_Options_Map_Provider.SelectedItem.ToString();
            }
        }

        private void cb_Options_Map_Provider_DropDown(object sender, EventArgs e)
        {
            // show the MapProviderAcceptDlg on first run
            if (String.IsNullOrEmpty(Properties.Settings.Default.Map_Provider_Accepted))
            {
                MapProviderDlg Dlg = new MapProviderDlg();
                if (Dlg.ShowDialog() == DialogResult.OK)
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
                    Properties.Settings.Default.Map_Provider_Accepted = ID;
                    Properties.Settings.Default.Map_Provider_Accepted = ID;
                    // get a full list of all map providers
                    cb_Options_Map_Provider.DataSource = null;
                    cb_Options_Map_Provider.Items.Clear();
                    cb_Options_Map_Provider.ValueMember = "Name";
                    cb_Options_Map_Provider.DataSource = GMapProviders.List;
                }
                else
                {
                    Properties.Settings.Default.Map_Provider_Accepted = "";
                    Properties.Settings.Default.Map_Provider_Accepted = "";
                }
            }
        }

        private void btn_Options_SelectFont_Click(object sender, EventArgs e)
        {
            try
            {
                FontDialog Dlg = new FontDialog();
                string[] a = Properties.Settings.Default.Map_ToolTipFont.Split(';');
                string fontfamily = a[0].Trim();
                float emsize = 0;
                float.TryParse(a[1].Trim(), NumberStyles.Float, ENprovider, out emsize);
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
                Dlg.Font = new Font(fontfamily, emsize, fontstyle, GraphicsUnit.Point);
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string font = Dlg.Font.FontFamily.Name + ";" + Dlg.Font.Size.ToString(ENprovider);
                    string style = Dlg.Font.Style.ToString();
                    if (style != "Regular")
                        font = font + ";style=" + style;
                    tb_Options_Map_ToolTipFont.Text = font;
                    Properties.Settings.Default.Map_ToolTipFont = font;
                }
            }
            catch
            {
                Properties.Settings.Default.Map_ToolTipFont = "Microsoft Sans Serif;14;style=Bold";
            }
        }


        #endregion

        #region tab_Options_GLOBE

        private void bw_GLOBE_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_GLOBE_MapUpdater.ReportProgress(0, "GLOBE: Creating elevation tile catalogue...");
            // get all locs needed for covered area
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_GLOBE_MapUpdater, ELEVATIONMODEL.GLOBE, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_GLOBE_MapUpdater.ReportProgress(0, "GLOBE: Processing tiles...");
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
                    bw_GLOBE_MapUpdater.ReportProgress(0, "GLOBE: Processing cancelled...");
                    return;
                }
            }
            bw_GLOBE_MapUpdater.ReportProgress(0, "GLOBE: " + found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_GLOBE_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
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
                Say((string)e.UserState);
            }
        }

        private void bw_GLOBE_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void tab_Options_GLOBE_Enter(object sender, EventArgs e)
        {
            // clear map polygons
            GLOBEpolygons.Clear();
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
            gm_Options_GLOBE.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_GLOBE_MapUpdater.IsBusy)
                bw_GLOBE_MapUpdater.RunWorkerAsync();
            // zoom the map
            gm_Options_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void tab_Options_GLOBE_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_GLOBE_MapUpdater.CancelAsync();
            // clear map polygons
            GLOBEpolygons.Clear();
            // do garbage collection
            GC.Collect();
            Say("");
        }

        private void btn_Options_GLOBE_Copyright_Click(object sender, EventArgs e)
        {
            ElevationCopyrightDlg Dlg = new ElevationCopyrightDlg();
            Dlg.Text = "GLOBE Copyright Information";
            Dlg.rtb_Copyright.Text = Properties.Settings.Default.Elevation_GLOBE_Copyright;
            Dlg.ShowDialog();
        }

        #endregion

        #region tab_Options_SRTM3

        private void bw_SRTM3_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_SRTM3_MapUpdater.ReportProgress(0, "SRTM3: Creating elevation tile catalogue...");
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_SRTM3_MapUpdater, ELEVATIONMODEL.SRTM3, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_SRTM3_MapUpdater.ReportProgress(0, "SRTM3: Processing tiles...");
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
                    bw_SRTM3_MapUpdater.ReportProgress(0, "SRTM3: Processing cancelled...");
                    return;
                }
            }
            bw_SRTM3_MapUpdater.ReportProgress(0, "SRTM3: " + found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_SRTM3_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
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
                Say((string)e.UserState);
            }
        }

        private void bw_SRTM3_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void tab_Options_SRTM3_Enter(object sender, EventArgs e)
        {
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
            gm_Options_SRTM3.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_SRTM3_MapUpdater.IsBusy)
                bw_SRTM3_MapUpdater.RunWorkerAsync();
            // zoom the map
            gm_Options_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void tab_Options_SRTM3_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_SRTM3_MapUpdater.CancelAsync();
            // clear map polygons
            SRTM3polygons.Clear();
            // do garbage collection
            GC.Collect();
            Say("");
        }

        private void btn_Options_SRTM3_Copyright_Click(object sender, EventArgs e)
        {
            ElevationCopyrightDlg Dlg = new ElevationCopyrightDlg();
            Dlg.Text = "SRTM3 Copyright Information";
            Dlg.rtb_Copyright.Text = Properties.Settings.Default.Elevation_SRTM3_Copyright;
            Dlg.ShowDialog();
        }

        #endregion

        #region tab_Options_SRTM1

        private void bw_SRTM1_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_SRTM1_MapUpdater.ReportProgress(0, "SRTM1: Creating elevation tile catalogue...");
            ElevationCatalogue availabletiles = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(bw_SRTM1_MapUpdater, ELEVATIONMODEL.SRTM1, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            bw_SRTM1_MapUpdater.ReportProgress(0, "SRTM1: Processing tiles...");
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
                    bw_SRTM1_MapUpdater.ReportProgress(0, "SRTM1: Processing cancelled...");
                    return;
                }
            }
            bw_SRTM1_MapUpdater.ReportProgress(0, "SRTM1: " + found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_SRTM1_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
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
                Say((string)e.UserState);
            }
        }

        private void bw_SRTM1_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void tab_Options_SRTM1_Enter(object sender, EventArgs e)
        {
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
            gm_Options_SRTM1.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon, Properties.Settings.Default.MinLat));
            // start map updater
            if (!bw_SRTM1_MapUpdater.IsBusy)
                bw_SRTM1_MapUpdater.RunWorkerAsync();
            // zoom the map
            gm_Options_Coverage.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void tab_Options_SRTM1_Leave(object sender, EventArgs e)
        {
            // stop map updater
            bw_SRTM1_MapUpdater.CancelAsync();
            // clear map polygons
            SRTM1polygons.Clear();
            // do garbage collection
            GC.Collect();
            Say("");
        }

        private void btn_Options_SRTM1_Copyright_Click(object sender, EventArgs e)
        {
            ElevationCopyrightDlg Dlg = new ElevationCopyrightDlg();
            Dlg.Text = "SRTM1 Copyright Information";
            Dlg.rtb_Copyright.Text = Properties.Settings.Default.Elevation_SRTM1_Copyright;
            Dlg.ShowDialog();
        }

        #endregion

        #region tab_Options_Path

        private void tab_Options_Path_Enter(object sender, EventArgs e)
        {
            dgv_BandSettings.DefaultCellStyle.Font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Regular);
            tb_Options_Path_StepWidth.Text = ElevationData.Database.GetDefaultStepWidth(GetElevationModel()).ToString("F0");
        }

        private void tab_Options_Path_Validating(object sender, CancelEventArgs e)
        {
            // include range checking here!
        }


        private void tab_Options_Path_Leave(object sender, EventArgs e)
        {
            // validate settings

        }

        public short GetElevation(double lat, double lon)
        {
            if (!GeographicalPoint.Check(lat, lon))
                return 0;
            short elv = ElevationData.Database.ElvMissingFlag;
            // try to get elevation data from distinct elevation model
            // start with detailed one
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

        private void btn_Options_Path_Export_Click(object sender, EventArgs e)
        {
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

            // export path to csv
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.AddExtension = true;
            Dlg.DefaultExt = "csv";
            Dlg.Filter = "Comma Separated Values *.csv |csv";
            Dlg.FileName = "Path Information " + Properties.Settings.Default.MyCall.Replace("/", "_") + " to " + Properties.Settings.Default.DXCall.Replace("/", "_");
            Dlg.InitialDirectory = Application.StartupPath;
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(Dlg.FileName))
                    {
                        sw.WriteLine("Distance[km];Lat[deg];Lon[deg];Elevation[m]");
                        for (int i = 0; i < epath.Path.Length; i++)
                        {
                            double distance = (double)i * epath.StepWidth / 1000.0;
                            LatLon.GPoint p = LatLon.DestinationPoint(myloc.Lat, myloc.Lon, epath.Bearing12, distance);
                            sw.WriteLine(distance.ToString("F8") + ";" +
                                p.Lat.ToString("F8") + ";" +
                                p.Lon.ToString("F8") + ";" +
                                epath.Path[i].ToString());
                        }
                    }
                }
                catch
                {
                    // do nothing, if export is going wrong
                }
            }
        }

        #endregion

        #region tab_Options_Planes

        private void tab_Options_Planes_Enter(object sender, EventArgs e)
        {
            // set initial settings for planes tab
            cb_Options_PlaneFeed1.DisplayMember = "Name";
            cb_Options_PlaneFeed2.DisplayMember = "Name";
            cb_Options_PlaneFeed3.DisplayMember = "Name";
            cb_Options_PlaneFeed1.Items.Clear();
            cb_Options_PlaneFeed2.Items.Clear();
            cb_Options_PlaneFeed3.Items.Clear();
            cb_Options_PlaneFeed1.Items.Add("[none]");
            cb_Options_PlaneFeed2.Items.Add("[none]");
            cb_Options_PlaneFeed3.Items.Add("[none]");
            // select plane feeds
            if (ParentDlg.PlaneFeedPlugins != null)
            {
                foreach (var plugin in ParentDlg.PlaneFeedPlugins)
                {
                    cb_Options_PlaneFeed1.Items.Add(plugin);
                    cb_Options_PlaneFeed2.Items.Add(plugin);
                    cb_Options_PlaneFeed3.Items.Add(plugin);
                }
            }
            cb_Options_PlaneFeed1.SelectedIndex = cb_Options_PlaneFeed1.FindStringExact(Properties.Settings.Default.Planes_PlaneFeed1);
            cb_Options_PlaneFeed2.SelectedIndex = cb_Options_PlaneFeed2.FindStringExact(Properties.Settings.Default.Planes_PlaneFeed2);
            cb_Options_PlaneFeed3.SelectedIndex = cb_Options_PlaneFeed3.FindStringExact(Properties.Settings.Default.Planes_PlaneFeed3);
            // reset all feeds not found anymore
            if (cb_Options_PlaneFeed1.SelectedIndex < 0)
                cb_Options_PlaneFeed1.SelectedItem = "[none]";
            if (cb_Options_PlaneFeed2.SelectedIndex < 0)
                cb_Options_PlaneFeed2.SelectedItem = "[none]";
            if (cb_Options_PlaneFeed3.SelectedIndex < 0)
                cb_Options_PlaneFeed3.SelectedItem = "[none]";

            // fill planes filter dropdown
            string[] cats = PlaneCategories.GetStringValues();
            foreach (string cat in cats)
                cb_Options_Planes_Filter_Min_Cat.Items.Add(cat);
            try
            {
                cb_Options_Planes_Filter_Min_Cat.SelectedItem = PlaneCategories.GetStringValue(Properties.Settings.Default.Planes_Filter_Min_Category);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        private void tab_Options_Planes_Validating(object sender, CancelEventArgs e)
        {
            if (tb_Options_Planes_MaxAlt.Value < tb_Options_Planes_MinAlt.Value)
            {
                MessageBox.Show("MaxAlt must be greater than MinAlt.", "Parameter Error");
                e.Cancel = true;
            }
        }

        private void cb_Options_PlaneFeed1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_Options_PlaneFeed1.SelectedItem == null) || (cb_Options_PlaneFeed1.GetItemText(cb_Options_PlaneFeed1.SelectedItem) == "[none]"))
            {
                btn_Options_PlaneFeed1_Settings.Enabled = false;
                btn_Options_PlaneFeed1_Import.Enabled = false;
                btn_Options_PlaneFeed1_Export.Enabled = false;
                btn_Options_PlaneFeed1_Default.Enabled = false;
                Properties.Settings.Default.Planes_PlaneFeed1 = "[none]";
                return;
            }
            IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem;
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
                    try
                    {
                        feed.SaveSettings();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString());
                    }
                }
                else
                {
                    cb_Options_PlaneFeed1.SelectedItem = "[none]";
                }
            }
            btn_Options_PlaneFeed1_Settings.Enabled = feed.HasSettings;
            btn_Options_PlaneFeed1_Import.Enabled = feed.CanImport;
            btn_Options_PlaneFeed1_Export.Enabled = feed.CanExport;
            btn_Options_PlaneFeed1_Default.Enabled = true;
            Properties.Settings.Default.Planes_PlaneFeed1 = feed.Name;
        }

        private void cb_Options_PlaneFeed2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_Options_PlaneFeed2.SelectedItem == null) || (cb_Options_PlaneFeed2.GetItemText(cb_Options_PlaneFeed2.SelectedItem) == "[none]"))
            {
                btn_Options_PlaneFeed2_Settings.Enabled = false;
                btn_Options_PlaneFeed2_Import.Enabled = false;
                btn_Options_PlaneFeed2_Export.Enabled = false;
                btn_Options_PlaneFeed2_Default.Enabled = false;
                Properties.Settings.Default.Planes_PlaneFeed2 = "[none]";
                return;
            }
            IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem;
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
                    try
                    {
                        feed.SaveSettings();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString());
                    }
                }
                else
                {
                    cb_Options_PlaneFeed2.SelectedItem = "[none]";
                }
            }
            btn_Options_PlaneFeed2_Settings.Enabled = feed.HasSettings;
            btn_Options_PlaneFeed2_Import.Enabled = feed.CanImport;
            btn_Options_PlaneFeed2_Export.Enabled = feed.CanExport;
            btn_Options_PlaneFeed2_Default.Enabled = true;
            Properties.Settings.Default.Planes_PlaneFeed2 = feed.Name;
        }

        private void cb_Options_PlaneFeed3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cb_Options_PlaneFeed3.SelectedItem == null) || (cb_Options_PlaneFeed3.GetItemText(cb_Options_PlaneFeed3.SelectedItem) == "[none]"))
            {
                btn_Options_PlaneFeed3_Settings.Enabled = false;
                btn_Options_PlaneFeed3_Import.Enabled = false;
                btn_Options_PlaneFeed3_Export.Enabled = false;
                btn_Options_PlaneFeed3_Default.Enabled = false;
                Properties.Settings.Default.Planes_PlaneFeed3 = "[none]";
                return;
            }
            IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem;
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
                    try
                    {
                        feed.SaveSettings();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.ToString());
                    }
                }
                else
                {
                    cb_Options_PlaneFeed3.SelectedItem = "[none]";
                }
            }
            btn_Options_PlaneFeed3_Settings.Enabled = feed.HasSettings;
            btn_Options_PlaneFeed3_Import.Enabled = feed.CanImport;
            btn_Options_PlaneFeed3_Export.Enabled = feed.CanExport;
            btn_Options_PlaneFeed3_Default.Enabled = true;
            Properties.Settings.Default.Planes_PlaneFeed3 = feed.Name;
        }

        private void btn_Options_PlaneFeed1_Settings_Click(object sender, EventArgs e)
        {
            try
            {
                PlaneFeedSettingsDlg Dlg = new PlaneFeedSettingsDlg();
                Dlg.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem).Name;
                Dlg.lbl_Info.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem).Info;
                Dlg.lbl_Version.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem).Version;
                Dlg.pg_Main.SelectedObject = ((IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem).GetSettings();
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                }
                ((IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem).SaveSettings();
            }
            catch
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed2_Settings_Click(object sender, EventArgs e)
        {
            try
            {
                PlaneFeedSettingsDlg Dlg = new PlaneFeedSettingsDlg();
                Dlg.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem).Name;
                Dlg.lbl_Info.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem).Info;
                Dlg.lbl_Version.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem).Version;
                Dlg.pg_Main.SelectedObject = ((IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem).GetSettings();
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                }
                ((IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem).SaveSettings();
            }
            catch
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed3_Settings_Click(object sender, EventArgs e)
        {
            try
            {
                PlaneFeedSettingsDlg Dlg = new PlaneFeedSettingsDlg();
                Dlg.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem).Name;
                Dlg.lbl_Info.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem).Info;
                Dlg.lbl_Version.Text = ((IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem).Version;
                Dlg.pg_Main.SelectedObject = ((IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem).GetSettings();
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                }
                ((IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem).SaveSettings();
            }
            catch
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed1_Import_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem;
                feed.ImportSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed2_Import_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem;
                feed.ImportSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed3_Import_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem;
                feed.ImportSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed1_Export_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem;
                feed.ExportSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed2_Export_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem;
                feed.ExportSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }

        }

        private void btn_Options_PlaneFeed3_Export_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem;
                feed.ExportSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }

        }

        private void btn_Options_PlaneFeed1_Default_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed1.SelectedItem;
                feed.ResetSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed2_Default_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed2.SelectedItem;
                feed.ResetSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Options_PlaneFeed3_Default_Click(object sender, EventArgs e)
        {
            try
            {
                IPlaneFeedPlugin feed = (IPlaneFeedPlugin)cb_Options_PlaneFeed3.SelectedItem;
                feed.ResetSettings();
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void cb_Options_Planes_Filter_Min_Cat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Planes_Filter_Min_Category = PlaneCategories.ParseStringValue((string) cb_Options_Planes_Filter_Min_Cat.SelectedItem);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
            }
        }

        #endregion

        #region tab_Options_Alarm

        private void tab_Options_Alarm_Enter(object sender, EventArgs e)
        {

        }

        #endregion

        #region tab_Options_Network

        private void tab_Options_Network_Enter(object sender, EventArgs e)
        {
        }

        private void tab_Options_Network_Validating(object sender, CancelEventArgs e)
        {
            if (tb_Options_Server_Port.Value == tb_Options_Webserver_Port.Value)
            {
                MessageBox.Show("UDP Server and HTTP Server must have different port numbers.", "Parameter Error");
                e.Cancel = true;
            }
        }

        #endregion

        #region tab_Options_SpecLab

        private void tab_Options_SpecLab_Enter(object sender, EventArgs e)
        {
        }

        private void tab_Options_SpecLab_Validating(object sender, CancelEventArgs e)
        {
            if (tb_Options_SpecLab_F1.Value > tb_Options_SpecLab_F2.Value)
            {
                MessageBox.Show("F1 must be lesser than F2.", "Parameter Error");
                e.Cancel = true;
            }
        }
        #endregion

        #region tab_Options_Track

        private void tab_Options_Track_Enter(object sender, EventArgs e)
        {

        }

        private void tc_Track_Validating(object sender, CancelEventArgs e)
        {
            if (tb_Options_Track_UDP_WinTest_Port.Value == tb_Options_Track_UDP_AirScout_Port.Value)
            {
                MessageBox.Show("Win-Test port and AirScout port must have different values.", "Parameter Error");
                e.Cancel = true;
            }
            List<int> baudrates = new List<int>(new int[] { 50, 100, 110, 150, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 });
            if (!baudrates.Contains(tb_Options_Track_Serial_Baudrate.Value))
            {
                string s = "";
                foreach (int b in baudrates)
                    s = s + b.ToString() + ", ";
                s = s.Trim().TrimEnd(',');
                MessageBox.Show("This is not a valid baudrate: " + tb_Options_Track_Serial_Baudrate.Text + "\nValid baudrates are:\n\n" + s, "Parameter Error");
                e.Cancel = true;
            }
        }

        #endregion

        #region tab_Options_Info

        private void tab_Options_Info_Enter(object sender, EventArgs e)
        {
            try
            {
                // clear links first
                lbl_Options_Map.Links.Clear();
                lbl_Options_Spherical.Links.Clear();
                lbl_Options_Elevation_GLOBE.Links.Clear();
                lbl_Options_Elevation_SRTM3.Links.Clear();
                lbl_Options_Elevation_SRTM1.Links.Clear();
                // populate link labels
                lbl_Options_Version.Text = "Version: " + Application.ProductVersion;
                lbl_Options_Map.Text = "GMap.NET Copyright (c) 2008 - 2011 Universe";
                lbl_Options_Map.Links.Add(0, 8, "http://greatmaps.codeplex.com/");
                lbl_Options_Spherical.Text = "http://www.movable-type.co.uk/scripts/latlong.html";
                lbl_Options_Spherical.Links.Add(0, lbl_Options_Spherical.Text.Length - 1, "http://www.movable-type.co.uk/scripts/latlong.html");
                lbl_Options_Elevation_GLOBE.Text = "1km based Elevation Data from GLOBE - Project";
                lbl_Options_Elevation_GLOBE.Links.Add(30, 5, "http://www.ngdc.noaa.gov/mgg/topo/globe.html");
                lbl_Options_Elevation_SRTM3.Text = "3arsec (90m x 90m) Elevation Data from  SRTM - Project";
                lbl_Options_Elevation_SRTM3.Links.Add(40, 14, "http://srtm.usgs.gov/");
                lbl_Options_Elevation_SRTM1.Text = "1arsec (30m x 30m) Elevation Data from  SRTM - Project and ASTER";
                lbl_Options_Elevation_SRTM1.Links.Add(40, 14, "http://srtm.usgs.gov");
                lbl_Options_Elevation_SRTM1.Links.Add(58, 6, "http://asterweb.jpl.nasa.gov/index.asp");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
        }


        private void lbl_Options_Map_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void lbl_Options_ElevationSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void lbl_Options_Spherical_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }



        private void lbl_Options_ElevationDataHowTo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void lbl_Options_Elevation_SRTM3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void lbl_Options_Elevation_SRTM1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void btn_Options_License_Click(object sender, EventArgs e)
        {
            LicenseDlg Dlg = new LicenseDlg();
            Dlg.ShowDialog();
        }


        #endregion

        #region tab_Options_Watchlist

        private void tab_Options_Watchlist_Enter(object sender, EventArgs e)
        {

        }

        private void cb_Options_Watchlist_SyncWithKST_CheckedChanged(object sender, EventArgs e)
        {
            // disable manage buttons when synced with KST
            btn_Options_Watchlist_Manage.Enabled = !cb_Options_Watchlist_SyncWithKST.Checked;
            btn_Options_Watchlist_Clear.Enabled = !cb_Options_Watchlist_SyncWithKST.Checked;
        }

        private void btn_Options_Watchlist_Manage_Click(object sender, EventArgs e)
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
            }
        }

        private void btn_Options_Watchlist_Clear_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Watchlist.Clear();
        }


        #endregion

        #region tab_Options_Misc

        private void btn_Options_Open_AirScoutDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(ParentDlg.AppDirectory);
        }

        private void btn_Options_Open_LogDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(ParentDlg.LogDirectory);
        }

        private void btn_Options_Open_TmpDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(ParentDlg.TmpDirectory);
        }

        private void btn_Options_Open_PluginDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(ParentDlg.PluginDirectory);
        }

        private void pb_Donate_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Properties.Settings.Default.Donate_URL);
            }
            catch
            {
            }
        }

        #endregion

        private void OptionsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            bw_GLOBE_MapUpdater.CancelAsync();
            bw_SRTM3_MapUpdater.CancelAsync();
            bw_SRTM1_MapUpdater.CancelAsync();
            bw_StationDataUpdater.CancelAsync();
            // do garbage collection
            GC.Collect();
        }


        private void OptionsDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btn_Options_Import_Callsigns_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.DefaultExt = ".txt";
            Dlg.CheckFileExists = true;
            Dlg.Multiselect = false;
            Dlg.Title = "Import Callsign Database as TXT-File";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                // import old callsign database
                using (StreamReader sr = new StreamReader(Dlg.FileName))
                {
                    int count = 0;
                    string s = sr.ReadToEnd();
                    List<LocationDesignator> l = StationData.Database.LocationFromTXT(s);
                    foreach (LocationDesignator ld in l)
                    {
                        int i = StationData.Database.LocationInsertOrUpdateIfNewer(ld);
                        if (i > 0)
                            count += i;
                    }
                    MessageBox.Show("The callsign database was imported succuessfully.\n" + count.ToString() + " entries were inserted/updated.", "Import Callsign Database as TXT-File", MessageBoxButtons.OK);
                }
            }
        }

        #region StationDataUpdater

        private void bw_StationDataUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            try
            {
                StationDataUpdaterDoWorkEventArgs args = (StationDataUpdaterDoWorkEventArgs)e.Argument;
                string encoded;
                string result;
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.FloatFormatHandling = FloatFormatHandling.String;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string json = JsonConvert.SerializeObject(args.ld, settings);
                encoded = Encryption.OpenSSLEncrypt(json, ParentDlg.SessionKey);
                WebClient client = new WebClient();
                result = client.UploadString(Properties.Settings.Default.AirScout_UploadLocation_URL + "?id=" + Properties.Settings.Default.AirScout_Instance_ID, encoded);
                foreach (QRVDesignator qrv in args.qrvs)
                {
                    json = JsonConvert.SerializeObject(qrv, settings);
                    encoded = Encryption.OpenSSLEncrypt(json, ParentDlg.SessionKey);
                    result = client.UploadString(Properties.Settings.Default.AirScout_UploadQRV_URL + "?id=" + Properties.Settings.Default.AirScout_Instance_ID, encoded);
                }
                bw_StationDataUpdater.ReportProgress(100, "Location upload successful.");
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString(), LogLevel.Error);
                bw_StationDataUpdater.ReportProgress(-1, "Error while uploading location:" + ex.Message);
            }
            Log.WriteMessage("Finished.");
        }

        private void bw_StationDataUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                Say((string)e.UserState);
            }
            catch
            {
            }
        }

        private void bw_StationDataUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }


        #endregion

    }

    public class StationDataUpdaterDoWorkEventArgs
    {
        public LocationDesignator ld;
        public List<QRVDesignator> qrvs;
    }



}
