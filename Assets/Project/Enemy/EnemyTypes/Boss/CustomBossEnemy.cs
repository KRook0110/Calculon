
using UnityEngine;

public class CustomBossEnemy : BossEnemy {

    protected override void Start()
    {
        base.Start();

        SetHP(QuestionGenerator.Instance.customQuestions.Count);
    }

}