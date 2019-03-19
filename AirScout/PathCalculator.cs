
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
using ScoutBase.Propagation;
using ScoutBase;
using System.Data.SQLite;

namespace AirScout
{

    public partial class MapDlg : Form
    {

        #region PathCalculator

        // Background worker for path calculation
        [DefaultPropertyAttribute("Name")]
        public class PathCalculator : BackgroundWorker
        {
            ELEVATIONMODEL Model = ELEVATIONMODEL.NONE;

            string Name = "";
            double StepWidth = 1000;

            public PathCalculator(ELEVATIONMODEL model) : base()
            {
                this.WorkerReportsProgress = true;
                this.WorkerSupportsCancellation = true;
                this.Model = model;
            }

            protected override void OnDoWork(DoWorkEventArgs e)
            {
                // get all parameters
                BACKGROUNDUPDATERSTARTOPTIONS Options = (BACKGROUNDUPDATERSTARTOPTIONS)e.Argument;
                // get update interval
                int interval = (int)Properties.Settings.Default.Background_Update_Period * 60;
                do
                {
                    if (Properties.Settings.Default.Background_Calculations_Enable)
                    {
                        // get all parameters
                        // set name and stepwidth according to model
                        switch (Model)
                        {
                            case ELEVATIONMODEL.GLOBE:
                                Name = "GLOBE";
                                StepWidth = ElevationData.Database.GetDefaultStepWidth(ELEVATIONMODEL.GLOBE);
                                break;
                            case ELEVATIONMODEL.SRTM3:
                                Name = "SRTM3";
                                StepWidth = ElevationData.Database.GetDefaultStepWidth(ELEVATIONMODEL.SRTM3);
                                break;
                            case ELEVATIONMODEL.SRTM1:
                                Name = "SRTM1";
                                StepWidth = ElevationData.Database.GetDefaultStepWidth(ELEVATIONMODEL.SRTM1);
                                break;
                        }
                        // name the thread for debugging
                        if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                            Thread.CurrentThread.Name = Name + "_" + this.GetType().Name;
                        try
                        {
                            // iterate through all locations in the database and calculate the propagation path
                            // chek if database is ready first
                            while (ElevationData.Database.GetDBStatusBit(Model, DATABASESTATUS.ERROR) || (!ElevationData.Database.GetDBStatusBit(Model, DATABASESTATUS.COMPLETE) && !ElevationData.Database.GetDBStatusBit(Model, DATABASESTATUS.UPTODATE)))
                            {
                                this.ReportProgress(0, Name + " waiting for database is complete...");
                                // sleep 10 sec
                                int i = 0;
                                while (!this.CancellationPending && (i < 10))
                                {
                                    Thread.Sleep(1000);
                                    i++;
                                }
                                if (this.CancellationPending)
                                    break;
                            }
                            this.ReportProgress(0, Name + " getting locations...");
                            // get all locations in covered area but don't report progress
                            this.WorkerReportsProgress = false;
                            List<LocationDesignator> lds = StationData.Database.LocationGetAll(this, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
                            this.WorkerReportsProgress = true;
                            // start over again with main loog when lds = null for some reason
                            if (lds == null)
                                continue;
                            // iterate through locations
                            QRVDesignator myqrv = null;
                            QRVDesignator dxqrv = null;
                            foreach (LocationDesignator ld in lds)
                            {
                                Stopwatch st = new Stopwatch();
                                st.Start();
                                try
                                {
                                    // leave the iteration when something went wrong --> start new
                                    if (ld == null)
                                        break;
                                    // check lat/lon if valid
                                    if (!GeographicalPoint.Check(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon))
                                        continue;
                                    // check lat/lon if valid
                                    if (!GeographicalPoint.Check(ld.Lat, ld.Lon))
                                        continue;
                                    // chek for path < MaxDistance
                                    double dist = LatLon.Distance(Properties.Settings.Default.MyLat, Properties.Settings.Default.MyLon, ld.Lat, ld.Lon);
                                    if (dist <= Properties.Settings.Default.Path_MaxLength)
                                    {
                                        // start calculation for each band
                                        foreach (BAND band in Bands.GetValuesExceptNoneAndAll())
                                        {
                                            PropagationPathDesignator pp;
                                            // get my lat/lon from settings    
                                            string mycall = Properties.Settings.Default.MyCall;
                                            double mylat = Properties.Settings.Default.MyLat;
                                            double mylon = Properties.Settings.Default.MyLon;
                                            string myloc = MaidenheadLocator.LocFromLatLon(mylat, mylon, false, 3);
                                            double myelv = ElevationData.Database[mylat, mylon, Model];
                                            // modify location in case of best case elevation is selected --> but do not store in database or settings!
                                            if (Properties.Settings.Default.Path_BestCaseElevation)
                                            {
                                                if (!MaidenheadLocator.IsPrecise(mylat, mylon, 3))
                                                {
                                                    ElvMinMaxInfo maxinfo = ElevationData.Database.ElevationTileFindMinMaxInfo(myloc, Model);
                                                    if (maxinfo != null)
                                                    {
                                                        mylat = maxinfo.MaxLat;
                                                        mylon = maxinfo.MaxLon;
                                                        myelv = maxinfo.MaxElv;
                                                    }
                                                }
                                            }
                                            myqrv = StationData.Database.QRVFind(mycall, myloc, band);
                                            double myheight = ((myqrv != null) && (myqrv.AntennaHeight != 0)) ? myqrv.AntennaHeight : StationData.Database.QRVGetDefaultAntennaHeight(band);
                                            string dxcall = ld.Call;
                                            // get my lat/lon from settings    
                                            double dxlat = ld.Lat;
                                            double dxlon = ld.Lon;
                                            string dxloc = MaidenheadLocator.LocFromLatLon(dxlat, dxlon, false, 3);
                                            double dxelv = ElevationData.Database[dxlat, dxlon, Model];
                                            // modify location in case of best case elevation is selected --> but do not store in database or settings!
                                            if (Properties.Settings.Default.Path_BestCaseElevation)
                                            {
                                                if (!MaidenheadLocator.IsPrecise(dxlat, dxlon, 3))
                                                {
                                                    ElvMinMaxInfo maxinfo = ElevationData.Database.ElevationTileFindMinMaxInfo(dxloc, Model);
                                                    if (maxinfo != null)
                                                    {
                                                        dxlat = maxinfo.MaxLat;
                                                        dxlon = maxinfo.MaxLon;
                                                        dxelv = maxinfo.MaxElv;
                                                    }
                                                }
                                            }
                                            dxqrv = StationData.Database.QRVFind(dxcall, dxloc, band);
                                            double dxheight = ((dxqrv != null) && (dxqrv.AntennaHeight != 0)) ? dxqrv.AntennaHeight : StationData.Database.QRVGetDefaultAntennaHeight(band);
                                            LocalObstructionDesignator o = ElevationData.Database.LocalObstructionFind(mylat, mylon, Model);
                                            double myobstr = (o != null) ? o.GetObstruction(myheight, LatLon.Bearing(mylat, mylon, dxlat, dxlon)) : double.MinValue;
                                            pp = PropagationData.Database.PropagationPathFind(
                                                mylat,
                                                mylon,
                                                myelv + myheight,
                                                dxlat,
                                                dxlon,
                                                dxelv + dxheight,
                                                Bands.ToGHz(band),
                                                LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[band].K_Factor,
                                                Properties.Settings.Default.Path_Band_Settings[band].F1_Clearance,
                                                ElevationData.Database.GetDefaultStepWidth(Model),
                                                Model,
                                                myobstr);
                                            // skip if path already in database
                                            if (pp != null)
                                            {
                                                Thread.Sleep(Properties.Settings.Default.Background_Calculations_ThreadWait);
                                                continue;
                                            }
                                            // create new propagation path
                                            pp = PropagationData.Database.PropagationPathCreateFromLatLon(
                                                    this,
                                                    mylat,
                                                    mylon,
                                                    myelv + myheight,
                                                    dxlat,
                                                    dxlon,
                                                    dxelv + dxheight,
                                                    Bands.ToGHz(band),
                                                    LatLon.Earth.Radius * Properties.Settings.Default.Path_Band_Settings[band].K_Factor,
                                                    Properties.Settings.Default.Path_Band_Settings[band].F1_Clearance,
                                                    ElevationData.Database.GetDefaultStepWidth(Model),
                                                    Model,
                                                    myobstr);
                                            st.Stop();
                                            this.ReportProgress(0, Name + " calculating path[ " + Bands.GetStringValue(band) + "]: " + Properties.Settings.Default.MyCall + "<>" + ld.Call + ", " + st.ElapsedMilliseconds.ToString() + " ms.");
                                        }
                                        if (this.CancellationPending)
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(Name + " error processing call [" + ld.Call + "]: " + ex.ToString());
                                }
                            }
                            // wait to keep cpu load low
                            Thread.Sleep(Properties.Settings.Default.Background_Calculations_ThreadWait);
                            this.ReportProgress(0, Name + " finished.");
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage(ex.ToString());
                        }
                    }
                    // sleep when running periodically
                    if (Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY)
                    {
                        int i = 0;
                        while (!this.CancellationPending && (i < interval))
                        {
                            Thread.Sleep(1000);
                            i++;
                        }
                    }
                }
                while (Options == BACKGROUNDUPDATERSTARTOPTIONS.RUNPERIODICALLY);
                if (this.CancellationPending)
                {
                    this.ReportProgress(0, Name + " cancelled.");
                    Log.WriteMessage(Name + " cancelled.");
                }
                else
                {
                    this.ReportProgress(0, Name + "finished.");
                    Log.WriteMessage(Name + " finished.");
                }
            }

            #endregion
        }
    }
}
