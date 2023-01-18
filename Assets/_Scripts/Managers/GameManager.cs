
using System.Collections.Generic;
using System.Linq;

public static class GameManager
{
    private static bool _inGame = false;
    private static GameTimeBehaviour _currentGameTime;

    public static void StartGame(SceneID scene)
    {
        if (scene != SceneID.Game)
        {
            _inGame = false;
            return;
        }
        _inGame = true;
        LevelManager.SpawnLevelToScene();
        
        var data = DataManager.GetLevelData(LevelManager.GetCurrentLevel());
        List<RecipeBags> recipeList = data.Orders.Select(rd => 
                                        DataManager.GetRecipeData(rd.RecipeType)).ToList();
        RecipeSystem.AddRecipeList(recipeList);
        OrderSystem.AddRecipeList(recipeList);

        //TODO delay 3 seconds before start playing
        var mainCanvas = UIManager.GetMainCanvas();
        _currentGameTime = UIManager.GetUIObject<GameTimeBehaviour>(UIType.GameTimeUI);
        _currentGameTime.transform.SetParent(mainCanvas.transform,false);
        _currentGameTime.Initialize(LevelManager.GetCurrentLevelDuration());
        
        OrderSystem.ResetOrdersList();
        OrderSystem.StartOrdering();
        ScoreSystem.StartScoring(_inGame);
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
    
}