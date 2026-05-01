using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightCoordinator : MonoBehaviour
{

    [SerializeField]
    private MultipleChoicesHandler _multipleChoicesHandler;
    [SerializeField]
    private PlayerEntity _playerEntity;
    [SerializeField]
    private Transform _spawnOrigin;
    [SerializeField]
    private GameObject _playerProjectilePrefab;
    [SerializeField]
    private Enemy[] _enemySpawns;

    // private Queue<Enemy> _enemyQueue = new Queue<Enemy>();
    private int _currentEnemyIndex = 0;
    private SortedSet<Enemy> _aliveEnemies = new SortedSet<Enemy>();

    public Action OnFinish;

    void Start()
    {
        if(_enemySpawns.Length > 0)
        {
            SpawnEnemy(_enemySpawns[0].gameObject);
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
        foreach (Enemy curEnemy in _aliveEnemies)
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

    void AnswerHandle(bool isCorrect)
    {
        QuestionGenerator.Instance.UpdateElo(isCorrect);

        if (!isCorrect)
        {
            return;
        }

        _playerEntity.Attack(FindClosestEnemy(), _playerProjectilePrefab);
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        if (!enemy) return;
        enemy.OnDeath -= HandleEnemyDeath;
        _aliveEnemies.Remove(enemy);

        SpawnNextEnemy();
    }

    private void SpawnNextEnemy()
    {
        if (_currentEnemyIndex + 1 < _enemySpawns.Length)
        {
            _currentEnemyIndex++;
            SpawnEnemy(_enemySpawns[_currentEnemyIndex].gameObject);
        }
        else
        {
            Debug.LogWarning("No enemies Left");
            OnFinish?.Invoke();
        }
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        var enemyGO = Instantiate(enemyPrefab, _spawnOrigin.position, _spawnOrigin.rotation);
        Enemy enemy = enemyGO.GetComponentInChildren<Enemy>();

        if (enemy == null)
        {
            Debug.LogError($"Not found Enemy Instance from {enemyGO.name}");
            return;
        }

        _aliveEnemies.Add(enemy);
        enemy.player = _playerEntity.transform;
        enemy.OnDeath += HandleEnemyDeath;
    }
}