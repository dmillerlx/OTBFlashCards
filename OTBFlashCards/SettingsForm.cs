using System;
using System.IO;
using System.Windows.Forms;

namespace OTBFlashCards
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            string currentPath = StudyDataManager.GetDataFilePath();
            if (!string.IsNullOrEmpty(currentPath))
            {
                textBoxDataPath.Text = currentPath;
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                sfd.DefaultExt = "json";
                sfd.FileName = "OTBFlashCards_StudyData.json";
                
                if (!string.IsNullOrEmpty(textBoxDataPath.Text))
                {
                    sfd.InitialDirectory = Path.GetDirectoryName(textBoxDataPath.Text);
                }
                
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    textBoxDataPath.Text = sfd.FileName;
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDataPath.Text))
            {
                MessageBox.Show("Please select a location for the study data file.", "Location Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StudyDataManager.SetDataFilePath(textBoxDataPath.Text);
            
            // Create file if it doesn't exist
            if (!File.Exists(textBoxDataPath.Text))
            {
                StudyDataManager.Save();
            }
            else
            {
                StudyDataManager.Load();
            }

            MessageBox.Show("Settings saved successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            
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
