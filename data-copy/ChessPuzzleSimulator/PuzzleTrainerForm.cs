using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessPuzzleSimulator
{
    public partial class PuzzleTrainerForm : Form
    {

        ChessPuzzleConfig config = new ChessPuzzleConfig();
        ChessBoard chessBoard;
        string configFile = null;

        //When releasing a new version you must update
        //  Version here
        //  Version in assembly
        //  Version in Setup and have it update the product code

        public string Version = "0.9.4b";



        public PuzzleTrainerForm()
        {
            InitializeComponent();

            configFile = RegistryUtils.GetString("configFile", null);
            if (configFile != null)
            {
                LoadConfigData(configFile);
            }

            checkBoxAllVariations.Checked = RegistryUtils.GetBool("AllVariations", false);
            checkBoxAutoNextPuzzle.Checked = RegistryUtils.GetBool("AutoNextPuzzle", false);
            checkBoxMultiUserMove.Checked = RegistryUtils.GetBool("NotifyMultiUserMove", false);

            // 1. Enable drag and drop
            this.AllowDrop = true;

            // 2. DragEnter event: Decide if we can accept the data
            this.DragEnter += MainForm_DragEnter;

            // 3. DragDrop event: Handle the dropped files
            this.DragDrop += MainForm_DragDrop;

            this.Text += " (" + Version + ")";



        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (configFile == null)
            {
                e.Effect = DragDropEffects.None;
            }

            // Check if the data is a file drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Extract file names
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // If all dropped files have a .pgn extension, show copy cursor
                if (files.All(file => Path.GetExtension(file).Equals(".pgn", StringComparison.OrdinalIgnoreCase)))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (config == null)
            {
                return;
            }
            // Retrieve the dropped files
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Filter only .pgn files and call LoadPGN
            string[] pgnFiles = files
                .Where(file => Path.GetExtension(file).Equals(".pgn", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (pgnFiles.Length > 0)
            {
                LoadPGN(pgnFiles);
            }
        }

        private void LoadPGN(string[] pgnFiles)
        {
            if (config.PGNData == null)
            {
                config.PGNData = new List<ChessPGNData>();
            }

            foreach (string filename in pgnFiles)
            {
                ChessPGNData chessPGNData = new ChessPGNData();
                chessPGNData.Filename = filename;
                config.PGNData.Add(chessPGNData);
            }

            ChessConfigManager.SaveChessConfigData(configFile, config);
            RefreshUI();
        }

        private void LoadConfigData(string fileName)
        {
            RegistryUtils.SetString("configFile", fileName);

            config = ChessConfigManager.LoadChessConfigData(fileName);

            if (config == null)
            {
                config = new ChessPuzzleConfig();
            }

            SetStatus("Config file: " + fileName);
            //textBoxConfigFilename.Text = fileName;

            RefreshUI();
        }

        int lastSelectedPGN = -1;
        private void RefreshUI()
        {
            if (config == null)
            {
                return;
            }
            int lastSelectedPGN = listBoxPGNFiles.SelectedIndex;
            int lastSelectedGame = listBoxPGNGames.SelectedIndex;

            listBoxPGNFiles.Items.Clear();
            listBoxPGNGames.Items.Clear();

            if (config.PGNData == null || config.PGNData.Count == 0)
            {
                return;
            }

            foreach (var item in config.PGNData)
            {
                listBoxPGNFiles.Items.Add(item);
            }


            if (lastSelectedPGN != -1)
            {
                if (lastSelectedPGN >= listBoxPGNFiles.Items.Count)
                {
                    lastSelectedPGN = listBoxPGNFiles.Items.Count - 1;
                }

                if (lastSelectedPGN >= 0)
                {
                    listBoxPGNFiles.SelectedIndex = lastSelectedPGN;
                }
            }

            if (lastSelectedGame != -1)
            {
                if (lastSelectedGame >= listBoxPGNGames.Items.Count)
                {
                    lastSelectedGame = listBoxPGNGames.Items.Count - 1;
                }

                if (lastSelectedGame >= 0)
                {
                    listBoxPGNGames.SelectedIndex = lastSelectedGame;
                }
            }

            textBoxLog.Text = LogHeader() + "\r\n" + config.Log;
        }


        private void loadPGN()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (config.PGNData == null)
                {
                    config.PGNData = new List<ChessPGNData>();
                }
                ChessPGNData chessPGNData = new ChessPGNData();
                chessPGNData.Filename = openFileDialog.FileName;
                config.PGNData.Add(chessPGNData);

                ChessConfigManager.SaveChessConfigData(configFile, config);
                RefreshUI();
            }
        }

        private void buttonLoadPGN_Click(object sender, EventArgs e)
        {
            loadPGN();
        }

        private void LoadPGNFile(string filename)
        {
            var fileData = ReadFileContents(filename);// "c:\\data\\testpgn.pgn");

            var parser = new PgnParser();
            var games = parser.ParseGames(fileData);

            listBoxPGNGames.Items.Clear();

            foreach (var game in games)
            {
                Console.WriteLine("===== GAME =====");
                foreach (var tag in game.Tags)
                {
                    Console.WriteLine($"[{tag.Key} \"{tag.Value}\"]");
                }
                Console.WriteLine();

                // Print the move tree (also fixes the SAN's)
                PrintMoveTree(game.MoveTreeRoot, 0);

                listBoxPGNGames.Items.Add(game);
            }

            pgnGames = games;
            gameIndex = 0;



        }




        List<PgnGame> pgnGames = null;

        public static string ReadFileContents(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow as needed
                Console.WriteLine($"Error reading file: {ex.Message}");
                return string.Empty;
            }
        }

        private static void fixSan(MoveNode node)
        {
            if (node.San.EndsWith("+") || node.San.EndsWith("#"))
            {
                node.SanExtra = node.San.Substring(node.San.Length - 1);
                node.San = node.San.Substring(0, node.San.Length - 1);
            }
        }
        private static void PrintMoveTree(MoveNode node, int depth)
        {
            // We skip printing the "root" if it has no SAN
            if (!string.IsNullOrEmpty(node.San) || node.MoveNumber > 0)
            {
                fixSan(node);
                var indent = new string(' ', depth * 2);
                string nags = node.Nags.Count > 0 ? " " + string.Join(" ", node.Nags) : "";
                Console.WriteLine($"{indent}{node.MoveNumber}. {node.San}{nags}");

                if (!string.IsNullOrEmpty(node.Comment))
                {
                    Console.WriteLine($"{indent}    {{Comment: {node.Comment}}}");
                }
            }

            // Recurse for each child branch
            foreach (var child in node.NextMoves)
            {
                PrintMoveTree(child, depth + 1);
            }
        }

        int gameIndex = 0;
        public string JoinWithComma(IEnumerable<string> items)
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.Append(item);
                first = false;
            }
            return sb.ToString();
        }

        private bool PlayPGN(PgnGame game, ChessPuzzleTrainingMode trainingMode, bool allVariations, bool playOppositeColor)
        {
            string fen = GetFen(game);
            if (String.IsNullOrEmpty(fen))
            {
                //If no FEN provided then assume it is the start of the game
                fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }

            chessBoard = new ChessBoard(fen);
            DateTime dateTimeStart = DateTime.Now;
            ChessPuzzleSimulatorForm sim = new ChessPuzzleSimulatorForm(chessBoard, game, allVariations, trainingMode, false, playOppositeColor, checkBoxMultiUserMove.Checked);
            DialogResult result = sim.ShowDialog(this);



            StringBuilder sb = new StringBuilder();
            List<string> items = new List<string>();
            String filename = "<unknown>";
            if (listBoxPGNFiles.SelectedItem != null)
            {
                ChessPGNData pgnData = (ChessPGNData)listBoxPGNFiles.SelectedItem;
                filename = pgnData.ToString();
            }
            items.Add(filename);
            items.Add(game.ToString().Replace(",", "."));
            items.Add(dateTimeStart.ToString());
            //items.Add(DateTime.Now.ToString());
            items.Add((DateTime.Now - dateTimeStart).TotalSeconds.ToString());
            items.Add((result == DialogResult.OK ? "Complete" : "Canceled"));
            items.Add(allVariations ? "AllVariations" : "MainLine");
            items.Add(trainingMode.ToString());
            items.Add(sim.MovesCorrect.ToString());
            items.Add(sim.MovesIncorrect.ToString());
            items.Add(sim.NodesExplored.ToString());
            items.Add(sim.CountUnVisitedNodes.ToString());

            config.Log = JoinWithComma(items) + "\r\n" + config.Log;
            ChessConfigManager.SaveChessConfigData(configFile, config);
            RefreshUI();

            if (result == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        private string LogHeader()
        {
            List<string> items = new List<string>();
            items.Add("Filename");
            items.Add("Puzzle");
            items.Add("Start");
            //items.Add("EndTime");
            items.Add("Seconds");
            items.Add("Result");
            items.Add("Variations");
            items.Add("Mode");
            items.Add("Correct");
            items.Add("Incorrect");
            items.Add("Nodes Explored");
            items.Add("Nodes Unvisited");
            return JoinWithComma(items);
        }




        private string GetFen(PgnGame game)
        {
            if (game == null)
            {
                return null;
            }

            foreach (var tag in game.Tags)
            {
                if (tag.Key.ToUpper() == "FEN")
                {
                    return tag.Value;
                }
            }

            return null;
        }

        private void createConfig()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                configFile = saveFileDialog.FileName;
                LoadConfigData(configFile);
            }
        }

        private void openConfig()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                configFile = openFileDialog.FileName;
                LoadConfigData(configFile);
            }
        }
        private void buttonCreateConfig_Click(object sender, EventArgs e)
        {
            createConfig();
        }

        private void SetStatus(string val)
        {
            toolStripStatusLabel1.Text = val;
        }

        private void PuzzleTrainerForm_Shown(object sender, EventArgs e)
        {
            if (configFile == null)
            {
                SetStatus("Please set a config file (Config -> Create Config), otherwise nothing will be saved when program is closed.");
                MessageBox.Show("Please set a config file to store local data (Config -> Create Config), otherwise nothing will be saved when program is closed.", "Notice");
            }
        }


        private void buttonOpenConfig_Click(object sender, EventArgs e)
        {
            openConfig();
        }

        private void listBoxPGNFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPGNFiles.SelectedIndex == -1)
            {
                return;
            }

            ChessPGNData pgnData = (ChessPGNData)listBoxPGNFiles.SelectedItem;

            LoadPGNFile(pgnData.Filename);

            if (listBoxPGNGames.Items.Count > 0)
            {
                listBoxPGNGames.SelectedIndex = 0;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int lastSelected = listBoxPGNFiles.SelectedIndex;
            if (listBoxPGNFiles.SelectedIndex == -1)
            {
                return;
            }
            if (config == null || config.PGNData == null)
            {
                return;
            }

            ChessPGNData pgnData = (ChessPGNData)listBoxPGNFiles.SelectedItem;
            listBoxPGNFiles.Items.Remove(pgnData);
            config.PGNData.Remove(pgnData);

            lastSelected--;
            if (lastSelected >= 0)
            {
                listBoxPGNFiles.SelectedIndex = lastSelected;
            }

            ChessConfigManager.SaveChessConfigData(configFile, config);
            RefreshUI();
        }
        private void buttonPlayPGN_Click(object sender, EventArgs e)
        {
            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;
            PlayPGN(game, ChessPuzzleTrainingMode.Explore, true, false);


            //if (gameIndex < pgnGames.Count)
            //{
            //    PgnGame game = pgnGames[gameIndex];

            //    string fen = GetFen(game);
            //    if (String.IsNullOrEmpty(fen))
            //    {
            //        //If no FEN provided then assume it is the start of the game
            //        fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            //    }

            //    chessBoard = new ChessBoard(fen);

            //    ChessPuzzleSimulatorForm sim = new ChessPuzzleSimulatorForm(chessBoard, game, true, false);
            //    //Application.Run(new ChessPuzzleSimulator("start-fen-here", new List<string> { "move1", "move2" }));
            //    sim.Width = 1000;
            //    sim.Height = 1000;
            //    sim.ShowDialog(this);
            //    gameIndex++;
            //}
            //else
            //{
            //    MessageBox.Show("No More Games");
            //}



        }
        private void buttonTestPGN_Click(object sender, EventArgs e)
        {

            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;

            while (PlayPGN(game, ChessPuzzleTrainingMode.Test, checkBoxAllVariations.Checked, false))
            {

                if (checkBoxAutoNextPuzzle.Checked)
                {
                    int lastSelectedGame = listBoxPGNGames.SelectedIndex;
                    lastSelectedGame++;
                    if (lastSelectedGame < listBoxPGNGames.Items.Count)
                    {
                        listBoxPGNGames.SelectedIndex = lastSelectedGame;
                        game = (PgnGame)listBoxPGNGames.SelectedItem;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {

        }

        private void listBoxPGNGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxPGNInfo.Text = "";

            groupBoxPGNGames.Text = "PGN Games";
            //labelPGNGames.Text = "PGN Games";

            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            groupBoxPGNGames.Text += " (" + (listBoxPGNGames.SelectedIndex + 1) + " / " + listBoxPGNGames.Items.Count + ")";

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;
            StringBuilder sb = new StringBuilder();
            foreach (var tag in game.Tags)
            {
                sb.AppendLine(tag.Key + ": " + tag.Value);
            }

            sb.AppendLine();
            (int nodeCount, int variations) = CountNodes(game.MoveTreeRoot);
            sb.AppendLine("Move Count: " + nodeCount);
            sb.AppendLine("Variations: " + variations);

            textBoxPGNInfo.Text = sb.ToString();

        }


        public int nodeCount = 0;
        public int variations = 0;
        private void CountNodes(MoveNode node, int depth)
        {
            // We skip printing the "root" if it has no SAN
            if (!string.IsNullOrEmpty(node.San) || node.MoveNumber > 0)
            {
                nodeCount++;
            }

            // Recurse for each child branch
            foreach (var child in node.NextMoves)
            {
                if (child.NextMoves != null && child.NextMoves.Count > 1)
                {
                    variations++;
                }
                CountNodes(child, depth + 1);
            }
        }

        private (int, int) CountNodes(MoveNode root)
        {
            nodeCount = 0;
            variations = 0;
            CountNodes(root, 0);
            return (nodeCount, variations);
        }

        private void listBoxPGNGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;
            PlayPGN(game, ChessPuzzleTrainingMode.Test, checkBoxAllVariations.Checked, false);
        }

        private void checkBoxAllVariations_CheckedChanged(object sender, EventArgs e)
        {
            RegistryUtils.SetBool("AllVariations", checkBoxAllVariations.Checked);

        }

        private void checkBoxAutoNextPuzzle_CheckedChanged(object sender, EventArgs e)
        {
            RegistryUtils.SetBool("AutoNextPuzzle", checkBoxAutoNextPuzzle.Checked);
        }

        private void buttonTrainPGN_Click(object sender, EventArgs e)
        {
            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;

            PlayPGN(game, game.MoveTreeRoot.isWhiteTurn ? ChessPuzzleTrainingMode.LearnWhite : ChessPuzzleTrainingMode.LearnBlack, checkBoxAllVariations.Checked, !game.MoveTreeRoot.isWhiteTurn);
        }

        private void buttonTrainPGNWhite_Click(object sender, EventArgs e)
        {
            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;

            PlayPGN(game, ChessPuzzleTrainingMode.LearnWhite, checkBoxAllVariations.Checked, game.MoveTreeRoot.isWhiteTurn);
        }

        private void buttonTrainPGNBlack_Click(object sender, EventArgs e)
        {
            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;

            PlayPGN(game, ChessPuzzleTrainingMode.LearnBlack, checkBoxAllVariations.Checked, !game.MoveTreeRoot.isWhiteTurn);
        }

        private void buttonTestAsWhite_Click(object sender, EventArgs e)
        {

            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;

            PlayPGN(game, ChessPuzzleTrainingMode.Test, checkBoxAllVariations.Checked, game.MoveTreeRoot.isWhiteTurn);
        }

        private void buttonTestAsBlack_Click(object sender, EventArgs e)
        {
            if (listBoxPGNGames.SelectedIndex == -1)
            {
                return;
            }

            PgnGame game = (PgnGame)listBoxPGNGames.SelectedItem;

            PlayPGN(game, ChessPuzzleTrainingMode.Test, checkBoxAllVariations.Checked, !game.MoveTreeRoot.isWhiteTurn);
        }

        private void listBoxPGNGames_DoubleClick_1(object sender, EventArgs e)
        {
            buttonTestPGN_Click(sender, e);
        }

        private void createConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createConfig();
        }

        private void loadOpenConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openConfig();
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (config == null)
            {
                return;
            }
            config.Log = "";
            ChessConfigManager.SaveChessConfigData(configFile, config);
            RefreshUI();
        }

        private void loadPGNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadPGN();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm(Version);
            form.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerLicenseCheck.Enabled = false;
            if (!RegistryUtils.GetBool("AcceptLicense", false))
            {
                LicenseForm licenseForm = new LicenseForm();
                if (licenseForm.ShowDialog() != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
                else
                {
                    RegistryUtils.SetBool("AcceptLicense", true);
                }
            }
        }

        private void resetPuzzleScreenLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Click OK to reset the location of the puzzle view.\n\nThis is only needed if the puzzle is no longer visible on the screen.\n\nIt will not affect the config or any puzzle results.", "Notice", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                RegistryUtils.SetInt("PuzzleTop", -1000);
                RegistryUtils.SetInt("PuzzleLeft", -1000);
                RegistryUtils.SetInt("PuzzleWidth", -1000);
                RegistryUtils.SetInt("PuzzleHeight", -1000);
                MessageBox.Show("Puzzle view location has been reset.\n\nPlease open a puzzle to check the location of the form.\n\nIf you need further assistance, contact us via the email on the Help->About form", "Notice");
            }
        }

        private void checkBoxMultiUserMove_CheckedChanged(object sender, EventArgs e)
        {
            RegistryUtils.SetBool("NotifyMultiUserMove", checkBoxAutoNextPuzzle.Checked);
        }
    }
}
