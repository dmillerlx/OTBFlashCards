using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OTBFlashCards
{
    public partial class Form1 : Form
    {
        private List<string> pgnFiles = new List<string>();
        private Dictionary<string, List<PgnGame>> loadedGames = new Dictionary<string, List<PgnGame>>();
        private PgnParser parser = new PgnParser();

        public Form1()
        {
            InitializeComponent();
            LoadPGNFilesFromRegistry();
            SetupDragDrop();
            CheckFirstRun();
        }

        private void CheckFirstRun()
        {
            string dataPath = StudyDataManager.GetDataFilePath();
            if (string.IsNullOrEmpty(dataPath))
            {
                MessageBox.Show(
                    "Welcome to OTB Flash Cards!\n\n" +
                    "Before you begin, please choose where to store your study data.\n" +
                    "This file will track your practice attempts, notes, and progress.",
                    "First Time Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                ShowSettings();
            }
        }

        private void LoadPGNFilesFromRegistry()
        {
            pgnFiles = RegistryUtils.GetFileList();
            RefreshFileList();
        }

        private void SavePGNFilesToRegistry()
        {
            RegistryUtils.SetFileList(pgnFiles);
        }

        private void SetupDragDrop()
        {
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddPGNFiles(files);
        }

        private void AddPGNFiles(string[] files)
        {
            bool addedAny = false;
            
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file);
                if (string.Equals(ext, ".pgn", StringComparison.OrdinalIgnoreCase))
                {
                    if (!pgnFiles.Contains(file))
                    {
                        pgnFiles.Add(file);
                        addedAny = true;
                    }
                }
            }

            if (addedAny)
            {
                SavePGNFilesToRegistry();
                RefreshFileList();
            }
        }

        private void RefreshFileList()
        {
            listBoxFiles.Items.Clear();
            foreach (var file in pgnFiles)
            {
                listBoxFiles.Items.Add(Path.GetFileName(file));
            }
        }

        private void buttonAddFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PGN Files (*.pgn)|*.pgn|All Files (*.*)|*.*";
                ofd.Multiselect = true;
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    AddPGNFiles(ofd.FileNames);
                }
            }
        }

        private void buttonRemoveFile_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedIndex >= 0)
            {
                int index = listBoxFiles.SelectedIndex;
                string file = pgnFiles[index];
                
                pgnFiles.RemoveAt(index);
                if (loadedGames.ContainsKey(file))
                    loadedGames.Remove(file);
                    
                SavePGNFilesToRegistry();
                RefreshFileList();
                
                listBoxGames.Items.Clear();
                listBoxVariations.Items.Clear();
            }
        }

        private void listBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxGames.Items.Clear();
            listBoxVariations.Items.Clear();

            if (listBoxFiles.SelectedIndex < 0)
                return;

            string filePath = pgnFiles[listBoxFiles.SelectedIndex];

            // Load games if not already loaded
            if (!loadedGames.ContainsKey(filePath))
            {
                try
                {
                    string pgnText = File.ReadAllText(filePath);
                    var games = parser.ParseGames(pgnText);
                    loadedGames[filePath] = games;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading PGN file: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Display games
            var gameList = loadedGames[filePath];
            foreach (var game in gameList)
            {
                listBoxGames.Items.Add(game.ToString());
            }
        }

        private void listBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxVariations.Items.Clear();

            if (listBoxFiles.SelectedIndex < 0 || listBoxGames.SelectedIndex < 0)
                return;

            string filePath = pgnFiles[listBoxFiles.SelectedIndex];
            var games = loadedGames[filePath];
            var selectedGame = games[listBoxGames.SelectedIndex];

            // Extract variations
            var variations = VariationExtractor.ExtractVariations(selectedGame);

            // Display variations
            for (int i = 0; i < variations.Count; i++)
            {
                var variation = variations[i];
                string display = $"Variation {i + 1} ({variation.MoveCount} moves): {variation.GetPreview(10)}";
                listBoxVariations.Items.Add(display);
            }

            // Store variations for later use
            listBoxVariations.Tag = variations;
        }

        private void buttonPractice_Click(object sender, EventArgs e)
        {
            StartPracticeMode();
        }

        private void listBoxVariations_DoubleClick(object sender, EventArgs e)
        {
            StartPracticeMode();
        }

        private void StartPracticeMode()
        {
            if (listBoxVariations.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a variation to practice.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var variations = listBoxVariations.Tag as List<VariationLine>;
            if (variations != null && listBoxVariations.SelectedIndex < variations.Count)
            {
                // Get the currently selected file
                string sourceFile = listBoxFiles.SelectedIndex >= 0 && listBoxFiles.SelectedIndex < pgnFiles.Count
                    ? pgnFiles[listBoxFiles.SelectedIndex]
                    : null;
                
                // Pass all variations with the selected one as the starting point and source file
                var assistedForm = new AssistedModeForm(variations, listBoxVariations.SelectedIndex, sourceFile);
                assistedForm.ShowDialog(this);
            }
        }

        private void ShowSettings()
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog(this);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void buttonMetrics_Click(object sender, EventArgs e)
        {
            // Get currently selected file if any
            string sourceFile = listBoxFiles.SelectedIndex >= 0 && listBoxFiles.SelectedIndex < pgnFiles.Count
                ? pgnFiles[listBoxFiles.SelectedIndex]
                : null;
            
            var metricsForm = new MetricsForm(sourceFile);
            if (metricsForm.ShowDialog(this) == DialogResult.OK && metricsForm.ShouldPractice)
            {
                // User wants to practice a variation from metrics
                var varData = metricsForm.SelectedVariation;
                var varLine = StudyDataManager.ConvertToVariationLine(varData);
                
                // Start practice mode with this variation
                var assistedForm = new AssistedModeForm(varLine, varData.SourceFile);
                assistedForm.ShowDialog(this);
            }
        }
    }
}
