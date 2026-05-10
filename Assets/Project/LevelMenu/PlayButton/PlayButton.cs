using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CanvasGroup))]
public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    private Button _buttonRef;
    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _buttonRef = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        _buttonRef.onClick.AddListener(HandleButtonClick);

        if (LevelSelector.Instance != null)
        {
            LevelSelector.Instance.OnSelectLevel += UpdateVisuals;
            UpdateVisuals(LevelSelector.Instance.selectedLevel);
        }
    }

    void OnDisable()
    {
        _buttonRef.onClick.RemoveListener(HandleButtonClick);
        
        if (LevelSelector.HasInstance)
        {
            LevelSelector.Instance.OnSelectLevel -= UpdateVisuals;
        }
    }

    private void UpdateVisuals(LevelData level)
    {
        bool hasLevel = level != null;
        _canvasGroup.alpha = hasLevel ? 1.0f : 0.5f;
        _buttonRef.interactable = hasLevel;
    }

    void HandleButtonClick()
    {
        SceneManager.LoadScene(_sceneName);
    }
    
}
