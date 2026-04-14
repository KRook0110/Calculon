using System;
using NUnit.Framework;
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
    public void Attack(Enemy enemy)
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
    }

}
