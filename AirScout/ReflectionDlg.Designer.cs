namespace AirScout
{
    partial class ReflectionDlg
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
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Reflection_UTC = new System.Windows.Forms.TextBox();
            this.tb_Reflection_Call = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Reflection_Lat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Reflection_Lon = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Reflection_Alt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Reflection_Yes = new System.Windows.Forms.Button();
            this.btn_Reflection_No = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "UTC:";
            // 
            // tb_Reflection_UTC
            // 
            this.tb_Reflection_UTC.Location = new System.Drawing.Point(43, 9);
            this.tb_Reflection_UTC.Name = "tb_Reflection_UTC";
            this.tb_Reflection_UTC.Size = new System.Drawing.Size(115, 20);
            this.tb_Reflection_UTC.TabIndex = 5;
            // 
            // tb_Reflection_Call
            // 
            this.tb_Reflection_Call.Location = new System.Drawing.Point(43, 35);
            this.tb_Reflection_Call.Name = "tb_Reflection_Call";
            this.tb_Reflection_Call.Size = new System.Drawing.Size(115, 20);
            this.tb_Reflection_Call.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Call:";
            // 
            // tb_Reflection_Lat
            // 
            this.tb_Reflection_Lat.Location = new System.Drawing.Point(43, 61);
            this.tb_Reflection_Lat.Name = "tb_Reflection_Lat";
            this.tb_Reflection_Lat.Size = new System.Drawing.Size(115, 20);
            this.tb_Reflection_Lat.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Lat:";
            // 
            // tb_Reflection_Lon
            // 
            this.tb_Reflection_Lon.Location = new System.Drawing.Point(43, 87);
            this.tb_Reflection_Lon.Name = "tb_Reflection_Lon";
            this.tb_Reflection_Lon.Size = new System.Drawing.Size(115, 20);
            this.tb_Reflection_Lon.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Lon:";
            // 
            // tb_Reflection_Alt
            // 
            this.tb_Reflection_Alt.Location = new System.Drawing.Point(43, 113);
            this.tb_Reflection_Alt.Name = "tb_Reflection_Alt";
            this.tb_Reflection_Alt.Size = new System.Drawing.Size(115, 20);
            this.tb_Reflection_Alt.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Alt:";
            // 
            // btn_Reflection_Yes
            // 
            this.btn_Reflection_Yes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btn_Reflection_Yes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Reflection_Yes.Location = new System.Drawing.Point(43, 151);
            this.btn_Reflection_Yes.Name = "btn_Reflection_Yes";
            this.btn_Reflection_Yes.Size = new System.Drawing.Size(54, 42);
            this.btn_Reflection_Yes.TabIndex = 14;
            this.btn_Reflection_Yes.Text = "&Yes";
            this.btn_Reflection_Yes.UseVisualStyleBackColor = true;
            // 
            // btn_Reflection_No
            // 
            this.btn_Reflection_No.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btn_Reflection_No.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Reflection_No.Location = new System.Drawing.Point(104, 151);
            this.btn_Reflection_No.Name = "btn_Reflection_No";
            this.btn_Reflection_No.Size = new System.Drawing.Size(54, 42);
            this.btn_Reflection_No.TabIndex = 15;
            this.btn_Reflection_No.Text = "&No";
            this.btn_Reflection_No.UseVisualStyleBackColor = true;
            // 
            // ReflectionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 205);
            this.Controls.Add(this.btn_Reflection_No);
            this.Controls.Add(this.btn_Reflection_Yes);
            this.Controls.Add(this.tb_Reflection_Alt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_Reflection_Lon);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_Reflection_Lat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Reflection_Call);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Reflection_UTC);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ReflectionDlg";
            this.Text = "Log Reflection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Reflection_Yes;
        private System.Windows.Forms.Button btn_Reflection_No;
        public System.Windows.Forms.TextBox tb_Reflection_UTC;
        public System.Windows.Forms.TextBox tb_Reflection_Call;
        public System.Windows.Forms.TextBox tb_Reflection_Lat;
        public System.Windows.Forms.TextBox tb_Reflection_Lon;
        public System.Windows.Forms.TextBox tb_Reflection_Alt;
    }
}