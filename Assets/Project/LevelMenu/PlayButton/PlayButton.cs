using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CanvasGroup))]
public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private AudioClip _clickSound;
    [SerializeField]
    private float _transitionDelay = 0.8f;
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
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        // Disable button interaction to prevent double-clicks
        _buttonRef.interactable = false;

        // Trigger character fade out if the animation handler is present
        if (LevelSelectionAnimationHandler.HasInstance)
        {
            LevelSelectionAnimationHandler.Instance.StartCoroutine(
                LevelSelectionAnimationHandler.Instance.FadeOutCharacterRoutine(_transitionDelay)
            );
        }

        if (_clickSound != null)
        {
            Debug.Log($"[PlayButton] Clicked! Spawning TempPlayButtonAudio. Playing sound: {_clickSound.name} (length: {_clickSound.length}s)");

            GameObject soundPlayer = new GameObject("TempPlayButtonAudio");
            DontDestroyOnLoad(soundPlayer);

            AudioSource audioSource = soundPlayer.AddComponent<AudioSource>();
            audioSource.clip = _clickSound;
            audioSource.volume = 1.0f;
            audioSource.pitch = 1.0f;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f; // 2D Sound

            audioSource.Play();

            // Destroy the player after the clip finishes playing
            Destroy(soundPlayer, _clickSound.length + 0.1f);
        }
        else
        {
            Debug.LogWarning("[PlayButton] Clicked, but no click sound is assigned!");
        }

        // Wait for the full transition delay so the fade out completes before the scene loads
        yield return new WaitForSeconds(_transitionDelay);

        SceneManager.LoadScene(_sceneName);
    }
    
}
