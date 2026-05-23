using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class LevelSelectionAnimationHandler : Singleton<LevelSelectionAnimationHandler>
{
    [SerializeField] private LevelPlatform startingPlatform;
    [SerializeField] private Transform character;
    [SerializeField] private Animator animator;

    [Header("Jumping Animation")]
    [SerializeField, Tooltip("The actual time it takes to jump from one platform to another")]
    private float totalDuration;
    [SerializeField, Tooltip("The original duration of the jump animation clip (used to scale animator speed)")]
    private float animationReferenceDuration = 1f;
    [SerializeField] private string jumpTrigger;
    [SerializeField] private float jumpHeight;
    [SerializeField] private AnimationCurve jumpCurve;

    private LevelPlatform currentPlatform;
    private LevelData _targetLevel;
    private bool _isMoving;

    void Start()
    {
        currentPlatform = startingPlatform;
        Vector3 targetPosition = startingPlatform.mainCharacterPivot.position;
        targetPosition.z = character.position.z;
        character.position = targetPosition;
        _targetLevel = startingPlatform.Level;
    }

    void OnEnable()
    {
        LevelSelector.Instance.OnSelectLevel += OnSelectedLevelHandle;
    }
    void OnDisable()
    {
        if (LevelSelector.HasInstance) LevelSelector.Instance.OnSelectLevel -= OnSelectedLevelHandle;
    }

    void OnSelectedLevelHandle(LevelData data)
    {
        _targetLevel = data;
        
        if (!_isMoving)
        {
            StartCoroutine(MoveToTargetRoutine());
        }
    }

    private IEnumerator MoveToTargetRoutine()
    {
        _isMoving = true;

        while (currentPlatform.Level != _targetLevel)
        {
            LevelPlatform targetPlatform = PlatformLookup.Instance.GetPlatform(_targetLevel);
            if (targetPlatform == null) break;

            List<LevelPlatform> path = PlatformLookup.Instance.GetPath(currentPlatform, targetPlatform);
            
            // If we have a path and the next step is valid
            if (path != null && path.Count > 1)
            {
                LevelPlatform nextStep = path[1];
                yield return StartCoroutine(JumpRoutine(currentPlatform.mainCharacterPivot.position, nextStep.mainCharacterPivot.position));
                currentPlatform = nextStep;
            }
            else
            {
                // No path found to the target
                break;
            }
        }

        _isMoving = false;
    }

    IEnumerator JumpRoutine(Vector2 startingPosition, Vector2 endingPosition)
    {
        float t = 0f;
        float originalZ = character.position.z;
        
        // Handle character flipping
        float xDiff = endingPosition.x - startingPosition.x;
        if (Mathf.Abs(xDiff) > 0.01f)
        {
            Vector3 localScale = character.localScale;
            localScale.x = Mathf.Abs(localScale.x) * (xDiff > 0 ? 1 : -1);
            character.localScale = localScale;
        }

        // Match animator speed to the duration
        if (totalDuration > 0)
        {
            animator.speed = animationReferenceDuration / totalDuration;
        }
        
        animator.SetTrigger(jumpTrigger);
        while (totalDuration > t)
        {
            float tRatio = t / totalDuration;
            float jumpDisplacement = jumpCurve.Evaluate(tRatio) * jumpHeight;
            Vector2 lerpedPos = Vector2.Lerp(startingPosition, endingPosition, tRatio) + Vector2.up * jumpDisplacement;
            character.position = new Vector3(lerpedPos.x, lerpedPos.y, originalZ);
            t += Time.deltaTime;

            yield return null;
        }
        character.position = new Vector3(endingPosition.x, endingPosition.y, originalZ);
    }

}
