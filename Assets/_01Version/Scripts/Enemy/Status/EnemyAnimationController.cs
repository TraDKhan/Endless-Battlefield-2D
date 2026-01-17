using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    static readonly int IsMoving = Animator.StringToHash("IsMoving");
    static readonly int IsDead = Animator.StringToHash("IsDead");

    static readonly int Attack = Animator.StringToHash("Attack");
    static readonly int AttackUp = Animator.StringToHash("AttackUp");
    static readonly int AttackDown = Animator.StringToHash("AttackDown");

    static readonly int Skill1 = Animator.StringToHash("Skill1");
    static readonly int Skill2 = Animator.StringToHash("Skill2");

    // ===== BASE ===== \\
    public void SetMoving(bool value)
    {
        animator.SetBool(IsMoving, value);
    }

    public void SetDead(bool value)
    {
        Debug.Log("Anim Dead = " + value);
        animator.SetBool(IsDead, value);
    }

    public void PlayAttack()
    {
        Debug.LogWarning("Anim: Attack");
        animator.ResetTrigger(Attack);
        animator.SetTrigger(Attack);
    }

    // ===== MEELE ===== \\
    public void PlayAttackUp()
    {
        animator.ResetTrigger(AttackUp);
        animator.SetTrigger(AttackUp);
    }

    public void PlayAttackDown()
    {
        animator.ResetTrigger(AttackDown);
        animator.SetTrigger(AttackDown);
    }

    // ===== BOSS SKILLS ===== \\
    public void PlaySkill1()
    {
        animator.ResetTrigger(Skill1);
        animator.SetTrigger(Skill1);
    }
    public void PlaySkill2()
    {
        animator.ResetTrigger(Skill2);
        animator.SetTrigger(Skill2);
    }
}
