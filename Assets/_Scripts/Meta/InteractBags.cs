using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectCook/Interact Bag", fileName = "Interact_Bag", order = 0)]
public class InteractBags : ScriptableObject
{
    [Header("Interact Data")] 
    public InteractableType Type;
    public InteractObject Prefab;
    public Material Material;
    public Material MaterialEmissive;
    
    [Header("Item placed on table")]
    public ItemType InitialItem;
    public string Name
    {
        get
        {
            var interactName = Type.ToString();
            if (InitialItem != ItemType.NoInitialItem)
                interactName += InitialItem.ToString();
            return interactName;  
        }
    }
}

