using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCore
{
    public static event Action<ItemType> OnWashComplete;

    public static void InvokeOnWashComplete(ItemType type)
    {
        OnWashComplete?.Invoke(type);
    }
}
