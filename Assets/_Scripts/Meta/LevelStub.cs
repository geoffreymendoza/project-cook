using System;
using UnityEngine;

public class LevelStub : MonoBehaviour
{
    [SerializeField] private bool _isDebugging;
    [SerializeField] private LevelType _levelToDebug;
    
    private void Start()
    {
        if (_isDebugging)
        {
            LevelManager.LevelToLoad(_levelToDebug);
        }
        
        LevelManager.SpawnLevelToScene();
    }
}