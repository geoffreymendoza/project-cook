using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

//TODO make it abstract class
[System.Serializable]
public class Interactable : IInteractHandler, IPickupHandler
{
    public event Action OnRemoveInteractObject;
    public event Action<ItemType> OnSpawnItemObject;
    public InteractableType Type { get; protected set; } = InteractableType.Unassigned;
    //only useful for plates in sink and counter table
    public int StacksCount { get; protected set; } = 0; 
    public bool HasItem => ItemObj != null;
    public ItemObject ItemObj { get; protected set; }
    protected Transform _itemPlacement;
    public bool CanTriggerInteractInput { protected set; get; } = false;

    private bool _init = false;
    public virtual void Initialize(ItemObject itemObject, Transform itemPlacement)
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

    public void ActivateInteractState(Item item)
    {
        //dud
    }

    public virtual void SpawnItem(ItemType type)
    {
        if (StacksCount <= 0)
        {
            SpawnFromData(type);
        }
        StacksCount++;
    }

    protected void SpawnFromData(ItemType type)
    {
        var itemData = DataManager.GetItemData(type);
        var itemCopy = Object.Instantiate(itemData.Prefab);
        itemCopy.Initialize(itemData);
        Transform itemTransform;
        (itemTransform = itemCopy.transform).SetParent(_itemPlacement.transform, false);
        itemTransform.position = _itemPlacement.position;
        ItemObj = itemCopy;
    }

    public virtual void InvokeRemoveInteractObject()
    {
        OnRemoveInteractObject?.Invoke();
    }

    public virtual void InvokeOnSpawnItemObject(ItemType type)
    {
        OnSpawnItemObject?.Invoke(type);
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
        StacksCount = 999;
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
        if (itemType is ItemType.Plate or ItemType.CookContainer or ItemType.DirtyPlate)
        {
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
        Type = InteractableType.Sink;
        CanTriggerInteractInput = true;
        Core.OnWashComplete += UpdateStackCount;
    }

    private void UpdateStackCount(ItemType type)
    {
        StacksCount--;
        
    }

    public override bool PickupItem(ItemObject itemObject)
    {
        var currentState = itemObject.GetItem().Type;
        if (currentState != ItemType.DirtyPlate)
            return false;
        if (StacksCount == 0)
        {
            base.PickupItem(itemObject);
        }
        else
            Object.Destroy(itemObject.gameObject);
        StacksCount++;
        return true;
    }
}

public class CounterTable : Interactable
{
    public CounterTable()
    {
        StacksCount = 0;
        Type = InteractableType.CounterTable;
    }
    
    public override bool PickupItem(ItemObject itemObject)
    {
        Item itm = itemObject.GetItem();
        if (itm.CurrentIngredients == null) 
            return false;
        var dish = itm.CurrentIngredients[0];
        var currentState = dish.State;
        if (currentState != ItemState.Prepared)
            return false;
        base.PickupItem(itemObject);
        OrderSystem.ServeOrder(dish.Type);
        Action onDone = () =>
        {
            InvokeOnSpawnItemObject(ItemType.DirtyPlate);
        };
        var timer = TimerManager.GetWorldSpaceTimerBehaviour();
        timer.Initialize(itm.InteractDuration, false, null, onDone);
        InvokeRemoveInteractObject();
        return true;
    }
}

public class DirtyPlateTable : Interactable
{
    public DirtyPlateTable()
    {
        StacksCount = 0;
        Type = InteractableType.DirtyPlateTable;
    }

    public override bool PickupItem(ItemObject itemObject)
    {
        return false;
    }

    public override void DropItem()
    {
        StacksCount--;
        if (StacksCount == 0)
        {
            ItemObj = null;
            return;
        }
        SpawnFromData(ItemType.DirtyPlate);
    }
}

public class CleanPlateTable : Interactable
{
    public CleanPlateTable()
    {
        StacksCount = 0;
        Type = InteractableType.CleanPlateTable;
    }

    public override bool PickupItem(ItemObject itemObject)
    {
        return false;
    }

    public override void DropItem()
    {
        StacksCount--;
        if (StacksCount == 0)
        {
            ItemObj = null;
            return;
        }
        SpawnFromData(ItemType.Plate);
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
    void ActivateInteractState(Item item);
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
    Sink,
    CounterTable,
    DirtyPlateTable,
    CleanPlateTable
}