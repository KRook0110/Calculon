using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FractionSubtractionStage : QuestionStage
{
    private void OnValidate()
    {
        name = "FractionSubtractionStage";
    }

    private struct FractionLevel
    {
        public int minElo;
        public int maxNumerator;
        public int maxDenominator;
        public bool forceSameDenominator;

        public FractionLevel(int minElo, int maxN, int maxD, bool same)
        {
            this.minElo = minElo;
            this.maxNumerator = maxN;
            this.maxDenominator = maxD;
            this.forceSameDenominator = same;
        }
    }

    private static readonly FractionLevel[] Levels = new FractionLevel[]
    {
        new FractionLevel(0, 1, 4, true),    // e.g., 1/2 - 1/2, 1/4 - 1/4
        new FractionLevel(100, 3, 6, true),  // same denoms up to 6
        new FractionLevel(250, 5, 10, true), // same denoms up to 10
        new FractionLevel(400, 3, 5, false), // diff denoms, small
        new FractionLevel(600, 5, 8, false), // diff denoms, medium
        new FractionLevel(800, 9, 12, false), // diff denoms, harder
    };

    public override MultipleChoiceQuestion GenerateQuestion()
    {
        FractionLevel level = Levels[0];
        for (int i = Levels.Length - 1; i >= 0; i--)
        {
            if (currentElo >= Levels[i].minElo)
            {
                level = Levels[i];
                break;
            }
        }

        int d1 = UnityEngine.Random.Range(2, level.maxDenominator + 1);
        int d2 = level.forceSameDenominator ? d1 : UnityEngine.Random.Range(2, level.maxDenominator + 1);
        
        int n1 = UnityEngine.Random.Range(1, Math.Min(d1, level.maxNumerator + 1));
        int n2 = UnityEngine.Random.Range(1, Math.Min(d2, level.maxNumerator + 1));

        Fraction f1 = new Fraction(n1, d1).Simplify();
        Fraction f2 = new Fraction(n2, d2).Simplify();

        // Ensure f1 >= f2 for subtraction to avoid negative results in early stages
        if (f1.Numerator * f2.Denominator < f2.Numerator * f1.Denominator)
        {
            Fraction temp = f1;
            f1 = f2;
            f2 = temp;
        }

        Fraction correct = Fraction.Subtract(f1, f2);

        List<Fraction> choices = GenerateChoices(f1, f2, correct);
        string[] options = choices.Select(f => f.ToString()).ToArray();
        int correctIndex = choices.IndexOf(correct);

        return MultipleChoiceQuestion.CreateQuestion($"{f1} - {f2} = ?", options, correctIndex);
    }

    private List<Fraction> GenerateChoices(Fraction f1, Fraction f2, Fraction correct)
    {
        HashSet<Fraction> choices = new HashSet<Fraction> { correct };

        // 1. Common mistake: (n1-n2)/(d1+d2) - Analogous to Addition's (n1+n2)/(d1+d2)
        Fraction mistake1 = new Fraction(Math.Abs(f1.Numerator - f2.Numerator), f1.Denominator + f2.Denominator).Simplify();
        if (!choices.Contains(mistake1)) choices.Add(mistake1);

        // 2. Common mistake: (n1-n2)/d1 - Analogous to Addition's (n1+n2)/d1
        Fraction mistake2 = new Fraction(Math.Abs(f1.Numerator - f2.Numerator), f1.Denominator).Simplify();
        if (!choices.Contains(mistake2)) choices.Add(mistake2);

        // 3. Common mistake: Multiplication instead of subtraction - Consistent with AdditionStage
        Fraction mistake3 = Fraction.Multiply(f1, f2);
        if (!choices.Contains(mistake3)) choices.Add(mistake3);

        // Fill remaining with random variations
        int attempts = 0;
        while (choices.Count < 4 && attempts < 20)
        {
            attempts++;
            int nOffset = UnityEngine.Random.Range(-2, 3);
            int dOffset = UnityEngine.Random.Range(-2, 3);
            
            if (nOffset == 0 && dOffset == 0) continue;

            int newN = Math.Max(0, correct.Numerator + nOffset);
            int newD = Math.Max(2, correct.Denominator + dOffset);
            
            Fraction variant = new Fraction(newN, newD).Simplify();
            if (!choices.Contains(variant))
            {
                choices.Add(variant);
            }
        }

        // Final fallback if still not enough choices
        while (choices.Count < 4)
        {
            Fraction fallback = new Fraction(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(2, 10)).Simplify();
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
