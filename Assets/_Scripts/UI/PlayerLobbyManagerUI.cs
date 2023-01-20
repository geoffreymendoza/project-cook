using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class PlayerLobbyManagerUI : MonoBehaviour
{
    [SerializeField] private InputSystemUIInputModule[] _playerInputModules;
    [SerializeField] private Canvas[] _playerCanvas;

    private void Awake()
    {
        CharacterManager.OnCharacterJoined += OnCharacterJoined;
    }
    
    private void OnDestroy()
    {
        CharacterManager.OnCharacterJoined -= OnCharacterJoined;
    }

    private void OnCharacterJoined(int playersCount)
    {
        var index = playersCount - 1;
        var player = CharacterManager.Instance.GetPlayer(index);
        player.TryGetComponent(out InputController controller);
        controller.AssignUIInputModule(_playerInputModules[index]);
        _playerCanvas[index].enabled = true;
    }
}