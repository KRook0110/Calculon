using UnityEngine;

public class LevelSectorReseter : MonoBehaviour
{
    public LevelData fallbackLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(LevelSelector.Instance.selectedLevel.resetLevelSelectionToDefault)
        {
            LevelSelector.Instance.selectedLevel = fallbackLevel;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
