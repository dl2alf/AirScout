
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Reflection;
using AirScout.Database.Core;
using AirScout.Database.Aircrafts;
using ScoutBase.Core;

namespace AirScoutPlaneServer
{

    public partial class MainDlg : Form
    {

        private void bw_JSONWriter_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bw_JSONWriter.CancellationPending)
            {
                Thread.Sleep(60000);
                DataTable dt = new DataTable();
                // DataTable dt = FlightRadar.GetPlanePositions((int)Properties.Settings.Default.Planes_Lifetime);
                // get planes each minute
                // write json file
                try
                {
                    using (StreamWriter sw = new StreamWriter(TmpDirectory + Path.DirectorySeparatorChar + "planes.json"))
                    {
                        int major = Assembly.GetExecutingAssembly().GetName().Version.Major;
                        sw.Write("{\"full_count\":" + dt.Rows.Count.ToString() + ",\"version\":" + major.ToString());
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string index = "\"" + i.ToString("x8") + "\"";
                            string hex = "\"" + dt.Rows[i]["Hex"].ToString() + "\"";
                            string lat = ((double)dt.Rows[i]["Lat"]).ToString("F4", CultureInfo.InvariantCulture);
                            string lon = ((double)dt.Rows[i]["Lon"]).ToString("F4", CultureInfo.InvariantCulture);
                            string track = dt.Rows[i]["Track"].ToString();
                            string alt = UnitConverter.m_ft((double)dt.Rows[i]["Alt"]).ToString("F0");
                            string speed = UnitConverter.kmh_kts((double)dt.Rows[i]["Speed"]).ToString("F0");
                            string squawk = "\"" + dt.Rows[i]["Squawk"].ToString() + "\"";
                            string radar = "\"" + dt.Rows[i]["Radar"].ToString() + "\"";
                            AircraftDesignator d = AircraftDatabase_old.AircraftFindByHex(dt.Rows[i]["Hex"].ToString());
                            string type;
                            if (d != null)
                                type = "\"" + d.TypeCode + "\"";
                            else
                                type = "\"" + "\"";
                            string reg = "\"" + dt.Rows[i]["Reg"].ToString() + "\"";
                            DateTime rtime = System.Convert.ToDateTime(dt.Rows[i]["Time"].ToString());
                            rtime = rtime.ToUniversalTime();
                            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            string time = ((long)(rtime - sTime).TotalSeconds).ToString();
                            string dep = "\"\"";
                            string dest = "\"\"";
                            string flight = "\"\"";
                            string dummy0 = "\"\"";
                            string dummy1 = "0";
                            string dummy2 = "0";
                            string call = "\"" + dt.Rows[i]["Call"].ToString() + "\"";
                            string dummy3 = "0";
                            sw.WriteLine("," + index + ":[" +
                                hex + "," +
                                lat + "," +
                                lon + "," +
                                track + "," +
                                alt + "," +
                                speed + "," +
                                squawk + "," +
                                radar + "," +
                                type + "," +
                                reg + "," +
                                time + "," +
                                dep + "," +
                                dest + "," +
                                flight + "," +
                                dummy1 + "," +
                                dummy2 + "," +
                                call + "," +
                                dummy3 +
                                "]");
                        }
                        sw.WriteLine("}");
                    }
                }
                catch
                {
                    // do nothing
                }
            }
        }
    }
}
