using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreSystem
{
    private static ScoreSheet _currentScoreSheet;
    private static ScoreSheetUI _scoreSheetUI;
    
    public static int GetCurrentScore()
    {
        return _currentScoreSheet.ScoreCount;
    }
    
    public static void StartScoring()
    {
        _currentScoreSheet = new ScoreSheet();
        var mainCanvas = UIManager.GetMainCanvas();
        _scoreSheetUI = UIManager.GetUIObject<ScoreSheetUI>(UIType.Score);
        _scoreSheetUI.transform.SetParent(mainCanvas.transform, false);
        _currentScoreSheet.OnUpdateScore += _scoreSheetUI.OnUpdateUI;
    }

    public static void UpdatePoints(int points)
    {
        _currentScoreSheet.ConstructScore(points);
    }
}

public class ScoreSheet
{
    public event Action<int> OnUpdateScore;
    private int _scoreCount;

    public int ScoreCount
    {
        get => _scoreCount;
        set => _scoreCount = (int)Mathf.Clamp(value, 0, Mathf.Infinity);
    }

    public void ConstructScore(int points)
    {
        ScoreCount += points;
        OnUpdateScore?.Invoke(ScoreCount);
    }
}
