namespace AirScout
{
    partial class HorizonDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Horizon_Lat = new System.Windows.Forms.TextBox();
            this.tb_Horizon_Lon = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Horizon_Elevation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Horizon_K_Factor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Horizon_QRG = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Horizon_F1_Clearance = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_Horizon_ElevationModel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gb_Parameter = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Horizon_Height = new System.Windows.Forms.TextBox();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.bw_Horizon_Calculate = new System.ComponentModel.BackgroundWorker();
            this.pa_Buttons = new System.Windows.Forms.Panel();
            this.btn_Horizon_Cancel = new System.Windows.Forms.Button();
            this.btn_Horizon_Export = new System.Windows.Forms.Button();
            this.btn_Horizon_Calculate = new System.Windows.Forms.Button();
            this.gb_Distance = new System.Windows.Forms.GroupBox();
            this.gb_Options = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rb_Horizon_Plot_Map = new System.Windows.Forms.RadioButton();
            this.rb_Horizon_Plot_Polar = new System.Windows.Forms.RadioButton();
            this.rb_Horizon_Plot_Cartesian = new System.Windows.Forms.RadioButton();
            this.gb_Map = new System.Windows.Forms.GroupBox();
            this.gm_Horizon = new GMap.NET.WindowsForms.GMapControl();
            this.gb_Elevation = new System.Windows.Forms.GroupBox();
            this.gb_Parameter.SuspendLayout();
            this.ss_Main.SuspendLayout();
            this.pa_Buttons.SuspendLayout();
            this.gb_Options.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gb_Map.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Latitude:";
            // 
            // tb_Horizon_Lat
            // 
            this.tb_Horizon_Lat.Enabled = false;
            this.tb_Horizon_Lat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_Lat.Location = new System.Drawing.Point(100, 16);
            this.tb_Horizon_Lat.Name = "tb_Horizon_Lat";
            this.tb_Horizon_Lat.ReadOnly = true;
            this.tb_Horizon_Lat.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_Lat.TabIndex = 1;
            // 
            // tb_Horizon_Lon
            // 
            this.tb_Horizon_Lon.Enabled = false;
            this.tb_Horizon_Lon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_Lon.Location = new System.Drawing.Point(100, 42);
            this.tb_Horizon_Lon.Name = "tb_Horizon_Lon";
            this.tb_Horizon_Lon.ReadOnly = true;
            this.tb_Horizon_Lon.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_Lon.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Longiitude:";
            // 
            // tb_Horizon_Elevation
            // 
            this.tb_Horizon_Elevation.Enabled = false;
            this.tb_Horizon_Elevation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_Elevation.Location = new System.Drawing.Point(100, 69);
            this.tb_Horizon_Elevation.Name = "tb_Horizon_Elevation";
            this.tb_Horizon_Elevation.ReadOnly = true;
            this.tb_Horizon_Elevation.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_Elevation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Elevation:";
            // 
            // tb_Horizon_K_Factor
            // 
            this.tb_Horizon_K_Factor.Enabled = false;
            this.tb_Horizon_K_Factor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_K_Factor.Location = new System.Drawing.Point(379, 16);
            this.tb_Horizon_K_Factor.Name = "tb_Horizon_K_Factor";
            this.tb_Horizon_K_Factor.ReadOnly = true;
            this.tb_Horizon_K_Factor.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_K_Factor.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(224, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "K-Factor:";
            // 
            // tb_Horizon_QRG
            // 
            this.tb_Horizon_QRG.Enabled = false;
            this.tb_Horizon_QRG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_QRG.Location = new System.Drawing.Point(379, 42);
            this.tb_Horizon_QRG.Name = "tb_Horizon_QRG";
            this.tb_Horizon_QRG.ReadOnly = true;
            this.tb_Horizon_QRG.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_QRG.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(224, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "QRG:";
            // 
            // tb_Horizon_F1_Clearance
            // 
            this.tb_Horizon_F1_Clearance.Enabled = false;
            this.tb_Horizon_F1_Clearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_F1_Clearance.Location = new System.Drawing.Point(379, 68);
            this.tb_Horizon_F1_Clearance.Name = "tb_Horizon_F1_Clearance";
            this.tb_Horizon_F1_Clearance.ReadOnly = true;
            this.tb_Horizon_F1_Clearance.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_F1_Clearance.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(224, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Fresnel Zone F1-Clearance:";
            // 
            // tb_Horizon_ElevationModel
            // 
            this.tb_Horizon_ElevationModel.Enabled = false;
            this.tb_Horizon_ElevationModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_ElevationModel.Location = new System.Drawing.Point(379, 94);
            this.tb_Horizon_ElevationModel.Name = "tb_Horizon_ElevationModel";
            this.tb_Horizon_ElevationModel.ReadOnly = true;
            this.tb_Horizon_ElevationModel.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_ElevationModel.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(224, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Elevation Model";
            // 
            // gb_Parameter
            // 
            this.gb_Parameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Parameter.Controls.Add(this.label8);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_Height);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_Lat);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_ElevationModel);
            this.gb_Parameter.Controls.Add(this.label1);
            this.gb_Parameter.Controls.Add(this.label7);
            this.gb_Parameter.Controls.Add(this.label2);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_F1_Clearance);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_Lon);
            this.gb_Parameter.Controls.Add(this.label6);
            this.gb_Parameter.Controls.Add(this.label3);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_QRG);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_Elevation);
            this.gb_Parameter.Controls.Add(this.label5);
            this.gb_Parameter.Controls.Add(this.label4);
            this.gb_Parameter.Controls.Add(this.tb_Horizon_K_Factor);
            this.gb_Parameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Parameter.Location = new System.Drawing.Point(12, 12);
            this.gb_Parameter.Name = "gb_Parameter";
            this.gb_Parameter.Size = new System.Drawing.Size(499, 126);
            this.gb_Parameter.TabIndex = 14;
            this.gb_Parameter.TabStop = false;
            this.gb_Parameter.Text = "Parameters";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Antenna Height:";
            // 
            // tb_Horizon_Height
            // 
            this.tb_Horizon_Height.Enabled = false;
            this.tb_Horizon_Height.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Horizon_Height.Location = new System.Drawing.Point(100, 94);
            this.tb_Horizon_Height.Name = "tb_Horizon_Height";
            this.tb_Horizon_Height.ReadOnly = true;
            this.tb_Horizon_Height.Size = new System.Drawing.Size(100, 20);
            this.tb_Horizon_Height.TabIndex = 15;
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Main,
            this.toolStripStatusLabel1});
            this.ss_Main.Location = new System.Drawing.Point(0, 540);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(784, 22);
            this.ss_Main.TabIndex = 21;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Main
            // 
            this.tsl_Main.Name = "tsl_Main";
            this.tsl_Main.Size = new System.Drawing.Size(197, 17);
            this.tsl_Main.Text = "Press Calculate to begin calculation.";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // bw_Horizon_Calculate
            // 
            this.bw_Horizon_Calculate.WorkerReportsProgress = true;
            this.bw_Horizon_Calculate.WorkerSupportsCancellation = true;
            this.bw_Horizon_Calculate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Horizon_Calculate_DoWork);
            this.bw_Horizon_Calculate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Horizon_Calculate_ProgressChanged);
            this.bw_Horizon_Calculate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Horizon_Calculate_RunWorkerCompleted);
            // 
            // pa_Buttons
            // 
            this.pa_Buttons.Controls.Add(this.btn_Horizon_Cancel);
            this.pa_Buttons.Controls.Add(this.btn_Horizon_Export);
            this.pa_Buttons.Controls.Add(this.btn_Horizon_Calculate);
            this.pa_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_Buttons.Location = new System.Drawing.Point(0, 490);
            this.pa_Buttons.Name = "pa_Buttons";
            this.pa_Buttons.Size = new System.Drawing.Size(784, 50);
            this.pa_Buttons.TabIndex = 25;
            // 
            // btn_Horizon_Cancel
            // 
            this.btn_Horizon_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Horizon_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Horizon_Cancel.Location = new System.Drawing.Point(698, 15);
            this.btn_Horizon_Cancel.Name = "btn_Horizon_Cancel";
            this.btn_Horizon_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Horizon_Cancel.TabIndex = 27;
            this.btn_Horizon_Cancel.Text = "Back";
            this.btn_Horizon_Cancel.UseVisualStyleBackColor = true;
            this.btn_Horizon_Cancel.Click += new System.EventHandler(this.btn_Horizon_Cancel_Click);
            // 
            // btn_Horizon_Export
            // 
            this.btn_Horizon_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Horizon_Export.Location = new System.Drawing.Point(588, 15);
            this.btn_Horizon_Export.Name = "btn_Horizon_Export";
            this.btn_Horizon_Export.Size = new System.Drawing.Size(104, 23);
            this.btn_Horizon_Export.TabIndex = 26;
            this.btn_Horizon_Export.Text = "Export to CSV";
            this.btn_Horizon_Export.UseVisualStyleBackColor = true;
            this.btn_Horizon_Export.Click += new System.EventHandler(this.btn_Horizon_Export_Click);
            // 
            // btn_Horizon_Calculate
            // 
            this.btn_Horizon_Calculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Horizon_Calculate.Location = new System.Drawing.Point(507, 15);
            this.btn_Horizon_Calculate.Name = "btn_Horizon_Calculate";
            this.btn_Horizon_Calculate.Size = new System.Drawing.Size(75, 23);
            this.btn_Horizon_Calculate.TabIndex = 25;
            this.btn_Horizon_Calculate.Text = "Calculate";
            this.btn_Horizon_Calculate.UseVisualStyleBackColor = true;
            this.btn_Horizon_Calculate.Click += new System.EventHandler(this.btn_Horizon_Calculate_Click);
            // 
            // gb_Distance
            // 
            this.gb_Distance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gb_Distance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Distance.Location = new System.Drawing.Point(397, 144);
            this.gb_Distance.Name = "gb_Distance";
            this.gb_Distance.Size = new System.Drawing.Size(375, 340);
            this.gb_Distance.TabIndex = 27;
            this.gb_Distance.TabStop = false;
            this.gb_Distance.Text = "Horizon Distance versus True North [km]";
            // 
            // gb_Options
            // 
            this.gb_Options.Controls.Add(this.panel2);
            this.gb_Options.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Options.Location = new System.Drawing.Point(517, 12);
            this.gb_Options.Name = "gb_Options";
            this.gb_Options.Size = new System.Drawing.Size(251, 125);
            this.gb_Options.TabIndex = 28;
            this.gb_Options.TabStop = false;
            this.gb_Options.Text = "Plot Options";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rb_Horizon_Plot_Map);
            this.panel2.Controls.Add(this.rb_Horizon_Plot_Polar);
            this.panel2.Controls.Add(this.rb_Horizon_Plot_Cartesian);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(245, 106);
            this.panel2.TabIndex = 2;
            // 
            // rb_Horizon_Plot_Map
            // 
            this.rb_Horizon_Plot_Map.AutoSize = true;
            this.rb_Horizon_Plot_Map.Checked = global::AirScout.Properties.Settings.Default.Horizon_Plot_Map;
            this.rb_Horizon_Plot_Map.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Horizon_Plot_Map", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rb_Horizon_Plot_Map.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Horizon_Plot_Map.Location = new System.Drawing.Point(90, 68);
            this.rb_Horizon_Plot_Map.Name = "rb_Horizon_Plot_Map";
            this.rb_Horizon_Plot_Map.Size = new System.Drawing.Size(46, 17);
            this.rb_Horizon_Plot_Map.TabIndex = 4;
            this.rb_Horizon_Plot_Map.Text = "Map";
            this.rb_Horizon_Plot_Map.UseVisualStyleBackColor = true;
            this.rb_Horizon_Plot_Map.Click += new System.EventHandler(this.rb_Horizon_Plot_Map_Click);
            // 
            // rb_Horizon_Plot_Polar
            // 
            this.rb_Horizon_Plot_Polar.AutoSize = true;
            this.rb_Horizon_Plot_Polar.Checked = global::AirScout.Properties.Settings.Default.Horizon_Plot_Polar;
            this.rb_Horizon_Plot_Polar.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Horizon_Plot_Polar", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rb_Horizon_Plot_Polar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Horizon_Plot_Polar.Location = new System.Drawing.Point(90, 22);
            this.rb_Horizon_Plot_Polar.Name = "rb_Horizon_Plot_Polar";
            this.rb_Horizon_Plot_Polar.Size = new System.Drawing.Size(49, 17);
            this.rb_Horizon_Plot_Polar.TabIndex = 3;
            this.rb_Horizon_Plot_Polar.Text = "Polar";
            this.rb_Horizon_Plot_Polar.UseVisualStyleBackColor = true;
            this.rb_Horizon_Plot_Polar.Click += new System.EventHandler(this.rb_Horizon_Plot_Polar_Click);
            // 
            // rb_Horizon_Plot_Cartesian
            // 
            this.rb_Horizon_Plot_Cartesian.AutoSize = true;
            this.rb_Horizon_Plot_Cartesian.Checked = global::AirScout.Properties.Settings.Default.Horizon_Plot_Cartesian;
            this.rb_Horizon_Plot_Cartesian.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Horizon_Plot_Cartesian", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rb_Horizon_Plot_Cartesian.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Horizon_Plot_Cartesian.Location = new System.Drawing.Point(90, 45);
            this.rb_Horizon_Plot_Cartesian.Name = "rb_Horizon_Plot_Cartesian";
            this.rb_Horizon_Plot_Cartesian.Size = new System.Drawing.Size(69, 17);
            this.rb_Horizon_Plot_Cartesian.TabIndex = 2;
            this.rb_Horizon_Plot_Cartesian.Text = "Cartesian";
            this.rb_Horizon_Plot_Cartesian.UseVisualStyleBackColor = true;
            this.rb_Horizon_Plot_Cartesian.Click += new System.EventHandler(this.rb_Horizon_Plot_Cartesian_Click);
            // 
            // gb_Map
            // 
            this.gb_Map.Controls.Add(this.gm_Horizon);
            this.gb_Map.Location = new System.Drawing.Point(12, 144);
            this.gb_Map.Name = "gb_Map";
            this.gb_Map.Size = new System.Drawing.Size(282, 244);
            this.gb_Map.TabIndex = 30;
            this.gb_Map.TabStop = false;
            this.gb_Map.Text = "Map";
            this.gb_Map.Visible = false;
            // 
            // gm_Horizon
            // 
            this.gm_Horizon.Bearing = 0F;
            this.gm_Horizon.CanDragMap = true;
            this.gm_Horizon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gm_Horizon.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Horizon.GrayScaleMode = false;
            this.gm_Horizon.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Horizon.LevelsKeepInMemmory = 5;
            this.gm_Horizon.Location = new System.Drawing.Point(3, 16);
            this.gm_Horizon.MarkersEnabled = true;
            this.gm_Horizon.MaxZoom = 2;
            this.gm_Horizon.MinZoom = 2;
            this.gm_Horizon.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Horizon.Name = "gm_Horizon";
            this.gm_Horizon.NegativeMode = false;
            this.gm_Horizon.PolygonsEnabled = true;
            this.gm_Horizon.RetryLoadTile = 0;
            this.gm_Horizon.RoutesEnabled = true;
            this.gm_Horizon.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Horizon.ShowTileGridLines = false;
            this.gm_Horizon.Size = new System.Drawing.Size(276, 225);
            this.gm_Horizon.TabIndex = 4;
            this.gm_Horizon.Zoom = 0D;
            this.gm_Horizon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gm_Horizon_MouseDown);
            this.gm_Horizon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gm_Horizon_MouseUp);
            // 
            // gb_Elevation
            // 
            this.gb_Elevation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gb_Elevation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Elevation.Location = new System.Drawing.Point(75, 138);
            this.gb_Elevation.Name = "gb_Elevation";
            this.gb_Elevation.Size = new System.Drawing.Size(372, 340);
            this.gb_Elevation.TabIndex = 31;
            this.gb_Elevation.TabStop = false;
            this.gb_Elevation.Text = "Minimum Elevation Angle versus True North [deg]";
            // 
            // HorizonDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.gb_Map);
            this.Controls.Add(this.gb_Options);
            this.Controls.Add(this.gb_Elevation);
            this.Controls.Add(this.gb_Distance);
            this.Controls.Add(this.pa_Buttons);
            this.Controls.Add(this.ss_Main);
            this.Controls.Add(this.gb_Parameter);
            this.Name = "HorizonDlg";
            this.Text = "Radio Horizon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HorizonDlg_FormClosing);
            this.SizeChanged += new System.EventHandler(this.HorizonDlg_SizeChanged);
            this.gb_Parameter.ResumeLayout(false);
            this.gb_Parameter.PerformLayout();
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.pa_Buttons.ResumeLayout(false);
            this.gb_Options.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gb_Map.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Horizon_Lat;
        private System.Windows.Forms.TextBox tb_Horizon_Lon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Horizon_Elevation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Horizon_K_Factor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Horizon_QRG;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Horizon_F1_Clearance;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_Horizon_ElevationModel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gb_Parameter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Horizon_Height;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Main;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker bw_Horizon_Calculate;
        private System.Windows.Forms.Panel pa_Buttons;
        private System.Windows.Forms.Button btn_Horizon_Cancel;
        private System.Windows.Forms.Button btn_Horizon_Export;
        private System.Windows.Forms.Button btn_Horizon_Calculate;
        private System.Windows.Forms.GroupBox gb_Distance;
        private System.Windows.Forms.GroupBox gb_Options;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rb_Horizon_Plot_Polar;
        private System.Windows.Forms.RadioButton rb_Horizon_Plot_Cartesian;
        private System.Windows.Forms.RadioButton rb_Horizon_Plot_Map;
        private System.Windows.Forms.GroupBox gb_Map;
        private GMap.NET.WindowsForms.GMapControl gm_Horizon;
        private System.Windows.Forms.GroupBox gb_Elevation;
    }
}