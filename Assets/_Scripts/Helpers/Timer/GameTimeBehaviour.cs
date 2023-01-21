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
        int minutes = Mathf.FloorToInt(duration / 60F);
        int seconds = Mathf.FloorToInt(duration - minutes * 60);
        string time = string.Format("{0:0}:{1:00}", minutes, seconds);
        var timeText = $"Time: {time}";
        _timeUIMesh.text = timeText;
    }
    
    private void OnTimerDone()
    {
        //PAUSE ALL ACTIONS AND END GAME
        //CALL GAME MANAGER TO END LEVEL, SAVE SCORE AND GO TO RESULTS SCREEN
        GameManager.FinishedLevel();
        Debug.Log("FINISHED LEVEL");
    }

    private void Update()
    {
        if (!_init) return;
        _timer.Tick(Time.deltaTime);
    }
}
