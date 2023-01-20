using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private float _duration = 1.25f;
    [SerializeField] private TextMeshProUGUI _textMesh;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _duration);
    }

    public void ShowMessage(string msg)
    {
        _textMesh.text = msg;
    }
    
}
