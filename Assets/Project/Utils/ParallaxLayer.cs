using UnityEngine;

/// <summary>
/// A parallax scrolling script for the X-axis.
/// Apply this to individual background layers.
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Tooltip("0 = Fixed to camera, 1 = Static in world, >1 = Moves faster than camera")]
    private float parallaxEffect;

    [SerializeField] private bool infiniteScroll = true;

    private float _length;
    private float _startPos;
    private Transform _cam;

    private void Start()
    {
        _cam = Camera.main != null ? Camera.main.transform : null;
        _startPos = transform.position.x;

        // Try to get the width from SpriteRenderer or use a default if missing
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float localLength = 0f;
            if (spriteRenderer.sprite != null)
            {
                localLength = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;
                if (spriteRenderer.drawMode != SpriteDrawMode.Simple)
                {
                    localLength = spriteRenderer.size.x;
                }
            }

            _length = localLength * transform.lossyScale.x;

            if (infiniteScroll && localLength > 0f)
            {
                CreateClones(spriteRenderer, localLength);
            }
        }
        else
        {
            // Fallback: use scale or a fixed value if no sprite is found
            _length = transform.localScale.x;
            Debug.LogWarning($"[ParallaxLayer] No SpriteRenderer found on {gameObject.name}. Infinite scroll might be inaccurate.", this);
        }
    }

    private void CreateClones(SpriteRenderer originalRenderer, float localLength)
    {
        // Clone left
        GameObject leftClone = new GameObject(gameObject.name + "_LeftClone");
        leftClone.transform.SetParent(transform);
        leftClone.transform.localPosition = new Vector3(-localLength, 0f, 0f);
        leftClone.transform.localRotation = Quaternion.identity;
        leftClone.transform.localScale = Vector3.one;

        SpriteRenderer leftRenderer = leftClone.AddComponent<SpriteRenderer>();
        CopySpriteRenderer(originalRenderer, leftRenderer);

        // Clone right
        GameObject rightClone = new GameObject(gameObject.name + "_RightClone");
        rightClone.transform.SetParent(transform);
        rightClone.transform.localPosition = new Vector3(localLength, 0f, 0f);
        rightClone.transform.localRotation = Quaternion.identity;
        rightClone.transform.localScale = Vector3.one;

        SpriteRenderer rightRenderer = rightClone.AddComponent<SpriteRenderer>();
        CopySpriteRenderer(originalRenderer, rightRenderer);
    }

    private void CopySpriteRenderer(SpriteRenderer source, SpriteRenderer target)
    {
        target.sprite = source.sprite;
        target.color = source.color;
        target.drawMode = source.drawMode;
        target.size = source.size;
        target.tileMode = source.tileMode;
        target.sortingLayerID = source.sortingLayerID;
        target.sortingOrder = source.sortingOrder;
        target.maskInteraction = source.maskInteraction;
        target.spriteSortPoint = source.spriteSortPoint;
        target.material = source.material;
        target.renderingLayerMask = source.renderingLayerMask;
        target.flipX = source.flipX;
        target.flipY = source.flipY;
    }

    private void LateUpdate()
    {
        if (_cam == null)
        {
            _cam = Camera.main != null ? Camera.main.transform : null;
            if (_cam == null) return;
        }

        // How far the camera has moved relative to the start position
        float temp = (_cam.position.x * (1 - parallaxEffect));
        
        // How far the object should move
        float dist = (_cam.position.x * parallaxEffect);

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        // Infinite wrapping logic
        if (infiniteScroll && _length > 0)
        {
            while (temp > _startPos + _length)
            {
                _startPos += _length;
            }
            while (temp < _startPos - _length)
            {
                _startPos -= _length;
            }
        }
    }
}
