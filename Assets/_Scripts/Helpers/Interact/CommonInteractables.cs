using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make it abstract class
[System.Serializable]
public class Interactable : IInteractHandler, IPickupHandler
{
    public event System.Action OnRemoveInteractObject;
    public InteractableType Type { get; protected set; } = InteractableType.Unassigned;
    //only useful for plates in sink and counter table
    public int StacksCount { get; protected set; } = 0; 
    public bool HasItem => ItemObj != null;
    public ItemObject ItemObj { get; protected set; }
    protected Transform _itemPlacement;
    public bool CanTriggerInteractInput { protected set; get; } = false;

    private bool _init = false;
    public void Initialize(ItemObject itemObject, Transform itemPlacement)
    {
        if(_init) return;
        _init = true;
        _itemPlacement = itemPlacement;
        ItemObj = itemObject;
    }

    public virtual bool PickupItem(ItemObject itemObject)
    {
        ItemObj = itemObject;
        var itemTransform = ItemObj.transform;
        itemTransform.SetParent(_itemPlacement, false);
        itemTransform.position = _itemPlacement.position;
        return true;
    }

    public virtual void DropItem()
    {
        ItemObj = null;
    }

    public virtual void InvokeRemoveInteractObject()
    {
        OnRemoveInteractObject?.Invoke();
    }
}

public class KitchenTable : Interactable
{
    public KitchenTable()
    {
        StacksCount = 0;
        Type = InteractableType.KitchenTable;
    }
}

public class IngredientStockTable : Interactable
{
    public IngredientStockTable()
    {
        StacksCount = 0;
        Type = InteractableType.IngredientStockTable;
    }

    public override bool PickupItem(ItemObject itemObject)
    {
        return true;
    }
    
    public override void DropItem()
    {
        //TODO object pooling
        var itemData = DataManager.GetItemData(ItemObj.GetItem().Type);
        var itemCopy = Object.Instantiate(itemData.Prefab);
        itemCopy.Initialize(itemData);
        ItemObj = itemCopy;
        Transform itemTransform;
        (itemTransform = ItemObj.transform).SetParent(_itemPlacement.transform, false);
        itemTransform.position = _itemPlacement.position;
    }
}

public class TrashBin : Interactable
{
    public TrashBin()
    {
        StacksCount = 0;
        Type = InteractableType.TrashBin;
    }
    
    public override bool PickupItem(ItemObject itemObject)
    {
        var itemType = itemObject.GetItem().Type;
        if (itemType is ItemType.Plate or ItemType.CookContainer)
        {
            //TODO only the contents of plate or hotpot should be throw away
            return false;
        }
        Object.Destroy(itemObject.gameObject);
        return false;
    }
}

public class ChoppingTable : Interactable
{
    public ChoppingTable()
    {
        StacksCount = 0;
        Type = InteractableType.ChoppingTable;
        CanTriggerInteractInput = true;
    }
    
    //TODO can only accept raw ingredients
    public override bool PickupItem(ItemObject itemObject)
    {
        var currentState = itemObject.GetItem().State;
        if (currentState != ItemState.Raw && currentState != ItemState.Sliced)
            return false;
        return base.PickupItem(itemObject);
    }
}

public class CookingTable : Interactable
{
    public CookingTable()
    {
        StacksCount = 0;
        Type = InteractableType.CookingTable;
    }
    
    //TODO only hotpot is accepted
    public override bool PickupItem(ItemObject itemObject)
    {
        var itemType = itemObject.GetItem().Type;
        return itemType == ItemType.CookContainer && base.PickupItem(itemObject);
    }
}

public class InvisibleTable : Interactable
{
    public InvisibleTable()
    {
        StacksCount = 0;
        Type = InteractableType.InvisibleTable;
    }

    public override void DropItem()
    {
        base.DropItem();
        //TODO move it to object pool
        InvokeRemoveInteractObject();
    }
}

public class Sink : Interactable
{
    public Sink()
    {
        StacksCount = 0;
        Type = InteractableType.InvisibleTable;
    }

    public override void DropItem()
    {
        base.DropItem();
        //TODO move it to object pool
        InvokeRemoveInteractObject();
    }
}

public interface IInteractHandler
{
    InteractableType Type { get; }
    int StacksCount { get; }
}

public interface IPickupHandler
{
    bool HasItem { get; }
    ItemObject ItemObj { get; }
    bool PickupItem(ItemObject itemObject);
    void DropItem();
}

public enum InteractableType
{
    Unassigned,
    KitchenTable,
    IngredientStockTable,
    TrashBin,
    ChoppingTable,
    CookingTable,
    InvisibleTable,
    Sink
}