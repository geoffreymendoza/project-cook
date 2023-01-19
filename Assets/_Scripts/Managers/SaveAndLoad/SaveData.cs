using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData 
{
    public int index = 1;
    [SerializeField]
    private float myFloat = 5.8f;
    public bool myBool = true;
    public Vector3 myVector = new Vector3(0, 10, 99);
}
