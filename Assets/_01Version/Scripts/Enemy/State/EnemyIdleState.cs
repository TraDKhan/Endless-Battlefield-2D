public class EnemyIdleState : IEnemyState
{
    EnemyController enemy;

    public EnemyIdleState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        if (enemy.IsPlayerDetected())
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }            
    }

    public void FixedUpdate() { }

    public void Exit() { }
}