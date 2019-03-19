namespace AirScout
{
    partial class PlaneFeedSettingsDlg
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
            this.pg_Main = new System.Windows.Forms.PropertyGrid();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pg_Main
            // 
            this.pg_Main.Location = new System.Drawing.Point(12, 15);
            this.pg_Main.Name = "pg_Main";
            this.pg_Main.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pg_Main.Size = new System.Drawing.Size(413, 253);
            this.pg_Main.TabIndex = 0;
            // 
            // lbl_Info
            // 
            this.lbl_Info.Location = new System.Drawing.Point(431, 41);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(176, 227);
            this.lbl_Info.TabIndex = 1;
            this.lbl_Info.Text = "PlaneFeed Info";
            // 
            // PlaneFeedSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 277);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.pg_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PlaneFeedSettingsDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PlaneFeedSettingsDlg";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PropertyGrid pg_Main;
        public System.Windows.Forms.Label lbl_Info;
    }
}