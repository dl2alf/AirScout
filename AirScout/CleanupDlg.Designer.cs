namespace AirScout
{
    partial class CleanupDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanupDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.cb_UserSettings = new System.Windows.Forms.CheckBox();
            this.cb_AirScout_Database = new System.Windows.Forms.CheckBox();
            this.cb_ScoutBase_Database = new System.Windows.Forms.CheckBox();
            this.cb_LogFiles = new System.Windows.Forms.CheckBox();
            this.cb_ElevationFiles = new System.Windows.Forms.CheckBox();
            this.cb_TmpFiles = new System.Windows.Forms.CheckBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.cb_PluginFiles = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(501, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "This will cleanup your AirScout database and user settings.\r\nPlease choose which " +
    "parts should be erased:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_UserSettings
            // 
            this.cb_UserSettings.AutoSize = true;
            this.cb_UserSettings.Checked = true;
            this.cb_UserSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_UserSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_UserSettings.Location = new System.Drawing.Point(42, 87);
            this.cb_UserSettings.Name = "cb_UserSettings";
            this.cb_UserSettings.Size = new System.Drawing.Size(107, 20);
            this.cb_UserSettings.TabIndex = 1;
            this.cb_UserSettings.Text = "User Settings";
            this.cb_UserSettings.UseVisualStyleBackColor = true;
            // 
            // cb_AirScout_Database
            // 
            this.cb_AirScout_Database.AutoSize = true;
            this.cb_AirScout_Database.Checked = true;
            this.cb_AirScout_Database.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_AirScout_Database.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_AirScout_Database.Location = new System.Drawing.Point(42, 110);
            this.cb_AirScout_Database.Name = "cb_AirScout_Database";
            this.cb_AirScout_Database.Size = new System.Drawing.Size(305, 20);
            this.cb_AirScout_Database.TabIndex = 2;
            this.cb_AirScout_Database.Text = "AirScout Database (Aircraft data and positions)";
            this.cb_AirScout_Database.UseVisualStyleBackColor = true;
            // 
            // cb_ScoutBase_Database
            // 
            this.cb_ScoutBase_Database.AutoSize = true;
            this.cb_ScoutBase_Database.Checked = true;
            this.cb_ScoutBase_Database.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_ScoutBase_Database.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ScoutBase_Database.Location = new System.Drawing.Point(42, 133);
            this.cb_ScoutBase_Database.Name = "cb_ScoutBase_Database";
            this.cb_ScoutBase_Database.Size = new System.Drawing.Size(400, 20);
            this.cb_ScoutBase_Database.TabIndex = 3;
            this.cb_ScoutBase_Database.Text = "ScoutBase Database (Elevation, propagation and station data)";
            this.cb_ScoutBase_Database.UseVisualStyleBackColor = true;
            // 
            // cb_LogFiles
            // 
            this.cb_LogFiles.AutoSize = true;
            this.cb_LogFiles.Checked = true;
            this.cb_LogFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_LogFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_LogFiles.Location = new System.Drawing.Point(42, 156);
            this.cb_LogFiles.Name = "cb_LogFiles";
            this.cb_LogFiles.Size = new System.Drawing.Size(107, 20);
            this.cb_LogFiles.TabIndex = 4;
            this.cb_LogFiles.Text = "Log Directory";
            this.cb_LogFiles.UseVisualStyleBackColor = true;
            // 
            // cb_ElevationFiles
            // 
            this.cb_ElevationFiles.AutoSize = true;
            this.cb_ElevationFiles.Checked = true;
            this.cb_ElevationFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_ElevationFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ElevationFiles.Location = new System.Drawing.Point(42, 179);
            this.cb_ElevationFiles.Name = "cb_ElevationFiles";
            this.cb_ElevationFiles.Size = new System.Drawing.Size(229, 20);
            this.cb_ElevationFiles.TabIndex = 5;
            this.cb_ElevationFiles.Text = "Elevation Directory (up to V1.2.0.7)";
            this.cb_ElevationFiles.UseVisualStyleBackColor = true;
            // 
            // cb_TmpFiles
            // 
            this.cb_TmpFiles.AutoSize = true;
            this.cb_TmpFiles.Checked = true;
            this.cb_TmpFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_TmpFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_TmpFiles.Location = new System.Drawing.Point(42, 205);
            this.cb_TmpFiles.Name = "cb_TmpFiles";
            this.cb_TmpFiles.Size = new System.Drawing.Size(112, 20);
            this.cb_TmpFiles.TabIndex = 6;
            this.cb_TmpFiles.Text = "Tmp Directory";
            this.cb_TmpFiles.UseVisualStyleBackColor = true;
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.Location = new System.Drawing.Point(152, 276);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(114, 29);
            this.btn_OK.TabIndex = 7;
            this.btn_OK.Text = "Continue";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(323, 276);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(114, 29);
            this.btn_Cancel.TabIndex = 8;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // cb_PluginFiles
            // 
            this.cb_PluginFiles.AutoSize = true;
            this.cb_PluginFiles.Checked = true;
            this.cb_PluginFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_PluginFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_PluginFiles.Location = new System.Drawing.Point(42, 231);
            this.cb_PluginFiles.Name = "cb_PluginFiles";
            this.cb_PluginFiles.Size = new System.Drawing.Size(121, 20);
            this.cb_PluginFiles.TabIndex = 9;
            this.cb_PluginFiles.Text = "Plugin Directory";
            this.cb_PluginFiles.UseVisualStyleBackColor = true;
            // 
            // CleanupDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(624, 317);
            this.Controls.Add(this.cb_PluginFiles);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.cb_TmpFiles);
            this.Controls.Add(this.cb_ElevationFiles);
            this.Controls.Add(this.cb_LogFiles);
            this.Controls.Add(this.cb_ScoutBase_Database);
            this.Controls.Add(this.cb_AirScout_Database);
            this.Controls.Add(this.cb_UserSettings);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CleanupDlg";
            this.Text = "AirScout Cleanup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CleanupDlg_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_UserSettings;
        private System.Windows.Forms.CheckBox cb_AirScout_Database;
        private System.Windows.Forms.CheckBox cb_ScoutBase_Database;
        private System.Windows.Forms.CheckBox cb_LogFiles;
        private System.Windows.Forms.CheckBox cb_ElevationFiles;
        private System.Windows.Forms.CheckBox cb_TmpFiles;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.CheckBox cb_PluginFiles;
    }
}