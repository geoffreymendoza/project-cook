using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InteractSystem
{
    public static void GrabItem(IPickupHandler character, Interactable interactable)
    {
        //TODO dropping item on the floor
        if (character.HasItem && interactable == null)
        {
            var invisibleTableBag = DataManager.GetInteractData(InteractableType.InvisibleTable);
            var instance = Object.Instantiate(invisibleTableBag.Prefab);
            instance.Initialize(invisibleTableBag);
            var position = character.ItemObj.transform.position;
            position.y = 0.5f; //subject to change
            instance.transform.position = position;
            PickupItemByInteractable(character, instance.GetInteract());
            return;
        }
        
        if (interactable == null) return;
        
        // character don't have item and interact table have
        if (!character.HasItem && interactable.HasItem)
        {
            PickupItemByPlayer(character, interactable);
            return;
        }

        // character have item and interact table don't have
        if (character.HasItem && !interactable.HasItem)
        {
            PickupItemByInteractable(character, interactable);
            return;
        }

        //TODO only plate is acceptable and pot
        // if (player.HasItem && interactable.HasItem)
        // {
        //     itemObject = player.ItemObj;
        //     var platterType = itemObject.GetItem().Type;
        //     if (platterType is not (ItemType.Plate or ItemType.CookContainer))
        //     {
        //         Debug.Log("no plates");
        //         return;
        //     }
        //     
        //     //TODO mix and match by recipe system
        //     PickupItemByPlayer(player, interactable);
        // }
    }

    private static void PickupItemByPlayer(IPickupHandler character, Interactable interactable)
    {
        ItemObject itemObject = interactable.ItemObj;
        bool pickedUp = character.PickupItem(itemObject);
        if(pickedUp)
            interactable.DropItem();
    }
    
    private static void PickupItemByInteractable(IPickupHandler character, Interactable interactable)
    {
        ItemObject itemObject = character.ItemObj;
        bool pickedUp = interactable.PickupItem(itemObject);
        if(pickedUp)
            character.DropItem();
    }

    public static void Interact(IPickupHandler player, Interactable interactable)
    {
        if (player.HasItem)
        {
            //TODO fire extinguisher
            return;
        }

        if (!interactable.CanTriggerInteractInput) return;
        var item = interactable.ItemObj.GetItem();
        item.Interact();
    }
}
