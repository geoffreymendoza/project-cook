using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterManager : PersistentSingleton<CharacterManager>
{
    public static event Action<int> OnCharacterJoined; 

    private List<Entity> _players = new List<Entity>();
    private int _currentPlayers = 0;
    private int _maxPlayers = 2;

    public int CurrentPlayersCount => _players.Count;
    public Entity GetPlayer(int index) => _players[index];
    
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private Transform[] _gameSpawnPoints;
    [SerializeField] private Transform[] _mainMenuPoints;

    protected override void Awake()
    {
        base.Awake();
        LobbyUI.OnLobbyPopup += OnLobbyPopup;
        GameManager.OnLevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted()
    {
        foreach (var p in _players)
        {
            Destroy(p.gameObject);
        }
        _players.Clear();
        _currentPlayers = 0;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LobbyUI.OnLobbyPopup -= OnLobbyPopup;
        GameManager.OnLevelCompleted -= OnLevelCompleted;
    }

    private void OnLobbyPopup()
    {
        Instantiate(_playerInputManager);
    }

    public void JoinCharacter(Entity entity)
    {
        if (_currentPlayers >= _maxPlayers) return;
        _players.Add(entity);
        _currentPlayers++;
        SetMainMenuPosition();
        OnCharacterJoined?.Invoke(_currentPlayers);
        Debug.Log($"Character's Count: {_players.Count}");
    }

    public void SpawnToGameScene()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].transform.position = _gameSpawnPoints[i].position;
        }
    }
    
    private void SetMainMenuPosition()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].transform.position = _mainMenuPoints[i].position;
        }
    }
}