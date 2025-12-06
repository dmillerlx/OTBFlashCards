using System;
using System.Windows.Forms;

namespace OTBFlashCards
{
    public partial class SetDepthDialog : Form
    {
        public int? DepthLimit { get; private set; }
        public bool IgnoreDepthForMainline { get; private set; }

        private TextBox textBoxDepth;
        private CheckBox checkBoxIgnoreMainline;
        private Button buttonOK;
        private Button buttonCancel;
        private Button buttonClear;
        private Label labelPrompt;
        private Label labelInfo;

        public SetDepthDialog(int? currentDepth, bool currentIgnoreMainline, int maxMoves)
        {
            InitializeComponent();
            
            textBoxDepth.Text = currentDepth?.ToString() ?? "";
            checkBoxIgnoreMainline.Checked = currentIgnoreMainline;
            labelInfo.Text = $"Valid range: 1-{maxMoves} moves";
        }

        private void InitializeComponent()
        {
            this.labelPrompt = new Label();
            this.labelInfo = new Label();
            this.textBoxDepth = new TextBox();
            this.checkBoxIgnoreMainline = new CheckBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.buttonClear = new Button();
            this.SuspendLayout();
            
            // labelPrompt
            this.labelPrompt.AutoSize = true;
            this.labelPrompt.Location = new System.Drawing.Point(20, 20);
            this.labelPrompt.Name = "labelPrompt";
            this.labelPrompt.Size = new System.Drawing.Size(200, 15);
            this.labelPrompt.Text = "Set depth limit (move numbers):";
            
            // labelInfo
            this.labelInfo.AutoSize = true;
            this.labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo.Location = new System.Drawing.Point(20, 40);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(150, 15);
            this.labelInfo.Text = "Valid range: 1-X moves";
            
            // textBoxDepth
            this.textBoxDepth.Location = new System.Drawing.Point(20, 65);
            this.textBoxDepth.Name = "textBoxDepth";
            this.textBoxDepth.Size = new System.Drawing.Size(100, 23);
            this.textBoxDepth.TabIndex = 0;
            
            // checkBoxIgnoreMainline
            this.checkBoxIgnoreMainline.AutoSize = true;
            this.checkBoxIgnoreMainline.Location = new System.Drawing.Point(20, 100);
            this.checkBoxIgnoreMainline.Name = "checkBoxIgnoreMainline";
            this.checkBoxIgnoreMainline.Size = new System.Drawing.Size(250, 19);
            this.checkBoxIgnoreMainline.TabIndex = 1;
            this.checkBoxIgnoreMainline.Text = "Ignore depth limit for mainline variations";
            this.checkBoxIgnoreMainline.UseVisualStyleBackColor = true;
            
            // buttonOK
            this.buttonOK.Location = new System.Drawing.Point(80, 140);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            
            // buttonClear
            this.buttonClear.Location = new System.Drawing.Point(165, 140);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 30);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            
            // buttonCancel
            this.buttonCancel.Location = new System.Drawing.Point(250, 140);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            
            // SetDepthDialog
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 190);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxIgnoreMainline);
            this.Controls.Add(this.textBoxDepth);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.labelPrompt);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetDepthDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Set Depth Limit";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDepth.Text))
            {
                DepthLimit = null;
                IgnoreDepthForMainline = checkBoxIgnoreMainline.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            if (int.TryParse(textBoxDepth.Text, out int depth) && depth > 0)
            {
                DepthLimit = depth;
                IgnoreDepthForMainline = checkBoxIgnoreMainline.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid positive number or leave blank to remove limit.",
                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            DepthLimit = null;
            IgnoreDepthForMainline = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
