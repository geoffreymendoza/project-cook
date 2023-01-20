using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEntity : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    private void Awake()
    {
        OrderSystem.OnOrderComplete += OnOrderComplete;
    }

    private void OnDestroy()
    {
        OrderSystem.OnOrderComplete -= OnOrderComplete;
    }

    private void OnOrderComplete(bool correctOrder)
    {
        _anim.SetTrigger(Data.ANIM_POSE);
    }
}
