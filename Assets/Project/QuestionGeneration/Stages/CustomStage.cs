using UnityEngine;

/// <summary>
/// A placeholder QuestionStage used when questions are provided externally (e.g., via React).
/// This stage does not generate questions itself.
/// </summary>
public class CustomStage : QuestionStage
{
    private void OnValidate()
    {
        name = "CustomStage";
    }

    public override MultipleChoiceQuestion GenerateQuestion()
    {
        // This stage is a placeholder. 
        // Actual questions are handled by the QuestionGenerator's custom question pool.
        return null;
    }
}
