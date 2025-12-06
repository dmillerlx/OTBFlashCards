namespace ChessPuzzleSimulator
{
    partial class PuzzleTrainerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PuzzleTrainerForm));
            this.buttonPlayPGN = new System.Windows.Forms.Button();
            this.buttonLoadPGN = new System.Windows.Forms.Button();
            this.listBoxPGNFiles = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBoxPGNGames = new System.Windows.Forms.ListBox();
            this.buttonTestPGN = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.textBoxPGNInfo = new System.Windows.Forms.TextBox();
            this.checkBoxAllVariations = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoNextPuzzle = new System.Windows.Forms.CheckBox();
            this.buttonTrainPGN = new System.Windows.Forms.Button();
            this.buttonTrainPGNWhite = new System.Windows.Forms.Button();
            this.buttonTrainPGNBlack = new System.Windows.Forms.Button();
            this.buttonTestAsBlack = new System.Windows.Forms.Button();
            this.buttonTestAsWhite = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPGNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadOpenConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetPuzzleScreenLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxPGNFiles = new System.Windows.Forms.GroupBox();
            this.groupBoxPGNGames = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxMultiUserMove = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timerLicenseCheck = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBoxPGNFiles.SuspendLayout();
            this.groupBoxPGNGames.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPlayPGN
            // 
            this.buttonPlayPGN.Location = new System.Drawing.Point(5, 18);
            this.buttonPlayPGN.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlayPGN.Name = "buttonPlayPGN";
            this.buttonPlayPGN.Size = new System.Drawing.Size(92, 31);
            this.buttonPlayPGN.TabIndex = 24;
            this.buttonPlayPGN.Text = "Review PGN";
            this.buttonPlayPGN.UseVisualStyleBackColor = true;
            this.buttonPlayPGN.Click += new System.EventHandler(this.buttonPlayPGN_Click);
            // 
            // buttonLoadPGN
            // 
            this.buttonLoadPGN.Location = new System.Drawing.Point(6, 328);
            this.buttonLoadPGN.Name = "buttonLoadPGN";
            this.buttonLoadPGN.Size = new System.Drawing.Size(96, 23);
            this.buttonLoadPGN.TabIndex = 23;
            this.buttonLoadPGN.Text = "Load PGN File";
            this.buttonLoadPGN.UseVisualStyleBackColor = true;
            this.buttonLoadPGN.Click += new System.EventHandler(this.buttonLoadPGN_Click);
            // 
            // listBoxPGNFiles
            // 
            this.listBoxPGNFiles.FormattingEnabled = true;
            this.listBoxPGNFiles.Location = new System.Drawing.Point(5, 18);
            this.listBoxPGNFiles.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxPGNFiles.Name = "listBoxPGNFiles";
            this.listBoxPGNFiles.Size = new System.Drawing.Size(254, 303);
            this.listBoxPGNFiles.TabIndex = 25;
            this.listBoxPGNFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxPGNFiles_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(108, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Remove Selected PGN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxPGNGames
            // 
            this.listBoxPGNGames.FormattingEnabled = true;
            this.listBoxPGNGames.Location = new System.Drawing.Point(5, 18);
            this.listBoxPGNGames.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxPGNGames.Name = "listBoxPGNGames";
            this.listBoxPGNGames.Size = new System.Drawing.Size(265, 329);
            this.listBoxPGNGames.TabIndex = 31;
            this.listBoxPGNGames.SelectedIndexChanged += new System.EventHandler(this.listBoxPGNGames_SelectedIndexChanged);
            this.listBoxPGNGames.DoubleClick += new System.EventHandler(this.listBoxPGNGames_DoubleClick_1);
            // 
            // buttonTestPGN
            // 
            this.buttonTestPGN.Location = new System.Drawing.Point(3, 19);
            this.buttonTestPGN.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTestPGN.Name = "buttonTestPGN";
            this.buttonTestPGN.Size = new System.Drawing.Size(92, 65);
            this.buttonTestPGN.TabIndex = 32;
            this.buttonTestPGN.Text = "TEST PGN\r\n\r\nAuto Color";
            this.buttonTestPGN.UseVisualStyleBackColor = true;
            this.buttonTestPGN.Click += new System.EventHandler(this.buttonTestPGN_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(5, 18);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(866, 161);
            this.textBoxLog.TabIndex = 33;
            this.textBoxLog.WordWrap = false;
            // 
            // textBoxPGNInfo
            // 
            this.textBoxPGNInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPGNInfo.Location = new System.Drawing.Point(5, 18);
            this.textBoxPGNInfo.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPGNInfo.Multiline = true;
            this.textBoxPGNInfo.Name = "textBoxPGNInfo";
            this.textBoxPGNInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPGNInfo.Size = new System.Drawing.Size(305, 327);
            this.textBoxPGNInfo.TabIndex = 38;
            this.textBoxPGNInfo.WordWrap = false;
            // 
            // checkBoxAllVariations
            // 
            this.checkBoxAllVariations.AutoSize = true;
            this.checkBoxAllVariations.Location = new System.Drawing.Point(5, 18);
            this.checkBoxAllVariations.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAllVariations.Name = "checkBoxAllVariations";
            this.checkBoxAllVariations.Size = new System.Drawing.Size(86, 17);
            this.checkBoxAllVariations.TabIndex = 40;
            this.checkBoxAllVariations.Text = "All Variations";
            this.checkBoxAllVariations.UseVisualStyleBackColor = true;
            this.checkBoxAllVariations.CheckedChanged += new System.EventHandler(this.checkBoxAllVariations_CheckedChanged);
            // 
            // checkBoxAutoNextPuzzle
            // 
            this.checkBoxAutoNextPuzzle.AutoSize = true;
            this.checkBoxAutoNextPuzzle.Location = new System.Drawing.Point(5, 39);
            this.checkBoxAutoNextPuzzle.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAutoNextPuzzle.Name = "checkBoxAutoNextPuzzle";
            this.checkBoxAutoNextPuzzle.Size = new System.Drawing.Size(155, 17);
            this.checkBoxAutoNextPuzzle.TabIndex = 41;
            this.checkBoxAutoNextPuzzle.Text = "Auto Next Puzzle (test only)";
            this.checkBoxAutoNextPuzzle.UseVisualStyleBackColor = true;
            this.checkBoxAutoNextPuzzle.CheckedChanged += new System.EventHandler(this.checkBoxAutoNextPuzzle_CheckedChanged);
            // 
            // buttonTrainPGN
            // 
            this.buttonTrainPGN.Location = new System.Drawing.Point(5, 19);
            this.buttonTrainPGN.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTrainPGN.Name = "buttonTrainPGN";
            this.buttonTrainPGN.Size = new System.Drawing.Size(92, 65);
            this.buttonTrainPGN.TabIndex = 42;
            this.buttonTrainPGN.Text = "TRAIN PGN\r\n\r\nAuto Color";
            this.buttonTrainPGN.UseVisualStyleBackColor = true;
            this.buttonTrainPGN.Click += new System.EventHandler(this.buttonTrainPGN_Click);
            // 
            // buttonTrainPGNWhite
            // 
            this.buttonTrainPGNWhite.Location = new System.Drawing.Point(101, 18);
            this.buttonTrainPGNWhite.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTrainPGNWhite.Name = "buttonTrainPGNWhite";
            this.buttonTrainPGNWhite.Size = new System.Drawing.Size(92, 31);
            this.buttonTrainPGNWhite.TabIndex = 43;
            this.buttonTrainPGNWhite.Text = "Train as White";
            this.buttonTrainPGNWhite.UseVisualStyleBackColor = true;
            this.buttonTrainPGNWhite.Click += new System.EventHandler(this.buttonTrainPGNWhite_Click);
            // 
            // buttonTrainPGNBlack
            // 
            this.buttonTrainPGNBlack.Location = new System.Drawing.Point(101, 53);
            this.buttonTrainPGNBlack.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTrainPGNBlack.Name = "buttonTrainPGNBlack";
            this.buttonTrainPGNBlack.Size = new System.Drawing.Size(92, 31);
            this.buttonTrainPGNBlack.TabIndex = 44;
            this.buttonTrainPGNBlack.Text = "Train as Black";
            this.buttonTrainPGNBlack.UseVisualStyleBackColor = true;
            this.buttonTrainPGNBlack.Click += new System.EventHandler(this.buttonTrainPGNBlack_Click);
            // 
            // buttonTestAsBlack
            // 
            this.buttonTestAsBlack.Location = new System.Drawing.Point(108, 53);
            this.buttonTestAsBlack.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTestAsBlack.Name = "buttonTestAsBlack";
            this.buttonTestAsBlack.Size = new System.Drawing.Size(92, 31);
            this.buttonTestAsBlack.TabIndex = 45;
            this.buttonTestAsBlack.Text = "Test as Black";
            this.buttonTestAsBlack.UseVisualStyleBackColor = true;
            this.buttonTestAsBlack.Click += new System.EventHandler(this.buttonTestAsBlack_Click);
            // 
            // buttonTestAsWhite
            // 
            this.buttonTestAsWhite.Location = new System.Drawing.Point(108, 19);
            this.buttonTestAsWhite.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTestAsWhite.Name = "buttonTestAsWhite";
            this.buttonTestAsWhite.Size = new System.Drawing.Size(92, 31);
            this.buttonTestAsWhite.TabIndex = 46;
            this.buttonTestAsWhite.Text = "Test as White";
            this.buttonTestAsWhite.UseVisualStyleBackColor = true;
            this.buttonTestAsWhite.Click += new System.EventHandler(this.buttonTestAsWhite_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configToolStripMenuItem,
            this.logToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(894, 24);
            this.menuStrip1.TabIndex = 47;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPGNToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadPGNToolStripMenuItem
            // 
            this.loadPGNToolStripMenuItem.Name = "loadPGNToolStripMenuItem";
            this.loadPGNToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.loadPGNToolStripMenuItem.Text = "Load PGN...";
            this.loadPGNToolStripMenuItem.Click += new System.EventHandler(this.loadPGNToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createConfigToolStripMenuItem,
            this.loadOpenConfigToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // createConfigToolStripMenuItem
            // 
            this.createConfigToolStripMenuItem.Name = "createConfigToolStripMenuItem";
            this.createConfigToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.createConfigToolStripMenuItem.Text = "Create Config...";
            this.createConfigToolStripMenuItem.Click += new System.EventHandler(this.createConfigToolStripMenuItem_Click);
            // 
            // loadOpenConfigToolStripMenuItem
            // 
            this.loadOpenConfigToolStripMenuItem.Name = "loadOpenConfigToolStripMenuItem";
            this.loadOpenConfigToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.loadOpenConfigToolStripMenuItem.Text = "Load/Open Config...";
            this.loadOpenConfigToolStripMenuItem.Click += new System.EventHandler(this.loadOpenConfigToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem});
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.logToolStripMenuItem.Text = "Log";
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.resetPuzzleScreenLocationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // resetPuzzleScreenLocationToolStripMenuItem
            // 
            this.resetPuzzleScreenLocationToolStripMenuItem.Name = "resetPuzzleScreenLocationToolStripMenuItem";
            this.resetPuzzleScreenLocationToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.resetPuzzleScreenLocationToolStripMenuItem.Text = "Reset Puzzle Screen Location...";
            this.resetPuzzleScreenLocationToolStripMenuItem.Click += new System.EventHandler(this.resetPuzzleScreenLocationToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 735);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(894, 22);
            this.statusStrip.TabIndex = 48;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // groupBoxPGNFiles
            // 
            this.groupBoxPGNFiles.Controls.Add(this.listBoxPGNFiles);
            this.groupBoxPGNFiles.Controls.Add(this.buttonLoadPGN);
            this.groupBoxPGNFiles.Controls.Add(this.button1);
            this.groupBoxPGNFiles.Location = new System.Drawing.Point(8, 35);
            this.groupBoxPGNFiles.Name = "groupBoxPGNFiles";
            this.groupBoxPGNFiles.Size = new System.Drawing.Size(274, 358);
            this.groupBoxPGNFiles.TabIndex = 49;
            this.groupBoxPGNFiles.TabStop = false;
            this.groupBoxPGNFiles.Text = "PGN Files";
            // 
            // groupBoxPGNGames
            // 
            this.groupBoxPGNGames.Controls.Add(this.listBoxPGNGames);
            this.groupBoxPGNGames.Location = new System.Drawing.Point(288, 35);
            this.groupBoxPGNGames.Name = "groupBoxPGNGames";
            this.groupBoxPGNGames.Size = new System.Drawing.Size(277, 356);
            this.groupBoxPGNGames.TabIndex = 50;
            this.groupBoxPGNGames.TabStop = false;
            this.groupBoxPGNGames.Text = "PGN Games";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBoxPGNInfo);
            this.groupBox3.Location = new System.Drawing.Point(571, 35);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(315, 356);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PGN Info";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.textBoxLog);
            this.groupBox4.Location = new System.Drawing.Point(8, 535);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(878, 184);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Log";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonTestPGN);
            this.groupBox1.Controls.Add(this.buttonTestAsBlack);
            this.groupBox1.Controls.Add(this.buttonTestAsWhite);
            this.groupBox1.Location = new System.Drawing.Point(8, 399);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 130);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Test moves as white or black. \r\n";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonTrainPGN);
            this.groupBox2.Controls.Add(this.buttonTrainPGNWhite);
            this.groupBox2.Controls.Add(this.buttonTrainPGNBlack);
            this.groupBox2.Location = new System.Drawing.Point(221, 399);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(201, 130);
            this.groupBox2.TabIndex = 54;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Train";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 26);
            this.label3.TabIndex = 57;
            this.label3.Text = "Train by showing next move and then\r\nyou make the move.\r\n";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBoxMultiUserMove);
            this.groupBox5.Controls.Add(this.checkBoxAllVariations);
            this.groupBox5.Controls.Add(this.checkBoxAutoNextPuzzle);
            this.groupBox5.Location = new System.Drawing.Point(638, 400);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(175, 129);
            this.groupBox5.TabIndex = 55;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Options";
            // 
            // checkBoxMultiUserMove
            // 
            this.checkBoxMultiUserMove.AutoSize = true;
            this.checkBoxMultiUserMove.Location = new System.Drawing.Point(5, 60);
            this.checkBoxMultiUserMove.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxMultiUserMove.Name = "checkBoxMultiUserMove";
            this.checkBoxMultiUserMove.Size = new System.Drawing.Size(133, 17);
            this.checkBoxMultiUserMove.TabIndex = 42;
            this.checkBoxMultiUserMove.Text = "Notify Multi User Move";
            this.checkBoxMultiUserMove.UseVisualStyleBackColor = true;
            this.checkBoxMultiUserMove.CheckedChanged += new System.EventHandler(this.checkBoxMultiUserMove_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.buttonPlayPGN);
            this.groupBox6.Location = new System.Drawing.Point(432, 400);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 129);
            this.groupBox6.TabIndex = 56;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Review";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 26);
            this.label1.TabIndex = 25;
            this.label1.Text = "Play through PGN using arrow keys.\r\nFlip board by pressing: f\r\n";
            // 
            // timerLicenseCheck
            // 
            this.timerLicenseCheck.Enabled = true;
            this.timerLicenseCheck.Interval = 1000;
            this.timerLicenseCheck.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // PuzzleTrainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 757);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxPGNGames);
            this.Controls.Add(this.groupBoxPGNFiles);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PuzzleTrainerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chess Puzzle Simulator";
            this.Shown += new System.EventHandler(this.PuzzleTrainerForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBoxPGNFiles.ResumeLayout(false);
            this.groupBoxPGNGames.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPlayPGN;
        private System.Windows.Forms.Button buttonLoadPGN;
        private System.Windows.Forms.ListBox listBoxPGNFiles;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBoxPGNGames;
        private System.Windows.Forms.Button buttonTestPGN;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.TextBox textBoxPGNInfo;
        private System.Windows.Forms.CheckBox checkBoxAllVariations;
        private System.Windows.Forms.CheckBox checkBoxAutoNextPuzzle;
        private System.Windows.Forms.Button buttonTrainPGN;
        private System.Windows.Forms.Button buttonTrainPGNWhite;
        private System.Windows.Forms.Button buttonTrainPGNBlack;
        private System.Windows.Forms.Button buttonTestAsBlack;
        private System.Windows.Forms.Button buttonTestAsWhite;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPGNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadOpenConfigToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBoxPGNFiles;
        private System.Windows.Forms.GroupBox groupBoxPGNGames;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Timer timerLicenseCheck;
        private System.Windows.Forms.ToolStripMenuItem resetPuzzleScreenLocationToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxMultiUserMove;
    }
}