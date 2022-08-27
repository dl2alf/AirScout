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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Server_URL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ud_Server_Port = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Server_Port)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server-URL:";
            // 
            // tb_Server_URL
            // 
            this.tb_Server_URL.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AirScoutViewClient.Properties.Settings.Default, "Server_URL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Server_URL.Location = new System.Drawing.Point(84, 20);
            this.tb_Server_URL.Name = "tb_Server_URL";
            this.tb_Server_URL.Size = new System.Drawing.Size(208, 20);
            this.tb_Server_URL.TabIndex = 1;
            this.tb_Server_URL.Text = global::AirScoutViewClient.Properties.Settings.Default.Server_URL;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Server-Port:";
            // 
            // ud_Server_Port
            // 
            this.ud_Server_Port.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScoutViewClient.Properties.Settings.Default, "Server_Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Server_Port.Location = new System.Drawing.Point(84, 46);
            this.ud_Server_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ud_Server_Port.Name = "ud_Server_Port";
            this.ud_Server_Port.Size = new System.Drawing.Size(79, 20);
            this.ud_Server_Port.TabIndex = 5;
            this.ud_Server_Port.Value = global::AirScoutViewClient.Properties.Settings.Default.Server_Port;
            // 
            // SettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 83);
            this.Controls.Add(this.ud_Server_Port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Server_URL);
            this.Controls.Add(this.label1);
            this.Name = "SettingsDlg";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.ud_Server_Port)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Server_URL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ud_Server_Port;
    }
}