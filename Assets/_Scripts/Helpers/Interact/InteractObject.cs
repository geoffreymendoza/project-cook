using System;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField] private Transform _itemPlacement;
    public Transform GetItemPlacement() => _itemPlacement;
    
    private Interactable _interactable;
    public Interactable GetInteract() => _interactable;

    private MeshFilter _meshFilter;
    private Renderer _renderer;

    private void Awake()
    {
        _meshFilter = this.GetComponent<MeshFilter>();
        _renderer = this.GetComponent<Renderer>();
    }

    private bool _init = false;
    public void Initialize(InteractBags data)
    {
        if (_init) return;
        _init = true;
        
        var type = Type.GetType(data.Type.ToString());
        if (type == null)
        {
            Debug.LogError($"Type {data.Type} cannot found. Please assign");
            return;
        }
        var instance = (Interactable)Activator.CreateInstance(type);
        _interactable = instance;
        
        var item = DataManager.GetItemObject(data.InitialItem);
        ItemObject instanceItem = null;
        if (item != null)
        {
            instanceItem = Instantiate(item, _itemPlacement, false);
            var itemData = DataManager.GetItemData(data.InitialItem);
            instanceItem.Initialize(itemData);
        }
        _interactable.Initialize(instanceItem, _itemPlacement);
        
        gameObject.name = data.name;
        _meshFilter.sharedMesh = data.Mesh;
        _renderer.material = data.Material;
        
        if(data.Type == InteractableType.InvisibleTable)
            Interactable.OnRemoveInteractObject += OnRemoveInteractObject;
    }

    private void OnRemoveInteractObject()
    {
        Interactable.OnRemoveInteractObject -= OnRemoveInteractObject;
        //TODO move to object pooling
        Destroy(this.gameObject);
    }
}