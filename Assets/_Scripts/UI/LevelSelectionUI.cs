using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private Button _sashimiLevelButton;
    [SerializeField] private Button _soupLevelButton;

    private void OnDisable()
    {
        _sashimiLevelButton.onClick.RemoveListener(() => SelectedLevel(LevelType.Sashimi));
        _soupLevelButton.onClick.RemoveListener(() => SelectedLevel(LevelType.Soup));
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        _sashimiLevelButton.onClick.AddListener(() => SelectedLevel(LevelType.Sashimi));
        _soupLevelButton.onClick.AddListener(() => SelectedLevel(LevelType.Soup));
    }

    private void SelectedLevel(LevelType level)
    {
        LevelManager.LevelToLoad(level);
        if (CharacterManager.Instance.CurrentPlayersCount <= 0)
        {
            var mainCanvas = UIManager.GetMainCanvas();
            var lobbyUI = UIManager.GetUIObject<LobbyUI>(UIType.Lobby);
            lobbyUI.transform.SetParent(mainCanvas.transform,false);
            lobbyUI.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(Data.GAME_SCENE);
        }
    }
}
