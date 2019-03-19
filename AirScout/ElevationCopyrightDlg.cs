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
    public partial class ElevationCopyrightDlg : Form
    {
        public ElevationCopyrightDlg()
        {
            InitializeComponent();
        }

        private void ElevationCopyrightDlg_Leave(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ElevationCopyrightDlg_Load(object sender, EventArgs e)
        {
            rtb_Copyright.SelectAll();
            rtb_Copyright.SelectionAlignment = HorizontalAlignment.Center;
            rtb_Copyright.DeselectAll();
        }
    }
}
