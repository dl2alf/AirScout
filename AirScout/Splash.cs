using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ScoutBase.Core;

namespace AirScout
{
    public partial class Splash : Form
    {
        Label status;

        // Define the CS_DROPSHADOW constant
        private const int CS_DROPSHADOW = 0x00020000;

        // Override the CreateParams property
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // change window style to dropshadow
                // does not work under Linux/Mono
                if (!SupportFunctions.IsMono)
                    cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public Splash()
        {
            InitializeComponent();
            // set border fram to NONE
            // does not work under Linux/Mono
            if (!SupportFunctions.IsMono)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.FixedSingle;
            status = new Label();
            status.Parent = pb_Main;
            status.BackColor = Color.Transparent;
            status.ForeColor = Color.White;
            status.Location = new Point(84, this.Height - 20);
            status.Height = 14;
            status.Width = 350;
            status.Font = new System.Drawing.Font(this.Font, FontStyle.Italic);
            status.TextAlign = ContentAlignment.MiddleCenter;
        }

        public void Status(string s)
        {
            Status(s, Color.White);
        }

        public void Status(string s, Color color)
        {
            status.ForeColor = color;
            status.Text = s;
            this.Refresh();
            Application.DoEvents();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
