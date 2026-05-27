using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* Handles the level platform, when clicked it will set the level selector to the preset level for the platform
*/
public class LevelPlatform : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private LevelData _level;

    public LevelData Level => _level;

    [Header("Level Platform")]
    [SerializeField] public Transform mainCharacterPivot;

    private LevelState.State _currentState = LevelState.State.Locked;

    private SpriteRenderer[] _spriteRenderers;
    private UnityEngine.UI.Image[] _uiImages;

    private void Awake()
    {
        if (!mainCharacterPivot) mainCharacterPivot = transform;

        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _uiImages = GetComponentsInChildren<UnityEngine.UI.Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentState != LevelState.State.Locked)
        {
            LevelSelector.Instance.selectedLevel = _level;
        }
    }

    void Start()
    {
        Checks();

        if (PlatformLookup.HasInstance)
        {
            PlatformLookup.Instance.RegisterPlatform(_level, this);
        }

        _currentState = LevelState.Instance.GetLevelState(_level.levelName);

        UpdateVisuals();
    }

    void Checks()
    {
        if (!LevelState.HasInstance)
        {
            Debug.LogError("UnlockedLevels has no instance in the scene.");
            return;
        }

        if (_level == null)
        {
            Debug.LogError($"LevelData is not assigned on {gameObject.name}");
            return;
        }

    }

    void UpdateVisuals()
    {
        bool isAccessible = _currentState != LevelState.State.Locked;

        if (_spriteRenderers != null)
        {
            foreach (var sr in _spriteRenderers) sr.enabled = isAccessible;
        }

        if (_uiImages != null)
        {
            foreach (var img in _uiImages) img.enabled = isAccessible;
        }

        if (isAccessible)
        {
            UnlockQuestionTypes();
        }
    }

    void UnlockQuestionTypes()
    {
        if (!QuestionGenerator.HasInstance)
        {
            Debug.LogError("Quetsion Generator Instance not found");
            return;
        }
        foreach (var questionTypes in _level.unlockQuestionTypes)
        {
            QuestionGenerator.Instance.EnableQuestionType(questionTypes);
        }
    }
}
