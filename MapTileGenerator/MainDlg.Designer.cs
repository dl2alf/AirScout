namespace MapTileGenerator
{
    partial class MainDlg
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
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.tb_Database = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_MapTilesRootPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_MapTilesRootPath = new System.Windows.Forms.TextBox();
            this.btn_Database = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Start = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pb_Zoom = new System.Windows.Forms.PictureBox();
            this.btn_CreatePNG = new System.Windows.Forms.Button();
            this.ss_Main.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Zoom)).BeginInit();
            this.SuspendLayout();
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Main});
            this.ss_Main.Location = new System.Drawing.Point(0, 523);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(666, 22);
            this.ss_Main.TabIndex = 0;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Main
            // 
            this.tsl_Main.Name = "tsl_Main";
            this.tsl_Main.Size = new System.Drawing.Size(39, 17);
            this.tsl_Main.Text = "Status";
            // 
            // tb_Database
            // 
            this.tb_Database.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MapTileGenerator.Properties.Settings.Default, "Database", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Database.Location = new System.Drawing.Point(138, 32);
            this.tb_Database.Name = "tb_Database";
            this.tb_Database.Size = new System.Drawing.Size(345, 20);
            this.tb_Database.TabIndex = 1;
            this.tb_Database.Text = global::MapTileGenerator.Properties.Settings.Default.Database;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Start);
            this.groupBox1.Controls.Add(this.btn_MapTilesRootPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_MapTilesRootPath);
            this.groupBox1.Controls.Add(this.btn_Database);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_Database);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 156);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mp Tiles from Database";
            // 
            // btn_MapTilesRootPath
            // 
            this.btn_MapTilesRootPath.Location = new System.Drawing.Point(505, 56);
            this.btn_MapTilesRootPath.Name = "btn_MapTilesRootPath";
            this.btn_MapTilesRootPath.Size = new System.Drawing.Size(75, 23);
            this.btn_MapTilesRootPath.TabIndex = 6;
            this.btn_MapTilesRootPath.Text = "Select";
            this.btn_MapTilesRootPath.UseVisualStyleBackColor = true;
            this.btn_MapTilesRootPath.Click += new System.EventHandler(this.btn_MapTilesRootPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Map Tiles Root Path:";
            // 
            // tb_MapTilesRootPath
            // 
            this.tb_MapTilesRootPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MapTileGenerator.Properties.Settings.Default, "MapTilesRootPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_MapTilesRootPath.Location = new System.Drawing.Point(138, 58);
            this.tb_MapTilesRootPath.Name = "tb_MapTilesRootPath";
            this.tb_MapTilesRootPath.Size = new System.Drawing.Size(345, 20);
            this.tb_MapTilesRootPath.TabIndex = 4;
            this.tb_MapTilesRootPath.Text = global::MapTileGenerator.Properties.Settings.Default.MapTilesRootPath;
            // 
            // btn_Database
            // 
            this.btn_Database.Location = new System.Drawing.Point(505, 30);
            this.btn_Database.Name = "btn_Database";
            this.btn_Database.Size = new System.Drawing.Size(75, 23);
            this.btn_Database.TabIndex = 3;
            this.btn_Database.Text = "Select";
            this.btn_Database.UseVisualStyleBackColor = true;
            this.btn_Database.Click += new System.EventHandler(this.btn_Database_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Database:";
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(505, 111);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 7;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_CreatePNG);
            this.groupBox2.Controls.Add(this.pb_Zoom);
            this.groupBox2.Location = new System.Drawing.Point(12, 174);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(610, 330);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create combined image from png";
            // 
            // pb_Zoom
            // 
            this.pb_Zoom.Location = new System.Drawing.Point(6, 19);
            this.pb_Zoom.Name = "pb_Zoom";
            this.pb_Zoom.Size = new System.Drawing.Size(477, 293);
            this.pb_Zoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Zoom.TabIndex = 0;
            this.pb_Zoom.TabStop = false;
            // 
            // btn_CreatePNG
            // 
            this.btn_CreatePNG.Location = new System.Drawing.Point(505, 19);
            this.btn_CreatePNG.Name = "btn_CreatePNG";
            this.btn_CreatePNG.Size = new System.Drawing.Size(75, 23);
            this.btn_CreatePNG.TabIndex = 8;
            this.btn_CreatePNG.Text = "Create PNG";
            this.btn_CreatePNG.UseVisualStyleBackColor = true;
            this.btn_CreatePNG.Click += new System.EventHandler(this.btn_CreatePNG_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 545);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ss_Main);
            this.Name = "MainDlg";
            this.Text = "MapTileGenerator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Zoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Main;
        private System.Windows.Forms.TextBox tb_Database;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Database;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_MapTilesRootPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_MapTilesRootPath;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_CreatePNG;
        private System.Windows.Forms.PictureBox pb_Zoom;
    }
}

