using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using ScoutBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutBase.Core;
using ScoutBase.Stations;
using ScoutBase.Elevation;

namespace AirScout
{
    public partial class MapStationDlg : Form
    {

        GMapOverlay Callsignpolygons = new GMapOverlay("Callsignpolygons");
        GMapOverlay Callsignspositions = new GMapOverlay("Callsignpositions");
        GMapOverlay Elevationgrid = new GMapOverlay("Elevationgrid");

        GMarkerGoogle UserPos = new GMarkerGoogle(new PointLatLng(0.0, 0.0), GMarkerGoogleType.red_dot);

        private bool IsDraggingMarker = false;

        public LocationDesignator StationLocation;

        public MapStationDlg(LocationDesignator ld)
        {
            StationLocation = ld;
            InitializeComponent();

            // set initial settings for user details
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            gm_Callsign.MapProvider = GMap.NET.MapProviders.GMapProviders.Find(Properties.Settings.Default.Map_Provider);
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
            gm_Callsign.Overlays.Add(Elevationgrid);
            Callsignspositions.Markers.Add(UserPos);
            // initially set textboxes
            tb_Callsign.SilentText = StationLocation.Call;
            tb_Latitude.SilentValue = StationLocation.Lat;
            tb_Longitude.SilentValue = StationLocation.Lon;
            tb_Locator.SilentText = MaidenheadLocator.LocFromLatLon(StationLocation.Lat, StationLocation.Lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength);
            tb_Elevation.SilentValue = GetElevation(StationLocation.Lat, StationLocation.Lon);
            ValidateDetails();

        }

        private void MapStationDlg_Load(object sender, EventArgs e)
        {
            // initialen Zoomlevel anzeigen
            gm_Callsign_OnMapZoomChanged();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

        }

        public int GetElevation(double lat, double lon)
        {
            int elv = ElevationData.Database.ElvMissingFlag;
            // try to get elevation data from distinct elevation model
            // start with detailed one
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.SRTM1, false];
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.SRTM3, false];
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[lat, lon, ELEVATIONMODEL.GLOBE, false];
            // set it to zero if still invalid
            if (elv == ElevationData.Database.ElvMissingFlag)
                elv = 0;

