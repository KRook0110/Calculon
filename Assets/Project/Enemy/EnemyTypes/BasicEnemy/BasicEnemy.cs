using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;


// Place in root game object on prefab
public class BasicEnemy : Enemy
{

    [Header("Enemy Info")]
    [SerializeField]
    private int _damage = 10;
    [SerializeField]
    private float _speed = 2f;


    void Update()
    {
        Vector3 dPos = player.position - transform.position;
        Vector3 totalDisplacement = dPos.normalized * _speed * Time.deltaTime;
        transform.position += totalDisplacement;
    }

    protected virtual void Attack(IEnemyHandler handler)
    {
        handler.Attack(_damage);
        Kill();
    }

    private void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag(playerTag))
        {
            var handler = collidedObject.GetComponentInChildren<IEnemyHandler>();
            if (handler == null)
            {
                Debug.LogError($"{collidedObject.name} has no type of {typeof(IEnemyHandler).Name}");
                return;
            }
            Attack(handler);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }
}