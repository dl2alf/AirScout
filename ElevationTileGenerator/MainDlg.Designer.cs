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
            this.btn_Delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Compare = new System.Windows.Forms.Button();
            this.btn_Compare_Tile2 = new System.Windows.Forms.Button();
            this.btn_Compare_Tile1 = new System.Windows.Forms.Button();
            this.tb_Tile2 = new System.Windows.Forms.TextBox();
            this.tb_Tile1 = new System.Windows.Forms.TextBox();
            this.tb_DestDir = new System.Windows.Forms.TextBox();
            this.tb_SourceDir = new System.Windows.Forms.TextBox();
            this.tb_Diff = new System.Windows.Forms.TextBox();
            this.btn_ShowDiff = new System.Windows.Forms.Button();
            this.lbl_Diff_Dimensions = new System.Windows.Forms.Label();
            this.lbl_Diff_Min = new System.Windows.Forms.Label();
            this.lbl_Diff_Total = new System.Windows.Forms.Label();
            this.lbl_Diff_Max = new System.Windows.Forms.Label();
            this.tb_Show_Diff = new System.Windows.Forms.TextBox();
            this.lbl_Diff_Voids = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pb_Diff = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_Diff_Mean = new System.Windows.Forms.Label();
            this.btn_PictureFromZIP = new System.Windows.Forms.Button();
            this.btn_ConvertSRTM1ToSRTM3 = new System.Windows.Forms.Button();
            this.ss_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Picture)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Diff)).BeginInit();
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
            this.btn_Start.Location = new System.Drawing.Point(36, 478);
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
            this.btn_Stop.Location = new System.Drawing.Point(117, 478);
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
            this.pb_Picture.Location = new System.Drawing.Point(36, 117);
            this.pb_Picture.Name = "pb_Picture";
            this.pb_Picture.Size = new System.Drawing.Size(586, 326);
            this.pb_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Picture.TabIndex = 9;
            this.pb_Picture.TabStop = false;
            // 
            // btn_Picture
            // 
            this.btn_Picture.Location = new System.Drawing.Point(310, 507);
            this.btn_Picture.Name = "btn_Picture";
            this.btn_Picture.Size = new System.Drawing.Size(105, 23);
            this.btn_Picture.TabIndex = 10;
            this.btn_Picture.Text = "Picture";
            this.btn_Picture.UseVisualStyleBackColor = true;
            this.btn_Picture.Click += new System.EventHandler(this.btn_Picture_Click);
            // 
            // btn_ZIP
            // 
            this.btn_ZIP.Location = new System.Drawing.Point(421, 478);
            this.btn_ZIP.Name = "btn_ZIP";
            this.btn_ZIP.Size = new System.Drawing.Size(75, 23);
            this.btn_ZIP.TabIndex = 11;
            this.btn_ZIP.Text = "ZIP";
            this.btn_ZIP.UseVisualStyleBackColor = true;
            this.btn_ZIP.Click += new System.EventHandler(this.btn_ZIP_Click);
            // 
            // btn_CAT
            // 
            this.btn_CAT.Location = new System.Drawing.Point(502, 478);
            this.btn_CAT.Name = "btn_CAT";
            this.btn_CAT.Size = new System.Drawing.Size(75, 23);
            this.btn_CAT.TabIndex = 12;
            this.btn_CAT.Text = "CAT";
            this.btn_CAT.UseVisualStyleBackColor = true;
            this.btn_CAT.Click += new System.EventHandler(this.btn_CAT_Click);
            // 
            // btn_TestCAT
            // 
            this.btn_TestCAT.Location = new System.Drawing.Point(502, 507);
            this.btn_TestCAT.Name = "btn_TestCAT";
            this.btn_TestCAT.Size = new System.Drawing.Size(75, 23);
            this.btn_TestCAT.TabIndex = 13;
            this.btn_TestCAT.Text = "TestCAT";
            this.btn_TestCAT.UseVisualStyleBackColor = true;
            this.btn_TestCAT.Click += new System.EventHandler(this.btn_TestCAT_Click);
            // 
            // btn_CATFromZIP
            // 
            this.btn_CATFromZIP.Location = new System.Drawing.Point(502, 449);
            this.btn_CATFromZIP.Name = "btn_CATFromZIP";
            this.btn_CATFromZIP.Size = new System.Drawing.Size(75, 23);
            this.btn_CATFromZIP.TabIndex = 14;
            this.btn_CATFromZIP.Text = "CAT fm ZIP";
            this.btn_CATFromZIP.UseVisualStyleBackColor = true;
            this.btn_CATFromZIP.Click += new System.EventHandler(this.btn_CATFromZIP_Click);
            // 
            // btn_PictureFromTiles
            // 
            this.btn_PictureFromTiles.Location = new System.Drawing.Point(310, 478);
            this.btn_PictureFromTiles.Name = "btn_PictureFromTiles";
            this.btn_PictureFromTiles.Size = new System.Drawing.Size(105, 23);
            this.btn_PictureFromTiles.TabIndex = 15;
            this.btn_PictureFromTiles.Text = "Pic fm Loc-Tiles";
            this.btn_PictureFromTiles.UseVisualStyleBackColor = true;
            this.btn_PictureFromTiles.Click += new System.EventHandler(this.btn_PictureFromTiles_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(229, 478);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 23);
            this.btn_Delete.TabIndex = 16;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lbl_Diff_Mean);
            this.groupBox1.Controls.Add(this.pb_Diff);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbl_Diff_Voids);
            this.groupBox1.Controls.Add(this.tb_Show_Diff);
            this.groupBox1.Controls.Add(this.lbl_Diff_Max);
            this.groupBox1.Controls.Add(this.lbl_Diff_Total);
            this.groupBox1.Controls.Add(this.lbl_Diff_Min);
            this.groupBox1.Controls.Add(this.lbl_Diff_Dimensions);
            this.groupBox1.Controls.Add(this.btn_ShowDiff);
            this.groupBox1.Controls.Add(this.tb_Diff);
            this.groupBox1.Controls.Add(this.btn_Compare);
            this.groupBox1.Controls.Add(this.btn_Compare_Tile2);
            this.groupBox1.Controls.Add(this.btn_Compare_Tile1);
            this.groupBox1.Controls.Add(this.tb_Tile2);
            this.groupBox1.Controls.Add(this.tb_Tile1);
            this.groupBox1.Location = new System.Drawing.Point(671, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 431);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TileCompare";
            // 
            // btn_Compare
            // 
            this.btn_Compare.Location = new System.Drawing.Point(243, 76);
            this.btn_Compare.Name = "btn_Compare";
            this.btn_Compare.Size = new System.Drawing.Size(69, 23);
            this.btn_Compare.TabIndex = 19;
            this.btn_Compare.Text = "Compare";
            this.btn_Compare.UseVisualStyleBackColor = true;
            this.btn_Compare.Click += new System.EventHandler(this.btn_Compare_Click);
            // 
            // btn_Compare_Tile2
            // 
            this.btn_Compare_Tile2.Location = new System.Drawing.Point(243, 47);
            this.btn_Compare_Tile2.Name = "btn_Compare_Tile2";
            this.btn_Compare_Tile2.Size = new System.Drawing.Size(69, 23);
            this.btn_Compare_Tile2.TabIndex = 18;
            this.btn_Compare_Tile2.Text = "Select";
            this.btn_Compare_Tile2.UseVisualStyleBackColor = true;
            this.btn_Compare_Tile2.Click += new System.EventHandler(this.btn_Compare_Tile2_Click);
            // 
            // btn_Compare_Tile1
            // 
            this.btn_Compare_Tile1.Location = new System.Drawing.Point(243, 18);
            this.btn_Compare_Tile1.Name = "btn_Compare_Tile1";
            this.btn_Compare_Tile1.Size = new System.Drawing.Size(69, 23);
            this.btn_Compare_Tile1.TabIndex = 17;
            this.btn_Compare_Tile1.Text = "Select";
            this.btn_Compare_Tile1.UseVisualStyleBackColor = true;
            this.btn_Compare_Tile1.Click += new System.EventHandler(this.btn_Compare_Tile1_Click);
            // 
            // tb_Tile2
            // 
            this.tb_Tile2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ElevationTileGenerator.Properties.Settings.Default, "Tile2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Tile2.Location = new System.Drawing.Point(7, 49);
            this.tb_Tile2.Name = "tb_Tile2";
            this.tb_Tile2.ReadOnly = true;
            this.tb_Tile2.Size = new System.Drawing.Size(230, 20);
            this.tb_Tile2.TabIndex = 1;
            this.tb_Tile2.Text = global::ElevationTileGenerator.Properties.Settings.Default.Tile2;
            // 
            // tb_Tile1
            // 
            this.tb_Tile1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ElevationTileGenerator.Properties.Settings.Default, "Tile1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Tile1.Location = new System.Drawing.Point(7, 20);
            this.tb_Tile1.Name = "tb_Tile1";
            this.tb_Tile1.ReadOnly = true;
            this.tb_Tile1.Size = new System.Drawing.Size(230, 20);
            this.tb_Tile1.TabIndex = 0;
            this.tb_Tile1.Text = global::ElevationTileGenerator.Properties.Settings.Default.Tile1;
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
            // tb_Diff
            // 
            this.tb_Diff.Location = new System.Drawing.Point(7, 78);
            this.tb_Diff.Name = "tb_Diff";
            this.tb_Diff.ReadOnly = true;
            this.tb_Diff.Size = new System.Drawing.Size(230, 20);
            this.tb_Diff.TabIndex = 20;
            // 
            // btn_ShowDiff
            // 
            this.btn_ShowDiff.Location = new System.Drawing.Point(243, 105);
            this.btn_ShowDiff.Name = "btn_ShowDiff";
            this.btn_ShowDiff.Size = new System.Drawing.Size(69, 23);
            this.btn_ShowDiff.TabIndex = 21;
            this.btn_ShowDiff.Text = "Show Diff";
            this.btn_ShowDiff.UseVisualStyleBackColor = true;
            this.btn_ShowDiff.Click += new System.EventHandler(this.btn_ShowDiff_Click);
            // 
            // lbl_Diff_Dimensions
            // 
            this.lbl_Diff_Dimensions.AutoSize = true;
            this.lbl_Diff_Dimensions.Location = new System.Drawing.Point(176, 140);
            this.lbl_Diff_Dimensions.Name = "lbl_Diff_Dimensions";
            this.lbl_Diff_Dimensions.Size = new System.Drawing.Size(22, 13);
            this.lbl_Diff_Dimensions.TabIndex = 22;
            this.lbl_Diff_Dimensions.Text = "xxx";
            // 
            // lbl_Diff_Min
            // 
            this.lbl_Diff_Min.AutoSize = true;
            this.lbl_Diff_Min.Location = new System.Drawing.Point(176, 192);
            this.lbl_Diff_Min.Name = "lbl_Diff_Min";
            this.lbl_Diff_Min.Size = new System.Drawing.Size(22, 13);
            this.lbl_Diff_Min.TabIndex = 23;
            this.lbl_Diff_Min.Text = "xxx";
            // 
            // lbl_Diff_Total
            // 
            this.lbl_Diff_Total.AutoSize = true;
            this.lbl_Diff_Total.Location = new System.Drawing.Point(176, 157);
            this.lbl_Diff_Total.Name = "lbl_Diff_Total";
            this.lbl_Diff_Total.Size = new System.Drawing.Size(22, 13);
            this.lbl_Diff_Total.TabIndex = 24;
            this.lbl_Diff_Total.Text = "xxx";
            // 
            // lbl_Diff_Max
            // 
            this.lbl_Diff_Max.AutoSize = true;
            this.lbl_Diff_Max.Location = new System.Drawing.Point(174, 208);
            this.lbl_Diff_Max.Name = "lbl_Diff_Max";
            this.lbl_Diff_Max.Size = new System.Drawing.Size(22, 13);
            this.lbl_Diff_Max.TabIndex = 25;
            this.lbl_Diff_Max.Text = "xxx";
            // 
            // tb_Show_Diff
            // 
            this.tb_Show_Diff.Location = new System.Drawing.Point(7, 107);
            this.tb_Show_Diff.Name = "tb_Show_Diff";
            this.tb_Show_Diff.ReadOnly = true;
            this.tb_Show_Diff.Size = new System.Drawing.Size(230, 20);
            this.tb_Show_Diff.TabIndex = 26;
            // 
            // lbl_Diff_Voids
            // 
            this.lbl_Diff_Voids.AutoSize = true;
            this.lbl_Diff_Voids.Location = new System.Drawing.Point(176, 175);
            this.lbl_Diff_Voids.Name = "lbl_Diff_Voids";
            this.lbl_Diff_Voids.Size = new System.Drawing.Size(22, 13);
            this.lbl_Diff_Voids.TabIndex = 27;
            this.lbl_Diff_Voids.Text = "xxx";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Dimensions:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Total differences:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Total voids:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Minimum difference:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Maximum difference:";
            // 
            // pb_Diff
            // 
            this.pb_Diff.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pb_Diff.Location = new System.Drawing.Point(15, 260);
            this.pb_Diff.Name = "pb_Diff";
            this.pb_Diff.Size = new System.Drawing.Size(181, 163);
            this.pb_Diff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Diff.TabIndex = 33;
            this.pb_Diff.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 225);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Mean difference:";
            // 
            // lbl_Diff_Mean
            // 
            this.lbl_Diff_Mean.AutoSize = true;
            this.lbl_Diff_Mean.Location = new System.Drawing.Point(175, 225);
            this.lbl_Diff_Mean.Name = "lbl_Diff_Mean";
            this.lbl_Diff_Mean.Size = new System.Drawing.Size(22, 13);
            this.lbl_Diff_Mean.TabIndex = 34;
            this.lbl_Diff_Mean.Text = "xxx";
            // 
            // btn_PictureFromZIP
            // 
            this.btn_PictureFromZIP.Location = new System.Drawing.Point(310, 449);
            this.btn_PictureFromZIP.Name = "btn_PictureFromZIP";
            this.btn_PictureFromZIP.Size = new System.Drawing.Size(105, 23);
            this.btn_PictureFromZIP.TabIndex = 18;
            this.btn_PictureFromZIP.Text = "Pic fm ZIP";
            this.btn_PictureFromZIP.UseVisualStyleBackColor = true;
            this.btn_PictureFromZIP.Click += new System.EventHandler(this.btn_PictureFromZIP_Click);
            // 
            // btn_ConvertSRTM1ToSRTM3
            // 
            this.btn_ConvertSRTM1ToSRTM3.Location = new System.Drawing.Point(36, 508);
            this.btn_ConvertSRTM1ToSRTM3.Name = "btn_ConvertSRTM1ToSRTM3";
            this.btn_ConvertSRTM1ToSRTM3.Size = new System.Drawing.Size(153, 23);
            this.btn_ConvertSRTM1ToSRTM3.TabIndex = 19;
            this.btn_ConvertSRTM1ToSRTM3.Text = "Convert SRTM1 to SRTM3";
            this.btn_ConvertSRTM1ToSRTM3.UseVisualStyleBackColor = true;
            this.btn_ConvertSRTM1ToSRTM3.Click += new System.EventHandler(this.btn_ConvertSRTM1ToSRTM3_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 556);
            this.Controls.Add(this.btn_ConvertSRTM1ToSRTM3);
            this.Controls.Add(this.btn_PictureFromZIP);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Diff)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Compare_Tile2;
        private System.Windows.Forms.Button btn_Compare_Tile1;
        private System.Windows.Forms.TextBox tb_Tile2;
        private System.Windows.Forms.TextBox tb_Tile1;
        private System.Windows.Forms.Button btn_Compare;
        private System.Windows.Forms.TextBox tb_Diff;
        private System.Windows.Forms.Label lbl_Diff_Dimensions;
        private System.Windows.Forms.Button btn_ShowDiff;
        private System.Windows.Forms.Label lbl_Diff_Max;
        private System.Windows.Forms.Label lbl_Diff_Total;
        private System.Windows.Forms.Label lbl_Diff_Min;
        private System.Windows.Forms.TextBox tb_Show_Diff;
        private System.Windows.Forms.Label lbl_Diff_Voids;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pb_Diff;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_Diff_Mean;
        private System.Windows.Forms.Button btn_PictureFromZIP;
        private System.Windows.Forms.Button btn_ConvertSRTM1ToSRTM3;
    }
}

