using System.Collections.Generic;
using UnityEngine;

public class AOEPlayerProjectile : BasicPlayerProjectile
{
    [Header("AOE Settings")]
    [SerializeField]
    private int _aoeDamage;
    [SerializeField]
    private float _aoeRadius;
    [Header("AOE Visuals")]
    [SerializeField]
    private AOEVisualEffect _explosionVFXPrefab;
    [SerializeField]
    private Color _explosionColor;
    [SerializeField]
    private float _explosionDuration;
    [SerializeField]
    private float _explosionRadius;

    public override void DamageHandle()
    {
        // handles the main damage to the target
        base.DamageHandle();

        // handles the aoe damage to all in radius
        List<Enemy> allEnemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in FightCoordinator.Instance.aliveEnemies)
        {
            var dpos = enemy.transform.position - transform.position;
            if (dpos.sqrMagnitude <= _aoeRadius * _aoeRadius && enemy != target)
            {
                allEnemiesInRange.Add(enemy);
            }
        }

        foreach (var enemy in allEnemiesInRange)
        {
            if (enemy == null) continue;
            enemy.Damage(_aoeDamage);
        }

        SpawnAOEVFX();
    }

    void SpawnAOEVFX()
    {
        AOEVisualEffect effect = Instantiate(_explosionVFXPrefab, transform.position, transform.rotation);
        effect.Initialize(_explosionRadius, _explosionColor, _explosionDuration);
    }

}
