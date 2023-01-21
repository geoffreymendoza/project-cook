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
        var fadeTransition = UIManager.GetUIObject<FadeTransitionUI>(UIType.FadeTransition);
        fadeTransition.EnterScene();
        
        if (CharacterManager.Instance == null)
        {
            var data = DataManager.GetSpawnData(SpawnType.CharacterManager);
            Instantiate(data.Prefab);
        }

        if (AudioManager.instance == null)
        {
            var data = DataManager.GetSpawnData(SpawnType.AudioManager);
            Instantiate(data.Prefab);
        }

        if (_isDebugging)
        {
            LevelManager.LevelToLoad(_levelToDebug);
            var data = DataManager.GetSpawnData(SpawnType.PlayerInputManager);
            Instantiate(data.Prefab);
            GameManager.InitializeScene(SceneID.Game);
            return;
        }
        
        GameManager.InitializeScene(_sceneID);
    }

    [ContextMenu("SpawnPlayerToScene")]
    public void SpawnPlayerToScene()
    {
        CharacterManager.Instance.SpawnToGameScene();
    }
}