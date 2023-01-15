using System;
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
    private ItemBags _data;
    
    public Item(ItemBags data, ItemObject container)
    {
        _data = data;
        Type = data.Type;
        State = data.State;
        CanSlice = data.CanSlice;
        CanCook = data.CanCook;
        ItemName = SplitWordsBySpace(Type.ToString());
        ItemContainer = container;
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

    public void Interact()
    {
        if (CanSlice && State == ItemState.Raw)
        {
            //TODO timer
            var info = _data.IngredientProcessInfo.FirstOrDefault(s => s.State == ItemState.Sliced);
            if (info == null) return;
            State = info.State;
            ItemContainer.ChangeMesh(info);
            Debug.Log($"{Type} sliced");
        }
        
        //TODO fire extinguisher
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
    Soup = 400,
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
}

public enum ItemState
{
    Unassigned = 0,
    Raw = 1,
    Sliced,
    Cooked,
    Prepared,
    Platter = 100,
}