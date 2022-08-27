
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoutBase;
using ScoutBase.Core;
using System.Data.SQLite;
using System.Text;

namespace AirScout.Aircrafts
{

    #region AircraftDatabaseUpdater


    public class AircraftDatabaseUpdaterStartOptions
    {
        public string Name;
        public string InstanceID;
        public string SessionKey;
        public string GetKeyURL;
        public BACKGROUNDUPDATERSTARTOPTIONS Options;
    }


    // Background worker for aircraft database update
    [DefaultPropertyAttribute("Name")]
    public class AircraftDatabaseUpdater : BackgroundWorker
    {

        AircraftDatabaseUpdaterStartOptions StartOptions;
        string Password;

        public AircraftDatabaseUpdater() : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }

        private bool ReadAircraftsFromURL(string url, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true, Password);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.ERROR) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename.Replace(".zip", ".json")))
                        json = sr.ReadToEnd();
                    List<AirScout.Aircrafts.AircraftDesignator> ads = AircraftData.Database.AircraftFromJSON(json);
                    // check for invalid entries
                    foreach (AircraftDesignator ad in ads)
                    {
                        if (String.IsNullOrEmpty(ad.Call))
                            ad.Call = "[unknown]";
                        if (String.IsNullOrEmpty(ad.Reg))
                            ad.Reg = "[unknown]";
                        if (String.IsNullOrEmpty(ad.TypeCode))
                            ad.TypeCode = "[unknown]";
                    }
                    // check for empty database
                    if (AircraftData.Database.AircraftCount() == 0)
                    {
                        // do bulk insert
                        AircraftData.Database.AircraftBulkInsert(ads);
                    }
                    else
                    {
                        // do bulk update
                        AircraftData.Database.AircraftBulkInsertOrUpdateIfNewer(ads);
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

        private bool ReadAircraftTypesFromURL(string url, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true, Password);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.ERROR) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename.Replace(".zip", ".json")))
                        json = sr.ReadToEnd();
                    List<AirScout.Aircrafts.AircraftTypeDesignator> tds = AircraftData.Database.AircraftTypeFromJSON(json);
                    // check for empty database
                    if (AircraftData.Database.AircraftTypeCount() == 0)
                    {
                        // do bulk insert
                        AircraftData.Database.AircraftTypeBulkInsert(tds);
                    }
                    else
                    {
                        // do bulk update
                        AircraftData.Database.AircraftTypeBulkInsertOrUpdateIfNewer(tds);
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

        private bool ReadAircraftRegistrationsFromURL(string url, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true, Password);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.ERROR) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename.Replace(".zip", ".json")))
                        json = sr.ReadToEnd();
                    List<AirScout.Aircrafts.AircraftRegistrationDesignator> rds = AircraftData.Database.AircraftRegistrationFromJSON(json);
                    // check for empty database
                    if (AircraftData.Database.AircraftRegistrationCount() == 0)
                    {
                        // do bulk insert
                        AircraftData.Database.AircraftRegistrationBulkInsert(rds);
                    }
                    else
                    {
                        // do bulk update
                        AircraftData.Database.AircraftRegistrationBulkInsertOrUpdateIfNewer(rds);
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

        private bool ReadAirportsFromURL(string url, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true, Password);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.ERROR) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename.Replace(".zip", ".json")))
                        json = sr.ReadToEnd();
                    List<AirScout.Aircrafts.AirportDesignator> pds = AircraftData.Database.AirportFromJSON(json);
                    // check for empty database
                    if (AircraftData.Database.AirportCount() == 0)
                    {
                        // do bulk insert
                        AircraftData.Database.AirportBulkInsert(pds);
                    }
                    else
                    {
                        // do bulk update
                        AircraftData.Database.AirportBulkInsertOrUpdateIfNewer(pds);
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

        private bool ReadAirlinesFromURL(string url, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true, Password);
                if (((status & DOWNLOADFILESTATUS.ERROR) > 0) && ((status & DOWNLOADFILESTATUS.ERROR) > 0))
                {
                    this.ReportProgress(-1, "Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename.Replace(".zip", ".json")))
                        json = sr.ReadToEnd();
                    List<AirScout.Aircrafts.AirlineDesignator> lds = AircraftData.Database.AirlineFromJSON(json);
                    // check for empty database
                    if (AircraftData.Database.AirlineCount() == 0)
                    {
                        // do bulk insert
                        AircraftData.Database.AirlineBulkInsert(lds);
                    }
                    else
                    {
                        // do bulk update
                        AircraftData.Database.AirlineBulkInsertOrUpdateIfNewer(lds);
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

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            StartOptions = (AircraftDatabaseUpdaterStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + "started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = nameof(AircraftDatabaseUpdater);
            // get update interval
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
            int interval = (int)Properties.Settings.Default.Database_BackgroundUpdate_Period * 60;
            do
            {                        
                try
                {
                    int errors = 0;
                    // check if any kind of update is enabled
                    if ((StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNONCE) || (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY))
                    {
                        this.ReportProgress(0, "Updating database...");
                        // get temp directory
                        string TmpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName, "Tmp").TrimEnd(Path.DirectorySeparatorChar);
                        if (!Directory.Exists(TmpDirectory))
                            Directory.CreateDirectory(TmpDirectory);
                        // reset database status
                        AircraftData.Database.SetDBStatus(DATABASESTATUS.UNDEFINED);
                        this.ReportProgress(1, AircraftData.Database.GetDBStatus());
                        Stopwatch st = new Stopwatch();
                        st.Start();
                        AircraftData.Database.SetDBStatus(DATABASESTATUS.UPDATING);
                        this.ReportProgress(1, AircraftData.Database.GetDBStatus());
                        // update aircraft database
                        this.ReportProgress(0, "Updating aircraft types from web database...");
                        if (!ReadAircraftTypesFromURL(Properties.Settings.Default.Aircrafts_UpdateURL + "AircraftTypes.zip", Path.Combine(TmpDirectory, "AircraftTypes.zip")))
                            errors++;
                        if (this.CancellationPending)
                            break;
                        this.ReportProgress(0, "Updating airports from web database...");
                        if (!ReadAirportsFromURL(Properties.Settings.Default.Aircrafts_UpdateURL + "Airports.zip", Path.Combine(TmpDirectory, "Airports.zip")))
                            errors++;
                        if (this.CancellationPending)
                            break;
                        this.ReportProgress(0, "Updating aircrafts from web database...");
                        if (!ReadAircraftsFromURL(Properties.Settings.Default.Aircrafts_UpdateURL + "Aircrafts.zip", Path.Combine(TmpDirectory, "Aircrafts.zip")))
                            errors++;
                        if (this.CancellationPending)
                            break;
                        this.ReportProgress(0, "Updating aircraft registrations from web database...");
                        if (!ReadAircraftRegistrationsFromURL(Properties.Settings.Default.Aircrafts_UpdateURL + "AircraftRegistrations.zip", Path.Combine(TmpDirectory, "AircraftRegistrations.zip")))
                            errors++;
                        if (this.CancellationPending)
                            break;
                        this.ReportProgress(0, "Updating airlines from web database...");
                        if (!ReadAirlinesFromURL(Properties.Settings.Default.Aircrafts_UpdateURL + "Airlines.zip", Path.Combine(TmpDirectory, "Airlines.zip")))
                            errors++;

                        st.Stop();
                        // display status
                        if (errors == 0)
                        {
                            AircraftData.Database.SetDBStatus(DATABASESTATUS.UPTODATE);
                            this.ReportProgress(1, AircraftData.Database.GetDBStatus());
                            this.ReportProgress(0, " Aircraft database update completed: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
                        }
                        else
                        {
                            AircraftData.Database.SetDBStatus(DATABASESTATUS.ERROR);
                            this.ReportProgress(1, AircraftData.Database.GetDBStatus());
                            this.ReportProgress(0, " Aircraft database update completed with errors[" + errors.ToString() + "]: " + st.Elapsed.ToString(@"hh\:mm\:ss"));
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
            }
            while (!this.CancellationPending && (StartOptions.Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY));
            if (this.CancellationPending)
                this.ReportProgress(0, StartOptions.Name + " cancelled.");
            else
                this.ReportProgress(0, StartOptions.Name + " finished.");
        }

        #endregion

    }
}
