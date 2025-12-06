using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OTBFlashCards
{
    /// <summary>
    /// Configuration and data storage for OTB Flash Cards
    /// </summary>
    public class StudyData
    {
        [JsonPropertyName("configVersion")]
        public int ConfigVersion { get; set; } = 1;

        [JsonPropertyName("pgnFiles")]
        public List<PgnFileData> PgnFiles { get; set; } = new List<PgnFileData>();

        [JsonPropertyName("variations")]
        public Dictionary<string, VariationData> Variations { get; set; } = new Dictionary<string, VariationData>();
    }

    public class PgnFileData
    {
        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("dateAdded")]
        public DateTime DateAdded { get; set; }

        [JsonPropertyName("maxDepth")]
        public int? MaxDepth { get; set; } = null; // Depth limit for this entire tree

        [JsonPropertyName("ignoreDepthForMainline")]
        public bool IgnoreDepthForMainline { get; set; } = false; // Ignore depth for mainline in this tree
    }

    public class VariationData
    {
        [JsonPropertyName("lineHash")]
        public string LineHash { get; set; }

        [JsonPropertyName("fullLine")]
        public string FullLine { get; set; }

        [JsonPropertyName("moveCount")]
        public int MoveCount { get; set; }

        [JsonPropertyName("sourceFile")]
        public string SourceFile { get; set; } = ""; // Which PGN file this came from

        [JsonPropertyName("lineNotes")]
        public string LineNotes { get; set; } = "";

        [JsonPropertyName("moveNotes")]
        public Dictionary<int, string> MoveNotes { get; set; } = new Dictionary<int, string>(); // moveIndex -> note

        [JsonPropertyName("attempts")]
        public List<AttemptData> Attempts { get; set; } = new List<AttemptData>();

        [JsonPropertyName("metrics")]
        public MetricsData Metrics { get; set; } = new MetricsData();
    }

    public class AttemptData
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("depthReached")]
        public int DepthReached { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = "";
    }

    public class MetricsData
    {
        [JsonPropertyName("totalAttempts")]
        public int TotalAttempts { get; set; } = 0;

        [JsonPropertyName("successfulAttempts")]
        public int SuccessfulAttempts { get; set; } = 0;

        [JsonPropertyName("failedAttempts")]
        public int FailedAttempts { get; set; } = 0;

        [JsonPropertyName("lastAttemptDate")]
        public DateTime? LastAttemptDate { get; set; } = null;

        [JsonPropertyName("successRate")]
        public double SuccessRate { get; set; } = 0.0;

        [JsonPropertyName("currentStreak")]
        public int CurrentStreak { get; set; } = 0; // consecutive successes
    }

    /// <summary>
    /// Manager for loading/saving study data
    /// </summary>
    public static class StudyDataManager
    {
        private static StudyData currentData = null;
        private static string dataFilePath = null;

        public static void Initialize()
        {
            // Get data file path from registry or prompt user
            dataFilePath = RegistryUtils.GetString("StudyDataPath", "");
            
            if (string.IsNullOrEmpty(dataFilePath) || !File.Exists(dataFilePath))
            {
                // Will be set via settings dialog
                currentData = new StudyData();
            }
            else
            {
                Load();
            }
        }

        public static void SetDataFilePath(string path)
        {
            dataFilePath = path;
            RegistryUtils.SetString("StudyDataPath", path);
        }

        public static string GetDataFilePath()
        {
            return dataFilePath;
        }

        public static StudyData GetData()
        {
            if (currentData == null)
            {
                currentData = new StudyData();
            }
            return currentData;
        }

        public static void Load()
        {
            if (string.IsNullOrEmpty(dataFilePath) || !File.Exists(dataFilePath))
            {
                currentData = new StudyData();
                return;
            }

            try
            {
                string json = File.ReadAllText(dataFilePath);
                currentData = JsonSerializer.Deserialize<StudyData>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading study data: {ex.Message}");
                currentData = new StudyData();
            }
        }

        public static void Save()
        {
            if (string.IsNullOrEmpty(dataFilePath))
            {
                return; // No path set yet
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(currentData, options);
                File.WriteAllText(dataFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving study data: {ex.Message}");
            }
        }

        public static string GenerateLineHash(List<string> moves)
        {
            string fullLine = string.Join(" ", moves);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fullLine));
                return Convert.ToBase64String(bytes).Substring(0, 32); // Shorter hash
            }
        }

        public static VariationData GetOrCreateVariation(VariationLine variation, string sourceFile = null)
        {
            string hash = GenerateLineHash(variation.Moves);
            
            if (!currentData.Variations.ContainsKey(hash))
            {
                currentData.Variations[hash] = new VariationData
                {
                    LineHash = hash,
                    FullLine = variation.GetFullLine(),
                    MoveCount = variation.MoveCount,
                    SourceFile = sourceFile ?? "",
                    Metrics = new MetricsData()
                };
            }
            else if (!string.IsNullOrEmpty(sourceFile) && string.IsNullOrEmpty(currentData.Variations[hash].SourceFile))
            {
                // Update source file if it wasn't set before
                currentData.Variations[hash].SourceFile = sourceFile;
            }

            return currentData.Variations[hash];
        }

        public static void RecordAttempt(VariationData varData, bool success, int depthReached, string notes = "")
        {
            var attempt = new AttemptData
            {
                Date = DateTime.Now,
                Success = success,
                DepthReached = depthReached,
                Notes = notes
            };

            varData.Attempts.Add(attempt);
            UpdateMetrics(varData);
            Save();
        }

        private static void UpdateMetrics(VariationData varData)
        {
            varData.Metrics.TotalAttempts = varData.Attempts.Count;
            varData.Metrics.SuccessfulAttempts = varData.Attempts.Count(a => a.Success);
            varData.Metrics.FailedAttempts = varData.Attempts.Count(a => !a.Success);
            varData.Metrics.LastAttemptDate = varData.Attempts.LastOrDefault()?.Date;

            if (varData.Metrics.TotalAttempts > 0)
            {
                varData.Metrics.SuccessRate = (double)varData.Metrics.SuccessfulAttempts / varData.Metrics.TotalAttempts * 100.0;
            }

            // Calculate current streak
            int streak = 0;
            for (int i = varData.Attempts.Count - 1; i >= 0; i--)
            {
                if (varData.Attempts[i].Success)
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }
            varData.Metrics.CurrentStreak = streak;
        }

        public static List<VariationData> GetFailedVariations()
        {
            var failed = new List<VariationData>();
            foreach (var varData in currentData.Variations.Values)
            {
                if (varData.Attempts.Count > 0 && !varData.Attempts.Last().Success)
                {
                    failed.Add(varData);
                }
            }
            return failed;
        }

        public static List<VariationData> GetLowSuccessRateVariations(double threshold = 70.0)
        {
            var lowSuccess = new List<VariationData>();
            foreach (var varData in currentData.Variations.Values)
            {
                if (varData.Metrics.TotalAttempts > 0 && varData.Metrics.SuccessRate < threshold)
                {
                    lowSuccess.Add(varData);
                }
            }
            return lowSuccess;
        }

        public static PgnFileData GetOrCreatePgnFile(string filePath)
        {
            var existing = currentData.PgnFiles.FirstOrDefault(f => f.FilePath == filePath);
            if (existing == null)
            {
                existing = new PgnFileData
                {
                    FilePath = filePath,
                    FileName = System.IO.Path.GetFileName(filePath),
                    DateAdded = DateTime.Now
                };
                currentData.PgnFiles.Add(existing);
                Save();
            }
            return existing;
        }

        public static void SetTreeDepthSettings(string filePath, int? maxDepth, bool ignoreMainline)
        {
            var pgnFile = GetOrCreatePgnFile(filePath);
            pgnFile.MaxDepth = maxDepth;
            pgnFile.IgnoreDepthForMainline = ignoreMainline;
            Save();
        }

        public static (int? maxDepth, bool ignoreMainline) GetTreeDepthSettings(string filePath)
        {
            var pgnFile = currentData.PgnFiles.FirstOrDefault(f => f.FilePath == filePath);
            if (pgnFile != null)
            {
                return (pgnFile.MaxDepth, pgnFile.IgnoreDepthForMainline);
            }
            return (null, false);
        }

        public static VariationLine ConvertToVariationLine(VariationData varData)
        {
            // Parse the full line back into moves
            var moves = new List<string>();
            var parts = varData.FullLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var part in parts)
            {
                // Skip move numbers (e.g., "1." "2.")
                if (!part.Contains("."))
                {
                    moves.Add(part);
                }
            }

            var varLine = new VariationLine();
            varLine.Moves = moves;
            // Comments are already stored in MoveNotes, but not in original format
            // Just leave comments empty for now
            varLine.Comments = new List<string>();
            return varLine;
        }
    }
}
