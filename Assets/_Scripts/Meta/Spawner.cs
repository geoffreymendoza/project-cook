using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private InteractableType _interactableType;
    [SerializeField] private ItemType _initialItem = ItemType.NoInitialItem;
    [SerializeField] private Vector3 _cubeSize;

    private bool _spawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    
    private void Spawn()
    {
        _spawned = true;
        var interactBag = DataManager.GetInteractData(_interactableType, _initialItem);
        // Debug.Log(interactBag.name);
        //TODO get from object pool
         var interactObj = Instantiate(interactBag.Prefab, this.transform.position, Quaternion.identity);
        interactObj.Initialize(interactBag);
        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(_spawned) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position,_cubeSize);
    }
}
