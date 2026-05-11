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

    public IPlayerAnimationHandler _animationHandler;
    public Action<PlayerDamageInfo> OnDamage;
    public Action OnDie;

    void Awake()
    {
        _animationHandler = GetComponentInChildren<IPlayerAnimationHandler>();
        Assert.IsNotNull(_animationHandler);
    }


    public void Damage(int damage)
    {
        damage = Mathf.Min(_currentHealth, damage);

        _currentHealth -= damage;

        OnDamage?.Invoke(new PlayerDamageInfo
        {
            maxHealth = _maxHealth,
            remainingHealth = _currentHealth,
            damageTaken = damage
        });

        if (_currentHealth <= 0)
        {
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
        
        _animationHandler.StartAnimation(animData.animationName);
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
