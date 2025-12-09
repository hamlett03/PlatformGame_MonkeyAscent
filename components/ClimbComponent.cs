using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClimbComponent : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3.0f;
    [SerializeField] private float soundInterval = 0.3f;
    private Rigidbody2D rb;
    private float originalGravityScale;

    private float soundTimer = 0f;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // called from the player climb state to start climbing
    public void StartClimbing()
    {
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
    }

    // called from the player climb state to move up/down
    public void HandleClimb(float verticalInput)
    {
        rb.velocity = new Vector2(0, verticalInput * climbSpeed);
        // audio
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0f)
            {
                SoundManager.Instance.PlayClimb();
                soundTimer = soundInterval;
            }
        }
    }

    // called from the playerClimbState when exiting
    public void StopClimbing()
    {
        rb.gravityScale = originalGravityScale;
    }
}