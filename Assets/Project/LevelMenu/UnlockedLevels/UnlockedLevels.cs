using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedLevels : Singleton<UnlockedLevels>
{

    [SerializeField]
    private LevelData[] _initialUnlockedLevels;
    private SortedSet<string> _unlockedLevels = new SortedSet<string>();

    public Action<string> OnLevelUnlocked;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        LoadData();
        DebugUnlockedLevels();

        UnlockInitialLevels();
    }
    private void DebugUnlockedLevels()
    {
        foreach(var levelName in _unlockedLevels)
        {
            Debug.Log($"{levelName} unlocked");
        }

    }

    private void UnlockInitialLevels()
    {
        foreach(var level in _initialUnlockedLevels)
        {
            UnlockLevel(level.levelName);
        }
    }

    private void LoadData()
    {
        GameSaveData data = SaveSystem.Load();
        _unlockedLevels = new SortedSet<string>(data.unlockedLevels);
    }

    private void SaveData()
    {
        GameSaveData data = new GameSaveData
        {
            unlockedLevels = new List<string>(_unlockedLevels)
        };
        SaveSystem.Save(data);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // TODO: Add check is levelName exists
    public void UnlockLevel(string levelName)
    {
        if (_unlockedLevels.Add(levelName))
        {
            OnLevelUnlocked?.Invoke(levelName);
            SaveData();
        }
    }

    // returns : if levelName is unlocked returns true, else false
    public bool Unlocked(string levelName)
    {
        return _unlockedLevels.Contains(levelName);
    }

}