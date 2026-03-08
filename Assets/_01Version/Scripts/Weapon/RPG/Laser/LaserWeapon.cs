using UnityEngine;

public class LaserWeapon : WeaponBase
{
    [Header("Laser Visual")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Laser Config")]
    [SerializeField] private float laserDuration = 1.5f;
    [SerializeField] private float tickRate = 0.1f;

    // ===== Runtime
    private bool isFiring;
    private float burstTimer;
    private float tickTimer;

    private Transform currentTarget;
    private WeaponContext ctx;

    protected override void Update()
    {
        base.Update(); // vẫn cho Weapon quản cooldown / trigger

        if (isFiring)
        {
            UpdateLaser();
            UpdateBurst();
        }
    }

    protected override void OnFireLogic()
    {
        if (isFiring) return;

        currentTarget = FindNearestEnemy();

        if (currentTarget == null) return;

        StartLaser();
    }

    // ====== LASER FLOW
    void StartLaser()
    {
        isFiring = true;
        burstTimer = 0f;
        tickTimer = 0f;

        ctx = CreateWeaponContext();
        lineRenderer.enabled = true;
    }

    void StopLaser()
    {
        isFiring = false;
        lineRenderer.enabled = false;
        currentTarget = null;
    }

    void UpdateBurst()
    {
        burstTimer += Time.deltaTime;

        if (burstTimer >= laserDuration)
            StopLaser();
    }

    // ===== LASER UPDATE
    void UpdateLaser()
    {
        if (currentTarget == null)
        {
            StopLaser();
            return;
        }

        Vector3 origin = transform.position;
        Vector3 dir = (currentTarget.position - origin).normalized;

        RotateToDirection(dir);
        float maxRange = ctx.Range;

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            origin,
            dir,
            maxRange,
            ctx.TargetLayer
        );

        foreach (var hit in hits)
        {
            Debug.Log("Laser hit: " + hit.collider.name);
        }

        Vector3 endPos = origin + dir * maxRange;
        DrawLaser(origin, endPos);

        // ===== Tick damage
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickRate)
        {
            tickTimer = 0f;
            ApplyLaserDamage(hits, dir);
        }
    }

    // ===== DAMAGE
    void ApplyLaserDamage(RaycastHit2D[] hits, Vector2 hitDir)
    {
        if (hits == null || hits.Length == 0) return;

        float damagePerTick = ctx.Damage * tickRate;

        foreach (var hit in hits)
        {
            if (!hit.collider) continue;

            if (!hit.collider.TryGetComponent<IDamageable>(out var target))
                continue;

            bool isCrit = Random.value < ctx.CritChance;
            float finalDamage = isCrit
                ? damagePerTick * 1.5f
                : damagePerTick;

            target.TakeDamage(Mathf.RoundToInt(finalDamage), isCrit);
        }
    }
    // ===== VISUAL
    void DrawLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void OnDestroy()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }
}
