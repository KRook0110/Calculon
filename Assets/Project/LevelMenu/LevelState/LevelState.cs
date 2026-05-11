using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : Singleton<LevelState>
{
    public enum State
    {
        Unlocked, Locked, Completed
    }
    [SerializeField]
    private LevelData[] _initialUnlockedLevels;
    private SortedSet<string> _unlockedLevels = new SortedSet<string>();
    private SortedSet<string> _completedLevels = new SortedSet<string>();

    public Action<string> OnLevelUnlocked;
    public Action<string> OnLevelCompleted;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        LoadData();
        DebugUnlockedLevels();
        DebugCompletedLevels();

        UnlockInitialLevels();
    }

    private void DebugUnlockedLevels()
    {
        foreach(var levelName in _unlockedLevels)
        {
            Debug.Log($"{levelName} unlocked");
        }

    }

    private void DebugCompletedLevels()
    {
        foreach(var levelName in _completedLevels)
        {
            Debug.Log($"{levelName} completed");
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
        _completedLevels = new SortedSet<string>(data.completedLevels);
    }

    private void SaveData()
    {
        GameSaveData data = SaveSystem.Load();
        data.unlockedLevels = new List<string>(_unlockedLevels);
        data.completedLevels = new List<string>(_completedLevels);
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

    public State GetLevelState(string levelName)
    {
        if (_completedLevels.Contains(levelName))
        {
            return State.Completed;
        }

        if (_unlockedLevels.Contains(levelName))
        {
            return State.Unlocked;
        }

        return State.Locked;
    }

    // returns : if levelName is unlocked returns true, else false
    public bool IsUnlocked(string levelName)
    {
        State state = GetLevelState(levelName);
        return state == State.Unlocked || state == State.Completed;
    }

    public void CompleteLevel(string levelName)
    {
        if (_completedLevels.Add(levelName))
        {
            OnLevelCompleted?.Invoke(levelName);
            SaveData();
        }
    }

    public bool IsCompleted(string levelName)
    {
        return GetLevelState(levelName) == State.Completed;
    }

}