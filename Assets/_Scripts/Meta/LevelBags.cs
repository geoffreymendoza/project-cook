using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectCook/Level Bag", fileName = "Level_Bag", order = 5)]
public class LevelBags : ScriptableObject
{
    public LevelType Type;
    public GameObject LevelPrefab;
    public float LevelDuration;
}
