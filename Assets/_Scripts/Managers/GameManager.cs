
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static event Action<bool> OnPaused;
    public static event Action OnLevelCompleted; 

    private static bool _inGame = false;
    private static GameTimeBehaviour _currentGameTime;

    public static void InitializeScene(SceneID scene)
    {
        AudioManager.instance.StopAll();
        var mainCanvas = UIManager.GetMainCanvas();
        switch (scene)
        {
            case SceneID.MainMenu:
                AudioManager.instance.Play(SoundType.MainMenuBGM);
                break;
            case SceneID.Game:
                SetupGameScene();
                AudioManager.instance.Play(SoundType.InGameBGM);
                break;
            case SceneID.Results:
                var resultsUI = UIManager.GetUIObject<ResultsUI>(UIType.Results);
                resultsUI.transform.SetParent(mainCanvas.transform,false);
                AudioManager.instance.Play(SoundType.ResultBGM);
                break;
            case SceneID.LevelSelection:
                mainCanvas = UIManager.GetMainCanvas();
                var levelSelectionUI = UIManager.GetUIObject<LevelSelectionUI>(UIType.LevelSelection);
                levelSelectionUI.transform.SetParent(mainCanvas.transform,false);
                AudioManager.instance.Play(SoundType.MainMenuBGM);
                break;
        }
    }

    private static void SetupGameScene()
    {
        CharacterManager.Instance.SpawnToGameScene();
        LevelManager.SpawnLevelToScene();
        var data = DataManager.GetLevelData(LevelManager.GetCurrentLevel());
        List<RecipeBags> recipeList = data.Orders.Select(rd => 
            DataManager.GetRecipeData(rd.RecipeType)).ToList();
        RecipeSystem.AddRecipeList(recipeList);
        OrderSystem.AddRecipeList(recipeList);
        var mainCanvas = UIManager.GetMainCanvas();
        _currentGameTime = UIManager.GetUIObject<GameTimeBehaviour>(UIType.GameTime);
        _currentGameTime.transform.SetParent(mainCanvas.transform,false);
        var delayUI = UIManager.GetUIObject<DelayStartBehaviour>(UIType.DelayStart);
        delayUI.transform.SetParent(mainCanvas.transform,false);
    }

    public static void StartGame()
    {
        _currentGameTime.Initialize(LevelManager.GetCurrentLevelDuration());
        OrderSystem.ResetOrdersList();
        OrderSystem.StartOrdering();
        ScoreSystem.StartScoring();
        OnPaused?.Invoke(false);
    }

    public static void FinishedLevel()
    {
        InvokePause();
        CompareHighScore();
        
        var mainCanvas = UIManager.GetMainCanvas();
        var delayUI = UIManager.GetUIObject<DelayEndBehaviour>(UIType.DelayEnd);
        delayUI.transform.SetParent(mainCanvas.transform,false);
    }

    private static void CompareHighScore()
    {
        //Save
        LevelType currentLevel = LevelManager.GetCurrentLevel();
        Debug.Log((int)currentLevel);
        int currentScore = ScoreSystem.GetCurrentScore();
        //load existing
        var previousSave = SaveAndLoadSystem.LoadGame();
        foreach (var lvl in previousSave.Levels)
        {
            if (currentLevel != lvl.Type || currentScore <= lvl.HighScore) continue;
            Debug.Log("new high score");
            LevelData levelData = new LevelData(currentLevel, currentScore);
            SaveAndLoadSystem.SaveGame(levelData);
            break;
        }
        //unlock next level
        LevelData nextLevelUnlocked = new LevelData(currentLevel + 1, true);
        SaveAndLoadSystem.SaveGame(nextLevelUnlocked);
    }

    public static void ShowResults()
    {
        SceneManager.LoadScene(Data.RESULTS_SCENE);
        OnLevelCompleted?.Invoke();
    }

    public static void InvokePause()
    {
        OnPaused?.Invoke(true);
    }

    public static bool StillInGame()
    {
        if (_currentGameTime == null) return false;
        return !_currentGameTime.GetTimer().TimesUp;
    }
}

public enum SceneID
{
    None,
    MainMenu,
    Game,
    Results,
    LevelSelection
    
}