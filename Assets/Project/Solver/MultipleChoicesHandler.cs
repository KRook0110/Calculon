using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ChoiceData
{
    public bool isCorrect;
    public string choiceText;
}

[Serializable]
public class MultipleChoiceQuestion
{
    public string questionText;
    public ChoiceData[] choices;

    public static MultipleChoiceQuestion CreateQuestion(string text, string[] options, int correctIndex)
    {
        MultipleChoiceQuestion q = new MultipleChoiceQuestion
        {
            questionText = text,
            choices = new ChoiceData[options.Length]
        };

        for (int i = 0; i < options.Length; i++)
        {
            q.choices[i] = new ChoiceData
            {
                choiceText = options[i],
                isCorrect = i == correctIndex
            };
        }

        return q;
    }
}

public class MultipleChoicesHandler : Singleton<MultipleChoicesHandler>
{
    [SerializeField]
    private TextMeshProUGUI questionTextObject;
    [SerializeField]
    private GameObject[] choiceObjects;

    private List<TextMeshProUGUI> _choiceTexts = new List<TextMeshProUGUI>();
    private List<Button> _choiceButtons = new List<Button>();
    private MultipleChoiceQuestion _currentQuestion;

    /// Invoked when a user submits an answer.
    /// The boolean parameter is true if the answer was correct; otherwise, false.
    public Action<bool> OnAnswer;

    // Invoked when there are no Questions left 
    public Action OnFinish;


    void Start()
    {

        for (int i = 0; i < choiceObjects.Length; i++)
        {
            Button foundButton = choiceObjects[i].GetComponentInChildren<Button>();
            TextMeshProUGUI foundText = choiceObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(foundText);
            Assert.IsNotNull(foundButton);

            if (!foundText && !foundButton)
            {
                Debug.LogError($"Choice object of {choiceObjects[i].name} has no Button Component or Text");
                continue;
            }

            _choiceTexts.Add(foundText);
            _choiceButtons.Add(foundButton);
        }

        NextQuestion();
        RefreshUI();
    }


    public void Answer(bool isCorrect)
    {
        if (isCorrect)
        {
            NextQuestion();
        }

        OnAnswer?.Invoke(isCorrect);
    }

    private void NextQuestion()
    {
        var info = QuestionGenerator.Instance.GenerateQuestion();

        if (info == null)
        {
            Debug.LogError("info is null");
            return;
        }

        _currentQuestion = info.question;

        RefreshUI();
    }

    void RefreshUI()
    {
        RefreshBlocker();
        RefreshQuestion();
        RefreshChoices();
    }

    void RefreshBlocker()
    {
        if (_currentQuestion == null)
        {
            ChoicesBlockerHandler.Instance.Block(gameObject);
        }
        else if (ChoicesBlockerHandler.Instance.IsBlocking(gameObject))
        {
            ChoicesBlockerHandler.Instance.UnBlock(gameObject);
        }
    }

    void RefreshQuestion()
    {
        if (_currentQuestion == null)
        {
            Debug.Log("MultipleChoicesHandler RefreshQuestion(): there are no questions left");
            return;
        }
        questionTextObject.text = _currentQuestion.questionText;
    }

    void RefreshChoices()
    {
        foreach (var button in _choiceButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        if (_currentQuestion == null)
        {
            Debug.Log("MultipleChoicesHandler RefreshChoices(): there are no questions left");
            return;
        }

        Assert.IsTrue(_currentQuestion.choices.Length <= choiceObjects.Length, "ChoiceObjects are not enough. Need more");

        for (int i = 0; i < _currentQuestion.choices.Length && i < choiceObjects.Length; i++)
        {
            bool isCorrect = _currentQuestion.choices[i].isCorrect;
            string choiceText = _currentQuestion.choices[i].choiceText;

            _choiceTexts[i].text = choiceText;
            _choiceButtons[i].onClick.AddListener(() => Answer(isCorrect));
            choiceObjects[i].SetActive(true);
        }

        for (int i = _currentQuestion.choices.Length; i < choiceObjects.Length; i++)
        {
            choiceObjects[i].SetActive(false);
        }
    }
}
