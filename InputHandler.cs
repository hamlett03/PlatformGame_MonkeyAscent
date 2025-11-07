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
#if UNITY_EDITOR || UNITY_STANDALONE
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetButtonDown("Jump");
        JumpHeld = Input.GetButton("Jump");
        JumpReleased = Input.GetButtonUp("Jump");
#elif UNITY_IOS || UNITY_ANDROID
#endif
    }

    public void OnMove(float direction) => HorizontalInput = direction;
    public void OnJumpPressed() => JumpPressed = true;
    public void OnJumpHeld() => JumpHeld = true;
    public void OnJumpReleased() => JumpReleased = true;

    public void ResetInputs()
    {
        JumpPressed = false;
        JumpReleased = false;
    }
}
