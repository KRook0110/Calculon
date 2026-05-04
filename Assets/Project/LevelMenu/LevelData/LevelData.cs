using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Description")]
    public string levelName;

    [Header("Enemies")]
    public List<GameObject> enemies;
}
