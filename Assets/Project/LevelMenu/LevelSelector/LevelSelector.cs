using System;
using UnityEngine;

public class LevelSelector : Singleton<LevelSelector>
{
    [Header("Default Level")]
    [SerializeField, Tooltip("Setting this value on runtime will be buggy")]
    private LevelData _selectedLevel;
    [SerializeField]
    private LevelData _startingSelectedLevel;

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
        _selectedLevel = _startingSelectedLevel;
    }

    public void InitializeLevel()
    {
    }
}
