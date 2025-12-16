using UnityEngine;

public class LaserWeapon : Weapon
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Laser Visual")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Laser Burst")]
    [SerializeField] private float laserDuration = 1.5f;
    [SerializeField] private float tickRate = 0.1f;

    private float burstTimer;
    private float tickTimer;
    private float lastFireTime;

    private bool isFiring;
    private Transform currentTarget;

    private void Update()
    {
        UpdateTarget();
        HandleBurst();
        HandleLaser();
    }

    // ================= TARGET =================
    void UpdateTarget()
    {
        currentTarget = FindNearestEnemy();
    }

    // ================= BURST / COOLDOWN =================
    void HandleBurst()
    {
        if (isFiring)
        {
            burstTimer += Time.deltaTime;

            if (burstTimer >= laserDuration)
                StopLaser();
        }
        else
        {
            if (CanFire() && currentTarget != null)
                StartLaser();
        }
    }

    bool CanFire()
    {
        return Time.time >= lastFireTime + stats.Cooldown;
    }

    void StartLaser()
    {
        isFiring = true;
        burstTimer = 0f;
        tickTimer = 0f;
        lastFireTime = Time.time;

        lineRenderer.enabled = true;
    }

    void StopLaser()
    {
        isFiring = false;
        lineRenderer.enabled = false;
    }

    public override void Fire()
    {
        // dành cho manual trigger nếu cần
    }

    // ================= LASER LOGIC =================
    void HandleLaser()
    {
        if (!isFiring || currentTarget == null)
        {
            lineRenderer.enabled = false;
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

        Vector3 endPos = origin + dir * stats.Range;
        DrawLaser(origin, endPos);

        tickTimer += Time.deltaTime;
        if (tickTimer >= tickRate)
        {
            tickTimer = 0f;
            ApplyLaserDamage(hits);
        }
    }

    // ================= DAMAGE =================
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

    // ================= VISUAL =================
    void DrawLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // ================= TARGET SEARCH =================
    Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            stats.Range,
            enemyLayer
        );

        float minDist = float.MaxValue;
        Transform nearest = null;

        foreach (var e in enemies)
        {
            float dist = Vector2.Distance(
                transform.position,
                e.transform.position
            );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }
}
