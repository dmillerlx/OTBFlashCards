using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OTBFlashCards
{
    public partial class TreeViewForm : Form
    {
        private TreeView treeView;
        private Panel panelControls;
        private CheckBox checkBoxShowPriorityOnly;
        private Button buttonMarkPriority;
        private Button buttonUnmarkPriority;
        private Button buttonClearAllPriority;
        private Button buttonPracticeSelected;
        private Button buttonClose;
        private Label labelStats;
        private Label labelLegend;
        
        private List<VariationLine> allVariations;
        private string sourceFile;
        private Dictionary<TreeNode, VariationLine> nodeToVariation;
        
        public VariationLine SelectedVariation { get; private set; }
        public bool ShouldPractice { get; private set; }

        public TreeViewForm(List<VariationLine> variations, string sourceFile = null)
        {
            this.allVariations = variations;
            this.sourceFile = sourceFile;
            this.nodeToVariation = new Dictionary<TreeNode, VariationLine>();
            InitializeComponent();
            BuildTree();
            UpdateStats();
        }

        private void InitializeComponent()
        {
            this.treeView = new TreeView();
            this.panelControls = new Panel();
            this.checkBoxShowPriorityOnly = new CheckBox();
            this.buttonMarkPriority = new Button();
            this.buttonUnmarkPriority = new Button();
            this.buttonClearAllPriority = new Button();
            this.buttonPracticeSelected = new Button();
            this.buttonClose = new Button();
            this.labelStats = new Label();
            this.labelLegend = new Label();
            this.SuspendLayout();

            // treeView
            this.treeView.Dock = DockStyle.Fill;
            this.treeView.Font = new Font("Consolas", 10F);
            this.treeView.HideSelection = false;
            this.treeView.Name = "treeView";
            this.treeView.ShowLines = true;
            this.treeView.ShowPlusMinus = true;
            this.treeView.ShowRootLines = true;
            this.treeView.DoubleClick += treeView_DoubleClick;
            this.treeView.AfterSelect += treeView_AfterSelect;
            this.treeView.KeyDown += treeView_KeyDown;

            // panelControls
            this.panelControls.Dock = DockStyle.Top;
            this.panelControls.Height = 130;
            this.panelControls.Name = "panelControls";

            // labelStats
            this.labelStats.AutoSize = false;
            this.labelStats.Dock = DockStyle.Top;
            this.labelStats.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.labelStats.Location = new Point(0, 0);
            this.labelStats.Name = "labelStats";
            this.labelStats.Padding = new Padding(10, 5, 10, 5);
            this.labelStats.Size = new Size(900, 30);

            // labelLegend
            this.labelLegend.AutoSize = false;
            this.labelLegend.Dock = DockStyle.Top;
            this.labelLegend.Font = new Font("Segoe UI", 8F);
            this.labelLegend.Location = new Point(0, 30);
            this.labelLegend.Name = "labelLegend";
            this.labelLegend.Padding = new Padding(10, 2, 10, 2);
            this.labelLegend.Size = new Size(900, 25);
            this.labelLegend.ForeColor = Color.DarkSlateGray;
            this.labelLegend.Text = "Colors: Bold Orange = Priority ⭐ | Green = Mastered (≥90%) | Dark Gold = Learning (50-69%) | Red = Needs Work (<50%) | Black = Not Practiced";

            // checkBoxShowPriorityOnly
            this.checkBoxShowPriorityOnly.AutoSize = true;
            this.checkBoxShowPriorityOnly.Location = new Point(10, 65);
            this.checkBoxShowPriorityOnly.Name = "checkBoxShowPriorityOnly";
            this.checkBoxShowPriorityOnly.Size = new Size(180, 19);
            this.checkBoxShowPriorityOnly.Text = "Show Priority Lines Only";
            this.checkBoxShowPriorityOnly.UseVisualStyleBackColor = true;
            this.checkBoxShowPriorityOnly.CheckedChanged += checkBoxShowPriorityOnly_CheckedChanged;

            // buttonMarkPriority
            this.buttonMarkPriority.Location = new Point(10, 95);
            this.buttonMarkPriority.Name = "buttonMarkPriority";
            this.buttonMarkPriority.Size = new Size(130, 28);
            this.buttonMarkPriority.Text = "⭐ Mark Priority (M)";
            this.buttonMarkPriority.UseVisualStyleBackColor = true;
            this.buttonMarkPriority.Click += buttonMarkPriority_Click;

            // buttonUnmarkPriority
            this.buttonUnmarkPriority.Location = new Point(150, 95);
            this.buttonUnmarkPriority.Name = "buttonUnmarkPriority";
            this.buttonUnmarkPriority.Size = new Size(130, 28);
            this.buttonUnmarkPriority.Text = "☆ Unmark (U)";
            this.buttonUnmarkPriority.UseVisualStyleBackColor = true;
            this.buttonUnmarkPriority.Click += buttonUnmarkPriority_Click;

            // buttonClearAllPriority
            this.buttonClearAllPriority.Location = new Point(290, 95);
            this.buttonClearAllPriority.Name = "buttonClearAllPriority";
            this.buttonClearAllPriority.Size = new Size(130, 28);
            this.buttonClearAllPriority.Text = "❌ Clear All Priority";
            this.buttonClearAllPriority.UseVisualStyleBackColor = true;
            this.buttonClearAllPriority.Click += buttonClearAllPriority_Click;

            // buttonPracticeSelected
            this.buttonPracticeSelected.Location = new Point(440, 95);
            this.buttonPracticeSelected.Name = "buttonPracticeSelected";
            this.buttonPracticeSelected.Size = new Size(140, 28);
            this.buttonPracticeSelected.Text = "Practice Selected";
            this.buttonPracticeSelected.UseVisualStyleBackColor = true;
            this.buttonPracticeSelected.Click += buttonPracticeSelected_Click;

            // buttonClose
            this.buttonClose.Location = new Point(790, 95);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(100, 28);
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += buttonClose_Click;

            this.panelControls.Controls.Add(this.buttonClose);
            this.panelControls.Controls.Add(this.buttonPracticeSelected);
            this.panelControls.Controls.Add(this.buttonClearAllPriority);
            this.panelControls.Controls.Add(this.buttonUnmarkPriority);
            this.panelControls.Controls.Add(this.buttonMarkPriority);
            this.panelControls.Controls.Add(this.checkBoxShowPriorityOnly);
            this.panelControls.Controls.Add(this.labelLegend);
            this.panelControls.Controls.Add(this.labelStats);

            // TreeViewForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 600);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.panelControls);
            this.Name = "TreeViewForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Variation Tree - All Lines";
            this.ResumeLayout(false);
        }

        private void BuildTree()
        {
            // Save current selection path before rebuilding
            var selectedPath = GetSelectedNodePath();
            
            treeView.Nodes.Clear();
            nodeToVariation.Clear();

            if (allVariations.Count == 0)
                return;

            bool showPriorityOnly = checkBoxShowPriorityOnly.Checked;

            // Filter variations if needed
            var variationsToShow = allVariations.Where(v =>
            {
                if (!showPriorityOnly) return true;
                var varData = StudyDataManager.GetOrCreateVariation(v, sourceFile);
                return varData.IsPriority;
            }).ToList();

            if (variationsToShow.Count == 0)
            {
                treeView.Nodes.Add("No variations to display");
                return;
            }

            // Build the actual tree structure by comparing move sequences
            var root = BuildTreeStructure(variationsToShow);
            treeView.Nodes.Add(root);
            root.ExpandAll();
            
            // Restore selection
            RestoreSelection(selectedPath);
        }

        private List<string> GetSelectedNodePath()
        {
            var path = new List<string>();
            var node = treeView.SelectedNode;
            
            while (node != null)
            {
                path.Insert(0, node.Text);
                node = node.Parent;
            }
            
            return path;
        }

        private void RestoreSelection(List<string> path)
        {
            if (path == null || path.Count == 0 || treeView.Nodes.Count == 0)
                return;

            TreeNode currentNode = null;
            TreeNodeCollection nodes = treeView.Nodes;

            foreach (var pathPart in path)
            {
                TreeNode foundNode = null;
                foreach (TreeNode node in nodes)
                {
                    if (node.Text == pathPart)
                    {
                        foundNode = node;
                        break;
                    }
                }

                if (foundNode == null)
                    return; // Path no longer exists

                currentNode = foundNode;
                nodes = currentNode.Nodes;
            }

            if (currentNode != null)
            {
                treeView.SelectedNode = currentNode;
                currentNode.EnsureVisible();
            }
        }

        private TreeNode BuildTreeStructure(List<VariationLine> variations)
        {
            var root = new TreeNode("Opening");
            
            // Recursive function to build tree from a set of variations
            BuildTreeRecursive(root, variations, 0);
            
            return root;
        }

        private void BuildTreeRecursive(TreeNode parentNode, List<VariationLine> variations, int moveIndex)
        {
            if (variations.Count == 0)
                return;

            // Group variations by their move at this index
            var groups = new Dictionary<string, List<VariationLine>>();
            var completeVariations = new List<VariationLine>();

            foreach (var variation in variations)
            {
                if (moveIndex >= variation.Moves.Count)
                {
                    // This variation ends here
                    completeVariations.Add(variation);
                }
                else
                {
                    string move = variation.Moves[moveIndex];
                    if (!groups.ContainsKey(move))
                        groups[move] = new List<VariationLine>();
                    groups[move].Add(variation);
                }
            }

            // Add complete variations first (these end at this node)
            foreach (var variation in completeVariations)
            {
                AddVariationLeafNode(parentNode, variation, moveIndex);
            }

            // Process each group (each unique move at this position)
            foreach (var kvp in groups.OrderBy(g => g.Key))
            {
                string move = kvp.Key;
                var groupVariations = kvp.Value;

                // Create node for this move
                string moveNum = GetMoveNumberString(moveIndex);
                string nodeText = $"{moveNum}{move}";
                
                TreeNode moveNode = new TreeNode(nodeText);
                parentNode.Nodes.Add(moveNode);

                // If all variations in this group continue with the same moves, collapse them
                if (groupVariations.Count > 1 && AllHaveSameNextMoves(groupVariations, moveIndex + 1, 3))
                {
                    // Show continuation inline for common moves
                    int commonMoves = CountCommonMoves(groupVariations, moveIndex + 1);
                    if (commonMoves > 0)
                    {
                        var continuation = BuildContinuationString(groupVariations[0], moveIndex + 1, commonMoves);
                        moveNode.Text = $"{moveNum}{move} {continuation}";
                        BuildTreeRecursive(moveNode, groupVariations, moveIndex + 1 + commonMoves);
                    }
                    else
                    {
                        BuildTreeRecursive(moveNode, groupVariations, moveIndex + 1);
                    }
                }
                else if (groupVariations.Count == 1)
                {
                    // Single variation - show rest of moves
                    var variation = groupVariations[0];
                    var remainingMoves = BuildRemainingMoves(variation, moveIndex + 1);
                    if (!string.IsNullOrEmpty(remainingMoves))
                    {
                        moveNode.Text = $"{moveNum}{move} {remainingMoves}";
                    }
                    
                    // This is a leaf - store the full variation
                    nodeToVariation[moveNode] = variation;
                    StyleNode(moveNode, variation);
                }
                else
                {
                    // Multiple variations diverge here - recurse
                    BuildTreeRecursive(moveNode, groupVariations, moveIndex + 1);
                }
            }
        }

        private void AddVariationLeafNode(TreeNode parentNode, VariationLine variation, int endIndex)
        {
            // This variation ends at the parent node
            var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
            string stats = GetStatsString(varData);
            var node = new TreeNode($"[Complete] {stats}");
            nodeToVariation[node] = variation;
            StyleNode(node, variation);
            parentNode.Nodes.Add(node);
        }

        private bool AllHaveSameNextMoves(List<VariationLine> variations, int startIndex, int lookAhead)
        {
            if (variations.Count <= 1)
                return false;

            for (int i = 0; i < lookAhead; i++)
            {
                int checkIndex = startIndex + i;
                string firstMove = null;
                
                foreach (var variation in variations)
                {
                    if (checkIndex >= variation.Moves.Count)
                        return false; // Some variation is too short
                    
                    if (firstMove == null)
                        firstMove = variation.Moves[checkIndex];
                    else if (variation.Moves[checkIndex] != firstMove)
                        return false; // Moves differ
                }
            }
            return true;
        }

        private int CountCommonMoves(List<VariationLine> variations, int startIndex)
        {
            if (variations.Count <= 1)
                return 0;

            int count = 0;
            int maxLength = variations.Min(v => v.Moves.Count);

            for (int i = startIndex; i < maxLength; i++)
            {
                string firstMove = variations[0].Moves[i];
                if (variations.All(v => v.Moves[i] == firstMove))
                    count++;
                else
                    break;
            }
            return count;
        }

        private string BuildContinuationString(VariationLine variation, int startIndex, int count)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < count && startIndex + i < variation.Moves.Count; i++)
            {
                int moveIdx = startIndex + i;
                string moveNum = GetMoveNumberString(moveIdx);
                sb.Append($"{moveNum}{variation.Moves[moveIdx]} ");
            }
            return sb.ToString().Trim();
        }

        private string BuildRemainingMoves(VariationLine variation, int startIndex)
        {
            if (startIndex >= variation.Moves.Count)
                return "";

            var sb = new System.Text.StringBuilder();
            int maxMoves = Math.Min(6, variation.Moves.Count - startIndex); // Show up to 6 more moves
            
            for (int i = 0; i < maxMoves; i++)
            {
                int moveIdx = startIndex + i;
                string moveNum = GetMoveNumberString(moveIdx);
                sb.Append($"{moveNum}{variation.Moves[moveIdx]} ");
            }
            
            if (startIndex + maxMoves < variation.Moves.Count)
                sb.Append("...");
            
            return sb.ToString().Trim();
        }

        private string GetMoveNumberString(int moveIndex)
        {
            if (moveIndex % 2 == 0)
            {
                return $"{(moveIndex / 2) + 1}.";
            }
            else
            {
                return ""; // Black's move, no number
            }
        }

        private void StyleNode(TreeNode node, VariationLine variation)
        {
            var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
            
            // Style based on priority and success rate
            if (varData.IsPriority)
            {
                node.ForeColor = Color.DarkOrange;
                node.NodeFont = new Font(treeView.Font, FontStyle.Bold);
            }
            else if (varData.Metrics.TotalAttempts > 0 && varData.Metrics.SuccessRate < 50)
            {
                node.ForeColor = Color.Red;
            }
            else if (varData.Metrics.TotalAttempts > 0 && varData.Metrics.SuccessRate < 70)
            {
                node.ForeColor = Color.DarkGoldenrod;
            }
            else if (varData.Metrics.SuccessRate >= 90)
            {
                node.ForeColor = Color.Green;
            }
        }

        private string GetStatsString(VariationData varData)
        {
            if (varData.Metrics.TotalAttempts == 0)
                return "[Not practiced]";

            string priority = varData.IsPriority ? "⭐ " : "";
            return $"{priority}[{varData.Metrics.SuccessRate:F0}% ({varData.Metrics.SuccessfulAttempts}/{varData.Metrics.TotalAttempts})]";
        }

        private void UpdateStats()
        {
            int total = allVariations.Count;
            int priority = 0;
            int practiced = 0;

            foreach (var variation in allVariations)
            {
                var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
                if (varData.IsPriority)
                    priority++;
                if (varData.Metrics.TotalAttempts > 0)
                    practiced++;
            }

            labelStats.Text = $"Total Variations: {total} | Priority: {priority} | Practiced: {practiced}";
        }

        private void checkBoxShowPriorityOnly_CheckedChanged(object sender, EventArgs e)
        {
            BuildTree();
        }

        private void buttonMarkPriority_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || !nodeToVariation.ContainsKey(treeView.SelectedNode))
            {
                MessageBox.Show("Please select a variation to mark as priority.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var variation = nodeToVariation[treeView.SelectedNode];
            var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
            varData.IsPriority = true;
            StudyDataManager.Save();

            BuildTree();
            UpdateStats();
        }

        private void buttonUnmarkPriority_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || !nodeToVariation.ContainsKey(treeView.SelectedNode))
            {
                MessageBox.Show("Please select a variation to unmark as priority.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var variation = nodeToVariation[treeView.SelectedNode];
            var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
            varData.IsPriority = false;
            StudyDataManager.Save();

            BuildTree();
            UpdateStats();
        }

        private void buttonClearAllPriority_Click(object sender, EventArgs e)
        {
            // Count how many priority variations exist
            int priorityCount = 0;
            foreach (var variation in allVariations)
            {
                var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
                if (varData.IsPriority)
                    priorityCount++;
            }

            if (priorityCount == 0)
            {
                MessageBox.Show("No priority variations to clear.", "No Priority Lines",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirmation dialog
            var result = MessageBox.Show(
                $"Are you sure you want to remove priority from ALL {priorityCount} variation(s)?\n\n" +
                "This action cannot be undone.",
                "Clear All Priority",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Remove priority from all variations
                foreach (var variation in allVariations)
                {
                    var varData = StudyDataManager.GetOrCreateVariation(variation, sourceFile);
                    varData.IsPriority = false;
                }
                StudyDataManager.Save();

                BuildTree();
                UpdateStats();

                MessageBox.Show($"Priority removed from {priorityCount} variation(s).",
                    "Priority Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonPracticeSelected_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || !nodeToVariation.ContainsKey(treeView.SelectedNode))
            {
                MessageBox.Show("Please select a variation to practice.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SelectedVariation = nodeToVariation[treeView.SelectedNode];
            ShouldPractice = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null && nodeToVariation.ContainsKey(treeView.SelectedNode))
            {
                buttonPracticeSelected_Click(sender, e);
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Could show more details about selected variation in a status bar
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                // Mark priority
                buttonMarkPriority_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.U)
            {
                // Unmark priority
                buttonUnmarkPriority_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // Practice selected
                buttonPracticeSelected_Click(sender, e);
                e.Handled = true;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
