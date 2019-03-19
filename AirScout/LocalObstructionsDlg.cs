using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutBase.Elevation;

namespace AirScout
{
    public partial class LocalObstructionsDlg : Form
    {
        public DataTableLocalObstructions LocalObstructions;

        public LocalObstructionsDlg(LocalObstructionDesignator obstr)
        {
            InitializeComponent();
            LocalObstructions = new DataTableLocalObstructions(obstr);
            dgv_Main.DataSource = LocalObstructions;
        }

        private void btn_ClearAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in LocalObstructions.Rows)
            {
                row["Distance"] = 0;
                row["Height"] = 0;
            }
        }
    }
}
