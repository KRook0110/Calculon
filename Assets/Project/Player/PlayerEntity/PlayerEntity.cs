using System;
using UnityEngine;

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

    public Action<PlayerDamageInfo> OnDamage;
    public Action OnDie;

    public void Damage(int damage)
    {
        damage = Mathf.Min(_currentHealth, damage);

        _currentHealth -= damage;

        OnDamage?.Invoke(new PlayerDamageInfo {
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
        var projectileGO = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);
        var projectile = projectileGO.GetComponentInChildren<PlayerProjectile>();
        if(projectile == null)
        {
            Debug.LogError($"PlayerPorjectile instance not found in {projectilePrefab.name}");
            return;
        }
        projectile.target = enemy;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
    }

}
