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
using AirScout.Aircrafts;
using AirScout.PlaneFeeds.Generic;
using ScoutBase.Core;

namespace AirScout.PlaneFeeds
{

    public class PlaneFeedSettings_FJ : PlaneFeedSettings
    {
        [DescriptionAttribute("Path to JSON file.")]
        public virtual string URL
        {
            get
            {
                return Properties.Settings.Default.FJ_Path;
            }
            set
            {
                Properties.Settings.Default.FJ_Path = value;
                Properties.Settings.Default.Save();
            }
        }

        [DescriptionAttribute("Update interval for file reading [seconds]")]
        public virtual int Interval
        {
            get
            {
                return Properties.Settings.Default.FJ_Interval;
            }
            set
            {
                Properties.Settings.Default.FJ_Interval = value;
                Properties.Settings.Default.Save();
            }
        }

    }

    public class PlaneFeed_FJ : PlaneFeed
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return Properties.Settings.Default.FJ_Name; ;
            }
            protected set
            {
                Properties.Settings.Default.FJ_Name = value;
                Properties.Settings.Default.Save();
            }
        }


        [Browsable(false)]
        public override string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.FJ_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.FJ_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.FJ_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.FJ_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Info
        {
            get
            {
                return Properties.Settings.Default.FJ_Info;
            }
            protected set
            {
                Properties.Settings.Default.FJ_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        public new PlaneFeedSettings_FJ FeedSettings = new PlaneFeedSettings_FJ();

        public PlaneFeed_FJ()
            : base ()
        {
            HasSettings = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            PlaneFeedWorkEventArgs args = (PlaneFeedWorkEventArgs)e.Argument;
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

            // check boundaries
            if ((MaxLat <= MinLat) || (MaxLon <= MinLon))
            {
                Status = STATUS.ERROR;
                this.ReportProgress((int)PROGRESS.ERROR, "Area boundaries mismatch. Check your Covered Area parameters!");
            }
            else
            {
                Status = STATUS.OK;
                int interval = Properties.Settings.Default.FJ_Interval;
                // run loop
                do
                {
                    string json = "";
                    DateTime start = DateTime.UtcNow;
                    // calculate url and get json
                    String path = Properties.Settings.Default.FJ_Path;
                    try
                    {
                        FileStream fs = new FileStream(path,FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            json = sr.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        // do nothing
                        Log.WriteMessage(ex.Message);
                    }
                    int count = 0;
                    List<PlaneInfo> planes = new List<PlaneInfo>();
                    // analyze json string for planes data
                    if (json.Contains(']'))
                    {
                        json = json.Remove(0, json.IndexOf(":[") - 10);
                        json = json.Replace("\r", String.Empty);
                        json = json.Replace("\n", string.Empty);
                        // split plane positions
                        string[] items = json.Split(']');
                        foreach (string item in items)
                        {
                            try
                            {
                                if (item.Length > 11)
                                {
                                    string d = item.Substring(2).Replace(":", ",").Replace("\"", string.Empty).Replace("[", string.Empty);
                                    string[] par = d.Split(',');

                                    // fill planeinfo with fields from JSON string
                                    PlaneInfo info = new PlaneInfo();
                                    long l = System.Convert.ToInt64(par[11].ToString());
                                    DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                    timestamp = timestamp.AddSeconds(l);
                                    info.Time = timestamp;
                                    try
                                    {
                                        // fill in the "must have" info
                                        info.Call = par[17];
                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        provider.NumberDecimalSeparator = ".";
                                        provider.NumberGroupSeparator = ",";
                                        provider.NumberGroupSizes = new int[] { 3 };
                                        info.Lat = System.Convert.ToDouble(par[2], provider);
                                        info.Lon = System.Convert.ToDouble(par[3], provider);
                                        info.Alt = System.Convert.ToInt32(par[5]);
                                        info.Speed = System.Convert.ToInt32(par[6]);
                                        info.Hex = par[1];
                                        try
                                        {
                                            if (par[4].Length > 0)
                                                info.Track = System.Convert.ToInt32(par[4]);
                                        }
                                        catch
                                        {
                                        }
                                        info.Reg = par[10].ToString();
                                        info.Type = par[9].ToString();
                                        AircraftTypeDesignator type = AircraftData.Database.AircraftTypeFindByICAO(info.Type);
                                        if (info != null)
                                        {
                                            info.Manufacturer = type.Manufacturer;
                                            info.Model = type.Model;
                                            info.Category = type.Category;
                                        }
                                        else
                                        {
                                            info.Manufacturer = "[unknown]";
                                            info.Model = "[unknown]";
                                            info.Category = PLANECATEGORY.NONE;
                                        }
                                        if ((info.Lat >= MinLat) &&
                                            (info.Lat <= MaxLat) &&
                                            (info.Lon >= MinLon) &&
                                            (info.Lon <= MaxLon) &&
                                            (info.Alt_m >= MinAlt) &&
                                            (info.Alt_m <= MaxAlt))
                                        {
                                            planes.Add(info);
                                            count++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // discard data if any exception occured
                                        Log.WriteMessage(ex.Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteMessage(ex.Message);
                            }
                        }
                    }
                    DateTime stop = DateTime.UtcNow;
                    ReportProgress((int)PROGRESS.PLANES, planes);
                    AircraftData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                    string msg = "[" + start.ToString("HH:mm:ss") + "] " +
                        count.ToString() + " Positions updated from " + path + ", " +
                        (stop - start).Milliseconds.ToString() + " ms.";
                    this.ReportProgress((int)PROGRESS.STATUS, msg);
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
