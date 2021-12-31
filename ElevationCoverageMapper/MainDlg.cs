using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using ScoutBase.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElevationCoverageMapper
{
    public partial class MainDlg : Form
    {
        GMapOverlay gm_tiles = new GMapOverlay("tiles");
        int count = 0;

        string Model = "GLOBE";
        Color CoveredColor = Color.Blue;
        public MainDlg()
        {
            InitializeComponent();
            // set initial settings for main map
            gm_Coverage.MapProvider = GMapProviders.OpenStreetMap;
            gm_Coverage.IgnoreMarkerOnMouseWheel = true;
            gm_Coverage.MinZoom = 0;
            gm_Coverage.MaxZoom = 20;
            gm_Coverage.Zoom = 2;
            gm_Coverage.DragButton = System.Windows.Forms.MouseButtons.Left;
            gm_Coverage.CanDragMap = true;
            gm_Coverage.ScalePen = new Pen(Color.Black, 3);
            gm_Coverage.MapScaleInfoEnabled = true;
            gm_Coverage.Overlays.Add(gm_tiles);
            gm_Coverage.ShowCenter = false;
            gm_Coverage.ScalePen = Pens.Transparent;
        }


        private void MainDlg_Load(object sender, EventArgs e)
        {
            gm_Coverage.Visible = false;
            ss_Main.Visible = true;

            Application.DoEvents();

            count = 0;
            MapUpdaterDoEventArgs args = new MapUpdaterDoEventArgs();
            args.Model = Model;

            bw_MapUpdater.RunWorkerAsync(args);
        }

        private void MainDlg_Shown(object sender, EventArgs e)
        {
        }

        private void gm_Coverage_OnTileLoadComplete(long ElapsedMilliseconds)
        {
//            WindowsFormsControlInvoke.Invoke(this, MainDlg_MapComplete);
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void MainDlg_Resize(object sender, EventArgs e)
        {
//            gm_Coverage.Height = this.Height - 50;
        }

        private void Say(string msg)
        {
            if (tsl_Status.Text != msg)
            {
                tsl_Status.Text = msg;
                ss_Main.Refresh();
            }
        }

        #region MapUpdater

        private void bw_MapUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            MapUpdaterDoEventArgs args = (MapUpdaterDoEventArgs)e.Argument;
            string url = "http://airscout.eu/downloads/ScoutBase/1/ElevationData/" + args.Model + "/files.zip";
            string zipfilename = "files.zip";
            string filename = "files.cat";
            bw_MapUpdater.ReportProgress(0, args.Model + ": Downloading elevation tile catalogue from " + url);
            if (File.Exists(zipfilename))
            {
                File.Delete(zipfilename);
            }
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            AutoDecompressionWebClient client = new AutoDecompressionWebClient();
            DOWNLOADFILESTATUS status = client.DownloadFileIfNewer(url, zipfilename, true, true);
            bw_MapUpdater.ReportProgress(0, args.Model + ": Creating elevation tile catalogue...");
            // get locs catalogue from web
            BackgroundWorker caller = bw_MapUpdater;
            List<string> availabletiles = new List<string>();

            if (File.Exists(filename))
            {
                // read catalogue and fill LastUpdated timestamp
                using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                {
                    int i = 0;
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        try
                        {
                            if (!String.IsNullOrEmpty(s) && !s.StartsWith("/"))
                            {
                                string[] a = s.Split(';');
                                DateTime lastupdated;
                                string square = a[0].ToUpper();
                                string dummy;
                                if (caller != null)
                                {
                                    if ((caller.WorkerReportsProgress) && (i % 1000 == 0))

                                        caller.ReportProgress(0, args.Model + ": Updating elevation tile information [" + i.ToString() + "], please wait...");
                                }
                                availabletiles.Add(square);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        i++;
                    }
                }
            }

            bw_MapUpdater.ReportProgress(0, args.Model + ": Processing tiles...");
            int missing = 0;
            int found = 0;
            foreach (string tilename in availabletiles)
            {
                bw_MapUpdater.ReportProgress(1, tilename);
                if (bw_MapUpdater.CancellationPending)
                {
                    bw_MapUpdater.ReportProgress(0, args.Model + ": Processing cancelled...");
                    return;
                }
            }
            bw_MapUpdater.ReportProgress(0, args.Model + ": " + found.ToString() + " tile(s) found, " + missing.ToString() + " more tile(s) available and missing.");
        }

        private void bw_MapUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                string loc = (string)e.UserState;
                int x = 0;
                int y = 0;
                try
                {

                    // add a tile found in database to map polygons
                    double baselat;
                    double baselon;
                    MaidenheadLocator.LatLonFromLoc(loc.Substring(0, 6), PositionInRectangle.BottomLeft, out baselat, out baselon);
                    List<PointLatLng> l = new List<PointLatLng>();
                    l.Add(new PointLatLng((decimal)baselat, (decimal)baselon));
                    l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)baselon));
                    l.Add(new PointLatLng((decimal)(baselat + 1 / 24.0), (decimal)(baselon + 2 / 24.0)));
                    l.Add(new PointLatLng((decimal)baselat, (decimal)(baselon + 2 / 24.0)));
                    GMapPolygon p = new GMapPolygon(l, loc);
                    p.Stroke = new Pen(Color.FromArgb(50, CoveredColor));
                    p.Fill = new SolidBrush(Color.FromArgb(50, CoveredColor));
                    gm_tiles.Polygons.Add(p);
                    if (count % 1000 == 0)
                    {
                        Say("Procesing tile " + count + "...");
                    }
                    count++;
                }
                catch (Exception ex)
                {
                    Say("Error while processing tile [" + loc + "," + x + "," + y + " ]: " + ex.Message);
                }
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
                gm_tiles.Polygons.Add(p);
            }
            else
            {
                Say((string)e.UserState);
            }

        }

        private void bw_MapUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gm_Coverage.Visible = true;
        }

        #endregion

    }

    public class MapUpdaterDoEventArgs
    {
        public string Model = "";
    }
}
