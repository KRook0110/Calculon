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

    public void InitializeFightCoordinator(FightData fightData)
    {
        _enemySpawns = fightData.enemies;
    }

    void Start()
    {
        LevelSelector.Instance.InitializeLevel();

        if (_enemySpawns.Count > 0)
        {
            SpawnEnemy(_enemySpawns[0]);
        }
        else
        {
            Debug.LogError("No enemies");
        }
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

        var projectilePrefab = ProjectileMapping.Instance.GetProjectilePrefab(stage.name).gameObject;
        _playerEntity.Attack(FindClosestEnemy(), projectilePrefab);
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