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
            this.lv_Watchlist_Callsigns = new System.Windows.Forms.ListView();
            this.ch_Watchlist_Callsgings_Call = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Watchlist_Selected = new System.Windows.Forms.ListView();
            this.ch_Watchlist_Selected_Calls = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ss_Watchlist_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Watchlist_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_Watchlist_Add = new System.Windows.Forms.Button();
            this.btn_Watchlist_Remove = new System.Windows.Forms.Button();
            this.btn_Watchlist_RemoveAll = new System.Windows.Forms.Button();
            this.tt_Watchlist_Main = new System.Windows.Forms.ToolTip(this.components);
            this.btn_Watchlist_Cancel = new System.Windows.Forms.Button();
            this.btn_Watchlist_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ss_Watchlist_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv_Watchlist_Callsigns
            // 
            this.lv_Watchlist_Callsigns.AutoArrange = false;
            this.lv_Watchlist_Callsigns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_Watchlist_Callsgings_Call});
            this.lv_Watchlist_Callsigns.FullRowSelect = true;
            this.lv_Watchlist_Callsigns.Location = new System.Drawing.Point(12, 50);
            this.lv_Watchlist_Callsigns.Name = "lv_Watchlist_Callsigns";
            this.lv_Watchlist_Callsigns.ShowGroups = false;
            this.lv_Watchlist_Callsigns.Size = new System.Drawing.Size(140, 300);
            this.lv_Watchlist_Callsigns.TabIndex = 2;
            this.lv_Watchlist_Callsigns.UseCompatibleStateImageBehavior = false;
            this.lv_Watchlist_Callsigns.View = System.Windows.Forms.View.Details;
            this.lv_Watchlist_Callsigns.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_Watchlist_Callsigns_MouseDoubleClick);
            // 
            // ch_Watchlist_Callsgings_Call
            // 
            this.ch_Watchlist_Callsgings_Call.Text = "Available Callsigns";
            this.ch_Watchlist_Callsgings_Call.Width = 118;
            // 
            // lv_Watchlist_Selected
            // 
            this.lv_Watchlist_Selected.AutoArrange = false;
            this.lv_Watchlist_Selected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_Watchlist_Selected_Calls});
            this.lv_Watchlist_Selected.FullRowSelect = true;
            this.lv_Watchlist_Selected.Location = new System.Drawing.Point(318, 50);
            this.lv_Watchlist_Selected.Name = "lv_Watchlist_Selected";
            this.lv_Watchlist_Selected.ShowGroups = false;
            this.lv_Watchlist_Selected.Size = new System.Drawing.Size(140, 300);
            this.lv_Watchlist_Selected.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv_Watchlist_Selected.TabIndex = 3;
            this.lv_Watchlist_Selected.UseCompatibleStateImageBehavior = false;
            this.lv_Watchlist_Selected.View = System.Windows.Forms.View.Details;
            this.lv_Watchlist_Selected.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_Watchlist_Selected_MouseDoubleClick);
            // 
            // ch_Watchlist_Selected_Calls
            // 
            this.ch_Watchlist_Selected_Calls.Text = "Selected Callsigns";
            this.ch_Watchlist_Selected_Calls.Width = 118;
            // 
            // ss_Watchlist_Main
            // 
            this.ss_Watchlist_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Watchlist_Main});
            this.ss_Watchlist_Main.Location = new System.Drawing.Point(0, 400);
            this.ss_Watchlist_Main.Name = "ss_Watchlist_Main";
            this.ss_Watchlist_Main.Size = new System.Drawing.Size(487, 22);
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
            this.btn_Watchlist_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Watchlist_Add.Location = new System.Drawing.Point(201, 131);
            this.btn_Watchlist_Add.Name = "btn_Watchlist_Add";
            this.btn_Watchlist_Add.Size = new System.Drawing.Size(69, 29);
            this.btn_Watchlist_Add.TabIndex = 5;
            this.btn_Watchlist_Add.Text = ">";
            this.tt_Watchlist_Main.SetToolTip(this.btn_Watchlist_Add, "Add selected call to watchlist.");
            this.btn_Watchlist_Add.UseVisualStyleBackColor = true;
            this.btn_Watchlist_Add.Click += new System.EventHandler(this.btn_Watchlist_Add_Click);
            // 
            // btn_Watchlist_Remove
            // 
            this.btn_Watchlist_Remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Watchlist_Remove.Location = new System.Drawing.Point(201, 181);
            this.btn_Watchlist_Remove.Name = "btn_Watchlist_Remove";
            this.btn_Watchlist_Remove.Size = new System.Drawing.Size(69, 29);
            this.btn_Watchlist_Remove.TabIndex = 6;
            this.btn_Watchlist_Remove.Text = "<";
            this.tt_Watchlist_Main.SetToolTip(this.btn_Watchlist_Remove, "Remove selected call from watchlist.");
            this.btn_Watchlist_Remove.UseVisualStyleBackColor = true;
            this.btn_Watchlist_Remove.Click += new System.EventHandler(this.btn_Watchlist_Remove_Click);
            // 
            // btn_Watchlist_RemoveAll
            // 
            this.btn_Watchlist_RemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Watchlist_RemoveAll.Location = new System.Drawing.Point(201, 229);
            this.btn_Watchlist_RemoveAll.Name = "btn_Watchlist_RemoveAll";
            this.btn_Watchlist_RemoveAll.Size = new System.Drawing.Size(69, 29);
            this.btn_Watchlist_RemoveAll.TabIndex = 7;
            this.btn_Watchlist_RemoveAll.Text = "<<";
            this.tt_Watchlist_Main.SetToolTip(this.btn_Watchlist_RemoveAll, "Remove all calls from watchlist.");
            this.btn_Watchlist_RemoveAll.UseVisualStyleBackColor = true;
            this.btn_Watchlist_RemoveAll.Click += new System.EventHandler(this.btn_Watchlist_RemoveAll_Click);
            // 
            // btn_Watchlist_Cancel
            // 
            this.btn_Watchlist_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Watchlist_Cancel.Location = new System.Drawing.Point(161, 365);
            this.btn_Watchlist_Cancel.Name = "btn_Watchlist_Cancel";
            this.btn_Watchlist_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Watchlist_Cancel.TabIndex = 8;
            this.btn_Watchlist_Cancel.Text = "Cancel";
            this.btn_Watchlist_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_Watchlist_OK
            // 
            this.btn_Watchlist_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Watchlist_OK.Location = new System.Drawing.Point(242, 365);
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
            this.label1.Location = new System.Drawing.Point(43, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "You can select callsigns from database and set them on a watchlist.";
            // 
            // WatchlistDlg
            // 
            this.AcceptButton = this.btn_Watchlist_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Watchlist_Cancel;
            this.ClientSize = new System.Drawing.Size(487, 422);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Watchlist_OK);
            this.Controls.Add(this.btn_Watchlist_Cancel);
            this.Controls.Add(this.btn_Watchlist_RemoveAll);
            this.Controls.Add(this.btn_Watchlist_Remove);
            this.Controls.Add(this.btn_Watchlist_Add);
            this.Controls.Add(this.ss_Watchlist_Main);
            this.Controls.Add(this.lv_Watchlist_Selected);
            this.Controls.Add(this.lv_Watchlist_Callsigns);
            this.Name = "WatchlistDlg";
            this.Text = "Manage Watchlist";
            this.Load += new System.EventHandler(this.WatchlistDlg_Load);
            this.Shown += new System.EventHandler(this.WatchlistDlg_Shown);
            this.ss_Watchlist_Main.ResumeLayout(false);
            this.ss_Watchlist_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader ch_Watchlist_Callsgings_Call;
        private System.Windows.Forms.ColumnHeader ch_Watchlist_Selected_Calls;
        public System.Windows.Forms.ListView lv_Watchlist_Callsigns;
        public System.Windows.Forms.ListView lv_Watchlist_Selected;
        private System.Windows.Forms.StatusStrip ss_Watchlist_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Watchlist_Main;
        private System.Windows.Forms.Button btn_Watchlist_Add;
        private System.Windows.Forms.Button btn_Watchlist_Remove;
        private System.Windows.Forms.Button btn_Watchlist_RemoveAll;
        private System.Windows.Forms.ToolTip tt_Watchlist_Main;
        private System.Windows.Forms.Button btn_Watchlist_Cancel;
        private System.Windows.Forms.Button btn_Watchlist_OK;
        private System.Windows.Forms.Label label1;
    }
}