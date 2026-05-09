
using System;
using System.Collections;
using UnityEngine;

public class BasicPlayerProjectile : PlayerProjectile
{
    [Header("Projectile Settings")]
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _killDistance;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField]private float _trackingDelay = 0.5f;
    private float _spawnTime;
    private Vector3 _dpos;

    private bool _onDeathSequence = false;


    protected virtual void Start()
    {
        _spawnTime = Time.time;
    }

    void UpdateDPos()
    {
        _dpos = Vector2.right;
        if (target)
        {
            _dpos = target.transform.position - transform.position;
        }
    }

    void HandleLifetime()
    {
        if(_spawnTime + _lifetime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateDPos();
        HandleLifetime();
        HandleMovement();
        HandleEnemyDetection();
    }

    private void HandleMovement()
    {
        if (_onDeathSequence) return;

        // move forward
        transform.position += transform.up * _moveSpeed * Time.deltaTime;

        if(_trackingDelay + _spawnTime  < Time.time) HandleSidewayMovement();
    }
    void HandleSidewayMovement()
    {

        // Sideway movement
        float dotProd = Vector3.Dot(_dpos, transform.right);
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
        if (_killDistance * _killDistance > _dpos.sqrMagnitude)
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
        DamageHandle();
        OnHit?.Invoke(this);
        Destroy(gameObject);
    }

    public virtual void DamageHandle()
    {
        target.Damage(_damage);
    }


}