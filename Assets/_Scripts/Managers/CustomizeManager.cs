using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeManager : MonoBehaviour
{
    [SerializeField] private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void SelectedButton()
    {
        this.outline.enabled = true;
    }

    public void DeselectedButton()
    {
        this.outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
