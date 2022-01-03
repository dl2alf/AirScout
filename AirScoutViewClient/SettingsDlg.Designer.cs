namespace AirScoutViewClient
{
    partial class SettingsDlg
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ud_Settings_Server_Port = new System.Windows.Forms.NumericUpDown();
            this.tb_Settings_Server_URL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Settings_Server_Port)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ud_Settings_Server_Port);
            this.groupBox1.Controls.Add(this.tb_Settings_Server_URL);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 103);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Settings";
            // 
            // ud_Settings_Server_Port
            // 
            this.ud_Settings_Server_Port.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutViewClient.Properties.Settings.Default, "Server_Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Settings_Server_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ud_Settings_Server_Port.Location = new System.Drawing.Point(91, 57);
            this.ud_Settings_Server_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ud_Settings_Server_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ud_Settings_Server_Port.Name = "ud_Settings_Server_Port";
            this.ud_Settings_Server_Port.Size = new System.Drawing.Size(76, 20);
            this.ud_Settings_Server_Port.TabIndex = 3;
            this.ud_Settings_Server_Port.Value = global::AirScoutViewClient.Properties.Settings.Default.Server_Port;
            // 
            // tb_Settings_Server_URL
            // 
            this.tb_Settings_Server_URL.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutViewClient.Properties.Settings.Default, "Server_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Settings_Server_URL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Settings_Server_URL.Location = new System.Drawing.Point(91, 24);
            this.tb_Settings_Server_URL.Name = "tb_Settings_Server_URL";
            this.tb_Settings_Server_URL.Size = new System.Drawing.Size(138, 20);
            this.tb_Settings_Server_URL.TabIndex = 2;
            this.tb_Settings_Server_URL.Text = global::AirScoutViewClient.Properties.Settings.Default.Server_URL;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server - Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server - IP:";
            // 
            // btn_OK
            // 
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(12, 134);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(266, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // SettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 169);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsDlg";
            this.Text = "AirScout View Client Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Settings_Server_Port)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_Settings_Server_URL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ud_Settings_Server_Port;
        private System.Windows.Forms.Button btn_OK;
    }
}