namespace AirScout
{
    partial class DatabaseEditDlg
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
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.pa_Buttons = new System.Windows.Forms.Panel();
            this.dgv_Main = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Search = new System.Windows.Forms.TextBox();
            this.btn_FindNext = new System.Windows.Forms.Button();
            this.ss_Main.SuspendLayout();
            this.pa_Buttons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 339);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(1008, 22);
            this.ss_Main.TabIndex = 0;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(39, 17);
            this.tsl_Status.Text = "Status";
            // 
            // pa_Buttons
            // 
            this.pa_Buttons.Controls.Add(this.btn_FindNext);
            this.pa_Buttons.Controls.Add(this.tb_Search);
            this.pa_Buttons.Controls.Add(this.label1);
            this.pa_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pa_Buttons.Location = new System.Drawing.Point(0, 285);
            this.pa_Buttons.Name = "pa_Buttons";
            this.pa_Buttons.Size = new System.Drawing.Size(1008, 54);
            this.pa_Buttons.TabIndex = 1;
            // 
            // dgv_Main
            // 
            this.dgv_Main.AllowUserToAddRows = false;
            this.dgv_Main.AllowUserToDeleteRows = false;
            this.dgv_Main.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Main.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv_Main.Location = new System.Drawing.Point(0, 0);
            this.dgv_Main.Name = "dgv_Main";
            this.dgv_Main.Size = new System.Drawing.Size(1008, 285);
            this.dgv_Main.TabIndex = 2;
            this.dgv_Main.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgv_Main_CellValidating);
            this.dgv_Main.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_Main_DataError);
            this.dgv_Main.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgv_Main_RowsAdded);
            this.dgv_Main.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgv_Main_RowValidating);
            this.dgv_Main.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_Main_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find in any column:";
            // 
            // tb_Search
            // 
            this.tb_Search.Location = new System.Drawing.Point(116, 15);
            this.tb_Search.Name = "tb_Search";
            this.tb_Search.Size = new System.Drawing.Size(143, 20);
            this.tb_Search.TabIndex = 1;
            this.tb_Search.TextChanged += new System.EventHandler(this.tb_Search_TextChanged);
            this.tb_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_Search_KeyDown);
            // 
            // btn_FindNext
            // 
            this.btn_FindNext.Location = new System.Drawing.Point(275, 13);
            this.btn_FindNext.Name = "btn_FindNext";
            this.btn_FindNext.Size = new System.Drawing.Size(75, 23);
            this.btn_FindNext.TabIndex = 2;
            this.btn_FindNext.Text = "Find next (F3)";
            this.btn_FindNext.UseVisualStyleBackColor = true;
            this.btn_FindNext.Click += new System.EventHandler(this.btn_FindNext_Click);
            this.btn_FindNext.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_FindNext_KeyDown);
            // 
            // DatabaseEditDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 361);
            this.Controls.Add(this.dgv_Main);
            this.Controls.Add(this.pa_Buttons);
            this.Controls.Add(this.ss_Main);
            this.Name = "DatabaseEditDlg";
            this.Text = "Edit Database";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DatabaseEditDlg_KeyDown);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.pa_Buttons.ResumeLayout(false);
            this.pa_Buttons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.Panel pa_Buttons;
        private System.Windows.Forms.DataGridView dgv_Main;
        private System.Windows.Forms.TextBox tb_Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_FindNext;
    }
}