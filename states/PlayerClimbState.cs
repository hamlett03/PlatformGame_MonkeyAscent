using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    // constructor
    public PlayerClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        anim.SetClimbing(true);
        stateMachine.ClimbMovement.StartClimbing();
    }

    public override void Update()
    {
        // inputs
        float yInput = stateMachine.Input.VerticalInput;
        float xInput = stateMachine.Input.HorizontalInput;

        var input = stateMachine.Input;

        float cooldown = 0.5f;

        movement.FlipSprite(xInput);

        // jump off climb
        if (input.JumpPressed)
        {
            jump.StartChargingJump(xInput);
            //Debug.Log("Jumping off climb");
        } else if (jump.IsCharging && input.JumpHeld)
        {
            jump.ContinueChargingJump(xInput);

            // cooldown before next climb
            if (!jump.IsCharging)
            {
                stateMachine.ClimbMovement.SetClimbCooldown(cooldown);
                stateMachine.ChangeState(stateMachine.AirState);
                return;
            }
        }
        else if (jump.IsCharging && input.JumpReleased)
        {
            jump.RealeseJump();
            stateMachine.ClimbMovement.SetClimbCooldown(cooldown);
            stateMachine.ChangeState(stateMachine.AirState);
            return;

        }
        else
        {
            if (!jump.IsCharging)
            {
                climbComponent.HandleClimb(yInput);
            }
        }

        // if no longer climbable, go to air state
        if (!stateMachine.Climb.IsClimbable() && !jump.IsCharging)
        {
            stateMachine.ChangeState(stateMachine.AirState);
        }

        //Debug.Log("In Climb State");
    }

    public override void FixedUpdate() {}

    public override void Exit() 
    {
        anim.SetClimbing(false);
        stateMachine.ClimbMovement.StopClimbing();
    }
}