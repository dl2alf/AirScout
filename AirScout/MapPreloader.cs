using ScoutBase.Core;
using ScoutBase.Elevation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using System.Data.SQLite;
using System.IO;
using System.Data;
using ScoutBase.Maps;
using System.Net;

namespace AirScout
{
    public partial class MapDlg : Form
    {

        #region MapPreloader

        class MapPreloaderTile
        {
            public int X;
            public int Y;
            public int Z;
            public int Type;

            public MapPreloaderTile(int x, int y, int z, int type)
            {
                X = x;
                Y = y;
                Z = z;
                Type = type;
            }
        }

        // Background worker for preloading of map tiles
        [DefaultPropertyAttribute("Name")]
        public class MapPreloader : BackgroundWorker
        {
            string Name = "MapPreloader";
            ELEVATIONMODEL Model = ELEVATIONMODEL.NONE;
            GMapControl gm_Map = new GMapControl();
            System.Data.SQLite.SQLiteDatabase db;

            public MapPreloader()
            {
                this.WorkerReportsProgress = true;
                this.WorkerSupportsCancellation = true;
            }

            int long2tilex(double lon, int z)
            {
                return (int)(Math.Floor((lon + 180.0) / 360.0 * (1 << z)));
            }

            int lat2tiley(double lat, int z)
            {
                return (int)Math.Floor((1 - Math.Log(Math.Tan((lat / 180.0 * Math.PI)) + 1 / Math.Cos((lat / 180.0 * Math.PI))) / Math.PI) / 2 * (1 << z));
            }

            double tilex2long(int x, int z)
            {
                return x / (double)(1 << z) * 360.0 - 180;
            }

            double tiley2lat(int y, int z)
            {
                double n = Math.PI - 2.0 * Math.PI * y / (double)(1 << z);
                return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
            }

            private bool LoadOSM (int x, int y , int zoom)
            {
                bool b = false;
                string url = ScoutBase.Maps.Properties.Settings.Default.Database_UpdateURL + zoom.ToString() + "/" + x.ToString() + "/" + y.ToString() + ".png";
                string filename = Path.Combine(Path.GetTempPath(), y.ToString() + ".png");
                try
                {
                    WebClient cl = new WebClient();
                    cl.DownloadFile(url, filename);
                    using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
                    {
                        byte[] buf = br.ReadBytes((int)br.BaseStream.Length);
                        //                        int id = MapData.Database.TileInsert(x, y, zoom, gm_Map.MapProvider.DbId, File.GetLastWriteTime(filename), buf);
                        int dbid = GMapProviders.OpenStreetMap.DbId;
                        GMaps.Instance.EnqueueCacheTask(new GMap.NET.Internals.CacheQueueItem(new GMap.NET.Internals.RawTile(dbid, new GPoint(x, y), zoom), buf,GMap.NET.Internals.CacheUsage.First));
                    }
                    File.Delete(filename);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(this.Name + ": " + ex.ToString());
                }
                return b;
            }

