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

    public void PlayAttack() { /* ... */ }
    public void PlayHit() { /* ... */ }
    public void PlayDeath() { /* ... */ }
}
