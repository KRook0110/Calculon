using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
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
}

public class MultipleChoicesHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questionTextObject;
    [SerializeField]
    private GameObject[] choiceObjects;

    private List<MultipleChoiceQuestion> questions = new List<MultipleChoiceQuestion>();
    private List<TextMeshProUGUI> _choiceTexts = new List<TextMeshProUGUI>();
    private List<Button> _choiceButtons = new List<Button>();
    private int _currentQuestion = 0;


    /// Invoked when a user submits an answer.
    /// The boolean parameter is true if the answer was correct; otherwise, false.
    public Action<bool> OnAnswer;

    // Invoked when there are no Questions left 
    public Action OnFinish;

    void Start()
    {
        for(int i =0 ;i < choiceObjects.Length;i++)
        {
            Button foundButton = choiceObjects[i].GetComponentInChildren<Button>();
            TextMeshProUGUI foundText = choiceObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(foundText);
            Assert.IsNotNull(foundButton);
            _choiceTexts.Add(foundText);
            _choiceButtons.Add(foundButton);
        }

        RefreshUI();
    }
    public void AddQuestion(MultipleChoiceQuestion question)
    {
        questions.Add(question);
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
        if(_currentQuestion >= questions.Count)
        {
            OnFinish?.Invoke();
            return;
        }
        _currentQuestion++;


        RefreshUI();
    }

    void RefreshUI()
    {
        RefreshQuestion();
        RefreshChoices();
    }

    void RefreshQuestion()
    {
        if(_currentQuestion >= questions.Count)
        {
            Debug.Log("MultipleChoicesHandler : there are no questions left");
            return;
        }
        questionTextObject.text = questions[_currentQuestion].questionText;
    }

    void RefreshChoices()
    {
        if(_currentQuestion >= questions.Count)
        {
            Debug.Log("MultipleChoicesHandler : there are no questions left");
            return;
        }
        Assert.IsTrue(questions[_currentQuestion].choices.Length <= choiceObjects.Length, "ChoiceObjects are not enough. Need more");
        for(int i =0;i < questions[_currentQuestion].choices.Length;i++)
        {
            bool isCorrect = questions[_currentQuestion].choices[i].isCorrect;
            string choiceText = questions[_currentQuestion].choices[i].choiceText;
            _choiceTexts[i].text = choiceText;
            _choiceButtons[i].onClick.AddListener(() => Answer(isCorrect));
        }
    }

    void CleanChoices()
    {

    }
}
