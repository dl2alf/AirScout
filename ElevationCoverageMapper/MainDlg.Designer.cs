namespace ElevationCoverageMapper
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
            this.gm_Coverage = new GMap.NET.WindowsForms.GMapControl();
            this.bw_MapUpdater = new System.ComponentModel.BackgroundWorker();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.ss_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // gm_Coverage
            // 
            this.gm_Coverage.Bearing = 0F;
            this.gm_Coverage.CanDragMap = true;
            this.gm_Coverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Coverage.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Coverage.GrayScaleMode = false;
            this.gm_Coverage.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Coverage.LevelsKeepInMemmory = 5;
            this.gm_Coverage.Location = new System.Drawing.Point(0, 0);
            this.gm_Coverage.MarkersEnabled = true;
            this.gm_Coverage.MaxZoom = 2;
            this.gm_Coverage.MinZoom = 2;
            this.gm_Coverage.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Coverage.Name = "gm_Coverage";
            this.gm_Coverage.NegativeMode = false;
            this.gm_Coverage.Opacity = 1D;
            this.gm_Coverage.PolygonsEnabled = true;
            this.gm_Coverage.RetryLoadTile = 0;
            this.gm_Coverage.RoutesEnabled = true;
            this.gm_Coverage.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Coverage.ShowTileGridLines = false;
            this.gm_Coverage.Size = new System.Drawing.Size(1008, 729);
            this.gm_Coverage.TabIndex = 0;
            this.gm_Coverage.Zoom = 0D;
            this.gm_Coverage.OnTileLoadComplete += new GMap.NET.TileLoadComplete(this.gm_Coverage_OnTileLoadComplete);
            // 
            // bw_MapUpdater
            // 
            this.bw_MapUpdater.WorkerReportsProgress = true;
            this.bw_MapUpdater.WorkerSupportsCancellation = true;
            this.bw_MapUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_MapUpdater_DoWork);
            this.bw_MapUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_MapUpdater_ProgressChanged);
            this.bw_MapUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_MapUpdater_RunWorkerCompleted);
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 707);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(1008, 22);
            this.ss_Main.TabIndex = 1;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(39, 17);
            this.tsl_Status.Text = "Status";
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.ss_Main);
            this.Controls.Add(this.gm_Coverage);
            this.Name = "MainDlg";
            this.Text = "ElevationCoverageMapper";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.Load += new System.EventHandler(this.MainDlg_Load);
            this.Shown += new System.EventHandler(this.MainDlg_Shown);
            this.Resize += new System.EventHandler(this.MainDlg_Resize);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gm_Coverage;
        private System.ComponentModel.BackgroundWorker bw_MapUpdater;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
    }
}

