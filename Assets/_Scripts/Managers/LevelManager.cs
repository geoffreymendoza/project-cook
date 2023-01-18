using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelManager
{
    private static LevelType _currentLevel;
    public static LevelType GetCurrentLevel() => _currentLevel;
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

    public static float GetCurrentLevelDuration()
    {
        var data = DataManager.GetLevelData(_currentLevel);
        return data.LevelDuration;
    }

    public static void SpawnLevelToScene()
    {
        GetLevelObject(_currentLevel);
    }
}

[System.Serializable]
public class RecipeData
{
    public ItemType RecipeType;
}

public enum LevelType
{
    None = 0,
    Sashimi = 1,
    Soup = 2,
    Salad = 3,
}
