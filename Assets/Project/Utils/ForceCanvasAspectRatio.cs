using UnityEngine;

/**
 * @brief Fits a RectTransform (e.g. a UI Panel directly under a Canvas) to match the Camera's aspect-ratio-restricted viewport.
 * 
 * Simply place this script on a root panel inside your Canvas, and all child UI elements will anchor
 * relative to the 16:9 viewport instead of the physical screen bounds.
 */
[RequireComponent(typeof(RectTransform))]
public class ForceCanvasAspectRatio : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    private RectTransform m_rectTransform;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Start()
    {
        UpdatePanelBounds();
    }

    private void Update()
    {
        UpdatePanelBounds();
    }

    private void UpdatePanelBounds()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            if (targetCamera == null) return;
        }

        // Get the viewport rect of the camera (which has been modified by ForceAspectRatio)
        Rect viewportRect = targetCamera.rect;

        // Set anchors of the RectTransform to match the camera's viewport
        m_rectTransform.anchorMin = new Vector2(viewportRect.x, viewportRect.y);
        m_rectTransform.anchorMax = new Vector2(viewportRect.x + viewportRect.width, viewportRect.y + viewportRect.height);

        // Reset offsets to make it stretch perfectly to the new anchors
        m_rectTransform.offsetMin = Vector2.zero;
        m_rectTransform.offsetMax = Vector2.zero;
    }
}
