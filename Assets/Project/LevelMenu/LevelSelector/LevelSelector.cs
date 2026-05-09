using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : Singleton<LevelSelector>
{
    [Header("Default Level")]
    [SerializeField, Tooltip("Setting this value on runtime will be buggy")]
    private LevelData _selectedLevel;

    public Action<LevelData> OnSelectLevel;

    public LevelData selectedLevel
    {
        get => _selectedLevel;
        set
        {
            if(!value)
            {
                Debug.LogError("Tried to set level to null");
                return;
            }
            _selectedLevel = value;
            OnSelectLevel?.Invoke(value);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeLevel()
    {
        FightCoordinator.Instance.InitializeFightCoordinator(new FightCoordinator.FightData {
            enemies = selectedLevel.enemies
        });
    }
}
