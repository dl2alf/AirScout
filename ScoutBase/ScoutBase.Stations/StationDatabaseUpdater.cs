
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
using ScoutBase;
using ScoutBase.Core;
using ScoutBase.Stations;
using System.Data.SQLite;
using System.Text;

namespace ScoutBase.Stations
{

    #region StationDatabaseUpdater

    public class StationDatabaseUpdaterStartOptions
    {
        public string Name;
        public bool RestrictToAreaOfInterest;
        public double MinLat;
        public double MinLon;
        public double MaxLat;
        public double MaxLon;
        public string InstanceID;
        public string SessionKey;
        public string GetKeyURL;
        public BACKGROUNDUPDATERSTARTOPTIONS Options;
    }


    // Background worker for station database update
    [DefaultPropertyAttribute("Name")]
    public class StationDatabaseUpdater : BackgroundWorker
    {
        string Password = "";
        StationDatabaseUpdaterStartOptions StartOptions;

        // Temp directory to save downloaded files
        string TmpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName, "Tmp", "StationData").TrimEnd(Path.DirectorySeparatorChar);

        public StationDatabaseUpdater() : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            // create temp directory if not exists
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
        }

        private DOWNLOADFILESTATUS GetUpdateFromURL(string url, string zipfilename, string filename)
        {
            AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
            DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, zipfilename, true, true, Password);
            return status;
        }

        private bool ReadLocationsFromURL(string url, string zipfilename, string filename)
        {
            try
            {
                DOWNLOADFILESTATUS status = GetUpdateFromURL(url, zipfilename, filename);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.NOTFOUND) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename))
                        json = sr.ReadToEnd();
                    List<LocationDesignator> tmp = StationData.Database.LocationFromJSON(json);
                    List<LocationDesignator> lds = new List<LocationDesignator>();
                    int rate_limit_cnt = 0;
                    foreach (LocationDesignator ld in tmp)
                    {
                        // skip locations outsid area of interest if option set
                        if (StartOptions.RestrictToAreaOfInterest && ((ld.Lat < StartOptions.MinLat) || (ld.Lat > StartOptions.MaxLat) || (ld.Lon < StartOptions.MinLon) || (ld.Lon > StartOptions.MaxLon)))
                            continue;
                        lds.Add(ld);
                        if (this.CancellationPending)
                            return false;
                        // reduce CPU load
                        if (++rate_limit_cnt > 100)
                        {
                            Thread.Sleep(1);
                            rate_limit_cnt = 0;
                        }
                    }
                    // check for empty database
                    if (StationData.Database.LocationCount() == 0)
                    {
                        // do bulk insert
                        StationData.Database.LocationBulkInsert(lds);
                    }
                    else
                    {
                        // do bulk update
                        StationData.Database.LocationBulkInsertOrUpdateIfNewer(lds);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Error loading database
                this.ReportProgress(-1, "[" + url + "]: " + ex.ToString());
            }
            return false;
        }

        private bool ReadQRVFromURL(string url, string zipfilename, string filename)
        {
            try
            {
                DOWNLOADFILESTATUS status = GetUpdateFromURL(url, zipfilename, filename);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.NOTFOUND) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename))
                        json = sr.ReadToEnd();
                    List<QRVDesignator> tmp = StationData.Database.QRVFromJSON(json);
                    List<QRVDesignator> qrvs = new List<QRVDesignator>();
                    int rate_limit_cnt = 0;
                    foreach (QRVDesignator qrv in tmp)
                    {
                        // skip locations outsid area of interest if option set
                        if (StartOptions.RestrictToAreaOfInterest)
                        {
                            LocationDesignator ld = StationData.Database.LocationFind(qrv.Call, qrv.Loc);
                            if ((ld == null) || ((ld.Lat < StartOptions.MinLat) || (ld.Lat > StartOptions.MaxLat) || (ld.Lon < StartOptions.MinLon) || (ld.Lon > StartOptions.MaxLon)))
                                continue;
                            qrvs.Add(qrv);
                        }
                        else
                            qrvs.Add(qrv);
                        if (this.CancellationPending)
                            return false;
                        // reduce CPU load
                        if (++rate_limit_cnt > 100)
                        {
                            Thread.Sleep(1);
                            rate_limit_cnt = 0;
                        }
                    }
                    // check for empty database
                    if (StationData.Database.QRVCount() == 0)
                    {
                        // do bulk insert
                        StationData.Database.QRVBulkInsert(qrvs);
                    }
                    else
                    {
                        // do bulk update
                        StationData.Database.QRVBulkInsertOrUpdateIfNewer(qrvs);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Error loading database
                this.ReportProgress(-1, "[" + url + "]: " + ex.ToString());
            }
            return false;
        }

        private bool ReadLocationsFromV1_2 (string filename)
        {
            List<LocationDesignator> tmp = StationData.Database.LocationFromV1_2(filename);
            List<LocationDesignator> lds = new List<LocationDesignator>();
            foreach (LocationDesignator ld in tmp)
            {
                // skip locations outsid area of interest if option set
                if (StartOptions.RestrictToAreaOfInterest && ((ld.Lat < StartOptions.MinLat) || (ld.Lat > StartOptions.MaxLat) || (ld.Lon < StartOptions.MinLon) || (ld.Lon > StartOptions.MaxLon)))
                    continue;
                lds.Add(ld);
            }
            try
            {
                foreach (LocationDesignator ld in lds)
                {
                    if (StationData.Database.LocationInsertOrUpdateIfNewer(ld) > 0)
                        this.ReportProgress(0, "Importing locations from V1.2 database: " + ld.Call + ", " + ld.Loc + ", " + ld.Source.ToString());
                    if (this.CancellationPending)
                        return false;
                    // reduce CPU load
                    Thread.Sleep(1);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, ex.ToString());
            }
            return false;
        }

        private DateTime GetDatabaseTimeStamp()
        {
            string filename = StationData.Database.GetDBLocation();
            DateTime dt = File.GetLastWriteTimeUtc(filename).ToUniversalTime();
            // convert to YYYY:MM:DD hh:mm:ss only as this is stored in settings
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Utc);
            return dt;
        }

        private DATABASESTATUS GetDatabaseStatus()
        {
            return StationData.Database.GetDBStatus();
        }

        private DateTime GetLocationtUpdateTimeStamp()
        {
            string filename = Path.Combine(TmpDirectory, "locations.json");
            DateTime dt = File.GetLastWriteTimeUtc(filename).ToUniversalTime();
            // convert to YYYY:MM:DD hh:mm:ss only as this is stored in settings
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Utc);
            return dt;
        }

        private DateTime GetQRVUpdateTimeStamp()
        {
            string filename = Path.Combine(TmpDirectory, "qrv.json");
            DateTime dt = File.GetLastWriteTimeUtc(filename).ToUniversalTime();
            // convert to YYYY:MM:DD hh:mm:ss only as this is stored in settings
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Utc);
            return dt;
        }

        private DateTime GetSavedDatabaseTimeStamp()
        {
            return StationData.Database.StationUpdateTimeStamp;
        }

        private DateTime GetSavedLocationUpdateTimeStamp()
        {
            return StationData.Database.LocationUpdateTimeStamp;
        }

        private DateTime GetSavedQRVUpdateTimeStamp()
        {
            return StationData.Database.QRVUpdateTimeStamp;
        }

        private DATABASESTATUS GetSavedDatabaseStatus()
        {
            return Properties.Settings.Default.Stations_Status;
        }

        private void SaveDatabaseTimeStamp()
        {
            // we do it twice to trigger an update to the database file - hack and makes me wonder if this timestamp is needed at all
            StationData.Database.StationUpdateTimeStamp = GetDatabaseTimeStamp();
            StationData.Database.StationUpdateTimeStamp = GetDatabaseTimeStamp();
        }

        private void SaveDatabaseStatus()
        {
            Properties.Settings.Default.Stations_Status = GetDatabaseStatus();
        }

        private void SaveLocationUpdateTimeStamp()
        {
            StationData.Database.LocationUpdateTimeStamp = GetLocationtUpdateTimeStamp();
        }

        private void SaveQRVUpdateTimeStamp()
        {
            StationData.Database.QRVUpdateTimeStamp = GetQRVUpdateTimeStamp();
        }

        private bool HasDatabaseChanged()
        {
            try
            {
                DateTime dt1 = GetSavedDatabaseTimeStamp();
                DateTime dt2 = GetDatabaseTimeStamp();
                return (dt2 - dt1).TotalSeconds > 5*60; // allow for some time difference
            }
            catch
            {
                // do nothing
            }
            return true;
        }

        private bool HasLocationUpdateChanged()
        {
            try
            {
                DateTime dt1 = GetSavedLocationUpdateTimeStamp();
                DateTime dt2 = GetLocationtUpdateTimeStamp();
                return dt1 != dt2;
            }
            catch
            {
                // do nothing
            }
            return true;
        }

        private bool HasQRVUpdateChanged()
        {
            try
            {
                DateTime dt1 = GetSavedQRVUpdateTimeStamp();
                DateTime dt2 = GetQRVUpdateTimeStamp();
                return dt1 != dt2;
            }
            catch
            {
                // do nothing
            }
            return true;
        }


        protected override void OnDoWork(DoWorkEventArgs e)
        {
            StartOptions = (StationDatabaseUpdaterStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + " started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = nameof(StationDatabaseUpdater);
            this.ReportProgress(0, "Updating station database...");
            // get current AirScout password phrase for Unzip from website
            try
            {
                WebClient client = new WebClient();
                string result = client.DownloadString(StartOptions.GetKeyURL +
                            "?id=" + StartOptions.InstanceID +
                            "&key=zip");
                if (!result.StartsWith("Error:"))
                {
                    result = result.Trim('\"');
                    Password = Encryption.OpenSSLDecrypt(result, StartOptions.SessionKey);
                }

            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, ex.ToString());
            }
            // get update interval
            int interval = (int)Properties.Settings.Default.Database_BackgroundUpdate_Period * 60;
            do
            {
                try
                {
                    int errors = 0;
                    // check if any kind of update is enabled
                    if ((StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE) || (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY))
                    {
                        // reset database status
                        StationData.Database.SetDBStatus(DATABASESTATUS.UNDEFINED);
                        this.ReportProgress(1, StationData.Database.GetDBStatus());
                        Stopwatch st = new Stopwatch();
                        st.Start();
                        // clear temporary files
                        try
                        {
                            SupportFunctions.DeleteFilesFromDirectory(TmpDirectory, new string[] { "*.tmp", "*.PendingOverwrite" });
                        }
                        catch (Exception ex)
                        {
                            this.ReportProgress(-1, ex.ToString());
                        }
                        // set status to updating
                        StationData.Database.SetDBStatus(DATABASESTATUS.UPDATING);
                        this.ReportProgress(1, StationData.Database.GetDBStatus());
                        // update location database
                        this.ReportProgress(0, "Updating locations from web database...");
                        // get update from url
                        GetUpdateFromURL(Properties.Settings.Default.Stations_UpdateURL + "locations.zip", Path.Combine(TmpDirectory, "locations.zip"), Path.Combine(TmpDirectory, "locations.json"));
                        if (HasDatabaseChanged() || HasLocationUpdateChanged())
                        {
                            // database and/or update changed --> do full check
                            if (!ReadLocationsFromURL(Properties.Settings.Default.Stations_UpdateURL + "locations.zip", Path.Combine(TmpDirectory, "locations.zip"), Path.Combine(TmpDirectory, "locations.json")))
                            {
                                errors++;
                            }
                            else
                            {
                                // save status & timestamps
                                SaveDatabaseStatus();
                                SaveLocationUpdateTimeStamp();
                                SaveDatabaseTimeStamp();
                            }
                        }
                        else
                        {
                            // dabase and update not changed --> nothing to do
                            // restore database status
                            StationData.Database.SetDBStatus(GetDatabaseStatus());
                        }
                        if (this.CancellationPending)
                            break;
                        // update qrv database
                        this.ReportProgress(0, "Updating qrv info from web database...");
                        // get update from url
                        GetUpdateFromURL(Properties.Settings.Default.Stations_UpdateURL + "qrv.zip", Path.Combine(TmpDirectory, "qrv.zip"), Path.Combine(TmpDirectory, "qrv.json"));
                        if (HasDatabaseChanged() || HasQRVUpdateChanged())
                        {
                            if (!ReadQRVFromURL(Properties.Settings.Default.Stations_UpdateURL + "qrv.zip", Path.Combine(TmpDirectory, "qrv.zip"), Path.Combine(TmpDirectory, "qrv.json")))
                            {
                                errors++;
                            }
                            else
                            {
                                // save status & timestamps
                                SaveDatabaseStatus();
                                SaveQRVUpdateTimeStamp();
                                SaveDatabaseTimeStamp();
                            }
                        }
                        else
                        {
                            // dabase and update not changed --> nothing to do
                            // restore database status
                            StationData.Database.SetDBStatus(GetDatabaseStatus());
                        }
                        // silently update locations from V1.2 database if found
                        string appdatadir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName).TrimEnd(Path.DirectorySeparatorChar);
                        string oldfile = Path.Combine(appdatadir.Replace("AirScout", "ScoutBase"), "Database", "ScoutBase.db3");
                        if (File.Exists(oldfile))
                        {
                            this.ReportProgress(0, "Import locations from V1.2 database...");
                            if (!ReadLocationsFromV1_2(oldfile))
                                errors++;
                        }
                        st.Stop();
                        // display status
                        if (errors == 0)
                        {
                            StationData.Database.SetDBStatus(DATABASESTATUS.UPTODATE);
                            this.ReportProgress(1, StationData.Database.GetDBStatus());
                            this.ReportProgress(0, " Station database update completed: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                        }
                        else
                        {
                            StationData.Database.SetDBStatus(DATABASESTATUS.ERROR);
                            this.ReportProgress(1, StationData.Database.GetDBStatus());
                            this.ReportProgress(0, " Station database update completed with errors[" + errors.ToString() + "]: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                        }
                        // sleep once to get all messages to main thread
                        Thread.Sleep(1000);
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
            }
            while (!this.CancellationPending && (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY));
            if (this.CancellationPending)
                this.ReportProgress(0, "Cancelled.");
            else
                this.ReportProgress(0, "Finished.");
        }

        #endregion

        // legacy!!!
        public class OldLocationDesignator: SQLiteEntry
        {
            public string Call { get; set; }
            public double Lat { get; set; }
            public double Lon { get; set; }
            public GEOSOURCE Source { get; set; }

            public OldLocationDesignator()
            {
                Call = "";
                Lat = 0;
                Lon = 0;
                Source = GEOSOURCE.UNKONWN;
                LastUpdated = DateTime.MinValue;
            }

        }
    }
}
