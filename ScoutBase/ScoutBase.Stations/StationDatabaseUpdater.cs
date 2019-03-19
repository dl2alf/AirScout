
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
        public BACKGROUNDUPDATERSTARTOPTIONS Options;
    }


    // Background worker for station database update
    [DefaultPropertyAttribute("Name")]
    public class StationDatabaseUpdater : BackgroundWorker
    {

        StationDatabaseUpdaterStartOptions StartOptions;
        string Password = "";

        public StationDatabaseUpdater() : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }

        private bool ReadLocationsFromURL(string url, string zipfilename, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, zipfilename, true, true, Password);
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
                    List<LocationDesignator> lds = StationData.Database.LocationFromJSON(json);
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
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, zipfilename, true, true, Password);
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
                    List<QRVDesignator> qrvs = StationData.Database.QRVFromJSON(json);
                    // chek for empty database
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
            List<LocationDesignator> lds = StationData.Database.LocationFromV1_2(filename);
            try
            {
                foreach (LocationDesignator ld in lds)
                {
                    if (StationData.Database.LocationInsertOrUpdateIfNewer(ld) > 0)
                        this.ReportProgress(0, "Importing locations from V1.2 database: " + ld.Call + ", " + ld.Loc + ", " + ld.Source.ToString());
                    if (this.CancellationPending)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ReportProgress(-1, ex.ToString());
            }
            return false;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            StartOptions = (StationDatabaseUpdaterStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + " started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = nameof(StationDatabaseUpdater);
            this.ReportProgress(0, "Updating station database...");
            // get current AirScout password phrase from website and store it in settings
            try
            {
                // get upload info
                WebRequest myWebRequest = WebRequest.Create(Properties.Settings.Default.Stations_PasswordURL);
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                Password = readStream.ReadToEnd();
                Password = ScoutBase.Core.Password.GetSFTPPassword(Password);
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
                        // get temp directory
                        string TmpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName, "Tmp").TrimEnd(Path.DirectorySeparatorChar);
                        if (!Directory.Exists(TmpDirectory))
                            Directory.CreateDirectory(TmpDirectory);
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
                        // update callsign database
                        this.ReportProgress(0, "Updating callsigns from web database...");
                        if (!ReadLocationsFromURL(Properties.Settings.Default.Stations_UpdateURL + "locations.zip", Path.Combine(TmpDirectory, "locations.zip"), Path.Combine(TmpDirectory, "locations.json")))
                            errors++;
                        if (this.CancellationPending)
                            break;
                        if (!ReadQRVFromURL(Properties.Settings.Default.Stations_UpdateURL + "qrv.zip", Path.Combine(TmpDirectory, "qrv.zip"), Path.Combine(TmpDirectory, "qrv.json")))
                            errors++;
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
