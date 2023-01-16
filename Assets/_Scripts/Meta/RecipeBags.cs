using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProjectCook/Recipe Bag", fileName = "Recipe_Bag", order = 2)]
public class RecipeBags : ScriptableObject
{
    public ItemType Type;
    public IngredientsInfo[] IngredientsData;
}

[System.Serializable]
public class IngredientsInfo
{
    public ItemType IngredientType;
    public ItemState IngredientState;

    public IngredientsInfo()
    {
        
    }

    public IngredientsInfo(ItemType type, ItemState state)
    {
        IngredientType = type;
        IngredientState = state;
    }
}
