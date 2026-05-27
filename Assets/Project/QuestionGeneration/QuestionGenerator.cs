using System.Collections.Generic;
using UnityEngine;

public class QuestionGenerator : Singleton<QuestionGenerator>
{
    [System.Serializable]
    public class StageInfo
    {
        public float chance;
        public QuestionStage stage;
    }

    [System.Serializable]
    public class QuestionInfo
    {
        public QuestionStage stage;
        public MultipleChoiceQuestion question;
    }

    [SerializeField]
    private List<StageInfo> _unlockedStages = new List<StageInfo>();
    [SerializeField]
    private List<StageInfo> _availableStages = new List<StageInfo>();
    [SerializeField]
    private QuestionStage _customGameStage;

    public List<MultipleChoiceQuestion> customQuestions {private set; get;} = new List<MultipleChoiceQuestion>();
    private int _currentCustomQuestionIndex = 0;

    [Header("Elo System")]
    public int currentElo {get; private set;} = 50;
    [SerializeField]
    private int eloGain = 5;
    [SerializeField]
    private int eloLoss = 3;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadElo();
    }

    private void LoadElo()
    {
        GameSaveData data = SaveSystem.Load();
        currentElo = data.currentElo;
    }

    private void SaveElo()
    {
        GameSaveData data = SaveSystem.Load();
        data.currentElo = currentElo;
        SaveSystem.Save(data);
    }

    public void EnableQuestionType(string stageName)
    {
        foreach (var stage in _unlockedStages)
        {
            if (stage.stage.name == stageName)
            {
                Debug.LogWarning($"Couldn't Unlock Stage {stageName}, Same stage is already present in unlocked stages");
                return;
            }
        }

        StageInfo info = null;
        foreach (var stage in _availableStages)
        {
            if (stage.stage.name == stageName)
            {
                info = stage;
                break;
            }
        }
        if(info == null)
        {
            Debug.LogError($"Couldn't find an available stage with name {stageName}.");
            return;
        }

        _unlockedStages.Add(info);

        Debug.Log($"Unlocked Stage {stageName}");
    }

    public void UpdateElo(bool isCorrect)
    {
        if (isCorrect)
        {
            currentElo += eloGain;
        }
        else
        {
            currentElo = Mathf.Max(0, currentElo - eloLoss);
        }
        SaveElo();
    }

    public void SetElo(int newElo)
    {
        currentElo = newElo;
        SaveElo();
    }

    public void ClearCustomQuestions()
    {
        customQuestions.Clear();
        _currentCustomQuestionIndex = 0;
    }

    public void ResetCustomQuestions()
    {
        _currentCustomQuestionIndex = 0;
    }

    public void AddCustomQuestion(MultipleChoiceQuestion question)
    {
        customQuestions.Add(question);
    }

    public QuestionInfo GenerateQuestion()
    {
        if (customQuestions != null && customQuestions.Count > 0)
        {
            if (_currentCustomQuestionIndex < customQuestions.Count)
            {
                var customQuestion = customQuestions[_currentCustomQuestionIndex++];

                return new QuestionInfo
                {
                    stage = _customGameStage,
                    question = customQuestion
                };
            }
            
            return null; // Finished all custom questions
        }

        if (_unlockedStages == null || _unlockedStages.Count == 0)
        {
            Debug.LogWarning("No stages unlocked to pick from!");
            return null;
        }

        var stage = GetRandomStage();
        stage.currentElo = currentElo;
        return new QuestionInfo
        {
            stage = stage,
            question = stage.GenerateQuestion()
        };
    }

    private QuestionStage GetRandomStage()
    {
        // 1. Calculate the total weight
        float totalChance = 0;
        foreach (var info in _unlockedStages)
        {
            totalChance += info.chance;
        }

        // 2. Pick a random number between 0 and totalChance
        float randomPoint = Random.Range(0, totalChance);

        // 3. Step through the list to find where the random point lands
        float currentSum = 0;
        foreach (var info in _unlockedStages)
        {
            currentSum += info.chance;
            if (randomPoint <= currentSum)
            {
                return info.stage;
            }
        }

        // Fallback (should only hit if list is empty or weights are 0)
        return _unlockedStages[0].stage;
    }
}
