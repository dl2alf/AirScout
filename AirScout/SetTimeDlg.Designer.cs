namespace AirScout
{
    partial class SetTimeDlg
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
            this.dtp_SetTimeDlg_Start = new System.Windows.Forms.DateTimePicker();
            this.btn_SetTimeDlg_OK = new System.Windows.Forms.Button();
            this.btn_SetTimeDlg_Cancel = new System.Windows.Forms.Button();
            this.cb_Time_Online = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(52, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can set online/offline mode here and set a start time for offline mode.";
            // 
            // dtp_SetTimeDlg_Start
            // 
            this.dtp_SetTimeDlg_Start.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_SetTimeDlg_Start.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScout.Properties.Settings.Default, "Time_Offline", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dtp_SetTimeDlg_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_SetTimeDlg_Start.Location = new System.Drawing.Point(129, 98);
            this.dtp_SetTimeDlg_Start.Name = "dtp_SetTimeDlg_Start";
            this.dtp_SetTimeDlg_Start.Size = new System.Drawing.Size(125, 20);
            this.dtp_SetTimeDlg_Start.TabIndex = 1;
            this.dtp_SetTimeDlg_Start.Value = global::AirScout.Properties.Settings.Default.Time_Offline;
            // 
            // btn_SetTimeDlg_OK
            // 
            this.btn_SetTimeDlg_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_SetTimeDlg_OK.Location = new System.Drawing.Point(108, 133);
            this.btn_SetTimeDlg_OK.Name = "btn_SetTimeDlg_OK";
            this.btn_SetTimeDlg_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_SetTimeDlg_OK.TabIndex = 2;
            this.btn_SetTimeDlg_OK.Text = "OK";
            this.btn_SetTimeDlg_OK.UseVisualStyleBackColor = true;
            // 
            // btn_SetTimeDlg_Cancel
            // 
            this.btn_SetTimeDlg_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_SetTimeDlg_Cancel.Location = new System.Drawing.Point(189, 133);
            this.btn_SetTimeDlg_Cancel.Name = "btn_SetTimeDlg_Cancel";
            this.btn_SetTimeDlg_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_SetTimeDlg_Cancel.TabIndex = 3;
            this.btn_SetTimeDlg_Cancel.Text = "Cancel";
            this.btn_SetTimeDlg_Cancel.UseVisualStyleBackColor = true;
            // 
            // cb_Time_Online
            // 
            this.cb_Time_Online.AutoSize = true;
            this.cb_Time_Online.Checked = global::AirScout.Properties.Settings.Default.Time_Mode_Online;
            this.cb_Time_Online.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Time_Online.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Time_Online", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Time_Online.Location = new System.Drawing.Point(70, 66);
            this.cb_Time_Online.Name = "cb_Time_Online";
            this.cb_Time_Online.Size = new System.Drawing.Size(229, 17);
            this.cb_Time_Online.TabIndex = 4;
            this.cb_Time_Online.Text = "Online mode: use current PC time (Default).";
            this.cb_Time_Online.UseVisualStyleBackColor = true;
            this.cb_Time_Online.CheckedChanged += new System.EventHandler(this.cb_Time_Online_CheckedChanged);
            // 
            // SetTimeDlg
            // 
            this.AcceptButton = this.btn_SetTimeDlg_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 174);
            this.Controls.Add(this.cb_Time_Online);
            this.Controls.Add(this.btn_SetTimeDlg_Cancel);
            this.Controls.Add(this.btn_SetTimeDlg_OK);
            this.Controls.Add(this.dtp_SetTimeDlg_Start);
            this.Controls.Add(this.label1);
            this.Name = "SetTimeDlg";
            this.Text = "SetTimeDlg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetTimeDlg_FormClosing);
            this.Load += new System.EventHandler(this.SetTimeDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SetTimeDlg_OK;
        private System.Windows.Forms.Button btn_SetTimeDlg_Cancel;
        public System.Windows.Forms.DateTimePicker dtp_SetTimeDlg_Start;
        public System.Windows.Forms.CheckBox cb_Time_Online;
    }
}