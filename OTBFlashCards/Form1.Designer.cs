namespace OTBFlashCards
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new Panel();
            buttonPractice = new Button();
            buttonRemoveFile = new Button();
            buttonAddFiles = new Button();
            buttonSettings = new Button();
            buttonMetrics = new Button();
            panelMain = new Panel();
            listBoxVariations = new ListBox();
            labelVariations = new Label();
            listBoxGames = new ListBox();
            labelGames = new Label();
            listBoxFiles = new ListBox();
            labelFiles = new Label();
            panelTop.SuspendLayout();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(buttonMetrics);
            panelTop.Controls.Add(buttonSettings);
            panelTop.Controls.Add(buttonPractice);
            panelTop.Controls.Add(buttonRemoveFile);
            panelTop.Controls.Add(buttonAddFiles);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(10);
            panelTop.Size = new Size(1100, 60);
            panelTop.TabIndex = 0;
            // 
            // buttonPractice
            // 
            buttonPractice.Location = new Point(280, 15);
            buttonPractice.Name = "buttonPractice";
            buttonPractice.Size = new Size(120, 30);
            buttonPractice.TabIndex = 2;
            buttonPractice.Text = "Practice Selected";
            buttonPractice.UseVisualStyleBackColor = true;
            buttonPractice.Click += buttonPractice_Click;
            // 
            // buttonRemoveFile
            // 
            buttonRemoveFile.Location = new Point(145, 15);
            buttonRemoveFile.Name = "buttonRemoveFile";
            buttonRemoveFile.Size = new Size(120, 30);
            buttonRemoveFile.TabIndex = 1;
            buttonRemoveFile.Text = "Remove Selected";
            buttonRemoveFile.UseVisualStyleBackColor = true;
            buttonRemoveFile.Click += buttonRemoveFile_Click;
            // 
            // buttonAddFiles
            // 
            buttonAddFiles.Location = new Point(10, 15);
            buttonAddFiles.Name = "buttonAddFiles";
            buttonAddFiles.Size = new Size(120, 30);
            buttonAddFiles.TabIndex = 0;
            buttonAddFiles.Text = "Add Files";
            buttonAddFiles.UseVisualStyleBackColor = true;
            buttonAddFiles.Click += buttonAddFiles_Click;
            // 
            // buttonSettings
            // 
            buttonSettings.Location = new Point(410, 15);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(100, 30);
            buttonSettings.TabIndex = 3;
            buttonSettings.Text = "⚙ Settings";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // buttonMetrics
            // 
            buttonMetrics.Location = new Point(520, 15);
            buttonMetrics.Name = "buttonMetrics";
            buttonMetrics.Size = new Size(110, 30);
            buttonMetrics.TabIndex = 4;
            buttonMetrics.Text = "📊 Metrics";
            buttonMetrics.UseVisualStyleBackColor = true;
            buttonMetrics.Click += buttonMetrics_Click;
            // 
            // panelMain
            // 
            panelMain.Controls.Add(listBoxVariations);
            panelMain.Controls.Add(labelVariations);
            panelMain.Controls.Add(listBoxGames);
            panelMain.Controls.Add(labelGames);
            panelMain.Controls.Add(listBoxFiles);
            panelMain.Controls.Add(labelFiles);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 60);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(10);
            panelMain.Size = new Size(1100, 590);
            panelMain.TabIndex = 1;
            // 
            // listBoxVariations
            // 
            listBoxVariations.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxVariations.Font = new Font("Consolas", 9F);
            listBoxVariations.FormattingEnabled = true;
            listBoxVariations.Location = new Point(810, 35);
            listBoxVariations.Name = "listBoxVariations";
            listBoxVariations.Size = new Size(280, 536);
            listBoxVariations.TabIndex = 5;
            listBoxVariations.DoubleClick += listBoxVariations_DoubleClick;
            // 
            // labelVariations
            // 
            labelVariations.AutoSize = true;
            labelVariations.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelVariations.Location = new Point(810, 10);
            labelVariations.Name = "labelVariations";
            labelVariations.Size = new Size(75, 19);
            labelVariations.TabIndex = 4;
            labelVariations.Text = "Variations";
            // 
            // listBoxGames
            // 
            listBoxGames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxGames.FormattingEnabled = true;
            listBoxGames.Location = new Point(410, 35);
            listBoxGames.Name = "listBoxGames";
            listBoxGames.Size = new Size(380, 529);
            listBoxGames.TabIndex = 3;
            listBoxGames.SelectedIndexChanged += listBoxGames_SelectedIndexChanged;
            // 
            // labelGames
            // 
            labelGames.AutoSize = true;
            labelGames.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelGames.Location = new Point(410, 10);
            labelGames.Name = "labelGames";
            labelGames.Size = new Size(97, 19);
            labelGames.TabIndex = 2;
            labelGames.Text = "Games in File";
            // 
            // listBoxFiles
            // 
            listBoxFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxFiles.FormattingEnabled = true;
            listBoxFiles.Location = new Point(10, 35);
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Size = new Size(380, 529);
            listBoxFiles.TabIndex = 1;
            listBoxFiles.SelectedIndexChanged += listBoxFiles_SelectedIndexChanged;
            // 
            // labelFiles
            // 
            labelFiles.AutoSize = true;
            labelFiles.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelFiles.Location = new Point(10, 10);
            labelFiles.Name = "labelFiles";
            labelFiles.Size = new Size(72, 19);
            labelFiles.TabIndex = 0;
            labelFiles.Text = "PGN Files";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(panelMain);
            Controls.Add(panelTop);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OTB Flash Cards - File Manager";
            panelTop.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonPractice;
        private System.Windows.Forms.Button buttonRemoveFile;
        private System.Windows.Forms.Button buttonAddFiles;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonMetrics;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ListBox listBoxVariations;
        private System.Windows.Forms.Label labelVariations;
        private System.Windows.Forms.ListBox listBoxGames;
        private System.Windows.Forms.Label labelGames;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Label labelFiles;
    }
}
