using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

[System.Serializable]
public class InitialGameData
{
    public string[] all_levels_unlocked;
    public int current_elo;
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
            InitialGameData initialData = JsonUtility.FromJson<InitialGameData>(data);
            if (initialData != null)
            {
                foreach (var level in initialData.all_levels_unlocked)
                {
                    LevelState.Instance.UnlockLevel(level);
                }
                QuestionGenerator.Instance.SetElo(initialData.current_elo);
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
        int numRepresentation = isCorrect?1:0;
        Debug.Log($"ReactUnityCommunication : Answer({numRepresentation}) Called");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Answer(numRepresentation);
#endif
    }

    public void SendFinished(bool isAlive)
    {
        int numRepresentation = isAlive?1:0;
        Debug.Log($"ReactUnityCommunication : Finished({numRepresentation}) Called");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Finished(numRepresentation);
#endif
    }
}
