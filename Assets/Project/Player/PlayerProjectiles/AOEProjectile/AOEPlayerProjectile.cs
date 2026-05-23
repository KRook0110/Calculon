using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEPlayerProjectile : BasicPlayerProjectile
{
    [Header("AOE Settings")]
    [SerializeField]
    private int _aoeDamage;
    [SerializeField]
    private float _aoeRadius;
    [SerializeField]
    private float _damageDelay = 0.4f;
    [Header("AOE Visuals")]
    [SerializeField]
    private GameObject _explosionVFXPrefab;

    public override void DamageHandle()
    {
        // handles the aoe damage to all in radius
        List<Enemy> allEnemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in FightCoordinator.Instance.aliveEnemies)
        {
            var dpos = enemy.transform.position - transform.position;
            if (dpos.sqrMagnitude <= _aoeRadius * _aoeRadius)
            {
                allEnemiesInRange.Add(enemy);
            }
        }

        foreach (var enemy in allEnemiesInRange)
        {
            if (enemy == null) continue;
            Debug.Log($"Damaging {enemy.name} {_damageDelay}");
            enemy.StartCoroutine(DamageDelayRoutine(enemy));
        }

        SpawnAOEVFX();
    }

    IEnumerator DamageDelayRoutine(Enemy enemy)
    {
        Debug.Log($"Trigger Damage {enemy.name}");
        yield return new WaitForSeconds(_damageDelay);
        enemy.Damage(_aoeDamage);
        Debug.Log($"After Damage {enemy.name}");
    }

    void SpawnAOEVFX()
    {
        Instantiate(_explosionVFXPrefab, transform.position, Quaternion.identity);
    }

}
