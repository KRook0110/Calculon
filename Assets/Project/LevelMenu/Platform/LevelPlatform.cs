using UnityEngine;
using UnityEngine.EventSystems;

/*
* Handles the level platform, when clicked it will set the level selector to the preset level for the platform
*/
public class LevelPlatform : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private LevelData _level;
    bool _unlocked = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_unlocked)
        {
            LevelSelector.Instance.selectedLevel = _level;
        }
    }

    void Start()
    {
        if (!UnlockedLevels.HasInstance)
        {
            Debug.LogError("UnlockedLevels has no instance in the scene.");
            return;
        }

        if (_level == null)
        {
            Debug.LogError($"LevelData is not assigned on {gameObject.name}");
            return;
        }

        if (UnlockedLevels.Instance.Unlocked(_level.name))
        {
            _unlocked = true;
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
