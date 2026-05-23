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

    [Header("Colors")]
    [SerializeField]
    private Color _lockedColor;
    [SerializeField]
    private Color _unlockedColor;
    [SerializeField]
    private Color _completedColor;
    [Header("Level Platform")]
    [SerializeField] public Transform mainCharacterPivot;

    private LevelState.State _currentState = LevelState.State.Locked;

    private SpriteRenderer _spriteRenderer;
    private UnityEngine.UI.Image _uiImage;

    private void Awake()
    {
        if (!mainCharacterPivot) mainCharacterPivot = transform;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _uiImage = GetComponent<UnityEngine.UI.Image>();
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

        HandleColor();
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

    void HandleColor()
    {
        switch (_currentState)
        {
            case LevelState.State.Unlocked:
                SetColor(_unlockedColor);
                UnlockQuestionTypes();
                break;
            case LevelState.State.Completed:
                SetColor(_completedColor);
                UnlockQuestionTypes();
                break;
            case LevelState.State.Locked:
                SetColor(_lockedColor);
                break;
        }

    }

    void SetColor(Color color)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = color;
        }
        else if (_uiImage != null)
        {
            _uiImage.color = color;
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
