using System.Collections.Generic;
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


    private Queue<Enemy> enemyQueue = new Queue<Enemy>();

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
        if(!isCorrect)
        {
            return;
        }

        _playerEntity.Attack(enemyQueue.Peek(), _playerProjectilePrefab);

        enemyQueue.Dequeue();
    }


    public void SpawnEnemy(EnemyData data)
    {
        // Spawn the actual enemy
        var enemyGO = Instantiate(data.enemyPrefab, _spawnOrigin.position, _spawnOrigin.rotation);
        Enemy enemy = enemyGO.GetComponentInChildren<Enemy>();
        enemy.player = _playerEntity.transform;

        if (enemy == null)
        {
            Debug.LogError($"Not found Enemy Instance from {enemyGO.name}");
            return;
        }

        // Add the Question
        _multipleChoicesHandler.AddQuestion(data.question);

        // Save the order when to kill the enemy
        enemyQueue.Enqueue(enemy);
    }
}