using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public List<string> unlockedLevels = new List<string>();
    public List<string> completedLevels = new List<string>();
    public int currentElo = 50;
}

public static class SaveSystem
{
    public static string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    public static void Save(GameSaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }

    public static GameSaveData Load()
    {
        if (!File.Exists(SavePath))
        {
            return new GameSaveData();
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
            return data ?? new GameSaveData();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game data: {e.Message}");
            return new GameSaveData();
        }
    }
}
