using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;

namespace ChessPuzzleSimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            databaseFile = RegistryUtils.GetString("databaseFile", null);
            if (databaseFile != null)
            {
                LoadPuzzleData(databaseFile);
            }
        }

        string databaseFile = null;


        ChessPuzzleData puzzleData = new ChessPuzzleData();
        ChessBoard chessBoard;

        private void LoadPuzzleData(string fileName)
        {
            puzzleData = ChessPuzzleManager.LoadChessPuzzleData(fileName);

            if (puzzleData != null)
            {
                textBoxDatabaseFilename.Text = fileName;
            }

            RefreshPuzzleData(puzzleData);
        }


        int lastSelectedPuzzle = -1;
        private void RefreshPuzzleData(ChessPuzzleData puzzleData)
        {
            int lastSelectedGroup = listBoxGroup.SelectedIndex;
            lastSelectedPuzzle = listBoxPuzzle.SelectedIndex;

            listBoxGroup.Items.Clear();
            listBoxPuzzle.Items.Clear();

            if (puzzleData.PuzzleGroups == null)
            {
                return;
            }

            foreach (ChessPuzzleGroup group in puzzleData.PuzzleGroups)
            {
                listBoxGroup.Items.Add(group);
            }

            if (lastSelectedGroup != -1)
            {
                if (lastSelectedGroup >= listBoxGroup.Items.Count)
                {
                    lastSelectedGroup = listBoxGroup.Items.Count - 1;
                }

                if (lastSelectedGroup >= 0)
                {
                    listBoxGroup.SelectedIndex = lastSelectedGroup;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fen = "8/ppp2k1K/8/7p/PP6/2P5/1r4PP/8 w - - 2 32";
            List<string> pgn = new List<string> { "g2g3", "h5h4", "g3h4" };//, "f6f7" };

            if (textBoxMoves.Text.Length > 0)
            {
                var moves = textBoxMoves.Text.Split(new char[] { ',' });
                pgn.Clear();
                foreach (var move in moves)
                {
                    pgn.Add(move);
                }
            }

            ChessPuzzleSimulator sim = new ChessPuzzleSimulator(fen, pgn, false);
            //ChessPuzzleSimulatorForm sim = new ChessPuzzleSimulatorForm(chessBoard, false);
            //Application.Run(new ChessPuzzleSimulator("start-fen-here", new List<string> { "move1", "move2" }));
            sim.Width = 1000;
            sim.Height = 1000;
            sim.ShowDialog(this);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (GetSelectedGroup() == null)
            {
                MessageBox.Show("No group selected");
                return;
            }



            CreateEditPuzzle form = new CreateEditPuzzle();
            if (form.ShowDialog(this) != DialogResult.OK)
            {

            }

            if (form.Name.Length <= 0)
            {
                MessageBox.Show("No Name Provided");
                return;
            }

            if (form.FEN.Length <= 0)
            {
                MessageBox.Show("No FEN provided");
                return;
            }

            string fen = form.FEN;// "8/ppp2k1K/8/7p/PP6/2P5/1r4PP/8 w - - 2 32";

            List<string> pgn = new List<string>();// { "g2g3", "h5h4", "g3h4" };//, "f6f7" };

            ChessPuzzleSimulator sim = new ChessPuzzleSimulator(fen, pgn, true);

            sim.Width = 1000;
            sim.Height = 1000;
            sim.ShowDialog(this);
            StringBuilder sb = new StringBuilder();
            foreach (var move in sim.GetRecordedMoves())
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(move);
            }
            textBoxMoves.Text = sb.ToString();

            ChessPuzzleGroup group = GetSelectedGroup();
            if (group.Puzzles == null)
            {
                group.Puzzles = new List<ChessPuzzleItem>();
            }


            ChessPuzzleItem puzzle = CreatePuzzle(form.PuzzleName, form.PuzzleDescription, fen, sb.ToString());

            group.Puzzles.Add(puzzle);

            RefreshPuzzleData(puzzleData);
            ChessPuzzleManager.SaveChessPuzzleData(databaseFile, puzzleData);
        }

        private ChessPuzzleItem CreatePuzzle(string name, string description, string fen, string moves)
        {
            ChessPuzzleItem puzzle = new ChessPuzzleItem();
            puzzle.Id = Guid.NewGuid().ToString();
            puzzle.Name = name;
            puzzle.Description = description;
            puzzle.FEN = fen;
            puzzle.Moves = moves;
            return puzzle;
        }



        private ChessPuzzleGroup CreatePuzzleGroup(string name, string description)
        {
            ChessPuzzleGroup group = new ChessPuzzleGroup();
            group.Id = Guid.NewGuid().ToString();
            group.Name = name;
            group.Description = description;
            return group;
        }

        private void buttonCreateGroup_Click(object sender, EventArgs e)
        {
            CreateEditGroup form = new CreateEditGroup();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                ChessPuzzleGroup group = CreatePuzzleGroup(form.GroupName, form.GroupDescription);
                if (puzzleData.PuzzleGroups == null)
                {
                    puzzleData.PuzzleGroups = new List<ChessPuzzleGroup>();
                }
                puzzleData.PuzzleGroups.Add(group);

                RefreshPuzzleData(puzzleData);
                ChessPuzzleManager.SaveChessPuzzleData(databaseFile, puzzleData);
            }
        }

        private void buttonSetDatabase_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                databaseFile = saveFileDialog.FileName;
                loadDatabase(databaseFile);
            }
        }

        private void loadDatabase(string databaseFile)
        {
            RegistryUtils.SetString("databaseFile", databaseFile);

            puzzleData = ChessPuzzleManager.LoadChessPuzzleData(databaseFile);
            if (puzzleData == null)
            {
                puzzleData = new ChessPuzzleData();
            }
            else
            {
                textBoxDatabaseFilename.Text = databaseFile;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (databaseFile == null)
            {
                MessageBox.Show("Please set a database file");
            }
        }

        private void buttonOpenDatabase_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                databaseFile = openFileDialog.FileName;
                loadDatabase(databaseFile);
            }
        }

        private void buttonDeleteGroup_Click(object sender, EventArgs e)
        {
            if (listBoxGroup.SelectedItems.Count == 0)
            {
                return;
            }

            ChessPuzzleGroup group = (ChessPuzzleGroup)listBoxGroup.SelectedItem;
            if (group == null)
            {
                return;
            }

            puzzleData.PuzzleGroups.Remove(group);

            RefreshPuzzleData(puzzleData);
            ChessPuzzleManager.SaveChessPuzzleData(databaseFile, puzzleData);
        }

        private void buttonEditGroup_Click(object sender, EventArgs e)
        {
            if (listBoxGroup.SelectedItems.Count == 0)
            {
                return;
            }

            ChessPuzzleGroup group = (ChessPuzzleGroup)listBoxGroup.SelectedItem;
            if (group == null)
            {
                return;
            }

            CreateEditGroup form = new CreateEditGroup(group.Name, group.Description);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                RefreshPuzzleData(puzzleData);
                ChessPuzzleManager.SaveChessPuzzleData(databaseFile, puzzleData);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private ChessPuzzleGroup GetSelectedGroup()
        {
            if (listBoxGroup.SelectedItems.Count <= 0)
            {
                return null;
            }

            ChessPuzzleGroup group = (ChessPuzzleGroup)listBoxGroup.SelectedItem;
            if (group == null)
            {
                return null;
            }

            return group;
        }

        private void listBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChessPuzzleGroup group = GetSelectedGroup();

            ShowGroupPuzzles(group);
        }

        private void ShowGroupPuzzles(ChessPuzzleGroup group)
        {
            if (group == null)
            {
                return;
            }

            listBoxPuzzle.Items.Clear();

            if (group.Puzzles == null)
            {
                return;
            }

            foreach (ChessPuzzleItem item in group.Puzzles)
            {
                listBoxPuzzle.Items.Add(item);
            }

            if (lastSelectedPuzzle >= 0)
            {
                if (lastSelectedPuzzle >= listBoxPuzzle.Items.Count)
                {
                    lastSelectedPuzzle = listBoxPuzzle.Items.Count - 1;
                }

                if (lastSelectedPuzzle >= 0)
                {
                    listBoxPuzzle.SelectedIndex = lastSelectedPuzzle;
                }
            }

        }


        private ChessPuzzleItem GetSelectedPuzzle()
        {
            if (listBoxPuzzle.SelectedItems.Count <= 0)
            {
                return null;
            }

            ChessPuzzleItem puzzle = (ChessPuzzleItem)listBoxPuzzle.SelectedItem;
            if (puzzle == null)
            {
                return null;
            }

            return puzzle;
        }


        private void PlayPuzzle(ChessPuzzleItem puzzle)
        {
            string fen = puzzle.FEN;
            List<string> pgn = new List<string>();

            var moves = puzzle.Moves.Split(new char[] { ',' });
            pgn.Clear();
            foreach (var move in moves)
            {
                pgn.Add(move);
            }

            ChessPuzzleSimulator sim = new ChessPuzzleSimulator(fen, pgn, false);
            sim.Width = 1000;
            sim.Height = 1000;
            sim.ShowDialog(this);
        }

        private void buttonPlaySelected_Click(object sender, EventArgs e)
        {
            ChessPuzzleItem puzzle = GetSelectedPuzzle();
            if (puzzle == null)
            {
                return;
            }

            PlayPuzzle(puzzle);
        }

        private void buttonDeletePuzzle_Click(object sender, EventArgs e)
        {
            ChessPuzzleItem puzzle = GetSelectedPuzzle();
            if (puzzle == null)
            {
                return;
            }

            ChessPuzzleGroup group = GetSelectedGroup();
            if (group == null)
            {
                return;
            }

            group.Puzzles.Remove(puzzle);

            RefreshPuzzleData(puzzleData);
            ChessPuzzleManager.SaveChessPuzzleData(databaseFile, puzzleData);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChessPuzzleItem puzzle = GetSelectedPuzzle();

            CreateEditPuzzle form = new CreateEditPuzzle(puzzle.Name, puzzle.Description, puzzle.FEN, puzzle.Moves);


            if (form.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            puzzle.Name = form.PuzzleName;
            puzzle.Description = form.PuzzleDescription;
            puzzle.FEN = form.FEN;
            puzzle.Moves = form.Moves;
        }

        private void listBoxPuzzle_DoubleClick(object sender, EventArgs e)
        {
            ChessPuzzleItem puzzle = GetSelectedPuzzle();
            if (puzzle == null)
            {
                return;
            }

            PlayPuzzle(puzzle);
        }

        private void listBoxPuzzle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChessPuzzleItem puzzle = GetSelectedPuzzle();
            if (puzzle == null)
            {
                return;
            }

            textBoxFen.Text = puzzle.FEN;
            textBoxMoves.Text = puzzle.Moves;
        }

        List<PgnGame> pgnGames = null;
        private void button3_Click(object sender, EventArgs e)
        {

            var fileData = ReadFileContents("c:\\data\\testpgn.pgn");

            var parser = new PgnParser();
            var games = parser.ParseGames(fileData);// samplePgn);

            // We only expect 1 game from that PGN, but let's just iterate:
            foreach (var game in games)
            {
                Console.WriteLine("===== GAME =====");
                foreach (var tag in game.Tags)
                {
                    Console.WriteLine($"[{tag.Key} \"{tag.Value}\"]");
                }
                Console.WriteLine();

                // Print the move tree
                PrintMoveTree(game.MoveTreeRoot, 0);
            }

            pgnGames = games;
            gameIndex = 0;

        }

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

        private void buttonPlayPGN_Click(object sender, EventArgs e)
        {
            if (pgnGames == null)
            {
                return;
            }

            if (gameIndex < pgnGames.Count)
            {
                PgnGame game = pgnGames[gameIndex];

                string fen = GetFen(game);
                if (String.IsNullOrEmpty(fen))
                {
                    //If no FEN provided then assume it is the start of the game
                    fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
                }

                chessBoard = new ChessBoard(fen);

                ChessPuzzleSimulatorForm sim = new ChessPuzzleSimulatorForm(chessBoard, game, true, ChessPuzzleTrainingMode.Explore, false, false, false);
                //Application.Run(new ChessPuzzleSimulator("start-fen-here", new List<string> { "move1", "move2" }));
                sim.Width = 1000;
                sim.Height = 1000;
                sim.ShowDialog(this);
                gameIndex++;
            } else {
                MessageBox.Show("No More Games");
            }

            

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

        

    }
}
