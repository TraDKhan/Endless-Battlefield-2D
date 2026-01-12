using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private readonly EnemyContext ctx;
    private readonly IEnemyAttack attack;

    public EnemyAttackState(EnemyContext context)
    {
        ctx = context;
        attack = ctx.Controller.GetComponent<IEnemyAttack>();
        if (attack == null)
            Debug.LogError($"{ctx.Controller.name} missing IEnemyAttack");
    }

    public void Enter()
    {
        ctx.Movement.Stop();
        ctx.Anim.SetMoving(false);

        FaceTarget();

        TryStartAttack();
    }

    public void Update()
    {
        FaceTarget();

        if (!ctx.Controller.IsInAttackRange())
        {
            ctx.Controller.ChangeState(EnemyStateID.Chase);
            return;
        }

        TryStartAttack();
        attack?.UpdateAttack();
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        attack.StopAttack();
    }

    // ===== Helpers =====

    void TryStartAttack()
    {
        if (attack == null || !attack.CanAttack) return;
        attack.StartAttack();
    }

    void FaceTarget()
    {
        if (ctx.Target == null) return;
        ctx.Controller.FaceTarget(ctx.Target.position);
    }
}
