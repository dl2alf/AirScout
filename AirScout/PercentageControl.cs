using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AirScout
{
    public partial class PercentageControl : NumericUpDown
    {
        public PercentageControl()
        {
            InitializeComponent();
        }

        protected override void UpdateEditText()
        {
            this.Text = Value.ToString("0%");
            // base.UpdateEditText();
        }
    }
}
