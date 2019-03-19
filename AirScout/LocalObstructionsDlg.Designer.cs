namespace AirScout
{
    partial class LocalObstructionsDlg
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
            this.dgv_Main = new System.Windows.Forms.DataGridView();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_ClearAll = new System.Windows.Forms.Button();
            this.dc_Bearing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dc_Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dc_Height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Main
            // 
            this.dgv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dc_Bearing,
            this.dc_Distance,
            this.dc_Height});
            this.dgv_Main.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv_Main.Location = new System.Drawing.Point(0, 0);
            this.dgv_Main.Name = "dgv_Main";
            this.dgv_Main.Size = new System.Drawing.Size(325, 468);
            this.dgv_Main.TabIndex = 0;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(151, 576);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_OK
            // 
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(232, 576);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // btn_ClearAll
            // 
            this.btn_ClearAll.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_ClearAll.Location = new System.Drawing.Point(12, 576);
            this.btn_ClearAll.Name = "btn_ClearAll";
            this.btn_ClearAll.Size = new System.Drawing.Size(111, 23);
            this.btn_ClearAll.TabIndex = 3;
            this.btn_ClearAll.Text = "Clear All";
            this.btn_ClearAll.UseVisualStyleBackColor = true;
            this.btn_ClearAll.Click += new System.EventHandler(this.btn_ClearAll_Click);
            // 
            // dc_Bearing
            // 
            this.dc_Bearing.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dc_Bearing.DataPropertyName = "Bearing";
            this.dc_Bearing.HeaderText = "Bearing [deg]";
            this.dc_Bearing.Name = "dc_Bearing";
            this.dc_Bearing.ReadOnly = true;
            this.dc_Bearing.Width = 95;
            // 
            // dc_Distance
            // 
            this.dc_Distance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dc_Distance.DataPropertyName = "Distance";
            this.dc_Distance.HeaderText = "Distance [m]";
            this.dc_Distance.Name = "dc_Distance";
            this.dc_Distance.Width = 91;
            // 
            // dc_Height
            // 
            this.dc_Height.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dc_Height.DataPropertyName = "Height";
            this.dc_Height.HeaderText = "Height [m]";
            this.dc_Height.Name = "dc_Height";
            this.dc_Height.Width = 80;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 484);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 75);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fill in your local obstructions here. You can set obe obstruction per degree azim" +
    "uth. Please enter the distance to obstruction and the obstructions\' height above" +
    " ground.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LocalObstructionsDlg
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(325, 621);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_ClearAll);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.dgv_Main);
            this.Name = "LocalObstructionsDlg";
            this.Text = "LocalObstructions at";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Main;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_ClearAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn dc_Bearing;
        private System.Windows.Forms.DataGridViewTextBoxColumn dc_Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn dc_Height;
        private System.Windows.Forms.Label label1;
    }
}