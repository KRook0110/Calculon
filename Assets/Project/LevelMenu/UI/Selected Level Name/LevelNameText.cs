using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelNameText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateLevelText(LevelSelector.Instance.selectedLevel);
    }

    void UpdateLevelText(LevelData data)
    {
        _text.text = data?.levelName ?? "None Selected";
    }

    void OnEnable()
    {
        LevelSelector.Instance.OnSelectLevel += UpdateLevelText;
    }


    void OnDisable()
    {
        LevelSelector.Instance.OnSelectLevel -= UpdateLevelText;
    }
}
