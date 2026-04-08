using UnityEngine;

public interface IEnemyHandler
{
    public void Attack(int damage);
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