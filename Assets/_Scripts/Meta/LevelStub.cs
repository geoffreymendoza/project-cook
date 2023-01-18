using System;
using UnityEngine;

public class LevelStub : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool _isDebugging;
    [SerializeField] private LevelType _levelToDebug;
    [Header("Current Scene")]
    [SerializeField] private SceneID _sceneID;

    private void Start()
    {
        if (_isDebugging)
        {
            LevelManager.LevelToLoad(_levelToDebug);
            GameManager.StartGame(SceneID.Game);
            return;
        }
        
        GameManager.StartGame(_sceneID);
    }
}