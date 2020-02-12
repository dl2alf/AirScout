using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirScout.PlaneFeeds.Plugin.MEFContract;

namespace AirScout.PlaneFeeds
{
    public class PlaneFeedWorkEventArgs
    {
        // Plane feed plugin
        public IPlaneFeedPlugin Feed = null;

        // Directories
        public string AppDirectory = "";
        public string AppDataDirectory = "";
        public string LogDirectory = "";
        public string TmpDirectory = "";
        public string DatabaseDirectory = "";

        // Scope für plane positions
        public double MaxLat = 0;
        public double MinLon = 0;
        public double MinLat = 0;
        public double MaxLon = 0;
        public int MinAlt = 0;
        public int MaxAlt = 0;

        // Station positions
        public double MyLat = 0;
        public double MyLon = 0;
        public double DXLat = 0;
        public double DXLon = 0;

        // Plane position history settings
        public bool KeepHistory = false;

        // Postion update interval
        public int Interval = 0;

        // Extended plausibility check
        public bool ExtendedPlausibilityCheck_Enable = true;
        public int ExtendedPlausiblityCheck_MaxErrorDist = 10;
        public bool LogErrors = false;

        public string InstanceID;
        public string SessionKey;
        public string GetKeyURL;

    }

}
