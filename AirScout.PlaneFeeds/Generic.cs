using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using AirScout.Aircrafts;
using ScoutBase.Core;

namespace AirScout.PlaneFeeds.Generic
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

    // class for PlaneFeed property setting
    public class PlaneFeedProperty
    {
        public string Info = "";
        public string Value = "";

        public PlaneFeedProperty(string info, string value)
        {
            Info = info;
            Value = value;
        }
    }

    public class PlaneFeedWorkEventArgs
    {
        public string AppDirectory = "";
        public string AppDataDirectory = "";
        public string LogDirectory = "";
        public string TmpDirectory = "";
        public string DatabaseDirectory = "";

        public double MaxLat = 0;
        public double MinLon = 0;
        public double MinLat = 0;
        public double MaxLon = 0;

        public int MinAlt = 0;
        public int MaxAlt = 0;

        public double MyLat = 0;
        public double MyLon = 0;
        public double DXLat = 0;
        public double DXLon = 0;

        public bool KeepHistory = false;
    }

    [Serializable]
    public class PlaneFeedSettings
    {
    }

    [DefaultPropertyAttribute("Name")]
    public class PlaneFeed : BackgroundWorker
    {
        [Browsable(false)]
        public virtual string Name
        {
            get
            {
                return Properties.Settings.Default.Generic_Name;
            }
            protected set
            {
                Properties.Settings.Default.Generic_Name = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public virtual string Disclaimer
        {
            get
            {
                return Properties.Settings.Default.Generic_Disclaimer;
            }
            protected set
            {
                Properties.Settings.Default.Generic_Disclaimer = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public virtual string DisclaimerAccepted
        {
            get
            {
                return Properties.Settings.Default.Generic_Disclaimer_Accepted;
            }
            set
            {
                Properties.Settings.Default.Generic_Disclaimer_Accepted = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public virtual string Info
        {
            get
            {
                return Properties.Settings.Default.Generic_Info;
            }
            protected set
            {
                Properties.Settings.Default.Generic_Info = value;
                Properties.Settings.Default.Save();
            }
        }

        [Browsable(false)]
        public bool HasSettings;

        [Browsable(false)]
        public bool CanImport;

        [Browsable(false)]
        public bool CanExport;

        public PlaneFeedSettings FeedSettings = new PlaneFeedSettings();

        public STATUS Status;

        public string AppDirectory = "";
        public string AppDataDirectory = "";
        public string LogDirectory = "";
        public string TmpDirectory = "";
        public string DatabaseDirectory = "";
 
        public double MaxLat = 60;
        public double MinLon = -15;
        public double MinLat = 30;
        public double MaxLon = 35;

        public int MinAlt = 5000;
        public int MaxAlt = 12200;

        public double MyLat;
        public double MyLon;
        public double DXLat;
        public double DXLon;

        public bool KeepHistory = false;

        protected VarConverter VC;

        protected LogWriter Log = LogWriter.Instance;

        public PlaneFeed()
            : base()
        {
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            this.CanImport = false;
            this.CanExport = false;
            this.HasSettings = false;
            // Initailize variables
            VC = new VarConverter();

        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            Log.WriteMessage("Started.");
            PlaneFeedWorkEventArgs args = (PlaneFeedWorkEventArgs)e.Argument;
            // set parameters from arguments

            AppDirectory = args.AppDirectory;
            AppDataDirectory = args.AppDataDirectory;
            LogDirectory = args.LogDirectory;
            TmpDirectory = args.TmpDirectory;
            DatabaseDirectory = args.DatabaseDirectory;
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

            Status = STATUS.OK;

            // narrow args according to QSO partners
            if (MyLat > DXLat)
            {
                MaxLat = MyLat;
                MinLat = DXLat;
            }
            else
            {
                MaxLat = DXLat;
                MinLat = MyLat;
            }
            if (MyLon > DXLon)
            {
                MaxLon = MyLon;
                MinLon = DXLon;
            }
            else
            {
                MaxLon = DXLon;
                MinLon = MyLon;
            }
            int interval = 60;
            do
            {
                int count = 100;
                DateTime start = DateTime.UtcNow;
                List<PlaneInfo> planes = new List<PlaneInfo>();
                Random rnd = new Random();
                for (int ii = 0; ii < count; ii++)
                {
                    PlaneInfo info = new PlaneInfo();
                    info.Call = "RND" + ii.ToString("0000");
                    info.Hex = ii.ToString("X4");
                    info.Lat = rnd.NextDouble() * (MaxLat-MinLat) + MinLat;
                    info.Lon = rnd.NextDouble() * (MaxLon - MinLon) + MinLon;
                    info.Alt = rnd.Next((int)UnitConverter.m_ft(MinAlt), (int)UnitConverter.m_ft(MaxAlt));
                    info.Speed = rnd.Next(200, 600);
                    info.Track = rnd.Next(0, 360);
                    int t = rnd.Next(0, 100);
                    info.Type = "C206";
                    if (ii > 10)
                        info.Type = "A320";
                    if (ii > 70)
                        info.Type = "B744";
                    if(ii > 95)
                        info.Type = "A388";
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
                    planes.Add(info);
                }
                ReportProgress((int)PROGRESS.PLANES, planes);
                AircraftData.Database.PlaneInfoBulkInsertOrUpdateIfNewer(planes);
                string msg = "[" + start.ToString("HH:mm:ss") + "] " +
                    count.ToString() + " Positions randomized.";
                this.ReportProgress((int)PROGRESS.STATUS, msg);
                int i = 0;
                while (!CancellationPending && (i < interval))
                {
                    Thread.Sleep(1000);
                    i++;
                }
            }
            while (!CancellationPending);
            this.ReportProgress((int)PROGRESS.FINISHED);
            Log.WriteMessage("Finished.");
        }

        public virtual Object GetFeedSettings()
        {
            return FeedSettings;
        }

        public virtual void Import()
        {

        }

        public virtual void Export()
        {
            SaveFileDialog Dlg = new SaveFileDialog();
            Dlg.DefaultExt = "feed";
            Dlg.Filter = "Plane Feeds | .feed";
            Dlg.OverwritePrompt = true;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer s = new XmlSerializer(typeof(PlaneFeed));
                s.Serialize(File.Create(Dlg.FileName), this);
            }

        }

        public override string ToString()
        {
            return Name;
        }

    }

    public class PlaneFeedEnumeration
    {
        public ArrayList EnumFeeds()
        {
            ArrayList feeds = new ArrayList();
            feeds.Add(new PlaneFeed());
            feeds.Add(new PlaneFeed_AS());
            feeds.Add(new PlaneFeed_PF());
            feeds.Add(new PlaneFeed_VR());
//            feeds.Add(new PlaneFeed_FR());
            feeds.Add(new PlaneFeed_AJ());
            feeds.Add(new PlaneFeed_FJ());
            feeds.Add(new PlaneFeed_ADSB());
            feeds.Add(new PlaneFeed_RTL());
            return feeds;
        }
    }
}
