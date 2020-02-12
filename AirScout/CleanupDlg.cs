using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScoutBase.Stations;
using ScoutBase.Elevation;
using ScoutBase.Propagation;
using AirScout.Aircrafts;

namespace AirScout
{
    public partial class CleanupDlg : Form
    {
        MapDlg ParentDlg;

        bool Success = false;
        bool ButtonPressed = false;

        public CleanupDlg(MapDlg parentDlg)
        {
            ParentDlg = parentDlg;
            InitializeComponent();
            cb_LogFiles.Text = cb_LogFiles.Text + "  [" + ParentDlg.LogDirectory + "]";
            cb_ElevationFiles.Text = cb_ElevationFiles.Text + "  [" + ParentDlg.ElevationDirectory + "]";
            cb_TmpFiles.Text = cb_TmpFiles.Text + "  [" + ParentDlg.TmpDirectory + "]";
            this.DialogResult = DialogResult.Abort;
        }

        private void CleanupDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            // chek if user pressed a button or closed the form other way
            if (!ButtonPressed)
                DialogResult = DialogResult.Abort;
        }

        private void Cleanup()
        {
            // do cleanup here
            Success = true;
            if (cb_UserSettings.Checked)
            {
                // reset all properties
                Properties.Settings.Default.Reset();
                AirScout.PlaneFeeds.Properties.Settings.Default.Reset();
                AirScout.Aircrafts.Properties.Settings.Default.Reset();
                ScoutBase.Elevation.Properties.Settings.Default.Reset();
                ScoutBase.Stations.Properties.Settings.Default.Reset();
            }
            if (cb_AirScout_Database.Checked)
                DeleteDirectory(Properties.Settings.Default.AircraftDatabase_Directory);
            if (cb_ScoutBase_Database.Checked)
            {
                DeleteDirectory(Properties.Settings.Default.StationsDatabase_Directory);
                DeleteDirectory(Properties.Settings.Default.ElevationDatabase_Directory);
                DeleteDirectory(Properties.Settings.Default.PropagationDatabase_Directory);
            }
            if (cb_LogFiles.Checked)
                DeleteDirectory(ParentDlg.LogDirectory);
            if (cb_ElevationFiles.Checked)
                DeleteDirectory(ParentDlg.ElevationDirectory);
            if (cb_TmpFiles.Checked)
                DeleteDirectory(ParentDlg.TmpDirectory);
            if (cb_PluginFiles.Checked)
                DeleteDirectory(ParentDlg.PluginDirectory);
        }

        private void DeleteDirectory(string target_dir)
        {
            // deletes directory and all its subdirectory recursively
            try
            { 
                if (!Directory.Exists(target_dir))
                    return;
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, target_dir);
                Success = false;
            }

        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Cleanup();
            if (Success)
            {
                ButtonPressed = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                btn_OK.Text = "Retry";
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            ButtonPressed = true;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
