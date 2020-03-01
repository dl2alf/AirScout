namespace AirScout
{
    partial class WatchlistDlg
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ss_Watchlist_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Watchlist_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_Watchlist_Add = new System.Windows.Forms.Button();
            this.btn_Watchlist_Remove = new System.Windows.Forms.Button();
            this.btn_Watchlist_RemoveAll = new System.Windows.Forms.Button();
            this.tt_Watchlist_Main = new System.Windows.Forms.ToolTip(this.components);
            this.btn_Watchlist_Cancel = new System.Windows.Forms.Button();
            this.btn_Watchlist_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Watchlist_Callsigns = new System.Windows.Forms.TextBox();
            this.tb_Watchlist_Selected = new System.Windows.Forms.TextBox();
            this.bw_Watchlist_Fill = new System.ComponentModel.BackgroundWorker();
            this.dgv_Watchlist_Callsigns = new System.Windows.Forms.DataGridView();
            this.dgv_Watchlist_Selected = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ss_Watchlist_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Watchlist_Callsigns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Watchlist_Selected)).BeginInit();
            this.SuspendLayout();
            // 
            // ss_Watchlist_Main
            // 
            this.ss_Watchlist_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Watchlist_Main});
            this.ss_Watchlist_Main.Location = new System.Drawing.Point(0, 435);
            this.ss_Watchlist_Main.Name = "ss_Watchlist_Main";
            this.ss_Watchlist_Main.Size = new System.Drawing.Size(484, 22);
            this.ss_Watchlist_Main.TabIndex = 4;
            // 
            // tsl_Watchlist_Main
            // 
            this.tsl_Watchlist_Main.Name = "tsl_Watchlist_Main";
            this.tsl_Watchlist_Main.Size = new System.Drawing.Size(51, 17);
            this.tsl_Watchlist_Main.Text = "tsl_Main";
            // 
            // btn_Watchlist_Add
            // 
            this.btn_Watchlist_Add.Enabled = false;
            this.btn_Watchlist_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Watchlist_Add.Location = new System.Drawing.Point(207, 176);
            this.btn_Watchlist_Add.Name = "btn_Watchlist_Add";
            this.btn_Watchlist_Add.Size = new System.Drawing.Size(70, 30);
            this.btn_Watchlist_Add.TabIndex = 5;
            this.btn_Watchlist_Add.Text = ">";
            this.tt_Watchlist_Main.SetToolTip(this.btn_Watchlist_Add, "Add selected call to watchlist.");
            this.btn_Watchlist_Add.UseVisualStyleBackColor = true;
            this.btn_Watchlist_Add.Click += new System.EventHandler(this.btn_Watchlist_Add_Click);
            // 
            // btn_Watchlist_Remove
            // 
            this.btn_Watchlist_Remove.Enabled = false;
            this.btn_Watchlist_Remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Watchlist_Remove.Location = new System.Drawing.Point(207, 226);
            this.btn_Watchlist_Remove.Name = "btn_Watchlist_Remove";
            this.btn_Watchlist_Remove.Size = new System.Drawing.Size(70, 30);
            this.btn_Watchlist_Remove.TabIndex = 6;
            this.btn_Watchlist_Remove.Text = "<";
            this.tt_Watchlist_Main.SetToolTip(this.btn_Watchlist_Remove, "Remove selected call from watchlist.");
            this.btn_Watchlist_Remove.UseVisualStyleBackColor = true;
            this.btn_Watchlist_Remove.Click += new System.EventHandler(this.btn_Watchlist_Remove_Click);
            // 
            // btn_Watchlist_RemoveAll
            // 
            this.btn_Watchlist_RemoveAll.Enabled = false;
            this.btn_Watchlist_RemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Watchlist_RemoveAll.Location = new System.Drawing.Point(207, 274);
            this.btn_Watchlist_RemoveAll.Name = "btn_Watchlist_RemoveAll";
            this.btn_Watchlist_RemoveAll.Size = new System.Drawing.Size(70, 30);
            this.btn_Watchlist_RemoveAll.TabIndex = 7;
            this.btn_Watchlist_RemoveAll.Text = "<<";
            this.tt_Watchlist_Main.SetToolTip(this.btn_Watchlist_RemoveAll, "Remove all calls from watchlist.");
            this.btn_Watchlist_RemoveAll.UseVisualStyleBackColor = true;
            this.btn_Watchlist_RemoveAll.Click += new System.EventHandler(this.btn_Watchlist_RemoveAll_Click);
            // 
            // btn_Watchlist_Cancel
            // 
            this.btn_Watchlist_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Watchlist_Cancel.Location = new System.Drawing.Point(147, 402);
            this.btn_Watchlist_Cancel.Name = "btn_Watchlist_Cancel";
            this.btn_Watchlist_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Watchlist_Cancel.TabIndex = 8;
            this.btn_Watchlist_Cancel.Text = "Cancel";
            this.btn_Watchlist_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_Watchlist_OK
            // 
            this.btn_Watchlist_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Watchlist_OK.Enabled = false;
            this.btn_Watchlist_OK.Location = new System.Drawing.Point(237, 402);
            this.btn_Watchlist_OK.Name = "btn_Watchlist_OK";
            this.btn_Watchlist_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_Watchlist_OK.TabIndex = 9;
            this.btn_Watchlist_OK.Text = "OK";
            this.btn_Watchlist_OK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "You can select callsigns from database and set them on a watchlist.";
            // 
            // tb_Watchlist_Callsigns
            // 
            this.tb_Watchlist_Callsigns.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Watchlist_Callsigns.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Watchlist_Callsigns.Location = new System.Drawing.Point(15, 75);
            this.tb_Watchlist_Callsigns.Name = "tb_Watchlist_Callsigns";
            this.tb_Watchlist_Callsigns.Size = new System.Drawing.Size(180, 20);
            this.tb_Watchlist_Callsigns.TabIndex = 11;
            this.tb_Watchlist_Callsigns.TextChanged += new System.EventHandler(this.tb_Watchlist_Callsigns_TextChanged);
            // 
            // tb_Watchlist_Selected
            // 
            this.tb_Watchlist_Selected.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_Watchlist_Selected.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Watchlist_Selected.Location = new System.Drawing.Point(288, 75);
            this.tb_Watchlist_Selected.Name = "tb_Watchlist_Selected";
            this.tb_Watchlist_Selected.Size = new System.Drawing.Size(180, 20);
            this.tb_Watchlist_Selected.TabIndex = 12;
            this.tb_Watchlist_Selected.TextChanged += new System.EventHandler(this.tb_Watchlist_Selected_TextChanged);
            // 
            // bw_Watchlist_Fill
            // 
            this.bw_Watchlist_Fill.WorkerReportsProgress = true;
            this.bw_Watchlist_Fill.WorkerSupportsCancellation = true;
            this.bw_Watchlist_Fill.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_Watchlist_Fill_DoWork);
            this.bw_Watchlist_Fill.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_Watchlist_Fill_ProgressChanged);
            this.bw_Watchlist_Fill.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_Watchlist_Fill_RunWorkerCompleted);
            // 
            // dgv_Watchlist_Callsigns
            // 
            this.dgv_Watchlist_Callsigns.AllowUserToAddRows = false;
            this.dgv_Watchlist_Callsigns.AllowUserToDeleteRows = false;
            this.dgv_Watchlist_Callsigns.AllowUserToResizeColumns = false;
            this.dgv_Watchlist_Callsigns.AllowUserToResizeRows = false;
            this.dgv_Watchlist_Callsigns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Watchlist_Callsigns.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_Watchlist_Callsigns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Watchlist_Callsigns.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_Watchlist_Callsigns.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv_Watchlist_Callsigns.Location = new System.Drawing.Point(15, 101);
            this.dgv_Watchlist_Callsigns.Name = "dgv_Watchlist_Callsigns";
            this.dgv_Watchlist_Callsigns.RowHeadersVisible = false;
            this.dgv_Watchlist_Callsigns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Watchlist_Callsigns.Size = new System.Drawing.Size(180, 280);
            this.dgv_Watchlist_Callsigns.TabIndex = 13;
            this.dgv_Watchlist_Callsigns.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Watchlist_Callsigns_CellContentDoubleClick);
            this.dgv_Watchlist_Callsigns.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv_Watchlist_Callsigns_CellPainting);
            // 
            // dgv_Watchlist_Selected
            // 
            this.dgv_Watchlist_Selected.AllowUserToAddRows = false;
            this.dgv_Watchlist_Selected.AllowUserToDeleteRows = false;
            this.dgv_Watchlist_Selected.AllowUserToResizeColumns = false;
            this.dgv_Watchlist_Selected.AllowUserToResizeRows = false;
            this.dgv_Watchlist_Selected.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Watchlist_Selected.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv_Watchlist_Selected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Watchlist_Selected.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_Watchlist_Selected.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv_Watchlist_Selected.Location = new System.Drawing.Point(288, 101);
            this.dgv_Watchlist_Selected.Name = "dgv_Watchlist_Selected";
            this.dgv_Watchlist_Selected.RowHeadersVisible = false;
            this.dgv_Watchlist_Selected.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Watchlist_Selected.Size = new System.Drawing.Size(180, 280);
            this.dgv_Watchlist_Selected.TabIndex = 14;
            this.dgv_Watchlist_Selected.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Watchlist_Selected_CellContentDoubleClick);
            this.dgv_Watchlist_Selected.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv_Watchlist_Selected_CellPainting);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Khaki;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 25);
            this.label2.TabIndex = 15;
            this.label2.Text = "Availbale Callsigns";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.PaleGreen;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(288, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 25);
            this.label3.TabIndex = 16;
            this.label3.Text = "Selected Callsigns";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WatchlistDlg
            // 
            this.AcceptButton = this.btn_Watchlist_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Watchlist_Cancel;
            this.ClientSize = new System.Drawing.Size(484, 457);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgv_Watchlist_Selected);
            this.Controls.Add(this.dgv_Watchlist_Callsigns);
            this.Controls.Add(this.tb_Watchlist_Selected);
            this.Controls.Add(this.tb_Watchlist_Callsigns);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Watchlist_OK);
            this.Controls.Add(this.btn_Watchlist_Cancel);
            this.Controls.Add(this.btn_Watchlist_RemoveAll);
            this.Controls.Add(this.btn_Watchlist_Remove);
            this.Controls.Add(this.btn_Watchlist_Add);
            this.Controls.Add(this.ss_Watchlist_Main);
            this.Name = "WatchlistDlg";
            this.Text = "Manage Watchlist";
            this.Load += new System.EventHandler(this.WatchlistDlg_Load);
            this.Shown += new System.EventHandler(this.WatchlistDlg_Shown);
            this.ss_Watchlist_Main.ResumeLayout(false);
            this.ss_Watchlist_Main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Watchlist_Callsigns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Watchlist_Selected)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip ss_Watchlist_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Watchlist_Main;
        private System.Windows.Forms.Button btn_Watchlist_Add;
        private System.Windows.Forms.Button btn_Watchlist_Remove;
        private System.Windows.Forms.Button btn_Watchlist_RemoveAll;
        private System.Windows.Forms.ToolTip tt_Watchlist_Main;
        private System.Windows.Forms.Button btn_Watchlist_Cancel;
        private System.Windows.Forms.Button btn_Watchlist_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Watchlist_Callsigns;
        private System.Windows.Forms.TextBox tb_Watchlist_Selected;
        private System.ComponentModel.BackgroundWorker bw_Watchlist_Fill;
        private System.Windows.Forms.DataGridView dgv_Watchlist_Callsigns;
        public System.Windows.Forms.DataGridView dgv_Watchlist_Selected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}