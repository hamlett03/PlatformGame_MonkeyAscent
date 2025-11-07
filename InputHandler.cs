using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }

    private void Update()
    {
        ResetInputs();
        
        #if UNITY_EDITOR || UNITY_STANDALONE
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            JumpHeld = Input.GetButton("Jump");
            
            if (Input.GetButtonDown("Jump"))
                JumpPressed = true;

            if (Input.GetButtonUp("Jump"))
                JumpReleased = Input.GetButtonUp("Jump");

        #elif UNITY_IOS || UNITY_ANDROID

        #endif
    }

    public void OnMove(float direction) => HorizontalInput = direction;
    public void OnJumpPressed() => JumpPressed = true;
    public void OnJumpHeld() => JumpHeld = true;
    public void OnJumpReleased() => JumpReleased = true;

    private void ResetInputs()
    {
        JumpPressed = false;
        JumpReleased = false;
    }
}
