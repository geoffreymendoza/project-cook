using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ScoreSystem : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI ScoreText;
    private int ScoreNum;

    private void Start()
    {
        ScoreNum = 0;
        ScoreText.text = "Score : " + ScoreNum;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            ScoreNum += 1;
            //Destroy(other.gameObject);
            ScoreText.text = "Score : " + ScoreNum;
            
        }
    }
}
