using UnityEngine;
using System.Collections;

public abstract class BaseBasicAttack : MonoBehaviour, IBasicAttack
{
    public virtual bool CanAttack(BossContext ctx)
    {
        if (ctx.Player == null)
            return false;

        float dist = Vector2.Distance(
            ctx.boss.transform.position,
            ctx.Player.position
        );

        return dist <= ctx.Stats.attackRange;
    }

    public abstract IEnumerator Attack(BossContext ctx);
}
