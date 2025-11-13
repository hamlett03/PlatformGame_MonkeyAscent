using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    private ClimbChecker climbChecker;
    private ClimbComponent climbComponent;

    // constructor
    public PlayerClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
    }

    public override void FixedUpdate() {}
    public override void Exit() {}
}