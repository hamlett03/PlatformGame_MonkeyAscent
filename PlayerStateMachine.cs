using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(JumpComponent))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(InputHandler))]
public class PlayerStateMachine : MonoBehaviour
{
    // References to components
    public Rigidbody2D Rb { get; private set; }
    public AnimationController Anim { get; private set; }
    public GroundChecker Ground { get; private set; }
    public MovementComponent Movement { get; private set; }
    public JumpComponent Jump { get; private set; }
    public InputHandler Input { get; private set; }

    // references to states
    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAirState AirState { get; private set; }

    // current state
    private PlayerBaseState currentState;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Ground = GetComponent<GroundChecker>();
        Movement = GetComponent<MovementComponent>();
        Jump = GetComponent<JumpComponent>();
        Anim = GetComponentInChildren<AnimationController>();
        Input = GetComponent<InputHandler>();

        // instantiate states
        GroundedState = new PlayerGroundedState(this);
        AirState = new PlayerAirState(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GroundedState;
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
        Anim.SetVelocityY(Rb.velocity.y);

        Input.ResetInputs();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
