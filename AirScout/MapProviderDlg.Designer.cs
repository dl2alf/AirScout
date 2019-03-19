namespace AirScout
{
    partial class MapProviderDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapProviderDlg));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_MapProviderDlg_Decline = new System.Windows.Forms.Button();
            this.btn_MapProviderDlg_Accept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.ShortcutsEnabled = false;
            this.richTextBox1.Size = new System.Drawing.Size(454, 210);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // btn_MapProviderDlg_Decline
            // 
            this.btn_MapProviderDlg_Decline.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_MapProviderDlg_Decline.Location = new System.Drawing.Point(246, 258);
            this.btn_MapProviderDlg_Decline.Name = "btn_MapProviderDlg_Decline";
            this.btn_MapProviderDlg_Decline.Size = new System.Drawing.Size(75, 23);
            this.btn_MapProviderDlg_Decline.TabIndex = 17;
            this.btn_MapProviderDlg_Decline.Text = "&Decline";
            this.btn_MapProviderDlg_Decline.UseVisualStyleBackColor = true;
            // 
            // btn_MapProviderDlg_Accept
            // 
            this.btn_MapProviderDlg_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_MapProviderDlg_Accept.Location = new System.Drawing.Point(148, 258);
            this.btn_MapProviderDlg_Accept.Name = "btn_MapProviderDlg_Accept";
            this.btn_MapProviderDlg_Accept.Size = new System.Drawing.Size(75, 23);
            this.btn_MapProviderDlg_Accept.TabIndex = 16;
            this.btn_MapProviderDlg_Accept.Text = "&Accept";
            this.btn_MapProviderDlg_Accept.UseVisualStyleBackColor = true;
            // 
            // MapProviderDlg
            // 
            this.AcceptButton = this.btn_MapProviderDlg_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_MapProviderDlg_Decline;
            this.ClientSize = new System.Drawing.Size(478, 293);
            this.Controls.Add(this.btn_MapProviderDlg_Decline);
            this.Controls.Add(this.btn_MapProviderDlg_Accept);
            this.Controls.Add(this.richTextBox1);
            this.Name = "MapProviderDlg";
            this.Text = "Map Provider - Term of Use";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btn_MapProviderDlg_Decline;
        private System.Windows.Forms.Button btn_MapProviderDlg_Accept;
    }
}