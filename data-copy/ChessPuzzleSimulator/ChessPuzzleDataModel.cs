using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessPuzzleSimulator
{
    public enum ChessPuzzleTrainingMode
    {
        Explore = 0,
        LearnWhite = 1,
        LearnBlack = 4,
        Study = 2,
        Test = 3,
    }

    public class ChessPuzzleTestResults
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PGN { get; set; }
        public int Attempts { get; set; }
        public int Failures { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Variations { get; set; }
    }

    public class ChessPuzzleConfig
    {
        public string Name { get; set; }
        public ChessPuzzleTrainingMode chessPuzzleTrainingMode { get; set; }
        public string Log { get; set; }
        public List<ChessPuzzleTestResults> PuzzleTestResults { get; set; }
        public List<ChessPGNData> PGNData { get; set; }

    }

    public class ChessPGNData
    {
        public string Filename { get; set; }

        public override string ToString()
        {
            int index = Filename.LastIndexOf('\\');
            string val = index < 0 ? Filename : Filename.Substring(index+1);
            return val;
        }
    }



    public class ChessPuzzleData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ChessPuzzleGroup> PuzzleGroups{ get; set; }
    }

    public class ChessPuzzleGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ChessPuzzleItem> Puzzles { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ChessPuzzleItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FEN { get; set; }
        public string Moves { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
