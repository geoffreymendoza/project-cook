using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelTextMesh;
    [SerializeField] private TextMeshProUGUI _scoreTextMesh;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _levelSelectionButton;
    [SerializeField] private Button _mainMenuButton;

    private void OnApplicationQuit()
    {
        _restartButton.onClick.RemoveListener(() => GoToScene(Data.GAME_SCENE));
        _levelSelectionButton.onClick.RemoveListener(() => GoToScene(Data.LEVEL_SELECTION_SCENE));
        _mainMenuButton.onClick.RemoveListener(() => GoToScene(Data.MAIN_MENU_SCENE));
    }

    // Start is called before the first frame update
    void Start()
    {
        var levelString = $"Level: {LevelManager.GetCurrentLevel().ToString()}";
        _levelTextMesh.text = levelString;
        var scoreString = $"Score: {ScoreSystem.GetCurrentScore().ToString()}";
        _scoreTextMesh.text = scoreString;
        _restartButton.onClick.AddListener(() => GoToScene(Data.GAME_SCENE));
        _levelSelectionButton.onClick.AddListener(() => GoToScene(Data.LEVEL_SELECTION_SCENE));
        _mainMenuButton.onClick.AddListener(() => GoToScene(Data.MAIN_MENU_SCENE));
    }

    private void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}