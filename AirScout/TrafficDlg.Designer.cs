namespace AirScout
{
    partial class TrafficDlg
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gm_Options_Traffic = new GMap.NET.WindowsForms.GMapControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Options_Traffic_Show = new System.Windows.Forms.Button();
            this.dtp_Options_Traffic_Stop = new System.Windows.Forms.DateTimePicker();
            this.dtp_Options_Traffic_Start = new System.Windows.Forms.DateTimePicker();
            this.btn_Options_Traffic_Close = new System.Windows.Forms.Button();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.bw_Calculate = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ud_Opacity = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ss_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Opacity)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gm_Options_Traffic);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(808, 482);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Traftic Map";
            // 
            // gm_Options_Traffic
            // 
            this.gm_Options_Traffic.Bearing = 0F;
            this.gm_Options_Traffic.CanDragMap = true;
            this.gm_Options_Traffic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Options_Traffic.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Options_Traffic.GrayScaleMode = false;
            this.gm_Options_Traffic.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Options_Traffic.LevelsKeepInMemmory = 5;
            this.gm_Options_Traffic.Location = new System.Drawing.Point(3, 16);
            this.gm_Options_Traffic.MarkersEnabled = true;
            this.gm_Options_Traffic.MaxZoom = 2;
            this.gm_Options_Traffic.MinZoom = 2;
            this.gm_Options_Traffic.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Options_Traffic.Name = "gm_Options_Traffic";
            this.gm_Options_Traffic.NegativeMode = false;
            this.gm_Options_Traffic.Opacity = 1D;
            this.gm_Options_Traffic.PolygonsEnabled = true;
            this.gm_Options_Traffic.RetryLoadTile = 0;
            this.gm_Options_Traffic.RoutesEnabled = true;
            this.gm_Options_Traffic.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Options_Traffic.ShowTileGridLines = false;
            this.gm_Options_Traffic.Size = new System.Drawing.Size(802, 463);
            this.gm_Options_Traffic.TabIndex = 0;
            this.gm_Options_Traffic.Zoom = 0D;
            this.gm_Options_Traffic.OnRouteEnter += new GMap.NET.WindowsForms.RouteEnter(this.gm_Options_Traffic_OnRouteEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ud_Opacity);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btn_Options_Traffic_Show);
            this.groupBox2.Controls.Add(this.dtp_Options_Traffic_Stop);
            this.groupBox2.Controls.Add(this.dtp_Options_Traffic_Start);
            this.groupBox2.Controls.Add(this.btn_Options_Traffic_Close);
            this.groupBox2.Location = new System.Drawing.Point(12, 500);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(808, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Traffic Data";
            // 
            // btn_Options_Traffic_Show
            // 
            this.btn_Options_Traffic_Show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Options_Traffic_Show.Location = new System.Drawing.Point(630, 19);
            this.btn_Options_Traffic_Show.Name = "btn_Options_Traffic_Show";
            this.btn_Options_Traffic_Show.Size = new System.Drawing.Size(75, 23);
            this.btn_Options_Traffic_Show.TabIndex = 3;
            this.btn_Options_Traffic_Show.Text = "Show";
            this.btn_Options_Traffic_Show.UseVisualStyleBackColor = true;
            this.btn_Options_Traffic_Show.Click += new System.EventHandler(this.btn_Options_Traffic_Show_Click);
            // 
            // dtp_Options_Traffic_Stop
            // 
            this.dtp_Options_Traffic_Stop.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_Options_Traffic_Stop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Options_Traffic_Stop.Location = new System.Drawing.Point(258, 18);
            this.dtp_Options_Traffic_Stop.Name = "dtp_Options_Traffic_Stop";
            this.dtp_Options_Traffic_Stop.Size = new System.Drawing.Size(131, 20);
            this.dtp_Options_Traffic_Stop.TabIndex = 2;
            // 
            // dtp_Options_Traffic_Start
            // 
            this.dtp_Options_Traffic_Start.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_Options_Traffic_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Options_Traffic_Start.Location = new System.Drawing.Point(55, 18);
            this.dtp_Options_Traffic_Start.Name = "dtp_Options_Traffic_Start";
            this.dtp_Options_Traffic_Start.Size = new System.Drawing.Size(128, 20);
            this.dtp_Options_Traffic_Start.TabIndex = 1;
            // 
            // btn_Options_Traffic_Close
            // 
            this.btn_Options_Traffic_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Options_Traffic_Close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Options_Traffic_Close.Location = new System.Drawing.Point(711, 19);
            this.btn_Options_Traffic_Close.Name = "btn_Options_Traffic_Close";
            this.btn_Options_Traffic_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Options_Traffic_Close.TabIndex = 0;
            this.btn_Options_Traffic_Close.Text = "Close";
            this.btn_Options_Traffic_Close.UseVisualStyleBackColor = true;
            this.btn_Options_Traffic_Close.Click += new System.EventHandler(this.btn_Options_Traffic_Close_Click);
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Main});
            this.ss_Main.Location = new System.Drawing.Point(0, 547);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(832, 22);
            this.ss_Main.TabIndex = 2;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Main
            // 
            this.tsl_Main.Name = "tsl_Main";
            this.tsl_Main.Size = new System.Drawing.Size(170, 17);
            this.tsl_Main.Text = "Press Show to start calculation.";
            // 
            // bw_Calculate
            // 
            this.bw_Calculate.WorkerReportsProgress = true;
            this.bw_Calculate.WorkerSupportsCancellation = true;
            this.bw_Calculate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Calculate_DoWork);
            this.bw_Calculate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Calculate_ProgressChanged);
            this.bw_Calculate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Calculate_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(415, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Opacity[%]:";
            // 
            // ud_Opacity
            // 
            this.ud_Opacity.Location = new System.Drawing.Point(481, 18);
            this.ud_Opacity.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ud_Opacity.Name = "ud_Opacity";
            this.ud_Opacity.Size = new System.Drawing.Size(53, 20);
            this.ud_Opacity.TabIndex = 7;
            this.ud_Opacity.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // TrafficDlg
            // 
            this.AcceptButton = this.btn_Options_Traffic_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 569);
            this.Controls.Add(this.ss_Main);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TrafficDlg";
            this.Text = "Show All Traffic";
            this.SizeChanged += new System.EventHandler(this.TrafficDlg_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Opacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private GMap.NET.WindowsForms.GMapControl gm_Options_Traffic;
        private System.Windows.Forms.Button btn_Options_Traffic_Close;
        private System.Windows.Forms.DateTimePicker dtp_Options_Traffic_Stop;
        private System.Windows.Forms.DateTimePicker dtp_Options_Traffic_Start;
        private System.Windows.Forms.Button btn_Options_Traffic_Show;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Main;
        private System.ComponentModel.BackgroundWorker bw_Calculate;
        private System.Windows.Forms.NumericUpDown ud_Opacity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}