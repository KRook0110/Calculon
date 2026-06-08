using System.Collections;
using UnityEngine;

public class MovingHand : MonoBehaviour
{
    [SerializeField]
    private Transform startingPosition;
    [SerializeField]
    private Transform endingPosition;
    [SerializeField]
    private float forwardDuration;
    [SerializeField]
    private float returnDuration;
    [SerializeField]
    private AnimationCurve forwardCurve;
    [SerializeField]
    private AnimationCurve returnCurve;
    [SerializeField]
    private string targetLevelName;
    [SerializeField]
    private string nextLevelName;


    IEnumerator AnimationRoutine()
    {
        while(true)
        {
            float t = 0f;
            while(t < forwardDuration)
            {
                float eval = forwardCurve != null && forwardCurve.length > 0 ? forwardCurve.Evaluate(t / forwardDuration) : (t / forwardDuration);
                transform.position = Vector3.Lerp(startingPosition.position, endingPosition.position, eval);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            while(t < returnDuration)
            {
                float eval = returnCurve != null && returnCurve.length > 0 ? returnCurve.Evaluate(t / returnDuration) : (t / returnDuration);
                transform.position = Vector3.Lerp(endingPosition.position, startingPosition.position, eval);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }

    bool IsTargetLevel()
    {
        bool unlockedTargetLevel =  LevelState.Instance.GetLevelState(targetLevelName) == LevelState.State.Unlocked;
        bool unlockedNextLevel =  LevelState.Instance.GetLevelState(nextLevelName) == LevelState.State.Unlocked;
        return !unlockedNextLevel && unlockedTargetLevel;
    }

    void Start()
    {
        if (IsTargetLevel())
        {
            StartCoroutine(AnimationRoutine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void HandleSelect(LevelData data)
    {
        if(data.levelName != targetLevelName) return;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        LevelSelector.Instance.OnSelectLevel +=  HandleSelect;
    }

    void OnDisable()
    {
        if(LevelSelector.HasInstance) LevelSelector.Instance.OnSelectLevel -=  HandleSelect;
    }
}
