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
        // component while in air
        jump.ApplyExtraGravity();
        jump.CheckHardLanding();

        // transition to grounded state
        if (ground.IsGrounded() && rb.velocity.y < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.GroundedState);
        }
    }

    public override void FixedUpdate() {}

    public override void Exit() {}
}
