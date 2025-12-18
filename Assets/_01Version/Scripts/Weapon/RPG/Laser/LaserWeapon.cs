using UnityEngine;

public class LaserWeapon : Weapon
{
    //[Header("Targeting")]
    //[SerializeField] private LayerMask enemyLayer;

    [Header("Laser Visual")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Laser Burst")]
    [SerializeField] private float laserDuration = 1.5f;
    [SerializeField] private float tickRate = 0.1f;

    private float burstTimer;
    private float tickTimer;

    private bool isFiring;
    private Transform currentTarget;

    protected override void Update()
    {
        base.Update(); // vẫn cho Weapon quản cooldown / trigger

        UpdateTarget();

        if (isFiring)
        {
            UpdateLaser();
            UpdateBurst();
        }
    }

    protected override void OnFireLogic()
    {
        if (isFiring) return;

        if (currentTarget == null) return;

        StartLaser();
    }

    // =====TARGET
    void UpdateTarget()
    {
        currentTarget = FindNearestEnemy();
    }

    // ===== BURST
    void StartLaser()
    {
        isFiring = true;
        burstTimer = 0f;
        tickTimer = 0f;

        lineRenderer.enabled = true;
    }

    void StopLaser()
    {
        isFiring = false;
        lineRenderer.enabled = false;
    }

    void UpdateBurst()
    {
        burstTimer += Time.deltaTime;

        if (burstTimer >= laserDuration)
            StopLaser();
    }

    // ===== LASER
    void UpdateLaser()
    {
        if (currentTarget == null)
        {
            StopLaser();
            return;
        }

        Vector3 origin = transform.position;
        Vector3 dir = (currentTarget.position - origin).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            origin,
            dir,
            stats.Range,
            enemyLayer
        );
        foreach (var hit in hits)
        {
            Debug.Log("Laser hit: " + hit.collider.name);
        }
        Vector3 endPos = origin + dir * stats.Range;
        DrawLaser(origin, endPos);

        tickTimer += Time.deltaTime;
        if (tickTimer >= tickRate)
        {
            tickTimer = 0f;
            ApplyLaserDamage(hits);
        }
    }

    // ===== DAMAGE
    void ApplyLaserDamage(RaycastHit2D[] hits)
    {
        if (hits == null || hits.Length == 0) return;

        WeaponDamageContext ctx = CreateDamageContext();
        float damagePerTick = ctx.damage * tickRate;

        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent<IDamageable>(out var target))
                continue;

            bool isCrit = Random.value < ctx.critChance;
            float finalDamage = isCrit
                ? damagePerTick * ctx.critMultiplier
                : damagePerTick;

            target.TakeDamage((int)finalDamage);
        }
    }
    // ===== VISUAL
    void DrawLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
