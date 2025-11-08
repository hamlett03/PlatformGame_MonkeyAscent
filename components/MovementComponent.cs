using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(JumpComponent))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private JumpComponent jumpComponent;
    private GroundChecker groundChecker;
    private bool wasWalking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpComponent = GetComponent<JumpComponent>();
        groundChecker = GetComponent<GroundChecker>();
    }

    public void Move(float horizontalInput)
    {
        // allow movement only when not jumping or falling
        if (!jumpComponent.IsJumping)
        {
            movement.x = horizontalInput;
            movement.y = 0;

            if (jumpComponent.IsCharging)
            {
                // When charging, stop horizontal movement completely
                rb.velocity = new Vector2(0, rb.velocity.y);
                StopWalkingSoundIfNeeded();
                return;
            }

            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

            // handle walking sound
            bool isWalkingNow = Mathf.Abs(horizontalInput) > 0.1f && groundChecker.IsGrounded();

            if (isWalkingNow && !wasWalking)
            {
                SoundManager.Instance.StartWalkingSound();
            }
            else if (!isWalkingNow && wasWalking)
            {
                SoundManager.Instance.StopWalkingSound();
            }

            wasWalking = isWalkingNow;
        }
        else 
        {
            StopWalkingSoundIfNeeded();
        }
    }

    private void StopWalkingSoundIfNeeded()
    {
        if (wasWalking)
        {
            SoundManager.Instance.StopWalkingSound();
            wasWalking = false;
        }
    }

    public void FlipSprite(float direction)
    {
        // Allow flipping while charging, but not while jumping
        if (!jumpComponent.IsJumping)
        {
            if (direction != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
            }
        }
    }

    private void OnDisable()
    {
        StopWalkingSoundIfNeeded();
    }
}