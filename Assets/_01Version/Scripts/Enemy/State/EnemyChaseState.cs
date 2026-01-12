using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private readonly EnemyContext ctx;

    public EnemyChaseState(EnemyContext context)
    {
        ctx = context;
    }
    public void Enter()
    {
        ctx.Anim.SetMoving(true);
    }

    public void Update()
    {
        if (ctx.Target == null) return;

        ctx.Controller.FaceTarget(ctx.Target.position);

        bool keepDistance =
            Vector2.Distance(
                ctx.Controller.transform.position,
                ctx.Target.position
            ) > ctx.Stats.attackRange * 0.6f;

        // Đủ gần → Attack
        if (ctx.Controller.IsInAttackRange() && keepDistance)
        {
            ctx.Controller.ChangeState(EnemyStateID.Attack);
        }
    }

    public void FixedUpdate()
    {
        if (ctx.Target == null || ctx.Movement == null) return;

        Vector2 pos = ctx.Controller.transform.position;
        Vector2 targetPos = ctx.Target.position;

        float dist = Vector2.Distance(pos, targetPos);
        Vector2 toTarget = (targetPos - pos).normalized;

        switch (ctx.Stats.enemyType)
        {
            case EnemyAttackType.Melee:
                HandleMelee(targetPos);
                break;

            case EnemyAttackType.Ranged:
                HandleRanged(pos, targetPos, toTarget, dist);
                break;
        }
    }
    public void Exit()
    {
        ctx.Movement.Stop();
    }

    // ================== MOVE LOGIC ==================
    void HandleMelee(Vector2 targetPos)
    {
        ctx.Movement.MoveTowards(targetPos, ctx.Stats.moveSpeed);
    }

    void HandleRanged(
        Vector2 pos,
        Vector2 targetPos,
        Vector2 toTarget,
        float dist
    )
    {
        float attackRange = ctx.Stats.attackRange;
        float speed = ctx.Stats.moveSpeed;

        // Quá xa → tiến
        if (dist > attackRange)
        {
            ctx.Movement.MoveTowards(targetPos, speed);
            return;
        }

        // Trong tầm đánh → đứng yên
        if (dist > attackRange * 0.6f)
        {
            ctx.Movement.Stop();
            return;
        }

        // Player áp sát → lùi chậm
        Vector2 retreatTarget = pos - toTarget;
        ctx.Movement.MoveTowards(retreatTarget, speed * 0.5f);
    }
}
