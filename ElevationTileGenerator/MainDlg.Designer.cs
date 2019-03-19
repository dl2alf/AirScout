namespace ElevationTileGenerator
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
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_Start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SourceDir = new System.Windows.Forms.Button();
            this.btn_DestDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.pb_Picture = new System.Windows.Forms.PictureBox();
            this.btn_Picture = new System.Windows.Forms.Button();
            this.btn_ZIP = new System.Windows.Forms.Button();
            this.btn_CAT = new System.Windows.Forms.Button();
            this.btn_TestCAT = new System.Windows.Forms.Button();
            this.btn_CATFromZIP = new System.Windows.Forms.Button();
            this.btn_PictureFromTiles = new System.Windows.Forms.Button();
            this.tb_DestDir = new System.Windows.Forms.TextBox();
            this.tb_SourceDir = new System.Windows.Forms.TextBox();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.ss_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Picture)).BeginInit();
            this.SuspendLayout();
            // 
            // ss_Main
            // 
            this.ss_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 534);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(1001, 22);
            this.ss_Main.TabIndex = 0;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(39, 17);
            this.tsl_Status.Text = "Status";
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(640, 478);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source Directory:";
            // 
            // btn_SourceDir
            // 
            this.btn_SourceDir.Location = new System.Drawing.Point(558, 40);
            this.btn_SourceDir.Name = "btn_SourceDir";
            this.btn_SourceDir.Size = new System.Drawing.Size(75, 23);
            this.btn_SourceDir.TabIndex = 4;
            this.btn_SourceDir.Text = "Select";
            this.btn_SourceDir.UseVisualStyleBackColor = true;
            this.btn_SourceDir.Click += new System.EventHandler(this.btn_SourceDir_Click);
            // 
            // btn_DestDir
            // 
            this.btn_DestDir.Location = new System.Drawing.Point(558, 66);
            this.btn_DestDir.Name = "btn_DestDir";
            this.btn_DestDir.Size = new System.Drawing.Size(75, 23);
            this.btn_DestDir.TabIndex = 7;
            this.btn_DestDir.Text = "Select";
            this.btn_DestDir.UseVisualStyleBackColor = true;
            this.btn_DestDir.Click += new System.EventHandler(this.btn_DestDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Destination Directory:";
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(721, 478);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(75, 23);
            this.btn_Stop.TabIndex = 8;
            this.btn_Stop.Text = "Stop";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // pb_Picture
            // 
            this.pb_Picture.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pb_Picture.Location = new System.Drawing.Point(36, 127);
            this.pb_Picture.Name = "pb_Picture";
            this.pb_Picture.Size = new System.Drawing.Size(586, 326);
            this.pb_Picture.TabIndex = 9;
            this.pb_Picture.TabStop = false;
            // 
            // btn_Picture
            // 
            this.btn_Picture.Location = new System.Drawing.Point(537, 478);
            this.btn_Picture.Name = "btn_Picture";
            this.btn_Picture.Size = new System.Drawing.Size(75, 23);
            this.btn_Picture.TabIndex = 10;
            this.btn_Picture.Text = "Picture";
            this.btn_Picture.UseVisualStyleBackColor = true;
            this.btn_Picture.Click += new System.EventHandler(this.btn_Picture_Click);
            // 
            // btn_ZIP
            // 
            this.btn_ZIP.Location = new System.Drawing.Point(802, 478);
            this.btn_ZIP.Name = "btn_ZIP";
            this.btn_ZIP.Size = new System.Drawing.Size(75, 23);
            this.btn_ZIP.TabIndex = 11;
            this.btn_ZIP.Text = "ZIP";
            this.btn_ZIP.UseVisualStyleBackColor = true;
            this.btn_ZIP.Click += new System.EventHandler(this.btn_ZIP_Click);
            // 
            // btn_CAT
            // 
            this.btn_CAT.Location = new System.Drawing.Point(883, 478);
            this.btn_CAT.Name = "btn_CAT";
            this.btn_CAT.Size = new System.Drawing.Size(75, 23);
            this.btn_CAT.TabIndex = 12;
            this.btn_CAT.Text = "CAT";
            this.btn_CAT.UseVisualStyleBackColor = true;
            this.btn_CAT.Click += new System.EventHandler(this.btn_CAT_Click);
            // 
            // btn_TestCAT
            // 
            this.btn_TestCAT.Location = new System.Drawing.Point(883, 390);
            this.btn_TestCAT.Name = "btn_TestCAT";
            this.btn_TestCAT.Size = new System.Drawing.Size(75, 23);
            this.btn_TestCAT.TabIndex = 13;
            this.btn_TestCAT.Text = "TestCAT";
            this.btn_TestCAT.UseVisualStyleBackColor = true;
            this.btn_TestCAT.Click += new System.EventHandler(this.btn_TestCAT_Click);
            // 
            // btn_CATFromZIP
            // 
            this.btn_CATFromZIP.Location = new System.Drawing.Point(883, 449);
            this.btn_CATFromZIP.Name = "btn_CATFromZIP";
            this.btn_CATFromZIP.Size = new System.Drawing.Size(75, 23);
            this.btn_CATFromZIP.TabIndex = 14;
            this.btn_CATFromZIP.Text = "CAT fm ZIP";
            this.btn_CATFromZIP.UseVisualStyleBackColor = true;
            this.btn_CATFromZIP.Click += new System.EventHandler(this.btn_CATFromZIP_Click);
            // 
            // btn_PictureFromTiles
            // 
            this.btn_PictureFromTiles.Location = new System.Drawing.Point(451, 478);
            this.btn_PictureFromTiles.Name = "btn_PictureFromTiles";
            this.btn_PictureFromTiles.Size = new System.Drawing.Size(75, 23);
            this.btn_PictureFromTiles.TabIndex = 15;
            this.btn_PictureFromTiles.Text = "Pic fm Tiles";
            this.btn_PictureFromTiles.UseVisualStyleBackColor = true;
            this.btn_PictureFromTiles.Click += new System.EventHandler(this.btn_PictureFromTiles_Click);
            // 
            // tb_DestDir
            // 
            this.tb_DestDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ElevationTileGenerator.Properties.Settings.Default, "DestDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_DestDir.Location = new System.Drawing.Point(156, 68);
            this.tb_DestDir.Name = "tb_DestDir";
            this.tb_DestDir.Size = new System.Drawing.Size(370, 20);
            this.tb_DestDir.TabIndex = 6;
            this.tb_DestDir.Text = global::ElevationTileGenerator.Properties.Settings.Default.DestDir;
            // 
            // tb_SourceDir
            // 
            this.tb_SourceDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ElevationTileGenerator.Properties.Settings.Default, "SourceDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_SourceDir.Location = new System.Drawing.Point(156, 42);
            this.tb_SourceDir.Name = "tb_SourceDir";
            this.tb_SourceDir.Size = new System.Drawing.Size(370, 20);
            this.tb_SourceDir.TabIndex = 3;
            this.tb_SourceDir.Text = global::ElevationTileGenerator.Properties.Settings.Default.SourceDir;
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(640, 449);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 23);
            this.btn_Delete.TabIndex = 16;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 556);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_PictureFromTiles);
            this.Controls.Add(this.btn_CATFromZIP);
            this.Controls.Add(this.btn_TestCAT);
            this.Controls.Add(this.btn_CAT);
            this.Controls.Add(this.btn_ZIP);
            this.Controls.Add(this.btn_Picture);
            this.Controls.Add(this.pb_Picture);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_DestDir);
            this.Controls.Add(this.tb_DestDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_SourceDir);
            this.Controls.Add(this.tb_SourceDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.ss_Main);
            this.Name = "MainDlg";
            this.Text = "ElevationTileGenerator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.Load += new System.EventHandler(this.MainDlg_Load);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_SourceDir;
        private System.Windows.Forms.Button btn_SourceDir;
        private System.Windows.Forms.Button btn_DestDir;
        private System.Windows.Forms.TextBox tb_DestDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.PictureBox pb_Picture;
        private System.Windows.Forms.Button btn_Picture;
        private System.Windows.Forms.Button btn_ZIP;
        private System.Windows.Forms.Button btn_CAT;
        private System.Windows.Forms.Button btn_TestCAT;
        private System.Windows.Forms.Button btn_CATFromZIP;
        private System.Windows.Forms.Button btn_PictureFromTiles;
        private System.Windows.Forms.Button btn_Delete;
    }
}

