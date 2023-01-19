using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DelayStartBehaviour : MonoBehaviour
{
    private Timer _timer;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private TextMeshProUGUI _textMesh;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new Timer(_duration);
        _textMesh.text = "READY?";
        _timer.OnTimerDone += OnTimerDone;
        _timer.OnTimerDone += GameManager.StartGame;
    }

    private void OnTimerDone()
    {
        _timer.OnTimerDone -= OnTimerDone;
        _timer.OnTimerDone -= GameManager.StartGame;
        _textMesh.text = "GO!";
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        _timer.Tick(Time.deltaTime);
    }
}
