using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TimerBehaviour : MonoBehaviour
{
    private Timer _timer;
    private bool _showUI = false;

    [SerializeField] private TimeDurationUI _timeUIPrefab;

    private TimeDurationUI _currentTimeUI;
    public TimeDurationUI GetCurrentTimeUI() => _currentTimeUI;
    private bool _interrupted = false;
    
    private bool _init = false;
    public void Initialize(float duration, bool showUI, Transform parent, Action onTimerDoneAction = null)
    {
        if(_init) return;
        _showUI = showUI;
        _timer = new Timer(duration);
        _timer.OnTimerDone += onTimerDoneAction;
        _timer.OnTimerDone += OnTimerDone;
        if (_showUI)
        {
            _currentTimeUI = Instantiate(_timeUIPrefab);
            _currentTimeUI.Initialize(duration, parent, _timer);
        }
        _init = true;
    }

    public void Interrupted(bool value)
    {
        _interrupted = value;
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
        if (_interrupted) return;
        _timer.Tick(Time.deltaTime);
    }
}
