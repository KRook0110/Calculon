
// using System.Collections;
// using UnityEngine;

// public class QuestionGeneratorExample : MonoBehaviour
// {
//     public IEnemySpawner enemySpawner;
//     public EnemyData data;
    

//     IEnumerator GenerateQuestion()
//     {
//         for(int i =0;i < 10;i++)
//         {
//             enemySpawner.SpawnEnemy(data);
//             yield return new WaitForSeconds(1);
//         }
        
//     }
//     void Start()
//     {
//         StartCoroutine(GenerateQuestion());
//     }

// }