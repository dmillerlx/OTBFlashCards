using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessPuzzleSimulator
{
    public partial class CreateEditGroup : Form
    {
        public CreateEditGroup(string name, string description)
        {
            this.textBoxDescription.Text = description;
            this.textBoxDescription.Text = name;
            InitializeComponent();
        }
        public CreateEditGroup()
        {
            InitializeComponent();
        }

        public string GroupDescription { get { return this.textBoxDescription.Text; } }
        public string GroupName { get { return this.textBoxName.Text; }}

        private void buttonOk_Click(object sender, EventArgs e)
        {
            CloseForm(DialogResult.OK);
        }

        private void CreateEditGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CloseForm(DialogResult.Cancel);
        }

        private void CloseForm(DialogResult result)
        {
            this.DialogResult = result;
            this.Close();
        }
        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyDown(e);
        }

        private void handleKeyDown (KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CloseForm(DialogResult.OK);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CloseForm(DialogResult.Cancel);
            }
        }

        private void textBoxDescription_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyDown(e);
        }
    }
}
