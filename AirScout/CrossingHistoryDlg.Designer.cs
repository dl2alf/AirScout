namespace AirScout
{
    partial class CrossingHistoryDlg
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.bw_History = new System.ComponentModel.BackgroundWorker();
            this.ss_Main = new System.Windows.Forms.StatusStrip();
            this.tsl_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ud_Analysis_AmbiguousGap = new System.Windows.Forms.NumericUpDown();
            this.cb_Analysis_CrossingHistory_WithSignlaLevel = new System.Windows.Forms.CheckBox();
            this.btn_History_Export = new System.Windows.Forms.Button();
            this.btn_History_Cancel = new System.Windows.Forms.Button();
            this.btn_History_Calculate = new System.Windows.Forms.Button();
            this.tt_Crossing_History = new System.Windows.Forms.ToolTip(this.components);
            this.ch_Crossing_History = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ss_Main.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Analysis_AmbiguousGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_Crossing_History)).BeginInit();
            this.SuspendLayout();
            // 
            // bw_History
            // 
            this.bw_History.WorkerReportsProgress = true;
            this.bw_History.WorkerSupportsCancellation = true;
            this.bw_History.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_History_DoWork);
            this.bw_History.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_History_ProgressChanged);
            this.bw_History.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_History_RunWorkerCompleted);
            // 
            // ss_Main
            // 
            this.ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_Main});
            this.ss_Main.Location = new System.Drawing.Point(0, 516);
            this.ss_Main.Name = "ss_Main";
            this.ss_Main.Size = new System.Drawing.Size(893, 22);
            this.ss_Main.TabIndex = 1;
            this.ss_Main.Text = "statusStrip1";
            // 
            // tsl_Main
            // 
            this.tsl_Main.Name = "tsl_Main";
            this.tsl_Main.Size = new System.Drawing.Size(135, 17);
            this.tsl_Main.Text = "Press Calculate to start...";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ud_Analysis_AmbiguousGap);
            this.panel1.Controls.Add(this.cb_Analysis_CrossingHistory_WithSignlaLevel);
            this.panel1.Controls.Add(this.btn_History_Export);
            this.panel1.Controls.Add(this.btn_History_Cancel);
            this.panel1.Controls.Add(this.btn_History_Calculate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 416);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(893, 100);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(393, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "secs.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Timespan within two crossings considered as ambiguous:";
            // 
            // ud_Analysis_AmbiguousGap
            // 
            this.ud_Analysis_AmbiguousGap.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::AirScout.Properties.Settings.Default, "Analysis_CrossingHistory_AmbigousGap", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ud_Analysis_AmbiguousGap.Location = new System.Drawing.Point(318, 49);
            this.ud_Analysis_AmbiguousGap.Name = "ud_Analysis_AmbiguousGap";
            this.ud_Analysis_AmbiguousGap.Size = new System.Drawing.Size(60, 20);
            this.ud_Analysis_AmbiguousGap.TabIndex = 4;
            this.ud_Analysis_AmbiguousGap.Value = global::AirScout.Properties.Settings.Default.Analysis_CrossingHistory_AmbigousGap;
            // 
            // cb_Analysis_CrossingHistory_WithSignlaLevel
            // 
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.AutoSize = true;
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.Checked = global::AirScout.Properties.Settings.Default.Analysis_CrossingHistory_WithSignalLevel;
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AirScout.Properties.Settings.Default, "Analysis_CrossingHistory_WithSignalLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.Location = new System.Drawing.Point(12, 21);
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.Name = "cb_Analysis_CrossingHistory_WithSignlaLevel";
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.Size = new System.Drawing.Size(258, 17);
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.TabIndex = 3;
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.Text = "Consider crossings with recorded signal level only";
            this.cb_Analysis_CrossingHistory_WithSignlaLevel.UseVisualStyleBackColor = true;
            // 
            // btn_History_Export
            // 
            this.btn_History_Export.Location = new System.Drawing.Point(697, 41);
            this.btn_History_Export.Name = "btn_History_Export";
            this.btn_History_Export.Size = new System.Drawing.Size(92, 23);
            this.btn_History_Export.TabIndex = 2;
            this.btn_History_Export.Text = "Export to CSV";
            this.btn_History_Export.UseVisualStyleBackColor = true;
            this.btn_History_Export.Click += new System.EventHandler(this.btn_History_Export_Click);
            // 
            // btn_History_Cancel
            // 
            this.btn_History_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_History_Cancel.Location = new System.Drawing.Point(795, 41);
            this.btn_History_Cancel.Name = "btn_History_Cancel";
            this.btn_History_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_History_Cancel.TabIndex = 1;
            this.btn_History_Cancel.Text = "Back";
            this.btn_History_Cancel.UseVisualStyleBackColor = true;
            this.btn_History_Cancel.Click += new System.EventHandler(this.btn_History_Cancel_Click);
            // 
            // btn_History_Calculate
            // 
            this.btn_History_Calculate.Location = new System.Drawing.Point(616, 41);
            this.btn_History_Calculate.Name = "btn_History_Calculate";
            this.btn_History_Calculate.Size = new System.Drawing.Size(75, 23);
            this.btn_History_Calculate.TabIndex = 0;
            this.btn_History_Calculate.Text = "Calculate";
            this.btn_History_Calculate.UseVisualStyleBackColor = true;
            this.btn_History_Calculate.Click += new System.EventHandler(this.btn_History_Calculate_Click);
            // 
            // tt_Crossing_History
            // 
            this.tt_Crossing_History.IsBalloon = true;
            this.tt_Crossing_History.OwnerDraw = true;
            // 
            // ch_Crossing_History
            // 
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 6;
            chartArea1.AxisX.LabelStyle.Angle = -90;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LabelAutoFitMaxFontSize = 6;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY2.LabelAutoFitMaxFontSize = 6;
            chartArea1.Name = "ChartArea1";
            this.ch_Crossing_History.ChartAreas.Add(chartArea1);
            this.ch_Crossing_History.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.ch_Crossing_History.Legends.Add(legend1);
            this.ch_Crossing_History.Location = new System.Drawing.Point(0, 0);
            this.ch_Crossing_History.Name = "ch_Crossing_History";
            series1.ChartArea = "ChartArea1";
            series1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ch_Crossing_History.Series.Add(series1);
            this.ch_Crossing_History.Size = new System.Drawing.Size(893, 416);
            this.ch_Crossing_History.TabIndex = 5;
            this.ch_Crossing_History.Text = "History";
            // 
            // CrossingHistoryDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_History_Cancel;
            this.ClientSize = new System.Drawing.Size(893, 538);
            this.Controls.Add(this.ch_Crossing_History);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ss_Main);
            this.Name = "CrossingHistoryDlg";
            this.Text = "CrossingHistoryDlg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CrossingHistoryDlg_FormClosing);
            this.ss_Main.ResumeLayout(false);
            this.ss_Main.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Analysis_AmbiguousGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_Crossing_History)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bw_History;
        private System.Windows.Forms.StatusStrip ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel tsl_Main;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_History_Cancel;
        private System.Windows.Forms.Button btn_History_Calculate;
        private System.Windows.Forms.Button btn_History_Export;
        private System.Windows.Forms.ToolTip tt_Crossing_History;
        private System.Windows.Forms.DataVisualization.Charting.Chart ch_Crossing_History;
        private System.Windows.Forms.CheckBox cb_Analysis_CrossingHistory_WithSignlaLevel;
        private System.Windows.Forms.NumericUpDown ud_Analysis_AmbiguousGap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}