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
    [SerializeField] private GameObject MainMenuDefaultButton;
    [SerializeField] private GameObject CustomizeBackButton;
    bool fadeIn = false;
    bool fadeOut = false;
    GameObject myEventSystem;

    void OnEnable()
    {
        UI.alpha = 0;
        UI.blocksRaycasts = false;
    }
   public void SettingsButton()
    {
        MainUI.blocksRaycasts = false;
        animCamera.SetBool("Customize",true);
        animUI.SetBool("Customize", true);
        fadeIn = true;
    }
    
    public void MainMenuButton()
    {
        UI.blocksRaycasts = false;
        animCamera.SetBool("Customize", false);
        animUI.SetBool("Customize", false);
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
                    myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(CustomizeBackButton);
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
                    myEventSystem = GameObject.Find("EventSystem");
                    myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(MainMenuDefaultButton);
                    MainUI.blocksRaycasts = true;
                    fadeOut = false;
                }
            }
        }
    }

    //TODO attach button once can touch the main menu scene -dyep
    [SerializeField]
    private Button _startButton;
    private void Start()
    {
        _startButton = GameObject.Find("Start Button").GetComponent<Button>();
        _startButton.onClick.AddListener(GoToLevelSelection);
    }

    private void GoToLevelSelection()
    {
        SceneManager.LoadScene(Data.LEVEL_SELECTION_SCENE);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
    }
}

