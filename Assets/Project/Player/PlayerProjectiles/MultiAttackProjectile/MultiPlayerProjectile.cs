using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// The idea is that instead of this being the actual projectile, 
// it is actually just a spawner Spawning multiple projectiles
public class MultiPlayerProjectile : PlayerProjectile
{
    [SerializeField]
    private int _amount;
    [SerializeField]
    private PlayerProjectile _projectile;
    [Header("Aesthetics")]
    [SerializeField]
    private ValueRange _rotationRange;

    void Start()
    {
        if (!FightCoordinator.HasInstance)
        {
            Debug.LogError("FightCoordinator Instance not found!");
            Destroy(gameObject);
            return;
        }

        var sortedEnemies = FightCoordinator.Instance.aliveEnemies
            .Where(e => e != null)
            .OrderBy(e => (e.transform.position - transform.position).sqrMagnitude)
            .ToList();

        if (sortedEnemies.Count == 0)
        {
            for (int i = 0; i < _amount; i++)
            {
                SpawnProjectile(null);
            }
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < _amount; i++)
        {
            // Cycle through sorted enemies if _amount > enemies.Count
            Enemy targetEnemy = sortedEnemies[i % sortedEnemies.Count];
            SpawnProjectile(targetEnemy);
        }

        Destroy(gameObject);
    }

    void SpawnProjectile(Enemy target)
    {
        float randomRotation = _rotationRange.Random();
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, randomRotation);

        GameObject projectileGO = Instantiate(_projectile.gameObject, transform.position, rotation);
        PlayerProjectile projectile = projectileGO.GetComponent<PlayerProjectile>();
        if (!projectile)
        {
            Debug.LogError($"There is no PlayerProjectile Script in {_projectile.name}");
            return;
        }

        projectile.target = target;
        projectile.OnHit += OnHit;
    }
}
