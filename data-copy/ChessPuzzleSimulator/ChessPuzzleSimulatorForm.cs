using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ChessPuzzleSimulator
{
    public partial class ChessPuzzleSimulatorForm : Form
    {
        public ChessPuzzleSimulatorForm()
        {
            InitializeComponent();
            hidePictureBox();
        }

        ChessPuzzleTrainingMode trainingMode;

        private Panel chessBoard;
        private Label[,] labels = new Label[8, 8];

        private Label FailedMove = new Label();
        private Label SuccessVariation = new Label();
        private Label PuzzleComplete = new Label();
        private Label StatusLabel = new Label();

        private Dictionary<string, Image> pieceImages = new Dictionary<string, Image>();
        private bool isWhiteBottom = true;
        private string fen;
        private List<string> pgn;
        private int moveIndex = 0;
        private Label selectedLabel;
        private TransparentControl floatingPiece;
        private Point originalMousePosition;

        private const int DefaultImageSize = 40; // Default size for images

        private bool record = false;
        private ChessBoard board;
        private PgnGame game;
        MoveNode node = null;
        MoveNode root = null;
        private bool allVariations = false;
        public int MovesCorrect = 0;
        public int MovesIncorrect = 0;
        public int NodesExplored = 0;
        bool startingIsWhiteToPlay = false;
        bool playOppositeColor = false;

        PictureBox topLevelPictureBox = new PictureBox();
        bool notifyMultiUserMove = false;

        public ChessPuzzleSimulatorForm(ChessBoard board, PgnGame game, bool allVariations, ChessPuzzleTrainingMode trainingMode, bool record, bool playOppositeColor, bool notifyMultiUserMove)
        {
            InitializeComponent();
            hidePictureBox();


            this.board = board;
            this.game = game;
            node = game.MoveTreeRoot;
            root = node;
            board.PrintBoard();
            isWhiteBottom = board.isWhiteTurn;
            LoadPieceImages("C:\\data\\ChessPuzzleSimulator\\images");
            InitializeChessBoardControls();
            InitializeBoard();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Resize += OnResize;            
            this.record = record;
            this.allVariations = allVariations;
            this.trainingMode = trainingMode;
            this.notifyMultiUserMove = notifyMultiUserMove;
            

            this.KeyDown += Form_KeyDown;
            this.playOppositeColor = playOppositeColor;

            startingIsWhiteToPlay = board.isWhiteTurn;
            this.Text = ((board.isWhiteTurn && !playOppositeColor) ? "White to play" : "Black to play");
            
            if (board.isWhiteTurn && playOppositeColor)
            {
                FlipBoard();
            }

            int top = RegistryUtils.GetInt("PuzzleTop", -1000);
            int left = RegistryUtils.GetInt("PuzzleLeft", -1000);
            int width = RegistryUtils.GetInt("PuzzleWidth", -1000);
            int height = RegistryUtils.GetInt("PuzzleHeight", -1000);

            if (top > -1000 && left > -1000 && width > -1000 && height > -1000)
            {
                this.Top = top;
                this.Left = left;
                this.Width = width;
                this.Height = height;
                this.StartPosition = FormStartPosition.Manual;
            } else
            {
                this.Width = 750;
                this.Height = 750;
            }

        }

        HashSet<MoveNode> visited = new HashSet<MoveNode>();

        public int CountUnVisitedNodes = 0;
        private void CountUnvisitedNodes(MoveNode node, int depth)
        {
            // We skip printing the "root" if it has no SAN
            if (!string.IsNullOrEmpty(node.San) || node.MoveNumber > 0)
            {
                if (!visited.Contains(node))
                {
                    CountUnVisitedNodes++;
                }
            }

            // Recurse for each child branch
            foreach (var child in node.NextMoves)
            {
                CountUnvisitedNodes(child, depth + 1);
            }
        }


        private void ResetToRoot()
        {
            Console.WriteLine("----PREV MOVE--BEFORE---");
            bool ret = false;

            board.PrintBoard();

            node = root;
            if (root.boardCache == null)
            {
                board.reloadFen();
            } else {
                board.SetState(node.boardCache, node.isWhiteTurn);
            }
            
            Console.WriteLine("----PREV MOVE--AFTER---");
            board.PrintBoard();
            InitializeBoard();
        }

        bool showVariationComplete = false;

        bool showPuzzleComplete = false;
        private void puzzleDone()
        {
            Console.WriteLine("Puzzle Done");
            if (allVariations) {
                Console.WriteLine("Variations Remaining: " + getCountUnvisited(root));
                if (getCountUnvisited(root) > 0) {
                    showVariationComplete = true;
                    ResetToRoot();
                    return;
                }
            }

            showPuzzleComplete = true;

            
        }

        private void LoadPieceImages(string directoryPath)
        {
            pieceImages["p"] = LoadTransparentImage(Path.Combine(directoryPath, "black_pawn.png"));
            pieceImages["n"] = LoadTransparentImage(Path.Combine(directoryPath, "black_knight.png"));
            pieceImages["b"] = LoadTransparentImage(Path.Combine(directoryPath, "black_bishop.png"));
            pieceImages["r"] = LoadTransparentImage(Path.Combine(directoryPath, "black_rook.png"));
            pieceImages["q"] = LoadTransparentImage(Path.Combine(directoryPath, "black_queen.png"));
            pieceImages["k"] = LoadTransparentImage(Path.Combine(directoryPath, "black_king.png"));
            pieceImages["P"] = LoadTransparentImage(Path.Combine(directoryPath, "white_pawn.png"));
            pieceImages["N"] = LoadTransparentImage(Path.Combine(directoryPath, "white_knight.png"));
            pieceImages["B"] = LoadTransparentImage(Path.Combine(directoryPath, "white_bishop.png"));
            pieceImages["R"] = LoadTransparentImage(Path.Combine(directoryPath, "white_rook.png"));
            pieceImages["Q"] = LoadTransparentImage(Path.Combine(directoryPath, "white_queen.png"));
            pieceImages["K"] = LoadTransparentImage(Path.Combine(directoryPath, "white_king.png"));
            pieceImages["FailedMove"] = LoadTransparentImage(Path.Combine(directoryPath, "FailedMove.png"));
            pieceImages["VariationComplete"] = LoadTransparentImage(Path.Combine(directoryPath, "VariationComplete.png"));
            pieceImages["PuzzleComplete"] = LoadTransparentImage(Path.Combine(directoryPath, "PuzzleComplete.png"));
            pieceImages["MultiUserMove"] = LoadTransparentImage(Path.Combine(directoryPath, "MultiUserMove.png"));
        }

        private Image LoadTransparentImage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Image file not found: " + filePath);
            }

            Bitmap bmp = new Bitmap(filePath);
            bmp.MakeTransparent(Color.White); // Set white background as transparent

            Bitmap transparentImage = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(transparentImage))
            {
                g.Clear(Color.Transparent); // Explicitly set transparent background
                g.DrawImage(bmp, 0, 0);
            }

            return transparentImage;// ResizeImage(transparentImage, new Size(DefaultImageSize, DefaultImageSize));
        }


        private void InitializeChessBoardControls()
        {
            chessBoard = new Panel
            {
                Location = new Point(10, 10),
                BackColor = Color.Transparent // Ensure the parent is transparent
            };
            this.Controls.Add(chessBoard);

            floatingPiece = new TransparentControl
            {
                BackColor = Color.Transparent,
                Visible = false,
                Parent = pictureBoxTop// chessBoard // Ensure it's part of the board to maintain transparency
            };

            StatusLabel = new Label
            {
                Visible = false,
                Parent = chessBoard
            };
            chessBoard.Controls.Add(StatusLabel);

            chessBoard.Controls.Add(floatingPiece);

            topLevelPictureBox = new PictureBox();
            topLevelPictureBox.Image = null;
            topLevelPictureBox.BackColor = Color.Transparent;
            

            ResizeChessBoard();
        }

        //Write labels around board
        private void InitializeBoardLabels()
        {
            if (isWhiteBottom)
            {
                for (int i = 0; i < 8; i++)
                {
                    var rowLabel = new Label
                    {
                        Text = isWhiteBottom ? (8 - i).ToString() : (i + 1).ToString(),
                        AutoSize = true
                    };
                    this.Controls.Add(rowLabel);

                    var colLabel = new Label
                    {
                        Text = isWhiteBottom ? ((char)('a' + i)).ToString() : ((char)('h' - i)).ToString(),
                        AutoSize = true
                    };
                    this.Controls.Add(colLabel);
                }
            } else
            {
                for (int i = 0; i < 8; i++)
                {
                    var rowLabel = new Label
                    {
                        Text = isWhiteBottom ? (8 - i).ToString() : (i + 1).ToString(),
                        AutoSize = true
                    };
                    this.Controls.Add(rowLabel);

                    var colLabel = new Label
                    {
                        Text = isWhiteBottom ? ((char)('a' + i)).ToString() : ((char)('h' - i)).ToString(),
                        AutoSize = true
                    };
                    this.Controls.Add(colLabel);
                }
            }            
        }

        private void InitializeBoard()
        {
            int squareSize = Math.Min(this.ClientSize.Width - 20, this.ClientSize.Height - 20) / 8;
            if (isWhiteBottom)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        string piece = board.get(row, col);
                        if (!String.IsNullOrEmpty(piece) && pieceImages.ContainsKey(piece))
                        {
                            labels[row, col].Image = ResizeImage(pieceImages[piece], new Size(squareSize - 10, squareSize - 10));
                            labels[row, col].Tag = piece;
                        }
                        else
                        {
                            labels[row, col].Image = null;
                            labels[row, col].Text = "";// piece;
                            labels[row, col].Font = new Font("Arial", 24, FontStyle.Bold);
                            labels[row, col].Tag = null;
                        }
                    }
                }
            }
            else
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        string piece = board.get(row, col);
                        if (!String.IsNullOrEmpty(piece) && pieceImages.ContainsKey(piece))
                        {
                            labels[7-row, 7-col].Image = ResizeImage(pieceImages[piece], new Size(squareSize - 10, squareSize - 10));
                            labels[7-row, 7-col].Tag = piece;
                        }
                        else
                        {
                            labels[7-row, 7-col].Image = null;
                            labels[7-row, 7-col].Text = "";// piece;
                            labels[7-row, 7-col].Font = new Font("Arial", 24, FontStyle.Bold);
                            labels[7-row, 7-col].Tag = null;
                        }
                    }
                }
            }


            //If we are in test mode, and if we have multiple moves the user can perform and we have already played
            //one of the moves, and the user wants the notification of multiple moves available
            //then show the multi move notification
            if (node != null && trainingMode == ChessPuzzleTrainingMode.Test)
            {
                if (node.NextMoves != null && node.NextMoves.Count > 1)
                {
                    int visitedNodes = 0;
                    int unVisited = 0;
                    foreach (var mv in node.NextMoves)
                    {                     
                        if (visited.Contains(mv))
                        {
                            visitedNodes++;
                        } else
                        {
                            unVisited++;
                        }
                    }

                    if (visitedNodes > 0)
                    {
                        if (notifyMultiUserMove)
                        {
                            showMultiUserMove = true;
                        }
                    }

                }
            }


        }


        private int getCountUnvisited(MoveNode root)
        {
            CountUnVisitedNodes = 0;
            CountUnvisitedNodes(root, 0);
            return CountUnVisitedNodes;
        }

        private bool NextMove(bool isLearnMode = false)
        {
            this.Text = ((startingIsWhiteToPlay && !playOppositeColor) ? "White to play" : "Black to play") + "  Unvisited Node Count (" + getCountUnvisited(root) + ")";

            if (!isLearnMode)
            {
                NodesExplored++;
            }
            bool ret = false;

            if (node == null)
            {
                return ret;
            }

            if (node.NextMoves != null && node.NextMoves.Count > 0)
            {
                if (allVariations)
                {
                    
                    MoveNode selectedNode = null;
                    foreach (var n in node.NextMoves)
                    {
                        if (getCountUnvisited(n) > 0)
                        {
                            selectedNode = n;
                            break;
                        }
                    }
                    if (selectedNode == null)
                    {
                        selectedNode = node.NextMoves[0];
                    }

                    node = selectedNode;
                }
                else
                {
                    node = node.NextMoves[0];
                }
                
                if (!isLearnMode)
                {
                    visited.Add(node);
                }

                (string[,] boardStateBefore, string[,] boardStateAfter, bool isWhiteTurn, int twoSquareRow, int twoSquareCol) = board.ApplyMove(node.San, node);
                node.Parent.boardCache = boardStateBefore;
                node.Parent.isWhiteTurn = isWhiteTurn;
                node.twoSquareRow = twoSquareRow;
                node.twoSquareCol = twoSquareCol;

                return true;
            }

            if (node.NextMoves == null || node.NextMoves.Count == 0)
            {
                if (!isLearnMode)
                {
                    puzzleDone();
                }
            }

            return ret;
        }

        private bool PrevMode()
        {
            Console.WriteLine("----PREV MOVE--BEFORE---");
            bool ret = false;

            if (node == null)
            {
                return ret;
            }

            if (node.Parent == null)
            {
                return ret;
            }
            
            board.PrintBoard();

            node = node.Parent;
            board.SetState(node.boardCache, node.isWhiteTurn);
            Console.WriteLine("----PREV MOVE--AFTER---");
            board.PrintBoard();

            return true;

        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            bool canMove = trainingMode != ChessPuzzleTrainingMode.Test;
            if (e.KeyCode == Keys.Right && canMove)
            {
                NextMove();
                InitializeBoard();
            }else if (e.KeyCode == Keys.Left && canMove)
            {
                PrevMode();
                InitializeBoard();
            } else if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            } else if (e.KeyCode == Keys.F)
            {
                FlipBoard();
            }
        }


        private void ResizeChessBoard()
        {
            int squareSize = Math.Min(this.ClientSize.Width - 20, this.ClientSize.Height - 20) / 8;
            chessBoard.Size = new Size(squareSize * 8, squareSize * 8);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (labels[i, j] == null)
                    {
                        labels[i, j] = new Label
                        {
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = (i + j) % 2 == 0 ? Color.FromArgb(235, 236, 208) : Color.FromArgb(115, 149, 82),
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        labels[i, j].MouseDown += ChessPieceMouseDown;
                        labels[i, j].MouseMove += ChessPieceMouseMove;
                        labels[i, j].MouseUp += ChessPieceMouseUp;
                        chessBoard.Controls.Add(labels[i, j]);
                    }
                    labels[i, j].Size = new Size(squareSize, squareSize);
                    labels[i, j].Location = new Point(j * squareSize, i * squareSize);

                    // Resize and reassign the image if the piece key exists
                    if (!string.IsNullOrEmpty((string)labels[i, j].Tag) && pieceImages.ContainsKey((string)labels[i, j].Tag))
                    {
                        labels[i, j].Image = ResizeImage(pieceImages[(string)labels[i, j].Tag], new Size(squareSize - 10, squareSize - 10));
                    }
                }
            }

            floatingPiece.Size = new Size(squareSize, squareSize);
            floatingPiece.BackColor = Color.Transparent; // Reinforce transparency

            pictureBoxTop.BackColor = Color.Transparent;
            //pictureBoxTop.Image = pieceImages["VariationComplete"];


            //chessBoard.Controls.Add(topLevelPictureBox);
        }


        public void showPictureBox()
        {
            this.pictureBoxTop.BringToFront();
            this.pictureBoxTop.Show();
        }

        public void hidePictureBox()
        {
            this.pictureBoxTop.Hide();
            this.pictureBoxTop.SendToBack();
        }

        public bool isPictureBoxVisible()
        {
            return this.pictureBoxTop.Visible;
        }

        private void CaptureClientAreaIntoPictureBox()
        {
            // 1. Get the client rectangle in *screen* coordinates.
            Rectangle clientRect = this.RectangleToScreen(this.ClientRectangle);

            // 2. Create a Bitmap just the size of the client area.
            Bitmap bmp = new Bitmap(clientRect.Width, clientRect.Height);

            // 3. Copy from screen using client rectangle's position & size.
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(clientRect.Location, new Point(0, 0), clientRect.Size);
            }

            pictureBoxTop.Image = GetPanelBitmap(chessBoard);
            pictureBoxTop.Top = chessBoard.Top;
            pictureBoxTop.Left = chessBoard.Left;
            pictureBoxTop.Width = chessBoard.Width;
            pictureBoxTop.Height = chessBoard.Height;

            // 4. Assign the bitmap to your PictureBox
            //pictureBoxTop.Image = bmp;
        }

        public static Bitmap GetPanelBitmap(Panel panel)
        {
            // Create a Bitmap that matches the panel’s client size
            Bitmap bmp = new Bitmap(panel.ClientSize.Width, panel.ClientSize.Height);

            // Draw the panel (and its child controls) onto the bitmap
            panel.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
            //string debugPath = @"C:\data\bitmap.bmp";
            //bmp.Save(debugPath, ImageFormat.Bmp);
            return bmp;
        }




        private void OnResize(object sender, EventArgs e)
        {
            ResizeChessBoard();
            SaveFormPosition();


        }

        private void SaveFormPosition()
        {
            RegistryUtils.SetInt("PuzzleTop", this.Top);
            RegistryUtils.SetInt("PuzzleLeft", this.Left);
            RegistryUtils.SetInt("PuzzleWidth", this.Width);
            RegistryUtils.SetInt("PuzzleHeight", this.Height);
        }

        private Image ResizeImage(Image image, Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
            {
                size = new Size(1, 1);
            }
            Bitmap resizedImage = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent);
                g.DrawImage(image, 0, 0, size.Width, size.Height);
            }
            return resizedImage;
        }



        private void ChessPieceMouseDown(object sender, MouseEventArgs e)
        {
            if (lockoutMoveDuringTrainingShow)
            {
                return;
            }

            selectedLabel = sender as Label;
            if (selectedLabel != null && selectedLabel.Image != null)
            {
                originalMousePosition = e.Location;
                floatingPiece.PieceImage = selectedLabel.Image;
                floatingPiece.Size = selectedLabel.Size;
                floatingPiece.BringToFront();
                floatingPiece.Visible = true;
                floatingPiece.Location = chessBoard.PointToClient(Cursor.Position);
                floatingPiece.Parent = pictureBoxTop;

                CaptureClientAreaIntoPictureBox();
                showPictureBox();
                floatingPiece.BringToFront();

                floatingPiece.Invalidate(); // Force redraw
            }
        }





        private void ChessPieceMouseMove(object sender, MouseEventArgs e)
        {
            if (floatingPiece.Visible && e.Button == MouseButtons.Left)
            {
                Point mousePosition = chessBoard.PointToClient(Cursor.Position);
                floatingPiece.Location = new Point(mousePosition.X - floatingPiece.Width / 2, mousePosition.Y - floatingPiece.Height / 2);
                floatingPiece.Invalidate(); // Redraw the control
            }
        }



        private void ChessPieceMouseUp(object sender, MouseEventArgs e)
        {
            if (selectedLabel != null && floatingPiece.Visible)
            {
                hidePictureBox();
                floatingPiece.Visible = false;

                Point mouseLocation = chessBoard.PointToClient(Cursor.Position);
                Label targetLabel = GetLabelAtPosition(mouseLocation);

                if (targetLabel != null && selectedLabel != targetLabel)
                {
                    string move = GenerateMoveFromLabels(selectedLabel, targetLabel);
                    if (!lockoutMoveDuringTrainingShow)
                    {
                        HandleMove(move);
                    }
                }

                selectedLabel = null;
            }
        }

        private List<string> recordMoves = new List<string>();

        public List<string> GetRecordedMoves()
        {
            return recordMoves;
        }


        public static (int row, int col) TranslateToIndex(string coordinate)
        {
            if (string.IsNullOrEmpty(coordinate) || coordinate.Length != 2)
                throw new ArgumentException("Invalid coordinate format. Must be a letter (a-h) followed by a number (1-8).");

            char column = coordinate[0];
            char row = coordinate[1];

            // Validate input
            if (column < 'a' || column > 'h' || row < '1' || row > '8')
                throw new ArgumentException("Coordinate out of range. Must be between a1 and h8.");

            // Convert to indices
            int colIndex = column - 'a'; // 'a' is 0, 'h' is 7
            int rowIndex = row - '1';   // '1' is 0, '8' is 7

            return (rowIndex, colIndex);
        }

        private void HandleMove(string move)
        {
            if (String.IsNullOrEmpty(move))
            {
                return;
            }

            waitBeforeShowingAgain = 0;

            string moveSource = move.Substring(0, 2);
            string moveTarget = move.Substring(2);

            (int sourceRow, int sourceCol) = TranslateToIndex(moveSource);
            (int targetRow, int targetCol) = TranslateToIndex(moveTarget);

            
            string pieceSource = board.get(7-sourceRow, sourceCol);
            string pieceTarget = board.get(7-targetRow, targetCol);

            if (String.IsNullOrEmpty(pieceSource))
            {
                Console.WriteLine("Piece source does not exist");
                return;
            }

            bool takes = false;
            if (!String.IsNullOrEmpty(pieceTarget))
            {
                takes = true;
            }

            bool isPawn = false;
            if (pieceSource == "P" || pieceSource == "p")
            {
                pieceSource = "";
                isPawn = true;
            }

            pieceSource = pieceSource.ToUpper();

            List<string> moveList = new List<string>();

            if (pieceSource == "k" || pieceSource == "K")
            {
                if (sourceRow == 0 && sourceCol == 4 && targetRow == 0 && targetCol == 2)
                {
                    moveList.Add("O-O-O");
                }else if (sourceRow == 0 && sourceCol == 4 && targetRow == 0 && targetCol == 6)
                {
                    moveList.Add("O-O");
                }
                else if (sourceRow == 7 && sourceCol == 4 && targetRow == 7 && targetCol == 6)
                {
                    moveList.Add("O-O");
                }
                else if (sourceRow == 7 && sourceCol == 4 && targetRow == 7 && targetCol == 2)
                {
                    moveList.Add("O-O-O");
                }
            }

            moveList.Add(pieceSource + (takes ? "x" : "") + moveTarget);
            moveList.Add(pieceSource + moveSource[0] + (takes ? "x" : "") + moveTarget);
            moveList.Add(pieceSource + moveSource[1] + (takes ? "x" : "") + moveTarget);
            moveList.Add(pieceSource + moveSource + (takes ? "x" : "") + moveTarget);

            if (isPawn)
            {
                string[] promotePieceList = { "Q", "N", "B" };
                List<string> moveListWithPromote = new List<string>();

                foreach (var moveItem in moveList)
                {
                    foreach (var promoPiece in promotePieceList)
                    {
                        moveListWithPromote.Add(moveItem + "=" + promoPiece);
                    }
                }
                moveList.AddRange(moveListWithPromote);
            }

            bool foundMove = false;
            
            if (node.NextMoves != null && node.NextMoves.Count > 0)
            {
                bool hasUnvisitedNode = false;
                foreach (var mv in node.NextMoves)
                {
                    if (!visited.Contains(mv))
                    {
                        hasUnvisitedNode = true;
                        break;
                    }
                }
                foreach (var mv in node.NextMoves)
                {
                    foreach (var moveItem in moveList)
                    {

                        if (mv.San == moveItem && (!hasUnvisitedNode || (hasUnvisitedNode && visited.Contains(mv) == false)))
                        {
                            node = mv;// node.NextMoves[0];
                            MovesCorrect++;
                            NodesExplored++;
                            visited.Add(node);
                            (string[,] boardStateBefore, string[,] boardStateAfter, bool isWhiteTurn, int twoMoveRow, int twoMoveCol) = board.ApplyMove(node.San, node);
                            node.Parent.boardCache = boardStateBefore;
                            node.Parent.isWhiteTurn = isWhiteTurn;
                            node.twoSquareCol = twoMoveCol;
                            node.twoSquareRow = twoMoveRow;
                            InitializeBoard();
                            foundMove = true;
                            break;
                        }
                    }
                }

            }

            if (!foundMove)
            {
                MovesIncorrect++;
                showFailedMove = true;
                //MessageBox.Show("Try Again");
                return;
            }

            autoPlayNext = true;
            ////Auto play next move
            //NextMove();
            //InitializeBoard();

            //if (node.NextMoves == null || node.NextMoves.Count == 0)
            //{
            //    puzzleDone();
            //}
        }

        bool autoPlayNext = false;




        private string GenerateFenFromLabels()
        {
            string fen = "";

            for (int i = 0; i < 8; i++)
            {
                int emptyCount = 0;
                for (int j = 0; j < 8; j++)
                {
                    string text = labels[i, j].Text;
                    if (string.IsNullOrEmpty(text))
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount > 0)
                        {
                            fen += emptyCount;
                            emptyCount = 0;
                        }
                        fen += text;
                    }
                }
                if (emptyCount > 0)
                {
                    fen += emptyCount;
                }
                if (i < 7)
                {
                    fen += "/";
                }
            }

            return fen;
        }

        private void ApplyMove(string move)
        {
            int sourceCol = move[0] - 'a';
            int sourceRow = 8 - (move[1] - '0');
            int targetCol = move[2] - 'a';
            int targetRow = 8 - (move[3] - '0');

            Label sourceLabel = labels[sourceRow, sourceCol];
            Label targetLabel = labels[targetRow, targetCol];

            targetLabel.Text = sourceLabel.Text;
            targetLabel.Image = sourceLabel.Image;
            sourceLabel.Text = "";
            sourceLabel.Image = null;
        }

        private Label GetLabelAtPosition(Point position)
        {
            foreach (Label label in labels)
            {
                if (label.Bounds.Contains(position))
                {
                    return label;
                }
            }
            return null;
        }

        private string GenerateMoveFromLabels(Label source, Label target)
        {
            int sourceRow = -1, sourceCol = -1, targetRow = -1, targetCol = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (labels[i, j] == source)
                    {
                        sourceRow = i;
                        sourceCol = j;
                    }
                    if (labels[i, j] == target)
                    {
                        targetRow = i;
                        targetCol = j;
                    }
                }
            }

            if (sourceRow == -1 || sourceCol == -1 || targetRow == -1 || targetCol == -1)
            {
                throw new InvalidOperationException("Source or target label not found on the board.");
            }

            char sourceFile = (char)('a' + sourceCol);
            char sourceRank = (char)('8' - sourceRow);
            char targetFile = (char)('a' + targetCol);
            char targetRank = (char)('8' - targetRow);

            if (!isWhiteBottom)
            {
                sourceFile = (char)('h' - sourceCol);
                sourceRank = (char)('1' + sourceRow);
                targetFile = (char)('h' - targetCol);
                targetRank = (char)('1' + targetRow);
            }

            return $"{sourceFile}{sourceRank}{targetFile}{targetRank}";
        }

        private void FlipBoard()
        {
            isWhiteBottom = !isWhiteBottom;
            //InitializeBoardLabels();
            InitializeBoard();
        }




        public class TransparentControl : Control
        {
            public Image PieceImage { get; set; }

            public TransparentControl()
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.Transparent;
                this.DoubleBuffered = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                //Pen pen = new Pen(Color.Blue, 5);
                //e.Graphics.DrawLine(pen, new Point(0, 0), new Point(50, 50));


                if (PieceImage != null)
                {
                    //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //e.Graphics.Clear(Color.Transparent);
                    e.Graphics.DrawImage(PieceImage, 0, 0, this.Width, this.Height);
                }
            }
        }

        private bool showFailedMove = false;
        private bool trainNextMove = false;
        private bool trainNextMoveDoPrev = false;
        private bool showMultiUserMove = false;
        private void timerCheckDone_Tick(object sender, EventArgs e)
        {
            if (autoPlayNext)
            {
                autoPlayNext = false;
                //Auto play next move
                NextMove();
                InitializeBoard();

                if (node.NextMoves == null || node.NextMoves.Count == 0)
                {
                    puzzleDone();
                }
            }

            if (trainNextMove)
            {
                trainNextMove = false;
                //Auto play next move, but then pull it back
                NextMove();
                InitializeBoard();

                trainNextMoveDoPrev = true;
            }


            if (showFailedMove)
            {
                Size size = getSize("FailedMove", 1);
                showFailedMove = false;
                StatusLabel.Image = ResizeImage(pieceImages["FailedMove"], size);
                timerStatus.Enabled = true;
                StatusLabel.Visible = true;
                StatusLabel.Width = size.Width;
                StatusLabel.Height = size.Height;
                Point p = new Point(Cursor.Position.X - size.Width / 2, Cursor.Position.Y - size.Height / 2);
                StatusLabel.Location = chessBoard.PointToClient(p);// Cursor.Position);
                StatusLabel.BackColor = Color.Transparent;
                StatusLabel.FlatStyle = FlatStyle.Flat;
                StatusLabel.Parent = pictureBoxTop;
                showPictureBox();
                StatusLabel.BringToFront();
                StatusLabel.Invalidate(); // Force redraw

                showFailedMoveClear = true;
            }

            if (showVariationComplete)
            {
                Size size = getSize("VariationComplete", 0.5);
                showVariationComplete = false;
                showVariationCompleteClear = true;
                StatusLabel.Image = ResizeImage(pieceImages["VariationComplete"], size);
                StatusLabel.Width = size.Width;
                StatusLabel.Height = size.Height;
                timerStatus.Enabled = true;
                StatusLabel.Visible = true;
                StatusLabel.Parent = pictureBoxTop;
                CaptureClientAreaIntoPictureBox();
                showPictureBox();
                StatusLabel.BringToFront();
                StatusLabel.Location = new Point(chessBoard.Width / 2 - StatusLabel.Image.Width / 2, chessBoard.Height / 2 - StatusLabel.Image.Height / 2);// chessBoard.PointToClient(Cursor.Position);
                StatusLabel.Invalidate(); // Force redraw

            }

            if (showMultiUserMove)
            {
                Size size = getSize("MultiUserMove", 0.5);
                showMultiUserMove = false;
                showMultiUserMoveClear = true;
                StatusLabel.Image = ResizeImage(pieceImages["MultiUserMove"], size);
                StatusLabel.Width = size.Width;
                StatusLabel.Height = size.Height;
                timerStatus.Enabled = true;
                StatusLabel.Visible = true;
                StatusLabel.Parent = pictureBoxTop;
                CaptureClientAreaIntoPictureBox();
                showPictureBox();
                StatusLabel.BringToFront();
                StatusLabel.Location = new Point(chessBoard.Width / 2 - StatusLabel.Image.Width / 2, chessBoard.Height / 2 - StatusLabel.Image.Height / 2);// chessBoard.PointToClient(Cursor.Position);
                StatusLabel.Invalidate(); // Force redraw

            }

            if (showPuzzleComplete)
            {
                Size size = getSize("PuzzleComplete", 0.5);
                showPuzzleComplete = false;
                showPuzzleCompleteClear = true;
                StatusLabel.Image = ResizeImage(pieceImages["PuzzleComplete"], size);
                StatusLabel.Width = size.Width;
                StatusLabel.Height = size.Height;
                timerStatus.Enabled = true;
                StatusLabel.Visible = true;
                StatusLabel.Parent= pictureBoxTop;
                CaptureClientAreaIntoPictureBox();
                showPictureBox();
                StatusLabel.BringToFront();
                StatusLabel.Location = new Point(chessBoard.Width / 2 - StatusLabel.Image.Width/2, chessBoard.Height / 2 - StatusLabel.Image.Height/2); //chessBoard.PointToClient(Cursor.Position);
                StatusLabel.Invalidate(); // Force redraw
            }        
        }

        private Size getSize(string image, double multiplier)
        {
            return new Size((int)(pieceImages[image].Width * multiplier), (int)(pieceImages[image].Height * multiplier));
        }

        bool showFailedMoveClear = false;
        bool showVariationCompleteClear = false;
        bool showPuzzleCompleteClear = false;
        bool showMultiUserMoveClear = false;
        private void timerStatus_Tick(object sender, EventArgs e)
        {
            timerStatus.Enabled = false;

            if (showFailedMoveClear)
            {
                StatusLabel.Visible = false;
                hidePictureBox();
                showFailedMove = false;
            }

            if (showVariationCompleteClear)
            {
                StatusLabel.Visible = false;
                hidePictureBox();
                showVariationCompleteClear = false;
                //ResetToRoot();
            }

            if (showMultiUserMoveClear)
            {
                StatusLabel.Visible = false;
                hidePictureBox();
                showMultiUserMoveClear = false;
            }

            if (showPuzzleCompleteClear)
            {
                StatusLabel.Visible = false;
                hidePictureBox();
                if (trainingMode == ChessPuzzleTrainingMode.Test)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            
            }
            
        }

        private bool lockoutMoveDuringTrainingShow = false;
        private int waitBeforeShowingAgain = 0;
        private void timerLearnMode_Tick(object sender, EventArgs e)
        {
            if (playOppositeColor && root == node)
            {
                NextMove();
                InitializeBoard();
                return;
            }

            if ((board.isWhiteTurn && trainingMode == ChessPuzzleTrainingMode.LearnWhite )
                || (!board.isWhiteTurn && trainingMode == ChessPuzzleTrainingMode.LearnBlack))
            {
                if (waitBeforeShowingAgain++ % 5 != 0)
                {
                    return;
                }
                lockoutMoveDuringTrainingShow = true;
                NextMove(true);
                InitializeBoard();
                trainNextMoveDoPrev = true;                
                return;
            } else if (trainingMode == ChessPuzzleTrainingMode.LearnWhite || trainingMode == ChessPuzzleTrainingMode.LearnBlack)
            {
                if (node == root)
                {
                    NextMove();
                }
            }

            if (trainNextMoveDoPrev)
            {
                trainNextMoveDoPrev = false;
                PrevMode();
                InitializeBoard();
                lockoutMoveDuringTrainingShow = false;
                return;
            }

        }

        private void ChessPuzzleSimulatorForm_Move(object sender, EventArgs e)
        {
            SaveFormPosition();
        }
    }
}
