using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator animCamera;
    [SerializeField] private Animator animUI;
    [SerializeField] private CanvasGroup UI;
    [SerializeField] private CanvasGroup MainUI;
    [SerializeField] private CanvasGroup LevelUI;
    [SerializeField] private GameObject MainMenuDefaultButton;
    [SerializeField] private GameObject CustomizeBackButton;
    [SerializeField] private GameObject LevelBackButton;
    bool CustomFadeIn = false;
    bool CustomFadeOut = false;
    bool LevelFadeIn = false;
    bool LevelFadeOut = false;
    GameObject myEventSystem;

    void OnEnable()
    {
        UI.alpha = 0;
        UI.blocksRaycasts = false;
        LevelUI.alpha = 0;
        LevelUI.blocksRaycasts = false;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    public void StartButton() 
    {
        MainUI.blocksRaycasts = false;
        animCamera.SetBool("Level", true);
        animUI.SetBool("Customize", true);
        LevelFadeIn = true;
    }

    public void MainMenuLevelButton()
    {
        LevelUI.blocksRaycasts = false;
        animCamera.SetBool("Level", false);
        animUI.SetBool("Customize", false);
        LevelFadeOut = true;
    }
        public void SettingsButton()
    {
        MainUI.blocksRaycasts = false;
        animCamera.SetBool("Customize",true);
        animUI.SetBool("Customize", true);
        CustomFadeIn = true;
    }
    
    public void MainMenuCustomizeButton()
    {
        UI.blocksRaycasts = false;
        animCamera.SetBool("Customize", false);
        animUI.SetBool("Customize", false);
        CustomFadeOut = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (CustomFadeIn)
        {
            if (UI.alpha < 1)
            {
                UI.alpha += Time.deltaTime;
                if (UI.alpha >= 1)
                {
                    myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(CustomizeBackButton);
                    UI.blocksRaycasts = true;
                    CustomFadeIn = false;
                }
            }
        }

        if (CustomFadeOut)
        {
            
            if (UI.alpha >= 0)
            {
                UI.alpha -= Time.deltaTime;
                if (UI.alpha == 0)
                {
                    myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(MainMenuDefaultButton);
                    MainUI.blocksRaycasts = true;
                    CustomFadeOut = false;
                }
            }
        }

        if (LevelFadeIn)
        {
            if (LevelUI.alpha < 1)
            {
                LevelUI.alpha += Time.deltaTime * 0.7f;
                if (LevelUI.alpha >= 1)
                {
                    myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(LevelBackButton);
                    LevelUI.blocksRaycasts = true;
                    LevelFadeIn = false;
                }
            }
        }

        if (LevelFadeOut)
        {

            if (LevelUI.alpha >= 0)
            {
                LevelUI.alpha -= Time.deltaTime;
                if (LevelUI.alpha == 0)
                {
                    myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(MainMenuDefaultButton);
                    MainUI.blocksRaycasts = true;
                    LevelFadeOut = false;
                }
            }
        }
    }
}

