using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCutomizeUI : MonoBehaviour
{
    [SerializeField] private int _playerIndex = 0;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _startButton;
    private PlayableEntity _player;

    private void Awake()
    {
        CharacterManager.OnCharacterJoined += OnCharacterJoined;
        _leftButton.onClick.AddListener(() => NavigateModel(-1));
        _rightButton.onClick.AddListener(() => NavigateModel(1));
        _selectButton.onClick.AddListener(OnSelect);
        _startButton.onClick.AddListener(GoToGame);
    }

    private void OnDestroy()
    {
        CharacterManager.OnCharacterJoined -= OnCharacterJoined;
        _leftButton.onClick.RemoveListener(() => NavigateModel(-1));
        _rightButton.onClick.RemoveListener(() => NavigateModel(1));
        _selectButton.onClick.RemoveListener(OnSelect);
        _startButton.onClick.RemoveListener(GoToGame);
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
        _startButton.interactable = true;
        _startButton.Select();
    }

    private void GoToGame()
    {
        //TODO transition effect
        SceneManager.LoadScene(Data.GAME_SCENE);
    }
}
