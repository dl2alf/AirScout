using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using AirScout.Core;
using AirScout.Aircrafts;
using ScoutBase.Core;
using ScoutBase.Stations;
using ScoutBase.Elevation;
using ScoutBase.Propagation;
using AirScout.AircraftPositions;
using AirScout.Signals;

namespace AirScout
{
    public partial class CrossingHistoryDlg : Form
    {

        DateTime From;
        DateTime To;
        int Stepwidth;
        List<AircraftPositionDesignator> AllPositions;
        List<PlaneInfo> NearestPositions = new List<PlaneInfo>();

        PropagationPathDesignator PPath;

        const int TOOLTIP_XOFFSET =3;
        const int TOOLTIP_YOFFSET = 3;

        List<PlaneInfo> Crossings = new List<PlaneInfo>();

        public CrossingHistoryDlg(DateTime from, DateTime to, int stepwidth, ref List<AircraftPositionDesignator> alllpositions)
        {
            InitializeComponent();
            From = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0, 0, from.Kind);
            DateTime oldest = AircraftPositionData.Database.AircraftPositionOldestEntry();
            if (From < oldest)
                From = oldest;
            To = to;
            // check bounds
            if (From > To)
                From = To;
            Stepwidth = stepwidth;
            AllPositions = alllpositions;
            // set minimum stepwidth to 1sec
            if (Stepwidth <= 0)
                Stepwidth = 1;
            this.Text = "Path Crossing History: " + Properties.Settings.Default.MyCall + " >>> " + Properties.Settings.Default.DXCall;
            btn_History_Export.Enabled = false;

        }

