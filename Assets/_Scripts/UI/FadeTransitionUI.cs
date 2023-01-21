using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTransitionUI : MonoBehaviour
{
    public static event Action OnFinishedFadeIn;
    
    [SerializeField] private Animator _animator;
    private string _sceneToLoad;
    
    // Start is called before the first frame update
    void Start()
    {
        // _animator.Play("fade in");
        
        Invoke(nameof(InvokeOnFinishedFadeIn), 2f);
    }

    private void InvokeOnFinishedFadeIn()
    {
        OnFinishedFadeIn?.Invoke();
    }

    public void EnterScene()
    {
        _animator.Play(Data.ANIM_FADE_IN);
    }

    public void ProceedToScene(string sceneName)
    {
        _animator.Play(Data.ANIM_FADE_OUT);
        _sceneToLoad = sceneName;
        Invoke(nameof(LoadScene), 1f);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
