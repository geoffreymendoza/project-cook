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
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _animateSpeed = 1f;
    private float _currentScore;

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveListener(() => GoToScene(Data.LOBBY_SCENE));
        _mainMenuButton.onClick.RemoveListener(() => GoToScene(Data.MAIN_MENU_SCENE));
        FadeTransitionUI.OnFinishedFadeIn -= OnFinishedFadeIn;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        _restartButton.onClick.AddListener(() => GoToScene(Data.LOBBY_SCENE));
        _mainMenuButton.onClick.AddListener(() => GoToScene(Data.MAIN_MENU_SCENE));
        FadeTransitionUI.OnFinishedFadeIn += OnFinishedFadeIn;
    }

    private void OnFinishedFadeIn()
    {
        FadeTransitionUI.OnFinishedFadeIn -= OnFinishedFadeIn;
        _slider.value = 0;
        _slider.maxValue = 100;
        _isAnimating = true;
    }

    private void Initialize()
    {
        _currentScore = ScoreSystem.GetCurrentScore();
        var levelString = $"Level: {LevelManager.GetCurrentLevel().ToString()}";
        _levelTextMesh.text = levelString;
        var scoreString = $"Score: {_currentScore.ToString()}";
        _scoreTextMesh.text = scoreString;
        _slider.maxValue = 100;
    }

    private bool _isAnimating = false;
    private void Update()
    {
        if (!_isAnimating) return;
        AnimateSlider(_currentScore);
    }

    private void AnimateSlider(float currentScore)
    {
        if (_slider.value < currentScore)
        {
            _slider.value += _animateSpeed * Time.deltaTime;
            return;
        }
        _isAnimating = false;
    }

    private void GoToScene(string sceneName)
    {
        var fadeTransition = UIManager.GetUIObject<FadeTransitionUI>(UIType.FadeTransition);
        fadeTransition.ProceedToScene(sceneName);
    }
}