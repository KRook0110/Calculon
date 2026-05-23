using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{

    [Header("Level Description")]
    public string levelName;
    public List<LevelData> nextLevels;
    public List<string> unlockQuestionTypes;
    public bool hasTutorial = false;
    public bool resetLevelSelectionToDefault = false;


    [Header("Enemies")]
    public List<GameObject> enemies;

}
