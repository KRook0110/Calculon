using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    void OnEnable()
    {
        GameOverHandle.Instance.OnWin += UnlockNextLevels;
    }

    void OnDisable()
    {
        GameOverHandle.Instance.OnWin -= UnlockNextLevels;
    }

    void UnlockNextLevels()
    {
        LevelData level = LevelSelector.Instance.selectedLevel;
        foreach (var nextLevel in level.nextLevels)
        {
            Debug.Log($"Trying to unlock {nextLevel.name}");
            UnlockedLevels.Instance.UnlockLevel(nextLevel.name);
        }
    }
}