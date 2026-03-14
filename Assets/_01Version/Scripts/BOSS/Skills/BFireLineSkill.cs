using System.Collections;
using UnityEngine;

public class BFireLineSkill : BaseBossSkill
{
    [Header("Prefabs")]
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject firePrefab;

    [Header("Lane Config")]
    [SerializeField] float width = 12f;
    [SerializeField] float spacing = 2.5f;

    [Header("Timing")]
    [SerializeField] float warningTime = 1f;
    [SerializeField] float fireDuration = 3f;
    [SerializeField] float laneDelay = 0.35f;

    private float attackRange;
    private Transform player;
    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        ctx.boss.SetCastingSkill(true);

        attackRange = ctx.Stats.attackRange;
        player = ctx.Player;

        if (!CheckPointPlayer(transform.position, player.position, attackRange))
        {
            ctx.boss.SetCastingSkill(false);
            width = ctx.Stats.attackRange * 2; 
            yield break;
        }

        Vector3 center = transform.position;

        Vector3[] lanes =
        {
            center,
            center + Vector3.up * spacing,
            center + Vector3.down * spacing
        };

        // Spawn warning
        foreach (var pos in lanes)
        {
            SpawnWarning(pos);
            yield return new WaitForSeconds(warningTime);
            SpawnFire(pos);
            yield return new WaitForSeconds(laneDelay);
        }

        

        //// Spawn fire từng lane
        //foreach (var pos in lanes)
        //{
            
        //    yield return new WaitForSeconds(laneDelay);
        //}

        yield return new WaitForSeconds(2f);
        ctx.boss.SetCastingSkill(false);
    }

    bool CheckPointPlayer(Vector2 _boss, Vector2 _player, float _attackRange)
    {
        return Mathf.Abs(_boss.y - _player.y) < 2f
               && Mathf.Abs(_boss.x - _player.x) <= attackRange + 0.5f;
    }

    void SpawnWarning(Vector3 pos)
    {
        GameObject w = Instantiate(warningPrefab, pos, Quaternion.identity);

        VFX_Warning_Line warning = w.GetComponent<VFX_Warning_Line>();
        warning.Init(width, warningTime);
    }

    void SpawnFire(Vector3 pos)
    {
        GameObject f = Instantiate(firePrefab, pos, Quaternion.identity);

        FireLine fire = f.GetComponent<FireLine>();
        fire.Init(width, fireDuration);
    }
}