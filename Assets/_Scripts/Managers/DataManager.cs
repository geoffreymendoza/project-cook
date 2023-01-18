using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private static readonly Dictionary<ItemType, ItemBags> _itemData = new Dictionary<ItemType, ItemBags>();
    private static readonly Dictionary<string, InteractBags> _interactableData =
        new Dictionary<string, InteractBags>();
    private static readonly Dictionary<LevelType, LevelBags> _levelData = new Dictionary<LevelType, LevelBags>();
    private static readonly Dictionary<ItemType, RecipeBags> _recipeData = new Dictionary<ItemType, RecipeBags>();

    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var itemBags = Resources.LoadAll<ItemBags>(Data.ITEM_BAGS_PATH);
        foreach (var ib in itemBags)
        {
            _itemData[ib.Type] = ib;
        }

        var interactBags = Resources.LoadAll<InteractBags>(Data.INTERACT_BAGS_PATH);
        foreach (var interB in interactBags)
        {
            _interactableData[interB.Name] = interB;
        }
        
        var levelBags = Resources.LoadAll<LevelBags>(Data.LEVEL_BAGS_PATH);
        foreach (var lvl in levelBags)
        {
            _levelData[lvl.Type] = lvl;
        }
        
        var recipeBags = Resources.LoadAll<RecipeBags>(Data.RECIPE_BAGS_PATH);
        foreach (var rb in recipeBags)
        {
            _recipeData[rb.Type] = rb;
        }
    }

    public static ItemBags GetItemData(ItemType type)
    {
        return _itemData.TryGetValue(type, out var itemBag) ? itemBag : null;
    }

    public static ItemObject GetItemObject(ItemType type)
    {
        return _itemData.TryGetValue(type, out var itemBag) ? itemBag.Prefab : null;
    }

    public static InteractBags GetInteractData(InteractableType interactableType, ItemType itemType = ItemType.NoInitialItem)
    {
        string keyString = interactableType.ToString();
        if (itemType != ItemType.NoInitialItem)
            keyString += itemType.ToString();
        return _interactableData.TryGetValue(keyString, out var interactBag) ? interactBag : null;
    }

    public static RecipeBags GetRecipeData(ItemType type)
    {
        return _recipeData.TryGetValue(type, out var recipeBag) ? recipeBag : null;
    }

    public static LevelBags GetLevelData(LevelType type)
    {
        return _levelData.TryGetValue(type, out var lvlBag) ? lvlBag : null;
    }
    
}
