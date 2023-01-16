using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator animCamera;
    [SerializeField] private Animator animUI;
    [SerializeField] private CanvasGroup UI;
    [SerializeField] private GameObject MainUI;
    bool fadeIn = false;
    bool fadeOut = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        UI.alpha = 0;
        UI.blocksRaycasts = false;
    }
   public void SettingsButton()
    {
        animCamera.SetTrigger("Customize");
        animUI.SetTrigger("Customize");
        fadeIn = true;
    }

    public void MainMenuButton()
    {
        animCamera.SetTrigger("MainMenu");
        animUI.SetTrigger("MainMenu");
        fadeOut = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            if (UI.alpha < 1)
            {
                UI.alpha += Time.deltaTime;
                if (UI.alpha >= 1)
                {
                    UI.blocksRaycasts = true;
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (UI.alpha >= 0)
            {
                UI.alpha -= Time.deltaTime;
                if (UI.alpha == 0)
                {
                    UI.blocksRaycasts = false;
                    fadeOut = false;
                }
            }
        }
    }



}

