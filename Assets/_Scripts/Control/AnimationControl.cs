using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator _anim;
    private Vector3 _velocity;
    private Rigidbody _rb;

    private void Awake()
    {
        Entity.OnInitializeEntity += OnInitializeEntity;
    }

    private void OnInitializeEntity(InitEntityData entity)
    {
        Entity.OnInitializeEntity -= OnInitializeEntity;
        _anim = entity.Anim;
        _rb = entity.Rigidbody;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }
    
    private void UpdateAnimation()
    {
        _velocity = _rb.velocity;
        var magnitude = _velocity.magnitude;
        magnitude = Mathf.Clamp01(magnitude);
        _anim.SetFloat(Data.ANIM_NAVIGATION_SPEED, magnitude);
    }
}
