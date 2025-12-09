using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }

    private void LateUpdate()
    {
        ResetInputs();
    }
    
    private void ResetInputs()
    {
        JumpPressed = false;
        JumpReleased = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float xValue = context.ReadValue<Vector2>().x;
        float yValue = context.ReadValue<Vector2>().y;

        float deadzone = 0.85f;
        
        if (Mathf.Abs(xValue) > deadzone)
        {
            HorizontalInput = xValue;
        }
        else
        {
            HorizontalInput = 0f;
        }

        if (Mathf.Abs(yValue) > deadzone) 
        {
            VerticalInput = yValue;
        }
        else
        {
            VerticalInput = 0f;
        }

        // Debug.Log("Horizontal Input: " + HorizontalInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpPressed = true;;
        }
        else if (context.canceled)
        {
            JumpReleased = true;
        }

        JumpHeld = context.ReadValueAsButton();
    }
}