using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

[System.Serializable]
public class Item
{
    public static event Action<ItemType> OnWashComplete;
    public ItemType Type { protected set; get; }
    public ItemState State { protected set; get; }
    public string ItemName { protected set; get; }
    public ItemObject ItemContainer { protected set; get; }
    public bool CanSlice { protected set; get; }
    public bool CanCook { protected set; get; }
    public bool CanWash { protected set; get; }
    public float InteractDuration { protected set; get; }
    private ItemBags _data;
    public List<Item> CurrentIngredients { protected set; get; }
    public TimerBehaviour CurrentTimerBehaviour { protected set; get; }

    public Item(ItemBags data, ItemObject container = null)
    {
        _data = data;
        Type = data.Type;
        State = data.State;
        CanSlice = data.CanSlice;
        CanCook = data.CanCook;
        CanWash = data.CanWash;
        InteractDuration = data.InteractDuration;
        ItemName = SplitWordsBySpace(Type.ToString());
        ItemContainer = container;
        if(_data.Type is ItemType.Plate or ItemType.CookContainer)
            CurrentIngredients = new List<Item>();
    }

    private string SplitWordsBySpace(string word)
    {
        var name = word;
        StringBuilder builder = new StringBuilder();
        foreach (var c in name)
        {
            if (char.IsUpper(c) && builder.Length > 0)
                builder.Append(' ');
            builder.Append(c);
        }
        name = builder.ToString();
        return name;
    }
    
    //TODO timers before changing state
    public void ChangeState(ItemState newState)
    {
        State = newState;
    }

    public void UpStateByOne()
    {
        State++;
    }

    public bool Interact()
    {
        if (CanSlice && State == ItemState.Raw)
        {
            //TODO timer
            var info = _data.IngredientProcessInfo.FirstOrDefault(s => s.State == ItemState.Sliced);
            if (info == null) return false;
            Action onDone = () =>
            {
                ChangeState(info.State);
                ItemContainer.ChangeMesh(info);
                Debug.Log($"{Type} sliced");
            };
            if (CurrentTimerBehaviour != null) return true;
            CurrentTimerBehaviour = TimerManager.GetTimerBehaviour();
            CurrentTimerBehaviour.Initialize(InteractDuration, true, ItemContainer.transform.position, onDone);
            return true;
        }

        if (CanWash)
        {
            Action onDone = () =>
            {
                OnWashComplete?.Invoke(ItemType.Plate);
                Debug.Log($"{Type} washed");
            };
            if (CurrentTimerBehaviour != null) return true;
            CurrentTimerBehaviour = TimerManager.GetTimerBehaviour();
            CurrentTimerBehaviour.Initialize(InteractDuration, true, ItemContainer.transform.position, onDone);
            return true;
        }

        return false;
        //TODO fire extinguisher
    }

    public void AddIngredient(Item ingredient)
    {
        CurrentIngredients.Add(ingredient);
    }
}

public enum ItemType
{
    NoInitialItem = -1,
    Unassigned = 0,
    
    //INGREDIENTS: 1-199
    Tomato = 1,
    Mushroom,
    Onion,
    Lettuce,
    Cucumber,
    Fish,
    Shrimp,

    //PLATING FOOD: 400-599
    REMOVED = 400,
    TomatoSoup,
    OnionSoup,
    MushroomSoup,
    FishSashimi,
    ShrimpSashimi,
    Salad,
    SaladWithTomato,
    SaladWithTomatoAndCucumber,
    
    //META: 600+
    Plate = 600,
    CookContainer,
    FireExtinguisher,
    DirtyPlate,
}

public enum ItemState
{
    Unassigned = 0,
    Raw = 1,
    Sliced,
    Cooked,
    Prepared,
}