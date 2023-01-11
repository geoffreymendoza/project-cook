using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static event Action<FrameInput> OnInput;
    private FrameInput _input;

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        _input = new FrameInput
        {
            //TODO integrate new input system for easier joystick compatibility
            Horizontal = Input.GetAxisRaw("Horizontal"),
            Vertical = Input.GetAxisRaw("Vertical"),
            PickupOrDrop = Input.GetKeyDown(KeyCode.Space),
            Interact = Input.GetKeyDown(KeyCode.LeftControl),
            Dash = Input.GetKeyDown(KeyCode.Space)
        };
        OnInput?.Invoke(_input);
    }
}

public struct FrameInput
{
    public float Horizontal;
    public float Vertical;
    public bool PickupOrDrop;
    public bool Interact;
    public bool Dash;
}
