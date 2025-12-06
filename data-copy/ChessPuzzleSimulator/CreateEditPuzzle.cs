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
    public partial class CreateEditPuzzle : Form
    {
        public CreateEditPuzzle()
        {
            InitializeComponent();
        }


        public CreateEditPuzzle(string name, string description, string fen, string moves)
        {

            InitializeComponent();
            this.textBoxDescription.Text = description;
            this.textBoxName.Text = name;
            this.textBoxFEN.Text = fen;
            this.textBoxMoves.Text = moves;
        }


        public string PuzzleDescription { get { return this.textBoxDescription.Text; } }
        public string PuzzleName { get { return this.textBoxName.Text; } }
        public string FEN { get { return this.textBoxFEN.Text; } }
        public string Moves { get { return this.textBoxMoves.Text; } }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            CloseForm(DialogResult.OK);
        }

        private void handleKeyDown(KeyEventArgs e)
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

        private void CloseForm(DialogResult result)
        {
            this.DialogResult = result;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CloseForm(DialogResult.Cancel);
        }

        private void textBoxMoves_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyDown(e);
        }

        private void textBoxFEN_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyDown(e);
        }

        private void textBoxDescription_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyDown(e);
        }

        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyDown(e);
        }
    }
}
