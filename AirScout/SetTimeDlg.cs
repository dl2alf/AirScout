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
    public partial class SetTimeDlg : Form
    {
        public SetTimeDlg()
        {
            InitializeComponent();
            dtp_SetTimeDlg_Start.Value = DateTime.UtcNow;
        }

        private void cb_Time_Online_CheckedChanged(object sender, EventArgs e)
        {
            dtp_SetTimeDlg_Start.Enabled = !cb_Time_Online.Checked;
        }

        private void SetTimeDlg_Load(object sender, EventArgs e)
        {
        }

        private void SetTimeDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
