using UnityEngine;

public class BattleReactHandler : Singleton<BattleReactHandler>
{
    [SerializeField]
    private PlayerEntity _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }
        ReactUnityCommunication.Instance.SendLevel(LevelSelector.Instance.selectedLevel.levelName);
    }

    void OnEnable()
    {
        FightCoordinator.Instance.OnFinish += HandleFinishAlive;
        _player.OnDie += HandleFinishDead;
        MultipleChoicesHandler.Instance.OnAnswer += HandleAnswers;
    }

    void OnDisable()
    {

        if(FightCoordinator.HasInstance) FightCoordinator.Instance.OnFinish -= HandleFinishAlive;
        if(_player != null) _player.OnDie -= HandleFinishDead;
        if(MultipleChoicesHandler.HasInstance) MultipleChoicesHandler.Instance.OnAnswer -= HandleAnswers;
    }

    void HandleAnswers(bool isCorrect, QuestionStage _)
    {
        ReactUnityCommunication.Instance.SendAnswer(isCorrect);
    }

    void HandleFinishAlive()
    {
        ReactUnityCommunication.Instance.SendFinished(true);
    }

    void  HandleFinishDead()
    {
        ReactUnityCommunication.Instance.SendFinished(false);

    }
}
