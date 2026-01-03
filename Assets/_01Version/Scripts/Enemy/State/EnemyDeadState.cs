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
        enemy.gameObject.SetActive(false);

        // Nếu dùng pooling
        //enemy.StartCoroutine(DisableAfterDeath());
    }
    //private IEnumerator DisableAfterDeath()
    //{
    //    yield return new WaitForSeconds(enemy.animation.DeathDuration);
    //    enemy.gameObject.SetActive(false);
    //}
    public void Update() { }
    public void FixedUpdate() { }
    public void Exit() { }
}
