using ScoutBase.Stations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AirScout
{
    public partial class DeleteSingleStationDlg : Form
    {
        bool SelectionChanged = false;

        public DeleteSingleStationDlg()
        {
            InitializeComponent();
        }

        private void cb_Call_TextChanged(object sender, EventArgs e)
        {
            if (!SelectionChanged)
            {
                int start = cb_Call.SelectionStart;
                int len = cb_Call.SelectionLength;
                string text = cb_Call.Text.ToUpper().Trim();
                if (cb_Call.Text != text)
                    cb_Call.Text = text;
                cb_Call.SelectionStart = start;
                cb_Call.SelectionLength = len;
            }
            SelectionChanged = false;
        }

        private void cb_Call_SelectedIndexChanged(object sender, EventArgs e)
        {
            // suppress handling on text input
            if (!cb_Call.DroppedDown)
                return;
            if (cb_Call.SelectedItem != null)
            {
                SelectionChanged = true;
                cb_Call.Text = (string)cb_Call.SelectedItem;
            }
        }

        private void cb_Call_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void cb_Call_DropDown(object sender, EventArgs e)
        {
            if (!cb_Call.DroppedDown && (cb_Call.Text.Length >= 2))
            {
                List<LocationDesignator> l = StationData.Database.LocationGetAll("%" + cb_Call.Text + "%");
                if (l != null)
                {
                    cb_Call.Items.Clear();
                    cb_Locator.Items.Clear();
                    foreach (LocationDesignator ld in l)
                    {
                        cb_Call.Items.Add(ld.Call);
                        cb_Locator.Items.Add(ld.Loc);
                    }
                }
            }
        }
    }
}
