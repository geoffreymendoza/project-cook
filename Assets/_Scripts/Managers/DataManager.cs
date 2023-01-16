using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    private static readonly Dictionary<ItemType, ItemBags> _itemData = new Dictionary<ItemType, ItemBags>();
    private static readonly Dictionary<string, InteractBags> _interactableData =
        new Dictionary<string, InteractBags>();

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
}
