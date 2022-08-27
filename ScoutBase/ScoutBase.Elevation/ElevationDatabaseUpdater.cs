using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoutBase.Core;
using ScoutBase.Stations;
using ScoutBase.Elevation;
using System.Data.SQLite;
using System.Reflection;

namespace ScoutBase.Elevation
{

    #region ElevationDatabaseUpdater

    public class ElevationDatabaseUpdaterStartOptions
    {
        public string Name;
        public BACKGROUNDUPDATERSTARTOPTIONS Options;
        public ELEVATIONMODEL Model;
        public double MinLat;
        public double MinLon;
        public double MaxLat;
        public double MaxLon;
        public bool FileCacheEnabled;
    }

    // Background worker for elevation database update
    [DefaultPropertyAttribute("Name")]
    public class ElevationDatabaseUpdater : BackgroundWorker
    {
        ElevationDatabaseUpdaterStartOptions StartOptions;

        // Temp directory to save downloaded files
        string TmpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName, "Tmp", "ElevationData").TrimEnd(Path.DirectorySeparatorChar);

        public ElevationDatabaseUpdater() : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            // create temp directory if not exists
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
        }

        // TRICKY: process a single elevation tile.
        // does check the database for elevation tile is already inside
        // returns true + elevation tile to collect tile for bulk insert
        // return true + null if elevation tile found and updated
        // returns false + null in case of errors
        private bool UpdateElevationTileFromURL(string tilename, DateTime lastupdated, ELEVATIONMODEL model, out ElevationTileDesignator tile)
        {
            try
            {
                // this.ReportProgress((int)DATABASESTATUS.UPDATING, "Processing " + tilename + "...");
                DateTime tilelastupdated = ElevationData.Database.ElevationTileFindLastUpdated(new ElevationTileDesignator(tilename.Substring(0,6).ToUpper()), model);
                // elevation tile found --> tilelastupdated contains timedstamp
                // elevation tile not found --> tilelastupdated = DateTime.MinValue
                TimeSpan diff = lastupdated - tilelastupdated;
                // check if catalogue tile is newer
                if (diff.TotalMinutes > 5)
                {
                    // download elevation zip file and unzip
                    string square = tilename.Substring(0, 4).ToUpper();
                    string zipfilename = Path.Combine(ElevationData.Database.DefaultDatabaseDirectory(model), square + ".zip");
                    string zipurl = ElevationData.Database.UpdateURL(model) + "/" + tilename.Substring(0, 2) + "/" + tilename.Substring(0, 4) + ".zip";
                    string filename = Path.Combine(ElevationData.Database.DefaultDatabaseDirectory(model), tilename);
                    if (!File.Exists(filename))
                    {
                        this.ReportProgress(0, StartOptions.Name + ": downloading " + Path.GetFileName(zipfilename) + "...");
                        Console.WriteLine(StartOptions.Name + ": downloading " + zipurl + " --> " + zipfilename);
                        try
                        {
                            // download zipfile if newer
                            AutoDecompressionWebClient client = new AutoDecompressionWebClient();
                            DOWNLOADFILESTATUS status = client.DownloadFileIfNewer(zipurl, zipfilename, true, true);
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress(-1, ex.ToString());
                            try
                            {
                                // try to delete zip file anyway
                                File.Delete(zipfilename);
                            }
                            catch
                            {

                            }
                        }
                        try
                        {
                            // delete zipfile if cache is disabled
                            if (!StartOptions.FileCacheEnabled)
                                File.Delete(zipfilename);
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress(-1, "Error deleting zipfile [" + zipfilename + "]: " + ex.ToString());
                        }
                        // new zip file extracted, assuming that the remaining *.loc files are orphans
                        // --> try to delete everything but current square and catalogue
                        // cleanup all *.loc files
                        foreach (string f in Directory.EnumerateFiles(ElevationData.Database.DefaultDatabaseDirectory(model), "*.loc"))
                        {
                            try
                            {
                                if (!f.Contains(square))
                                    File.Delete(f);
                            }
                            catch (Exception ex)
                            {
                                this.ReportProgress(-1, "Error deleting locfile [" + f + "]: " + ex.ToString());
                            }
                        }
                    }
                    // wait at last 60sec for file is being unzipped or throw FileNotFOundException if not
                    // unzip procedure is sometimes returning the results with delay
                    int timeout = 0;
                    while (!File.Exists(filename))
                    {
                        if (timeout > 600)
                            throw new FileNotFoundException("Elevation file not found. " + filename);
                        Thread.Sleep(100);
                        if (this.CancellationPending)
                            break;
                        timeout++;
                    }
                    string json = "";
                    using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
                    {
                        json = sr.ReadToEnd();
                    }
                    if (!string.IsNullOrEmpty(json))
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        settings.FloatFormatHandling = FloatFormatHandling.String;
                        settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        tile = JsonConvert.DeserializeObject<ElevationTileDesignator>(json, settings);
                        // perform a single update if elevation tile was already found in database
                        if (tilelastupdated != DateTime.MinValue)
                        {
                            ElevationData.Database.ElevationTileUpdate(tile, model);
                            tile = null;
                        }
                        // return tile to be collected for bulk insert in main procedure
                        return true;
                    }
                    File.Delete(filename);
                }
                else
                {
                    // tile found and up to date --> nothing to do
                    tile = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, ex.ToString() + ": tilename=" + tilename);
            }
            tile = null;
            return false;
        }

        private int UpdateDatabase()
        {
            // updates elevation database
            int errors = 0;
            int bulkmax = 400;
            try
            {
                // get all locs needed for covered area
                // try to read cached catalogue first
                ElevationCatalogue cat = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(null, StartOptions.Model, StartOptions.MinLat, StartOptions.MinLon, StartOptions.MaxLat, StartOptions.MaxLon);
                // process tiles
                List<ElevationTileDesignator> tiles = new List<ElevationTileDesignator>();
                foreach (KeyValuePair<string, DateTime> availabletile in cat.Files)
                {
                    // collect tiles for bulk insert
                    ElevationTileDesignator t;
                    bool b = UpdateElevationTileFromURL(availabletile.Key, availabletile.Value, StartOptions.Model, out t);
                    if (b)
                    {
                        if (t != null)
                            tiles.Add(t);
                    }
                    else
                        errors++;
                    if (tiles.Count >= bulkmax)
                    {
                        errors -= ElevationData.Database.ElevationTileBulkInsert(tiles, StartOptions.Model);
                        tiles.Clear();
                    }
                    if (this.CancellationPending)
                        break;
                    // sleep or not whether database is complete
                    if ((ElevationData.Database.GetDBStatus(StartOptions.Model) & DATABASESTATUS.COMPLETE) > 0)
                        Thread.Sleep(Properties.Settings.Default.Database_BackgroundUpdate_ThreadWait);
                    else Thread.Sleep(10);
                }
                // update rest of tiles
                if (tiles.Count > 0)
                    errors -= ElevationData.Database.ElevationTileBulkInsert(tiles, StartOptions.Model);
                // cleanup all *.loc files
                foreach (string f in Directory.EnumerateFiles(ElevationData.Database.DefaultDatabaseDirectory(StartOptions.Model), "*.loc"))
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch (Exception ex)
                    {
                        this.ReportProgress(-1, ex.ToString() + ": " + f);
                        errors++;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, ex.ToString());
                // report at least one error
                if (errors == 0)
                    errors = 1;
                return -errors;
            }
            return -errors;
        }

        private bool IsDatabaseComplete()
        {
            // checks if elevation database is complete
            try
            {
                // get all locs needed for covered area
                // try to read cached catalogue first
                ElevationCatalogue cat = ElevationData.Database.ElevationCatalogueCreateCheckBoundsAndLastModified(null, StartOptions.Model, StartOptions.MinLat, StartOptions.MinLon, StartOptions.MaxLat, StartOptions.MaxLon);
                // process tiles
                foreach (KeyValuePair<string, DateTime> availabletile in cat.Files)
                {
                    // check if tile is in database and return false if tile is not found
                    if (!ElevationData.Database.ElevationTileExists(availabletile.Key.Substring(0, 6).ToUpper(), StartOptions.Model))
                        return false;
                    if (this.CancellationPending)
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, ex.ToString());
                return false;
            }
            return true;
        }

        private DateTime GetDatabaseTimeStamp()
        {
            string filename = ElevationData.Database.GetDBLocation(StartOptions.Model);
            DateTime dt = File.GetLastWriteTimeUtc(filename).ToUniversalTime();
            // convert to YYYY:MM:DD hh:mm:ss only as this is stored in settings
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Utc);
            return dt;
        }

        private DATABASESTATUS GetDatabaseStatus()
        {
            return ElevationData.Database.GetDBStatus(StartOptions.Model);
        }

        private DateTime GetUpdateTimeStamp()
        {
            string filename = Path.Combine(ElevationData.Database.DefaultDatabaseDirectory(StartOptions.Model), ElevationData.Database.JSONFile(StartOptions.Model)); 
            DateTime dt = File.GetLastWriteTimeUtc(filename).ToUniversalTime();
            // convert to YYYY:MM:DD hh:mm:ss only as this is stored in settings
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Utc);
            return dt;
        }

        private DateTime GetSavedTimeStamp()
        {
            DateTime dt = DateTime.MinValue;
            if ((StartOptions.Model == ELEVATIONMODEL.GLOBE) && (Properties.Settings.Default.Elevation_GLOBE_TimeStamp != null))
                dt = Properties.Settings.Default.Elevation_GLOBE_TimeStamp;
            else if ((StartOptions.Model == ELEVATIONMODEL.SRTM3) && (Properties.Settings.Default.Elevation_SRTM3_TimeStamp != null))
                dt = Properties.Settings.Default.Elevation_SRTM3_TimeStamp;
            else if ((StartOptions.Model == ELEVATIONMODEL.SRTM1) && (Properties.Settings.Default.Elevation_SRTM1_TimeStamp != null))
                dt = Properties.Settings.Default.Elevation_SRTM1_TimeStamp;
            else if ((StartOptions.Model == ELEVATIONMODEL.ASTER3) && (Properties.Settings.Default.Elevation_ASTER3_TimeStamp != null))
                dt = Properties.Settings.Default.Elevation_ASTER3_TimeStamp;
            else if ((StartOptions.Model == ELEVATIONMODEL.ASTER1) && (Properties.Settings.Default.Elevation_ASTER1_TimeStamp != null))
                dt = Properties.Settings.Default.Elevation_ASTER1_TimeStamp;
            // change kind to UTC as it is not specified in settings
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            return dt;
        }

        private DATABASESTATUS GetSavedDatabaseStatus()
        {
            if (StartOptions.Model == ELEVATIONMODEL.GLOBE)
                return Properties.Settings.Default.Elevation_GLOBE_Status;
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM3)
                return Properties.Settings.Default.Elevation_SRTM3_Status;
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM1)
                return Properties.Settings.Default.Elevation_SRTM1_Status;
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER3)
                return Properties.Settings.Default.Elevation_ASTER3_Status;
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER1)
                return Properties.Settings.Default.Elevation_ASTER1_Status;
            return DATABASESTATUS.UNDEFINED;
        }

        private DateTime GetSavedUpdateTimeStamp()
        {
            DateTime dt = DateTime.MinValue;
            if (StartOptions.Model == ELEVATIONMODEL.GLOBE)
                dt = Properties.Settings.Default.Elevation_GLOBE_Update_TimeStamp;
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM3)
                dt = Properties.Settings.Default.Elevation_SRTM3_Update_TimeStamp;
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM1)
                dt = Properties.Settings.Default.Elevation_SRTM1_Update_TimeStamp;
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER3)
                dt = Properties.Settings.Default.Elevation_ASTER3_Update_TimeStamp;
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER1)
                dt = Properties.Settings.Default.Elevation_ASTER1_Update_TimeStamp;
            // change kind to UTC as it is not specified in settings
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            return dt;
        }

        private void SaveDatabaseTimeStamp()
        {
            if (StartOptions.Model == ELEVATIONMODEL.GLOBE)
                Properties.Settings.Default.Elevation_GLOBE_TimeStamp = GetDatabaseTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM3)
                Properties.Settings.Default.Elevation_SRTM3_TimeStamp = GetDatabaseTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM1)
                Properties.Settings.Default.Elevation_SRTM1_TimeStamp = GetDatabaseTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER3)
                Properties.Settings.Default.Elevation_ASTER3_TimeStamp = GetDatabaseTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER1)
                Properties.Settings.Default.Elevation_ASTER1_TimeStamp = GetDatabaseTimeStamp();
        }

        private void SaveDatabaseStatus()
        {
            if (StartOptions.Model == ELEVATIONMODEL.GLOBE)
                Properties.Settings.Default.Elevation_GLOBE_Status = GetDatabaseStatus();
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM3)
                Properties.Settings.Default.Elevation_SRTM3_Status = GetDatabaseStatus();
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM1)
                Properties.Settings.Default.Elevation_SRTM1_Status = GetDatabaseStatus();
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER3)
                Properties.Settings.Default.Elevation_ASTER3_Status = GetDatabaseStatus();
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER1)
                Properties.Settings.Default.Elevation_ASTER1_Status = GetDatabaseStatus();
        }

        private void SaveUpdateTimeStamp()
        {
            if (StartOptions.Model == ELEVATIONMODEL.GLOBE)
                Properties.Settings.Default.Elevation_GLOBE_Update_TimeStamp = GetUpdateTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM3)
                Properties.Settings.Default.Elevation_SRTM3_Update_TimeStamp = GetUpdateTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.SRTM1)
                Properties.Settings.Default.Elevation_SRTM1_Update_TimeStamp = GetUpdateTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER3)
                Properties.Settings.Default.Elevation_ASTER3_Update_TimeStamp = GetUpdateTimeStamp();
            else if (StartOptions.Model == ELEVATIONMODEL.ASTER1)
                Properties.Settings.Default.Elevation_ASTER1_Update_TimeStamp = GetUpdateTimeStamp();
        }

        private void SaveBounds()
        {
            Properties.Settings.Default.MaxLat = StartOptions.MaxLat;
            Properties.Settings.Default.MinLat = StartOptions.MinLat;
            Properties.Settings.Default.MaxLon = StartOptions.MaxLon;
            Properties.Settings.Default.MinLon = StartOptions.MinLon;
        }

        private bool HasDatabaseChanged()
        {
            try
            {
                DateTime dt1 = GetSavedTimeStamp();
                DateTime dt2 = GetDatabaseTimeStamp();
                return  dt1 != dt2;
            }
            catch
            {
                // do nothing
            }
            return true;
        }

        private bool HasUpdateChanged()
        {
            try
            {
                DateTime dt1 = GetSavedUpdateTimeStamp();
                DateTime dt2 = GetUpdateTimeStamp();
                return dt1 != dt2;
            }
            catch
            {
                // do nothing
            }
            return true;
        }

        private bool HaveBoundsChanged()
        {
            if (StartOptions.MaxLat != Properties.Settings.Default.MaxLat)
                return true;
            if (StartOptions.MinLat != Properties.Settings.Default.MinLat)
                return true;
            if (StartOptions.MaxLon != Properties.Settings.Default.MaxLon)
                return true;
            if (StartOptions.MinLon != Properties.Settings.Default.MinLon)
                return true;
            return false;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            // get all parameters
            StartOptions = (ElevationDatabaseUpdaterStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + " started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = StartOptions.Name + "_" + this.GetType().Name;
            // get update interval
            int interval = (int)Properties.Settings.Default.Datatbase_BackgroundUpdate_Period * 60;
            do
            {
                try
                {
                    int errors = 0;
                    // check if any kind of update is enabled
                    if ((StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE) || (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY))
                    { 
                        // reset status
                        ElevationData.Database.SetDBStatus(StartOptions.Model, DATABASESTATUS.UNDEFINED);
                        this.ReportProgress(1, ElevationData.Database.GetDBStatus(StartOptions.Model));
                        // clear temporary files
                        try
                        {
                            SupportFunctions.DeleteFilesFromDirectory(TmpDirectory, new string[] { "*.tmp", "*.PendingOverwrite" });
                            SupportFunctions.DeleteFilesFromDirectory(ElevationData.Database.DefaultDatabaseDirectory(StartOptions.Model), new string[] { "*.tmp", "*.PendingOverwrite" });
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress(-1, ex.ToString());
                        }
                        Stopwatch st = new Stopwatch();
                        st.Start();
                        // check last change of database file and no errors
                        if (HasDatabaseChanged() || HasUpdateChanged() || HaveBoundsChanged() || ((GetSavedDatabaseStatus() &  DATABASESTATUS.ERROR) > 0))
                        {
                            // database and/or update has changed --> full check necessary
                            // check if database is complete
                            this.ReportProgress(0, "Checking " + StartOptions.Name + "...");
                            bool b = IsDatabaseComplete();
                            st.Stop();
                            if (b)
                            {
                                // set status
                                ElevationData.Database.SetDBStatusBit(StartOptions.Model, DATABASESTATUS.COMPLETE);
                                // set bounds
                                ElevationData.Database.MinLat = StartOptions.MinLat;
                                ElevationData.Database.MinLon = StartOptions.MinLon;
                                ElevationData.Database.MaxLat = StartOptions.MaxLat;
                                ElevationData.Database.MaxLon = StartOptions.MaxLon;

                                this.ReportProgress(0, StartOptions.Name + " complete: " + st.ElapsedMilliseconds / 1000 + "sec.");
                            }
                            else
                            {
                                ElevationData.Database.ResetDBStatusBit(StartOptions.Model, DATABASESTATUS.COMPLETE);
                            }
                            this.ReportProgress(1, ElevationData.Database.GetDBStatus(StartOptions.Model));
                            if (this.CancellationPending)
                                break;
                            st.Restart();
                            // update elevation database if necessary
                            this.ReportProgress(0, "Updating " + StartOptions.Name + "...");
                            // reset database status
                            ElevationData.Database.ResetDBStatusBit(StartOptions.Model, DATABASESTATUS.UPTODATE);
                            ElevationData.Database.ResetDBStatusBit(StartOptions.Model, DATABASESTATUS.ERROR);
                            ElevationData.Database.SetDBStatusBit(StartOptions.Model, DATABASESTATUS.UPDATING);
                            this.ReportProgress(1, ElevationData.Database.GetDBStatus(StartOptions.Model));
                            // start update
                            int er = -UpdateDatabase();
                            // set database status
                            ElevationData.Database.ResetDBStatusBit(StartOptions.Model, DATABASESTATUS.UPDATING);
                            if (er > 0)
                            {
                                ElevationData.Database.SetDBStatusBit(StartOptions.Model, DATABASESTATUS.ERROR);
                            }
                            else
                            {
                                ElevationData.Database.ResetDBStatusBit(StartOptions.Model, DATABASESTATUS.ERROR);
                                ElevationData.Database.SetDBStatusBit(StartOptions.Model, DATABASESTATUS.UPTODATE);
                            }
                            this.ReportProgress(1, ElevationData.Database.GetDBStatus(StartOptions.Model));
                            st.Stop();
                            if (this.CancellationPending)
                                break;

                            // display status
                            if (errors == 0)
                            {
                                this.ReportProgress(0, StartOptions.Name + " update completed: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                            }
                            else
                            {
                                this.ReportProgress(0, StartOptions.Name + " update completed with errors[" + errors.ToString() + "]: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                            }
                            // store database timestamp & status in settings
                            SaveDatabaseTimeStamp();
                            SaveDatabaseStatus();
                            SaveUpdateTimeStamp();
                            SaveBounds();
                            // sleep once to get all messages to main thread
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            // database and update not changed -> nothing to do
                            // restore database status
                            ElevationData.Database.SetDBStatus(StartOptions.Model, GetSavedDatabaseStatus());
                            this.ReportProgress(1, ElevationData.Database.GetDBStatus(StartOptions.Model));
                            this.ReportProgress(0, StartOptions.Name + " update completed: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                        }

                        // sleep when running periodically
                        if (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY)
                        {
                            int i = 0;
                            while (!this.CancellationPending && (i < interval))
                            {
                                Thread.Sleep(1000);
                                i++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ReportProgress(-1, ex.ToString());
                }
                if (this.CancellationPending)
                    break;
                // sleep when running periodically
                if (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY)
                {
                    int i = 0;
                    while (!this.CancellationPending && (i < interval))
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                }

            }
            while (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
            if (this.CancellationPending)
                this.ReportProgress(0, StartOptions.Name + " cancelled.");
            else
                this.ReportProgress(0, StartOptions.Name + " finished.");
        }
    }

    #endregion
}
