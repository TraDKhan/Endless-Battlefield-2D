using UnityEngine;

public class AreaSkill : MonoBehaviour
{
    [SerializeField] private AreaSkillData data;
    private float tickTimer;

    void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0)
        {
            DealDamage();
            tickTimer = data.tickInterval;
        }
    }

    void DealDamage()
    {
        var hits = Physics2D.OverlapCircleAll(
            transform.position,
            data.radius,
            LayerMask.GetMask("Enemy")
        );

        foreach (var h in hits)
            h.GetComponent<EnemyHealthController>()?.TakeDamage(data.damage);
    }
}