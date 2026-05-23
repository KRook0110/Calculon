using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using TMPro;

[System.Serializable]
public class CustomChoice
{
    public string text;
    public bool is_correct;
}

[System.Serializable]
public class CustomQuestion
{
    public string question;
    public CustomChoice[] choices;
}

[System.Serializable]
public class InitialGameData
{
    public string[] all_levels_unlocked;
    public int current_elo;
    public CustomQuestion[] questions;
}

[System.Serializable]
public class FinalizedGameData
{
    public int elo_gained;
    public string[] new_levels_unlocked;
}

public class ReactUnityCommunication : Singleton<ReactUnityCommunication>
{
    public TextMeshProUGUI _latestMessage;

    [SerializeField, Tooltip("The enemy prefab to spawn for custom question levels.")]
    private GameObject customEnemyPrefab;

    [DllImport("__Internal")]
    private static extern void Init();
    [DllImport("__Internal")]
    private static extern void Level(string level_name);
    [DllImport("__Internal")]
    private static extern void Answer(int correct);
    [DllImport("__Internal")]
    private static extern void Finished(int alive);

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void GiveInitialdata(string data)
    {
        Debug.Log($"ReactUnityCommunication : GiveInitialData called with data {data}");
        try
        {
            // If the data is a raw array, wrap it to match InitialGameData
            if (data.Trim().StartsWith("["))
            {
                data = "{\"questions\":" + data + "}";
            }

            InitialGameData initialData = JsonUtility.FromJson<InitialGameData>(data);
            if (initialData != null)
            {
                if (initialData.all_levels_unlocked != null)
                {
                    foreach (var level in initialData.all_levels_unlocked)
                    {
                        LevelState.Instance.UnlockLevel(level);
                    }
                }
                QuestionGenerator.Instance.SetElo(initialData.current_elo);

                if (initialData.questions != null && initialData.questions.Length > 0)
                {
                    QuestionGenerator.Instance.ClearCustomQuestions();
                    foreach (var cq in initialData.questions)
                    {
                        MultipleChoiceQuestion q = new MultipleChoiceQuestion();
                        q.questionText = cq.question;
                        q.choices = new ChoiceData[cq.choices.Length];
                        for (int i = 0; i < cq.choices.Length; i++)
                        {
                            q.choices[i] = new ChoiceData
                            {
                                choiceText = cq.choices[i].text,
                                isCorrect = cq.choices[i].is_correct
                            };
                        }
                        QuestionGenerator.Instance.AddCustomQuestion(q);
                    }

                    if (customEnemyPrefab != null)
                    {
                        // Create dynamic level data
                        LevelData customLevel = ScriptableObject.CreateInstance<LevelData>();
                        customLevel.levelName = "Custom React Level";
                        customLevel.enemies = new System.Collections.Generic.List<GameObject> { customEnemyPrefab };
                        customLevel.resetLevelSelectionToDefault = true;

                        LevelSelector.Instance.selectedLevel = customLevel;
                    }
                    else
                    {
                        Debug.LogWarning("ReactUnityCommunication: customEnemyPrefab is not assigned. Cannot generate custom level.");
                    }

                    SceneManager.LoadScene("BattleScene");
                }

                Debug.Log("ReactUnityCommunication: Initial data received and saved.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ReactUnityCommunication: Failed to parse initial data: {e.Message}");
        }
    }

    public void Finalized(string data)
    {
        Debug.Log($"ReactUnityCommunication : Finalized called with data {data}");
        try
        {
            FinalizedGameData finalData = JsonUtility.FromJson<FinalizedGameData>(data);
            if (finalData != null)
            {
                foreach (var level in finalData.new_levels_unlocked)
                {
                    LevelState.Instance.UnlockLevel(level);
                }
                QuestionGenerator.Instance.SetElo(finalData.elo_gained + QuestionGenerator.Instance.currentElo);

                Debug.Log("ReactUnityCommunication: Finalized data received and saved.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ReactUnityCommunication: Failed to parse finalized data: {e.Message}");
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test GiveInitialData")]
    public void TestGiveInitialData()
    {
        string testData = @"{
            ""all_levels_unlocked"":[""Level 1"",""Level 6""],
            ""current_elo"":530,
            ""questions"": [
                {""question"":""What is 2 + 2?"",""choices"":[{""text"":""3"",""is_correct"":false},{""text"":""4"",""is_correct"":true},{""text"":""5"",""is_correct"":false},{""text"":""22"",""is_correct"":false}]},
                {""question"":""   "",""choices"":[{""text"":""A"",""is_correct"":true},{""text"":""B"",""is_correct"":false},{""text"":""C"",""is_correct"":false},{""text"":""D"",""is_correct"":false}]},
                {""question"":""If f(x) = x³ - 6x² + 11x - 6, which of the following is NOT a root of f(x)?"",""choices"":[{""text"":""x = 1"",""is_correct"":false},{""text"":""x = 2"",""is_correct"":false},{""text"":""x = 4"",""is_correct"":true},{""text"":""x = 3"",""is_correct"":false}]},
                {""question"":""<script>alert('xss')</script>"",""choices"":[{""text"":""<b>bold</b>"",""is_correct"":false},{""text"":""'; DROP TABLE questions; –"",""is_correct"":false},{""text"":""&lt;sanitized&gt;"",""is_correct"":true},{""text"":""undefined"",""is_correct"":false}]},
                {""question"":""A train travels at 120 km/h. How long does it take to cover 450 km, in hours and minutes?"",""choices"":[{""text"":""3 hours 30 minutes"",""is_correct"":false},{""text"":""3 hours 45 minutes"",""is_correct"":true},{""text"":""4 hours 0 minutes"",""is_correct"":false},{""text"":""3 hours 15 minutes"",""is_correct"":false}]},
                {""question"":""Which element has the atomic number 79?"",""choices"":[{""text"":""Silver"",""is_correct"":false},{""text"":""Platinum"",""is_correct"":false},{""text"":""Gold"",""is_correct"":true},{""text"":""Copper"",""is_correct"":false}]},
                {""question"":""! @#$%^&*()_+{}|:<>? Which of the following is the correct answer?"",""choices"":[{""text"":""Option with 'single quotes' and \""double quotes\"""",""is_correct"":false},{""text"":""Option with\nnewline\tand tab"",""is_correct"":false},{""text"":""Option with emoji 🎉🔥💯"",""is_correct"":false},{""text"":""None of the above — this is a QA boundary test"",""is_correct"":true}]},
                {""question"":""Given the integral ∫(0 to π) sin(x) dx, what is the result?"",""choices"":[{""text"":""0"",""is_correct"":false},{""text"":""π"",""is_correct"":false},{""text"":""2"",""is_correct"":true},{""text"":""-2"",""is_correct"":false}]},
                {""question"":"""",""choices"":[{""text"":""Empty question — should be flagged by validation"",""is_correct"":true},{""text"":""Valid answer"",""is_correct"":false},{""text"":""Another valid answer"",""is_correct"":false},{""text"":""Yet another answer"",""is_correct"":false}]},
                {""question"":""In Shakespeare’s 'Hamlet', what are Hamlet’s famous opening words of his soliloquy in Act 3, Scene 1?"",""choices"":[{""text"":""'All the world's a stage'"",""is_correct"":false},{""text"":""'To be, or not to be, that is the question'"",""is_correct"":true},{""text"":""'Friends, Romans, countrymen, lend me your ears'"",""is_correct"":false},{""text"":""'What a piece of work is a man'"",""is_correct"":false}]}
            ]
        }";
        GiveInitialdata(testData);
    }
#endif

    public void SendInit()
    {
        Debug.Log($"ReactUnityCommunication : Init() Called");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Init();
#endif
    }

    public void SendLevel(string levelName)
    {
        Debug.Log($"ReactUnityCommunication : Level({levelName}) Called");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Level(levelName);
#endif
    }

    public void SendAnswer(bool isCorrect)
    {
        int numRepresentation = isCorrect ? 1 : 0;
        Debug.Log($"ReactUnityCommunication : Answer({numRepresentation}) Called");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Answer(numRepresentation);
#endif
    }

    public void SendFinished(bool isAlive)
    {
        int numRepresentation = isAlive ? 1 : 0;
        Debug.Log($"ReactUnityCommunication : Finished({numRepresentation}) Called");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Finished(numRepresentation);
#endif
    }
}
