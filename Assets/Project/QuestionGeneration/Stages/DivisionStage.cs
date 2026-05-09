using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DivisionStage : QuestionStage
{
    private void OnValidate()
    {
        name = "DivisionStage";
    }

    private struct DivisionLevel
    {
        public int minElo;
        public int[] divisorPool;
        public int[] quotientPool;

        public DivisionLevel(int minElo, int[] divisorPool, int[] quotientPool)
        {
            this.minElo = minElo;
            this.divisorPool = divisorPool;
            this.quotientPool = quotientPool;
        }
    }

    private static readonly DivisionLevel[] Levels = new DivisionLevel[]
    {
        new DivisionLevel(0, new int[] { 2, 5, 10 }, new int[] { 1, 2, 3, 4, 5, 10 }),
        new DivisionLevel(150, new int[] { 2, 3, 4, 5, 10, 11 }, new int[] { 2, 3, 4, 5, 10, 11 }),
        new DivisionLevel(300, new int[] { 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 }),
        new DivisionLevel(500, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9, 10, 11, 12 }),
        new DivisionLevel(700, new int[] { 3, 7, 13 }, new int[] { 2, 3, 4, 5, 6 }),
        new DivisionLevel(900, new int[] { 7, 11, 13 }, new int[] { 7, 8, 9, 11, 12 }),
        new DivisionLevel(1100, new int[] { 3, 7, 13, 17 }, new int[] { 3, 7, 11, 13, 17 }),
    };

    public override MultipleChoiceQuestion GenerateQuestion()
    {
        DivisionLevel level = Levels[0];
        for (int i = Levels.Length - 1; i >= 0; i--)
        {
            if (currentElo >= Levels[i].minElo)
            {
                level = Levels[i];
                break;
            }
        }

        // To ensure integer results: dividend = divisor * quotient
        int divisor = level.divisorPool[UnityEngine.Random.Range(0, level.divisorPool.Length)];
        int quotient = level.quotientPool[UnityEngine.Random.Range(0, level.quotientPool.Length)];
        int dividend = divisor * quotient;

        int correct = quotient;

        List<int> choiceValues = GenerateChoiceValues(dividend, divisor, correct, level);
        string[] options = choiceValues.Select(v => v.ToString()).ToArray();
        int correctIndex = choiceValues.IndexOf(correct);

        return MultipleChoiceQuestion.CreateQuestion($"{dividend} ÷ {divisor} = ?", options, correctIndex);
    }

    private List<int> GenerateChoiceValues(int dividend, int divisor, int correct, DivisionLevel level)
    {
        HashSet<int> choices = new HashSet<int> { correct };

        // 1. Add dividend - divisor or dividend + divisor (unlikely but possible mistakes)
        // More likely: dividend - correct or divisor - correct
        if (dividend - divisor > 0) choices.Add(dividend - divisor);

        // 2. Logical errors: off-by-one in the result
        if (correct - 1 > 0) choices.Add(correct - 1);
        choices.Add(correct + 1);

        // 3. Distractors from the current level's pool (other possible quotients)
        int attempts = 0;
        while (choices.Count < 4 && attempts < 20)
        {
            int randQuotient = level.quotientPool[UnityEngine.Random.Range(0, level.quotientPool.Length)];
            if (randQuotient != correct)
            {
                choices.Add(randQuotient);
            }
            attempts++;
        }

        // 4. Fallback: if we still don't have enough, use multiples
        int fallback = 1;
        while (choices.Count < 4)
        {
            int val = correct + (fallback * 5);
            if (!choices.Contains(val)) choices.Add(val);
            fallback++;
        }

        List<int> result = choices.ToList();
        // Shuffle result
        for (int i = 0; i < result.Count; i++)
        {
            int temp = result[i];
            int randomIndex = UnityEngine.Random.Range(i, result.Count);
            result[i] = result[randomIndex];
            result[randomIndex] = temp;
        }

        return result;
    }
}
