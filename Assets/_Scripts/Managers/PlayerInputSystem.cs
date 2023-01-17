using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputSystem : MonoBehaviour
{
    public Button _joinedBtn;
    [SerializeField] private Entity _character;
    
    // Start is called before the first frame update
    void Start()
    {
        // _joinedBtn.onClick.AddListener(JoinPlayer);
    }

    // private void OnApplicationQuit()
    // {
    //     _joinedBtn.onClick.RemoveListener(JoinPlayer);
    //
    // }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void JoinPlayer()
    {
        Instantiate(_character);
    }
}
