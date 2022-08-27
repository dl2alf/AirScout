using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AirScout.PlaneFeeds.Plugin;
using AirScout.PlaneFeeds.Plugin.OpenSky;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            OpenSkyPlugin test = new OpenSkyPlugin();
            var args = new PlaneFeedPluginArgs();
            args.MinLat = 35;
            args.MaxLat = 60;
            args.MinLon = 0;
            args.MaxLon = 20;
            var planes = test.GetPlanes(args);
        }
    }
}
