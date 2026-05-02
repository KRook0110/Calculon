using UnityEngine;

public class LevelSelector : Singleton<LevelSelector>
{
    [Header("Level")]
    public LevelData selectedLevel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeLevel()
    {
        FightCoordinator.Instance.InitializeFightCoordinator(new FightCoordinator.FightData {
            enemies = selectedLevel.enemies
        });
    }
}
