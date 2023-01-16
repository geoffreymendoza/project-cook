using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerManager
{
    //TODO only one time init to object pool
    public static TimerBehaviour GetTimerBehaviour()
    {
        var instance = new GameObject("Timer_Behaviour");
        var timer = instance.AddComponent<TimerBehaviour>();
        return timer;
    }
}

public class Timer
{
    private float _remainingSeconds;
    public event Action OnTimerDone;
    public Timer(float duration)
    {
        _remainingSeconds = duration;
    }

    public void Tick(float deltaTime)
    {
        if (_remainingSeconds <= 0f)
            return;
        _remainingSeconds -= deltaTime;
        CheckForTimerEnd();
    }

    private void CheckForTimerEnd()
    {
        if (_remainingSeconds > 0)
            return;
        _remainingSeconds = 0f;
        OnTimerDone?.Invoke();
    }
}
