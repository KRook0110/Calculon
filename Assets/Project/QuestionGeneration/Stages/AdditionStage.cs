using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdditionStage : QuestionStage
{
    private void OnValidate()
    {
        name = "AdditionStage";
    }

    private int[] offsets = { 1, -1, 10, -10, 2, -2, 5, -5, -4, 4};
    private struct AdditionLevel
    {
        public int minElo;
        public int minA, maxA;
        public int minB, maxB;

        public AdditionLevel(int minElo, int minA, int maxA, int minB, int maxB)
        {
            this.minElo = minElo;
            this.minA = minA;
            this.maxA = maxA;
            this.minB = minB;
            this.maxB = maxB;
        }
    }

    private static readonly AdditionLevel[] Levels = new AdditionLevel[]
    {
        new AdditionLevel(0, 1, 10, 1, 1),    // +1
        new AdditionLevel(50, 1, 10, 2, 2),   // +2
        new AdditionLevel(100, 1, 10, 1, 2),  // +1 to +2
        new AdditionLevel(150, 1, 10, 3, 3),  // +3
        new AdditionLevel(200, 1, 10, 1, 3),  // +1 to +3
        new AdditionLevel(250, 1, 10, 4, 4),  // +4
        new AdditionLevel(300, 1, 10, 1, 4),  // +1 to +4
        new AdditionLevel(350, 1, 10, 5, 5),  // +5
        new AdditionLevel(400, 1, 10, 1, 5),  // +1 to +5
        new AdditionLevel(500, 1, 10, 6, 10), // +6 to +10
        new AdditionLevel(600, 11, 20, 1, 9), // 2-digit + 1-digit (no carry focus maybe?)
        new AdditionLevel(700, 11, 99, 1, 9), // 2-digit + 1-digit
        new AdditionLevel(800, 11, 50, 11, 50), // 2-digit + 2-digit
        new AdditionLevel(1000, 11, 99, 11, 99), // 2-digit + 2-digit (harder)
    };

    public override MultipleChoiceQuestion GenerateQuestion()
    {
        AdditionLevel level = Levels[0];
        for (int i = Levels.Length - 1; i >= 0; i--)
        {
            if (currentElo >= Levels[i].minElo)
            {
                level = Levels[i];
                break;
            }
        }

        int a = UnityEngine.Random.Range(level.minA, level.maxA + 1);
        int b = UnityEngine.Random.Range(level.minB, level.maxB + 1);
        int correct = a + b;

        List<int> choiceValues = GenerateChoiceValues(correct);
        string[] options = choiceValues.Select(v => v.ToString()).ToArray();
        int correctIndex = choiceValues.IndexOf(correct);

        return MultipleChoiceQuestion.CreateQuestion($"{a} + {b} = ?", options, correctIndex);
    }

    private List<int> GenerateChoiceValues(int correct)
    {
        HashSet<int> choices = new HashSet<int> {
            correct
        };

        // Shuffle offsets
        for (int i = 0; i < offsets.Length; i++)
        {
            int temp = offsets[i];
            int randomIndex = UnityEngine.Random.Range(i, offsets.Length);
            offsets[i] = offsets[randomIndex];
            offsets[randomIndex] = temp;
        }

        foreach (int offset in offsets)
        {
            int val = correct + offset;
            if (val >= 0 && !choices.Contains(val))
            {
                choices.Add(val);
                if (choices.Count >= 4) break;
            }
        }

        // Fill with random if still not enough
        while (choices.Count < 4)
        {
            int val = correct + UnityEngine.Random.Range(-20, 21);
            if (val >= 0 && !choices.Contains(val))
            {
                choices.Add(val);
            }
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
