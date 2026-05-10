using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class MutlipleChoiceCooldown : Singleton<MutlipleChoiceCooldown>
{

    [Header("Cooldowns")]
    [SerializeField]
    private float wrongAnswerCooldown = 2f;

    private SortedSet<float> cooldowns = new SortedSet<float>();
    public Action OnUsable;
    public Action OnCooldown;


    public void AddCooldown(float timeAmount)
    {
        if (timeAmount < 0)
        {
            Debug.LogWarning("Tried to apply a negative cooldown");
        }
        if (cooldowns.Count == 0)
        {
            OnCooldown?.Invoke();
        }
        cooldowns.Add(Time.time + timeAmount);
    }

    void WrongAnswerHandle(bool isCorrect, QuestionStage stage)
    {
        if (isCorrect) return;
        AddCooldown(wrongAnswerCooldown);
    }

    void OnEnable()
    {
        MultipleChoicesHandler.Instance.OnAnswer += WrongAnswerHandle;
    }

    void OnDisable()
    {
        MultipleChoicesHandler.Instance.OnAnswer -= WrongAnswerHandle;
    }


    void Update()
    {
        RemovePastCooldowns();
    }

    void RemovePastCooldowns()
    {
        bool hasRemoved = false;
        while (cooldowns.Count > 0 && cooldowns.Min < Time.time)
        {
            hasRemoved = true;
            cooldowns.Remove(cooldowns.Min);
        }

        if (cooldowns.Count == 0 && hasRemoved)
        {
            OnUsable?.Invoke();
        }
    }

}
