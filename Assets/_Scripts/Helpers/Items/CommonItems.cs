using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

[System.Serializable]
public class Item
{
    public ItemType Type { protected set; get; }
    public ItemState State { protected set; get; }
    public string ItemName { protected set; get; }
    public ItemObject ItemContainer { protected set; get; }
    public bool CanSlice { protected set; get; }
    public bool CanCook { protected set; get; }
    public bool CanWash { protected set; get; }
    public float InteractDuration { protected set; get; }
    public ItemBags _data;
    public List<Item> CurrentIngredients { protected set; get; }
    public WorldSpaceTimerBehaviour CurrentWorldSpaceTimerBehaviour { protected set; get; }

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
        if(_data.Type is ItemType.Plate or ItemType.CookContainer or ItemType.DirtyPlate)
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
            var info = _data.IngredientProcessInfo.FirstOrDefault(s => s.State == ItemState.Sliced);
            if (info == null) return false;
            AudioManager.instance.Play(SoundType.ChopFX);
            if (CurrentWorldSpaceTimerBehaviour != null) return true;
            Action onDone = () =>
            {
                AudioManager.instance.Stop(SoundType.ChopFX);
                ChangeState(info.State);
                ItemContainer.ChangeMesh(info);
                Debug.Log($"{Type} sliced");
            };
            
            CreateTimerUI(onDone);
            return true;
        }

        if (CanWash)
        {
            AudioManager.instance.Play(SoundType.WashFX);
            if (CurrentWorldSpaceTimerBehaviour != null) return true;
            Action onDone = () =>
            {
                AudioManager.instance.Stop(SoundType.WashFX);
                Core.InvokeOnWashComplete(ItemType.Plate);
                //OnWashComplete?.Invoke(ItemType.Plate);
                Debug.Log($"{Type} washed");
            };
            CreateTimerUI(onDone);
            return true;
        }
        //TODO fire extinguisher
        return false;
    }

    public void AddIngredient(Item ingredient)
    {
        CurrentIngredients.Add(ingredient);
    }

    public void CreateTimerUI(Action onDone)
    {
        CreateTimerUI(InteractDuration, onDone);
    }
    
    public void CreateTimerUI(float duration, Action onDone)
    {
        CurrentWorldSpaceTimerBehaviour = TimerManager.GetWorldSpaceTimerBehaviour();
        CurrentWorldSpaceTimerBehaviour.Initialize(duration, true, ItemContainer.transform, onDone);
    }

    public void ExtendTime(float duration)
    {
        CurrentWorldSpaceTimerBehaviour.GetCurrentTimeUI().UpdateSlider(duration);
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