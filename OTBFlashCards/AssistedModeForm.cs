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
        private VariationData currentVariationData;
        private bool markedAsFailed = false;
        private DateTime sessionStartTime;
        private string sourceFilePath; // Track which PGN file these variations came from
        private int? treeMaxDepth; // Tree-wide depth limit
        private bool treeIgnoreMainline; // Tree-wide ignore mainline setting

        public AssistedModeForm(VariationLine variation, string sourceFile = null)
        {
            InitializeComponent();
            this.variations = new List<VariationLine> { variation };
            this.currentVariationIndex = 0;
            this.KeyPreview = true;
            this.sourceFilePath = sourceFile ?? "";
            LoadHelpPreference();
            LoadTreeDepthSettings();
            InitializeDisplay();
        }

        public AssistedModeForm(List<VariationLine> variations, int startIndex = 0, string sourceFile = null)
        {
            InitializeComponent();
            this.variations = variations;
            this.currentVariationIndex = startIndex;
            this.KeyPreview = true;
            this.sourceFilePath = sourceFile ?? "";
            LoadHelpPreference();
            LoadTreeDepthSettings();
            InitializeDisplay();
        }

        private void LoadTreeDepthSettings()
        {
            if (!string.IsNullOrEmpty(sourceFilePath))
            {
                var settings = StudyDataManager.GetTreeDepthSettings(sourceFilePath);
                treeMaxDepth = settings.maxDepth;
                treeIgnoreMainline = settings.ignoreMainline;
            }
            else
            {
                treeMaxDepth = null;
                treeIgnoreMainline = false;
            }
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
            
            // Load variation data
            currentVariationData = StudyDataManager.GetOrCreateVariation(variation, sourceFilePath);
            markedAsFailed = false;
            sessionStartTime = DateTime.Now;
            
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

                // Check for move notes
                string moveNote = "";
                if (currentVariationData.MoveNotes.ContainsKey(i))
                {
                    moveNote = $" [{currentVariationData.MoveNotes[i]}]";
                }

                // Highlight current move
                if (i == currentMoveIndex)
                {
                    string color = (i % 2 == 0) ? "White" : "Black";
                    string status = (i == variation.MoveCount - 1) ? " - COMPLETE" : "";
                    
                    textBoxMoves.AppendText(">>> ");
                    AppendMoveWithBoldNAGs($"{moveNum,-5} {move}");
                    textBoxMoves.AppendText($"{comment}{moveNote} <<< ({color}{status})\n");
                }
                else
                {
                    textBoxMoves.AppendText("    ");
                    AppendMoveWithBoldNAGs($"{moveNum,-5} {move}");
                    textBoxMoves.AppendText($"{comment}{moveNote}\n");
                }
            }

            textBoxMoves.AppendText("\n");
            textBoxMoves.AppendText("═══════════════════════════════════════════════════\n");
            
            // Depth limit info
            string depthInfo = "";
            if (treeMaxDepth.HasValue)
            {
                depthInfo = $" | Depth Limit: {treeMaxDepth}";
                if (treeIgnoreMainline && IsMainlineVariation())
                {
                    depthInfo += " (ignored for mainline)";
                }
            }
            textBoxMoves.AppendText($"Variation {currentVariationIndex + 1} of {variations.Count} | Position: Move {currentMoveIndex + 1} of {variation.MoveCount}{depthInfo}\n");
            
            // Metrics info
            if (currentVariationData.Metrics.TotalAttempts > 0)
            {
                string successRate = currentVariationData.Metrics.SuccessRate.ToString("F0");
                string lastAttempt = currentVariationData.Metrics.LastAttemptDate.HasValue ?
                    GetTimeAgo(currentVariationData.Metrics.LastAttemptDate.Value) : "Never";
                string streak = currentVariationData.Metrics.CurrentStreak > 0 ?
                    $" | Streak: {currentVariationData.Metrics.CurrentStreak}" : "";
                
                textBoxMoves.AppendText($"Success: {successRate}% ({currentVariationData.Metrics.SuccessfulAttempts}/{currentVariationData.Metrics.TotalAttempts}){streak} | Last: {lastAttempt}\n");
            }
            
            textBoxMoves.AppendText($"Unreviewed: {unreviewedCount} | Reviewed: {reviewedVariations.Count}" + 
                     (reviewedVariations.Contains(currentVariationIndex) ? " | [REVIEWED]" : "") + 
                     (markedAsFailed ? " | [MARKED AS FAILED]" : "") + "\n");
            
            // Show line notes if any
            if (!string.IsNullOrEmpty(currentVariationData.LineNotes))
            {
                textBoxMoves.AppendText($"[Line Notes: {currentVariationData.LineNotes}]\n");
            }
            
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
                textBoxMoves.AppendText("Study Tools:\n");
                textBoxMoves.AppendText("  F → Mark as Failed  |  S → Mark as Success  |  D → Set Depth Limit\n");
                textBoxMoves.AppendText("  L → Line Notes  |  M → Move Notes\n");
                textBoxMoves.AppendText("\n");
                textBoxMoves.AppendText("  H → Hide help  |  Esc → Exit (saves attempt)\n");
            }
            else
            {
                textBoxMoves.AppendText("H=Help | Space/Arrows=Nav | N/P=Vars | F/S=Fail/Success | D=Depth | L/M=Notes | Esc=Exit\n");
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
            
            // Depth counts move numbers (1.e4 e5 = move 1, 2.Nf3 Nc6 = move 2)
            // currentMoveIndex is 0-based half-moves, so divide by 2 and add 1 for move number
            int currentMoveNumber = (currentMoveIndex / 2) + 1;
            int nextMoveNumber = ((currentMoveIndex + 1) / 2) + 1;
            
            // Check if depth limit applies (ignore if mainline and checkbox is checked)
            bool applyDepthLimit = treeMaxDepth.HasValue && 
                                   !(treeIgnoreMainline && IsMainlineVariation());
            
            // Check if we're at depth limit
            if (applyDepthLimit && currentMoveNumber >= treeMaxDepth.Value)
            {
                MessageBox.Show($"Depth limit reached (move {treeMaxDepth}).\n\nUse D to change depth limit or navigate to another variation.",
                    "At Depth Limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (currentMoveIndex < variation.MoveCount - 1)
            {
                currentMoveIndex++;
                
                // Check if we've reached the depth limit NOW
                if (applyDepthLimit && nextMoveNumber > treeMaxDepth.Value)
                {
                    MarkCurrentAsReviewed();
                    MessageBox.Show($"Depth limit reached (move {treeMaxDepth}).\n\nThis variation is marked as complete.",
                        "Depth Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Check if we've reached the end
                else if (currentMoveIndex == variation.MoveCount - 1)
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
                currentVariationData = StudyDataManager.GetOrCreateVariation(variations[currentVariationIndex], sourceFilePath);
                markedAsFailed = false;
                sessionStartTime = DateTime.Now;
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
                currentVariationData = StudyDataManager.GetOrCreateVariation(variations[currentVariationIndex], sourceFilePath);
                markedAsFailed = false;
                sessionStartTime = DateTime.Now;
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
                currentVariationData = StudyDataManager.GetOrCreateVariation(variations[currentVariationIndex], sourceFilePath);
                markedAsFailed = false;
                sessionStartTime = DateTime.Now;
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
                currentVariationData = StudyDataManager.GetOrCreateVariation(variations[currentVariationIndex], sourceFilePath);
                markedAsFailed = false;
                sessionStartTime = DateTime.Now;
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

        private bool IsMainlineVariation()
        {
            // A mainline variation has no branches - it's just a single line of moves
            // In PGN terms, it has no parenthetical variations
            // For our purposes, we'll consider it mainline if it's a straight sequence
            // This is a simple check - you could make it more sophisticated
            var variation = variations[currentVariationIndex];
            
            // For now, we'll just check if it's the first/primary variation
            // You could enhance this by checking the PGN structure if needed
            return currentVariationIndex == 0;
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

        private string GetTimeAgo(DateTime past)
        {
            var span = DateTime.Now - past;
            if (span.TotalMinutes < 1) return "Just now";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes}m ago";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours}h ago";
            if (span.TotalDays < 7) return $"{(int)span.TotalDays}d ago";
            if (span.TotalDays < 30) return $"{(int)(span.TotalDays / 7)}w ago";
            return past.ToShortDateString();
        }

        private void MarkAsFailed()
        {
            markedAsFailed = true;
            MessageBox.Show("This variation will be marked as failed.\n\nPress S to undo and mark as success.", "Marked as Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDisplay();
        }

        private void MarkAsSuccess()
        {
            markedAsFailed = false;
            MessageBox.Show("This variation will be marked as successful.", "Marked as Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateDisplay();
        }

        private void SetDepthLimit()
        {
            var variation = variations[currentVariationIndex];
            var dialog = new SetDepthDialog(
                treeMaxDepth,
                treeIgnoreMainline,
                variation.MoveCount);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                treeMaxDepth = dialog.DepthLimit;
                treeIgnoreMainline = dialog.IgnoreDepthForMainline;
                
                // Save to tree settings
                if (!string.IsNullOrEmpty(sourceFilePath))
                {
                    StudyDataManager.SetTreeDepthSettings(sourceFilePath, treeMaxDepth, treeIgnoreMainline);
                }
                
                UpdateDisplay();
            }
        }

        private void EditLineNotes()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter notes for this entire variation line:",
                "Line Notes",
                currentVariationData.LineNotes);

            if (input != null)
            {
                currentVariationData.LineNotes = input;
                StudyDataManager.Save();
                UpdateDisplay();
            }
        }

        private void EditMoveNotes()
        {
            if (currentMoveIndex < 0)
            {
                MessageBox.Show("Navigate to a move first to add notes.", "No Move Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string currentNote = "";
            currentVariationData.MoveNotes.TryGetValue(currentMoveIndex, out currentNote);

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter notes for move {currentMoveIndex + 1} ({variations[currentVariationIndex].Moves[currentMoveIndex]}):",
                "Move Notes",
                currentNote ?? "");

            if (input != null)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    currentVariationData.MoveNotes.Remove(currentMoveIndex);
                }
                else
                {
                    currentVariationData.MoveNotes[currentMoveIndex] = input;
                }
                StudyDataManager.Save();
                UpdateDisplay();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Record the attempt
            int depthReached = currentMoveIndex + 1;
            bool success = !markedAsFailed;

            StudyDataManager.RecordAttempt(
                currentVariationData,
                success,
                depthReached,
                ""
            );

            base.OnFormClosing(e);
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

                case Keys.F:
                    MarkAsFailed();
                    return true;

                case Keys.S:
                    MarkAsSuccess();
                    return true;

                case Keys.D:
                    SetDepthLimit();
                    return true;

                case Keys.L:
                    EditLineNotes();
                    return true;

                case Keys.M:
                    EditMoveNotes();
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
