using UnityEngine;

public abstract class QuestionStage : MonoBehaviour
{
    public new string name;
    public int currentElo;
    public abstract MultipleChoiceQuestion GenerateQuestion();
}