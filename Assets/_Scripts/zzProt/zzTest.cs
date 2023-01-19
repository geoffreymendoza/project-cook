using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class zzTest : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private InteractableType _interactableType;

    [SerializeField] private LevelType _level;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Test Item")]
    public void TestItem()
    {
        var itemBag = DataManager.GetItemData(_itemType);
        Debug.Log(itemBag.name);
        var itemObj = Instantiate(itemBag.Prefab);
        itemObj.Initialize(itemBag);
    }

    [ContextMenu("Test Interact")]
    public void TestInteract()
    {
        // var interactBag = DataManager.GetInteractData(_interactableType);
        // Debug.Log(interactBag.name);
        // var interactObj = Instantiate(interactBag.Prefab);
        // interactObj.Initialize(interactBag);
    }
    
    [ContextMenu("TestLevel")]
    public void TestLevel()
    {
        LevelManager.LevelToLoad(_level);
        SceneManager.LoadScene(Data.GAME_SCENE);
    }

    [ContextMenu("Test Save")]
    public void TestSave()
    {
        LevelData levelData = new LevelData(LevelType.Sashimi, true);
        SaveAndLoadSystem.SaveGame(levelData);
    }
}
