using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    private static readonly Dictionary<UIType, UIBags> _uiBags = new Dictionary<UIType, UIBags>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var uiBags = Resources.LoadAll<UIBags>(Data.UI_BAGS_PATH);
        foreach (var ui in uiBags)
        {
            _uiBags[ui.Type] = ui;
        }
    }

    public static GameObject GetUIObject(UIType type)
    {
        _uiBags.TryGetValue(type, out UIBags bag);
        if (bag == null) return null;
        var uiObj = Object.Instantiate(bag.Prefab);
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
    
}
