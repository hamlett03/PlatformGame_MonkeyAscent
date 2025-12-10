using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    // constructor
    public PlayerAirState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter() {}

    public override void Update()
    {
        var input = stateMachine.Input;
        // component while in air
        jump.ApplyExtraGravity(input.JumpHeld);
        jump.CheckHardLanding();

        //Debug.Log("In Air State");
    }

    public override void FixedUpdate() {
        // climb
        if (stateMachine.Climb.IsClimbable() && Mathf.Abs(stateMachine.Input.VerticalInput) > 0.1f && stateMachine.ClimbMovement.CanClimb())
        {
            stateMachine.ChangeState(stateMachine.ClimbState);
            return;
        }

        // transition to grounded state
        if (ground.IsGrounded() && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.GroundedState);
        }
    }

    public override void Exit() {}
}
