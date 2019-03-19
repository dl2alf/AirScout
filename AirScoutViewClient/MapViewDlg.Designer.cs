namespace AirScoutViewClient
{
    partial class MapViewDlg
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
            ScoutBase.Core.LatLon.GPoint gPoint5 = new ScoutBase.Core.LatLon.GPoint();
            ScoutBase.Core.LatLon.GPoint gPoint6 = new ScoutBase.Core.LatLon.GPoint();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapViewDlg));
            this.mnu_Main = new System.Windows.Forms.MenuStrip();
            this.tsi_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsi_Info = new System.Windows.Forms.ToolStripMenuItem();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.gb_Info = new System.Windows.Forms.GroupBox();
            this.cb_DXLoc = new ScoutBase.Core.LocatorComboBox();
            this.cb_MyLoc = new ScoutBase.Core.LocatorComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_Band = new System.Windows.Forms.ComboBox();
            this.tb_QTF = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_QRB = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_DXCall = new ScoutBase.Core.CallsignTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_MyCall = new ScoutBase.Core.CallsignTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_UTC = new System.Windows.Forms.TextBox();
            this.gb_Map = new System.Windows.Forms.GroupBox();
            this.spc_Main = new System.Windows.Forms.SplitContainer();
            this.gm_Main = new GMap.NET.WindowsForms.GMapControl();
            this.gb_Path = new System.Windows.Forms.GroupBox();
            this.ti_Progress = new System.Windows.Forms.Timer(this.components);
            this.ti_Startup = new System.Windows.Forms.Timer(this.components);
            this.il_Planes_H = new System.Windows.Forms.ImageList(this.components);
            this.il_Planes_L = new System.Windows.Forms.ImageList(this.components);
            this.il_Planes_S = new System.Windows.Forms.ImageList(this.components);
            this.il_Airports = new System.Windows.Forms.ImageList(this.components);
            this.il_Planes_M = new System.Windows.Forms.ImageList(this.components);
            this.ti_ShowLegends = new System.Windows.Forms.Timer(this.components);
            this.btn_Map_PlayPause = new System.Windows.Forms.Button();
            this.gb_Control = new System.Windows.Forms.GroupBox();
            this.il_Main = new System.Windows.Forms.ImageList(this.components);
            this.gb_Map_Zoom = new System.Windows.Forms.GroupBox();
            this.pa_Map_Zoom = new System.Windows.Forms.Panel();
            this.tb_Map_Zoom = new System.Windows.Forms.TextBox();
            this.btn_Map_Zoom_Out = new System.Windows.Forms.Button();
            this.btn_Map_Zoom_In = new System.Windows.Forms.Button();
            this.tsl_ConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.cb_Map_AutoCenter = new System.Windows.Forms.CheckBox();
            this.mnu_Main.SuspendLayout();
            this.ss_Main.SuspendLayout();
            this.gb_Info.SuspendLayout();
            this.gb_Map.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spc_Main)).BeginInit();
            this.spc_Main.Panel1.SuspendLayout();
            this.spc_Main.Panel2.SuspendLayout();
            this.spc_Main.SuspendLayout();
            this.gb_Control.SuspendLayout();
            this.gb_Map_Zoom.SuspendLayout();
            this.pa_Map_Zoom.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu_Main
            // 
            this.mnu_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsi_Exit,
            this.settingsToolStripMenuItem,
            this.tsi_Info});
            this.mnu_Main.Location = new System.Drawing.Point(0, 0);
            this.mnu_Main.Name = "mnu_Main";
            this.mnu_Main.Size = new System.Drawing.Size(836, 24);
            this.mnu_Main.TabIndex = 0;
            this.mnu_Main.Text = "menuStrip1";
            // 
            // tsi_Exit
            // 
            this.tsi_Exit.Name = "tsi_Exit";
            this.tsi_Exit.Size = new System.Drawing.Size(37, 20);
            this.tsi_Exit.Text = "E&xit";
            this.tsi_Exit.Click += new System.EventHandler(this.tsi_Exit_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // tsi_Info
            // 
            this.tsi_Info.Name = "tsi_Info";
            this.tsi_Info.Size = new System.Drawing.Size(40, 20);
            this.tsi_Info.Text = "&Info";
            this.tsi_Info.Click += new System.EventHandler(this.tsi_Info_Click);
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status,
            this.tsl_ConnectionStatus});
            this.ss_Main.Location = new System.Drawing.Point(0, 518);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(836, 22);
            this.ss_Main.TabIndex = 1;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(782, 17);
            this.tsl_Status.Spring = true;
            this.tsl_Status.Text = "Status";
            this.tsl_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gb_Info
            // 
            this.gb_Info.Controls.Add(this.gb_Map_Zoom);
            this.gb_Info.Controls.Add(this.gb_Control);
            this.gb_Info.Controls.Add(this.cb_DXLoc);
            this.gb_Info.Controls.Add(this.cb_MyLoc);
            this.gb_Info.Controls.Add(this.label8);
            this.gb_Info.Controls.Add(this.cb_Band);
            this.gb_Info.Controls.Add(this.tb_QTF);
            this.gb_Info.Controls.Add(this.label6);
            this.gb_Info.Controls.Add(this.tb_QRB);
            this.gb_Info.Controls.Add(this.label16);
            this.gb_Info.Controls.Add(this.label4);
            this.gb_Info.Controls.Add(this.tb_DXCall);
            this.gb_Info.Controls.Add(this.label5);
            this.gb_Info.Controls.Add(this.label3);
            this.gb_Info.Controls.Add(this.tb_MyCall);
            this.gb_Info.Controls.Add(this.label2);
            this.gb_Info.Controls.Add(this.label1);
            this.gb_Info.Controls.Add(this.tb_UTC);
            this.gb_Info.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Info.Location = new System.Drawing.Point(691, 24);
            this.gb_Info.Name = "gb_Info";
            this.gb_Info.Size = new System.Drawing.Size(145, 494);
            this.gb_Info.TabIndex = 3;
            this.gb_Info.TabStop = false;
            this.gb_Info.Text = "Info";
            // 
            // cb_DXLoc
            // 
            this.cb_DXLoc.AutoLength = false;
            this.cb_DXLoc.BackColor = System.Drawing.SystemColors.Window;
            this.cb_DXLoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cb_DXLoc.ErrorBackColor = System.Drawing.Color.Red;
            this.cb_DXLoc.ErrorForeColor = System.Drawing.Color.White;
            this.cb_DXLoc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DXLoc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_DXLoc.FormattingEnabled = true;
            this.cb_DXLoc.GeoLocation = gPoint5;
            this.cb_DXLoc.Location = new System.Drawing.Point(6, 243);
            this.cb_DXLoc.Name = "cb_DXLoc";
            this.cb_DXLoc.Precision = 3;
            this.cb_DXLoc.SilentItemChange = false;
            this.cb_DXLoc.Size = new System.Drawing.Size(133, 24);
            this.cb_DXLoc.SmallLettersForSubsquares = true;
            this.cb_DXLoc.TabIndex = 63;
            this.cb_DXLoc.TextChanged += new System.EventHandler(this.cb_DXLoc_TextChanged);
            // 
            // cb_MyLoc
            // 
            this.cb_MyLoc.AutoLength = false;
            this.cb_MyLoc.BackColor = System.Drawing.SystemColors.Window;
            this.cb_MyLoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cb_MyLoc.ErrorBackColor = System.Drawing.Color.Red;
            this.cb_MyLoc.ErrorForeColor = System.Drawing.Color.White;
            this.cb_MyLoc.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_MyLoc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_MyLoc.FormattingEnabled = true;
            this.cb_MyLoc.GeoLocation = gPoint6;
            this.cb_MyLoc.Location = new System.Drawing.Point(6, 161);
            this.cb_MyLoc.Name = "cb_MyLoc";
            this.cb_MyLoc.Precision = 3;
            this.cb_MyLoc.SilentItemChange = false;
            this.cb_MyLoc.Size = new System.Drawing.Size(133, 24);
            this.cb_MyLoc.SmallLettersForSubsquares = true;
            this.cb_MyLoc.TabIndex = 62;
            this.cb_MyLoc.TextChanged += new System.EventHandler(this.cb_MyLoc_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Band";
            // 
            // cb_Band
            // 
            this.cb_Band.AllowDrop = true;
            this.cb_Band.BackColor = System.Drawing.Color.FloralWhite;
            this.cb_Band.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Band.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Band.FormattingEnabled = true;
            this.cb_Band.Location = new System.Drawing.Point(6, 74);
            this.cb_Band.Name = "cb_Band";
            this.cb_Band.Size = new System.Drawing.Size(133, 24);
            this.cb_Band.TabIndex = 29;
            // 
            // tb_QTF
            // 
            this.tb_QTF.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_QTF.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_QTF.Location = new System.Drawing.Point(6, 323);
            this.tb_QTF.Name = "tb_QTF";
            this.tb_QTF.ReadOnly = true;
            this.tb_QTF.Size = new System.Drawing.Size(133, 22);
            this.tb_QTF.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "QTF";
            // 
            // tb_QRB
            // 
            this.tb_QRB.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_QRB.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_QRB.Location = new System.Drawing.Point(6, 282);
            this.tb_QRB.Name = "tb_QRB";
            this.tb_QRB.ReadOnly = true;
            this.tb_QRB.Size = new System.Drawing.Size(133, 22);
            this.tb_QRB.TabIndex = 25;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(7, 266);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 13);
            this.label16.TabIndex = 24;
            this.label16.Text = "QRB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "DXLoc";
            // 
            // tb_DXCall
            // 
            this.tb_DXCall.BackColor = System.Drawing.SystemColors.Window;
            this.tb_DXCall.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_DXCall.ErrorBackColor = System.Drawing.Color.Red;
            this.tb_DXCall.ErrorForeColor = System.Drawing.Color.White;
            this.tb_DXCall.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DXCall.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tb_DXCall.Location = new System.Drawing.Point(6, 202);
            this.tb_DXCall.Name = "tb_DXCall";
            this.tb_DXCall.Size = new System.Drawing.Size(133, 22);
            this.tb_DXCall.TabIndex = 21;
            this.tb_DXCall.TextChanged += new System.EventHandler(this.tb_DXCall_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "DXCall";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "MyLoc";
            // 
            // tb_MyCall
            // 
            this.tb_MyCall.BackColor = System.Drawing.SystemColors.Window;
            this.tb_MyCall.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_MyCall.ErrorBackColor = System.Drawing.Color.Red;
            this.tb_MyCall.ErrorForeColor = System.Drawing.Color.White;
            this.tb_MyCall.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_MyCall.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tb_MyCall.Location = new System.Drawing.Point(6, 120);
            this.tb_MyCall.Name = "tb_MyCall";
            this.tb_MyCall.Size = new System.Drawing.Size(133, 22);
            this.tb_MyCall.TabIndex = 17;
            this.tb_MyCall.TextChanged += new System.EventHandler(this.tb_MyCall_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "MyCall";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "UTC";
            // 
            // tb_UTC
            // 
            this.tb_UTC.BackColor = System.Drawing.Color.LightSalmon;
            this.tb_UTC.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_UTC.ForeColor = System.Drawing.Color.White;
            this.tb_UTC.Location = new System.Drawing.Point(6, 33);
            this.tb_UTC.Name = "tb_UTC";
            this.tb_UTC.ReadOnly = true;
            this.tb_UTC.Size = new System.Drawing.Size(133, 22);
            this.tb_UTC.TabIndex = 14;
            // 
            // gb_Map
            // 
            this.gb_Map.Controls.Add(this.spc_Main);
            this.gb_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Map.Location = new System.Drawing.Point(0, 24);
            this.gb_Map.Name = "gb_Map";
            this.gb_Map.Size = new System.Drawing.Size(691, 494);
            this.gb_Map.TabIndex = 4;
            this.gb_Map.TabStop = false;
            this.gb_Map.Text = "Map";
            // 
            // spc_Main
            // 
            this.spc_Main.BackColor = System.Drawing.SystemColors.ControlDark;
            this.spc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_Main.Location = new System.Drawing.Point(3, 16);
            this.spc_Main.Name = "spc_Main";
            this.spc_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spc_Main.Panel1
            // 
            this.spc_Main.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.spc_Main.Panel1.Controls.Add(this.gm_Main);
            this.spc_Main.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // spc_Main.Panel2
            // 
            this.spc_Main.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.spc_Main.Panel2.Controls.Add(this.gb_Path);
            this.spc_Main.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.spc_Main.Size = new System.Drawing.Size(685, 475);
            this.spc_Main.SplitterDistance = 300;
            this.spc_Main.TabIndex = 0;
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
            this.gm_Main.PolygonsEnabled = true;
            this.gm_Main.RetryLoadTile = 0;
            this.gm_Main.RoutesEnabled = true;
            this.gm_Main.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Main.ShowTileGridLines = false;
            this.gm_Main.Size = new System.Drawing.Size(685, 300);
            this.gm_Main.TabIndex = 0;
            this.gm_Main.Zoom = 0D;
            this.gm_Main.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gm_Main_OnMarkerClick);
            this.gm_Main.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gm_Main_OnMapZoomChanged);
            // 
            // gb_Path
            // 
            this.gb_Path.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Path.Location = new System.Drawing.Point(0, 0);
            this.gb_Path.Name = "gb_Path";
            this.gb_Path.Size = new System.Drawing.Size(685, 171);
            this.gb_Path.TabIndex = 0;
            this.gb_Path.TabStop = false;
            this.gb_Path.Text = "Path";
            this.gb_Path.Resize += new System.EventHandler(this.gb_Path_Resize);
            // 
            // ti_Progress
            // 
            this.ti_Progress.Enabled = true;
            this.ti_Progress.Interval = 1000;
            this.ti_Progress.Tick += new System.EventHandler(this.ti_Progress_Tick);
            // 
            // ti_Startup
            // 
            this.ti_Startup.Enabled = true;
            this.ti_Startup.Interval = 1000;
            this.ti_Startup.Tick += new System.EventHandler(this.ti_Startup_Tick);
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
            // il_Airports
            // 
            this.il_Airports.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Airports.ImageSize = new System.Drawing.Size(16, 16);
            this.il_Airports.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // il_Planes_M
            // 
            this.il_Planes_M.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.il_Planes_M.ImageSize = new System.Drawing.Size(24, 24);
            this.il_Planes_M.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ti_ShowLegends
            // 
            this.ti_ShowLegends.Enabled = true;
            this.ti_ShowLegends.Interval = 5000;
            this.ti_ShowLegends.Tick += new System.EventHandler(this.ti_ShowLegends_Tick);
            // 
            // btn_Map_PlayPause
            // 
            this.btn_Map_PlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Map_PlayPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Map_PlayPause.ImageIndex = 1;
            this.btn_Map_PlayPause.ImageList = this.il_Main;
            this.btn_Map_PlayPause.Location = new System.Drawing.Point(14, 17);
            this.btn_Map_PlayPause.Name = "btn_Map_PlayPause";
            this.btn_Map_PlayPause.Size = new System.Drawing.Size(114, 29);
            this.btn_Map_PlayPause.TabIndex = 64;
            this.btn_Map_PlayPause.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Map_PlayPause.UseVisualStyleBackColor = true;
            this.btn_Map_PlayPause.Click += new System.EventHandler(this.btn_Map_PlayPause_Click);
            // 
            // gb_Control
            // 
            this.gb_Control.Controls.Add(this.btn_Map_PlayPause);
            this.gb_Control.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_Control.Location = new System.Drawing.Point(3, 436);
            this.gb_Control.Name = "gb_Control";
            this.gb_Control.Size = new System.Drawing.Size(139, 55);
            this.gb_Control.TabIndex = 64;
            this.gb_Control.TabStop = false;
            // 
            // il_Main
            // 
            this.il_Main.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_Main.ImageStream")));
            this.il_Main.TransparentColor = System.Drawing.Color.Transparent;
            this.il_Main.Images.SetKeyName(0, "PauseHS.png");
            this.il_Main.Images.SetKeyName(1, "PlayHS.png");
            this.il_Main.Images.SetKeyName(2, "RecordHS.png");
            // 
            // gb_Map_Zoom
            // 
            this.gb_Map_Zoom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gb_Map_Zoom.Controls.Add(this.pa_Map_Zoom);
            this.gb_Map_Zoom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_Map_Zoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Map_Zoom.Location = new System.Drawing.Point(3, 357);
            this.gb_Map_Zoom.Name = "gb_Map_Zoom";
            this.gb_Map_Zoom.Size = new System.Drawing.Size(139, 79);
            this.gb_Map_Zoom.TabIndex = 66;
            this.gb_Map_Zoom.TabStop = false;
            this.gb_Map_Zoom.Text = "Map Zoom";
            // 
            // pa_Map_Zoom
            // 
            this.pa_Map_Zoom.Controls.Add(this.cb_Map_AutoCenter);
            this.pa_Map_Zoom.Controls.Add(this.tb_Map_Zoom);
            this.pa_Map_Zoom.Controls.Add(this.btn_Map_Zoom_Out);
            this.pa_Map_Zoom.Controls.Add(this.btn_Map_Zoom_In);
            this.pa_Map_Zoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pa_Map_Zoom.Location = new System.Drawing.Point(3, 16);
            this.pa_Map_Zoom.Name = "pa_Map_Zoom";
            this.pa_Map_Zoom.Size = new System.Drawing.Size(133, 60);
            this.pa_Map_Zoom.TabIndex = 65;
            // 
            // tb_Map_Zoom
            // 
            this.tb_Map_Zoom.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_Map_Zoom.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Map_Zoom.Location = new System.Drawing.Point(45, 9);
            this.tb_Map_Zoom.Name = "tb_Map_Zoom";
            this.tb_Map_Zoom.Size = new System.Drawing.Size(41, 22);
            this.tb_Map_Zoom.TabIndex = 23;
            // 
            // btn_Map_Zoom_Out
            // 
            this.btn_Map_Zoom_Out.Location = new System.Drawing.Point(92, 8);
            this.btn_Map_Zoom_Out.Name = "btn_Map_Zoom_Out";
            this.btn_Map_Zoom_Out.Size = new System.Drawing.Size(30, 23);
            this.btn_Map_Zoom_Out.TabIndex = 22;
            this.btn_Map_Zoom_Out.Text = "-";
            this.btn_Map_Zoom_Out.UseVisualStyleBackColor = true;
            this.btn_Map_Zoom_Out.Click += new System.EventHandler(this.btn_Map_Zoom_Out_Click);
            // 
            // btn_Map_Zoom_In
            // 
            this.btn_Map_Zoom_In.Location = new System.Drawing.Point(9, 8);
            this.btn_Map_Zoom_In.Name = "btn_Map_Zoom_In";
            this.btn_Map_Zoom_In.Size = new System.Drawing.Size(30, 23);
            this.btn_Map_Zoom_In.TabIndex = 21;
            this.btn_Map_Zoom_In.Text = "+";
            this.btn_Map_Zoom_In.UseVisualStyleBackColor = true;
            this.btn_Map_Zoom_In.Click += new System.EventHandler(this.btn_Map_Zoom_In_Click);
            // 
            // tsl_ConnectionStatus
            // 
            this.tsl_ConnectionStatus.Name = "tsl_ConnectionStatus";
            this.tsl_ConnectionStatus.Size = new System.Drawing.Size(39, 17);
            this.tsl_ConnectionStatus.Text = "Status";
            // 
            // cb_Map_AutoCenter
            // 
            this.cb_Map_AutoCenter.AutoSize = true;
            this.cb_Map_AutoCenter.Checked = global::AirScoutViewClient.Properties.Settings.Default.Map_AutoCenter;
            this.cb_Map_AutoCenter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Map_AutoCenter.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScoutViewClient.Properties.Settings.Default, "Map_AutoCenter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Map_AutoCenter.Location = new System.Drawing.Point(12, 37);
            this.cb_Map_AutoCenter.Name = "cb_Map_AutoCenter";
            this.cb_Map_AutoCenter.Size = new System.Drawing.Size(93, 17);
            this.cb_Map_AutoCenter.TabIndex = 24;
            this.cb_Map_AutoCenter.Text = "Auto Center";
            this.cb_Map_AutoCenter.UseVisualStyleBackColor = true;
            // 
            // MapViewDlg
            // 
            this.AcceptButton = this.btn_Map_PlayPause;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 540);
            this.Controls.Add(this.gb_Map);
            this.Controls.Add(this.gb_Info);
            this.Controls.Add(this.ss_Main);
            this.Controls.Add(this.mnu_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnu_Main;
            this.Name = "MapViewDlg";
            this.Text = "AirScoutViewClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapViewDlg_FormClosing);
            this.Load += new System.EventHandler(this.MapViewDlg_Load);
            this.mnu_Main.ResumeLayout(false);
            this.mnu_Main.PerformLayout();
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.gb_Info.ResumeLayout(false);
            this.gb_Info.PerformLayout();
            this.gb_Map.ResumeLayout(false);
            this.spc_Main.Panel1.ResumeLayout(false);
            this.spc_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spc_Main)).EndInit();
            this.spc_Main.ResumeLayout(false);
            this.gb_Control.ResumeLayout(false);
            this.gb_Map_Zoom.ResumeLayout(false);
            this.pa_Map_Zoom.ResumeLayout(false);
            this.pa_Map_Zoom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnu_Main;
        private System.Windows.Forms.ToolStripMenuItem tsi_Exit;
        private System.Windows.Forms.ToolStripMenuItem tsi_Info;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.GroupBox gb_Info;
        private System.Windows.Forms.GroupBox gb_Map;
        private System.Windows.Forms.Timer ti_Progress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_UTC;
        private ScoutBase.Core.CallsignTextBox tb_MyCall;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ScoutBase.Core.CallsignTextBox tb_DXCall;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_QTF;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_QRB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_Band;
        private System.Windows.Forms.SplitContainer spc_Main;
        private GMap.NET.WindowsForms.GMapControl gm_Main;
        private System.Windows.Forms.Timer ti_Startup;
        public System.Windows.Forms.ImageList il_Planes_H;
        public System.Windows.Forms.ImageList il_Planes_L;
        public System.Windows.Forms.ImageList il_Planes_S;
        public System.Windows.Forms.ImageList il_Airports;
        public System.Windows.Forms.ImageList il_Planes_M;
        private System.Windows.Forms.GroupBox gb_Path;
        private System.Windows.Forms.Timer ti_ShowLegends;
        private ScoutBase.Core.LocatorComboBox cb_DXLoc;
        private ScoutBase.Core.LocatorComboBox cb_MyLoc;
        private System.Windows.Forms.GroupBox gb_Control;
        private System.Windows.Forms.Button btn_Map_PlayPause;
        private System.Windows.Forms.ImageList il_Main;
        private System.Windows.Forms.GroupBox gb_Map_Zoom;
        private System.Windows.Forms.Panel pa_Map_Zoom;
        private System.Windows.Forms.CheckBox cb_Map_AutoCenter;
        private System.Windows.Forms.TextBox tb_Map_Zoom;
        private System.Windows.Forms.Button btn_Map_Zoom_Out;
        private System.Windows.Forms.Button btn_Map_Zoom_In;
        private System.Windows.Forms.ToolStripStatusLabel tsl_ConnectionStatus;
    }
}

