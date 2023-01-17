using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDurationUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private Action<float> _onUpdateUI;

    private void OnApplicationQuit()
    {
        _onUpdateUI -= OnUpdateUI;
    }

    public void Initialize(float startDuration, Vector3 position, Timer timer)
    {
        this.transform.position = position;
        _slider.maxValue = startDuration;
        timer.OnUpdateTimeUI += OnUpdateUI;
        timer.OnTimerDone += OnTimerDone;
    }

    private void OnTimerDone()
    {
        Destroy(this.gameObject);
    }

    private void OnUpdateUI(float duration)
    {
        _slider.value = duration;
    }
}
