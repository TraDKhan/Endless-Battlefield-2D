using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayWalk()
    {
        animator.SetBool("IsMoving", true);
    }

    public void PlayIdle()
    {
        animator.SetBool("IsMoving", false);
    }

    public void PlayAttack()
    {
        animator.SetBool("IsMoving", false);

        animator.SetTrigger("Attack");
    }

    public void PlayHit()
    {
        animator.SetTrigger("Hit");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");

        // Nếu bạn muốn khoá điều khiển animation sau khi chết:
        // animator.SetBool("IsMoving", false);
    }
}
