using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ScoutBase.Core;
using ScoutBase.Elevation;
using ScoutBase.Stations;
using ScoutBase.Propagation;
using System.Diagnostics;

namespace HorizonGenerator
{
    public partial class MainDlg : Form
    {
        public MainDlg()
        {
            InitializeComponent();
        }

        private void Say(string text)
        {
            if (tsl_Status.Text != text)
            {
                tsl_Status.Text = text;
                ss_Main.Refresh();
            }
        }

        private void CalculateHorizons()
        {
            List<LocationDesignator> lds = StationData.Database.LocationGetAll();
            foreach (LocationDesignator ld in lds)
            {
                Stopwatch st = new Stopwatch();
                try
                {
                    //                short max = ElevationModel.Database.ElevationTilesMaxElevation(ELEVATIONMODEL.SRTM1, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
                    short max = 4797;
                    ELEVATIONMODEL model = ELEVATIONMODEL.SRTM1;
                    BAND band = BAND.B1_2G;
                    short antenna_height = 40;
                    short h1 = (short)(ElevationData.Database[ld.Lat, ld.Lon, model] + antenna_height);
                    double dist = 500.0;
                    double stepwidth = ElevationData.Database.GetDefaultStepWidth(model);
                    int count = (int)(dist / stepwidth / 1000.0);
                    HorizonPoint hp = new HorizonPoint();
                    st.Start();
                    List<HorizonPoint>[] l = new List<HorizonPoint>[360];
                    for (int j = 0; j < 360; j++)
                    {
                        Say("Calculation horizon of " + ld.Call + ": " + j + " of 360...");
                        l[j] = new List<HorizonPoint>();
                        ElevationPathDesignator path = new ElevationPathDesignator();
                        path = ElevationData.Database.ElevationPathCreateFromBearing(this, ld.Lat, ld.Lon, j, dist, stepwidth, model);
                        short[] elv = path.Path;
                        // iterate through frequencies
                        foreach (int f in Enum.GetValues(typeof(BAND)))
                        {
                            double f1_clearance = 0.6;
                            if (f <= 144)
                                f1_clearance = 0.2;
                            else if (f <= 432)
                                f1_clearance = 0.4;
                            hp = PropagationHorizonDesignator.EpsilonMaxFromPath(h1, ref elv, dist, stepwidth, f / 1000.0, f1_clearance, max, LatLon.Earth.Radius * 1.33);
                            l[j].Add(hp);
                        }
                        hp = l[j].ElementAt(0);
                        Application.DoEvents();
                    }
                    st.Stop();
                    using (StreamWriter sw = new StreamWriter("Horizon.csv"))
                    {
                        sw.WriteLine("bearing[deg];eps_50M[deg];eps_70M[deg];eps[144M[deg];eps_432M[deg];eps_1.2G[deg];eps_2.3G[deg];eps_3.4G[deg];eps_5.7G[deg];eps_10G[deg];eps_24G[deg];eps_47G[deg];eps_76G[deg]");
                        for (int j = 0; j < l.Length; j++)
                        {
                            sw.Write(j + ";");
                            for (int k = 0; k < l[0].Count; k++)
                            {
                                sw.Write((l[j].ElementAt(k).Epsmin / Math.PI * 180.0).ToString("F8"));
                                if (k < l[0].Count - 1)
                                    sw.Write(";");
                            }

                            sw.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            CalculateHorizons();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
