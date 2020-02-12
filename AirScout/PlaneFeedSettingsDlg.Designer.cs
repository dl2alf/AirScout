namespace AirScout
{
    partial class PlaneFeedSettingsDlg
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
            this.gb_Info = new System.Windows.Forms.GroupBox();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.gb_Properties = new System.Windows.Forms.GroupBox();
            this.pg_Main = new System.Windows.Forms.PropertyGrid();
            this.gb_Version = new System.Windows.Forms.GroupBox();
            this.lbl_Version = new System.Windows.Forms.Label();
            this.gb_Info.SuspendLayout();
            this.gb_Properties.SuspendLayout();
            this.gb_Version.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_Info
            // 
            this.gb_Info.Controls.Add(this.lbl_Info);
            this.gb_Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Info.Location = new System.Drawing.Point(431, 9);
            this.gb_Info.Name = "gb_Info";
            this.gb_Info.Size = new System.Drawing.Size(341, 350);
            this.gb_Info.TabIndex = 3;
            this.gb_Info.TabStop = false;
            this.gb_Info.Text = "Info";
            // 
            // lbl_Info
            // 
            this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Info.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Info.Location = new System.Drawing.Point(3, 16);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(335, 331);
            this.lbl_Info.TabIndex = 2;
            this.lbl_Info.Text = "PlaneFeed Info";
            // 
            // gb_Properties
            // 
            this.gb_Properties.Controls.Add(this.pg_Main);
            this.gb_Properties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Properties.Location = new System.Drawing.Point(12, 9);
            this.gb_Properties.Name = "gb_Properties";
            this.gb_Properties.Size = new System.Drawing.Size(413, 420);
            this.gb_Properties.TabIndex = 4;
            this.gb_Properties.TabStop = false;
            this.gb_Properties.Text = "Settings";
            // 
            // pg_Main
            // 
            this.pg_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pg_Main.Location = new System.Drawing.Point(3, 16);
            this.pg_Main.Name = "pg_Main";
            this.pg_Main.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pg_Main.Size = new System.Drawing.Size(407, 401);
            this.pg_Main.TabIndex = 1;
            // 
            // gb_Version
            // 
            this.gb_Version.Controls.Add(this.lbl_Version);
            this.gb_Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Version.Location = new System.Drawing.Point(431, 365);
            this.gb_Version.Name = "gb_Version";
            this.gb_Version.Size = new System.Drawing.Size(341, 61);
            this.gb_Version.TabIndex = 6;
            this.gb_Version.TabStop = false;
            this.gb_Version.Text = "Version";
            // 
            // lbl_Version
            // 
            this.lbl_Version.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Version.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Version.Location = new System.Drawing.Point(3, 16);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new System.Drawing.Size(335, 42);
            this.lbl_Version.TabIndex = 3;
            this.lbl_Version.Text = "PlaneFeed Version";
            // 
            // PlaneFeedSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.gb_Version);
            this.Controls.Add(this.gb_Properties);
            this.Controls.Add(this.gb_Info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PlaneFeedSettingsDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PlaneFeedSettingsDlg";
            this.gb_Info.ResumeLayout(false);
            this.gb_Properties.ResumeLayout(false);
            this.gb_Version.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Info;
        public System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.GroupBox gb_Properties;
        public System.Windows.Forms.PropertyGrid pg_Main;
        private System.Windows.Forms.GroupBox gb_Version;
        public System.Windows.Forms.Label lbl_Version;
    }
}