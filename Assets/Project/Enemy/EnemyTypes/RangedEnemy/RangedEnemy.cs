using System.Collections;
using UnityEngine;


public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy Info")]
    [SerializeField] private int _damage;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _attackBurstCount;
    [SerializeField] private float _attackBurstInterval;
    [SerializeField] private float _spread;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Transform projectileOrigin;
    

    private bool _isAttacking = false;
    private float _lastAttack = Mathf.NegativeInfinity;

    void Update()
    {
        var dPos = player.position - transform.position;
        if(dPos.sqrMagnitude > _attackRange * _attackRange)
        {
            transform.position += dPos.normalized * _movementSpeed * Time.deltaTime;
        }
        else
        {
            if(!_isAttacking)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }


    protected virtual IEnumerator AttackSequence()
    {
        if(_lastAttack + _attackInterval > Time.time) yield break;
        _isAttacking = true;
        for(int i =0;i< _attackBurstCount;i++)
        {
            var dir =(player.position - transform.position).normalized;
            // if memory is leaking might need to change with 
            Quaternion spreadAngle = Quaternion.Euler(0f, 0f, Random.Range(-_spread, _spread));
            var go = Instantiate(this.projectile, projectileOrigin.position, projectileOrigin.rotation * spreadAngle); 
            var projectile = go.GetComponentInChildren<IProjectile>();
            projectile.Target = player;
            yield return new WaitForSeconds(_attackBurstInterval);
        }
        _lastAttack = Time.time;
        _isAttacking = false;
    }



}