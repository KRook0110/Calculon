using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;
    private Button _buttonRef;

    void OnEnable()
    {
        if(!_buttonRef)
        {
            _buttonRef = GetComponent<Button>();
        }
        _buttonRef.onClick.AddListener(HandleButtonClick);
    }

    void OnDisable()
    {
        _buttonRef.onClick.RemoveListener(HandleButtonClick);
    }

    void HandleButtonClick()
    {
        SceneManager.LoadScene(_sceneName);
    }
    
}
