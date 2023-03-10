using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    private Entity _entity;
    private Rigidbody _rb;
    private EntityType _entityType;
    private float _moveSpeed;
    private Vector3 _direction;
    private Vector3 _prevDirection;
    
    private InputController _inputController;

    private void Awake()
    {
        _inputController = GetComponent<InputController>();
        Entity.OnInitializeEntity += OnInitializeEntity;
    }

    private void OnDestroy()
    {
        _inputController.OnInput -= OnInput;
    }

    private void OnInitializeEntity(InitEntityData data)
    {
        Entity.OnInitializeEntity -= OnInitializeEntity;
        _entity = data.Entity;
        _entityType = data.Type;
        if (_entityType == EntityType.PlayableCharacter)
        {
            _inputController.OnInput += OnInput;
        }
    }

    private void OnInput(FrameInput input)
    {
        _direction = new Vector3(input.Horizontal, 0, input.Vertical);
        if(!_entity.CanMove) return;
        if (_direction != Vector3.zero)
            _prevDirection = _direction;

        var dashDirection = GetDirection();
        bool dashing = false;
        if (input.Dash)
            dashing = _entity.DashEntity(dashDirection);
        if (dashing)
        {
            _entity.SpawnDashEffect();
        }

        _entity.MoveEntity(_direction);
    }

    // Update is called once per frame
    void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        //TODO slerp for smoother rotation
        var currentDir = GetDirection();
        this.transform.LookAt(this.transform.position + currentDir);
    }

    private Vector3 GetDirection()
    {
        return _direction != Vector3.zero ? _direction : _prevDirection;
    }
}