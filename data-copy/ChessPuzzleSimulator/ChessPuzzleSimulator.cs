using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessPuzzleSimulator
{
    public partial class ChessPuzzleSimulator : Form
    {
        public ChessPuzzleSimulator()
        {
            InitializeComponent();            
        }


        private Panel chessBoard;
        private Label[,] labels = new Label[8, 8];
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

        public ChessPuzzleSimulator(string fenInput, List<string> pgnInput, bool record)
        {
            fen = fenInput;
            pgn = pgnInput;
            LoadPieceImages("C:\\data\\ChessPuzzleSimulator\\images");
            InitializeChessBoard();
            InitializeFen(fen);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Resize += OnResize;
            this.record = record;
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

            return ResizeImage(transparentImage, new Size(DefaultImageSize, DefaultImageSize));
        }


        private void InitializeChessBoard()
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
                Parent = chessBoard // Ensure it's part of the board to maintain transparency
            };
            chessBoard.Controls.Add(floatingPiece);



            ResizeChessBoard();
        }

        private void InitializeBoardLabels()
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

        private void InitializeFen(string fen)
        {
            string[] parts = fen.Split(' ');
            if (parts.Length < 1) throw new ArgumentException("Invalid FEN string");

            string[] rows = parts[0].Split('/');
            if (rows.Length != 8) throw new ArgumentException("Invalid FEN board layout");

            for (int i = 0; i < 8; i++)
            {
                int col = 0;
                foreach (char c in rows[i])
                {
                    if (char.IsDigit(c))
                    {
                        col += c - '0';
                    }
                    else
                    {
                        string piece = c.ToString();
                        if (pieceImages.ContainsKey(piece))
                        {
                            labels[i, col].Image = pieceImages[piece];
                            //labels[i, col].Text = piece; // Store the piece key for resizing logic
                            labels[i, col].Tag = piece;
                        }
                        else
                        {
                            labels[i, col].Text = "";// piece;
                            labels[i, col].Font = new Font("Arial", 24, FontStyle.Bold);
                        }
                        col++;
                    }
                }
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

        }






        private void OnResize(object sender, EventArgs e)
        {
            ResizeChessBoard();
        }

        private Image ResizeImage(Image image, Size size)
        {
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
            selectedLabel = sender as Label;
            if (selectedLabel != null && selectedLabel.Image != null)
            {
                originalMousePosition = e.Location;
                floatingPiece.PieceImage = selectedLabel.Image;
                floatingPiece.Size = selectedLabel.Size;
                floatingPiece.BringToFront();
                floatingPiece.Visible = true;
                floatingPiece.Location = chessBoard.PointToClient(Cursor.Position);
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
                floatingPiece.Visible = false;

                Point mouseLocation = chessBoard.PointToClient(Cursor.Position);
                Label targetLabel = GetLabelAtPosition(mouseLocation);

                if (targetLabel != null && selectedLabel != targetLabel)
                {
                    string move = GenerateMoveFromLabels(selectedLabel, targetLabel);
                    HandleMove(move);
                }

                selectedLabel = null;
            }
        }

        private List<string> recordMoves = new List<string>();

        public List<string> GetRecordedMoves()
        {
            return recordMoves;
        }

        private void HandleMove(string move)
        {
            string currentFen = GenerateFenFromLabels();
            if (record)
            {
                recordMoves.Add(move);
                ApplyMove(move);
            }
            else
            {

                string expectedMove = pgn[moveIndex];

                if (move == expectedMove)
                {
                    ApplyMove(move);
                    moveIndex++;

                    if (moveIndex < pgn.Count)
                    {
                        ApplyMove(pgn[moveIndex]);
                        moveIndex++;
                    }

                    if (moveIndex >= pgn.Count)
                    {
                        MessageBox.Show("Congratulations! Puzzle solved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect move! Puzzle failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

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

            return $"{sourceFile}{sourceRank}{targetFile}{targetRank}";
        }

        private void FlipBoard()
        {
            isWhiteBottom = !isWhiteBottom;
            InitializeBoardLabels();
        }


       

        public class TransparentControl : Control
        {
            public Image PieceImage { get; set; }

            public TransparentControl()
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.Transparent;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (PieceImage != null)
                {
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.Clear(Color.Transparent);
                    e.Graphics.DrawImage(PieceImage, 0, 0, this.Width, this.Height);
                }
            }
        }





    }
}
