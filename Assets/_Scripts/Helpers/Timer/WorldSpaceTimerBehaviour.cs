using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceTimerBehaviour : MonoBehaviour
{
    private Timer _timer;
    private bool _showUI = false;

    [SerializeField] private WorldSpaceSliderUI _timeUIPrefab;

    private WorldSpaceSliderUI _currentTimeUI;
    public WorldSpaceSliderUI GetCurrentTimeUI() => _currentTimeUI;
    private bool _interrupted = false;

    private Action _onTimerDoneAction;
    
    private bool _init = false;
    public void Initialize(float duration, bool showUI, Transform parent, Action onTimerDoneAction = null)
    {
        if(_init) return;
        _showUI = showUI;
        _timer = new Timer(duration);
        _onTimerDoneAction = onTimerDoneAction;
        _timer.OnTimerDone += _onTimerDoneAction;
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
        _timer.OnTimerDone -= _onTimerDoneAction;
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
