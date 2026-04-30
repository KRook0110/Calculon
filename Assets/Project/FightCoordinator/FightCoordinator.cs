using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FightCoordinator : MonoBehaviour, IEnemySpawner
{

    [SerializeField]
    private MultipleChoicesHandler _multipleChoicesHandler;
    [SerializeField]
    private PlayerEntity _playerEntity;
    [SerializeField]
    private Transform _spawnOrigin;
    [SerializeField]
    private GameObject _playerProjectilePrefab;


    private Queue<Enemy> _enemyQueue = new Queue<Enemy>();

    void OnEnable()
    {
        _multipleChoicesHandler.OnAnswer += AnswerHandle;
    }
    void OnDisable()
    {
        _multipleChoicesHandler.OnAnswer -= AnswerHandle;
    }

    void AnswerHandle(bool isCorrect)
    {
        if (!isCorrect)
        {
            return;
        }

        _playerEntity.Attack(_enemyQueue.Peek(), _playerProjectilePrefab);
    }

    private void HandleEnemyDeath()
    {
        if (_enemyQueue.Count > 0)
        {
            Enemy deadEnemy = _enemyQueue.Dequeue();
            deadEnemy.OnDeath -= HandleEnemyDeath;
        }
    }

    public void SpawnEnemy(EnemyData data)
    {
        // Spawn the actual enemy
        var enemyGO = Instantiate(data.enemyPrefab, _spawnOrigin.position, _spawnOrigin.rotation);
        Enemy enemy = enemyGO.GetComponentInChildren<Enemy>();
        if (enemy == null)
        {
            Debug.LogError($"Not found Enemy Instance from {enemyGO.name}");
            return;
        }

        enemy.player = _playerEntity.transform;
        enemy.OnDeath += HandleEnemyDeath;

        // Add the Question
        _multipleChoicesHandler.AddQuestion(data.question);

        // Save the order when to kill the enemy
        _enemyQueue.Enqueue(enemy);
    }
}