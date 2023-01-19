using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DelayEndBehaviour : MonoBehaviour
{
    private Timer _timer;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private TextMeshProUGUI _textMesh;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new Timer(_duration);
        _textMesh.text = "TIMES UP!";
        _timer.OnTimerDone += OnTimerDone;
        _timer.OnTimerDone += GameManager.ShowResults;
    }

    private void OnTimerDone()
    {
        _timer.OnTimerDone -= OnTimerDone;
        _timer.OnTimerDone -= GameManager.ShowResults;
        // _textMesh.text = "";
        // Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _timer.Tick(Time.deltaTime);
    }
}
