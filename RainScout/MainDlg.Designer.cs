namespace RainScout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Dummy = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Locations = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Radar = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database = new System.Windows.Forms.ToolStripStatusLabel();
            this.spl_Main = new System.Windows.Forms.SplitContainer();
            this.spl_Map = new System.Windows.Forms.SplitContainer();
            this.gm_Main = new GMap.NET.WindowsForms.GMapControl();
            this.gb_Filter = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ud_Filter_MaxDistance = new System.Windows.Forms.NumericUpDown();
            this.label222 = new System.Windows.Forms.Label();
            this.ud_Filter_MinDistance = new System.Windows.Forms.NumericUpDown();
            this.cb_Filter_VisibleScpOnly = new System.Windows.Forms.CheckBox();
            this.gb_Map = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_Map_Distances = new System.Windows.Forms.CheckBox();
            this.cb_Map_Horizons = new System.Windows.Forms.CheckBox();
            this.cb_Map_Bounds = new System.Windows.Forms.CheckBox();
            this.cb_Map_Lightning = new System.Windows.Forms.CheckBox();
            this.cb_Map_CloudTops = new System.Windows.Forms.CheckBox();
            this.cb_Map_Intensity = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btn_Map_ZoomIn = new System.Windows.Forms.Button();
            this.btn_Map_ZoomOut = new System.Windows.Forms.Button();
            this.tb_Map_Zoom = new System.Windows.Forms.TextBox();
            this.pa_CommonInfo = new System.Windows.Forms.Panel();
            this.lbl_Radar_Timestamp = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_Radar = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.cb_ElevationModel = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.tb_UTC = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.cb_Band = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_Mouse_Lightning = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.tb_Mouse_Intensity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Mouse_CloudTop = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.tb_Mouse_Distance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Mouse_Bearing = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_Mouse_Loc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Mouse_Lon = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Mouse_Lat = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_Options = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.gb_DXData = new System.Windows.Forms.GroupBox();
            this.tb_DXData_Elevation = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.tb_DXData_Distance = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tb_DXData_Bearing = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tb_DXData_Loc = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tb_DXData_Lon = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tb_DXData_Lat = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.gb_Scp = new System.Windows.Forms.GroupBox();
            this.tb_Scp_Lightning = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.tb_Scp_Intensity = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.tb_Scp_CloudTop = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.btn_scp_Clear = new System.Windows.Forms.Button();
            this.tb_Scp_Loc = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tb_Scp_Lon = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_Scp_Lat = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.gb_MyData = new System.Windows.Forms.GroupBox();
            this.tb_MyData_Height = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tb_MyData_Elevation = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_MyData_Distance = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_MyData_Bearing = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_MyData_Loc = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_MyDataLon = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_MyData_Lat = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bw_Horizons = new System.ComponentModel.BackgroundWorker();
            this.ti_Main = new System.Windows.Forms.Timer(this.components);
            this.bw_Stations = new System.ComponentModel.BackgroundWorker();
            this.bw_Locations = new System.ComponentModel.BackgroundWorker();
            this.bw_Radar = new System.ComponentModel.BackgroundWorker();
            this.tsl_Database_LED_Stations = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_GLOBE = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_SRTM3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsl_Database_LED_SRTM1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ss_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spl_Main)).BeginInit();
            this.spl_Main.Panel1.SuspendLayout();
            this.spl_Main.Panel2.SuspendLayout();
            this.spl_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spl_Map)).BeginInit();
            this.spl_Map.Panel1.SuspendLayout();
            this.spl_Map.Panel2.SuspendLayout();
            this.spl_Map.SuspendLayout();
            this.gb_Filter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Filter_MaxDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Filter_MinDistance)).BeginInit();
            this.gb_Map.SuspendLayout();
            this.pa_CommonInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_DXData.SuspendLayout();
            this.gb_Scp.SuspendLayout();
            this.gb_MyData.SuspendLayout();
            this.SuspendLayout();
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Main,
            this.tsl_Dummy,
            this.tsl_Locations,
            this.tsl_Radar,
            this.tsl_Database,
            this.tsl_Database_LED_Stations,
            this.tsl_Database_LED_GLOBE,
            this.tsl_Database_LED_SRTM3,
            this.tsl_Database_LED_SRTM1});
            this.ss_Main.Location = new System.Drawing.Point(0, 707);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(1008, 22);
            this.ss_Main.TabIndex = 3;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Main
            // 
            this.tsl_Main.BackColor = System.Drawing.SystemColors.Control;
            this.tsl_Main.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tsl_Main.Name = "tsl_Main";
            this.tsl_Main.Size = new System.Drawing.Size(39, 17);
            this.tsl_Main.Text = "Status";
            // 
            // tsl_Dummy
            // 
            this.tsl_Dummy.BackColor = System.Drawing.SystemColors.Control;
            this.tsl_Dummy.Name = "tsl_Dummy";
            this.tsl_Dummy.Size = new System.Drawing.Size(717, 17);
            this.tsl_Dummy.Spring = true;
            // 
            // tsl_Locations
            // 
            this.tsl_Locations.BackColor = System.Drawing.SystemColors.Control;
            this.tsl_Locations.Name = "tsl_Locations";
            this.tsl_Locations.Size = new System.Drawing.Size(58, 17);
            this.tsl_Locations.Text = "Locations";
            // 
            // tsl_Radar
            // 
            this.tsl_Radar.BackColor = System.Drawing.SystemColors.Control;
            this.tsl_Radar.Name = "tsl_Radar";
            this.tsl_Radar.Size = new System.Drawing.Size(37, 17);
            this.tsl_Radar.Text = "Radar";
            // 
            // tsl_Database
            // 
            this.tsl_Database.BackColor = System.Drawing.SystemColors.Control;
            this.tsl_Database.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tsl_Database.Name = "tsl_Database";
            this.tsl_Database.Size = new System.Drawing.Size(55, 17);
            this.tsl_Database.Text = "Database";
            // 
            // spl_Main
            // 
            this.spl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spl_Main.Location = new System.Drawing.Point(0, 0);
            this.spl_Main.Name = "spl_Main";
            this.spl_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spl_Main.Panel1
            // 
            this.spl_Main.Panel1.Controls.Add(this.spl_Map);
            // 
            // spl_Main.Panel2
            // 
            this.spl_Main.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.spl_Main.Panel2.Controls.Add(this.groupBox1);
            this.spl_Main.Panel2.Controls.Add(this.btn_Options);
            this.spl_Main.Panel2.Controls.Add(this.btn_Refresh);
            this.spl_Main.Panel2.Controls.Add(this.gb_DXData);
            this.spl_Main.Panel2.Controls.Add(this.gb_Scp);
            this.spl_Main.Panel2.Controls.Add(this.gb_MyData);
            this.spl_Main.Size = new System.Drawing.Size(1008, 707);
            this.spl_Main.SplitterDistance = 478;
            this.spl_Main.TabIndex = 4;
            // 
            // spl_Map
            // 
            this.spl_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spl_Map.Location = new System.Drawing.Point(0, 0);
            this.spl_Map.Name = "spl_Map";
            // 
            // spl_Map.Panel1
            // 
            this.spl_Map.Panel1.Controls.Add(this.gm_Main);
            // 
            // spl_Map.Panel2
            // 
            this.spl_Map.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.spl_Map.Panel2.Controls.Add(this.gb_Filter);
            this.spl_Map.Panel2.Controls.Add(this.gb_Map);
            this.spl_Map.Panel2.Controls.Add(this.pa_CommonInfo);
            this.spl_Map.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spl_Map.Size = new System.Drawing.Size(1008, 478);
            this.spl_Map.SplitterDistance = 854;
            this.spl_Map.TabIndex = 0;
            // 
            // gm_Main
            // 
            this.gm_Main.Bearing = 0F;
            this.gm_Main.CanDragMap = true;
            this.gm_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Main.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Main.GrayScaleMode = false;
            this.gm_Main.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Main.LevelsKeepInMemmory = 5;
            this.gm_Main.Location = new System.Drawing.Point(0, 0);
            this.gm_Main.MarkersEnabled = true;
            this.gm_Main.MaxZoom = 2;
            this.gm_Main.MinZoom = 2;
            this.gm_Main.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Main.Name = "gm_Main";
            this.gm_Main.NegativeMode = false;
            this.gm_Main.Opacity = 1D;
            this.gm_Main.PolygonsEnabled = true;
            this.gm_Main.RetryLoadTile = 0;
            this.gm_Main.RoutesEnabled = true;
            this.gm_Main.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Main.ShowTileGridLines = false;
            this.gm_Main.Size = new System.Drawing.Size(854, 478);
            this.gm_Main.TabIndex = 1;
            this.gm_Main.Zoom = 0D;
            this.gm_Main.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gm_Main_OnMarkerClick);
            this.gm_Main.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.gm_Main_OnMarkerEnter);
            this.gm_Main.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.gm_Main_OnMarkerLeave);
            this.gm_Main.OnMapDrag += new GMap.NET.MapDrag(this.gm_Main_OnMapDrag);
            this.gm_Main.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gm_Main_OnMapZoomChanged);
            this.gm_Main.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseClick);
            this.gm_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseDown);
            this.gm_Main.MouseEnter += new System.EventHandler(this.gm_Main_MouseEnter);
            this.gm_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseMove);
            this.gm_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gm_Main_MouseUp);
            // 
            // gb_Filter
            // 
            this.gb_Filter.Controls.Add(this.label3);
            this.gb_Filter.Controls.Add(this.ud_Filter_MaxDistance);
            this.gb_Filter.Controls.Add(this.label222);
            this.gb_Filter.Controls.Add(this.ud_Filter_MinDistance);
            this.gb_Filter.Controls.Add(this.cb_Filter_VisibleScpOnly);
            this.gb_Filter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Filter.Location = new System.Drawing.Point(0, 344);
            this.gb_Filter.Name = "gb_Filter";
            this.gb_Filter.Size = new System.Drawing.Size(150, 127);
            this.gb_Filter.TabIndex = 65;
            this.gb_Filter.TabStop = false;
            this.gb_Filter.Text = "Filter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Max. Distance";
            // 
            // ud_Filter_MaxDistance
            // 
            this.ud_Filter_MaxDistance.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::RainScout.Properties.Settings.Default, "Filter_MaxDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Filter_MaxDistance.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ud_Filter_MaxDistance.Location = new System.Drawing.Point(12, 99);
            this.ud_Filter_MaxDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ud_Filter_MaxDistance.Name = "ud_Filter_MaxDistance";
            this.ud_Filter_MaxDistance.Size = new System.Drawing.Size(120, 21);
            this.ud_Filter_MaxDistance.TabIndex = 12;
            this.ud_Filter_MaxDistance.Value = global::RainScout.Properties.Settings.Default.Filter_MaxDistance;
            // 
            // label222
            // 
            this.label222.AutoSize = true;
            this.label222.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label222.Location = new System.Drawing.Point(10, 39);
            this.label222.Name = "label222";
            this.label222.Size = new System.Drawing.Size(72, 13);
            this.label222.TabIndex = 11;
            this.label222.Text = "Min. Distance";
            // 
            // ud_Filter_MinDistance
            // 
            this.ud_Filter_MinDistance.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::RainScout.Properties.Settings.Default, "Filter_MinDistance", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Filter_MinDistance.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ud_Filter_MinDistance.Location = new System.Drawing.Point(13, 55);
            this.ud_Filter_MinDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ud_Filter_MinDistance.Name = "ud_Filter_MinDistance";
            this.ud_Filter_MinDistance.Size = new System.Drawing.Size(120, 21);
            this.ud_Filter_MinDistance.TabIndex = 1;
            this.ud_Filter_MinDistance.Value = global::RainScout.Properties.Settings.Default.Filter_MinDistance;
            // 
            // cb_Filter_VisibleScpOnly
            // 
            this.cb_Filter_VisibleScpOnly.AutoSize = true;
            this.cb_Filter_VisibleScpOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_Filter_VisibleScpOnly.Checked = global::RainScout.Properties.Settings.Default.Filter_VisibleScpOnly;
            this.cb_Filter_VisibleScpOnly.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Filter_VisibleScpOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Filter_VisibleScpOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Filter_VisibleScpOnly.Location = new System.Drawing.Point(8, 19);
            this.cb_Filter_VisibleScpOnly.Name = "cb_Filter_VisibleScpOnly";
            this.cb_Filter_VisibleScpOnly.Size = new System.Drawing.Size(126, 17);
            this.cb_Filter_VisibleScpOnly.TabIndex = 0;
            this.cb_Filter_VisibleScpOnly.Text = "Visible Scp Stns Only";
            this.cb_Filter_VisibleScpOnly.UseVisualStyleBackColor = true;
            // 
            // gb_Map
            // 
            this.gb_Map.Controls.Add(this.label26);
            this.gb_Map.Controls.Add(this.label4);
            this.gb_Map.Controls.Add(this.cb_Map_Distances);
            this.gb_Map.Controls.Add(this.cb_Map_Horizons);
            this.gb_Map.Controls.Add(this.cb_Map_Bounds);
            this.gb_Map.Controls.Add(this.cb_Map_Lightning);
            this.gb_Map.Controls.Add(this.cb_Map_CloudTops);
            this.gb_Map.Controls.Add(this.cb_Map_Intensity);
            this.gb_Map.Controls.Add(this.label15);
            this.gb_Map.Controls.Add(this.btn_Map_ZoomIn);
            this.gb_Map.Controls.Add(this.btn_Map_ZoomOut);
            this.gb_Map.Controls.Add(this.tb_Map_Zoom);
            this.gb_Map.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Map.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map.Location = new System.Drawing.Point(0, 187);
            this.gb_Map.Name = "gb_Map";
            this.gb_Map.Size = new System.Drawing.Size(150, 157);
            this.gb_Map.TabIndex = 64;
            this.gb_Map.TabStop = false;
            this.gb_Map.Text = "Map";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(113, 106);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(16, 16);
            this.label26.TabIndex = 18;
            this.label26.Text = "↓";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(113, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "↑";
            // 
            // cb_Map_Distances
            // 
            this.cb_Map_Distances.AutoSize = true;
            this.cb_Map_Distances.Checked = global::RainScout.Properties.Settings.Default.Map_Distances;
            this.cb_Map_Distances.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Map_Distances.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Map_Distances", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_Distances.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Map_Distances.Location = new System.Drawing.Point(12, 41);
            this.cb_Map_Distances.Name = "cb_Map_Distances";
            this.cb_Map_Distances.Size = new System.Drawing.Size(73, 17);
            this.cb_Map_Distances.TabIndex = 16;
            this.cb_Map_Distances.Text = "Distances";
            this.cb_Map_Distances.UseVisualStyleBackColor = true;
            this.cb_Map_Distances.CheckedChanged += new System.EventHandler(this.cb_Map_Distances_CheckedChanged);
            // 
            // cb_Map_Horizons
            // 
            this.cb_Map_Horizons.AutoSize = true;
            this.cb_Map_Horizons.Checked = global::RainScout.Properties.Settings.Default.Map_Horizons;
            this.cb_Map_Horizons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Map_Horizons.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Map_Horizons", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_Horizons.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Map_Horizons.Location = new System.Drawing.Point(12, 64);
            this.cb_Map_Horizons.Name = "cb_Map_Horizons";
            this.cb_Map_Horizons.Size = new System.Drawing.Size(67, 17);
            this.cb_Map_Horizons.TabIndex = 15;
            this.cb_Map_Horizons.Text = "Horizons";
            this.cb_Map_Horizons.UseVisualStyleBackColor = true;
            this.cb_Map_Horizons.CheckedChanged += new System.EventHandler(this.cb_Map_Horizons_CheckedChanged);
            // 
            // cb_Map_Bounds
            // 
            this.cb_Map_Bounds.AutoSize = true;
            this.cb_Map_Bounds.Checked = global::RainScout.Properties.Settings.Default.Map_Bounds;
            this.cb_Map_Bounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Map_Bounds.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Map_Bounds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_Bounds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Map_Bounds.Location = new System.Drawing.Point(12, 18);
            this.cb_Map_Bounds.Name = "cb_Map_Bounds";
            this.cb_Map_Bounds.Size = new System.Drawing.Size(62, 17);
            this.cb_Map_Bounds.TabIndex = 14;
            this.cb_Map_Bounds.Text = "Bounds";
            this.cb_Map_Bounds.UseVisualStyleBackColor = true;
            this.cb_Map_Bounds.CheckedChanged += new System.EventHandler(this.cb_Map_Bounds_CheckedChanged);
            // 
            // cb_Map_Lightning
            // 
            this.cb_Map_Lightning.AutoSize = true;
            this.cb_Map_Lightning.Checked = global::RainScout.Properties.Settings.Default.Map_Lightning;
            this.cb_Map_Lightning.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Map_Lightning", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_Lightning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Map_Lightning.Location = new System.Drawing.Point(12, 134);
            this.cb_Map_Lightning.Name = "cb_Map_Lightning";
            this.cb_Map_Lightning.Size = new System.Drawing.Size(69, 17);
            this.cb_Map_Lightning.TabIndex = 13;
            this.cb_Map_Lightning.Text = "Lightning";
            this.cb_Map_Lightning.UseVisualStyleBackColor = true;
            this.cb_Map_Lightning.CheckedChanged += new System.EventHandler(this.cb_Map_Lightning_CheckedChanged);
            // 
            // cb_Map_CloudTops
            // 
            this.cb_Map_CloudTops.AutoSize = true;
            this.cb_Map_CloudTops.Checked = global::RainScout.Properties.Settings.Default.Map_CloudTops;
            this.cb_Map_CloudTops.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Map_CloudTops", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_CloudTops.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Map_CloudTops.Location = new System.Drawing.Point(13, 111);
            this.cb_Map_CloudTops.Name = "cb_Map_CloudTops";
            this.cb_Map_CloudTops.Size = new System.Drawing.Size(77, 17);
            this.cb_Map_CloudTops.TabIndex = 12;
            this.cb_Map_CloudTops.Text = "CloudTops";
            this.cb_Map_CloudTops.UseVisualStyleBackColor = true;
            this.cb_Map_CloudTops.CheckedChanged += new System.EventHandler(this.cb_Map_CloudTops_CheckedChanged);
            // 
            // cb_Map_Intensity
            // 
            this.cb_Map_Intensity.AutoSize = true;
            this.cb_Map_Intensity.Checked = global::RainScout.Properties.Settings.Default.Map_Intensity;
            this.cb_Map_Intensity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Map_Intensity.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RainScout.Properties.Settings.Default, "Map_Intensity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_Intensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Map_Intensity.Location = new System.Drawing.Point(12, 88);
            this.cb_Map_Intensity.Name = "cb_Map_Intensity";
            this.cb_Map_Intensity.Size = new System.Drawing.Size(65, 17);
            this.cb_Map_Intensity.TabIndex = 11;
            this.cb_Map_Intensity.Text = "Intensity";
            this.cb_Map_Intensity.UseVisualStyleBackColor = true;
            this.cb_Map_Intensity.CheckedChanged += new System.EventHandler(this.cb_Map_Intensity_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(101, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Zoom";
            // 
            // btn_Map_ZoomIn
            // 
            this.btn_Map_ZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Map_ZoomIn.Location = new System.Drawing.Point(109, 32);
            this.btn_Map_ZoomIn.Name = "btn_Map_ZoomIn";
            this.btn_Map_ZoomIn.Size = new System.Drawing.Size(23, 23);
            this.btn_Map_ZoomIn.TabIndex = 9;
            this.btn_Map_ZoomIn.Text = "+";
            this.btn_Map_ZoomIn.UseVisualStyleBackColor = true;
            this.btn_Map_ZoomIn.Click += new System.EventHandler(this.btn_Map_ZoomIn_Click);
            // 
            // btn_Map_ZoomOut
            // 
            this.btn_Map_ZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Map_ZoomOut.Location = new System.Drawing.Point(109, 128);
            this.btn_Map_ZoomOut.Name = "btn_Map_ZoomOut";
            this.btn_Map_ZoomOut.Size = new System.Drawing.Size(23, 23);
            this.btn_Map_ZoomOut.TabIndex = 8;
            this.btn_Map_ZoomOut.Text = "-";
            this.btn_Map_ZoomOut.UseVisualStyleBackColor = true;
            this.btn_Map_ZoomOut.Click += new System.EventHandler(this.btn_Map_ZoomOut_Click);
            // 
            // tb_Map_Zoom
            // 
            this.tb_Map_Zoom.BackColor = System.Drawing.Color.White;
            this.tb_Map_Zoom.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Map_Zoom.Location = new System.Drawing.Point(109, 82);
            this.tb_Map_Zoom.Name = "tb_Map_Zoom";
            this.tb_Map_Zoom.ReadOnly = true;
            this.tb_Map_Zoom.Size = new System.Drawing.Size(23, 20);
            this.tb_Map_Zoom.TabIndex = 7;
            this.tb_Map_Zoom.Text = "0";
            this.tb_Map_Zoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pa_CommonInfo
            // 
            this.pa_CommonInfo.Controls.Add(this.lbl_Radar_Timestamp);
            this.pa_CommonInfo.Controls.Add(this.label2);
            this.pa_CommonInfo.Controls.Add(this.cb_Radar);
            this.pa_CommonInfo.Controls.Add(this.label32);
            this.pa_CommonInfo.Controls.Add(this.cb_ElevationModel);
            this.pa_CommonInfo.Controls.Add(this.label27);
            this.pa_CommonInfo.Controls.Add(this.tb_UTC);
            this.pa_CommonInfo.Controls.Add(this.label28);
            this.pa_CommonInfo.Controls.Add(this.cb_Band);
            this.pa_CommonInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pa_CommonInfo.Location = new System.Drawing.Point(0, 0);
            this.pa_CommonInfo.MinimumSize = new System.Drawing.Size(150, 142);
            this.pa_CommonInfo.Name = "pa_CommonInfo";
            this.pa_CommonInfo.Size = new System.Drawing.Size(150, 187);
            this.pa_CommonInfo.TabIndex = 60;
            // 
            // lbl_Radar_Timestamp
            // 
            this.lbl_Radar_Timestamp.AutoSize = true;
            this.lbl_Radar_Timestamp.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Radar_Timestamp.Location = new System.Drawing.Point(15, 167);
            this.lbl_Radar_Timestamp.Name = "lbl_Radar_Timestamp";
            this.lbl_Radar_Timestamp.Size = new System.Drawing.Size(119, 14);
            this.lbl_Radar_Timestamp.TabIndex = 22;
            this.lbl_Radar_Timestamp.Text = "0000-00-00 00:00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Radar";
            // 
            // cb_Radar
            // 
            this.cb_Radar.AllowDrop = true;
            this.cb_Radar.BackColor = System.Drawing.Color.FloralWhite;
            this.cb_Radar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Radar.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Radar.FormattingEnabled = true;
            this.cb_Radar.Location = new System.Drawing.Point(8, 142);
            this.cb_Radar.Name = "cb_Radar";
            this.cb_Radar.Size = new System.Drawing.Size(130, 22);
            this.cb_Radar.TabIndex = 20;
            this.cb_Radar.SelectedIndexChanged += new System.EventHandler(this.cb_Radar_SelectedIndexChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(6, 86);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(83, 13);
            this.label32.TabIndex = 19;
            this.label32.Text = "Elevation Model";
            // 
            // cb_ElevationModel
            // 
            this.cb_ElevationModel.AllowDrop = true;
            this.cb_ElevationModel.BackColor = System.Drawing.Color.FloralWhite;
            this.cb_ElevationModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ElevationModel.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ElevationModel.FormattingEnabled = true;
            this.cb_ElevationModel.Location = new System.Drawing.Point(8, 102);
            this.cb_ElevationModel.Name = "cb_ElevationModel";
            this.cb_ElevationModel.Size = new System.Drawing.Size(130, 22);
            this.cb_ElevationModel.TabIndex = 18;
            this.cb_ElevationModel.SelectedIndexChanged += new System.EventHandler(this.cb_ElevationModel_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(4, 3);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 13);
            this.label27.TabIndex = 17;
            this.label27.Text = "UTC";
            // 
            // tb_UTC
            // 
            this.tb_UTC.BackColor = System.Drawing.Color.LightSalmon;
            this.tb_UTC.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_UTC.ForeColor = System.Drawing.Color.White;
            this.tb_UTC.Location = new System.Drawing.Point(4, 18);
            this.tb_UTC.Name = "tb_UTC";
            this.tb_UTC.ReadOnly = true;
            this.tb_UTC.Size = new System.Drawing.Size(133, 22);
            this.tb_UTC.TabIndex = 13;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(4, 43);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(32, 13);
            this.label28.TabIndex = 14;
            this.label28.Text = "Band";
            // 
            // cb_Band
            // 
            this.cb_Band.AllowDrop = true;
            this.cb_Band.BackColor = System.Drawing.Color.FloralWhite;
            this.cb_Band.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Band.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Band.FormattingEnabled = true;
            this.cb_Band.Location = new System.Drawing.Point(8, 59);
            this.cb_Band.Name = "cb_Band";
            this.cb_Band.Size = new System.Drawing.Size(130, 22);
            this.cb_Band.TabIndex = 16;
            this.cb_Band.SelectedIndexChanged += new System.EventHandler(this.cb_Band_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_Mouse_Lightning);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.tb_Mouse_Intensity);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_Mouse_CloudTop);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.tb_Mouse_Distance);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_Mouse_Bearing);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_Mouse_Loc);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tb_Mouse_Lon);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tb_Mouse_Lat);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(648, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 208);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mouse Position";
            // 
            // tb_Mouse_Lightning
            // 
            this.tb_Mouse_Lightning.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Mouse_Lightning.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Lightning.Location = new System.Drawing.Point(92, 169);
            this.tb_Mouse_Lightning.Name = "tb_Mouse_Lightning";
            this.tb_Mouse_Lightning.ReadOnly = true;
            this.tb_Mouse_Lightning.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Lightning.TabIndex = 29;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(6, 172);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(78, 13);
            this.label33.TabIndex = 28;
            this.label33.Text = "Lightning [min]:";
            // 
            // tb_Mouse_Intensity
            // 
            this.tb_Mouse_Intensity.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Mouse_Intensity.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Intensity.Location = new System.Drawing.Point(92, 127);
            this.tb_Mouse_Intensity.Name = "tb_Mouse_Intensity";
            this.tb_Mouse_Intensity.ReadOnly = true;
            this.tb_Mouse_Intensity.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Intensity.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Intensity [dbZ]:";
            // 
            // tb_Mouse_CloudTop
            // 
            this.tb_Mouse_CloudTop.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Mouse_CloudTop.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_CloudTop.Location = new System.Drawing.Point(92, 148);
            this.tb_Mouse_CloudTop.Name = "tb_Mouse_CloudTop";
            this.tb_Mouse_CloudTop.ReadOnly = true;
            this.tb_Mouse_CloudTop.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_CloudTop.TabIndex = 25;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(6, 151);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(76, 13);
            this.label30.TabIndex = 24;
            this.label30.Text = "Cloud Top [m]:";
            // 
            // tb_Mouse_Distance
            // 
            this.tb_Mouse_Distance.BackColor = System.Drawing.Color.White;
            this.tb_Mouse_Distance.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Distance.Location = new System.Drawing.Point(92, 106);
            this.tb_Mouse_Distance.Name = "tb_Mouse_Distance";
            this.tb_Mouse_Distance.ReadOnly = true;
            this.tb_Mouse_Distance.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Distance.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Distance[km]:";
            // 
            // tb_Mouse_Bearing
            // 
            this.tb_Mouse_Bearing.BackColor = System.Drawing.Color.White;
            this.tb_Mouse_Bearing.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Bearing.Location = new System.Drawing.Point(92, 85);
            this.tb_Mouse_Bearing.Name = "tb_Mouse_Bearing";
            this.tb_Mouse_Bearing.ReadOnly = true;
            this.tb_Mouse_Bearing.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Bearing.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Bearing[deg]:";
            // 
            // tb_Mouse_Loc
            // 
            this.tb_Mouse_Loc.BackColor = System.Drawing.Color.White;
            this.tb_Mouse_Loc.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Loc.Location = new System.Drawing.Point(92, 64);
            this.tb_Mouse_Loc.Name = "tb_Mouse_Loc";
            this.tb_Mouse_Loc.ReadOnly = true;
            this.tb_Mouse_Loc.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Loc.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Locator:";
            // 
            // tb_Mouse_Lon
            // 
            this.tb_Mouse_Lon.BackColor = System.Drawing.Color.White;
            this.tb_Mouse_Lon.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Lon.Location = new System.Drawing.Point(92, 43);
            this.tb_Mouse_Lon.Name = "tb_Mouse_Lon";
            this.tb_Mouse_Lon.ReadOnly = true;
            this.tb_Mouse_Lon.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Lon.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Longitude:";
            // 
            // tb_Mouse_Lat
            // 
            this.tb_Mouse_Lat.BackColor = System.Drawing.Color.White;
            this.tb_Mouse_Lat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Mouse_Lat.Location = new System.Drawing.Point(92, 22);
            this.tb_Mouse_Lat.Name = "tb_Mouse_Lat";
            this.tb_Mouse_Lat.ReadOnly = true;
            this.tb_Mouse_Lat.Size = new System.Drawing.Size(97, 20);
            this.tb_Mouse_Lat.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Latitude:";
            // 
            // btn_Options
            // 
            this.btn_Options.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Options.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Options.Location = new System.Drawing.Point(871, 126);
            this.btn_Options.Name = "btn_Options";
            this.btn_Options.Size = new System.Drawing.Size(123, 38);
            this.btn_Options.TabIndex = 4;
            this.btn_Options.Text = "Options";
            this.btn_Options.UseVisualStyleBackColor = true;
            this.btn_Options.Click += new System.EventHandler(this.btn_Options_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Refresh.Location = new System.Drawing.Point(871, 177);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(123, 38);
            this.btn_Refresh.TabIndex = 3;
            this.btn_Refresh.Text = "Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // gb_DXData
            // 
            this.gb_DXData.Controls.Add(this.tb_DXData_Elevation);
            this.gb_DXData.Controls.Add(this.label25);
            this.gb_DXData.Controls.Add(this.tb_DXData_Distance);
            this.gb_DXData.Controls.Add(this.label20);
            this.gb_DXData.Controls.Add(this.tb_DXData_Bearing);
            this.gb_DXData.Controls.Add(this.label21);
            this.gb_DXData.Controls.Add(this.tb_DXData_Loc);
            this.gb_DXData.Controls.Add(this.label22);
            this.gb_DXData.Controls.Add(this.tb_DXData_Lon);
            this.gb_DXData.Controls.Add(this.label23);
            this.gb_DXData.Controls.Add(this.tb_DXData_Lat);
            this.gb_DXData.Controls.Add(this.label24);
            this.gb_DXData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_DXData.Location = new System.Drawing.Point(436, 7);
            this.gb_DXData.Name = "gb_DXData";
            this.gb_DXData.Size = new System.Drawing.Size(206, 208);
            this.gb_DXData.TabIndex = 2;
            this.gb_DXData.TabStop = false;
            this.gb_DXData.Text = "DX Data";
            // 
            // tb_DXData_Elevation
            // 
            this.tb_DXData_Elevation.BackColor = System.Drawing.SystemColors.Control;
            this.tb_DXData_Elevation.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXData_Elevation.Location = new System.Drawing.Point(92, 127);
            this.tb_DXData_Elevation.Name = "tb_DXData_Elevation";
            this.tb_DXData_Elevation.ReadOnly = true;
            this.tb_DXData_Elevation.Size = new System.Drawing.Size(97, 20);
            this.tb_DXData_Elevation.TabIndex = 23;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(6, 130);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(68, 13);
            this.label25.TabIndex = 22;
            this.label25.Text = "Elevation[m]:";
            // 
            // tb_DXData_Distance
            // 
            this.tb_DXData_Distance.BackColor = System.Drawing.Color.White;
            this.tb_DXData_Distance.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXData_Distance.Location = new System.Drawing.Point(92, 106);
            this.tb_DXData_Distance.Name = "tb_DXData_Distance";
            this.tb_DXData_Distance.ReadOnly = true;
            this.tb_DXData_Distance.Size = new System.Drawing.Size(97, 20);
            this.tb_DXData_Distance.TabIndex = 19;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(6, 109);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 13);
            this.label20.TabIndex = 18;
            this.label20.Text = "Distance[km]:";
            // 
            // tb_DXData_Bearing
            // 
            this.tb_DXData_Bearing.BackColor = System.Drawing.Color.White;
            this.tb_DXData_Bearing.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXData_Bearing.Location = new System.Drawing.Point(92, 85);
            this.tb_DXData_Bearing.Name = "tb_DXData_Bearing";
            this.tb_DXData_Bearing.ReadOnly = true;
            this.tb_DXData_Bearing.Size = new System.Drawing.Size(97, 20);
            this.tb_DXData_Bearing.TabIndex = 17;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(6, 87);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 13);
            this.label21.TabIndex = 16;
            this.label21.Text = "Bearing[deg]:";
            // 
            // tb_DXData_Loc
            // 
            this.tb_DXData_Loc.BackColor = System.Drawing.Color.White;
            this.tb_DXData_Loc.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXData_Loc.Location = new System.Drawing.Point(92, 64);
            this.tb_DXData_Loc.Name = "tb_DXData_Loc";
            this.tb_DXData_Loc.ReadOnly = true;
            this.tb_DXData_Loc.Size = new System.Drawing.Size(97, 20);
            this.tb_DXData_Loc.TabIndex = 15;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(6, 67);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(46, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "Locator:";
            // 
            // tb_DXData_Lon
            // 
            this.tb_DXData_Lon.BackColor = System.Drawing.Color.White;
            this.tb_DXData_Lon.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXData_Lon.Location = new System.Drawing.Point(92, 43);
            this.tb_DXData_Lon.Name = "tb_DXData_Lon";
            this.tb_DXData_Lon.ReadOnly = true;
            this.tb_DXData_Lon.Size = new System.Drawing.Size(97, 20);
            this.tb_DXData_Lon.TabIndex = 13;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(6, 46);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(57, 13);
            this.label23.TabIndex = 12;
            this.label23.Text = "Longitude:";
            // 
            // tb_DXData_Lat
            // 
            this.tb_DXData_Lat.BackColor = System.Drawing.Color.White;
            this.tb_DXData_Lat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXData_Lat.Location = new System.Drawing.Point(92, 22);
            this.tb_DXData_Lat.Name = "tb_DXData_Lat";
            this.tb_DXData_Lat.ReadOnly = true;
            this.tb_DXData_Lat.Size = new System.Drawing.Size(97, 20);
            this.tb_DXData_Lat.TabIndex = 11;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(6, 25);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(48, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "Latitude:";
            // 
            // gb_Scp
            // 
            this.gb_Scp.Controls.Add(this.tb_Scp_Lightning);
            this.gb_Scp.Controls.Add(this.label31);
            this.gb_Scp.Controls.Add(this.tb_Scp_Intensity);
            this.gb_Scp.Controls.Add(this.label34);
            this.gb_Scp.Controls.Add(this.tb_Scp_CloudTop);
            this.gb_Scp.Controls.Add(this.label35);
            this.gb_Scp.Controls.Add(this.btn_scp_Clear);
            this.gb_Scp.Controls.Add(this.tb_Scp_Loc);
            this.gb_Scp.Controls.Add(this.label17);
            this.gb_Scp.Controls.Add(this.tb_Scp_Lon);
            this.gb_Scp.Controls.Add(this.label18);
            this.gb_Scp.Controls.Add(this.tb_Scp_Lat);
            this.gb_Scp.Controls.Add(this.label19);
            this.gb_Scp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Scp.Location = new System.Drawing.Point(224, 7);
            this.gb_Scp.Name = "gb_Scp";
            this.gb_Scp.Size = new System.Drawing.Size(206, 208);
            this.gb_Scp.TabIndex = 1;
            this.gb_Scp.TabStop = false;
            this.gb_Scp.Text = "Scatterpoint";
            // 
            // tb_Scp_Lightning
            // 
            this.tb_Scp_Lightning.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Scp_Lightning.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Scp_Lightning.Location = new System.Drawing.Point(92, 127);
            this.tb_Scp_Lightning.Name = "tb_Scp_Lightning";
            this.tb_Scp_Lightning.ReadOnly = true;
            this.tb_Scp_Lightning.Size = new System.Drawing.Size(97, 20);
            this.tb_Scp_Lightning.TabIndex = 35;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(6, 130);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(78, 13);
            this.label31.TabIndex = 34;
            this.label31.Text = "Lightning [min]:";
            // 
            // tb_Scp_Intensity
            // 
            this.tb_Scp_Intensity.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Scp_Intensity.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Scp_Intensity.Location = new System.Drawing.Point(92, 85);
            this.tb_Scp_Intensity.Name = "tb_Scp_Intensity";
            this.tb_Scp_Intensity.ReadOnly = true;
            this.tb_Scp_Intensity.Size = new System.Drawing.Size(97, 20);
            this.tb_Scp_Intensity.TabIndex = 33;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(6, 88);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(77, 13);
            this.label34.TabIndex = 32;
            this.label34.Text = "Intensity [dbZ]:";
            // 
            // tb_Scp_CloudTop
            // 
            this.tb_Scp_CloudTop.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Scp_CloudTop.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Scp_CloudTop.Location = new System.Drawing.Point(92, 106);
            this.tb_Scp_CloudTop.Name = "tb_Scp_CloudTop";
            this.tb_Scp_CloudTop.ReadOnly = true;
            this.tb_Scp_CloudTop.Size = new System.Drawing.Size(97, 20);
            this.tb_Scp_CloudTop.TabIndex = 31;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(6, 109);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(76, 13);
            this.label35.TabIndex = 30;
            this.label35.Text = "Cloud Top [m]:";
            // 
            // btn_scp_Clear
            // 
            this.btn_scp_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_scp_Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_scp_Clear.Location = new System.Drawing.Point(92, 178);
            this.btn_scp_Clear.Name = "btn_scp_Clear";
            this.btn_scp_Clear.Size = new System.Drawing.Size(97, 20);
            this.btn_scp_Clear.TabIndex = 28;
            this.btn_scp_Clear.Text = "Clear";
            this.btn_scp_Clear.UseVisualStyleBackColor = true;
            this.btn_scp_Clear.Click += new System.EventHandler(this.btn_scp_Clear_Click);
            // 
            // tb_Scp_Loc
            // 
            this.tb_Scp_Loc.BackColor = System.Drawing.Color.White;
            this.tb_Scp_Loc.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Scp_Loc.ForeColor = System.Drawing.Color.Magenta;
            this.tb_Scp_Loc.Location = new System.Drawing.Point(92, 64);
            this.tb_Scp_Loc.Name = "tb_Scp_Loc";
            this.tb_Scp_Loc.ReadOnly = true;
            this.tb_Scp_Loc.Size = new System.Drawing.Size(97, 20);
            this.tb_Scp_Loc.TabIndex = 15;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(6, 67);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(46, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "Locator:";
            // 
            // tb_Scp_Lon
            // 
            this.tb_Scp_Lon.BackColor = System.Drawing.Color.White;
            this.tb_Scp_Lon.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Scp_Lon.ForeColor = System.Drawing.Color.Magenta;
            this.tb_Scp_Lon.Location = new System.Drawing.Point(92, 43);
            this.tb_Scp_Lon.Name = "tb_Scp_Lon";
            this.tb_Scp_Lon.ReadOnly = true;
            this.tb_Scp_Lon.Size = new System.Drawing.Size(97, 20);
            this.tb_Scp_Lon.TabIndex = 13;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(6, 46);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "Longitude:";
            // 
            // tb_Scp_Lat
            // 
            this.tb_Scp_Lat.BackColor = System.Drawing.Color.White;
            this.tb_Scp_Lat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Scp_Lat.ForeColor = System.Drawing.Color.Magenta;
            this.tb_Scp_Lat.Location = new System.Drawing.Point(92, 22);
            this.tb_Scp_Lat.Name = "tb_Scp_Lat";
            this.tb_Scp_Lat.ReadOnly = true;
            this.tb_Scp_Lat.Size = new System.Drawing.Size(97, 20);
            this.tb_Scp_Lat.TabIndex = 11;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(6, 25);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(48, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Latitude:";
            // 
            // gb_MyData
            // 
            this.gb_MyData.Controls.Add(this.tb_MyData_Height);
            this.gb_MyData.Controls.Add(this.label29);
            this.gb_MyData.Controls.Add(this.tb_MyData_Elevation);
            this.gb_MyData.Controls.Add(this.label16);
            this.gb_MyData.Controls.Add(this.tb_MyData_Distance);
            this.gb_MyData.Controls.Add(this.label14);
            this.gb_MyData.Controls.Add(this.tb_MyData_Bearing);
            this.gb_MyData.Controls.Add(this.label13);
            this.gb_MyData.Controls.Add(this.tb_MyData_Loc);
            this.gb_MyData.Controls.Add(this.label12);
            this.gb_MyData.Controls.Add(this.tb_MyDataLon);
            this.gb_MyData.Controls.Add(this.label11);
            this.gb_MyData.Controls.Add(this.tb_MyData_Lat);
            this.gb_MyData.Controls.Add(this.label10);
            this.gb_MyData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_MyData.Location = new System.Drawing.Point(12, 7);
            this.gb_MyData.Name = "gb_MyData";
            this.gb_MyData.Size = new System.Drawing.Size(206, 208);
            this.gb_MyData.TabIndex = 0;
            this.gb_MyData.TabStop = false;
            this.gb_MyData.Text = "My Data";
            // 
            // tb_MyData_Height
            // 
            this.tb_MyData_Height.BackColor = System.Drawing.SystemColors.Control;
            this.tb_MyData_Height.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyData_Height.Location = new System.Drawing.Point(92, 148);
            this.tb_MyData_Height.Name = "tb_MyData_Height";
            this.tb_MyData_Height.ReadOnly = true;
            this.tb_MyData_Height.Size = new System.Drawing.Size(97, 20);
            this.tb_MyData_Height.TabIndex = 23;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(6, 151);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(80, 13);
            this.label29.TabIndex = 22;
            this.label29.Text = "Ant. Height [m]:";
            // 
            // tb_MyData_Elevation
            // 
            this.tb_MyData_Elevation.BackColor = System.Drawing.SystemColors.Control;
            this.tb_MyData_Elevation.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyData_Elevation.Location = new System.Drawing.Point(92, 127);
            this.tb_MyData_Elevation.Name = "tb_MyData_Elevation";
            this.tb_MyData_Elevation.ReadOnly = true;
            this.tb_MyData_Elevation.Size = new System.Drawing.Size(97, 20);
            this.tb_MyData_Elevation.TabIndex = 21;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(6, 130);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(68, 13);
            this.label16.TabIndex = 20;
            this.label16.Text = "Elevation[m]:";
            // 
            // tb_MyData_Distance
            // 
            this.tb_MyData_Distance.BackColor = System.Drawing.Color.White;
            this.tb_MyData_Distance.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyData_Distance.Location = new System.Drawing.Point(92, 106);
            this.tb_MyData_Distance.Name = "tb_MyData_Distance";
            this.tb_MyData_Distance.ReadOnly = true;
            this.tb_MyData_Distance.Size = new System.Drawing.Size(97, 20);
            this.tb_MyData_Distance.TabIndex = 19;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(6, 109);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Distance[km]:";
            // 
            // tb_MyData_Bearing
            // 
            this.tb_MyData_Bearing.BackColor = System.Drawing.Color.White;
            this.tb_MyData_Bearing.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyData_Bearing.Location = new System.Drawing.Point(92, 85);
            this.tb_MyData_Bearing.Name = "tb_MyData_Bearing";
            this.tb_MyData_Bearing.ReadOnly = true;
            this.tb_MyData_Bearing.Size = new System.Drawing.Size(97, 20);
            this.tb_MyData_Bearing.TabIndex = 17;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Bearing[deg]:";
            // 
            // tb_MyData_Loc
            // 
            this.tb_MyData_Loc.BackColor = System.Drawing.Color.White;
            this.tb_MyData_Loc.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyData_Loc.Location = new System.Drawing.Point(92, 64);
            this.tb_MyData_Loc.Name = "tb_MyData_Loc";
            this.tb_MyData_Loc.ReadOnly = true;
            this.tb_MyData_Loc.Size = new System.Drawing.Size(97, 20);
            this.tb_MyData_Loc.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "Locator:";
            // 
            // tb_MyDataLon
            // 
            this.tb_MyDataLon.BackColor = System.Drawing.Color.White;
            this.tb_MyDataLon.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyDataLon.Location = new System.Drawing.Point(92, 43);
            this.tb_MyDataLon.Name = "tb_MyDataLon";
            this.tb_MyDataLon.ReadOnly = true;
            this.tb_MyDataLon.Size = new System.Drawing.Size(97, 20);
            this.tb_MyDataLon.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Longitude:";
            // 
            // tb_MyData_Lat
            // 
            this.tb_MyData_Lat.BackColor = System.Drawing.Color.White;
            this.tb_MyData_Lat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyData_Lat.Location = new System.Drawing.Point(92, 22);
            this.tb_MyData_Lat.Name = "tb_MyData_Lat";
            this.tb_MyData_Lat.ReadOnly = true;
            this.tb_MyData_Lat.Size = new System.Drawing.Size(97, 20);
            this.tb_MyData_Lat.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Latitude:";
            // 
            // bw_Horizons
            // 
            this.bw_Horizons.WorkerReportsProgress = true;
            this.bw_Horizons.WorkerSupportsCancellation = true;
            this.bw_Horizons.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Horizons_DoWork);
            this.bw_Horizons.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Horizons_ProgressChanged);
            this.bw_Horizons.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Horizons_RunWorkerCompleted);
            // 
            // ti_Main
            // 
            this.ti_Main.Enabled = true;
            this.ti_Main.Interval = 1000;
            this.ti_Main.Tick += new System.EventHandler(this.ti_Main_Tick);
            // 
            // bw_Stations
            // 
            this.bw_Stations.WorkerReportsProgress = true;
            this.bw_Stations.WorkerSupportsCancellation = true;
            this.bw_Stations.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Stations_DoWork);
            this.bw_Stations.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Stations_ProgressChanged);
            this.bw_Stations.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Stations_RunWorkerCompleted);
            // 
            // bw_Locations
            // 
            this.bw_Locations.WorkerReportsProgress = true;
            this.bw_Locations.WorkerSupportsCancellation = true;
            this.bw_Locations.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Locations_DoWork);
            this.bw_Locations.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Locations_ProgressChanged);
            this.bw_Locations.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Locations_RunWorkerCompleted);
            // 
            // bw_Radar
            // 
            this.bw_Radar.WorkerReportsProgress = true;
            this.bw_Radar.WorkerSupportsCancellation = true;
            this.bw_Radar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Radar_DoWork);
            this.bw_Radar.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Radar_ProgressChanged);
            this.bw_Radar.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Radar_RunWorkerCompleted);
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
            this.tsl_Database_LED_Stations.Size = new System.Drawing.Size(12, 12);
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
            this.tsl_Database_LED_GLOBE.Size = new System.Drawing.Size(12, 12);
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
            this.tsl_Database_LED_SRTM3.Size = new System.Drawing.Size(12, 12);
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
            this.tsl_Database_LED_SRTM1.Size = new System.Drawing.Size(12, 12);
            this.tsl_Database_LED_SRTM1.Text = " SRTM3 database status LED";
            this.tsl_Database_LED_SRTM1.ToolTipText = "SRTM1 database status LED";
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.spl_Main);
            this.Controls.Add(this.ss_Main);
            this.Name = "MainDlg";
            this.Text = "RainScout";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.Shown += new System.EventHandler(this.MainDlg_Shown);
            this.SizeChanged += new System.EventHandler(this.MainDlg_SizeChanged);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.spl_Main.Panel1.ResumeLayout(false);
            this.spl_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl_Main)).EndInit();
            this.spl_Main.ResumeLayout(false);
            this.spl_Map.Panel1.ResumeLayout(false);
            this.spl_Map.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spl_Map)).EndInit();
            this.spl_Map.ResumeLayout(false);
            this.gb_Filter.ResumeLayout(false);
            this.gb_Filter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Filter_MaxDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Filter_MinDistance)).EndInit();
            this.gb_Map.ResumeLayout(false);
            this.gb_Map.PerformLayout();
            this.pa_CommonInfo.ResumeLayout(false);
            this.pa_CommonInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_DXData.ResumeLayout(false);
            this.gb_DXData.PerformLayout();
            this.gb_Scp.ResumeLayout(false);
            this.gb_Scp.PerformLayout();
            this.gb_MyData.ResumeLayout(false);
            this.gb_MyData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.SplitContainer spl_Main;
        private System.Windows.Forms.SplitContainer spl_Map;
        private GMap.NET.WindowsForms.GMapControl gm_Main;
        private System.Windows.Forms.GroupBox gb_MyData;
        private System.Windows.Forms.GroupBox gb_DXData;
        private System.Windows.Forms.TextBox tb_DXData_Distance;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tb_DXData_Bearing;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tb_DXData_Loc;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tb_DXData_Lon;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tb_DXData_Lat;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox gb_Scp;
        private System.Windows.Forms.TextBox tb_Scp_Loc;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tb_Scp_Lon;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb_Scp_Lat;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tb_MyData_Distance;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_MyData_Bearing;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_MyData_Loc;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb_MyDataLon;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_MyData_Lat;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_Refresh;
        private System.ComponentModel.BackgroundWorker bw_Horizons;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Main;
        private System.Windows.Forms.Button btn_Options;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_Mouse_Distance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Mouse_Bearing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_Mouse_Loc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Mouse_Lon;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Mouse_Lat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_DXData_Elevation;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox tb_MyData_Elevation;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel pa_CommonInfo;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox tb_UTC;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ComboBox cb_Band;
        private System.Windows.Forms.GroupBox gb_Map;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btn_Map_ZoomIn;
        private System.Windows.Forms.Button btn_Map_ZoomOut;
        private System.Windows.Forms.TextBox tb_Map_Zoom;
        private System.Windows.Forms.Timer ti_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Dummy;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database;
        private System.Windows.Forms.TextBox tb_MyData_Height;
        private System.Windows.Forms.Label label29;
        private System.ComponentModel.BackgroundWorker bw_Stations;
        private System.Windows.Forms.TextBox tb_Mouse_CloudTop;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btn_scp_Clear;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox cb_ElevationModel;
        private System.Windows.Forms.GroupBox gb_Filter;
        private System.Windows.Forms.CheckBox cb_Filter_VisibleScpOnly;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ud_Filter_MaxDistance;
        private System.Windows.Forms.Label label222;
        private System.Windows.Forms.NumericUpDown ud_Filter_MinDistance;
        private System.Windows.Forms.CheckBox cb_Map_Intensity;
        private System.Windows.Forms.CheckBox cb_Map_Lightning;
        private System.Windows.Forms.CheckBox cb_Map_CloudTops;
        private System.Windows.Forms.TextBox tb_Mouse_Intensity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_Radar;
        private System.Windows.Forms.Label lbl_Radar_Timestamp;
        private System.Windows.Forms.CheckBox cb_Map_Bounds;
        private System.Windows.Forms.CheckBox cb_Map_Horizons;
        private System.Windows.Forms.CheckBox cb_Map_Distances;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Mouse_Lightning;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox tb_Scp_Lightning;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox tb_Scp_Intensity;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox tb_Scp_CloudTop;
        private System.Windows.Forms.Label label35;
        private System.ComponentModel.BackgroundWorker bw_Locations;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Locations;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Radar;
        private System.ComponentModel.BackgroundWorker bw_Radar;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_Stations;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_GLOBE;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_SRTM3;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Database_LED_SRTM1;
    }
}

