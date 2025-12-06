using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OTBFlashCards
{
    public partial class MetricsForm : Form
    {
        private ListBox listBoxVariations;
        private Label labelStats;
        private Button buttonClose;
        private Button buttonPracticeFailed;
        private Button buttonPracticeLowSuccess;
        private Button buttonPracticeSelected;
        private ComboBox comboBoxSort;
        private Label labelSort;
        private ComboBox comboBoxFilter;
        private Label labelFilter;

        private List<(VariationData data, string displayText)> allVariations;
        private string currentSourceFile;

        public VariationData SelectedVariation { get; private set; }
        public bool ShouldPractice { get; private set; }

        public MetricsForm(string sourceFile = null)
        {
            currentSourceFile = sourceFile;
            InitializeComponent();
            LoadMetrics();
        }

        private void InitializeComponent()
        {
            this.labelStats = new Label();
            this.labelFilter = new Label();
            this.comboBoxFilter = new ComboBox();
            this.labelSort = new Label();
            this.comboBoxSort = new ComboBox();
            this.listBoxVariations = new ListBox();
            this.buttonPracticeFailed = new Button();
            this.buttonPracticeLowSuccess = new Button();
            this.buttonPracticeSelected = new Button();
            this.buttonClose = new Button();
            this.SuspendLayout();

            // labelStats
            this.labelStats.AutoSize = false;
            this.labelStats.Dock = DockStyle.Top;
            this.labelStats.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.labelStats.Location = new Point(0, 0);
            this.labelStats.Name = "labelStats";
            this.labelStats.Padding = new Padding(10);
            this.labelStats.Size = new Size(800, 80);
            this.labelStats.Text = "Loading...";

            // labelFilter
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new Point(10, 90);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Text = "Show:";

            // comboBoxFilter
            this.comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Items.AddRange(new object[] {
                "All Variations",
                "Current File Only"
            });
            this.comboBoxFilter.Location = new Point(60, 87);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new Size(150, 23);
            this.comboBoxFilter.SelectedIndex = string.IsNullOrEmpty(currentSourceFile) ? 0 : 1;
            this.comboBoxFilter.SelectedIndexChanged += comboBoxFilter_SelectedIndexChanged;

            // labelSort
            this.labelSort.AutoSize = true;
            this.labelSort.Location = new Point(240, 90);
            this.labelSort.Name = "labelSort";
            this.labelSort.Text = "Sort by:";

            // comboBoxSort
            this.comboBoxSort.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSort.FormattingEnabled = true;
            this.comboBoxSort.Items.AddRange(new object[] {
                "Most Failed (total)",
                "Lowest Success Rate",
                "Most Recent Failure",
                "Most Attempts",
                "Least Practiced"
            });
            this.comboBoxSort.Location = new Point(300, 87);
            this.comboBoxSort.Name = "comboBoxSort";
            this.comboBoxSort.Size = new Size(200, 23);
            this.comboBoxSort.SelectedIndex = 0;
            this.comboBoxSort.SelectedIndexChanged += comboBoxSort_SelectedIndexChanged;

            // listBoxVariations
            this.listBoxVariations.Font = new Font("Consolas", 9F);
            this.listBoxVariations.FormattingEnabled = true;
            this.listBoxVariations.ItemHeight = 14;
            this.listBoxVariations.Location = new Point(10, 120);
            this.listBoxVariations.Name = "listBoxVariations";
            this.listBoxVariations.Size = new Size(780, 350);
            this.listBoxVariations.TabIndex = 0;
            this.listBoxVariations.DoubleClick += listBoxVariations_DoubleClick;

            // buttonPracticeFailed
            this.buttonPracticeFailed.Location = new Point(10, 485);
            this.buttonPracticeFailed.Name = "buttonPracticeFailed";
            this.buttonPracticeFailed.Size = new Size(160, 30);
            this.buttonPracticeFailed.Text = "Practice Random Failed";
            this.buttonPracticeFailed.UseVisualStyleBackColor = true;
            this.buttonPracticeFailed.Click += buttonPracticeFailed_Click;

            // buttonPracticeLowSuccess
            this.buttonPracticeLowSuccess.Location = new Point(180, 485);
            this.buttonPracticeLowSuccess.Name = "buttonPracticeLowSuccess";
            this.buttonPracticeLowSuccess.Size = new Size(160, 30);
            this.buttonPracticeLowSuccess.Text = "Practice Low Success";
            this.buttonPracticeLowSuccess.UseVisualStyleBackColor = true;
            this.buttonPracticeLowSuccess.Click += buttonPracticeLowSuccess_Click;

            // buttonPracticeSelected
            this.buttonPracticeSelected.Location = new Point(350, 485);
            this.buttonPracticeSelected.Name = "buttonPracticeSelected";
            this.buttonPracticeSelected.Size = new Size(160, 30);
            this.buttonPracticeSelected.Text = "Practice Selected";
            this.buttonPracticeSelected.UseVisualStyleBackColor = true;
            this.buttonPracticeSelected.Click += buttonPracticeSelected_Click;

            // buttonClose
            this.buttonClose.Location = new Point(690, 485);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(100, 30);
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += buttonClose_Click;

            // MetricsForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 530);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonPracticeSelected);
            this.Controls.Add(this.buttonPracticeLowSuccess);
            this.Controls.Add(this.buttonPracticeFailed);
            this.Controls.Add(this.listBoxVariations);
            this.Controls.Add(this.comboBoxSort);
            this.Controls.Add(this.labelSort);
            this.Controls.Add(this.comboBoxFilter);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.labelStats);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MetricsForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Practice Metrics & Statistics";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadMetrics()
        {
            var data = StudyDataManager.GetData();
            allVariations = new List<(VariationData, string)>();

            // Filter variations based on selection
            bool filterByFile = comboBoxFilter.SelectedIndex == 1 && !string.IsNullOrEmpty(currentSourceFile);
            IEnumerable<VariationData> variationsToShow = data.Variations.Values;
            
            if (filterByFile)
            {
                variationsToShow = variationsToShow.Where(v => v.SourceFile == currentSourceFile);
            }

            int totalVariations = variationsToShow.Count();
            int practiced = variationsToShow.Count(v => v.Metrics.TotalAttempts > 0);
            int totalAttempts = variationsToShow.Sum(v => v.Metrics.TotalAttempts);
            int totalSuccesses = variationsToShow.Sum(v => v.Metrics.SuccessfulAttempts);
            int totalFailures = variationsToShow.Sum(v => v.Metrics.FailedAttempts);
            double overallSuccess = totalAttempts > 0 ? (totalSuccesses * 100.0 / totalAttempts) : 0;

            int needsReview = variationsToShow.Count(v => 
                v.Metrics.TotalAttempts > 0 && v.Metrics.SuccessRate < 70);

            string fileInfo = filterByFile ? $" ({System.IO.Path.GetFileName(currentSourceFile)})" : "";
            labelStats.Text = $"Overall Statistics{fileInfo}\n" +
                             $"Total Variations: {totalVariations} | Practiced: {practiced} ({practiced * 100.0 / Math.Max(1, totalVariations):F0}%)\n" +
                             $"Total Attempts: {totalAttempts} | Success: {totalSuccesses} | Failed: {totalFailures} | Success Rate: {overallSuccess:F1}%\n" +
                             $"Needs Review (< 70%): {needsReview}";

            // Build variation list
            foreach (var varData in variationsToShow)
            {
                if (varData.Metrics.TotalAttempts == 0)
                    continue; // Skip unpracticed

                string line = varData.FullLine;
                if (line.Length > 50)
                    line = line.Substring(0, 47) + "...";

                string lastAttempt = varData.Metrics.LastAttemptDate.HasValue
                    ? GetTimeAgo(varData.Metrics.LastAttemptDate.Value)
                    : "Never";

                string display = $"{varData.Metrics.SuccessRate,5:F0}% ({varData.Metrics.SuccessfulAttempts,2}/{varData.Metrics.TotalAttempts,2}) | " +
                                $"Failed: {varData.Metrics.FailedAttempts,2} | " +
                                $"Streak: {varData.Metrics.CurrentStreak,2} | " +
                                $"Last: {lastAttempt,-12} | {line}";

                allVariations.Add((varData, display));
            }

            SortAndDisplayVariations();
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMetrics();
        }

        private void SortAndDisplayVariations()
        {
            List<(VariationData data, string displayText)> sorted;

            switch (comboBoxSort.SelectedIndex)
            {
                case 0: // Most Failed (total)
                    sorted = allVariations.OrderByDescending(v => v.data.Metrics.FailedAttempts).ToList();
                    break;
                case 1: // Lowest Success Rate
                    sorted = allVariations.OrderBy(v => v.data.Metrics.SuccessRate).ToList();
                    break;
                case 2: // Most Recent Failure
                    sorted = allVariations
                        .Where(v => v.data.Attempts.Any(a => !a.Success))
                        .OrderByDescending(v => v.data.Attempts.Where(a => !a.Success).Max(a => a.Date))
                        .Concat(allVariations.Where(v => !v.data.Attempts.Any(a => !a.Success)))
                        .ToList();
                    break;
                case 3: // Most Attempts
                    sorted = allVariations.OrderByDescending(v => v.data.Metrics.TotalAttempts).ToList();
                    break;
                case 4: // Least Practiced
                    sorted = allVariations.OrderBy(v => v.data.Metrics.TotalAttempts).ToList();
                    break;
                default:
                    sorted = allVariations;
                    break;
            }

            listBoxVariations.Items.Clear();
            foreach (var item in sorted)
            {
                listBoxVariations.Items.Add(item.displayText);
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

        private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortAndDisplayVariations();
        }

        private void buttonPracticeFailed_Click(object sender, EventArgs e)
        {
            var failed = StudyDataManager.GetFailedVariations();
            if (failed.Count == 0)
            {
                MessageBox.Show("No failed variations found!\n\nAll your recent attempts were successful.", 
                    "No Failed Variations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pick a random failed variation
            Random rand = new Random();
            SelectedVariation = failed[rand.Next(failed.Count)];
            ShouldPractice = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonPracticeLowSuccess_Click(object sender, EventArgs e)
        {
            var lowSuccess = StudyDataManager.GetLowSuccessRateVariations(70);
            if (lowSuccess.Count == 0)
            {
                MessageBox.Show("No variations with success rate below 70%!\n\nGreat job!", 
                    "No Low Success Variations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pick a random low success variation
            Random rand = new Random();
            SelectedVariation = lowSuccess[rand.Next(lowSuccess.Count)];
            ShouldPractice = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonPracticeSelected_Click(object sender, EventArgs e)
        {
            if (listBoxVariations.SelectedIndex < 0 || listBoxVariations.SelectedIndex >= allVariations.Count)
            {
                MessageBox.Show("Please select a variation to practice.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SelectedVariation = allVariations[listBoxVariations.SelectedIndex].data;
            ShouldPractice = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listBoxVariations_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxVariations.SelectedIndex >= 0 && listBoxVariations.SelectedIndex < allVariations.Count)
            {
                buttonPracticeSelected_Click(sender, e);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
