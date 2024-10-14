using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirScout.PlaneFeeds.Plugin.MEFContract;

namespace AirScout
{
    public class WebserverStartArgs
    {
        public string TmpDirectory = "";
        public string WebserverDirectory = "";
        public List<IPlaneFeedPlugin> PlaneFeedPlugins = new List<IPlaneFeedPlugin>();
    }
}
