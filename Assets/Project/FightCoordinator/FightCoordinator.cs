using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightCoordinator : Singleton<FightCoordinator>
{
    [Serializable]
    public class FightData
    {
        public List<GameObject> enemies;
    }

    [SerializeField]
    private MultipleChoicesHandler _multipleChoicesHandler;
    [SerializeField]
    private PlayerEntity _playerEntity;
    [SerializeField]
    private Transform _spawnOrigin;
    [SerializeField]
    private GameObject _playerProjectilePrefab;
    [SerializeField]
    public List<GameObject> _enemySpawns;

    // Careful : enemy.Damage(damage) updates this set, when a certain enemy dies
    public SortedSet<Enemy> aliveEnemies { get; private set; } = new SortedSet<Enemy>();

    // private Queue<Enemy> _enemyQueue = new Queue<Enemy>();
    private int _currentEnemyIndex = 0;

    public Action OnFinish;

    public void Initialize(FightData fightData)
    {
        _enemySpawns = fightData.enemies;
        if (_enemySpawns.Count <= 0)
        {
            Debug.LogError("No enemies");
            return;
        }
        SpawnEnemy(_enemySpawns[0]);
    }

    void OnEnable()
    {
        _multipleChoicesHandler.OnAnswer += AnswerHandle;
    }


    void OnDisable()
    {
        _multipleChoicesHandler.OnAnswer -= AnswerHandle;
    }

    Enemy FindClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestEnemySqrDist = Mathf.Infinity;
        foreach (Enemy curEnemy in aliveEnemies)
        {
            var sqrDist = (curEnemy.transform.position - _playerEntity.transform.position).sqrMagnitude;

            if (closestEnemySqrDist > sqrDist)
            {
                closestEnemy = curEnemy;
                closestEnemySqrDist = sqrDist;
            }
        }
        return closestEnemy;
    }

    void AnswerHandle(bool isCorrect, QuestionStage stage)
    {
        QuestionGenerator.Instance.UpdateElo(isCorrect);

        if (!isCorrect)
        {
            return;
        }

        PlayerProjectile projectilePrefab = null;
        if (stage != null)
        {
            projectilePrefab = ProjectileMapping.Instance.GetProjectilePrefab(stage.name);
        }
        
        // Fallback to a default if the stage is null or mapping is missing
        if (projectilePrefab == null)
        {
            // Try "Default" or just any mapping if "Default" isn't found
            projectilePrefab = ProjectileMapping.Instance.GetProjectilePrefab("Default");
            
            if (projectilePrefab == null)
            {
                Debug.LogWarning("FightCoordinator: No projectile mapping found for stage or 'Default'. Player cannot attack.");
                return;
            }
        }

        _playerEntity.Attack(FindClosestEnemy(), projectilePrefab.gameObject);
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        if (!enemy) return;
        enemy.OnDeath -= HandleEnemyDeath;
        aliveEnemies.Remove(enemy);

        if (aliveEnemies.Count == 0)
        {
            SpawnNextEnemy();
        }
    }

    private void SpawnNextEnemy()
    {
        if (_currentEnemyIndex + 1 < _enemySpawns.Count)
        {
            _currentEnemyIndex++;
            SpawnEnemy(_enemySpawns[_currentEnemyIndex]);
        }
        else
        {
            Debug.LogWarning("No enemies Left");
            OnFinish?.Invoke();
        }
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        SpawnEnemy(enemyPrefab, _spawnOrigin.position, _spawnOrigin.rotation);
    }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
    {
        var enemyGO = Instantiate(enemyPrefab, position, rotation);
        Enemy enemy = enemyGO.GetComponentInChildren<Enemy>();

        if (enemy == null)
        {
            Debug.LogError($"Not found Enemy Instance from {enemyGO.name}");
            return;
        }

        aliveEnemies.Add(enemy);
        enemy.player = _playerEntity.transform;
        enemy.OnDeath += HandleEnemyDeath;
    }

}