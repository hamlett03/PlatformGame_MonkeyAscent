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
    }

    // methos to be implemented by derived states
    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}