        private void bw_History_DoWork(object sender, DoWorkEventArgs e)
        {
            // check that Stepwidth ist positive in any case
            if (Properties.Settings.Default.Path_History_StepWidth <= 0)
                Properties.Settings.Default.Path_History_StepWidth = 1;
            bw_History.ReportProgress(0, "Calculating Path....");
            LocationDesignator mycall = StationData.Database.LocationFindOrCreate(Properties.Settings.Default.MyCall, MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, false, 3));
            QRVDesignator myqrv = StationData.Database.QRVFindOrCreateDefault(mycall.Call, mycall.Loc, Properties.Settings.Default.Band);
            // set qrv defaults if zero
            if (myqrv.AntennaHeight == 0)
                myqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
            if (myqrv.AntennaGain == 0)
                myqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
            if (myqrv.Power == 0)
                myqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);
            if (Properties.Settings.Default.Path_BestCaseElevation)
            {
                if (!MaidenheadLocator.IsPrecise(mycall.Lat, mycall.Lon, 3))
                {
                    ElvMinMaxInfo maxinfo = ElevationData.Database.ElevationTileFindMinMaxInfo(mycall.Loc, Properties.Settings.Default.ElevationModel);
                    if (maxinfo != null)
                    {
                        mycall.Lat = maxinfo.MaxLat;
                        mycall.Lon = maxinfo.MaxLon;
                    }
                }
            }
            LocationDesignator dxcall = StationData.Database.LocationFindOrCreate(Properties.Settings.Default.DXCall, MaidenheadLocator.LocFromLatLon(Properties.Settings.Default.DXLat, Properties.Settings.Default.DXLon, false, 3));
            QRVDesignator dxqrv = StationData.Database.QRVFindOrCreateDefault(dxcall.Call, dxcall.Loc, Properties.Settings.Default.Band);
            // set qrv defaults if zero
            if (dxqrv.AntennaHeight == 0)
                dxqrv.AntennaHeight = StationData.Database.QRVGetDefaultAntennaHeight(Properties.Settings.Default.Band);
            if (dxqrv.AntennaGain == 0)
                dxqrv.AntennaGain = StationData.Database.QRVGetDefaultAntennaGain(Properties.Settings.Default.Band);
            if (dxqrv.Power == 0)
                dxqrv.Power = StationData.Database.QRVGetDefaultPower(Properties.Settings.Default.Band);
            if (Properties.Settings.Default.Path_BestCaseElevation)
            {
                if (!MaidenheadLocator.IsPrecise(dxcall.Lat, dxcall.Lon, 3))
                {
                    ElvMinMaxInfo maxinfo = ElevationData.Database.ElevationTileFindMinMaxInfo(dxcall.Loc, Properties.Settings.Default.ElevationModel);
                    if (maxinfo != null)
                    {
                        dxcall.Lat = maxinfo.MaxLat;
                        dxcall.Lon = maxinfo.MaxLon;
                    }
                }
            }

            // find local obstruction, if any
            LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(mycall.Lat, mycall.Lon, Properties.Settings.Default.ElevationModel);
            double mybearing = LatLon.Bearing(mycall.Lat, mycall.Lon, dxcall.Lat, dxcall.Lon);
            double myobstr = (o != null) ? o.GetObstruction(myqrv.AntennaHeight, mybearing) : double.MinValue;
            
            // try to find propagation path in database or create new one and store
            PPath = PropagationData.Database.PropagationPathFindOrCreateFromLatLon(
                bw_History,
                mycall.Lat,
                mycall.Lon,
                ElevationData.Database[mycall.Lat, mycall.Lon,Properties.Settings.Default.ElevationModel] + myqrv.AntennaHeight,
                dxcall.Lat,
                dxcall.Lon,
                ElevationData.Database[dxcall.Lat, dxcall.Lon,Properties.Settings.Default.ElevationModel] + dxqrv.AntennaHeight,
                Bands.ToGHz(Properties.Settings.Default.Band),
                LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].K_Factor,
                Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].F1_Clearance,
                ElevationData.Database.GetDefaultStepWidth(Properties.Settings.Default.ElevationModel),
                Properties.Settings.Default.ElevationModel,
                myobstr);
            DateTime time = From;
            lock (Crossings)
            {
                Crossings.Clear();
            }
            lock(NearestPositions)
            {
                NearestPositions.Clear();
            }
            // pre-select nearest positions only
            bw_History.ReportProgress(0, "Pre-selecting nearest positions...");
            LatLon.GPoint midpoint = PPath.GetMidPoint();
            double maxdist = PPath.Distance / 2;
            foreach (AircraftPositionDesignator ap in AllPositions)
            {
                if ((ap.LastUpdated >= From) && (ap.LastUpdated <= To) && (LatLon.Distance(ap.Lat, ap.Lon, midpoint.Lat, midpoint.Lon) <= maxdist))
                {
                    AircraftDesignator ac = null;
                    AircraftTypeDesignator at = null;
                    ac = AircraftData.Database.AircraftFindByHex(ap.Hex);
                    if (ac != null)
                        at = AircraftData.Database.AircraftTypeFindByICAO(ac.TypeCode);
                    PlaneInfo plane = new PlaneInfo(ap.LastUpdated, ap.Call, ((ac != null) && (!String.IsNullOrEmpty(ac.TypeCode)))? ac.Reg : "[unknown]", ap.Hex, ap.Lat, ap.Lon, ap.Track, ap.Alt, ap.Speed, (ac != null) && (!String.IsNullOrEmpty(ac.TypeCode)) ? ac.TypeCode : "[unkomwn]", ((at != null) && (!String.IsNullOrEmpty(at.Manufacturer))) ? at.Manufacturer : "[unknown]", ((at != null) && (!String.IsNullOrEmpty(at.Model))) ? at.Model : "[unknown]", (at != null) ? at.Category : PLANECATEGORY.NONE);
                    lock (NearestPositions)
                    {
                        NearestPositions.Add(plane);
                    }
                    if (NearestPositions.Count % 1000 == 0)
                        bw_History.ReportProgress(0, "Pre-selecting nearest positions..." + "[" + NearestPositions.Count.ToString() + "]");
                }
                if (bw_History.CancellationPending)
                    break;
            }
            bw_History.ReportProgress(0, "Pre-selecting nearest positions finished, " + NearestPositions.Count.ToString() + " positions.");
            // return if no positions left over
            if (NearestPositions.Count == 0)
                return;
            int startindex = 0;
            // set timeline to first reported position
            time = NearestPositions[0].Time;
            while ((!bw_History.CancellationPending) && (time <= To))
            {
                if (Crossings.Count % 1000 == 0)
                    bw_History.ReportProgress(0, "Calculating at " + time.ToString("yyyy-MM-dd HH:mm:ss") + ", " + Crossings.Count.ToString() + " crossings so far.");
                // calculate from timestamp
                DateTime from = time.AddMinutes(-Properties.Settings.Default.Planes_Position_TTL);
                // fill plane position cache
                PlaneInfoCache ac = new PlaneInfoCache();
                int i = startindex;
                startindex = -1;
                while ((!bw_History.CancellationPending) && (i < NearestPositions.Count))
                {
                    // update ap in cache if relevant
                    if (NearestPositions[i].Time >= from)
                    {
                        // store first index as startindex for next iteration
                        if (startindex == -1)
                            startindex = i;
                        lock (ac)
                        {
                            ac.InsertOrUpdateIfNewer(NearestPositions[i]);
                        }
                    }
                    // stop if position is newer than current time
                    if (NearestPositions[i].Time > time)
                        break;
                    i++;
                }
                List<PlaneInfo> allplanes = ac.GetAll(time, Properties.Settings.Default.Planes_Position_TTL);
                // get nearest planes
                List<PlaneInfo> nearestplanes = AircraftData.Database.GetNearestPlanes(time, PPath, allplanes, Properties.Settings.Default.Planes_Filter_Max_Circumcircle, Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].MaxDistance, Properties.Settings.Default.Planes_MaxAlt);
                if ((nearestplanes != null) && (nearestplanes.Count() > 0))
                {
                    // get all planes crossing the path
                    foreach (PlaneInfo plane in nearestplanes)
                    {
                        if (plane.IntQRB <= Properties.Settings.Default.Path_Band_Settings[Properties.Settings.Default.Band].MaxDistance)
                        {
                            // check if level value is available
                            SignalLevelDesignator ad = SignalData.Database.SignalLevelFind(plane.Time);
                            if (ad != null)
                                plane.SignalStrength = ad.Level;
                            else
                                plane.SignalStrength = double.MinValue;
                            lock (Crossings)
                            {
                                if (!Properties.Settings.Default.Analysis_CrossingHistory_WithSignalLevel || (ad != null))
                                    Crossings.Add(plane);
                            }
                        }
                        bw_History.ReportProgress(0, "Calculating at " + time.ToString("yyyy-MM-dd HH:mm:ss") + ", " + Crossings.Count.ToString() + " crossings so far.");
                    }
                }
                time = time.AddSeconds(Stepwidth);
            }
            bw_History.ReportProgress(100, "Calculation done.");
        }

        private void bw_History_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                tsl_Main.Text = (string)e.UserState;
