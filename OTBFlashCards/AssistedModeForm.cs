using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OTBFlashCards
{
    public partial class AssistedModeForm : Form
    {
        private List<VariationLine> variations;
        private int currentVariationIndex;
        private int currentMoveIndex = -1;
        private List<string> displayedMoves = new List<string>();
        private HashSet<int> reviewedVariations = new HashSet<int>();
        private bool showFullHelp = true;

        public AssistedModeForm(VariationLine variation)
        {
            InitializeComponent();
            this.variations = new List<VariationLine> { variation };
            this.currentVariationIndex = 0;
            this.KeyPreview = true;
            LoadHelpPreference();
            InitializeDisplay();
        }

        public AssistedModeForm(List<VariationLine> variations, int startIndex = 0)
        {
            InitializeComponent();
            this.variations = variations;
            this.currentVariationIndex = startIndex;
            this.KeyPreview = true;
            LoadHelpPreference();
            InitializeDisplay();
        }

        private void LoadHelpPreference()
        {
            showFullHelp = RegistryUtils.GetBool("AssistedMode_ShowFullHelp", true);
        }

        private void SaveHelpPreference()
        {
            RegistryUtils.SetBool("AssistedMode_ShowFullHelp", showFullHelp);
        }

        private void ToggleHelp()
        {
            showFullHelp = !showFullHelp;
            SaveHelpPreference();
            UpdateDisplay();
        }

        private void InitializeDisplay()
        {
            // Set up the form
            var variation = variations[currentVariationIndex];
            labelTitle.Text = $"Assisted Mode - Variation {currentVariationIndex + 1}/{variations.Count} ({variation.MoveCount} moves)";
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var variation = variations[currentVariationIndex];
            
            // Count unreviewed variations
            int unreviewedCount = 0;
            for (int i = 0; i < variations.Count; i++)
            {
                if (!reviewedVariations.Contains(i))
                {
                    unreviewedCount++;
                }
            }
            
            // Clear and prepare RichTextBox
            textBoxMoves.Clear();
            textBoxMoves.SelectionFont = new Font("Consolas", 12, FontStyle.Regular);

            // Show all moves up to current position
            for (int i = 0; i <= currentMoveIndex && i < variation.Moves.Count; i++)
            {
                string moveNum = "";
                if (i % 2 == 0)
                {
                    moveNum = $"{(i / 2) + 1}.";
                }

                string move = variation.Moves[i];
                string comment = (i < variation.Comments.Count && !string.IsNullOrEmpty(variation.Comments[i])) 
                    ? " {" + variation.Comments[i] + "}" 
                    : "";

                // Highlight current move
                if (i == currentMoveIndex)
                {
                    string color = (i % 2 == 0) ? "White" : "Black";
                    string status = (i == variation.MoveCount - 1) ? " - COMPLETE" : "";
                    textBoxMoves.AppendText(">>> ");
                    AppendMoveWithBoldNAGs($"{moveNum,-5} {move}");
                    textBoxMoves.AppendText($"{comment} <<< ({color}{status})\n");
                }
                else
                {
                    textBoxMoves.AppendText("    ");
                    AppendMoveWithBoldNAGs($"{moveNum,-5} {move}");
                    textBoxMoves.AppendText($"{comment}\n");
                }
            }

            textBoxMoves.AppendText("\n");
            textBoxMoves.AppendText("═══════════════════════════════════════════════════\n");
            textBoxMoves.AppendText($"Variation {currentVariationIndex + 1} of {variations.Count} | Position: Move {currentMoveIndex + 1} of {variation.MoveCount}\n");
            textBoxMoves.AppendText($"Unreviewed: {unreviewedCount} | Reviewed: {reviewedVariations.Count}" + 
                     (reviewedVariations.Contains(currentVariationIndex) ? " | [REVIEWED]" : "") + "\n");
            textBoxMoves.AppendText("═══════════════════════════════════════════════════\n");
            
            if (showFullHelp)
            {
                textBoxMoves.AppendText("Navigation:\n");
                textBoxMoves.AppendText("  Space / Right Arrow → Next move  |  Left Arrow → Previous move\n");
                textBoxMoves.AppendText("  Home → Go to start  |  End → Go to end\n");
                textBoxMoves.AppendText("\n");
                textBoxMoves.AppendText("Variations:\n");
                textBoxMoves.AppendText("  P/PgUp → Previous  |  N/PgDn → Next  |  R → Random  |  U → Random unreviewed\n");
                textBoxMoves.AppendText("\n");
                textBoxMoves.AppendText("  H → Hide help  |  Esc → Exit\n");
            }
            else
            {
                textBoxMoves.AppendText("Press H for help | Space/Arrows=Navigate | N/P=Variations | Esc=Exit\n");
            }
            
            textBoxMoves.AppendText("═══════════════════════════════════════════════════");

            // Auto-scroll to bottom
            textBoxMoves.SelectionStart = textBoxMoves.Text.Length;
            textBoxMoves.ScrollToCaret();

            // Update title
            labelTitle.Text = $"Assisted Mode - Variation {currentVariationIndex + 1}/{variations.Count} ({variation.MoveCount} moves)";

            // Update buttons
            buttonPrevious.Enabled = currentMoveIndex >= 0;
            buttonNext.Enabled = currentMoveIndex < variation.MoveCount - 1;
        }

        private void AppendMoveWithBoldNAGs(string moveText)
        {
            // NAG symbols to make bold - order matters! Check multi-character first
            // Note: We exclude single 'N' here because it conflicts with Knight moves
            string[] nags = { "!!", "??", "!?", "?!", "!", "?", "☐", "∞", "⩲", "⩱", "±", "∓", "+-", "-+", 
                            "⨀", "⟳", "↑", "→", "⇆", "⟲", "⊕", "∆", "=" };
            
            int position = 0;
            while (position < moveText.Length)
            {
                bool foundNag = false;
                
                // Try to find a NAG at current position
                foreach (var nag in nags)
                {
                    if (position + nag.Length <= moveText.Length && 
                        moveText.Substring(position, nag.Length) == nag)
                    {
                        // Found a NAG - append it in bold
                        int currentLength = textBoxMoves.TextLength;
                        textBoxMoves.AppendText(nag);
                        textBoxMoves.Select(currentLength, nag.Length);
                        textBoxMoves.SelectionFont = new Font("Consolas", 12, FontStyle.Bold);
                        textBoxMoves.Select(textBoxMoves.TextLength, 0);
                        textBoxMoves.SelectionFont = new Font("Consolas", 12, FontStyle.Regular);
                        
                        position += nag.Length;
                        foundNag = true;
                        break;
                    }
                }
                
                if (!foundNag)
                {
                    // Not a NAG - append single character
                    textBoxMoves.AppendText(moveText[position].ToString());
                    position++;
                }
            }
        }

        private void NextMove()
        {
            var variation = variations[currentVariationIndex];
            if (currentMoveIndex < variation.MoveCount - 1)
            {
                currentMoveIndex++;
                // Check if we've reached the end
                if (currentMoveIndex == variation.MoveCount - 1)
                {
                    MarkCurrentAsReviewed();
                }
                UpdateDisplay();
            }
        }

        private void PreviousMove()
        {
            if (currentMoveIndex >= 0)
            {
                currentMoveIndex--;
                UpdateDisplay();
            }
        }

        private void GoToStart()
        {
            currentMoveIndex = -1;
            UpdateDisplay();
        }

        private void GoToEnd()
        {
            var variation = variations[currentVariationIndex];
            currentMoveIndex = variation.MoveCount - 1;
            MarkCurrentAsReviewed();
            UpdateDisplay();
        }

        private void PreviousVariation()
        {
            if (currentVariationIndex > 0)
            {
                currentVariationIndex--;
                currentMoveIndex = -1;
                UpdateDisplay();
            }
        }

        private void NextVariation()
        {
            if (currentVariationIndex < variations.Count - 1)
            {
                MarkCurrentAsReviewed();
                currentVariationIndex++;
                currentMoveIndex = -1;
                UpdateDisplay();
            }
        }

        private void RandomVariation()
        {
            if (variations.Count > 1)
            {
                Random rand = new Random();
                int newIndex = rand.Next(variations.Count);
                currentVariationIndex = newIndex;
                currentMoveIndex = -1;
                UpdateDisplay();
            }
        }

        private void RandomUnreviewedVariation()
        {
            var unreviewed = new List<int>();
            for (int i = 0; i < variations.Count; i++)
            {
                if (!reviewedVariations.Contains(i))
                {
                    unreviewed.Add(i);
                }
            }

            if (unreviewed.Count > 0)
            {
                Random rand = new Random();
                int randomIndex = rand.Next(unreviewed.Count);
                currentVariationIndex = unreviewed[randomIndex];
                currentMoveIndex = -1;
                UpdateDisplay();
            }
            else
            {
                // All variations reviewed - ask if they want to reset
                var result = MessageBox.Show(
                    "All variations have been reviewed!\n\nWould you like to reset and start over?",
                    "All Done",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    // Reset all reviews
                    reviewedVariations.Clear();
                    // Pick a random variation
                    Random rand = new Random();
                    currentVariationIndex = rand.Next(variations.Count);
                    currentMoveIndex = -1;
                    UpdateDisplay();
                }
            }
        }

        private void MarkCurrentAsReviewed()
        {
            var variation = variations[currentVariationIndex];
            // Mark as reviewed if we've reached the end of the variation
            if (currentMoveIndex >= variation.MoveCount - 1)
            {
                reviewedVariations.Add(currentVariationIndex);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Space:
                case Keys.Right:
                    NextMove();
                    return true;

                case Keys.Left:
                    PreviousMove();
                    return true;

                case Keys.Home:
                    GoToStart();
                    return true;

                case Keys.End:
                    GoToEnd();
                    return true;

                case Keys.PageUp:
                case Keys.P:
                    PreviousVariation();
                    return true;

                case Keys.PageDown:
                case Keys.N:
                    NextVariation();
                    return true;

                case Keys.R:
                    RandomVariation();
                    return true;

                case Keys.U:
                    RandomUnreviewedVariation();
                    return true;

                case Keys.H:
                    ToggleHelp();
                    return true;

                case Keys.Escape:
                    this.Close();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextMove();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            PreviousMove();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
