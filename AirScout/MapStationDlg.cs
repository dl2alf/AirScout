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
            Callsignspositions.Markers.Add(UserPos);
            // initially set textboxes
            tb_Callsign.SilentText = StationLocation.Call;
            tb_Latitude.SilentValue = StationLocation.Lat;
            tb_Longitude.SilentValue = StationLocation.Lon;
            tb_Locator.SilentText = MaidenheadLocator.LocFromLatLon(StationLocation.Lat, StationLocation.Lon, Properties.Settings.Default.Locator_SmallLettersForSubsquares, (int)Properties.Settings.Default.Locator_MaxLength / 2, Properties.Settings.Default.Locator_AutoLength);
            tb_Elevation.SilentValue = GetElevation(StationLocation.Lat, StationLocation.Lon);
            ValidateDetails();
            // show initial zoom level in text box
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
        }
    }
}
