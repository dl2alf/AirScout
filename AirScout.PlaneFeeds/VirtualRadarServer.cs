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
using AirScout.PlaneFeeds.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScoutBase.Core;
using System.Diagnostics;

namespace AirScout.PlaneFeeds
{
    public class PlaneFeedSettings_VR : PlaneFeedSettings
    {
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Base URL for website.")]
        public virtual string URL
        {
            get
            {
                return Properties.Settings.Default.VR_URL;
            }
            set
            {
                Properties.Settings.Default.VR_URL = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Update interval for website request [seconds]")]
        public virtual int Interval
        {
            get
            {
                return Properties.Settings.Default.VR_Interval;
            }
            set
            {
                Properties.Settings.Default.VR_Interval = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Save downloaded JSON to file")]
        public virtual bool SaveToFile
        {
            get
            {
                return Properties.Settings.Default.VR_SaveToFile;
            }
            set
            {
                Properties.Settings.Default.VR_SaveToFile = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Extended plausibility check of received position messages")]
        public virtual bool ExtendedPlausibilityCheck
        {
            get
            {
                return Properties.Settings.Default.VR_ExtendedPlausibilityCheck;
            }
            set
            {
                Properties.Settings.Default.VR_ExtendedPlausibilityCheck = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Maximum error in [km] between estimated and reported position")]
        public virtual double EstimatedPositionMaxError
        {
            get
            {
                return Properties.Settings.Default.VR_EstimatedPosition_MaxError;
            }
            set
            {
                Properties.Settings.Default.VR_EstimatedPosition_MaxError = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Write errors to log file")]
        public virtual bool LogErros
        {
            get
            {
                return Properties.Settings.Default.VR_LogErrors;
            }
            set
            {
                Properties.Settings.Default.VR_LogErrors = value;
                Properties.Settings.Default.Save();
            }
        }

    }

    public class PlaneFeed_VR : PlaneFeed
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return Properties.Settings.Default.VR_Name; ;
            }
            protected set
            {
                Properties.Settings.Default.VR_Name = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.VR_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.VR_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.VR_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.VR_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Info
        {
            get
            {
                return Properties.Settings.Default.VR_Info;
            }
            protected set
            {
                Properties.Settings.Default.VR_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        public new PlaneFeedSettings_VR FeedSettings = new PlaneFeedSettings_VR();

        private PlaneInfoCache PlanePositions = new PlaneInfoCache();

        public PlaneFeed_VR()
            : base ()
        {
            HasSettings = true;
        }

        private string ReadPropertyString(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return null;
            return o.Property(propertyname).Value.Value<string>();
        }

        private int ReadPropertyDoubleToInt(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return int.MinValue;
            double d = ReadPropertyDouble(o,propertyname);
            if ((d != double.MinValue) && (d >= int.MinValue) && (d <= int.MaxValue))
                return (int)d;
            return int.MinValue;
        }

        private double ReadPropertyDouble(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return double.MinValue;
            return o.Property(propertyname).Value.Value<double>();
        }

        private long ReadPropertyLong(JObject o, string propertyname)
        {
            if (o.Property(propertyname) == null)
                return long.MinValue;
            return o.Property(propertyname).Value.Value<long>();
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            PlaneFeedWorkEventArgs args = (PlaneFeedWorkEventArgs)e.Argument;
            if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = this.GetType().Name;

            // set directories
            AppDirectory = args.AppDirectory;
            AppDataDirectory = args.AppDataDirectory;
            LogDirectory = args.LogDirectory;
            TmpDirectory = args.TmpDirectory;
            DatabaseDirectory = args.DatabaseDirectory;

            // set boundaries from arguments
            MaxLat = args.MaxLat;
            MinLon = args.MinLon;
            MinLat = args.MinLat;
            MaxLon = args.MaxLon;
            MyLat = args.MyLat;
            MyLon = args.MyLon;
            DXLat = args.DXLat;
            DXLon = args.DXLon;
            MinAlt = args.MinAlt;
            MaxAlt = args.MaxAlt;

            // keep history settings from arguments
            KeepHistory = args.KeepHistory;

            // intialize variables
            VC.AddVar("APPDIR", AppDirectory);
            VC.AddVar("DATADIR", AppDataDirectory);
            VC.AddVar("LOGDIR", LogDirectory);
            VC.AddVar("DATABASEDIR", DatabaseDirectory);
            VC.AddVar("MINLAT", MinLat);
            VC.AddVar("MAXLAT", MaxLat);
            VC.AddVar("MINLON", MinLon);
            VC.AddVar("MAXLON", MaxLon);
            VC.AddVar("MINALTM", MinAlt);
            VC.AddVar("MAXALTM", MaxAlt);
            VC.AddVar("MINALTFT", (int)UnitConverter.m_ft((double)MinAlt));
            VC.AddVar("MAXALTFT", (int)UnitConverter.m_ft((double)MaxAlt));

            // use PlaneInfoConverter for plausibility check
            PlaneInfoConverter C = new PlaneInfoConverter();

            // check boundaries
            if ((MaxLat <= MinLat) || (MaxLon <= MinLon))
            {
                Status = STATUS.ERROR;
                this.ReportProgress((int)PROGRESS.ERROR, "Area boundaries mismatch. Check your Covered Area parameters!");
            }
            else
            {
                Status = STATUS.OK;
                int interval = Properties.Settings.Default.VR_Interval;
                // run loop
                do
                {
                    string json = "";
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    // calculate url and get json
                    String url = VC.ReplaceAllVars(Properties.Settings.Default.VR_URL);
                    try
                    {
                        Console.WriteLine("Creating web request: " + url);
                        HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                        webrequest.Referer = "http://www.vrs-world.com/";
                        webrequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:37.0) Gecko/20100101 Firefox/37.0";
                        webrequest.Accept = "application/json, text/javascript, */*;q=0.01";
                        webrequest.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
                        Console.WriteLine("Getting web response");
                        HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                        Console.WriteLine("Reading stream");
                        using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
                        {
                            json = sr.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Log.WriteMessage(ex.ToString());
                        // do nothing
                    }
                    if (FeedSettings.SaveToFile)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "vrs" + DateTime.UtcNow.ToString("yyyy-MM-dd HH_mm_ss") + ".json"))
                            {
                                sw.WriteLine(json);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage(ex.ToString());
                        }
                    }
                    // deserialize JSON file
                    int total = 0;
                    int count = 0;
                    int errors = 0;
                    try
                    {
                        JObject root = (JObject)JsonConvert.DeserializeObject(json);
                        // 2017-07-23: workaround for "jumping planes" due to incorrect time stamps
                        // try to get the server time to adjust the time stamps in plane positions
                        // --> compare server time with local time and calculate offset
                        // default offset is 0
                        long toffset = 0;
                        try
                        {
                            // get local time of request in milliseconds
                            DateTime lt = DateTime.UtcNow;
                            long ltime = (long)(lt - new DateTime(1970, 1, 1)).TotalMilliseconds;
                            // get server time in milliseconds
                            long stime = ReadPropertyLong(root, "stm");
                            DateTime sti = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(stime);
                            // calculate offset in milliseconds
                            toffset = ltime - stime;
                            // check value
                            string message = "Server timestamp difference (server <> local): " + sti.ToString("yyyy-MM-dd HH:mm:ss,fffZ") + " <> " + lt.ToString("yyyy-MM-dd HH:mm:ss,fffZ");
                            // server time is more than 10.000 ms in the future --> keep offset for correction and log an error message
                            // else --> set offset to 0 to work with real timestamps of entries without correction
                            if (toffset <= -10000)
                            {
                                message += " --> timestamp correction applied!";
                                Log.WriteMessage(message);

                            }
                            else
                            {
                                // clear offset
                                toffset = 0;
                            }
                        }
                        catch
                        {
                            // do nothing if property is not found
                        }
                        // analyze json string for planes data
                        foreach (JProperty proot in root.Children<JProperty>())
                        {
                            // get the planes position list
                            if (proot.Name == "acList")
                            {
                                List<PlaneInfo> planes = new List<PlaneInfo>();
                                foreach (JArray a in proot.Children<JArray>())
                                {
                                    foreach (JObject o in a.Values<JObject>())
                                    {
                                        PlaneInfo plane = new PlaneInfo();
                                        total++;
                                        try
                                        {
                                            // get hex first
                                            plane.Hex = ReadPropertyString(o, "Icao");
                                            // do basic check on hex --> is strictly needed as identifier
                                            if (!PlaneInfoChecker.Check_Hex(plane.Hex))
                                            {
                                                if (Properties.Settings.Default.VR_LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Hex]: " + plane.Hex);
                                                errors++;
                                                continue;
                                            }
                                            // get position and do basic check on lat/lon
                                            plane.Lat = ReadPropertyDouble(o, "Lat");
                                            if (!PlaneInfoChecker.Check_Lat(plane.Lat))
                                            {
                                                if (Properties.Settings.Default.VR_LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Lat]: " + plane.Lat.ToString("F8", CultureInfo.InvariantCulture));
                                                errors++;
                                                continue;
                                            }
                                            plane.Lon = ReadPropertyDouble(o, "Long");
                                            if (!PlaneInfoChecker.Check_Lon(plane.Lon))
                                            {
                                                if (Properties.Settings.Default.VR_LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Lon]: " + plane.Lon.ToString("F8", CultureInfo.InvariantCulture));
                                                errors++;
                                                continue;
                                            }
                                            // get altitude
                                            // 2017-07-23: take "GAlt" (corrected altitude by air pressure) rather than "Alt"
                                            plane.Alt = ReadPropertyDoubleToInt(o, "GAlt");
                                            // do basic chekc on altitude
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
                                                    if (Properties.Settings.Default.VR_LogErrors)
                                                        Log.WriteMessage("Incorrect aircraft data received [Alt]: " + plane.Alt.ToString("F8", CultureInfo.InvariantCulture));
                                                    errors++;
                                                    continue;
                                                }
                                            }
                                            // continue if alt is out of bounds
                                            if ((plane.Alt_m < MinAlt) || (plane.Alt_m > MaxAlt))
                                            {
                                                continue;
                                            }
                                            // get callsign
                                            plane.Call = ReadPropertyString(o, "Call");
                                            // do basic check --> try to recover from cache if check fails or set it to [unknown]
                                            if (!PlaneInfoChecker.Check_Call(plane.Call))
                                            {
                                                PlaneInfo info = null;
                                                if (PlanePositions.TryGetValue(plane.Hex, out info))
                                                {
                                                    plane.Call = info.Call;
                                                }
                                                else
                                                    plane.Call = "[unknown]";
                                            }
                                            // still unknown --> try to recover last known call from database
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
                                            // get registration
                                            plane.Reg = ReadPropertyString(o, "Reg");
                                            // do basic check --> try to recover from cache if check fails or set it to [unknown]
                                            if (!PlaneInfoChecker.Check_Reg(plane.Reg))
                                            {
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
                                            // get track
                                            plane.Track = ReadPropertyDoubleToInt(o, "Trak");
                                            // do basic check
                                            if (!PlaneInfoChecker.Check_Track(plane.Track))
                                            {
                                                if (Properties.Settings.Default.VR_LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Track]: " + plane.Track.ToString("F8", CultureInfo.InvariantCulture));
                                                errors++;
                                                continue;
                                            }
                                            // get speed
                                            plane.Speed = ReadPropertyDoubleToInt(o, "Spd");
                                            // do basic check
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
                                                    if (Properties.Settings.Default.VR_LogErrors)
                                                        Log.WriteMessage("Incorrect aircraft data received [Speed]: " + plane.Speed.ToString("F8", CultureInfo.InvariantCulture));
                                                    errors++;
                                                    continue;
                                                }
                                            }
                                            // get position timestamp
                                            // CAUTION!! time is UNIX time in milliseconds
                                            long l = ReadPropertyLong(o, "PosTime");
                                            if (l != long.MinValue)
                                            {
                                                // 2017-07-23: correct timestamp with offset
                                                l = l + toffset;
                                                DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                                timestamp = timestamp.AddMilliseconds(l);
                                                plane.Time = timestamp;
                                                plane.Comment = toffset.ToString();
                                            }
                                            else
                                            {
                                                if (Properties.Settings.Default.VR_LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft data received [Time]: " + l.ToString());
                                                errors++;
                                                continue;
                                            }
                                            // get type info
                                            plane.Type = ReadPropertyString(o, "Type");
                                            if (!PlaneInfoChecker.Check_Type(plane.Type))
                                            {
                                                AircraftDesignator ad = AircraftData.Database.AircraftFindByHex(plane.Hex);
                                                if (ad != null)
                                                    plane.Type = ad.TypeCode;
                                                else
                                                    plane.Type = "[unknown]";
                                            }
                                            // get extended plane type info
                                            plane.Manufacturer = ReadPropertyString(o, "Man");
                                            plane.Model = ReadPropertyString(o, "Mdl");
                                            try
                                            {
                                                plane.Category = (PLANECATEGORY)ReadPropertyLong(o, "WTC");
                                            }
                                            catch
                                            {
                                                plane.Category = PLANECATEGORY.NONE;
                                            }
                                            // keep the "SUPERHEAVY" category for A380-800 as not supported by VirtualRadarServer yet
                                            if (plane.Type == "A388")
                                                plane.Category = PLANECATEGORY.SUPERHEAVY;
                                            // try to recover type info from database if check fails
                                            if (!PlaneInfoChecker.Check_Manufacturer(plane.Manufacturer) || !PlaneInfoChecker.Check_Model(plane.Model))
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
                                            // remove manufacturer if part of model description
                                            if (plane.Model.StartsWith(plane.Manufacturer))
                                                plane.Model = plane.Model.Remove(0, plane.Manufacturer.Length).Trim();
                                            // check position against etimated position if possible
                                            PlaneInfo oldplane = PlanePositions.Get(plane.Hex, plane.Time, 5);
                                            double dist = 0;
                                            if (Properties.Settings.Default.VR_ExtendedPlausibilityCheck && (oldplane != null) && ((dist = LatLon.Distance(oldplane.Lat, oldplane.Lon, plane.Lat, plane.Lon)) > Properties.Settings.Default.VR_EstimatedPosition_MaxError))
                                            {
                                                // report error
                                                if (Properties.Settings.Default.VR_LogErrors)
                                                    Log.WriteMessage("Incorrect aircraft position received [(" + oldplane.Lat.ToString("F8") + "," + oldplane.Lon.ToString("F8") + ")<" + dist.ToString("F0") + "km>(" + plane.Lat.ToString("F8") + "," + plane.Lon.ToString("F8") + ")]: " + plane.ToString());
                                                errors++;
                                                continue;
                                            }
                                            if (plane.Speed < 0)
                                                Console.WriteLine("");
                                            // all checks successfully done --> add plane to list
                                            planes.Add(plane);
                                            count++;
                                            if (this.CancellationPending)
                                                return;
                                        }
                                        catch (Exception ex)
                                        {
                                            Log.WriteMessage(ex.Message);
                                            errors++;
                                        }
                                    }
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
                                if (KeepHistory)
                                    AircraftPositionData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                                st.Stop();
                                string msg = "[" + DateTime.UtcNow.ToString("HH:mm:ss") + "] " +
                                    total.ToString() + " Positions updated from " + url + ", " +
                                    st.ElapsedMilliseconds.ToString() + " ms. OK: " + count.ToString() + ", Errors: " + errors.ToString();
                                this.ReportProgress((int)PROGRESS.STATUS, msg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteMessage(ex.Message);
                    }
                    int i = 0;
                    while (!CancellationPending && (i < interval))
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                }
                while (!this.CancellationPending || (Status != STATUS.OK));
            }
            this.ReportProgress((int)PROGRESS.FINISHED);
            Log.WriteMessage("Finished.");
        }

        public override Object GetFeedSettings()
        {
            return FeedSettings;
        }

    }
}
