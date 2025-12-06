using ChessPuzzleSimulator;
using System.Text;

namespace PGNtoFlashCard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // enable drag/drop on the input box
            textBoxInput.AllowDrop = true;
            textBoxInput.DragEnter += TextBoxInput_DragEnter;
            textBoxInput.DragDrop += TextBoxInput_DragDrop;
        }

        private void TextBoxInput_DragEnter(object sender, DragEventArgs e)
        {
            // only allow file drops
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void TextBoxInput_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 0) return;

            var path = files[0];
            var ext = Path.GetExtension(path);
            if (string.Equals(ext, ".pgn", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, ".txt", StringComparison.OrdinalIgnoreCase))
            {
                // load file into the input box
                textBoxInput.Text = File.ReadAllText(path);
                // optional: auto-run processing on drop
                textBoxOutput.Text = ProcessPgn(textBoxInput.Text);
            }
            else
            {
                MessageBox.Show(
                    "Please drop a .pgn or .txt file.",
                    "Invalid File",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxInput.Text = string.Empty;
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            string inputPgn = textBoxInput.Text;
            textBoxOutput.Text = ProcessPgn(inputPgn);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxOutput.Text))
            {
                Clipboard.SetText(textBoxOutput.Text);
                MessageBox.Show("Output copied to clipboard!", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string ProcessPgn(string pgnText)
        {
            var parser = new PgnParser();
            var games = parser.ParseGames(pgnText);

            var sb = new StringBuilder();
            foreach (var game in games)
            {
                // Header (Event / White / Black or fallback to "Game")
                sb.AppendLine(game.ToString());

                // Build all lines from the dummy root's NextMoves :contentReference[oaicite:3]{index=3}
                var allLines = new List<List<string>>();
                foreach (var first in game.MoveTreeRoot.NextMoves)
                    CollectLines(first, new List<string>(), allLines);

                // Output each variation with numbering
                int varNum = 1;
                foreach (var line in allLines)
                {
                    sb.AppendLine($"Variation {varNum++}:");
                    sb.AppendLine(FormatWithMoveNumbers(line));
                    sb.AppendLine();  // blank line between variations
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Walks each branch of the move tree, accumulating SANs,
        /// emitting a list when you hit a leaf (no further branches).
        /// </summary>
        private void CollectLines(MoveNode node, List<string> current, List<List<string>> output)
        {
            current.Add(node.San);
            if (node.NextMoves.Count == 0)  // includes mainline & variations :contentReference[oaicite:4]{index=4}
            {
                output.Add(new List<string>(current));
            }
            else
            {
                foreach (var child in node.NextMoves)
                    CollectLines(child, current, output);
            }
            current.RemoveAt(current.Count - 1);
        }

        /// <summary>
        /// Takes a flat SAN list and returns a single string:
        /// "1. e5 b5 2. c4 c6 ..." or shorter if fewer moves.
        /// </summary>
        private string FormatWithMoveNumbers(List<string> moves)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < moves.Count; i++)
            {
                if (i % 2 == 0)
                {
                    int moveNo = (i / 2) + 1;
                    sb.Append($"{moveNo}. {moves[i]}");
                }
                else
                {
                    sb.Append($" {moves[i]} \r\n");
                }
            }
            return sb.ToString().Trim();
        }
    }
}
