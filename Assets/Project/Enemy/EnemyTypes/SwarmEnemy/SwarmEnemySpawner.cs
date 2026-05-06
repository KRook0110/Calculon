using UnityEngine;

public class SwarmEnemySpawner : Enemy
{
    [SerializeField]
    private GameObject _enemySpawnType;
    [SerializeField]
    private int _spawnAmount;
    [SerializeField]
    private ValueRange _rotationRange;

    void Start()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            SpawnEnemy();
        }
        Kill();
    }

    void SpawnEnemy()
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, 0f, _rotationRange.Random());
        FightCoordinator.Instance.SpawnEnemy(_enemySpawnType, transform.position, newRotation);
    }
}
