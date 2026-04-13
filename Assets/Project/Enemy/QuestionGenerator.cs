using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGenerator : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int enemiesPerWave = 10;
    public int currentWave;
    public int handicap = 1;
    public List<EnemyType> materialList = new List<EnemyType> { (EnemyType)1 };
    public Difficulty maxDifficulty = (Difficulty)5;
    public IEnemySpawner enemySpawner;
    public EnemyData data;

    [Header("Adaptive Timing")]
    [Tooltip("Wait time when recent enemies were mostly Easy")]
    public List<float> minSpawnTime = new List<float> { 2.0f , 1.5f , 1.0f }; 
    [Tooltip("Wait time when recent enemies were mostly Hard")]
    public List<float> maxSpawnTime = new List<float> { 5.0f , 4.0f , 3.5f }; 
    
    // Difficulty handler
    private Queue<Difficulty> recentDifficulties = new Queue<Difficulty>();
    private int historySize = 3;

    void Start()
    {
        StartCoroutine(GenerateQuestion());
    }

    private IEnumerator GenerateQuestion()
    {
        for (currentWave = 0; currentWave < 3; currentWave++)
        {
            Debug.Log("STARTING WAVE " + (currentWave + 1));
            // enemy type generation
            List<EnemyType> enemies = new List<EnemyType>();
            while (enemies.Count < enemiesPerWave)
            {
                foreach (EnemyType e in materialList)
                {
                    enemies.Add(e);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyType curr = enemies[i];

                int randomIndex = Random.Range(i, enemies.Count);

                enemies[i] = enemies[randomIndex];
                enemies[randomIndex] = curr;
            }

            // enemy spawning
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Difficulty chosenDifficulty = (Difficulty)(Random.Range(handicap, (int)maxDifficulty));

                // spawn enemy

                // for testing
                // GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                // EnemyData enemyScript = spawnedEnemy.GetComponent<EnemyData>();
                // if (enemyScript != null)
                // {
                //     enemyScript.SetupProblem(enemies[i], chosenDifficulty);
                // }
                // else
                // {
                //     Debug.LogError("The enemy prefab is missing the EnemyData script!");
                // }

                recentDifficulties.Enqueue(chosenDifficulty);
                if (recentDifficulties.Count > historySize)
                {
                    recentDifficulties.Dequeue();
                }

                // spawn enemy
                Debug.Log("GENERATED " + chosenDifficulty + " ENEMY");

                float waitTime = CalculateWaitTime();
                Debug.Log("PEND " + waitTime);
                yield return new WaitForSeconds(waitTime);
            }

            while (recentDifficulties.Count > 0)
            {
                recentDifficulties.Dequeue();
            }
            
        }
    }

    private float CalculateWaitTime()
    {
        // default waiting time
        if (recentDifficulties.Count == 0) return (minSpawnTime[currentWave] + maxSpawnTime[currentWave]) / 2f;

        // percentage based calculation of average
        float totalDifficulty = 0f;
        foreach (Difficulty d in recentDifficulties)
        {
            totalDifficulty += (float)d - handicap;
        }
        float averageDifficulty = totalDifficulty / recentDifficulties.Count;
        float difficultyPercentage = (averageDifficulty - 1f) / (float)maxDifficulty;

        return Mathf.Lerp(minSpawnTime[currentWave], maxSpawnTime[currentWave], difficultyPercentage);
    }
}