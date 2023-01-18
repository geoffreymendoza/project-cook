using UnityEngine;

[CreateAssetMenu(menuName = "ProjectCook/Spawn Bag", fileName = "Spawn_Bag", order = 6)]
public class SpawnBags : ScriptableObject
{
    public SpawnType Type;
    public GameObject Prefab;
}