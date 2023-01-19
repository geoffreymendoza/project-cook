using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public LevelData[] Levels = new LevelData[2];
}

[System.Serializable]
public class LevelData
{
    public LevelType Type = LevelType.None;
    public bool Unlocked = false;
    public int HighScore = 0;

    public LevelData()
    {
        Type = LevelType.None;
        Unlocked = false;
        HighScore = 0;
    }

    public LevelData(LevelType type, bool unlocked)
    {
        Type = type;
        Unlocked = unlocked;
        HighScore = 0;
    }

    public LevelData(LevelType type, int highScore)
    {
        Type = type;
        Unlocked = true;
        HighScore = highScore;
    }
}
