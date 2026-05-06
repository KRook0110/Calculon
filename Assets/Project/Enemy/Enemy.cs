using System;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int damage);
}

/**
* @brief enemy baseclass
*  update the Kill to change the deathscene, make sure to invoke onDeath()
*/
public class Enemy : MonoBehaviour, IComparable<Enemy>
{
    public struct DamageContext
    {
        public float remainingHealth;
        public int damageDone;
    }

    [SerializeField]
    public Transform player;

    [SerializeField]
    protected string playerTag = "Player";

    [SerializeField]
    protected int maxHealth = 20;

    [SerializeField]
    protected int health = 20;

    public Action<Enemy> OnDeath;
    public Action<DamageContext> OnDamaged;

    public virtual void Kill()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public virtual void Damage(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        health = Math.Clamp(health - amount, 0, maxHealth);

        OnDamaged?.Invoke(new DamageContext
        {
            remainingHealth = health,
            damageDone = amount
        });

        if (health == 0)
        {
            Kill();
        }
    }

    public int CompareTo(Enemy other)
    {
        return GetInstanceID().CompareTo(other.GetInstanceID());
    }
}