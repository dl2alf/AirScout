namespace AirScout
{
    partial class ElevationCopyrightDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElevationCopyrightDlg));
            this.rtb_Copyright = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtb_Copyright
            // 
            this.rtb_Copyright.BackColor = System.Drawing.Color.White;
            this.rtb_Copyright.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Copyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Copyright.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_Copyright.Location = new System.Drawing.Point(0, 0);
            this.rtb_Copyright.Name = "rtb_Copyright";
            this.rtb_Copyright.ReadOnly = true;
            this.rtb_Copyright.Size = new System.Drawing.Size(1077, 253);
            this.rtb_Copyright.TabIndex = 0;
            this.rtb_Copyright.Text = "";
            // 
            // ElevationCopyrightDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 253);
            this.Controls.Add(this.rtb_Copyright);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ElevationCopyrightDlg";
            this.Text = resources.GetString("$this.Text");
            this.Load += new System.EventHandler(this.ElevationCopyrightDlg_Load);
            this.Leave += new System.EventHandler(this.ElevationCopyrightDlg_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox rtb_Copyright;
    }
}