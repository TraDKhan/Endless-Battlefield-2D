using System.Collections;
using UnityEngine;

public class EnemyDeadState : IEnemyState
{
    EnemyController enemy;

    public EnemyDeadState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.movement.Stop();
        enemy.StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.5f);
        ObjectPoolManager.Instance.Despawn(enemy.enemyBase);
    }

    public void Update() { }
    public void FixedUpdate() { }
    public void Exit() { }
}
