using System;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public static event Action<ItemType> OnSpawnDirtyPlate;
    
    [SerializeField] private Transform _itemPlacement;
    private InteractableType _type;
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

    private void OnApplicationQuit()
    {
        switch (_type)
        {
            case InteractableType.InvisibleTable:
                _interactable.OnRemoveInteractObject -= OnRemoveInteractObject;
                break;
            case InteractableType.CounterTable:
                _interactable.OnSpawnItemObject -= OnSpawnDirtyPlateObject;
                break;
            case InteractableType.DirtyPlateTable:
                OnSpawnDirtyPlate -= _interactable.SpawnItem;
                break;
            case InteractableType.CleanPlateTable:
                Item.OnWashComplete -= _interactable.SpawnItem;
                break;
            case InteractableType.Sink:
                Item.OnWashComplete -= OnWashComplete;
                break;
        }
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
        
        var itemObj = DataManager.GetItemObject(data.InitialItem);
        ItemObject instanceItem = null;
        if (itemObj != null)
        {
            instanceItem = Instantiate(itemObj, _itemPlacement, false);
            var itemData = DataManager.GetItemData(data.InitialItem);
            instanceItem.Initialize(itemData);
        }
        _interactable.Initialize(instanceItem, _itemPlacement);
        
        gameObject.name = data.name;
        _meshFilter.sharedMesh = data.Mesh;
        _renderer.material = data.Material;

        _type = data.Type;

        switch (_type)
        {
            case InteractableType.InvisibleTable:
                _interactable.OnRemoveInteractObject += OnRemoveInteractObject;
                break;
            case InteractableType.CounterTable:
                _interactable.OnSpawnItemObject += OnSpawnDirtyPlateObject;
                _interactable.OnRemoveInteractObject += OnDestroyInteractObject;
                break;
            case InteractableType.DirtyPlateTable:
                OnSpawnDirtyPlate += _interactable.SpawnItem;
                break;
            case InteractableType.CleanPlateTable:
                Item.OnWashComplete += _interactable.SpawnItem;
                break;
            case InteractableType.Sink:
                Item.OnWashComplete += OnWashComplete;
                break;
        }
    }

    private void OnWashComplete(ItemType type)
    {
        Destroy(_interactable.ItemObj.gameObject);
    }

    private void OnDestroyInteractObject()
    {
        Destroy(_interactable.ItemObj.gameObject);
    }

    private void OnSpawnDirtyPlateObject(ItemType type)
    {
        //TODO move to object pooling and timer
        // Destroy(_interactable.ItemObj.gameObject);
        OnSpawnDirtyPlate?.Invoke(type);
    }

    private void OnRemoveInteractObject()
    {
        _interactable.OnRemoveInteractObject -= OnRemoveInteractObject;
        //TODO move to object pooling
        Destroy(this.gameObject);
    }
}