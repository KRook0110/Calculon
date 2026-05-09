using System.Collections;
using UnityEngine;

public class UIFade 
{
    public static IEnumerator Fade(CanvasGroup canvas, float time = 1f, float startAlpha = 0f, float endAlpha = 1f)
    {
        canvas.alpha = startAlpha;
        float t = 0f;
        while(t < time)
        {
            canvas.alpha = Mathf.Lerp(startAlpha, endAlpha, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = endAlpha;
    }
}