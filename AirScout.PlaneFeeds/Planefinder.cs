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
using AirScout.PlaneFeeds.Generic;
using Newtonsoft.Json;
using ScoutBase.Core;

namespace AirScout.PlaneFeeds
{
    public class PlaneFeedSettings_PF : PlaneFeedSettings
    {
        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Base URL for website.")]
        public virtual string URL
        {
            get
            {
                return Properties.Settings.Default.PF_URL;
            }
            set
            {
                Properties.Settings.Default.PF_URL = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Update interval for website request [seconds]")]
        public virtual int Interval
        {
            get
            {
                return Properties.Settings.Default.PF_Interval;
            }
            set
            {
                Properties.Settings.Default.PF_Interval = value;
                Properties.Settings.Default.Save();
            }
        }

        [CategoryAttribute("Web Feed")]
        [DescriptionAttribute("Save downloaded JSON to file")]
        public virtual bool SaveToFile
        {
            get
            {
                return Properties.Settings.Default.PF_SaveToFile;
            }
            set
            {
                Properties.Settings.Default.PF_SaveToFile = value;
                Properties.Settings.Default.Save();
            }
        }

    }

    public class PlaneFeed_PF : PlaneFeed
    {
        [Browsable(false)]
        public override string Name
        {
            get
            {
                return Properties.Settings.Default.PF_Name; ;
            }
            protected set
            {
                Properties.Settings.Default.PF_Name = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.PF_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.PF_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.PF_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.PF_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public override string Info
        {
            get
            {
                return Properties.Settings.Default.PF_Info;
            }
            protected set
            {
                Properties.Settings.Default.PF_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        public new PlaneFeedSettings_PF FeedSettings = new PlaneFeedSettings_PF();

        public PlaneFeed_PF()
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


            // check boundaries
            if ((MaxLat <= MinLat) || (MaxLon <= MinLon))
            {
                Status = STATUS.ERROR;
                this.ReportProgress((int)PROGRESS.ERROR, "Area boundaries mismatch. Check your Covered Area parameters!");
            }
            else
            {
                Status = STATUS.OK;
                int interval = Properties.Settings.Default.PF_Interval;
                // run loop
                do
                {
                    string json = "";
                    DateTime start = DateTime.UtcNow;
                    String url = VC.ReplaceAllVars(Properties.Settings.Default.PF_URL);
                    try
                    {
                        HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                        webrequest.Referer = "http://planefinder.net/";
                        webrequest.UserAgent = "Mozilla/5.0 (X11; Linux x86_64; rv:17.0) Gecko/20130807 Firefox/17.0";
                        HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                        using (StreamReader sr = new StreamReader(webresponse.GetResponseStream()))
                        {
                            json = sr.ReadToEnd();
                        }
                    }
                    catch
                    {
                        // do nothing
                    }
                    if (FeedSettings.SaveToFile)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "planefinder.json"))
                            {
                                sw.WriteLine(json);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteMessage(ex.Message);
                        }
                    }
                    int total = 0;
                    int count = 0;
                    int errors = 0;
                    List<PlaneInfo> planes = new List<PlaneInfo>();
                    // analyze json string for planes data
                    if (json.Contains(']'))
                    {
                        json = json.Remove(0, json.IndexOf(":{") + 2);
                        // split plane positions
                        string[] items = json.Split(']');
                        string[] par = new string[0];
                        foreach (string item in items)
                        {
                            try
                            {
                                if (item.Length > 11)
                                {
                                    total++;
                                    string d = item.Substring(2).Replace(":", ",").Replace("\"", string.Empty).Replace("[", string.Empty);
                                    par = d.Split(',');

                                    // fill planeinfo with fields from JSON string
                                    PlaneInfo info = new PlaneInfo();
                                    long l = System.Convert.ToInt64(par[10].ToString());
                                    DateTime timestamp = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                    timestamp = timestamp.AddSeconds(l);
                                    info.Time = timestamp;
                                    try
                                    {
                                        // fill in the "must have" info
                                        info.Call = par[4];
                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        provider.NumberDecimalSeparator = ".";
                                        provider.NumberGroupSeparator = ",";
                                        provider.NumberGroupSizes = new int[] { 3 };
                                        info.Lat = double.Parse(par[5], NumberStyles.Float, provider);
                                        info.Lon = double.Parse(par[6], NumberStyles.Float, provider);
                                        info.Alt = double.Parse(par[7], NumberStyles.Float, provider);
                                        info.Speed = double.Parse(par[9], NumberStyles.Float, provider);
                                        info.Hex = par[0];
                                        try
                                        {
                                            if (par[8].Length > 0)
                                                info.Track = System.Convert.ToInt32(par[8]);
                                        }
                                        catch
                                        {
                                        }
                                        info.Reg = par[3].ToString();
                                        info.Type = par[1].ToString();
                                        AircraftTypeDesignator type = AircraftData.Database.AircraftTypeFindByICAO(info.Type);
                                        if (type != null)
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
                                        errors++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // discard data if any exception occured
                                Log.WriteMessage(ex.Message);
                                errors++;
                            }
                        }
                    }
                    DateTime stop = DateTime.UtcNow;
                    ReportProgress((int)PROGRESS.PLANES, planes);
                    AircraftData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                    string msg = "[" + start.ToString("HH:mm:ss") + "] " +
                        total.ToString() + " Positions updated from " + url + ", " +
                        (stop - start).Milliseconds.ToString() + " ms. OK: " + count.ToString() + ", Errors: " + errors.ToString() ;
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
