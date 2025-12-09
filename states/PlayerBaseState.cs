using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    // references to states and components 
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected AnimationController anim;
    protected GroundChecker ground;
    protected MovementComponent movement;
    protected JumpComponent jump;
    protected ClimbChecker climbChecker;
    protected ClimbComponent climbComponent;

    // constructor
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        // get component references
        rb = stateMachine.Rb;
        anim = stateMachine.Anim;
        ground = stateMachine.Ground;
        movement = stateMachine.Movement;
        jump = stateMachine.Jump;
        climbChecker = stateMachine.Climb;
        climbComponent = stateMachine.ClimbMovement;
    }

    // methos to be implemented by derived states
    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}
