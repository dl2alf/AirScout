namespace WinTestSimulator
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Send = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_To = new System.Windows.Forms.TextBox();
            this.ud_Port = new System.Windows.Forms.NumericUpDown();
            this.tb_From = new System.Windows.Forms.TextBox();
            this.tb_Message = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Port)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Message:";
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(498, 86);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(243, 23);
            this.btn_Send.TabIndex = 2;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "From:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "To:";
            // 
            // tb_To
            // 
            this.tb_To.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::WinTestSimulator.Properties.Settings.Default, "To", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_To.Location = new System.Drawing.Point(363, 88);
            this.tb_To.Name = "tb_To";
            this.tb_To.Size = new System.Drawing.Size(100, 20);
            this.tb_To.TabIndex = 7;
            this.tb_To.Text = global::WinTestSimulator.Properties.Settings.Default.To;
            // 
            // ud_Port
            // 
            this.ud_Port.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::WinTestSimulator.Properties.Settings.Default, "Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Port.Location = new System.Drawing.Point(71, 89);
            this.ud_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ud_Port.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ud_Port.Name = "ud_Port";
            this.ud_Port.Size = new System.Drawing.Size(61, 20);
            this.ud_Port.TabIndex = 4;
            this.ud_Port.Value = global::WinTestSimulator.Properties.Settings.Default.Port;
            // 
            // tb_From
            // 
            this.tb_From.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::WinTestSimulator.Properties.Settings.Default, "From", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_From.Location = new System.Drawing.Point(215, 89);
            this.tb_From.Name = "tb_From";
            this.tb_From.Size = new System.Drawing.Size(75, 20);
            this.tb_From.TabIndex = 3;
            this.tb_From.Text = global::WinTestSimulator.Properties.Settings.Default.From;
            // 
            // tb_Message
            // 
            this.tb_Message.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::WinTestSimulator.Properties.Settings.Default, "Message", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Message.Location = new System.Drawing.Point(71, 38);
            this.tb_Message.Name = "tb_Message";
            this.tb_Message.Size = new System.Drawing.Size(107, 20);
            this.tb_Message.TabIndex = 0;
            this.tb_Message.Text = global::WinTestSimulator.Properties.Settings.Default.Message;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Data:";
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::WinTestSimulator.Properties.Settings.Default, "Data", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(255, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(486, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = global::WinTestSimulator.Properties.Settings.Default.Data;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 146);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_To);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ud_Port);
            this.Controls.Add(this.tb_From);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Message);
            this.Name = "MainDlg";
            this.Text = "WinTest Message Simulator";
            ((System.ComponentModel.ISupportInitialize)(this.ud_Port)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Message;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox tb_From;
        private System.Windows.Forms.NumericUpDown ud_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_To;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
    }
}

