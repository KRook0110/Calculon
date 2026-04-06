using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class TestingQuestionsGenerator : MonoBehaviour
{

    [SerializeField]
    private MultipleChoicesHandler handler;
    [SerializeField]
    private MultipleChoiceQuestion[] questions;
    [SerializeField]
    private bool onStartRun = false;

    [Header("Settings")]
    [SerializeField]
    private float initialDelay = 1f;
    [SerializeField]
    private float gapDelay = 1f;

    IEnumerator GapDelayTest()
    {
        if (initialDelay > 0f) yield return new WaitForSeconds(initialDelay);
        foreach (var question in questions)
        {
            handler.AddQuestion(question);
            if (gapDelay > 0f) yield return new WaitForSeconds(gapDelay);
        }
    }

    void Start()
    {
        if (onStartRun)
        {
            StartCoroutine(GapDelayTest());
        }
    }

    [Button("SetExampleQuestions")]
    private void SetExampleQuestions()
    {
        // Initialize the main array for 10 questions
        questions = new MultipleChoiceQuestion[10];

        // --- ADDITION ---
        questions[0] = MultipleChoiceQuestion.CreateQuestion("What is 15 + 27?", new string[] { "32", "42", "45", "52" }, 1);
        questions[1] = MultipleChoiceQuestion.CreateQuestion("What is 8 + 14?", new string[] { "22", "20", "24", "18" }, 0);

        // --- SUBTRACTION ---
        questions[2] = MultipleChoiceQuestion.CreateQuestion("What is 50 - 18?", new string[] { "32", "42", "22", "38" }, 0);
        questions[3] = MultipleChoiceQuestion.CreateQuestion("What is 100 - 45?", new string[] { "65", "55", "45", "75" }, 1);

        // --- MULTIPLICATION ---
        questions[4] = MultipleChoiceQuestion.CreateQuestion("What is 6 x 7?", new string[] { "36", "49", "42", "48" }, 2);
        questions[5] = MultipleChoiceQuestion.CreateQuestion("What is 9 x 4?", new string[] { "32", "36", "45", "27" }, 1);
        questions[6] = MultipleChoiceQuestion.CreateQuestion("What is 12 x 5?", new string[] { "50", "60", "70", "125" }, 1);

        // --- AREA ---
        questions[7] = MultipleChoiceQuestion.CreateQuestion("Area of a 5x5 square?", new string[] { "10", "20", "25", "30" }, 2);
        questions[8] = MultipleChoiceQuestion.CreateQuestion("Area of a rectangle with length 10 and width 4?", new string[] { "14", "40", "28", "44" }, 1);
        questions[9] = MultipleChoiceQuestion.CreateQuestion("Area of a 3x8 rectangle?", new string[] { "24", "11", "18", "32" }, 0);
    }
}
