namespace RainScout.Core
{
    partial class ColorDlg
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
            this.lbl_Color = new System.Windows.Forms.Label();
            this.lbl_Value = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Color
            // 
            this.lbl_Color.BackColor = System.Drawing.Color.DarkGray;
            this.lbl_Color.Location = new System.Drawing.Point(18, 22);
            this.lbl_Color.Name = "lbl_Color";
            this.lbl_Color.Size = new System.Drawing.Size(83, 24);
            this.lbl_Color.TabIndex = 0;
            // 
            // lbl_Value
            // 
            this.lbl_Value.BackColor = System.Drawing.Color.AntiqueWhite;
            this.lbl_Value.Location = new System.Drawing.Point(135, 22);
            this.lbl_Value.Name = "lbl_Value";
            this.lbl_Value.Size = new System.Drawing.Size(87, 24);
            this.lbl_Value.TabIndex = 1;
            this.lbl_Value.Text = "0";
            this.lbl_Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ColorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 94);
            this.Controls.Add(this.lbl_Value);
            this.Controls.Add(this.lbl_Color);
            this.Name = "ColorDlg";
            this.Text = "ColorDlg";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lbl_Color;
        public System.Windows.Forms.Label lbl_Value;
    }
}