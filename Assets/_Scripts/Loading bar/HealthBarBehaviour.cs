using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider healthBar;
    public Color Low;
    public Color High;
    public Vector3 Offset;


    public void SetHealth(float health, float maxhealth)
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(health < maxhealth);
            healthBar.value = health;
            healthBar.maxValue = maxhealth;

            healthBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, healthBar.normalizedValue);
        }
    }
    
    void Update()
    {
      
    }
}
