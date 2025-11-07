using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    // constructor
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        anim.SetGrounded(true);
    }

    public override void Update()
    {
        // input
        var input = stateMachine.Input;
        float xInput = input.HorizontalInput;

        // movement
        movement.Move(xInput);
        // flip sprite
        movement.FlipSprite(xInput);
        // animation updates
        anim.SetWalking(Mathf.Abs(xInput) > 0.1f && ground.IsGrounded());

        // jump
        if (input.JumpPressed)
        {
            jump.StartChargingJump(xInput);
        }
        else if (jump.IsCharging && input.JumpHeld)
        {
            jump.ContinueChargingJump(xInput);
        }
        else if (jump.IsCharging && input.JumpReleased)
        {
            jump.RealeseJump();
        }

        // change state
        if (!ground.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }

    public override void FixedUpdate() {}

    public override void Exit()
    {
        anim.SetGrounded(false);
    }
}
