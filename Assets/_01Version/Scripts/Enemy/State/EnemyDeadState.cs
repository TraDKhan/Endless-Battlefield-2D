using System.Collections;
using UnityEngine;

public class EnemyDeadState : IEnemyState
{
    private readonly EnemyContext ctx;

    public EnemyDeadState(EnemyContext context)
    {
        ctx = context;
    }

    public void Enter()
    {
        ctx.Movement?.Stop();
        ctx.Anim?.SetMoving(false);

        ctx.Controller.StartCoroutine(DespawnRoutine());
    }

    IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        ctx.Controller.Despawn();
    }

    public void Update() { }
    public void FixedUpdate() { }
    public void Exit() { }
}
