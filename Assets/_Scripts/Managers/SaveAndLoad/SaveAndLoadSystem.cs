using System.IO;
using UnityEngine;

public static class SaveAndLoadSystem
{
    public static SaveData CurrentSavedata = new SaveData();

    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.sav";
    
    public static void SaveGame(SaveData saveData)
    {

        var dir = Application.persistentDataPath + SaveDirectory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);


        // string json = JsonUtility.ToJson(CurrentSavedata, true);
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(dir + FileName, json);

        GUIUtility.systemCopyBuffer = dir;

        Debug.Log("Saved Game");
    }

    public static SaveData LoadGame()
    {
        string fullpath = Application.persistentDataPath + SaveDirectory + FileName;
        SaveData tempData = new SaveData();

        if (File.Exists(fullpath))
        {
            string json = File.ReadAllText(fullpath);
            tempData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            //Debug.LogError(message:"save file does not exist!");
            SaveGame(tempData);
        }

        CurrentSavedata = tempData;
        return CurrentSavedata;
    }

    public static void SaveGame(LevelData levelData)
    {
        int index = (int)levelData.Type - 1;
        Debug.Log(index);
        var existingSaveData = LoadGame();
        if (index > existingSaveData.Levels.Length)
            return;
        existingSaveData.Levels[index] = levelData;
        SaveGame(existingSaveData);
    }
}
