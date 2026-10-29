using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FlappyBird;

public class FileManager
{
    private const string FilePath = "highscore.txt";
    
    public void SaveBestScore(int score)
    {
        string jsonString = JsonSerializer.Serialize(score);
        try
        {
            File.WriteAllText(FilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error saving score: {ex}");
        }
        
    }

    public int LoadBestScore()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string jsonString = File.ReadAllText(FilePath);
                int score = JsonSerializer.Deserialize<int?>(jsonString) ?? 0;
                Console.WriteLine("loaded successfully!");
                return score;
            }
            catch(Exception)
            {
                Console.WriteLine("error! invalid file!");
            }   
        }
        return 0;
    }
}