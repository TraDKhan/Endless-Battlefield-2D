using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Vector2 lastMoveDir = Vector2.down;

    public void SetMovement(bool isMoving, Vector2 moveInput)
    {
        animator.SetBool("IsMoving", isMoving);

        if (moveInput.sqrMagnitude > 0.0001f)
            lastMoveDir = moveInput.normalized;

        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
    }

    public void SetDash(bool isDashing)
    {
        animator.SetBool("IsDashing", isDashing);
    }

    public void PlayHit()
    {
        animator.SetTrigger("Hit");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
}
