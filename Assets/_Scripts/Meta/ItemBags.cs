using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectCook/Item Bag", fileName = "Item_Bag", order = 1)]
public class ItemBags : ScriptableObject
{
    [Header("Item Data")]
    public ItemType Type;
    public ItemState State;
    public bool CanSlice;
    public bool CanCook;
    public bool CanWash;
    public float InteractDuration;
    public ItemObject Prefab;
    public GameObject Model;
    public ProcessInfo[] IngredientProcessInfo;
}

[System.Serializable]
public class ProcessInfo
{
    public ItemState State;
    public GameObject Model;
}
