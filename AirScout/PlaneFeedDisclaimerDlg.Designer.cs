namespace AirScout
{
    partial class PlaneFeedDisclaimerDlg
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
            this.btn_DeepLinkDlg_Accept = new System.Windows.Forms.Button();
            this.btn_DeepLinkDlg_Decline = new System.Windows.Forms.Button();
            this.tb_DisclaimerText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn_DeepLinkDlg_Accept
            // 
            this.btn_DeepLinkDlg_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_DeepLinkDlg_Accept.Location = new System.Drawing.Point(135, 253);
            this.btn_DeepLinkDlg_Accept.Name = "btn_DeepLinkDlg_Accept";
            this.btn_DeepLinkDlg_Accept.Size = new System.Drawing.Size(75, 23);
            this.btn_DeepLinkDlg_Accept.TabIndex = 12;
            this.btn_DeepLinkDlg_Accept.Text = "&Accept";
            this.btn_DeepLinkDlg_Accept.UseVisualStyleBackColor = true;
            // 
            // btn_DeepLinkDlg_Decline
            // 
            this.btn_DeepLinkDlg_Decline.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_DeepLinkDlg_Decline.Location = new System.Drawing.Point(231, 253);
            this.btn_DeepLinkDlg_Decline.Name = "btn_DeepLinkDlg_Decline";
            this.btn_DeepLinkDlg_Decline.Size = new System.Drawing.Size(75, 23);
            this.btn_DeepLinkDlg_Decline.TabIndex = 13;
            this.btn_DeepLinkDlg_Decline.Text = "&Decline";
            this.btn_DeepLinkDlg_Decline.UseVisualStyleBackColor = true;
            // 
            // tb_DisclaimerText
            // 
            this.tb_DisclaimerText.BackColor = System.Drawing.Color.White;
            this.tb_DisclaimerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DisclaimerText.Location = new System.Drawing.Point(12, 12);
            this.tb_DisclaimerText.Name = "tb_DisclaimerText";
            this.tb_DisclaimerText.ReadOnly = true;
            this.tb_DisclaimerText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.tb_DisclaimerText.ShortcutsEnabled = false;
            this.tb_DisclaimerText.Size = new System.Drawing.Size(454, 210);
            this.tb_DisclaimerText.TabIndex = 14;
            this.tb_DisclaimerText.Text = "";
            // 
            // PlaneFeedDisclaimerDlg
            // 
            this.AcceptButton = this.btn_DeepLinkDlg_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_DeepLinkDlg_Decline;
            this.ClientSize = new System.Drawing.Size(482, 288);
            this.Controls.Add(this.tb_DisclaimerText);
            this.Controls.Add(this.btn_DeepLinkDlg_Decline);
            this.Controls.Add(this.btn_DeepLinkDlg_Accept);
            this.Name = "PlaneFeedDisclaimerDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Use of Deep Links from Internet Server";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DeepLinkDlg_Accept;
        private System.Windows.Forms.Button btn_DeepLinkDlg_Decline;
        public System.Windows.Forms.RichTextBox tb_DisclaimerText;
    }
}