using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Core
{
    public static event Action<ItemType> OnWashComplete;

    public static void InvokeOnWashComplete(ItemType type)
    {
        OnWashComplete?.Invoke(type);
    }

    public static event Action<SceneID> OnInitializeScene;

    public static void InvokeOnInitializeScene(SceneID id)
    {
        OnInitializeScene?.Invoke(id);
    }
    
    
}
