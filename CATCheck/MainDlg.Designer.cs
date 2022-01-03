namespace CATCheck
{
    partial class MainDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.tt_Set = new System.Windows.Forms.ToolTip(this.components);
            this.mnu_Main = new System.Windows.Forms.MenuStrip();
            this.mni_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_Commands = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_ClearRIT = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_FrequencyOfTone = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_About = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_Log = new System.Windows.Forms.GroupBox();
            this.lv_Messages = new System.Windows.Forms.ListView();
            this.ch_LogLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_TimeStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.il_MessageStates = new System.Windows.Forms.ImageList(this.components);
            this.lbl_Queue = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_LogVerbosity = new System.Windows.Forms.ComboBox();
            this.btn_Log_Show = new System.Windows.Forms.Button();
            this.btn_Log_Clear = new System.Windows.Forms.Button();
            this.btn_Log_StartStop = new System.Windows.Forms.Button();
            this.gb_RigControl = new System.Windows.Forms.GroupBox();
            this.lbl_RIT_OFS = new System.Windows.Forms.Label();
            this.lbl_FreqB = new System.Windows.Forms.Label();
            this.lbl_FreqA = new System.Windows.Forms.Label();
            this.lbl_Freq = new System.Windows.Forms.Label();
            this.lbl_RIT_OFF = new System.Windows.Forms.Label();
            this.lbl_RIT_ON = new System.Windows.Forms.Label();
            this.lbl_SPLIT_OFF = new System.Windows.Forms.Label();
            this.lbl_SPLIT_ON = new System.Windows.Forms.Label();
            this.lbl_XIT_OFF = new System.Windows.Forms.Label();
            this.lbl_XIT_ON = new System.Windows.Forms.Label();
            this.lbl_VFO_Swap = new System.Windows.Forms.Label();
            this.lbl_VFO_Equal = new System.Windows.Forms.Label();
            this.lbl_VFO_B = new System.Windows.Forms.Label();
            this.lbl_VFO_A = new System.Windows.Forms.Label();
            this.lbl_VFO_AB = new System.Windows.Forms.Label();
            this.lbl_VFO_AA = new System.Windows.Forms.Label();
            this.lbl_VFO_BB = new System.Windows.Forms.Label();
            this.lbl_VFO_BA = new System.Windows.Forms.Label();
            this.lbl_TX = new System.Windows.Forms.Label();
            this.lbl_RX = new System.Windows.Forms.Label();
            this.lbl_FM = new System.Windows.Forms.Label();
            this.lbl_AM = new System.Windows.Forms.Label();
            this.lbl_DIG_L = new System.Windows.Forms.Label();
            this.lbl_DIG_U = new System.Windows.Forms.Label();
            this.lbl_CW_U = new System.Windows.Forms.Label();
            this.lbl_CW_L = new System.Windows.Forms.Label();
            this.lbl_SSB_L = new System.Windows.Forms.Label();
            this.lbl_SSB_U = new System.Windows.Forms.Label();
            this.tb_Freq = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_Status = new System.Windows.Forms.TextBox();
            this.tb_FreqA = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_FreqB = new System.Windows.Forms.TextBox();
            this.gb_RigSettings = new System.Windows.Forms.GroupBox();
            this.btn_Restart = new System.Windows.Forms.Button();
            this.ud_TimeoutMs = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.ud_PollMs = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cb_DTR = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_RTS = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cb_Stopbits = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_Parity = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cb_Databits = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_Baudrate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_PortName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_Rig = new System.Windows.Forms.ComboBox();
            this.gb_RigSettingsFolder = new System.Windows.Forms.GroupBox();
            this.btn_RigDefinitions_Folder = new System.Windows.Forms.Button();
            this.tb_RigDefinitionsFolder = new System.Windows.Forms.TextBox();
            this.ti_Main = new System.Windows.Forms.Timer(this.components);
            this.gb_COM = new System.Windows.Forms.GroupBox();
            this.lbl_RTS = new System.Windows.Forms.Label();
            this.lbl_DTR = new System.Windows.Forms.Label();
            this.lbl_CTS = new System.Windows.Forms.Label();
            this.lbl_DSR = new System.Windows.Forms.Label();
            this.mnu_Main.SuspendLayout();
            this.gb_Log.SuspendLayout();
            this.gb_RigControl.SuspendLayout();
            this.gb_RigSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_TimeoutMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_PollMs)).BeginInit();
            this.gb_RigSettingsFolder.SuspendLayout();
            this.gb_COM.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu_Main
            // 
            this.mnu_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mni_Exit,
            this.mni_Commands,
            this.mni_About});
            this.mnu_Main.Location = new System.Drawing.Point(0, 0);
            this.mnu_Main.Name = "mnu_Main";
            this.mnu_Main.Size = new System.Drawing.Size(1008, 24);
            this.mnu_Main.TabIndex = 10;
            this.mnu_Main.Text = "menuStrip1";
            // 
            // mni_Exit
            // 
            this.mni_Exit.Name = "mni_Exit";
            this.mni_Exit.Size = new System.Drawing.Size(38, 20);
            this.mni_Exit.Text = "E&xit";
            this.mni_Exit.Click += new System.EventHandler(this.mni_Exit_Click);
            // 
            // mni_Commands
            // 
            this.mni_Commands.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mni_ClearRIT,
            this.mni_FrequencyOfTone});
            this.mni_Commands.Name = "mni_Commands";
            this.mni_Commands.Size = new System.Drawing.Size(81, 20);
            this.mni_Commands.Text = "Commands";
            // 
            // mni_ClearRIT
            // 
            this.mni_ClearRIT.Name = "mni_ClearRIT";
            this.mni_ClearRIT.Size = new System.Drawing.Size(170, 22);
            this.mni_ClearRIT.Text = "Clear RIT";
            this.mni_ClearRIT.Click += new System.EventHandler(this.mni_ClearRIT_Click);
            // 
            // mni_FrequencyOfTone
            // 
            this.mni_FrequencyOfTone.Name = "mni_FrequencyOfTone";
            this.mni_FrequencyOfTone.Size = new System.Drawing.Size(170, 22);
            this.mni_FrequencyOfTone.Text = "Frequency of tone";
            this.mni_FrequencyOfTone.Click += new System.EventHandler(this.mni_FrequencyOfTone_Click);
            // 
            // mni_About
            // 
            this.mni_About.Name = "mni_About";
            this.mni_About.Size = new System.Drawing.Size(52, 20);
            this.mni_About.Text = "&About";
            this.mni_About.Click += new System.EventHandler(this.mni_About_Click);
            // 
            // gb_Log
            // 
            this.gb_Log.Controls.Add(this.lv_Messages);
            this.gb_Log.Controls.Add(this.lbl_Queue);
            this.gb_Log.Controls.Add(this.label15);
            this.gb_Log.Controls.Add(this.label1);
            this.gb_Log.Controls.Add(this.cb_LogVerbosity);
            this.gb_Log.Controls.Add(this.btn_Log_Show);
            this.gb_Log.Controls.Add(this.btn_Log_Clear);
            this.gb_Log.Controls.Add(this.btn_Log_StartStop);
            this.gb_Log.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Log.Location = new System.Drawing.Point(234, 191);
            this.gb_Log.Name = "gb_Log";
            this.gb_Log.Size = new System.Drawing.Size(774, 358);
            this.gb_Log.TabIndex = 14;
            this.gb_Log.TabStop = false;
            this.gb_Log.Text = "Log";
            // 
            // lv_Messages
            // 
            this.lv_Messages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_LogLevel,
            this.ch_TimeStamp,
            this.ch_Message});
            this.lv_Messages.Dock = System.Windows.Forms.DockStyle.Top;
            this.lv_Messages.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_Messages.FullRowSelect = true;
            this.lv_Messages.GridLines = true;
            this.lv_Messages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Messages.HideSelection = false;
            this.lv_Messages.Location = new System.Drawing.Point(3, 16);
            this.lv_Messages.Name = "lv_Messages";
            this.lv_Messages.OwnerDraw = true;
            this.lv_Messages.Size = new System.Drawing.Size(768, 280);
            this.lv_Messages.SmallImageList = this.il_MessageStates;
            this.lv_Messages.TabIndex = 13;
            this.lv_Messages.UseCompatibleStateImageBehavior = false;
            this.lv_Messages.View = System.Windows.Forms.View.Details;
            this.lv_Messages.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lv_Messages_DrawColumnHeader);
            this.lv_Messages.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lv_Messages_DrawItem);
            this.lv_Messages.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lv_Messages_DrawSubItem);
            // 
            // ch_LogLevel
            // 
            this.ch_LogLevel.Text = "L";
            this.ch_LogLevel.Width = 30;
            // 
            // ch_TimeStamp
            // 
            this.ch_TimeStamp.Text = "UTC";
            this.ch_TimeStamp.Width = 180;
            // 
            // ch_Message
            // 
            this.ch_Message.Text = "Message";
            this.ch_Message.Width = 540;
            // 
            // il_MessageStates
            // 
            this.il_MessageStates.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_MessageStates.ImageStream")));
            this.il_MessageStates.TransparentColor = System.Drawing.Color.Transparent;
            this.il_MessageStates.Images.SetKeyName(0, "CAT.png");
            this.il_MessageStates.Images.SetKeyName(1, "eventlogInfo.ico");
            this.il_MessageStates.Images.SetKeyName(2, "eventlogWarn.ico");
            this.il_MessageStates.Images.SetKeyName(3, "eventlogError.ico");
            // 
            // lbl_Queue
            // 
            this.lbl_Queue.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Queue.Location = new System.Drawing.Point(291, 324);
            this.lbl_Queue.Name = "lbl_Queue";
            this.lbl_Queue.Size = new System.Drawing.Size(61, 19);
            this.lbl_Queue.TabIndex = 12;
            this.lbl_Queue.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(231, 325);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 13);
            this.label15.TabIndex = 11;
            this.label15.Text = "In Queue:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(368, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Log Verbosity:";
            // 
            // cb_LogVerbosity
            // 
            this.cb_LogVerbosity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LogVerbosity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_LogVerbosity.FormattingEnabled = true;
            this.cb_LogVerbosity.Location = new System.Drawing.Point(448, 322);
            this.cb_LogVerbosity.Name = "cb_LogVerbosity";
            this.cb_LogVerbosity.Size = new System.Drawing.Size(121, 21);
            this.cb_LogVerbosity.TabIndex = 7;
            this.cb_LogVerbosity.SelectedIndexChanged += new System.EventHandler(this.cb_LogVerbosity_SelectedIndexChanged);
            // 
            // btn_Log_Show
            // 
            this.btn_Log_Show.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Log_Show.Location = new System.Drawing.Point(608, 318);
            this.btn_Log_Show.Name = "btn_Log_Show";
            this.btn_Log_Show.Size = new System.Drawing.Size(148, 27);
            this.btn_Log_Show.TabIndex = 5;
            this.btn_Log_Show.Text = "Show Log File in Explorer";
            this.btn_Log_Show.UseVisualStyleBackColor = true;
            this.btn_Log_Show.Click += new System.EventHandler(this.btn_Log_Show_Click);
            // 
            // btn_Log_Clear
            // 
            this.btn_Log_Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Log_Clear.Location = new System.Drawing.Point(115, 318);
            this.btn_Log_Clear.Name = "btn_Log_Clear";
            this.btn_Log_Clear.Size = new System.Drawing.Size(103, 27);
            this.btn_Log_Clear.TabIndex = 4;
            this.btn_Log_Clear.Text = "Clear Log";
            this.btn_Log_Clear.UseVisualStyleBackColor = true;
            this.btn_Log_Clear.Click += new System.EventHandler(this.btn_Log_Clear_Click);
            // 
            // btn_Log_StartStop
            // 
            this.btn_Log_StartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Log_StartStop.Location = new System.Drawing.Point(6, 318);
            this.btn_Log_StartStop.Name = "btn_Log_StartStop";
            this.btn_Log_StartStop.Size = new System.Drawing.Size(103, 27);
            this.btn_Log_StartStop.TabIndex = 3;
            this.btn_Log_StartStop.Text = "Stop Log";
            this.btn_Log_StartStop.UseVisualStyleBackColor = true;
            this.btn_Log_StartStop.Click += new System.EventHandler(this.btn_Log_StartStop_Click);
            // 
            // gb_RigControl
            // 
            this.gb_RigControl.Controls.Add(this.lbl_RIT_OFS);
            this.gb_RigControl.Controls.Add(this.lbl_FreqB);
            this.gb_RigControl.Controls.Add(this.lbl_FreqA);
            this.gb_RigControl.Controls.Add(this.lbl_Freq);
            this.gb_RigControl.Controls.Add(this.lbl_RIT_OFF);
            this.gb_RigControl.Controls.Add(this.lbl_RIT_ON);
            this.gb_RigControl.Controls.Add(this.lbl_SPLIT_OFF);
            this.gb_RigControl.Controls.Add(this.lbl_SPLIT_ON);
            this.gb_RigControl.Controls.Add(this.lbl_XIT_OFF);
            this.gb_RigControl.Controls.Add(this.lbl_XIT_ON);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_Swap);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_Equal);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_B);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_A);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_AB);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_AA);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_BB);
            this.gb_RigControl.Controls.Add(this.lbl_VFO_BA);
            this.gb_RigControl.Controls.Add(this.lbl_TX);
            this.gb_RigControl.Controls.Add(this.lbl_RX);
            this.gb_RigControl.Controls.Add(this.lbl_FM);
            this.gb_RigControl.Controls.Add(this.lbl_AM);
            this.gb_RigControl.Controls.Add(this.lbl_DIG_L);
            this.gb_RigControl.Controls.Add(this.lbl_DIG_U);
            this.gb_RigControl.Controls.Add(this.lbl_CW_U);
            this.gb_RigControl.Controls.Add(this.lbl_CW_L);
            this.gb_RigControl.Controls.Add(this.lbl_SSB_L);
            this.gb_RigControl.Controls.Add(this.lbl_SSB_U);
            this.gb_RigControl.Controls.Add(this.tb_Freq);
            this.gb_RigControl.Controls.Add(this.label14);
            this.gb_RigControl.Controls.Add(this.tb_Status);
            this.gb_RigControl.Controls.Add(this.tb_FreqA);
            this.gb_RigControl.Controls.Add(this.label3);
            this.gb_RigControl.Controls.Add(this.label2);
            this.gb_RigControl.Controls.Add(this.tb_FreqB);
            this.gb_RigControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_RigControl.Location = new System.Drawing.Point(0, 77);
            this.gb_RigControl.Name = "gb_RigControl";
            this.gb_RigControl.Size = new System.Drawing.Size(938, 114);
            this.gb_RigControl.TabIndex = 13;
            this.gb_RigControl.TabStop = false;
            this.gb_RigControl.Text = "Rig Control";
            // 
            // lbl_RIT_OFS
            // 
            this.lbl_RIT_OFS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_RIT_OFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RIT_OFS.ForeColor = System.Drawing.Color.Gray;
            this.lbl_RIT_OFS.Location = new System.Drawing.Point(792, 84);
            this.lbl_RIT_OFS.Name = "lbl_RIT_OFS";
            this.lbl_RIT_OFS.Size = new System.Drawing.Size(65, 20);
            this.lbl_RIT_OFS.TabIndex = 46;
            this.lbl_RIT_OFS.Text = "0.00";
            this.lbl_RIT_OFS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FreqB
            // 
            this.lbl_FreqB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_FreqB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FreqB.ForeColor = System.Drawing.Color.Gray;
            this.lbl_FreqB.Location = new System.Drawing.Point(462, 79);
            this.lbl_FreqB.Name = "lbl_FreqB";
            this.lbl_FreqB.Size = new System.Drawing.Size(45, 25);
            this.lbl_FreqB.TabIndex = 45;
            this.lbl_FreqB.Text = "Set";
            this.lbl_FreqB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_FreqB.Click += new System.EventHandler(this.lbl_FreqB_Click);
            // 
            // lbl_FreqA
            // 
            this.lbl_FreqA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_FreqA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FreqA.ForeColor = System.Drawing.Color.Gray;
            this.lbl_FreqA.Location = new System.Drawing.Point(462, 47);
            this.lbl_FreqA.Name = "lbl_FreqA";
            this.lbl_FreqA.Size = new System.Drawing.Size(45, 25);
            this.lbl_FreqA.TabIndex = 44;
            this.lbl_FreqA.Text = "Set";
            this.lbl_FreqA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_FreqA.Click += new System.EventHandler(this.lbl_FreqA_Click);
            // 
            // lbl_Freq
            // 
            this.lbl_Freq.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Freq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Freq.ForeColor = System.Drawing.Color.Gray;
            this.lbl_Freq.Location = new System.Drawing.Point(462, 15);
            this.lbl_Freq.Name = "lbl_Freq";
            this.lbl_Freq.Size = new System.Drawing.Size(45, 25);
            this.lbl_Freq.TabIndex = 43;
            this.lbl_Freq.Text = "Set";
            this.lbl_Freq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Freq.Click += new System.EventHandler(this.lbl_Freq_Click);
            // 
            // lbl_RIT_OFF
            // 
            this.lbl_RIT_OFF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_RIT_OFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RIT_OFF.ForeColor = System.Drawing.Color.Gray;
            this.lbl_RIT_OFF.Location = new System.Drawing.Point(863, 61);
            this.lbl_RIT_OFF.Name = "lbl_RIT_OFF";
            this.lbl_RIT_OFF.Size = new System.Drawing.Size(65, 20);
            this.lbl_RIT_OFF.TabIndex = 42;
            this.lbl_RIT_OFF.Text = "RIT OFF";
            this.lbl_RIT_OFF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_RIT_OFF.Click += new System.EventHandler(this.lbl_RIT_OFF_Click);
            // 
            // lbl_RIT_ON
            // 
            this.lbl_RIT_ON.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_RIT_ON.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RIT_ON.ForeColor = System.Drawing.Color.Gray;
            this.lbl_RIT_ON.Location = new System.Drawing.Point(792, 61);
            this.lbl_RIT_ON.Name = "lbl_RIT_ON";
            this.lbl_RIT_ON.Size = new System.Drawing.Size(65, 20);
            this.lbl_RIT_ON.TabIndex = 41;
            this.lbl_RIT_ON.Text = "RIT ON";
            this.lbl_RIT_ON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_RIT_ON.Click += new System.EventHandler(this.lbl_RIT_ON_Click);
            // 
            // lbl_SPLIT_OFF
            // 
            this.lbl_SPLIT_OFF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_SPLIT_OFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SPLIT_OFF.ForeColor = System.Drawing.Color.Gray;
            this.lbl_SPLIT_OFF.Location = new System.Drawing.Point(863, 15);
            this.lbl_SPLIT_OFF.Name = "lbl_SPLIT_OFF";
            this.lbl_SPLIT_OFF.Size = new System.Drawing.Size(65, 20);
            this.lbl_SPLIT_OFF.TabIndex = 40;
            this.lbl_SPLIT_OFF.Text = "SPLIT OFF";
            this.lbl_SPLIT_OFF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_SPLIT_OFF.Click += new System.EventHandler(this.lbl_SPLIT_OFF_Click);
            // 
            // lbl_SPLIT_ON
            // 
            this.lbl_SPLIT_ON.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_SPLIT_ON.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SPLIT_ON.ForeColor = System.Drawing.Color.Gray;
            this.lbl_SPLIT_ON.Location = new System.Drawing.Point(792, 15);
            this.lbl_SPLIT_ON.Name = "lbl_SPLIT_ON";
            this.lbl_SPLIT_ON.Size = new System.Drawing.Size(65, 20);
            this.lbl_SPLIT_ON.TabIndex = 39;
            this.lbl_SPLIT_ON.Text = "SPLIT ON";
            this.lbl_SPLIT_ON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_SPLIT_ON.Click += new System.EventHandler(this.lbl_SPLIT_ON_Click);
            // 
            // lbl_XIT_OFF
            // 
            this.lbl_XIT_OFF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_XIT_OFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_XIT_OFF.ForeColor = System.Drawing.Color.Gray;
            this.lbl_XIT_OFF.Location = new System.Drawing.Point(863, 38);
            this.lbl_XIT_OFF.Name = "lbl_XIT_OFF";
            this.lbl_XIT_OFF.Size = new System.Drawing.Size(65, 20);
            this.lbl_XIT_OFF.TabIndex = 38;
            this.lbl_XIT_OFF.Text = "XIT OFF";
            this.lbl_XIT_OFF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_XIT_OFF.Click += new System.EventHandler(this.lbl_XIT_OFF_Click);
            // 
            // lbl_XIT_ON
            // 
            this.lbl_XIT_ON.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_XIT_ON.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_XIT_ON.ForeColor = System.Drawing.Color.Gray;
            this.lbl_XIT_ON.Location = new System.Drawing.Point(792, 38);
            this.lbl_XIT_ON.Name = "lbl_XIT_ON";
            this.lbl_XIT_ON.Size = new System.Drawing.Size(65, 20);
            this.lbl_XIT_ON.TabIndex = 37;
            this.lbl_XIT_ON.Text = "XIT ON";
            this.lbl_XIT_ON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_XIT_ON.Click += new System.EventHandler(this.lbl_XIT_ON_Click);
            // 
            // lbl_VFO_Swap
            // 
            this.lbl_VFO_Swap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_Swap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_Swap.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_Swap.Location = new System.Drawing.Point(721, 84);
            this.lbl_VFO_Swap.Name = "lbl_VFO_Swap";
            this.lbl_VFO_Swap.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_Swap.TabIndex = 36;
            this.lbl_VFO_Swap.Text = "A <> B";
            this.lbl_VFO_Swap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_Swap.Click += new System.EventHandler(this.lbl_VFO_Swap_Click);
            // 
            // lbl_VFO_Equal
            // 
            this.lbl_VFO_Equal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_Equal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_Equal.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_Equal.Location = new System.Drawing.Point(660, 84);
            this.lbl_VFO_Equal.Name = "lbl_VFO_Equal";
            this.lbl_VFO_Equal.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_Equal.TabIndex = 35;
            this.lbl_VFO_Equal.Text = "A = B";
            this.lbl_VFO_Equal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_Equal.Click += new System.EventHandler(this.lbl_VFO_Equal_Click);
            // 
            // lbl_VFO_B
            // 
            this.lbl_VFO_B.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_B.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_B.Location = new System.Drawing.Point(721, 61);
            this.lbl_VFO_B.Name = "lbl_VFO_B";
            this.lbl_VFO_B.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_B.TabIndex = 34;
            this.lbl_VFO_B.Text = "VFO B";
            this.lbl_VFO_B.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_B.Click += new System.EventHandler(this.lbl_VFO_B_Click);
            // 
            // lbl_VFO_A
            // 
            this.lbl_VFO_A.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_A.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_A.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_A.Location = new System.Drawing.Point(660, 61);
            this.lbl_VFO_A.Name = "lbl_VFO_A";
            this.lbl_VFO_A.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_A.TabIndex = 33;
            this.lbl_VFO_A.Text = "VFO A";
            this.lbl_VFO_A.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_A.Click += new System.EventHandler(this.lbl_VFO_A_Click);
            // 
            // lbl_VFO_AB
            // 
            this.lbl_VFO_AB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_AB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_AB.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_AB.Location = new System.Drawing.Point(721, 15);
            this.lbl_VFO_AB.Name = "lbl_VFO_AB";
            this.lbl_VFO_AB.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_AB.TabIndex = 32;
            this.lbl_VFO_AB.Text = "VFA AB";
            this.lbl_VFO_AB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_AB.Click += new System.EventHandler(this.lbl_VFO_AB_Click);
            // 
            // lbl_VFO_AA
            // 
            this.lbl_VFO_AA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_AA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_AA.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_AA.Location = new System.Drawing.Point(660, 15);
            this.lbl_VFO_AA.Name = "lbl_VFO_AA";
            this.lbl_VFO_AA.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_AA.TabIndex = 31;
            this.lbl_VFO_AA.Text = "VFO AA";
            this.lbl_VFO_AA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_AA.Click += new System.EventHandler(this.lbl_VFO_AA_Click);
            // 
            // lbl_VFO_BB
            // 
            this.lbl_VFO_BB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_BB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_BB.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_BB.Location = new System.Drawing.Point(721, 38);
            this.lbl_VFO_BB.Name = "lbl_VFO_BB";
            this.lbl_VFO_BB.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_BB.TabIndex = 30;
            this.lbl_VFO_BB.Text = "VFO BB";
            this.lbl_VFO_BB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_BB.Click += new System.EventHandler(this.lbl_VFO_BB_Click);
            // 
            // lbl_VFO_BA
            // 
            this.lbl_VFO_BA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_VFO_BA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VFO_BA.ForeColor = System.Drawing.Color.Gray;
            this.lbl_VFO_BA.Location = new System.Drawing.Point(660, 38);
            this.lbl_VFO_BA.Name = "lbl_VFO_BA";
            this.lbl_VFO_BA.Size = new System.Drawing.Size(55, 20);
            this.lbl_VFO_BA.TabIndex = 29;
            this.lbl_VFO_BA.Text = "VFO BA";
            this.lbl_VFO_BA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_VFO_BA.Click += new System.EventHandler(this.lbl_VFO_BA_Click);
            // 
            // lbl_TX
            // 
            this.lbl_TX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_TX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TX.ForeColor = System.Drawing.Color.Gray;
            this.lbl_TX.Location = new System.Drawing.Point(110, 58);
            this.lbl_TX.Name = "lbl_TX";
            this.lbl_TX.Size = new System.Drawing.Size(84, 39);
            this.lbl_TX.TabIndex = 28;
            this.lbl_TX.Text = "TX";
            this.lbl_TX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_TX.Click += new System.EventHandler(this.lbl_TX_Click);
            // 
            // lbl_RX
            // 
            this.lbl_RX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_RX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RX.ForeColor = System.Drawing.Color.Gray;
            this.lbl_RX.Location = new System.Drawing.Point(12, 58);
            this.lbl_RX.Name = "lbl_RX";
            this.lbl_RX.Size = new System.Drawing.Size(84, 39);
            this.lbl_RX.TabIndex = 27;
            this.lbl_RX.Text = "RX";
            this.lbl_RX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_RX.Click += new System.EventHandler(this.lbl_RX_Click);
            // 
            // lbl_FM
            // 
            this.lbl_FM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_FM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FM.ForeColor = System.Drawing.Color.Gray;
            this.lbl_FM.Location = new System.Drawing.Point(586, 84);
            this.lbl_FM.Name = "lbl_FM";
            this.lbl_FM.Size = new System.Drawing.Size(55, 20);
            this.lbl_FM.TabIndex = 26;
            this.lbl_FM.Text = "FM";
            this.lbl_FM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_FM.Click += new System.EventHandler(this.lbl_FM_Click);
            // 
            // lbl_AM
            // 
            this.lbl_AM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_AM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_AM.ForeColor = System.Drawing.Color.Gray;
            this.lbl_AM.Location = new System.Drawing.Point(525, 84);
            this.lbl_AM.Name = "lbl_AM";
            this.lbl_AM.Size = new System.Drawing.Size(55, 20);
            this.lbl_AM.TabIndex = 25;
            this.lbl_AM.Text = "AM";
            this.lbl_AM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_AM.Click += new System.EventHandler(this.lbl_AM_Click);
            // 
            // lbl_DIG_L
            // 
            this.lbl_DIG_L.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_DIG_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DIG_L.ForeColor = System.Drawing.Color.Gray;
            this.lbl_DIG_L.Location = new System.Drawing.Point(586, 61);
            this.lbl_DIG_L.Name = "lbl_DIG_L";
            this.lbl_DIG_L.Size = new System.Drawing.Size(55, 20);
            this.lbl_DIG_L.TabIndex = 24;
            this.lbl_DIG_L.Text = "DATA_R";
            this.lbl_DIG_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_DIG_L.Click += new System.EventHandler(this.lbl_DIG_L_Click);
            // 
            // lbl_DIG_U
            // 
            this.lbl_DIG_U.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_DIG_U.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DIG_U.ForeColor = System.Drawing.Color.Gray;
            this.lbl_DIG_U.Location = new System.Drawing.Point(525, 61);
            this.lbl_DIG_U.Name = "lbl_DIG_U";
            this.lbl_DIG_U.Size = new System.Drawing.Size(55, 20);
            this.lbl_DIG_U.TabIndex = 23;
            this.lbl_DIG_U.Text = "DATA";
            this.lbl_DIG_U.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_DIG_U.Click += new System.EventHandler(this.lbl_DIG_U_Click);
            // 
            // lbl_CW_U
            // 
            this.lbl_CW_U.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_CW_U.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CW_U.ForeColor = System.Drawing.Color.Gray;
            this.lbl_CW_U.Location = new System.Drawing.Point(586, 15);
            this.lbl_CW_U.Name = "lbl_CW_U";
            this.lbl_CW_U.Size = new System.Drawing.Size(55, 20);
            this.lbl_CW_U.TabIndex = 22;
            this.lbl_CW_U.Text = "CW-R";
            this.lbl_CW_U.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_CW_U.Click += new System.EventHandler(this.lbl_CW_U_Click);
            // 
            // lbl_CW_L
            // 
            this.lbl_CW_L.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_CW_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CW_L.ForeColor = System.Drawing.Color.Gray;
            this.lbl_CW_L.Location = new System.Drawing.Point(525, 15);
            this.lbl_CW_L.Name = "lbl_CW_L";
            this.lbl_CW_L.Size = new System.Drawing.Size(55, 20);
            this.lbl_CW_L.TabIndex = 21;
            this.lbl_CW_L.Text = "CW";
            this.lbl_CW_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_CW_L.Click += new System.EventHandler(this.lbl_CW_L_Click);
            // 
            // lbl_SSB_L
            // 
            this.lbl_SSB_L.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_SSB_L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SSB_L.ForeColor = System.Drawing.Color.Gray;
            this.lbl_SSB_L.Location = new System.Drawing.Point(586, 38);
            this.lbl_SSB_L.Name = "lbl_SSB_L";
            this.lbl_SSB_L.Size = new System.Drawing.Size(55, 20);
            this.lbl_SSB_L.TabIndex = 20;
            this.lbl_SSB_L.Text = "LSB";
            this.lbl_SSB_L.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_SSB_L.Click += new System.EventHandler(this.lbl_SSB_L_Click);
            // 
            // lbl_SSB_U
            // 
            this.lbl_SSB_U.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_SSB_U.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SSB_U.ForeColor = System.Drawing.Color.Gray;
            this.lbl_SSB_U.Location = new System.Drawing.Point(525, 38);
            this.lbl_SSB_U.Name = "lbl_SSB_U";
            this.lbl_SSB_U.Size = new System.Drawing.Size(55, 20);
            this.lbl_SSB_U.TabIndex = 19;
            this.lbl_SSB_U.Text = "USB";
            this.lbl_SSB_U.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_SSB_U.Click += new System.EventHandler(this.lbl_SSB_U_Click);
            // 
            // tb_Freq
            // 
            this.tb_Freq.BackColor = System.Drawing.Color.Gray;
            this.tb_Freq.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Freq.ForeColor = System.Drawing.Color.Chartreuse;
            this.tb_Freq.Location = new System.Drawing.Point(254, 14);
            this.tb_Freq.Name = "tb_Freq";
            this.tb_Freq.ReadOnly = true;
            this.tb_Freq.Size = new System.Drawing.Size(186, 26);
            this.tb_Freq.TabIndex = 6;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(204, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "Freq:";
            // 
            // tb_Status
            // 
            this.tb_Status.BackColor = System.Drawing.Color.AntiqueWhite;
            this.tb_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Status.Location = new System.Drawing.Point(12, 20);
            this.tb_Status.Name = "tb_Status";
            this.tb_Status.Size = new System.Drawing.Size(182, 26);
            this.tb_Status.TabIndex = 0;
            // 
            // tb_FreqA
            // 
            this.tb_FreqA.BackColor = System.Drawing.Color.Gray;
            this.tb_FreqA.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_FreqA.ForeColor = System.Drawing.Color.Chartreuse;
            this.tb_FreqA.Location = new System.Drawing.Point(254, 46);
            this.tb_FreqA.Name = "tb_FreqA";
            this.tb_FreqA.ReadOnly = true;
            this.tb_FreqA.Size = new System.Drawing.Size(186, 26);
            this.tb_FreqA.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(204, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Freq B:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(204, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Freq A:";
            // 
            // tb_FreqB
            // 
            this.tb_FreqB.BackColor = System.Drawing.Color.Gray;
            this.tb_FreqB.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_FreqB.ForeColor = System.Drawing.Color.Chartreuse;
            this.tb_FreqB.Location = new System.Drawing.Point(254, 78);
            this.tb_FreqB.Name = "tb_FreqB";
            this.tb_FreqB.ReadOnly = true;
            this.tb_FreqB.Size = new System.Drawing.Size(186, 26);
            this.tb_FreqB.TabIndex = 4;
            // 
            // gb_RigSettings
            // 
            this.gb_RigSettings.Controls.Add(this.btn_Restart);
            this.gb_RigSettings.Controls.Add(this.ud_TimeoutMs);
            this.gb_RigSettings.Controls.Add(this.label13);
            this.gb_RigSettings.Controls.Add(this.ud_PollMs);
            this.gb_RigSettings.Controls.Add(this.label12);
            this.gb_RigSettings.Controls.Add(this.label11);
            this.gb_RigSettings.Controls.Add(this.cb_DTR);
            this.gb_RigSettings.Controls.Add(this.label10);
            this.gb_RigSettings.Controls.Add(this.cb_RTS);
            this.gb_RigSettings.Controls.Add(this.label9);
            this.gb_RigSettings.Controls.Add(this.cb_Stopbits);
            this.gb_RigSettings.Controls.Add(this.label8);
            this.gb_RigSettings.Controls.Add(this.cb_Parity);
            this.gb_RigSettings.Controls.Add(this.label7);
            this.gb_RigSettings.Controls.Add(this.cb_Databits);
            this.gb_RigSettings.Controls.Add(this.label6);
            this.gb_RigSettings.Controls.Add(this.cb_Baudrate);
            this.gb_RigSettings.Controls.Add(this.label5);
            this.gb_RigSettings.Controls.Add(this.cb_PortName);
            this.gb_RigSettings.Controls.Add(this.label4);
            this.gb_RigSettings.Controls.Add(this.cb_Rig);
            this.gb_RigSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_RigSettings.Location = new System.Drawing.Point(18, 191);
            this.gb_RigSettings.Name = "gb_RigSettings";
            this.gb_RigSettings.Size = new System.Drawing.Size(210, 358);
            this.gb_RigSettings.TabIndex = 12;
            this.gb_RigSettings.TabStop = false;
            this.gb_RigSettings.Text = "Rig Settings";
            // 
            // btn_Restart
            // 
            this.btn_Restart.BackColor = System.Drawing.Color.PaleGreen;
            this.btn_Restart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Restart.Location = new System.Drawing.Point(12, 318);
            this.btn_Restart.Name = "btn_Restart";
            this.btn_Restart.Size = new System.Drawing.Size(182, 27);
            this.btn_Restart.TabIndex = 8;
            this.btn_Restart.Text = "Restart Rig";
            this.btn_Restart.UseVisualStyleBackColor = false;
            this.btn_Restart.Click += new System.EventHandler(this.btn_Restart_Click);
            // 
            // ud_TimeoutMs
            // 
            this.ud_TimeoutMs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ud_TimeoutMs.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ud_TimeoutMs.Location = new System.Drawing.Point(125, 276);
            this.ud_TimeoutMs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ud_TimeoutMs.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ud_TimeoutMs.Name = "ud_TimeoutMs";
            this.ud_TimeoutMs.Size = new System.Drawing.Size(69, 20);
            this.ud_TimeoutMs.TabIndex = 26;
            this.ud_TimeoutMs.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.ud_TimeoutMs.ValueChanged += new System.EventHandler(this.ud_TimeoutMs_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(9, 278);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Timeout [ms]:";
            // 
            // ud_PollMs
            // 
            this.ud_PollMs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ud_PollMs.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ud_PollMs.Location = new System.Drawing.Point(125, 250);
            this.ud_PollMs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ud_PollMs.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ud_PollMs.Name = "ud_PollMs";
            this.ud_PollMs.Size = new System.Drawing.Size(69, 20);
            this.ud_PollMs.TabIndex = 8;
            this.ud_PollMs.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ud_PollMs.ValueChanged += new System.EventHandler(this.ud_PollMs_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(9, 252);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Poll Int [ms]:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(9, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "DTR:";
            // 
            // cb_DTR
            // 
            this.cb_DTR.DisplayMember = "Text";
            this.cb_DTR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_DTR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_DTR.FormattingEnabled = true;
            this.cb_DTR.Location = new System.Drawing.Point(87, 222);
            this.cb_DTR.Name = "cb_DTR";
            this.cb_DTR.Size = new System.Drawing.Size(107, 21);
            this.cb_DTR.TabIndex = 22;
            this.cb_DTR.ValueMember = "Value";
            this.cb_DTR.SelectedIndexChanged += new System.EventHandler(this.cb_DTR_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(9, 198);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "RTS:";
            // 
            // cb_RTS
            // 
            this.cb_RTS.DisplayMember = "Text";
            this.cb_RTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_RTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_RTS.FormattingEnabled = true;
            this.cb_RTS.Location = new System.Drawing.Point(87, 195);
            this.cb_RTS.Name = "cb_RTS";
            this.cb_RTS.Size = new System.Drawing.Size(107, 21);
            this.cb_RTS.TabIndex = 20;
            this.cb_RTS.ValueMember = "Value";
            this.cb_RTS.SelectedIndexChanged += new System.EventHandler(this.cb_RTS_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 171);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Stopbits:";
            // 
            // cb_Stopbits
            // 
            this.cb_Stopbits.DisplayMember = "Text";
            this.cb_Stopbits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Stopbits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Stopbits.FormattingEnabled = true;
            this.cb_Stopbits.Location = new System.Drawing.Point(87, 168);
            this.cb_Stopbits.Name = "cb_Stopbits";
            this.cb_Stopbits.Size = new System.Drawing.Size(107, 21);
            this.cb_Stopbits.TabIndex = 18;
            this.cb_Stopbits.ValueMember = "Value";
            this.cb_Stopbits.SelectedIndexChanged += new System.EventHandler(this.cb_Stopbits_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Parity:";
            // 
            // cb_Parity
            // 
            this.cb_Parity.DisplayMember = "Text";
            this.cb_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Parity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Parity.FormattingEnabled = true;
            this.cb_Parity.Location = new System.Drawing.Point(87, 141);
            this.cb_Parity.Name = "cb_Parity";
            this.cb_Parity.Size = new System.Drawing.Size(107, 21);
            this.cb_Parity.TabIndex = 16;
            this.cb_Parity.ValueMember = "Value";
            this.cb_Parity.SelectedIndexChanged += new System.EventHandler(this.cb_Parity_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Databits:";
            // 
            // cb_Databits
            // 
            this.cb_Databits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Databits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Databits.FormattingEnabled = true;
            this.cb_Databits.Location = new System.Drawing.Point(87, 114);
            this.cb_Databits.Name = "cb_Databits";
            this.cb_Databits.Size = new System.Drawing.Size(107, 21);
            this.cb_Databits.TabIndex = 14;
            this.cb_Databits.SelectedIndexChanged += new System.EventHandler(this.cb_Databits_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Baudrate:";
            // 
            // cb_Baudrate
            // 
            this.cb_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Baudrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Baudrate.FormattingEnabled = true;
            this.cb_Baudrate.Location = new System.Drawing.Point(87, 87);
            this.cb_Baudrate.Name = "cb_Baudrate";
            this.cb_Baudrate.Size = new System.Drawing.Size(107, 21);
            this.cb_Baudrate.TabIndex = 12;
            this.cb_Baudrate.SelectedIndexChanged += new System.EventHandler(this.cb_Baudrate_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "PortName:";
            // 
            // cb_PortName
            // 
            this.cb_PortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_PortName.FormattingEnabled = true;
            this.cb_PortName.Location = new System.Drawing.Point(87, 60);
            this.cb_PortName.Name = "cb_PortName";
            this.cb_PortName.Size = new System.Drawing.Size(107, 21);
            this.cb_PortName.TabIndex = 10;
            this.cb_PortName.SelectedIndexChanged += new System.EventHandler(this.cb_PortName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Rig:";
            // 
            // cb_Rig
            // 
            this.cb_Rig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Rig.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Rig.FormattingEnabled = true;
            this.cb_Rig.Location = new System.Drawing.Point(87, 33);
            this.cb_Rig.Name = "cb_Rig";
            this.cb_Rig.Size = new System.Drawing.Size(107, 21);
            this.cb_Rig.TabIndex = 8;
            this.cb_Rig.SelectedIndexChanged += new System.EventHandler(this.cb_Rig_SelectedIndexChanged);
            // 
            // gb_RigSettingsFolder
            // 
            this.gb_RigSettingsFolder.Controls.Add(this.btn_RigDefinitions_Folder);
            this.gb_RigSettingsFolder.Controls.Add(this.tb_RigDefinitionsFolder);
            this.gb_RigSettingsFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_RigSettingsFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_RigSettingsFolder.Location = new System.Drawing.Point(0, 24);
            this.gb_RigSettingsFolder.Name = "gb_RigSettingsFolder";
            this.gb_RigSettingsFolder.Size = new System.Drawing.Size(1008, 53);
            this.gb_RigSettingsFolder.TabIndex = 11;
            this.gb_RigSettingsFolder.TabStop = false;
            this.gb_RigSettingsFolder.Text = "Rig Definitions Folder";
            // 
            // btn_RigDefinitions_Folder
            // 
            this.btn_RigDefinitions_Folder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_RigDefinitions_Folder.Location = new System.Drawing.Point(915, 18);
            this.btn_RigDefinitions_Folder.Name = "btn_RigDefinitions_Folder";
            this.btn_RigDefinitions_Folder.Size = new System.Drawing.Size(75, 23);
            this.btn_RigDefinitions_Folder.TabIndex = 2;
            this.btn_RigDefinitions_Folder.Text = "Select";
            this.btn_RigDefinitions_Folder.UseVisualStyleBackColor = true;
            this.btn_RigDefinitions_Folder.Click += new System.EventHandler(this.btn_Configurations_Folder_Click);
            // 
            // tb_RigDefinitionsFolder
            // 
            this.tb_RigDefinitionsFolder.BackColor = System.Drawing.Color.AntiqueWhite;
            this.tb_RigDefinitionsFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::CATCheck.Properties.Settings.Default, "RigDefinitionsFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_RigDefinitionsFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_RigDefinitionsFolder.Location = new System.Drawing.Point(12, 19);
            this.tb_RigDefinitionsFolder.Name = "tb_RigDefinitionsFolder";
            this.tb_RigDefinitionsFolder.ReadOnly = true;
            this.tb_RigDefinitionsFolder.Size = new System.Drawing.Size(897, 20);
            this.tb_RigDefinitionsFolder.TabIndex = 1;
            this.tb_RigDefinitionsFolder.Text = global::CATCheck.Properties.Settings.Default.RigDefinitionsFolder;
            // 
            // ti_Main
            // 
            this.ti_Main.Enabled = true;
            this.ti_Main.Tick += new System.EventHandler(this.ti_Main_Tick);
            // 
            // gb_COM
            // 
            this.gb_COM.Controls.Add(this.lbl_DSR);
            this.gb_COM.Controls.Add(this.lbl_CTS);
            this.gb_COM.Controls.Add(this.lbl_DTR);
            this.gb_COM.Controls.Add(this.lbl_RTS);
            this.gb_COM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_COM.Location = new System.Drawing.Point(944, 77);
            this.gb_COM.Name = "gb_COM";
            this.gb_COM.Size = new System.Drawing.Size(64, 114);
            this.gb_COM.TabIndex = 15;
            this.gb_COM.TabStop = false;
            this.gb_COM.Text = "COM";
            // 
            // lbl_RTS
            // 
            this.lbl_RTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_RTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RTS.Location = new System.Drawing.Point(10, 21);
            this.lbl_RTS.Name = "lbl_RTS";
            this.lbl_RTS.Size = new System.Drawing.Size(41, 19);
            this.lbl_RTS.TabIndex = 0;
            this.lbl_RTS.Text = "RTS";
            this.lbl_RTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_RTS.Click += new System.EventHandler(this.lbl_RTS_Click);
            // 
            // lbl_DTR
            // 
            this.lbl_DTR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_DTR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DTR.Location = new System.Drawing.Point(10, 42);
            this.lbl_DTR.Name = "lbl_DTR";
            this.lbl_DTR.Size = new System.Drawing.Size(41, 19);
            this.lbl_DTR.TabIndex = 1;
            this.lbl_DTR.Text = "DTR";
            this.lbl_DTR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_DTR.Click += new System.EventHandler(this.lbl_DTR_Click);
            // 
            // lbl_CTS
            // 
            this.lbl_CTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CTS.Location = new System.Drawing.Point(10, 63);
            this.lbl_CTS.Name = "lbl_CTS";
            this.lbl_CTS.Size = new System.Drawing.Size(41, 19);
            this.lbl_CTS.TabIndex = 2;
            this.lbl_CTS.Text = "CTS";
            this.lbl_CTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_DSR
            // 
            this.lbl_DSR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_DSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DSR.Location = new System.Drawing.Point(10, 84);
            this.lbl_DSR.Name = "lbl_DSR";
            this.lbl_DSR.Size = new System.Drawing.Size(41, 19);
            this.lbl_DSR.TabIndex = 3;
            this.lbl_DSR.Text = "DSR";
            this.lbl_DSR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.gb_COM);
            this.Controls.Add(this.gb_Log);
            this.Controls.Add(this.gb_RigControl);
            this.Controls.Add(this.gb_RigSettings);
            this.Controls.Add(this.gb_RigSettingsFolder);
            this.Controls.Add(this.mnu_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainDlg";
            this.Text = "CAT- Checker Vxxx (c) 2012 DL2ALF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.Load += new System.EventHandler(this.MainDlg_Load);
            this.mnu_Main.ResumeLayout(false);
            this.mnu_Main.PerformLayout();
            this.gb_Log.ResumeLayout(false);
            this.gb_Log.PerformLayout();
            this.gb_RigControl.ResumeLayout(false);
            this.gb_RigControl.PerformLayout();
            this.gb_RigSettings.ResumeLayout(false);
            this.gb_RigSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ud_TimeoutMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_PollMs)).EndInit();
            this.gb_RigSettingsFolder.ResumeLayout(false);
            this.gb_RigSettingsFolder.PerformLayout();
            this.gb_COM.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip tt_Set;
        private System.Windows.Forms.MenuStrip mnu_Main;
        private System.Windows.Forms.GroupBox gb_Log;
        private System.Windows.Forms.Button btn_Log_Show;
        private System.Windows.Forms.Button btn_Log_Clear;
        private System.Windows.Forms.Button btn_Log_StartStop;
        private System.Windows.Forms.GroupBox gb_RigControl;
        private System.Windows.Forms.Label lbl_FreqB;
        private System.Windows.Forms.Label lbl_FreqA;
        private System.Windows.Forms.Label lbl_Freq;
        private System.Windows.Forms.Label lbl_RIT_OFF;
        private System.Windows.Forms.Label lbl_RIT_ON;
        private System.Windows.Forms.Label lbl_SPLIT_OFF;
        private System.Windows.Forms.Label lbl_SPLIT_ON;
        private System.Windows.Forms.Label lbl_XIT_OFF;
        private System.Windows.Forms.Label lbl_XIT_ON;
        private System.Windows.Forms.Label lbl_VFO_Swap;
        private System.Windows.Forms.Label lbl_VFO_Equal;
        private System.Windows.Forms.Label lbl_VFO_B;
        private System.Windows.Forms.Label lbl_VFO_A;
        private System.Windows.Forms.Label lbl_VFO_AB;
        private System.Windows.Forms.Label lbl_VFO_AA;
        private System.Windows.Forms.Label lbl_VFO_BB;
        private System.Windows.Forms.Label lbl_VFO_BA;
        private System.Windows.Forms.Label lbl_TX;
        private System.Windows.Forms.Label lbl_RX;
        private System.Windows.Forms.Label lbl_FM;
        private System.Windows.Forms.Label lbl_AM;
        private System.Windows.Forms.Label lbl_DIG_L;
        private System.Windows.Forms.Label lbl_DIG_U;
        private System.Windows.Forms.Label lbl_CW_U;
        private System.Windows.Forms.Label lbl_CW_L;
        private System.Windows.Forms.Label lbl_SSB_L;
        private System.Windows.Forms.Label lbl_SSB_U;
        private System.Windows.Forms.TextBox tb_Freq;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_Status;
        private System.Windows.Forms.TextBox tb_FreqA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_FreqB;
        private System.Windows.Forms.GroupBox gb_RigSettings;
        private System.Windows.Forms.Button btn_Restart;
        private System.Windows.Forms.NumericUpDown ud_TimeoutMs;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown ud_PollMs;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cb_DTR;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cb_RTS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cb_Stopbits;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_Parity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cb_Databits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_Baudrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_PortName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_Rig;
        private System.Windows.Forms.GroupBox gb_RigSettingsFolder;
        private System.Windows.Forms.Button btn_RigDefinitions_Folder;
        private System.Windows.Forms.TextBox tb_RigDefinitionsFolder;
        private System.Windows.Forms.ToolStripMenuItem mni_Exit;
        private System.Windows.Forms.ToolStripMenuItem mni_Commands;
        private System.Windows.Forms.ToolStripMenuItem mni_About;
        private System.Windows.Forms.Timer ti_Main;
        private System.Windows.Forms.ImageList il_MessageStates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_LogVerbosity;
        private System.Windows.Forms.Label lbl_Queue;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ListView lv_Messages;
        private System.Windows.Forms.ColumnHeader ch_LogLevel;
        private System.Windows.Forms.ColumnHeader ch_TimeStamp;
        private System.Windows.Forms.ColumnHeader ch_Message;
        private System.Windows.Forms.Label lbl_RIT_OFS;
        private System.Windows.Forms.ToolStripMenuItem mni_ClearRIT;
        private System.Windows.Forms.ToolStripMenuItem mni_FrequencyOfTone;
        private System.Windows.Forms.GroupBox gb_COM;
        private System.Windows.Forms.Label lbl_DSR;
        private System.Windows.Forms.Label lbl_CTS;
        private System.Windows.Forms.Label lbl_DTR;
        private System.Windows.Forms.Label lbl_RTS;
    }
}

