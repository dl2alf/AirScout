using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.IO;
using AirScout.Core;
using AirScout.Aircrafts;
using ScoutBase.Core;
using AirScout.AircraftPositions;

namespace AirScout
{
    public partial class TrafficDlg  : Form
    {
        GMapOverlay Coveragepolygons = new GMapOverlay("Coveragepolygons");
        GMapOverlay routes = new GMapOverlay("routes");

        public TrafficDlg()
        {
            InitializeComponent();

            // set initial settings for Map
            gm_Options_Traffic.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Options_Traffic.IgnoreMarkerOnMouseWheel = true;
            gm_Options_Traffic.MinZoom = 0;
            gm_Options_Traffic.MaxZoom = 20;
            gm_Options_Traffic.Zoom = 6;
            gm_Options_Traffic.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Options_Traffic.CanDragMap = true;
            gm_Options_Traffic.ScalePen = new Pen(Color.Black, 3);
            gm_Options_Traffic.HelperLinePen = null;
            gm_Options_Traffic.SelectionPen = null;
            gm_Options_Traffic.MapScaleInfoEnabled = true;
            gm_Options_Traffic.Overlays.Add(Coveragepolygons);
            gm_Options_Traffic.Overlays.Add(routes);

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
            gm_Options_Traffic.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));

            try
            {
                dtp_Options_Traffic_Start.Value = AircraftPositionData.Database.AircraftPositionOldestEntry();
                dtp_Options_Traffic_Stop.Value = AircraftPositionData.Database.AircraftPositionYoungestEntry();
            }
            catch
            {
                dtp_Options_Traffic_Start.Value = DateTime.UtcNow;
                dtp_Options_Traffic_Stop.Value = DateTime.UtcNow;
            }
        }

        private void Say(string text)
        {
            if (tsl_Main.Text != text)
            {
                tsl_Main.Text = text;
                ss_Main.Refresh();
            }
        }

        private void TrafficDlg_SizeChanged(object sender, EventArgs e)
        {
            gm_Options_Traffic.SetZoomToFitRect(RectLatLng.FromLTRB(Properties.Settings.Default.MinLon - 1, Properties.Settings.Default.MaxLat + 1, Properties.Settings.Default.MaxLon + 1, Properties.Settings.Default.MinLat - 1));
        }

        private void btn_Options_Traffic_Show_Click(object sender, EventArgs e)
        {
            if (!bw_Calculate.IsBusy)
            {
                // clear routes
                routes.Clear();
                // start background worker
                bw_Calculate.RunWorkerAsync();
            }
        }

        private void gm_Options_Traffic_OnRouteEnter(GMapRoute item)
        {
            int k = 0;
        }

        private void bw_Calculate_DoWork(object sender, DoWorkEventArgs e)
        {
            // show plane tracks
            List<string> allhex = AircraftPositionData.Database.AircraftPositionGetAllHex(dtp_Options_Traffic_Start.Value, dtp_Options_Traffic_Stop.Value);
            int i = 1;
            foreach (string hex in allhex)
            {
                bw_Calculate.ReportProgress(0,"Calculating " + hex + " [" + i.ToString() + " of " + allhex.Count().ToString() + "]");
                List<AircraftPositionDesignator> allpos = AircraftPositionData.Database.AircraftPositionGetAllByHex(hex, dtp_Options_Traffic_Start.Value, dtp_Options_Traffic_Stop.Value);
                int count = 0;
                GMapRoute r = new GMapRoute(hex + count.ToString());
                r.Stroke = new Pen(Color.FromArgb((int)((double)ud_Opacity.Value * 255.0 / 100.0), Color.Black), 1);
                foreach (AircraftPositionDesignator pos in allpos)
                {
                    PointLatLng po = new PointLatLng(pos.Lat, pos.Lon);
                    if (r.Points.Count < 1)
                    {
                        r.Points.Add(po);
                    }
                    else
                    {
                        // check the distance between last waypoint for disruption 
                        // start a new route then
                        double d = LatLon.Distance(r.Points[r.Points.Count - 1].Lat, r.Points[r.Points.Count - 1].Lng, po.Lat, po.Lng);
                        if (d < 100)
                            r.Points.Add(po);
                        else
                        {
                            // add old route
                            bw_Calculate.ReportProgress(1, r);
                            count++;
                            // create a new one
                            r = new GMapRoute(hex + count.ToString());
                            r.Stroke = new Pen(Color.FromArgb((int)((double)ud_Opacity.Value * 255.0 / 100.0), Color.Black), 1);
                        }
                    }
                }
                i++;
                bw_Calculate.ReportProgress(1, r);
                if (bw_Calculate.CancellationPending)
                    return;
            }
        }

        private void bw_Calculate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                Say((string)(e.UserState));
            }
            else if (e.ProgressPercentage == 1)
            {
                GMapRoute r = (GMapRoute)e.UserState;
                routes.Routes.Add(r);
            }
        }

        private void bw_Calculate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void btn_Options_Traffic_Close_Click(object sender, EventArgs e)
        {
            if (bw_Calculate.IsBusy)
            {
                bw_Calculate.CancelAsync();
            }
            this.Close();
        }
    }
}
