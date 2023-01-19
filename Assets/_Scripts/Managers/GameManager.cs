
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static event Action<bool> OnPaused;
    public static event Action OnLevelCompleted; 

    private static bool _inGame = false;
    private static GameTimeBehaviour _currentGameTime;

    public static void InitializeScene(SceneID scene)
    {
        var mainCanvas = UIManager.GetMainCanvas();

        switch (scene)
        {
            case SceneID.MainMenu:
                break;
            case SceneID.Game:
                SetupGameScene();
                break;
            case SceneID.Results:
                var resultsUI = UIManager.GetUIObject<ResultsUI>(UIType.Results);
                resultsUI.transform.SetParent(mainCanvas.transform,false);
                break;
            case SceneID.LevelSelection:
                mainCanvas = UIManager.GetMainCanvas();
                var levelSelectionUI = UIManager.GetUIObject<LevelSelectionUI>(UIType.LevelSelection);
                levelSelectionUI.transform.SetParent(mainCanvas.transform,false);
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
        var mainCanvas = UIManager.GetMainCanvas();
        var delayUI = UIManager.GetUIObject<DelayEndBehaviour>(UIType.DelayEnd);
        delayUI.transform.SetParent(mainCanvas.transform,false);
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