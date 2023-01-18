using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceSliderUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private Timer _currentTimer;

    private void OnApplicationQuit()
    {
        _currentTimer.OnUpdateTimeUI -= OnUpdateUI;
        _currentTimer.OnTimerDone -= OnTimerDone;
    }

    public void Initialize(float startDuration, Transform parent, Timer timer)
    {
        _slider.direction = Slider.Direction.RightToLeft;
        // this.transform.position = position;
        transform.SetParent(parent, false);
        transform.position = parent.position;
        _slider.maxValue = startDuration;
        _currentTimer = timer;
        _currentTimer.OnUpdateTimeUI += OnUpdateUI;
        _currentTimer.OnTimerDone += OnTimerDone;
    }

    private void OnTimerDone()
    {
        Destroy(this.gameObject);
    }

    private void OnUpdateUI(float duration)
    {
        _slider.value = duration;
    }
    
    public void UpdateSlider(float value)
    {
        _slider.maxValue += value;
        _currentTimer.ExtendDuration(value);
        // Debug.Log(value);
    }
}
