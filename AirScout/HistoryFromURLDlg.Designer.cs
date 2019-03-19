namespace AirScout
{
    partial class HistoryFromURLDlg
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
            this.label2 = new System.Windows.Forms.Label();
            this.btn_History_Start = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_History_Select = new System.Windows.Forms.Button();
            this.btn_History_Cancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ud_History_Stepwidth = new System.Windows.Forms.NumericUpDown();
            this.tb_History_Directory = new System.Windows.Forms.TextBox();
            this.dtp_History_Date = new System.Windows.Forms.DateTimePicker();
            this.tb_History_URL = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ud_History_Stepwidth)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Download from:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Date:";
            // 
            // btn_History_Start
            // 
            this.btn_History_Start.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_History_Start.Location = new System.Drawing.Point(183, 245);
            this.btn_History_Start.Name = "btn_History_Start";
            this.btn_History_Start.Size = new System.Drawing.Size(106, 23);
            this.btn_History_Start.TabIndex = 7;
            this.btn_History_Start.Text = "Start";
            this.btn_History_Start.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(523, 52);
            this.label3.TabIndex = 8;
            this.label3.Text = "Use this dialog to download one day of plane positions history and convert it to " +
    "AirScout\'s database  import file format.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(12, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(523, 52);
            this.label4.TabIndex = 9;
            this.label4.Text = "CAUTION: This will download a large file (abt. 8GB) and will use abt. 50GB of tem" +
    "porary disk space!";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Directory:";
            // 
            // tb_History_Select
            // 
            this.tb_History_Select.Location = new System.Drawing.Point(429, 149);
            this.tb_History_Select.Name = "tb_History_Select";
            this.tb_History_Select.Size = new System.Drawing.Size(106, 23);
            this.tb_History_Select.TabIndex = 12;
            this.tb_History_Select.Text = "Select";
            this.tb_History_Select.UseVisualStyleBackColor = true;
            this.tb_History_Select.Click += new System.EventHandler(this.tb_History_Select_Click);
            // 
            // btn_History_Cancel
            // 
            this.btn_History_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_History_Cancel.Location = new System.Drawing.Point(317, 245);
            this.btn_History_Cancel.Name = "btn_History_Cancel";
            this.btn_History_Cancel.Size = new System.Drawing.Size(106, 23);
            this.btn_History_Cancel.TabIndex = 13;
            this.btn_History_Cancel.Text = "Cancel";
            this.btn_History_Cancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Date:";
            // 
            // ud_History_Stepwidth
            // 
            this.ud_History_Stepwidth.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScout.Properties.Settings.Default, "Analysis_History_Stepwidth", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_History_Stepwidth.Location = new System.Drawing.Point(115, 207);
            this.ud_History_Stepwidth.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.ud_History_Stepwidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ud_History_Stepwidth.Name = "ud_History_Stepwidth";
            this.ud_History_Stepwidth.Size = new System.Drawing.Size(100, 20);
            this.ud_History_Stepwidth.TabIndex = 15;
            this.ud_History_Stepwidth.Value = global::AirScout.Properties.Settings.Default.Analysis_History_Stepwidth;
            // 
            // tb_History_Directory
            // 
            this.tb_History_Directory.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScout.Properties.Settings.Default, "Analysis_History_Directory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_History_Directory.Location = new System.Drawing.Point(115, 151);
            this.tb_History_Directory.Name = "tb_History_Directory";
            this.tb_History_Directory.ReadOnly = true;
            this.tb_History_Directory.Size = new System.Drawing.Size(308, 20);
            this.tb_History_Directory.TabIndex = 11;
            this.tb_History_Directory.Tag = "";
            this.tb_History_Directory.Text = global::AirScout.Properties.Settings.Default.Analysis_History_Directory;
            // 
            // dtp_History_Date
            // 
            this.dtp_History_Date.CustomFormat = "";
            this.dtp_History_Date.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScout.Properties.Settings.Default, "Analysis_History_Date", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dtp_History_Date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_History_Date.Location = new System.Drawing.Point(115, 177);
            this.dtp_History_Date.Name = "dtp_History_Date";
            this.dtp_History_Date.Size = new System.Drawing.Size(100, 20);
            this.dtp_History_Date.TabIndex = 2;
            this.dtp_History_Date.Value = global::AirScout.Properties.Settings.Default.Analysis_History_Date;
            // 
            // tb_History_URL
            // 
            this.tb_History_URL.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScout.Properties.Settings.Default, "Analysis_History_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_History_URL.Location = new System.Drawing.Point(115, 125);
            this.tb_History_URL.Name = "tb_History_URL";
            this.tb_History_URL.Size = new System.Drawing.Size(420, 20);
            this.tb_History_URL.TabIndex = 1;
            this.tb_History_URL.Tag = "Planes_History_URL";
            this.tb_History_URL.Text = global::AirScout.Properties.Settings.Default.Analysis_History_URL;
            // 
            // HistoryFromURLDlg
            // 
            this.AcceptButton = this.btn_History_Start;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_History_Cancel;
            this.ClientSize = new System.Drawing.Size(577, 280);
            this.Controls.Add(this.ud_History_Stepwidth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_History_Cancel);
            this.Controls.Add(this.tb_History_Select);
            this.Controls.Add(this.tb_History_Directory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_History_Start);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtp_History_Date);
            this.Controls.Add(this.tb_History_URL);
            this.Controls.Add(this.label1);
            this.Name = "HistoryFromURLDlg";
            this.Text = "Get Planes History From URL";
            ((System.ComponentModel.ISupportInitialize)(this.ud_History_Stepwidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_History_Start;
        public System.Windows.Forms.TextBox tb_History_URL;
        public System.Windows.Forms.DateTimePicker dtp_History_Date;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox tb_History_Directory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button tb_History_Select;
        private System.Windows.Forms.Button btn_History_Cancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown ud_History_Stepwidth;
    }
}