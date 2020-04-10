namespace AirScout
{
    partial class MapDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapDlg));
            ScoutBase.Core.LatLon.GPoint gPoint1 = new ScoutBase.Core.LatLon.GPoint();
            ScoutBase.Core.LatLon.GPoint gPoint2 = new ScoutBase.Core.LatLon.GPoint();
            this.il_Main = new System.Windows.Forms.ImageList(this.components);
            this.ti_Progress = new System.Windows.Forms.Timer(this.components);
            this.sc_Map = new System.Windows.Forms.SplitContainer();
            this.tc_Map = new System.Windows.Forms.TabControl();
            this.tp_Map = new System.Windows.Forms.TabPage();
            this.ag_Azimuth = new AquaControls.AquaGauge();
            this.ag_Elevation = new AquaControls.AquaGauge();
            this.gm_Main = new GMap.NET.WindowsForms.GMapControl();
            this.tp_News = new System.Windows.Forms.TabPage();
            this.tc_Main = new System.Windows.Forms.TabControl();
            this.tp_Elevation = new System.Windows.Forms.TabPage();
            this.tp_Spectrum = new System.Windows.Forms.TabPage();
            this.gb_Spectrum_NearestInfo = new System.Windows.Forms.GroupBox();
            this.lbl_Nearest_Dist = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lbl_Nearest_Call = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lbl_Nearest_Angle = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_Nearest_Alt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_Nearest_Cat = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_Nearest_Type = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gb_Spectrum_Status = new System.Windows.Forms.GroupBox();
            this.tb_Spectrum_Status = new System.Windows.Forms.TextBox();
            this.gb_NearestPlaneMap = new System.Windows.Forms.GroupBox();
            this.gm_Nearest = new GMap.NET.WindowsForms.GMapControl();
            this.gb_Spectrum = new System.Windows.Forms.GroupBox();
            this.tp_Analysis = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gb_Analysis_Player = new System.Windows.Forms.GroupBox();
            this.dtp_Analysis_MaxValue = new System.Windows.Forms.DateTimePicker();
            this.dtp_Analysis_MinValue = new System.Windows.Forms.DateTimePicker();
            this.sb_Analysis_Play = new CustomScrollBar.ScrollBarEx();
            this.gb_Analysis_Controls = new System.Windows.Forms.GroupBox();
            this.btn_Analysis_FastForward = new System.Windows.Forms.Button();
            this.btn_Analysis_Forward = new System.Windows.Forms.Button();
            this.btn_Analysis_Pause = new System.Windows.Forms.Button();
            this.btn_Analysis_Back = new System.Windows.Forms.Button();
            this.btn_Analysis_Rewind = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gb_Analysis_Functions = new System.Windows.Forms.GroupBox();
            this.btn_Analysis_Plane_History = new System.Windows.Forms.Button();
            this.btn_Analysis_CrossingHistory = new System.Windows.Forms.Button();
            this.btn_Analysis_Path_SaveToFile = new System.Windows.Forms.Button();
            this.btn_Analysis_Planes_ShowTraffic = new System.Windows.Forms.Button();
            this.gb_Analysis_Database = new System.Windows.Forms.GroupBox();
            this.tb_Analysis_Status = new System.Windows.Forms.TextBox();
            this.btn_Analysis_Planes_History = new System.Windows.Forms.Button();
            this.btn_Analysis_Planes_Clear = new System.Windows.Forms.Button();
            this.btn_Analysis_Planes_Save = new System.Windows.Forms.Button();
            this.btn_Analysis_Planes_Load = new System.Windows.Forms.Button();
            this.gb_Analysis_Mode = new System.Windows.Forms.GroupBox();
            this.btn_Analysis_OFF = new System.Windows.Forms.Button();
            this.btn_Analysis_ON = new System.Windows.Forms.Button();
            this.gb_Analysis_Time = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_Analysis_Stepwidth = new System.Windows.Forms.TextBox();
            this.tb_Analysis_Time = new System.Windows.Forms.TextBox();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Dummy = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Calculations = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_Aircraft = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_Stations = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_GLOBE = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_SRTM3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_SRTM1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tt_Main = new System.Windows.Forms.ToolTip(this.components);
            this.btn_Map_PlayPause = new System.Windows.Forms.Button();
            this.btn_Map_Save = new System.Windows.Forms.Button();
            this.btn_Options = new System.Windows.Forms.Button();
            this.cb_Band = new System.Windows.Forms.ComboBox();
            this.tb_QTF = new System.Windows.Forms.TextBox();
            this.tb_QRB = new System.Windows.Forms.TextBox();
            this.gb_Map_Alarms = new System.Windows.Forms.GroupBox();
            this.cb_Alarms_Activate = new System.Windows.Forms.CheckBox();
            this.gb_Map_Zoom = new System.Windows.Forms.GroupBox();
            this.pa_Map_Zoom = new System.Windows.Forms.Panel();
            this.cb_AutoCenter = new System.Windows.Forms.CheckBox();
            this.tb_Zoom = new System.Windows.Forms.TextBox();
            this.btn_Zoom_Out = new System.Windows.Forms.Button();
            this.btn_Zoom_In = new System.Windows.Forms.Button();
            this.gb_Map_Filter = new System.Windows.Forms.GroupBox();
            this.pa_Planes_Filter = new System.Windows.Forms.Panel();
            this.tb_Planes_Filter_Min_Alt = new ScoutBase.Core.Int32TextBox();
            this.tb_Planes_Filter_Max_Circumcircle = new ScoutBase.Core.Int32TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cb_Planes_Filter_Min_Cat = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.il_Sat = new System.Windows.Forms.ImageList(this.components);
            this.bw_WinTestReceive = new System.ComponentModel.BackgroundWorker();
            this.il_Planes_M = new System.Windows.Forms.ImageList(this.components);
            this.bw_SpecLab_Receive = new System.ComponentModel.BackgroundWorker();
            this.sc_Main = new System.Windows.Forms.SplitContainer();
            this.tc_Control = new System.Windows.Forms.TabControl();
            this.tp_Control_Single = new System.Windows.Forms.TabPage();
            this.gb_Map_Info = new System.Windows.Forms.GroupBox();
            this.cb_DXCall = new ScoutBase.Core.CallsignComboBox();
            this.cb_DXLoc = new ScoutBase.Core.LocatorComboBox();
            this.cb_MyLoc = new ScoutBase.Core.LocatorComboBox();
            this.cb_MyCall = new ScoutBase.Core.CallsignComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.tp_Control_Multi = new System.Windows.Forms.TabPage();
            this.lv_Control_Watchlist = new System.Windows.Forms.ListView();
            this.ch_Control_Watchlist_Call = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_Control_Watchlist_Loc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Control_Manage_Watchlist = new System.Windows.Forms.Button();
            this.tp_Control_Options = new System.Windows.Forms.TabPage();
            this.pa_CommonInfo = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_UTC = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gb_Map_Buttons = new System.Windows.Forms.GroupBox();
            this.bw_Track = new System.ComponentModel.BackgroundWorker();
            this.il_Planes_H = new System.Windows.Forms.ImageList(this.components);
            this.il_Planes_L = new System.Windows.Forms.ImageList(this.components);
            this.il_Planes_S = new System.Windows.Forms.ImageList(this.components);
            this.bw_Webserver = new System.ComponentModel.BackgroundWorker();
            this.bw_JSONWriter = new System.ComponentModel.BackgroundWorker();
            this.bw_NewsFeed = new System.ComponentModel.BackgroundWorker();
            this.il_Airports = new System.Windows.Forms.ImageList(this.components);
            this.bw_HistoryDownloader = new System.ComponentModel.BackgroundWorker();
            this.ti_Startup = new System.Windows.Forms.Timer(this.components);
            this.ti_ShowLegends = new System.Windows.Forms.Timer(this.components);
            this.tt_Control_Watchlist = new System.Windows.Forms.ToolTip(this.components);
            this.bw_Analysis_DataGetter = new System.ComponentModel.BackgroundWorker();
            this.bw_Analysis_FileSaver = new System.ComponentModel.BackgroundWorker();
            this.bw_Analysis_FileLoader = new System.ComponentModel.BackgroundWorker();
            this.bw_AirportMapper = new System.ComponentModel.BackgroundWorker();
            this.gm_Cache = new GMap.NET.WindowsForms.GMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.sc_Map)).BeginInit();
            this.sc_Map.Panel1.SuspendLayout();
            this.sc_Map.Panel2.SuspendLayout();
            this.sc_Map.SuspendLayout();
            this.tc_Map.SuspendLayout();
            this.tp_Map.SuspendLayout();
            this.tc_Main.SuspendLayout();
            this.tp_Spectrum.SuspendLayout();
            this.gb_Spectrum_NearestInfo.SuspendLayout();
            this.gb_Spectrum_Status.SuspendLayout();
            this.gb_NearestPlaneMap.SuspendLayout();
            this.tp_Analysis.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gb_Analysis_Player.SuspendLayout();
            this.gb_Analysis_Controls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gb_Analysis_Functions.SuspendLayout();
            this.gb_Analysis_Database.SuspendLayout();
            this.gb_Analysis_Mode.SuspendLayout();
            this.gb_Analysis_Time.SuspendLayout();
            this.ss_Main.SuspendLayout();
            this.gb_Map_Alarms.SuspendLayout();
            this.gb_Map_Zoom.SuspendLayout();
            this.pa_Map_Zoom.SuspendLayout();
            this.gb_Map_Filter.SuspendLayout();
            this.pa_Planes_Filter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sc_Main)).BeginInit();
            this.sc_Main.Panel1.SuspendLayout();
            this.sc_Main.Panel2.SuspendLayout();
            this.sc_Main.SuspendLayout();
            this.tc_Control.SuspendLayout();
            this.tp_Control_Single.SuspendLayout();
            this.gb_Map_Info.SuspendLayout();
            this.tp_Control_Multi.SuspendLayout();
            this.tp_Control_Options.SuspendLayout();
            this.pa_CommonInfo.SuspendLayout();
            this.gb_Map_Buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // il_Main
            // 
            this.il_Main.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_Main.ImageStream")));
            this.il_Main.TransparentColor = System.Drawing.Color.Transparent;
            this.il_Main.Images.SetKeyName(0, "PauseHS.png");
            this.il_Main.Images.SetKeyName(1, "PlayHS.png");
            this.il_Main.Images.SetKeyName(2, "RecordHS.png");
            // 
            // ti_Progress
            // 
            this.ti_Progress.Enabled = true;
            this.ti_Progress.Interval = 1000;
            this.ti_Progress.Tick += new System.EventHandler(this.ti_Progress_Tick);
            // 
            // sc_Map
            // 
            this.sc_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_Map.Location = new System.Drawing.Point(0, 0);
            this.sc_Map.Name = "sc_Map";
            this.sc_Map.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sc_Map.Panel1
            // 
            this.sc_Map.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.sc_Map.Panel1.Controls.Add(this.tc_Map);
            // 
            // sc_Map.Panel2
            // 
            this.sc_Map.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.sc_Map.Panel2.Controls.Add(this.tc_Main);
            this.sc_Map.Size = new System.Drawing.Size(852, 706);
            this.sc_Map.SplitterDistance = 341;
            this.sc_Map.SplitterWidth = 5;
            this.sc_Map.TabIndex = 20;
            this.sc_Map.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.sc_Map_SplitterMoved);
            // 
            // tc_Map
            // 
            this.tc_Map.Controls.Add(this.tp_Map);
            this.tc_Map.Controls.Add(this.tp_News);
            this.tc_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_Map.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tc_Map.Location = new System.Drawing.Point(0, 0);
            this.tc_Map.Name = "tc_Map";
            this.tc_Map.SelectedIndex = 0;
            this.tc_Map.Size = new System.Drawing.Size(852, 341);
            this.tc_Map.TabIndex = 14;
            this.tc_Map.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tc_Map_Selecting);
            // 
            // tp_Map
            // 
            this.tp_Map.Controls.Add(this.ag_Azimuth);
            this.tp_Map.Controls.Add(this.ag_Elevation);
            this.tp_Map.Controls.Add(this.gm_Main);
            this.tp_Map.Controls.Add(this.gm_Cache);
            this.tp_Map.Location = new System.Drawing.Point(4, 22);
            this.tp_Map.Name = "tp_Map";
            this.tp_Map.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Map.Size = new System.Drawing.Size(844, 315);
            this.tp_Map.TabIndex = 0;
            this.tp_Map.Text = "Map";
            this.tp_Map.UseVisualStyleBackColor = true;
            this.tp_Map.Enter += new System.EventHandler(this.tp_Map_Enter);
            // 
            // ag_Azimuth
            // 
            this.ag_Azimuth.BackColor = System.Drawing.Color.Transparent;
            this.ag_Azimuth.DialColor = System.Drawing.Color.SlateGray;
            this.ag_Azimuth.DialText = "Azimuth";
            this.ag_Azimuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ag_Azimuth.ForeColor = System.Drawing.Color.Black;
            this.ag_Azimuth.Glossiness = 50F;
            this.ag_Azimuth.Location = new System.Drawing.Point(489, 272);
            this.ag_Azimuth.MaxValue = 360F;
            this.ag_Azimuth.MinValue = 0F;
            this.ag_Azimuth.Name = "ag_Azimuth";
            this.ag_Azimuth.NoOfDivisions = 12;
            this.ag_Azimuth.RecommendedValue = 0F;
            this.ag_Azimuth.Size = new System.Drawing.Size(175, 175);
            this.ag_Azimuth.TabIndex = 27;
            this.ag_Azimuth.ThresholdPercent = 0F;
            this.ag_Azimuth.Value = 0F;
            this.ag_Azimuth.Visible = false;
            // 
            // ag_Elevation
            // 
            this.ag_Elevation.BackColor = System.Drawing.Color.Transparent;
            this.ag_Elevation.DialColor = System.Drawing.Color.SlateGray;
            this.ag_Elevation.DialText = "Elevation";
            this.ag_Elevation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ag_Elevation.ForeColor = System.Drawing.Color.Black;
            this.ag_Elevation.Glossiness = 50F;
            this.ag_Elevation.Location = new System.Drawing.Point(670, 273);
            this.ag_Elevation.MaxValue = 90F;
            this.ag_Elevation.MinValue = 0F;
            this.ag_Elevation.Name = "ag_Elevation";
            this.ag_Elevation.NoOfDivisions = 4;
            this.ag_Elevation.RecommendedValue = 0F;
            this.ag_Elevation.Size = new System.Drawing.Size(175, 175);
            this.ag_Elevation.TabIndex = 28;
            this.ag_Elevation.ThresholdPercent = 0F;
            this.ag_Elevation.Value = 0F;
            this.ag_Elevation.Visible = false;
            // 
            // gm_Main
            // 
            this.gm_Main.Bearing = 0F;
            this.gm_Main.CanDragMap = true;
            this.gm_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Main.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Main.GrayScaleMode = false;
            this.gm_Main.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Main.LevelsKeepInMemmory = 10;
            this.gm_Main.Location = new System.Drawing.Point(3, 3);
            this.gm_Main.MarkersEnabled = true;
            this.gm_Main.MaxZoom = 2;
            this.gm_Main.MinZoom = 2;
            this.gm_Main.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Main.Name = "gm_Main";
            this.gm_Main.NegativeMode = false;
            this.gm_Main.PolygonsEnabled = true;
            this.gm_Main.RetryLoadTile = 0;
            this.gm_Main.RoutesEnabled = true;
            this.gm_Main.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Main.ShowTileGridLines = false;
            this.gm_Main.Size = new System.Drawing.Size(838, 309);
            this.gm_Main.TabIndex = 4;
            this.gm_Main.Zoom = 0D;
            this.gm_Main.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gm_Main_OnMarkerClick);
            this.gm_Main.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.gm_Main_OnMarkerEnter);
            this.gm_Main.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.gm_Main_OnMarkerLeave);
            this.gm_Main.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gm_Main_OnMapZoomChanged);
            this.gm_Main.SizeChanged += new System.EventHandler(this.gm_Main_SizeChanged);
            this.gm_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.gm_Main_Paint);
            this.gm_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseDown);
            this.gm_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseMove);
            this.gm_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseUp);
            // 
            // tp_News
            // 
            this.tp_News.Location = new System.Drawing.Point(4, 22);
            this.tp_News.Name = "tp_News";
            this.tp_News.Size = new System.Drawing.Size(844, 315);
            this.tp_News.TabIndex = 3;
            this.tp_News.Text = "Latest News";
            this.tp_News.UseVisualStyleBackColor = true;
            this.tp_News.Enter += new System.EventHandler(this.tp_News_Enter);
            // 
            // tc_Main
            // 
            this.tc_Main.Controls.Add(this.tp_Elevation);
            this.tc_Main.Controls.Add(this.tp_Spectrum);
            this.tc_Main.Controls.Add(this.tp_Analysis);
            this.tc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_Main.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tc_Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tc_Main.Location = new System.Drawing.Point(0, 0);
            this.tc_Main.MinimumSize = new System.Drawing.Size(0, 200);
            this.tc_Main.Name = "tc_Main";
            this.tc_Main.SelectedIndex = 0;
            this.tc_Main.Size = new System.Drawing.Size(852, 360);
            this.tc_Main.TabIndex = 0;
            this.tc_Main.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tc_Main_DrawItem);
            this.tc_Main.SelectedIndexChanged += new System.EventHandler(this.tc_Main_SelectedIndexChanged);
            this.tc_Main.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tc_Main_Selecting);
            // 
            // tp_Elevation
            // 
            this.tp_Elevation.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Elevation.Location = new System.Drawing.Point(4, 22);
            this.tp_Elevation.Name = "tp_Elevation";
            this.tp_Elevation.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Elevation.Size = new System.Drawing.Size(844, 334);
            this.tp_Elevation.TabIndex = 0;
            this.tp_Elevation.Text = "Path Profile";
            this.tp_Elevation.Enter += new System.EventHandler(this.tp_Elevation_Enter);
            this.tp_Elevation.Resize += new System.EventHandler(this.tp_Elevation_Resize);
            // 
            // tp_Spectrum
            // 
            this.tp_Spectrum.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Spectrum.Controls.Add(this.gb_Spectrum_NearestInfo);
            this.tp_Spectrum.Controls.Add(this.gb_Spectrum_Status);
            this.tp_Spectrum.Controls.Add(this.gb_NearestPlaneMap);
            this.tp_Spectrum.Controls.Add(this.gb_Spectrum);
            this.tp_Spectrum.Location = new System.Drawing.Point(4, 22);
            this.tp_Spectrum.Name = "tp_Spectrum";
            this.tp_Spectrum.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Spectrum.Size = new System.Drawing.Size(844, 334);
            this.tp_Spectrum.TabIndex = 1;
            this.tp_Spectrum.Text = "Spectrum";
            this.tp_Spectrum.Enter += new System.EventHandler(this.tp_Spectrum_Enter);
            this.tp_Spectrum.Resize += new System.EventHandler(this.tp_Spectrum_Resize);
            // 
            // gb_Spectrum_NearestInfo
            // 
            this.gb_Spectrum_NearestInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Spectrum_NearestInfo.Controls.Add(this.lbl_Nearest_Dist);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.label22);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.lbl_Nearest_Call);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.label21);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.lbl_Nearest_Angle);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.label7);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.lbl_Nearest_Alt);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.label4);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.lbl_Nearest_Cat);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.label5);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.lbl_Nearest_Type);
            this.gb_Spectrum_NearestInfo.Controls.Add(this.label2);
            this.gb_Spectrum_NearestInfo.Location = new System.Drawing.Point(489, 54);
            this.gb_Spectrum_NearestInfo.Name = "gb_Spectrum_NearestInfo";
            this.gb_Spectrum_NearestInfo.Size = new System.Drawing.Size(175, 271);
            this.gb_Spectrum_NearestInfo.TabIndex = 8;
            this.gb_Spectrum_NearestInfo.TabStop = false;
            this.gb_Spectrum_NearestInfo.Text = "Nearest Plane Info";
            // 
            // lbl_Nearest_Dist
            // 
            this.lbl_Nearest_Dist.AutoSize = true;
            this.lbl_Nearest_Dist.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Nearest_Dist.Location = new System.Drawing.Point(82, 114);
            this.lbl_Nearest_Dist.Name = "lbl_Nearest_Dist";
            this.lbl_Nearest_Dist.Size = new System.Drawing.Size(14, 14);
            this.lbl_Nearest_Dist.TabIndex = 11;
            this.lbl_Nearest_Dist.Text = "0";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(6, 114);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(52, 13);
            this.label22.TabIndex = 10;
            this.label22.Text = "Distance:";
            // 
            // lbl_Nearest_Call
            // 
            this.lbl_Nearest_Call.AutoSize = true;
            this.lbl_Nearest_Call.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Nearest_Call.Location = new System.Drawing.Point(82, 26);
            this.lbl_Nearest_Call.Name = "lbl_Nearest_Call";
            this.lbl_Nearest_Call.Size = new System.Drawing.Size(14, 14);
            this.lbl_Nearest_Call.TabIndex = 9;
            this.lbl_Nearest_Call.Text = "0";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(6, 26);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(27, 13);
            this.label21.TabIndex = 8;
            this.label21.Text = "Call:";
            // 
            // lbl_Nearest_Angle
            // 
            this.lbl_Nearest_Angle.AutoSize = true;
            this.lbl_Nearest_Angle.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Nearest_Angle.Location = new System.Drawing.Point(82, 97);
            this.lbl_Nearest_Angle.Name = "lbl_Nearest_Angle";
            this.lbl_Nearest_Angle.Size = new System.Drawing.Size(14, 14);
            this.lbl_Nearest_Angle.TabIndex = 7;
            this.lbl_Nearest_Angle.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Angle:";
            // 
            // lbl_Nearest_Alt
            // 
            this.lbl_Nearest_Alt.AutoSize = true;
            this.lbl_Nearest_Alt.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Nearest_Alt.Location = new System.Drawing.Point(82, 79);
            this.lbl_Nearest_Alt.Name = "lbl_Nearest_Alt";
            this.lbl_Nearest_Alt.Size = new System.Drawing.Size(14, 14);
            this.lbl_Nearest_Alt.TabIndex = 5;
            this.lbl_Nearest_Alt.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Altitude:";
            // 
            // lbl_Nearest_Cat
            // 
            this.lbl_Nearest_Cat.AutoSize = true;
            this.lbl_Nearest_Cat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Nearest_Cat.Location = new System.Drawing.Point(82, 60);
            this.lbl_Nearest_Cat.Name = "lbl_Nearest_Cat";
            this.lbl_Nearest_Cat.Size = new System.Drawing.Size(14, 14);
            this.lbl_Nearest_Cat.TabIndex = 3;
            this.lbl_Nearest_Cat.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Category:";
            // 
            // lbl_Nearest_Type
            // 
            this.lbl_Nearest_Type.AutoSize = true;
            this.lbl_Nearest_Type.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Nearest_Type.Location = new System.Drawing.Point(82, 44);
            this.lbl_Nearest_Type.Name = "lbl_Nearest_Type";
            this.lbl_Nearest_Type.Size = new System.Drawing.Size(14, 14);
            this.lbl_Nearest_Type.TabIndex = 1;
            this.lbl_Nearest_Type.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type:";
            // 
            // gb_Spectrum_Status
            // 
            this.gb_Spectrum_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Spectrum_Status.Controls.Add(this.tb_Spectrum_Status);
            this.gb_Spectrum_Status.Location = new System.Drawing.Point(8, 6);
            this.gb_Spectrum_Status.Name = "gb_Spectrum_Status";
            this.gb_Spectrum_Status.Size = new System.Drawing.Size(830, 40);
            this.gb_Spectrum_Status.TabIndex = 7;
            this.gb_Spectrum_Status.TabStop = false;
            this.gb_Spectrum_Status.Text = "Status";
            // 
            // tb_Spectrum_Status
            // 
            this.tb_Spectrum_Status.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Spectrum_Status.Location = new System.Drawing.Point(6, 14);
            this.tb_Spectrum_Status.Name = "tb_Spectrum_Status";
            this.tb_Spectrum_Status.Size = new System.Drawing.Size(129, 20);
            this.tb_Spectrum_Status.TabIndex = 4;
            // 
            // gb_NearestPlaneMap
            // 
            this.gb_NearestPlaneMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_NearestPlaneMap.Controls.Add(this.gm_Nearest);
            this.gb_NearestPlaneMap.Location = new System.Drawing.Point(670, 54);
            this.gb_NearestPlaneMap.Name = "gb_NearestPlaneMap";
            this.gb_NearestPlaneMap.Size = new System.Drawing.Size(168, 274);
            this.gb_NearestPlaneMap.TabIndex = 6;
            this.gb_NearestPlaneMap.TabStop = false;
            this.gb_NearestPlaneMap.Text = "Nearest Plane Map";
            // 
            // gm_Nearest
            // 
            this.gm_Nearest.Bearing = 0F;
            this.gm_Nearest.CanDragMap = false;
            this.gm_Nearest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Nearest.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Nearest.GrayScaleMode = false;
            this.gm_Nearest.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Nearest.LevelsKeepInMemmory = 5;
            this.gm_Nearest.Location = new System.Drawing.Point(3, 16);
            this.gm_Nearest.MarkersEnabled = true;
            this.gm_Nearest.MaxZoom = 2;
            this.gm_Nearest.MinZoom = 2;
            this.gm_Nearest.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Nearest.Name = "gm_Nearest";
            this.gm_Nearest.NegativeMode = false;
            this.gm_Nearest.PolygonsEnabled = true;
            this.gm_Nearest.RetryLoadTile = 0;
            this.gm_Nearest.RoutesEnabled = true;
            this.gm_Nearest.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Nearest.ShowTileGridLines = false;
            this.gm_Nearest.Size = new System.Drawing.Size(162, 255);
            this.gm_Nearest.TabIndex = 0;
            this.gm_Nearest.Zoom = 0D;
            // 
            // gb_Spectrum
            // 
            this.gb_Spectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Spectrum.Location = new System.Drawing.Point(8, 54);
            this.gb_Spectrum.Name = "gb_Spectrum";
            this.gb_Spectrum.Size = new System.Drawing.Size(475, 274);
            this.gb_Spectrum.TabIndex = 5;
            this.gb_Spectrum.TabStop = false;
            this.gb_Spectrum.Text = "Spectrum";
            // 
            // tp_Analysis
            // 
            this.tp_Analysis.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Analysis.Controls.Add(this.panel2);
            this.tp_Analysis.Controls.Add(this.panel1);
            this.tp_Analysis.Location = new System.Drawing.Point(4, 22);
            this.tp_Analysis.Name = "tp_Analysis";
            this.tp_Analysis.Size = new System.Drawing.Size(844, 334);
            this.tp_Analysis.TabIndex = 3;
            this.tp_Analysis.Text = "Analysis";
            this.tp_Analysis.Enter += new System.EventHandler(this.tp_Analysis_Enter);
            this.tp_Analysis.Leave += new System.EventHandler(this.tp_Analysis_Leave);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gb_Analysis_Player);
            this.panel2.Controls.Add(this.gb_Analysis_Controls);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 137);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(844, 197);
            this.panel2.TabIndex = 1;
            // 
            // gb_Analysis_Player
            // 
            this.gb_Analysis_Player.Controls.Add(this.dtp_Analysis_MaxValue);
            this.gb_Analysis_Player.Controls.Add(this.dtp_Analysis_MinValue);
            this.gb_Analysis_Player.Controls.Add(this.sb_Analysis_Play);
            this.gb_Analysis_Player.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Analysis_Player.Location = new System.Drawing.Point(0, 0);
            this.gb_Analysis_Player.MinimumSize = new System.Drawing.Size(565, 58);
            this.gb_Analysis_Player.Name = "gb_Analysis_Player";
            this.gb_Analysis_Player.Size = new System.Drawing.Size(591, 197);
            this.gb_Analysis_Player.TabIndex = 4;
            this.gb_Analysis_Player.TabStop = false;
            this.gb_Analysis_Player.Text = "Player";
            this.gb_Analysis_Player.SizeChanged += new System.EventHandler(this.gb_Analysis_Player_SizeChanged);
            // 
            // dtp_Analysis_MaxValue
            // 
            this.dtp_Analysis_MaxValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Analysis_MaxValue.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_Analysis_MaxValue.Enabled = false;
            this.dtp_Analysis_MaxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_Analysis_MaxValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Analysis_MaxValue.Location = new System.Drawing.Point(463, 161);
            this.dtp_Analysis_MaxValue.Name = "dtp_Analysis_MaxValue";
            this.dtp_Analysis_MaxValue.Size = new System.Drawing.Size(122, 20);
            this.dtp_Analysis_MaxValue.TabIndex = 25;
            this.dtp_Analysis_MaxValue.ValueChanged += new System.EventHandler(this.dtp_Analysis_MaxValue_ValueChanged);
            // 
            // dtp_Analysis_MinValue
            // 
            this.dtp_Analysis_MinValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dtp_Analysis_MinValue.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_Analysis_MinValue.Enabled = false;
            this.dtp_Analysis_MinValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtp_Analysis_MinValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_Analysis_MinValue.Location = new System.Drawing.Point(6, 161);
            this.dtp_Analysis_MinValue.Name = "dtp_Analysis_MinValue";
            this.dtp_Analysis_MinValue.Size = new System.Drawing.Size(123, 20);
            this.dtp_Analysis_MinValue.TabIndex = 24;
            this.dtp_Analysis_MinValue.ValueChanged += new System.EventHandler(this.dtp_Analysis_MinValue_ValueChanged);
            // 
            // sb_Analysis_Play
            // 
            this.sb_Analysis_Play.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.sb_Analysis_Play.Location = new System.Drawing.Point(146, 161);
            this.sb_Analysis_Play.Maximum = 1000000;
            this.sb_Analysis_Play.Name = "sb_Analysis_Play";
            this.sb_Analysis_Play.Orientation = CustomScrollBar.ScrollBarOrientation.Horizontal;
            this.sb_Analysis_Play.Size = new System.Drawing.Size(289, 19);
            this.sb_Analysis_Play.TabIndex = 22;
            this.sb_Analysis_Play.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sb_Analysis_Play_Scroll);
            this.sb_Analysis_Play.SizeChanged += new System.EventHandler(this.sb_Analysis_Play_SizeChanged);
            // 
            // gb_Analysis_Controls
            // 
            this.gb_Analysis_Controls.Controls.Add(this.btn_Analysis_FastForward);
            this.gb_Analysis_Controls.Controls.Add(this.btn_Analysis_Forward);
            this.gb_Analysis_Controls.Controls.Add(this.btn_Analysis_Pause);
            this.gb_Analysis_Controls.Controls.Add(this.btn_Analysis_Back);
            this.gb_Analysis_Controls.Controls.Add(this.btn_Analysis_Rewind);
            this.gb_Analysis_Controls.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_Analysis_Controls.Location = new System.Drawing.Point(591, 0);
            this.gb_Analysis_Controls.Name = "gb_Analysis_Controls";
            this.gb_Analysis_Controls.Size = new System.Drawing.Size(253, 197);
            this.gb_Analysis_Controls.TabIndex = 2;
            this.gb_Analysis_Controls.TabStop = false;
            this.gb_Analysis_Controls.Text = "Controls";
            // 
            // btn_Analysis_FastForward
            // 
            this.btn_Analysis_FastForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Analysis_FastForward.Enabled = false;
            this.btn_Analysis_FastForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_FastForward.Image = ((System.Drawing.Image)(resources.GetObject("btn_Analysis_FastForward.Image")));
            this.btn_Analysis_FastForward.Location = new System.Drawing.Point(202, 154);
            this.btn_Analysis_FastForward.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Analysis_FastForward.Name = "btn_Analysis_FastForward";
            this.btn_Analysis_FastForward.Size = new System.Drawing.Size(47, 29);
            this.btn_Analysis_FastForward.TabIndex = 20;
            this.btn_Analysis_FastForward.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tt_Main.SetToolTip(this.btn_Analysis_FastForward, "Click here to speed up playing by 10 sec");
            this.btn_Analysis_FastForward.UseVisualStyleBackColor = true;
            this.btn_Analysis_FastForward.Click += new System.EventHandler(this.btn_Analysis_FastForward_Click);
            // 
            // btn_Analysis_Forward
            // 
            this.btn_Analysis_Forward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Analysis_Forward.Enabled = false;
            this.btn_Analysis_Forward.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Forward.Image = ((System.Drawing.Image)(resources.GetObject("btn_Analysis_Forward.Image")));
            this.btn_Analysis_Forward.Location = new System.Drawing.Point(153, 154);
            this.btn_Analysis_Forward.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Analysis_Forward.Name = "btn_Analysis_Forward";
            this.btn_Analysis_Forward.Size = new System.Drawing.Size(47, 29);
            this.btn_Analysis_Forward.TabIndex = 19;
            this.btn_Analysis_Forward.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tt_Main.SetToolTip(this.btn_Analysis_Forward, "Click here to speed up playing by 1 sec");
            this.btn_Analysis_Forward.UseVisualStyleBackColor = true;
            this.btn_Analysis_Forward.Click += new System.EventHandler(this.btn_Analysis_Forward_Click);
            // 
            // btn_Analysis_Pause
            // 
            this.btn_Analysis_Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Analysis_Pause.Enabled = false;
            this.btn_Analysis_Pause.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Pause.Image = ((System.Drawing.Image)(resources.GetObject("btn_Analysis_Pause.Image")));
            this.btn_Analysis_Pause.Location = new System.Drawing.Point(104, 154);
            this.btn_Analysis_Pause.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Analysis_Pause.Name = "btn_Analysis_Pause";
            this.btn_Analysis_Pause.Size = new System.Drawing.Size(47, 29);
            this.btn_Analysis_Pause.TabIndex = 18;
            this.btn_Analysis_Pause.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tt_Main.SetToolTip(this.btn_Analysis_Pause, "Click here to stop playing");
            this.btn_Analysis_Pause.UseVisualStyleBackColor = true;
            this.btn_Analysis_Pause.Click += new System.EventHandler(this.btn_Analysis_Pause_Click);
            // 
            // btn_Analysis_Back
            // 
            this.btn_Analysis_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Analysis_Back.Enabled = false;
            this.btn_Analysis_Back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Back.Image = ((System.Drawing.Image)(resources.GetObject("btn_Analysis_Back.Image")));
            this.btn_Analysis_Back.Location = new System.Drawing.Point(55, 154);
            this.btn_Analysis_Back.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Analysis_Back.Name = "btn_Analysis_Back";
            this.btn_Analysis_Back.Size = new System.Drawing.Size(47, 29);
            this.btn_Analysis_Back.TabIndex = 17;
            this.btn_Analysis_Back.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tt_Main.SetToolTip(this.btn_Analysis_Back, "Click here to slow down playing by 1 sec");
            this.btn_Analysis_Back.UseVisualStyleBackColor = true;
            this.btn_Analysis_Back.Click += new System.EventHandler(this.btn_Analysis_Back_Click);
            // 
            // btn_Analysis_Rewind
            // 
            this.btn_Analysis_Rewind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Analysis_Rewind.Enabled = false;
            this.btn_Analysis_Rewind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Rewind.Image = ((System.Drawing.Image)(resources.GetObject("btn_Analysis_Rewind.Image")));
            this.btn_Analysis_Rewind.Location = new System.Drawing.Point(6, 154);
            this.btn_Analysis_Rewind.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Analysis_Rewind.Name = "btn_Analysis_Rewind";
            this.btn_Analysis_Rewind.Size = new System.Drawing.Size(47, 29);
            this.btn_Analysis_Rewind.TabIndex = 16;
            this.btn_Analysis_Rewind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tt_Main.SetToolTip(this.btn_Analysis_Rewind, "Click here to slow down playing by 10 sec");
            this.btn_Analysis_Rewind.UseVisualStyleBackColor = true;
            this.btn_Analysis_Rewind.Click += new System.EventHandler(this.btn_Analysis_Rewind_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gb_Analysis_Functions);
            this.panel1.Controls.Add(this.gb_Analysis_Database);
            this.panel1.Controls.Add(this.gb_Analysis_Mode);
            this.panel1.Controls.Add(this.gb_Analysis_Time);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(844, 137);
            this.panel1.TabIndex = 0;
            // 
            // gb_Analysis_Functions
            // 
            this.gb_Analysis_Functions.Controls.Add(this.btn_Analysis_Plane_History);
            this.gb_Analysis_Functions.Controls.Add(this.btn_Analysis_CrossingHistory);
            this.gb_Analysis_Functions.Controls.Add(this.btn_Analysis_Path_SaveToFile);
            this.gb_Analysis_Functions.Controls.Add(this.btn_Analysis_Planes_ShowTraffic);
            this.gb_Analysis_Functions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Analysis_Functions.Location = new System.Drawing.Point(358, 0);
            this.gb_Analysis_Functions.Name = "gb_Analysis_Functions";
            this.gb_Analysis_Functions.Size = new System.Drawing.Size(233, 137);
            this.gb_Analysis_Functions.TabIndex = 5;
            this.gb_Analysis_Functions.TabStop = false;
            this.gb_Analysis_Functions.Text = "Functions";
            // 
            // btn_Analysis_Plane_History
            // 
            this.btn_Analysis_Plane_History.Enabled = false;
            this.btn_Analysis_Plane_History.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Plane_History.Location = new System.Drawing.Point(6, 108);
            this.btn_Analysis_Plane_History.Name = "btn_Analysis_Plane_History";
            this.btn_Analysis_Plane_History.Size = new System.Drawing.Size(113, 23);
            this.btn_Analysis_Plane_History.TabIndex = 40;
            this.btn_Analysis_Plane_History.Text = "Export Plane History";
            this.btn_Analysis_Plane_History.UseVisualStyleBackColor = true;
            this.btn_Analysis_Plane_History.Click += new System.EventHandler(this.btn_Analysis_Plane_History_Click);
            // 
            // btn_Analysis_CrossingHistory
            // 
            this.btn_Analysis_CrossingHistory.Enabled = false;
            this.btn_Analysis_CrossingHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_CrossingHistory.Location = new System.Drawing.Point(6, 79);
            this.btn_Analysis_CrossingHistory.Name = "btn_Analysis_CrossingHistory";
            this.btn_Analysis_CrossingHistory.Size = new System.Drawing.Size(113, 23);
            this.btn_Analysis_CrossingHistory.TabIndex = 39;
            this.btn_Analysis_CrossingHistory.Text = "Crossing History";
            this.btn_Analysis_CrossingHistory.UseVisualStyleBackColor = true;
            this.btn_Analysis_CrossingHistory.Click += new System.EventHandler(this.btn_Analysis_CrossingHistory_Click);
            // 
            // btn_Analysis_Path_SaveToFile
            // 
            this.btn_Analysis_Path_SaveToFile.Enabled = false;
            this.btn_Analysis_Path_SaveToFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Path_SaveToFile.Location = new System.Drawing.Point(6, 50);
            this.btn_Analysis_Path_SaveToFile.Name = "btn_Analysis_Path_SaveToFile";
            this.btn_Analysis_Path_SaveToFile.Size = new System.Drawing.Size(113, 23);
            this.btn_Analysis_Path_SaveToFile.TabIndex = 38;
            this.btn_Analysis_Path_SaveToFile.Text = "Export Path";
            this.btn_Analysis_Path_SaveToFile.UseVisualStyleBackColor = true;
            this.btn_Analysis_Path_SaveToFile.Click += new System.EventHandler(this.btn_Analysis_Path_SaveToFile_Click);
            // 
            // btn_Analysis_Planes_ShowTraffic
            // 
            this.btn_Analysis_Planes_ShowTraffic.Enabled = false;
            this.btn_Analysis_Planes_ShowTraffic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Planes_ShowTraffic.Location = new System.Drawing.Point(6, 21);
            this.btn_Analysis_Planes_ShowTraffic.Name = "btn_Analysis_Planes_ShowTraffic";
            this.btn_Analysis_Planes_ShowTraffic.Size = new System.Drawing.Size(113, 23);
            this.btn_Analysis_Planes_ShowTraffic.TabIndex = 37;
            this.btn_Analysis_Planes_ShowTraffic.Text = "Show All Traffic";
            this.btn_Analysis_Planes_ShowTraffic.UseVisualStyleBackColor = true;
            this.btn_Analysis_Planes_ShowTraffic.Click += new System.EventHandler(this.btn_Analysis_Planes_ShowTraffic_Click);
            // 
            // gb_Analysis_Database
            // 
            this.gb_Analysis_Database.Controls.Add(this.tb_Analysis_Status);
            this.gb_Analysis_Database.Controls.Add(this.btn_Analysis_Planes_History);
            this.gb_Analysis_Database.Controls.Add(this.btn_Analysis_Planes_Clear);
            this.gb_Analysis_Database.Controls.Add(this.btn_Analysis_Planes_Save);
            this.gb_Analysis_Database.Controls.Add(this.btn_Analysis_Planes_Load);
            this.gb_Analysis_Database.Dock = System.Windows.Forms.DockStyle.Left;
            this.gb_Analysis_Database.Location = new System.Drawing.Point(78, 0);
            this.gb_Analysis_Database.Name = "gb_Analysis_Database";
            this.gb_Analysis_Database.Size = new System.Drawing.Size(280, 137);
            this.gb_Analysis_Database.TabIndex = 4;
            this.gb_Analysis_Database.TabStop = false;
            this.gb_Analysis_Database.Text = "Database";
            // 
            // tb_Analysis_Status
            // 
            this.tb_Analysis_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Analysis_Status.Location = new System.Drawing.Point(8, 111);
            this.tb_Analysis_Status.Name = "tb_Analysis_Status";
            this.tb_Analysis_Status.ReadOnly = true;
            this.tb_Analysis_Status.Size = new System.Drawing.Size(262, 20);
            this.tb_Analysis_Status.TabIndex = 38;
            // 
            // btn_Analysis_Planes_History
            // 
            this.btn_Analysis_Planes_History.Enabled = false;
            this.btn_Analysis_Planes_History.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Planes_History.Location = new System.Drawing.Point(142, 21);
            this.btn_Analysis_Planes_History.Name = "btn_Analysis_Planes_History";
            this.btn_Analysis_Planes_History.Size = new System.Drawing.Size(128, 23);
            this.btn_Analysis_Planes_History.TabIndex = 36;
            this.btn_Analysis_Planes_History.Text = "Load History from URL";
            this.btn_Analysis_Planes_History.UseVisualStyleBackColor = true;
            this.btn_Analysis_Planes_History.Click += new System.EventHandler(this.btn_Analysis_Planes_History_Click);
            // 
            // btn_Analysis_Planes_Clear
            // 
            this.btn_Analysis_Planes_Clear.Enabled = false;
            this.btn_Analysis_Planes_Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Planes_Clear.Location = new System.Drawing.Point(8, 79);
            this.btn_Analysis_Planes_Clear.Name = "btn_Analysis_Planes_Clear";
            this.btn_Analysis_Planes_Clear.Size = new System.Drawing.Size(128, 23);
            this.btn_Analysis_Planes_Clear.TabIndex = 34;
            this.btn_Analysis_Planes_Clear.Text = "Clear Positions";
            this.btn_Analysis_Planes_Clear.UseVisualStyleBackColor = true;
            this.btn_Analysis_Planes_Clear.Click += new System.EventHandler(this.btn_Analysis_Planes_Clear_Click);
            // 
            // btn_Analysis_Planes_Save
            // 
            this.btn_Analysis_Planes_Save.Enabled = false;
            this.btn_Analysis_Planes_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Planes_Save.Location = new System.Drawing.Point(8, 50);
            this.btn_Analysis_Planes_Save.Name = "btn_Analysis_Planes_Save";
            this.btn_Analysis_Planes_Save.Size = new System.Drawing.Size(128, 23);
            this.btn_Analysis_Planes_Save.TabIndex = 33;
            this.btn_Analysis_Planes_Save.Text = "Save Positions to File";
            this.btn_Analysis_Planes_Save.UseVisualStyleBackColor = true;
            this.btn_Analysis_Planes_Save.Click += new System.EventHandler(this.btn_Analysis_Planes_Save_Click);
            // 
            // btn_Analysis_Planes_Load
            // 
            this.btn_Analysis_Planes_Load.Enabled = false;
            this.btn_Analysis_Planes_Load.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Analysis_Planes_Load.Location = new System.Drawing.Point(8, 21);
            this.btn_Analysis_Planes_Load.Name = "btn_Analysis_Planes_Load";
            this.btn_Analysis_Planes_Load.Size = new System.Drawing.Size(128, 23);
            this.btn_Analysis_Planes_Load.TabIndex = 32;
            this.btn_Analysis_Planes_Load.Text = "Load Positions from File";
            this.btn_Analysis_Planes_Load.UseVisualStyleBackColor = true;
            this.btn_Analysis_Planes_Load.Click += new System.EventHandler(this.btn_Analysis_Planes_Load_Click);
            // 
            // gb_Analysis_Mode
            // 
            this.gb_Analysis_Mode.Controls.Add(this.btn_Analysis_OFF);
            this.gb_Analysis_Mode.Controls.Add(this.btn_Analysis_ON);
            this.gb_Analysis_Mode.Dock = System.Windows.Forms.DockStyle.Left;
            this.gb_Analysis_Mode.Location = new System.Drawing.Point(0, 0);
            this.gb_Analysis_Mode.Name = "gb_Analysis_Mode";
            this.gb_Analysis_Mode.Size = new System.Drawing.Size(78, 137);
            this.gb_Analysis_Mode.TabIndex = 1;
            this.gb_Analysis_Mode.TabStop = false;
            this.gb_Analysis_Mode.Text = "Analysis";
            // 
            // btn_Analysis_OFF
            // 
            this.btn_Analysis_OFF.BackColor = System.Drawing.Color.LightCoral;
            this.btn_Analysis_OFF.Enabled = false;
            this.btn_Analysis_OFF.Location = new System.Drawing.Point(8, 76);
            this.btn_Analysis_OFF.Name = "btn_Analysis_OFF";
            this.btn_Analysis_OFF.Size = new System.Drawing.Size(64, 52);
            this.btn_Analysis_OFF.TabIndex = 1;
            this.btn_Analysis_OFF.Text = "OFF";
            this.btn_Analysis_OFF.UseVisualStyleBackColor = false;
            this.btn_Analysis_OFF.Click += new System.EventHandler(this.btn_Analysis_OFF_Click);
            // 
            // btn_Analysis_ON
            // 
            this.btn_Analysis_ON.BackColor = System.Drawing.Color.YellowGreen;
            this.btn_Analysis_ON.Enabled = false;
            this.btn_Analysis_ON.Location = new System.Drawing.Point(8, 20);
            this.btn_Analysis_ON.Name = "btn_Analysis_ON";
            this.btn_Analysis_ON.Size = new System.Drawing.Size(64, 52);
            this.btn_Analysis_ON.TabIndex = 0;
            this.btn_Analysis_ON.Text = "ON";
            this.btn_Analysis_ON.UseVisualStyleBackColor = false;
            this.btn_Analysis_ON.Click += new System.EventHandler(this.btn_Analysis_ON_Click);
            // 
            // gb_Analysis_Time
            // 
            this.gb_Analysis_Time.Controls.Add(this.label15);
            this.gb_Analysis_Time.Controls.Add(this.label14);
            this.gb_Analysis_Time.Controls.Add(this.tb_Analysis_Stepwidth);
            this.gb_Analysis_Time.Controls.Add(this.tb_Analysis_Time);
            this.gb_Analysis_Time.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_Analysis_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Analysis_Time.Location = new System.Drawing.Point(591, 0);
            this.gb_Analysis_Time.Name = "gb_Analysis_Time";
            this.gb_Analysis_Time.Size = new System.Drawing.Size(253, 137);
            this.gb_Analysis_Time.TabIndex = 3;
            this.gb_Analysis_Time.TabStop = false;
            this.gb_Analysis_Time.Text = "Time";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(210, 85);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(27, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "sec.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(7, 84);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Stepwidth:";
            // 
            // tb_Analysis_Stepwidth
            // 
            this.tb_Analysis_Stepwidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Analysis_Stepwidth.Location = new System.Drawing.Point(84, 82);
            this.tb_Analysis_Stepwidth.Name = "tb_Analysis_Stepwidth";
            this.tb_Analysis_Stepwidth.ReadOnly = true;
            this.tb_Analysis_Stepwidth.Size = new System.Drawing.Size(96, 20);
            this.tb_Analysis_Stepwidth.TabIndex = 1;
            this.tb_Analysis_Stepwidth.Text = "0";
            // 
            // tb_Analysis_Time
            // 
            this.tb_Analysis_Time.BackColor = System.Drawing.Color.Black;
            this.tb_Analysis_Time.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Analysis_Time.ForeColor = System.Drawing.Color.Chartreuse;
            this.tb_Analysis_Time.Location = new System.Drawing.Point(6, 27);
            this.tb_Analysis_Time.Name = "tb_Analysis_Time";
            this.tb_Analysis_Time.ReadOnly = true;
            this.tb_Analysis_Time.Size = new System.Drawing.Size(242, 35);
            this.tb_Analysis_Time.TabIndex = 0;
            this.tb_Analysis_Time.Text = "0000-00-00 00:00:00";
            this.tb_Analysis_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status,
            this.tsl_Dummy,
            this.tsl_Calculations,
            this.tsl_Database,
            this.tsl_Database_LED_Aircraft,
            this.tsl_Database_LED_Stations,
            this.tsl_Database_LED_GLOBE,
            this.tsl_Database_LED_SRTM3,
            this.tsl_Database_LED_SRTM1});
            this.ss_Main.Location = new System.Drawing.Point(0, 706);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.ShowItemToolTips = true;
            this.ss_Main.Size = new System.Drawing.Size(1008, 24);
            this.ss_Main.TabIndex = 22;
            this.ss_Main.Text = "ss_Main";
            // 
            // tsl_Status
            // 
            this.tsl_Status.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsl_Status.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(742, 19);
            this.tsl_Status.Spring = true;
            this.tsl_Status.Text = "No Messages.";
            this.tsl_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsl_Dummy
            // 
            this.tsl_Dummy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsl_Dummy.Name = "tsl_Dummy";
            this.tsl_Dummy.Size = new System.Drawing.Size(0, 19);
            this.tsl_Dummy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsl_Calculations
            // 
            this.tsl_Calculations.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsl_Calculations.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsl_Calculations.Name = "tsl_Calculations";
            this.tsl_Calculations.Size = new System.Drawing.Size(79, 19);
            this.tsl_Calculations.Text = "Calculations.";
            // 
            // tsl_Database
            // 
            this.tsl_Database.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsl_Database.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsl_Database.Name = "tsl_Database";
            this.tsl_Database.Size = new System.Drawing.Size(96, 19);
            this.tsl_Database.Text = "Database status.";
            // 
            // tsl_Database_LED_Aircraft
            // 
            this.tsl_Database_LED_Aircraft.AutoSize = false;
            this.tsl_Database_LED_Aircraft.BackColor = System.Drawing.Color.Plum;
            this.tsl_Database_LED_Aircraft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.tsl_Database_LED_Aircraft.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsl_Database_LED_Aircraft.Image = ((System.Drawing.Image)(resources.GetObject("tsl_Database_LED_Aircraft.Image")));
            this.tsl_Database_LED_Aircraft.Margin = new System.Windows.Forms.Padding(7, 5, 1, 5);
            this.tsl_Database_LED_Aircraft.Name = "tsl_Database_LED_Aircraft";
            this.tsl_Database_LED_Aircraft.Size = new System.Drawing.Size(12, 14);
            this.tsl_Database_LED_Aircraft.Text = "Aircraft database status LED";
            this.tsl_Database_LED_Aircraft.ToolTipText = "Aircraft database status LED";
            // 
            // tsl_Database_LED_Stations
            // 
            this.tsl_Database_LED_Stations.AutoSize = false;
            this.tsl_Database_LED_Stations.BackColor = System.Drawing.Color.Plum;
            this.tsl_Database_LED_Stations.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.tsl_Database_LED_Stations.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsl_Database_LED_Stations.Image = ((System.Drawing.Image)(resources.GetObject("tsl_Database_LED_Stations.Image")));
            this.tsl_Database_LED_Stations.Margin = new System.Windows.Forms.Padding(1, 5, 1, 5);
            this.tsl_Database_LED_Stations.Name = "tsl_Database_LED_Stations";
            this.tsl_Database_LED_Stations.Size = new System.Drawing.Size(12, 14);
            this.tsl_Database_LED_Stations.Text = " Station database status LED";
            this.tsl_Database_LED_Stations.ToolTipText = "Station database status LED";
            // 
            // tsl_Database_LED_GLOBE
            // 
            this.tsl_Database_LED_GLOBE.AutoSize = false;
            this.tsl_Database_LED_GLOBE.BackColor = System.Drawing.Color.Plum;
            this.tsl_Database_LED_GLOBE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.tsl_Database_LED_GLOBE.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsl_Database_LED_GLOBE.Image = ((System.Drawing.Image)(resources.GetObject("tsl_Database_LED_GLOBE.Image")));
            this.tsl_Database_LED_GLOBE.Margin = new System.Windows.Forms.Padding(1, 5, 1, 5);
            this.tsl_Database_LED_GLOBE.Name = "tsl_Database_LED_GLOBE";
            this.tsl_Database_LED_GLOBE.Size = new System.Drawing.Size(12, 14);
            this.tsl_Database_LED_GLOBE.Text = " GLOBE database status LED";
            this.tsl_Database_LED_GLOBE.ToolTipText = "GLOBE database status LED";
            // 
            // tsl_Database_LED_SRTM3
            // 
            this.tsl_Database_LED_SRTM3.AutoSize = false;
            this.tsl_Database_LED_SRTM3.BackColor = System.Drawing.Color.Plum;
            this.tsl_Database_LED_SRTM3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.tsl_Database_LED_SRTM3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsl_Database_LED_SRTM3.Image = ((System.Drawing.Image)(resources.GetObject("tsl_Database_LED_SRTM3.Image")));
            this.tsl_Database_LED_SRTM3.Margin = new System.Windows.Forms.Padding(1, 5, 1, 5);
            this.tsl_Database_LED_SRTM3.Name = "tsl_Database_LED_SRTM3";
            this.tsl_Database_LED_SRTM3.Size = new System.Drawing.Size(12, 14);
            this.tsl_Database_LED_SRTM3.Text = " SRTM3 database status LED";
            this.tsl_Database_LED_SRTM3.ToolTipText = "SRTM3 database status LED";
            // 
            // tsl_Database_LED_SRTM1
            // 
            this.tsl_Database_LED_SRTM1.AutoSize = false;
            this.tsl_Database_LED_SRTM1.BackColor = System.Drawing.Color.Plum;
            this.tsl_Database_LED_SRTM1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.tsl_Database_LED_SRTM1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsl_Database_LED_SRTM1.Image = ((System.Drawing.Image)(resources.GetObject("tsl_Database_LED_SRTM1.Image")));
            this.tsl_Database_LED_SRTM1.Margin = new System.Windows.Forms.Padding(1, 5, 1, 5);
            this.tsl_Database_LED_SRTM1.Name = "tsl_Database_LED_SRTM1";
            this.tsl_Database_LED_SRTM1.Size = new System.Drawing.Size(12, 14);
            this.tsl_Database_LED_SRTM1.Text = " SRTM3 database status LED";
            this.tsl_Database_LED_SRTM1.ToolTipText = "SRTM1 database status LED";
            // 
            // btn_Map_PlayPause
            // 
            this.btn_Map_PlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Map_PlayPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Map_PlayPause.ImageIndex = 1;
            this.btn_Map_PlayPause.ImageList = this.il_Main;
            this.btn_Map_PlayPause.Location = new System.Drawing.Point(20, 88);
            this.btn_Map_PlayPause.Name = "btn_Map_PlayPause";
            this.btn_Map_PlayPause.Size = new System.Drawing.Size(114, 29);
            this.btn_Map_PlayPause.TabIndex = 15;
            this.btn_Map_PlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tt_Main.SetToolTip(this.btn_Map_PlayPause, "Press button to toggle betwwen Play and Pause.\r\nIf playing, aircafts are shown on" +
        " the map in real time.\r\nIf paused, calls and options can be set.");
            this.btn_Map_PlayPause.UseVisualStyleBackColor = true;
            this.btn_Map_PlayPause.Click += new System.EventHandler(this.btn_Map_PlayPause_Click);
            // 
            // btn_Map_Save
            // 
            this.btn_Map_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Map_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Map_Save.Location = new System.Drawing.Point(19, 53);
            this.btn_Map_Save.Name = "btn_Map_Save";
            this.btn_Map_Save.Size = new System.Drawing.Size(114, 29);
            this.btn_Map_Save.TabIndex = 14;
            this.btn_Map_Save.Text = "&Save";
            this.tt_Main.SetToolTip(this.btn_Map_Save, "Press button to save a hordcopy of the current main window.\r\nThe file is saved in" +
        " the program\'s temp directory.");
            this.btn_Map_Save.UseVisualStyleBackColor = true;
            this.btn_Map_Save.Click += new System.EventHandler(this.btn_Map_Save_Click);
            // 
            // btn_Options
            // 
            this.btn_Options.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Options.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Options.Location = new System.Drawing.Point(19, 18);
            this.btn_Options.Name = "btn_Options";
            this.btn_Options.Size = new System.Drawing.Size(114, 29);
            this.btn_Options.TabIndex = 16;
            this.btn_Options.Text = "&Options";
            this.tt_Main.SetToolTip(this.btn_Options, "Press button to change program options.");
            this.btn_Options.UseVisualStyleBackColor = true;
            this.btn_Options.Click += new System.EventHandler(this.btn_Options_Click);
            // 
            // cb_Band
            // 
            this.cb_Band.AllowDrop = true;
            this.cb_Band.BackColor = System.Drawing.Color.FloralWhite;
            this.cb_Band.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Band.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Band.FormattingEnabled = true;
            this.cb_Band.Location = new System.Drawing.Point(8, 60);
            this.cb_Band.Name = "cb_Band";
            this.cb_Band.Size = new System.Drawing.Size(130, 24);
            this.cb_Band.TabIndex = 16;
            this.tt_Main.SetToolTip(this.cb_Band, "Select the current band here.\r\nThis is used to calculate the Fresnel Zone F1 clea" +
        "rance.");
            this.cb_Band.SelectedIndexChanged += new System.EventHandler(this.cb_Band_SelectedIndexChanged);
            // 
            // tb_QTF
            // 
            this.tb_QTF.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_QTF.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_QTF.Location = new System.Drawing.Point(3, 242);
            this.tb_QTF.Name = "tb_QTF";
            this.tb_QTF.ReadOnly = true;
            this.tb_QTF.Size = new System.Drawing.Size(133, 22);
            this.tb_QTF.TabIndex = 11;
            this.tt_Main.SetToolTip(this.tb_QTF, "The bearing to your QSO partner.");
            // 
            // tb_QRB
            // 
            this.tb_QRB.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_QRB.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_QRB.Location = new System.Drawing.Point(3, 198);
            this.tb_QRB.Name = "tb_QRB";
            this.tb_QRB.ReadOnly = true;
            this.tb_QRB.Size = new System.Drawing.Size(133, 22);
            this.tb_QRB.TabIndex = 9;
            this.tt_Main.SetToolTip(this.tb_QRB, "The distance between both QSO partners.");
            // 
            // gb_Map_Alarms
            // 
            this.gb_Map_Alarms.Controls.Add(this.cb_Alarms_Activate);
            this.gb_Map_Alarms.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Map_Alarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map_Alarms.Location = new System.Drawing.Point(0, 110);
            this.gb_Map_Alarms.Name = "gb_Map_Alarms";
            this.gb_Map_Alarms.Size = new System.Drawing.Size(143, 47);
            this.gb_Map_Alarms.TabIndex = 62;
            this.gb_Map_Alarms.TabStop = false;
            this.gb_Map_Alarms.Text = "Alarms";
            this.tt_Main.SetToolTip(this.gb_Map_Alarms, "Left Click on Title to show/hide box");
            // 
            // cb_Alarms_Activate
            // 
            this.cb_Alarms_Activate.AutoSize = true;
            this.cb_Alarms_Activate.Checked = global::AirScout.Properties.Settings.Default.Alarm_Activate;
            this.cb_Alarms_Activate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Alarm_Activate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Alarms_Activate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Alarms_Activate.Location = new System.Drawing.Point(14, 19);
            this.cb_Alarms_Activate.Name = "cb_Alarms_Activate";
            this.cb_Alarms_Activate.Size = new System.Drawing.Size(99, 17);
            this.cb_Alarms_Activate.TabIndex = 0;
            this.cb_Alarms_Activate.Text = "Activate Alarms";
            this.cb_Alarms_Activate.UseVisualStyleBackColor = true;
            this.cb_Alarms_Activate.CheckedChanged += new System.EventHandler(this.cb_Alarms_Activate_CheckedChanged);
            // 
            // gb_Map_Zoom
            // 
            this.gb_Map_Zoom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gb_Map_Zoom.Controls.Add(this.pa_Map_Zoom);
            this.gb_Map_Zoom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_Map_Zoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map_Zoom.Location = new System.Drawing.Point(0, 504);
            this.gb_Map_Zoom.Name = "gb_Map_Zoom";
            this.gb_Map_Zoom.Size = new System.Drawing.Size(152, 79);
            this.gb_Map_Zoom.TabIndex = 60;
            this.gb_Map_Zoom.TabStop = false;
            this.gb_Map_Zoom.Text = "Map Zoom";
            this.tt_Main.SetToolTip(this.gb_Map_Zoom, "Left Click on Title to show/hide box");
            // 
            // pa_Map_Zoom
            // 
            this.pa_Map_Zoom.Controls.Add(this.cb_AutoCenter);
            this.pa_Map_Zoom.Controls.Add(this.tb_Zoom);
            this.pa_Map_Zoom.Controls.Add(this.btn_Zoom_Out);
            this.pa_Map_Zoom.Controls.Add(this.btn_Zoom_In);
            this.pa_Map_Zoom.Location = new System.Drawing.Point(3, 16);
            this.pa_Map_Zoom.Name = "pa_Map_Zoom";
            this.pa_Map_Zoom.Size = new System.Drawing.Size(146, 60);
            this.pa_Map_Zoom.TabIndex = 0;
            // 
            // cb_AutoCenter
            // 
            this.cb_AutoCenter.AutoSize = true;
            this.cb_AutoCenter.Checked = global::AirScout.Properties.Settings.Default.Map_AutoCenter;
            this.cb_AutoCenter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_AutoCenter.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Map_AutoCenter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_AutoCenter.Location = new System.Drawing.Point(20, 37);
            this.cb_AutoCenter.Name = "cb_AutoCenter";
            this.cb_AutoCenter.Size = new System.Drawing.Size(93, 17);
            this.cb_AutoCenter.TabIndex = 24;
            this.cb_AutoCenter.Text = "Auto Center";
            this.cb_AutoCenter.UseVisualStyleBackColor = true;
            // 
            // tb_Zoom
            // 
            this.tb_Zoom.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_Zoom.Location = new System.Drawing.Point(53, 9);
            this.tb_Zoom.Name = "tb_Zoom";
            this.tb_Zoom.Size = new System.Drawing.Size(41, 20);
            this.tb_Zoom.TabIndex = 23;
            // 
            // btn_Zoom_Out
            // 
            this.btn_Zoom_Out.Location = new System.Drawing.Point(100, 8);
            this.btn_Zoom_Out.Name = "btn_Zoom_Out";
            this.btn_Zoom_Out.Size = new System.Drawing.Size(30, 23);
            this.btn_Zoom_Out.TabIndex = 22;
            this.btn_Zoom_Out.Text = "-";
            this.btn_Zoom_Out.UseVisualStyleBackColor = true;
            this.btn_Zoom_Out.Click += new System.EventHandler(this.btn_Zoom_Out_Click);
            // 
            // btn_Zoom_In
            // 
            this.btn_Zoom_In.Location = new System.Drawing.Point(17, 8);
            this.btn_Zoom_In.Name = "btn_Zoom_In";
            this.btn_Zoom_In.Size = new System.Drawing.Size(30, 23);
            this.btn_Zoom_In.TabIndex = 21;
            this.btn_Zoom_In.Text = "+";
            this.btn_Zoom_In.UseVisualStyleBackColor = true;
            this.btn_Zoom_In.Click += new System.EventHandler(this.btn_Zoom_In_Click);
            // 
            // gb_Map_Filter
            // 
            this.gb_Map_Filter.Controls.Add(this.pa_Planes_Filter);
            this.gb_Map_Filter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Map_Filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map_Filter.Location = new System.Drawing.Point(0, 0);
            this.gb_Map_Filter.Name = "gb_Map_Filter";
            this.gb_Map_Filter.Size = new System.Drawing.Size(143, 110);
            this.gb_Map_Filter.TabIndex = 61;
            this.gb_Map_Filter.TabStop = false;
            this.gb_Map_Filter.Text = "Planes Filter";
            this.tt_Main.SetToolTip(this.gb_Map_Filter, "Left Click on Title to show/hide box");
            // 
            // pa_Planes_Filter
            // 
            this.pa_Planes_Filter.Controls.Add(this.tb_Planes_Filter_Min_Alt);
            this.pa_Planes_Filter.Controls.Add(this.tb_Planes_Filter_Max_Circumcircle);
            this.pa_Planes_Filter.Controls.Add(this.label12);
            this.pa_Planes_Filter.Controls.Add(this.label13);
            this.pa_Planes_Filter.Controls.Add(this.cb_Planes_Filter_Min_Cat);
            this.pa_Planes_Filter.Controls.Add(this.label11);
            this.pa_Planes_Filter.Controls.Add(this.label10);
            this.pa_Planes_Filter.Controls.Add(this.label9);
            this.pa_Planes_Filter.Location = new System.Drawing.Point(3, 16);
            this.pa_Planes_Filter.Name = "pa_Planes_Filter";
            this.pa_Planes_Filter.Size = new System.Drawing.Size(146, 91);
            this.pa_Planes_Filter.TabIndex = 0;
            // 
            // tb_Planes_Filter_Min_Alt
            // 
            this.tb_Planes_Filter_Min_Alt.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScout.Properties.Settings.Default, "Planes_Filter_Min_Alt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Planes_Filter_Min_Alt.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Planes_Filter_Min_Alt.FormatSpecifier = "F0";
            this.tb_Planes_Filter_Min_Alt.Location = new System.Drawing.Point(59, 34);
            this.tb_Planes_Filter_Min_Alt.MaxValue = 12000;
            this.tb_Planes_Filter_Min_Alt.MinValue = 0;
            this.tb_Planes_Filter_Min_Alt.Name = "tb_Planes_Filter_Min_Alt";
            this.tb_Planes_Filter_Min_Alt.Size = new System.Drawing.Size(57, 22);
            this.tb_Planes_Filter_Min_Alt.TabIndex = 33;
            this.tb_Planes_Filter_Min_Alt.Text = "0";
            this.tt_Main.SetToolTip(this.tb_Planes_Filter_Min_Alt, "Set the minimum aircraft altitude here.");
            this.tb_Planes_Filter_Min_Alt.Value = global::AirScout.Properties.Settings.Default.Planes_Filter_Min_Alt;
            // 
            // tb_Planes_Filter_Max_Circumcircle
            // 
            this.tb_Planes_Filter_Max_Circumcircle.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScout.Properties.Settings.Default, "Planes_Filter_Max_Circumcircle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Planes_Filter_Max_Circumcircle.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Planes_Filter_Max_Circumcircle.FormatSpecifier = "F0";
            this.tb_Planes_Filter_Max_Circumcircle.Location = new System.Drawing.Point(59, 8);
            this.tb_Planes_Filter_Max_Circumcircle.MaxValue = 1000;
            this.tb_Planes_Filter_Max_Circumcircle.MinValue = -1;
            this.tb_Planes_Filter_Max_Circumcircle.Name = "tb_Planes_Filter_Max_Circumcircle";
            this.tb_Planes_Filter_Max_Circumcircle.Size = new System.Drawing.Size(57, 22);
            this.tb_Planes_Filter_Max_Circumcircle.TabIndex = 32;
            this.tb_Planes_Filter_Max_Circumcircle.Text = "0";
            this.tt_Main.SetToolTip(this.tb_Planes_Filter_Max_Circumcircle, "Set the maximum path midpoint circumcircle here.");
            this.tb_Planes_Filter_Max_Circumcircle.Value = global::AirScout.Properties.Settings.Default.Planes_Filter_Max_Circumcircle;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(121, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "km";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "Max. Circ.:";
            // 
            // cb_Planes_Filter_Min_Cat
            // 
            this.cb_Planes_Filter_Min_Cat.AllowDrop = true;
            this.cb_Planes_Filter_Min_Cat.BackColor = System.Drawing.Color.FloralWhite;
            this.cb_Planes_Filter_Min_Cat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Planes_Filter_Min_Cat.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Planes_Filter_Min_Cat.FormattingEnabled = true;
            this.cb_Planes_Filter_Min_Cat.Location = new System.Drawing.Point(59, 60);
            this.cb_Planes_Filter_Min_Cat.Name = "cb_Planes_Filter_Min_Cat";
            this.cb_Planes_Filter_Min_Cat.Size = new System.Drawing.Size(77, 24);
            this.cb_Planes_Filter_Min_Cat.TabIndex = 28;
            this.tt_Main.SetToolTip(this.cb_Planes_Filter_Min_Cat, "Set the minimum category (wake) here. \r\nSo far there are 4 categories:\r\n\r\n(L)ight" +
        "\r\n(M)edium\r\n(H)eavy\r\n(Super)\r\n\r\nPlanes with lower categories than selected are n" +
        "ot shown on the map.");
            this.cb_Planes_Filter_Min_Cat.SelectedIndexChanged += new System.EventHandler(this.cb_Planes_Filter_Min_Category_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Min. Cat:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(119, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "m";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Min. Alt.:";
            // 
            // il_Sat
            // 
            this.il_Sat.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_Sat.ImageStream")));
            this.il_Sat.TransparentColor = System.Drawing.Color.Transparent;
            this.il_Sat.Images.SetKeyName(0, "ISS.png");
            // 
            // bw_WinTestReceive
            // 
            this.bw_WinTestReceive.WorkerReportsProgress = true;
            this.bw_WinTestReceive.WorkerSupportsCancellation = true;
            this.bw_WinTestReceive.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_WinTestReceive_DoWork);
            this.bw_WinTestReceive.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_WinTestReceive_ProgressChanged);
            this.bw_WinTestReceive.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_WinTestReceive_RunWorkerCompleted);
            // 
            // il_Planes_M
            // 
            this.il_Planes_M.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Planes_M.ImageSize = new System.Drawing.Size(24, 24);
            this.il_Planes_M.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bw_SpecLab_Receive
            // 
            this.bw_SpecLab_Receive.WorkerReportsProgress = true;
            this.bw_SpecLab_Receive.WorkerSupportsCancellation = true;
            this.bw_SpecLab_Receive.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_SpecLab_Receive_DoWork);
            this.bw_SpecLab_Receive.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_SpecLab_Receive_ProgressChanged);
            this.bw_SpecLab_Receive.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_SpecLab_Receive_RunWorkerCompleted);
            // 
            // sc_Main
            // 
            this.sc_Main.BackColor = System.Drawing.Color.LightGray;
            this.sc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc_Main.Location = new System.Drawing.Point(0, 0);
            this.sc_Main.Name = "sc_Main";
            // 
            // sc_Main.Panel1
            // 
            this.sc_Main.Panel1.Controls.Add(this.sc_Map);
            // 
            // sc_Main.Panel2
            // 
            this.sc_Main.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.sc_Main.Panel2.Controls.Add(this.tc_Control);
            this.sc_Main.Panel2.Controls.Add(this.gb_Map_Zoom);
            this.sc_Main.Panel2.Controls.Add(this.pa_CommonInfo);
            this.sc_Main.Panel2.Controls.Add(this.gb_Map_Buttons);
            this.sc_Main.Panel2MinSize = 152;
            this.sc_Main.Size = new System.Drawing.Size(1008, 706);
            this.sc_Main.SplitterDistance = 852;
            this.sc_Main.SplitterWidth = 5;
            this.sc_Main.TabIndex = 24;
            this.sc_Main.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.sc_Main_SplitterMoved);
            // 
            // tc_Control
            // 
            this.tc_Control.Controls.Add(this.tp_Control_Single);
            this.tc_Control.Controls.Add(this.tp_Control_Multi);
            this.tc_Control.Controls.Add(this.tp_Control_Options);
            this.tc_Control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_Control.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tc_Control.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tc_Control.Location = new System.Drawing.Point(0, 100);
            this.tc_Control.Name = "tc_Control";
            this.tc_Control.SelectedIndex = 0;
            this.tc_Control.Size = new System.Drawing.Size(152, 404);
            this.tc_Control.TabIndex = 60;
            this.tc_Control.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tc_Control_DrawItem);
            this.tc_Control.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tc_Control_Selecting);
            // 
            // tp_Control_Single
            // 
            this.tp_Control_Single.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Control_Single.Controls.Add(this.gb_Map_Info);
            this.tp_Control_Single.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tp_Control_Single.Location = new System.Drawing.Point(4, 22);
            this.tp_Control_Single.Name = "tp_Control_Single";
            this.tp_Control_Single.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Control_Single.Size = new System.Drawing.Size(144, 378);
            this.tp_Control_Single.TabIndex = 0;
            this.tp_Control_Single.Text = "Single";
            this.tp_Control_Single.Enter += new System.EventHandler(this.tp_Control_Single_Enter);
            // 
            // gb_Map_Info
            // 
            this.gb_Map_Info.Controls.Add(this.cb_DXCall);
            this.gb_Map_Info.Controls.Add(this.cb_DXLoc);
            this.gb_Map_Info.Controls.Add(this.cb_MyLoc);
            this.gb_Map_Info.Controls.Add(this.cb_MyCall);
            this.gb_Map_Info.Controls.Add(this.tb_QTF);
            this.gb_Map_Info.Controls.Add(this.label6);
            this.gb_Map_Info.Controls.Add(this.tb_QRB);
            this.gb_Map_Info.Controls.Add(this.label16);
            this.gb_Map_Info.Controls.Add(this.label17);
            this.gb_Map_Info.Controls.Add(this.label18);
            this.gb_Map_Info.Controls.Add(this.label19);
            this.gb_Map_Info.Controls.Add(this.label20);
            this.gb_Map_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Map_Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map_Info.Location = new System.Drawing.Point(3, 3);
            this.gb_Map_Info.MinimumSize = new System.Drawing.Size(137, 0);
            this.gb_Map_Info.Name = "gb_Map_Info";
            this.gb_Map_Info.Size = new System.Drawing.Size(138, 275);
            this.gb_Map_Info.TabIndex = 58;
            this.gb_Map_Info.TabStop = false;
            this.gb_Map_Info.Text = "Info";
            // 
            // cb_DXCall
            // 
            this.cb_DXCall.BackColor = System.Drawing.SystemColors.Window;
            this.cb_DXCall.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cb_DXCall.ErrorBackColor = System.Drawing.Color.Red;
            this.cb_DXCall.ErrorForeColor = System.Drawing.Color.White;
            this.cb_DXCall.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DXCall.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_DXCall.FormattingEnabled = true;
            this.cb_DXCall.Location = new System.Drawing.Point(3, 113);
            this.cb_DXCall.Name = "cb_DXCall";
            this.cb_DXCall.Size = new System.Drawing.Size(133, 24);
            this.cb_DXCall.Sorted = true;
            this.cb_DXCall.TabIndex = 24;
            this.cb_DXCall.DropDown += new System.EventHandler(this.cb_DXCall_DropDown);
            this.cb_DXCall.SelectedIndexChanged += new System.EventHandler(this.cb_DXCall_SelectedIndexChanged);
            this.cb_DXCall.TextChanged += new System.EventHandler(this.cb_DXCall_TextChanged);
            // 
            // cb_DXLoc
            // 
            this.cb_DXLoc.AutoLength = false;
            this.cb_DXLoc.BackColor = System.Drawing.SystemColors.Window;
            this.cb_DXLoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cb_DXLoc.DataBindings.Add(new System.Windows.Forms.Binding("SmallLettersForSubsquares", global::AirScout.Properties.Settings.Default, "Locator_SmallLettersForSubsquares", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_DXLoc.ErrorBackColor = System.Drawing.Color.Red;
            this.cb_DXLoc.ErrorForeColor = System.Drawing.Color.White;
            this.cb_DXLoc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DXLoc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_DXLoc.FormattingEnabled = true;
            this.cb_DXLoc.GeoLocation = gPoint1;
            this.cb_DXLoc.Location = new System.Drawing.Point(3, 154);
            this.cb_DXLoc.Name = "cb_DXLoc";
            this.cb_DXLoc.Precision = 3;
            this.cb_DXLoc.SilentItemChange = true;
            this.cb_DXLoc.Size = new System.Drawing.Size(133, 24);
            this.cb_DXLoc.SmallLettersForSubsquares = global::AirScout.Properties.Settings.Default.Locator_SmallLettersForSubsquares;
            this.cb_DXLoc.TabIndex = 23;
            this.cb_DXLoc.DropDown += new System.EventHandler(this.cb_DXLoc_DropDown);
            this.cb_DXLoc.SelectedIndexChanged += new System.EventHandler(this.cb_DXLoc_SelectedIndexChanged);
            this.cb_DXLoc.SelectionChangeCommitted += new System.EventHandler(this.cb_DXLoc_SelectionChangeCommittedWithNoUpdate);
            this.cb_DXLoc.TextChanged += new System.EventHandler(this.cb_DXLoc_TextChanged);
            // 
            // cb_MyLoc
            // 
            this.cb_MyLoc.AutoLength = false;
            this.cb_MyLoc.BackColor = System.Drawing.SystemColors.Window;
            this.cb_MyLoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cb_MyLoc.DataBindings.Add(new System.Windows.Forms.Binding("SmallLettersForSubsquares", global::AirScout.Properties.Settings.Default, "Locator_SmallLettersForSubsquares", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_MyLoc.ErrorBackColor = System.Drawing.Color.Red;
            this.cb_MyLoc.ErrorForeColor = System.Drawing.Color.White;
            this.cb_MyLoc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_MyLoc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_MyLoc.FormattingEnabled = true;
            this.cb_MyLoc.GeoLocation = gPoint2;
            this.cb_MyLoc.Location = new System.Drawing.Point(3, 71);
            this.cb_MyLoc.Name = "cb_MyLoc";
            this.cb_MyLoc.Precision = 3;
            this.cb_MyLoc.SilentItemChange = true;
            this.cb_MyLoc.Size = new System.Drawing.Size(133, 24);
            this.cb_MyLoc.SmallLettersForSubsquares = global::AirScout.Properties.Settings.Default.Locator_SmallLettersForSubsquares;
            this.cb_MyLoc.TabIndex = 22;
            this.cb_MyLoc.DropDown += new System.EventHandler(this.cb_MyLoc_DropDown);
            this.cb_MyLoc.SelectedIndexChanged += new System.EventHandler(this.cb_MyLoc_SelectedIndexChanged);
            this.cb_MyLoc.SelectionChangeCommitted += new System.EventHandler(this.cb_MyLoc_SelectionChangeCommittedWithNoUpdate);
            this.cb_MyLoc.TextChanged += new System.EventHandler(this.cb_MyLoc_TextChanged);
            // 
            // cb_MyCall
            // 
            this.cb_MyCall.BackColor = System.Drawing.SystemColors.Window;
            this.cb_MyCall.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cb_MyCall.ErrorBackColor = System.Drawing.Color.Red;
            this.cb_MyCall.ErrorForeColor = System.Drawing.Color.White;
            this.cb_MyCall.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_MyCall.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_MyCall.FormattingEnabled = true;
            this.cb_MyCall.Location = new System.Drawing.Point(3, 34);
            this.cb_MyCall.Name = "cb_MyCall";
            this.cb_MyCall.Size = new System.Drawing.Size(133, 24);
            this.cb_MyCall.Sorted = true;
            this.cb_MyCall.TabIndex = 21;
            this.cb_MyCall.DropDown += new System.EventHandler(this.cb_MyCall_DropDown);
            this.cb_MyCall.SelectedIndexChanged += new System.EventHandler(this.cb_MyCall_SelectedIndexChanged);
            this.cb_MyCall.TextChanged += new System.EventHandler(this.cb_MyCall_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "QTF";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(2, 181);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "QRB";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 139);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "DX Loc";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(2, 97);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(42, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "DX Call";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(2, 58);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(42, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "My Loc";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(3, 18);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "My Call";
            // 
            // tp_Control_Multi
            // 
            this.tp_Control_Multi.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Control_Multi.Controls.Add(this.lv_Control_Watchlist);
            this.tp_Control_Multi.Controls.Add(this.btn_Control_Manage_Watchlist);
            this.tp_Control_Multi.Location = new System.Drawing.Point(4, 22);
            this.tp_Control_Multi.Name = "tp_Control_Multi";
            this.tp_Control_Multi.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Control_Multi.Size = new System.Drawing.Size(143, 378);
            this.tp_Control_Multi.TabIndex = 1;
            this.tp_Control_Multi.Text = "Multi";
            this.tp_Control_Multi.Enter += new System.EventHandler(this.tp_Control_Multi_Enter);
            // 
            // lv_Control_Watchlist
            // 
            this.lv_Control_Watchlist.CheckBoxes = true;
            this.lv_Control_Watchlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_Control_Watchlist_Call,
            this.ch_Control_Watchlist_Loc});
            this.lv_Control_Watchlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_Control_Watchlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_Control_Watchlist.FullRowSelect = true;
            this.lv_Control_Watchlist.GridLines = true;
            this.lv_Control_Watchlist.HideSelection = false;
            this.lv_Control_Watchlist.Location = new System.Drawing.Point(3, 3);
            this.lv_Control_Watchlist.Name = "lv_Control_Watchlist";
            this.lv_Control_Watchlist.OwnerDraw = true;
            this.lv_Control_Watchlist.Size = new System.Drawing.Size(137, 349);
            this.lv_Control_Watchlist.TabIndex = 1;
            this.tt_Control_Watchlist.SetToolTip(this.lv_Control_Watchlist, "Watchlist");
            this.lv_Control_Watchlist.UseCompatibleStateImageBehavior = false;
            this.lv_Control_Watchlist.View = System.Windows.Forms.View.Details;
            this.lv_Control_Watchlist.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lv_Control_Watchlist_ColumnClick);
            this.lv_Control_Watchlist.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.lv_Control_Watchlist_ColumnWidthChanged);
            this.lv_Control_Watchlist.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lv_Control_Watchlist_DrawColumnHeader);
            this.lv_Control_Watchlist.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lv_Control_Watchlist_DrawItem);
            this.lv_Control_Watchlist.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lv_Control_Watchlist_DrawSubItem);
            this.lv_Control_Watchlist.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lv_Control_Watchlist_ItemCheck);
            this.lv_Control_Watchlist.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lv_Control_Watchlist_ItemChecked);
            this.lv_Control_Watchlist.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.lv_Control_Watchlist_ItemMouseHover);
            this.lv_Control_Watchlist.SelectedIndexChanged += new System.EventHandler(this.lv_Control_Watchlist_SelectedIndexChanged);
            this.lv_Control_Watchlist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lv_Control_Watchlist_MouseMove);
            this.lv_Control_Watchlist.Resize += new System.EventHandler(this.lv_Control_Watchlist_Resize);
            // 
            // ch_Control_Watchlist_Call
            // 
            this.ch_Control_Watchlist_Call.Text = "   Call";
            this.ch_Control_Watchlist_Call.Width = 86;
            // 
            // ch_Control_Watchlist_Loc
            // 
            this.ch_Control_Watchlist_Loc.Text = "Loc";
            this.ch_Control_Watchlist_Loc.Width = 47;
            // 
            // btn_Control_Manage_Watchlist
            // 
            this.btn_Control_Manage_Watchlist.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Control_Manage_Watchlist.Location = new System.Drawing.Point(3, 352);
            this.btn_Control_Manage_Watchlist.Name = "btn_Control_Manage_Watchlist";
            this.btn_Control_Manage_Watchlist.Size = new System.Drawing.Size(137, 23);
            this.btn_Control_Manage_Watchlist.TabIndex = 0;
            this.btn_Control_Manage_Watchlist.Text = "Manage Watchlist";
            this.btn_Control_Manage_Watchlist.UseVisualStyleBackColor = true;
            this.btn_Control_Manage_Watchlist.Click += new System.EventHandler(this.btn_Control_Manage_Watchlist_Click);
            // 
            // tp_Control_Options
            // 
            this.tp_Control_Options.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Control_Options.Controls.Add(this.gb_Map_Alarms);
            this.tp_Control_Options.Controls.Add(this.gb_Map_Filter);
            this.tp_Control_Options.Location = new System.Drawing.Point(4, 22);
            this.tp_Control_Options.Name = "tp_Control_Options";
            this.tp_Control_Options.Size = new System.Drawing.Size(143, 378);
            this.tp_Control_Options.TabIndex = 2;
            this.tp_Control_Options.Text = "Opt";
            this.tp_Control_Options.Enter += new System.EventHandler(this.tp_Control_Options_Enter);
            // 
            // pa_CommonInfo
            // 
            this.pa_CommonInfo.Controls.Add(this.label1);
            this.pa_CommonInfo.Controls.Add(this.tb_UTC);
            this.pa_CommonInfo.Controls.Add(this.label8);
            this.pa_CommonInfo.Controls.Add(this.cb_Band);
            this.pa_CommonInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pa_CommonInfo.Location = new System.Drawing.Point(0, 0);
            this.pa_CommonInfo.Name = "pa_CommonInfo";
            this.pa_CommonInfo.Size = new System.Drawing.Size(152, 100);
            this.pa_CommonInfo.TabIndex = 59;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "UTC";
            // 
            // tb_UTC
            // 
            this.tb_UTC.BackColor = System.Drawing.Color.LightSalmon;
            this.tb_UTC.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_UTC.ForeColor = System.Drawing.Color.White;
            this.tb_UTC.Location = new System.Drawing.Point(4, 19);
            this.tb_UTC.Name = "tb_UTC";
            this.tb_UTC.ReadOnly = true;
            this.tb_UTC.Size = new System.Drawing.Size(133, 22);
            this.tb_UTC.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Band";
            // 
            // gb_Map_Buttons
            // 
            this.gb_Map_Buttons.BackColor = System.Drawing.SystemColors.Control;
            this.gb_Map_Buttons.Controls.Add(this.btn_Map_PlayPause);
            this.gb_Map_Buttons.Controls.Add(this.btn_Map_Save);
            this.gb_Map_Buttons.Controls.Add(this.btn_Options);
            this.gb_Map_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_Map_Buttons.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map_Buttons.Location = new System.Drawing.Point(0, 583);
            this.gb_Map_Buttons.Name = "gb_Map_Buttons";
            this.gb_Map_Buttons.Size = new System.Drawing.Size(152, 123);
            this.gb_Map_Buttons.TabIndex = 57;
            this.gb_Map_Buttons.TabStop = false;
            this.gb_Map_Buttons.Text = "Control";
            // 
            // bw_Track
            // 
            this.bw_Track.WorkerReportsProgress = true;
            this.bw_Track.WorkerSupportsCancellation = true;
            this.bw_Track.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Track_DoWork);
            this.bw_Track.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Track_ProgressChanged);
            this.bw_Track.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Track_RunWorkerCompleted);
            // 
            // il_Planes_H
            // 
            this.il_Planes_H.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Planes_H.ImageSize = new System.Drawing.Size(36, 36);
            this.il_Planes_H.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // il_Planes_L
            // 
            this.il_Planes_L.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Planes_L.ImageSize = new System.Drawing.Size(16, 16);
            this.il_Planes_L.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // il_Planes_S
            // 
            this.il_Planes_S.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Planes_S.ImageSize = new System.Drawing.Size(48, 48);
            this.il_Planes_S.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bw_Webserver
            // 
            this.bw_Webserver.WorkerReportsProgress = true;
            this.bw_Webserver.WorkerSupportsCancellation = true;
            this.bw_Webserver.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Webserver_DoWork);
            this.bw_Webserver.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Webserver_ProgressChanged);
            // 
            // bw_JSONWriter
            // 
            this.bw_JSONWriter.WorkerReportsProgress = true;
            this.bw_JSONWriter.WorkerSupportsCancellation = true;
            this.bw_JSONWriter.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_JSONWriter_DoWork);
            // 
            // bw_NewsFeed
            // 
            this.bw_NewsFeed.WorkerReportsProgress = true;
            this.bw_NewsFeed.WorkerSupportsCancellation = true;
            this.bw_NewsFeed.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_NewsFeed_DoWork);
            this.bw_NewsFeed.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_NewsFeed_ProgressChanged);
            this.bw_NewsFeed.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_NewsFeed_RunWorkerCompleted);
            // 
            // il_Airports
            // 
            this.il_Airports.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Airports.ImageSize = new System.Drawing.Size(16, 16);
            this.il_Airports.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bw_HistoryDownloader
            // 
            this.bw_HistoryDownloader.WorkerReportsProgress = true;
            this.bw_HistoryDownloader.WorkerSupportsCancellation = true;
            this.bw_HistoryDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_HistoryDownloader_DoWork);
            this.bw_HistoryDownloader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_HistoryDownloader_ProgressChanged);
            this.bw_HistoryDownloader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_HistoryDownloader_RunWorkerCompleted);
            // 
            // ti_Startup
            // 
            this.ti_Startup.Interval = 5000;
            this.ti_Startup.Tick += new System.EventHandler(this.ti_Startup_Tick);
            // 
            // ti_ShowLegends
            // 
            this.ti_ShowLegends.Interval = 5000;
            this.ti_ShowLegends.Tick += new System.EventHandler(this.ti_ShowLegends_Tick);
            // 
            // bw_Analysis_DataGetter
            // 
            this.bw_Analysis_DataGetter.WorkerReportsProgress = true;
            this.bw_Analysis_DataGetter.WorkerSupportsCancellation = true;
            this.bw_Analysis_DataGetter.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Analysis_DataGetter_DoWork);
            this.bw_Analysis_DataGetter.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Analysis_DataGetter_ProgressChanged);
            this.bw_Analysis_DataGetter.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Analysis_DataGetter_RunWorkerCompleted);
            // 
            // bw_Analysis_FileSaver
            // 
            this.bw_Analysis_FileSaver.WorkerReportsProgress = true;
            this.bw_Analysis_FileSaver.WorkerSupportsCancellation = true;
            this.bw_Analysis_FileSaver.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Analysis_FileSaver_DoWork);
            this.bw_Analysis_FileSaver.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Analysis_FileSaver_ProgressChanged);
            this.bw_Analysis_FileSaver.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Analysis_FileSaver_RunWorkerCompleted);
            // 
            // bw_Analysis_FileLoader
            // 
            this.bw_Analysis_FileLoader.WorkerReportsProgress = true;
            this.bw_Analysis_FileLoader.WorkerSupportsCancellation = true;
            this.bw_Analysis_FileLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Analysis_FileLoader_DoWork);
            this.bw_Analysis_FileLoader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Analysis_FileLoader_ProgressChanged);
            this.bw_Analysis_FileLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Analysis_FileLoader_RunWorkerCompleted);
            // 
            // bw_AirportMapper
            // 
            this.bw_AirportMapper.WorkerReportsProgress = true;
            this.bw_AirportMapper.WorkerSupportsCancellation = true;
            this.bw_AirportMapper.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_AirportMapper_DoWork);
            this.bw_AirportMapper.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_AirportMapper_ProgressChanged);
            this.bw_AirportMapper.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_AirportMapper_RunWorkerCompleted);
            // 
            // gm_Cache
            // 
            this.gm_Cache.Bearing = 0F;
            this.gm_Cache.CanDragMap = true;
            this.gm_Cache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Cache.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Cache.GrayScaleMode = false;
            this.gm_Cache.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Cache.LevelsKeepInMemmory = 5;
            this.gm_Cache.Location = new System.Drawing.Point(3, 3);
            this.gm_Cache.MarkersEnabled = true;
            this.gm_Cache.MaxZoom = 2;
            this.gm_Cache.MinZoom = 2;
            this.gm_Cache.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Cache.Name = "gm_Cache";
            this.gm_Cache.NegativeMode = false;
            this.gm_Cache.PolygonsEnabled = true;
            this.gm_Cache.RetryLoadTile = 0;
            this.gm_Cache.RoutesEnabled = true;
            this.gm_Cache.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Cache.ShowTileGridLines = false;
            this.gm_Cache.Size = new System.Drawing.Size(838, 309);
            this.gm_Cache.TabIndex = 29;
            this.gm_Cache.Zoom = 0D;
            // 
            // MapDlg
            // 
            this.AcceptButton = this.btn_Map_PlayPause;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.sc_Main);
            this.Controls.Add(this.ss_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MapDlg";
            this.Text = "AirScout - Aircraft Scatter Prediction (c) 2013 by DL2ALF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapDlg_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapDlg_FormClosed);
            this.Load += new System.EventHandler(this.MapDlg_Load);
            this.SizeChanged += new System.EventHandler(this.MapDlg_SizeChanged);
            this.Resize += new System.EventHandler(this.MapDlg_Resize);
            this.sc_Map.Panel1.ResumeLayout(false);
            this.sc_Map.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sc_Map)).EndInit();
            this.sc_Map.ResumeLayout(false);
            this.tc_Map.ResumeLayout(false);
            this.tp_Map.ResumeLayout(false);
            this.tc_Main.ResumeLayout(false);
            this.tp_Spectrum.ResumeLayout(false);
            this.gb_Spectrum_NearestInfo.ResumeLayout(false);
            this.gb_Spectrum_NearestInfo.PerformLayout();
            this.gb_Spectrum_Status.ResumeLayout(false);
            this.gb_Spectrum_Status.PerformLayout();
            this.gb_NearestPlaneMap.ResumeLayout(false);
            this.tp_Analysis.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.gb_Analysis_Player.ResumeLayout(false);
            this.gb_Analysis_Controls.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gb_Analysis_Functions.ResumeLayout(false);
            this.gb_Analysis_Database.ResumeLayout(false);
            this.gb_Analysis_Database.PerformLayout();
            this.gb_Analysis_Mode.ResumeLayout(false);
            this.gb_Analysis_Time.ResumeLayout(false);
            this.gb_Analysis_Time.PerformLayout();
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.gb_Map_Alarms.ResumeLayout(false);
            this.gb_Map_Alarms.PerformLayout();
            this.gb_Map_Zoom.ResumeLayout(false);
            this.pa_Map_Zoom.ResumeLayout(false);
            this.pa_Map_Zoom.PerformLayout();
            this.gb_Map_Filter.ResumeLayout(false);
            this.pa_Planes_Filter.ResumeLayout(false);
            this.pa_Planes_Filter.PerformLayout();
            this.sc_Main.Panel1.ResumeLayout(false);
            this.sc_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sc_Main)).EndInit();
            this.sc_Main.ResumeLayout(false);
            this.tc_Control.ResumeLayout(false);
            this.tp_Control_Single.ResumeLayout(false);
            this.gb_Map_Info.ResumeLayout(false);
            this.gb_Map_Info.PerformLayout();
            this.tp_Control_Multi.ResumeLayout(false);
            this.tp_Control_Options.ResumeLayout(false);
            this.pa_CommonInfo.ResumeLayout(false);
            this.pa_CommonInfo.PerformLayout();
            this.gb_Map_Buttons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer ti_Progress;
        private System.Windows.Forms.ImageList il_Main;
        private System.Windows.Forms.SplitContainer sc_Map;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.ToolTip tt_Main;
        private System.Windows.Forms.ImageList il_Sat;
        private System.ComponentModel.BackgroundWorker bw_WinTestReceive;
        public System.Windows.Forms.ImageList il_Planes_M;
        private System.ComponentModel.BackgroundWorker bw_SpecLab_Receive;
        private System.Windows.Forms.TabControl tc_Main;
        private System.Windows.Forms.TabPage tp_Elevation;
        private System.Windows.Forms.TabPage tp_Spectrum;
        private System.Windows.Forms.TextBox tb_Spectrum_Status;
        private System.Windows.Forms.SplitContainer sc_Main;
        private System.Windows.Forms.GroupBox gb_Map_Buttons;
        private System.Windows.Forms.Button btn_Map_PlayPause;
        private System.Windows.Forms.Button btn_Map_Save;
        private System.Windows.Forms.Button btn_Options;
        private System.ComponentModel.BackgroundWorker bw_Track;
        public System.Windows.Forms.ImageList il_Planes_H;
        public System.Windows.Forms.ImageList il_Planes_L;
        public System.Windows.Forms.ImageList il_Planes_S;
        private System.ComponentModel.BackgroundWorker bw_Webserver;
        private System.ComponentModel.BackgroundWorker bw_JSONWriter;
        private System.ComponentModel.BackgroundWorker bw_NewsFeed;
        public System.Windows.Forms.ImageList il_Airports;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Dummy;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_SRTM1;
        private System.Windows.Forms.TabPage tp_Analysis;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gb_Analysis_Time;
        private System.ComponentModel.BackgroundWorker bw_HistoryDownloader;
        private System.Windows.Forms.GroupBox gb_Analysis_Player;
        private System.Windows.Forms.GroupBox gb_Analysis_Controls;
        private System.Windows.Forms.Button btn_Analysis_FastForward;
        private System.Windows.Forms.Button btn_Analysis_Forward;
        private System.Windows.Forms.Button btn_Analysis_Pause;
        private System.Windows.Forms.Button btn_Analysis_Back;
        private System.Windows.Forms.Button btn_Analysis_Rewind;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_Analysis_Stepwidth;
        private System.Windows.Forms.TextBox tb_Analysis_Time;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Calculations;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_GLOBE;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_SRTM3;
        private System.Windows.Forms.Timer ti_Startup;
        private System.Windows.Forms.Timer ti_ShowLegends;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_Stations;
        private System.Windows.Forms.ComboBox cb_Band;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_UTC;
        private System.Windows.Forms.ToolTip tt_Control_Watchlist;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_Aircraft;
        private System.Windows.Forms.TabControl tc_Control;
        private System.Windows.Forms.TabPage tp_Control_Single;
        private System.Windows.Forms.GroupBox gb_Map_Info;
        private ScoutBase.Core.CallsignComboBox cb_DXCall;
        private ScoutBase.Core.LocatorComboBox cb_DXLoc;
        private ScoutBase.Core.LocatorComboBox cb_MyLoc;
        private ScoutBase.Core.CallsignComboBox cb_MyCall;
        private System.Windows.Forms.TextBox tb_QTF;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_QRB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage tp_Control_Multi;
        private System.Windows.Forms.ListView lv_Control_Watchlist;
        private System.Windows.Forms.ColumnHeader ch_Control_Watchlist_Call;
        private System.Windows.Forms.ColumnHeader ch_Control_Watchlist_Loc;
        private System.Windows.Forms.Button btn_Control_Manage_Watchlist;
        private System.Windows.Forms.TabPage tp_Control_Options;
        private System.Windows.Forms.GroupBox gb_Map_Alarms;
        private System.Windows.Forms.CheckBox cb_Alarms_Activate;
        private System.Windows.Forms.GroupBox gb_Map_Zoom;
        private System.Windows.Forms.Panel pa_Map_Zoom;
        private System.Windows.Forms.CheckBox cb_AutoCenter;
        private System.Windows.Forms.TextBox tb_Zoom;
        private System.Windows.Forms.Button btn_Zoom_Out;
        private System.Windows.Forms.Button btn_Zoom_In;
        private System.Windows.Forms.GroupBox gb_Map_Filter;
        private System.Windows.Forms.Panel pa_Planes_Filter;
        private ScoutBase.Core.Int32TextBox tb_Planes_Filter_Min_Alt;
        private ScoutBase.Core.Int32TextBox tb_Planes_Filter_Max_Circumcircle;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cb_Planes_Filter_Min_Cat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pa_CommonInfo;
        private System.Windows.Forms.Label label1;
        private CustomScrollBar.ScrollBarEx sb_Analysis_Play;
        private System.Windows.Forms.DateTimePicker dtp_Analysis_MaxValue;
        private System.Windows.Forms.DateTimePicker dtp_Analysis_MinValue;
        private System.Windows.Forms.GroupBox gb_Analysis_Functions;
        private System.Windows.Forms.Button btn_Analysis_CrossingHistory;
        private System.Windows.Forms.Button btn_Analysis_Path_SaveToFile;
        private System.Windows.Forms.Button btn_Analysis_Planes_ShowTraffic;
        private System.Windows.Forms.GroupBox gb_Analysis_Database;
        private System.Windows.Forms.TextBox tb_Analysis_Status;
        private System.Windows.Forms.Button btn_Analysis_Planes_History;
        private System.Windows.Forms.Button btn_Analysis_Planes_Clear;
        private System.Windows.Forms.Button btn_Analysis_Planes_Save;
        private System.Windows.Forms.Button btn_Analysis_Planes_Load;
        private System.Windows.Forms.GroupBox gb_Analysis_Mode;
        private System.Windows.Forms.Button btn_Analysis_OFF;
        private System.Windows.Forms.Button btn_Analysis_ON;
        private System.Windows.Forms.GroupBox gb_NearestPlaneMap;
        private System.Windows.Forms.GroupBox gb_Spectrum;
        private System.Windows.Forms.GroupBox gb_Spectrum_Status;
        private GMap.NET.WindowsForms.GMapControl gm_Nearest;
        private System.Windows.Forms.GroupBox gb_Spectrum_NearestInfo;
        private System.Windows.Forms.Label lbl_Nearest_Type;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_Nearest_Cat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_Nearest_Angle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_Nearest_Alt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_Nearest_Call;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lbl_Nearest_Dist;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btn_Analysis_Plane_History;
        private System.ComponentModel.BackgroundWorker bw_Analysis_DataGetter;
        private System.ComponentModel.BackgroundWorker bw_Analysis_FileSaver;
        private System.ComponentModel.BackgroundWorker bw_Analysis_FileLoader;
        private System.ComponentModel.BackgroundWorker bw_AirportMapper;
        private System.Windows.Forms.TabControl tc_Map;
        private System.Windows.Forms.TabPage tp_Map;
        private AquaControls.AquaGauge ag_Azimuth;
        private AquaControls.AquaGauge ag_Elevation;
        private GMap.NET.WindowsForms.GMapControl gm_Main;
        private System.Windows.Forms.TabPage tp_News;
        private GMap.NET.WindowsForms.GMapControl gm_Cache;
    }
}

