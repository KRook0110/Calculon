using UnityEngine;

/**
 * @brief Forces the camera to render at a specific target aspect ratio (default 16:9).
 * 
 * If the screen is wider than the target aspect ratio, it adds pillarboxes (black bars on the sides).
 * If the screen is taller, it adds letterboxes (black bars on the top and bottom).
 */
[RequireComponent(typeof(Camera))]
public class ForceAspectRatio : MonoBehaviour
{
    [SerializeField]
    private float targetAspectWidth = 16f;

    [SerializeField]
    private float targetAspectHeight = 9f;

    private Camera m_camera;
    private Camera m_backgroundCamera;
    
    private int m_lastWidth = -1;
    private int m_lastHeight = -1;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        CreateBackgroundCamera();
        UpdateAspectRatio();
    }

    private void Update()
    {
        // Dynamic update if screen resolution changes (resizing window in editor or player)
        if (Screen.width != m_lastWidth || Screen.height != m_lastHeight)
        {
            UpdateAspectRatio();
        }
    }

    /**
     * @brief Creates a secondary camera that clears the screen to black before our main camera renders.
     * This prevents rendering garbage/trails in the black bar zones.
     */
    private void CreateBackgroundCamera()
    {
        GameObject backgroundCamObj = new GameObject("BackgroundClearCamera");
        backgroundCamObj.transform.SetParent(transform);
        
        m_backgroundCamera = backgroundCamObj.AddComponent<Camera>();
        m_backgroundCamera.depth = m_camera.depth - 1;
        m_backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        m_backgroundCamera.backgroundColor = Color.black;
        m_backgroundCamera.cullingMask = 0; // Don't render anything, just clear screen
    }

    private void UpdateAspectRatio()
    {
        m_lastWidth = Screen.width;
        m_lastHeight = Screen.height;

        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)m_lastWidth / (float)m_lastHeight;
        float scaleHeight = windowAspect / targetAspect;

        Rect rect = m_camera.rect;

        if (scaleHeight < 1.0f)
        {
            // Taller window (e.g. 4:3 or 16:10) -> Letterbox
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // Wider window (e.g. 21:9) -> Pillarbox
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        m_camera.rect = rect;
    }
}
