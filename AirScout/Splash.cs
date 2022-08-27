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
        Label version;
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
                /*
                if (!SupportFunctions.IsMono)
                    cp.ClassStyle |= CS_DROPSHADOW;
                */
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
            // initialize status label
            status = new Label();
            status.Parent = pb_Main;
            status.BackColor = Color.Transparent;
            status.ForeColor = Color.White;
            status.Location = new Point(84, this.Height - 20);
            status.Height = 14;
            status.Width = 350;
            status.Font = new System.Drawing.Font(this.Font, FontStyle.Italic);
            status.TextAlign = ContentAlignment.MiddleCenter;
            // set to full transparent view at first;
            Opacity = 0;
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
            // start close timer
            ti_Close.Start();
            // start animation timer
            ti_Animation.Start();
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            // stop timers
            ti_Animation.Stop();
            ti_Close.Stop();
        }

        private void ti_Close_Tick(object sender, EventArgs e)
        {
            // close form immediately if not closed by main form
            this.Close();
        }

        private void ti_Animation_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.01;
                ti_Animation.Start();
            }
            else
            {
                // rest topmost style
                this.TopMost = false;
            }
        }

        private void pb_Main_Paint(object sender, PaintEventArgs e)
        {
            // show Version
            string text = "Version " + Application.ProductVersion;
            using (Font myFont = new System.Drawing.Font(this.Font.FontFamily, 24, FontStyle.Bold | FontStyle.Italic))
            {
                e.Graphics.DrawString(text, myFont, Brushes.DimGray, new Point(140, 10));
                e.Graphics.DrawString(text, myFont, Brushes.White, new Point(140-2, 10-2));
            }
        }

        private void pb_Main_Click(object sender, EventArgs e)
        {
            // hide the splash window when clicking on it
            this.Visible = false;
        }
    }
}