            return elv;
        }


        private bool ValidateDetails()
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
                // update locator text if not focusd
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
                        case 6:
                            gm_Callsign.Zoom = 12;
                            break;
                        case 8:
                            gm_Callsign.Zoom = 15;
                            break;
                        case 10:
                            gm_Callsign.Zoom = 17;
                            break;
                    }
                }
            }
            // check all values
            if (Callsign.Check(tb_Callsign.Text) && MaidenheadLocator.Check(tb_Locator.Text) && !double.IsNaN(tb_Latitude.Value) && !double.IsNaN(tb_Longitude.Value))
            {
                StationLocation.Lat = tb_Latitude.Value;
                StationLocation.Lon = tb_Longitude.Value;
                StationLocation.Source = MaidenheadLocator.IsPrecise(tb_Latitude.Value, tb_Longitude.Value, 3) ? GEOSOURCE.FROMUSER : GEOSOURCE.FROMLOC;
                StationLocation.Loc = MaidenheadLocator.LocFromLatLon(StationLocation.Lat, StationLocation.Lon, false, 3);
                tb_Elevation.SilentValue = GetElevation(StationLocation.Lat, StationLocation.Lon);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void tb_Callsign_TextChanged(object sender, EventArgs e)
        {
        }

        private void tb_Latitude_TextChanged(object sender, EventArgs e)
        {
            ValidateDetails();
        }

        private void tb_Longitude_TextChanged(object sender, EventArgs e)
        {
            ValidateDetails();
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
            ValidateDetails();
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
                    StationLocation.Lat = p.Lat;
                    StationLocation.Lon = p.Lng;
                    ValidateDetails();
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
            ShowElevationGrid();
        }

        private void ShowElevationGrid()
        {
            while (bw_Elevationgrid.IsBusy)
            {
                bw_Elevationgrid.CancelAsync();
                Application.DoEvents();
            }
            Elevationgrid.Polygons.Clear();
            if (cb_Options_StationsMap_OverlayElevation.Checked && (gm_Callsign.Zoom >= 17))
            {
                bw_Elevationgrid.RunWorkerAsync();
            }
        }

        private void bw_Elevationgrid_DoWork(object sender, DoWorkEventArgs e)
        {
            // fill elevation grid
            bw_Elevationgrid.ReportProgress(0, "Calculating elevation grid...");
            Elevationgrid.Polygons.Clear();
            // convert view bounds to the beginning/end of Maidenhead locators
            string loc = MaidenheadLocator.LocFromLatLon(gm_Callsign.ViewArea.Lat - gm_Callsign.ViewArea.HeightLat, gm_Callsign.ViewArea.Lng,false,3);
            double minlat = MaidenheadLocator.LatFromLoc(loc, PositionInRectangle.BottomLeft);
            double minlon = MaidenheadLocator.LonFromLoc(loc, PositionInRectangle.BottomLeft);
            loc = MaidenheadLocator.LocFromLatLon(gm_Callsign.ViewArea.Lat, gm_Callsign.ViewArea.Lng + gm_Callsign.ViewArea.WidthLng, false, 3);
            double maxlat = MaidenheadLocator.LatFromLoc(loc, PositionInRectangle.TopRight);
            double maxlon = MaidenheadLocator.LonFromLoc(loc, PositionInRectangle.TopRight);
            double lat = minlat;
            double lon = minlon;
            double stepwidthlat = 5.5555555555555555555555555555556e-4 / 2;
            double stepwidthlon = 5.5555555555555555555555555555556e-4;
            List<ElevationTile> elvs = new List<ElevationTile>();
            double elvmin = short.MaxValue;
            double elvmax = short.MinValue;
            while (!bw_Elevationgrid.CancellationPending && (lat < maxlat))
            {
                lon = minlon;
                while (!bw_Elevationgrid.CancellationPending && (lon < maxlon))
                {
                    double elv = 0;
                    if ((lat + stepwidthlat >= gm_Callsign.ViewArea.Lat - gm_Callsign.ViewArea.HeightLat) &&
                        (lat <= gm_Callsign.ViewArea.Lat) &&
                        (lon + stepwidthlon >= gm_Callsign.ViewArea.Lng) &&
                        (lon <= gm_Callsign.ViewArea.Lng + gm_Callsign.ViewArea.WidthLng))
                    {
                        elv = ElevationData.Database[lat, lon, Properties.Settings.Default.ElevationModel];
                        ElevationTile t = new ElevationTile();
                        t.Lat = lat;
                        t.Lon = lon;
                        t.Elv = elv;
                        elvs.Add(t);
                        if (elv < elvmin)
                            elvmin = elv;
                        if (elv > elvmax)
                            elvmax = elv;
                    }
                    else
                    {
                    }
                    lon += stepwidthlon;
                }
                lat += stepwidthlat;
            }
            foreach (ElevationTile t in elvs)
            {
                List<PointLatLng> l = new List<PointLatLng>();
                l.Add(new PointLatLng((decimal)t.Lat, (decimal)t.Lon));
                l.Add(new PointLatLng((decimal)(t.Lat + stepwidthlat), (decimal)t.Lon));
                l.Add(new PointLatLng((decimal)(t.Lat + stepwidthlat), (decimal)(t.Lon + stepwidthlon)));
                l.Add(new PointLatLng((decimal)t.Lat, (decimal)(t.Lon + stepwidthlon)));
                GMapTextPolygon p = new GMapTextPolygon(l, t.Elv + " m");
                Color c = Color.FromArgb(100, ElevationData.Database.GetElevationColor((t.Elv - elvmin) / (elvmax - elvmin) * 10000.0));
                p.Stroke = new Pen(c);
                p.Fill = new SolidBrush(c);
                Font f = new Font("Courier New", 8, GraphicsUnit.Pixel);
                bw_Elevationgrid.ReportProgress(1, p);
                if (bw_Elevationgrid.CancellationPending)
                    break;
            }
            bw_Elevationgrid.ReportProgress(100, "Calculating elevation grid finished.");
        }

        private void bw_Elevationgrid_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                tsl_Status.Text = (string)e.UserState;
                ss_Main.Refresh();
            }
            if (e.ProgressPercentage == 0)
            {
                Elevationgrid.Polygons.Clear();
                tsl_Status.Text = (string)e.UserState;
                ss_Main.Refresh();
            }
            else if (e.ProgressPercentage == 1)
            {
                GMapPolygon p = (GMapPolygon)e.UserState;
                Elevationgrid.Polygons.Add(p);
                tsl_Status.Text = "Adding elevation tile " + p.Name;
                ss_Main.Refresh();
            }
            else if (e.ProgressPercentage == 100)
            {
                tsl_Status.Text = (string)e.UserState;
                ss_Main.Refresh();
            }
        }

        private void bw_Elevationgrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void cb_Options_StationsMap_OverlayElevation_CheckedChanged(object sender, EventArgs e)
        {
            ShowElevationGrid();
        }
    }

    public class GMapTextPolygon : GMapPolygon
    {
        public GMapTextPolygon(List<PointLatLng> points, string name) : base(points, name)
        {
        }

        public override void OnRender(Graphics g)
        {
            base.OnRender(g);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            long minx = long.MaxValue;
            long maxx = long.MinValue;
            long miny = long.MaxValue;
            long maxy = long.MinValue;
            for (int i = 0; i < this.LocalPoints.Count; i++)
            {
                if (this.LocalPoints[i].X < minx)
                    minx = this.LocalPoints[i].X;
                if (this.LocalPoints[i].X > maxx)
                    maxx = this.LocalPoints[i].X;
                if (this.LocalPoints[i].Y < miny)
                    miny = this.LocalPoints[i].Y;
                if (this.LocalPoints[i].Y > maxy)
                    maxy = this.LocalPoints[i].Y;
            }
            RectangleF f = new RectangleF(minx, miny, maxx - minx, maxy - miny);
            g.DrawString(this.Name, SystemFonts.DefaultFont, Brushes.Black, f, format);
        }
    }

    public class ElevationTile
    {
        public double Lat = 0;
        public double Lon = 0;
        public double Elv = 0;
    }


}
