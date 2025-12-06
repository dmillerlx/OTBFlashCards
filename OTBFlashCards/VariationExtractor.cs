using System;
using System.Collections.Generic;
using System.Text;

namespace OTBFlashCards
{
    /// <summary>
    /// Represents a single variation line extracted from a PGN game
    /// </summary>
    public class VariationLine
    {
        public List<string> Moves { get; set; } = new List<string>();
        public List<string> Comments { get; set; } = new List<string>();
        public int MoveCount => Moves.Count;
        
        /// <summary>
        /// Returns first few moves for display (e.g., "1.e4 e5 2.Nf3 Nc6...")
        /// </summary>
        public string GetPreview(int maxMoves = 8)
        {
            var sb = new StringBuilder();
            int count = Math.Min(maxMoves, Moves.Count);
            
            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                {
                    int moveNo = (i / 2) + 1;
                    sb.Append($"{moveNo}.{Moves[i]} ");
                }
                else
                {
                    sb.Append($"{Moves[i]} ");
                }
            }
            
            if (Moves.Count > maxMoves)
                sb.Append("...");
                
            return sb.ToString().Trim();
        }
        
        /// <summary>
        /// Returns the full line with move numbers
        /// </summary>
        public string GetFullLine()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Moves.Count; i++)
            {
                if (i % 2 == 0)
                {
                    int moveNo = (i / 2) + 1;
                    sb.Append($"{moveNo}.{Moves[i]} ");
                }
                else
                {
                    sb.Append($"{Moves[i]} ");
                }
            }
            return sb.ToString().Trim();
        }
    }
    
    /// <summary>
    /// Helper class to extract all variation lines from a PGN game
    /// </summary>
    public static class VariationExtractor
    {
        /// <summary>
        /// Translates PGN NAG (Numeric Annotation Glyph) codes to symbols
        /// </summary>
        private static string TranslateNAG(string nag)
        {
            switch (nag)
            {
                case "$1": return "!";
                case "$2": return "?";
                case "$3": return "!!";
                case "$4": return "??";
                case "$5": return "!?";
                case "$6": return "?!";
                case "$7": return "☐"; // forced move
                case "$10": return "=";
                case "$11": return "="; // equal chances
                case "$13": return "∞"; // unclear position
                case "$14": return "⩲"; // White has slight advantage
                case "$15": return "⩱"; // Black has slight advantage
                case "$16": return "±"; // White has moderate advantage
                case "$17": return "∓"; // Black has moderate advantage
                case "$18": return "+-"; // White has decisive advantage
                case "$19": return "-+"; // Black has decisive advantage
                case "$22": return "⨀"; // zugzwang
                case "$32": return "⟳"; // development advantage
                case "$36": return "↑"; // initiative
                case "$40": return "→"; // attack
                case "$44": return "⇆"; // compensation
                case "$132": return "⟲"; // counterplay
                case "$138": return "⊕"; // time trouble
                case "$140": return "∆"; // with the idea
                case "$146": return "N"; // novelty
                default: return nag; // return as-is if unknown
            }
        }
        
        /// <summary>
        /// Translates multiple NAGs to a single string
        /// </summary>
        private static string TranslateNAGs(List<string> nags)
        {
            if (nags == null || nags.Count == 0)
                return "";
            
            var sb = new StringBuilder();
            foreach (var nag in nags)
            {
                sb.Append(TranslateNAG(nag));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Extracts all variation lines from a PGN game's move tree
        /// </summary>
        public static List<VariationLine> ExtractVariations(PgnGame game)
        {
            var allLines = new List<VariationLine>();
            
            // Start from each first move in the root
            foreach (var firstMove in game.MoveTreeRoot.NextMoves)
            {
                CollectLines(firstMove, new List<string>(), new List<string>(), allLines);
            }
            
            return allLines;
        }
        
        /// <summary>
        /// Recursively walks the move tree and collects complete lines
        /// </summary>
        private static void CollectLines(MoveNode node, List<string> current, List<string> currentComments, List<VariationLine> output)
        {
            // Add move with NAGs translated
            string moveWithNags = node.San + TranslateNAGs(node.Nags);
            current.Add(moveWithNags);
            currentComments.Add(node.Comment ?? "");
            
            if (node.NextMoves.Count == 0)
            {
                // Leaf node - this is a complete line
                var variation = new VariationLine();
                variation.Moves.AddRange(current);
                variation.Comments.AddRange(currentComments);
                output.Add(variation);
            }
            else
            {
                // Continue down each branch
                foreach (var child in node.NextMoves)
                {
                    CollectLines(child, current, currentComments, output);
                }
            }
            
            current.RemoveAt(current.Count - 1);
            currentComments.RemoveAt(currentComments.Count - 1);
        }
    }
}
