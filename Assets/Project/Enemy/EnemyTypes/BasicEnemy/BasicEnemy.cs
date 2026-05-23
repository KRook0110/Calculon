using System;
using System.Collections;
using System.Runtime.InteropServices;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;


// Place in root game object on prefab
public class BasicEnemy : Enemy
{
    [Header("Enemy Info")]
    [SerializeField]
    private int _damage = 10;
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _attackCooldown = 0.5f;
    [SerializeField]
    private float _attackKnockbackDistance = 0;
    [SerializeField]
    private float _attackKnockbackDuration = 1f;

    [Header("Movement Settings")]
    [SerializeField]
    private bool _onlyMoveForwards = false;
    [SerializeField]
    private float _rotationTrackingSpeed = 10f;
    [SerializeField]
    private float _trackingDelay = 2f;

    [Header("Attack settings")]
    [SerializeField]
    private float _attackDelay;

    [Header("Attack Animation Settings")]
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    private string _atkAnimationName = "Attack1";

    [Header("Running Animation Settings")]
    [SerializeField]
    private string _isRunningAnimationParamName = "isRunning";

    [Header("Hurt Animation Settings")]
    [SerializeField]
    private string _hurtAnimationName = "Take Hit";
    [SerializeField]
    private float _hurtStunTime = 1f;
    [SerializeField]
    private float _hurtKnockbackDistance = 1f;
    [SerializeField]
    private float _hurtKnockbackDuration = 1f;

    [Header("Idle Animation Settings")]
    [SerializeField]
    private string _idleAnimationName = "Idle";

    [Header("Death Sequence Settings")]
    [SerializeField]
    private string _deathAnimationName = "Death";

    private float _lastAttack = -Mathf.Infinity;
    private bool _allowMove = true;
    private float _spawnTime;
    private bool _isHurting = false;
    private IDamageable _touchedEntity = null;
    private bool _isDead = false;

    private int _isRunningAnimationParamHash;
    private int _atkAnimationNameHash;
    private int _hurtAnimationNameHash;
    private int _idleAnimationNameHash;
    private int _deathAnimationNameHash;
    private PlayerEntity playerEntity;


    protected virtual void Awake()
    {
        _atkAnimationNameHash = Animator.StringToHash(_atkAnimationName);
        _isRunningAnimationParamHash = Animator.StringToHash(_isRunningAnimationParamName);
        _hurtAnimationNameHash = Animator.StringToHash(_hurtAnimationName);
        _idleAnimationNameHash = Animator.StringToHash(_idleAnimationName);
        _deathAnimationNameHash = Animator.StringToHash(_deathAnimationName);
    }

    protected virtual void Start()
    {
        Debug.Log($"Player : {player}");
        playerEntity = player.GetComponent<PlayerEntity>();
        _spawnTime = Time.time;
    }
    protected virtual bool IsAllowedToMove()
    {
        return _allowMove && _touchedEntity == null && !_isHurting && !_isDead;
    }

    void Update()
    {
        if (IsAllowedToMove())
        {
            if (_onlyMoveForwards)
            {
                MoveForward();
            }
            else
            {
                MoveDirectly();
            }
            _animator.SetBool(_isRunningAnimationParamHash, true);
        }
        else
        {
            _animator.SetBool(_isRunningAnimationParamHash, false);
        }

        if (_touchedEntity != null && !_isDead)
        {
            Attack(_touchedEntity);
        }
    }

    void HandleHurt(DamageContext damageContext)
    {
        StartCoroutine(HurtRoutine());
    }

    public override void Kill()
    {
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        _isDead = true;
        _animator.Play(_deathAnimationNameHash);
        yield return new WaitForSeconds(1f);
        base.Kill();
    }

    IEnumerator HurtRoutine()
    {
        _isHurting = true;
        _animator.Play(_hurtAnimationNameHash);
        Vector3 dir = transform.position - player.position;
        StartCoroutine(SelfKnockbackRoutine(dir, _hurtKnockbackDistance, _hurtKnockbackDuration));

        yield return new WaitForSeconds(_hurtStunTime);

        _isHurting = false;
        if(!_isDead) _animator.Play(_idleAnimationNameHash);

    }

    void OnEnable()
    {
        OnDamaged += HandleHurt;
    }

    void OnDisable()
    {
        OnDamaged -= HandleHurt;
    }

    void MoveForward()
    {
        transform.position += -transform.right * Time.deltaTime * _speed;
        if (Time.time > _spawnTime + _trackingDelay)
        {
            RotationTracking(_rotationTrackingSpeed, player.position);
        }
    }

    void RotationTracking(float speed, Vector3 position)
    {
        Vector3 dPos = position - transform.position;
        float dotProduct = Vector2.Dot(-transform.up, dPos.normalized);
        transform.rotation *= Quaternion.Euler(0f, 0f, (dotProduct > 0f ? speed : -speed) * Time.deltaTime);
    }

    // Moving without rotating the player.
    void MoveDirectly()
    {
        Vector3 dPos = player.position - transform.position;
        Vector3 totalDisplacement = dPos.normalized * _speed * Time.deltaTime;
        transform.position += totalDisplacement;
    }

    protected virtual void Attack(IDamageable handler)
    {
        if (Time.time > _lastAttack + _attackCooldown && !playerEntity.isDead)
        {
            StartCoroutine(AttackRoutine(handler));
        }
    }
    IEnumerator AttackRoutine(IDamageable handler)
    {
        _lastAttack = Time.time;
        _animator.Play(_atkAnimationNameHash);
        yield return new WaitForSeconds(_attackDelay);
        handler.Damage(_damage);
        var dir = (player.position - transform.position).normalized;
        yield return SelfKnockbackRoutine(-dir, _attackKnockbackDistance, _attackKnockbackDuration);
    }

    // Make sure dir is normalized
    private IEnumerator SelfKnockbackRoutine(Vector3 dir, float knockbackDistance, float knockbackDuration)
    {
        _allowMove = false;
        var t = 0f;
        while (t < knockbackDuration)
        {
            transform.position += dir * Time.deltaTime * knockbackDistance;

            t += Time.deltaTime;
            yield return null;
        }
        _allowMove = true;
    }

    private void HandleCollisionEnter(GameObject collidedObject)
    {
        if (collidedObject.CompareTag(playerTag))
        {
            var handler = collidedObject.GetComponentInChildren<IDamageable>();
            if (handler == null)
            {
                Debug.LogError($"{collidedObject.name} has no type of {typeof(IDamageable).Name}");
                return;
            }
            _touchedEntity = handler;
        }
    }

    private void HandleCollisionExit(GameObject collidedObject)
    {
        if (collidedObject.CompareTag(playerTag))
        {
            var handler = collidedObject.GetComponentInChildren<IDamageable>();
            if (handler == null)
            {
                Debug.LogError($"{collidedObject.name} has no type of {typeof(IDamageable).Name}");
                return;
            }
            _touchedEntity = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionEnter(collision.gameObject);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        HandleCollisionExit(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollisionEnter(collision.gameObject);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        HandleCollisionExit(collision.gameObject);
    }
}