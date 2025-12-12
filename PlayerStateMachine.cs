using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(JumpComponent))]
[RequireComponent(typeof(ClimbComponent))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(InputHandler))]
public class PlayerStateMachine : MonoBehaviour
{
    // References to components
    public Rigidbody2D Rb { get; private set; }
    public AnimationController Anim { get; private set; }
    public GroundChecker Ground { get; private set; }
    public ClimbChecker Climb { get; private set; }
    public MovementComponent Movement { get; private set; }
    public JumpComponent Jump { get; private set; }
    public ClimbComponent ClimbMovement { get; private set; }
    public InputHandler Input { get; private set; }

    // references to states
    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerClimbState ClimbState { get; private set; }
    public PlayerDashState PlayerDashState { get; private set; }

    // current state
    private PlayerBaseState currentState;

    public bool HasUsedAbility { get; set; } = false;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Ground = GetComponent<GroundChecker>();
        Climb = GetComponent<ClimbChecker>();
        Movement = GetComponent<MovementComponent>();
        Jump = GetComponent<JumpComponent>();
        ClimbMovement = GetComponent<ClimbComponent>();
        Anim = GetComponentInChildren<AnimationController>();
        Input = GetComponent<InputHandler>();

        // instantiate states
        GroundedState = new PlayerGroundedState(this);
        AirState = new PlayerAirState(this);
        ClimbState = new PlayerClimbState(this);
        PlayerDashState = new PlayerDashState(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // charge saved game
        if (GameManager.Instance.IsLoadingContinue && GameManager.Instance.HasSaveFile())
        {
            // move the player to the saved position
            Vector3 savedPos = GameManager.Instance.GetSavedPosition();
            transform.position = savedPos;

            // camera position
            if (Camera.main != null)
            {
                Vector3 camPos = GameManager.Instance.GetSavedCameraPosition();
                Camera.main.transform.position = camPos;
            }

            GameManager.Instance.LoadFalls();

            GameManager.Instance.LoadAbilities(GetComponent<AbilityManager>());
        }
        else
        {
            GameManager.Instance.ClearSave();
        }

        currentState = GroundedState;
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
        Anim.SetVelocityY(Rb.velocity.y);
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

    // automatic save when quitting the game
    private void OnApplicationQuit()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            FindObjectOfType<GameManager>().SaveGame(transform.position, GetComponent<AbilityManager>());
            Debug.Log("Game saved successfully.");
        }
    }
}
