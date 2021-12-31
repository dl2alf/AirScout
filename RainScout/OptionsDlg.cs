using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using ScoutBase.Core;
using ScoutBase.Elevation;

namespace RainScout
{
    public partial class OptionsDlg : Form
    {
        MainDlg ParentDlg;

        bool ChangeLatFromLoc;
        bool ChangeLonFromLoc;
        bool ChangeLocFromLatLon;

        public OptionsDlg(MainDlg parentdlg)
        {
            InitializeComponent();
            ParentDlg = parentdlg;
        }

        private void tp_Stations_Enter(object sender, EventArgs e)
        {
            // try to fill infos from properties
            tb_Options_Stations_Call.Text = Properties.Settings.Default.MyCall;
            tb_Options_Stations_Lat.Text = Properties.Settings.Default.MyLat.ToString("F8", CultureInfo.InvariantCulture);
            tb_Options_Stations_Lon.Text = Properties.Settings.Default.MyLon.ToString("F8", CultureInfo.InvariantCulture);
            tb_Options_Stations_Loc.Text = Properties.Settings.Default.MyLoc;
            tb_Options_Stations_Elevation.Text = Properties.Settings.Default.MyElevation.ToString("F0");
            tb_Options_Stations_Height.Text = Properties.Settings.Default.MyHeight.ToString("F0");

        }

