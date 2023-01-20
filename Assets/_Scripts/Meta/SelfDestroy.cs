using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
