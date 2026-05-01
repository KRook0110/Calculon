using UnityEngine;

public abstract class QuestionStage : MonoBehaviour
{
    public int currentElo;
    public abstract MultipleChoiceQuestion GenerateQuestion();
}