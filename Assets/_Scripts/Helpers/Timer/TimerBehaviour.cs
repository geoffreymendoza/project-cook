using System;
using UnityEngine;

public class TimerBehaviour : MonoBehaviour
{
    public Timer Timer { private set; get; }

    public void Initialize(float duration)
    {
        Timer = new Timer(duration);
        //testing
        Timer.OnTimerDone += DestroyTimer;
    }

    private void Update()
    {
        Timer.Tick(Time.deltaTime);
    }

    private void DestroyTimer()
    {
        Timer.OnTimerDone -= DestroyTimer;
        Destroy(this.gameObject);
    }
}