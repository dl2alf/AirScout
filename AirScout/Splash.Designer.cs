namespace AirScout
{
    partial class Splash
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.pb_Main = new System.Windows.Forms.PictureBox();
            this.ti_Close = new System.Windows.Forms.Timer(this.components);
            this.ti_Animation = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_Main
            // 
            this.pb_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Main.Image = global::AirScout.Properties.Resources.Intro;
            this.pb_Main.Location = new System.Drawing.Point(0, 0);
            this.pb_Main.Name = "pb_Main";
            this.pb_Main.Size = new System.Drawing.Size(522, 394);
            this.pb_Main.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Main.TabIndex = 0;
            this.pb_Main.TabStop = false;
            this.pb_Main.Click += new System.EventHandler(this.pb_Main_Click);
            this.pb_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_Main_Paint);
            // 
            // ti_Close
            // 
            this.ti_Close.Enabled = true;
            this.ti_Close.Interval = 60000;
            this.ti_Close.Tick += new System.EventHandler(this.ti_Close_Tick);
            // 
            // ti_Animation
            // 
            this.ti_Animation.Enabled = true;
            this.ti_Animation.Interval = 20;
            this.ti_Animation.Tick += new System.EventHandler(this.ti_Animation_Tick);
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 394);
            this.ControlBox = false;
            this.Controls.Add(this.pb_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Splash";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AirScout";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Splash_FormClosing);
            this.Load += new System.EventHandler(this.Splash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_Main;
        private System.Windows.Forms.Timer ti_Close;
        private System.Windows.Forms.Timer ti_Animation;
    }
}