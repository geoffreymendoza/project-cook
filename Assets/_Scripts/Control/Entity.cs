using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public static event Action<InitEntityData> OnInitializeEntity; 
    [SerializeField] protected EntityType _type;
    [SerializeField] protected float _moveSpeed = 4;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Animator _anim;
    
    //TODO dash mechanic
    protected bool _canDash = true;
    protected bool _isDashing = false;
    [SerializeField] protected float _dashPower = 24f;
    protected float _dashTime = 0.2f;
    protected float _dashCooldown = 1f;
    
    private WaitForSeconds _waitForDashTime;
    private WaitForSeconds _waitForDashCooldown;


    protected virtual void Initialize()
    {
        _waitForDashTime = new WaitForSeconds(_dashTime);
        _waitForDashCooldown = new WaitForSeconds(_dashCooldown);
        var data = new InitEntityData(this, _type, _rb,_anim);
        OnInitializeEntity?.Invoke(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual void MoveEntity(Vector3 direction)
    {
        if (_isDashing) return;
        
        //TODO make smoother movement
        _rb.velocity = direction * _moveSpeed;
    }

    public virtual void RotateEntity(Vector3 direction)
    {
        
    }

    public virtual void DashEntity(Vector3 direction)
    {
        if(_canDash)
            StartCoroutine(Dash(direction));
    }

    private IEnumerator Dash(Vector3 direction)
    {
        // yield return;
        _canDash = false;
        _isDashing = true;
        _rb.velocity = direction * _dashPower;
        yield return _waitForDashTime;
        _isDashing = false;
        yield return _waitForDashCooldown;
        _canDash = true;
    }
}

public class InitEntityData
{
    public readonly Entity Entity;
    public readonly EntityType Type;
    public readonly Rigidbody Rigidbody;
    public readonly Animator Anim;

    public InitEntityData(Entity entity, EntityType type, Rigidbody rb,Animator anim)
    {
        Entity = entity;
        Type = type;
        Rigidbody = rb;
        Anim = anim;
    }
}