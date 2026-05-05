using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplicationStage : QuestionStage
{
    private struct MultiplicationLevel
    {
        public int minElo;
        public int[] poolA;
        public int[] poolB;

        public MultiplicationLevel(int minElo, int[] poolA, int[] poolB)
        {
            this.minElo = minElo;
            this.poolA = poolA;
            this.poolB = poolB;
        }
    }

    private static readonly MultiplicationLevel[] Levels = new MultiplicationLevel[]
    {
        new MultiplicationLevel(0, new int[] { 2, 5, 10 }, new int[] { 1, 2, 3, 4, 5, 10 }),
        new MultiplicationLevel(100, new int[] { 2, 5, 10, 11 }, new int[] { 2, 3, 4, 5, 10, 11 }),
        new MultiplicationLevel(250, new int[] { 3, 4, 5 }, new int[] { 2, 3, 4, 5, 10 }),
        new MultiplicationLevel(400, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 3, 4, 5 }),
        new MultiplicationLevel(600, new int[] { 3, 7, 13 }, new int[] { 2, 3, 4, 5, 6 }),
        new MultiplicationLevel(800, new int[] { 3, 7, 13 }, new int[] { 7, 8, 9, 11, 12 }),
        new MultiplicationLevel(1000, new int[] { 3, 7, 13, 17 }, new int[] { 3, 7, 13, 17 }),
    };

    public override MultipleChoiceQuestion GenerateQuestion()
    {
        MultiplicationLevel level = Levels[0];
        for (int i = Levels.Length - 1; i >= 0; i--)
        {
            if (currentElo >= Levels[i].minElo)
            {
                level = Levels[i];
                break;
            }
        }

        int a = level.poolA[UnityEngine.Random.Range(0, level.poolA.Length)];
        int b = level.poolB[UnityEngine.Random.Range(0, level.poolB.Length)];
        int correct = a * b;

        List<int> choiceValues = GenerateChoiceValues(a, b, correct, level);
        string[] options = choiceValues.Select(v => v.ToString()).ToArray();
        int correctIndex = choiceValues.IndexOf(correct);

        return MultipleChoiceQuestion.CreateQuestion($"{a} × {b} = ?", options, correctIndex);
    }

    private List<int> GenerateChoiceValues(int a, int b, int correct, MultiplicationLevel level)
    {
        HashSet<int> choices = new HashSet<int> { correct };

        // 1. Add "a + b" as a common mistake
        if (a + b != correct)
        {
            choices.Add(a + b);
        }

        // 2. Add some "off-by-one" operand products (logical mistakes, not offsets)
        int[] altA = { a - 1, a + 1 };
        int[] altB = { b - 1, b + 1 };
        
        foreach (int aa in altA)
        {
            if (choices.Count >= 4) break;
            if (aa > 0) choices.Add(aa * b);
        }

        foreach (int bb in altB)
        {
            if (choices.Count >= 4) break;
            if (bb > 0) choices.Add(a * bb);
        }

        // 3. Fill with other possible products from the level's pool
        int attempts = 0;
        while (choices.Count < 4 && attempts < 20)
        {
            int randA = level.poolA[UnityEngine.Random.Range(0, level.poolA.Length)];
            int randB = level.poolB[UnityEngine.Random.Range(0, level.poolB.Length)];
            int val = randA * randB;
            if (val != correct)
            {
                choices.Add(val);
            }
            attempts++;
        }

        // 4. Fallback: if we still don't have enough, use some other products
        int fallback = 1;
        while (choices.Count < 4)
        {
            int val = correct + (fallback * 10); // Not a simple offset, but a distinct value
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
