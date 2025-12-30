using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float cooldown = 1f;
    private float timer;

    public void TryAttack()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        // TODO: gây damage / bắn đạn
        timer = cooldown;
    }
}