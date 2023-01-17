using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectCook/UI Bag", fileName = "UI_Bag", order = 4)]
public class UIBags : ScriptableObject
{
    public UIType Type;
    public GameObject Prefab;
}
