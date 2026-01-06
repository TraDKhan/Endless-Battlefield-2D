using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    static readonly int IsMoving = Animator.StringToHash("IsMoving");
    static readonly int IsDead = Animator.StringToHash("IsDead");
    static readonly int AttackPhase = Animator.StringToHash("AttackPhase");

    public void SetMoving(bool value)
    {
        animator.SetBool(IsMoving, value);
    }

    public void SetDead(bool value)
    {
        animator.SetBool(IsDead, value);
    }

    public void SetAttackPhase(AttackAnimPhase phase)
    {
        Debug.Log($"AttackPhase = {animator.GetInteger("AttackPhase")}");
        animator.SetInteger(AttackPhase, (int)phase);
    }

    //MEELE ENEMY
    public void PlayAttack()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }

    public void StopAttack() { }
}

public enum AttackAnimPhase
{
    None = 0,
    WindUp = 1,
    Attack = 2,
    Recover = 3
}
