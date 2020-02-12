namespace AirScout
{
    partial class DatabaseMaintenanceDlg
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
            this.lbl_Description = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lv_Tables = new System.Windows.Forms.ListView();
            this.ch_TableName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_Entries = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_Action1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.ch_Action2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.ss_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Description
            // 
            this.lbl_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Description.Location = new System.Drawing.Point(18, 157);
            this.lbl_Description.Name = "lbl_Description";
            this.lbl_Description.Size = new System.Drawing.Size(754, 144);
            this.lbl_Description.TabIndex = 5;
            this.lbl_Description.Text = "Database Description";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lv_Tables);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 148);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tables";
            // 
            // lv_Tables
            // 
            this.lv_Tables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_TableName,
            this.ch_Description,
            this.ch_Entries,
            this.ch_Action1,
            this.ch_Action2});
            this.lv_Tables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_Tables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Tables.HideSelection = false;
            this.lv_Tables.Location = new System.Drawing.Point(6, 19);
            this.lv_Tables.Name = "lv_Tables";
            this.lv_Tables.Size = new System.Drawing.Size(748, 114);
            this.lv_Tables.TabIndex = 0;
            this.lv_Tables.UseCompatibleStateImageBehavior = false;
            this.lv_Tables.View = System.Windows.Forms.View.Details;
            // 
            // ch_TableName
            // 
            this.ch_TableName.Text = "TableName";
            this.ch_TableName.Width = 134;
            // 
            // ch_Description
            // 
            this.ch_Description.Text = "Description";
            this.ch_Description.Width = 322;
            // 
            // ch_Entries
            // 
            this.ch_Entries.Text = "Entries";
            this.ch_Entries.Width = 88;
            // 
            // ch_Action1
            // 
            this.ch_Action1.Text = "Action 1";
            this.ch_Action1.Width = 100;
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 339);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(784, 22);
            this.ss_Main.TabIndex = 3;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(39, 17);
            this.tsl_Status.Text = "Status";
            // 
            // ch_Action2
            // 
            this.ch_Action2.Text = "Action 2";
            this.ch_Action2.Width = 100;
            // 
            // DatabaseMaintenanceDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.lbl_Description);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ss_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DatabaseMaintenanceDlg";
            this.Text = "DatabaseMaintenanceDlg";
            this.groupBox1.ResumeLayout(false);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Description;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lv_Tables;
        private System.Windows.Forms.ColumnHeader ch_TableName;
        private System.Windows.Forms.ColumnHeader ch_Description;
        private System.Windows.Forms.ColumnHeader ch_Entries;
        private System.Windows.Forms.ColumnHeader ch_Action1;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.ColumnHeader ch_Action2;
    }
}