using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator animator;

    // Initialize the animator reference
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Methods to set animation parameters
    public void SetWalking(bool value) => animator.SetBool("isWalking", value);
    public void SetGrounded(bool value) => animator.SetBool("isGrounded", value);
    public void SetChargingJump(bool value) => animator.SetBool("isChargingJump", value);
    public void SetVelocityY(float value) => animator.SetFloat("velocityY", value);
    public void TriggerJump() => animator.SetTrigger("Jump");
    public void TriggerDash() => animator.SetTrigger("Dash");
    public void TriggerHardLand() => animator.SetTrigger("HardLand");
}