using System.Collections;
using UnityEditor.Rendering.Universal;
using UnityEngine;


public class RangedEnemy : Enemy
{
    [Header("Ranged Enemy Info")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _attackBurstCount;
    [SerializeField] private float _attackBurstInterval;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _projectileOrigin;
    

    private bool isAttacking = false;

    void Update()
    {
        var dPos = player.position - transform.position;
        if(dPos.sqrMagnitude > _attackRange * _attackRange)
        {
            transform.position += dPos.normalized * _movementSpeed * Time.deltaTime;
        }
        else
        {
            if(!isAttacking)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    IEnumerator AttackSequence()
    {
        for(int i =0;i< _attackBurstCount;i++)
        {
            var dir =(player.position - transform.position).normalized;
            Instantiate(_projectile,  _projectileOrigin.position, Quaternion.LookRotation(dir));
            yield return new WaitForSeconds(_attackBurstInterval);
        }
    }



}