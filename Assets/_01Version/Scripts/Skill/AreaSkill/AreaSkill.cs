using UnityEngine;

[SerializeField]
public class AreaSkill : MonoBehaviour
{
    [SerializeField] private AreaSkillData _areaSkillData;
    public GameObject _areaEffectPrefab;

    private float tickTimer;
    void Awake()
    {
        var col = GetComponent<CircleCollider2D>();
        if (col != null) col.radius = _areaSkillData.radius;
    }

    void Update()
    {
        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0f)
        {
            DealDamage();
            tickTimer = _areaSkillData.tickInterval;
        }
    }

    void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            _areaSkillData.radius,
            LayerMask.GetMask("Enemy")
        );

        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("Area: gay sat thuong len " + enemy.name);
            enemy.GetComponent<EnemyHealthController>().TakeDamage(_areaSkillData.damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _areaSkillData.radius);
    }
}