            protected override void OnDoWork(DoWorkEventArgs e)
            {
                BACKGROUNDUPDATERSTARTOPTIONS Options = (BACKGROUNDUPDATERSTARTOPTIONS)e.Argument;
                // name the thread for debugging
                if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = this.Name + "_" + this.GetType().Name;
                this.ReportProgress(0, this.Name + " started.");
                Log.WriteMessage(this.Name + " started.");
                // get update interval
                int interval = (int)Properties.Settings.Default.Background_Update_Period * 60;
                // get mpst simple elevation model
                if (Properties.Settings.Default.Elevation_GLOBE_Enabled)
                    Model = ELEVATIONMODEL.GLOBE;
                else if (Properties.Settings.Default.Elevation_SRTM3_Enabled)
                    Model = ELEVATIONMODEL.SRTM3;
                else if (Properties.Settings.Default.Elevation_SRTM1_Enabled)
                    Model = ELEVATIONMODEL.SRTM1;
                // return if no elevation model selected
                if (Model == ELEVATIONMODEL.NONE)
                    return;
                // setting User Agent to fix Open Street Map issue 2016-09-20
                GMap.NET.MapProviders.GMapProvider.UserAgent = "AirScout";
                // clearing referrer URL issue 2019-12-14
                gm_Map.MapProvider.RefererUrl = "";
                // set initial settings for main map
                gm_Map.MapProvider = GMapProviders.Find(Properties.Settings.Default.Map_Provider);
                gm_Map.MinZoom = 0;
                gm_Map.MaxZoom = 20;
                // get database filename
                int i = 0;
                int count = 0;
                int total = 0;
                do
                {
                    i = 0;
                    count = 0;
                    total = 0;
                    // checks if elevation database is complete
                    try
                    {
                        this.ReportProgress(0, this.Name + " getting tiles from database.");
                        int zmin = 5;
                        int zmax = 11;
                        List<MapPreloaderTile> l = new List<MapPreloaderTile>();
                        for (int z = zmin; z <= zmax; z++)
                        {
                            int xmin = long2tilex(Properties.Settings.Default.MinLon, z);
                            int xmax = long2tilex(Properties.Settings.Default.MaxLon, z);
                            int ymin = lat2tiley(Properties.Settings.Default.MaxLat, z);
                            int ymax = lat2tiley(Properties.Settings.Default.MinLat, z);
                            for (int x = xmin; x <= xmax; x++)
                            {
                                for (int y = ymin; y <= ymax; y++)
                                {
                                    // check if tile already in database --> add it to list to get it from the web
                                    if (!MapData.Database.TileExists(x, y, z, gm_Map.MapProvider.DbId))
                                    {
                                        MapPreloaderTile t = new MapPreloaderTile(x, y, z, gm_Map.MapProvider.DbId);
                                        l.Add(t);
                                    }
                                    total++;
                                    if (this.CancellationPending)
                                        break;
                                }
                                if (this.CancellationPending)
                                    break;
                            }
                            if (this.CancellationPending)
                                break;
                        }
                        if (this.CancellationPending)
                            break;
                        count = l.Count();
                        Random rng = new Random();
                        // shuffle the list
                        int n = l.Count;
                        while (n > 1)
                        {
                            n--;
                            int k = rng.Next(n + 1);
                            MapPreloaderTile value = l[k];
                            l[k] = l[n];
                            l[n] = value;
                        }
                        n = 0;
                        foreach (MapPreloaderTile t in l)
                        {
                            Exception ex = null;
                            this.ReportProgress(0, "Preloading " + "/" + t.Z.ToString() + "/" + t.X.ToString() + "/" + t.Y.ToString() + ".png");
                            try
                            {
                                // try to donwload from www.airscout.eu first
                                if (gm_Map.MapProvider.GetType() == typeof(OpenStreetMapProvider))
                                {
                                    LoadOSM(t.X, t.Y, t.Z);
                                }
                                else
                                {
                                    PureImage img = gm_Map.Manager.GetImageFrom(gm_Map.MapProvider, new GPoint(t.X, t.Y), t.Z, out ex);
                                    // wait until cache is written to database
                                }
                                if (ex == null)
                                    Console.WriteLine("Preload tile [" + i.ToString() + " of " + count.ToString() + "] x=" + t.X + ", y=" + t.Y + ", z=" + t.Z + ": OK");
                                else
                                    Console.WriteLine("Preload tile [" + i.ToString() + " of " + count.ToString() + "] x=" + t.X + ", y=" + t.Y + ", z=" + t.Z + ": " + ex.ToString());
                            }
                            catch (Exception e1)
                            {
                                Console.WriteLine(this.Name + ": " + e1.ToString());
                            }
                            Thread.Sleep(100);
                            i++;
                            n++;
                            if (n > 100)
                            {
                                while (GMaps.Instance.tileCacheQueue.Count > 0)
                                {
                                    Application.DoEvents();
                                    if (this.CancellationPending)
                                        break;
                                }
                                n = 0;
                            }
                            if (this.CancellationPending)
                                break;
                        }
                        if (this.CancellationPending)
                            break;
                    }
                    catch (Exception ex)
                    {
                        this.ReportProgress(-1, ex.ToString());
                    }

                    // sleep when running periodically
                    if (Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY)
                    {
                        int l = 0;
                        while (!this.CancellationPending && (l < interval))
                        {
                            Thread.Sleep(1000);
                            l++;
                        }
                    }
                    if (this.CancellationPending)
                        break;
                }
                while (Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (this.CancellationPending)
                {
                    this.ReportProgress(0, Name + " cancelled.");
                    Log.WriteMessage(Name + " cancelled.");
                }
                else
                {
                    this.ReportProgress(0, Name + " finished, total  " + total.ToString() + " tile(s), " + (count-i).ToString() + " left.");
                    Log.WriteMessage(Name + " finished, total  " + total.ToString() + " tile(s), " + (count - i).ToString() + " left.");
                }
            }

        }


     #endregion

    }
}
