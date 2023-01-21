using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCutomizeUI : MonoBehaviour
{
    [SerializeField] private int _playerIndex = 0;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _enterButton;
    [SerializeField] private GameObject _controlsTutorial;
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _fillSpeed;

    private PlayableEntity _player;

    private void Awake()
    {
        CharacterManager.OnCharacterJoined += OnCharacterJoined;
        _leftButton.onClick.AddListener(() => NavigateModel(-1));
        _rightButton.onClick.AddListener(() => NavigateModel(1));
        _selectButton.onClick.AddListener(OnSelect);
        _enterButton.onClick.AddListener(GoToGame);
    }

    private void OnDestroy()
    {
        CharacterManager.OnCharacterJoined -= OnCharacterJoined;
        _leftButton.onClick.RemoveListener(() => NavigateModel(-1));
        _rightButton.onClick.RemoveListener(() => NavigateModel(1));
        _selectButton.onClick.RemoveListener(OnSelect);
        _enterButton.onClick.RemoveListener(GoToGame);
    }

    private void OnCharacterJoined(int playerCount)
    {
        var index = playerCount - 1;
        if (index != _playerIndex) return;
        _player = (PlayableEntity)CharacterManager.Instance.GetPlayer(index);
    }

    private void NavigateModel(int index)
    {
        if(_player != null)
            _player.AssignModel(index);
    }

    private void OnSelect()
    {
        _leftButton.interactable = false;
        _rightButton.interactable = false;
        _selectButton.interactable = false;
        _enterButton.interactable = true;
        _enterButton.Select();
    }

    private void GoToGame()
    {
        if (_playerIndex != 0) return;
        _controlsTutorial.SetActive(true);
        // LoadingBar();
        _loading = true;
        Invoke(nameof(ProceedToScene), 5f);
    }
    
    private void ProceedToScene()
    {
        var fadeTransition = UIManager.GetUIObject<FadeTransitionUI>(UIType.FadeTransition);
        fadeTransition.ProceedToScene(Data.GAME_SCENE);
    }

    private bool _loading = false;
    private void LoadingBar()
    {
        _loading = true;
    }

    private void Update()
    {
        if (!_loading) return;
        _fillImage.fillAmount += _fillSpeed * Time.deltaTime;
    }
}
