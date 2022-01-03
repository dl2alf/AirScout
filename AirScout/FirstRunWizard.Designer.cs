namespace AirScout
{
    partial class FirstRunWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstRunWizard));
            this.sw_FirstRun = new AeroWizard.StepWizardControl();
            this.wp_TermsAndConditions = new AeroWizard.WizardPage();
            this.cb_TermsAndConditions = new System.Windows.Forms.CheckBox();
            this.rb_TermsAndConditions = new System.Windows.Forms.RichTextBox();
            this.wp_GeneralCoverage = new AeroWizard.WizardPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ud_MaxLat = new System.Windows.Forms.NumericUpDown();
            this.ud_MinLat = new System.Windows.Forms.NumericUpDown();
            this.ud_MaxLon = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ud_MinLon = new System.Windows.Forms.NumericUpDown();
            this.gm_Coverage = new GMap.NET.WindowsForms.GMapControl();
            this.wp_ElevationModel = new AeroWizard.WizardPage();
            this.cb_ASTER1 = new System.Windows.Forms.CheckBox();
            this.cb_ASTER3 = new System.Windows.Forms.CheckBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.cb_SRTM1 = new System.Windows.Forms.CheckBox();
            this.cb_SRTM3 = new System.Windows.Forms.CheckBox();
            this.cb_GLOBE = new System.Windows.Forms.CheckBox();
            this.wp_GLOBE = new AeroWizard.WizardPage();
            this.lbl_GLOBE_Status = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_GLOBE_EnableCache = new System.Windows.Forms.CheckBox();
            this.gm_GLOBE = new GMap.NET.WindowsForms.GMapControl();
            this.wp_SRTM3 = new AeroWizard.WizardPage();
            this.lbl_SRTM3_Status = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cb_SRTM3_EnableCache = new System.Windows.Forms.CheckBox();
            this.gm_SRTM3 = new GMap.NET.WindowsForms.GMapControl();
            this.wp_SRTM1 = new AeroWizard.WizardPage();
            this.lbl_SRTM1_Status = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cb_SRTM1_EnableCache = new System.Windows.Forms.CheckBox();
            this.gm_SRTM1 = new GMap.NET.WindowsForms.GMapControl();
            this.wp_ASTER3 = new AeroWizard.WizardPage();
            this.lbl_ASTER3_Status = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cb_ASTER3_EnableCache = new System.Windows.Forms.CheckBox();
            this.gm_ASTER3 = new GMap.NET.WindowsForms.GMapControl();
            this.wp_ASTER1 = new AeroWizard.WizardPage();
            this.lbl_ASTER1_Status = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.cb_ASTER1_EnableCache = new System.Windows.Forms.CheckBox();
            this.gm_ASTER1 = new GMap.NET.WindowsForms.GMapControl();
            this.wp_UserDetails = new AeroWizard.WizardPage();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_Zoom = new System.Windows.Forms.TextBox();
            this.btn_Zoom_Out = new System.Windows.Forms.Button();
            this.btn_Zoom_In = new System.Windows.Forms.Button();
            this.btn_QRZ = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.gm_Callsign = new GMap.NET.WindowsForms.GMapControl();
            this.tb_Callsign = new ScoutBase.Core.CallsignTextBox();
            this.tb_Locator = new ScoutBase.Core.LocatorTextBox();
            this.tb_Longitude = new ScoutBase.Core.DoubleTextBox();
            this.tb_Latitude = new ScoutBase.Core.DoubleTextBox();
            this.wp_PlaneFeeds = new AeroWizard.WizardPage();
            this.label20 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cb_PlaneFeed3 = new System.Windows.Forms.ComboBox();
            this.cb_PlaneFeed2 = new System.Windows.Forms.ComboBox();
            this.cb_PlaneFeed1 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.wp_Finish = new AeroWizard.WizardPage();
            this.lbl_Finish = new System.Windows.Forms.Label();
            this.bw_GLOBE_MapUpdater = new System.ComponentModel.BackgroundWorker();
            this.bw_SRTM3_MapUpdater = new System.ComponentModel.BackgroundWorker();
            this.bw_SRTM1_MapUpdater = new System.ComponentModel.BackgroundWorker();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.bw_ASTER3_MapUpdater = new System.ComponentModel.BackgroundWorker();
            this.bw_ASTER1_MapUpdater = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.sw_FirstRun)).BeginInit();
            this.wp_TermsAndConditions.SuspendLayout();
            this.wp_GeneralCoverage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLon)).BeginInit();
            this.wp_ElevationModel.SuspendLayout();
            this.wp_GLOBE.SuspendLayout();
            this.wp_SRTM3.SuspendLayout();
            this.wp_SRTM1.SuspendLayout();
            this.wp_ASTER3.SuspendLayout();
            this.wp_ASTER1.SuspendLayout();
            this.wp_UserDetails.SuspendLayout();
            this.wp_PlaneFeeds.SuspendLayout();
            this.wp_Finish.SuspendLayout();
            this.ss_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // sw_FirstRun
            // 
            this.sw_FirstRun.BackColor = System.Drawing.Color.White;
            this.sw_FirstRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sw_FirstRun.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sw_FirstRun.Location = new System.Drawing.Point(0, 0);
            this.sw_FirstRun.Name = "sw_FirstRun";
            this.sw_FirstRun.Pages.Add(this.wp_TermsAndConditions);
            this.sw_FirstRun.Pages.Add(this.wp_GeneralCoverage);
            this.sw_FirstRun.Pages.Add(this.wp_ElevationModel);
            this.sw_FirstRun.Pages.Add(this.wp_GLOBE);
            this.sw_FirstRun.Pages.Add(this.wp_SRTM3);
            this.sw_FirstRun.Pages.Add(this.wp_SRTM1);
            this.sw_FirstRun.Pages.Add(this.wp_ASTER3);
            this.sw_FirstRun.Pages.Add(this.wp_ASTER1);
            this.sw_FirstRun.Pages.Add(this.wp_UserDetails);
            this.sw_FirstRun.Pages.Add(this.wp_PlaneFeeds);
            this.sw_FirstRun.Pages.Add(this.wp_Finish);
            this.sw_FirstRun.Size = new System.Drawing.Size(834, 539);
            this.sw_FirstRun.StepListFont = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sw_FirstRun.StepListWidth = 200;
            this.sw_FirstRun.TabIndex = 0;
            this.sw_FirstRun.Text = "Welcome to AirScout";
            this.sw_FirstRun.Title = "Welcome to AirScout";
            this.sw_FirstRun.TitleIcon = ((System.Drawing.Icon)(resources.GetObject("sw_FirstRun.TitleIcon")));
            // 
            // wp_TermsAndConditions
            // 
            this.wp_TermsAndConditions.AllowNext = false;
            this.wp_TermsAndConditions.Controls.Add(this.cb_TermsAndConditions);
            this.wp_TermsAndConditions.Controls.Add(this.rb_TermsAndConditions);
            this.wp_TermsAndConditions.Name = "wp_TermsAndConditions";
            this.wp_TermsAndConditions.NextPage = this.wp_GeneralCoverage;
            this.wp_TermsAndConditions.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_TermsAndConditions, "Agree to the Terms and Conditions");
            this.wp_TermsAndConditions.TabIndex = 2;
            this.wp_TermsAndConditions.Text = "Agree to the Terms and C.";
            this.wp_TermsAndConditions.Enter += new System.EventHandler(this.wp_TermsAndConditions_Enter);
            // 
            // cb_TermsAndConditions
            // 
            this.cb_TermsAndConditions.AutoSize = true;
            this.cb_TermsAndConditions.Location = new System.Drawing.Point(15, 373);
            this.cb_TermsAndConditions.Name = "cb_TermsAndConditions";
            this.cb_TermsAndConditions.Size = new System.Drawing.Size(260, 19);
            this.cb_TermsAndConditions.TabIndex = 1;
            this.cb_TermsAndConditions.Text = "I agree with the terms and conditions above.";
            this.cb_TermsAndConditions.UseVisualStyleBackColor = true;
            this.cb_TermsAndConditions.CheckedChanged += new System.EventHandler(this.cb_TermsAndConditions_CheckedChanged);
            // 
            // rb_TermsAndConditions
            // 
            this.rb_TermsAndConditions.BackColor = System.Drawing.Color.White;
            this.rb_TermsAndConditions.Dock = System.Windows.Forms.DockStyle.Top;
            this.rb_TermsAndConditions.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_TermsAndConditions.Location = new System.Drawing.Point(0, 0);
            this.rb_TermsAndConditions.Name = "rb_TermsAndConditions";
            this.rb_TermsAndConditions.ReadOnly = true;
            this.rb_TermsAndConditions.Size = new System.Drawing.Size(586, 343);
            this.rb_TermsAndConditions.TabIndex = 0;
            this.rb_TermsAndConditions.Text = resources.GetString("rb_TermsAndConditions.Text");
            // 
            // wp_GeneralCoverage
            // 
            this.wp_GeneralCoverage.Controls.Add(this.label6);
            this.wp_GeneralCoverage.Controls.Add(this.label17);
            this.wp_GeneralCoverage.Controls.Add(this.label4);
            this.wp_GeneralCoverage.Controls.Add(this.label3);
            this.wp_GeneralCoverage.Controls.Add(this.ud_MaxLat);
            this.wp_GeneralCoverage.Controls.Add(this.ud_MinLat);
            this.wp_GeneralCoverage.Controls.Add(this.ud_MaxLon);
            this.wp_GeneralCoverage.Controls.Add(this.label2);
            this.wp_GeneralCoverage.Controls.Add(this.label1);
            this.wp_GeneralCoverage.Controls.Add(this.ud_MinLon);
            this.wp_GeneralCoverage.Controls.Add(this.gm_Coverage);
            this.wp_GeneralCoverage.Name = "wp_GeneralCoverage";
            this.wp_GeneralCoverage.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_GeneralCoverage, "Select the Covered Area");
            this.wp_GeneralCoverage.TabIndex = 3;
            this.wp_GeneralCoverage.Text = "Select the Covered Area";
            this.wp_GeneralCoverage.Enter += new System.EventHandler(this.wp_GeneralCoverage_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(14, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(532, 30);
            this.label6.TabIndex = 9;
            this.label6.Text = "CAUTION: Select area of interest just as large to cover a range of 1000-2000km ar" +
    "ound your qth.\r\nLarger areas together with SRTM3 and SRTM1 elevation data will r" +
    "esult in extensive disk space usage!";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(14, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(504, 15);
            this.label17.TabIndex = 1;
            this.label17.Text = "Select a covered area of your interest by entering lat/long values in the numeric" +
    " boxes below.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Max. Latitude (-90 ... + 90):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Min. Latitude (-90 ... +90):";
            // 
            // ud_MaxLat
            // 
            this.ud_MaxLat.Location = new System.Drawing.Point(163, 83);
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
            this.ud_MaxLat.Size = new System.Drawing.Size(49, 23);
            this.ud_MaxLat.TabIndex = 6;
            this.ud_MaxLat.ValueChanged += new System.EventHandler(this.ud_MaxLat_ValueChanged);
            // 
            // ud_MinLat
            // 
            this.ud_MinLat.Location = new System.Drawing.Point(163, 59);
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
            this.ud_MinLat.Size = new System.Drawing.Size(49, 23);
            this.ud_MinLat.TabIndex = 5;
            this.ud_MinLat.ValueChanged += new System.EventHandler(this.ud_MinLat_ValueChanged);
            // 
            // ud_MaxLon
            // 
            this.ud_MaxLon.Location = new System.Drawing.Point(424, 83);
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
            this.ud_MaxLon.Size = new System.Drawing.Size(49, 23);
            this.ud_MaxLon.TabIndex = 4;
            this.ud_MaxLon.ValueChanged += new System.EventHandler(this.ud_MaxLon_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max. Longitude (-180 ... +180):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Min. Longitude (-180 ...  +180):";
            // 
            // ud_MinLon
            // 
            this.ud_MinLon.Location = new System.Drawing.Point(426, 59);
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
            this.ud_MinLon.Size = new System.Drawing.Size(47, 23);
            this.ud_MinLon.TabIndex = 1;
            this.ud_MinLon.ValueChanged += new System.EventHandler(this.ud_MinLon_ValueChanged);
            // 
            // gm_Coverage
            // 
            this.gm_Coverage.Bearing = 0F;
            this.gm_Coverage.CanDragMap = true;
            this.gm_Coverage.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Coverage.GrayScaleMode = false;
            this.gm_Coverage.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Coverage.LevelsKeepInMemmory = 5;
            this.gm_Coverage.Location = new System.Drawing.Point(17, 108);
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
            this.gm_Coverage.Size = new System.Drawing.Size(456, 287);
            this.gm_Coverage.TabIndex = 0;
            this.gm_Coverage.Zoom = 0D;
            this.gm_Coverage.Enter += new System.EventHandler(this.gm_Coverage_Enter);
            // 
            // wp_ElevationModel
            // 
            this.wp_ElevationModel.Controls.Add(this.cb_ASTER1);
            this.wp_ElevationModel.Controls.Add(this.cb_ASTER3);
            this.wp_ElevationModel.Controls.Add(this.richTextBox2);
            this.wp_ElevationModel.Controls.Add(this.cb_SRTM1);
            this.wp_ElevationModel.Controls.Add(this.cb_SRTM3);
            this.wp_ElevationModel.Controls.Add(this.cb_GLOBE);
            this.wp_ElevationModel.Name = "wp_ElevationModel";
            this.wp_ElevationModel.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_ElevationModel, "Elevation Model");
            this.wp_ElevationModel.TabIndex = 7;
            this.wp_ElevationModel.Text = "Elevation Model";
            this.wp_ElevationModel.Enter += new System.EventHandler(this.wp_ElevationModel_Enter);
            // 
            // cb_ASTER1
            // 
            this.cb_ASTER1.AutoSize = true;
            this.cb_ASTER1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ASTER1.Location = new System.Drawing.Point(85, 355);
            this.cb_ASTER1.Name = "cb_ASTER1";
            this.cb_ASTER1.Size = new System.Drawing.Size(332, 25);
            this.cb_ASTER1.TabIndex = 6;
            this.cb_ASTER1.Text = "Use ASTER1 Elevation Model (experimental)";
            this.cb_ASTER1.UseVisualStyleBackColor = true;
            this.cb_ASTER1.CheckedChanged += new System.EventHandler(this.cb_ASTER1_CheckedChanged);
            // 
            // cb_ASTER3
            // 
            this.cb_ASTER3.AutoSize = true;
            this.cb_ASTER3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ASTER3.Location = new System.Drawing.Point(85, 329);
            this.cb_ASTER3.Name = "cb_ASTER3";
            this.cb_ASTER3.Size = new System.Drawing.Size(332, 25);
            this.cb_ASTER3.TabIndex = 5;
            this.cb_ASTER3.Text = "Use ASTER3 Elevation Model (experimental)";
            this.cb_ASTER3.UseVisualStyleBackColor = true;
            this.cb_ASTER3.CheckedChanged += new System.EventHandler(this.cb_ASTER3_CheckedChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.White;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Location = new System.Drawing.Point(13, 13);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(558, 212);
            this.richTextBox2.TabIndex = 4;
            this.richTextBox2.Text = resources.GetString("richTextBox2.Text");
            // 
            // cb_SRTM1
            // 
            this.cb_SRTM1.AutoSize = true;
            this.cb_SRTM1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_SRTM1.Location = new System.Drawing.Point(85, 303);
            this.cb_SRTM1.Name = "cb_SRTM1";
            this.cb_SRTM1.Size = new System.Drawing.Size(365, 25);
            this.cb_SRTM1.TabIndex = 3;
            this.cb_SRTM1.Text = "Use SRTM1 Elevation Model (not recommended)";
            this.cb_SRTM1.UseVisualStyleBackColor = true;
            this.cb_SRTM1.CheckedChanged += new System.EventHandler(this.cb_SRTM1_CheckedChanged);
            // 
            // cb_SRTM3
            // 
            this.cb_SRTM3.AutoSize = true;
            this.cb_SRTM3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_SRTM3.Location = new System.Drawing.Point(85, 277);
            this.cb_SRTM3.Name = "cb_SRTM3";
            this.cb_SRTM3.Size = new System.Drawing.Size(294, 25);
            this.cb_SRTM3.TabIndex = 2;
            this.cb_SRTM3.Text = "Use SRTM3 Elevation Model (optional)";
            this.cb_SRTM3.UseVisualStyleBackColor = true;
            this.cb_SRTM3.CheckedChanged += new System.EventHandler(this.cb_SRTM3_CheckedChanged);
            // 
            // cb_GLOBE
            // 
            this.cb_GLOBE.AutoSize = true;
            this.cb_GLOBE.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_GLOBE.Location = new System.Drawing.Point(85, 252);
            this.cb_GLOBE.Name = "cb_GLOBE";
            this.cb_GLOBE.Size = new System.Drawing.Size(397, 25);
            this.cb_GLOBE.TabIndex = 1;
            this.cb_GLOBE.Text = "Use GLOBE Elevation Model (strongly recommended)";
            this.cb_GLOBE.UseVisualStyleBackColor = true;
            this.cb_GLOBE.CheckedChanged += new System.EventHandler(this.cb_GLOBE_CheckedChanged);
            // 
            // wp_GLOBE
            // 
            this.wp_GLOBE.Controls.Add(this.lbl_GLOBE_Status);
            this.wp_GLOBE.Controls.Add(this.label5);
            this.wp_GLOBE.Controls.Add(this.cb_GLOBE_EnableCache);
            this.wp_GLOBE.Controls.Add(this.gm_GLOBE);
            this.wp_GLOBE.Name = "wp_GLOBE";
            this.wp_GLOBE.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_GLOBE, "GLOBE");
            this.wp_GLOBE.TabIndex = 6;
            this.wp_GLOBE.Text = "GLOBE";
            this.wp_GLOBE.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wp_GLOBE_Commit);
            this.wp_GLOBE.Enter += new System.EventHandler(this.wp_GLOBE_Enter);
            this.wp_GLOBE.Leave += new System.EventHandler(this.wp_GLOBE_Leave);
            // 
            // lbl_GLOBE_Status
            // 
            this.lbl_GLOBE_Status.AutoSize = true;
            this.lbl_GLOBE_Status.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_GLOBE_Status.Location = new System.Drawing.Point(4, 58);
            this.lbl_GLOBE_Status.Name = "lbl_GLOBE_Status";
            this.lbl_GLOBE_Status.Size = new System.Drawing.Size(40, 15);
            this.lbl_GLOBE_Status.TabIndex = 2;
            this.lbl_GLOBE_Status.Text = "Status";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SteelBlue;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(371, 21);
            this.label5.TabIndex = 2;
            this.label5.Text = "(G)lobal (L)and (O)ne-km (B)ase (E)levation Project";
            // 
            // cb_GLOBE_EnableCache
            // 
            this.cb_GLOBE_EnableCache.AutoSize = true;
            this.cb_GLOBE_EnableCache.Checked = global::AirScout.Properties.Settings.Default.Elevation_GLOBE_EnableCache;
            this.cb_GLOBE_EnableCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_GLOBE_EnableCache.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Elevation_GLOBE_EnableCache", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_GLOBE_EnableCache.Location = new System.Drawing.Point(7, 36);
            this.cb_GLOBE_EnableCache.Name = "cb_GLOBE_EnableCache";
            this.cb_GLOBE_EnableCache.Size = new System.Drawing.Size(243, 19);
            this.cb_GLOBE_EnableCache.TabIndex = 1;
            this.cb_GLOBE_EnableCache.Text = "Keep downloaded elevation tiles in cache";
            this.cb_GLOBE_EnableCache.UseVisualStyleBackColor = true;
            // 
            // gm_GLOBE
            // 
            this.gm_GLOBE.Bearing = 0F;
            this.gm_GLOBE.CanDragMap = true;
            this.gm_GLOBE.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_GLOBE.GrayScaleMode = false;
            this.gm_GLOBE.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_GLOBE.LevelsKeepInMemmory = 5;
            this.gm_GLOBE.Location = new System.Drawing.Point(0, 84);
            this.gm_GLOBE.MarkersEnabled = true;
            this.gm_GLOBE.MaxZoom = 2;
            this.gm_GLOBE.MinZoom = 2;
            this.gm_GLOBE.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_GLOBE.Name = "gm_GLOBE";
            this.gm_GLOBE.NegativeMode = false;
            this.gm_GLOBE.Opacity = 1D;
            this.gm_GLOBE.PolygonsEnabled = true;
            this.gm_GLOBE.RetryLoadTile = 0;
            this.gm_GLOBE.RoutesEnabled = true;
            this.gm_GLOBE.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_GLOBE.ShowTileGridLines = false;
            this.gm_GLOBE.Size = new System.Drawing.Size(480, 311);
            this.gm_GLOBE.TabIndex = 0;
            this.gm_GLOBE.Zoom = 0D;
            // 
            // wp_SRTM3
            // 
            this.wp_SRTM3.Controls.Add(this.lbl_SRTM3_Status);
            this.wp_SRTM3.Controls.Add(this.label21);
            this.wp_SRTM3.Controls.Add(this.cb_SRTM3_EnableCache);
            this.wp_SRTM3.Controls.Add(this.gm_SRTM3);
            this.wp_SRTM3.Name = "wp_SRTM3";
            this.wp_SRTM3.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_SRTM3, "SRTM3");
            this.wp_SRTM3.TabIndex = 8;
            this.wp_SRTM3.Text = "SRTM3";
            this.wp_SRTM3.Enter += new System.EventHandler(this.wp_SRTM3_Enter);
            this.wp_SRTM3.Leave += new System.EventHandler(this.wp_SRTM3_Leave);
            // 
            // lbl_SRTM3_Status
            // 
            this.lbl_SRTM3_Status.AutoSize = true;
            this.lbl_SRTM3_Status.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SRTM3_Status.Location = new System.Drawing.Point(4, 65);
            this.lbl_SRTM3_Status.Name = "lbl_SRTM3_Status";
            this.lbl_SRTM3_Status.Size = new System.Drawing.Size(40, 15);
            this.lbl_SRTM3_Status.TabIndex = 5;
            this.lbl_SRTM3_Status.Text = "Status";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.SteelBlue;
            this.label21.Location = new System.Drawing.Point(3, 13);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(358, 21);
            this.label21.TabIndex = 4;
            this.label21.Text = "(S)huttle (R)adar (T)opography (M)ission 3 arcsec";
            // 
            // cb_SRTM3_EnableCache
            // 
            this.cb_SRTM3_EnableCache.AutoSize = true;
            this.cb_SRTM3_EnableCache.Checked = global::AirScout.Properties.Settings.Default.Elevation_SRTM3_EnableCache;
            this.cb_SRTM3_EnableCache.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Elevation_SRTM3_EnableCache", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_SRTM3_EnableCache.Location = new System.Drawing.Point(7, 43);
            this.cb_SRTM3_EnableCache.Name = "cb_SRTM3_EnableCache";
            this.cb_SRTM3_EnableCache.Size = new System.Drawing.Size(243, 19);
            this.cb_SRTM3_EnableCache.TabIndex = 3;
            this.cb_SRTM3_EnableCache.Text = "Keep downloaded elevation tiles in cache";
            this.cb_SRTM3_EnableCache.UseVisualStyleBackColor = true;
            // 
            // gm_SRTM3
            // 
            this.gm_SRTM3.Bearing = 0F;
            this.gm_SRTM3.CanDragMap = true;
            this.gm_SRTM3.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_SRTM3.GrayScaleMode = false;
            this.gm_SRTM3.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_SRTM3.LevelsKeepInMemmory = 5;
            this.gm_SRTM3.Location = new System.Drawing.Point(3, 84);
            this.gm_SRTM3.MarkersEnabled = true;
            this.gm_SRTM3.MaxZoom = 2;
            this.gm_SRTM3.MinZoom = 2;
            this.gm_SRTM3.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_SRTM3.Name = "gm_SRTM3";
            this.gm_SRTM3.NegativeMode = false;
            this.gm_SRTM3.Opacity = 1D;
            this.gm_SRTM3.PolygonsEnabled = true;
            this.gm_SRTM3.RetryLoadTile = 0;
            this.gm_SRTM3.RoutesEnabled = true;
            this.gm_SRTM3.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_SRTM3.ShowTileGridLines = false;
            this.gm_SRTM3.Size = new System.Drawing.Size(480, 311);
            this.gm_SRTM3.TabIndex = 0;
            this.gm_SRTM3.Zoom = 0D;
            // 
            // wp_SRTM1
            // 
            this.wp_SRTM1.Controls.Add(this.lbl_SRTM1_Status);
            this.wp_SRTM1.Controls.Add(this.label7);
            this.wp_SRTM1.Controls.Add(this.cb_SRTM1_EnableCache);
            this.wp_SRTM1.Controls.Add(this.gm_SRTM1);
            this.wp_SRTM1.Name = "wp_SRTM1";
            this.wp_SRTM1.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_SRTM1, "SRTM1");
            this.wp_SRTM1.TabIndex = 9;
            this.wp_SRTM1.Text = "SRTM1";
            this.wp_SRTM1.Enter += new System.EventHandler(this.wp_SRTM1_Enter);
            this.wp_SRTM1.Leave += new System.EventHandler(this.wp_SRTM1_Leave);
            // 
            // lbl_SRTM1_Status
            // 
            this.lbl_SRTM1_Status.AutoSize = true;
            this.lbl_SRTM1_Status.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SRTM1_Status.Location = new System.Drawing.Point(4, 60);
            this.lbl_SRTM1_Status.Name = "lbl_SRTM1_Status";
            this.lbl_SRTM1_Status.Size = new System.Drawing.Size(40, 15);
            this.lbl_SRTM1_Status.TabIndex = 12;
            this.lbl_SRTM1_Status.Text = "Status";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SteelBlue;
            this.label7.Location = new System.Drawing.Point(3, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(357, 21);
            this.label7.TabIndex = 11;
            this.label7.Text = "(S)huttle (R)adar (T)opography (M)ission 1 arcsec";
            // 
            // cb_SRTM1_EnableCache
            // 
            this.cb_SRTM1_EnableCache.AutoSize = true;
            this.cb_SRTM1_EnableCache.Checked = global::AirScout.Properties.Settings.Default.Elevation_SRTM3_EnableCache;
            this.cb_SRTM1_EnableCache.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Elevation_SRTM3_EnableCache", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_SRTM1_EnableCache.Location = new System.Drawing.Point(7, 38);
            this.cb_SRTM1_EnableCache.Name = "cb_SRTM1_EnableCache";
            this.cb_SRTM1_EnableCache.Size = new System.Drawing.Size(243, 19);
            this.cb_SRTM1_EnableCache.TabIndex = 10;
            this.cb_SRTM1_EnableCache.Text = "Keep downloaded elevation tiles in cache";
            this.cb_SRTM1_EnableCache.UseVisualStyleBackColor = true;
            // 
            // gm_SRTM1
            // 
            this.gm_SRTM1.Bearing = 0F;
            this.gm_SRTM1.CanDragMap = true;
            this.gm_SRTM1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_SRTM1.GrayScaleMode = false;
            this.gm_SRTM1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_SRTM1.LevelsKeepInMemmory = 5;
            this.gm_SRTM1.Location = new System.Drawing.Point(3, 84);
            this.gm_SRTM1.MarkersEnabled = true;
            this.gm_SRTM1.MaxZoom = 2;
            this.gm_SRTM1.MinZoom = 2;
            this.gm_SRTM1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_SRTM1.Name = "gm_SRTM1";
            this.gm_SRTM1.NegativeMode = false;
            this.gm_SRTM1.Opacity = 1D;
            this.gm_SRTM1.PolygonsEnabled = true;
            this.gm_SRTM1.RetryLoadTile = 0;
            this.gm_SRTM1.RoutesEnabled = true;
            this.gm_SRTM1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_SRTM1.ShowTileGridLines = false;
            this.gm_SRTM1.Size = new System.Drawing.Size(480, 314);
            this.gm_SRTM1.TabIndex = 9;
            this.gm_SRTM1.Zoom = 0D;
            // 
            // wp_ASTER3
            // 
            this.wp_ASTER3.Controls.Add(this.lbl_ASTER3_Status);
            this.wp_ASTER3.Controls.Add(this.label23);
            this.wp_ASTER3.Controls.Add(this.cb_ASTER3_EnableCache);
            this.wp_ASTER3.Controls.Add(this.gm_ASTER3);
            this.wp_ASTER3.Name = "wp_ASTER3";
            this.wp_ASTER3.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_ASTER3, "ASTER3");
            this.wp_ASTER3.TabIndex = 12;
            this.wp_ASTER3.Text = "ASTER3";
            this.wp_ASTER3.Enter += new System.EventHandler(this.wp_ASTER3_Enter);
            this.wp_ASTER3.Leave += new System.EventHandler(this.wp_ASTER3_Leave);
            // 
            // lbl_ASTER3_Status
            // 
            this.lbl_ASTER3_Status.AutoSize = true;
            this.lbl_ASTER3_Status.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ASTER3_Status.Location = new System.Drawing.Point(54, 60);
            this.lbl_ASTER3_Status.Name = "lbl_ASTER3_Status";
            this.lbl_ASTER3_Status.Size = new System.Drawing.Size(40, 15);
            this.lbl_ASTER3_Status.TabIndex = 9;
            this.lbl_ASTER3_Status.Text = "Status";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label23.ForeColor = System.Drawing.Color.SteelBlue;
            this.label23.Location = new System.Drawing.Point(53, 8);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(459, 19);
            this.label23.TabIndex = 8;
            this.label23.Text = "(A)dvanced (S)paceborne (T)hermal (E)mission and (R)eflection 3 arcsec";
            // 
            // cb_ASTER3_EnableCache
            // 
            this.cb_ASTER3_EnableCache.AutoSize = true;
            this.cb_ASTER3_EnableCache.Checked = global::AirScout.Properties.Settings.Default.Elevation_SRTM3_EnableCache;
            this.cb_ASTER3_EnableCache.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Elevation_SRTM3_EnableCache", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_ASTER3_EnableCache.Location = new System.Drawing.Point(57, 38);
            this.cb_ASTER3_EnableCache.Name = "cb_ASTER3_EnableCache";
            this.cb_ASTER3_EnableCache.Size = new System.Drawing.Size(243, 19);
            this.cb_ASTER3_EnableCache.TabIndex = 7;
            this.cb_ASTER3_EnableCache.Text = "Keep downloaded elevation tiles in cache";
            this.cb_ASTER3_EnableCache.UseVisualStyleBackColor = true;
            // 
            // gm_ASTER3
            // 
            this.gm_ASTER3.Bearing = 0F;
            this.gm_ASTER3.CanDragMap = true;
            this.gm_ASTER3.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_ASTER3.GrayScaleMode = false;
            this.gm_ASTER3.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_ASTER3.LevelsKeepInMemmory = 5;
            this.gm_ASTER3.Location = new System.Drawing.Point(53, 79);
            this.gm_ASTER3.MarkersEnabled = true;
            this.gm_ASTER3.MaxZoom = 2;
            this.gm_ASTER3.MinZoom = 2;
            this.gm_ASTER3.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_ASTER3.Name = "gm_ASTER3";
            this.gm_ASTER3.NegativeMode = false;
            this.gm_ASTER3.Opacity = 1D;
            this.gm_ASTER3.PolygonsEnabled = true;
            this.gm_ASTER3.RetryLoadTile = 0;
            this.gm_ASTER3.RoutesEnabled = true;
            this.gm_ASTER3.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_ASTER3.ShowTileGridLines = false;
            this.gm_ASTER3.Size = new System.Drawing.Size(480, 311);
            this.gm_ASTER3.TabIndex = 6;
            this.gm_ASTER3.Zoom = 0D;
            // 
            // wp_ASTER1
            // 
            this.wp_ASTER1.Controls.Add(this.lbl_ASTER1_Status);
            this.wp_ASTER1.Controls.Add(this.label25);
            this.wp_ASTER1.Controls.Add(this.cb_ASTER1_EnableCache);
            this.wp_ASTER1.Controls.Add(this.gm_ASTER1);
            this.wp_ASTER1.Name = "wp_ASTER1";
            this.wp_ASTER1.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_ASTER1, "ASTER1");
            this.wp_ASTER1.TabIndex = 13;
            this.wp_ASTER1.Text = "ASTER1";
            this.wp_ASTER1.Enter += new System.EventHandler(this.wp_ASTER1_Enter);
            this.wp_ASTER1.Leave += new System.EventHandler(this.wp_ASTER1_Leave);
            // 
            // lbl_ASTER1_Status
            // 
            this.lbl_ASTER1_Status.AutoSize = true;
            this.lbl_ASTER1_Status.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ASTER1_Status.Location = new System.Drawing.Point(54, 60);
            this.lbl_ASTER1_Status.Name = "lbl_ASTER1_Status";
            this.lbl_ASTER1_Status.Size = new System.Drawing.Size(40, 15);
            this.lbl_ASTER1_Status.TabIndex = 9;
            this.lbl_ASTER1_Status.Text = "Status";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label25.ForeColor = System.Drawing.Color.SteelBlue;
            this.label25.Location = new System.Drawing.Point(53, 8);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(458, 19);
            this.label25.TabIndex = 8;
            this.label25.Text = "(A)dvanced (S)paceborne (T)hermal (E)mission and (R)eflection 1 arcsec";
            // 
            // cb_ASTER1_EnableCache
            // 
            this.cb_ASTER1_EnableCache.AutoSize = true;
            this.cb_ASTER1_EnableCache.Checked = global::AirScout.Properties.Settings.Default.Elevation_SRTM3_EnableCache;
            this.cb_ASTER1_EnableCache.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Elevation_SRTM3_EnableCache", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_ASTER1_EnableCache.Location = new System.Drawing.Point(57, 38);
            this.cb_ASTER1_EnableCache.Name = "cb_ASTER1_EnableCache";
            this.cb_ASTER1_EnableCache.Size = new System.Drawing.Size(243, 19);
            this.cb_ASTER1_EnableCache.TabIndex = 7;
            this.cb_ASTER1_EnableCache.Text = "Keep downloaded elevation tiles in cache";
            this.cb_ASTER1_EnableCache.UseVisualStyleBackColor = true;
            // 
            // gm_ASTER1
            // 
            this.gm_ASTER1.Bearing = 0F;
            this.gm_ASTER1.CanDragMap = true;
            this.gm_ASTER1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_ASTER1.GrayScaleMode = false;
            this.gm_ASTER1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_ASTER1.LevelsKeepInMemmory = 5;
            this.gm_ASTER1.Location = new System.Drawing.Point(53, 79);
            this.gm_ASTER1.MarkersEnabled = true;
            this.gm_ASTER1.MaxZoom = 2;
            this.gm_ASTER1.MinZoom = 2;
            this.gm_ASTER1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_ASTER1.Name = "gm_ASTER1";
            this.gm_ASTER1.NegativeMode = false;
            this.gm_ASTER1.Opacity = 1D;
            this.gm_ASTER1.PolygonsEnabled = true;
            this.gm_ASTER1.RetryLoadTile = 0;
            this.gm_ASTER1.RoutesEnabled = true;
            this.gm_ASTER1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_ASTER1.ShowTileGridLines = false;
            this.gm_ASTER1.Size = new System.Drawing.Size(480, 311);
            this.gm_ASTER1.TabIndex = 6;
            this.gm_ASTER1.Zoom = 0D;
            // 
            // wp_UserDetails
            // 
            this.wp_UserDetails.Controls.Add(this.label19);
            this.wp_UserDetails.Controls.Add(this.label18);
            this.wp_UserDetails.Controls.Add(this.tb_Zoom);
            this.wp_UserDetails.Controls.Add(this.btn_Zoom_Out);
            this.wp_UserDetails.Controls.Add(this.btn_Zoom_In);
            this.wp_UserDetails.Controls.Add(this.btn_QRZ);
            this.wp_UserDetails.Controls.Add(this.label12);
            this.wp_UserDetails.Controls.Add(this.label11);
            this.wp_UserDetails.Controls.Add(this.label10);
            this.wp_UserDetails.Controls.Add(this.label9);
            this.wp_UserDetails.Controls.Add(this.label8);
            this.wp_UserDetails.Controls.Add(this.gm_Callsign);
            this.wp_UserDetails.Controls.Add(this.tb_Callsign);
            this.wp_UserDetails.Controls.Add(this.tb_Locator);
            this.wp_UserDetails.Controls.Add(this.tb_Longitude);
            this.wp_UserDetails.Controls.Add(this.tb_Latitude);
            this.wp_UserDetails.Name = "wp_UserDetails";
            this.wp_UserDetails.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_UserDetails, "User Details");
            this.wp_UserDetails.TabIndex = 5;
            this.wp_UserDetails.Text = "User Details";
            this.wp_UserDetails.Enter += new System.EventHandler(this.wp_UserDetails_Enter);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(9, 6);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(531, 60);
            this.label19.TabIndex = 28;
            this.label19.Text = resources.GetString("label19.Text");
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(342, 365);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 15);
            this.label18.TabIndex = 27;
            this.label18.Text = "Map Zoom:";
            // 
            // tb_Zoom
            // 
            this.tb_Zoom.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_Zoom.Location = new System.Drawing.Point(495, 361);
            this.tb_Zoom.Name = "tb_Zoom";
            this.tb_Zoom.Size = new System.Drawing.Size(30, 23);
            this.tb_Zoom.TabIndex = 26;
            // 
            // btn_Zoom_Out
            // 
            this.btn_Zoom_Out.Location = new System.Drawing.Point(533, 360);
            this.btn_Zoom_Out.Name = "btn_Zoom_Out";
            this.btn_Zoom_Out.Size = new System.Drawing.Size(30, 25);
            this.btn_Zoom_Out.TabIndex = 25;
            this.btn_Zoom_Out.Text = "-";
            this.btn_Zoom_Out.UseVisualStyleBackColor = true;
            this.btn_Zoom_Out.Click += new System.EventHandler(this.btn_Zoom_Out_Click);
            // 
            // btn_Zoom_In
            // 
            this.btn_Zoom_In.Location = new System.Drawing.Point(456, 360);
            this.btn_Zoom_In.Name = "btn_Zoom_In";
            this.btn_Zoom_In.Size = new System.Drawing.Size(30, 25);
            this.btn_Zoom_In.TabIndex = 24;
            this.btn_Zoom_In.Text = "+";
            this.btn_Zoom_In.UseVisualStyleBackColor = true;
            this.btn_Zoom_In.Click += new System.EventHandler(this.btn_Zoom_In_Click);
            // 
            // btn_QRZ
            // 
            this.btn_QRZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_QRZ.Location = new System.Drawing.Point(458, 216);
            this.btn_QRZ.Name = "btn_QRZ";
            this.btn_QRZ.Size = new System.Drawing.Size(107, 26);
            this.btn_QRZ.TabIndex = 17;
            this.btn_QRZ.Text = "QRZ lookup";
            this.btn_QRZ.UseVisualStyleBackColor = true;
            this.btn_QRZ.Click += new System.EventHandler(this.btn_QRZ_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 15);
            this.label12.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(344, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 15);
            this.label11.TabIndex = 8;
            this.label11.Text = "Locator:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(344, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 15);
            this.label10.TabIndex = 6;
            this.label10.Text = "Longitude:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(344, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "Latitude:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(344, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Callsign:";
            // 
            // gm_Callsign
            // 
            this.gm_Callsign.Bearing = 0F;
            this.gm_Callsign.CanDragMap = true;
            this.gm_Callsign.EmptyTileColor = System.Drawing.Color.Navy;
            this.gm_Callsign.GrayScaleMode = false;
            this.gm_Callsign.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gm_Callsign.LevelsKeepInMemmory = 5;
            this.gm_Callsign.Location = new System.Drawing.Point(0, 84);
            this.gm_Callsign.MarkersEnabled = true;
            this.gm_Callsign.MaxZoom = 2;
            this.gm_Callsign.MinZoom = 2;
            this.gm_Callsign.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Callsign.Name = "gm_Callsign";
            this.gm_Callsign.NegativeMode = false;
            this.gm_Callsign.Opacity = 1D;
            this.gm_Callsign.PolygonsEnabled = true;
            this.gm_Callsign.RetryLoadTile = 0;
            this.gm_Callsign.RoutesEnabled = true;
            this.gm_Callsign.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Callsign.ShowTileGridLines = false;
            this.gm_Callsign.Size = new System.Drawing.Size(320, 311);
            this.gm_Callsign.TabIndex = 0;
            this.gm_Callsign.Zoom = 0D;
            this.gm_Callsign.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.gm_Callsign_OnMarkerEnter);
            this.gm_Callsign.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gm_Callsign_OnMapZoomChanged);
            this.gm_Callsign.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gm_Callsign_MouseDown);
            this.gm_Callsign.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gm_Callsign_MouseMove);
            this.gm_Callsign.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gm_Callsign_MouseUp);
            // 
            // tb_Callsign
            // 
            this.tb_Callsign.BackColor = System.Drawing.SystemColors.Window;
            this.tb_Callsign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Callsign.ErrorBackColor = System.Drawing.Color.Red;
            this.tb_Callsign.ErrorForeColor = System.Drawing.Color.White;
            this.tb_Callsign.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Callsign.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tb_Callsign.Location = new System.Drawing.Point(458, 90);
            this.tb_Callsign.Name = "tb_Callsign";
            this.tb_Callsign.Size = new System.Drawing.Size(107, 21);
            this.tb_Callsign.TabIndex = 1;
            this.tb_Callsign.TextChanged += new System.EventHandler(this.tb_Callsign_TextChanged);
            // 
            // tb_Locator
            // 
            this.tb_Locator.DataBindings.Add(new System.Windows.Forms.Binding("SmallLettersForSubsquares", global::AirScout.Properties.Settings.Default, "Locator_SmallLettersForSubsquares", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Locator.ErrorBackColor = System.Drawing.Color.Red;
            this.tb_Locator.ErrorForeColor = System.Drawing.Color.White;
            this.tb_Locator.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Locator.Location = new System.Drawing.Point(458, 117);
            this.tb_Locator.Name = "tb_Locator";
            this.tb_Locator.Size = new System.Drawing.Size(107, 21);
            this.tb_Locator.SmallLettersForSubsquares = global::AirScout.Properties.Settings.Default.Locator_SmallLettersForSubsquares;
            this.tb_Locator.TabIndex = 4;
            this.tb_Locator.TextChanged += new System.EventHandler(this.tb_Locator_TextChanged);
            // 
            // tb_Longitude
            // 
            this.tb_Longitude.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Longitude.FormatSpecifier = "F8";
            this.tb_Longitude.Location = new System.Drawing.Point(458, 174);
            this.tb_Longitude.MaxValue = 180D;
            this.tb_Longitude.MinValue = -180D;
            this.tb_Longitude.Name = "tb_Longitude";
            this.tb_Longitude.Size = new System.Drawing.Size(107, 21);
            this.tb_Longitude.TabIndex = 3;
            this.tb_Longitude.Text = "10.68327000";
            this.tb_Longitude.Value = 10.68327D;
            this.tb_Longitude.TextChanged += new System.EventHandler(this.tb_Longitude_TextChanged);
            // 
            // tb_Latitude
            // 
            this.tb_Latitude.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Latitude.FormatSpecifier = "F8";
            this.tb_Latitude.Location = new System.Drawing.Point(458, 144);
            this.tb_Latitude.MaxValue = 90D;
            this.tb_Latitude.MinValue = -90D;
            this.tb_Latitude.Name = "tb_Latitude";
            this.tb_Latitude.Size = new System.Drawing.Size(107, 21);
            this.tb_Latitude.TabIndex = 2;
            this.tb_Latitude.Text = "50.93706700";
            this.tb_Latitude.Value = 50.937067D;
            this.tb_Latitude.TextChanged += new System.EventHandler(this.tb_Latitude_TextChanged);
            // 
            // wp_PlaneFeeds
            // 
            this.wp_PlaneFeeds.Controls.Add(this.label20);
            this.wp_PlaneFeeds.Controls.Add(this.label16);
            this.wp_PlaneFeeds.Controls.Add(this.label15);
            this.wp_PlaneFeeds.Controls.Add(this.label14);
            this.wp_PlaneFeeds.Controls.Add(this.cb_PlaneFeed3);
            this.wp_PlaneFeeds.Controls.Add(this.cb_PlaneFeed2);
            this.wp_PlaneFeeds.Controls.Add(this.cb_PlaneFeed1);
            this.wp_PlaneFeeds.Controls.Add(this.label13);
            this.wp_PlaneFeeds.Name = "wp_PlaneFeeds";
            this.wp_PlaneFeeds.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_PlaneFeeds, "Select a Plane Feed");
            this.wp_PlaneFeeds.TabIndex = 10;
            this.wp_PlaneFeeds.Text = "Select a Plane Feed";
            this.wp_PlaneFeeds.Enter += new System.EventHandler(this.wp_PlaneFeeds_Enter);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(29, 34);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(521, 85);
            this.label20.TabIndex = 8;
            this.label20.Text = resources.GetString("label20.Text");
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(12, 242);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(101, 21);
            this.label16.TabIndex = 7;
            this.label16.Text = "Plane Feed 3:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(12, 213);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 21);
            this.label15.TabIndex = 6;
            this.label15.Text = "Plane Feed 2:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(12, 184);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(101, 21);
            this.label14.TabIndex = 1;
            this.label14.Text = "Plane Feed 1:";
            // 
            // cb_PlaneFeed3
            // 
            this.cb_PlaneFeed3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PlaneFeed3.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_PlaneFeed3.FormattingEnabled = true;
            this.cb_PlaneFeed3.Location = new System.Drawing.Point(119, 242);
            this.cb_PlaneFeed3.Name = "cb_PlaneFeed3";
            this.cb_PlaneFeed3.Size = new System.Drawing.Size(431, 26);
            this.cb_PlaneFeed3.TabIndex = 5;
            this.cb_PlaneFeed3.SelectedIndexChanged += new System.EventHandler(this.cb_PlaneFeed3_SelectedIndexChanged);
            // 
            // cb_PlaneFeed2
            // 
            this.cb_PlaneFeed2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PlaneFeed2.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_PlaneFeed2.FormattingEnabled = true;
            this.cb_PlaneFeed2.Location = new System.Drawing.Point(119, 213);
            this.cb_PlaneFeed2.Name = "cb_PlaneFeed2";
            this.cb_PlaneFeed2.Size = new System.Drawing.Size(431, 26);
            this.cb_PlaneFeed2.TabIndex = 4;
            this.cb_PlaneFeed2.SelectedIndexChanged += new System.EventHandler(this.cb_PlaneFeed2_SelectedIndexChanged);
            // 
            // cb_PlaneFeed1
            // 
            this.cb_PlaneFeed1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PlaneFeed1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_PlaneFeed1.FormattingEnabled = true;
            this.cb_PlaneFeed1.Location = new System.Drawing.Point(119, 184);
            this.cb_PlaneFeed1.Name = "cb_PlaneFeed1";
            this.cb_PlaneFeed1.Size = new System.Drawing.Size(431, 26);
            this.cb_PlaneFeed1.TabIndex = 3;
            this.cb_PlaneFeed1.SelectedIndexChanged += new System.EventHandler(this.cb_PlaneFeed1_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 14);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 15);
            this.label13.TabIndex = 2;
            // 
            // wp_Finish
            // 
            this.wp_Finish.Controls.Add(this.lbl_Finish);
            this.wp_Finish.IsFinishPage = true;
            this.wp_Finish.Name = "wp_Finish";
            this.wp_Finish.Size = new System.Drawing.Size(586, 398);
            this.sw_FirstRun.SetStepText(this.wp_Finish, "Finish");
            this.wp_Finish.TabIndex = 11;
            this.wp_Finish.Text = "Finish";
            this.wp_Finish.Enter += new System.EventHandler(this.wp_Finish_Enter);
            // 
            // lbl_Finish
            // 
            this.lbl_Finish.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Finish.Location = new System.Drawing.Point(55, 67);
            this.lbl_Finish.Name = "lbl_Finish";
            this.lbl_Finish.Size = new System.Drawing.Size(486, 271);
            this.lbl_Finish.TabIndex = 1;
            this.lbl_Finish.Text = "Finish statement.";
            this.lbl_Finish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bw_GLOBE_MapUpdater
            // 
            this.bw_GLOBE_MapUpdater.WorkerReportsProgress = true;
            this.bw_GLOBE_MapUpdater.WorkerSupportsCancellation = true;
            this.bw_GLOBE_MapUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_GLOBE_MapUpdater_DoWork);
            this.bw_GLOBE_MapUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_GLOBE_MapUpdater_ProgressChanged);
            this.bw_GLOBE_MapUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_GLOBE_MapUpdater_RunWorkerCompleted);
            // 
            // bw_SRTM3_MapUpdater
            // 
            this.bw_SRTM3_MapUpdater.WorkerReportsProgress = true;
            this.bw_SRTM3_MapUpdater.WorkerSupportsCancellation = true;
            this.bw_SRTM3_MapUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_SRTM3_MapUpdater_DoWork);
            this.bw_SRTM3_MapUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_SRTM3_MapUpdater_ProgressChanged);
            this.bw_SRTM3_MapUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_SRTM3_MapUpdater_RunWorkerCompleted);
            // 
            // bw_SRTM1_MapUpdater
            // 
            this.bw_SRTM1_MapUpdater.WorkerReportsProgress = true;
            this.bw_SRTM1_MapUpdater.WorkerSupportsCancellation = true;
            this.bw_SRTM1_MapUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_SRTM1_MapUpdater_DoWork);
            this.bw_SRTM1_MapUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_SRTM1_MapUpdater_ProgressChanged);
            this.bw_SRTM1_MapUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_SRTM1_MapUpdater_RunWorkerCompleted);
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 539);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(834, 22);
            this.ss_Main.TabIndex = 1;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(42, 17);
            this.tsl_Status.Text = "Status.";
            // 
            // bw_ASTER3_MapUpdater
            // 
            this.bw_ASTER3_MapUpdater.WorkerReportsProgress = true;
            this.bw_ASTER3_MapUpdater.WorkerSupportsCancellation = true;
            this.bw_ASTER3_MapUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_ASTER3_MapUpdater_DoWork);
            this.bw_ASTER3_MapUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_ASTER3_MapUpdater_ProgressChanged);
            this.bw_ASTER3_MapUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_ASTER3_MapUpdater_RunWorkerCompleted);
            // 
            // bw_ASTER1_MapUpdater
            // 
            this.bw_ASTER1_MapUpdater.WorkerReportsProgress = true;
            this.bw_ASTER1_MapUpdater.WorkerSupportsCancellation = true;
            this.bw_ASTER1_MapUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_ASTER1_MapUpdater_DoWork);
            this.bw_ASTER1_MapUpdater.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_ASTER1_MapUpdater_ProgressChanged);
            this.bw_ASTER1_MapUpdater.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_ASTER1_MapUpdater_RunWorkerCompleted);
            // 
            // FirstRunWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.sw_FirstRun);
            this.Controls.Add(this.ss_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FirstRunWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FirstRunWizard_FormClosing);
            this.Load += new System.EventHandler(this.FirstRunWizard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sw_FirstRun)).EndInit();
            this.wp_TermsAndConditions.ResumeLayout(false);
            this.wp_TermsAndConditions.PerformLayout();
            this.wp_GeneralCoverage.ResumeLayout(false);
            this.wp_GeneralCoverage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLon)).EndInit();
            this.wp_ElevationModel.ResumeLayout(false);
            this.wp_ElevationModel.PerformLayout();
            this.wp_GLOBE.ResumeLayout(false);
            this.wp_GLOBE.PerformLayout();
            this.wp_SRTM3.ResumeLayout(false);
            this.wp_SRTM3.PerformLayout();
            this.wp_SRTM1.ResumeLayout(false);
            this.wp_SRTM1.PerformLayout();
            this.wp_ASTER3.ResumeLayout(false);
            this.wp_ASTER3.PerformLayout();
            this.wp_ASTER1.ResumeLayout(false);
            this.wp_ASTER1.PerformLayout();
            this.wp_UserDetails.ResumeLayout(false);
            this.wp_UserDetails.PerformLayout();
            this.wp_PlaneFeeds.ResumeLayout(false);
            this.wp_PlaneFeeds.PerformLayout();
            this.wp_Finish.ResumeLayout(false);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AeroWizard.StepWizardControl sw_FirstRun;
        private AeroWizard.WizardPage wp_TermsAndConditions;
        private AeroWizard.WizardPage wp_ElevationModel;
        private AeroWizard.WizardPage wp_SRTM3;
        private AeroWizard.WizardPage wp_SRTM1;
        private System.Windows.Forms.CheckBox cb_TermsAndConditions;
        private System.Windows.Forms.RichTextBox rb_TermsAndConditions;
        private AeroWizard.WizardPage wp_GeneralCoverage;
        private AeroWizard.WizardPage wp_UserDetails;
        private GMap.NET.WindowsForms.GMapControl gm_Coverage;
        private System.Windows.Forms.NumericUpDown ud_MinLon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ud_MaxLon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ud_MaxLat;
        private System.Windows.Forms.NumericUpDown ud_MinLat;
        private AeroWizard.WizardPage wp_GLOBE;
        private GMap.NET.WindowsForms.GMapControl gm_GLOBE;
        private System.Windows.Forms.CheckBox cb_GLOBE;
        private System.Windows.Forms.CheckBox cb_SRTM1;
        private System.Windows.Forms.CheckBox cb_SRTM3;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.ComponentModel.BackgroundWorker bw_GLOBE_MapUpdater;
        private AeroWizard.WizardPage wp_PlaneFeeds;
        private AeroWizard.WizardPage wp_Finish;
        private System.ComponentModel.BackgroundWorker bw_SRTM3_MapUpdater;
        private System.ComponentModel.BackgroundWorker bw_SRTM1_MapUpdater;
        private GMap.NET.WindowsForms.GMapControl gm_SRTM3;
        private GMap.NET.WindowsForms.GMapControl gm_SRTM1;
        private GMap.NET.WindowsForms.GMapControl gm_Callsign;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private ScoutBase.Core.DoubleTextBox tb_Longitude;
        private ScoutBase.Core.DoubleTextBox tb_Latitude;
        private ScoutBase.Core.LocatorTextBox tb_Locator;
        private ScoutBase.Core.CallsignTextBox tb_Callsign;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cb_PlaneFeed1;
        private System.Windows.Forms.ComboBox cb_PlaneFeed3;
        private System.Windows.Forms.ComboBox cb_PlaneFeed2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lbl_Finish;
        private System.Windows.Forms.Button btn_QRZ;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb_Zoom;
        private System.Windows.Forms.Button btn_Zoom_Out;
        private System.Windows.Forms.Button btn_Zoom_In;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox cb_GLOBE_EnableCache;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_GLOBE_Status;
        private System.Windows.Forms.Label lbl_SRTM3_Status;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox cb_SRTM3_EnableCache;
        private System.Windows.Forms.Label lbl_SRTM1_Status;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cb_SRTM1_EnableCache;
        private System.Windows.Forms.Label label6;
        private AeroWizard.WizardPage wp_ASTER3;
        private System.Windows.Forms.Label lbl_ASTER3_Status;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.CheckBox cb_ASTER3_EnableCache;
        private GMap.NET.WindowsForms.GMapControl gm_ASTER3;
        private AeroWizard.WizardPage wp_ASTER1;
        private System.Windows.Forms.Label lbl_ASTER1_Status;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox cb_ASTER1_EnableCache;
        private GMap.NET.WindowsForms.GMapControl gm_ASTER1;
        private System.ComponentModel.BackgroundWorker bw_ASTER3_MapUpdater;
        private System.ComponentModel.BackgroundWorker bw_ASTER1_MapUpdater;
        private System.Windows.Forms.CheckBox cb_ASTER1;
        private System.Windows.Forms.CheckBox cb_ASTER3;
    }
}