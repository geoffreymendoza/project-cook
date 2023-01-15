using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayableEntity : Entity, IPickupHandler
{
    //Add the trackable item
    [Header("Detect Radius")] 
    [SerializeField] private float _detectRadius = 0.5f;
    [SerializeField] private Transform _itemPlacement;

    public bool HasItem => ItemObj != null;
    public ItemObject ItemObj { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        InputController.OnInput += OnInput;
    }

    private void OnApplicationQuit()
    {
        InputController.OnInput -= OnInput;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInput(FrameInput input)
    {
        InteractItem(input.Interact);
        GrabItem(input.Grab);
    }

    private void GrabItem(bool grab)
    {
        if (!grab) return;
        var obj = DetectInteractObject();
        if (obj != null)
        {
            InteractSystem.GrabItem(this, obj.GetInteract());
            return;
        }
        InteractSystem.GrabItem(this, null);
    }

    private void InteractItem(bool interact)
    {
        if (!interact) return;
        //TODO 1. if detecting obj, slice item 2. throw item
        var obj = DetectInteractObject();
        if (obj != null)
        {
            InteractSystem.Interact(this, obj.GetInteract());
        }
    }

    private InteractObject DetectInteractObject()
    {
        var colliders = Physics.OverlapSphere(_itemPlacement.position, _detectRadius, Data.InteractableLayerMask);
        if (colliders.Length <= 0) return null;
        colliders[0].TryGetComponent(out InteractObject interObj);
        return interObj;
    }

    public bool PickupItem(ItemObject itemObject)
    {
        ItemObj = itemObject;
        var itemTransform = ItemObj.transform;
        itemTransform.SetParent(this.transform, false);
        itemTransform.position = _itemPlacement.position;
        return true;
    }

    public void DropItem()
    {
        ItemObj = null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_itemPlacement.position, _detectRadius);
    }
}