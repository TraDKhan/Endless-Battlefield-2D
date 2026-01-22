using UnityEngine;

[RequireComponent(
    typeof(PlayerMovementController),
    typeof(PlayerHealthController)
)]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public CharacterStats Stats { get; private set; }


    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    [SerializeField] private PlayerMovementController movement;
    [SerializeField] private PlayerHealthController health;
    [SerializeField] private PlayerEnergyController energy;

    public PlayerEnergyController Energy => energy;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        movement ??= GetComponent<PlayerMovementController>();
        health ??= GetComponent<PlayerHealthController>();
        energy ??= GetComponent<PlayerEnergyController>();

        Stats = new CharacterStats(
            playerData,
            GetComponent<PlayerLevelSystem>(),
            FindAnyObjectByType<UpgradeSystem>()
        );

        Stats.RecalculateStats();

        movement.Initialize(this);
        health.Initialize(this);
        energy.Initialize(this);

        OnStatsChanged();

        Stats.OnStatsChanged += OnStatsChanged;
    }

    private void OnDestroy()
    {
        if (Stats != null)
            Stats.OnStatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged()
    {
        movement.ApplyStats(Stats);
        health.ApplyStats(Stats);
    }
}
