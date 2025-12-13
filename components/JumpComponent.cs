using UnityEngine;
using System;

public class JumpComponent : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float minJumpForce = 8f;
    [SerializeField] private float maxJumpForce = 24f;
    [SerializeField] private float chargeTime = 0.7f;
    [SerializeField] private float fallMultiplier = 1.5f;
    [SerializeField] private float lowJumpMultiplier = 0.5f;
    [SerializeField] private float minparabolicAngle = 30f;
    [SerializeField] private float maxparabolicAngle = 60f;
    [SerializeField] private float maxFallSpeed = 28f;
    [SerializeField] private bool debugDrawPath = true;

    private Rigidbody2D rb;
    private GroundChecker groundChecker;
    private ClimbChecker climbChecker;
    private AnimationController anim;

    private float jumpChargeTimer = 0f;
    private float jumpDirection = 0f;
    private float gravityMagnitude;

    private bool isCharging = false;
    private bool isJumping = false;
    private bool shouldPerformJump = false;
    private bool hasPlayedHardLandSound = false;
    private bool hasLanded = false;
    
    // Cached jump calculations
    private float initialVelocityX = 0f;
    private float initialVelocityY = 0f;
    private float timeToApex = 0f;

    public event Action OnJumpStarted;
    public event Action OnJumpReleased;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<GroundChecker>();
        climbChecker = GetComponent<ClimbChecker>();
        anim = GetComponentInChildren<AnimationController>();
        gravityMagnitude = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        hasLanded = true;
    }

    public void ResetJumpState()
    {
        isJumping = false;
        isCharging = false;
        anim.SetChargingJump(false);
    }

    public void StartChargingJump(float horizontalInput)
    {
        if (isJumping || isCharging) return;

        if (groundChecker.IsGrounded() || climbChecker.IsClimbable())
        {
            isCharging = true;
            jumpChargeTimer = 0f;
            jumpDirection = Mathf.Abs(horizontalInput) > 0.1f ? Mathf.Sign(horizontalInput) : 0f;
            anim.SetChargingJump(true);
            OnJumpStarted?.Invoke();
        }
    }

    public void ContinueChargingJump(float horizontalInput)
    {
        if (!isCharging) return;
        
        jumpChargeTimer += Time.deltaTime;
        jumpDirection = Mathf.Abs(horizontalInput) > 0.1f ? Mathf.Sign(horizontalInput) : 0f;

        if (jumpChargeTimer >= chargeTime)
        {
            jumpChargeTimer = chargeTime;
            RealeseJump();
        }

        // if (debugDrawPath)
        // {
            // CalculateJumpTrajectory();
            // DrawJumpTrajectory();
        // }
    }

    public void RealeseJump()
    {
        if (!isCharging) return;

        isCharging = false;
        anim.SetChargingJump(false);
        CalculateJumpTrajectory();
        shouldPerformJump = true;
        OnJumpReleased?.Invoke();
    }

    private void FixedUpdate()
    {
        if (shouldPerformJump)
        {
            ApplyJumpForce();
            shouldPerformJump = false;
        }

        if (isCharging && !groundChecker.IsGrounded() && !climbChecker.IsClimbable())
        {
            isCharging = false;
            anim.SetChargingJump(false);
        }

        if (groundChecker.IsGrounded() && rb.velocity.y < 0.1f && !hasLanded)
        {            
            // check if it was hard landing
            if (hasPlayedHardLandSound)
            {
                SoundManager.Instance.PlayHardLanding();
                GameManager.Instance.FallsCounted();
                hasPlayedHardLandSound = false;
            }
            else
            {
                SoundManager.Instance.PlayJumpLanding();
            }

            isJumping = false;
            hasLanded = true;
        }
        else if (!groundChecker.IsGrounded())
        {
            hasLanded = false;
        }
    }

    private void CalculateJumpTrajectory()
    {
        float initialVelocity = Mathf.Lerp(minJumpForce, maxJumpForce, jumpChargeTimer / chargeTime);
        float parabolicAngle = Mathf.Lerp(minparabolicAngle, maxparabolicAngle, jumpChargeTimer / chargeTime);
        float angleInRadians = jumpDirection == 0f ? Mathf.PI/2 : parabolicAngle * Mathf.Deg2Rad;
        float gravity = gravityMagnitude;

        // inicialVelocity descomposed into X and Y components
        initialVelocityY = initialVelocity * Mathf.Sin(angleInRadians);
        initialVelocityX = initialVelocity * Mathf.Cos(angleInRadians) * jumpDirection;

        // time to apex: t = v0y/g
        timeToApex = initialVelocityY / gravity;
    }

    private void ApplyJumpForce()
    {
        rb.velocity = new Vector2(initialVelocityX, initialVelocityY);
        isJumping = true;
        anim.TriggerJump();
        SoundManager.Instance.PlayJumpRealese();
    }

    private void DrawJumpTrajectory()
    {
        if (!debugDrawPath) return;

        int resolution = 40;
        float gravity = gravityMagnitude;
        Vector3 startPoint = transform.position;
        Vector3 previousPoint = startPoint;

        for (int i = 0; i <= resolution; i++)
        {
            // t = (2 * timeToApex) * (i / resolution)
            float t = (2 * timeToApex) * ((float)i / resolution);
            // x = v0x * t
            float x = initialVelocityX * t;
            // y = v0y * t - (1/2) * g * t^2
            float y = initialVelocityY * t - (0.5f * gravity * t * t);

            Vector3 point = startPoint + new Vector3(x, y, 0);
            Debug.DrawLine(previousPoint, point, Color.white);
            previousPoint = point;
        }
    }

    public void ApplyExtraGravity(bool jumpHeld)
    {
        if (rb.velocity.y < 0f)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
            // Limit fall speed
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else if (rb.velocity.y > 0f && !jumpHeld)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    public void CheckHardLanding()
    { 
        if (!groundChecker.IsGrounded() && rb.velocity.y <= -maxFallSpeed)
        {
            // trigger hard landing animation
            anim.TriggerHardLand();
            if (!hasPlayedHardLandSound)
            {
                hasPlayedHardLandSound = true;
            }

        }
    }

    public bool IsCharging => isCharging;
    public bool IsJumping => isJumping;
}