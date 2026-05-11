using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LeafRangerAnimationHandler : MonoBehaviour, IPlayerAnimationHandler
{
    public class EventGroup
    {
        public Action Start;
        public Action End;
    }
    public class AtkEventGroup : EventGroup
    {
        public Action Release;
    }

    [SerializeField] private string _idleAnimationName = "idle";
    [SerializeField] private string _atk2AnimationName = "2_atk";
    [SerializeField] private string _atk3AnimationName = "3_atk";

    public EventGroup OnIdle = new EventGroup();
    public AtkEventGroup OnAtk2 = new AtkEventGroup();
    public AtkEventGroup OnAtk3 = new AtkEventGroup();

    private Animator _animator;

    private int _idleHash;
    private int _atk2Hash;
    private int _atk3Hash;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _idleHash = Animator.StringToHash(_idleAnimationName);
        _atk2Hash = Animator.StringToHash(_atk2AnimationName);
        _atk3Hash = Animator.StringToHash(_atk3AnimationName);
    }

    // --- State Triggers ---

    [ContextMenu("Trigger Idle")]
    public void TriggerIdle()
    {
        _animator.Play(_idleHash);
    }

    [ContextMenu("Trigger 2_atk")]
    public void TriggerAtk2()
    {
        _animator.Play(_atk2Hash);
    }

    [ContextMenu("Trigger 3_atk")]
    public void TriggerAtk3()
    {
        _animator.Play(_atk3Hash);
    }

    // --- Idle Event Invocations ---

    public void TriggerIdleStart()
    {
        OnIdle.Start?.Invoke();
    }

    public void TriggerIdleEnd()
    {
        OnIdle.End?.Invoke();
    }

    // --- Attack 2 Event Invocations ---

    public void TriggerAtk2Start()
    {
        OnAtk2.Start?.Invoke();
    }

    public void TriggerAtk2Release()
    {
        OnAtk2.Release?.Invoke();
    }

    public void TriggerAtk2End()
    {
        OnAtk2.End?.Invoke();
    }

    // --- Attack 3 Event Invocations ---

    public void TriggerAtk3Start()
    {
        OnAtk3.Start?.Invoke();
    }

    public void TriggerAtk3Release()
    {
        OnAtk3.Release?.Invoke();
    }

    public void TriggerAtk3End()
    {
        OnAtk3.End?.Invoke();
    }

    public void StartAnimation(string animationName)
    {
        _animator.Play(_idleHash);
        _animator.Play(animationName);
    }
}