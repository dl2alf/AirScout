using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Globalization;
using System.Net;
using System.IO;
using AirScout.Core;
using AirScout.Aircrafts;
using AirScout.AircraftPositions;
using AirScout.PlaneFeeds.Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoutBase.Core;
using System.Diagnostics;
using AirScout.PlaneFeeds.Plugin.MEFContract;

namespace AirScout.PlaneFeeds
{

    // enum for ReportProgress messages
    public enum PROGRESS
    {
        ERROR = -1,
        STATUS = 0,
        PLANES = 1,
        FINISHED = 100
    }

    // enum for PlaneFeed status
    public enum STATUS
    {
        ERROR = -1,
        OK = 0
    }

    [System.ComponentModel.DesignerCategory("")]
    [DefaultPropertyAttribute("Name")]
    public class PlaneFeed : BackgroundWorker
    { 
        private PlaneInfoCache PlanePositions = new PlaneInfoCache();
        private PlaneFeedWorkEventArgs Arguments;

        public PlaneFeed()
            : base ()
        {
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }

        public STATUS Status;

        protected LogWriter Log = LogWriter.Instance;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            Arguments = (PlaneFeedWorkEventArgs)e.Argument;
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = this.GetType().Name;

            // use PlaneInfoConverter for plausibility check
            PlaneInfoConverter C = new PlaneInfoConverter();

