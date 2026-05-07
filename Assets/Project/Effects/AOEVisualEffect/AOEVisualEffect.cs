using System.Collections;
using UnityEngine;

// Automatically add a SpriteRenderer when this script is added
[RequireComponent(typeof(SpriteRenderer))]
public class AOEVisualEffect : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Configures the sprite size/color and starts the animation.
    /// </summary>
    public void Initialize(float radius, Color baseColor, float duration)
    {
        // 1. Set the color (ensuring it starts fully opaque)
        _spriteRenderer.color = baseColor;

        // 2. Scale the GameObject to match the AOE radius.
        // (Radius is from the center to the edge, so we multiply by 2 for the full diameter)
        transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);

        // 3. Start the fade-out process
        StartCoroutine(AnimateExplosion(duration));
    }

    IEnumerator AnimateExplosion(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = _spriteRenderer.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            
            // Calculate the alpha percentage (fading from 1 to 0)
            float alphaPercentage = 1f - (elapsedTime / duration);
            
            // Apply the new alpha
            startColor.a = alphaPercentage;
            _spriteRenderer.color = startColor;
            
            yield return null; // Wait for next frame
        }

        // Animation done, clean up
        Destroy(gameObject);
    }
}