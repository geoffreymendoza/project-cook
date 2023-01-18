using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private Item _item;
    public Item GetItem() => _item;

    [SerializeField] private GameObject _currentModel;

    private bool _init = false;
    public void Initialize(ItemBags data)
    {
        if (_init) return;
        _init = true;
        var item = new Item(data, this);
        _item = item;

        gameObject.name = _item.ItemName;
        ChangeMesh(data);
    }

    public void ChangeMesh(ItemBags data)
    {
        if (data.Model == null)
        {
            Debug.Log($"Prepare the gameobject for these");
            return;
        }
        Destroy(_currentModel);
        _currentModel = Instantiate(data.Model, this.transform);
    }

    public void ChangeMesh(ProcessInfo data)
    {
        if (data.Model == null)
        {
            Debug.Log($"Prepare the gameobject for these");
            return;
        }
        //TODO just do SetActive
        Destroy(_currentModel);
        _currentModel = Instantiate(data.Model, this.transform);
    }
}
