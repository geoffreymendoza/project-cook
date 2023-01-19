using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{ 
    [SerializeField] private SpawnCatergoryType _type;
    [Header("Interactable Details")]
    [SerializeField] private InteractableType _interactableType;
    [SerializeField] private ItemType _initialItem = ItemType.NoInitialItem;
    [SerializeField] private Vector3 _cubeSize;

    [Header("Entity Details")] 
    [SerializeField] private SpawnType _spawnType;

    private bool _spawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (_type)
        {
            case SpawnCatergoryType.Interactable:
                SpawnInteractable();
                break;
            case SpawnCatergoryType.Entity:
                SpawnEntity();
                break;
        }
    }
    
    private void SpawnInteractable()
    {
        _spawned = true;
        var interactBag = DataManager.GetInteractData(_interactableType, _initialItem);
        var interactObj = Instantiate(interactBag.Prefab, this.transform.position, Quaternion.identity);
        interactObj.Initialize(interactBag);
        this.gameObject.SetActive(false);
    }

    private void SpawnEntity()
    {
        _spawned = true;
        var spawnBag = DataManager.GetSpawnData(_spawnType);
        var interactObj = Instantiate(spawnBag.Prefab, this.transform.position, Quaternion.identity);
        //TODO INITIALIZE INPUTS 
        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(_spawned) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position,_cubeSize);
    }
}

public enum SpawnCatergoryType
{
    Interactable,
    Entity,
    
}
