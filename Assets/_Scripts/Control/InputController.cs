using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _dashAction;
    private InputAction _grabAction;
    private InputAction _interactAction;
    public event Action<FrameInput> OnInput;
    private FrameInput _input;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions[Data.INPUT_MOVE];
        _dashAction = _playerInput.actions[Data.INPUT_DASH];
        _grabAction = _playerInput.actions[Data.INPUT_GRAB];
        _interactAction = _playerInput.actions[Data.INPUT_INTERACT];
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        var direction = _moveAction.ReadValue<Vector2>();
        _input = new FrameInput
        {
            Horizontal = direction.x,
            Vertical = direction.y,
            Grab = _grabAction.triggered,
            Interact = _interactAction.triggered,
            Dash = _dashAction.triggered
        };
        OnInput?.Invoke(_input);
    }

    public void AssignUIInputModule(InputSystemUIInputModule inputModule)
    {
        _playerInput.uiInputModule = inputModule;
    }
}

public struct FrameInput
{
    public float Horizontal;
    public float Vertical;
    /// <summary>
    /// Pickup or Drop Items
    /// </summary>
    public bool Grab; 
    /// <summary>
    /// Chop or Throw Items
    /// </summary>
    public bool Interact;
    public bool Dash;
}
