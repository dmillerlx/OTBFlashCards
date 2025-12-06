namespace PGNtoFlashCard
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
            textBoxInput = new TextBox();
            textBoxOutput = new TextBox();
            buttonGo = new Button();
            buttonCopy = new Button();
            buttonClear = new Button();
            SuspendLayout();
            // 
            // textBoxInput
            // 
            textBoxInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            textBoxInput.Location = new Point(12, 12);
            textBoxInput.Multiline = true;
            textBoxInput.Name = "textBoxInput";
            textBoxInput.ScrollBars = ScrollBars.Vertical;
            textBoxInput.Size = new Size(385, 477);
            textBoxInput.TabIndex = 0;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxOutput.Location = new Point(484, 12);
            textBoxOutput.Multiline = true;
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.ScrollBars = ScrollBars.Vertical;
            textBoxOutput.Size = new Size(373, 477);
            textBoxOutput.TabIndex = 1;
            // 
            // buttonGo
            // 
            buttonGo.Location = new Point(403, 110);
            buttonGo.Name = "buttonGo";
            buttonGo.Size = new Size(75, 65);
            buttonGo.TabIndex = 2;
            buttonGo.Text = "Extract Variations -->";
            buttonGo.UseVisualStyleBackColor = true;
            buttonGo.Click += buttonGo_Click;
            // 
            // buttonCopy
            // 
            buttonCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCopy.Location = new Point(782, 495);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new Size(75, 23);
            buttonCopy.TabIndex = 3;
            buttonCopy.Text = "Copy";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // buttonClear
            // 
            buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonClear.Location = new Point(12, 495);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(75, 23);
            buttonClear.TabIndex = 4;
            buttonClear.Text = "Clear";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(869, 530);
            Controls.Add(buttonClear);
            Controls.Add(buttonCopy);
            Controls.Add(buttonGo);
            Controls.Add(textBoxOutput);
            Controls.Add(textBoxInput);
            Name = "Form1";
            Text = "PGNtoFlashCard";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxInput;
        private TextBox textBoxOutput;
        private Button buttonGo;
        private Button buttonCopy;
        private Button buttonClear;
    }
}
