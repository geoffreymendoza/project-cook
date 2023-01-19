using System.IO;
using UnityEngine;

public static class SaveAndLoadSystem
{
    public static SaveData CurrentSavedata = new SaveData();

    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.sav";
    
    public static bool SaveGame()
    {

        var dir = Application.persistentDataPath + SaveDirectory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);


        string json = JsonUtility.ToJson(CurrentSavedata, true);
        File.WriteAllText(dir + FileName, json);

        GUIUtility.systemCopyBuffer = dir;

        return true;
    }

    public static void LoadGame()
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
            Debug.LogError(message:"save file does not exist!");
        }

        CurrentSavedata = tempData;
    }
}
