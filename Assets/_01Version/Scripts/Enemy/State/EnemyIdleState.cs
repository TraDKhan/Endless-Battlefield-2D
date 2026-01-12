using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private readonly EnemyContext ctx;

    public EnemyIdleState(EnemyContext context)
    {
        ctx = context;
    }

    public void Enter()
    {
        ctx.Movement?.Stop();
        ctx.Anim?.SetMoving(false);
    }

    public void Update()
    {
    }

    public void FixedUpdate() { }

    public void Exit() { }
}
