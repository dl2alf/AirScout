
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

namespace AirScout.AircraftPositions
{

    #region AircraftPositionDatabaseMaintainer


    public class AircraftPositionDatabaseMaintainerStartOptions
    {
        public string Name;
        public double Database_MaxSize;
        public long Database_MaxCount;
        public int Database_MaxDaysLifetime;
    }


    // Background worker for aircraft position database maintainance
    [DefaultPropertyAttribute("Name")]
    public class AircraftPositionDatabaseMaintainer : BackgroundWorker
    {

        AircraftPositionDatabaseMaintainerStartOptions StartOptions;
        double DBLastSize = 0;

        public AircraftPositionDatabaseMaintainer() : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            StartOptions = (AircraftPositionDatabaseMaintainerStartOptions)e.Argument;
            this.ReportProgress(0, StartOptions.Name + "started.");
            // name the thread for debugging
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = nameof(AircraftPositionDatabaseMaintainer);
            // get maintenance interval
            int interval = (int)Properties.Settings.Default.Database_BackgroundMaintenance_Period * 60;
            do
            {                        
                try
                {
                    this.ReportProgress(0, "Maintaining database...");
                    try
                    {
                        // check database entry count and remove oldest entries
                        long count = AircraftPositionData.Database.AircraftPositionCount();
                        if (this.CancellationPending)
                            break;
                        // check against limit
                        if (count > StartOptions.Database_MaxCount)
                        {
                            // remove oldest entries from database
                            this.ReportProgress(0, "Exceeding database entries count limit, removing entries...");
                            AircraftPositionData.Database.AircraftPositionBulkDeleteFirst(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ReportProgress(-1, ex.ToString());
                    }
                    try
                    {
                        // check and reduce database size
                        // tricky, because deleting entries not necessarily reduces database size
                        // neeeds pragma auto_vacuum to be set to full
                        // delete an amount of entries from AircraftPositions table and let auto_vacuum do the rest
                        // keep last reported size in memory trigger deletion only if size is increasing again
                        double size = AircraftPositionData.Database.GetDBSize();
                        // check against last reported size
                        if (size > DBLastSize)
                        {
                            // keep last reported size
                            DBLastSize = size;
                            if (DBLastSize > StartOptions.Database_MaxSize)
                            {
                                // remove first 1000 entries from database
                                this.ReportProgress(0, "Exceeding database size limit, removing entries...");
                                AircraftPositionData.Database.AircraftPositionBulkDeleteFirst(1000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ReportProgress(-1, ex.ToString());
                    }
                    try
                    {
                        // check and remove oldest entries
                        DateTime olderthan = DateTime.UtcNow - new TimeSpan(StartOptions.Database_MaxDaysLifetime, 0, 0, 0);
                        // remove oldest entries from database
                        this.ReportProgress(0, "Cleaning up aircraft positions...");
                        AircraftPositionData.Database.AircraftPositionBulkDeleteOlderThan(olderthan);
                    }
                    catch (Exception ex)
                    {
                        this.ReportProgress(-1, ex.ToString());
                    }
                    this.ReportProgress(0, "");
                    // sleep 
                    int i = 0;
                    while (!this.CancellationPending && (i < interval))
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    this.ReportProgress(-1, ex.ToString());
                }
            }
            while (!this.CancellationPending);
            if (this.CancellationPending)
                this.ReportProgress(0, StartOptions.Name + "cancelled.");
            else
                this.ReportProgress(0, StartOptions.Name + " finished.");
        }

        #endregion

    }
}
