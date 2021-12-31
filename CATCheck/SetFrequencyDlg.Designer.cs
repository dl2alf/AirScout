namespace CATCheck
{
    partial class SetFrequencyDlg
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
            this.btn_Set = new System.Windows.Forms.Button();
            this.ud_Frequency = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Frequency)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Set
            // 
            this.btn_Set.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Set.Location = new System.Drawing.Point(214, 12);
            this.btn_Set.Name = "btn_Set";
            this.btn_Set.Size = new System.Drawing.Size(75, 26);
            this.btn_Set.TabIndex = 0;
            this.btn_Set.Text = "Set";
            this.btn_Set.UseVisualStyleBackColor = true;
            // 
            // ud_Frequency
            // 
            this.ud_Frequency.BackColor = System.Drawing.Color.Gray;
            this.ud_Frequency.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ud_Frequency.ForeColor = System.Drawing.Color.Chartreuse;
            this.ud_Frequency.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ud_Frequency.Location = new System.Drawing.Point(12, 12);
            this.ud_Frequency.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.ud_Frequency.Name = "ud_Frequency";
            this.ud_Frequency.Size = new System.Drawing.Size(192, 26);
            this.ud_Frequency.TabIndex = 1;
            this.ud_Frequency.ThousandsSeparator = true;
            // 
            // SetFrequencyDlg
            // 
            this.AcceptButton = this.btn_Set;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 53);
            this.Controls.Add(this.ud_Frequency);
            this.Controls.Add(this.btn_Set);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetFrequencyDlg";
            this.Text = "Set Frequency";
            ((System.ComponentModel.ISupportInitialize)(this.ud_Frequency)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Set;
        public System.Windows.Forms.NumericUpDown ud_Frequency;
    }
}