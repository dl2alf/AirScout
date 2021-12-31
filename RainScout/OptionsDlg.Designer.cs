namespace RainScout
{
    partial class OptionsDlg
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
            this.tc_Main = new System.Windows.Forms.TabControl();
            this.tp_General = new System.Windows.Forms.TabPage();
            this.tp_Stations = new System.Windows.Forms.TabPage();
            this.btn_Options_Stations_Update = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_Options_Stations_5760M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_76G = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_47G = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_24G = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_10G = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_3400M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_2320M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_1296M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_432M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_144M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_70M = new System.Windows.Forms.CheckBox();
            this.cb_Options_Stations_50M = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_Options_Stations_Height = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Options_Stations_Elevation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Options_Stations_Loc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Options_Stations_Lon = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Options_Stations_Lat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Options_Stations_Call = new System.Windows.Forms.TextBox();
            this.tc_Scatter = new System.Windows.Forms.TabPage();
            this.tb_Options_Scatter_MaxAngle = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_Options_Scatter_MaxHeight = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Options_Scatter_MinHeight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.ud_MinLat = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ud_MaxLon = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.ud_MaxLat = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.ud_MinLon = new System.Windows.Forms.NumericUpDown();
            this.tc_Main.SuspendLayout();
            this.tp_General.SuspendLayout();
            this.tp_Stations.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tc_Scatter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLon)).BeginInit();
            this.SuspendLayout();
            // 
            // tc_Main
            // 
            this.tc_Main.Controls.Add(this.tp_General);
            this.tc_Main.Controls.Add(this.tp_Stations);
            this.tc_Main.Controls.Add(this.tc_Scatter);
            this.tc_Main.Location = new System.Drawing.Point(12, 12);
            this.tc_Main.Name = "tc_Main";
            this.tc_Main.SelectedIndex = 0;
            this.tc_Main.Size = new System.Drawing.Size(592, 403);
            this.tc_Main.TabIndex = 0;
            // 
            // tp_General
            // 
            this.tp_General.BackColor = System.Drawing.SystemColors.Control;
            this.tp_General.Controls.Add(this.label13);
            this.tp_General.Controls.Add(this.ud_MinLon);
            this.tp_General.Controls.Add(this.label12);
            this.tp_General.Controls.Add(this.ud_MaxLat);
            this.tp_General.Controls.Add(this.label11);
            this.tp_General.Controls.Add(this.ud_MaxLon);
            this.tp_General.Controls.Add(this.label10);
            this.tp_General.Controls.Add(this.ud_MinLat);
            this.tp_General.Location = new System.Drawing.Point(4, 22);
            this.tp_General.Name = "tp_General";
            this.tp_General.Padding = new System.Windows.Forms.Padding(3);
            this.tp_General.Size = new System.Drawing.Size(584, 377);
            this.tp_General.TabIndex = 0;
            this.tp_General.Text = "General";
            // 
            // tp_Stations
            // 
            this.tp_Stations.BackColor = System.Drawing.SystemColors.Control;
            this.tp_Stations.Controls.Add(this.btn_Options_Stations_Update);
            this.tp_Stations.Controls.Add(this.groupBox2);
            this.tp_Stations.Controls.Add(this.groupBox1);
            this.tp_Stations.Location = new System.Drawing.Point(4, 22);
            this.tp_Stations.Name = "tp_Stations";
            this.tp_Stations.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Stations.Size = new System.Drawing.Size(584, 377);
            this.tp_Stations.TabIndex = 1;
            this.tp_Stations.Text = "Stations";
            this.tp_Stations.Enter += new System.EventHandler(this.tp_Stations_Enter);
            // 
            // btn_Options_Stations_Update
            // 
            this.btn_Options_Stations_Update.Location = new System.Drawing.Point(468, 42);
            this.btn_Options_Stations_Update.Name = "btn_Options_Stations_Update";
            this.btn_Options_Stations_Update.Size = new System.Drawing.Size(84, 20);
            this.btn_Options_Stations_Update.TabIndex = 14;
            this.btn_Options_Stations_Update.Text = "Update";
            this.btn_Options_Stations_Update.UseVisualStyleBackColor = true;
            this.btn_Options_Stations_Update.Click += new System.EventHandler(this.btn_Options_Stations_Update_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_Options_Stations_5760M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_76G);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_47G);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_24G);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_10G);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_3400M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_2320M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_1296M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_432M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_144M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_70M);
            this.groupBox2.Controls.Add(this.cb_Options_Stations_50M);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 136);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "QRV";
            // 
            // cb_Options_Stations_5760M
            // 
            this.cb_Options_Stations_5760M.AutoSize = true;
            this.cb_Options_Stations_5760M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_5760M.Location = new System.Drawing.Point(113, 106);
            this.cb_Options_Stations_5760M.Name = "cb_Options_Stations_5760M";
            this.cb_Options_Stations_5760M.Size = new System.Drawing.Size(62, 17);
            this.cb_Options_Stations_5760M.TabIndex = 11;
            this.cb_Options_Stations_5760M.Text = "5.7GHz";
            this.cb_Options_Stations_5760M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_76G
            // 
            this.cb_Options_Stations_76G.AutoSize = true;
            this.cb_Options_Stations_76G.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_76G.Location = new System.Drawing.Point(207, 108);
            this.cb_Options_Stations_76G.Name = "cb_Options_Stations_76G";
            this.cb_Options_Stations_76G.Size = new System.Drawing.Size(59, 17);
            this.cb_Options_Stations_76G.TabIndex = 10;
            this.cb_Options_Stations_76G.Text = "76GHz";
            this.cb_Options_Stations_76G.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_47G
            // 
            this.cb_Options_Stations_47G.AutoSize = true;
            this.cb_Options_Stations_47G.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_47G.Location = new System.Drawing.Point(207, 83);
            this.cb_Options_Stations_47G.Name = "cb_Options_Stations_47G";
            this.cb_Options_Stations_47G.Size = new System.Drawing.Size(59, 17);
            this.cb_Options_Stations_47G.TabIndex = 9;
            this.cb_Options_Stations_47G.Text = "47GHz";
            this.cb_Options_Stations_47G.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_24G
            // 
            this.cb_Options_Stations_24G.AutoSize = true;
            this.cb_Options_Stations_24G.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_24G.Location = new System.Drawing.Point(207, 56);
            this.cb_Options_Stations_24G.Name = "cb_Options_Stations_24G";
            this.cb_Options_Stations_24G.Size = new System.Drawing.Size(59, 17);
            this.cb_Options_Stations_24G.TabIndex = 8;
            this.cb_Options_Stations_24G.Text = "24GHz";
            this.cb_Options_Stations_24G.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_10G
            // 
            this.cb_Options_Stations_10G.AutoSize = true;
            this.cb_Options_Stations_10G.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_10G.Location = new System.Drawing.Point(207, 27);
            this.cb_Options_Stations_10G.Name = "cb_Options_Stations_10G";
            this.cb_Options_Stations_10G.Size = new System.Drawing.Size(59, 17);
            this.cb_Options_Stations_10G.TabIndex = 7;
            this.cb_Options_Stations_10G.Text = "10GHz";
            this.cb_Options_Stations_10G.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_3400M
            // 
            this.cb_Options_Stations_3400M.AutoSize = true;
            this.cb_Options_Stations_3400M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_3400M.Location = new System.Drawing.Point(113, 79);
            this.cb_Options_Stations_3400M.Name = "cb_Options_Stations_3400M";
            this.cb_Options_Stations_3400M.Size = new System.Drawing.Size(62, 17);
            this.cb_Options_Stations_3400M.TabIndex = 6;
            this.cb_Options_Stations_3400M.Text = "3.4GHz";
            this.cb_Options_Stations_3400M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_2320M
            // 
            this.cb_Options_Stations_2320M.AutoSize = true;
            this.cb_Options_Stations_2320M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_2320M.Location = new System.Drawing.Point(113, 53);
            this.cb_Options_Stations_2320M.Name = "cb_Options_Stations_2320M";
            this.cb_Options_Stations_2320M.Size = new System.Drawing.Size(62, 17);
            this.cb_Options_Stations_2320M.TabIndex = 5;
            this.cb_Options_Stations_2320M.Text = "2.3GHz";
            this.cb_Options_Stations_2320M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_1296M
            // 
            this.cb_Options_Stations_1296M.AutoSize = true;
            this.cb_Options_Stations_1296M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_1296M.Location = new System.Drawing.Point(113, 27);
            this.cb_Options_Stations_1296M.Name = "cb_Options_Stations_1296M";
            this.cb_Options_Stations_1296M.Size = new System.Drawing.Size(62, 17);
            this.cb_Options_Stations_1296M.TabIndex = 4;
            this.cb_Options_Stations_1296M.Text = "1.2GHz";
            this.cb_Options_Stations_1296M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_432M
            // 
            this.cb_Options_Stations_432M.AutoSize = true;
            this.cb_Options_Stations_432M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_432M.Location = new System.Drawing.Point(21, 108);
            this.cb_Options_Stations_432M.Name = "cb_Options_Stations_432M";
            this.cb_Options_Stations_432M.Size = new System.Drawing.Size(64, 17);
            this.cb_Options_Stations_432M.TabIndex = 3;
            this.cb_Options_Stations_432M.Text = "432Mhz";
            this.cb_Options_Stations_432M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_144M
            // 
            this.cb_Options_Stations_144M.AutoSize = true;
            this.cb_Options_Stations_144M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_144M.Location = new System.Drawing.Point(21, 83);
            this.cb_Options_Stations_144M.Name = "cb_Options_Stations_144M";
            this.cb_Options_Stations_144M.Size = new System.Drawing.Size(64, 17);
            this.cb_Options_Stations_144M.TabIndex = 2;
            this.cb_Options_Stations_144M.Text = "144Mhz";
            this.cb_Options_Stations_144M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_70M
            // 
            this.cb_Options_Stations_70M.AutoSize = true;
            this.cb_Options_Stations_70M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_70M.Location = new System.Drawing.Point(21, 56);
            this.cb_Options_Stations_70M.Name = "cb_Options_Stations_70M";
            this.cb_Options_Stations_70M.Size = new System.Drawing.Size(58, 17);
            this.cb_Options_Stations_70M.TabIndex = 1;
            this.cb_Options_Stations_70M.Text = "70Mhz";
            this.cb_Options_Stations_70M.UseVisualStyleBackColor = true;
            // 
            // cb_Options_Stations_50M
            // 
            this.cb_Options_Stations_50M.AutoSize = true;
            this.cb_Options_Stations_50M.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Options_Stations_50M.Location = new System.Drawing.Point(21, 27);
            this.cb_Options_Stations_50M.Name = "cb_Options_Stations_50M";
            this.cb_Options_Stations_50M.Size = new System.Drawing.Size(58, 17);
            this.cb_Options_Stations_50M.TabIndex = 0;
            this.cb_Options_Stations_50M.Text = "50Mhz";
            this.cb_Options_Stations_50M.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_Options_Stations_Height);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_Options_Stations_Elevation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_Options_Stations_Loc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_Options_Stations_Lon);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_Options_Stations_Lat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_Options_Stations_Call);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 194);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Geographical";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Antenna Height:";
            // 
            // tb_Options_Stations_Height
            // 
            this.tb_Options_Stations_Height.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Options_Stations_Height.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Options_Stations_Height.Location = new System.Drawing.Point(116, 158);
            this.tb_Options_Stations_Height.Name = "tb_Options_Stations_Height";
            this.tb_Options_Stations_Height.Size = new System.Drawing.Size(134, 20);
            this.tb_Options_Stations_Height.TabIndex = 22;
            this.tb_Options_Stations_Height.TextChanged += new System.EventHandler(this.tb_Options_Stations_Height_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Elevation:";
            // 
            // tb_Options_Stations_Elevation
            // 
            this.tb_Options_Stations_Elevation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Options_Stations_Elevation.Location = new System.Drawing.Point(116, 132);
            this.tb_Options_Stations_Elevation.Name = "tb_Options_Stations_Elevation";
            this.tb_Options_Stations_Elevation.ReadOnly = true;
            this.tb_Options_Stations_Elevation.Size = new System.Drawing.Size(134, 20);
            this.tb_Options_Stations_Elevation.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Locator:";
            // 
            // tb_Options_Stations_Loc
            // 
            this.tb_Options_Stations_Loc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Options_Stations_Loc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Options_Stations_Loc.Location = new System.Drawing.Point(116, 106);
            this.tb_Options_Stations_Loc.Name = "tb_Options_Stations_Loc";
            this.tb_Options_Stations_Loc.Size = new System.Drawing.Size(134, 20);
            this.tb_Options_Stations_Loc.TabIndex = 18;
            this.tb_Options_Stations_Loc.Text = "JO50IW";
            this.tb_Options_Stations_Loc.TextChanged += new System.EventHandler(this.tb_Options_Stations_Loc_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Longitude:";
            // 
            // tb_Options_Stations_Lon
            // 
            this.tb_Options_Stations_Lon.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Options_Stations_Lon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Options_Stations_Lon.Location = new System.Drawing.Point(116, 80);
            this.tb_Options_Stations_Lon.Name = "tb_Options_Stations_Lon";
            this.tb_Options_Stations_Lon.Size = new System.Drawing.Size(134, 20);
            this.tb_Options_Stations_Lon.TabIndex = 16;
            this.tb_Options_Stations_Lon.TextChanged += new System.EventHandler(this.tb_Options_Stations_Lon_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Latitude:";
            // 
            // tb_Options_Stations_Lat
            // 
            this.tb_Options_Stations_Lat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Options_Stations_Lat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Options_Stations_Lat.Location = new System.Drawing.Point(116, 54);
            this.tb_Options_Stations_Lat.Name = "tb_Options_Stations_Lat";
            this.tb_Options_Stations_Lat.Size = new System.Drawing.Size(134, 20);
            this.tb_Options_Stations_Lat.TabIndex = 14;
            this.tb_Options_Stations_Lat.TextChanged += new System.EventHandler(this.tb_Options_Stations_Lat_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Call sign:";
            // 
            // tb_Options_Stations_Call
            // 
            this.tb_Options_Stations_Call.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Options_Stations_Call.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Options_Stations_Call.Location = new System.Drawing.Point(116, 28);
            this.tb_Options_Stations_Call.Name = "tb_Options_Stations_Call";
            this.tb_Options_Stations_Call.Size = new System.Drawing.Size(134, 20);
            this.tb_Options_Stations_Call.TabIndex = 12;
            this.tb_Options_Stations_Call.Text = "DL2ALF";
            this.tb_Options_Stations_Call.TextChanged += new System.EventHandler(this.tb_Options_Stations_Call_TextChanged);
            // 
            // tc_Scatter
            // 
            this.tc_Scatter.BackColor = System.Drawing.SystemColors.Control;
            this.tc_Scatter.Controls.Add(this.tb_Options_Scatter_MaxAngle);
            this.tc_Scatter.Controls.Add(this.label9);
            this.tc_Scatter.Controls.Add(this.tb_Options_Scatter_MaxHeight);
            this.tc_Scatter.Controls.Add(this.label8);
            this.tc_Scatter.Controls.Add(this.tb_Options_Scatter_MinHeight);
            this.tc_Scatter.Controls.Add(this.label7);
            this.tc_Scatter.Location = new System.Drawing.Point(4, 22);
            this.tc_Scatter.Name = "tc_Scatter";
            this.tc_Scatter.Padding = new System.Windows.Forms.Padding(3);
            this.tc_Scatter.Size = new System.Drawing.Size(584, 377);
            this.tc_Scatter.TabIndex = 2;
            this.tc_Scatter.Text = "Scatter";
            this.tc_Scatter.Enter += new System.EventHandler(this.tc_Scatter_Enter);
            // 
            // tb_Options_Scatter_MaxAngle
            // 
            this.tb_Options_Scatter_MaxAngle.Location = new System.Drawing.Point(122, 86);
            this.tb_Options_Scatter_MaxAngle.Name = "tb_Options_Scatter_MaxAngle";
            this.tb_Options_Scatter_MaxAngle.Size = new System.Drawing.Size(100, 20);
            this.tb_Options_Scatter_MaxAngle.TabIndex = 5;
            this.tb_Options_Scatter_MaxAngle.TextChanged += new System.EventHandler(this.tb_Options_Scatter_MaxAngle_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 89);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Scatter Max Angle:";
            // 
            // tb_Options_Scatter_MaxHeight
            // 
            this.tb_Options_Scatter_MaxHeight.Location = new System.Drawing.Point(122, 57);
            this.tb_Options_Scatter_MaxHeight.Name = "tb_Options_Scatter_MaxHeight";
            this.tb_Options_Scatter_MaxHeight.Size = new System.Drawing.Size(100, 20);
            this.tb_Options_Scatter_MaxHeight.TabIndex = 3;
            this.tb_Options_Scatter_MaxHeight.TextChanged += new System.EventHandler(this.tb_Options_Scatter_MaxHeight_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Scatter Max Height:";
            // 
            // tb_Options_Scatter_MinHeight
            // 
            this.tb_Options_Scatter_MinHeight.Location = new System.Drawing.Point(122, 31);
            this.tb_Options_Scatter_MinHeight.Name = "tb_Options_Scatter_MinHeight";
            this.tb_Options_Scatter_MinHeight.Size = new System.Drawing.Size(100, 20);
            this.tb_Options_Scatter_MinHeight.TabIndex = 1;
            this.tb_Options_Scatter_MinHeight.TextChanged += new System.EventHandler(this.tb_Options_Scatter_MinHeight_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Scatter Min Height:";
            // 
            // btn_OK
            // 
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(647, 63);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(647, 104);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // ud_MinLat
            // 
            this.ud_MinLat.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::RainScout.Properties.Settings.Default, "MinLat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MinLat.Location = new System.Drawing.Point(505, 244);
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
            this.ud_MinLat.Size = new System.Drawing.Size(51, 20);
            this.ud_MinLat.TabIndex = 0;
            this.ud_MinLat.Value = global::RainScout.Properties.Settings.Default.MinLat;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(435, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "MinLat:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(435, 324);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "MaxLon:";
            // 
            // ud_MaxLon
            // 
            this.ud_MaxLon.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::RainScout.Properties.Settings.Default, "MaxLon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MaxLon.Location = new System.Drawing.Point(505, 322);
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
            this.ud_MaxLon.Size = new System.Drawing.Size(51, 20);
            this.ud_MaxLon.TabIndex = 2;
            this.ud_MaxLon.Value = global::RainScout.Properties.Settings.Default.MaxLon;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(435, 298);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "MaxLat:";
            // 
            // ud_MaxLat
            // 
            this.ud_MaxLat.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::RainScout.Properties.Settings.Default, "MaxLat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MaxLat.Location = new System.Drawing.Point(505, 296);
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
            this.ud_MaxLat.Size = new System.Drawing.Size(51, 20);
            this.ud_MaxLat.TabIndex = 4;
            this.ud_MaxLat.Value = global::RainScout.Properties.Settings.Default.MaxLat;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(435, 272);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "MinLon:";
            // 
            // ud_MinLon
            // 
            this.ud_MinLon.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::RainScout.Properties.Settings.Default, "MinLon", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_MinLon.Location = new System.Drawing.Point(505, 270);
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
            this.ud_MinLon.Size = new System.Drawing.Size(51, 20);
            this.ud_MinLon.TabIndex = 6;
            this.ud_MinLon.Value = global::RainScout.Properties.Settings.Default.MinLon;
            // 
            // OptionsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 442);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.tc_Main);
            this.Name = "OptionsDlg";
            this.Text = "RainScout Options";
            this.tc_Main.ResumeLayout(false);
            this.tp_General.ResumeLayout(false);
            this.tp_General.PerformLayout();
            this.tp_Stations.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tc_Scatter.ResumeLayout(false);
            this.tc_Scatter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MaxLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_MinLon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_Main;
        private System.Windows.Forms.TabPage tp_General;
        private System.Windows.Forms.TabPage tp_Stations;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_Options_Stations_50M;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_Options_Stations_Height;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Options_Stations_Elevation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Options_Stations_Loc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Options_Stations_Lon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Options_Stations_Lat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Options_Stations_Call;
        private System.Windows.Forms.CheckBox cb_Options_Stations_1296M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_432M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_144M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_70M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_3400M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_2320M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_5760M;
        private System.Windows.Forms.CheckBox cb_Options_Stations_76G;
        private System.Windows.Forms.CheckBox cb_Options_Stations_47G;
        private System.Windows.Forms.CheckBox cb_Options_Stations_24G;
        private System.Windows.Forms.CheckBox cb_Options_Stations_10G;
        private System.Windows.Forms.TabPage tc_Scatter;
        private System.Windows.Forms.TextBox tb_Options_Scatter_MaxAngle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_Options_Scatter_MaxHeight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Options_Scatter_MinHeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Options_Stations_Update;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown ud_MinLon;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown ud_MaxLat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown ud_MaxLon;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown ud_MinLat;
    }
}