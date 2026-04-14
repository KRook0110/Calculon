using System;
using UnityEngine;

[Serializable]
public struct EnemyData
{
    public GameObject enemyPrefab;
    public MultipleChoiceQuestion question;
}

public interface IEnemySpawner
{
    public void SpawnEnemy(EnemyData data);
}