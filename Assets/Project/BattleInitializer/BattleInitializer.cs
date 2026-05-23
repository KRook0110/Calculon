using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

/**
 * @brief Initializes the battle scene by triggering the LevelSelector's initialization logic.
 */
public class BattleInitializer : Singleton<BattleInitializer>
{
    private bool _hasInitializedBattle = false;

    void OnEnable()
    {
        TutorialHandler.Instance.OnTutorialComplete += InitializeBattle;
    }
    void OnDisable()
    {
        if(TutorialHandler.HasInstance) TutorialHandler.Instance.OnTutorialComplete -= InitializeBattle;
    }

    void InitializeBattle()
    {
        if (_hasInitializedBattle) return;
        _hasInitializedBattle = true;

        LevelData selectedLevel = LevelSelector.Instance.selectedLevel;
        FightCoordinator.Instance.Initialize(new FightCoordinator.FightData
        {
            enemies = selectedLevel.enemies
        });
    }
}