            // check boundaries
            if ((Arguments.MaxLat <= Arguments.MinLat) || (Arguments.MaxLon <= Arguments.MinLon))
            {
                Status = STATUS.ERROR;
                this.ReportProgress((int)PROGRESS.ERROR, "Area boundaries mismatch. Check your Covered Area parameters!");
                Log.WriteMessage("Area boundaries mismatch. Check your Covered Area parameters!", LogLevel.Error);
            }
            else
            {
                if (Arguments.Feed == null)
                {
                    Status = STATUS.ERROR;
                    this.ReportProgress((int)PROGRESS.ERROR, "Plane feed plugin not found. Check your settings!");
                    Log.WriteMessage("Plane feed plugin not found. Check your settings!", LogLevel.Error);
                }
                else
                {
                    do
                    {
                        try
                        {
                            Status = STATUS.OK;
                            int interval = Arguments.Interval;
                            // build arguments
                            PlaneFeedPluginArgs feedargs = new PlaneFeedPluginArgs();
                            feedargs.AppDirectory = Arguments.AppDirectory;
                            feedargs.AppDataDirectory = Arguments.AppDataDirectory;
                            feedargs.LogDirectory = Arguments.LogDirectory;
                            feedargs.TmpDirectory = Arguments.TmpDirectory;
                            feedargs.DatabaseDirectory = Arguments.DatabaseDirectory;
                            feedargs.MaxLat = Arguments.MaxLat;
                            feedargs.MinLon = Arguments.MinLon;
                            feedargs.MinLat = Arguments.MinLat;
                            feedargs.MaxLon = Arguments.MaxLon;
                            feedargs.MyLat = Arguments.MyLat;
                            feedargs.MyLon = Arguments.MyLon;
                            feedargs.DXLat = Arguments.DXLat;
                            feedargs.DXLon = Arguments.DXLon;
                            feedargs.MinAlt = Arguments.MinAlt;
                            feedargs.MaxAlt = Arguments.MaxAlt;
                            feedargs.KeepHistory = Arguments.KeepHistory;
                            feedargs.InstanceID = Arguments.InstanceID;
                            feedargs.SessionKey = Arguments.SessionKey;
                            feedargs.GetKeyURL = Arguments.GetKeyURL;

                            // do start procedure
                            Arguments.Feed.Start(feedargs);
                            // run inner loop
                            do
                            {
                                // call plugin's interface to get the planes
                                try
                                {
                                    Stopwatch st = new Stopwatch();
                                    st.Start();
                                    // get plane raw data and do addtional checks
                                    PlaneFeedPluginPlaneInfoList acs = Arguments.Feed.GetPlanes(feedargs);
                                    PlaneInfoList planes = new PlaneInfoList();
                                    int total = acs.Count;
                                    int count = 0;
                                    int errors = 0;
                                    foreach (PlaneFeedPluginPlaneInfo ac in acs)
                                    {
                                        // skip without error when on ground
                                        if (ac.Ground)
                                            continue;
                                        // copy raw data to new PlaneInfo object
                                        PlaneInfo plane = new PlaneInfo();
                                        plane.Hex = ac.Hex;
                                        plane.Lat = ac.Lat;
                                        plane.Lon = ac.Lon;
                                        plane.Alt = ac.Alt;
                                        plane.Call = ac.Call;
                                        plane.Reg = ac.Reg;
                                        plane.Track = ac.Track;
                                        plane.Speed = ac.Speed;
                                        plane.Time = ac.Time;
                                        plane.From = ac.From;
                                        plane.To = ac.To;
                                        plane.VSpeed = ac.VSpeed;
                                        try
                                        {
                                            plane.Category = (PLANECATEGORY)ac.Category;
                                        }
                                        catch
                                        {
                                            plane.Category = PLANECATEGORY.NONE;
                                        }
                                        plane.Type = ac.Type;
                                        plane.Model = ac.Model;
                                        plane.Manufacturer = ac.Manufacturer;
                                        // start checks
                                        // assuming that at least a timestamp is set!
                                        // do basic check on hex --> is strictly needed as identifier
                                        if (!PlaneInfoChecker.Check_Hex(plane.Hex))
                                        {
                                            // try to fill hex from reg
                                            if (!PlaneInfoChecker.Check_Reg(plane.Reg))
                                            {
                                                if (Arguments.LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Hex]: " + plane.Hex, LogLevel.Warning);
                                                errors++;
                                                continue;
                                            }
                                            AircraftDesignator ad = AircraftData.Database.AircraftFindByReg(plane.Reg);
                                            if (ad == null)
                                            {
                                                if (Arguments.LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Hex]: " + plane.Hex, LogLevel.Warning);
                                                errors++;
                                                continue;
                                            }
                                            plane.Hex = ad.Hex;
                                        }
                                        // check latitude
                                        if (!PlaneInfoChecker.Check_Lat(plane.Lat))
                                        {
                                            if (Arguments.LogErrors)
                                                Log.WriteMessage("Incorrect aircraft data received [Lat]: " + plane.Lat.ToString("F8", CultureInfo.InvariantCulture), LogLevel.Warning);
                                            errors++;
                                            continue;
                                        }
                                        // skip without error when latitude is out of scope
                                        if ((plane.Lat < Arguments.MinLat) || (plane.Lat > Arguments.MaxLat))
                                            continue;
                                        // check longitude
                                        if (!PlaneInfoChecker.Check_Lon(plane.Lon))
                                        {
                                            if (Arguments.LogErrors)
                                                Log.WriteMessage("Incorrect aircraft data received [Lon]: " + plane.Lon.ToString("F8", CultureInfo.InvariantCulture), LogLevel.Warning);
                                            errors++;
                                            continue;
                                        }
                                        // skip without error when longitude is out of scope
                                        if ((plane.Lon < Arguments.MinLon) || (plane.Lon > Arguments.MaxLon))
                                            continue;
                                        // check altitude
                                        if (!PlaneInfoChecker.Check_Alt(plane.Alt))
                                        {
                                            // try to recover altitude from previuos messages
                                            PlaneInfo info = null;
                                            if (PlanePositions.TryGetValue(plane.Hex, out info))
                                            {
                                                plane.Alt = info.Alt;
                                            }
                                            else
                                            {
                                                if (Arguments.LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Alt]: " + plane.Alt.ToString("F8", CultureInfo.InvariantCulture), LogLevel.Warning);
                                                errors++;
                                                continue;
                                            }
                                        }
                                        // skip without error when altitude_ is out of bounds
                                        if ((plane.Alt_m < Arguments.MinAlt) || (plane.Alt_m > Arguments.MaxAlt))
                                        {
                                            continue;
                                        }
                                        // check call
                                        if (!PlaneInfoChecker.Check_Call(plane.Call))
                                        {
                                            // try to recover from cache if check fails or set it to [unknown]
                                            PlaneInfo info = null;
                                            if (PlanePositions.TryGetValue(plane.Hex, out info))
                                            {
                                                plane.Call = info.Call;
                                            }
                                            else
                                                plane.Call = "[unknown]";
                                        }
                                        // still unknown call --> try to recover last known call from database
                                        if (!PlaneInfoChecker.Check_Call(plane.Call))
                                        {
                                            AircraftDesignator ad = AircraftData.Database.AircraftFindByHex(plane.Hex);
                                            if (ad != null)
                                            {
                                                plane.Call = ad.Call;
                                            }
                                            else
                                                plane.Call = "[unknown]";
                                        }
                                        // check registration 
                                        if (!PlaneInfoChecker.Check_Reg(plane.Reg))
                                        {
                                            // try to recover from cache if check fails or set it to [unknown]
                                            PlaneInfo info = null;
                                            if (PlanePositions.TryGetValue(plane.Hex, out info))
                                            {
                                                plane.Reg = info.Reg;
                                            }
                                            else
                                                plane.Reg = "[unknown]";
                                        }
                                        // still unknown --> try to recover last known reg from database
                                        if (!PlaneInfoChecker.Check_Reg(plane.Reg))
                                        {
                                            AircraftDesignator ad = AircraftData.Database.AircraftFindByHex(plane.Hex);
                                            if (ad != null)
                                            {
                                                plane.Reg = ad.Reg;
                                            }
                                            else
                                                plane.Reg = "[unknown]";
                                        }
                                        // check speed
                                        if (!PlaneInfoChecker.Check_Track(plane.Track))
                                        {
                                            if (Arguments.LogErrors)
                                                Log.WriteMessage("Incorrect aircraft data received [Track]: " + plane.Track.ToString("F8", CultureInfo.InvariantCulture), LogLevel.Warning);
                                            errors++;
                                            continue;
                                        }
                                        // check speed
                                        if (!PlaneInfoChecker.Check_Speed(plane.Speed))
                                        {
                                            // try to recover speed from previous messages
                                            PlaneInfo info = null;
                                            if (PlanePositions.TryGetValue(plane.Hex, out info))
                                            {
                                                plane.Speed = info.Speed;
                                            }
                                            else
                                            {
                                                if (Arguments.LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Speed]: " + plane.Speed.ToString("F8", CultureInfo.InvariantCulture), LogLevel.Warning);
                                                errors++;
                                                continue;
                                            }
                                        }
                                        // check type
                                        if (!PlaneInfoChecker.Check_Type(plane.Type))
                                        {
                                            AircraftDesignator ad = AircraftData.Database.AircraftFindByHex(plane.Hex);
                                            if (ad != null)
                                            {
                                                plane.Type = ad.TypeCode;
                                                // getrest of info later later
                                            }
                                            else
                                            {
                                                // set all type info to unknown
                                                plane.Type = "[unknown]";
                                                plane.Model = "[unknown]";
                                                plane.Manufacturer = "[unknown]";
                                                plane.Category = PLANECATEGORY.NONE;
                                            }
                                        }
                                        // try to recover type info from database if check fails or unknown
                                        if (!PlaneInfoChecker.Check_Manufacturer(plane.Manufacturer) || !PlaneInfoChecker.Check_Model(plane.Model) ||
                                            (plane.Manufacturer == "[unkonwn]") || (plane.Model == "[unknown]"))
                                        {
                                            AircraftTypeDesignator td = AircraftData.Database.AircraftTypeFindByICAO(plane.Type);
                                            if (td != null)
                                            {
                                                plane.Manufacturer = td.Manufacturer;
                                                plane.Model = td.Model;
                                                plane.Category = td.Category;
                                            }
                                            else
                                            {
                                                plane.Manufacturer = "[unknown]";
                                                plane.Model = "[unknown]";
                                                plane.Category = PLANECATEGORY.NONE;
                                            }
                                        }
                                        // remove manufacturer info if part of model description
                                        if (plane.Model.StartsWith(plane.Manufacturer))
                                            plane.Model = plane.Model.Remove(0, plane.Manufacturer.Length).Trim();
                                        // check position against estimated position from last konwn if possible
                                        PlaneInfo oldplane = PlanePositions.Get(plane.Hex, plane.Time, 5);
                                        double dist = 0;
                                        if (Arguments.ExtendedPlausibilityCheck_Enable && (oldplane != null) && ((dist = LatLon.Distance(oldplane.Lat, oldplane.Lon, plane.Lat, plane.Lon)) > Arguments.ExtendedPlausiblityCheck_MaxErrorDist))
                                        {
                                            // report error
                                            if (Arguments.LogErrors)
                                                Log.WriteMessage("Incorrect aircraft position received [(" + oldplane.Lat.ToString("F8") + "," + oldplane.Lon.ToString("F8") + ")<" + dist.ToString("F0") + "km>(" + plane.Lat.ToString("F8") + "," + plane.Lon.ToString("F8") + ")]: " + plane.ToString(), LogLevel.Warning);
                                            errors++;
                                            continue;
                                        }
                                        // all checks successfully done --> add plane to list
                                        planes.Add(plane);
                                        count++;
                                        // cancel thread if requested
                                        if (this.CancellationPending)
                                            return;
                                    }
                                    // update local cache
                                    this.PlanePositions.BulkInsertOrUpdateIfNewer(planes);
                                    // report planes to main program
                                    this.ReportProgress((int)PROGRESS.PLANES, planes);
                                    // update global database
                                    AircraftData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(this, planes);
                                    // update position database if enabled
                                    if (Arguments.KeepHistory)
                                        AircraftPositionData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                                    st.Stop();
                                    string msg = "[" + DateTime.UtcNow.ToString("HH:mm:ss") + "] " +
                                        total.ToString() + " Positions updated from " + Arguments.Feed.Name + ", " +
                                        st.ElapsedMilliseconds.ToString() + " ms. OK: " + count.ToString() + ", Errors: " + errors.ToString();
                                    this.ReportProgress((int)PROGRESS.STATUS, msg);
                                    // write all planes to file
                                    try
                                    {
                                        using (StreamWriter sw = new StreamWriter(Path.Combine(Arguments.TmpDirectory, "planes.csv")))
                                        {
                                            sw.WriteLine("Time;Hex;Lat;Lon;Alt;Track;Speed;Call;Reg;From;To;VSpeed");
                                            foreach (PlaneInfo plane in planes)
                                            {
                                                sw.WriteLine(plane.Time + ";" +
                                                plane.Hex + ";" +
                                                plane.Lat + ";" +
                                                plane.Lon + ";" +
                                                plane.Alt + ";" +
                                                plane.Track + ";" +
                                                plane.Speed + ";" +
                                                plane.Call + ";" +
                                                plane.Reg + ";" +
                                                plane.From + ";" +
                                                plane.To + ";" +
                                                plane.VSpeed);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // do nothing
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Status = STATUS.ERROR;
                                    this.ReportProgress((int)PROGRESS.ERROR, "Plane Feed Execption: " + ex.Message);
                                    Log.WriteMessage(ex.ToString(), LogLevel.Error);
                                }
                                // wait for next execution
                                int i = 0;
                                while (!CancellationPending && (i < interval))
                                {
                                    Thread.Sleep(1000);
                                    i++;
                                }
                            }
                            while (!CancellationPending);
                            // do stop procedure
                            Arguments.Feed.Stop(feedargs);
                        }
                        catch (Exception ex)
                        {
                            Status = STATUS.ERROR;
                            this.ReportProgress((int)PROGRESS.ERROR, "Plane Feed Execption: " + ex.Message);
                            Log.WriteMessage(ex.ToString(), LogLevel.Error);
                            Console.WriteLine("Plane Feed Execption: " + ex.ToString(), LogLevel.Error);
                        }
                    }
                    while (!this.CancellationPending);
                }
            }
            this.ReportProgress((int)PROGRESS.FINISHED);
            Log.WriteMessage("Finished.");
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            // do stop procedure
            try
            {
                Arguments.Feed.Stop(null);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }

            base.OnRunWorkerCompleted(e);
        }

    }

}
