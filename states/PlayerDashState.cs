using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float dashTime = 0.2f;
    private float dashSpeed = 20f;
    private float curbDashSpeed = 0.5f;
    
    private float timer;
    private float originalGravity;
    private float dashDirection;

    // constructor
    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) 
    {
    }

    public override void Enter()
    {
        anim.TriggerDash();

        SoundManager.Instance.PlayDashSound();

        // store original gravity and set to 0
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // determine dash direction based on input
        float xInput = stateMachine.Input.HorizontalInput;

        if (Mathf.Abs(xInput) > 0.1f)
        {
            dashDirection = Mathf.Sign(xInput);
        }
        else
        {
            dashDirection = stateMachine.transform.localScale.x;
        }

        stateMachine.transform.localScale = new Vector3(dashDirection, 1, 1);

        // apply velocity
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        timer = dashTime;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // transition to air state
            stateMachine.ChangeState(stateMachine.AirState);
        }
    }

    public override void FixedUpdate() {}

    public override void Exit()
    {
        // ensure gravity is restored
        rb.gravityScale = originalGravity;
        rb.velocity = new Vector2(rb.velocity.x * curbDashSpeed, rb.velocity.y);
    }
}
