namespace OTBFlashCards
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelDataPath = new System.Windows.Forms.Label();
            this.textBoxDataPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDataPath
            // 
            this.labelDataPath.AutoSize = true;
            this.labelDataPath.Location = new System.Drawing.Point(20, 60);
            this.labelDataPath.Name = "labelDataPath";
            this.labelDataPath.Size = new System.Drawing.Size(120, 15);
            this.labelDataPath.TabIndex = 0;
            this.labelDataPath.Text = "Study Data File Location:";
            // 
            // textBoxDataPath
            // 
            this.textBoxDataPath.Location = new System.Drawing.Point(20, 80);
            this.textBoxDataPath.Name = "textBoxDataPath";
            this.textBoxDataPath.ReadOnly = true;
            this.textBoxDataPath.Size = new System.Drawing.Size(400, 23);
            this.textBoxDataPath.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(430, 79);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 25);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(330, 130);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 30);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(425, 130);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 30);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(20, 15);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(485, 35);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Choose where to store your study data (practice attempts, notes, metrics).\\nThis file will track your progress across all variations.";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 180);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxDataPath);
            this.Controls.Add(this.labelDataPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings - OTB Flash Cards";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelDataPath;
        private System.Windows.Forms.TextBox textBoxDataPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelDescription;
    }
}
