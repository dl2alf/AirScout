using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AirScout.Aircrafts;
using ScoutBase.Core;
using System.Globalization;
using AirScout.AircraftPositions;

namespace AirScout
{
    public partial class PlaneHistoryDlg : Form
    {
        DateTime From;
        DateTime To;
        string TmpDirectory;

        List<string> Hexs;
        List<string> Calls;
        List<AircraftPositionDesignator> AircraftPositions;

        PlaneInfoConverter c = new PlaneInfoConverter();

        public PlaneHistoryDlg(DateTime from, DateTime to, string tmpdir)
        {
            InitializeComponent();
            From = from;
            To = to;
            TmpDirectory = tmpdir;
        }

        private void Say (string msg)
        {
            if (tsl_Status.Text != msg)
            {
                tsl_Status.Text = msg;
                ss_Main.Refresh();
            }
        }

        private void PlaneHistoryDlg_Load(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            cb_Hex.Enabled = false;
            cb_Call.Enabled = false;
            btn_Export.Enabled = false;
            bw_GetHexAndCalls.RunWorkerAsync();
        }

        private void cb_Hex_TextChanged(object sender, EventArgs e)
        {
            cb_Call.SelectedIndex = -1;
        }

        private void cb_Call_TextChanged(object sender, EventArgs e)
        {
            cb_Hex.SelectedIndex = -1;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            Say("Getting positions...");
            CultureInfo uiculture = CultureInfo.CurrentUICulture;
            if (!String.IsNullOrEmpty(cb_Hex.Text))
            {
                AircraftPositions = AircraftPositionData.Database.AircraftPositionGetAllByHex(cb_Hex.Text, From, To);
                if (AircraftPositions == null)
                    return;
                // export by hex and save it with current UI culture
                SaveFileDialog Dlg = new SaveFileDialog();
                Dlg.AddExtension = true;
                Dlg.DefaultExt = "csv";
                Dlg.FileName = "Plane History_" + cb_Hex.Text + "_" + From.ToString("yyyyMMdd_HHmmss") + "_to_" + To.ToString("yyyyMMdd_HHmmss");
                Dlg.InitialDirectory = TmpDirectory;
                Dlg.OverwritePrompt = true;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(Dlg.FileName))
                    {
                        sw.WriteLine("time;hex;call;lat[deg];lon[deg];alt[m];track[deg];speed[kmh];dist[km];av_speed[km/h]");
                        for (int i = 0; i < AircraftPositions.Count; i++)
                        {
                            double dist = 0;
                            double av_speed = 0;
                            if (i > 0)
                            {
                                dist = LatLon.Distance(AircraftPositions[i - 1].Lat, AircraftPositions[i - 1].Lon, AircraftPositions[i].Lat, AircraftPositions[i].Lon);
                                av_speed = dist/(AircraftPositions[i].LastUpdated - AircraftPositions[i - 1].LastUpdated).TotalHours;
                            }
                            sw.WriteLine(AircraftPositions[i].LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") + ";" +
                                AircraftPositions[i].Hex + ";" +
                                AircraftPositions[i].Call + ";" +
                                AircraftPositions[i].Lat.ToString("F8", uiculture) + ";" +
                                AircraftPositions[i].Lon.ToString("F8", uiculture) + ";" +
                                UnitConverter.ft_m(AircraftPositions[i].Alt).ToString("F8", uiculture) + ";" +
                                AircraftPositions[i].Track.ToString("F8", uiculture) + ";" +
                                UnitConverter.kts_kmh(AircraftPositions[i].Speed).ToString("F8", uiculture) + ";" +
                                dist.ToString("F8", uiculture) + ";" +
                                av_speed.ToString("F8", uiculture)
                                );
                        }
                    }
                }
            }
            else if (!String.IsNullOrEmpty(cb_Call.Text))
            {
                // export by call
                AircraftPositions = AircraftPositionData.Database.AircraftPositionGetAllByCall(cb_Call.Text, From, To);
                if (AircraftPositions == null)
                    return;
                // export by hex
                SaveFileDialog Dlg = new SaveFileDialog();
                Dlg.AddExtension = true;
                Dlg.DefaultExt = "csv";
                Dlg.FileName = "Plane History_" + cb_Call.Text + "_" + From.ToString("yyyyMMdd_HHmmss") + "_to_" + To.ToString("yyyyMMdd_HHmmss");
                Dlg.InitialDirectory = TmpDirectory;
                Dlg.OverwritePrompt = true;
                if (Dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(Dlg.FileName))
                    {
                        sw.WriteLine("time;hex;call;lat[deg];lon[deg];alt[m];track[deg];speed[kmh];dist[km];av_speed[km/h]");
                        for (int i = 0; i < AircraftPositions.Count; i++)
                        {
                            double dist = 0;
                            double av_speed = 0;
                            if (i > 0)
                            {
                                dist = LatLon.Distance(AircraftPositions[i - 1].Lat, AircraftPositions[i - 1].Lon, AircraftPositions[i].Lat, AircraftPositions[i].Lon);
                                av_speed = dist / (AircraftPositions[i].LastUpdated - AircraftPositions[i - 1].LastUpdated).TotalHours;
                            }
                            sw.WriteLine(AircraftPositions[i].LastUpdated.ToString("yyyy-MM-dd HH:mm:ss") + ";" +
                                AircraftPositions[i].Hex + ";" +
                                AircraftPositions[i].Call + ";" +
                                AircraftPositions[i].Lat.ToString("F8", uiculture) + ";" +
                                AircraftPositions[i].Lon.ToString("F8", uiculture) + ";" +
                                UnitConverter.ft_m(AircraftPositions[i].Alt).ToString("F8", uiculture) + ";" +
                                AircraftPositions[i].Track.ToString("F8", uiculture) + ";" +
                                UnitConverter.kts_kmh(AircraftPositions[i].Speed).ToString("F8", uiculture) + ";" +
                                dist.ToString("F8", uiculture) + ";" +
                                av_speed.ToString("F8", uiculture)
                                );
                        }
                    }
                }
            }
            Say("Ready.");
        }

        private void bw_GetHexAndCalls_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_GetHexAndCalls.ReportProgress(0, ("Collecting data, please wait..."));
            List<string> hexs = AircraftPositionData.Database.AircraftPositionGetAllHex(From, To);
            if (hexs != null)
                bw_GetHexAndCalls.ReportProgress(1, hexs);
            List<string> calls = AircraftPositionData.Database.AircraftPositionGetAllCalls(From, To);
            if (calls != null)
                bw_GetHexAndCalls.ReportProgress(2, calls);
        }

        private void bw_GetHexAndCalls_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 0)
            {
                Say((string)e.UserState);
            }
            else if (e.ProgressPercentage == 1)
            {
                // hexs completed
                Hexs = (List<string>)e.UserState;
                if (Hexs != null)
                {
                    cb_Hex.BeginUpdate();
                    foreach (string hex in Hexs)
                    {
                        string s = c.To_Hex(hex);
                        if (!String.IsNullOrEmpty(s))
                            cb_Hex.Items.Add(s);
                    }
                    cb_Hex.EndUpdate();
                }
            }
            else if (e.ProgressPercentage == 2)
            {
                // calls compeleted
                Calls = (List<string>)e.UserState;
                if (Calls != null)
                {
                    cb_Call.BeginUpdate();
                    foreach (string call in Calls)
                    {
                        string s = c.To_Call(call);
                        if (!String.IsNullOrEmpty(s))
                            cb_Call.Items.Add(s);
                    }
                    cb_Call.EndUpdate();
                }
            }
        }

        private void bw_GetHexAndCalls_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (bw_GetHexAndCalls.CancellationPending)
            {
                Say("Cancelled.");
            }
            else
            {
                Say("Ready.");
                cb_Hex.Enabled = true;
                cb_Call.Enabled = true;
                btn_Export.Enabled = true;
            }
        }

        private void PlaneHistoryDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            bw_GetHexAndCalls.CancelAsync();
        }
    }
}
