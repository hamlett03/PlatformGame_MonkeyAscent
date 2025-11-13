using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClimbComponent : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3.0f;

    private Rigidbody2D rb;
    private float originalGravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    // called from the player climb state to start climbing
    public void StartClimbing()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    // called from the player climb state to move up/down
    public void Climb(float verticalInput)
    {
        rb.velocity = new Vector2(0, verticalInput * climbSpeed);
    }

    // called from the playerClimbState when exiting
    public void StopClimbing()
    {
        rb.gravityScale = originalGravityScale;
    }
}