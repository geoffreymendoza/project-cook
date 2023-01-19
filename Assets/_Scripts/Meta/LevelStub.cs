using System;
using UnityEngine;

public class LevelStub : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool _isDebugging;
    [SerializeField] private LevelType _levelToDebug;
    [Header("Current Scene")]
    [SerializeField] private SceneID _sceneID;

    private Action OnSomething;

    private void Start()
    {
        if (CharacterManager.Instance == null)
        {
            var data = DataManager.GetSpawnData(SpawnType.CharacterManager);
            Instantiate(data.Prefab);
        }
        
        if (_isDebugging)
        {
            LevelManager.LevelToLoad(_levelToDebug);
            GameManager.InitializeScene(SceneID.Game);
            return;
        }
        
        GameManager.InitializeScene(_sceneID);
    }
}