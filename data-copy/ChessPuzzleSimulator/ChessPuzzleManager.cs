using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ChessPuzzleSimulator
{
    public class ChessPuzzleManager
    {
        // Method to save data to disk
        public static void SaveChessPuzzleData(string filePath, ChessPuzzleData data)
        {
            try
            {
                // Serialize the ChessPuzzleData object to JSON using Newtonsoft.Json
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

                // Write the JSON string to a file
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Method to load data from disk
        public static ChessPuzzleData LoadChessPuzzleData(string filePath)
        {
            try
            {
                // Check if file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File not found.");
                    return null;
                }

                // Read the JSON string from the file
                string jsonData = File.ReadAllText(filePath);

                // Deserialize the JSON string back into a ChessPuzzleData object
                ChessPuzzleData data = JsonConvert.DeserializeObject<ChessPuzzleData>(jsonData);
                Console.WriteLine("Data loaded successfully.");
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return null;
            }
        }

    }


    public class ChessConfigManager
    {
        // Method to save data to disk
        public static void SaveChessConfigData(string filePath, ChessPuzzleConfig data)
        {
            try
            {
                // Serialize the ChessPuzzleData object to JSON using Newtonsoft.Json
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

                // Write the JSON string to a file
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Method to load data from disk
        public static ChessPuzzleConfig LoadChessConfigData(string filePath)
        {
            try
            {
                // Check if file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File not found.");
                    return null;
                }

                // Read the JSON string from the file
                string jsonData = File.ReadAllText(filePath);

                // Deserialize the JSON string back into a ChessPuzzleData object
                ChessPuzzleConfig data = JsonConvert.DeserializeObject<ChessPuzzleConfig>(jsonData);
                Console.WriteLine("Data loaded successfully.");
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return null;
            }
        }

    }
}
