using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableEntity : Entity, IPickupHandler
{
    [Header("Detect Radius")] 
    [SerializeField] private float _detectRadius = 0.5f;
    [SerializeField] private Transform _itemPlacement;

    public bool HasItem => ItemObj != null;
    public ItemObject ItemObj { get; private set; }

    private WorldSpaceTimerBehaviour _worldSpaceTimer;
    private bool _isInteracting = false;
    
    //TODO temporary
    private InteractObject _highlightedObject;
    private InteractBags _currentHighlightData;

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

    private void Update()
    {
        CheckHighlightObject();
    }

    private void CheckHighlightObject()
    {
        var colliders = Physics.OverlapSphere(_itemPlacement.position, _detectRadius, Data.InteractableLayerMask);
        if (colliders.Length <= 0)
        {
            if (_highlightedObject != null)
                _highlightedObject.HighlightObject(_currentHighlightData.Material);
            return;
        }
        colliders[0].TryGetComponent(out InteractObject interObj);
        _highlightedObject = interObj;
        _currentHighlightData = _highlightedObject.Data;
        _highlightedObject.HighlightObject(_currentHighlightData.MaterialEmissive);
    }

    private void OnInput(FrameInput input)
    {
        InteractInterrupted(input);
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
        var item = ItemObj.GetItem();
        if (item.CurrentWorldSpaceTimerBehaviour != null)
            item.CurrentWorldSpaceTimerBehaviour.GetCurrentTimeUI().gameObject.SetActive(false);
        return true;
    }

    public void DropItem()
    {
        var item = ItemObj.GetItem();
        if (item.CurrentWorldSpaceTimerBehaviour != null)
            item.CurrentWorldSpaceTimerBehaviour.GetCurrentTimeUI().gameObject.SetActive(true);
        ItemObj = null;
    }

    private void InteractInterrupted(FrameInput input)
    {
        if (_isInteracting && (input.Horizontal > 0 || input.Vertical > 0))
        {
            _isInteracting = false;
            _worldSpaceTimer.Interrupted(true);
        }
    }

    public void ActivateInteractState(Item item)
    {
        _isInteracting = true;
        _worldSpaceTimer = item.CurrentWorldSpaceTimerBehaviour;
        _worldSpaceTimer.Interrupted(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_itemPlacement.position, _detectRadius);
    }
}