using UnityEngine;
using System.Collections.Generic;

public class AreaEffectDamage : MonoBehaviour
{
    private int damage;
    private float tickInterval;
    private float timer;

    private readonly HashSet<EnemyHealthController> targets = new();
    private readonly List<EnemyHealthController> toAdd = new();
    private readonly List<EnemyHealthController> toRemove = new();

    public void Init(int dmg, float tick)
    {
        damage = dmg;
        tickInterval = tick;
        timer = tickInterval;
    }

    private void Update()
    {
        // Áp dụng các thay đổi trước khi duyệt
        foreach (var a in toAdd)
            targets.Add(a);
        toAdd.Clear();

        foreach (var r in toRemove)
            targets.Remove(r);
        toRemove.Clear();

        // Tick damage
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        foreach (var t in targets)
            t.TakeDamage(damage);

        timer = tickInterval;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHealthController hp))
            toAdd.Add(hp);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHealthController hp))
            toRemove.Add(hp);
    }
}