        private void tb_Options_Stations_Call_TextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);
            /*
            CallsignEntry entry = ParentDlg.Locations.FindEntry(tb_Options_Stations_Call.Text);
            if (entry != null)
            {
                Properties.Settings.Default.MyLat = entry.Lat;
                Properties.Settings.Default.MyLon = entry.Lon;
                Properties.Settings.Default.MyLoc = entry.Loc;
                tb_Options_Stations_Lat.Text = entry.Lat.ToString("F8", CultureInfo.InvariantCulture);
                tb_Options_Stations_Lon.Text = entry.Lon.ToString("F8", CultureInfo.InvariantCulture);
                tb_Options_Stations_Loc.Text = entry.Loc;
                tb_Options_Stations_Elevation.Text = entry.Elevation.ToString();
                tb_Options_Stations_Height.Text = entry.Height.ToString();
                if (entry.IsPrecise)
                    tb_Options_Stations_Loc.BackColor = Color.LightGreen;
                else
                    tb_Options_Stations_Loc.BackColor = Color.White;
            }
            else
            {
                Properties.Settings.Default.MyLat = double.NaN;
                Properties.Settings.Default.MyLon = double.NaN;
                Properties.Settings.Default.MyLoc = "";
                Properties.Settings.Default.MyElevation = 0;
                Properties.Settings.Default.MyHeight = 0;
                tb_Options_Stations_Loc.BackColor = Color.White;
                tb_Options_Stations_Lat.Text = "";
                tb_Options_Stations_Lon.Text = "";
                tb_Options_Stations_Loc.Text = "";
                tb_Options_Stations_Elevation.Text = "";
                tb_Options_Stations_Height.Text = "";

            }
            */
        }

        private void tb_Options_Stations_Lat_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_Options_Stations_Lat.Text))
                return;
            try
            {
                double lat = System.Convert.ToDouble(tb_Options_Stations_Lat.Text, CultureInfo.InvariantCulture);
                double lon = System.Convert.ToDouble(tb_Options_Stations_Lon.Text, CultureInfo.InvariantCulture);
                tb_Options_Stations_Elevation.Text = ElevationData.Database[lat, lon, Properties.Settings.Default.CurrentElevationModel].ToString("F0");
                if (ChangeLatFromLoc)
                {
                    ChangeLatFromLoc = false;
                }
                else
                {
                    ChangeLocFromLatLon = true;
                    tb_Options_Stations_Loc.Text = MaidenheadLocator.LocFromLatLon(lat, lon,false, 3);
                }
            }
            catch
            {
            }
        }

        private void tb_Options_Stations_Lon_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_Options_Stations_Lon.Text))
                return;
            try
            {
                double lat = System.Convert.ToDouble(tb_Options_Stations_Lat.Text, CultureInfo.InvariantCulture);
                double lon = System.Convert.ToDouble(tb_Options_Stations_Lon.Text, CultureInfo.InvariantCulture);
                tb_Options_Stations_Elevation.Text = ElevationData.Database[lat, lon, Properties.Settings.Default.CurrentElevationModel].ToString("F0");
                if (ChangeLonFromLoc)
                {
                    ChangeLonFromLoc = false;
                }
                else
                {
                    ChangeLocFromLatLon = true;
                    tb_Options_Stations_Loc.Text = MaidenheadLocator.LocFromLatLon(lat, lon, false, 3);
                }
            }
            catch
            {
            }
        }

        private void tb_Options_Stations_Loc_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tb_Options_Stations_Loc.Text))
                return;
            if (!MaidenheadLocator.Check(tb_Options_Stations_Loc.Text))
                return;
            if (ChangeLocFromLatLon)
            {
                ChangeLocFromLatLon = false;
            }
            else
            {
                ChangeLatFromLoc = true;
                tb_Options_Stations_Lat.Text = MaidenheadLocator.LatFromLoc(tb_Options_Stations_Loc.Text).ToString("F8", CultureInfo.InvariantCulture);
                ChangeLonFromLoc = true;
                tb_Options_Stations_Lon.Text = MaidenheadLocator.LonFromLoc(tb_Options_Stations_Loc.Text).ToString("F8", CultureInfo.InvariantCulture);
            }
        }

        private void tb_Options_Stations_Height_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Options_Stations_Update_Click(object sender, EventArgs e)
        {
            // check all entries and update callsign database
            /*
            try
            {
                CallsignEntry entry = ParentDlg.Locations.FindEntry(tb_Options_Stations_Call.Text);
                if (entry != null)
                {
                    entry.Lat = System.Convert.ToDouble(tb_Options_Stations_Lat.Text, CultureInfo.InvariantCulture);
                    entry.Lon = System.Convert.ToDouble(tb_Options_Stations_Lon.Text, CultureInfo.InvariantCulture);
                    entry.Loc = tb_Options_Stations_Loc.Text;
                    entry.Elevation = ParentDlg.ElevationData[entry.Lat, entry.Lon];
                    entry.Height = System.Convert.ToDouble(tb_Options_Stations_Height.Text, CultureInfo.InvariantCulture);
                }
                else
                {
                    entry = new CallsignEntry();
                    entry.Call = tb_Options_Stations_Call.Text;
                    entry.Lat = System.Convert.ToDouble(tb_Options_Stations_Lat.Text, CultureInfo.InvariantCulture);
                    entry.Lon = System.Convert.ToDouble(tb_Options_Stations_Lon.Text, CultureInfo.InvariantCulture);
                    entry.Loc = tb_Options_Stations_Loc.Text;
                    entry.Elevation = ParentDlg.ElevationData[entry.Lat, entry.Lon];
                    entry.Height = System.Convert.ToDouble(tb_Options_Stations_Height.Text, CultureInfo.InvariantCulture);
                    ParentDlg.Locations.AddEntry(entry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while updating call sign database: " + ex.Message, "Error");
            }
            */
        }

        private void tc_Scatter_Enter(object sender, EventArgs e)
        {
            tb_Options_Scatter_MinHeight.Text = Properties.Settings.Default.Scatter_MinHeight.ToString("F0", CultureInfo.InvariantCulture);
            tb_Options_Scatter_MaxHeight.Text = Properties.Settings.Default.Scatter_MaxHeight.ToString("F0", CultureInfo.InvariantCulture);
            tb_Options_Scatter_MaxAngle.Text = Properties.Settings.Default.Scatter_MaxAngle.ToString("F0", CultureInfo.InvariantCulture);
        }

        private void tb_Options_Scatter_MinHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Scatter_MinHeight = System.Convert.ToDouble(tb_Options_Scatter_MinHeight.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Invalid value: " + tb_Options_Scatter_MinHeight.Text, "Error");
            }
        }

        private void tb_Options_Scatter_MaxHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Scatter_MaxHeight = System.Convert.ToDouble(tb_Options_Scatter_MaxHeight.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Invalid value: " + tb_Options_Scatter_MaxHeight.Text, "Error");
            }
        }

        private void tb_Options_Scatter_MaxAngle_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Scatter_MaxAngle = System.Convert.ToDouble(tb_Options_Scatter_MaxAngle.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Invalid value: " + tb_Options_Scatter_MaxAngle.Text, "Error");
            }
        }

    }
}
