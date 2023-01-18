using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    private static LevelType _currentLevel;
    public static GameObject GetLevelObject(LevelType type)
    {
        var data = DataManager.GetLevelData(type);
        var instance = Object.Instantiate(data.LevelPrefab);
        return instance;
    }

    public static void LevelToLoad(LevelType level)
    {
        _currentLevel = level;
    }

    public static void SpawnLevelToScene()
    {
        GetLevelObject(_currentLevel);
    }
}

public enum LevelType
{
    None = 0,
    Sashimi = 1,
    Soup = 2,
    Salad = 3,
}
