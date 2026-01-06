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
        if (!enemy.player) return;

        enemy.FaceTarget(enemy.player.position);

        bool check = Vector2.Distance(enemy.transform.position, enemy.player.position) > enemy.stats.attackRange * 0.6f;
        // Đủ gần → Attack
        if (enemy.IsInAttackRange() && check)
        {
            enemy.ChangeState(enemy.attackState);
        }
    }

    public void FixedUpdate()
    {
        if (!enemy.player || !enemy.movement) return;

        Vector2 pos = enemy.transform.position;
        Vector2 playerPos = enemy.player.position;

        float dist = Vector2.Distance(pos, playerPos);
        Vector2 toPlayer = (playerPos - pos).normalized;

        switch (enemy.stats.enemyType)
        {
            case EnemyAttackType.Melee:
                HandleMelee(playerPos);
                break;

            case EnemyAttackType.Ranged:
                HandleRanged(pos, playerPos, toPlayer, dist);
                break;
        }
    }

    void HandleMelee(Vector2 playerPos)
    {
        enemy.movement.MoveTowards(playerPos, enemy.stats.moveSpeed);
    }

    void HandleRanged(Vector2 pos, Vector2 playerPos, Vector2 toPlayer, float dist)
    {
        float attackRange = enemy.stats.attackRange;
        float speed = enemy.stats.moveSpeed;

        // Quá xa → tiến
        if (dist > attackRange)
        {
            enemy.movement.MoveTowards(playerPos, enemy.stats.moveSpeed);
            return;
        }

        //trong tầm đánh đổi state
        if (dist > attackRange * 0.6f)
        {
            enemy.movement.Stop();
            return;
        }

        // Player áp sát → lùi chậm
        Vector2 retreatTarget = pos - toPlayer;
        enemy.movement.MoveTowards(
            retreatTarget,
            speed * 0.5f
        );
    }
    public void Exit()
    {
        enemy.movement.Stop();
    }
}
