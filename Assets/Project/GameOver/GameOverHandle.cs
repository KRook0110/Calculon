using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandle : Singleton<GameOverHandle>
{
    [SerializeField]
    private CanvasGroup _winUI;
    [SerializeField]
    private CanvasGroup _loseUI;
    [SerializeField]
    private PlayerEntity _playerEntity;
    [Header("Scene References")]
    [SerializeField]
    private string _battleSceneName = "BattleScene";
    [SerializeField]
    private string _levelSelectSceneName = "LevelSelect";

    private bool hasEnded = false;
    public Action OnWin;
    public Action OnLose;

    void OnEnable()
    {
        // Using .Instance instead of .HasInstance ensures we find the FightCoordinator 
        // even if its Awake() hasn't run yet, as it will perform a search in the scene.
        FightCoordinator.Instance.OnFinish += HandleWin;

        if (_playerEntity == null)
        {
            Debug.LogError("_playerEntity is null");
        }
        else
        {
            _playerEntity.OnDie += HandleLose;
        }
    }

    void OnDisable()
    {
        if (FightCoordinator.HasInstance) { FightCoordinator.Instance.OnFinish -= HandleWin; }
        _playerEntity.OnDie -= HandleLose;
    }

    void HandleWin()
    {
        if (hasEnded)
        {
            Debug.LogWarning("Game has already ended, can't show another UI");
            return;
        }
        hasEnded = true;

        if (_winUI == null)
        {
            Debug.LogError("winUI is null");
            return;
        }
        OnWin?.Invoke();
        _winUI.gameObject.SetActive(true);
        StartCoroutine(UIFade.Fade(_winUI, 0.8f, 0f, 1f));
    }

    void HandleLose()
    {
        if (hasEnded)
        {
            Debug.LogWarning("Game has already ended, can't show another UI");
            return;
        }
        hasEnded = true;
        if (_loseUI == null)
        {
            Debug.LogError("loseUI is null");
            return;
        }
        OnLose?.Invoke();
        _loseUI.gameObject.SetActive(true);
        StartCoroutine(UIFade.Fade(_loseUI, 0.8f, 0f, 1f));
    }

    public void RestartGame()
    {
        QuestionGenerator.Instance.ResetCustomQuestions();
        SceneManager.LoadScene(_battleSceneName);
    }
    public void GoToLevelManager()
    {
        QuestionGenerator.Instance.ClearCustomQuestions();
        SceneManager.LoadScene(_levelSelectSceneName);
    }
}
