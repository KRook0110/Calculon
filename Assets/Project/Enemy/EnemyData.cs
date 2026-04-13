using UnityEngine;
using System.Collections.Generic;

public enum Difficulty
{
    Bonus = 1,
    Easy = 2,
    Medium = 3,
    Hard = 4
}

public enum EnemyType
{
    Addition = 1
}

public class EnemyData : MonoBehaviour
{
    public Difficulty currentDifficulty;
    public int correctAnswer;
    public string questionString;
    public List<int> wrongAnswer;

    public void SetupProblem(EnemyType currentEnemyType, Difficulty difficulty)
    {
        // enemy generation here
        // Debug.Log("TRY");
    }

    public void TakeDamageIfCorrect(int playerAnswer)
    {
        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Correct! Enemy defeated.");
        }
        else
        {
            Debug.Log("Wrong answer!");
        }
    }
}