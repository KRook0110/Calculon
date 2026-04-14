using UnityEngine;

public interface IDamageable
{
    public void Damage(int damage);
}

/**
* @brief enemy baseclass
*  update the Kill to change the deathscene
*/
public class Enemy : MonoBehaviour
{

    [SerializeField]
    public Transform player;

    [SerializeField]
    protected string playerTag = "Player";

    public virtual void Kill() { Destroy(gameObject); }
}