using System;
using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Enemy target;
    public Action OnHit;

    [Header("Projectile Settings")]
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _killDistance;
    [SerializeField] private int _damage;

    private bool _onDeathSequence = false;

    void Update()
    {
        HandleMovement();
        HandleEnemyDetection();
    }

    private void HandleMovement()
    {
        if (_onDeathSequence) return;

        Vector3 dpos = target.transform.position - transform.position;

        // move forward
        transform.position += transform.up * _moveSpeed * Time.deltaTime;

        // Sideway movement
        float dotProd = Vector3.Dot(dpos, transform.right);
        // check if on the right of projectile
        if (dotProd > 0f)
        {
            transform.rotation *= Quaternion.Euler(0f, 0f, -_turnSpeed * Time.deltaTime);
        }
        // check if on the left of the projectile
        else if (dotProd < 0f)
        {
            transform.rotation *= Quaternion.Euler(0f, 0f, _turnSpeed * Time.deltaTime);
        }
    }

    void HandleEnemyDetection()
    {
        Vector3 dpos = target.transform.position - transform.position;
        if (_killDistance * _killDistance > dpos.sqrMagnitude)
        {
            StartCoroutine(ProjectileDeathSequence());
        }
    }

    IEnumerator ProjectileDeathSequence()
    {
        if (_onDeathSequence)
        {
            yield break;
        }

        _onDeathSequence = true;
        target.Damage(_damage);
        OnHit?.Invoke();
        Destroy(gameObject);
    }

}