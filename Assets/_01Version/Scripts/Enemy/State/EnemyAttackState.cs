using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    EnemyController enemy;
    IEnemyAttack attack;

    public EnemyAttackState(EnemyController enemy)
    {
        this.enemy = enemy;
        attack = enemy.GetComponent<IEnemyAttack>();
    }

    public void Enter()
    {
        enemy.movement.Stop();
        enemy.anim.SetMoving(false);

        enemy.FaceTarget(enemy.player.position);

        TryStartAttack();
    }

    public void Update()
    {
        enemy.FaceTarget(enemy.player.position);

        if (!enemy.IsInAttackRange())
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        TryStartAttack();
        attack.UpdateAttack();
    }

    void TryStartAttack()
    {
        if (!attack.CanAttack) return;

        attack.StartAttack();
    }
    public void FixedUpdate() { }

    public void Exit() 
    {
        attack.StopAttack();
    }
}
