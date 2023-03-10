using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class InteractSystem
{
    public static void GrabItem(IPickupHandler character, Interactable interactable)
    {
        if (character.HasItem && interactable == null)
        {
            var invisibleTableBag = DataManager.GetInteractData(InteractableType.InvisibleTable);
            var instance = Object.Instantiate(invisibleTableBag.Prefab);
            instance.Initialize(invisibleTableBag);
            var position = character.ItemObj.transform.position;
            position.y = 0.5f; //TODO falling animation or formula
            instance.transform.position = position;
            PickupItemByInteractable(character, instance.GetInteract());
            return;
        }
        
        if (interactable == null) return;
        
        if (!character.HasItem && interactable.HasItem)
        {
            PickupItemByCharacter(character, interactable);
            return;
        }

        if (character.HasItem && !interactable.HasItem)
        {
            PickupItemByInteractable(character, interactable);
            return;
        }

        if (character.HasItem && interactable.HasItem)
        {
            var items = GetItems(character, interactable);
            if(items.Container == null) return;
            if (items.Container.Type is ItemType.Plate && items.ItemToCombine.Type is ItemType.Plate) return;            
            if (items.Container.Type is ItemType.DirtyPlate && items.ItemToCombine.Type is ItemType.DirtyPlate) return;
            if (items.Container.Type is ItemType.DirtyPlate && items.ItemToCombine.Type is ItemType.DirtyPlate)
            {
                PickupItemByInteractable(character, interactable);
                return;
            }
            RecipeSystem.CombineItem(items.Container, items.ItemToCombine);
        }
    }

    private static void PickupItemByCharacter(IPickupHandler character, Interactable interactable)
    {
        if (interactable.Type is InteractableType.Sink) return;
        
        ItemObject itemObject = interactable.ItemObj;
        bool pickedUp = character.PickupItem(itemObject);
        if(pickedUp)
            interactable.DropItem();
    }
    
    private static void PickupItemByInteractable(IPickupHandler character, Interactable interactable)
    {
        //TODO HERE
        ItemObject itemObject = character.ItemObj;
        bool pickedUp = interactable.PickupItem(itemObject);
        if(pickedUp)
            character.DropItem();
    }

    public static void Interact(IPickupHandler character, Interactable interactable)
    {
        if (character.HasItem)
        {
            //TODO fire extinguisher
            return;
        }

        if (!interactable.CanTriggerInteractInput) return;
        var item = interactable.ItemObj.GetItem();
        bool isInteracting = item.Interact();
        if(isInteracting)
            character.ActivateInteractState(item);
    }

    //TODO refactor
    private static (Item Container, Item ItemToCombine) GetItems(IPickupHandler character, Interactable interactable)
    {
        Item container = null;
        Item ingredient = null;
        ItemType charItemType = character.ItemObj.GetItem().Type;
        ItemType interactableItemType = interactable.ItemObj.GetItem().Type;

        if ((charItemType is ItemType.Plate && interactableItemType is ItemType.DirtyPlate) ||
            interactableItemType is ItemType.Plate && charItemType is ItemType.DirtyPlate)
        {
            return (null, null);
        }

        if (charItemType is ItemType.DirtyPlate && interactableItemType is ItemType.DirtyPlate)
        {
            container = interactable.ItemObj.GetItem();
            ingredient = character.ItemObj.GetItem();
            return (container, ingredient);
        }

        if (charItemType is ItemType.Plate or ItemType.CookContainer)
        {
            container = character.ItemObj.GetItem();
            ingredient = interactable.ItemObj.GetItem();
            return (container, ingredient);
        }

        if (interactableItemType is ItemType.Plate or ItemType.CookContainer)
        {
            container = interactable.ItemObj.GetItem();
            ingredient = character.ItemObj.GetItem();
            return (container, ingredient);
        }
        return (null, null);
    }
}
