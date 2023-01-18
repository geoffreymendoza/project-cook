using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeUIMesh;
    private Timer _timer;
    public Timer GetTimer() => _timer;

    private bool _init = false;
    public void Initialize(float duration)
    {
        if (_init) return;
        _timer = new Timer(duration);
        _timer.OnUpdateTimeUI += OnUpdateTimeUI;
        _timer.OnTimerDone += OnTimerDone;
        _init = true;
    }

    private void OnUpdateTimeUI(float duration)
    {
        var timeText = $"Time: {duration:F0}";
        _timeUIMesh.text = timeText;
    }
    
    private void OnTimerDone()
    {
        //PAUSE ALL ACTIONS AND END GAME
        //CALL GAME MANAGER TO END LEVEL, SAVE SCORE AND GO TO RESULTS SCREEN
        Debug.Log("FINISHED LEVEL");
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);
    }
}
