namespace AirScoutPlaneServer
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
            this.components = new System.ComponentModel.Container();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Spring = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_CPULoad = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_MemoryAvailable = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_DBSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Uptime = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_Close = new System.Windows.Forms.Button();
            this.ni_Main = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms_NotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsi_Restore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsi_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.bw_Webserver = new System.ComponentModel.BackgroundWorker();
            this.ti_Main = new System.Windows.Forms.Timer(this.components);
            this.bw_ADSBFeed = new AirScout.PlaneFeeds.PlaneFeed_ADSB();
            this.bw_JSONWriter = new System.ComponentModel.BackgroundWorker();
            this.bw_DatabaseUpdater = new System.ComponentModel.BackgroundWorker();
            this.tp_ADSBFeed = new System.Windows.Forms.TabPage();
            this.tp_WebFeed1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pg_WebFeed1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_WebFeed1 = new System.Windows.Forms.ComboBox();
            this.btn_WebFeed1_Export = new System.Windows.Forms.Button();
            this.btn_WebFeed1_Import = new System.Windows.Forms.Button();
            this.tp_Planes = new System.Windows.Forms.TabPage();
            this.gb_CoveredArea = new System.Windows.Forms.GroupBox();
            this.ud_Planes_Lifetime = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.ud_Planes_MaxAlt = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.ud_Planes_MinAlt = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.gm_Planes_Coverage = new GMap.NET.WindowsForms.GMapControl();
            this.ud_MinLat = new System.Windows.Forms.NumericUpDown();
            this.ud_MaxLat = new System.Windows.Forms.NumericUpDown();
            this.ud_MinLon = new System.Windows.Forms.NumericUpDown();
            this.ud_MaxLon = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tp_Server = new System.Windows.Forms.TabPage();
            this.gb_Network_Server = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ud_Server_Port = new System.Windows.Forms.NumericUpDown();
            this.cb_Server_Active = new System.Windows.Forms.CheckBox();
            this.tp_Database = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_Database_PlanePositions_RowCount = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_Database_Filesize = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_Database_Location = new System.Windows.Forms.TextBox();
            this.tp_General = new System.Windows.Forms.TabPage();
            this.gb_LogWriter = new System.Windows.Forms.GroupBox();
            this.btn_Log_Show = new System.Windows.Forms.Button();
            this.ud_Log_Verbosity = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Log_Directory = new System.Windows.Forms.Button();
            this.cb_Log_Active = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Log_Directory = new System.Windows.Forms.TextBox();
            this.gb_General_Windows = new System.Windows.Forms.GroupBox();
            this.cb_Windows_Startup_Autorun = new System.Windows.Forms.CheckBox();
            this.cb_Windows_Startup_Systray = new System.Windows.Forms.CheckBox();
            this.cb_Windows_Startup_Minimized = new System.Windows.Forms.CheckBox();
            this.tc_Main = new System.Windows.Forms.TabControl();
            this.tp_WebFeed2 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pg_WebFeed2 = new System.Windows.Forms.PropertyGrid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cb_WebFeed2 = new System.Windows.Forms.ComboBox();
            this.btn_WebFeed2_Export = new System.Windows.Forms.Button();
            this.btn_WebFeed2_Import = new System.Windows.Forms.Button();
            this.tp_WebFeed3 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pg_WebFeed3 = new System.Windows.Forms.PropertyGrid();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cb_WebFeed3 = new System.Windows.Forms.ComboBox();
            this.btn_WebFeed3_Export = new System.Windows.Forms.Button();
            this.btn_WebFeed3_Import = new System.Windows.Forms.Button();
            this.ss_Main.SuspendLayout();
            this.cms_NotifyIcon.SuspendLayout();
            this.tp_WebFeed1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tp_Planes.SuspendLayout();
            this.gb_CoveredArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Planes_Lifetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Planes_MaxAlt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Planes_MinAlt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLon)).BeginInit();
            this.tp_Server.SuspendLayout();
            this.gb_Network_Server.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Server_Port)).BeginInit();
            this.tp_Database.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tp_General.SuspendLayout();
            this.gb_LogWriter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Log_Verbosity)).BeginInit();
            this.gb_General_Windows.SuspendLayout();
            this.tc_Main.SuspendLayout();
            this.tp_WebFeed2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tp_WebFeed3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Main,
            this.tsl_Spring,
            this.tsl_CPULoad,
            this.tsl_MemoryAvailable,
            this.tsl_DBSize,
            this.tsl_Uptime});
            this.ss_Main.Location = new System.Drawing.Point(0, 428);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(634, 24);
            this.ss_Main.TabIndex = 0;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Main
            // 
            this.tsl_Main.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsl_Main.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsl_Main.Name = "tsl_Main";
            this.tsl_Main.Size = new System.Drawing.Size(46, 19);
            this.tsl_Main.Text = "Ready.";
            // 
            // tsl_Spring
            // 
            this.tsl_Spring.Name = "tsl_Spring";
            this.tsl_Spring.Size = new System.Drawing.Size(353, 19);
            this.tsl_Spring.Spring = true;
            // 
            // tsl_CPULoad
            // 
            this.tsl_CPULoad.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsl_CPULoad.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsl_CPULoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsl_CPULoad.Name = "tsl_CPULoad";
            this.tsl_CPULoad.Size = new System.Drawing.Size(60, 19);
            this.tsl_CPULoad.Text = "CPU load";
            // 
            // tsl_MemoryAvailable
            // 
            this.tsl_MemoryAvailable.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsl_MemoryAvailable.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsl_MemoryAvailable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsl_MemoryAvailable.Name = "tsl_MemoryAvailable";
            this.tsl_MemoryAvailable.Size = new System.Drawing.Size(62, 19);
            this.tsl_MemoryAvailable.Text = "MEM free";
            // 
            // tsl_DBSize
            // 
            this.tsl_DBSize.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsl_DBSize.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsl_DBSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsl_DBSize.Name = "tsl_DBSize";
            this.tsl_DBSize.Size = new System.Drawing.Size(48, 19);
            this.tsl_DBSize.Text = "DB size";
            // 
            // tsl_Uptime
            // 
            this.tsl_Uptime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsl_Uptime.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsl_Uptime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsl_Uptime.Name = "tsl_Uptime";
            this.tsl_Uptime.Size = new System.Drawing.Size(50, 19);
            this.tsl_Uptime.Text = "Uptime";
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(543, 390);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // ni_Main
            // 
            this.ni_Main.ContextMenuStrip = this.cms_NotifyIcon;
            this.ni_Main.Text = "AirScout PlaneServer";
            this.ni_Main.Visible = true;
            this.ni_Main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cms_NotifyIcon_MouseDoubleClick);
            // 
            // cms_NotifyIcon
            // 
            this.cms_NotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsi_Restore,
            this.toolStripSeparator1,
            this.tsi_Close});
            this.cms_NotifyIcon.Name = "cms_NotifyIcon";
            this.cms_NotifyIcon.Size = new System.Drawing.Size(119, 54);
            this.cms_NotifyIcon.Opening += new System.ComponentModel.CancelEventHandler(this.cms_NotifyIcon_Opening);
            this.cms_NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cms_NotifyIcon_MouseDoubleClick);
            // 
            // tsi_Restore
            // 
            this.tsi_Restore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsi_Restore.Name = "tsi_Restore";
            this.tsi_Restore.Size = new System.Drawing.Size(118, 22);
            this.tsi_Restore.Text = "&Restore";
            this.tsi_Restore.Click += new System.EventHandler(this.tsi_Restore_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(115, 6);
            // 
            // tsi_Close
            // 
            this.tsi_Close.Name = "tsi_Close";
            this.tsi_Close.Size = new System.Drawing.Size(118, 22);
            this.tsi_Close.Text = "&Close";
            this.tsi_Close.Click += new System.EventHandler(this.tsi_Close_Click);
            // 
            // bw_Webserver
            // 
            this.bw_Webserver.WorkerReportsProgress = true;
            this.bw_Webserver.WorkerSupportsCancellation = true;
            this.bw_Webserver.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Webserver_DoWork);
            this.bw_Webserver.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Webserver_ProgressChanged);
            // 
            // ti_Main
            // 
            this.ti_Main.Interval = 1000;
            this.ti_Main.Tick += new System.EventHandler(this.ti_Main_Tick);
            // 
            // bw_ADSBFeed
            // 
            this.bw_ADSBFeed.DisclaimerAccepted = "";
            this.bw_ADSBFeed.WorkerReportsProgress = true;
            this.bw_ADSBFeed.WorkerSupportsCancellation = true;
            // 
            // bw_JSONWriter
            // 
            this.bw_JSONWriter.WorkerReportsProgress = true;
            this.bw_JSONWriter.WorkerSupportsCancellation = true;
            this.bw_JSONWriter.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_JSONWriter_DoWork);
            // 
            // bw_DatabaseUpdater
            // 
            this.bw_DatabaseUpdater.WorkerReportsProgress = true;
            this.bw_DatabaseUpdater.WorkerSupportsCancellation = true;
            this.bw_DatabaseUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DatabaseUpdater_DoWork);
            this.bw_DatabaseUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_DatabaseUpdater_ProgressChanged);
            // 
            // tp_ADSBFeed
            // 
            this.tp_ADSBFeed.BackColor = System.Drawing.SystemColors.Control;
            this.tp_ADSBFeed.Location = new System.Drawing.Point(4, 22);
            this.tp_ADSBFeed.Name = "tp_ADSBFeed";
            this.tp_ADSBFeed.Padding = new System.Windows.Forms.Padding(3);
            this.tp_ADSBFeed.Size = new System.Drawing.Size(602, 337);
            this.tp_ADSBFeed.TabIndex = 6;
            this.tp_ADSBFeed.Text = "ADS-B Feed";
            // 
            // tp_WebFeed1
            // 
            this.tp_WebFeed1.BackColor = System.Drawing.SystemColors.Control;
            this.tp_WebFeed1.Controls.Add(this.panel2);
            this.tp_WebFeed1.Controls.Add(this.panel1);
            this.tp_WebFeed1.Location = new System.Drawing.Point(4, 22);
            this.tp_WebFeed1.Name = "tp_WebFeed1";
            this.tp_WebFeed1.Padding = new System.Windows.Forms.Padding(3);
            this.tp_WebFeed1.Size = new System.Drawing.Size(602, 337);
            this.tp_WebFeed1.TabIndex = 3;
            this.tp_WebFeed1.Text = "WebFeed 1";
            this.tp_WebFeed1.Enter += new System.EventHandler(this.tp_WebFeed1_Enter);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pg_WebFeed1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(596, 275);
            this.panel2.TabIndex = 4;
            // 
            // pg_WebFeed1
            // 
            this.pg_WebFeed1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_WebFeed1.Location = new System.Drawing.Point(0, 0);
            this.pg_WebFeed1.Name = "pg_WebFeed1";
            this.pg_WebFeed1.Size = new System.Drawing.Size(596, 275);
            this.pg_WebFeed1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_WebFeed1);
            this.panel1.Controls.Add(this.btn_WebFeed1_Export);
            this.panel1.Controls.Add(this.btn_WebFeed1_Import);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(596, 56);
            this.panel1.TabIndex = 3;
            // 
            // cb_WebFeed1
            // 
            this.cb_WebFeed1.DisplayMember = "Name";
            this.cb_WebFeed1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_WebFeed1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_WebFeed1.FormattingEnabled = true;
            this.cb_WebFeed1.Location = new System.Drawing.Point(22, 16);
            this.cb_WebFeed1.Name = "cb_WebFeed1";
            this.cb_WebFeed1.Size = new System.Drawing.Size(367, 24);
            this.cb_WebFeed1.TabIndex = 3;
            this.cb_WebFeed1.SelectedIndexChanged += new System.EventHandler(this.cb_WebFeed1_SelectedIndexChanged);
            // 
            // btn_WebFeed1_Export
            // 
            this.btn_WebFeed1_Export.Location = new System.Drawing.Point(493, 15);
            this.btn_WebFeed1_Export.Name = "btn_WebFeed1_Export";
            this.btn_WebFeed1_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_WebFeed1_Export.TabIndex = 2;
            this.btn_WebFeed1_Export.Text = "Export";
            this.btn_WebFeed1_Export.UseVisualStyleBackColor = true;
            this.btn_WebFeed1_Export.Click += new System.EventHandler(this.btn_WebFeed1_Export_Click);
            // 
            // btn_WebFeed1_Import
            // 
            this.btn_WebFeed1_Import.Location = new System.Drawing.Point(412, 15);
            this.btn_WebFeed1_Import.Name = "btn_WebFeed1_Import";
            this.btn_WebFeed1_Import.Size = new System.Drawing.Size(75, 23);
            this.btn_WebFeed1_Import.TabIndex = 1;
            this.btn_WebFeed1_Import.Text = "Import";
            this.btn_WebFeed1_Import.UseVisualStyleBackColor = true;
            this.btn_WebFeed1_Import.Click += new System.EventHandler(this.btn_WebFeed1_Import_Click);
            // 
            // tp_Planes
            // 
            this.tp_Planes.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Planes.Controls.Add(this.gb_CoveredArea);
            this.tp_Planes.Location = new System.Drawing.Point(4, 22);
            this.tp_Planes.Name = "tp_Planes";
            this.tp_Planes.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Planes.Size = new System.Drawing.Size(602, 337);
            this.tp_Planes.TabIndex = 2;
            this.tp_Planes.Text = "Planes";
            this.tp_Planes.Enter += new System.EventHandler(this.tab_Planes_Enter);
            // 
            // gb_CoveredArea
            // 
            this.gb_CoveredArea.Controls.Add(this.ud_Planes_Lifetime);
            this.gb_CoveredArea.Controls.Add(this.label15);
            this.gb_CoveredArea.Controls.Add(this.ud_Planes_MaxAlt);
            this.gb_CoveredArea.Controls.Add(this.label9);
            this.gb_CoveredArea.Controls.Add(this.ud_Planes_MinAlt);
            this.gb_CoveredArea.Controls.Add(this.label8);
            this.gb_CoveredArea.Controls.Add(this.gm_Planes_Coverage);
            this.gb_CoveredArea.Controls.Add(this.ud_MinLat);
            this.gb_CoveredArea.Controls.Add(this.ud_MaxLat);
            this.gb_CoveredArea.Controls.Add(this.ud_MinLon);
            this.gb_CoveredArea.Controls.Add(this.ud_MaxLon);
            this.gb_CoveredArea.Controls.Add(this.label4);
            this.gb_CoveredArea.Controls.Add(this.label5);
            this.gb_CoveredArea.Controls.Add(this.label6);
            this.gb_CoveredArea.Controls.Add(this.label7);
            this.gb_CoveredArea.Location = new System.Drawing.Point(15, 15);
            this.gb_CoveredArea.Name = "gb_CoveredArea";
            this.gb_CoveredArea.Size = new System.Drawing.Size(571, 316);
            this.gb_CoveredArea.TabIndex = 0;
            this.gb_CoveredArea.TabStop = false;
            this.gb_CoveredArea.Text = "Covered Area";
            // 
            // ud_Planes_Lifetime
            // 
            this.ud_Planes_Lifetime.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_Lifetime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Planes_Lifetime.Location = new System.Drawing.Point(476, 228);
            this.ud_Planes_Lifetime.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.ud_Planes_Lifetime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ud_Planes_Lifetime.Name = "ud_Planes_Lifetime";
            this.ud_Planes_Lifetime.Size = new System.Drawing.Size(77, 20);
            this.ud_Planes_Lifetime.TabIndex = 14;
            this.ud_Planes_Lifetime.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_Lifetime;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(346, 230);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 13;
            this.label15.Text = "Lifetime [min]:";
            // 
            // ud_Planes_MaxAlt
            // 
            this.ud_Planes_MaxAlt.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_MaxAlt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Planes_MaxAlt.Location = new System.Drawing.Point(479, 181);
            this.ud_Planes_MaxAlt.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ud_Planes_MaxAlt.Name = "ud_Planes_MaxAlt";
            this.ud_Planes_MaxAlt.Size = new System.Drawing.Size(77, 20);
            this.ud_Planes_MaxAlt.TabIndex = 12;
            this.ud_Planes_MaxAlt.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_MaxAlt;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(349, 183);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Max. Altitude [m]:";
            // 
            // ud_Planes_MinAlt
            // 
            this.ud_Planes_MinAlt.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_MinAlt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Planes_MinAlt.Location = new System.Drawing.Point(479, 155);
            this.ud_Planes_MinAlt.Maximum = new decimal(new int[] {
            12000,
            0,
            0,
            0});
            this.ud_Planes_MinAlt.Name = "ud_Planes_MinAlt";
            this.ud_Planes_MinAlt.Size = new System.Drawing.Size(77, 20);
            this.ud_Planes_MinAlt.TabIndex = 10;
            this.ud_Planes_MinAlt.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_MinAlt;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(349, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Min. Altitude [m]:";
            // 
            // gm_Planes_Coverage
            // 
            this.gm_Planes_Coverage.Bearing = 0F;
            this.gm_Planes_Coverage.CanDragMap = true;
            this.gm_Planes_Coverage.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Planes_Coverage.GrayScaleMode = false;
            this.gm_Planes_Coverage.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Planes_Coverage.LevelsKeepInMemmory = 5;
            this.gm_Planes_Coverage.Location = new System.Drawing.Point(6, 19);
            this.gm_Planes_Coverage.MarkersEnabled = true;
            this.gm_Planes_Coverage.MaxZoom = 2;
            this.gm_Planes_Coverage.MinZoom = 2;
            this.gm_Planes_Coverage.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Planes_Coverage.Name = "gm_Planes_Coverage";
            this.gm_Planes_Coverage.NegativeMode = false;
            this.gm_Planes_Coverage.PolygonsEnabled = true;
            this.gm_Planes_Coverage.RetryLoadTile = 0;
            this.gm_Planes_Coverage.RoutesEnabled = true;
            this.gm_Planes_Coverage.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Planes_Coverage.ShowTileGridLines = false;
            this.gm_Planes_Coverage.Size = new System.Drawing.Size(337, 291);
            this.gm_Planes_Coverage.TabIndex = 0;
            this.gm_Planes_Coverage.Zoom = 0D;
            // 
            // ud_MinLat
            // 
            this.ud_MinLat.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_MinLat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MinLat.DecimalPlaces = 2;
            this.ud_MinLat.Location = new System.Drawing.Point(479, 81);
            this.ud_MinLat.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.ud_MinLat.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.ud_MinLat.Name = "ud_MinLat";
            this.ud_MinLat.Size = new System.Drawing.Size(77, 20);
            this.ud_MinLat.TabIndex = 6;
            this.ud_MinLat.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_MinLat;
            this.ud_MinLat.ValueChanged += new System.EventHandler(this.ud_Planes_LatLon_ValueChanged);
            // 
            // ud_MaxLat
            // 
            this.ud_MaxLat.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_MaxLat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MaxLat.DecimalPlaces = 2;
            this.ud_MaxLat.Location = new System.Drawing.Point(479, 107);
            this.ud_MaxLat.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.ud_MaxLat.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.ud_MaxLat.Name = "ud_MaxLat";
            this.ud_MaxLat.Size = new System.Drawing.Size(77, 20);
            this.ud_MaxLat.TabIndex = 8;
            this.ud_MaxLat.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_MaxLat;
            this.ud_MaxLat.ValueChanged += new System.EventHandler(this.ud_Planes_LatLon_ValueChanged);
            // 
            // ud_MinLon
            // 
            this.ud_MinLon.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_MinLon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MinLon.DecimalPlaces = 2;
            this.ud_MinLon.Location = new System.Drawing.Point(479, 29);
            this.ud_MinLon.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.ud_MinLon.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.ud_MinLon.Name = "ud_MinLon";
            this.ud_MinLon.Size = new System.Drawing.Size(77, 20);
            this.ud_MinLon.TabIndex = 2;
            this.ud_MinLon.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_MinLon;
            this.ud_MinLon.ValueChanged += new System.EventHandler(this.ud_Planes_LatLon_ValueChanged);
            // 
            // ud_MaxLon
            // 
            this.ud_MaxLon.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Planes_MaxLon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MaxLon.DecimalPlaces = 2;
            this.ud_MaxLon.Location = new System.Drawing.Point(479, 55);
            this.ud_MaxLon.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.ud_MaxLon.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.ud_MaxLon.Name = "ud_MaxLon";
            this.ud_MaxLon.Size = new System.Drawing.Size(77, 20);
            this.ud_MaxLon.TabIndex = 4;
            this.ud_MaxLon.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Planes_MaxLon;
            this.ud_MaxLon.ValueChanged += new System.EventHandler(this.ud_Planes_LatLon_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(349, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Min. Lon [-90 ... 90deg]:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(349, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Max. Lon [-90 ... 90deg]:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(349, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Min. Lat [-180 ... 180deg]:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(349, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Max. Lat [-180 .. 180deg]:";
            // 
            // tp_Server
            // 
            this.tp_Server.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Server.Controls.Add(this.gb_Network_Server);
            this.tp_Server.Location = new System.Drawing.Point(4, 22);
            this.tp_Server.Name = "tp_Server";
            this.tp_Server.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Server.Size = new System.Drawing.Size(602, 337);
            this.tp_Server.TabIndex = 1;
            this.tp_Server.Text = "Server";
            // 
            // gb_Network_Server
            // 
            this.gb_Network_Server.Controls.Add(this.label1);
            this.gb_Network_Server.Controls.Add(this.ud_Server_Port);
            this.gb_Network_Server.Controls.Add(this.cb_Server_Active);
            this.gb_Network_Server.Location = new System.Drawing.Point(15, 15);
            this.gb_Network_Server.Name = "gb_Network_Server";
            this.gb_Network_Server.Size = new System.Drawing.Size(307, 94);
            this.gb_Network_Server.TabIndex = 0;
            this.gb_Network_Server.TabStop = false;
            this.gb_Network_Server.Text = "Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port (1024...65535, Default=9872):";
            // 
            // ud_Server_Port
            // 
            this.ud_Server_Port.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Webserver_Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Server_Port.Location = new System.Drawing.Point(218, 56);
            this.ud_Server_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ud_Server_Port.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.ud_Server_Port.Name = "ud_Server_Port";
            this.ud_Server_Port.Size = new System.Drawing.Size(71, 20);
            this.ud_Server_Port.TabIndex = 3;
            this.ud_Server_Port.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Webserver_Port;
            // 
            // cb_Server_Active
            // 
            this.cb_Server_Active.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_Server_Active.Checked = global::AirScoutPlaneServer.Properties.Settings.Default.Webserver_Active;
            this.cb_Server_Active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Server_Active.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScoutPlaneServer.Properties.Settings.Default, "Webserver_Active", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Server_Active.Location = new System.Drawing.Point(15, 29);
            this.cb_Server_Active.Name = "cb_Server_Active";
            this.cb_Server_Active.Size = new System.Drawing.Size(274, 21);
            this.cb_Server_Active.TabIndex = 2;
            this.cb_Server_Active.Text = "Activate Server:";
            this.cb_Server_Active.UseVisualStyleBackColor = true;
            this.cb_Server_Active.CheckedChanged += new System.EventHandler(this.cb_Server_Active_CheckedChanged);
            // 
            // tp_Database
            // 
            this.tp_Database.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Database.Controls.Add(this.groupBox2);
            this.tp_Database.Controls.Add(this.groupBox1);
            this.tp_Database.Location = new System.Drawing.Point(4, 22);
            this.tp_Database.Name = "tp_Database";
            this.tp_Database.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Database.Size = new System.Drawing.Size(602, 337);
            this.tp_Database.TabIndex = 7;
            this.tp_Database.Text = "Database";
            this.tp_Database.Enter += new System.EventHandler(this.tp_Database_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.tb_Database_PlanePositions_RowCount);
            this.groupBox2.Location = new System.Drawing.Point(15, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(570, 84);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Plane Positions";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(216, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "entries.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(38, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Count:";
            // 
            // tb_Database_PlanePositions_RowCount
            // 
            this.tb_Database_PlanePositions_RowCount.Location = new System.Drawing.Point(63, 28);
            this.tb_Database_PlanePositions_RowCount.Name = "tb_Database_PlanePositions_RowCount";
            this.tb_Database_PlanePositions_RowCount.ReadOnly = true;
            this.tb_Database_PlanePositions_RowCount.Size = new System.Drawing.Size(136, 20);
            this.tb_Database_PlanePositions_RowCount.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tb_Database_Filesize);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tb_Database_Location);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Information";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(141, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "MB.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Filesize:";
            // 
            // tb_Database_Filesize
            // 
            this.tb_Database_Filesize.Location = new System.Drawing.Point(63, 43);
            this.tb_Database_Filesize.Name = "tb_Database_Filesize";
            this.tb_Database_Filesize.ReadOnly = true;
            this.tb_Database_Filesize.Size = new System.Drawing.Size(72, 20);
            this.tb_Database_Filesize.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Location:";
            // 
            // tb_Database_Location
            // 
            this.tb_Database_Location.Location = new System.Drawing.Point(63, 19);
            this.tb_Database_Location.Name = "tb_Database_Location";
            this.tb_Database_Location.ReadOnly = true;
            this.tb_Database_Location.Size = new System.Drawing.Size(485, 20);
            this.tb_Database_Location.TabIndex = 0;
            // 
            // tp_General
            // 
            this.tp_General.BackColor = System.Drawing.SystemColors.Control;
            this.tp_General.Controls.Add(this.gb_LogWriter);
            this.tp_General.Controls.Add(this.gb_General_Windows);
            this.tp_General.Location = new System.Drawing.Point(4, 22);
            this.tp_General.Name = "tp_General";
            this.tp_General.Padding = new System.Windows.Forms.Padding(3);
            this.tp_General.Size = new System.Drawing.Size(602, 337);
            this.tp_General.TabIndex = 0;
            this.tp_General.Text = "General";
            // 
            // gb_LogWriter
            // 
            this.gb_LogWriter.Controls.Add(this.btn_Log_Show);
            this.gb_LogWriter.Controls.Add(this.ud_Log_Verbosity);
            this.gb_LogWriter.Controls.Add(this.label3);
            this.gb_LogWriter.Controls.Add(this.btn_Log_Directory);
            this.gb_LogWriter.Controls.Add(this.cb_Log_Active);
            this.gb_LogWriter.Controls.Add(this.label2);
            this.gb_LogWriter.Controls.Add(this.tb_Log_Directory);
            this.gb_LogWriter.Location = new System.Drawing.Point(15, 134);
            this.gb_LogWriter.Name = "gb_LogWriter";
            this.gb_LogWriter.Size = new System.Drawing.Size(570, 113);
            this.gb_LogWriter.TabIndex = 1;
            this.gb_LogWriter.TabStop = false;
            this.gb_LogWriter.Text = "Log";
            // 
            // btn_Log_Show
            // 
            this.btn_Log_Show.Location = new System.Drawing.Point(474, 15);
            this.btn_Log_Show.Name = "btn_Log_Show";
            this.btn_Log_Show.Size = new System.Drawing.Size(75, 23);
            this.btn_Log_Show.TabIndex = 6;
            this.btn_Log_Show.Text = "Show";
            this.btn_Log_Show.UseVisualStyleBackColor = true;
            this.btn_Log_Show.Click += new System.EventHandler(this.btn_Log_Show_Click);
            // 
            // ud_Log_Verbosity
            // 
            this.ud_Log_Verbosity.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutPlaneServer.Properties.Settings.Default, "Log_Verbosity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Log_Verbosity.Location = new System.Drawing.Point(116, 77);
            this.ud_Log_Verbosity.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ud_Log_Verbosity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ud_Log_Verbosity.Name = "ud_Log_Verbosity";
            this.ud_Log_Verbosity.Size = new System.Drawing.Size(37, 20);
            this.ud_Log_Verbosity.TabIndex = 5;
            this.ud_Log_Verbosity.Value = global::AirScoutPlaneServer.Properties.Settings.Default.Log_Verbosity;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Verbosity (1..9):";
            // 
            // btn_Log_Directory
            // 
            this.btn_Log_Directory.Location = new System.Drawing.Point(474, 46);
            this.btn_Log_Directory.Name = "btn_Log_Directory";
            this.btn_Log_Directory.Size = new System.Drawing.Size(75, 23);
            this.btn_Log_Directory.TabIndex = 3;
            this.btn_Log_Directory.Text = "Select";
            this.btn_Log_Directory.UseVisualStyleBackColor = true;
            this.btn_Log_Directory.Click += new System.EventHandler(this.btn_Log_Directory_Click);
            // 
            // cb_Log_Active
            // 
            this.cb_Log_Active.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_Log_Active.Checked = global::AirScoutPlaneServer.Properties.Settings.Default.Log_Active;
            this.cb_Log_Active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Log_Active.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScoutPlaneServer.Properties.Settings.Default, "Log_Active", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Log_Active.Location = new System.Drawing.Point(13, 19);
            this.cb_Log_Active.Name = "cb_Log_Active";
            this.cb_Log_Active.Size = new System.Drawing.Size(120, 19);
            this.cb_Log_Active.TabIndex = 3;
            this.cb_Log_Active.Text = "Activate Logging:";
            this.cb_Log_Active.UseVisualStyleBackColor = true;
            this.cb_Log_Active.CheckedChanged += new System.EventHandler(this.cb_Log_Active_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Directory:";
            // 
            // tb_Log_Directory
            // 
            this.tb_Log_Directory.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutPlaneServer.Properties.Settings.Default, "Log_Directory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Log_Directory.Location = new System.Drawing.Point(116, 48);
            this.tb_Log_Directory.Name = "tb_Log_Directory";
            this.tb_Log_Directory.Size = new System.Drawing.Size(333, 20);
            this.tb_Log_Directory.TabIndex = 0;
            this.tb_Log_Directory.Text = global::AirScoutPlaneServer.Properties.Settings.Default.Log_Directory;
            // 
            // gb_General_Windows
            // 
            this.gb_General_Windows.Controls.Add(this.cb_Windows_Startup_Autorun);
            this.gb_General_Windows.Controls.Add(this.cb_Windows_Startup_Systray);
            this.gb_General_Windows.Controls.Add(this.cb_Windows_Startup_Minimized);
            this.gb_General_Windows.Location = new System.Drawing.Point(15, 15);
            this.gb_General_Windows.Name = "gb_General_Windows";
            this.gb_General_Windows.Size = new System.Drawing.Size(570, 113);
            this.gb_General_Windows.TabIndex = 0;
            this.gb_General_Windows.TabStop = false;
            this.gb_General_Windows.Text = "Windows";
            // 
            // cb_Windows_Startup_Autorun
            // 
            this.cb_Windows_Startup_Autorun.AutoSize = true;
            this.cb_Windows_Startup_Autorun.Checked = global::AirScoutPlaneServer.Properties.Settings.Default.Windows_Startup_Autorun;
            this.cb_Windows_Startup_Autorun.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScoutPlaneServer.Properties.Settings.Default, "Windows_Startup_Autorun", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Windows_Startup_Autorun.Location = new System.Drawing.Point(15, 29);
            this.cb_Windows_Startup_Autorun.Name = "cb_Windows_Startup_Autorun";
            this.cb_Windows_Startup_Autorun.Size = new System.Drawing.Size(212, 17);
            this.cb_Windows_Startup_Autorun.TabIndex = 2;
            this.cb_Windows_Startup_Autorun.Text = "Startup on System Start (Windows only)";
            this.cb_Windows_Startup_Autorun.UseVisualStyleBackColor = true;
            this.cb_Windows_Startup_Autorun.CheckedChanged += new System.EventHandler(this.cb_Windows_Startup_Autorun_CheckedChanged);
            // 
            // cb_Windows_Startup_Systray
            // 
            this.cb_Windows_Startup_Systray.AutoSize = true;
            this.cb_Windows_Startup_Systray.Checked = global::AirScoutPlaneServer.Properties.Settings.Default.Windows_Startup_Systray;
            this.cb_Windows_Startup_Systray.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScoutPlaneServer.Properties.Settings.Default, "Windows_Startup_Systray", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Windows_Startup_Systray.Location = new System.Drawing.Point(15, 75);
            this.cb_Windows_Startup_Systray.Name = "cb_Windows_Startup_Systray";
            this.cb_Windows_Startup_Systray.Size = new System.Drawing.Size(277, 17);
            this.cb_Windows_Startup_Systray.TabIndex = 1;
            this.cb_Windows_Startup_Systray.Text = "Show on systray only when minimized (Windows only)";
            this.cb_Windows_Startup_Systray.UseVisualStyleBackColor = true;
            // 
            // cb_Windows_Startup_Minimized
            // 
            this.cb_Windows_Startup_Minimized.AutoSize = true;
            this.cb_Windows_Startup_Minimized.Checked = global::AirScoutPlaneServer.Properties.Settings.Default.Windows_Startup_Minimized;
            this.cb_Windows_Startup_Minimized.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScoutPlaneServer.Properties.Settings.Default, "Windows_Startup_Minimized", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Windows_Startup_Minimized.Location = new System.Drawing.Point(15, 52);
            this.cb_Windows_Startup_Minimized.Name = "cb_Windows_Startup_Minimized";
            this.cb_Windows_Startup_Minimized.Size = new System.Drawing.Size(183, 17);
            this.cb_Windows_Startup_Minimized.TabIndex = 0;
            this.cb_Windows_Startup_Minimized.Text = "Startup minimized (Windows only)";
            this.cb_Windows_Startup_Minimized.UseVisualStyleBackColor = true;
            // 
            // tc_Main
            // 
            this.tc_Main.Controls.Add(this.tp_General);
            this.tc_Main.Controls.Add(this.tp_Database);
            this.tc_Main.Controls.Add(this.tp_Server);
            this.tc_Main.Controls.Add(this.tp_Planes);
            this.tc_Main.Controls.Add(this.tp_WebFeed1);
            this.tc_Main.Controls.Add(this.tp_WebFeed2);
            this.tc_Main.Controls.Add(this.tp_WebFeed3);
            this.tc_Main.Controls.Add(this.tp_ADSBFeed);
            this.tc_Main.Location = new System.Drawing.Point(12, 12);
            this.tc_Main.Name = "tc_Main";
            this.tc_Main.SelectedIndex = 0;
            this.tc_Main.Size = new System.Drawing.Size(610, 363);
            this.tc_Main.TabIndex = 1;
            // 
            // tp_WebFeed2
            // 
            this.tp_WebFeed2.BackColor = System.Drawing.SystemColors.Control;
            this.tp_WebFeed2.Controls.Add(this.panel3);
            this.tp_WebFeed2.Controls.Add(this.panel4);
            this.tp_WebFeed2.Location = new System.Drawing.Point(4, 22);
            this.tp_WebFeed2.Name = "tp_WebFeed2";
            this.tp_WebFeed2.Padding = new System.Windows.Forms.Padding(3);
            this.tp_WebFeed2.Size = new System.Drawing.Size(602, 337);
            this.tp_WebFeed2.TabIndex = 8;
            this.tp_WebFeed2.Text = "WebFeed 2";
            this.tp_WebFeed2.Enter += new System.EventHandler(this.tp_WebFeed2_Enter);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pg_WebFeed2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 59);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(596, 275);
            this.panel3.TabIndex = 4;
            // 
            // pg_WebFeed2
            // 
            this.pg_WebFeed2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_WebFeed2.Location = new System.Drawing.Point(0, 0);
            this.pg_WebFeed2.Name = "pg_WebFeed2";
            this.pg_WebFeed2.Size = new System.Drawing.Size(596, 275);
            this.pg_WebFeed2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cb_WebFeed2);
            this.panel4.Controls.Add(this.btn_WebFeed2_Export);
            this.panel4.Controls.Add(this.btn_WebFeed2_Import);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(596, 56);
            this.panel4.TabIndex = 3;
            // 
            // cb_WebFeed2
            // 
            this.cb_WebFeed2.DisplayMember = "Name";
            this.cb_WebFeed2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_WebFeed2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_WebFeed2.FormattingEnabled = true;
            this.cb_WebFeed2.Location = new System.Drawing.Point(22, 16);
            this.cb_WebFeed2.Name = "cb_WebFeed2";
            this.cb_WebFeed2.Size = new System.Drawing.Size(367, 24);
            this.cb_WebFeed2.TabIndex = 4;
            this.cb_WebFeed2.SelectedIndexChanged += new System.EventHandler(this.cb_WebFeed2_SelectedIndexChanged);
            // 
            // btn_WebFeed2_Export
            // 
            this.btn_WebFeed2_Export.Location = new System.Drawing.Point(509, 15);
            this.btn_WebFeed2_Export.Name = "btn_WebFeed2_Export";
            this.btn_WebFeed2_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_WebFeed2_Export.TabIndex = 2;
            this.btn_WebFeed2_Export.Text = "Export";
            this.btn_WebFeed2_Export.UseVisualStyleBackColor = true;
            this.btn_WebFeed2_Export.Click += new System.EventHandler(this.btn_WebFeed2_Export_Click);
            // 
            // btn_WebFeed2_Import
            // 
            this.btn_WebFeed2_Import.Location = new System.Drawing.Point(428, 15);
            this.btn_WebFeed2_Import.Name = "btn_WebFeed2_Import";
            this.btn_WebFeed2_Import.Size = new System.Drawing.Size(75, 23);
            this.btn_WebFeed2_Import.TabIndex = 1;
            this.btn_WebFeed2_Import.Text = "Import";
            this.btn_WebFeed2_Import.UseVisualStyleBackColor = true;
            this.btn_WebFeed2_Import.Click += new System.EventHandler(this.btn_WebFeed2_Import_Click);
            // 
            // tp_WebFeed3
            // 
            this.tp_WebFeed3.BackColor = System.Drawing.SystemColors.Control;
            this.tp_WebFeed3.Controls.Add(this.panel5);
            this.tp_WebFeed3.Controls.Add(this.panel6);
            this.tp_WebFeed3.Location = new System.Drawing.Point(4, 22);
            this.tp_WebFeed3.Name = "tp_WebFeed3";
            this.tp_WebFeed3.Padding = new System.Windows.Forms.Padding(3);
            this.tp_WebFeed3.Size = new System.Drawing.Size(602, 337);
            this.tp_WebFeed3.TabIndex = 9;
            this.tp_WebFeed3.Text = "WebFeed 3";
            this.tp_WebFeed3.Enter += new System.EventHandler(this.tp_WebFeed3_Enter);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pg_WebFeed3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 59);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(596, 275);
            this.panel5.TabIndex = 4;
            // 
            // pg_WebFeed3
            // 
            this.pg_WebFeed3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pg_WebFeed3.Location = new System.Drawing.Point(0, 0);
            this.pg_WebFeed3.Name = "pg_WebFeed3";
            this.pg_WebFeed3.Size = new System.Drawing.Size(596, 275);
            this.pg_WebFeed3.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cb_WebFeed3);
            this.panel6.Controls.Add(this.btn_WebFeed3_Export);
            this.panel6.Controls.Add(this.btn_WebFeed3_Import);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(596, 56);
            this.panel6.TabIndex = 3;
            // 
            // cb_WebFeed3
            // 
            this.cb_WebFeed3.DisplayMember = "Name";
            this.cb_WebFeed3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_WebFeed3.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_WebFeed3.FormattingEnabled = true;
            this.cb_WebFeed3.Location = new System.Drawing.Point(22, 16);
            this.cb_WebFeed3.Name = "cb_WebFeed3";
            this.cb_WebFeed3.Size = new System.Drawing.Size(367, 24);
            this.cb_WebFeed3.TabIndex = 4;
            this.cb_WebFeed3.SelectedIndexChanged += new System.EventHandler(this.cb_WebFeed3_SelectedIndexChanged);
            // 
            // btn_WebFeed3_Export
            // 
            this.btn_WebFeed3_Export.Location = new System.Drawing.Point(503, 15);
            this.btn_WebFeed3_Export.Name = "btn_WebFeed3_Export";
            this.btn_WebFeed3_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_WebFeed3_Export.TabIndex = 2;
            this.btn_WebFeed3_Export.Text = "Export";
            this.btn_WebFeed3_Export.UseVisualStyleBackColor = true;
            this.btn_WebFeed3_Export.Click += new System.EventHandler(this.btn_WebFeed3_Export_Click);
            // 
            // btn_WebFeed3_Import
            // 
            this.btn_WebFeed3_Import.Location = new System.Drawing.Point(422, 15);
            this.btn_WebFeed3_Import.Name = "btn_WebFeed3_Import";
            this.btn_WebFeed3_Import.Size = new System.Drawing.Size(75, 23);
            this.btn_WebFeed3_Import.TabIndex = 1;
            this.btn_WebFeed3_Import.Text = "Import";
            this.btn_WebFeed3_Import.UseVisualStyleBackColor = true;
            this.btn_WebFeed3_Import.Click += new System.EventHandler(this.btn_WebFeed3_Import_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 452);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.tc_Main);
            this.Controls.Add(this.ss_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "AirScout Plane Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainDlg_FormClosed);
            this.Load += new System.EventHandler(this.MainDlg_Load);
            this.Resize += new System.EventHandler(this.MainDlg_Resize);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.cms_NotifyIcon.ResumeLayout(false);
            this.tp_WebFeed1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tp_Planes.ResumeLayout(false);
            this.gb_CoveredArea.ResumeLayout(false);
            this.gb_CoveredArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Planes_Lifetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Planes_MaxAlt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Planes_MinAlt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLon)).EndInit();
            this.tp_Server.ResumeLayout(false);
            this.gb_Network_Server.ResumeLayout(false);
            this.gb_Network_Server.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Server_Port)).EndInit();
            this.tp_Database.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tp_General.ResumeLayout(false);
            this.gb_LogWriter.ResumeLayout(false);
            this.gb_LogWriter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Log_Verbosity)).EndInit();
            this.gb_General_Windows.ResumeLayout(false);
            this.gb_General_Windows.PerformLayout();
            this.tc_Main.ResumeLayout(false);
            this.tp_WebFeed2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tp_WebFeed3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Main;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.NotifyIcon ni_Main;
        private System.Windows.Forms.ContextMenuStrip cms_NotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem tsi_Restore;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsi_Close;
        private System.ComponentModel.BackgroundWorker bw_Webserver;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Spring;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Uptime;
        private System.Windows.Forms.Timer ti_Main;
        private AirScout.PlaneFeeds.PlaneFeed_ADSB bw_ADSBFeed;
        private System.Windows.Forms.ToolStripStatusLabel tsl_CPULoad;
        private System.Windows.Forms.ToolStripStatusLabel tsl_MemoryAvailable;
        private System.Windows.Forms.ToolStripStatusLabel tsl_DBSize;
        private System.ComponentModel.BackgroundWorker bw_JSONWriter;
        private System.ComponentModel.BackgroundWorker bw_DatabaseUpdater;
        private System.Windows.Forms.TabPage tp_ADSBFeed;
        private System.Windows.Forms.TabPage tp_WebFeed1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PropertyGrid pg_WebFeed1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_WebFeed1_Export;
        private System.Windows.Forms.Button btn_WebFeed1_Import;
        private System.Windows.Forms.TabPage tp_Planes;
        private System.Windows.Forms.GroupBox gb_CoveredArea;
        private System.Windows.Forms.NumericUpDown ud_Planes_Lifetime;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown ud_Planes_MaxAlt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ud_Planes_MinAlt;
        private System.Windows.Forms.Label label8;
        private GMap.NET.WindowsForms.GMapControl gm_Planes_Coverage;
        private System.Windows.Forms.NumericUpDown ud_MinLat;
        private System.Windows.Forms.NumericUpDown ud_MaxLat;
        private System.Windows.Forms.NumericUpDown ud_MinLon;
        private System.Windows.Forms.NumericUpDown ud_MaxLon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tp_Server;
        private System.Windows.Forms.GroupBox gb_Network_Server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ud_Server_Port;
        private System.Windows.Forms.CheckBox cb_Server_Active;
        private System.Windows.Forms.TabPage tp_Database;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_Database_PlanePositions_RowCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_Database_Filesize;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_Database_Location;
        private System.Windows.Forms.TabPage tp_General;
        private System.Windows.Forms.GroupBox gb_LogWriter;
        private System.Windows.Forms.Button btn_Log_Show;
        private System.Windows.Forms.NumericUpDown ud_Log_Verbosity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Log_Directory;
        private System.Windows.Forms.CheckBox cb_Log_Active;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Log_Directory;
        private System.Windows.Forms.GroupBox gb_General_Windows;
        private System.Windows.Forms.CheckBox cb_Windows_Startup_Autorun;
        private System.Windows.Forms.CheckBox cb_Windows_Startup_Systray;
        private System.Windows.Forms.CheckBox cb_Windows_Startup_Minimized;
        private System.Windows.Forms.TabControl tc_Main;
        private System.Windows.Forms.TabPage tp_WebFeed2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PropertyGrid pg_WebFeed2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btn_WebFeed2_Export;
        private System.Windows.Forms.Button btn_WebFeed2_Import;
        private System.Windows.Forms.TabPage tp_WebFeed3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PropertyGrid pg_WebFeed3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btn_WebFeed3_Export;
        private System.Windows.Forms.Button btn_WebFeed3_Import;
        private System.Windows.Forms.ComboBox cb_WebFeed1;
        private System.Windows.Forms.ComboBox cb_WebFeed2;
        private System.Windows.Forms.ComboBox cb_WebFeed3;
    }
}

