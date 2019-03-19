using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using ScoutBase.Core;
using ScoutBase.Stations;
using ScoutBase.Elevation;
using ScoutBase.Propagation;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;


namespace AirScout
{
    public partial class HorizonDlg : Form
    {

        new LocationDesignator Location;
        QRVDesignator QRV;
        LocalObstructionDesignator LocalObstruction;
        short Elevation;
        double AntennaHeight;

        PropagationHorizonDesignator Horizon;
        double CalculationDistance = 500;

        GMapOverlay horizons = new GMapOverlay("Horizons");
        GMapRoute horizon = new GMapRoute("Horizon");

        ToolTip TT;
        IWin32Window OwnerWin;

        // charting
        // horizon chart
        PlotView pv_Elevation_Polar = new PlotView();
        PlotModel pm_Elevation_Polar = new PlotModel();
        AngleAxis Elevation_Polar_X = new AngleAxis();
        MagnitudeAxis Elevation_Polar_Y = new MagnitudeAxis();
        LineSeries Elevation_Polar_Series = new LineSeries();
        PlotView pv_Elevation_Cartesian = new PlotView();
        PlotModel pm_Elevation_Cartesian = new PlotModel();
        LinearAxis Elevation_Cartesian_X = new LinearAxis();
        LinearAxis Elevation_Cartesian_Y = new LinearAxis();
        LineSeries Elevation_Cartesian_Series = new LineSeries();

        // horizon distance chart
        PlotView pv_Distance_Polar = new PlotView();
        PlotModel pm_Distance_Polar = new PlotModel();
        AngleAxis Distance_Polar_X = new AngleAxis();
        MagnitudeAxis Distance_Polar_Y = new MagnitudeAxis();
        LineSeries Distance_Polar_Series = new LineSeries();
        PlotView pv_Distance_Cartesian = new PlotView();
        PlotModel pm_Distance_Cartesian = new PlotModel();
        LinearAxis Distance_Cartesian_X = new LinearAxis();
        LinearAxis Distance_Cartesian_Y = new LinearAxis();
        LineSeries Distance_Cartesian_Series = new LineSeries();

        double Map_Left;
        double Map_Right;
        double Map_Top;
        double Map_Bottom;

