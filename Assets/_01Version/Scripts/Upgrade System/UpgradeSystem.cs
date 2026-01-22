using System;
using System.Collections.Generic;
using UnityEngine;
public class UpgradeSystem : MonoBehaviour, IStatSource
{
    public static UpgradeSystem Instance;

    [Header("Upgrade Pool")]
    public List<UpgradeData> upgradePool;

    [Header("Sub Systems")]
    [SerializeField] private WeaponUpgradeSystem weaponSystem;

    public WeaponUpgradeSystem Weapon => weaponSystem;

    public event Action<List<UpgradeData>> OnShowUpgradeUI;

    private int pendingLevelUps = 0;
    private bool isChoosingUpgrade = false;

    // ===== PLAYER STAT =====
    private readonly Dictionary<StatType, StatUpgrade> playerStatUpgrades = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        var playerLevelSystem = FindFirstObjectByType<PlayerLevelSystem>();
        if (playerLevelSystem != null)
            playerLevelSystem.OnLevelUp += OnPlayerLevelUp;
    }

    private void Update()
    {
        TryShowUpgrade();
    }

    #region LEVEL FLOW
    public void OnPlayerLevelUp(int newLevel)
    {
        pendingLevelUps++;
    }

    private void TryShowUpgrade()
    {
        if (isChoosingUpgrade || pendingLevelUps <= 0)
            return;

        pendingLevelUps--;
        isChoosingUpgrade = true;

        Time.timeScale = 0f;
        OnShowUpgradeUI?.Invoke(GetRandomUpgrades(3));
    }

    public void SelectUpgrade(UpgradeData data)
    {
        if (!data.CanApply(this))
            return;

        data.Apply(this);

        isChoosingUpgrade = false;
        Time.timeScale = 1f;
    }

    #endregion

    #region PLAYER

    public void ApplyPlayerStatUpgrade(PlayerUpgradeData data)
    {
        if (!playerStatUpgrades.TryGetValue(data.statType, out var runtime))
        {
            runtime = new StatUpgrade
            {
                level = 0,
                valuePerLevel = data.valuePerLevel,
                modType = data.modType
            };
        }

        runtime.level++;
        playerStatUpgrades[data.statType] = runtime;
        PlayerController.Instance.Stats.RecalculateStats();
    }

    public int GetPlayerStatLevel(StatType stat)
    {
        return playerStatUpgrades.TryGetValue(stat, out var up)
            ? up.level
            : 0;
    }

    public List<StatModifier> GetModifiers()
    {
        List<StatModifier> result = new();

        foreach (var kv in playerStatUpgrades)
        {
            result.Add(new StatModifier
            {
                statType = kv.Key,
                modType = kv.Value.modType,
                value = kv.Value.Value
            });
        }

        return result;
    }
    #endregion

    #region SKILL
    // ===== SKILL ===== \\
    List<BaseSkill> skills = new();
    public void ApplySkillUpgrade(SkillUpgradeData data)
    {
        foreach (var s in skills)
        {
            if (s.UpgradeData == data)
            {
                s.OnLevelUp();
                return;
            }
        }

        var go = Instantiate(data.skillPrefab);
        var skill = go.GetComponent<BaseSkill>();

        skill.Init(
            PlayerController.Instance.transform,
            PlayerController.Instance.Stats
        );

        skill.OnUnlock();
        skills.Add(skill);
    }

    public int GetSkillLevel(SkillUpgradeData data)
    {
        foreach (var s in skills)
            if (s.UpgradeData == data)
                return s.Level;

        return 0;
    }
    #endregion

    #region WEAPON
    // ===== WEAPON ===== \\
    HashSet<UnLockWeaponUpgrade> unlockedWeapons = new();

    public void UnlockWeapon(UnLockWeaponUpgrade data)
    {
        if (unlockedWeapons.Contains(data))
            return;

        unlockedWeapons.Add(data);

        var weaponGO = Instantiate(data.weaponPrefab);

        var controller = weaponGO.GetComponent<WeaponController>();
        var weaponData = controller.Data;

        Transform socket = WeaponSocketController.Instance.GetSocket(weaponData.slotType);

        weaponGO.transform.SetParent(socket);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;
    }

    public bool HasWeapon(UnLockWeaponUpgrade data)
    {
        return unlockedWeapons.Contains(data);
    }

    public void ApplyWeaponStatsUpgrade(WeaponUpgradeData data)
    {
        weaponSystem.ApplyUpgrade(data);
    }

    #endregion

    #region UTILS
    private List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> valid = new();
        foreach (var up in upgradePool)
            if (up.CanApply(this))
                valid.Add(up);

        List<UpgradeData> result = new();
        while (result.Count < count && valid.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, valid.Count);
            result.Add(valid[index]);
            valid.RemoveAt(index);
        }

        return result;
    }
    #endregion
}

