
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
using ScoutBase.Elevation;
using ScoutBase.Stations;

namespace AirScoutDatabaseManager
{
    public partial class MainDlg : Form
    {

        #region StationDatabaseUpdater

        private bool ReadLocationsFromURL(string url, string filename)
        {
            try
            {
                AutoDecompressionWebClient cl = new AutoDecompressionWebClient();
                DOWNLOADFILESTATUS status = cl.DownloadFileIfNewer(url, filename, true, true);
                if ((status & DOWNLOADFILESTATUS.ERROR) > 0)
                {
                    Log.WriteMessage("Error while downloading and extracting " + filename);
                    return false;
                }
                else if (((status & DOWNLOADFILESTATUS.NEWER) > 0) || ((status & DOWNLOADFILESTATUS.NOTNEWER) > 0))
                {
                    string json = "";
                    using (StreamReader sr = new StreamReader(filename))
                        json = sr.ReadToEnd();
                    List<LocationDesignator> lds = StationData.Database.LocationFromJSON(json);
                    // chek for empty database
                    if (StationData.Database.LocationCount() == 0)
                    {
                        // do bulk insert
                        StationData.Database.LocationBulkInsert(lds);
                    }
                    else
                    {
                        // do update on single elements
                        foreach(LocationDesignator ld in lds)
                        {
                            StationData.Database.LocationInsertOrUpdateIfNewer(ld);
                            // return if cancellation is pending
                            if (bw_DatabaseUpdater.CancellationPending)
                                return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Error loading database
                Log.WriteMessage("[" + url + "]: " + ex.ToString());
            }
            return false;
        }

        private void bw_DatabaseUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = nameof(bw_DatabaseUpdater);
            bw_DatabaseUpdater.ReportProgress(0, "Updating database...");
            // get temp directory
            string TmpDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName, "Tmp").TrimEnd(Path.DirectorySeparatorChar);
            if (!Directory.Exists(TmpDirectory))
                Directory.CreateDirectory(TmpDirectory);
            int errors = 0;
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                // update callsign database
                bw_DatabaseUpdater.ReportProgress(0, "Updating callsigns from web database...");
                if (!ReadLocationsFromURL(Properties.Settings.Default.Station_URL + "locations.json", Path.Combine(TmpDirectory, "locations.json")))
                    errors++;
                st.Stop();
                Log.WriteMessage("Database update completed in " + st.Elapsed.ToString(@"hh\:mm\:ss") + ", errors: " + errors.ToString());

                // sleep once to get all messages to main thread
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
            bw_DatabaseUpdater.ReportProgress(0, "Updating callsigns finished.");
            Log.WriteMessage("Finished.");
        }

        private void bw_DatabaseUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage == 0)
                {
                    // status message received
                    string msg = (string)e.UserState;
                    Say(msg);
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
        }

        private void bw_DatabaseUpdater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tc_Main.Enabled = true;
        }

        #endregion

    }
}
