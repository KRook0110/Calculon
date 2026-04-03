using UnityEngine;

struct EnemyData
{
    public GameObject go;
    public MultipleChoiceQuestion question;
}

interface IEnemySpawner
{
    public void SpawnEnemy();
}