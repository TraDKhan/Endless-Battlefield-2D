using System;
using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    [SerializeField] private EnemyHealthController health;

    [Header("Phase Settings")]
    [SerializeField] private int maxPhase = 3;

    // Phase bắt đầu từ 1
    public int CurrentPhase { get; private set; } = 1;

    public event Action<int> OnPhaseChanged;

    private void Awake()
    {
        if (health == null)
            health = GetComponent<EnemyHealthController>();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int current, int max)
    {
        float hpPercent = (float)current / max;
        int newPhase = CalculatePhase(hpPercent);

        if (newPhase != CurrentPhase)
        {
            CurrentPhase = newPhase;
            Debug.Log($"[Boss] Phase → {CurrentPhase}");
            OnPhaseChanged?.Invoke(CurrentPhase);
        }
    }

    private int CalculatePhase(float hpPercent)
    {
        // Ví dụ maxPhase = 3
        // Phase 1: 100% → 66%
        // Phase 2: 66% → 33%
        // Phase 3: < 33%

        float step = 1f / maxPhase;
        int phase = Mathf.FloorToInt((1f - hpPercent) / step) + 1;

        return Mathf.Clamp(phase, 1, maxPhase);
    }
}
