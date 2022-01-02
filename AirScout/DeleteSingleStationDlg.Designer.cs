namespace AirScout
{
    partial class DeleteSingleStationDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteSingleStationDlg));
            this.label1 = new System.Windows.Forms.Label();
            this.cb_Call = new System.Windows.Forms.ComboBox();
            this.cb_Locator = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 92);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // cb_Call
            // 
            this.cb_Call.FormattingEnabled = true;
            this.cb_Call.Location = new System.Drawing.Point(55, 132);
            this.cb_Call.Name = "cb_Call";
            this.cb_Call.Size = new System.Drawing.Size(121, 21);
            this.cb_Call.TabIndex = 1;
            this.cb_Call.DropDown += new System.EventHandler(this.cb_Call_DropDown);
            this.cb_Call.SelectedIndexChanged += new System.EventHandler(this.cb_Call_SelectedIndexChanged);
            this.cb_Call.TextChanged += new System.EventHandler(this.cb_Call_TextChanged);
            this.cb_Call.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cb_Call_KeyPress);
            // 
            // cb_Locator
            // 
            this.cb_Locator.FormattingEnabled = true;
            this.cb_Locator.Location = new System.Drawing.Point(279, 132);
            this.cb_Locator.Name = "cb_Locator";
            this.cb_Locator.Size = new System.Drawing.Size(121, 21);
            this.cb_Locator.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Call:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Locator:";
            // 
            // btn_Delete
            // 
            this.btn_Delete.BackColor = System.Drawing.Color.Tomato;
            this.btn_Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Delete.ForeColor = System.Drawing.Color.White;
            this.btn_Delete.Location = new System.Drawing.Point(55, 173);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(121, 35);
            this.btn_Delete.TabIndex = 5;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = false;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(279, 173);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(121, 35);
            this.btn_Cancel.TabIndex = 6;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // DeleteSingleStationDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(485, 220);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_Locator);
            this.Controls.Add(this.cb_Call);
            this.Controls.Add(this.label1);
            this.Name = "DeleteSingleStationDlg";
            this.Text = "Delete Single Station";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_Call;
        private System.Windows.Forms.ComboBox cb_Locator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Cancel;
    }
}