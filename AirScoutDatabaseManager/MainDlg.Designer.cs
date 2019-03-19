namespace AirScoutDatabaseManager
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
            this.tc_Main = new System.Windows.Forms.TabControl();
            this.tp_General = new System.Windows.Forms.TabPage();
            this.gm_Coverage = new GMap.NET.WindowsForms.GMapControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label35 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.tp_Locations = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgv_Locations = new System.Windows.Forms.DataGridView();
            this.gm_Locations = new GMap.NET.WindowsForms.GMapControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Locations_Callsign_Filter = new System.Windows.Forms.TextBox();
            this.cb_Locations_ChangedOnly = new System.Windows.Forms.CheckBox();
            this.btn_Locations_Sort = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Locations_Import_USR = new System.Windows.Forms.Button();
            this.btn_Locations_Import_CSV = new System.Windows.Forms.Button();
            this.btn_Locations_Import_DTB = new System.Windows.Forms.Button();
            this.btn_Locations_Import_CALL3 = new System.Windows.Forms.Button();
            this.btn_Locations_Import_AirScout = new System.Windows.Forms.Button();
            this.btn_Locations_Export = new System.Windows.Forms.Button();
            this.btn_Locations_Save = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_QRZ_Stop = new System.Windows.Forms.Button();
            this.btn_QRZ_Start = new System.Windows.Forms.Button();
            this.tb_Locations_Status = new System.Windows.Forms.TextBox();
            this.tp_QRV = new System.Windows.Forms.TabPage();
            this.dgv_QRV = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cb_QRV_ChangedOnly = new System.Windows.Forms.CheckBox();
            this.btn_QRV_Sort = new System.Windows.Forms.Button();
            this.btn_QRV_Export = new System.Windows.Forms.Button();
            this.btn_QRV_Save = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_QRV_Callsign_Filter = new System.Windows.Forms.TextBox();
            this.tb_QRV_Status = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_QRV_Import_EDI = new System.Windows.Forms.Button();
            this.btn_QRV_Import_WinTest = new System.Windows.Forms.Button();
            this.tp_Aircrafts = new System.Windows.Forms.TabPage();
            this.lbl_Aircrafts_UnkownHex = new System.Windows.Forms.Label();
            this.lbl_Aircrafts_UnkownType = new System.Windows.Forms.Label();
            this.lbl_Aircrafts_UnkownCall = new System.Windows.Forms.Label();
            this.lbl_Aircrafts_Total = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Update_Aircrafts_Stop = new System.Windows.Forms.Button();
            this.btn_Update_Aicrafts_Start = new System.Windows.Forms.Button();
            this.btn_Update_Airports = new System.Windows.Forms.Button();
            this.btn_Update_Airlines = new System.Windows.Forms.Button();
            this.tp_Upload = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_StationDatabase_Export = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tp_Extras = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_SFTP_GenarateFile = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bw_QRZ = new System.ComponentModel.BackgroundWorker();
            this.bw_DatabaseUpdater = new System.ComponentModel.BackgroundWorker();
            this.bw_AircraftUpdater = new System.ComponentModel.BackgroundWorker();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btn_AircraftDatabase_Export = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_Coverage_MaxLat = new ScoutBase.Core.DoubleTextBox();
            this.tb_Coverage_MinLat = new ScoutBase.Core.DoubleTextBox();
            this.tb_Coverage_MaxLon = new ScoutBase.Core.DoubleTextBox();
            this.tb_Coverage_MinLon = new ScoutBase.Core.DoubleTextBox();
            this.tb_Update_Aircrafts = new System.Windows.Forms.TextBox();
            this.tb_Update_Airports = new System.Windows.Forms.TextBox();
            this.tb_Update_Airlines = new System.Windows.Forms.TextBox();
            this.tb_AircraftDatabase_RemoteHost = new System.Windows.Forms.TextBox();
            this.tb_AircraftDatabase_Password = new System.Windows.Forms.TextBox();
            this.tb_AircraftDatabase_User = new System.Windows.Forms.TextBox();
            this.tb_AircraftDatabase_RemoteDir = new System.Windows.Forms.TextBox();
            this.tb_AircraftDabase_LocalDir = new System.Windows.Forms.TextBox();
            this.tb_StationDatabase_RemoteHost = new System.Windows.Forms.TextBox();
            this.tb_StationDatabase_Export_Password = new System.Windows.Forms.TextBox();
            this.tb_StationDatabase_Export_User = new System.Windows.Forms.TextBox();
            this.tb_StationDatabase_RemoteDir = new System.Windows.Forms.TextBox();
            this.tb_StationDatabase_LocalDir = new System.Windows.Forms.TextBox();
            this.tb_SFTP_Password = new System.Windows.Forms.TextBox();
            this.tb_SFTP_User = new System.Windows.Forms.TextBox();
            this.tb_SFTP_URL = new System.Windows.Forms.TextBox();
            this.ss_Main.SuspendLayout();
            this.tc_Main.SuspendLayout();
            this.tp_General.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tp_Locations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Locations)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tp_QRV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_QRV)).BeginInit();
            this.panel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tp_Aircrafts.SuspendLayout();
            this.tp_Upload.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tp_Extras.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 575);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(1106, 22);
            this.ss_Main.TabIndex = 0;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(39, 17);
            this.tsl_Status.Text = "Status";
            // 
            // tc_Main
            // 
            this.tc_Main.Controls.Add(this.tp_General);
            this.tc_Main.Controls.Add(this.tp_Locations);
            this.tc_Main.Controls.Add(this.tp_QRV);
            this.tc_Main.Controls.Add(this.tp_Aircrafts);
            this.tc_Main.Controls.Add(this.tp_Upload);
            this.tc_Main.Controls.Add(this.tp_Extras);
            this.tc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_Main.Location = new System.Drawing.Point(0, 0);
            this.tc_Main.Name = "tc_Main";
            this.tc_Main.SelectedIndex = 0;
            this.tc_Main.Size = new System.Drawing.Size(1106, 575);
            this.tc_Main.TabIndex = 1;
            // 
            // tp_General
            // 
            this.tp_General.BackColor = System.Drawing.SystemColors.Control;
            this.tp_General.Controls.Add(this.gm_Coverage);
            this.tp_General.Controls.Add(this.panel1);
            this.tp_General.Location = new System.Drawing.Point(4, 22);
            this.tp_General.Name = "tp_General";
            this.tp_General.Padding = new System.Windows.Forms.Padding(3);
            this.tp_General.Size = new System.Drawing.Size(1098, 549);
            this.tp_General.TabIndex = 0;
            this.tp_General.Text = "General";
            this.tp_General.Enter += new System.EventHandler(this.tp_General_Enter);
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
            this.gm_Coverage.Location = new System.Drawing.Point(3, 3);
            this.gm_Coverage.MarkersEnabled = true;
            this.gm_Coverage.MaxZoom = 2;
            this.gm_Coverage.MinZoom = 2;
            this.gm_Coverage.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Coverage.Name = "gm_Coverage";
            this.gm_Coverage.NegativeMode = false;
            this.gm_Coverage.PolygonsEnabled = true;
            this.gm_Coverage.RetryLoadTile = 0;
            this.gm_Coverage.RoutesEnabled = true;
            this.gm_Coverage.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Coverage.ShowTileGridLines = false;
            this.gm_Coverage.Size = new System.Drawing.Size(917, 543);
            this.gm_Coverage.TabIndex = 1;
            this.gm_Coverage.Zoom = 0D;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_Coverage_MaxLat);
            this.panel1.Controls.Add(this.tb_Coverage_MinLat);
            this.panel1.Controls.Add(this.tb_Coverage_MaxLon);
            this.panel1.Controls.Add(this.tb_Coverage_MinLon);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.label54);
            this.panel1.Controls.Add(this.label59);
            this.panel1.Controls.Add(this.label60);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(920, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(175, 543);
            this.panel1.TabIndex = 0;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(19, 109);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(74, 13);
            this.label35.TabIndex = 32;
            this.label35.Text = "Max. Latitude:";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.Location = new System.Drawing.Point(19, 83);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(71, 13);
            this.label54.TabIndex = 31;
            this.label54.Text = "Min. Latitude:";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.Location = new System.Drawing.Point(19, 56);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(83, 13);
            this.label59.TabIndex = 30;
            this.label59.Text = "Max. Longitude:";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.Location = new System.Drawing.Point(19, 30);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(80, 13);
            this.label60.TabIndex = 29;
            this.label60.Text = "Min. Longitude:";
            // 
            // tp_Locations
            // 
            this.tp_Locations.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Locations.Controls.Add(this.splitContainer1);
            this.tp_Locations.Controls.Add(this.panel2);
            this.tp_Locations.Location = new System.Drawing.Point(4, 22);
            this.tp_Locations.Name = "tp_Locations";
            this.tp_Locations.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Locations.Size = new System.Drawing.Size(1098, 549);
            this.tp_Locations.TabIndex = 1;
            this.tp_Locations.Text = "Locations";
            this.tp_Locations.Enter += new System.EventHandler(this.tp_Locations_Enter);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgv_Locations);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gm_Locations);
            this.splitContainer1.Size = new System.Drawing.Size(1092, 414);
            this.splitContainer1.SplitterDistance = 363;
            this.splitContainer1.TabIndex = 3;
            // 
            // dgv_Locations
            // 
            this.dgv_Locations.AllowUserToResizeRows = false;
            this.dgv_Locations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Locations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Locations.Location = new System.Drawing.Point(0, 0);
            this.dgv_Locations.Name = "dgv_Locations";
            this.dgv_Locations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Locations.Size = new System.Drawing.Size(363, 414);
            this.dgv_Locations.TabIndex = 1;
            this.dgv_Locations.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_Locations_CellFormatting);
            this.dgv_Locations.SelectionChanged += new System.EventHandler(this.dgv_Locations_SelectionChanged);
            // 
            // gm_Locations
            // 
            this.gm_Locations.Bearing = 0F;
            this.gm_Locations.CanDragMap = true;
            this.gm_Locations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Locations.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Locations.GrayScaleMode = false;
            this.gm_Locations.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Locations.LevelsKeepInMemmory = 5;
            this.gm_Locations.Location = new System.Drawing.Point(0, 0);
            this.gm_Locations.MarkersEnabled = true;
            this.gm_Locations.MaxZoom = 2;
            this.gm_Locations.MinZoom = 2;
            this.gm_Locations.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Locations.Name = "gm_Locations";
            this.gm_Locations.NegativeMode = false;
            this.gm_Locations.PolygonsEnabled = true;
            this.gm_Locations.RetryLoadTile = 0;
            this.gm_Locations.RoutesEnabled = true;
            this.gm_Locations.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Locations.ShowTileGridLines = false;
            this.gm_Locations.Size = new System.Drawing.Size(725, 414);
            this.gm_Locations.TabIndex = 2;
            this.gm_Locations.Zoom = 0D;
            this.gm_Locations.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.gm_Locations_OnMarkerEnter);
            this.gm_Locations.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.gm_Locations_OnMarkerLeave);
            this.gm_Locations.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gm_Locations_MouseDown);
            this.gm_Locations.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gm_Locations_MouseMove);
            this.gm_Locations.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gm_Locations_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.tb_Locations_Callsign_Filter);
            this.panel2.Controls.Add(this.cb_Locations_ChangedOnly);
            this.panel2.Controls.Add(this.btn_Locations_Sort);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.btn_Locations_Export);
            this.panel2.Controls.Add(this.btn_Locations_Save);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.tb_Locations_Status);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 417);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1092, 129);
            this.panel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(668, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Callsign Filter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Status";
            // 
            // tb_Locations_Callsign_Filter
            // 
            this.tb_Locations_Callsign_Filter.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Locations_Callsign_Filter.Location = new System.Drawing.Point(671, 36);
            this.tb_Locations_Callsign_Filter.Name = "tb_Locations_Callsign_Filter";
            this.tb_Locations_Callsign_Filter.Size = new System.Drawing.Size(100, 20);
            this.tb_Locations_Callsign_Filter.TabIndex = 8;
            this.tb_Locations_Callsign_Filter.TextChanged += new System.EventHandler(this.tb_Locations_Callsign_Filter_TextChanged);
            // 
            // cb_Locations_ChangedOnly
            // 
            this.cb_Locations_ChangedOnly.AutoSize = true;
            this.cb_Locations_ChangedOnly.Location = new System.Drawing.Point(797, 38);
            this.cb_Locations_ChangedOnly.Name = "cb_Locations_ChangedOnly";
            this.cb_Locations_ChangedOnly.Size = new System.Drawing.Size(145, 17);
            this.cb_Locations_ChangedOnly.TabIndex = 7;
            this.cb_Locations_ChangedOnly.Text = "Show changed rows only";
            this.cb_Locations_ChangedOnly.UseVisualStyleBackColor = true;
            this.cb_Locations_ChangedOnly.CheckedChanged += new System.EventHandler(this.cb_Locations_ChangedOnly_CheckedChanged);
            // 
            // btn_Locations_Sort
            // 
            this.btn_Locations_Sort.Location = new System.Drawing.Point(735, 94);
            this.btn_Locations_Sort.Name = "btn_Locations_Sort";
            this.btn_Locations_Sort.Size = new System.Drawing.Size(75, 23);
            this.btn_Locations_Sort.TabIndex = 6;
            this.btn_Locations_Sort.Text = "Sort";
            this.btn_Locations_Sort.UseVisualStyleBackColor = true;
            this.btn_Locations_Sort.Click += new System.EventHandler(this.btn_Locations_Sort_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_Locations_Import_USR);
            this.groupBox2.Controls.Add(this.btn_Locations_Import_CSV);
            this.groupBox2.Controls.Add(this.btn_Locations_Import_DTB);
            this.groupBox2.Controls.Add(this.btn_Locations_Import_CALL3);
            this.groupBox2.Controls.Add(this.btn_Locations_Import_AirScout);
            this.groupBox2.Location = new System.Drawing.Point(188, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(531, 55);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import";
            // 
            // btn_Locations_Import_USR
            // 
            this.btn_Locations_Import_USR.Location = new System.Drawing.Point(113, 26);
            this.btn_Locations_Import_USR.Name = "btn_Locations_Import_USR";
            this.btn_Locations_Import_USR.Size = new System.Drawing.Size(92, 23);
            this.btn_Locations_Import_USR.TabIndex = 4;
            this.btn_Locations_Import_USR.Text = "AirScout (USR)";
            this.btn_Locations_Import_USR.UseVisualStyleBackColor = true;
            this.btn_Locations_Import_USR.Click += new System.EventHandler(this.btn_Locations_Import_USR_Click);
            // 
            // btn_Locations_Import_CSV
            // 
            this.btn_Locations_Import_CSV.Location = new System.Drawing.Point(432, 26);
            this.btn_Locations_Import_CSV.Name = "btn_Locations_Import_CSV";
            this.btn_Locations_Import_CSV.Size = new System.Drawing.Size(93, 23);
            this.btn_Locations_Import_CSV.TabIndex = 3;
            this.btn_Locations_Import_CSV.Text = "LOC (CSV)";
            this.btn_Locations_Import_CSV.UseVisualStyleBackColor = true;
            this.btn_Locations_Import_CSV.Click += new System.EventHandler(this.btn_Locations_Import_CSV_Click);
            // 
            // btn_Locations_Import_DTB
            // 
            this.btn_Locations_Import_DTB.Location = new System.Drawing.Point(333, 26);
            this.btn_Locations_Import_DTB.Name = "btn_Locations_Import_DTB";
            this.btn_Locations_Import_DTB.Size = new System.Drawing.Size(93, 23);
            this.btn_Locations_Import_DTB.TabIndex = 2;
            this.btn_Locations_Import_DTB.Text = "DTB (Win-Test)";
            this.btn_Locations_Import_DTB.UseVisualStyleBackColor = true;
            this.btn_Locations_Import_DTB.Click += new System.EventHandler(this.btn_Locations_Import_DTB_Click);
            // 
            // btn_Locations_Import_CALL3
            // 
            this.btn_Locations_Import_CALL3.Location = new System.Drawing.Point(239, 26);
            this.btn_Locations_Import_CALL3.Name = "btn_Locations_Import_CALL3";
            this.btn_Locations_Import_CALL3.Size = new System.Drawing.Size(88, 23);
            this.btn_Locations_Import_CALL3.TabIndex = 1;
            this.btn_Locations_Import_CALL3.Text = "CALL3 (WSJT)";
            this.btn_Locations_Import_CALL3.UseVisualStyleBackColor = true;
            this.btn_Locations_Import_CALL3.Click += new System.EventHandler(this.btn_Locations_Import_CALL3_Click);
            // 
            // btn_Locations_Import_AirScout
            // 
            this.btn_Locations_Import_AirScout.Location = new System.Drawing.Point(15, 26);
            this.btn_Locations_Import_AirScout.Name = "btn_Locations_Import_AirScout";
            this.btn_Locations_Import_AirScout.Size = new System.Drawing.Size(92, 23);
            this.btn_Locations_Import_AirScout.TabIndex = 0;
            this.btn_Locations_Import_AirScout.Text = "AirScout (TXT)";
            this.btn_Locations_Import_AirScout.UseVisualStyleBackColor = true;
            this.btn_Locations_Import_AirScout.Click += new System.EventHandler(this.btn_Locations_Import_AirScout_Click);
            // 
            // btn_Locations_Export
            // 
            this.btn_Locations_Export.Location = new System.Drawing.Point(897, 94);
            this.btn_Locations_Export.Name = "btn_Locations_Export";
            this.btn_Locations_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_Locations_Export.TabIndex = 4;
            this.btn_Locations_Export.Text = "Export";
            this.btn_Locations_Export.UseVisualStyleBackColor = true;
            this.btn_Locations_Export.Click += new System.EventHandler(this.btn_Locations_Export_Click);
            // 
            // btn_Locations_Save
            // 
            this.btn_Locations_Save.Location = new System.Drawing.Point(816, 94);
            this.btn_Locations_Save.Name = "btn_Locations_Save";
            this.btn_Locations_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Locations_Save.TabIndex = 3;
            this.btn_Locations_Save.Text = "Save";
            this.btn_Locations_Save.UseVisualStyleBackColor = true;
            this.btn_Locations_Save.Click += new System.EventHandler(this.btn_Locations_Save_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_QRZ_Stop);
            this.groupBox1.Controls.Add(this.btn_QRZ_Start);
            this.groupBox1.Location = new System.Drawing.Point(5, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "QRZ.COM Update";
            // 
            // btn_QRZ_Stop
            // 
            this.btn_QRZ_Stop.Location = new System.Drawing.Point(96, 26);
            this.btn_QRZ_Stop.Name = "btn_QRZ_Stop";
            this.btn_QRZ_Stop.Size = new System.Drawing.Size(75, 23);
            this.btn_QRZ_Stop.TabIndex = 1;
            this.btn_QRZ_Stop.Text = "Stop";
            this.btn_QRZ_Stop.UseVisualStyleBackColor = true;
            this.btn_QRZ_Stop.Click += new System.EventHandler(this.btn_QRZ_Stop_Click);
            // 
            // btn_QRZ_Start
            // 
            this.btn_QRZ_Start.Location = new System.Drawing.Point(15, 26);
            this.btn_QRZ_Start.Name = "btn_QRZ_Start";
            this.btn_QRZ_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_QRZ_Start.TabIndex = 0;
            this.btn_QRZ_Start.Text = "Start";
            this.btn_QRZ_Start.UseVisualStyleBackColor = true;
            this.btn_QRZ_Start.Click += new System.EventHandler(this.btn_QRZ_Start_Click);
            // 
            // tb_Locations_Status
            // 
            this.tb_Locations_Status.Location = new System.Drawing.Point(20, 35);
            this.tb_Locations_Status.Name = "tb_Locations_Status";
            this.tb_Locations_Status.ReadOnly = true;
            this.tb_Locations_Status.Size = new System.Drawing.Size(608, 20);
            this.tb_Locations_Status.TabIndex = 2;
            // 
            // tp_QRV
            // 
            this.tp_QRV.Controls.Add(this.dgv_QRV);
            this.tp_QRV.Controls.Add(this.panel4);
            this.tp_QRV.Location = new System.Drawing.Point(4, 22);
            this.tp_QRV.Name = "tp_QRV";
            this.tp_QRV.Size = new System.Drawing.Size(1098, 549);
            this.tp_QRV.TabIndex = 2;
            this.tp_QRV.Text = "QRV";
            this.tp_QRV.UseVisualStyleBackColor = true;
            this.tp_QRV.Enter += new System.EventHandler(this.tp_QRV_Enter);
            // 
            // dgv_QRV
            // 
            this.dgv_QRV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_QRV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_QRV.Location = new System.Drawing.Point(0, 0);
            this.dgv_QRV.Name = "dgv_QRV";
            this.dgv_QRV.Size = new System.Drawing.Size(1098, 404);
            this.dgv_QRV.TabIndex = 2;
            this.dgv_QRV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_QRV_CellEndEdit);
            this.dgv_QRV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_QRV_CellFormatting);
            this.dgv_QRV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_QRV_CellValueChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel4.Controls.Add(this.cb_QRV_ChangedOnly);
            this.panel4.Controls.Add(this.btn_QRV_Sort);
            this.panel4.Controls.Add(this.btn_QRV_Export);
            this.panel4.Controls.Add(this.btn_QRV_Save);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.tb_QRV_Callsign_Filter);
            this.panel4.Controls.Add(this.tb_QRV_Status);
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 404);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1098, 145);
            this.panel4.TabIndex = 1;
            // 
            // cb_QRV_ChangedOnly
            // 
            this.cb_QRV_ChangedOnly.AutoSize = true;
            this.cb_QRV_ChangedOnly.Location = new System.Drawing.Point(799, 38);
            this.cb_QRV_ChangedOnly.Name = "cb_QRV_ChangedOnly";
            this.cb_QRV_ChangedOnly.Size = new System.Drawing.Size(145, 17);
            this.cb_QRV_ChangedOnly.TabIndex = 10;
            this.cb_QRV_ChangedOnly.Text = "Show changed rows only";
            this.cb_QRV_ChangedOnly.UseVisualStyleBackColor = true;
            this.cb_QRV_ChangedOnly.CheckedChanged += new System.EventHandler(this.cb_QRV_ChangedOnly_CheckedChanged);
            // 
            // btn_QRV_Sort
            // 
            this.btn_QRV_Sort.Location = new System.Drawing.Point(707, 99);
            this.btn_QRV_Sort.Name = "btn_QRV_Sort";
            this.btn_QRV_Sort.Size = new System.Drawing.Size(75, 23);
            this.btn_QRV_Sort.TabIndex = 9;
            this.btn_QRV_Sort.Text = "Sort";
            this.btn_QRV_Sort.UseVisualStyleBackColor = true;
            this.btn_QRV_Sort.Click += new System.EventHandler(this.btn_QRV_Sort_Click);
            // 
            // btn_QRV_Export
            // 
            this.btn_QRV_Export.Location = new System.Drawing.Point(869, 99);
            this.btn_QRV_Export.Name = "btn_QRV_Export";
            this.btn_QRV_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_QRV_Export.TabIndex = 8;
            this.btn_QRV_Export.Text = "Export";
            this.btn_QRV_Export.UseVisualStyleBackColor = true;
            this.btn_QRV_Export.Click += new System.EventHandler(this.btn_QRV_Export_Click);
            // 
            // btn_QRV_Save
            // 
            this.btn_QRV_Save.Location = new System.Drawing.Point(788, 99);
            this.btn_QRV_Save.Name = "btn_QRV_Save";
            this.btn_QRV_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_QRV_Save.TabIndex = 7;
            this.btn_QRV_Save.Text = "Save";
            this.btn_QRV_Save.UseVisualStyleBackColor = true;
            this.btn_QRV_Save.Click += new System.EventHandler(this.btn_QRV_Save_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(628, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Callsign Filter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Status";
            // 
            // tb_QRV_Callsign_Filter
            // 
            this.tb_QRV_Callsign_Filter.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_QRV_Callsign_Filter.Location = new System.Drawing.Point(631, 38);
            this.tb_QRV_Callsign_Filter.Name = "tb_QRV_Callsign_Filter";
            this.tb_QRV_Callsign_Filter.Size = new System.Drawing.Size(100, 20);
            this.tb_QRV_Callsign_Filter.TabIndex = 2;
            this.tb_QRV_Callsign_Filter.TextChanged += new System.EventHandler(this.tb_QRV_Callsign_Filter_TextChanged);
            // 
            // tb_QRV_Status
            // 
            this.tb_QRV_Status.Location = new System.Drawing.Point(14, 38);
            this.tb_QRV_Status.Name = "tb_QRV_Status";
            this.tb_QRV_Status.ReadOnly = true;
            this.tb_QRV_Status.Size = new System.Drawing.Size(485, 20);
            this.tb_QRV_Status.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_QRV_Import_EDI);
            this.groupBox3.Controls.Add(this.btn_QRV_Import_WinTest);
            this.groupBox3.Location = new System.Drawing.Point(8, 76);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(284, 52);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Import";
            // 
            // btn_QRV_Import_EDI
            // 
            this.btn_QRV_Import_EDI.Location = new System.Drawing.Point(87, 23);
            this.btn_QRV_Import_EDI.Name = "btn_QRV_Import_EDI";
            this.btn_QRV_Import_EDI.Size = new System.Drawing.Size(75, 23);
            this.btn_QRV_Import_EDI.TabIndex = 1;
            this.btn_QRV_Import_EDI.Text = "EDI";
            this.btn_QRV_Import_EDI.UseVisualStyleBackColor = true;
            this.btn_QRV_Import_EDI.Click += new System.EventHandler(this.btn_QRV_Import_EDI_Click);
            // 
            // btn_QRV_Import_WinTest
            // 
            this.btn_QRV_Import_WinTest.Location = new System.Drawing.Point(6, 23);
            this.btn_QRV_Import_WinTest.Name = "btn_QRV_Import_WinTest";
            this.btn_QRV_Import_WinTest.Size = new System.Drawing.Size(75, 23);
            this.btn_QRV_Import_WinTest.TabIndex = 0;
            this.btn_QRV_Import_WinTest.Text = "Win-Test (QRV)";
            this.btn_QRV_Import_WinTest.UseVisualStyleBackColor = true;
            this.btn_QRV_Import_WinTest.Click += new System.EventHandler(this.btn_QRV_Import_WinTest_Click);
            // 
            // tp_Aircrafts
            // 
            this.tp_Aircrafts.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Aircrafts.Controls.Add(this.lbl_Aircrafts_UnkownHex);
            this.tp_Aircrafts.Controls.Add(this.lbl_Aircrafts_UnkownType);
            this.tp_Aircrafts.Controls.Add(this.lbl_Aircrafts_UnkownCall);
            this.tp_Aircrafts.Controls.Add(this.lbl_Aircrafts_Total);
            this.tp_Aircrafts.Controls.Add(this.label8);
            this.tp_Aircrafts.Controls.Add(this.button1);
            this.tp_Aircrafts.Controls.Add(this.btn_Update_Aircrafts_Stop);
            this.tp_Aircrafts.Controls.Add(this.btn_Update_Aicrafts_Start);
            this.tp_Aircrafts.Controls.Add(this.btn_Update_Airports);
            this.tp_Aircrafts.Controls.Add(this.btn_Update_Airlines);
            this.tp_Aircrafts.Controls.Add(this.tb_Update_Aircrafts);
            this.tp_Aircrafts.Controls.Add(this.tb_Update_Airports);
            this.tp_Aircrafts.Controls.Add(this.tb_Update_Airlines);
            this.tp_Aircrafts.Location = new System.Drawing.Point(4, 22);
            this.tp_Aircrafts.Name = "tp_Aircrafts";
            this.tp_Aircrafts.Size = new System.Drawing.Size(1098, 549);
            this.tp_Aircrafts.TabIndex = 4;
            this.tp_Aircrafts.Text = "Aircrafts";
            this.tp_Aircrafts.Enter += new System.EventHandler(this.tp_Aircrafts_Enter);
            // 
            // lbl_Aircrafts_UnkownHex
            // 
            this.lbl_Aircrafts_UnkownHex.AutoSize = true;
            this.lbl_Aircrafts_UnkownHex.Location = new System.Drawing.Point(330, 133);
            this.lbl_Aircrafts_UnkownHex.Name = "lbl_Aircrafts_UnkownHex";
            this.lbl_Aircrafts_UnkownHex.Size = new System.Drawing.Size(30, 13);
            this.lbl_Aircrafts_UnkownHex.TabIndex = 12;
            this.lbl_Aircrafts_UnkownHex.Text = "total:";
            // 
            // lbl_Aircrafts_UnkownType
            // 
            this.lbl_Aircrafts_UnkownType.AutoSize = true;
            this.lbl_Aircrafts_UnkownType.Location = new System.Drawing.Point(738, 133);
            this.lbl_Aircrafts_UnkownType.Name = "lbl_Aircrafts_UnkownType";
            this.lbl_Aircrafts_UnkownType.Size = new System.Drawing.Size(30, 13);
            this.lbl_Aircrafts_UnkownType.TabIndex = 11;
            this.lbl_Aircrafts_UnkownType.Text = "total:";
            // 
            // lbl_Aircrafts_UnkownCall
            // 
            this.lbl_Aircrafts_UnkownCall.AutoSize = true;
            this.lbl_Aircrafts_UnkownCall.Location = new System.Drawing.Point(517, 133);
            this.lbl_Aircrafts_UnkownCall.Name = "lbl_Aircrafts_UnkownCall";
            this.lbl_Aircrafts_UnkownCall.Size = new System.Drawing.Size(30, 13);
            this.lbl_Aircrafts_UnkownCall.TabIndex = 10;
            this.lbl_Aircrafts_UnkownCall.Text = "total:";
            // 
            // lbl_Aircrafts_Total
            // 
            this.lbl_Aircrafts_Total.AutoSize = true;
            this.lbl_Aircrafts_Total.Location = new System.Drawing.Point(161, 133);
            this.lbl_Aircrafts_Total.Name = "lbl_Aircrafts_Total";
            this.lbl_Aircrafts_Total.Size = new System.Drawing.Size(30, 13);
            this.lbl_Aircrafts_Total.TabIndex = 9;
            this.lbl_Aircrafts_Total.Text = "total:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 133);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Aircraft Database Statistics";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(8, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Update Aircrafts";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btn_Update_Aircrafts_Stop
            // 
            this.btn_Update_Aircrafts_Stop.Location = new System.Drawing.Point(1001, 96);
            this.btn_Update_Aircrafts_Stop.Name = "btn_Update_Aircrafts_Stop";
            this.btn_Update_Aircrafts_Stop.Size = new System.Drawing.Size(75, 23);
            this.btn_Update_Aircrafts_Stop.TabIndex = 5;
            this.btn_Update_Aircrafts_Stop.Text = "Stop";
            this.btn_Update_Aircrafts_Stop.UseVisualStyleBackColor = true;
            this.btn_Update_Aircrafts_Stop.Click += new System.EventHandler(this.btn_Update_Aircrafts_Stop_Click);
            // 
            // btn_Update_Aicrafts_Start
            // 
            this.btn_Update_Aicrafts_Start.Location = new System.Drawing.Point(906, 96);
            this.btn_Update_Aicrafts_Start.Name = "btn_Update_Aicrafts_Start";
            this.btn_Update_Aicrafts_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Update_Aicrafts_Start.TabIndex = 4;
            this.btn_Update_Aicrafts_Start.Text = "Start";
            this.btn_Update_Aicrafts_Start.UseVisualStyleBackColor = true;
            this.btn_Update_Aicrafts_Start.Click += new System.EventHandler(this.btn_Update_Aicrafts_Start_Click);
            // 
            // btn_Update_Airports
            // 
            this.btn_Update_Airports.Location = new System.Drawing.Point(8, 50);
            this.btn_Update_Airports.Name = "btn_Update_Airports";
            this.btn_Update_Airports.Size = new System.Drawing.Size(150, 23);
            this.btn_Update_Airports.TabIndex = 2;
            this.btn_Update_Airports.Text = "Update Airports";
            this.btn_Update_Airports.UseVisualStyleBackColor = true;
            this.btn_Update_Airports.Click += new System.EventHandler(this.btn_Update_Airports_Click);
            // 
            // btn_Update_Airlines
            // 
            this.btn_Update_Airlines.Location = new System.Drawing.Point(8, 21);
            this.btn_Update_Airlines.Name = "btn_Update_Airlines";
            this.btn_Update_Airlines.Size = new System.Drawing.Size(150, 23);
            this.btn_Update_Airlines.TabIndex = 0;
            this.btn_Update_Airlines.Text = "Update Airlines";
            this.btn_Update_Airlines.UseVisualStyleBackColor = true;
            this.btn_Update_Airlines.Click += new System.EventHandler(this.btn_Update_Airlines_Click);
            // 
            // tp_Upload
            // 
            this.tp_Upload.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Upload.Controls.Add(this.groupBox6);
            this.tp_Upload.Controls.Add(this.groupBox5);
            this.tp_Upload.Location = new System.Drawing.Point(4, 22);
            this.tp_Upload.Name = "tp_Upload";
            this.tp_Upload.Size = new System.Drawing.Size(1098, 549);
            this.tp_Upload.TabIndex = 5;
            this.tp_Upload.Text = "Upload";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tb_StationDatabase_RemoteHost);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.tb_StationDatabase_Export_Password);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.tb_StationDatabase_Export_User);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.btn_StationDatabase_Export);
            this.groupBox5.Controls.Add(this.tb_StationDatabase_RemoteDir);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.tb_StationDatabase_LocalDir);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Location = new System.Drawing.Point(8, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(881, 189);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Station Database";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 13);
            this.label13.TabIndex = 9;
            this.label13.Text = "Remote Host:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 127);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "Password:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "User:";
            // 
            // btn_StationDatabase_Export
            // 
            this.btn_StationDatabase_Export.Location = new System.Drawing.Point(107, 150);
            this.btn_StationDatabase_Export.Name = "btn_StationDatabase_Export";
            this.btn_StationDatabase_Export.Size = new System.Drawing.Size(178, 23);
            this.btn_StationDatabase_Export.TabIndex = 4;
            this.btn_StationDatabase_Export.Text = "Export to JSON and Upload";
            this.btn_StationDatabase_Export.UseVisualStyleBackColor = true;
            this.btn_StationDatabase_Export.Click += new System.EventHandler(this.btn_StationDatabase_Export_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Remote Directory:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Local Directory:";
            // 
            // tp_Extras
            // 
            this.tp_Extras.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Extras.Controls.Add(this.groupBox4);
            this.tp_Extras.Location = new System.Drawing.Point(4, 22);
            this.tp_Extras.Name = "tp_Extras";
            this.tp_Extras.Size = new System.Drawing.Size(1098, 549);
            this.tp_Extras.TabIndex = 3;
            this.tp_Extras.Text = "Extras";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_SFTP_GenarateFile);
            this.groupBox4.Controls.Add(this.tb_SFTP_Password);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.tb_SFTP_User);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.tb_SFTP_URL);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(8, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(778, 111);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SFTP Password File";
            // 
            // btn_SFTP_GenarateFile
            // 
            this.btn_SFTP_GenarateFile.Location = new System.Drawing.Point(423, 23);
            this.btn_SFTP_GenarateFile.Name = "btn_SFTP_GenarateFile";
            this.btn_SFTP_GenarateFile.Size = new System.Drawing.Size(252, 72);
            this.btn_SFTP_GenarateFile.TabIndex = 6;
            this.btn_SFTP_GenarateFile.Text = "Generate File";
            this.btn_SFTP_GenarateFile.UseVisualStyleBackColor = true;
            this.btn_SFTP_GenarateFile.Click += new System.EventHandler(this.btn_SFTP_GenerateFile_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "User:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "URL:";
            // 
            // bw_QRZ
            // 
            this.bw_QRZ.WorkerReportsProgress = true;
            this.bw_QRZ.WorkerSupportsCancellation = true;
            this.bw_QRZ.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_QRZ_DoWork);
            this.bw_QRZ.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_QRZ_ProgressChanged);
            this.bw_QRZ.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_QRZ_RunWorkerCompleted);
            // 
            // bw_DatabaseUpdater
            // 
            this.bw_DatabaseUpdater.WorkerReportsProgress = true;
            this.bw_DatabaseUpdater.WorkerSupportsCancellation = true;
            this.bw_DatabaseUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DatabaseUpdater_DoWork);
            this.bw_DatabaseUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_DatabaseUpdater_ProgressChanged);
            this.bw_DatabaseUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_DatabaseUpdater_RunWorkerCompleted);
            // 
            // bw_AircraftUpdater
            // 
            this.bw_AircraftUpdater.WorkerReportsProgress = true;
            this.bw_AircraftUpdater.WorkerSupportsCancellation = true;
            this.bw_AircraftUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_AircraftUpdater_DoWork);
            this.bw_AircraftUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_AircraftUpdater_ProgressChanged);
            this.bw_AircraftUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_AircraftUpdater_RunWorkerCompleted);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tb_AircraftDatabase_RemoteHost);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.tb_AircraftDatabase_Password);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.tb_AircraftDatabase_User);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.btn_AircraftDatabase_Export);
            this.groupBox6.Controls.Add(this.tb_AircraftDatabase_RemoteDir);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.tb_AircraftDabase_LocalDir);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Location = new System.Drawing.Point(8, 210);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(881, 189);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Aicraft Database";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Remote Host:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 127);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Password:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 101);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "User:";
            // 
            // btn_AircraftDatabase_Export
            // 
            this.btn_AircraftDatabase_Export.Location = new System.Drawing.Point(107, 150);
            this.btn_AircraftDatabase_Export.Name = "btn_AircraftDatabase_Export";
            this.btn_AircraftDatabase_Export.Size = new System.Drawing.Size(178, 23);
            this.btn_AircraftDatabase_Export.TabIndex = 4;
            this.btn_AircraftDatabase_Export.Text = "Export to JSON and Upload";
            this.btn_AircraftDatabase_Export.UseVisualStyleBackColor = true;
            this.btn_AircraftDatabase_Export.Click += new System.EventHandler(this.btn_AircraftDatabase_Export_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 74);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(92, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "Remote Directory:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 22);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(81, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Local Directory:";
            // 
            // tb_Coverage_MaxLat
            // 
            this.tb_Coverage_MaxLat.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutDatabaseManager.Properties.Settings.Default, "MaxLat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Coverage_MaxLat.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Coverage_MaxLat.FormatSpecifier = "F0";
            this.tb_Coverage_MaxLat.Location = new System.Drawing.Point(105, 104);
            this.tb_Coverage_MaxLat.MaxValue = 90D;
            this.tb_Coverage_MaxLat.MinValue = -90D;
            this.tb_Coverage_MaxLat.Name = "tb_Coverage_MaxLat";
            this.tb_Coverage_MaxLat.Size = new System.Drawing.Size(50, 22);
            this.tb_Coverage_MaxLat.TabIndex = 28;
            this.tb_Coverage_MaxLat.Text = "60";
            this.tb_Coverage_MaxLat.Value = global::AirScoutDatabaseManager.Properties.Settings.Default.MaxLat;
            this.tb_Coverage_MaxLat.TextChanged += new System.EventHandler(this.tp_General_Update);
            // 
            // tb_Coverage_MinLat
            // 
            this.tb_Coverage_MinLat.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutDatabaseManager.Properties.Settings.Default, "MinLat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Coverage_MinLat.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Coverage_MinLat.FormatSpecifier = "F0";
            this.tb_Coverage_MinLat.Location = new System.Drawing.Point(105, 79);
            this.tb_Coverage_MinLat.MaxValue = 90D;
            this.tb_Coverage_MinLat.MinValue = -90D;
            this.tb_Coverage_MinLat.Name = "tb_Coverage_MinLat";
            this.tb_Coverage_MinLat.Size = new System.Drawing.Size(50, 22);
            this.tb_Coverage_MinLat.TabIndex = 27;
            this.tb_Coverage_MinLat.Text = "35";
            this.tb_Coverage_MinLat.Value = global::AirScoutDatabaseManager.Properties.Settings.Default.MinLat;
            this.tb_Coverage_MinLat.TextChanged += new System.EventHandler(this.tp_General_Update);
            // 
            // tb_Coverage_MaxLon
            // 
            this.tb_Coverage_MaxLon.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutDatabaseManager.Properties.Settings.Default, "MaxLon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Coverage_MaxLon.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Coverage_MaxLon.FormatSpecifier = "F0";
            this.tb_Coverage_MaxLon.Location = new System.Drawing.Point(105, 52);
            this.tb_Coverage_MaxLon.MaxValue = 180D;
            this.tb_Coverage_MaxLon.MinValue = -180D;
            this.tb_Coverage_MaxLon.Name = "tb_Coverage_MaxLon";
            this.tb_Coverage_MaxLon.Size = new System.Drawing.Size(50, 22);
            this.tb_Coverage_MaxLon.TabIndex = 26;
            this.tb_Coverage_MaxLon.Text = "30";
            this.tb_Coverage_MaxLon.Value = global::AirScoutDatabaseManager.Properties.Settings.Default.MaxLon;
            this.tb_Coverage_MaxLon.TextChanged += new System.EventHandler(this.tp_General_Update);
            // 
            // tb_Coverage_MinLon
            // 
            this.tb_Coverage_MinLon.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutDatabaseManager.Properties.Settings.Default, "MinLon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Coverage_MinLon.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Coverage_MinLon.FormatSpecifier = "F0";
            this.tb_Coverage_MinLon.Location = new System.Drawing.Point(105, 25);
            this.tb_Coverage_MinLon.MaxValue = 180D;
            this.tb_Coverage_MinLon.MinValue = -180D;
            this.tb_Coverage_MinLon.Name = "tb_Coverage_MinLon";
            this.tb_Coverage_MinLon.Size = new System.Drawing.Size(50, 22);
            this.tb_Coverage_MinLon.TabIndex = 25;
            this.tb_Coverage_MinLon.Text = "-15";
            this.tb_Coverage_MinLon.Value = global::AirScoutDatabaseManager.Properties.Settings.Default.MinLon;
            this.tb_Coverage_MinLon.TextChanged += new System.EventHandler(this.tp_General_Update);
            // 
            // tb_Update_Aircrafts
            // 
            this.tb_Update_Aircrafts.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "Aircrafts_BaseURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Update_Aircrafts.Location = new System.Drawing.Point(164, 98);
            this.tb_Update_Aircrafts.Name = "tb_Update_Aircrafts";
            this.tb_Update_Aircrafts.Size = new System.Drawing.Size(722, 20);
            this.tb_Update_Aircrafts.TabIndex = 7;
            this.tb_Update_Aircrafts.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.Aircrafts_BaseURL;
            // 
            // tb_Update_Airports
            // 
            this.tb_Update_Airports.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "Airports_Update_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Update_Airports.Location = new System.Drawing.Point(164, 53);
            this.tb_Update_Airports.Name = "tb_Update_Airports";
            this.tb_Update_Airports.Size = new System.Drawing.Size(722, 20);
            this.tb_Update_Airports.TabIndex = 3;
            this.tb_Update_Airports.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.Airports_Update_URL;
            // 
            // tb_Update_Airlines
            // 
            this.tb_Update_Airlines.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "Airlines_Update_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Update_Airlines.Location = new System.Drawing.Point(164, 24);
            this.tb_Update_Airlines.Name = "tb_Update_Airlines";
            this.tb_Update_Airlines.Size = new System.Drawing.Size(722, 20);
            this.tb_Update_Airlines.TabIndex = 1;
            this.tb_Update_Airlines.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.Airlines_Update_URL;
            // 
            // tb_AircraftDatabase_RemoteHost
            // 
            this.tb_AircraftDatabase_RemoteHost.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "AircraftDatabase_Export_RemoteHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_AircraftDatabase_RemoteHost.Location = new System.Drawing.Point(107, 45);
            this.tb_AircraftDatabase_RemoteHost.Name = "tb_AircraftDatabase_RemoteHost";
            this.tb_AircraftDatabase_RemoteHost.Size = new System.Drawing.Size(543, 20);
            this.tb_AircraftDatabase_RemoteHost.TabIndex = 10;
            this.tb_AircraftDatabase_RemoteHost.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.AircraftDatabase_Export_RemoteHost;
            // 
            // tb_AircraftDatabase_Password
            // 
            this.tb_AircraftDatabase_Password.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "AircraftDatabase_Export_Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_AircraftDatabase_Password.Location = new System.Drawing.Point(107, 124);
            this.tb_AircraftDatabase_Password.Name = "tb_AircraftDatabase_Password";
            this.tb_AircraftDatabase_Password.Size = new System.Drawing.Size(107, 20);
            this.tb_AircraftDatabase_Password.TabIndex = 8;
            this.tb_AircraftDatabase_Password.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.AircraftDatabase_Export_Password;
            // 
            // tb_AircraftDatabase_User
            // 
            this.tb_AircraftDatabase_User.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "AircraftDatabase_Export_User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_AircraftDatabase_User.Location = new System.Drawing.Point(107, 98);
            this.tb_AircraftDatabase_User.Name = "tb_AircraftDatabase_User";
            this.tb_AircraftDatabase_User.Size = new System.Drawing.Size(107, 20);
            this.tb_AircraftDatabase_User.TabIndex = 6;
            this.tb_AircraftDatabase_User.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.AircraftDatabase_Export_User;
            // 
            // tb_AircraftDatabase_RemoteDir
            // 
            this.tb_AircraftDatabase_RemoteDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "AircraftDatabase_Export_RemoteDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_AircraftDatabase_RemoteDir.Location = new System.Drawing.Point(107, 71);
            this.tb_AircraftDatabase_RemoteDir.Name = "tb_AircraftDatabase_RemoteDir";
            this.tb_AircraftDatabase_RemoteDir.Size = new System.Drawing.Size(543, 20);
            this.tb_AircraftDatabase_RemoteDir.TabIndex = 3;
            this.tb_AircraftDatabase_RemoteDir.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.AircraftDatabase_Export_RemoteDir;
            // 
            // tb_AircraftDabase_LocalDir
            // 
            this.tb_AircraftDabase_LocalDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "AircraftDatabase_Export_LocalDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_AircraftDabase_LocalDir.Location = new System.Drawing.Point(107, 19);
            this.tb_AircraftDabase_LocalDir.Name = "tb_AircraftDabase_LocalDir";
            this.tb_AircraftDabase_LocalDir.Size = new System.Drawing.Size(543, 20);
            this.tb_AircraftDabase_LocalDir.TabIndex = 1;
            this.tb_AircraftDabase_LocalDir.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.AircraftDatabase_Export_LocalDir;
            // 
            // tb_StationDatabase_RemoteHost
            // 
            this.tb_StationDatabase_RemoteHost.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "StationDatabase_Export_RemoteHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_StationDatabase_RemoteHost.Location = new System.Drawing.Point(107, 45);
            this.tb_StationDatabase_RemoteHost.Name = "tb_StationDatabase_RemoteHost";
            this.tb_StationDatabase_RemoteHost.Size = new System.Drawing.Size(543, 20);
            this.tb_StationDatabase_RemoteHost.TabIndex = 10;
            this.tb_StationDatabase_RemoteHost.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.StationDatabase_Export_RemoteHost;
            // 
            // tb_StationDatabase_Export_Password
            // 
            this.tb_StationDatabase_Export_Password.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "StationDatabase_Export_Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_StationDatabase_Export_Password.Location = new System.Drawing.Point(107, 124);
            this.tb_StationDatabase_Export_Password.Name = "tb_StationDatabase_Export_Password";
            this.tb_StationDatabase_Export_Password.Size = new System.Drawing.Size(107, 20);
            this.tb_StationDatabase_Export_Password.TabIndex = 8;
            this.tb_StationDatabase_Export_Password.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.StationDatabase_Export_Password;
            // 
            // tb_StationDatabase_Export_User
            // 
            this.tb_StationDatabase_Export_User.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "StationDatabase_Export_User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_StationDatabase_Export_User.Location = new System.Drawing.Point(107, 98);
            this.tb_StationDatabase_Export_User.Name = "tb_StationDatabase_Export_User";
            this.tb_StationDatabase_Export_User.Size = new System.Drawing.Size(107, 20);
            this.tb_StationDatabase_Export_User.TabIndex = 6;
            this.tb_StationDatabase_Export_User.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.StationDatabase_Export_User;
            // 
            // tb_StationDatabase_RemoteDir
            // 
            this.tb_StationDatabase_RemoteDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "StationDatabase_Export_RemoteDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_StationDatabase_RemoteDir.Location = new System.Drawing.Point(107, 71);
            this.tb_StationDatabase_RemoteDir.Name = "tb_StationDatabase_RemoteDir";
            this.tb_StationDatabase_RemoteDir.Size = new System.Drawing.Size(543, 20);
            this.tb_StationDatabase_RemoteDir.TabIndex = 3;
            this.tb_StationDatabase_RemoteDir.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.StationDatabase_Export_RemoteDir;
            // 
            // tb_StationDatabase_LocalDir
            // 
            this.tb_StationDatabase_LocalDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "StationDatabase_Export_LocalDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_StationDatabase_LocalDir.Location = new System.Drawing.Point(107, 19);
            this.tb_StationDatabase_LocalDir.Name = "tb_StationDatabase_LocalDir";
            this.tb_StationDatabase_LocalDir.Size = new System.Drawing.Size(543, 20);
            this.tb_StationDatabase_LocalDir.TabIndex = 1;
            this.tb_StationDatabase_LocalDir.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.StationDatabase_Export_LocalDir;
            // 
            // tb_SFTP_Password
            // 
            this.tb_SFTP_Password.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "SFTP_Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_SFTP_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_SFTP_Password.Location = new System.Drawing.Point(71, 75);
            this.tb_SFTP_Password.Name = "tb_SFTP_Password";
            this.tb_SFTP_Password.Size = new System.Drawing.Size(318, 20);
            this.tb_SFTP_Password.TabIndex = 5;
            this.tb_SFTP_Password.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.SFTP_Password;
            // 
            // tb_SFTP_User
            // 
            this.tb_SFTP_User.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "SFTP_User", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_SFTP_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_SFTP_User.Location = new System.Drawing.Point(71, 49);
            this.tb_SFTP_User.Name = "tb_SFTP_User";
            this.tb_SFTP_User.Size = new System.Drawing.Size(318, 20);
            this.tb_SFTP_User.TabIndex = 3;
            this.tb_SFTP_User.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.SFTP_User;
            // 
            // tb_SFTP_URL
            // 
            this.tb_SFTP_URL.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutDatabaseManager.Properties.Settings.Default, "SFTP_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_SFTP_URL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_SFTP_URL.Location = new System.Drawing.Point(71, 23);
            this.tb_SFTP_URL.Name = "tb_SFTP_URL";
            this.tb_SFTP_URL.Size = new System.Drawing.Size(318, 20);
            this.tb_SFTP_URL.TabIndex = 1;
            this.tb_SFTP_URL.Text = global::AirScoutDatabaseManager.Properties.Settings.Default.SFTP_URL;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 597);
            this.Controls.Add(this.tc_Main);
            this.Controls.Add(this.ss_Main);
            this.Name = "MainDlg";
            this.Text = "AirScout Database Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.Load += new System.EventHandler(this.MainDlg_Load);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.tc_Main.ResumeLayout(false);
            this.tp_General.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tp_Locations.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Locations)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tp_QRV.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_QRV)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tp_Aircrafts.ResumeLayout(false);
            this.tp_Aircrafts.PerformLayout();
            this.tp_Upload.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tp_Extras.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.TabControl tc_Main;
        private System.Windows.Forms.TabPage tp_General;
        private System.Windows.Forms.TabPage tp_Locations;
        private System.Windows.Forms.Panel panel1;
        private GMap.NET.WindowsForms.GMapControl gm_Coverage;
        private ScoutBase.Core.DoubleTextBox tb_Coverage_MaxLat;
        private ScoutBase.Core.DoubleTextBox tb_Coverage_MinLat;
        private ScoutBase.Core.DoubleTextBox tb_Coverage_MaxLon;
        private ScoutBase.Core.DoubleTextBox tb_Coverage_MinLon;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv_Locations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_Locations_Status;
        private System.Windows.Forms.Button btn_QRZ_Stop;
        private System.Windows.Forms.Button btn_QRZ_Start;
        private System.ComponentModel.BackgroundWorker bw_QRZ;
        private System.Windows.Forms.Button btn_Locations_Export;
        private System.Windows.Forms.Button btn_Locations_Save;
        private System.ComponentModel.BackgroundWorker bw_DatabaseUpdater;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_Locations_Import_CALL3;
        private System.Windows.Forms.Button btn_Locations_Sort;
        private System.Windows.Forms.Button btn_Locations_Import_DTB;
        private System.Windows.Forms.Button btn_Locations_Import_CSV;
        private System.Windows.Forms.CheckBox cb_Locations_ChangedOnly;
        private GMap.NET.WindowsForms.GMapControl gm_Locations;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tb_Locations_Callsign_Filter;
        private System.Windows.Forms.Button btn_Locations_Import_USR;
        private System.Windows.Forms.Button btn_Locations_Import_AirScout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tp_QRV;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_QRV_Import_WinTest;
        private System.Windows.Forms.DataGridView dgv_QRV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_QRV_Callsign_Filter;
        private System.Windows.Forms.TextBox tb_QRV_Status;
        private System.Windows.Forms.Button btn_QRV_Sort;
        private System.Windows.Forms.Button btn_QRV_Export;
        private System.Windows.Forms.Button btn_QRV_Save;
        private System.Windows.Forms.CheckBox cb_QRV_ChangedOnly;
        private System.Windows.Forms.Button btn_QRV_Import_EDI;
        private System.Windows.Forms.TabPage tp_Extras;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_SFTP_GenarateFile;
        private System.Windows.Forms.TextBox tb_SFTP_Password;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_SFTP_User;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_SFTP_URL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tp_Aircrafts;
        private System.Windows.Forms.TextBox tb_Update_Airlines;
        private System.Windows.Forms.Button btn_Update_Airlines;
        private System.Windows.Forms.TextBox tb_Update_Airports;
        private System.Windows.Forms.Button btn_Update_Airports;
        private System.ComponentModel.BackgroundWorker bw_AircraftUpdater;
        private System.Windows.Forms.Button btn_Update_Aircrafts_Stop;
        private System.Windows.Forms.Button btn_Update_Aicrafts_Start;
        private System.Windows.Forms.TextBox tb_Update_Aircrafts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_Aircrafts_UnkownHex;
        private System.Windows.Forms.Label lbl_Aircrafts_UnkownType;
        private System.Windows.Forms.Label lbl_Aircrafts_UnkownCall;
        private System.Windows.Forms.Label lbl_Aircrafts_Total;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tp_Upload;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_StationDatabase_Export;
        private System.Windows.Forms.TextBox tb_StationDatabase_RemoteDir;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_StationDatabase_LocalDir;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_StationDatabase_Export_User;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_StationDatabase_Export_Password;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb_StationDatabase_RemoteHost;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tb_AircraftDatabase_RemoteHost;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_AircraftDatabase_Password;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tb_AircraftDatabase_User;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btn_AircraftDatabase_Export;
        private System.Windows.Forms.TextBox tb_AircraftDatabase_RemoteDir;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tb_AircraftDabase_LocalDir;
        private System.Windows.Forms.Label label18;
    }
}

