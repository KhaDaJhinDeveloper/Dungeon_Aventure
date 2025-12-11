using UnityEngine;
using System.IO;
using System;
public static class JsonFileUtility
{
    private static string SaveFolder
    {
        get
        { 
            if(Application.isEditor)
            {
                return Path.Combine(Application.dataPath, "SaveData");
            }
            else
            {
                string exeDirettory = Path.GetDirectoryName(Application.dataPath);
                return Path.Combine(exeDirettory, "SaveData");
            }
        }
    }
    private static void EnsureSaveDirectoryExists()
    {
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
            DebugLogger.Log($"Created save directory: {SaveFolder}");
        }
    }
    public static bool SaveToJson<T>(T data, string filename, bool print = true) where T : class
    {
        try
        {
            EnsureSaveDirectoryExists();
            filename = CheckFileFormat(filename);
            string filePath = Path.Combine(SaveFolder, filename);
            string json = JsonUtility.ToJson(data, print);
            File.WriteAllText(filePath, json);
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
                writer.Flush();
                fs.Flush(true); // Force flush to disk
            }
            DebugLogger.Log($"Saved JSON to: {filePath}");
            return true;
        }
        catch (Exception e)
        {
            DebugLogger.LogError($"Failed to save JSON file {filename}: {e.Message}");
            return false;
        }
    }
    public static T LoadFromJson<T>(string filename) where T : class
    {
        try
        {
            filename = CheckFileFormat(filename);
            string filePath = Path.Combine (SaveFolder, filename);
            if (!File.Exists(filePath))
            {
                DebugLogger.LogWarning($"JSON file not found: {filePath}");
                return null;
            }
            string json = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(json);
            DebugLogger.Log($"Loaded JSON from: {filePath}");
            return data;
        }
        catch (Exception e)
        {
            DebugLogger.LogError($"Failed to load JSON file {filename}: {e.Message}");
            return null;
        }
    }
    public static bool JsonFileExists(string filename)
    {

        string filePath = Path.Combine(SaveFolder, filename);
        return File.Exists(filePath);
    }
    public static bool DeleteJsonFile(string filename) 
    {
        try
        {
            filename = CheckFileFormat(filename);
            string filePath = Path.Combine(SaveFolder, filename);
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                DebugLogger.Log($"Deleted JSON file: {filePath}");
                return true;
            }
            else
            {
                DebugLogger.LogWarning($"JSON file not found to delete: {filePath}");
                return false;
            }
        }
        catch (Exception e)
        {
            DebugLogger.LogError($"Failed to delete JSON file {filename}: {e.Message}");
            return false;
        }
    }
    public static void DeleteAllJsonFile()
    {
        try
        {
            if(Directory.Exists(SaveFolder))
            {
                string[] jsonFiles = Directory.GetFiles(SaveFolder, "*.json");
                foreach (string file in jsonFiles)
                {
                    File.Delete(file);
                }
                DebugLogger.Log($"Deleted {jsonFiles.Length} JSON files");
            }
        }
        catch (Exception e)
        {
            DebugLogger.LogError($"Failed to delete all JSON files: {e.Message}");
        }
    }
    public static string GetFilePath(string filename)
    {
        CheckFileFormat(filename);
        return Path.Combine(SaveFolder, filename);
    }
    public static string GetSaveFolder()
    {
        return SaveFolder;
    }
    public static string CheckFileFormat(string filename)
    {
        if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            filename += ".json";
        }
        return filename;
    }
}
