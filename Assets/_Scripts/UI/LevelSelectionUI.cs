using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private Button _sashimiLevelButton;
    [SerializeField] private TextMeshProUGUI _sashimiScoreText;
    [SerializeField] private Button _soupLevelButton;
    [SerializeField] private TextMeshProUGUI _soupScoreText;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var previousSave = SaveAndLoadSystem.LoadGame();
        foreach (var lvl in previousSave.Levels)
        {
            var scoreText = "";
            switch (lvl.Type)
            {
                case LevelType.Sashimi:
                    _sashimiLevelButton.interactable = lvl.Unlocked;
                    scoreText = $"High Score: {lvl.HighScore}";
                    _sashimiScoreText.text = scoreText;
                    break;
                case LevelType.Soup:
                    _soupLevelButton.interactable = lvl.Unlocked;
                    scoreText = $"High Score: {lvl.HighScore}";
                    _soupScoreText.text = scoreText;
                    break;
                case LevelType.Salad:
                    break;
                case LevelType.None:
                    _soupLevelButton.interactable = lvl.Unlocked;
                    scoreText = $"High Score: {lvl.HighScore}";
                    _sashimiScoreText.text = scoreText;
                    _soupScoreText.text = scoreText;
                    break;
            }
        }
    }

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
        var fadeTransition = UIManager.GetUIObject<FadeTransitionUI>(UIType.FadeTransition);
        fadeTransition.ExitScene(Data.LOBBY_SCENE);
        // SceneManager.LoadScene(Data.LOBBY_SCENE);
    }
}
