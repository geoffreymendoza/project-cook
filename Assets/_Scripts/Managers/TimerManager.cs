using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerManager
{
    //TODO only one time init to object pool
    public static WorldSpaceTimerBehaviour GetWorldSpaceTimerBehaviour()
    {
        var timer = UIManager.GetUIObject<WorldSpaceTimerBehaviour>(UIType.WorldSpaceTimeDuration);
        return timer;
    }

    public static WorldSpaceTimerBehaviour GetScreenSpaceTimerBehaviour(UIType type)
    {
        var timer = UIManager.GetUIObject<WorldSpaceTimerBehaviour>(type);
        return timer;
    }
}

public class Timer
{
    private float _remainingTime;
    public bool TimesUp => _remainingTime <= 0;
    public event Action<float> OnUpdateTimeUI;
    public event Action OnTimerDone;
    public Timer(float duration)
    {
        _remainingTime = duration;
    }
    
    public void ExtendDuration(float duration)
    {
        _remainingTime += duration;
    }

    public void Tick(float deltaTime)
    {
        if (_remainingTime <= 0f)
            return;
        _remainingTime -= deltaTime;
        OnUpdateTimeUI?.Invoke(_remainingTime);
        CheckForTimerEnd();
    }

    private void CheckForTimerEnd()
    {
        if (_remainingTime > 0)
            return;
        _remainingTime = 0f;
        OnTimerDone?.Invoke();
    }
}
