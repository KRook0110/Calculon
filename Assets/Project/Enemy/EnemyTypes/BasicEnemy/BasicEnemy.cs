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
    [SerializeField]
    private float _attackCooldown = 0.5f;
    [SerializeField]
    private float _knockbackDistance = 0;
    [SerializeField]
    private float _knockbackDuration = 1f;

    private float _lastAttack = -Mathf.Infinity;
    private bool _allowMove = true;


    void Update()
    {
        if (_allowMove)
        {
            Vector3 dPos = player.position - transform.position;
            Vector3 totalDisplacement = dPos.normalized * _speed * Time.deltaTime;
            transform.position += totalDisplacement;
        }
    }

    protected virtual void Attack(IDamageable handler)
    {
        if (Time.time > _lastAttack + _attackCooldown)
        {
            handler.Damage(_damage);
            var dir = (player.position - transform.position).normalized;
            StartCoroutine(SelfKnockbackRoutine(-dir));
        }
    }

    // Make sure dir is normalized
    private IEnumerator SelfKnockbackRoutine(Vector3 dir)
    {
        _allowMove = false;
        var t = 0f;
        while(t < _knockbackDuration)
        {
            transform.position +=  dir * Time.deltaTime * _knockbackDistance;

            t += Time.deltaTime;
            yield return null;
        }
        _allowMove = true;
    }

    private void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag(playerTag))
        {
            var handler = collidedObject.GetComponentInChildren<IDamageable>();
            if (handler == null)
            {
                Debug.LogError($"{collidedObject.name} has no type of {typeof(IDamageable).Name}");
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