        public HorizonDlg(string call, double lat, double lon, LocalObstructionDesignator localobstruction)
        {
            InitializeComponent();
            Location = StationData.Database.LocationFindOrCreate(call, MaidenheadLocator.LocFromLatLon(lat, lon, false, 3));
            QRV = StationData.Database.QRVFindOrCreateDefault(call, MaidenheadLocator.LocFromLatLon(lat, lon, false, 3), Properties.Settings.Default.Band);
            LocalObstruction = localobstruction;
            Elevation = ElevationData.Database[Location.Lat, Location.Lon, Properties.Settings.Default.ElevationModel];
            AntennaHeight = (QRV.AntennaHeight != 0) ? QRV.AntennaHeight : StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            tb_Horizon_Lat.Text = Location.Lat.ToString("F8",provider);
            tb_Horizon_Lon.Text = Location.Lon.ToString("F8",provider);
            tb_Horizon_Elevation.Text = ElevationData.Database[Location.Lat, Location.Lon, Properties.Settings.Default.ElevationModel].ToString("F0",provider);
            tb_Horizon_Height.Text = AntennaHeight.ToString("F0",provider);
            tb_Horizon_K_Factor.Text = Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor.ToString("F2",provider);
            tb_Horizon_QRG.Text = Bands.GetStringValue(Properties.Settings.Default.Band);
            tb_Horizon_F1_Clearance.Text = Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance.ToString("F2",provider);
            tb_Horizon_ElevationModel.Text = Properties.Settings.Default.ElevationModel.ToString();
            // setting User Agent to fix Open Street Map issue 2016-09-20
            GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
            // set initial settings for main map
            gm_Horizon.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
            gm_Horizon.IgnoreMarkerOnMouseWheel = true;
            gm_Horizon.MinZoom = 0;
            gm_Horizon.MaxZoom = 20;
            gm_Horizon.Zoom = 8;
            gm_Horizon.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Horizon.CanDragMap = true;
            gm_Horizon.ScalePen = new Pen(Color.Black, 3);
            gm_Horizon.MapScaleInfoEnabled = true;
            gm_Horizon.Overlays.Add(horizons);
            GMarkerGoogle gm = new GMarkerGoogle(new PointLatLng(Location.Lat, Location.Lon), GMarkerGoogleType.red_dot);
            gm.ToolTipText = Location.Call;
            horizons.Markers.Add(gm);
            horizon.Stroke = new Pen(Color.Red, 3);
            horizons.Routes.Add(horizon);
            this.Text = "Radio Horizon of " + Location.Call;
            // initialize charts
            InitializeCharts();
            // activate Polar if nothing checked
            if (!Properties.Settings.Default.Horizon_Plot_Polar && !Properties.Settings.Default.Horizon_Plot_Cartesian && !Properties.Settings.Default.Horizon_Plot_Map)
                Properties.Settings.Default.Horizon_Plot_Polar = true;
            // show according child windows
            UpdateCharts();
            // create ToolTip on this window
            TT = new ToolTip();
            OwnerWin = gm_Horizon;
            HorizonDlg_SizeChanged(this, null);
            // set map bounds
            Map_Left = Location.Lon;
            Map_Right = Location.Lon;
            Map_Top = Location.Lat;
            Map_Bottom = Location.Lat;
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


        private void InitializeCharts()
        {
            // elevation polar chart
            pm_Elevation_Polar.PlotType = PlotType.Polar;
            pm_Elevation_Polar.Title = String.Empty;
            pm_Elevation_Polar.DefaultFontSize = 6F;
            pv_Elevation_Polar.BackColor = Color.White;
            pv_Elevation_Polar.Dock = DockStyle.Fill;
            pv_Elevation_Polar.Model = pm_Elevation_Polar;
            // add axes
            pm_Elevation_Polar.Axes.Clear();
            // add x-axis
            Elevation_Polar_X.MajorGridlineStyle = LineStyle.Solid;
            Elevation_Polar_X.MinorGridlineStyle = LineStyle.Dot;
            Elevation_Polar_X.Angle = 90;
            Elevation_Polar_X.MajorStep = 30;
            Elevation_Polar_X.MinorStep = 10;
            Elevation_Polar_X.Minimum = 0;
            Elevation_Polar_X.Maximum = 360;
            Elevation_Polar_X.StartPosition = 1;
            Elevation_Polar_X.EndPosition = 0;
            Elevation_Polar_X.StartAngle = 360 + 90;
            Elevation_Polar_X.EndAngle = 90;
            pm_Elevation_Polar.Axes.Add(Elevation_Polar_X);
            // add y-axis
            Elevation_Polar_Y.MajorGridlineStyle = LineStyle.Solid;
            Elevation_Polar_Y.MinorGridlineStyle = LineStyle.Dot;
            Elevation_Polar_Y.Angle = 0;
            Elevation_Polar_Y.Position = AxisPosition.Right;
            pm_Elevation_Polar.Axes.Add(Elevation_Polar_Y);
            // add series            
            pm_Elevation_Polar.Series.Clear();
            Elevation_Polar_Series.StrokeThickness = 2;
            Elevation_Polar_Series.LineStyle = LineStyle.Solid;
            Elevation_Polar_Series.Color = OxyColors.Blue;
            pm_Elevation_Polar.Series.Add(Elevation_Polar_Series);
            gb_Elevation.Controls.Add(pv_Elevation_Polar);

            // elevation cartesian chart
            pv_Elevation_Cartesian.BackColor = Color.White;
            pv_Elevation_Cartesian.Dock = DockStyle.Fill;
            pv_Elevation_Cartesian.Model = pm_Elevation_Cartesian;
            pm_Elevation_Cartesian.Title = String.Empty;
            pm_Elevation_Cartesian.DefaultFontSize = 6F;
            // add axes
            pm_Elevation_Cartesian.Axes.Clear();
            // add x-axis
            Elevation_Cartesian_X.MajorGridlineStyle = LineStyle.Solid;
            Elevation_Cartesian_X.MinorGridlineStyle = LineStyle.Dot;
            Elevation_Cartesian_X.MajorStep = 30;
            Elevation_Cartesian_X.MinorStep = 10;
            Elevation_Cartesian_X.Minimum = 0;
            Elevation_Cartesian_X.Maximum = 360;
            Elevation_Cartesian_X.Position = AxisPosition.Bottom;
            pm_Elevation_Cartesian.Axes.Add(Elevation_Cartesian_X);
            // add y-axis
            Elevation_Cartesian_Y.MajorGridlineStyle = LineStyle.Solid;
            Elevation_Cartesian_Y.MinorGridlineStyle = LineStyle.Dot;
            Elevation_Cartesian_Y.Position = AxisPosition.Left;
            pm_Elevation_Cartesian.Axes.Add(Elevation_Cartesian_Y);
            // add series            
            pm_Elevation_Cartesian.Series.Clear();
            Elevation_Cartesian_Series.StrokeThickness = 2;
            Elevation_Cartesian_Series.LineStyle = LineStyle.Solid;
            Elevation_Cartesian_Series.Color = OxyColors.Blue;
            pm_Elevation_Cartesian.Series.Add(Elevation_Cartesian_Series);
            gb_Elevation.Controls.Add(pv_Elevation_Cartesian);

            // distance polar chart
            pm_Distance_Polar.PlotType = PlotType.Polar;
            pm_Distance_Polar.Title = String.Empty;
            pm_Distance_Polar.DefaultFontSize = 6F;
            pv_Distance_Polar.BackColor = Color.White;
            pv_Distance_Polar.Dock = DockStyle.Fill;
            pv_Distance_Polar.Model = pm_Distance_Polar;
            // add axes
            pm_Distance_Polar.Axes.Clear();
            // add x-axis
            Distance_Polar_X.MajorGridlineStyle = LineStyle.Solid;
            Distance_Polar_X.MinorGridlineStyle = LineStyle.Dot;
            Distance_Polar_X.Angle = 90;
            Distance_Polar_X.MajorStep = 30;
            Distance_Polar_X.MinorStep = 10;
            Distance_Polar_X.Minimum = 0;
            Distance_Polar_X.Maximum = 360;
            Distance_Polar_X.StartPosition = 1;
            Distance_Polar_X.EndPosition = 0;
            Distance_Polar_X.StartAngle = 360 + 90;
            Distance_Polar_X.EndAngle = 90;
            pm_Distance_Polar.Axes.Add(Distance_Polar_X);
            // add y-axis
            Distance_Polar_Y.MajorGridlineStyle = LineStyle.Solid;
            Distance_Polar_Y.MinorGridlineStyle = LineStyle.Dot;
            Distance_Polar_Y.Angle = 0;
            Distance_Polar_Y.Position = AxisPosition.Right;
            pm_Distance_Polar.Axes.Add(Distance_Polar_Y);
            // add series            
            pm_Distance_Polar.Series.Clear();
            Distance_Polar_Series.StrokeThickness = 2;
            Distance_Polar_Series.LineStyle = LineStyle.Solid;
            Distance_Polar_Series.Color = OxyColors.Red;
            pm_Distance_Polar.Series.Add(Distance_Polar_Series);
            gb_Distance.Controls.Add(pv_Distance_Polar);

            // distance cartesian chart
            pv_Distance_Cartesian.BackColor = Color.White;
            pv_Distance_Cartesian.Dock = DockStyle.Fill;
            pv_Distance_Cartesian.Model = pm_Distance_Cartesian;
            pm_Distance_Cartesian.Title = String.Empty;
            pm_Distance_Cartesian.DefaultFontSize = 6F;
            // add axes
            pm_Distance_Cartesian.Axes.Clear();
            // add x-axis
            Distance_Cartesian_X.MajorGridlineStyle = LineStyle.Solid;
            Distance_Cartesian_X.MinorGridlineStyle = LineStyle.Dot;
            Distance_Cartesian_X.MajorStep = 30;
            Distance_Cartesian_X.MinorStep = 10;
            Distance_Cartesian_X.Minimum = 0;
            Distance_Cartesian_X.Maximum = 360;
            Distance_Cartesian_X.Position = AxisPosition.Bottom;
            pm_Distance_Cartesian.Axes.Add(Distance_Cartesian_X);
            // add y-axis
            Distance_Cartesian_Y.MajorGridlineStyle = LineStyle.Solid;
            Distance_Cartesian_Y.MinorGridlineStyle = LineStyle.Dot;
            Distance_Cartesian_Y.Position = AxisPosition.Left;
            pm_Distance_Cartesian.Axes.Add(Distance_Cartesian_Y);
            // add series            
            pm_Distance_Cartesian.Series.Clear();
            Distance_Cartesian_Series.StrokeThickness = 2;
            Distance_Cartesian_Series.LineStyle = LineStyle.Solid;
            Distance_Cartesian_Series.Color = OxyColors.Red;
            pm_Distance_Cartesian.Series.Add(Distance_Cartesian_Series);
            gb_Distance.Controls.Add(pv_Distance_Cartesian);

        }

        private void btn_Horizon_Calculate_Click(object sender, EventArgs e)
        {
            // disable buttons
            btn_Horizon_Calculate.Enabled = false;
            btn_Horizon_Export.Enabled = false;
            // let the background worker do the rest
            bw_Horizon_Calculate.RunWorkerAsync();
        }

        private void btn_Horizon_Export_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog Dlg = new SaveFileDialog();
                Dlg.CheckPathExists = true;
                Dlg.FileName = Location.Call.ToUpper() + "_Horizon";
                if (Properties.Settings.Default.Elevation_SRTM1_Enabled)
                    Dlg.FileName = Dlg.FileName + "_SRTM1";
                if (Properties.Settings.Default.Elevation_SRTM3_Enabled)
                    Dlg.FileName = Dlg.FileName + "_SRTM3";
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled)
                    Dlg.FileName = Dlg.FileName + "_GLOBE";
                else
                    Dlg.FileName = Dlg.FileName + "_NONE";
                Dlg.FileName = Dlg.FileName + "_" + ElevationData.Database.GetDefaultStepWidth(GetElevationModel()).ToString("F0") + "m";
                Dlg.FileName = Dlg.FileName + ".csv";
                Dlg.DefaultExt = ".csv";
                if (Dlg.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(Dlg.FileName))
                    {
                        sw.WriteLine("Bearing[deg];Distance[km];Eps_Min[deg];Elevation[m]");
                        for (int i = 0; i < 360; i++)
                        {
                            sw.WriteLine(i.ToString() + ";" +
                                (Horizon.Horizon[i].Epsmin / Math.PI * 180).ToString("F8") + ";" +
                                Horizon.Horizon[i].Dist.ToString("F8") + ";" +
                                Horizon.Horizon[i].Elv.ToString("F8"));
                        }
                    }
                }
            }
            catch
            {
                // do nothing, if export is going wrong
            }
        }

        private void btn_Horizon_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HorizonDlg_SizeChanged(object sender, EventArgs e)
        {
            int width = this.Width - 50;
            gb_Elevation.Top = gb_Parameter.Height + 16;
            gb_Elevation.Left = 10;
            gb_Elevation.Width = width / 2;
            gb_Elevation.Height = this.Height - gb_Parameter.Height - pa_Buttons.Height - 70;
            gb_Distance.Top = gb_Parameter.Height + 16;
            gb_Distance.Left = gb_Elevation.Width + 30;
            gb_Distance.Width = width / 2;
            gb_Distance.Height = this.Height - gb_Parameter.Height - pa_Buttons.Height - 70;
            gb_Map.Left = 10;
            gb_Map.Top = gb_Parameter.Height + 16;
            gb_Map.Width = gb_Elevation.Width + gb_Distance.Width + 15;
            gb_Map.Height = this.Height - gb_Parameter.Height - pa_Buttons.Height - 70;
        }

        private void ClearAllDiagrams()
        {
            Elevation_Cartesian_Series.Points.Clear();
            Elevation_Polar_Series.Points.Clear();
            Distance_Cartesian_Series.Points.Clear();
            Distance_Cartesian_Series.Points.Clear();
            horizon.Clear();
        }

        private void DrawHorizonPoint(int azimuth, HorizonPoint hp)
        {
            Elevation_Polar_Series.Points.Add(new DataPoint(hp.Epsmin / Math.PI * 180.0, azimuth));
            pm_Elevation_Polar.InvalidatePlot(true);
            Elevation_Cartesian_Series.Points.Add(new DataPoint(azimuth, hp.Epsmin / Math.PI * 180.0));
            pm_Elevation_Cartesian.InvalidatePlot(true);
            Distance_Polar_Series.Points.Add(new DataPoint(hp.Dist, azimuth));
            pm_Distance_Polar.InvalidatePlot(true);
            Distance_Cartesian_Series.Points.Add(new DataPoint(azimuth, hp.Dist));
            pm_Distance_Cartesian.InvalidatePlot(true);
            LatLon.GPoint gp = LatLon.DestinationPoint(Location.Lat, Location.Lon, azimuth, hp.Dist);
            PointLatLng p = new PointLatLng(gp.Lat, gp.Lon);
            horizon.Points.Add(p);
            if (p.Lng < Map_Left)
                Map_Left = p.Lng;
            if (p.Lng > Map_Right)
                Map_Right = p.Lng;
            if (p.Lat < Map_Bottom)
                Map_Bottom = p.Lat;
            if (p.Lat > Map_Top)
                Map_Top = p.Lat;
            // toggle visible to redraw
            horizon.IsVisible = false;
            horizon.IsVisible = true;
            gm_Horizon.SetZoomToFitRect(RectLatLng.FromLTRB(Map_Left, Map_Top, Map_Right, Map_Bottom));
        }

        private void bw_Horizon_Calculate_DoWork(object sender, DoWorkEventArgs e)
        {
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = Name + "_" + this.GetType().Name;
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                // clear all diagrams
                ClearAllDiagrams();
                PropagationHorizonDesignator hd = PropagationData.Database.PropagationHorizonFindOrCreate(
                    bw_Horizon_Calculate,
                    Location.Lat,
                    Location.Lon,
                    Elevation + AntennaHeight,
                    CalculationDistance,
                    Bands.ToGHz(Properties.Settings.Default.Band),
                    LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor,
                    Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance,
                    ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                    Properties.Settings.Default.ElevationModel, LocalObstruction);
                st.Stop();
                bw_Horizon_Calculate.ReportProgress(-1, "Calculation of radio horizon of " + Location.Call + " finished, " + st.ElapsedMilliseconds + "ms.");
                // report complete horizon to main thread
                bw_Horizon_Calculate.ReportProgress(360, hd);
            }
            catch (Exception ex)
            {
                bw_Horizon_Calculate.ReportProgress(-1, ex.ToString());
            }

        }

        private void bw_Horizon_Calculate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                tsl_Main.Text = (string)e.UserState;
            }
            else if ((e.ProgressPercentage >= 0) && (e.ProgressPercentage <= 359))
            {
                // draw single horizon point when new calculation is running
                HorizonPoint hp = (HorizonPoint)e.UserState;
                DrawHorizonPoint(e.ProgressPercentage, hp);
            }
            else if (e.ProgressPercentage == 360)
            {
                // store complete horizon
                Horizon = (PropagationHorizonDesignator)e.UserState;
            }
        }

        private void bw_Horizon_Calculate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // clear all drawing and draw again when finished
            if (Horizon != null)
            {
                // clear all diagrams and maps
                ClearAllDiagrams();
                // draw all points
                for (int j = 0; j < 360; j++)
                {
                    DrawHorizonPoint(j, Horizon.Horizon[j]);
                }
                // draw first point again to close the circle
                DrawHorizonPoint(0, Horizon.Horizon[0]);
            }
            // enable radio buttons
            btn_Horizon_Calculate.Enabled = true;
            btn_Horizon_Export.Enabled = true;
        }

        private void HorizonDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            bw_Horizon_Calculate.CancelAsync();
        }

        private void rb_Horizon_Plot_Cartesian_Click(object sender, EventArgs e)
        {
            rb_Horizon_Plot_Cartesian.Checked = true;
            rb_Horizon_Plot_Polar.Checked = false;
            rb_Horizon_Plot_Map.Checked = false;
            UpdateCharts();
        }

        private void rb_Horizon_Plot_Polar_Click(object sender, EventArgs e)
        {
            rb_Horizon_Plot_Polar.Checked = true;
            rb_Horizon_Plot_Cartesian.Checked = false;
            rb_Horizon_Plot_Map.Checked = false;
            UpdateCharts();
        }

        private void rb_Horizon_Plot_Map_Click(object sender, EventArgs e)
        {
            rb_Horizon_Plot_Map.Checked = true;
            rb_Horizon_Plot_Cartesian.Checked = false;
            rb_Horizon_Plot_Polar.Checked = false;
            UpdateCharts();
        }

        private void UpdateCharts()
        {
            try
            {
                gb_Elevation.BringToFront();
                if (rb_Horizon_Plot_Polar.Checked)
                {
                    gb_Distance.Visible = true;
                    gb_Elevation.Visible = true;
                    gb_Map.Visible = false;
                    pv_Elevation_Polar.Visible = true;
                    pv_Elevation_Cartesian.Visible = false;
                    pv_Distance_Polar.Visible = true;
                    pv_Distance_Cartesian.Visible = false;
                }
                else if (rb_Horizon_Plot_Cartesian.Checked)
                {
                    gb_Distance.Visible = true;
                    gb_Elevation.Visible = true;
                    gb_Map.Visible = false;
                    pv_Elevation_Polar.Visible = false;
                    pv_Elevation_Cartesian.Visible = true;
                    pv_Distance_Polar.Visible = false;
                    pv_Distance_Cartesian.Visible = true;
                }
                else if (rb_Horizon_Plot_Map.Checked)
                {
                    gb_Distance.Visible = false;
                    gb_Elevation.Visible = false;
                    gb_Map.Visible = true;
                    gm_Horizon.Position = new PointLatLng(Location.Lat, Location.Lon);
                }
            }
            catch ( Exception ex)
            {
                MapDlg.Log.WriteMessage(ex.ToString());
            }
        }

        private void gm_Horizon_MouseDown(object sender, MouseEventArgs e)
        {
            // get geographic coordinates of mouse pointer
            PointLatLng p = gm_Horizon.FromLocalToLatLng(e.X, e.Y);
            Point loc = e.Location;
            loc.X += gm_Horizon.Cursor.Size.Width;
            loc.Y += gm_Horizon.Cursor.Size.Height;
            int elv = ElevationData.Database.ElvMissingFlag;
            // try to get elevation data from distinct elevation model
            // start with detailed one
            if (Properties.Settings.Default.Elevation_SRTM1_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[p.Lat, p.Lng, ELEVATIONMODEL.SRTM1, false];
            if (Properties.Settings.Default.Elevation_SRTM3_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[p.Lat, p.Lng, ELEVATIONMODEL.SRTM3, false];
            if (Properties.Settings.Default.Elevation_GLOBE_Enabled && (elv == ElevationData.Database.ElvMissingFlag))
                elv = ElevationData.Database[p.Lat, p.Lng, ELEVATIONMODEL.GLOBE, false];
            // set it to zero if still invalid
            if (elv == ElevationData.Database.ElvMissingFlag)
                elv = 0;
            double bearing = LatLon.Bearing(Location.Lat, Location.Lon, p.Lat, p.Lng);
            double dist = LatLon.Distance(Location.Lat, Location.Lon, p.Lat, p.Lng);
            TT.Show("Lat: " + p.Lat.ToString("F8", CultureInfo.InvariantCulture) + 
                "°\nLon: " + p.Lng.ToString("F8", CultureInfo.InvariantCulture) + 
                "°\nElv: " + elv.ToString() + 
                "m\n\nQRB: " + dist.ToString("F2", CultureInfo.InvariantCulture) + 
                "km\nQTF: " + bearing.ToString("F0",CultureInfo.InvariantCulture) + 
                "°", OwnerWin, loc);
        }

        private void gm_Horizon_MouseUp(object sender, MouseEventArgs e)
        {
            TT.Hide(OwnerWin);
        }
    }
}
