using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    [Serializable]
    public struct PlayerDamageInfo
    {
        public int maxHealth;
        public int remainingHealth;
        public int damageTaken;
    }

    [SerializeField]
    private Transform projectileOrigin;

    [SerializeField]
    private int _maxHealth;

    private int _currentHealth;
    public bool isDead { get; private set; } = false;

    [SerializeField]
    private Animator _animator;

    [Header("Animation Names")]
    [SerializeField] private string _idleAnimationName = "idle";
    [SerializeField] private string _hurtAnimationName = "take_hit";
    [SerializeField] private string _deathAnimationName = "death";

    private int _idleHash;
    private int _hurtHash;
    private int _deathHash;

    public Action<PlayerDamageInfo> OnDamage;
    public Action OnDie;

    void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        Assert.IsNotNull(_animator);

        _idleHash = Animator.StringToHash(_idleAnimationName);
        _hurtHash = Animator.StringToHash(_hurtAnimationName);
        _deathHash = Animator.StringToHash(_deathAnimationName);
    }


    public void Damage(int damage)
    {
        if (isDead)
        {
            return;
        }
        damage = Mathf.Min(_currentHealth, damage);

        _currentHealth -= damage;

        OnDamage?.Invoke(new PlayerDamageInfo
        {
            maxHealth = _maxHealth,
            remainingHealth = _currentHealth,
            damageTaken = damage
        });

        _animator.Play(_hurtHash, 0, 0f);

        if (_currentHealth <= 0)
        {
            isDead = true;
            _animator.Play(_deathHash, 0, 0f);
            OnDie?.Invoke();
        }
    }

    public void Attack(Enemy enemy, GameObject projectilePrefab)
    {
        var projectileComponent = projectilePrefab.GetComponentInChildren<PlayerProjectile>();
        if (projectileComponent == null)
        {
            Debug.LogError($"PlayerProjectile component not found in prefab {projectilePrefab.name}");
            return;
        }

        var animData = AnimationMapping.Instance.GetAnimationData(projectileComponent.type);

        _animator.Play(animData.animationName, 0, 0f);
        StartCoroutine(DelayedSpawn(enemy, projectilePrefab, animData.projectileSpawnDelay));
    }

    private IEnumerator DelayedSpawn(Enemy enemy, GameObject projectilePrefab, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy == null) yield break;

        var projectileGO = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);
        var projectile = projectileGO.GetComponentInChildren<PlayerProjectile>();
        if (projectile == null)
        {
            Debug.LogError($"PlayerProjectile instance not found in {projectilePrefab.name}");
            yield break;
        }
        projectile.target = enemy;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
    }

}
