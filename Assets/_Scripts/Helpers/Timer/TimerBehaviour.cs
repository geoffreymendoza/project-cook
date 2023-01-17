using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBehaviour : MonoBehaviour
{
    private Timer _timer;
    private bool _showUI = false;
    private Vector3 _spawnPosition;

    [SerializeField] private TimeDurationUI _timeUI;
    
    private bool _init = false;
    public void Initialize(float duration, bool showUI, Vector3 spawnPos, Action onTimerDoneAction = null)
    {
        if(_init) return;
        _showUI = showUI;
        _spawnPosition = spawnPos;
        _timer = new Timer(duration);
        _timer.OnTimerDone += onTimerDoneAction;
        _timer.OnTimerDone += OnTimerDone;
        if (_showUI)
        {
            var timeUI = Instantiate(_timeUI);
            timeUI.Initialize(duration, _spawnPosition, _timer);
        }
        _init = true;
    }

    private void OnTimerDone()
    {
        _timer.OnTimerDone -= OnTimerDone;
        //TODO return to the timer manager class
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_init) return;
        //TODO pause if character walk away or in game paused
        
        _timer.Tick(Time.deltaTime);
    }
}
