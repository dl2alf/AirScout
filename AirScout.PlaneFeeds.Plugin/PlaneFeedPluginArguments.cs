using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirScout.PlaneFeeds.Plugin
{
    public class PlaneFeedPluginArgs
    {
        public string AppDirectory = "";
        public string AppDataDirectory = "";
        public string LogDirectory = "";
        public string TmpDirectory = "";
        public string DatabaseDirectory = "";
        public string PlanePositionsDirectory = "";

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

        public string InstanceID;
        public string SessionKey;
        public string GetKeyURL;

        public bool LogPlanePositions = false;

    }

}
