using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClimbComponent : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3.0f;

    private Rigidbody2D rb;
    
    private float originalGravityScale;
    private float nextClimbTime = 0f;
    private bool wasClimbSoundPlaying = false;

    private JumpComponent jumpComponent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpComponent = GetComponent<JumpComponent>();
    }

    // cooldown
    public void SetClimbCooldown(float duration)
    {
        nextClimbTime = Time.time + duration;
    }

    public bool CanClimb()
    {
        return Time.time >= nextClimbTime;
    }

    // called from the player climb state to start climbing
    public void StartClimbing()
    {
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

        if (jumpComponent != null)
        {
            jumpComponent.ResetJumpState();
        }
    }

    // called from the player climb state to move up/down
    public void HandleClimb(float verticalInput)
    {
        rb.velocity = new Vector2(0, verticalInput * climbSpeed);
        // audio
        bool isClimbing = Mathf.Abs(verticalInput) > 0.1f;

        if (isClimbing && !wasClimbSoundPlaying)
        {
            SoundManager.Instance.StartClimbSound();
            wasClimbSoundPlaying = true;
        }
        else if (!isClimbing)
        {
            SoundManager.Instance.StopClimbSound();
            wasClimbSoundPlaying = false;
        }
    }

    // called from the playerClimbState when exiting
    public void StopClimbing()
    {
        rb.gravityScale = originalGravityScale;

        if (wasClimbSoundPlaying)
        {
            SoundManager.Instance.StopClimbSound();
            wasClimbSoundPlaying = false;
        }
    }
}