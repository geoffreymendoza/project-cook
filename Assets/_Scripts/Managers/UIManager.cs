using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    private static readonly Dictionary<UIType, UIBags> _uiBags = new Dictionary<UIType, UIBags>();
    public static Canvas MainCanvas { private set; get; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var uiBags = Resources.LoadAll<UIBags>(Data.UI_BAGS_PATH);
        foreach (var ui in uiBags)
        {
            _uiBags[ui.Type] = ui;
        }
    }

    public static Canvas GetMainCanvas()
    {
        if(MainCanvas == null)
            MainCanvas = GetUIObject<Canvas>(UIType.MainCanvas);
        return MainCanvas;
    }

    public static GameObject GetUIObject(UIType type)
    {
        _uiBags.TryGetValue(type, out UIBags bag);
        if (bag == null) return null;
        var uiObj = Object.Instantiate(bag.Prefab);
        //TODO only in main canvas all ui
        //when in world space just retract from main canvas
        // uiObj.transform.SetParent(MainCanvas.transform,false);
        return uiObj;
    }

    public static T GetUIObject<T>(UIType type)
    {
        return GetUIObject(type).GetComponent<T>();
    }
}

public enum UIType
{
    None,
    WorldSpaceTimeDuration,
    MainCanvas,
    GameTime,
    OneIngredientOrder,
    TwoIngredientOrder,
    ThreeIngredientOrder,
    OrdersGrid,
    Score,
    DelayStart,
    DelayEnd,
    Results,
    LevelSelection
}
