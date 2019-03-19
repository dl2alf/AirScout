using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace AirScout
{
    public partial class HistoryFromURLDlg : Form
    {

        public HistoryFromURLDlg()
        {
            InitializeComponent();
        }

        private void tb_History_Select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            Dlg.SelectedPath = Properties.Settings.Default.Analysis_History_Directory;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Analysis_History_Directory = Dlg.SelectedPath;
            }
        }
    }
}
