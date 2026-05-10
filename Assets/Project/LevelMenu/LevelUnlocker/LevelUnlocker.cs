using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    void OnEnable()
    {
        GameOverHandle.Instance.OnWin += UnlockNextLevels;
    }

    void OnDisable()
    {
        if (GameOverHandle.HasInstance)
        {
            GameOverHandle.Instance.OnWin -= UnlockNextLevels;
        }
    }

    void UnlockNextLevels()
    {
        LevelData level = LevelSelector.Instance.selectedLevel;
        foreach (var nextLevel in level.nextLevels)
        {
            Debug.Log($"Trying to unlock {nextLevel.name}");
            LevelState.Instance.UnlockLevel(nextLevel.name);
        }
    }
}