//                ss_Main.Refresh();
            }
            else if (e.ProgressPercentage == 100)
            {
                tsl_Main.Text = (string)e.UserState;
                // this.Refresh();
                ch_Crossing_History.Show();
            }
        }

        private void bw_History_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!bw_History.CancellationPending)
                btn_History_Export.Enabled = true;
        }

        private void btn_History_Calculate_Click(object sender, EventArgs e)
        {
            btn_History_Calculate.Enabled = false;
            // drwaing charts
            ch_Crossing_History.Series.Clear();
            ch_Crossing_History.Series.Add("Count_Lo");
            ch_Crossing_History.Series["Count_Lo"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            ch_Crossing_History.Series["Count_Lo"].Color = Color.Blue;
            ch_Crossing_History.Series["Count_Lo"].BorderWidth = 2;
            ch_Crossing_History.Series["Count_Lo"].BackSecondaryColor = Color.Blue;
            ch_Crossing_History.Series["Count_Lo"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            ch_Crossing_History.Series["Count_Lo"]["PointWidth"] = "1";
            ch_Crossing_History.Series.Add("Count_Hi");
            ch_Crossing_History.Series["Count_Hi"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            ch_Crossing_History.Series["Count_Hi"].Color = Color.Red;
            ch_Crossing_History.Series["Count_Hi"].BorderWidth = 2;
            ch_Crossing_History.Series["Count_Hi"].BackSecondaryColor = Color.Red;
            ch_Crossing_History.Series["Count_Hi"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            ch_Crossing_History.Series["Count_Hi"]["PointWidth"] = "1";

            ch_Crossing_History.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd\nHH:mm:ss";
            bw_History.RunWorkerAsync();
        }

        private void btn_History_Export_Click(object sender, EventArgs e)
        {
            if (PPath == null)
                return;
            if (Crossings == null)
                return;
            if (Crossings.Count() <= 0)
                return;
            tsl_Main.Text = "Checking ambiguous crossings...";
            int start = 0;
            Dictionary<string, PlaneInfo> p = new Dictionary<string, PlaneInfo>();
            for (int i = 1; i < Crossings.Count; i++)
            {
//                if ((Crossings[i].Call == "DLH2LJ") && (Crossings[i].Time.Day == 26))
//                    Console.WriteLine("");
                Crossings[i].Ambiguous = false;
                PlaneInfo pl = null;
                if (!p.TryGetValue(Crossings[i].Hex, out pl))
                    p.Add(Crossings[i].Hex, Crossings[i]);
                if (((Crossings[i].Time - Crossings[i - 1].Time).TotalSeconds > (double)Properties.Settings.Default.Analysis_CrossingHistory_AmbigousGap) || (i >= Crossings.Count - 1))
                {
                    // gap detected
                    if (p.Count > 1)
                    {
                        // multiple planes detected --> mark all as ambiguous
                        for (int j = start; j <= i - 1; j++)
                            Crossings[j].Ambiguous = true;
                    }
                    // reset all and start new crossing
                    p.Clear();
                    start = i;
                }
            }
            // mark last plane as ambiguous in any case
            Crossings[Crossings.Count - 1].Ambiguous = true;
            CultureInfo uiculture = CultureInfo.CurrentUICulture;
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.AddExtension = true;
            Dlg.DefaultExt = "csv";
            Dlg.FileName = "Path Crossing History_" + Properties.Settings.Default.MyCall + "_" + Properties.Settings.Default.DXCall + "_" + From.ToString("yyyyMMdd_HHmmss") + "_to_" + To.ToString("yyyyMMdd_HHmmss");
//            Dlg.InitialDirectory = Application.StartupPath;
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(Dlg.FileName))
                {
                    sw.WriteLine("time[utc];call;lat[deg];lon[deg];type;category;potential;track[deg];alt[m];altdiff[m];dist[km];crossangle[deg];squint[deg];dist1[km];dist2[km];eps1[deg];eps2[deg];theta1[deg];theta2[deg];signal_strength[dB];ambiguous");
                    foreach (PlaneInfo plane in Crossings)
                    {
                        double a1 = LatLon.Distance(PPath.Lat1, PPath.Lon1, plane.Lat, plane.Lon);
                        double a2 = LatLon.Distance(PPath.Lat2, PPath.Lon2, plane.Lat, plane.Lon);
                        double sl = plane.SignalStrength;
                        if (sl == double.MinValue)
                            sl = -255;
                        sw.WriteLine(plane.Time.ToString("yyyy-MM-dd HH:mm:ss") + ";" +
                            plane.Call + ";" +
                            plane.Lat.ToString("F8", uiculture) + ";" +
                            plane.Lon.ToString("F8", uiculture) + ";" +
                            plane.Type + ";" +
                            plane.Category.ToString() + ";" +
                            plane.Potential.ToString() + ";" +
                            plane.Track.ToString("F8", uiculture) + ";" +
                            plane.Alt_m.ToString("F8", uiculture) + ";" +
                            plane.AltDiff.ToString("F8", uiculture) + ";" +
                            plane.IntQRB.ToString("F8", uiculture) + ";" +
                            (plane.Angle / Math.PI * 180.0).ToString("F8", uiculture) + ";" +
                            (plane.Squint / Math.PI * 180.0).ToString("F8", uiculture) + ";" +
                            a1.ToString("F8", uiculture) + ";" +
                            a2.ToString("F8", uiculture) + ";" +
                            (plane.Eps1 / Math.PI * 180.0).ToString("F8", uiculture) + ";" +
                            (plane.Eps2 / Math.PI * 180.0).ToString("F8", uiculture) + ";" +
                            (plane.Theta1 / Math.PI * 180.0).ToString("F8", uiculture) + ";" +
                            (plane.Theta2 / Math.PI * 180.0).ToString("F8", uiculture) + ";" +
                            sl.ToString("F2", uiculture) + ";" +
                            plane.Ambiguous.ToString());
                    }
                }
                tsl_Main.Text = "Export done.";
            }
        }

        private void CrossingHistoryDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            bw_History.CancelAsync();
        }

        private void btn_History_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ch_Crossing_History_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
