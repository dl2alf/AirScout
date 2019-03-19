using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ScoutBase.Core;

namespace AirScout
{
    public partial class LicenseDlg : Form
    {

        private LogWriter Log = LogWriter.Instance;

        public LicenseDlg()
        {
            InitializeComponent();
            try
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\license"))
                {
                    rtb_License.Text = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.ToString());
            }
        }
    }
}
