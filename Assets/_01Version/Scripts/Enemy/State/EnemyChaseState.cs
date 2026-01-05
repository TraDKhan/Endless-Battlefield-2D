using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyController enemy;
    public EnemyChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.anim.SetMoving(true);
    }

    public void Update()
    {
        // Mất player → Idle
        if (!enemy.IsPlayerDetected())
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }

        //Đủ gần → Attack(sau này)
         if (enemy.IsInAttackRange())
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }
    }

    public void FixedUpdate()
    {
        if (!enemy.player || !enemy.movement) return;

        Vector2 pos = enemy.transform.position;
        Vector2 playerPos = enemy.player.position;

        Vector2 toPlayer = (playerPos - pos).normalized;
        float dist = Vector2.Distance(pos, playerPos);

        switch (enemy.stats.enemyType)
        {
            case EnemyAttackType.Melee:
                HandleMelee(pos, playerPos, toPlayer, dist);
                break;

            case EnemyAttackType.Ranged:
                HandleRanged(pos, playerPos, toPlayer, dist);
                break;
        }
    }

    void HandleMelee(Vector2 pos, Vector2 playerPos, Vector2 toPlayer, float dist)
    {
        float stop = enemy.stats.meleeStopRange;
        float tol = enemy.stats.meleeTolerance;

        // Còn xa → tiến
        if (dist > stop + tol)
        {
            enemy.movement.MoveTowards(playerPos, enemy.stats.moveSpeed);
        }
        // Đã đủ gần → dừng / ép
        else
        {
            enemy.movement.Stop();
            enemy.ChangeState(enemy.attackState);
        }
    }


    void HandleRanged(Vector2 pos, Vector2 playerPos, Vector2 toPlayer, float dist)
    {
        float pref = enemy.stats.preferredRange;
        float tol = enemy.stats.rangeTolerance;

        // Quá xa → tiến
        if (dist > pref + tol)
        {
            enemy.movement.MoveTowards(playerPos, enemy.stats.moveSpeed);
        }
        // Quá gần → lùi
        else if (dist < pref - tol)
        {
            Vector2 retreatTarget = pos - toPlayer;
            enemy.movement.MoveTowards(retreatTarget, enemy.stats.moveSpeed);
        }
        // Đúng tầm → strafe
        else
        {
            enemy.movement.Stop();
            enemy.ChangeState(enemy.attackState);
        }
    }
    public void Exit()
    {
        enemy.movement.Stop();
    }
}
