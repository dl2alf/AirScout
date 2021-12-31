using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutBase.CAT;

namespace CATCheck
{
    public partial class FrequencyOfToneDlg : Form
    {
        Rig Rig = null;

        public FrequencyOfToneDlg(Rig rig)
        {
            Rig = rig;

            InitializeComponent();

            if ((Rig != null) && Rig.Online)
            {
                ud_Tone.Value = Rig.Pitch;
            }
        }

        private void ud_Tone_ValueChanged(object sender, EventArgs e)
        {
            if ((Rig != null) && Rig.Online)
            {
                tb_Freq.Text = Rig.FrequencyOfTone((int)ud_Tone.Value).ToString();
            }
            else
            {
                tb_Freq.Text = "Rig not avalibale";
            }
        }
    }
}
