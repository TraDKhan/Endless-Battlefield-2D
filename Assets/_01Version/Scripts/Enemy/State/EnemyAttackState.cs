using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    EnemyController enemy;
    float lastAttackTime;

    public EnemyAttackState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        //enemy.movement.Stop();
        lastAttackTime = Time.time - enemy.stats.attackCooldown;
    }

    public void Update()
    {
        float dist = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (dist > enemy.stats.attackRange)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        if (Time.time - lastAttackTime >= enemy.stats.attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy melee attack!");
        // Gọi animation
        // Gây damage cho player
    }

    public void FixedUpdate() { }

    public void Exit() { }
}
