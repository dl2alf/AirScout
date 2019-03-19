namespace AirScout
{
    partial class PlaneHistoryDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Hex = new System.Windows.Forms.ComboBox();
            this.cb_Call = new System.Windows.Forms.ComboBox();
            this.btn_Export = new System.Windows.Forms.Button();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Button();
            this.bw_GetHexAndCalls = new System.ComponentModel.BackgroundWorker();
            this.ss_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hex:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Call:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "or";
            // 
            // cb_Hex
            // 
            this.cb_Hex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Hex.FormattingEnabled = true;
            this.cb_Hex.Location = new System.Drawing.Point(118, 94);
            this.cb_Hex.Name = "cb_Hex";
            this.cb_Hex.Size = new System.Drawing.Size(121, 24);
            this.cb_Hex.TabIndex = 5;
            this.cb_Hex.TextChanged += new System.EventHandler(this.cb_Hex_TextChanged);
            // 
            // cb_Call
            // 
            this.cb_Call.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Call.FormattingEnabled = true;
            this.cb_Call.Location = new System.Drawing.Point(118, 152);
            this.cb_Call.Name = "cb_Call";
            this.cb_Call.Size = new System.Drawing.Size(121, 24);
            this.cb_Call.TabIndex = 6;
            this.cb_Call.TextChanged += new System.EventHandler(this.cb_Call_TextChanged);
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(118, 199);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(54, 23);
            this.btn_Export.TabIndex = 7;
            this.btn_Export.Text = "Export";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Status});
            this.ss_Main.Location = new System.Drawing.Point(0, 240);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(352, 22);
            this.ss_Main.TabIndex = 8;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Status
            // 
            this.tsl_Status.Name = "tsl_Status";
            this.tsl_Status.Size = new System.Drawing.Size(39, 17);
            this.tsl_Status.Text = "Status";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(312, 65);
            this.label4.TabIndex = 9;
            this.label4.Text = "Use this dialog to export a single aircraft\'s position history. \r\nYou can select " +
    "the aircraft by Hex or by Call.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Close
            // 
            this.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Close.Location = new System.Drawing.Point(185, 199);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(54, 23);
            this.btn_Close.TabIndex = 10;
            this.btn_Close.Text = "Close";
            this.btn_Close.UseVisualStyleBackColor = true;
            // 
            // bw_GetHexAndCalls
            // 
            this.bw_GetHexAndCalls.WorkerReportsProgress = true;
            this.bw_GetHexAndCalls.WorkerSupportsCancellation = true;
            this.bw_GetHexAndCalls.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_GetHexAndCalls_DoWork);
            this.bw_GetHexAndCalls.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_GetHexAndCalls_ProgressChanged);
            this.bw_GetHexAndCalls.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_GetHexAndCalls_RunWorkerCompleted);
            // 
            // PlaneHistoryDlg
            // 
            this.AcceptButton = this.btn_Export;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Close;
            this.ClientSize = new System.Drawing.Size(352, 262);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ss_Main);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.cb_Call);
            this.Controls.Add(this.cb_Hex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlaneHistoryDlg";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PlaneHistoryDlg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlaneHistoryDlg_FormClosing);
            this.Load += new System.EventHandler(this.PlaneHistoryDlg_Load);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_Hex;
        private System.Windows.Forms.ComboBox cb_Call;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Status;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Close;
        private System.ComponentModel.BackgroundWorker bw_GetHexAndCalls;
    }
}