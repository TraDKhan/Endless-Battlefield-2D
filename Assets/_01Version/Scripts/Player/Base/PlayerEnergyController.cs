using System;
using UnityEngine;

public class PlayerEnergyController : MonoBehaviour
{
    [SerializeField] private int regenAmount = 5;   // mỗi lần hồi bao nhiêu
    [SerializeField] private float regenInterval = 1f; // 1 giây

    public int CurrentEnergy { get; private set; }

    private int maxEnergy;
    private float regenTimer;

    private PlayerController player;
    private CharacterStats stats;

    public event Action<int, int> OnEnergyChanged;

    // ===== INIT
    public void Initialize(PlayerController player)
    {
        this.player = player;
        stats = player.Stats;

        ApplyStats(stats);
    }

    // ===== APPLY STATS
    public void ApplyStats(CharacterStats stats)
    {
        int newMaxEnergy = stats.GetMaxEnergy();

        float percent = maxEnergy > 0
            ? (float)CurrentEnergy / maxEnergy
            : 1f;

        maxEnergy = newMaxEnergy;
        CurrentEnergy = Mathf.RoundToInt(maxEnergy * percent);

        regenTimer = 0f; // reset regen (fix bug nhỏ)

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }

    private void Update()
    {
        RegenTick();
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
