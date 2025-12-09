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

        stateMachine.ClimbMovement.HandleClimb(yInput);

        // if jump is pressed, charge a jump
        if (!stateMachine.Climb.IsClimbable())
        {
            stateMachine.ChangeState(stateMachine.AirState);
        }

        if (!stateMachine.Climb.IsClimbable())
        {
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }

    public override void FixedUpdate() {}

    public override void Exit() 
    {
        anim.SetClimbing(false);
        stateMachine.ClimbMovement.StopClimbing();
    }
}