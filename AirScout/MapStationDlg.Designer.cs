namespace AirScout
{
    partial class MapStationDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapStationDlg));
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_Zoom = new System.Windows.Forms.TextBox();
            this.btn_Zoom_Out = new System.Windows.Forms.Button();
            this.btn_Zoom_In = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.gm_Callsign = new GMap.NET.WindowsForms.GMapControl();
            this.tb_Callsign = new ScoutBase.Core.CallsignTextBox();
            this.tb_Locator = new ScoutBase.Core.LocatorTextBox();
            this.tb_Longitude = new ScoutBase.Core.DoubleTextBox();
            this.tb_Latitude = new ScoutBase.Core.DoubleTextBox();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Elevation = new ScoutBase.Core.DoubleTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(12, 9);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(702, 60);
            this.label19.TabIndex = 43;
            this.label19.Text = resources.GetString("label19.Text");
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(801, 352);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(61, 13);
            this.label18.TabIndex = 42;
            this.label18.Text = "Map Zoom:";
            // 
            // tb_Zoom
            // 
            this.tb_Zoom.BackColor = System.Drawing.Color.FloralWhite;
            this.tb_Zoom.Location = new System.Drawing.Point(914, 346);
            this.tb_Zoom.Name = "tb_Zoom";
            this.tb_Zoom.Size = new System.Drawing.Size(30, 20);
            this.tb_Zoom.TabIndex = 41;
            // 
            // btn_Zoom_Out
            // 
            this.btn_Zoom_Out.Location = new System.Drawing.Point(952, 345);
            this.btn_Zoom_Out.Name = "btn_Zoom_Out";
            this.btn_Zoom_Out.Size = new System.Drawing.Size(30, 25);
            this.btn_Zoom_Out.TabIndex = 40;
            this.btn_Zoom_Out.Text = "-";
            this.btn_Zoom_Out.UseVisualStyleBackColor = true;
            this.btn_Zoom_Out.Click += new System.EventHandler(this.btn_Zoom_Out_Click);
            // 
            // btn_Zoom_In
            // 
            this.btn_Zoom_In.Location = new System.Drawing.Point(875, 345);
            this.btn_Zoom_In.Name = "btn_Zoom_In";
            this.btn_Zoom_In.Size = new System.Drawing.Size(30, 25);
            this.btn_Zoom_In.TabIndex = 39;
            this.btn_Zoom_In.Text = "+";
            this.btn_Zoom_In.UseVisualStyleBackColor = true;
            this.btn_Zoom_In.Click += new System.EventHandler(this.btn_Zoom_In_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(801, 144);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 37;
            this.label11.Text = "Locator:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(801, 201);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Longitude:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(801, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Latitude:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(801, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 31;
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
            this.gm_Callsign.Location = new System.Drawing.Point(15, 87);
            this.gm_Callsign.MarkersEnabled = true;
            this.gm_Callsign.MaxZoom = 2;
            this.gm_Callsign.MinZoom = 2;
            this.gm_Callsign.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gm_Callsign.Name = "gm_Callsign";
            this.gm_Callsign.NegativeMode = false;
            this.gm_Callsign.PolygonsEnabled = true;
            this.gm_Callsign.RetryLoadTile = 0;
            this.gm_Callsign.RoutesEnabled = true;
            this.gm_Callsign.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gm_Callsign.ShowTileGridLines = false;
            this.gm_Callsign.Size = new System.Drawing.Size(760, 621);
            this.gm_Callsign.TabIndex = 29;
            this.gm_Callsign.Zoom = 0D;
            this.gm_Callsign.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.gm_Callsign_OnMarkerEnter);
            this.gm_Callsign.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gm_Callsign_OnMapZoomChanged);
            this.gm_Callsign.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gm_Callsign_MouseDown);
            this.gm_Callsign.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gm_Callsign_MouseMove);
            this.gm_Callsign.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gm_Callsign_MouseUp);
            // 
            // tb_Callsign
            // 
            this.tb_Callsign.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Callsign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Callsign.ErrorBackColor = System.Drawing.Color.Red;
            this.tb_Callsign.ErrorForeColor = System.Drawing.Color.White;
            this.tb_Callsign.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Callsign.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tb_Callsign.Location = new System.Drawing.Point(875, 113);
            this.tb_Callsign.Name = "tb_Callsign";
            this.tb_Callsign.ReadOnly = true;
            this.tb_Callsign.Size = new System.Drawing.Size(107, 21);
            this.tb_Callsign.TabIndex = 30;
            // 
            // tb_Locator
            // 
            this.tb_Locator.DataBindings.Add(new System.Windows.Forms.Binding("SmallLettersForSubsquares", global::AirScout.Properties.Settings.Default, "Locator_SmallLettersForSubsquares", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Locator.ErrorBackColor = System.Drawing.Color.Red;
            this.tb_Locator.ErrorForeColor = System.Drawing.Color.White;
            this.tb_Locator.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Locator.Location = new System.Drawing.Point(875, 140);
            this.tb_Locator.Name = "tb_Locator";
            this.tb_Locator.Size = new System.Drawing.Size(107, 21);
            this.tb_Locator.SmallLettersForSubsquares = global::AirScout.Properties.Settings.Default.Locator_SmallLettersForSubsquares;
            this.tb_Locator.TabIndex = 35;
            this.tb_Locator.TextChanged += new System.EventHandler(this.tb_Locator_TextChanged);
            // 
            // tb_Longitude
            // 
            this.tb_Longitude.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Longitude.FormatSpecifier = "";
            this.tb_Longitude.Location = new System.Drawing.Point(875, 197);
            this.tb_Longitude.MaxValue = 180D;
            this.tb_Longitude.MinValue = -180D;
            this.tb_Longitude.Name = "tb_Longitude";
            this.tb_Longitude.Size = new System.Drawing.Size(107, 21);
            this.tb_Longitude.TabIndex = 33;
            this.tb_Longitude.Text = "10.68327000";
            this.tb_Longitude.Value = 10.68327D;
            this.tb_Longitude.TextChanged += new System.EventHandler(this.tb_Longitude_TextChanged);
            // 
            // tb_Latitude
            // 
            this.tb_Latitude.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Latitude.FormatSpecifier = "";
            this.tb_Latitude.Location = new System.Drawing.Point(875, 167);
            this.tb_Latitude.MaxValue = 90D;
            this.tb_Latitude.MinValue = -90D;
            this.tb_Latitude.Name = "tb_Latitude";
            this.tb_Latitude.Size = new System.Drawing.Size(107, 21);
            this.tb_Latitude.TabIndex = 32;
            this.tb_Latitude.Text = "50.93706700";
            this.tb_Latitude.Value = 50.937067D;
            this.tb_Latitude.TextChanged += new System.EventHandler(this.tb_Latitude_TextChanged);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(806, 685);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 44;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(907, 685);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 45;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(801, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Elevation:";
            // 
            // tb_Elevation
            // 
            this.tb_Elevation.BackColor = System.Drawing.SystemColors.Control;
            this.tb_Elevation.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Elevation.FormatSpecifier = "F0";
            this.tb_Elevation.Location = new System.Drawing.Point(875, 223);
            this.tb_Elevation.MaxValue = 0D;
            this.tb_Elevation.MinValue = 0D;
            this.tb_Elevation.Name = "tb_Elevation";
            this.tb_Elevation.ReadOnly = true;
            this.tb_Elevation.Size = new System.Drawing.Size(53, 21);
            this.tb_Elevation.TabIndex = 48;
            this.tb_Elevation.Text = "0";
            this.tb_Elevation.Value = 0D;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(938, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "m asl";
            // 
            // MapStationDlg
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Elevation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tb_Zoom);
            this.Controls.Add(this.btn_Zoom_Out);
            this.Controls.Add(this.btn_Zoom_In);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gm_Callsign);
            this.Controls.Add(this.tb_Callsign);
            this.Controls.Add(this.tb_Locator);
            this.Controls.Add(this.tb_Longitude);
            this.Controls.Add(this.tb_Latitude);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapStationDlg";
            this.Text = "MapStationDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb_Zoom;
        private System.Windows.Forms.Button btn_Zoom_Out;
        private System.Windows.Forms.Button btn_Zoom_In;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private GMap.NET.WindowsForms.GMapControl gm_Callsign;
        private ScoutBase.Core.CallsignTextBox tb_Callsign;
        private ScoutBase.Core.LocatorTextBox tb_Locator;
        private ScoutBase.Core.DoubleTextBox tb_Longitude;
        private ScoutBase.Core.DoubleTextBox tb_Latitude;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label1;
        private ScoutBase.Core.DoubleTextBox tb_Elevation;
        private System.Windows.Forms.Label label2;
    }
}