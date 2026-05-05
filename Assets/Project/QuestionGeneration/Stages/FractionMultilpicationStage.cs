using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FractionStageMultiplication : QuestionStage
{
    private struct FractionMultiplicationLevel
    {
        public int minElo;
        public int[] numeratorPool;
        public int[] denominatorPool;

        public FractionMultiplicationLevel(int minElo, int[] nPool, int[] dPool)
        {
            this.minElo = minElo;
            this.numeratorPool = nPool;
            this.denominatorPool = dPool;
        }
    }

    private static readonly FractionMultiplicationLevel[] Levels = new FractionMultiplicationLevel[]
    {
        new FractionMultiplicationLevel(0, new int[] { 1 }, new int[] { 2, 3, 4, 5 }),
        new FractionMultiplicationLevel(150, new int[] { 1, 2 }, new int[] { 2, 3, 4, 5, 6 }),
        new FractionMultiplicationLevel(300, new int[] { 1, 2, 3 }, new int[] { 2, 3, 4, 5, 6, 7, 8 }),
        new FractionMultiplicationLevel(500, new int[] { 2, 3, 4, 5 }, new int[] { 3, 4, 5, 6, 7, 8, 9, 10 }),
        new FractionMultiplicationLevel(800, new int[] { 3, 4, 5, 6, 7 }, new int[] { 5, 6, 7, 8, 9, 10, 11, 12 }),
    };

    public override MultipleChoiceQuestion GenerateQuestion()
    {
        FractionMultiplicationLevel level = Levels[0];
        for (int i = Levels.Length - 1; i >= 0; i--)
        {
            if (currentElo >= Levels[i].minElo)
            {
                level = Levels[i];
                break;
            }
        }

        int n1 = level.numeratorPool[UnityEngine.Random.Range(0, level.numeratorPool.Length)];
        int d1 = level.denominatorPool[UnityEngine.Random.Range(0, level.denominatorPool.Length)];
        int n2 = level.numeratorPool[UnityEngine.Random.Range(0, level.numeratorPool.Length)];
        int d2 = level.denominatorPool[UnityEngine.Random.Range(0, level.denominatorPool.Length)];

        // Ensure proper fractions for early levels
        if (n1 >= d1) d1 = n1 + UnityEngine.Random.Range(1, 3);
        if (n2 >= d2) d2 = n2 + UnityEngine.Random.Range(1, 3);

        Fraction f1 = new Fraction(n1, d1).Simplify();
        Fraction f2 = new Fraction(n2, d2).Simplify();
        Fraction correct = Fraction.Multiply(f1, f2);

        List<Fraction> choices = GenerateChoices(f1, f2, correct, level);
        string[] options = choices.Select(f => f.ToString()).ToArray();
        int correctIndex = choices.IndexOf(correct);

        return MultipleChoiceQuestion.CreateQuestion($"{f1} × {f2} = ?", options, correctIndex);
    }

    private List<Fraction> GenerateChoices(Fraction f1, Fraction f2, Fraction correct, FractionMultiplicationLevel level)
    {
        HashSet<Fraction> choices = new HashSet<Fraction> { correct };

        // 1. Common mistake: (n1+n2)/(d1+d2) - Adding everything
        Fraction mistake1 = new Fraction(f1.Numerator + f2.Numerator, f1.Denominator + f2.Denominator).Simplify();
        if (!choices.Contains(mistake1)) choices.Add(mistake1);

        // 2. Common mistake: Adding instead of multiplying
        Fraction mistake2 = Fraction.Add(f1, f2);
        if (!choices.Contains(mistake2)) choices.Add(mistake2);

        // 3. Common mistake: Cross-multiplying result (n1*d2)/(d1*n2)
        if (f2.Numerator != 0)
        {
            Fraction mistake3 = new Fraction(f1.Numerator * f2.Denominator, f1.Denominator * f2.Numerator).Simplify();
            if (!choices.Contains(mistake3)) choices.Add(mistake3);
        }

        // 4. Random variations or "off-by-one" logical mistakes from pool
        int attempts = 0;
        while (choices.Count < 4 && attempts < 20)
        {
            attempts++;
            int rn1 = level.numeratorPool[UnityEngine.Random.Range(0, level.numeratorPool.Length)];
            int rd1 = level.denominatorPool[UnityEngine.Random.Range(0, level.denominatorPool.Length)];
            int rn2 = level.numeratorPool[UnityEngine.Random.Range(0, level.numeratorPool.Length)];
            int rd2 = level.denominatorPool[UnityEngine.Random.Range(0, level.denominatorPool.Length)];
            
            Fraction variant = Fraction.Multiply(new Fraction(rn1, rd1), new Fraction(rn2, rd2)).Simplify();
            if (!choices.Contains(variant)) choices.Add(variant);
        }

        // Final fallbacks
        while (choices.Count < 4)
        {
            Fraction fallback = new Fraction(UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(2, 20)).Simplify();
            if (!choices.Contains(fallback)) choices.Add(fallback);
        }

        List<Fraction> result = choices.ToList();
        // Shuffle
        for (int i = 0; i < result.Count; i++)
        {
            Fraction temp = result[i];
            int randomIndex = UnityEngine.Random.Range(i, result.Count);
            result[i] = result[randomIndex];
            result[randomIndex] = temp;
        }

        return result;
    }
}
