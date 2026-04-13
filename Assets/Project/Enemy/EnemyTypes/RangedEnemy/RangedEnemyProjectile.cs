using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour, IProjectile
{

    public Transform Target {get; set;}

    [Header("Projectile Settings")]

    [SerializeField] private int _damage;
    [SerializeField] private float _projectileLifeTime;

    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _hitDistance;

    public event Action<GameObject> OnHit;

    private float _lifeTime = 0f;
    void Update()
    {
        _lifeTime += Time.deltaTime;
        if(_lifeTime < _projectileLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // go forward
        transform.position  += transform.up * _speed * Time.deltaTime;

        // turn a little towards target
        Vector3 dPos = Target.position - transform.position;
        float dotProduct = Vector3.Dot(dPos.normalized, transform.right);
        if(dotProduct > 0f)
        {
            transform.Rotate(0f, 0f, -_turnSpeed * Time.deltaTime);
        }
        else if(dotProduct < 0f)
        {
            transform.Rotate(0f, 0f, _turnSpeed * Time.deltaTime);
        }

        if(dPos.sqrMagnitude < _hitDistance * _hitDistance) 
        {
            OnHit?.Invoke(Target.gameObject);

            Debug.Log($"hit {Target.name} {dPos.sqrMagnitude}");
            var handler = Target.GetComponentInChildren<IEnemyHandler>();
            handler.Attack(_damage);

            Destroy(gameObject);
            return;
        }

    }
}