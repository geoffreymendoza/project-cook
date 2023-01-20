using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour
{
    public static event Action<InitEntityData> OnInitializeEntity; 
    [SerializeField] protected EntityType _type;
    [SerializeField] protected float _moveSpeed = 4;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Animator _anim;
    
    //TODO assign values on a scriptable object
    [Header("Dash Values")]
    protected bool _canDash = true;
    protected bool _isDashing = false;
    [SerializeField] protected float _dashPower = 24f;
    protected float _dashTime = 0.2f;
    [SerializeField] protected float _dashCooldown = 1f;
    
    public bool CanMove { protected set; get; } = false;

    private WaitForSeconds _waitForDashTime;
    private WaitForSeconds _waitForDashCooldown;

    private InitEntityData _data;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
        _data = new InitEntityData(this, _type, _rb,_anim);
    }

    protected bool _init = false;
    protected virtual void Initialize()
    {
        if (_init) return;
        _waitForDashTime = new WaitForSeconds(_dashTime);
        _waitForDashCooldown = new WaitForSeconds(_dashCooldown);
        OnInitializeEntity?.Invoke(_data);
        _init = true;
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

    public virtual bool DashEntity(Vector3 direction)
    {
        if (_canDash)
        {
            StartCoroutine(Dash(direction));
            return true;
        }
        return false;
    }

    private IEnumerator Dash(Vector3 direction)
    {
        _canDash = false;
        _isDashing = true;
        _rb.velocity = direction * _dashPower;
        yield return _waitForDashTime;
        _isDashing = false;
        yield return _waitForDashCooldown;
        _canDash = true;
    }

    public virtual void SpawnDashEffect()
    {
        
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
