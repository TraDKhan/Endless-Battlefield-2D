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

    private float laserTimer;
    private float tickTimer;
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

    // ================= BURST =================
    void HandleBurst()
    {
        if (isFiring)
        {
            laserTimer += Time.deltaTime;

            if (laserTimer >= laserDuration)
            {
                StopLaser();
            }
        }
        else
        {
            if (CanShoot() && currentTarget != null)
            {
                StartLaser();
            }
        }
    }

    void StartLaser()
    {
        isFiring = true;
        laserTimer = 0f;
        tickTimer = 0f;
        lastShootTime = Time.time;
    }

    void StopLaser()
    {
        isFiring = false;
        DisableLaser();
    }

    // ================= LASER =================
    void HandleLaser()
    {
        if (!isFiring || currentTarget == null)
        {
            DisableLaser();
            return;
        }

        Vector3 origin = transform.position;
        Vector3 dir = (currentTarget.position - origin).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            origin,
            dir,
            currentStats.range,
            enemyLayer
        );

        Vector3 endPos = origin + dir * currentStats.range;
        DrawLaser(endPos);

        tickTimer += Time.deltaTime;

        if (tickTimer >= tickRate)
        {
            tickTimer = 0f;
            ApplyLaserDamage(hits);
        }
    }

    void ApplyLaserDamage(RaycastHit2D[] hits)
    {
        if (hits == null || hits.Length == 0) return;

        DamageContext ctx = CreateDamageContext();
        float damagePerTick = ctx.damage * tickRate;

        foreach (RaycastHit2D hit in hits)
        {
            EnemyHealthController enemy = hit.collider.GetComponent<EnemyHealthController>();
            if (enemy == null) continue;

            float finalDamage = damagePerTick;

            if (Random.value < ctx.critChance)
            {
                finalDamage *= ctx.critMultiplier;
            }

            enemy.TakeDamage((int)finalDamage);
        }
    }

    // ================= VISUAL =================
    void DrawLaser(Vector3 endPos)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPos);
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }

    // ================= FIND TARGET =================
    Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            currentStats.range,
            enemyLayer
        );

        float minDist = float.MaxValue;
        Transform nearest = null;

        foreach (Collider2D e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    Gizmos.DrawWireSphere(transform.position, currentStats.range);
    //}
}
