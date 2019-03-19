namespace AirScout
{
    partial class LicenseDlg
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
            this.rtb_License = new System.Windows.Forms.RichTextBox();
            this.btn_Options_License_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtb_License
            // 
            this.rtb_License.BackColor = System.Drawing.Color.White;
            this.rtb_License.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_License.Location = new System.Drawing.Point(12, 12);
            this.rtb_License.Name = "rtb_License";
            this.rtb_License.ReadOnly = true;
            this.rtb_License.Size = new System.Drawing.Size(599, 659);
            this.rtb_License.TabIndex = 0;
            this.rtb_License.Text = "";
            // 
            // btn_Options_License_OK
            // 
            this.btn_Options_License_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Options_License_OK.Location = new System.Drawing.Point(266, 687);
            this.btn_Options_License_OK.Name = "btn_Options_License_OK";
            this.btn_Options_License_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_Options_License_OK.TabIndex = 1;
            this.btn_Options_License_OK.Text = "Close";
            this.btn_Options_License_OK.UseVisualStyleBackColor = true;
            // 
            // LicenseDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 722);
            this.Controls.Add(this.btn_Options_License_OK);
            this.Controls.Add(this.rtb_License);
            this.Name = "LicenseDlg";
            this.Text = "LicenseDlg";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_License;
        private System.Windows.Forms.Button btn_Options_License_OK;
    }
}