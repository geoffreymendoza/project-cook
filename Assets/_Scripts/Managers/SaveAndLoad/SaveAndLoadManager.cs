using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{
   public void SaveGame()
    {
        
        SaveAndLoadSystem.SaveGame();
    }

    public void LoadGame()
    {
        
        SaveAndLoadSystem.LoadGame();
        
    }

}
