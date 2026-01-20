using System.Collections.Generic;
using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    [SerializeField] private UIBossHealthBar bossHealthPrefab;
    [SerializeField] private Transform container; // Canvas cố định

    private readonly Dictionary<BossController, UIBossHealthBar> bars = new();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterBoss(BossController boss)
    {
        if (bars.ContainsKey(boss)) return;

        UIBossHealthBar bar = Instantiate(bossHealthPrefab, container);
        bar.Bind(boss.Health);

        bars.Add(boss, bar);
    }

    public void UnregisterBoss(BossController boss)
    {
        if (!bars.TryGetValue(boss, out var bar)) return;

        if (bar != null)
            Destroy(bar.gameObject);

        bars.Remove(boss);
    }
}
