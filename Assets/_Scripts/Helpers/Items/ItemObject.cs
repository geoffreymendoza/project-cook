using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private Item _item;
    public Item GetItem() => _item;

    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Renderer _renderer;

    private void Awake()
    {
        // _meshFilter = GetComponent<MeshFilter>();
        // _renderer = GetComponent<Renderer>();
    }

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
        _meshFilter.sharedMesh = data.Mesh;
        _renderer.material = data.Material;
    }

    public void ChangeMesh(ProcessInfo data)
    {
        if (data.Mesh == null)
        {
            Debug.Log($"Prepare the mesh for these");
            return;  
        }
        _meshFilter.sharedMesh = data.Mesh;
        _renderer.material = data.Material;
    }
}
