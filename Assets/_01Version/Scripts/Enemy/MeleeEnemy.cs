using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    private Vector2 moveDir;

    protected override void ResetState()
    {
        moveDir = Vector2.zero;
    }

    protected override void OnSpawned()
    {
        // spawn effect / sound
    }

    void Update()
    {
        if (!isAlive || target == null) return;

        moveDir = (target.position - transform.position).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
}
