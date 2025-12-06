namespace ChessPuzzleSimulator
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.listBoxGroup = new System.Windows.Forms.ListBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.textBoxFen = new System.Windows.Forms.TextBox();
            this.textBoxMoves = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxPuzzle = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCreateGroup = new System.Windows.Forms.Button();
            this.buttonDeleteGroup = new System.Windows.Forms.Button();
            this.buttonEditGroup = new System.Windows.Forms.Button();
            this.buttonDeletePuzzle = new System.Windows.Forms.Button();
            this.buttonCreateDatabase = new System.Windows.Forms.Button();
            this.buttonOpenDatabase = new System.Windows.Forms.Button();
            this.buttonPlaySelected = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDatabaseFilename = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.buttonPlayPGN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(770, 1042);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxGroup
            // 
            this.listBoxGroup.FormattingEnabled = true;
            this.listBoxGroup.ItemHeight = 25;
            this.listBoxGroup.Location = new System.Drawing.Point(24, 73);
            this.listBoxGroup.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.listBoxGroup.Name = "listBoxGroup";
            this.listBoxGroup.Size = new System.Drawing.Size(468, 579);
            this.listBoxGroup.TabIndex = 1;
            this.listBoxGroup.SelectedIndexChanged += new System.EventHandler(this.listBoxGroup_SelectedIndexChanged);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(556, 667);
            this.buttonCreate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(196, 44);
            this.buttonCreate.TabIndex = 2;
            this.buttonCreate.Text = "Create Puzzle";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // textBoxFen
            // 
            this.textBoxFen.Location = new System.Drawing.Point(136, 865);
            this.textBoxFen.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxFen.Name = "textBoxFen";
            this.textBoxFen.ReadOnly = true;
            this.textBoxFen.Size = new System.Drawing.Size(1028, 31);
            this.textBoxFen.TabIndex = 3;
            this.textBoxFen.Text = "8/ppp2k1K/8/7p/PP6/2P5/1r4PP/8 w - - 2 32";
            // 
            // textBoxMoves
            // 
            this.textBoxMoves.Location = new System.Drawing.Point(136, 921);
            this.textBoxMoves.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxMoves.Name = "textBoxMoves";
            this.textBoxMoves.ReadOnly = true;
            this.textBoxMoves.Size = new System.Drawing.Size(1028, 31);
            this.textBoxMoves.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Group";
            // 
            // listBoxPuzzle
            // 
            this.listBoxPuzzle.FormattingEnabled = true;
            this.listBoxPuzzle.ItemHeight = 25;
            this.listBoxPuzzle.Location = new System.Drawing.Point(556, 73);
            this.listBoxPuzzle.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.listBoxPuzzle.Name = "listBoxPuzzle";
            this.listBoxPuzzle.Size = new System.Drawing.Size(468, 579);
            this.listBoxPuzzle.TabIndex = 6;
            this.listBoxPuzzle.SelectedIndexChanged += new System.EventHandler(this.listBoxPuzzle_SelectedIndexChanged);
            this.listBoxPuzzle.DoubleClick += new System.EventHandler(this.listBoxPuzzle_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Puzzle";
            // 
            // buttonCreateGroup
            // 
            this.buttonCreateGroup.Location = new System.Drawing.Point(30, 667);
            this.buttonCreateGroup.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonCreateGroup.Name = "buttonCreateGroup";
            this.buttonCreateGroup.Size = new System.Drawing.Size(222, 44);
            this.buttonCreateGroup.TabIndex = 8;
            this.buttonCreateGroup.Text = "Create Group";
            this.buttonCreateGroup.UseVisualStyleBackColor = true;
            this.buttonCreateGroup.Click += new System.EventHandler(this.buttonCreateGroup_Click);
            // 
            // buttonDeleteGroup
            // 
            this.buttonDeleteGroup.Location = new System.Drawing.Point(264, 667);
            this.buttonDeleteGroup.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonDeleteGroup.Name = "buttonDeleteGroup";
            this.buttonDeleteGroup.Size = new System.Drawing.Size(222, 44);
            this.buttonDeleteGroup.TabIndex = 9;
            this.buttonDeleteGroup.Text = "Delete Group";
            this.buttonDeleteGroup.UseVisualStyleBackColor = true;
            this.buttonDeleteGroup.Click += new System.EventHandler(this.buttonDeleteGroup_Click);
            // 
            // buttonEditGroup
            // 
            this.buttonEditGroup.Location = new System.Drawing.Point(30, 723);
            this.buttonEditGroup.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonEditGroup.Name = "buttonEditGroup";
            this.buttonEditGroup.Size = new System.Drawing.Size(222, 44);
            this.buttonEditGroup.TabIndex = 10;
            this.buttonEditGroup.Text = "Edit Group";
            this.buttonEditGroup.UseVisualStyleBackColor = true;
            this.buttonEditGroup.Click += new System.EventHandler(this.buttonEditGroup_Click);
            // 
            // buttonDeletePuzzle
            // 
            this.buttonDeletePuzzle.Location = new System.Drawing.Point(806, 667);
            this.buttonDeletePuzzle.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonDeletePuzzle.Name = "buttonDeletePuzzle";
            this.buttonDeletePuzzle.Size = new System.Drawing.Size(222, 44);
            this.buttonDeletePuzzle.TabIndex = 11;
            this.buttonDeletePuzzle.Text = "Delete Puzzle";
            this.buttonDeletePuzzle.UseVisualStyleBackColor = true;
            this.buttonDeletePuzzle.Click += new System.EventHandler(this.buttonDeletePuzzle_Click);
            // 
            // buttonCreateDatabase
            // 
            this.buttonCreateDatabase.Location = new System.Drawing.Point(30, 1060);
            this.buttonCreateDatabase.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonCreateDatabase.Name = "buttonCreateDatabase";
            this.buttonCreateDatabase.Size = new System.Drawing.Size(222, 44);
            this.buttonCreateDatabase.TabIndex = 14;
            this.buttonCreateDatabase.Text = "Create Database";
            this.buttonCreateDatabase.UseVisualStyleBackColor = true;
            this.buttonCreateDatabase.Click += new System.EventHandler(this.buttonSetDatabase_Click);
            // 
            // buttonOpenDatabase
            // 
            this.buttonOpenDatabase.Location = new System.Drawing.Point(274, 1060);
            this.buttonOpenDatabase.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonOpenDatabase.Name = "buttonOpenDatabase";
            this.buttonOpenDatabase.Size = new System.Drawing.Size(222, 44);
            this.buttonOpenDatabase.TabIndex = 15;
            this.buttonOpenDatabase.Text = "Open Database";
            this.buttonOpenDatabase.UseVisualStyleBackColor = true;
            this.buttonOpenDatabase.Click += new System.EventHandler(this.buttonOpenDatabase_Click);
            // 
            // buttonPlaySelected
            // 
            this.buttonPlaySelected.Location = new System.Drawing.Point(1084, 138);
            this.buttonPlaySelected.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonPlaySelected.Name = "buttonPlaySelected";
            this.buttonPlaySelected.Size = new System.Drawing.Size(232, 198);
            this.buttonPlaySelected.TabIndex = 16;
            this.buttonPlaySelected.Text = "Play Puzzle!";
            this.buttonPlaySelected.UseVisualStyleBackColor = true;
            this.buttonPlaySelected.Click += new System.EventHandler(this.buttonPlaySelected_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(556, 723);
            this.button2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(196, 44);
            this.button2.TabIndex = 17;
            this.button2.Text = "Edit Puzzle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 975);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Database";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 865);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 25);
            this.label4.TabIndex = 18;
            this.label4.Text = "FEN";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 921);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 25);
            this.label5.TabIndex = 19;
            this.label5.Text = "Moves";
            // 
            // textBoxDatabaseFilename
            // 
            this.textBoxDatabaseFilename.Location = new System.Drawing.Point(136, 971);
            this.textBoxDatabaseFilename.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBoxDatabaseFilename.Name = "textBoxDatabaseFilename";
            this.textBoxDatabaseFilename.ReadOnly = true;
            this.textBoxDatabaseFilename.Size = new System.Drawing.Size(1028, 31);
            this.textBoxDatabaseFilename.TabIndex = 20;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1084, 504);
            this.button3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(192, 44);
            this.button3.TabIndex = 21;
            this.button3.Text = "Load PGN File";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonPlayPGN
            // 
            this.buttonPlayPGN.Location = new System.Drawing.Point(1084, 592);
            this.buttonPlayPGN.Name = "buttonPlayPGN";
            this.buttonPlayPGN.Size = new System.Drawing.Size(183, 60);
            this.buttonPlayPGN.TabIndex = 22;
            this.buttonPlayPGN.Text = "Play PGN";
            this.buttonPlayPGN.UseVisualStyleBackColor = true;
            this.buttonPlayPGN.Click += new System.EventHandler(this.buttonPlayPGN_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 1127);
            this.Controls.Add(this.buttonPlayPGN);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxDatabaseFilename);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonPlaySelected);
            this.Controls.Add(this.buttonOpenDatabase);
            this.Controls.Add(this.buttonCreateDatabase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDeletePuzzle);
            this.Controls.Add(this.buttonEditGroup);
            this.Controls.Add(this.buttonDeleteGroup);
            this.Controls.Add(this.buttonCreateGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxPuzzle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMoves);
            this.Controls.Add(this.textBoxFen);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.listBoxGroup);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBoxGroup;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.TextBox textBoxFen;
        private System.Windows.Forms.TextBox textBoxMoves;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxPuzzle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCreateGroup;
        private System.Windows.Forms.Button buttonDeleteGroup;
        private System.Windows.Forms.Button buttonEditGroup;
        private System.Windows.Forms.Button buttonDeletePuzzle;
        private System.Windows.Forms.Button buttonCreateDatabase;
        private System.Windows.Forms.Button buttonOpenDatabase;
        private System.Windows.Forms.Button buttonPlaySelected;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDatabaseFilename;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonPlayPGN;
    }
}

