using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A smooth horizontal camera scroller that works with the New Input System.
/// Includes boundaries and lerp-based smoothing.
/// </summary>
public class SmoothCameraScroller : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField, Tooltip("Speed when holding buttons (A/D, Arrows)")] 
    private float buttonSpeed = 20f;
    [SerializeField, Tooltip("Sensitivity for the mouse scroll wheel")]
    private float scrollWheelSensitivity = 0.05f;
    [SerializeField] private float smoothTime = 0.15f;

    [Header("Boundaries")]
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;

    private float _targetX;
    private float _currentVelocity;
    private float _horizontalInput;
    private bool _hasCentredInitial;

    private void Start()
    {
        _targetX = transform.position.x;
    }

    /// <summary>
    /// Assign this to an Input Action (Value). 
    /// Handles both discrete Scroll Wheel deltas and continuous Button/Stick input.
    /// </summary>
    public void OnScrollInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.valueType == typeof(Vector2))
            {
                Vector2 val = context.ReadValue<Vector2>();
                float delta = (Mathf.Abs(val.y) > Mathf.Abs(val.x)) ? val.y : val.x;

                // Detect if this is a Scroll Wheel impulse (usually large values like 120/-120)
                // or a continuous Axis/Stick (usually -1 to 1)
                if (Mathf.Abs(delta) > 1.05f)
                {
                    // Treat scroll as an immediate one-time addition to target
                    _targetX += delta * scrollWheelSensitivity;
                    _horizontalInput = 0f; // Reset continuous input to prevent drift
                }
                else
                {
                    // Treat as continuous input (Buttons/Stick)
                    _horizontalInput = delta;
                }
            }
            else if (context.valueType == typeof(float))
            {
                _horizontalInput = context.ReadValue<float>();
            }
        }
        else if (context.canceled)
        {
            _horizontalInput = 0f;
        }
    }

    private void Update()
    {
        // Update boundaries dynamically based on registered level platforms
        if (PlatformLookup.HasInstance && PlatformLookup.Instance.TryGetPlatformBoundaries(out float leftX, out float rightX))
        {
            minX = leftX;
            maxX = rightX;
        }

        // Center on the character initially if we haven't done so yet
        if (!_hasCentredInitial && LevelSelector.HasInstance && PlatformLookup.HasInstance)
        {
            var selected = LevelSelector.Instance.selectedLevel;
            if (selected != null)
            {
                var platform = PlatformLookup.Instance.GetPlatform(selected);
                if (platform != null)
                {
                    float charX = platform.mainCharacterPivot.position.x;
                    _targetX = Mathf.Clamp(charX, minX, maxX);
                    transform.position = new Vector3(_targetX, transform.position.y, transform.position.z);
                    _hasCentredInitial = true;
                }
            }
        }

        // Apply continuous movement (Buttons/Stick)
        if (Mathf.Abs(_horizontalInput) > 0.01f)
        {
            _targetX += _horizontalInput * buttonSpeed * Time.deltaTime;
        }

        // Clamp target position to boundaries
        _targetX = Mathf.Clamp(_targetX, minX, maxX);

        // Smoothly move the camera
        float newX = Mathf.SmoothDamp(transform.position.x, _targetX, ref _currentVelocity, smoothTime);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
