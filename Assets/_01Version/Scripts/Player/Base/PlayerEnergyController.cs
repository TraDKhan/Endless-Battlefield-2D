using System;
using UnityEngine;

public class PlayerEnergyController : MonoBehaviour
{
    public static PlayerEnergyController Instance;

    [SerializeField] private int regenAmount = 5;   // mỗi lần hồi bao nhiêu
    [SerializeField] private float regenInterval = 1f; // 1 giây

    public int CurrentEnergy { get; private set; }

    private int maxEnergy;
    private float regenTimer;

    private CharacterStats stats;

    public event Action<int, int> OnEnergyChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CharacterStatsController.OnStatsReady += OnStatsReady;
    }

    private void OnDestroy()
    {
        CharacterStatsController.OnStatsReady -= OnStatsReady;

        if (stats != null)
            stats.OnStatsChanged -= OnStatsChanged;
    }

    private void Update()
    {
        RegenTick();
    }


    private void OnStatsReady(CharacterStats characterStats)
    {
        stats = characterStats;
        stats.OnStatsChanged += OnStatsChanged;
        ApplyMaxEnergyFromStats();
    }

    private void OnStatsChanged()
    {
        ApplyMaxEnergyFromStats();
    }

    private void ApplyMaxEnergyFromStats()
    {
        int newMaxEnergy = stats.GetMaxEnergy();

        float percent = maxEnergy > 0
            ? (float)CurrentEnergy / maxEnergy
            : 1f;

        maxEnergy = newMaxEnergy;
        CurrentEnergy = Mathf.RoundToInt(maxEnergy * percent);

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }
    private void RegenTick()
    {
        if (CurrentEnergy >= maxEnergy) return;

        regenTimer += Time.deltaTime;

        if (regenTimer < regenInterval) return;

        regenTimer = 0f;

        CurrentEnergy += regenAmount;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, maxEnergy);

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }

    public bool CanConsume(int amount) => CurrentEnergy >= amount;

    public bool Consume(int amount)
    {
        if (!CanConsume(amount)) return false;

        CurrentEnergy -= amount;
        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
        return true;
    }
}
